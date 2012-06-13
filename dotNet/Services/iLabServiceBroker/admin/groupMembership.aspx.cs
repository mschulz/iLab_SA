/*
 * Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
 * Please see license.txt in top level directory for full license.
 * 
 * $Id$
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Mail;

using iLabs.Core;
using iLabs.DataTypes;
using iLabs.ServiceBroker;
using iLabs.ServiceBroker.Administration;
using iLabs.ServiceBroker.Authorization;
using iLabs.ServiceBroker.Internal;
using iLabs.UtilLib;

namespace iLabs.ServiceBroker.admin
{
	/// <summary>
	/// Summary description for groupMembership.
	/// </summary>
	public partial class groupMembership : System.Web.UI.Page
	{
        string rootImage = "../img/GrantImages/root.gif";
        string groupImage = "../img/GrantImages/Folder.gif";
        string userImage = "../img/GrantImages/user.gif";
        //int userGroupID = -1;
        //int requestGroupID = -1;
		AuthorizationWrapperClass wrapper = new AuthorizationWrapperClass();
        BrokerDB brokerDB = new BrokerDB();
		string registrationMailAddress = ConfigurationManager.AppSettings["registrationMailAddress"];

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (Session["UserID"]==null)
				Response.Redirect("../login.aspx");
            
			if(!IsPostBack)
			{

                BuildTrees();
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
			this.ibtnCopyTo.Click += new System.Web.UI.ImageClickEventHandler(this.ibtnCopyCB_Click);
			this.ibtnMoveTo.Click += new System.Web.UI.ImageClickEventHandler(this.ibtnMoveCB_Click);
			this.ibtnRemove.Click += new System.Web.UI.ImageClickEventHandler(this.ibtnRemoveCB_Click);
			
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    

		}
		#endregion

	
        private void BuildTrees()
        {
            List<int> groupIDs = new List<int>();
            int groupID = Convert.ToInt32(Session["GroupID"]);
           
            if (Convert.ToBoolean(Session["IsAdmin"]))
            {
                if (Session["GroupName"].ToString().Contains("-admin"))
                {
                    int gID = AdministrativeAPI.GetAssociatedGroupID(groupID);
                    if (gID > 0)
                    {

                        DbParameter param = FactoryDB.CreateParameter("@groupID", gID, DbType.Int32);
                        int rgID = brokerDB.GetInt("Group_RetrieveRequestGroupID", param);
                        if (rgID > 0)
                        {
                            groupIDs.Add(rgID);
                        }
                        groupIDs.Add(gID);

                    }
                }

                else
                {
                    int rootID = AdministrativeAPI.GetGroupID(Group.ROOT);
                    groupIDs.Add(rootID);
                }
            }
            try
            {
                foreach (int i in groupIDs)
                {
                    Group startGroup = AdministrativeAPI.GetGroup(i);
                    string startImage = null;
                    if (startGroup.groupName.CompareTo(Group.ROOT) == 0)
                        startImage = rootImage;
                    else
                        startImage = groupImage;

                    TreeNode rootNodeAgents = new TreeNode(startGroup.groupName, startGroup.groupID.ToString(), startImage);
                    rootNodeAgents.Expanded = true;
                    rootNodeAgents.SelectAction = TreeNodeSelectAction.None;

                    TreeNode rootNodeGroups = new TreeNode(startGroup.groupName, startGroup.groupID.ToString(), startImage);
                    rootNodeGroups.Expanded = true;
                    rootNodeGroups.SelectAction = TreeNodeSelectAction.None;
                    AddAgentsRecursively(rootNodeAgents, rootNodeGroups);

                    agentsTreeView.Nodes.Add(rootNodeAgents);
                    groupsTreeView.Nodes.Add(rootNodeGroups);
                }
                
            }
            catch (Exception ex)
            {
                string msg = "Exception: Cannot list groups. " + ex.Message + ". " + ex.GetBaseException();
                lblResponse.Text = Utilities.FormatErrorMessage(msg);
                lblResponse.Visible = true;
            }
        }

		/* 
		 * We need to pass in 2 nodes here, since the Groups tree is different from the agents tree
		 * The nAgents will contain users in addition to group members.
		 * nGroups will only contain members that are groups.
		 */
		private void AddAgentsRecursively(TreeNode nAgents, TreeNode nGroups)
		{
           
			try
			{
                if (nAgents.ImageUrl.CompareTo(userImage) != 0)// Do not process if it is a user
                {
                    int groupID = Convert.ToInt32(nAgents.Value);

                    //Should Filter by Wrapper
                    IntTag[] userTags = brokerDB.GetIntTags("Group_RetrieveUserTags",
                        FactoryDB.CreateParameter("@groupID", groupID, DbType.Int32));
                    Group[] groups = AdministrativeAPI.GetGroups(
                    brokerDB.GetInts("Group_RetrieveChildrenGroupIDs",
                        FactoryDB.CreateParameter("@groupID", groupID, DbType.Int32)).ToArray());

                    //int[] childUserIDs = wrapper.ListUserIDsInGroupWrapper(Convert.ToInt32(nAgents.Value));
                    //int[] childGroupIDs = wrapper.ListSubgroupIDsWrapper(Convert.ToInt32(nAgents.Value));

                    //User[] usersList = wrapper.GetUsersWrapper(childUserIDs);
                    //List<User> childUsersList = new List<User>();
                    //childUsersList.AddRange(usersList);
                    //childUsersList.Sort();

                    //Group[] groupsList = wrapper.GetGroupsWrapper(childGroupIDs);
                    //List<Group> childGroupsList = new List<Group>();
                    //childGroupsList.AddRange(groupsList);
                    //childGroupsList.Sort();

                    foreach (IntTag u in userTags)
                    {
                        TreeNode childNode = new TreeNode(u.tag, u.id.ToString(), userImage);
                        //childNode.Status = 1;
                        childNode.ShowCheckBox = true;
                        childNode.SelectAction = TreeNodeSelectAction.None;
                        childNode.Expanded = false;
                        nAgents.ChildNodes.Add(childNode);
                    }


                    if (groups == null || groups.Length < 1)
                    {
                        nAgents.Expanded = false;
                    }
                    foreach (Group g in groups)
                    {
                        if (g.groupID > 0)
                        {
                            TreeNode childNode = new TreeNode(g.groupName, g.groupID.ToString(), groupImage);
                            childNode.SelectAction = TreeNodeSelectAction.None;
                            childNode.ShowCheckBox = isRegular(g);
                            childNode.Collapse();
                            TreeNode groupChildNode = new TreeNode(g.groupName, g.groupID.ToString(), groupImage);
                            groupChildNode.ShowCheckBox = true;
                            groupChildNode.SelectAction = TreeNodeSelectAction.None;
                            this.AddAgentsRecursively(childNode, groupChildNode);
                            nAgents.ChildNodes.Add(childNode);
                            nGroups.ChildNodes.Add(groupChildNode);
                        }
                    }
                }
			}
			catch (Exception ex)
			{
				lblResponse.Text = Utilities.FormatErrorMessage("Cannot list users and groups. "+ex.GetBaseException());
                lblResponse.Visible = true;
			}
		}

		private void ExpandNode(TreeNodeCollection nodes, int nodeID)
		{
			foreach(TreeNode n in nodes)
			{
                
				if(Convert.ToInt32(n.Value) == nodeID)
				{
					n.Expanded=true;
					TreeNode parent = (TreeNode)n.Parent;
					//parent.Expanded = true;
					if (!parent.Text.Equals("ROOT"))
						ExpandParent(parent);
				}
				ExpandNode(n.ChildNodes, nodeID);
			}
		}
        private void ExpandNodes(TreeNodeCollection nodes, int[] nodeIDs)
        {
            foreach (TreeNode n in nodes)
            {
                int nID = Convert.ToInt32(n.Value);
                foreach (int nodeID in nodeIDs)
                {
                    if (nID == nodeID)
                    {
                        n.Expanded = true;
                        //TreeNode parent = (TreeNode)n.Parent;
                        //parent.Expanded = true;
                        //if (parent != null && parent.Text != null && !parent.Text.Equals("ROOT"))
                        //    ExpandParent(parent);
                        ExpandParent(n);
                    }
                }
                ExpandNodes(n.ChildNodes, nodeIDs);
            }
        }

		private void ExpandParent(TreeNode n)
		{
			n.Expanded=true;
			TreeNode parent = (TreeNode)n.Parent;
            if (parent != null && parent.Text != null && !parent.Text.Equals("ROOT"))
				ExpandParent(parent);
		}

        private bool isRegular(Group g)
        {
            return (GroupType.REGULAR.CompareTo(g.GroupType) == 0);
        }

        private void ibtnRemoveCB_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            lblResponse.Visible = false;
            lblResponse.Text = "";
            StringBuilder message = new StringBuilder();
            List<int> parentIDs = new List<int>();
            
            TreeNodeCollection agentNodes = agentsTreeView.CheckedNodes;
            if (agentNodes == null || agentNodes.Count < 1)
            {
                string msg = "Error: You must select agents to remove.";
                lblResponse.Text = Utilities.FormatErrorMessage(msg);
                lblResponse.Visible = true;
                return;
            }
            message.Append(removeAgentNodes(agentNodes, ref parentIDs));
            if(message.Length > 0){
                lblResponse.Text = Utilities.FormatConfirmationMessage(message.ToString());
                lblResponse.Visible = true;
            }
            //Refresh tree views
            agentsTreeView.Nodes.Clear();
            groupsTreeView.Nodes.Clear();
            this.BuildTrees();

            //expand the user tree to show the state of the parent group
            foreach (int parentID in parentIDs)
            {
                ExpandNode(agentsTreeView.Nodes, parentID);
            }

        }
       

         private void ibtnCopyCB_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            lblResponse.Visible = false;
            lblResponse.Text = "";
            copyAgents(false);
        }

        private void ibtnMoveCB_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            lblResponse.Visible = false;
            lblResponse.Text = "";
            copyAgents(true);
        }

        protected void btnSearch_Click(object sender, System.EventArgs e)
        {
        }


        private string removeAgentNodes(TreeNodeCollection agentNodes, ref List<int> parentIDs)
        {
            bool status = false;
            StringBuilder msg = new StringBuilder();
            List<int> agents = new List<int>();
            List<int> users = new List<int>();
            foreach (TreeNode agentNode in agentNodes)
            {
                status = false;
                int memberID = Convert.ToInt32(agentNode.Value);
                string memberName = agentNode.Text;
                int parentID = Convert.ToInt32(agentNode.Parent.Value);
                bool isUser = (agentNode.ImageUrl.CompareTo(userImage) == 0);
                string parentName = agentNode.Parent.Text;
                if(!isUser){
                // Note that because of the business logic you may not remove built-in groups
                    if (memberName.Equals(Group.ROOT)
                        || (memberName.Equals(Group.NEWUSERGROUP))
                        || (memberName.Equals(Group.ORPHANEDGROUP))
                        || (memberName.Equals(Group.SUPERUSER)))
                    {
                        // if they're trying to remove the ROOT, superUser etc.
                        msg.Append("The '" + memberName + "' group cannot be removed from the system.<br />");
                    } 
                    else if (memberID == parentID)
                    {
                           msg.Append("'" + memberName + "'  cannot be removed from itself.<br />");
                    }
                    else //if parent!=groupname
                    {
                        status = AdministrativeAPI.RemoveGroupFromGroup(memberID, parentID);
                        if (status)
                        {
                            lblResponse.Visible = true;
                            msg.Append("'" + memberName + "' was successfully removed from '" + parentName + "'.<br />");
                        }
                    }
                }
                else //User
                {
                    try
                    {
                        //Does anyone want functionality to send email once a user has been moved to a new group.
                        //This can get annoying, so maybe it should be configurable in web.config
                        // If yes.. then put code in here  - CV, 2/15/05
                        //Logic for this is as follows - if user get email address, otherwise get email addresses of all users in a group (GetUserIDsRecursively call?)
                      
                        //if it has removed all the members
                       
                        status =AdministrativeAPI.RemoveUserFromGroup(memberID,parentID);
                        if (status)
                        {
                            if (!users.Contains(memberID))
                                users.Add(memberID);
                            //if (!parentIDs.Contains(parentID))
                           //     parentIDs.Add(parentID);
                            lblResponse.Visible = true;
                            msg.Append("'" + memberName + "' was successfully removed from '" + parentName + "'.<br />");
                        }
                    }
                    catch (Exception removeEx)
                    {
                        msg.Append("ERROR: removing '" + memberName + "' from '" + parentName + "'. Exception: " + removeEx.Message + "<br />");
                    }
                }
            } // END foreach agent

            foreach (int id in users)
            {
                int[] parents = wrapper.ListGroupsForUserWrapper(id);
                if ((parents == null) || (parents.Length == 0))
                {
                   AdministrativeAPI.AddUserToGroup(id, InternalAdminDB.SelectGroupID(Group.ORPHANEDGROUP));
                }
            }
            return msg.ToString();
        }

        private void copyAgents(bool move)
        {
            StringBuilder msg = new StringBuilder();
            List<int> expandIDs = new List<int>();
            lblResponse.Visible = false;
            TreeNodeCollection agentNodes = agentsTreeView.CheckedNodes;
            TreeNodeCollection groupNodes = groupsTreeView.CheckedNodes;
            if (agentNodes == null || agentNodes.Count < 1 || groupNodes == null || groupNodes.Count < 1)
            {
                msg.Append("Error: You must check at least one item from each list");
                lblResponse.Text = Utilities.FormatErrorMessage(msg.ToString());
                lblResponse.Visible = true;
                return;
            }
            foreach (TreeNode agentNode in agentNodes)
            {
                int count = 0;
                bool status = false;
                int agentID = Convert.ToInt32(agentNode.Value);
                string agentName = agentNode.Text;
                bool isUser = (agentNode.ImageUrl.CompareTo(userImage) == 0);
                //Now get the ID of the Parent group
                TreeNode parentNode = (TreeNode)agentNode.Parent;
                Group parentGroup = wrapper.GetGroupsWrapper(new int[] { Convert.ToInt32(parentNode.Value) })[0];

                foreach (TreeNode groupNode in groupNodes)
                {
                    int destinationID = Convert.ToInt32(groupNode.Value);
                    string destinationName = groupNode.Text;


                    // Note that because of the business logic, no agent can be moved to the ROOT node.
                    // The business logic says that agents cannot simultaneuosly exist under the ROOT node
                    // and under some other group node.
                    if (agentName.Equals(Group.ROOT)
                       || ( !isUser && agentID == destinationID)
                       || agentName.Equals(Group.NEWUSERGROUP)
                       || agentName.Equals(Group.ORPHANEDGROUP)
                       || agentName.Equals(Group.SUPERUSER))
                    {
                        msg.Append("'ERROR: You may not copy/move " + agentName + "' to '" + destinationName + "'<br />");
                    }
                    else
                    {

                        try
                        {
                            bool isMember = false;
                            if(isUser){
                                isMember =AdministrativeAPI.IsUserMember(agentID, destinationID);
                            }
                            else{
                                isMember =AdministrativeAPI.IsGroupMember(agentID, destinationID);
                            }
                            if (isMember)
                            {
                                msg.Append("Warning: '" + agentName + "' is already a member of '" + destinationName + "'<br />");
                            }
                            else
                            {
                                bool added = false;
                                if (isUser)
                                {
                                    added = AdministrativeAPI.AddUserToGroup(agentID, destinationID);
                                }
                                else
                                {
                                   added = AdministrativeAPI.AddGroupToGroup(agentID, destinationID);
                                }
                                if (added)
                                {
                                    count++;
                                    if (!expandIDs.Contains(destinationID))
                                        expandIDs.Add(destinationID);
                                    msg.Append("'" + agentName + "' was successfully added to '" + destinationName + "'<br />");

                                    //send email message to moved user/group if given access from a request group
                                    if ((parentGroup.groupType.Equals(GroupType.REQUEST)) && (wrapper.GetAssociatedGroupIDWrapper(parentGroup.GroupID) == destinationID))
                                    {


                                        string email = null;
                                        if (isUser)
                                        {
                                            email = wrapper.GetUsersWrapper(new int[] { agentID })[0].email;
                                        }
                                        else
                                        {
                                            email = wrapper.GetGroupsWrapper(new int[] { agentID })[0].email;
                                        }
                                        if (email != null && email.Length > 0)
                                        {
                                            MailMessage mail = new MailMessage();
                                            mail.To = email;
                                            mail.From = registrationMailAddress;

                                            mail.Subject = "[iLabs] Request to join '" + destinationName + "' approved";
                                            mail.Body = "You have been given permission to access the '" + destinationName + "' group.";
                                            mail.Body += "\n\r\n\r";
                                            mail.Body += "Login with the username and password that you registered with to use the lab.";
                                            mail.Body += "\n\r\n\r";
                                            mail.Body += "-------------------------------------------------\n\r";
                                            mail.Body += "This is an automatically generated message. ";
                                            mail.Body += "DO NOT reply to the sender. \n\n";
                                            //mail.Body += "For questions regarding this service, email ilab-debug@mit.edu";
                                            SmtpMail.SmtpServer = "127.0.0.1";
                                            try
                                            {
                                                SmtpMail.Send(mail);
                                                msg.Append(" An email has been sent confirming the move.<br />");

                                            }
                                            catch (Exception ex)
                                            {
                                                // Report detailed SMTP Errors
                                                StringBuilder smtpErrorMsg = new StringBuilder();
                                                smtpErrorMsg.Append("Exception: " + ex.Message);
                                                //check the InnerException
                                                if (ex.InnerException != null)
                                                    smtpErrorMsg.Append("<br>Inner Exceptions:");
                                                while (ex.InnerException != null)
                                                {
                                                    smtpErrorMsg.Append("<br>" + ex.InnerException.Message);
                                                    ex = ex.InnerException;
                                                }
                                                msg.Append(" However an error occurred while sending email to the member." + ". Exception: " + smtpErrorMsg.ToString() + "<br />");

                                            }
                                        } //End Send Mail
                                    }
                                }
                                else
                                {
                                    msg.Append("'ERROR: adding " + agentName + "' to '" + destinationName + "'<br />");
                                }
                            }
                        }// END try Add member
                        catch (Exception addEx)
                        {
                            msg.Append("Error: Adding " + agentName + "' to '" + destinationName + "' Exception: " + addEx.Message + "<br />");
                        }
                    }
                } 
            } // END Agents
            if (move) // Made at least one copy
            {
                msg.Append(removeAgentNodes(agentNodes, ref expandIDs));
            }
            //Refresh tree views
            agentsTreeView.Nodes.Clear();
            groupsTreeView.Nodes.Clear();
            this.BuildTrees();

            //expand the user tree to show the state of the destination group
            //& select the node that was just moved
                foreach(int id in expandIDs){
            ExpandNode(agentsTreeView.Nodes, id);
                }
            lblResponse.Text = Utilities.FormatConfirmationMessage(msg.ToString());
            lblResponse.Visible = true;
        }

       

	}

    //public class TreeNodeStatus : TreeNode
    //{
    //    protected int status = 0;
    //    public TreeNodeStatus(string text, string value, string imageUrl) : base(text,value,imageUrl){}
       
    //    public int Status
    //    {
    //        get
    //        {
    //            return status;
    //        }
    //        set
    //        {
    //            status = value;
    //        }
    //    }
    //}
}
