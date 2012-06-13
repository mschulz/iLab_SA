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
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Security;

using iLabs.ServiceBroker;
using iLabs.ServiceBroker.Administration;
using iLabs.ServiceBroker.Authorization;
using iLabs.ServiceBroker.DataStorage;
using iLabs.ServiceBroker.Internal;
using iLabs.UtilLib;

//using Microsoft.Web.UI.WebControls.Design;

namespace iLabs.ServiceBroker.admin
{
	/// <summary>
	/// Summary description for grants.
	/// </summary>
	public partial class grants : System.Web.UI.Page
	{
		AuthorizationWrapperClass wrapper = new AuthorizationWrapperClass();
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (Session["UserID"]==null)
				Response.Redirect("../login.aspx");

			//only superusers can view this page
			if (!Session["GroupName"].ToString().Equals(Group.SUPERUSER))
				Response.Redirect("../home.aspx");

			if(!IsPostBack)
			{
				this.BuildAgentsTree();
				this.BuildQualifierTree();
                this.BuildFunctionList();
               
				lblDirections.Text = "<p> Select an agent, function and qualifier when you want to add or remove a grant. To view a grant select a function and either an agent or a qualifier (but not both). Make sure that the non-selected tree is set to ROOT (for e.g. if you select agents, select ROOT in the qualifiers tree)</p>";
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

        private void BuildFunctionList()
        {
            
               //populate functions in the drop down list
               lbxFunctions.Items.Add(Function.addMemberFunctionType);
               lbxFunctions.Items.Add(Function.administerGroupFunctionType);
               lbxFunctions.Items.Add(Function.createExperimentFunctionType);
               lbxFunctions.Items.Add(Function.readExperimentFunctionType);
               lbxFunctions.Items.Add(Function.useLabClientFunctionType);
               lbxFunctions.Items.Add(Function.useLabServerFunctionType);
               lbxFunctions.Items.Add(Function.writeExperimentFunctionType);
               lbxFunctions.Items.Add(Function.administerLSS);
               lbxFunctions.Items.Add(Function.administerESS);
               lbxFunctions.Items.Add(Function.administerLS);
               lbxFunctions.Items.Add(Function.administerUSS);
               lbxFunctions.Items.Add(Function.manageLAB);
               lbxFunctions.Items.Add(Function.administerExperiment);
               lbxFunctions.Items.Add(Function.manageUSSGroup);
               lbxFunctions.Items.Add(Function.requestReservation);

        }

		private void BuildAgentsTree()
		{
			TreeNode rootNode = new TreeNode();
			rootNode.Text = "ROOT";
			int rootID = wrapper.GetGroupIDWrapper(Group.ROOT);
			rootNode.Value = rootID.ToString();
			rootNode.ImageUrl = "../img/GrantImages/root.gif";
            rootNode.Expanded = false;
            rootNode.PopulateOnDemand = true;
            rootNode.SelectAction = TreeNodeSelectAction.Select;
            //AddAgents(rootNode,false);
            userTreeView.Nodes.Add(rootNode);

			/*
			//nodes under root node (all these are groups)
			// since all are groups, the processing of these nodes is done separately here
			// as opposed to doing it in an AddAgentsRecursively(RootNode) call
			// This reduces the no. of database calls, made from isAgentUser
			int[] gIDs = wrapper.ListMemberIDsInGroupFromDSWrapper (rootID);
			Group[] gList = wrapper.GetGroupsWrapper(gIDs);

			//foreach node under the root node:
			//1- Add that node to the tree
			//2- recursively add children nodes to the tree
			foreach (Group g in gList)
			{
				if (g.groupID!=0)
				{
					TreeNode newNode = new TreeNode();
					newNode.Text =  g.groupName ;
					newNode.ImageUrl = "../img/GrantImages/Folder.gif";
					newNode.Value = g.groupID.ToString();
                    newNode.PopulateOnDemand = true;
                    newNode.SelectAction = TreeNodeSelectAction.Expand;
					rootNode.ChildNodes.Add(newNode);
				}
			}
            */
			
		}

        private void BuildQualifierTree()
        {
            TreeNode rootNode = new TreeNode();
            rootNode.Text = "ROOT";
            rootNode.Value = Qualifier.ROOT.ToString();
            rootNode.ImageUrl = "../img/GrantImages/root.gif";
            rootNode.Expanded = false;
            rootNode.PopulateOnDemand = true;
            //this.AddQualifiers(rootNode,false);
            qualifierTreeView.Nodes.Add(rootNode);
        }


        public void PopulateAgentNode(Object sender, TreeNodeEventArgs e)
        {
            AddAgents(e.Node,false);
        }
        
        public void PopulateQualifierNode(Object sender, TreeNodeEventArgs e)
        {
            AddQualifiers(e.Node,false);
        }

   

        private void AddAgents(TreeNode n, bool recurse)
        {
            int[] childUserIDs = wrapper.ListUserIDsInGroupWrapper(Convert.ToInt32(n.Value));
            int[] childGroupIDs = wrapper.ListSubgroupIDsWrapper(Convert.ToInt32(n.Value));

            User[] childUsersList = wrapper.GetUsersWrapper(childUserIDs);
            Group[] childGroupsList = wrapper.GetGroupsWrapper(childGroupIDs);

            foreach (User u in childUsersList)
            {
                TreeNode childNode = new TreeNode();
                childNode.Text = u.userName;
                childNode.Value = u.userID.ToString();
                childNode.ImageUrl = "../img/GrantImages/user.gif";
                childNode.SelectAction = TreeNodeSelectAction.Select;
                n.ChildNodes.Add(childNode);
            }

            foreach (Group g in childGroupsList)
            {
                TreeNode newNode = new TreeNode();
                newNode.Text = g.groupName;
                newNode.Value = g.groupID.ToString();
                newNode.ImageUrl = "../img/GrantImages/Folder.gif";
                newNode.PopulateOnDemand = true;
                newNode.SelectAction = TreeNodeSelectAction.Select;
                if (recurse)
                    AddAgents(newNode, true);
                n.ChildNodes.Add(newNode);
            }
        }


		

		
        private void AddQualifiers(TreeNode n,bool recurse)
        {
            int[] childIDs = AuthorizationAPI.ListQualifierChildren(Convert.ToInt32(n.Value));
            foreach (int childID in childIDs)
            {
                Qualifier q = AuthorizationAPI.GetQualifier(childID);
                if (q.qualifierID != Qualifier.nullQualifierID)
                {
                    string imageFile = "";
                    switch (q.qualifierType)
                    {
                        case Qualifier.groupQualifierTypeID:
                            imageFile = "Folder.gif";
                            break;
                        case Qualifier.labClientQualifierTypeID:
                            imageFile = "client.gif";
                            break;
                        case Qualifier.labServerQualifierTypeID:
                            imageFile = "server.gif";
                            break;
                        case Qualifier.experimentQualifierTypeID:
                            imageFile = "expt.gif";
                            break;
                        case Qualifier.experimentCollectionQualifierTypeID:
                            imageFile = "exptcol.gif";
                            break;
                        case Qualifier.resourceMappingQualifierTypeID:
                            imageFile = "mapping.GIF";
                            break;
                        default:
                            imageFile = "question.gif";
                            break;
                    }

                    string qualifierText = q.qualifierName;

                    TreeNode childNode = new TreeNode();
                    childNode.Text = qualifierText;
                    childNode.ImageUrl = "../img/GrantImages/" + imageFile;
                    childNode.Value = childID.ToString();
                    if (recurse)
                        AddQualifiers(childNode, true);
                    n.ChildNodes.Add(childNode);
                }
            }
        }


		private void ClearNodes (TreeNodeCollection treeNodes)
		{
			foreach (TreeNode n in treeNodes)
			{
                n.Text = n.Text.Replace("<font color=red>", "").Replace("<font color=green>", "").Replace("</font>", "");
                //n.Text = n.Text.Replace("<font color=green>", "");
				ClearNodes(n.ChildNodes);
			}
		}

		protected void btnAddGrant_Click(object sender, System.EventArgs e)
		{
			lblResponse.Visible=false;
			this.ClearNodes(userTreeView.Nodes);
			this.ClearNodes(qualifierTreeView.Nodes);

			if(lbxFunctions.SelectedIndex < 0 || userTreeView.SelectedNode == null || qualifierTreeView.SelectedNode == null)
			{
				lblResponse.Text = "<div class=errormessage><p>You must select an agent, function and qualifier. </p></div>";
				lblResponse.Visible = true;
			}
			else
			{
				int agentID = Convert.ToInt32(userTreeView.SelectedValue);
				string agentName = userTreeView.SelectedNode.Text;
				string functionType = lbxFunctions.SelectedItem.Text;
				int qualifierID = Convert.ToInt32(qualifierTreeView.SelectedValue);
                Qualifier q = AuthorizationAPI.GetQualifier(qualifierID);
				string qualifierName = q.qualifierName;
				bool error = false;

				/* Checking if the user entered selected an appropriate qualifier, 
				 * given the function.
				 * For e.g. we don't want grants that give an agent "useLabClient" 
				 * permission, when the qualifier selected is a lab server.
				 */
				switch (functionType)
				{
					case Function.addMemberFunctionType:
						if (q.qualifierType!=Qualifier.groupQualifierTypeID)
						{
							lblResponse.Text = "<div class=errormessage><p>The qualifier must be a group. </p></div>";
							lblResponse.Visible = true;
							error=true;
						}
						break;

					case Function.administerGroupFunctionType:
						if (q.qualifierType!=Qualifier.groupQualifierTypeID)
						{
							lblResponse.Text = "<div class=errormessage><p>The qualifier must be a group. </p></div>";
							lblResponse.Visible = true;
							error=true;
						}
						break;

					case Function.createExperimentFunctionType:
						if(! ((q.qualifierType==Qualifier.groupQualifierTypeID)||(q.qualifierType==Qualifier.experimentQualifierTypeID)||(q.qualifierType==Qualifier.experimentCollectionQualifierTypeID)))
						{
							lblResponse.Text = "<div class=errormessage><p>The qualifier must be a group, experiment or experiment collection. </p></div>";
							lblResponse.Visible = true;
							error=true;
						}
						break;

					case Function.readExperimentFunctionType:
						if(! ((q.qualifierType==Qualifier.groupQualifierTypeID)||(q.qualifierType==Qualifier.experimentQualifierTypeID)||(q.qualifierType==Qualifier.experimentCollectionQualifierTypeID)))
						{
							lblResponse.Text = "<div class=errormessage><p>The qualifier must be a group, experiment or experiment collection. </p></div>";
							lblResponse.Visible = true;
							error=true;
						}
						break;

					case Function.useLabClientFunctionType:
						if (q.qualifierType!=Qualifier.labClientQualifierTypeID)
						{
							lblResponse.Visible = true;
							lblResponse.Text = Utilities.FormatErrorMessage("The qualifier must be a lab client.");
							error=true;
						}
						break;

					case Function.useLabServerFunctionType:
						if (q.qualifierType!=Qualifier.labServerQualifierTypeID)
						{
							lblResponse.Visible = true;
							lblResponse.Text = Utilities.FormatErrorMessage("The qualifier must be a lab server.");
							error=true;
						}
						break;

					case Function.writeExperimentFunctionType:
						if(! ((q.qualifierType==Qualifier.groupQualifierTypeID)||(q.qualifierType==Qualifier.experimentQualifierTypeID)||(q.qualifierType==Qualifier.experimentCollectionQualifierTypeID)))
						{
							lblResponse.Visible = true;
							lblResponse.Text = Utilities.FormatErrorMessage("The qualifier must be a group, experiment or experiment collection.");
							error=true;
						}
						break;
				}

				/* If everything looks ok, then add the grant*/
				if (!error)
				{
					try
					{
						wrapper.AddGrantWrapper(agentID,functionType,qualifierID);
						lblResponse.Visible = true;
						string msg = "The grant {" + agentName + ", " +  functionType + ", " +qualifierName + "} was successfully added.";
						lblResponse.Text = Utilities.FormatConfirmationMessage(msg);
					}
					catch (Exception ex)
					{
						lblResponse.Text = Utilities.FormatErrorMessage("Cannot add grant. "+ex.Message+".");
						lblResponse.Visible = true;
					}
				}
			}
		}

		protected void btnRemoveGrant_Click(object sender, System.EventArgs e)
		{
			lblResponse.Visible=false;
			this.ClearNodes(userTreeView.Nodes);
			this.ClearNodes(qualifierTreeView.Nodes);

			if(lbxFunctions.SelectedIndex <= 0 || userTreeView.SelectedNode == null || qualifierTreeView.SelectedNode == null)
			{
				lblResponse.Visible = true;
				lblResponse.Text = Utilities.FormatErrorMessage("You must select an agent, function and qualifier.");
			}
			else
			{
				int agentID = Convert.ToInt32(userTreeView.SelectedValue);
				string agentName = userTreeView.SelectedNode.Text;
				string functionType = lbxFunctions.SelectedItem.Text;
				int qualifierID = Convert.ToInt32(qualifierTreeView.SelectedValue);
                Qualifier q = AuthorizationAPI.GetQualifier(qualifierID);
				string qualifierName = q.qualifierName;
				bool error = false;

				switch (functionType)
				{
					case Function.addMemberFunctionType:
						if (q.qualifierType!=Qualifier.groupQualifierTypeID)
						{
							lblResponse.Visible = true;
							lblResponse.Text=Utilities.FormatErrorMessage("The qualifier must be a group.");
							error=true;
						}
						break;

					case Function.administerGroupFunctionType:
						if (q.qualifierType!=Qualifier.groupQualifierTypeID)
						{
							lblResponse.Text=Utilities.FormatErrorMessage("The qualifier must be a group.");
							lblResponse.Visible = true;
							error=true;
						}
						break;

					case Function.createExperimentFunctionType:
						if ((q.qualifierType!=Qualifier.groupQualifierTypeID)||(q.qualifierType!=Qualifier.experimentQualifierTypeID)||(q.qualifierType!=Qualifier.experimentCollectionQualifierTypeID))
						{
							lblResponse.Visible = true;
							lblResponse.Text=Utilities.FormatErrorMessage("The qualifier must be a group, experiment or experiment collection.");
							error=true;
						}
						break;

					case Function.readExperimentFunctionType:
						if ((q.qualifierType!=Qualifier.groupQualifierTypeID)||(q.qualifierType!=Qualifier.experimentQualifierTypeID)||(q.qualifierType!=Qualifier.experimentCollectionQualifierTypeID))
						{
							lblResponse.Visible = true;
							lblResponse.Text=Utilities.FormatErrorMessage("The qualifier must be a group, experiment or experiment collection.");
							error=true;
						}
						break;

					case Function.useLabClientFunctionType:
						if (q.qualifierType!=Qualifier.labClientQualifierTypeID)
						{
							lblResponse.Text=Utilities.FormatErrorMessage("The qualifier must be a lab client.");
							lblResponse.Visible = true;
							error=true;
						}
						break;

					case Function.useLabServerFunctionType:
						if (q.qualifierType!=Qualifier.labServerQualifierTypeID)
						{
							lblResponse.Text=Utilities.FormatErrorMessage("The qualifier must be a lab server.");
							lblResponse.Visible = true;
							error=true;
						}
						break;

					case Function.writeExperimentFunctionType:
						if ((q.qualifierType!=Qualifier.groupQualifierTypeID)||(q.qualifierType!=Qualifier.experimentQualifierTypeID)||(q.qualifierType!=Qualifier.experimentCollectionQualifierTypeID))
						{
							lblResponse.Text=Utilities.FormatErrorMessage("The qualifier must be a group, experiment or experiment collection.");
							lblResponse.Visible = true;
							error=true;
						}
						break;
				}
				
				if (!error)
				{
				int[] grantIDs = wrapper.FindGrantsWrapper(agentID,functionType,qualifierID);

					if(grantIDs.Length == 0)
					{
						lblResponse.Visible = true;
						string msg = "A grant of the form {" + agentName + ", " +  functionType + ", " +qualifierName + "} does not exist in the database.";
						lblResponse.Text=Utilities.FormatErrorMessage(msg);
					}
					else
					{
						try
						{
							wrapper.RemoveGrantsWrapper(grantIDs);

							lblResponse.Visible = true;
							string msg = "The grant {" + agentName + ", " +  functionType + ", " +qualifierName + "} was successfully removed.";
							lblResponse.Text = Utilities.FormatConfirmationMessage(msg);
                           
                            // The deleted grants remain highlighted.
						}
						catch (Exception ex)
						{
							string msg = "Cannot remove grant. "+ex.Message;
							lblResponse.Visible = true;
							lblResponse.Text = Utilities.FormatErrorMessage(msg);
						}
					}
				}
			}
		}

		protected void btnViewGrant_Click(object sender, System.EventArgs e)
		{
			lblResponse.Visible=false;
			this.ClearNodes(userTreeView.Nodes);
			this.ClearNodes(qualifierTreeView.Nodes);

			if(lbxFunctions.SelectedIndex <= 0)
			{
				lblResponse.Text = "<div class=errormessage><p>You must select a function. </p></div>";
				lblResponse.Visible = true;
			}
			else
			{
				if (userTreeView.SelectedNode == null && qualifierTreeView.SelectedNode == null)
				{																													   		
					lblResponse.Text = "<div class=errormessage><p>You must select an agent or a qualifer. </p></div>";
					lblResponse.Visible = true;
				}
				else
				{
					// if an agent has been selected
					if (userTreeView.SelectedNode != null)
					{
						int agentID = Convert.ToInt32(userTreeView.SelectedValue);
						string agentName = userTreeView.SelectedNode.Text;
						string functionType = lbxFunctions.SelectedItem.Text;

						lblDirections.Text = "<p> The qualifiers pertaining to agent \"" + agentName
							+ "\" and function \"" + functionType 
							+ "\" are color-coded in the qualifiers tree. "
							+ "<font color=\"red\">Red</font> denotes explicit qualifiers. "
							+ "<font color=\"green\">Green</font> denotes implicit qualifiers. </p>";

						// need to get all the implicit and explicit grants associated with this agentID and functionType and render the tree accordingly
						ArrayList explicitQualifiers = new ArrayList();
						ArrayList implicitQualifiers = new ArrayList();

						int[] explicitGrantIDs = wrapper.FindGrantsWrapper(agentID,functionType,-1);
						Grant[] explicitGrants = wrapper.GetGrantsWrapper(explicitGrantIDs);

						foreach(Grant g in explicitGrants)
						{
							explicitQualifiers.Add(g.qualifierID.ToString());

							// now get all the descendant qualifiers of this qualifier
                            int[] qIDs = AuthorizationAPI.ListQualifierDescendants(g.qualifierID, true);
							foreach(int qID in qIDs)
							{
								implicitQualifiers.Add(qID.ToString());
							}
						}

						RenderTreeWithColor(explicitQualifiers, implicitQualifiers,qualifierTreeView.Nodes);
					}
					else
					{
						int qualifierID = Convert.ToInt32(qualifierTreeView.SelectedValue);
						string qualifierName = qualifierTreeView.SelectedNode.Text;
						string functionType = lbxFunctions.SelectedItem.Text;
						
						lblDirections.Text = "<p>The agents pertaining to qualifier \"" + 	qualifierName
							+ "\" and function \"" + functionType
							+ "\" are color-coded in the users/groups tree. "
							+ "<font color=\"red\">Red</font> denotes explicit agents. "
							+ "<font color=\"green\">Green</font> denotes implicit agents. </p>";

						userTreeView.SelectedNode.Selected = false;
						this.CollapseTree(userTreeView);

						// need to get all the implicit and explicit grants associated with this qualifierID and functionType and render the tree accordingly
						ArrayList explicitAgents = new ArrayList();
						ArrayList implicitAgentIDs = new ArrayList();
						ArrayList implicitAgents = new ArrayList();

						int[] explicitGrantIDs = wrapper.FindGrantsWrapper(-1,functionType,qualifierID);
						Grant[] explicitGrants = wrapper.GetGrantsWrapper(explicitGrantIDs);

						foreach(Grant g in explicitGrants)
						{
							explicitAgents.Add(g.agentID.ToString());
							AuthorizationUtilities.GetGroupDescendants(g.agentID, implicitAgentIDs);
						}

						//convert to a string array - this is necessary to make the color-coding work
						foreach (int agentID in implicitAgentIDs)
						{
							implicitAgents.Add(agentID.ToString());
						}

						RenderTreeWithColor(explicitAgents, implicitAgents,userTreeView.Nodes);
					}
				}
			}
		}

        
        // This method collapses all the open nodes in the entire TreeView. 
        private void CollapseTree(TreeView tv)
        {
            TreeNodeCollection children = tv.Nodes;
            foreach (TreeNode child in children)
            {
                if (child.Expanded == true)
                {
                    child.Expanded = false;
                }
                if (child.ChildNodes.Count > 0)
                    this.CollapseTree(child);
            }
        }

		//precede this method with SomeTreeView.SelectNode
		// This method collapses all the open nodes in the tree starting at the specified node. 
		private void CollapseTree(TreeNode n)
		{
			TreeNodeCollection children = n.ChildNodes;
			foreach(TreeNode child in children)
			{
				if(child.Expanded == true)
				{
					child.Expanded = false;
				}
				if(child.ChildNodes.Count > 0)
					this.CollapseTree(child);
			}
		}

		//This method highlights a tree according to the specified grant color scheme
		//NOTE: the arraylists must contain string versions of the int ids, 
		// or the contains won't work properly since renderNode.ID is a string
		private void RenderTreeWithColor(ArrayList explicitList, ArrayList implicitList, TreeNodeCollection renderNodes)
		{
			foreach(TreeNode renderNode in renderNodes)
			{
				// If explicit grant then highlight in red
				if(explicitList.Contains(renderNode.Value))
				{
					renderNode.Text = "<font color=red>" + renderNode.Text + "</font>";
					TreeNode parent = (TreeNode) renderNode.Parent;
					parent.Expanded=true;
					//Changing default style doesn't work
					//renderNode.DefaultStyle.AppendCssText("font-color:red");
				}

				// If implicit grant then highlight in green
				if(implicitList.Contains(renderNode.Value))
				{
					renderNode.Text = "<font color=green>" + renderNode.Text + "</font>";
					TreeNode parent = (TreeNode) renderNode.Parent;
					parent.Expanded=true;
				}

				// This recursively steps into each node's children
				this.RenderTreeWithColor (explicitList, implicitList,renderNode.ChildNodes);
			}
		}
	}
}
