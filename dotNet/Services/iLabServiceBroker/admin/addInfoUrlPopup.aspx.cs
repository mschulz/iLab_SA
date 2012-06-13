/*
 * Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
 * Please see license.txt in top level directory for full license.
 */

/* $Id$ */

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using iLabs.ServiceBroker.Administration;
using iLabs.ServiceBroker.Authorization;
using iLabs.UtilLib;


namespace iLabs.ServiceBroker.admin
{
	/// <summary>
	/// Summary description for addInfoUrlPopup.
	/// </summary>
	public partial class addInfoUrlPopup : System.Web.UI.Page
	{
		int labClientID;
		string clientName;
		//ClientInfo[] clientInfos;
		List<ClientInfo> clientInfoList;
        ClientInfo currentInfo = null;

        //AuthorizationWrapperClass wrapper = new AuthorizationWrapperClass();
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (Session["UserID"]==null)
				Response.Redirect("../login.aspx");

			labClientID = int.Parse(Request.Params["lc"]);
            ClientInfo[] clientInfos = AdministrativeAPI.ListClientInfos(labClientID);
            clientInfoList = new List<ClientInfo>();
            
            if (clientInfos != null && clientInfos.Length > 0)
                clientInfoList.AddRange(clientInfos);
            //clientName = AdministrativeAPI.GetLabClientName(labClientID);

            // Add the JavaScript code to the page.
            if (!ClientScript.IsClientScriptBlockRegistered("ValueChanged"))
            {
                StringBuilder jsBuf = new StringBuilder();
                jsBuf.AppendLine("<script> function ValueChanged() {");
                //jsBuf.AppendLine("debugger");
                jsBuf.AppendLine("document.getElementById('btnSaveInfoChanges').disabled = false; ");
                //jsBuf.AppendLine("return false;");
                jsBuf.AppendLine("}</script>");
                ClientScript.RegisterClientScriptBlock(this.GetType(), "ValueChanged", jsBuf.ToString());
            }
			if(!IsPostBack)
			{
                clearMessage();
                hdnClientInfoID.Value = "0";
                lblLabClient.Text = AdministrativeAPI.GetLabClientName(labClientID);
                RefreshClientInfoRepeater();
				ClearFormFields();
				LoadListBox();
                
               
			}
            txtInfoname.Attributes.Add("onkeypress", "ValueChanged();");
            txtUrl.Attributes.Add("onkeypress", "ValueChanged();");
            txtDesc.Attributes.Add("onkeypress", "ValueChanged();");
			

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
			this.ibtnMoveUp.Click += new System.Web.UI.ImageClickEventHandler(this.ibtnMoveUp_Click);
			this.ibtnMoveDown.Click += new System.Web.UI.ImageClickEventHandler(this.ibtnMoveDown_Click);
			this.repClientInfo.ItemDataBound += new System.Web.UI.WebControls.RepeaterItemEventHandler(this.repClientInfo_ItemDataBound);
			this.repClientInfo.ItemCommand += new System.Web.UI.WebControls.RepeaterCommandEventHandler(this.repClientInfo_ItemCommand);

		}
		#endregion

		private void RefreshClientInfoRepeater()
		{
			
			// refresh the array of LabClient objects from the database.
			// This insures that any changed ClientInfo arrays (one per LabClient Object)
			// are retrieved.

            ClientInfo[] clientInfos = AdministrativeAPI.ListClientInfos(labClientID);
            if (clientInfoList == null)
            {
                clientInfoList = new List<ClientInfo>();
            }
            else
                clientInfoList.Clear();
            if (clientInfos != null && clientInfos.Length > 0)
                clientInfoList.AddRange(clientInfos);
           
            repClientInfo.DataSource = clientInfoList;
			repClientInfo.DataBind();

		}

		/// <summary>
		/// ItemCommand event for the ClientInfo Repeater.
		/// Fires when the Edit or Remove buttons are clicked.
		/// </summary>
		/// <param name="source"></param>
		/// <param name="e"></param>
		private void repClientInfo_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
		{
            clearMessage();
			int clientInfoID = Convert.ToInt32(e.CommandArgument);
			
			if(e.CommandName == "Edit"){

                LoadFormFields(clientInfoID);
			}
			else if(e.CommandName == "Remove")
			{
				RemoveClientInfo(clientInfoID);
                RefreshClientInfoRepeater();
                ClearFormFields();
			}
		}

