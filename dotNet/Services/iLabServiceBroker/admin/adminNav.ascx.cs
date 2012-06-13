/*
 * Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
 * Please see license.txt in top level directory for full license.
 * 
 * $Id$
 */

using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Security;
using System.Configuration;
using iLabs.ServiceBroker.Administration;
using iLabs.ServiceBroker.iLabSB;

namespace iLabs.ServiceBroker.admin
{
	

	/// <summary>
	///		Summary description for adminNav.
	/// </summary>
	public partial class adminNav : System.Web.UI.UserControl
	{
		protected string currentPage;
		protected string helpURL = "../help.aspx";

		public string HelpURL
		{
			get {return helpURL;}
			set {helpURL = value; }
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			//Don't see the following pages if you're not superUser
			// LabServers & Clients
			// Grants

            if (!Session["GroupName"].ToString().Equals(Group.SUPERUSER))
            {
                aManageServices.Visible = false;
                aGrants.Visible = false;
                aMyLabs.Visible = true;
            }
            else
            {
                aMyLabs.Visible = false;
            }

			// Get the current page name w/o path name or slashes
			currentPage = Request.Url.Segments[Request.Url.Segments.Length -1];
			
			//Set sub-menus to not visible by default
			ulNav3Labs.Visible = false;
			ulNav3UsersGroups.Visible = false;
			ulNav3Records.Visible = false;

			//Logout Button
			lbtnLogout.CausesValidation = false;
			lbtnLogout.Attributes["onclick"] = "parent.theapplet.location.replace('../no_applet.html');";
			aHelp.HRef = helpURL;
			switch(currentPage)
			{
					// Groups group
				case "myGroups.aspx":
					aMyGroups.Attributes.Add("class", "topactive");
					break;

					// Labs Group
					//    Nav level
				

					//   Nav3 level
                case "SelfRegistration.aspx":
                    aManageServices.Attributes.Add("class", "topactive");
                    ulNav3Labs.Visible = true;
                    //aNav3ServiceBrokerInfo.Attributes.Add("class", "first");
                    aNav3ServiceBrokerInfo.Attributes.Add("class", "nav3active");
                    aNav3Authorities.Attributes.Add("class", "last"); 
                    break;
				case "manageServices.aspx":
					aManageServices.Attributes.Add("class", "topactive");
					ulNav3Labs.Visible = true;
                    aNav3ServiceBrokerInfo.Attributes.Add("class", "first");
					aNav3ManageServices.Attributes.Add("class", "nav3active");
                    aNav3Authorities.Attributes.Add("class", "last"); 
					break;
				case "manageLabClients.aspx":
					aManageServices.Attributes.Add("class", "topactive");
					ulNav3Labs.Visible = true;
                    aNav3ServiceBrokerInfo.Attributes.Add("class", "first"); ;
					aNav3ManageLabClients.Attributes.Add("class", "nav3active");
                    aNav3Authorities.Attributes.Add("class", "last"); 
					break;
				case "crossRegistration.aspx":
					aManageServices.Attributes.Add("class", "topactive");
					ulNav3Labs.Visible = true;
                    aNav3ServiceBrokerInfo.Attributes.Add("class", "first");
					aNav3ManageLabs.Attributes.Add("class", "nav3active");
                    aNav3Authorities.Attributes.Add("class", "last"); 
					break;
                case "authorities.aspx":
                    aManageServices.Attributes.Add("class", "topactive");
                    ulNav3Labs.Visible = true;
                    aNav3ServiceBrokerInfo.Attributes.Add("class", "first");
                    aNav3Authorities.Attributes.Add("class", "nav3active");
                    //aNav3Authorities.Attributes.Add("class", "last");
                    break;
					// Users Group
					//   Nav level
				case "manageUsers.aspx":
					aManageUsers.Attributes.Add("class", "topactive");
					ulNav3UsersGroups.Visible = true;
                    aNav3ManageUsers.Attributes.Add("class", "first");
					aNav3ManageUsers.Attributes.Add("class", "nav3active");
					// aNav3AdministerGroups
					aNav3GroupMembership.Attributes.Add("class", "last");
					break;

					//  Nav3 level
				case "administerGroups.aspx":
					aManageUsers.Attributes.Add("class", "topactive");
					ulNav3UsersGroups.Visible = true;
					aNav3ManageUsers.Attributes.Add("class", "first");
					aNav3AdministerGroups.Attributes.Add("class", "nav3active");
					aNav3GroupMembership.Attributes.Add("class", "last");
					break;
				case "groupMembership.aspx":
					aManageUsers.Attributes.Add("class", "topactive");
					ulNav3UsersGroups.Visible = true;
					aNav3ManageUsers.Attributes.Add("class", "first");
					// aNav3AdministerGroups.Attributes.Add("class", "");
					aNav3GroupMembership.Attributes.Add("class", "nav3active");
                    aNav3GroupMembership.Attributes.Add("class", "last");
					break;

					// Grants Group
				case "grants.aspx":
					aGrants.Attributes.Add("class", "topactive");
					break;

                case "adminResourceMappings.aspx":
                    aMappings.Attributes.Add("class", "topactive");                    
                    break;
                    
					// Records Group
					//   Nav Level
				case "experimentRecords.aspx":
					aExperimentRecords.Attributes.Add("class", "topactive");
					ulNav3Records.Visible = true;
					aNav3ExperimentRecords.Attributes.Add("class", "nav3active");
					aNav3SessionHistory.Attributes.Add("class", "last");
					break;
                case "showAdminExperiment.aspx":

                    ulNav3Records.Visible = true;
                    //AdminNav1_aExperimentRecords.visible = true;
                    aExperimentRecords.Attributes.Add("class", "topactive");
                    //ulNav3Records.Visible = true;
                    //ulNav3Records.Visible = true;
                    //aNav3ExperimentRecords.Attributes.Add("class", "nav3active");
                    aNav3SessionHistory.Attributes.Add("class", "last");
                    break;

					//  Nav3 level
                case "sbStats.aspx":
                    aExperimentRecords.Attributes.Add("class", "topactive");
                    ulNav3Records.Visible = true;
                    aNav3SBinfo.Attributes.Add("class", "first");
                    aNav3SBinfo.Attributes.Add("class", "nav3active");
                    aNav3SessionHistory.Attributes.Add("class", "last");
                    break;

                case "sbReport.aspx":
                    aExperimentRecords.Attributes.Add("class", "topactive");
                    ulNav3Records.Visible = true;
                    aNav3SBinfo.Attributes.Add("class", "first");
                    aNav3Reports.Attributes.Add("class", "nav3active");
                    aNav3SessionHistory.Attributes.Add("class", "last");
                    break;

				case "loginRecords.aspx":
					aExperimentRecords.Attributes.Add("class", "topactive");
					ulNav3Records.Visible = true;
                    aNav3SBinfo.Attributes.Add("class", "first");
                    //aNav3LoginRecords.Attributes.Add("class", "last");
					aNav3LoginRecords.Attributes.Add("class", "nav3active");
                    aNav3SessionHistory.Attributes.Add("class", "last");
                    
					break;
                case "sessionHistory.aspx":
                    aExperimentRecords.Attributes.Add("class", "topactive");
                    ulNav3Records.Visible = true;
                    aNav3SBinfo.Attributes.Add("class", "first");
                    //aNav3LoginRecords.Attributes.Add("class", "last");
                    aNav3SessionHistory.Attributes.Add("class", "nav3active");

                    break;
					// Nav Level
				case "messages.aspx":
					aMessages.Attributes.Add("class", "topactive");
					break;

				default:
					break;
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
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{

		}
		#endregion

		protected void lbtnLogout_Click(object sender, System.EventArgs e)
		{
			AdministrativeAPI.SaveUserSessionEndTime (Convert.ToInt64 (Session["SessionID"]));
			FormsAuthentication.SignOut();
			Session.Abandon();
			bool requireSSL = Convert.ToBoolean(ConfigurationManager.AppSettings["haveSSL"]);
			string URL = "";
			if (requireSSL)
				URL = Global.FormatSecureURL(Request,"login.aspx");
			else
				URL = Global.FormatRegularURL(Request,"login.aspx");
			Response.Redirect(URL);
		}
	}
}
