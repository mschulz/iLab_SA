/*
 * Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
 * Please see license.txt in top level directory for full license.
 */

/* $Id$ */

using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using iLabs.ServiceBroker;
using iLabs.ServiceBroker.Administration;
using iLabs.ServiceBroker.Authorization;
using iLabs.ServiceBroker.Internal;
using iLabs.UtilLib;

namespace iLabs.ServiceBroker.admin
{
	/// <summary>
	/// Summary description for addEditGroupPopup.
	/// </summary>
	public partial class addEditGroupPopup : System.Web.UI.Page
	{
		Boolean displaySubgroups = false;
        protected System.Web.UI.WebControls.Repeater repSubGroups;
		protected Group [] allGroups;
		int [] parentIDs;
		protected string actionCmd = "Edit";
		protected int status = -1;
		protected GroupInfo groupInfo;
		protected ArrayList expGrants;
		protected Boolean showExpGrants = false;
		protected Boolean showAssocLabClients = false;

        AuthorizationWrapperClass wrapper = new AuthorizationWrapperClass();
		

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (Session["UserID"]==null)
				Response.Redirect("../login.aspx");

            groupInfo = (GroupInfo)Session["GroupInfo"];
			if(groupInfo == null)
			{
				groupInfo = new GroupInfo();
				actionCmd = "Add";
			}
			
			//after clicking button from list groups page.
			if(!Page.IsPostBack )			// populate controls with the initial group information
			{	
				Session.Remove("GroupInfo");
                groupInfo = new GroupInfo();
				if(Request.Params["gid"] != null)
				{
					groupInfo.id = Convert.ToInt32(Request.Params["gid"]);
				}
				lblResponse.Text = "";
				txtRequestgroup.ReadOnly=true;
				txtRequestgroup.Enabled=false;
				txtRequestgroup.Visible=false;
		
				txtTAGroup.ReadOnly=true;
				txtTAGroup.Enabled=false;
				txtTAGroup.Visible=false;

				int[] groupIDs = wrapper.ListGroupIDsWrapper();
				allGroups = wrapper.GetGroupsWrapper(groupIDs);
				
				if(groupInfo.id > 0)
				{
					actionCmd = "Edit";
					int [] tg = new int[1];
					tg[0] = groupInfo.id;
					Group [] curGroup = wrapper.GetGroupsWrapper(tg);
					if(curGroup.Length == 1)
					{
						displayGroup(curGroup[0]);
					}
					else
					{
						lblResponse.Text = "Error finding group to edit: " + groupInfo.id;
						lblResponse.Visible= true;
					}
				}
				else 
				{ // a new group
					
					txtName.ReadOnly= false;
					txtName.Enabled = true;
					// set Default parent
					parentIDs = new int[] {wrapper.GetGroupIDWrapper(Group.ROOT)};
					loadParentLists();
					actionCmd = "Add";
				}
				Session["GroupInfo"] = groupInfo;
			}
		}

