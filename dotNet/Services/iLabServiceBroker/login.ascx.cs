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
using iLabs.ServiceBroker.Authentication;
using iLabs.ServiceBroker.Authorization;



namespace iLabs.ServiceBroker.iLabSB
{
	

	/// <summary>
	///	Login User Control.
	///	Encapsulates Login functionality.
	/// </summary>
	public partial class login1 : System.Web.UI.UserControl
	{
		
		AuthorizationWrapperClass wrapper = new AuthorizationWrapperClass();
		string supportMailAddress = ConfigurationManager.AppSettings["supportMailAddress"];

		protected void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here
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

		protected void btnLogIn_Click(object sender, System.EventArgs e)
		{
			if(txtUsername.Text.Equals("") || txtPassword.Text.Equals(""))
			{
				lblLoginErrorMessage.Text = "<div class=errormessage><p>Missing user ID and/or password field. </p></div>";
				lblLoginErrorMessage.Visible = true;
				return;
			}
			
			int userID = -1;
			userID = wrapper.GetUserIDWrapper(txtUsername.Text,0);

			if (userID >0)
			{
				User user = wrapper.GetUsersWrapper(new int[] {userID})[0];

				if(userID != -1 && user.lockAccount == true)
				{
					lblLoginErrorMessage.Text = "<div class=errormessage><p>Account locked - Email " + supportMailAddress + ". </p></div>";
					lblLoginErrorMessage.Visible = true;
					return;
				}

				if (CheckCredentials(userID , txtPassword.Text ))
				{
					FormsAuthentication.SetAuthCookie (txtUsername.Text , false);
					Session["UserID"] = userID;
					Session["UserName"] = user.userName;
                    Session["UserTZ"] = Request.Params["userTZ"];
					Session["SessionID"] = AdministrativeAPI.InsertUserSession (userID, 0, Convert.ToInt32(Request.Params["userTZ"]), Session.SessionID.ToString()).ToString ();
                    HttpCookie cookie = new HttpCookie(ConfigurationManager.AppSettings["isbAuthCookieName"], Session["SessionID"].ToString());
                    Response.AppendCookie(cookie);
					Response.Redirect(Global.FormatRegularURL(Request,"myGroups.aspx"));
				}
				else
				{
					lblLoginErrorMessage.Text = "<div class=errormessage><p>Invalid user ID and/or password. </p></div>";
					lblLoginErrorMessage.Visible = true;
				}
			}
			else
			{
				lblLoginErrorMessage.Text = "<div class=errormessage><p>Username does not exist. </p></div>";
				lblLoginErrorMessage.Visible = true;
			}
		}

		/// <summary>
		/// Authenticates a user against information in the database.
		/// </summary>
		/// <param name="userID"></param>
		/// <param name="password"></param>
		/// <returns></returns>
		private bool CheckCredentials (int userID, string password)
		{
			if(AuthenticationAPI.Authenticate (userID, password))
				return true;
			else
				return false;
		}
	}
}