        private void LoadFormFields(int infoID)
        {
            int index = 0;
            bool found = false;
            for (index = 0; index < clientInfoList.Count; index++)
            {
                if ((clientInfoList[index].clientInfoID == infoID))
                {
                    LoadFormFields(clientInfoList[index]);
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                ClearFormFields();
            }
        }

		/// <summary>
		/// Loads the fields in the Info Edit Box with information
		/// from the selected record, to prepare for editing.
		/// </summary>
		private void LoadFormFields(ClientInfo clientInfo)
		{
			txtDesc.Text = clientInfo.description;
			txtUrl.Text = clientInfo.infoURL;
			txtInfoname.Text = clientInfo.infoURLName;
			hdnClientInfoID.Value = clientInfo.ClientInfoID.ToString();
            hdnDisplayOrder.Value = clientInfo.displayOrder.ToString();
            btnSaveInfoChanges.Enabled = false;
		}

		/// <summary>
		/// Clears the fields in the Info Edit Box on the right
		/// to prepare for the creation of a new record.
		/// </summary>
		private void ClearFormFields()
		{
			txtInfoname.Text = "";
			txtUrl.Text = "";
			txtDesc.Text = "";
			// keeps track of current clientInfos index. New record should
			// have and index that is one higher than the highest current index.
			// Array Length property is not zero-based, hence equality here.
			hdnClientInfoID.Value = "0";
            btnSaveInfoChanges.Enabled = false;
            
		}
        protected void clearMessage()
        {
            lblResponse.Visible = false;
            lblResponse.Text = "";
        }

		protected void btnNew_Click(object sender, System.EventArgs e)
		{
            clearMessage();
			ClearFormFields();
            btnSaveInfoChanges.Enabled = true;
		}

		protected void btnSaveInfoChanges_Click(object sender, System.EventArgs e)
		{
            clearMessage();
            if (txtUrl.Text == null || txtUrl.Text == String.Empty || txtInfoname.Text == null || txtInfoname.Text == String.Empty)
            {
                lblResponse.Text = Utilities.FormatErrorMessage("You must supply both a name and URL for the info item!");
                lblResponse.Visible = true;
                return;
            }
			int id = int.Parse(hdnClientInfoID.Value);
			
		// Set the correct element of the clientInfos array to the values from the text boxes
            if (id == 0)
			{

                hdnDisplayOrder.Value = clientInfoList.Count.ToString();
                id = AdministrativeAPI.InsertLabClientInfo(labClientID, txtUrl.Text, txtInfoname.Text, txtDesc.Text, clientInfoList.Count);
                hdnClientInfoID.Value = id.ToString();
                RefreshClientInfoRepeater();
                LoadListBox();
                btnSaveInfoChanges.Enabled = false;
                //btnSaveOrderChanges.Enabled = true;
				
			}
			else
			{
                int order = int.Parse(hdnDisplayOrder.Value);
                AdministrativeAPI.UpdateLabClientInfo(id, labClientID, txtUrl.Text, txtInfoname.Text, txtDesc.Text, order);
                RefreshClientInfoRepeater();
                LoadListBox();
				
			}

			//Save the LabClient object with the updated clientInfos array
            //UpdateLabClientInfo(clientInfoList);
		
		}

		private void RemoveClientInfo(int infoID)
		{
            int count = AdministrativeAPI.DeleteLabClientInfo(labClientID, infoID);
            if (count > 0)
            {
                RefreshClientInfoRepeater();
                LoadListBox();
                btnSaveOrderChanges.Enabled = true;
            }

		}

		/// <summary>
		/// Databound event for the repClientInfo Repeater.
		/// You have to initialize any javascript for the buttons in the 
		/// Databound event.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void repClientInfo_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
			{
				Button remBtn = (Button) e.Item.FindControl("btnRemove");
				// "Are you sure" javascript for Remove button
				string script = "javascript:if(confirm('Are you sure you want to remove this Client Resource?')== false) return false;";
				remBtn.Attributes.Add("onClick",script);
				remBtn.CommandArgument = ((ClientInfo)e.Item.DataItem).clientInfoID.ToString();

				//Adding comman arguments to edit button
				Button editBtn = (Button) e.Item.FindControl("btnEdit");
				editBtn.CommandArgument = ((ClientInfo)e.Item.DataItem).clientInfoID.ToString();
			}

		}
		
