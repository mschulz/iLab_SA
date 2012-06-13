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
using System.Configuration;
using System.Web.Mail;

using iLabs.DataTypes;
using iLabs.ServiceBroker;
using iLabs.ServiceBroker.Internal;
using iLabs.ServiceBroker.Administration;
using iLabs.ServiceBroker.Authorization;
using iLabs.ServiceBroker.Authentication;
using iLabs.UtilLib;

namespace iLabs.ServiceBroker.iLabSB
{
	/// <summary>
	/// MyAccount: User account management page.
	/// </summary>
	public partial class myAccount : System.Web.UI.Page
	{
		string supportMailAddress = ConfigurationManager.AppSettings["supportMailAddress"];
		string registrationMailAddress = ConfigurationManager.AppSettings["registrationMailAddress"];
		
		protected void Page_Load(object sender, System.EventArgs e)
		{
			AuthorizationWrapperClass wrapper = new AuthorizationWrapperClass();

			if(! IsPostBack)
			{
                LoadAuthorityList();
				//Populate textboxes with User's data
				User sessionUser = new User();
				sessionUser = wrapper.GetUsersWrapper(new int[]{Convert.ToInt32(Session["UserID"])})[0];
				
				txtUsername.Text = sessionUser.userName;
                txtUsername.Enabled = false;
				txtFirstName.Text = sessionUser.firstName;
				txtLastName.Text = sessionUser.lastName;
				txtEmail.Text = sessionUser.email;
				txtNewPassword.Text = "";
				txtConfirmPassword.Text = "";
                ddlAuthorities.SelectedValue = sessionUser.authID.ToString();
                ddlAuthorities.Enabled = false;

				// To list all the groups a user belongs to
				int userID = Convert.ToInt32(Session["UserID"]);
				int[] groupIDs = wrapper.ListGroupsForUserWrapper (userID);

				//since we already have the groups a user has access
				// if we use wrapper here, it will deny authentication
				Group[] gps = AdministrativeAPI.GetGroups(groupIDs);
				ArrayList nonRequestGroups = new ArrayList();
				ArrayList requestGroups = new ArrayList();

				foreach(Group g in gps)
				{
					if (g.groupName.EndsWith("request"))
						requestGroups.Add(g);
					else 
						if(!g.groupName.Equals("NewUserGroup"))
						nonRequestGroups.Add(g);	
				}

				//List Groups that user belongs to in blue box
				if ((nonRequestGroups!=null)&& (nonRequestGroups.Count>0))
				{
					for (int i=0;i<nonRequestGroups.Count;i++)
					{
						lblGroups.Text+= ((Group)nonRequestGroups[i]).groupName;
						if (i != nonRequestGroups.Count-1)
							lblGroups.Text +=", ";
					}
				}
				else
				{
					lblGroups.Text = "No group";
				}

				//List Groups that user has requested to in blue box
				if ((requestGroups!=null)&& (requestGroups.Count>0))
				{
					for (int i=0;i<requestGroups.Count;i++)
					{
						int origGroupID = AdministrativeAPI.GetAssociatedGroupID(((Group)requestGroups[i]).groupID);
						string origGroupName = AdministrativeAPI.GetGroups(new int[] {origGroupID})[0].groupName;
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

        private void LoadAuthorityList()
        {
            BrokerDB brokerDB = new BrokerDB();
            ddlAuthorities.Items.Clear();
            ListItem liHeaderAdminGroup = new ListItem("--- Select Authority ---", "-1");
            ddlAuthorities.Items.Add(liHeaderAdminGroup);
            IntTag[] authTags = brokerDB.GetAuthorityTags();
            if (authTags != null && authTags.Length > 0)
            {
                foreach (IntTag t in authTags)
                {
                    ListItem li = new ListItem(t.tag, t.id.ToString());
                    ddlAuthorities.Items.Add(li);
                }
            }
        }
		protected void btnSaveChanges_Click(object sender, System.EventArgs e)
		{
            BrokerDB brokerDB = new BrokerDB();
            
			AuthorizationWrapperClass wrapper = new AuthorizationWrapperClass();

			if(txtNewPassword.Text.CompareTo(txtConfirmPassword.Text) != 0 )
			{
				lblResponse.Text = Utilities.FormatErrorMessage("Password fields don't match. Try again!");
				lblResponse.Visible = true;
				txtNewPassword.Text = null;
				txtConfirmPassword.Text = null;
			}
			else
			{
				//if a field is left blank, it is not updated
				try
				{
					User userInfo = wrapper.GetUsersWrapper(new int[] {Convert.ToInt32(Session["UserID"])})[0];
                    Authority auth = brokerDB.AuthorityRetrieve(userInfo.authID);	
					if (txtUsername.Text.Trim()=="")
					{
						txtUsername.Text = userInfo.userName;
					}
					if(txtFirstName.Text.Trim() == "")
					{
						txtFirstName.Text = userInfo.firstName ;
					}
					if(txtLastName.Text.Trim() == "")
					{
						txtLastName.Text = userInfo.lastName ;
					}
					if(txtEmail.Text.Trim() == "")
					{
						txtEmail.Text = userInfo.email ;
					}

					if (userInfo.reason==null)
						userInfo.reason = "";
					if (userInfo.affiliation==null)
						userInfo.affiliation="";
					if (userInfo.xmlExtension==null)
						userInfo.xmlExtension="";

					wrapper.ModifyUserWrapper (userInfo.userID,txtUsername.Text,auth.authorityID,auth.authTypeID, txtFirstName.Text , txtLastName.Text , txtEmail.Text ,userInfo.affiliation, userInfo.reason, userInfo.xmlExtension,userInfo.lockAccount );
					lblResponse.Text = Utilities.FormatConfirmationMessage("User \"" + txtUsername.Text  + "\" information has been updated.");
					lblResponse.Visible = true;
                    if (auth.authTypeID == (int) AuthenticationType.AuthTypeID.Native)
                    {
                        if (txtNewPassword.Text != "")
                        {
                            wrapper.SetNativePasswordWrapper(Convert.ToInt32(Session["UserID"]), txtNewPassword.Text);
                        }
                    }
					if (txtUsername.Text.CompareTo(Session["UserName"].ToString())!= 0)
						Session["UserName"]= txtUsername.Text;

					// Send a confirmation message to the user
					string email;
					if(txtEmail.Text.Trim() == "")
					{
						// use old email if it wasn't changed, new if it was
						email = userInfo.email;
					}
					else
					{
						email = txtEmail.Text.Trim();
					}
                    if (email != null && email.Length > 0)
                    {
                        MailMessage mail = new MailMessage();
                        mail.From = registrationMailAddress;
                        mail.To = email;
                        mail.Subject = "[iLabs] Service Broker Account Update Confirmation";
                        mail.Body = "Your Service Broker account has been updated to the following:\n\r";
                        mail.Body += "-------------------------------------------------------------\n\r\n\r";
                        mail.Body += "User Name: " + txtUsername.Text + "\n\r";
                        mail.Body += "First Name: " + txtFirstName.Text + "\n\r";
                        mail.Body += "Last Name: " + txtLastName.Text + "\n\r";
                        mail.Body += "Email: " + txtEmail.Text + "\n\r\n\r";
                        mail.Body += "For security reasons, your password has not been included in this message." + "\n\r";

                        SmtpMail.SmtpServer = "127.0.0.1";
                        try
                        {
                            SmtpMail.Send(mail);
                        }
                        catch
                        {
                            // if the confirmation message fails, c'est la vie...
                        }
                    }
				}
				catch (Exception ex)
				{
					string msg = "Error updating account ("+ex.Message+". "+ex.GetBaseException()+"). Contact " + supportMailAddress + ".";
					lblResponse.Text = Utilities.FormatErrorMessage(msg);
					lblResponse.Visible = true;
				}
			}
		}
	}
}
