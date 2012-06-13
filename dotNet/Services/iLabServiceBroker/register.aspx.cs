/*
 * Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
 * Please see license.txt in top level directory for full license.
 * 
 * $Id$
 */
using System;
using System.Configuration;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Security;
using System.Web.Mail;
using System.Xml;
using System.Xml.XPath;

using iLabs.Core;
using iLabs.DataTypes;
using iLabs.DataTypes.ProcessAgentTypes;
using iLabs.ServiceBroker;
using iLabs.ServiceBroker.Authentication;
using iLabs.ServiceBroker.Administration;
using iLabs.ServiceBroker.Authorization;
using iLabs.ServiceBroker.Internal;
using iLabs.UtilLib;

namespace iLabs.ServiceBroker.iLabSB
{
	/// <summary>
	/// Summary description for register.
	/// </summary>
	public partial class register : System.Web.UI.Page
	{
		string registrationMailAddress = ConfigurationManager.AppSettings["registrationMailAddress"];
		string supportMailAddress = ConfigurationManager.AppSettings["supportMailAddress"];
        bool chooseGroup = true;
        bool useRequestGroups = true;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			AuthorizationWrapperClass wrapper = new AuthorizationWrapperClass();
            if (Request.UserAgent.Contains("MSIE 6") || Request.UserAgent.Contains("MSIE 7"))
            {
                recaptcha.EmbedJavascript = true;
            }
            // Group options -- Default is to use the DropDownList with request groups, if no request group is selected
            //		the specified initialGroup will be used. Default to newUserGroup if no initialGroup
            // If useRequestGroup is set to false the dropdownList will be populated with actual groups and user will be
            //		made a member of the selected group. If defaultGroups is set the comma delimited list of groups will be used.
            // If chooseGroup is set to false the dropdown list will not be displayed and user will be assigned to the initialGroup