		private  void displayGroup(Group gp)
		{
			txtName.Text = gp.GroupName;
			groupInfo.name = gp.GroupName;
		
			groupInfo.id=gp.groupID;
			txtDescription.Text = gp.description ;
			groupInfo.description = gp.description ;
			if(groupInfo.id > 0)
			{
				txtName.ReadOnly = true;
				txtEmail.Text = gp.email;
				groupInfo.email =gp.email;
				
				// Check for a request group
				int request = AdministrativeUtilities.GetGroupRequestGroup(groupInfo.id);
				if(request > 0)
				{
					Group [] requestGroup = wrapper.GetGroupsWrapper(new int [] {request});
					if(requestGroup.Length > 0)
					{
						groupInfo.request = requestGroup[0].groupID;
						cbxRequestgroup.Checked = true;
						txtRequestgroup.Text = requestGroup[0].GroupName.ToString();
						txtRequestgroup.Visible=true;
					}
					else
					{
						cbxRequestgroup.Checked = false;
							
					}
				}

				// Check for a TA group
				int TAGrp = AdministrativeUtilities.GetGroupAdminGroup(groupInfo.id);				if(TAGrp > 0)
				{
					Group [] TAGroup = wrapper.GetGroupsWrapper(new int [] {TAGrp});
					if(TAGroup.Length > 0)
					{
						groupInfo.admin = TAGroup[0].groupID;
						cbxTAGroup.Checked = true;
						txtTAGroup.Text = TAGroup[0].GroupName.ToString();
						txtTAGroup.Visible=true;
					}
					else
					{
						cbxTAGroup.Checked = false;
							
					}
				}
				

				int qualId = AuthorizationAPI.GetQualifierID(groupInfo.id,Qualifier.experimentCollectionQualifierTypeID);
					
				if(qualId >0)
				{
					groupInfo.collectionNode = qualId;
					showExpGrants=true;
					getExpGrants();
					repExperiments.DataSource = expGrants;
					repExperiments.DataBind();
				}
				// find parents
				parentIDs =
					InternalAdminDB.ListGroupParentIDs(gp.GroupID);
				loadParentLists();
			}
			//list lab clients
			int[] clientIDs = AdministrativeUtilities.GetGroupLabClients(gp.groupID);
			if (clientIDs.Length>0)
			{
				LabClient[] clients = wrapper.GetLabClientsWrapper(clientIDs);
				repLabClients.DataSource = clients;
				repLabClients.DataBind();
				showAssocLabClients = true;
			}
		}

		private void getExpGrants()
		{
			// Process grants
			Group [] subGroups;
			expGrants = new ArrayList();
			GrantInfo gInfo = new GrantInfo();
			gInfo.name = txtName.Text;
			gInfo.isEditable = true;
			// Find all grants for this group
			processGrantInfo(groupInfo.id,gInfo);
			expGrants.Add(gInfo);

			// Get any child groups
			//int []subIds = wrapper.ListGroupsForAgentWrapper(groupInfo.id);

			int []subGroupIDs = wrapper.ListSubgroupIDsWrapper(groupInfo.id);

            if (subGroupIDs != null && subGroupIDs.Length > 0)
			{
				subGroups = wrapper.GetGroupsWrapper(subGroupIDs);
				if(displaySubgroups)
				{
					repSubGroups.DataSource = subGroups;
					repSubGroups.DataBind();
				}
					
				foreach(Group sgrp in subGroups)
				{
					gInfo = new GrantInfo();
					gInfo.name = sgrp.GroupName;
					gInfo.id=sgrp.groupID;
					processGrantInfo(sgrp.GroupID,gInfo);
					expGrants.Add(gInfo);
				}
			}
		}
		/// <summary>
		/// Finds all eperiment collection grants derived from the specified qualID,
		/// and loads the values into the GrantInfo.
		/// </summary>
		/// <param name="qualID"></param>
		/// <param name="gInfo"></param>
		private void processGrantInfo(int groupID,GrantInfo gInfo)
		{
			int [] grantIDs = wrapper.FindGrantsWrapper(groupID,null,groupInfo.collectionNode);
			if(grantIDs.Length > 0)
			{			
				Grant [] grants = wrapper.GetGrantsWrapper(grantIDs);		
				foreach(Grant g in grants)
				{
					if(g.function.CompareTo(Function.readExperimentFunctionType) ==0)
					{
						gInfo.isRead = true;
					}
					if(g.function.CompareTo(Function.writeExperimentFunctionType) ==0)
					{
						gInfo.isWrite = true;
					}			 
					if(g.function.CompareTo(Function.createExperimentFunctionType) ==0)
					{
						gInfo.isCreate = true;
					}		
				}
			}
		}
		private void repExperiments_ItemBound(Object sender, RepeaterItemEventArgs e)
		{
			
			if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
			{
				GrantInfo curG = (GrantInfo) e.Item.DataItem;
				Label lbl = (Label) e.Item.FindControl("lblExp");
				lbl.Text = curG.name;
				CheckBox curB = (CheckBox) e.Item.FindControl("cbxRead");
				curB.Checked = curG.isRead;
				if (!(curG.name.Equals(Session["GroupName"].ToString())))
					curB.Enabled=true;
				//curB.Attributes.Add("onClick",script);
				
				curB = (CheckBox) e.Item.FindControl("cbxWrite");
				curB.Checked = curG.isWrite;
				if (!(curG.name.Equals(Session["GroupName"].ToString())))
					curB.Enabled=true;
				//curB.Attributes.Add("onClick",script);

				curB = (CheckBox) e.Item.FindControl("cbxCreate");
				curB.Checked = curG.isCreate;
				if (!(curG.name.Equals(Session["GroupName"].ToString())))
					curB.Enabled=true;
				//curB.Attributes.Add("onClick",script);
			}
		}

