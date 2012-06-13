/*
 * Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
 * Please see license.txt in top level directory for full license.
 * 
 * $Id$
 */
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Security;
using iLabs.ServiceBroker.Internal;
using iLabs.ServiceBroker.Administration;
using iLabs.ServiceBroker.Authorization;

namespace iLabs.ServiceBroker.iLabSB
{
	/// <summary>
	/// User Groups Page.
	/// </summary>
	public partial class myGroups : System.Web.UI.Page
	{
		protected ArrayList nonRequestGroups = new ArrayList();
		protected ArrayList requestGroups = new ArrayList();
		AuthorizationWrapperClass wrapper = new AuthorizationWrapperClass();
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			//if(! IsPostBack)
			{

				// To list all the groups a user belongs to
				int userID = Convert.ToInt32(Session["UserID"]);
				int[] groupIDs = wrapper.ListGroupsForUserWrapper (userID);

				//since we already have the groups a user has access
				// if we use wrapper here, it will deny authentication
                Group[] gps = AdministrativeAPI.GetGroups(groupIDs);

				foreach(Group g in gps)
				{
					if (g.groupName.EndsWith("request"))
						requestGroups.Add(g);
					else 
						if(!g.groupName.Equals(Group.NEWUSERGROUP))
						nonRequestGroups.Add(g);	
				}
			}

			if ((nonRequestGroups==null ) ||(nonRequestGroups.Count ==0))
			{
				lblNoGroups.Text = "<p>You currently do not have access to any group. It can take upto 48 hours for a group administrator to give you permission to the groups you've requested.</p>";
				lblNoGroups.Visible = true;
				Session["GroupCount"] = 0;
			}
			else
			{
                Session["GroupCount"] = nonRequestGroups.Count;
				//Redirect to single lab single group page?
                if (nonRequestGroups.Count == 1)
                {
                    
                        if (nonRequestGroups[0] != null)
                        {
                           PageRedirect((Group)nonRequestGroups[0]);
                        }
                    
                }
				repGroups.DataSource = nonRequestGroups;
				repGroups.DataBind();
			}

			int repCount=1;
			// To list all the labs belonging to a group
			foreach (Group g in nonRequestGroups)
			{
				int[] lcIDsList = AdministrativeUtilities.GetGroupLabClients(g.groupID);

				LabClient[] lcList = wrapper.GetLabClientsWrapper(lcIDsList);

				Label lblGroupLabs = new Label();
				lblGroupLabs.Visible=true;
				lblGroupLabs.Text="<ul>";

				for(int i=0;i<lcList.Length;i++)
				{
					lblGroupLabs.Text += "<li><strong class=lab>"+
						lcList[i].clientName+"</strong> - "+
						lcList[i].clientShortDescription+"</li>";
				}
				lblGroupLabs.Text +="</ul>";
				repGroups.Controls.AddAt(repCount, lblGroupLabs);
				repCount+=2;
			}

			if ((requestGroups!=null)&& (requestGroups.Count>0))
			{
				for (int i=0;i<requestGroups.Count;i++)
				{
                    int origGroupID = AdministrativeAPI.GetAssociatedGroupID(((Group)requestGroups[i]).groupID);
                    string origGroupName = AdministrativeAPI.GetGroups(new int[] { origGroupID })[0].groupName;
					lblRequestGroups.Text+= origGroupName;
					if (i != requestGroups.Count-1)
						lblRequestGroups.Text +=", ";
				}
			}
			else
			{
				lblRequestGroups.Text = "No group";
			}
		}
		
		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.repGroups.ItemCommand += new System.Web.UI.WebControls.RepeaterCommandEventHandler(this.repGroups_ItemCommand);

		}
		#endregion

		private void PageRedirect(Group effectiveGroup)
		{
            // initialize boolean session variables that indicate what type of effective group this is
            Session["IsAdmin"] = false;
            Session["IsServiceAdmin"] = false;

            Session["GroupID"] = effectiveGroup.groupID;
            Session["GroupName"] = effectiveGroup.groupName;
            Session["GroupCount"] = 1;

            int client = 0;
            if (Session["ClientID"] != null)
                client = Convert.ToInt32(Session["ClientID"]);
            AdministrativeAPI.ModifyUserSession(Convert.ToInt64(Session["SessionID"]), effectiveGroup.groupID, client, Session.SessionID);
                            
            
            if((effectiveGroup.groupName.Equals(Group.SUPERUSER))||(effectiveGroup.groupType.Equals(GroupType.COURSE_STAFF)))
			{
				Session["IsAdmin"] = true;
				Response.Redirect ("admin/manageUsers.aspx");
			}

            // if the effective group is a service admin group, then redirect to the service admin page.
            // the session variable is used in the userNav page to check whether to make the corresponing tab visible
            else if (effectiveGroup.groupType.Equals(GroupType.SERVICE_ADMIN))
            {
                Session["IsServiceAdmin"] = true;
                Response.Redirect ("admin/adminServices.aspx");
            }          
            
            else
			{
				Session["IsAdmin"] = false;
                Session["IsServiceAdmin"] = false;
				Response.Redirect ("myLabs.aspx");
			}
		}

		private void repGroups_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
		{
			if (Session["UserID"] == null)
				Response.Redirect("login.aspx");
			else
			{
				if(e.CommandName=="SetEffectiveGroup")
				{
					// get the groupID from the nonRequestGroups ArrayList.
					// The index of the ArrayList will match the index of the repeater
					// since the repeater was loaded from the ArrayList.
                    //int groupID = ((Group)nonRequestGroups[e.Item.ItemIndex]).groupID;
				
                    //// Set the GroupID session value and redirect
                    //Session["GroupID"] = groupID;
                    //Session["GroupName"]= ((Group)nonRequestGroups[e.Item.ItemIndex]).groupName;
                    //int client = 0;
                    //Session.Remove("ClientID");
                    //Session.Remove("ClientCount");
                    ////if (Session["ClientID"] != null)
                    /////    client = Convert.ToInt32(Session["ClientID"]);
                    //AdministrativeAPI.ModifyUserSession(Convert.ToInt64(Session["SessionID"]), groupID, client, Session.SessionID);
                            
					PageRedirect ((Group)nonRequestGroups[e.Item.ItemIndex]);
				}
			}
		}
	}
}
