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
using System.Data.Common;
using System.Drawing;
using System.Globalization;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Security;

using iLabs.Core;
using iLabs.DataTypes;
using iLabs.DataTypes.ProcessAgentTypes;
using iLabs.DataTypes.TicketingTypes;
using iLabs.ServiceBroker;
using iLabs.ServiceBroker.Internal;
using iLabs.ServiceBroker.Administration;
using iLabs.ServiceBroker.Authentication;
using iLabs.ServiceBroker.Authorization;
using iLabs.Ticketing;
//using iLabs.Services;
using iLabs.DataTypes;
using iLabs.DataTypes.TicketingTypes;
using iLabs.UtilLib;

namespace iLabs.ServiceBroker.admin
{
	/// <summary>
	/// Summary description for messages.
	/// </summary>
	public partial class messages : System.Web.UI.Page
	{

		AuthorizationWrapperClass wrapper = new AuthorizationWrapperClass();
        CultureInfo culture;
        int userTZ;
        string zero = "0";
		
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (Session["UserID"]==null)
				Response.Redirect("../login.aspx");

			btnRemove.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to remove this message?')== false) return false;");
            userTZ = Convert.ToInt32(Session["UserTZ"]);
            culture = DateUtil.ParseCulture(Request.Headers["Accept-Language"]);
			if (!Page.IsPostBack)
			{
                lblDateFormat.Text = culture.DateTimeFormat.ShortDatePattern;
                lblTzOff.Text = "&nbsp;&nbsp;UTC:&nbsp;&nbsp;" + userTZ / 60.0;
				if (!Session["GroupName"].ToString().Equals(Group.SUPERUSER))
				{
					try
					{
						ddlMessageTarget.Items .Clear ();
						ddlMessageTarget.Items .Add(new ListItem("--Select one--",zero));
						int[] groupIDs = wrapper.ListGroupIDsWrapper();
						Group[] groups=wrapper.GetGroupsWrapper(groupIDs);
						foreach(Group gr in groups)
						{
							//if(!gr.groupName.EndsWith ("request"))
                            if ((gr.groupID > 0) && gr.GroupType.Equals(GroupType.REGULAR) && (!gr.groupName.Equals(Group.ROOT))
                                && (!gr.groupName.Equals(Group.SUPERUSER)) && (!gr.groupName.Equals(Group.NEWUSERGROUP))
                                && (!gr.groupName.Equals(Group.ORPHANEDGROUP)))
                            {
                                ddlMessageTarget.Items.Add(new ListItem(gr.groupName, gr.groupID.ToString()));
                            }
						}
					}
					catch(Exception ex)
					{
						string msg = "Exception: Cannot list groups. "+ex.Message+". "+ex.GetBaseException()+".";
                        lblResponse.Text = Utilities.FormatErrorMessage(msg);
						lblResponse.Visible = true; 
			
					}
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
        private void ResetState()
        {
           txtLastModified.Text ="";
           txtMessageBody.Text = "";
           txtMessageID.Text = "";
           txtMessageTitle.Text = "";
           txtTargetGroups.Text = "";
           txtTargetLabs.Text = "";
           lbxSelectMessage.Items.Clear();
            

        }

			
		private void BuildMsgListBox(string msgType, int groupID, int clientID, int agentID)
		{
			lbxSelectMessage.ClearSelection();

			try
			{
				SystemMessage[] msg=InternalAdminDB.SelectAdminSystemMessages(msgType, groupID, clientID, agentID);
				lbxSelectMessage.Items.Clear ();

				for (int i =0;i <msg.Length ; i++)
				{
					lbxSelectMessage.Items .Add(new ListItem(msg[i].messageTitle + " / (" + DateUtil.ToUserTime(msg[i].lastModified,culture,userTZ) +")", msg[i].messageID.ToString () ));
				}
			}
			catch(Exception ex)
			{
				string msg = "Exception: Cannot retrieve system messages. "+ex.Message+". "+ex.GetBaseException()+".";
				lblResponse.Text = "<div class=errormessage><p>E</p></div>";
				lblResponse.Visible = true;
			}
		}

		private void BuildMsgListBox(string radioButtonValue)
		{
			switch (radioButtonValue)
			{
				case "system":
					BuildMsgListBox(SystemMessage.SYSTEM,0,0,0);
					break;
				case "group":
					if(ddlMessageTarget.SelectedIndex==0)
					{
						lbxSelectMessage.Items .Clear ();
                        lblResponse.Text = Utilities.FormatErrorMessage("Specify the group the message is for.");
						lblResponse.Visible = true;
					}
					else
					{
						BuildMsgListBox(SystemMessage.GROUP,Convert.ToInt32(ddlMessageTarget.SelectedItem.Value),0,0);
					}
					break;
				case"lab":
				
					if(ddlMessageTarget.SelectedIndex==0)
					{
						lbxSelectMessage.Items .Clear ();
                        lblResponse.Text = Utilities.FormatErrorMessage("Specify the lab server the message is for.");
						lblResponse.Visible = true;
					}
					else
					{
						BuildMsgListBox(SystemMessage.LAB,0,0,Convert.ToInt32(ddlMessageTarget.SelectedItem.Value));
					}
					break;			
			}
		}

		protected void rbtnSelectType_SelectedIndexChanged(object sender, System.EventArgs e)
		{
            ResetState();
			//lbxSelectMessage.Items.Clear();
			switch (rbtnSelectType.SelectedValue)
			{
				case "group":
					try
					{
						ddlMessageTarget.Items .Clear ();
						ddlMessageTarget.Items .Add(new ListItem("--Select one--",zero));
						int[] groupIDs = wrapper.ListGroupIDsWrapper();
						Group[] groups=wrapper.GetGroupsWrapper(groupIDs);
						foreach(Group gr in groups)
						{
							//if(!gr.groupName.EndsWith ("request"))
                            if ((gr.groupID > 0) && gr.GroupType.Equals(GroupType.REGULAR) && (!gr.groupName.Equals(Group.ROOT)) && (!gr.groupName.Equals(Group.SUPERUSER)) && (!gr.groupName.Equals(Group.NEWUSERGROUP)) && (!gr.groupName.Equals(Group.ORPHANEDGROUP)))
								ddlMessageTarget.Items .Add(new ListItem(gr.groupName,gr.groupID.ToString()));
						}
					}
					catch(Exception ex)
					{
						string msg = "Exception: Cannot list groups. "+ex.Message+". "+ex.GetBaseException()+".";
                        lblResponse.Text = Utilities.FormatErrorMessage(msg);
						lblResponse.Visible = true; 
					}
					break;
				case "lab":
					try
					{
                        BrokerDB brokerDB = new BrokerDB();
						ddlMessageTarget.Items .Clear ();
						ddlMessageTarget.Items .Add(new ListItem("--Select one--",zero));
						int[] labServerIDs = wrapper.ListLabServerIDsWrapper();
                        DbParameter param = FactoryDB.CreateParameter("@typeMask", ProcessAgentType.AgentType.LAB_SERVER,DbType.Int32);
                        IntTag[] labServers = brokerDB.GetIntTags("GetProcessAgentTagsByTypeMask", param);
						foreach(IntTag ls in labServers)
						{
							if (ls.id > 0)
								ddlMessageTarget.Items .Add(new ListItem(ls.tag,ls.id.ToString()));
						}
					}
					catch(Exception ex)
					{
						string msg = "Exception: Cannot list lab servers. "+ex.Message+"."+ex.GetBaseException()+".";
                        lblResponse.Text = Utilities.FormatErrorMessage(msg);
						lblResponse.Visible = true; 
					}
					break;
				case "system":
				{
					ddlMessageTarget.Items .Clear ();
					ddlMessageTarget.Items .Add("System");
					ddlMessageTarget.Items[0].Selected=true;
					BuildMsgListBox(SystemMessage.SYSTEM, 0, 0, 0);
				}
					break;
			}
		}

		protected void lbxSelectMessage_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if(lbxSelectMessage.SelectedIndex > -1)
			{
				try
				{
					SystemMessage msg = InternalAdminDB.SelectAdminSystemMessages(new int[] {Int32.Parse (lbxSelectMessage.Items [lbxSelectMessage.SelectedIndex].Value)})[0];
					txtMessageBody.Text = msg.messageBody ;
					txtLastModified.Text = DateUtil.ToUserTime(msg.lastModified,culture,userTZ);
					txtMessageID.Text = msg.messageID .ToString ();
					txtMessageTitle.Text = msg.messageTitle ;
					cbxDisplayMessage.Checked = msg.toBeDisplayed ;
						
				}
				catch(Exception ex)
				{
					string msg = "Exception: Cannot retrieve system message. "+ex.Message+". "+ex.GetBaseException()+".";
                    lblResponse.Text = Utilities.FormatErrorMessage(msg);
					lblResponse.Visible = true;
				}
			}
		}

		protected void btnNew_Click(object sender, System.EventArgs e)
		{
			cbxDisplayMessage.Checked = false;
			
			// clear all text boxes
			lbxSelectMessage.Items .Clear ();
			txtTargetGroups.Text = "";
			txtTargetLabs.Text="";
			txtMessageID.Text = "";
			txtMessageTitle.Text = "";
			txtLastModified.Text = "";
			txtMessageBody.Text = "";
			
			// fill dropdown box
			if (Session["GroupName"].ToString().Equals(Group.SUPERUSER))
			{
				ddlMessageTarget.Items .Clear ();
				rbtnSelectType.ClearSelection();
			}
			else
			{
				ddlMessageTarget.SelectedIndex=0;
			}
		}

		protected void btnRemove_Click(object sender, System.EventArgs e)
		{
			if(Convert.ToInt32(txtMessageID.Text) <= 0)
			{
				lblResponse.Text = "<div class=errormessage><p>Select the message to delete</p></div>";
				lblResponse.Visible = true;
			}
			else
			{
				try
				{
					//delete message
					wrapper.RemoveSystemMessagesWrapper (new int[] {Int32.Parse (txtMessageID.Text )});

					//display confirmation message
					string msg = "Message '"+txtMessageTitle.Text+"' has been deleted.";
                    lblResponse.Text = Utilities.FormatConfirmationMessage(msg);
					lblResponse.Visible = true;

					//reset fields
					txtMessageID.Text = "";
					txtMessageTitle.Text = "";
					txtLastModified.Text = "";
					txtMessageBody.Text = "";
					cbxDisplayMessage.Checked = false;

					//Build list box according to the radio button selection. 
					BuildMsgListBox(rbtnSelectType.SelectedValue);
					
				}
				catch (Exception ex)
				{
					string msg = "Exception: Cannot delete message. "+ex.Message+". "+ex.GetBaseException()+".";
                    lblResponse.Text = Utilities.FormatErrorMessage(msg);
					lblResponse.Visible = true;
				}
			}
		}

		protected void btnGo_Click(object sender, System.EventArgs e)
		{
			//reset fields
			txtTargetGroups.Text = "";
			txtTargetLabs.Text="";
			txtMessageID.Text = "";
			txtMessageTitle.Text = "";
			txtLastModified.Text = "";
			txtMessageBody.Text = "";
			cbxDisplayMessage.Checked = false;

			if (!Session["GroupName"].ToString().Equals(Group.SUPERUSER))
			{

				// select group
				if(ddlMessageTarget.SelectedIndex!=0)
				{
					BuildMsgListBox("Group", Convert.ToInt32(ddlMessageTarget.SelectedValue),0,0);
					txtTargetGroups.Text=ddlMessageTarget.SelectedItem.Text;	
					ProcessAgentInfo[] labServers=wrapper.GetProcessAgentInfosWrapper(AdministrativeUtilities.GetGroupLabServers(Int32.Parse(ddlMessageTarget.SelectedValue)));
					foreach (ProcessAgentInfo ls in labServers)
                        if(!ls.retired)
						    txtTargetLabs.Text+=ls.agentName+"\n";
				}
			}
			else
			{
				//check if radio button is set to either system, group or lab
				if(rbtnSelectType.SelectedIndex<0)
				{
                    lblResponse.Text = Utilities.FormatErrorMessage("Specify if new message is a 'Group' ,'Lab'or 'System' one.");
					lblResponse.Visible = true;
					return;
				}
				else
				{
					//display lists according to radio button selection
					BuildMsgListBox(rbtnSelectType.SelectedValue);

					//populate text boxes
					switch(rbtnSelectType.SelectedValue)
					{
					
						case "group":
							if(ddlMessageTarget.SelectedIndex!=0)
							{
								txtTargetGroups.Text=ddlMessageTarget.SelectedItem.Text;	
								ProcessAgentInfo[] labServers=wrapper.GetProcessAgentInfosWrapper(AdministrativeUtilities.GetGroupLabServers(Int32.Parse(ddlMessageTarget.SelectedValue)));
								foreach (ProcessAgentInfo ls in labServers)
                                    if(!ls.retired)
									    txtTargetLabs.Text+=ls.agentName+"\n";
							}
							break;

						case"lab":
							if(ddlMessageTarget.SelectedIndex!=0)
							{
								txtTargetLabs.Text=ddlMessageTarget.SelectedItem.Text;	
								int[] groupIDs = AdministrativeUtilities.GetLabServerGroups(Int32.Parse(ddlMessageTarget.SelectedValue));
								Group[] targetGroups=wrapper.GetGroupsWrapper(groupIDs);
								foreach (Group g in targetGroups)
									txtTargetGroups.Text+= g.groupName+"\n";
							}
							break;			
					}
				}
			}
		}

		protected void btnSaveChanges_Click(object sender, System.EventArgs e)
		{
			//create message struct & populate with form values
			SystemMessage msg = new SystemMessage ();

			// if not superuser
			if (!Session["GroupName"].ToString().Equals(Group.SUPERUSER))
			{
				rbtnSelectType.SelectedValue="group";
				rbtnSelectType.SelectedIndex=0;
			}
			
			// if radio button not selected throw error
			if(rbtnSelectType.SelectedIndex<0)
			{
                lblResponse.Text = Utilities.FormatErrorMessage("Specify if new message is a 'Group' ,'Lab'or 'System' one.");
				lblResponse.Visible = true;
				return;
			}
			else
			{
				switch (rbtnSelectType.SelectedValue)
				{
					case "system":
					{
						msg.messageType = SystemMessage.SYSTEM;
						msg.groupID = 0;
						msg.clientID=0;
                        msg.agentID = 0;
					}
						break;

					case "group":
					{
						if(ddlMessageTarget.SelectedIndex<1)
						{
                            lblResponse.Text = Utilities.FormatErrorMessage("Specify the target group of the new message");
							lblResponse.Visible = true;
							return;
						}
						else
						{
							msg.messageType =SystemMessage.GROUP;
							msg.groupID = Int32.Parse(ddlMessageTarget.SelectedValue);
						}
					}
						break;

					case "lab":
					{
						if(ddlMessageTarget.SelectedIndex<1)
						{
                            lblResponse.Text = Utilities.FormatErrorMessage("Specify the target lab server of the new message.");
							lblResponse.Visible = true;
							return;
						}
						else
						{
							msg.messageType =SystemMessage.LAB;
							msg.agentID=Int32.Parse(ddlMessageTarget.SelectedValue);
						}
					}
						break;
				}
			}
			msg.lastModified = DateTime.UtcNow;
			msg.messageTitle = txtMessageTitle.Text ;
			msg.messageBody = txtMessageBody.Text ;
			msg.toBeDisplayed = cbxDisplayMessage.Checked ;

			if((txtMessageID.Text=="")&&(lbxSelectMessage.SelectedIndex < 0)) // new record
			{
				try
				{
                    msg.messageID = wrapper.AddSystemMessageWrapper(msg.messageType, msg.toBeDisplayed, msg.groupID, msg.clientID, msg.agentID, msg.messageTitle, msg.messageBody);

					BuildMsgListBox(rbtnSelectType.SelectedValue);
					
					txtMessageID.Text = msg.messageID.ToString();
					txtLastModified.Text = DateUtil.ToUserTime(msg.lastModified,culture,userTZ);
					string cmsg = "Message '"+msg.messageTitle+"' has been created.";
                    lblResponse.Text = Utilities.FormatConfirmationMessage(cmsg);
					lblResponse.Visible = true;
				}
				catch(Exception ex)
				{
					string emsg = "Exception: Cannot create message '"+txtMessageTitle.Text+"'. "+ex.Message+". "+ex.GetBaseException()+".";
                    lblResponse.Text = Utilities.FormatErrorMessage(emsg);
					lblResponse.Visible = true;
					return;											
				}
			}
			else 
			{
				msg.messageID = Int32.Parse (txtMessageID.Text) ;
				try 
				{
					wrapper.ModifySystemMessageWrapper(msg.messageID, msg.messageType, msg.toBeDisplayed, msg.groupID, msg.clientID, msg.agentID, msg.messageBody, msg.messageTitle);
                    txtLastModified.Text = DateUtil.ToUserTime(msg.lastModified, culture, userTZ); ;

					//update record in list box
					//this crashes when updating something that hasn't been selected
					//lbxSelectMessage.Items [lbxSelectMessage.SelectedIndex].Text = msg.messageTitle + " / (" + msg.LastModified.ToString () +")";

					BuildMsgListBox(rbtnSelectType.SelectedValue);

					string cmsg = "Message '"+msg.messageTitle+"' has been updated.";
					lblResponse.Text = Utilities.FormatConfirmationMessage(cmsg);
					lblResponse.Visible = true;
				}
				catch (Exception ex)
				{
					string emsg = "Exception: Cannot update message '"+txtMessageTitle.Text+"'. "+ex.Message+". "+ex.GetBaseException()+".";
					lblResponse.Text = Utilities.FormatErrorMessage(emsg);
					lblResponse.Visible = true;									
				}		
			}
		}

		protected void previewMessage_Click(object sender, System.EventArgs e)
		{
			string title = txtMessageTitle.Text.Replace("'", "");
			string msg = txtMessageBody.Text.Replace("'", "");
			
			string script = @"<script language='javascript'>window.open('messagePreviewPopup.aspx?msg="+ Server.UrlEncode(msg)+"&amp;title="+ Server.UrlEncode(txtMessageTitle.Text) +
                   "&amp;date=" + Server.UrlEncode(txtLastModified.Text) + "','_blank','scrollbars=yes,resizable=yes,width=450,height=300')</script>";
            Page.RegisterStartupScript("MessagePreview",script);
		}
	}
	
}