            if (ConfigurationManager.AppSettings["chooseGroups"] != null)
            {
                if (ConfigurationManager.AppSettings["chooseGroups"].Equals("false"))
                    chooseGroup = false;
            }
            if (ConfigurationManager.AppSettings["useRequestGroup"] != null)
            {
                if (ConfigurationManager.AppSettings["useRequestGroup"].Equals("false"))
                    useRequestGroups = false;
            }
			if(!IsPostBack)
			{
                // Set up affiliation options
				if(ConfigurationManager.AppSettings["useAffiliationDDL"].Equals("true"))
				{
					String afList = ConfigurationManager.AppSettings["affiliationOptions"];
					char [] delimiter = {','};
					String [] options =afList.Split(delimiter,100);
                    ddlAffiliation.Items.Clear();
					for(int i =0;i< options.Length;i++)
					{
						ddlAffiliation.Items.Add(options[i]);
					}
					if(options.Length > 0)
					{
						ddlAffiliation.Items[0].Selected = false;
					}
				}
				else
				{
					// Setup default affiliation
				}

                if (chooseGroup)
                {
                    ddlGroup.Items.Clear();
                    ddlGroup.Items.Add("-- None --");
                    //Don' t use wrapper since it only lists a user's group
                    int[] gpIDs = wrapper.ListGroupIDsWrapper();
                    Group[] gps = AdministrativeAPI.GetGroups(gpIDs);

                    ArrayList aList = new ArrayList();
                    for (int i = 0; i < gps.Length; i++)
                    {
                        if (useRequestGroups)
                        {
                            if (gps[i].groupType.Equals(GroupType.REQUEST))
                            {
                                int origGroupID = AdministrativeAPI.GetAssociatedGroupID(((Group)gps[i]).groupID);
                                string origGroupName = AdministrativeAPI.GetGroups(new int[] { origGroupID })[0].groupName;
                                aList.Add(origGroupName);
                            }
                        }
                        else
                        {
                            if (gps[i].groupType.Equals(GroupType.REGULAR) && (gps[i].groupID >= 10))
                            {
                                aList.Add(gps[i].groupName);
                            }
                        }
                    }
                    for (int i = 0; i < aList.Count; i++)
                    {
                        ddlGroup.Items.Add(aList[i].ToString());
                    }
                }
                else
                {
                    ddlGroup.Visible = false;
                    trowRequestGroup.Visible = false;
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
			this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void btnSubmit_Click(object sender, System.EventArgs e)
		{
            bool error = false;
            lblResponse.Text = "";
            lblResponse.Visible = false;
            if (!recaptcha.IsValid)
            {
                lblResponse.Text = Utilities.FormatErrorMessage("You must enter the security code!");
                lblResponse.Visible = true;
                return;
            }

            StringBuilder msg = new StringBuilder();
			AuthorizationWrapperClass wrapper = new AuthorizationWrapperClass();
            string userName = null;
            if (txtUsername.Text.Trim() == "" || txtFirstName.Text.Trim() == "" || txtLastName.Text.Trim() == "" || txtEmail.Text.Trim() == "" || txtPassword.Text.Trim() == "" || txtConfirmPassword.Text.Trim() == "")
			{
                msg.Append("You must enter a Username, first name, last name, email and password.<br/>");
                error = true;
				
			}
            if (txtPassword.Text.Trim() != txtConfirmPassword.Text.Trim())
			{
				msg.Append("Password fields don't match, please reenter.<br/>");
                error = true;
				txtPassword.Text = null;
				txtConfirmPassword.Text = null;
			}
            userName = txtUsername.Text.Trim();
            int curUser = AdministrativeAPI.GetUserID(userName,0);
            if (curUser > 0)
            {
                msg.Append("The username you entered is already registered. Please check to see if you have a forgotten password, or choose another username.<br/>");
                error = true;
                txtPassword.Text = null;
                txtConfirmPassword.Text = null;
            }
			if(ConfigurationManager.AppSettings["useAffiliationDDL"].Equals("true"))
			{
				if (ddlAffiliation.SelectedIndex < 1)
				{
					msg.Append("Please select an affiliation.<br/>");
                    error = true;
				}
			}
			else
			{
				if (txtAffiliation.Text.Trim() == "")
				{
					msg.Append("Please enter an affiliation.<br/>");
					error = true;
				}
			}
            if (error)
            {
                lblResponse.Text = Utilities.FormatErrorMessage(msg.ToString());
                lblResponse.Visible = true;
                return;
            }

			try
			{
				string firstName = txtFirstName.Text.Trim() ;
				string lastName = txtLastName.Text.Trim() ;
				string email = txtEmail.Text.Trim() ;
				string affiliation;
				if(ConfigurationManager.AppSettings["useAffiliationDDL"].Equals("true"))
				{
					affiliation = ddlAffiliation.Items [ddlAffiliation.SelectedIndex ].Value ;
				}
				else
				{
					affiliation = txtAffiliation.Text.Trim();
				}
				string principalString = userName;
				string authenType = AuthenticationType.NativeAuthentication ;
				string reason = txtReason.Text.Trim();
                if (ConfigurationManager.AppSettings["chooseGroups"] != null)
                {
                    if (ConfigurationManager.AppSettings["chooseGroups"].Equals("false"))
                        chooseGroup = false;
                }
                int initialGroup = wrapper.GetGroupIDWrapper(Group.NEWUSERGROUP);
                int newUserGroupID = initialGroup;
                if (ConfigurationManager.AppSettings["initialGroup"] != null)
                {
                    int tmpID = wrapper.GetGroupIDWrapper(ConfigurationManager.AppSettings["initialGroup"]);
                    if (tmpID > 0)
                        initialGroup = tmpID;
                }
                if (chooseGroup)
                {
                    if (ConfigurationManager.AppSettings["useRequestGroup"] != null)
                    {
                        if (ConfigurationManager.AppSettings["useRequestGroup"].Equals("false"))
                            useRequestGroups = false;
                    }

                    if (ddlGroup.SelectedIndex > 0)

                        initialGroup = wrapper.GetGroupIDWrapper(ddlGroup.Items[ddlGroup.SelectedIndex].Text);
                }

				int userID = -1;
				try
				{
					// adduserwrapper doesn't work here since there the user isn't logged in yet.
					// user the admin API call directly instead
                    if ((useRequestGroups) && (initialGroup != newUserGroupID))
                    {
                        initialGroup = AdministrativeUtilities.GetGroupRequestGroup(initialGroup);
                        userID = AdministrativeAPI.AddUser(userName, 0, (int) AuthenticationType.AuthTypeID.Native, firstName, lastName, email,
                            affiliation.Trim(), reason, "",initialGroup, false);
                        msg.Append("Added user: " + userName + " into request group ");
                    }
                    else
                    {
                        userID = AdministrativeAPI.AddUser(userName, 0, (int) AuthenticationType.AuthTypeID.Native, firstName, lastName, email,
                            affiliation.Trim(), reason, "", initialGroup, false);
                        msg.Append("Added user: " + userName + " ");
                    }
                }
				catch(Exception ex)
				{
                    msg.Append("Exception adding user! User could not be added.<br/>Please notify " + supportMailAddress +"<br/>");
                    msg.Append(Utilities.DumpException(ex));
					lblResponse.Text = Utilities.FormatErrorMessage(msg.ToString());
					lblResponse.Visible = true;
					return;
				}

				if( userID!= -1)
				{
					Session["UserID"] = userID;
					Session["UserName"] = userName;
					AuthenticationAPI.SetNativePassword (userID, txtPassword.Text.Trim() );
					// setnativepasswordwrapper doesn't work here since there the user isn't logged in yet.
					// user the admin API call directly instead
					//wrapper.SetNativePasswordWrapper (userID, txtPassword.Text );

					FormsAuthentication.SetAuthCookie(userName , false);
                    Session["UserID"] = userID;
                    Session["UserName"] = userName;
                    Session["UserTZ"] = Request.Params["userTZ"];
                    Session["SessionID"] = AdministrativeAPI.InsertUserSession(userID, initialGroup, Convert.ToInt32(Request.Params["userTZ"]), Session.SessionID.ToString()).ToString();
                    HttpCookie cookie = new HttpCookie(ConfigurationManager.AppSettings["isbAuthCookieName"], Session["SessionID"].ToString());
                    Response.AppendCookie(cookie);
                    try
                    {
                        // Check for GroupItems, since the user may not be in the target group at this time
                        // We can not recusively check all groups, but will us the initial target group.
                        //int[] groupIDs = AdministrativeAPI.ListGroupsForAgentRecursively(userID);
                        Group[] groups = AdministrativeAPI.GetGroups(new int[] { initialGroup });
                        foreach (Group grp in groups)
                        {
                            if (ConfigurationManager.AppSettings[grp.groupName + "Item"] != null)
                            {
                                string docUrl = ConfigurationManager.AppSettings[grp.groupName + "Item"];

                                if (docUrl != null)
                                {
                                    addClientItems(docUrl, userID);
                                }
                            }
                        }
                    }
                    catch (Exception ge)
                    {
                        msg.Append("Error trying group Items: " + ge.Message + "<br/>");
                        error = true;
                        //lblResponse.Text = Utilities.FormatErrorMessage(ge.Message);
                    }
					// email registration
                    StringBuilder message = new StringBuilder();
                    string subject = "[iLabs] New User Registration";
                    message.Append("\n");
                    message.Append("User Name: " + userName + "\n\r");
                    message.Append("Name: " + firstName + " " + lastName + "\n\r");
                    message.Append("Email:  " + email + "\n\r\n\r");
                    message.Append("iLab URL:  " + ProcessAgentDB.ServiceAgent.codeBaseUrl + "\n\r\n\r");
                    Group[] myGroups = AdministrativeAPI.GetGroups(new int[] { initialGroup });
                    if (useRequestGroups)
                    {
                        subject += " Request";
                        message.Append("You have requested to be added to: " + myGroups[0].GroupName + "\n\r\n\r");
                        message.Append("Your request has been forwarded to the administrator. ");
                        message.Append("An email will be sent to you once your request has been processed.\n\r\n\r");

                    }
                    else
                    {
                        subject = "[iLabs] New User Registration";
                        message.Append("You have been added to: " + myGroups[0].GroupName + "\n\r\n\r");

                    }
                    MailMessage mail = new MailMessage();
                    mail.From = registrationMailAddress;
                    mail.To = registrationMailAddress;
                    if (email != "")
                    {
                        mail.Cc = email;
                    }
                    mail.Subject = subject;
                    mail.Body = message.ToString();

					SmtpMail.SmtpServer = "127.0.0.1";
						
					try
					{
						SmtpMail.Send(mail);
						Response.Redirect("login.aspx");
					}
					catch (Exception ex)
					{
						// Report detailed SMTP Errors
						string smtpErrorMsg;
						smtpErrorMsg = "SMTP Error-Exception: " + ex.Message;
						//check the InnerException
						if (ex.InnerException != null)
							smtpErrorMsg += "<br>Inner Exceptions:";
						while( ex.InnerException != null )
						{
							smtpErrorMsg += "<br>" +  ex.InnerException.Message;
							ex = ex.InnerException;
						}

						
						msg.Append("Your request has been submitted, but the system was unable to send the notification email. Please cut & paste this entire message, and send it to " + registrationMailAddress);
						msg.Append("<br><br>" + mail.Subject + "<br>" + mail.Body);
						msg.Append("<br><br>" + smtpErrorMsg);
						error = true;
					}
				}
				else
				{
					msg.Append("Your ID has been taken. Please choose a different user ID.<br/>");
					error = true;
				}
				// moved 2 statements into if block which sets user ID to the session - Karim
			}
			catch (Exception ex)
			{
                msg.Append("Error registering this user. Please report to an administrator at " + supportMailAddress + ".<br/>");
                msg.Append(Utilities.DumpException(ex) + "<br/>");
				error = true;
			}
            if (error)
            {
                lblResponse.Text = Utilities.FormatErrorMessage(msg.ToString());
                lblResponse.Visible = true;
            }
            else if (msg.Length > 0)
            {
                lblResponse.Text = Utilities.FormatConfirmationMessage(msg.ToString());
                lblResponse.Visible = true;
            }

		}


        /// <summary>
        /// Parses the contents of a URL, expected contents are line delimited.
        /// Each entry must contain 3 lines of text. 
        ///		client name on this service broker
        ///		item name - must be unique for the specified client
        ///		data
        /// There may be multiple entries
        /// </summary>
        /// <param name="url"></param>
        /// <param name="userID"></param>
        private void addClientItems(String url, int userID)
        {
            WebResponse result = null;
            try
            {
                WebRequest req = WebRequest.Create(url);
                result = req.GetResponse();
                Stream ReceiveStream = result.GetResponseStream();
                StreamReader sr = new StreamReader(ReceiveStream);


                string client = null;
                string name = null;
                string data = null;

                while ((client = sr.ReadLine()) != null)
                {
                    name = sr.ReadLine();
                    data = sr.ReadLine();
                    int clientID = InternalAdminDB.GetLabClientIDFromName(client);
                    AdministrativeAPI.SaveClientItemValue(clientID, userID, name, data);
                }

            }
            catch (Exception ex)
            {
                lblResponse.Text = Utilities.FormatErrorMessage("Error registering this user. Please report to an administrator at " + supportMailAddress + ".<br>" + ex.Message);
                lblResponse.Visible = true;
            }
            finally
            {
                if (result != null)
                {
                    result.Close();
                }
            }
        }
  
	
	}
}