		private void LoadListBox()
		{
            if (clientInfoList != null && clientInfoList.Count > 0)
            {
                lbxChangeOrder.Items.Clear();
                for (int i = 0; i < clientInfoList.Count; i++)
                {
                    if (clientInfoList[i] != null)
                    {
                        if ((clientInfoList[i].infoURLName != null) && (clientInfoList[i].infoURLName.Length > 0))
                        {
                            StringBuilder buf = new StringBuilder(clientInfoList[i].infoURLName);
                            if (clientInfoList[i].description != null && clientInfoList[i].description.Length > 0)
                                buf.Append(" - " + clientInfoList[i].description);
                            lbxChangeOrder.Items.Add(new ListItem(buf.ToString(), clientInfoList[i].clientInfoID.ToString()));
                        }
                    }
                }
            }
            btnSaveOrderChanges.Enabled = false;
		}

		/// <summary>
		/// Moves an item in the ListBox up one position
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ibtnMoveUp_Click(object sender, System.Web.UI.ImageClickEventArgs e)
		{
			int i;
			if ( (i = lbxChangeOrder.SelectedIndex) >0 ) 
			{
				ListItem li1 = lbxChangeOrder.Items[i];
                lbxChangeOrder.Items.RemoveAt(i);
				lbxChangeOrder.Items.Insert(i-1,li1);
                btnSaveOrderChanges.Enabled = true;
			}
		}

		/// <summary>
		/// Moves an item in the ListBox down one position.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ibtnMoveDown_Click(object sender, System.Web.UI.ImageClickEventArgs e)
		{
			int i=lbxChangeOrder.SelectedIndex;
			if (i >= 0 && i < lbxChangeOrder.Items.Count-1) 
			{
                ListItem li1 = lbxChangeOrder.Items[i];
                lbxChangeOrder.Items.RemoveAt(i);
                lbxChangeOrder.Items.Insert(i + 1, li1);
                btnSaveOrderChanges.Enabled = true;
			}
		}

		/// <summary>
		/// Saves the LabClient with the ClienInfo array rewritten into a new order.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void btnSaveOrderChanges_Click(object sender, System.EventArgs e)
		{
			int[] infoIDs = new int[clientInfoList.Count];
			for (int i=0; i < infoIDs.Length; i++)
			{
				//Take the infoID out of the value in the list box, where it has been preserved.
                infoIDs[i] = int.Parse(lbxChangeOrder.Items[i].Value);
			}
			//Save the LabClient object with the updated clientInfos array
            AdministrativeAPI.UpdateLabClientInfoOrder(infoIDs);
            RefreshClientInfoRepeater();
            btnSaveOrderChanges.Enabled = false;
		}

		/// <summary>
		/// This is the main database update routine.
		/// In order to update the ClientInfo structure in the Lab Client, it is necessary to 
		/// update the entire lab client.
		/// </summary>
		/// <param name="clientInfos"></param>
		private void UpdateLabClientInfo(ClientInfo[] clientInfos)
		{
			try
			{
                int count = 0;
                foreach (ClientInfo info in clientInfos)
                {
                    AdministrativeAPI.UpdateLabClientInfo(info.clientInfoID, info.clientID, info.infoURL, info.infoURLName, info.description, count);
                    count++;
                }
				// Create the javascript which will cause a page refresh event to fire on the popup's parent page
				string jScript;
				jScript = "<script language=javascript> window.opener.Form1.hiddenPopupOnSave.value='1';";
				// jScript += "window.close();";
				jScript += "</script>";
				Page.RegisterClientScriptBlock("postbackScript", jScript);
			}
			catch (Exception ex)
			{
				lblResponse.Visible = true;
				lblResponse.Text = "Cannot update Lab Client. " + ex.Message;
			}

			RefreshClientInfoRepeater();
			LoadListBox();
		}

	}
}