		private void resetPage()
		{
			txtName.Text = null;
			txtDescription.Text = null;
			txtName.ReadOnly = false;
			txtName.BackColor = Color.White ;
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
			this.ibtnAddParent.Click += new System.Web.UI.ImageClickEventHandler(this.ibtnAddParent_Click);
			this.ibtnRemoveParent.Click += new System.Web.UI.ImageClickEventHandler(this.ibtnRemoveParent_Click);
			this.btnSaveChanges.Click += new System.EventHandler(this.btnSaveChanges_Click);
			this.repExperiments.ItemDataBound += new RepeaterItemEventHandler(this.repExperiments_ItemBound);
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
		/// Write Available and Associated Group Parents List Boxes
		/// </summary>
		private void loadParentLists()
		{
			bool notAssociated;

			foreach(Group grp in allGroups)
			{
				if((grp.groupID > 0) && (grp.groupID != groupInfo.id))
				{
					notAssociated = true;
				
					// Don't write Lab Servers that are already associated with the client
					// to the Available List box
					foreach(int i in parentIDs)
					{
						if(grp.groupID == i)
						{
							notAssociated = false;
							break;
						}
					}
					if(notAssociated)
					{
						if ((grp.groupID >0) && (!grp.groupType.Equals(GroupType.REQUEST))
							&&(!grp.groupName.Equals(Group.ORPHANEDGROUP))
                            &&(!grp.groupName.Equals(Group.NEWUSERGROUP))
                            &&(!grp.groupName.Equals(Group.SUPERUSER))
                            &&(!grp.groupType.Equals(GroupType.SERVICE_ADMIN)))
							//Write to Available Listbox if not associated
							if ((Session["GroupName"].ToString().Equals(Group.SUPERUSER)))
								lbxAllParents.Items.Add(new ListItem(grp.groupName, grp.GroupID.ToString()));
							else
								if (!(grp.groupName.Equals(Group.ROOT)))
									lbxAllParents.Items.Add(new ListItem(grp.groupName, grp.GroupID.ToString()));
					}
					else
					{
						//Write to Associated Listbox if already associated
						if ((Session["GroupName"].ToString().Equals(Group.SUPERUSER)))
							lbxParentGroups.Items.Add(new ListItem(grp.groupName, grp.GroupID.ToString()));	
						else
							lbxParentGroups.Items.Add(new ListItem(grp.groupName, grp.GroupID.ToString()));	
					}
				}
			}
		}

		/// <summary>
		/// Add Button.
		/// Adds items from the Available Listbox to the Associated Listbox.
		/// Removes added items from the Available Listbox.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ibtnAddParent_Click(object sender, System.Web.UI.ImageClickEventArgs e)
		{
			if (lbxAllParents.SelectedIndex >= 0)
			{
				ListItem li = lbxAllParents.SelectedItem;

				lbxAllParents.Items.Remove(li);
				lbxAllParents.SelectedIndex = 0;

				lbxParentGroups.SelectedIndex = -1;
				lbxParentGroups.Items.Add(li);
			}
		}

		/// <summary>
		/// Remove Button.
		/// Removes items from the Parent Listbox.
		/// Adds removed items back to the Parent Listbox.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ibtnRemoveParent_Click(object sender, System.Web.UI.ImageClickEventArgs e)
		{
			if (lbxParentGroups.SelectedIndex >= 0)
			{
				ListItem li = lbxParentGroups.SelectedItem;

				lbxParentGroups.Items.Remove(li);
				lbxParentGroups.SelectedIndex = 0;

				lbxAllParents.SelectedIndex = -1;
				lbxAllParents.Items.Add(li);
			}
		}
		
		protected void btnSaveChanges_Click(object sender, System.EventArgs  e)
		{
			try
			{
				lblResponse.Text = null;
				if(lbxParentGroups.Items.Count <1)
				{
					lblResponse.Text = Utilities.FormatErrorMessage("You must select a parent group!");
					lblResponse.Visible = true;
					return;
				}	
				if(groupInfo.id == -1) // adding a new record
				{
					saveNew();
				}
				else // Modify an existing group
				{
					saveModified();
				}
				//Session.Remove("GroupInfo");
				string jScript;
				jScript = "<script language=javascript> window.opener.Form1.hiddenPopupOnSave.value='1';";
				jScript += "window.close();</script>";
				Page.RegisterClientScriptBlock("postbackScript", jScript);
			}
			catch (Exception ex)
			{
				lblResponse.Text = Utilities.FormatErrorMessage(ex.GetBaseException().ToString());
			}
		}

		protected void saveNew()
		{
			StringBuilder sb = new StringBuilder();
			lblResponse.Text = null;
			
			try
			{
				if(txtName.Text == null )
				{
					throw new Exception("Group Name can not be empty.");
				}
			
				if(AdministrativeAPI.GetGroupID(txtName.Text) > 0) // group record exists, choose another name
				{
					throw new Exception("Group: " + txtName.Text + " exists, choose another name.");
				}

                //if (AdministrativeAPI.GetUserID(txtName.Text) > 0) // user record exists, choose another name
                //{
                //    throw new Exception("The name '" + txtName.Text + "' is not available, choose another name.");
                //}
									
			
				int groupRootID = AdministrativeAPI.GetGroupID(Group.ROOT); //Group.ROOT;
				int tmpParent = groupRootID;

				// If a parent is specified, then add group under that parent group; otherwise add it under ROOT
				if(lbxParentGroups.Items.Count >0)
					tmpParent = Convert.ToInt32(lbxParentGroups.Items[0].Value); // find tmpParent
				groupInfo.id = wrapper.AddGroupWrapper(txtName.Text, tmpParent, txtDescription.Text,txtEmail.Text, GroupType.REGULAR, 0); //AddGroup

				// If more than 1 parent is specified, then add group to all other parent groups (except the one under which it was created in the previous step)
				// AddMemberToGroup automatically handles all qualifier transfers
				if(lbxParentGroups.Items.Count >1)
				{
					for(int i = 1;i < lbxParentGroups.Items.Count;i++)
					{
						wrapper.AddGroupToGroupWrapper(groupInfo.id,Convert.ToInt32(lbxParentGroups.Items[i].Value));
					}
				}

				/* Give current session group priviliges to administer the new group */
				int loginGroupID = Convert.ToInt32(Session["GroupID"]);
				int groupQID = AuthorizationAPI.GetQualifierID(groupInfo.id, Qualifier.groupQualifierTypeID);
				AuthorizationAPI.AddGrant(loginGroupID, Function.administerGroupFunctionType,groupQID);
				//wrapper.AddGrantWrapper(loginGroupID, Function.administerGroupFunctionType,groupQID);

			
				int requestID =0;
				// Create a Request group if the box has been checked
				if(cbxRequestgroup.Checked == true)
				{
					string requestGp = txtName.Text + "-request";
					requestID = wrapper.AddGroupWrapper(requestGp, AdministrativeAPI.GetGroupID(Group.NEWUSERGROUP),
						"Request Group for group: " + txtName.Text, txtEmail.Text, GroupType.REQUEST, groupInfo.id );

					/* Give current session group priviliges to administer the new group's request group */
					int requestGroupQID = AuthorizationAPI.GetQualifierID(requestID, Qualifier.groupQualifierTypeID);
					//wrapper.AddGrantWrapper(loginGroupID, Function.administerGroupFunctionType,requestGroupQID);
					AuthorizationAPI.AddGrant(loginGroupID, Function.administerGroupFunctionType,requestGroupQID);

					if(requestID <= 0)
					{
						sb.Append("<p>Request group could not be added!</p>");
					}
				}

				// Create a TA group if the box has been checked
				if(cbxTAGroup.Checked == true)
				{
					string TAGp = txtName.Text + "-admin";
					int TAGrpID = wrapper.AddGroupWrapper(TAGp, groupInfo.id,
						"Course Staff Group for group: " + txtName.Text, txtEmail.Text, GroupType.COURSE_STAFF, groupInfo.id );
					if(TAGrpID <= 0)
					{
						sb.Append("<font size=12>Course Staff group could not be added!</font>");
					}
					else
					{
						/* Add permissions for TA groups */
						int groupQualID = AuthorizationAPI.GetQualifierID(groupInfo.id,Qualifier.groupQualifierTypeID);
						int expCollQualID = AuthorizationAPI.GetQualifierID(groupInfo.id,Qualifier.experimentCollectionQualifierTypeID);
						
						/* administer group & read experiment privileges over main group */
						wrapper.AddGrantWrapper(TAGrpID,Function.administerGroupFunctionType,groupQualID);
						wrapper.AddGrantWrapper(TAGrpID,Function.readExperimentFunctionType,expCollQualID);
						
						requestID = AdministrativeUtilities.GetGroupRequestGroup(groupInfo.id);
						if (requestID>0)
						{
							int reqGroupQualID = AuthorizationAPI.GetQualifierID(requestID,Qualifier.groupQualifierTypeID);
							/* administer group privilege over request group */
							wrapper.AddGrantWrapper(TAGrpID,Function.administerGroupFunctionType,reqGroupQualID);
						}

						/* administer group privilege over itself */
						int TAGroupQualID = AuthorizationAPI.GetQualifierID(TAGrpID,Qualifier.groupQualifierTypeID);
						wrapper.AddGrantWrapper(TAGrpID, Function.administerGroupFunctionType,TAGroupQualID);

						/* Give current session group priviliges to administer the new group's request group */
						//wrapper.AddGrantWrapper(loginGroupID, Function.administerGroupFunctionType,TAGroupQualID);
						AuthorizationAPI.AddGrant(loginGroupID, Function.administerGroupFunctionType,TAGroupQualID);

					}
				}
				
				lblResponse.Text = Utilities.FormatConfirmationMessage("Group '" + txtName.Text + "' has successfully been created.");
				lblResponse.Visible = true;

			}
			catch (Exception ex)
			{
				sb.Append("<p>Trouble adding new group '" + txtName.Text +"'."+ex.GetBaseException()+"</p>");
				lblResponse.Text = Utilities.FormatErrorMessage(sb.ToString());
				lblResponse.Visible = true;
				//throw new Exception();
			}
            AuthCache.Refresh();
		}

		protected void saveModified()
		{
			StringBuilder sb = new StringBuilder();
			lblResponse.Text = null;
			try
			{
				if((!txtDescription.Text.Equals(groupInfo.description)) || (!txtEmail.Text.Equals(groupInfo.email)))
				{
					wrapper.ModifyGroupWrapper (groupInfo.id, txtName.Text , txtDescription.Text,txtEmail.Text);
				}

				int reqID = 0;
				if((cbxRequestgroup.Checked) == true && (groupInfo.request == -1))
				{
					string requestGp = txtName.Text + "-request";
					reqID = wrapper.AddGroupWrapper(requestGp, AdministrativeAPI.GetGroupID(Group.NEWUSERGROUP), "Request Group for group: " + txtName.Text,txtEmail.Text, GroupType.REQUEST, groupInfo.id );
					if(reqID <= 0)
					{
						sb.Append("<p>Request group could not be added!</p>");
					}
					else
					{
						int reqGroupQualID = AuthorizationAPI.GetQualifierID(reqID,Qualifier.groupQualifierTypeID);
						/* anybody can addmember privilege over request group */
						/* Specify ROOT as everybody! */
						int rootGroupID = AdministrativeAPI.GetGroupID (Group.ROOT);
						wrapper.AddGrantWrapper(rootGroupID,Function.addMemberFunctionType,reqGroupQualID);
						groupInfo.request = reqID;
					}
				}

				// Create a TA group if the box has been checked
				if((cbxTAGroup.Checked == true)&& (groupInfo.admin == -1))
				{
					string TAGp = txtName.Text + "-admin";
					int TAGrpID = wrapper.AddGroupWrapper(TAGp,groupInfo.id,
						"Course Staff Group for group: " + txtName.Text, txtEmail.Text, GroupType.COURSE_STAFF, groupInfo.id );
					groupInfo.admin = TAGrpID;
					if(TAGrpID <= 0)
					{
						sb.Append("<p>Course Staff group could not be added!</p>");
					}
					else
					{
						/* Add permissions for TA groups */
						int groupQualID = AuthorizationAPI.GetQualifierID(groupInfo.id,Qualifier.groupQualifierTypeID);
						int expCollQualID = AuthorizationAPI.GetQualifierID(groupInfo.id,Qualifier.experimentCollectionQualifierTypeID);
						
						/* administer group & read experiment privileges over main group */
						wrapper.AddGrantWrapper(TAGrpID,Function.administerGroupFunctionType,groupQualID);
						wrapper.AddGrantWrapper(TAGrpID,Function.readExperimentFunctionType,expCollQualID);
						
						/* administer group privilege over itself */
						int TAGroupQualID = AuthorizationAPI.GetQualifierID(TAGrpID,Qualifier.groupQualifierTypeID);
						wrapper.AddGrantWrapper(TAGrpID, Function.administerGroupFunctionType,TAGroupQualID);
					}
				}
				
				if (((cbxRequestgroup.Checked)==true)&& ((cbxTAGroup.Checked)==true)&&(groupInfo.admin >0))
				{
					int reqGroupQualID = AuthorizationAPI.GetQualifierID(groupInfo.request,Qualifier.groupQualifierTypeID);

					// add request group adminstration grant
					int[] reqGroupGrant = AuthorizationAPI.FindGrants(groupInfo.admin,Function.administerGroupFunctionType, reqGroupQualID);

					if (reqGroupGrant.Length==0)
						wrapper.AddGrantWrapper(groupInfo.admin, Function.administerGroupFunctionType,reqGroupQualID);
				}

				//Process before request / TA groups or else grants will get overwritten
				getExpGrants(); // fill expList with existing grants

				// Step through the expGrants and modify if needed
				saveExperimentStatus();

				if((cbxRequestgroup.Checked) == false && (groupInfo.request != -1))
				{
					wrapper.RemoveGroupsWrapper(new int[] {groupInfo.request});
					
				}
				if((cbxTAGroup.Checked) == false && (groupInfo.admin != -1))
				{
					wrapper.RemoveGroupsWrapper(new int[] {groupInfo.admin});
					
				}
				
				
				// Process the parents list
				// 
				ArrayList removeParentList = new ArrayList();
				ArrayList addParentList = new ArrayList();
				int [] parIDs = AdministrativeAPI.ListParentGroupsForGroup(groupInfo.id);
				for(int i = 0; i< parIDs.Length ;i++)
				{
					ListItem it = lbxParentGroups.Items.FindByValue(parIDs[i].ToString());
					if(it != null)
					{
						lbxParentGroups.Items.Remove(it);
						addParentList.Add(it);
					}
					else
					{
						removeParentList.Add(parIDs[i]);
					}
				}
				
				foreach(ListItem li in lbxParentGroups.Items)
				{
					wrapper.AddGroupToGroupWrapper(groupInfo.id,Convert.ToInt32(li.Value));
				}
				if(removeParentList.Count >0)
				{
					foreach(int k in removeParentList)
					{
                        AdministrativeAPI.RemoveGroupFromGroup(groupInfo.id, k);
					}
				}
				//refreshing the parents list box - otherwise removed items wont appear
				foreach (ListItem li in addParentList)
				{
					lbxParentGroups.Items.Add(li);
				}

				//AdministrativeAPI.Administration .ModifyGroup (txtName.Text , txtDescription.Text );
						
				lblResponse.Text = Utilities.FormatConfirmationMessage("Group '" + txtName.Text + "' has successfully been modified.");
				lblResponse.Visible = true;
						
			}
			catch (Exception ex)
			{
				sb.Append("Group " + txtName.Text + " can't be modified."+ex.GetBaseException());
				lblResponse.Text = Utilities.FormatErrorMessage(sb.ToString());
				lblResponse.Visible = true;
				//throw new Exception();
			}
            AuthCache.Refresh();
		}

		private void saveExperimentStatus()
		{
			foreach(RepeaterItem it in repExperiments.Items)
			{
				String groupName = ((Label)it.FindControl("lblExp")).Text;
				Boolean hasRead = ((CheckBox) it.FindControl("cbxRead")).Checked;
				Boolean hasWrite = ((CheckBox) it.FindControl("cbxWrite")).Checked;
				Boolean hasCreate = ((CheckBox) it.FindControl("cbxCreate")).Checked;
				int groupid = wrapper.GetGroupIDWrapper(groupName);
				int [] grantIDs = wrapper.FindGrantsWrapper(groupid,Function.readExperimentFunctionType,groupInfo.collectionNode);
				if(grantIDs.Length >0)
				{ 
					if(!hasRead)
					{
						wrapper.RemoveGrantsWrapper(grantIDs);
					}
				}
				else
				{
					if(hasRead)
					{
						/*
						int qualID = AuthorizationAPI.GetQualifierID(groupid,Qualifier.experimentQualifierTypeID);
						if(qualID <= 0)
						{
							qualID = AuthorizationAPI.AddQualifier(groupid,Qualifier.experimentQualifierTypeID,"experiment: "+ groupName,Qualifier.ROOT);
						}
						*/
						if(groupInfo.collectionNode > 0)
						{
							wrapper.AddGrantWrapper(groupid,Function.readExperimentFunctionType,groupInfo.collectionNode);
						}
					}
				}
				
				grantIDs = wrapper.FindGrantsWrapper(groupid,Function.writeExperimentFunctionType,groupInfo.collectionNode);
				if(grantIDs.Length >0)
				{ 
					if(!hasWrite)
					{
						wrapper.RemoveGrantsWrapper(grantIDs);
					}

				}
				else
				{
					if(hasWrite)
					{
						if(groupInfo.collectionNode > 0)
						{
							wrapper.AddGrantWrapper(groupid,Function.writeExperimentFunctionType,groupInfo.collectionNode);
						}
					}
				}

				grantIDs = wrapper.FindGrantsWrapper(groupid,Function.createExperimentFunctionType,groupInfo.collectionNode);
				if(grantIDs.Length >0)
				{ 
					if(!hasCreate)
					{
						wrapper.RemoveGrantsWrapper(grantIDs);
					}

				}
				else
				{
					if(hasCreate)
					{
						if(groupInfo.collectionNode > 0)
						{
							wrapper.AddGrantWrapper(groupid,Function.createExperimentFunctionType,groupInfo.collectionNode);
						}
					}
				}

			}
		}


        public class GrantInfo
        {
            public int id = -1;
            public string name;
            public Boolean isRead = false;
            public Boolean isWrite = false;
            public Boolean isCreate = false;
            public Boolean isEditable = false;
            public Boolean isCollectionNode = false;

            public string Name
            {
                get { return name; }
                set { name = value; }
            }
        }

}

}
