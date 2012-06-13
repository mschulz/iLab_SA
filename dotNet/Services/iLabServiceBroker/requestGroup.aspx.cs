/*
 * Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
 * Please see license.txt in top level directory for full license.
 * 
 * $Id$
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Security;
using System.Web.Mail;
using System.Configuration;

using iLabs.DataTypes;
using iLabs.Core;
using iLabs.ServiceBroker;
using iLabs.ServiceBroker.Internal;
using iLabs.ServiceBroker.Administration;
using iLabs.ServiceBroker.Authorization;
using iLabs.UtilLib;

namespace iLabs.ServiceBroker.iLabSB
{
	/// <summary>
	/// Summary description for requestgroup.
	/// </summary>
	public partial class requestGroup : System.Web.UI.Page
	{
		AuthorizationWrapperClass wrapper = new AuthorizationWrapperClass();
        BrokerDB brokerDB = new BrokerDB();
		
        List<IntTag> nonRequestGroups = new List<IntTag>();
		List<IntTag> requestGroups = new List<IntTag>();
        List<IntTag> currentGroups = new List<IntTag>();
		List<Group> canRequestGroups = new List<Group>();

		int userID;
		int[] userGroupIDs;
		int[] groupIDs;

		string supportMailAddress = ConfigurationManager.AppSettings["supportMailAddress"];
		bool adminRequestGroup;
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			userID = Convert.ToInt32(Session["UserID"]);

			if(ConfigurationManager.AppSettings["adminRequestGroup"].Trim().ToLower() == "true")
				adminRequestGroup = true;
			else
				adminRequestGroup = false;
			
			// Reset error message
			lblResponse.Visible = false;

			// Initialize ArrayLists requestGroups, nonRequestGroups
			LoadGroupArrays();

			if(!IsPostBack)
			{

				// List groups user belongs to, and groups user wants to join, in the blue box on the right of the page.
				LoadBlueBox();

				// Load the repeater containing checkboxes next to groups the user may request membership in
				LoadRepeater();
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

		}
		#endregion
		
		/// <summary>
		/// Initialize ArrayLists requestGroups, nonRequestGroups
		/// </summary>
		private void LoadGroupArrays()
		{
            currentGroups.Clear();
            nonRequestGroups.Clear();
            requestGroups.Clear();
            canRequestGroups.Clear();
            // Gets a list of all the groups a user belongs to
            DbParameter idParam = FactoryDB.CreateParameter("@userID", userID, DbType.Int32);
            currentGroups.AddRange(brokerDB.GetIntTags("User_RetrieveGroupTags", idParam));

            int[] curIDs = AdministrativeAPI.ListGroupIDsForUserRecursively(userID);
            List<int> curGroupIDs = new List<int>();
            curGroupIDs.AddRange(curIDs);

			// Gets the list of all requestGroups
           
			Group [] reqGroups = InternalAdminDB.SelectGroupsByType(GroupType.REQUEST);
            if(reqGroups != null && reqGroups.Length > 0){
                foreach(Group g in reqGroups){
                    if(!curGroupIDs.Contains(g.associatedGroupID) && !curGroupIDs.Contains(g.groupID)){
                        canRequestGroups.Add(g);
                    }
                }
            }
		
			foreach(IntTag g in currentGroups)
			{
                if (g.tag.EndsWith("-request"))
                {
                    requestGroups.Add(g);
                }
                else
                {
                    if ((g.tag.ToUpper()).CompareTo("ROOT") != 0)
                        nonRequestGroups.Add(g);
                }
			}
		}

		/// <summary>
		/// List groups user belongs to, and groups user wants to join, in the blue box on the right of the page.
		/// </summary>
		private void LoadBlueBox()
		{
			//List Groups that user belongs to in blue box
            lblGroups.Text = "";
			if ((nonRequestGroups!=null)&& (nonRequestGroups.Count>0))
			{
				
                StringBuilder buf = new StringBuilder();
				for (int i=0;i<nonRequestGroups.Count;i++)
				{
					buf.Append(nonRequestGroups[i].tag);
					if (i != nonRequestGroups.Count-1)
						buf.Append(", ");
				}
                lblGroups.Text = buf.ToString();
			}
			else
			{
				lblGroups.Text = "No group";
			}

			//List Groups that user has requested to in blue box
            lblRequestGroups.Text = "";
			if ((requestGroups!=null)&& (requestGroups.Count>0))
			{
                StringBuilder buf2 = new StringBuilder();
				for (int i=0;i<requestGroups.Count;i++)
				{
                    buf2.Append(requestGroups[i].tag.Remove(requestGroups[i].tag.LastIndexOf("-request")));
					if (i != requestGroups.Count-1)
						buf2.Append(", ");
				}
                lblRequestGroups.Text = buf2.ToString();
			}
			else
			{
				lblRequestGroups.Text = "No group";
			}
		}

		/// <summary>
		/// Load the repAvailableGroups repeater from the canRequestGroups ArrayList
		/// </summary>
        private void LoadRepeater()
        {
            if ((canRequestGroups == null) || (canRequestGroups.Count == 0))
            {
                lblNoGroups.Text = "<p>No groups exist that you may request membership. Contact " + supportMailAddress + " if you wish to be added to any other group.</p>";
                lblNoGroups.Visible = true;
                repAvailableGroups.Visible = false;
                btnRequestMembership.Visible = false;
            }
            else
            {
                try
                {
                    foreach (Group g in canRequestGroups)
                    {
                        g.groupName = g.groupName.Remove(g.groupName.LastIndexOf("-request"));
                    }
                    repAvailableGroups.DataSource = canRequestGroups;
					repAvailableGroups.DataBind();
				
                    // have to bypass wrapper class here				
                    //canRequestGroups = AdministrativeAPI.GetGroups(Utilities.ArrayListToIntArray(canRequestGroupIDs));
                    int itemIdx = 0;
                    int repCount = 1;
                    foreach (Group g in canRequestGroups)
                    {
                       
                        //HiddenField reqID = new HiddenField();
                        //reqID.Value = g.groupID.ToString();
                        //repAvailableGroups.Controls.AddAt(itemIdx, reqID);
                       

                        Label lblGroupLabs = new Label();
                        lblGroupLabs.Visible = true;
                        int[] lcIDsList = AdministrativeUtilities.GetGroupLabClients(g.associatedGroupID);
                        LabClient[] lcList = wrapper.GetLabClientsWrapper(lcIDsList);
                        StringBuilder buf = new StringBuilder();
                        if (lcList != null && lcList.Length > 0)
                        {
                            buf.Append("<p>Associated Labs</p><ul>");
                            for (int i = 0; i < lcList.Length; i++)
                            {
                                buf.Append("<li><strong class=lab>" +
                                    lcList[i].clientName + "</strong> - " +
                                    lcList[i].clientShortDescription + "</li>");
                            }
                            buf.AppendLine("</ul>");
                        }
                        else
                        {
                            buf.AppendLine("<p>No Associated Labs</p>");
                        }
                        lblGroupLabs.Text = buf.ToString();

                        repAvailableGroups.Controls.AddAt(repCount, lblGroupLabs);
                        //HiddenField hdnReqID = new HiddenField();
                        //hdnReqID.Value = g.groupID.ToString();
                        //repAvailableGroups.Controls.AddAt(repCount, hdnReqID);
						repCount += 3;

                        //repAvailableGroups.Controls.AddAt(itemIdx, lblGroupLabs);
                        //Label lblG = new Label();
                        //lblG.Text = g.groupName.Remove(g.groupName.LastIndexOf("-request"));
                        //repAvailableGroups.Controls.AddAt(itemIdx, lblG);
                        //repAvailableGroups.Controls.AddAt(itemIdx, new CheckBox());

                    }
                }
                catch (AccessDeniedException adex)
                {
                    lblResponse.Visible = true;
                    lblResponse.Text = Utilities.FormatErrorMessage(adex.Message);
                }
            }
        }

		
		protected void btnRequestMembership_Click(object sender, System.EventArgs e)
		{
			bool atLeastOneGroupSelected = false;
			for (int i=0; i<repAvailableGroups.Items.Count; i++)
			{
				if(((CheckBox)repAvailableGroups.Items[i].Controls[1]).Checked == true)
				{
					atLeastOneGroupSelected = true;
					try
					{ 
                        AdministrativeAPI.AddUserToGroup(Convert.ToInt32(Session["UserID"]), canRequestGroups[i].groupID);
						
					}
				catch (Exception ex)
					{
						lblResponse.Visible = true;
						lblResponse.Text = Utilities.FormatErrorMessage("Cannot add member to Group! " + ex.Message);
					}
				}
			}
            if (atLeastOneGroupSelected)
            {
                LoadGroupArrays();
                LoadRepeater();
                LoadBlueBox();
            }
            else
            { // If no groups were selected - show a warning
				lblResponse.Visible = true;
				lblResponse.Text = Utilities.FormatWarningMessage("No groups selected!");
				LoadRepeater();
				return;
			}
			
		}
	}
}
