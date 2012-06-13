/*
 * Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
 * Please see license.txt in top level directory for full license.
 * 
 * $Id$
 */
using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Security;
using System.Web.Mail;

using iLabs.Core;
using iLabs.DataTypes;
using iLabs.DataTypes.ProcessAgentTypes;
using iLabs.DataTypes.TicketingTypes;
using iLabs.ServiceBroker.Administration;
using iLabs.ServiceBroker.Authorization;
using iLabs.ServiceBroker.Internal;
using iLabs.UtilLib;


namespace iLabs.ServiceBroker.iLabSB
{
	/// <summary>
	/// reportBug
	/// </summary>
	public partial class reportBug : System.Web.UI.Page
	{
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
			userID = -1;
			currentUser = new User();
			if((Session != null) && (Session["UserID"] != null))
			{
				userID = Convert.ToInt32(Session["UserID"]);
				currentUser = wrapper.GetUsersWrapper(new int[] {userID})[0];
			}

			if(! IsPostBack)
			{
			
				if(Request.Params["ex"] != null)
					excep = Server.GetLastError();
				
				if(userID == -1)
				{
					lblUserName.Visible = true;
					txtUserName.Visible = true;
					lblEmail.Visible = true;
					txtEmail.Visible = true;
				}


				String optList = ConfigurationManager.AppSettings["bugReportOptions"];
				if((optList != null)&& (optList.Length >0))
				{
					char [] delimiter = {','};
					String [] options =optList.Split(delimiter,100);
					for(int i =0;i< options.Length;i++)
					{
						//ddlArea.Items.Add(new ListItem(options[i],i.ToString()));
						ddlArea.Items.Add(options[i]);
					}
					if(options.Length > 0)
					{
						ddlArea.Items[0].Selected = false;
					}
				}
                // TO DO: this is not what I would expect
                IntTag[] ls = wrapper.GetProcessAgentTagsByTypeWrapper(ProcessAgentType.LAB_SERVER);
				
				ddlArea.Items.Add("Content - need to change");
                foreach (IntTag l in ls)
                {
                    ddlArea.Items.Add(new ListItem(l.tag, l.id.ToString()));
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
			this.btnReportBug.Click += new System.EventHandler(this.btnReportBug_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

	
		private void btnReportBug_Click(object sender, System.EventArgs e)
		{
			if((userID == -1) && (txtEmail.Text.Length == 0))
			{
                lblResponse.Text = Utilities.FormatErrorMessage("Please enter an emailaddress, so we can respond to your report.");
				lblResponse.Visible = true;
			}
			else if(ddlArea.SelectedItem.Text.CompareTo("") == 0)
			{
				lblResponse.Text = Utilities.FormatErrorMessage("Please select a general problem catagory.");
				lblResponse.Visible = true;
			}
			else if (txtBugReport.Text == "")
			{
				lblResponse.Text = Utilities.FormatErrorMessage("Please enter a description of the problem!");
				lblResponse.Visible = true;
			}
            else if (!recaptcha.IsValid)
            {
                lblResponse.Text = Utilities.FormatErrorMessage("You must enter the security code!");
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
				string problemArea = ddlArea.SelectedItem.Text;
				StringBuilder sb = new StringBuilder();
                ProcessAgentDB agentDB = new ProcessAgentDB();
                SystemSupport support = agentDB.RetrieveSystemSupport(ProcessAgentDB.ServiceGuid);
				//Generate email
				MailMessage mail = new MailMessage();
                if (support.bugEmail != null && support.bugEmail.Length > 2)
                {
                    mail.To = support.bugEmail;
                }
                else
                {
                    mail.To = ConfigurationManager.AppSettings["bugReportMailAddress"];
                }
				mail.From = userEmail;
				mail.Subject = "[iLabs] " + Server.MachineName + " Bug Report: " + problemArea ;
				
				if(userID == -1 )
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
				
				
				sb.Append("reports the following bug '"+problemArea+"':  \n\r\n\r");
				sb.Append(txtBugReport.Text) ;
				sb.Append("\n\r\n\r");
				sb.Append("Additional Information:\n\r");
				sb.Append("User Browser: "+ Request.Browser.Type +"\n\r");
				sb.Append("User Browser Agent: "+ Request.UserAgent +"\n\r");
				sb.Append("User Platform: "+ Request.Browser.Platform+"\n\r");
				sb.Append("URL used to access page: "+ Request.Url+"\n\r");
				sb.Append("Machine Name: " + Server.MachineName+"\n\r");
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
						uMail.From = ConfigurationManager.AppSettings["bugReportMailAddress"];
						uMail.Subject = "[iLabs] Bug Report: " + problemArea ;
						uMail.Body = "Thank you for taking the time to report the following bug:\n\r";
						uMail.Body += txtBugReport.Text;
						SmtpMail.Send(uMail);
					}
					lblResponse.Text = Utilities.FormatConfirmationMessage("Thank-you! Your bug report has been submitted. An administrator will contact you within 24-48 hours.");
					lblResponse.Visible = true;

				}
				catch (Exception ex)
				{
					lblResponse.Text = Utilities.FormatErrorMessage("Error sending your bug report, please email "
						+ ConfigurationManager.AppSettings["bugReportMailAddress"] + ". "+ex.Message);
					lblResponse.Visible = true;
				}
			}
		}
	}
}
