/*
 * Copyright (c) 2011 The Massachusetts Institute of Technology. All rights reserved.
 * Please see license.txt in top level directory for full license.
 * 
 * $Id$
 */
using System;
using System.Collections;
using System.Configuration;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Security;
using System.Web.Mail;
using System.Text;

using Recaptcha;

using iLabs.Core;
using iLabs.DataTypes.ProcessAgentTypes;
using iLabs.DataTypes.TicketingTypes;
using iLabs.ServiceBroker;
using iLabs.ServiceBroker.Administration;
using iLabs.ServiceBroker.Authorization;
using iLabs.ServiceBroker.Internal;

using iLabs.Ticketing;
using iLabs.DataTypes.TicketingTypes;
using iLabs.UtilLib;

namespace iLabs.ServiceBroker.iLabSB
{
	/// <summary>
	/// Help Page
	/// </summary>
	public partial class help : System.Web.UI.Page
	{
		string bugReportMailAddress = ConfigurationManager.AppSettings["bugReportMailAddress"];
		AuthorizationWrapperClass wrapper = new AuthorizationWrapperClass();

		int userID = -1;
		
		User currentUser;
		Exception excep;

		private void Page_Load(object sender, System.EventArgs e)
		{
            if (Request.UserAgent.Contains("MSIE 6") || Request.UserAgent.Contains("MSIE 7"))
            {
               recaptcha.EmbedJavascript = true;
            }
            lblRevision.Text = "<!--  " + iLabGlobal.Release + "  -->";
			userID = -1;
			currentUser = new User();
			if((Session != null) && (Session["UserID"] != null))
			{
				userID = Convert.ToInt32(Session["UserID"]);
				currentUser = wrapper.GetUsersWrapper(new int[] {userID})[0];
			}
            //recaptcha.PublicKey = "6LcLF8ISAAAAAMhwOM1ipf9N1Kh_obO1VG0PwCQB";
            //recaptcha.PrivateKey = "6LcLF8ISAAAAALHhc_wOQibhl3jIc5gf9MUKvOw_";
			if(! IsPostBack)
			{
                
				if (userID != -1) //user logged in
				{
					lblUserName.Visible = false;
					lblEmail.Visible = false;
					txtUserName.Visible = false;
					txtEmail.Visible = false;
				}

                int[] lsIDs = AdministrativeAPI.ListLabServerIDs();
                ProcessAgentInfo[] ls = wrapper.GetProcessAgentInfosWrapper(lsIDs);
				//ddlWhichLab.Items.Add("System-wide error");
			
				String optList = ConfigurationManager.AppSettings["helpOptions"];
				if((optList != null)&& (optList.Length >0)){
					char [] delimiter = {','};
					String [] options =optList.Split(delimiter,100);
					for(int i =0;i< options.Length;i++)
					{
						//ddlHelpType.Items.Add(new ListItem(options[i],i.ToString()));						
						ddlHelpType.Items.Add(options[i]);
					}
					if(options.Length > 0)
					{
						ddlHelpType.Items[0].Selected = false;
					}
				}

                foreach (ProcessAgentInfo l in ls)
                {
                        ddlHelpType.Items.Add(new ListItem(l.agentName));                     
                }
			
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
			this.btnRequestHelp.Click += new System.EventHandler(this.btnRequestHelp_Click);
			this.btnReportBug.Click += new System.EventHandler(this.btnReportBug_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void btnRequestHelp_Click(object sender, System.EventArgs e)
		{
            if (!recaptcha.IsValid)
            {
                lblResponse.Text = Utilities.FormatErrorMessage("You must respond to the security question!");
                lblResponse.Visible = true;
                return;
            }
			if((userID == -1) && (txtEmail.Text.Length == 0))
			{
                lblResponse.Text = Utilities.FormatErrorMessage("Please enter an email address, so that we can respond to you.");
				lblResponse.Visible = true;
			}

			else if(ddlHelpType.SelectedItem.Text.CompareTo("") == 0)
			{
				lblResponse.Text = Utilities.FormatErrorMessage("Please select the type of help you need.");
				lblResponse.Visible = true;
			}
			else if (txtProblem.Text == "")
			{
				lblResponse.Text = Utilities.FormatErrorMessage("Enter a description of the problem!");
				lblResponse.Visible = true;
			}
			else
			{
				string userEmail = null;
				if((currentUser.email != null) && (currentUser.email != ""))
				{
					userEmail = currentUser.email;
				}
				else if( ( txtEmail != null) && (txtEmail.Text != null) && (txtEmail.Text != ""))
				{
					userEmail = txtEmail.Text;
				}

				string helpType = ddlHelpType.SelectedItem.Text;
				StringBuilder sb = new StringBuilder();

				MailMessage mail = new MailMessage();
				mail.To = ConfigurationManager.AppSettings["supportMailAddress"];
				mail.From = userEmail;
				mail.Subject = "[iLabs] Help Request: " + helpType;

				if (userID == -1)
				{
					sb.Append("User Not Logged In:\n\r");
					sb.Append("Username: " + txtUserName.Text + "\n\r");
					sb.Append("Email:  " + txtEmail.Text + "\n\r");
					if(Session["GroupName"] != null)
						sb.Append("Group: " + Session["GroupName"].ToString()+ "\n\r");
				}

				else
				{
					sb.Append(currentUser.firstName + " "+ currentUser.lastName +"\n\r");
					sb.Append("Username: " + currentUser.userName + "\n\r");
					sb.Append("Email:  " + currentUser.email + "\n\r");
					if(Session["GroupName"] != null)
						sb.Append("Group: " + Session["GroupName"].ToString()+ "\n\r");
				}
				sb.Append("\n\r");		
				
				sb.Append("requests help - '"+helpType+"':  \n\r\n\r");
				sb.Append(txtProblem.Text) ;
				sb.Append("\n\r\n\r");
				sb.Append("Additional Information:\n\r");
				sb.Append("User Browser: "+ Request.Browser.Type +"\n\r");
				sb.Append("User Browser Agent: "+ Request.UserAgent +"\n\r");
				sb.Append("User Platform: "+ Request.Browser.Platform+"\n\r");
				sb.Append("URL used to access page: "+ Request.Url+"\n\r");
                sb.Append("Machine Name: " + Server.MachineName + "\n\r");
                sb.Append("Server Type: " + Server.GetType() + "\n\r");
                sb.Append("Site URL: " + ProcessAgentDB.ServiceAgent.codeBaseUrl + "\n\r");
                sb.Append("Site GUID: " + ProcessAgentDB.ServiceAgent.agentGuid + "\n\r");
                sb.Append("iLab Release: " + iLabGlobal.Release + "\n\r");
				
				if(excep != null)
				{
					sb.Append("\n\rException Thrown:\n\r");
					sb.Append(excep.Message + "\n\r\n\r");
					sb.Append(excep.StackTrace);
					Server.ClearError();
				}

				mail.Body = sb.ToString();

				SmtpMail.SmtpServer = "127.0.0.1";

				try
				{
					SmtpMail.Send(mail);
					if(userEmail != null)
					{
						MailMessage uMail = new MailMessage();
						uMail.To = userEmail;
						uMail.From = ConfigurationManager.AppSettings["supportMailAddress"];
						uMail.Subject = "[iLabs] Help Request: " + helpType ;
						uMail.Body = "Thank you for taking the time to request help from us:\n\r";
						uMail.Body += txtProblem.Text;
						SmtpMail.Send(uMail);
					}
					lblResponse.Text = Utilities.FormatConfirmationMessage("Thank-you! Your help request has been submitted. An administrator will contact you within 24-48 hours.");
					lblResponse.Visible = true;

				}
				catch (Exception ex)
				{
					// Report detailed SMTP Errors
					StringBuilder smtpErrorMsg = new StringBuilder();
					smtpErrorMsg.Append("Exception: " + ex.Message);
					//check the InnerException
					if (ex.InnerException != null)
						smtpErrorMsg.Append("<br>Inner Exceptions:");
					while( ex.InnerException != null )
					{
						smtpErrorMsg.Append("<br>" +  ex.InnerException.Message);
						ex = ex.InnerException;
					}
					lblResponse.Text = Utilities.FormatErrorMessage("Error sending your help request, please email " + ConfigurationManager.AppSettings["supportMailAddress"]+ ".<br>" + smtpErrorMsg.ToString());
					lblResponse.Visible = true;
				}
			}
		
		}
		private void btnReportBug_Click(object sender, System.EventArgs e)
		{
			Response.Redirect("reportBug.aspx");
		}
		
	}
}
