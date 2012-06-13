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
using iLabs.ServiceBroker.Administration;

namespace iLabs.ServiceBroker.iLabSB
{
	
	/// <summary>
	///		Summary description for userNav.
	/// </summary>
	public partial class userNav : System.Web.UI.UserControl
	{


		protected string currentPage;
		protected string helpURL = "help.aspx";

		public string HelpURL
		{
			get {return helpURL;}
			set {helpURL = value; }
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			// Get the current page name w/o path name or slashes
			currentPage = Request.Url.Segments[Request.Url.Segments.Length -1];
			aHelp.HRef = helpURL;
			
			// Only show the link to Home if not logged in
			if (Session["UserID"] == null)
			{	
				aHome.Attributes.Add("class", "only");
				liNavlistMyGroups.Visible = false;
				liNavlistMyLabs.Visible = false;
				liNavlistExperiments.Visible = false;
				liNavlistMyAccount.Visible = false;
				liNavlistAdmin.Visible = false;
                liNavlistServiceAdmin.Visible = false;
                lbtnLogout.Visible = false;
			}
			else
			{
				lbtnLogout.Visible = true;
				lbtnLogout.Attributes["onclick"] = "parent.theapplet.location.replace('no_applet.html');";
				SetNavList();
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

		/// <summary>
		/// Sets the button state on the items in the unorderd list "ulNavList"
		/// </summary>
		private void SetNavList()
		{
            // check that this user has admin privilidges, in which case, the manageUsers page should be sent to it.
            object adminState = Session["IsAdmin"];
            bool isSuperUser = false;
            if(Session["GroupName"] != null)
                isSuperUser = (Session["GroupName"].ToString().Equals(Group.SUPERUSER));
			liNavlistAdmin.Visible = ((adminState != null) && Convert.ToBoolean(adminState));

            // check that this user has service admin privilidges, in which case, the adminServices page should be sent to it.
            object serviceAdminState = Session["IsServiceAdmin"];
            liNavlistServiceAdmin.Visible = ((serviceAdminState != null) && Convert.ToBoolean(serviceAdminState));

			// Do not show Labs or Experiments if Effective Group has not been specified
			if (Session["GroupID"] !=null)
			{
				if ((bool)adminState)
				{
                    liNavlistMyLabs.Visible = !isSuperUser;
                    liNavlistExperiments.Visible = !isSuperUser;
				}
                else if ((bool)serviceAdminState)
                {
                    liNavlistMyLabs.Visible = false;
                    liNavlistExperiments.Visible = false;
                }
                else
                {
                    liNavlistMyLabs.Visible = true;
                    liNavlistExperiments.Visible = true;
                }
			}
			else
			{
				liNavlistMyLabs.Visible = false;
				liNavlistExperiments.Visible = false;
			}

			// Only show the groups page if the user has more than one lab
            if (Session["GroupCount"] == null 
                || Session["GroupCount"].ToString().Length == 0
                || Convert.ToInt32(Session["GroupCount"]) <= 1)
            {
                int[] grps = AdministrativeAPI.ListNonRequestGroupsForUser(Convert.ToInt32(Session["UserID"]));
                if (grps != null)
                {
                    Session["GroupCount"] = grps.Length;
                }
                else
                {
                    Session["GroupCount"] = 0;
                }
            }
			if (Convert.ToInt32(Session["GroupCount"]) > 1)
			{
				liNavlistMyGroups.Visible = true;
			}
			else
			{
				liNavlistMyGroups.Visible = false;
			}
			
			//Logout Button
			lbtnLogout.CausesValidation = false;

			switch(currentPage)
			{
				case "home.aspx":
					aHome.Attributes.Add("class", "topactive");
					aMyAccount.Attributes.Add("class", "last");
					break;
				case "myGroups.aspx":
					aHome.Attributes.Add("class", "first");
					aMyGroups.Attributes.Add("class", "topactive");
					aMyAccount.Attributes.Add("class", "last");
					break;
				case "myClient.aspx":
					//Note: the myLabs page determines which clients a user/group can access,
					// then redirects to myClient.aspx. So myLabs.aspx is never displayed, though
					// it looks as though it is the page to be linked to.
					aHome.Attributes.Add("class", "first");
					aMyLabs.Attributes.Add("class", "topactive");
                    liNavlistExperiments.Visible = true;
					aMyAccount.Attributes.Add("class", "last");
					break;
                case "myClientList.aspx":
                    //Note: the myLabs page determines which clients a user/group can access,
                    // then redirects to myClient.aspx. So myLabs.aspx is never displayed, though
                    // it looks as though it is the page to be linked to.
                    aHome.Attributes.Add("class", "first");
                    aMyLabs.Attributes.Add("class", "topactive");
                    aMyAccount.Attributes.Add("class", "last");
                    break;
				case "myExperiments.aspx":
					aHome.Attributes.Add("class", "first");
					aMyExperiments.Attributes.Add("class", "topactive");
					aMyAccount.Attributes.Add("class", "last");
					break;
				case "requestGroup.aspx":
					aHome.Attributes.Add("class", "first");
					aMyAccount.Attributes.Add("class", "last");
					break;
				case "help.aspx":
					aHome.Attributes.Add("class", "first");
					aHelp.Attributes.Add("class", "topactive");
					aMyAccount.Attributes.Add("class", "last");
					break;
				case "myAccount.aspx":
					aHome.Attributes.Add("class", "first");
					aMyAccount.Attributes.Add("class", "topactive");
					break;
				default:
					aHome.Attributes.Add("class", "first");
					aMyAccount.Attributes.Add("class", "last");
					break;
			}
		}
		
		protected void lbtnLogout_Click(object sender, System.EventArgs e)
		{
			AdministrativeAPI.SaveUserSessionEndTime (Convert.ToInt64 (Session["SessionID"]));
			FormsAuthentication.SignOut();
            Session.Clear();
			Session.Abandon();
			Response.Redirect("login.aspx");
		}
	}
}
