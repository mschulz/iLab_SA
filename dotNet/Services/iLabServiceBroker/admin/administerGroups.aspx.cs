/*
 * Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
 * Please see license.txt in top level directory for full license.
 */

/* $Id$ */

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using iLabs.ServiceBroker.Administration;
using iLabs.ServiceBroker.Authorization;
using iLabs.ServiceBroker.Internal;
using iLabs.ServiceBroker;
using iLabs.UtilLib;

namespace iLabs.ServiceBroker.admin
{
	/// <summary>
	/// Summary description for adminsterGroups.
	/// </summary>
	public partial class administerGroups : System.Web.UI.Page
	{
	
		protected string actionCmd = "Edit";
		protected string target;
		protected List<Group> groupList;
		protected int associatedGroup = 0;

		AuthorizationWrapperClass wrapper = new AuthorizationWrapperClass ();

		protected void Page_Load(object sender, System.EventArgs e)
		{

			if (Session["UserID"]==null)
				Response.Redirect("../login.aspx");

			// Controls that enable the popup to fire an event on the caller when the Save button is hit.
			btnRefresh.CausesValidation = false;
			
			// This is a hidden input tag. The associatedLabServers popup changes its value using a window.opener call in javascript,
			// then the GetPostBackEventReference fires the event associated with the btnRefresh button.
			// The result is that the LabServer repeater (repLabServers) is refreshed when the Save button is clicked
			// on the popup.
			hiddenPopupOnSave.Attributes.Add("onpropertychange", Page.GetPostBackEventReference(btnRefresh));

			associatedGroup = wrapper.GetAssociatedGroupIDWrapper(Convert.ToInt32(Session["GroupID"]));

			if(!Page.IsPostBack )			// populate with all the group IDs
			{
				refreshGroupRepeater();
				btnAddGroup.Attributes.Add("onClick","javascript:window.open('addEditGroupPopup.aspx?action=Add','addeditgroup','scrollbars=yes,resizable=yes,width=700,height=600')");
                btnAddServAdminGroup.Attributes.Add("onClick", "javascript:window.open('addEditServAdminGroupPopup.aspx?action=Add','addeditgroup','scrollbars=yes,resizable=yes,width=700,height=600')");
			}
		}

		private void refreshGroupRepeater()
		{
			try
			{
				int[] groupIDs = wrapper.ListGroupIDsWrapper();
				groupList = new List<Group>();
				Group [] groups = wrapper.GetGroupsWrapper(groupIDs);
				foreach(Group g in groups)
				{
					if(g.groupID >0) 
						if (g.groupType.Equals(GroupType.REGULAR) || g.groupType.Equals(GroupType.SERVICE_ADMIN))
                            groupList.Add(g);
				}
                groupList.Sort();
				repGroups.DataSource=groupList;
				repGroups.DataBind();
			}
			catch (Exception ex)
			{
				lblResponse.Text = Utilities.FormatErrorMessage("Cannot list groups. "+ex.GetBaseException());
			}
		}
		/// <summary>
		/// This is a hidden HTML button that is "clicked" by an event raised 
		/// by the closing of the associatedLabServers popup.
		/// It causes the Lab Servers repeater to be refreshed.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnRefresh_ServerClick(object sender, System.EventArgs e)
		{
			refreshGroupRepeater();
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
			this.btnRefresh.ServerClick += new System.EventHandler(this.btnRefresh_ServerClick);
			//
			// The following is added to support window popups from within the repeater.
			//
			this.repGroups.ItemCreated += new RepeaterItemEventHandler(this.repGroups_ItemCreated);
			this.repGroups.ItemDataBound += new RepeaterItemEventHandler(this.repGroups_ItemBound);
			this.repGroups.ItemCommand += new RepeaterCommandEventHandler(this.repGroups_ItemCommand);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    

		}
		#endregion

		private void repGroups_ItemCreated(Object sender, RepeaterItemEventArgs e)
		{
			if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
			{
				Button curBtn = (Button) e.Item.FindControl("btnRemove");
				//curBtn.Attributes.Add("onClick", "return confirmDelete();");
			}
		}

		private void repGroups_ItemBound(Object sender, RepeaterItemEventArgs e)
		{		
			if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
			{
				Group curGroup = (Group) e.Item.DataItem;
				Label lbl = (Label) e.Item.FindControl("lblDescription");
				if((curGroup.description != null) && (curGroup.description.Length >0))
				{
					lbl.Text= curGroup.description;
				}
				else
				{
					lbl.Text= "&nbsp;";
				}
				Button curBtn = (Button) e.Item.FindControl("btnEdit");
				// check which window to open based on the selected group's type
                // (whether it's a regular group or a service admin group)
                string script = null;
                if (curGroup.groupType.Equals(GroupType.SERVICE_ADMIN))
                    script = "javascript:openPopupWindow('addEditServAdminGroupPopup.aspx?gid=" + curGroup.GroupID + "')";
                else
                    script = "javascript:openPopupWindow('addEditGroupPopup.aspx?gid=" + curGroup.GroupID + "')";
                
				curBtn.Attributes.Add("onClick",script);
				
				curBtn = (Button) e.Item.FindControl("btnRemove");
				script = "return confirmDelete()";
				curBtn.Attributes.Add("onClick", script);
				curBtn.CommandArgument = curGroup.GroupID.ToString();
				if ((curGroup.groupID == Convert.ToInt32(Session["GroupID"]))||(curGroup.groupID == associatedGroup))
				{
					curBtn.Enabled = false;
					curBtn.BackColor = Color.Silver;
				}
			}
			
		}
		private void repGroups_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
		{
				if(e.CommandName.Equals("Remove"))
				{
				
					int groupID = Convert.ToInt32(e.CommandArgument);
					string gname = wrapper.GetGroupsWrapper(new int[] {groupID})[0].groupName;
					//first remove request group
					int requestGroupID = AdministrativeUtilities.GetGroupRequestGroup(groupID);
					if (requestGroupID >0)
					{
						int[] groups = new int[2];
						groups[0] = requestGroupID;
						groups[1] =groupID;
						wrapper.RemoveGroupsWrapper(groups);
					}
					else
					{
						int [] groups = new int[1];
						groups[0] = groupID;
						wrapper.RemoveGroupsWrapper(groups);
					}

					refreshGroupRepeater();
					lblResponse.Text = Utilities.FormatConfirmationMessage("Group '"+gname+"' has successfully been removed.");
					lblResponse.Visible = true;
				}
			}
}
}
