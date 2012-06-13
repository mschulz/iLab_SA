/*
 * Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
 * Please see license.txt in top level directory for full license.
 * 
 * $Id$
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web .Security ;
using System.Text.RegularExpressions;

using iLabs.DataTypes;
using iLabs.ServiceBroker;
using iLabs.ServiceBroker.Administration;
using iLabs.ServiceBroker.Internal;
using iLabs.ServiceBroker.Authentication;
using iLabs.ServiceBroker.Authorization;
using iLabs.UtilLib;


namespace iLabs.ServiceBroker.admin
{
	/// <summary>
	/// Summary description for manageUser.
	/// </summary>
	public partial class manageUser : System.Web.UI.Page
	{
		AuthorizationWrapperClass wrapper = new AuthorizationWrapperClass();
        BrokerDB brokerDB = new BrokerDB();
        int defaultGroupID;
        int userGroupID = 0;

		protected void Page_Load(object sender, System.EventArgs e)
		{
            Object o = Events;
            defaultGroupID = wrapper.GetGroupIDWrapper(ServiceBroker.Administration.Group.NEWUSERGROUP);
			if (Session["UserID"]==null)
				Response.Redirect("../login.aspx");
			
			btnSaveChanges.CssClass="button";
			btnSaveChanges.Enabled=true;
			
			btnRemove.Attributes.Add("onclick", "javascript:if(confirm('This will remove the user from the groups and subgroups it belongs to. Are you sure you want to remove this user?')== false) return false;");
            if (Convert.ToBoolean(Session["IsAdmin"]))
            {
                if (Session["GroupName"].ToString().Contains("-admin"))
                {
                    userGroupID = AdministrativeAPI.GetAssociatedGroupID(Convert.ToInt32(Session["GroupID"]));
                }
            }
            else{
				txtPassword.Enabled = false;
				txtConfirmPassword.Enabled = false;
				txtPassword.BackColor = Color.Silver;
				txtConfirmPassword.BackColor = Color.Silver;
			}

			if(!IsPostBack)
			{
				/* Check the Web.Config for the Affilitation setting. 
				 * Affiliation can either be set up as a drop down list or a text box.
				 * This is specified in the Web.Config file
				 */
				if(ConfigurationManager.AppSettings["useAffiliationDDL"].Equals("true"))
				{
					String afList = ConfigurationManager.AppSettings["affiliationOptions"];
					char [] delimiter = {','};
					String [] options =afList.Split(delimiter,100);
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
                LoadAuthorityList();
				BuildUserListBox();
                ResetState();
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
            ddlAuthority.Items.Clear();
            //ListItem liHeaderAdminGroup = new ListItem("--- Select Authority ---", "-1");
            //ddlAuthorities.Items.Add(liHeaderAdminGroup);
           
            IntTag[] authTags = brokerDB.GetAuthorityTags();
            if (authTags != null && authTags.Length > 0)
            {
                foreach (IntTag t in authTags)
                {
                    ListItem li = new ListItem(t.tag, t.id.ToString());
                    ddlAuthority.Items.Add(li);
                }
            }
        }

		/* 
		 * Builds the Select Users List box. 
		 * By default, the box gets filled with all the users in the database
		 */
		private void BuildUserListBox()
		{
			try
			{
                int[] userIDs = null;
                if (userGroupID == 0)
                    userIDs = wrapper.ListUserIDsWrapper();
                else
                    userIDs = AdministrativeAPI.ListUserIDsInGroupRecursively(userGroupID);

				List<User> users = new List<User>(wrapper.GetUsersWrapper(userIDs));
                BuildUserListBox(users);
			}
			catch(Exception ex)
			{
				string msg = "Exception: Cannot list userNames. "+ex.Message+". "+ex.GetBaseException();
				lblResponse.Text = Utilities.FormatErrorMessage(msg);
				lblResponse.Visible = true;
			}
		}

		/* 
		 * Builds the Select Users List using a specified array of users. 
		 * This is used to return the results of a search
		 */
		private void BuildUserListBox(List<User> users)
		{
			lbxSelectUser.Items.Clear();
            users.Sort();
			foreach (User user in users) 
			{
				ListItem userItem = new ListItem();
                userItem.Text = String.Format("{0,-20} --  {1}, {2}", user.userName, user.lastName, user.firstName);
				//userItem.Text = user.lastName+", "+user.firstName+" - "+user.userName;
				userItem.Value = user.userID.ToString();
				lbxSelectUser.Items .Add(userItem);
			}
		}

		private void ResetState()
		{
			if(ConfigurationManager.AppSettings["useAffiliationDDL"].Equals("true"))
			{
				ddlAffiliation.ClearSelection ();
			}
			else
			{
				txtAffiliation.Text = "";
			}
			txtEmail.Text = "";
			txtLastName.Text = "";
			txtFirstName.Text = "";
			txtUsername.Text = "";
			txtUsername.Enabled=true;
            ddlAuthority.SelectedValue = "0";
            ddlAuthority.Enabled = true;
			lblGroups.Text="";
			lblRequestGroups.Text="";
			cbxLockAccount.Checked = false;
			btnSaveChanges.CssClass="button";
			btnSaveChanges.Enabled = true;
            lblResponse.Text = "";
            lblResponse.Visible = false; 
		}

		private void DisplayUserInfo(User user)
		{
			ResetState();
            hdnUserId.Value = user.userID.ToString();
			txtUsername.Text = user.userName;
			txtUsername.Enabled = false;
			txtFirstName.Text = user.firstName;
			txtLastName.Text = user.lastName;
			txtEmail.Text = user.email;
            ddlAuthority.SelectedValue = user.authID.ToString();
            ddlAuthority.Enabled = false;

			/* Note if you change your drop down list options after launching the SB, */
			if(ConfigurationManager.AppSettings["useAffiliationDDL"].Equals("true"))
			{
                ListItem liAffil = ddlAffiliation.Items.FindByText(user.affiliation);
                if (liAffil == null)
                {
                    ddlAffiliation.Items.Add(user.affiliation);
                }
				ddlAffiliation.Items.FindByText (user.affiliation).Selected = true;
			}
			else
			{
				txtAffiliation.Text = user.affiliation;
			}

			cbxLockAccount.Checked = user.lockAccount ;

			try
			{
				//Get explicit groups the user belongs to
				ArrayList nonRequestGroups = new ArrayList();
				ArrayList requestGroups = new ArrayList();
				int[] gpIDs = wrapper.ListGroupsForUserWrapper (user.userID );
                //int[] gpIDs = wrapper.ListGroupsForUserWrapper(user.userID);
				ServiceBroker.Administration.Group[] gps=wrapper.GetGroupsWrapper(gpIDs);
				foreach(ServiceBroker.Administration.Group g in gps)
				{
					if (g.GroupType.CompareTo(GroupType.REQUEST) == 0)
						requestGroups.Add(g);
					else 
						if(!g.groupName.Equals("NewUserGroup"))
						nonRequestGroups.Add(g);	
				}

				string groupsDisplay = "'"+user.userName + "'"+" is a member of the following groups:" + "<p>";
				if ((nonRequestGroups!=null)&& (nonRequestGroups.Count>0))
				{
					foreach (ServiceBroker.Administration.Group g in nonRequestGroups)
					{
						groupsDisplay += " &nbsp;&nbsp;-&nbsp;&nbsp;"+ g.groupName+ "<br>";
					}
				}
				else
				{
					groupsDisplay += " &nbsp;&nbsp;-&nbsp;&nbsp;no group <br>";
				}
					
				lblGroups.Text = groupsDisplay;

				string requestGroupsDisplay = "'"+user.userName + "'"+" has requested membership in the following groups:" + "<p>";
				if ((requestGroups!=null)&& (requestGroups.Count>0))
				{
					foreach (Administration.Group g in requestGroups)
					{
						int origGroupID = AdministrativeAPI.GetAssociatedGroupID(g.groupID);
						string origGroupName = AdministrativeAPI.GetGroups(new int[] {origGroupID})[0].groupName;
						requestGroupsDisplay += " &nbsp;&nbsp;-&nbsp;&nbsp;"+ origGroupName+ "<br>";
					}
				}
				else
				{
					requestGroupsDisplay += " &nbsp;&nbsp;-&nbsp;&nbsp;no group <br>";
				}
				lblRequestGroups.Text=requestGroupsDisplay;

				if (!Session["GroupName"].ToString().Equals(ServiceBroker.Administration.Group.SUPERUSER))
				{
					btnSaveChanges.Enabled=false;
					btnSaveChanges.CssClass="buttongray";
				}
			}

			catch (Exception ex)
			{
				string msg = "Exception: Trouble accessing page. "+ex.Message+". "+ex.GetBaseException();
				lblResponse.Text = Utilities.FormatErrorMessage(msg);
				lblResponse.Visible = true;
			}
		}

		protected void lbxSelectUser_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if(lbxSelectUser.SelectedIndex < 0)
			{
				lblResponse.Text = Utilities.FormatErrorMessage("You must select a user!");
				lblResponse.Visible = true;
			}
			else
			{
				try
				{
					User[] user = wrapper.GetUsersWrapper (new int[] {Int32.Parse(lbxSelectUser.SelectedValue)});
					DisplayUserInfo(user[0]);
				}
				catch(Exception ex)
				{
					string msg = "Exception: Cannot retrieve user's information. " +ex.Message+". "+ex.GetBaseException();
					lblResponse.Text = Utilities.FormatErrorMessage (msg);
					lblResponse.Visible = true;
				}
			}
		}

        protected void btnSearch_Click(object sender, System.EventArgs e)
        {
            Object o = Events;
            lblResponse.Text = "";
            lblResponse.Visible = false;
            //only superusers can view this page
            if (!Session["GroupName"].ToString().Equals(ServiceBroker.Administration.Group.SUPERUSER))
                btnSaveChanges.Enabled = false;
            btnSaveChanges.CssClass = "buttongray";

            // If an option is not selected in the "Search by" drop down list
            if (ddlSearchBy.SelectedIndex < 0)
            {
                lblResponse.Text = Utilities.FormatErrorMessage("Select a search criterion.");
                lblResponse.Visible = true;
            }
            else if (ddlSearchBy.SelectedIndex == 0)
            {
                BuildUserListBox();
            }
            else
            {
                //if blank entry
                if (txtSearchBy.Text == "")
                {
                    lblResponse.Text = Utilities.FormatErrorMessage("Enter the text you want to search for.");
                    lblResponse.Visible = true;
                }
                else
                {
                    List<User> foundUsers = new List<User>();
                    string option = ddlSearchBy.SelectedItem.Text;

                    int[] userIDs = null;
                    User[] users = null;

                    switch (option)
                    {
                        case "Username":
                            {
                                userIDs = wrapper.ListUserIDsWrapper();
                                users = wrapper.GetUsersWrapper(userIDs);
                                foreach (User u in users)
                                {
                                    if (Utilities.WildCardMatch(txtSearchBy.Text, u.userName))
                                        //if (! foundUsers.Contains(u))
                                        foundUsers.Add(u);
                                }
                                if (foundUsers.Count <= 0)
                                {
                                    string msg = "There are no usernames matching " + txtSearchBy.Text + ".";
                                    lblResponse.Text = Utilities.FormatErrorMessage(msg);
                                    lblResponse.Visible = true;
                                }
                            }
                            break;
                        case "First Name":
                            {
                                userIDs = wrapper.ListUserIDsWrapper();
                                users = wrapper.GetUsersWrapper(userIDs);
                                foreach (User u in users)
                                {
                                    if (Utilities.WildCardMatch(txtSearchBy.Text, u.firstName))
                                        foundUsers.Add(u);
                                }
                                if (foundUsers.Count <= 0)
                                {
                                    string msg = "There are no first names that match " + txtSearchBy.Text + ".";
                                    lblResponse.Text = Utilities.FormatErrorMessage(msg);
                                    lblResponse.Visible = true;
                                }
                            }
                            break;
                        case "Last Name":
                            {
                                userIDs = wrapper.ListUserIDsWrapper();
                                users = wrapper.GetUsersWrapper(userIDs);
                                foreach (User u in users)
                                {
                                    if (Utilities.WildCardMatch(txtSearchBy.Text, u.lastName))
                                        foundUsers.Add(u);
                                }
                                if (foundUsers.Count <= 0)
                                {
                                    string msg = "There are no last names that match " + txtSearchBy.Text + ".";
                                    lblResponse.Text = Utilities.FormatErrorMessage(msg);
                                    lblResponse.Visible = true;
                                }
                            }
                            break;
                        case "Group":
                            {

                                //Get a list of the groups from the database
                                int[] groupIDs = wrapper.ListGroupIDsWrapper();
                                ServiceBroker.Administration.Group[] groups = wrapper.GetGroupsWrapper(groupIDs);

                                //Find the relevant groups using the wild card search
                                ArrayList foundGroups = new ArrayList();
                                foreach (ServiceBroker.Administration.Group g in groups)
                                {
                                    if (Utilities.WildCardMatch(txtSearchBy.Text, g.groupName))
                                        foundGroups.Add(g);
                                }

                                //if the group exists in the database
                                List<int> foundUserIDs = new List<int>();

                                if (foundGroups.Count > 0)
                                {
                                    foreach (ServiceBroker.Administration.Group foundg in foundGroups)
                                    {
                                        //Get the list of users in the group
                                        int[] userIDsForGroup = wrapper.ListUserIDsInGroupRecursivelyWrapper(foundg.groupID);

                                        //Put this in the foundUserID ArrayList
                                        foreach (int userID in userIDsForGroup)
                                        {
                                            if (!foundUserIDs.Contains(userID))
                                                foundUserIDs.Add(userID);
                                        }
                                    }

                                    //if the group contains users
                                    if (foundUserIDs.Count > 0)
                                    {
                                        foundUsers.AddRange(wrapper.GetUsersWrapper(foundUserIDs.ToArray()));

                                    }
                                    else //no users exist in the group
                                    {
                                        string msg = "No users exist in groups matching " + txtSearchBy.Text + ".";
                                        lblResponse.Text = Utilities.FormatErrorMessage(msg);
                                        lblResponse.Visible = true;
                                    }
                                }
                                else //groupID < 0, group doesn't exist
                                {
                                    string msg = "No group names matching " + txtSearchBy.Text + " exist.";
                                    lblResponse.Text = Utilities.FormatErrorMessage(msg);
                                    lblResponse.Visible = true;
                                }
                            }
                            break;
                        default:
                            break;
                    } //end switch

                    if (foundUsers.Count > 0)
                    {
                        // if only one record found
                        if (foundUsers.Count == 1)
                        {
                            /* Need to rebuild the listbox, incase of multiple searches.
                             * The results of a search are displayed in the list box 
                             * & hence if one needs to do a 2nd search, the list has to be rebuilt.
                             */
                            BuildUserListBox();
                            User foundUser = foundUsers[0];
                            lbxSelectUser.Items.FindByValue(foundUser.userID.ToString()).Selected = true;
                            DisplayUserInfo(foundUser);
                        }
                        else
                        {
                            BuildUserListBox(foundUsers);
                            ResetState();
                        }
                    }
                    else // no users found
                    {
                        lbxSelectUser.Items.Clear();
                       // Error messages set in switch block
                    }
                }
            }
        }
	
		protected void btnSaveChanges_Click(object sender, System.EventArgs e)
		{
            Authority auth = brokerDB.AuthorityRetrieve(Convert.ToInt32(ddlAuthority.SelectedValue));
            lblResponse.Text = "";
            lblResponse.Visible = false; 
			//Error checking for empty fields
			if(txtUsername.Text.CompareTo("")==0 )
			{
				lblResponse.Text = Utilities.FormatErrorMessage("Enter a Username.");
				lblResponse.Visible=true;
				return;
			}
            if (auth.authorityID == 0)
            {
                if (txtFirstName.Text.CompareTo("") == 0)
                {
                    lblResponse.Text = Utilities.FormatErrorMessage("You must enter the user's first name.");
                    lblResponse.Visible = true;
                    return;
                }

                if (txtLastName.Text.CompareTo("") == 0)
                {
                    lblResponse.Text = Utilities.FormatErrorMessage("You must enter the user's last name.");
                    lblResponse.Visible = true;
                    return;
                }

                if (txtEmail.Text.CompareTo("") == 0)
                {
                    lblResponse.Text = Utilities.FormatErrorMessage("You must enter the user's email.");
                    lblResponse.Visible = true;
                    return;
                }
            }
            if (Convert.ToBoolean(Session["IsAdmin"]))
            //if (Session["GroupName"].Equals(ServiceBroker.Administration.Group.SUPERUSER))
            {
                if (auth.authTypeID == (int) AuthenticationType.AuthTypeID.Native)
                {
                    //Password checks
                    if (txtPassword.Text.CompareTo("") == 0)
                    {
                        lblResponse.Text = Utilities.FormatErrorMessage("You must enter a password.");
                        lblResponse.Visible = true;
                        return;
                    }

                    if (txtConfirmPassword.Text.CompareTo("") == 0)
                    {
                        lblResponse.Text = Utilities.FormatErrorMessage("Retype the password in the 'Confirm Password' field.");
                        lblResponse.Visible = true;
                        return;
                    }

                    if (txtPassword.Text != txtConfirmPassword.Text)
                    {
                        lblResponse.Text = Utilities.FormatErrorMessage("Password fields do not match. Retype password.");
                        lblResponse.Visible = true;
                        return;
                    }
                }
            }
			String strAffiliation = "";
			if(ConfigurationManager.AppSettings["useAffiliationDDL"].Equals("true"))
			{
				if (ddlAffiliation.SelectedIndex > 0)
				{
					strAffiliation = ddlAffiliation.SelectedItem.Text;
				}
			}
			else
			{
				strAffiliation = txtAffiliation.Text;
			}
			
			if ((strAffiliation==null)||(strAffiliation.CompareTo("")==0))
			{
				lblResponse.Text = Utilities.FormatErrorMessage("Please select an affiliation group.");
				lblResponse.Visible=true;
				return;
			}
		
			//If all the error checks are cleared
			
				//if adding a new user
			if(txtUsername.Enabled)
			{
				try
				{
					// the database will also throw an exception if the agentName exists
					// since username must be unique across both users and groups.
					// this is just another check to throw a meaningful exception
					if (wrapper.GetUserIDWrapper(txtUsername.Text,Convert.ToInt32(ddlAuthority.SelectedValue))>0) // then the user already exists in database
					{
						string msg = "The username '"+txtUsername.Text+"' already exists. Please choose another username.";
						lblResponse.Text = Utilities.FormatErrorMessage(msg);
						lblResponse.Visible=true;
						return;
					}
					else
					{
                     

						//Add User
                        int userID = wrapper.AddUserWrapper(txtUsername.Text, Convert.ToInt32(ddlAuthority.SelectedValue), (int) AuthenticationType.AuthTypeID.Native,
							txtFirstName.Text, txtLastName.Text, txtEmail.Text,strAffiliation, null,
							"", auth.defaultGroupID, cbxLockAccount.Checked);

						//Set Password - Can only change password if you're superuser
						if (Session["GroupName"].Equals(ServiceBroker.Administration.Group.SUPERUSER))
						{
                            if (auth.authTypeID == (int) AuthenticationType.AuthTypeID.Native)
                            {
                                wrapper.SetNativePasswordWrapper(userID, txtPassword.Text);
                            }
						}

						string msg = "The record for "+txtUsername.Text + " has been created successfully.";
						lblResponse.Text= Utilities.FormatConfirmationMessage(msg);
						lblResponse.Visible=true;

						txtUsername.Enabled=false;
						BuildUserListBox();
						//select the recently added user in the list box
						lbxSelectUser.Items.FindByValue(userID.ToString());
					}
				}
				catch (Exception ex)
				{
					string msg = "Exception: Cannot add '"+txtUsername.Text +"'. "+ex.Message +". "+ex.GetBaseException()+".";
					lblResponse.Text= Utilities.FormatErrorMessage(msg);
					lblResponse.Visible=true;
				}
			}
			else // if updating an old user
			{
				try
				{
					//Update user information
                    wrapper.ModifyUserWrapper(Convert.ToInt32(lbxSelectUser.SelectedValue), txtUsername.Text, auth.authorityID, auth.authTypeID, txtFirstName.Text, txtLastName.Text, txtEmail.Text, strAffiliation, "", "", cbxLockAccount.Checked);

					//Update password information only if the old password has not been changed
                    if (auth.authTypeID == (int) AuthenticationType.AuthTypeID.Native)
                    {
                        if (txtPassword.Text.CompareTo("") != 0)
                        {
                            if (txtConfirmPassword.Text.CompareTo("") == 0)
                            {
                                lblResponse.Text = Utilities.FormatErrorMessage("Retype the password in the 'Confirm Password' field.");
                                lblResponse.Visible = true;
                                return;
                            }

                            if (txtPassword.Text != txtConfirmPassword.Text)
                            {
                                lblResponse.Text = Utilities.FormatErrorMessage("Password fields do not match. Retype password.");
                                lblResponse.Visible = true;
                                return;
                            }

                            //Update password
                            wrapper.SetNativePasswordWrapper(Convert.ToInt32(lbxSelectUser.SelectedValue), txtPassword.Text);
                        }
                    }			
					string msg = "The record for '"+txtUsername.Text + "' has been updated.";
					lblResponse.Text= Utilities.FormatConfirmationMessage(msg);
					lblResponse.Visible=true;
				}
				catch(Exception ex)
				{
					string msg = "Exception: Cannot update '"+txtUsername.Text +"'. "+ex.Message +". "+ex.GetBaseException()+".";
					lblResponse.Text= Utilities.FormatErrorMessage(msg);
					lblResponse.Visible=true;
				}
			}
		}

		protected void btnRemove_Click(object sender, System.EventArgs e)
		{
			if (lbxSelectUser.SelectedIndex<0)
			{
				lblResponse.Text = Utilities.FormatErrorMessage("Select a user to be deleted.");
				lblResponse.Visible=true;
				return;
			}
			try
			{
                if (wrapper.RemoveUsersWrapper(new int[] { Convert.ToInt32(lbxSelectUser.SelectedValue) }).Length > 0)
				{
					string msg = "'" + txtUsername.Text + "' was not deleted.";
					lblResponse.Text = Utilities.FormatErrorMessage(msg);
					lblResponse.Visible=true;
				}
				else
				{
					string msg = "'"+txtUsername.Text + "' has been deleted.";
					lblResponse.Text = Utilities.FormatConfirmationMessage(msg);
					lblResponse.Visible=true;
				}
			}
			catch (Exception ex)
			{
				string msg = "Exception: Cannot delete '" + txtUsername.Text + "'. "+ex.Message+". "+ex.GetBaseException();
				lblResponse.Text = Utilities.FormatErrorMessage(msg);
				lblResponse.Visible=true;
			}
			finally
			{
				ResetState();
				txtSearchBy.Text = "";
				BuildUserListBox();
			}
		}

		protected void btnNew_Click(object sender, System.EventArgs e)
		{
			ResetState();
			txtSearchBy.Text = "";
			BuildUserListBox();
		}
        protected void txtSearchBy_TextChanged(object sender, EventArgs e)
        {

        }
}
	
}


	

