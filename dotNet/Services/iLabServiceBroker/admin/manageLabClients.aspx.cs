/*
 * Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
 * Please see license.txt in top level directory for full license.
 * 
 * $Id$
 */
using System;
using System.Configuration;
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

using iLabs.Core;
using iLabs.DataTypes;
using iLabs.DataTypes.ProcessAgentTypes;
using iLabs.DataTypes.SoapHeaderTypes;
using iLabs.DataTypes.TicketingTypes;
using iLabs.Proxies.LSS;
using iLabs.Proxies.USS;
using iLabs.ServiceBroker;
using iLabs.ServiceBroker.Internal;
using iLabs.ServiceBroker.Administration;
using iLabs.ServiceBroker.Authorization;
using iLabs.ServiceBroker.iLabSB;
using iLabs.ServiceBroker.Mapping;
using iLabs.Ticketing;

using iLabs.UtilLib;
//using iLabs.Services;

namespace iLabs.ServiceBroker.admin
{
    /// <summary>
    /// Summary description for manageLabClients.
    /// </summary>
    public partial class manageLabClients : System.Web.UI.Page
    {
        AuthorizationWrapperClass wrapper = new AuthorizationWrapperClass();

        private Color disabled = Color.FromArgb(243, 239, 229);
        private Color enabled = Color.White;
        int labClientID;
        LabClient labClient;
        int[] assocGroupIDs = null;
        string zero = "0";

        protected BrokerDB ticketing;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            ticketing = new BrokerDB();
            if (Session["UserID"] == null)
                Response.Redirect("../login.aspx");

            //only superusers can view this page
            if (!Session["GroupName"].ToString().Equals(Group.SUPERUSER))
                Response.Redirect("../home.aspx");



            // Set the popup buttons' "CausesValidation" property.
            // For any button that is related to a popup, you have to set the CausesValidation
            // propery to false, otherwise
            // any RequiredFieldValidator control will cause the onclick event to be
            // comandeered by a routine in ASP.NET's WebValidationUI.js file.
            // The result would be that the custom onclick for the popup would not fire.

            //btnEditList.CausesValidation = false;
            btnAddEditResources.CausesValidation = false;
            btnRemove.CausesValidation = false;
            btnMetadata.CausesValidation = false;

            // This button enables the popup to fire an event on the caller when the Save button is hit.
            btnRefresh.CausesValidation = false;

            // "Are you sure" javascript for Remove button
            StringBuilder jScript = new StringBuilder();
            jScript.Append( "javascript:if(confirm('Are you sure you want to remove this Lab Client? ");
            jScript.Append(" Removing the client will also delete all past experiments  and clientItems!");
            jScript.Append(" Deleting a ClientID should only be done prior to any user experiments being run.");
            jScript.Append( "')== false) return false;");
            btnRemove.Attributes.Add("onclick",jScript.ToString());

            // This is a hidden input tag. The associatedLabServers popup changes its value using a window.opener call in javascript,
            // then the GetPostBackEventReference fires the event associated with the btnRefresh button.
            // The result is that the LabServer repeater (repLabServers) is refreshed when the Save button is clicked
            // on the popup.
            hiddenPopupOnSave.Attributes.Add("onpropertychange", Page.GetPostBackEventReference(btnRefresh));

            if (!Page.IsPostBack)
            {
                hdnEssID.Value = zero;
                hdnNeedsEss.Value = zero;
                hdnLabServerID.Value = zero;
                hdnUssID.Value = zero;
                hdnNeedsUss.Value = zero;

                // Load Lab Client dropdown
                InitializeClientDropDown();

                //Put in client types
                ddlClientTypes.Items.Add(new ListItem("--- Select a Client Type ---", zero));
                string[] clientTypes = InternalAdminDB.SelectLabClientTypes();
                foreach (string cType in clientTypes)
                {
                    ddlClientTypes.Items.Add(new ListItem(cType));
                }

                ddlLabServer.Items.Add(new ListItem("--- Select a Lab Server ---", zero));
                IntTag[] lsTags = ticketing.GetProcessAgentTagsByType(new int[] { (int)ProcessAgentType.AgentType.BATCH_LAB_SERVER, (int)ProcessAgentType.AgentType.LAB_SERVER });
                foreach (IntTag ls in lsTags)
                {
                    ListItem li = new ListItem(ls.tag, ls.id.ToString());
                    ddlLabServer.Items.Add(li);
                }

                //Put in availabe USS
                ListItem liHeaderUss = new ListItem("---Select User Side Scheduling Server---", zero);
                ddlAssociatedUSS.Items.Add(liHeaderUss);
                IntTag[] usses = ticketing.GetProcessAgentTagsByType(ProcessAgentType.SCHEDULING_SERVER, ProcessAgentDB.ServiceGuid);
                foreach (IntTag uss in usses)
                {
                    ListItem li = new ListItem(uss.tag, uss.id.ToString());
                    ddlAssociatedUSS.Items.Add(li);
                }
                //Put in availabe ESS
                ListItem liHeaderEss = new ListItem("---Select Experiment Storage Server---", "0");
                ddlAssociatedESS.Items.Add(liHeaderEss);
                IntTag[] esses = ticketing.GetProcessAgentTagsByType(ProcessAgentType.EXPERIMENT_STORAGE_SERVER, ProcessAgentDB.ServiceGuid);
                foreach (IntTag ess in esses)
                {
                    ListItem li = new ListItem(ess.tag, ess.id.ToString());
                    ddlAssociatedESS.Items.Add(li);
                }


                // Disable the "Edit Lab Servers" button at first
                //btnEditList.Visible = false;
                txtLabServer.ReadOnly = true;
                txtLabServer.BackColor = disabled;
                txtLsUrl.ReadOnly = true;
                txtLsUrl.BackColor = disabled;
                txtAssociatedESS.ReadOnly = true;
                txtAssociatedESS.BackColor = disabled;
                txtAssociatedUSS.ReadOnly = true;
                txtAssociatedUSS.BackColor = disabled;
                ClearFormFields();
                // Reset error/confirmation message
                lblResponse.Text = "";
                lblResponse.Visible = false;

            }
            else if (Request.Params["refresh"] != null)
            {
                string tst = Request.Params["refresh"];
                int cid = Int32.Parse(Request.Params["refresh"]);
                ListItem theItem = ddlLabClient.Items.FindByValue(cid.ToString());
                if (theItem != null)
                {
                    theItem.Selected = true;
                    LoadFormFields();
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

        protected bool hasGroups()
        {
            return (assocGroupIDs != null && assocGroupIDs.Length > 0);
        }

        /// <summary>
        /// Clears the Lab Client dropdown and reloads it from the array of LabClient objects
        /// </summary>
        private void InitializeClientDropDown()
        {
            IntTag[] clientTags = AdministrativeAPI.GetLabClientTags();
            // Load Lab Client dropdown
            ddlLabClient.Items.Clear();
            ddlLabClient.Items.Add(new ListItem(" --- select Lab Client --- ", zero));

            foreach (IntTag lc in clientTags)
            {
                ddlLabClient.Items.Add(new ListItem(lc.tag, lc.id.ToString()));
            }
        }

        /// <summary>
        /// This method clears the form fields.
        /// </summary>
        private void ClearFormFields()
        {
            hdnEssID.Value = zero;
            hdnNeedsEss.Value = zero;
            hdnLabServerID.Value = zero;
            hdnUssID.Value = zero;
            hdnNeedsUss.Value = zero;
            assocGroupIDs = null;
            txtLabClientName.Text = "";
            txtVersion.Text = "";
            txtShortDesc.Text = "";
            txtLongDesc.Text = "";
            txtContactFirstName.Text = "";
            txtContactLastName.Text = "";
            txtContactEmail.Text = "";
            txtDocURL.Text = "";
            txtNotes.Text = "";
            txtLoaderScript.Text = "";
            //Disable guid modification
            txtClientGuid.Text = "";
            txtClientGuid.ReadOnly = false;
            txtClientGuid.BackColor = enabled;
            btnGuid.Visible = true;

            ddlClientTypes.SelectedIndex = 0;
            cbxIsReentrant.Checked = false;

            ddlLabServer.SelectedIndex = 0;
            ddlLabServer.Visible = true;
            txtLabServer.Visible = false;
            txtLabServer.Text = "";
            btnRegisterLS.Visible = false;
            txtLsUrl.Text = "";

            cbxESS.Checked = true;
            ddlAssociatedESS.SelectedIndex = 0;
            ddlAssociatedESS.Visible = true;
            txtAssociatedESS.Visible = false;
            txtAssociatedESS.Text = "";
            btnRegisterESS.Visible = false;

            cbxScheduling.Checked = true;
            ddlAssociatedUSS.SelectedIndex = 0;
            ddlAssociatedUSS.Visible = true;
            txtAssociatedUSS.Visible = false;
            txtAssociatedUSS.Text = "";
            btnRegisterUSS.Visible = false;

            trOptions.Visible = false;
            Session.Remove("ESSmapID");
            Session.Remove("USSmapID");
        }

        /// <summary>
        /// This method loads the text fields on the form from an array of
        /// LabClient objects loaded from the database
        /// </summary>
        private void LoadFormFields()
        {
            ClearFormFields();
            //int labServerID = 0;
            // load the LabClient based on the ddlLabClient.SelectedValue
            labClientID = Convert.ToInt32(ddlLabClient.SelectedValue);
            if (labClientID > 0)
            {
                labClient = AdministrativeAPI.GetLabClient(labClientID);
                if (labClient != null)
                {
                    assocGroupIDs = AdministrativeUtilities.GetLabClientGroups(labClient.clientID, false);
                    txtClientGuid.Text = labClient.clientGuid;

                    if(labClient.clientName != null)
                        txtLabClientName.Text = labClient.clientName;
                    txtVersion.Text = labClient.version;
                    txtShortDesc.Text = labClient.clientShortDescription;
                    txtLongDesc.Text = labClient.clientLongDescription;
                    txtDocURL.Text = labClient.documentationURL;
                    txtContactFirstName.Text = labClient.contactFirstName;
                    txtContactLastName.Text = labClient.contactLastName;
                    txtContactEmail.Text = labClient.contactEmail;

                    cbxIsReentrant.Checked = labClient.IsReentrant;
                    if (labClient.clientType != null && labClient.clientType.Length > 0)
                        ddlClientTypes.SelectedValue = labClient.clientType;
                    txtNotes.Text = labClient.notes;
                    txtLoaderScript.Text = labClient.loaderScript;
                    cbxESS.Checked = labClient.needsESS;
                    hdnNeedsEss.Value = labClient.needsESS ? "1" : zero;
                    cbxScheduling.Checked = labClient.needsScheduling;
                    hdnNeedsUss.Value = labClient.needsScheduling ? "1" : zero;
                    //Check if there is an associated USS and/or ESS => lab experiment needs scheduling and/or storage
                    CheckAssociatedResources(labClient);

                    // NOTE: This code is not currently used, but may return once support for multiple labServers per client is implemented.
                    // Load Lab Server Repeater
                    // RefreshLabServerRepeater();

                    // Load ClientInfo Repeater
                    //RefreshClientInfoRepeater();

                    //// Associated LabServersPopup is currently not used as only one LabServer per client is supported.
                    //// Initialize and load the "Edit Associated Lab Servers" button's 
                    //// javascript onclick routine with the correct Lab Client ID in the querystring
                    //string assocPopupScript;
                    //assocPopupScript = "javascript:window.open('assocLabServersPopup.aspx?lc=";
                    //assocPopupScript += labClient.clientID.ToString();
                    //assocPopupScript += "','managelabclients','scrollbars=yes,resizable=yes,width=700,height=400').focus()";
                    //btnEditList.Attributes.Remove("onClick");
                    //btnEditList.Attributes.Add("onClick", assocPopupScript);
                    // End: LabServer Popup code

                    // Initialize and load the "Edit Client Resources" button's 
                    // javascript onclick routine with the correct Lab Client ID in the querystring
                    StringBuilder infoPopupScript = new StringBuilder("javascript:window.open('addInfoURLPopup.aspx?lc=");
                    infoPopupScript.Append(labClient.clientID);
                    infoPopupScript.Append("','manageclientinfo','scrollbars=yes,resizable=yes,width=800').focus()");
                    btnAddEditResources.Attributes.Remove("onClick");
                    btnAddEditResources.Attributes.Add("onClick", infoPopupScript.ToString());

                    // Initialize and load the "Edit Associated groups" button's 
                    // javascript onclick routine with the correct Lab Client ID in the querystring
                    StringBuilder groupsPopupScript = new StringBuilder("javascript:window.open('manageLabGroups.aspx?lc=");
                    groupsPopupScript.Append(labClient.clientID);
                    groupsPopupScript.Append("','manageLabGroups','scrollbars=yes,resizable=yes,width=800').focus()");
                    btnAssociateGroups.Attributes.Remove("onClick");
                    btnAssociateGroups.Attributes.Add("onClick", groupsPopupScript.ToString());

                    //// Initialize and load the "Edit Metadata" button's 
                    // No Longer a popup see btnMetadata_Click
                    //// javascript onclick routine with the correct Lab Client ID in the querystring
                    //StringBuilder metadataPopupScript = new StringBuilder("javascript:window.open('editMetadataPopup.aspx?lc=");
                    //metadataPopupScript.Append(labClient.clientID);
                    //metadataPopupScript.Append("','editMetadataPopup','scrollbars=yes,resizable=yes,width=800').focus()");
                    //btnMetadata.Attributes.Remove("onClick");
                    //btnMetadata.Attributes.Add("onClick", metadataPopupScript.ToString());

                    trOptions.Visible = true;
                }
            }
        }

        //Checks whether there are a USS and/or an ESS associated with the selected client
        private void CheckAssociatedResources(LabClient client)
        {
            if (client == null)
                return;
            int labServerID = 0;
            int lssId = 0;
            int ussId = 0;
            int essId = 0;
           

            //cbxScheduling.Checked = client.needsScheduling;
            //cbxESS.Checked = client.needsESS;

            ProcessAgentInfo[] labServers = AdministrativeAPI.GetLabServersForClient(client.clientID);
            if (labServers != null && labServers.Length > 0 && labServers[0].agentId > 0)
            {
                labServerID = labServers[0].agentId;
                if (labServerID > 0)
                {
                    lssId = ResourceMapManager.FindResourceProcessAgentID(ResourceMappingTypes.PROCESS_AGENT,
                        labServerID, ProcessAgentType.LAB_SCHEDULING_SERVER);
                }
            }
            hdnLabServerID.Value = labServerID.ToString();
            ddlLabServer.SelectedValue = hdnLabServerID.Value;
            ussId = ResourceMapManager.FindResourceProcessAgentID(ResourceMappingTypes.CLIENT,
                   client.clientID, ProcessAgentType.SCHEDULING_SERVER);
            ddlAssociatedUSS.SelectedValue = ussId.ToString();
            hdnUssID.Value = ussId.ToString();
            essId = ResourceMapManager.FindResourceProcessAgentID(ResourceMappingTypes.CLIENT, client.clientID,
                    ProcessAgentType.EXPERIMENT_STORAGE_SERVER);
            ddlAssociatedESS.SelectedValue = essId.ToString();
            hdnEssID.Value = essId.ToString();
            if (client.needsESS)
            {
                btnRegisterESS.Visible = true;
                
                if (essId > 0)
                {
                    btnRegisterESS.Text = "Dissociate";
                    ddlAssociatedESS.Visible = false;
                    txtAssociatedESS.Visible = true;
                    txtAssociatedESS.Text = ddlAssociatedESS.SelectedItem.Text;
                }
                else
                {

                    btnRegisterESS.Text = "Register";
                    //btnRegisterESS.Visible = true;
                    ddlAssociatedESS.Visible = true;
                    txtAssociatedESS.Visible = false;
                }
            }
            else // ESS Not needed
            {
                btnRegisterESS.Visible = false;
                ddlAssociatedESS.Visible = true;
                txtAssociatedESS.Visible = false;
            }

            if (labServerID  > 0)
            {
                ddlLabServer.Visible = false;
                txtLabServer.Visible = true;
                txtLabServer.Text = ddlLabServer.SelectedItem.Text;
                btnRegisterLS.Text = "Dissociate";
                txtLsUrl.Text = labServers[0].webServiceUrl;
            }
            else
            {
                ddlLabServer.Visible = true;
                txtLabServer.Visible = false;
                btnRegisterLS.Text = "Register";
            }
            btnRegisterLS.Visible = true;
            if (client.needsScheduling)
            {
               
                btnRegisterUSS.Visible = true;
                if (ussId > 0)
                {
                    btnRegisterUSS.Text = "Dissociate";
                    ddlAssociatedUSS.Visible = false;
                    txtAssociatedUSS.Visible = true;
                    txtAssociatedUSS.Text = ddlAssociatedUSS.SelectedItem.Text;
                }
                else
                {
                    if (Convert.ToInt32(ddlLabServer.SelectedValue) > 0)
                    {
                        btnRegisterUSS.Text = "Register";
                        ddlAssociatedUSS.Visible = true;
                        txtAssociatedUSS.Visible = false;

                    }
                }
            } // End needsScheduling
            else
            {
                ddlAssociatedUSS.Visible = true;
                txtAssociatedUSS.Visible = false;
                btnRegisterUSS.Text = "Register";
            }
           
            if (hasGroups())
            {
                btnRegisterLS.Enabled = false;
                btnRegisterLS.ToolTip = "You may not modify the LabServer while groups are associated with the client.";
                cbxScheduling.Enabled = false;
                btnRegisterUSS.Enabled = false;
                btnRegisterUSS.ToolTip = "You may not modify Scheduling while groups are associated with the client.";
                cbxScheduling.ToolTip = "You may not modify Scheduling while groups are associated with the client.";
            }
            else
            {
                btnRegisterLS.Enabled = true;
                btnRegisterLS.ToolTip = "";
                cbxScheduling.Enabled = true;
                btnRegisterUSS.Enabled = true;
                btnRegisterUSS.ToolTip = "";
                cbxScheduling.ToolTip = "";
            }

            if (labServerID == 0 && essId == 0 && ussId == 0 && !hasGroups())
            {
                txtClientGuid.ReadOnly = false;
                txtClientGuid.BackColor = enabled;
                btnGuid.Visible = true;
            }
            else
            {
                txtClientGuid.ReadOnly = true;
                txtClientGuid.BackColor = disabled;
                btnGuid.Visible = false;
            }
            // Check if we can assign groups, or if groups are assigned display button
            if (labServerID > 0)
            {
                if (!hasGroups() && client.needsScheduling)
                {
                    btnAssociateGroups.Visible = (lssId > 0 && ussId > 0) ? true : false;
                }
                else
                {
                    btnAssociateGroups.Visible = true;
                    btnAssociateGroups.Enabled = true;
                }
            }
            // for testing
            //btnAssociateGroups.Visible = true;
            //btnAssociateGroups.Enabled = true;
        }

        protected void ddlLabClient_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            lblResponse.Visible = false;
            lblResponse.Text = "";

            if (ddlLabClient.SelectedIndex == 0)
            {
                // prepare for a new record
                ClearFormFields();

                // Disable the button that pops up the Associated Lab Server edit page
                //btnEditList.Visible = false;			
            }
            else
            {
                // edit an existing record
                LoadFormFields();

                // Enable the button that pops up the Associated Lab Server edit page
                //btnEditList.Visible = true;			
            }
        }

        /// <summary>
        /// Clears the form in preparation for a new record
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNew_Click(object sender, System.EventArgs e)
        {
            lblResponse.Visible = false;
            lblResponse.Text = "";
            labClientID = 0;
            labClient = null;
            ddlLabClient.SelectedIndex = 0;
            //hdnEssID.Value = zero;
            //hdnLabServerID.Value = zero;
            //hdnUssID.Value = zero;
            ClearFormFields();
        }

        /// <summary>
        /// The Save Button method.
        /// The index of the Lab Client dropdown will be used to determine whether 
        /// this is a new (0) or an existing (>0) Lab Client.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSaveChanges_Click(object sender, System.EventArgs e)
        {
            lblResponse.Visible = false;
            lblResponse.Text = "";
            StringBuilder message = new StringBuilder();
            int status = 1;
            int result = 1;
            int labServerID = 0;
            int lssID = 0;
            if (txtVersion.Text == null || txtVersion.Text.Equals(""))
            {
                status = Math.Min(status, 0);
                message.AppendLine("You must specify a version for the client!<br/>");
            }
            if (txtClientGuid.Text == null || txtClientGuid.Text.Equals(""))
            {
                status = Math.Min(status, 0);
                message.AppendLine("You must specify a GUID for the client!<br/>");
            }
            if (txtClientGuid.Text.Length > 50)
            {
                status = Math.Min(status, 0);
                message.AppendLine("The GUID is too long, the maximun number of characters is 50!<br/>");
            }
            if (txtLoaderScript.Text == null || txtLoaderScript.Text.Equals(""))
            {
                status = Math.Min(status, 0);
                message.AppendLine("You must specify a loader script for the client!<br/>");
            }
            if (ddlClientTypes.SelectedIndex <= 0)
            {
                status = Math.Min(status, 0);
                message.AppendLine("You must specify a Client Type!<br/>");
            }
            if (status < 1)
            {
                lblResponse.Text = Utilities.FormatWarningMessage(message.ToString());
                lblResponse.Visible = true;
                return;
            }
            if (ddlLabClient.SelectedIndex == 0)
            {
                ///////////////////////////////////////////////////////////////
                /// ADD a new Lab Client                                     //
                ///////////////////////////////////////////////////////////////

                // Add the Lab Client
                try
                {
                    labClientID = wrapper.AddLabClientWrapper(txtClientGuid.Text, txtLabClientName.Text, txtVersion.Text,
                        txtShortDesc.Text, txtLongDesc.Text, ddlClientTypes.SelectedItem.Text, txtLoaderScript.Text, txtDocURL.Text,
                        txtContactEmail.Text, txtContactFirstName.Text, txtContactLastName.Text, txtNotes.Text,
                        cbxESS.Checked, cbxScheduling.Checked, cbxIsReentrant.Checked);
                    if (labClientID > 0)
                    {
                        labClient = AdministrativeAPI.GetLabClient(labClientID);
                        message.AppendLine("Registering Lab Client: " + labClient.clientName + ".<br/>");
                        InitializeClientDropDown();
                        ddlLabClient.SelectedValue = labClientID.ToString();
                    }
                    labServerID = Convert.ToInt32(ddlLabServer.SelectedValue);
                    if (labServerID > 0)
                    {
                        result = registerLS(labServerID, ref message);
                        if (result > 0)
                        {
                            hdnLabServerID.Value = labServerID.ToString();
                            message.AppendLine("Associating Lab Server: " + ddlLabServer.SelectedItem.Text + ".<br/>");
                            status = Math.Min(status, result);
                        }
                        else
                        {
                            message.AppendLine("Unable to associate Lab Server: " + ddlLabServer.SelectedItem.Text + ".<br/>");
                            status = Math.Min(status, 0);
                        }
                    }
                    if (cbxESS.Checked)
                    {
                        int essId = Convert.ToInt32(ddlAssociatedESS.SelectedValue);
                        if (essId > 0)
                        {
                            status = Math.Min(status, registerESS(essId, ref message));
                            message.AppendLine("Associating ESS: " + ddlAssociatedESS.SelectedItem.Text + ".<br/>");
                            status = Math.Min(status, result);
                        }
                    }
                    if (cbxScheduling.Checked)
                    {
                        int ussId = Convert.ToInt32(ddlAssociatedUSS.SelectedValue);
                        if (ussId > 0)
                        {
                            if (labServerID > 0)
                            {
                                lssID = ResourceMapManager.FindResourceProcessAgentID(ResourceMappingTypes.PROCESS_AGENT,
                                labServerID, ProcessAgentType.LAB_SCHEDULING_SERVER);
                                if (lssID > 0)
                                {
                                    result = registerUSS(ussId, ref message);
                                    status = Math.Min(status, result);
                                    message.AppendLine("Associating USS: " + ddlAssociatedUSS.SelectedItem.Text + ".<br/>");
                                }
                                else
                                {
                                    message.AppendLine("You must assign a lab scheduling server before you may register a Scheduling Server!<br/>");
                                    status = Math.Min(status, 0);
                                }
                            }
                            else
                            {
                                message.AppendLine("You must assign a Lab Server and Lab Scheduling Server before you may register a User Scheduling Server!<br/>");
                                status = Math.Min(status, 0);
                            }
                        }
                    }
                }
                catch (AccessDeniedException ex)
                {
                    lblResponse.Visible = true;
                    message.AppendLine(ex.Message + ". " + ex.GetBaseException());
                    lblResponse.Text = Utilities.FormatErrorMessage(message.ToString());
                    return;
                }

                // If successful...
                if (labClientID > 0)
                {
                    lblResponse.Visible = true;
                    message.AppendLine("Lab Client " + txtLabClientName.Text + " has been added.");
                    lblResponse.Text = Utilities.FormatConfirmationMessage(message.ToString());
                    //// set dropdown to newly created Lab Client.
                    //InitializeClientDropDown();
                    //ddlLabClient.SelectedValue = labClientID.ToString();
                    // Prepare record for editing
                    LoadFormFields();
                }
                else // cannot create lab client
                {
                    lblResponse.Visible = true;
                    message.AppendLine("Cannot create Lab Client " + txtLabClientName.Text + ".");
                    lblResponse.Text = Utilities.FormatErrorMessage(message.ToString());
                }
                //Disable guid modification
                //txtClientGuid.ReadOnly = true;
                ////txtClientGuid.Enabled = false;
                // Enable the button that pops up the Associated Lab Server edit page
                //btnEditList.Visible = true;
            }
            else
            ///////////////////////////////////////////////////////////////
            /// MODIFY an existing Lab Client                            //
            /// Note: Modify only changes the primary attributes of      //
            /// the client, resourceMapped values use the butttons.      // 
            ///////////////////////////////////////////////////////////////
            {
                // Save the index
                string savedSelectedValue = ddlLabClient.SelectedValue;

                // obtain information not edited in the text boxes from the array of LabClient objects
                labClientID = Convert.ToInt32(ddlLabClient.SelectedValue);

                // Modify the Lab Client Record
                try
                {
                    //LabClient lc = labClients[ddlLabClient.SelectedIndex -1];

                    wrapper.ModifyLabClientWrapper(labClientID, txtClientGuid.Text, txtLabClientName.Text, txtVersion.Text,
                        txtShortDesc.Text, txtLongDesc.Text, ddlClientTypes.SelectedItem.Text,
                        txtLoaderScript.Text, txtDocURL.Text,
                        txtContactEmail.Text, txtContactFirstName.Text, txtContactLastName.Text, txtNotes.Text,
                        cbxESS.Checked, cbxScheduling.Checked, cbxIsReentrant.Checked);
                    labClient = AdministrativeAPI.GetLabClient(labClientID);
/***********
                    // Add support for Modified LabServer, Ess & Uss
                    int currentLS = Convert.ToInt32(hdnLabServerID.Value);
                    int lsId = Convert.ToInt32(ddlLabServer.SelectedValue);
                    if (currentLS != lsId)
                    {
                        if (currentLS > 0)
                        {
                            result = dissociateLS(currentLS, ref message);
                            status = Math.Min(status, result);
                        }
                        if (lsId > 0)
                        {
                            result = registerLS(lsId, ref message);
                            status = Math.Min(status, result);
                        }
                    }
                    int currentESS = Convert.ToInt32(hdnEssID.Value);
                    int essId = Convert.ToInt32(ddlAssociatedESS.SelectedValue);
                    if ((!labClient.needsESS && currentESS > 0) || (currentESS != essId))
                    {
                        if (currentESS > 0)
                        {
                            result = dissociateESS(currentESS, ref message);
                            status = Math.Min(status, result);
                        }
                    }
                    if (labClient.needsESS)
                    {
                        if (essId > 0)
                        {
                            result = registerESS(essId, ref message);
                            status = Math.Min(status, result);
                        }
                    }
                    int currentUSS = Convert.ToInt32(hdnUssID.Value);
                    int ussId = Convert.ToInt32(ddlAssociatedUSS.SelectedValue);
                    if ((!labClient.needsScheduling && currentUSS > 0) || (currentUSS != ussId))
                    {
                        if (currentUSS > 0)
                        {
                            result = dissociateUSS(currentUSS, ref message);
                            status = Math.Min(status, result);
                        }
                    }
                    if (labClient.needsScheduling)
                    {
                        if (ussId > 0)
                        {
                            result = registerUSS(ussId, ref message);
                            status = Math.Min(status, result);
                        }
                    }
 * ****************/

                    // Reload the Lab Client dropdown
                    InitializeClientDropDown();
                    ddlLabClient.SelectedValue = savedSelectedValue;
                    LoadFormFields();
                    //Disable guid modification
                    txtClientGuid.ReadOnly = true;
                    //txtClientGuid.Enabled = false;
                    lblResponse.Visible = true;
                    message.Insert(0, "Lab Client " + txtLabClientName.Text + " has been modified.<br/>");
                    if (status > 0)
                    {
                        lblResponse.Text = Utilities.FormatConfirmationMessage(message.ToString());
                    }
                    else if (status < 0)
                    {
                        lblResponse.Text = Utilities.FormatErrorMessage(message.ToString());
                    }
                    else
                    {
                        lblResponse.Text = Utilities.FormatWarningMessage(message.ToString());
                    }
                }
                catch (Exception ex)
                {
                    lblResponse.Visible = true;
                    lblResponse.Text = Utilities.FormatErrorMessage("Lab Client " + txtLabClientName.Text + " could not be modified." + ex.GetBaseException());
                }
            }

        }

        ///// <summary>
        ///// Creates an ArrayList of LabServer objects.
        ///// Binds the LabServer Repeater to this ArrayList.
        ///// </summary>
        //private void RefreshLabServerRepeater()
        //{
        //    ProcessAgentInfo[]labServers  = AdministrativeAPI.GetLabServersForClient(labClient.clientID);
        //    repLabServers.DataSource = labServers;
        //    repLabServers.DataBind();

        //}

        ///// <summary>
        ///// This is a hidden HTML button that is "clicked" by an event raised 
        ///// by the closing of the associatedLabServers popup.
        ///// It causes the Lab Servers repeater to be refreshed.
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void btnRefresh_ServerClick(object sender, System.EventArgs e)
        //{
        //    lblResponse.Visible = false;
        //    lblResponse.Text = "";

        //    RefreshLabServerRepeater();
        //    RefreshClientInfoRepeater();
        //    LoadFormFields();
        //}

        //private void RefreshClientInfoRepeater()
        //{
        //    // refresh the array of LabClient objects from the database.
        //    // This insures that any changed ClientInfo arrays (one per LabClient Object)
        //    // are retrieved.
        //    if (labClient != null && labClient.clientID > 0)
        //    {
        //        ClientInfo[] clientInfos = AdministrativeAPI.ListClientInfos(labClient.clientID);
        //        ArrayList clientInfosList = new ArrayList();

        //        foreach (ClientInfo ci in clientInfos)
        //        {
        //            clientInfosList.Add(ci);
        //        }

        //        repClientInfo.DataSource = clientInfosList;
        //        repClientInfo.DataBind();
        //        //Disable guid modification
        //        txtClientGuid.ReadOnly = true;
        //        txtClientGuid.Enabled = false;
        //    }
        //}

 
        /// <summary>
        /// This will remove the client from the database and any ResourceMapping. 
        /// Because of cascading deletes all clientInfos, User's past experiments, and the user's clientItems will also be deleted
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRemove_Click(object sender, System.EventArgs e)
        {
            lblResponse.Visible = false;
            lblResponse.Text = "";

            if (ddlLabClient.SelectedIndex == 0)
            {
                lblResponse.Visible = true;
                lblResponse.Text = Utilities.FormatWarningMessage("Please select a lab client from dropdown list to delete!");
                return;
            }
            else
            {
                labClientID = Convert.ToInt32(ddlLabClient.SelectedValue);
                labClient = AdministrativeAPI.GetLabClient(labClientID);
                if (hasGroups() && labClient.needsScheduling)
                {
                    lblResponse.Visible = true;
                    lblResponse.Text = Utilities.FormatWarningMessage("You may not delete a client that has groups assigned to it and needs scheduling! Please remove the grpoup(s) first.");
                    return;
                }
                StringBuilder message = new StringBuilder();
                int status = 1; 
                try
                {
                    int oldEssId = int.Parse(hdnEssID.Value);
                    if (oldEssId > 0)
                    {
                        status = Math.Min(status,dissociateESS(oldEssId, ref message));
                    }
                    int oldUssId = int.Parse(hdnUssID.Value);
                    if (oldUssId > 0)
                    {
                        status = Math.Min(status,dissociateUSS(oldUssId, ref message));
                    }
                    int oldLabServerId = int.Parse(hdnLabServerID.Value);
                    if (oldLabServerId > 0)
                    {
                        status = Math.Min(status,dissociateLS(oldLabServerId, ref message));
                    }
                    wrapper.RemoveLabClientsWrapper(new int[] { labClientID });
                    status = Math.Min(status, 1);
                    message.Append("Lab Client '" + txtLabClientName.Text + "' has been deleted");
                    lblResponse.Visible = true;
                    if(status < 1)
                        lblResponse.Text = Utilities.FormatWarningMessage(message.ToString());
                    else
                        lblResponse.Text = Utilities.FormatConfirmationMessage(message.ToString());
                    InitializeClientDropDown();
                    ClearFormFields();
                }
                catch (Exception ex)
                {
                    lblResponse.Visible = true;
                    lblResponse.Text = Utilities.FormatErrorMessage("Lab Client " + txtLabClientName.Text + "' cannot be deleted." + ex.GetBaseException());
                }
            }
        }

        protected void btnAddEditResources_Click(object sender, System.EventArgs e)
        {

        }

        protected void btnMetadata_Click(object sender, EventArgs e)
        {
            bool requireSSL = Convert.ToBoolean(ConfigurationManager.AppSettings["haveSSL"]);
            string URL = "";
            string target = "admin/clientMetadata.aspx?lc=" + Convert.ToInt32(ddlLabClient.SelectedValue);
            if (requireSSL)
                URL = Global.FormatSecureURL(Request, target);
            else
                URL = Global.FormatRegularURL(Request, target);
            Response.Redirect(URL);
        }

        protected void btnRegisterLS_Click(object sender, EventArgs e)
        {
            int status = 1;
            int result = 1;
            labClientID = Convert.ToInt32(ddlLabClient.SelectedValue);
            labClient = AdministrativeAPI.GetLabClient(labClientID);
            StringBuilder message = new StringBuilder();
            lblResponse.Visible = false;
            lblResponse.Text = "";
            int oldId = int.Parse(hdnLabServerID.Value);
            int lsId = int.Parse(ddlLabServer.SelectedValue);
            if (btnRegisterLS.Text.CompareTo("Register") == 0)
            {
                if (oldId != lsId)
                {
                    if (oldId > 0)
                    {
                        result = dissociateLS(oldId, ref message);
                        status = Math.Min(status, result);
                    }
                    result = registerLS(lsId, ref message);
                    status = Math.Min(status, result);
                }
            }
            else if (btnRegisterLS.Text.CompareTo("Dissociate") == 0)
            {
                result = dissociateLS(oldId, ref message);
                status = Math.Min(status, result);
            }
            if (message.Length > 0)
            {
                lblResponse.Visible = true;
                if (status > 0)
                {
                    lblResponse.Text = Utilities.FormatConfirmationMessage(message.ToString());
                }
                else if (status < 0)
                {
                    lblResponse.Text = Utilities.FormatErrorMessage(message.ToString());
                }
                else
                {
                    lblResponse.Text = Utilities.FormatWarningMessage(message.ToString());
                }
            }
            LoadFormFields();
        }

        protected int registerLS(int serverId, ref StringBuilder message)
        {
            int status = 1;
            bool registered = false;
            labClientID = Convert.ToInt32(ddlLabClient.SelectedValue);
            int labServerID = Convert.ToInt32(ddlLabServer.SelectedValue);
            List<int> assocLabServers = new List<int>(AdministrativeAPI.GetLabServerIDsForClient(labClientID));
            if (assocLabServers != null && assocLabServers.Count > 0)
            {
                // Check that only one LabServer is specified and it is not the specified one
                if (assocLabServers.Contains(labServerID))  // Target LabServer already associated
                {
                    assocLabServers.Remove(labServerID);
                    registered = true;
                }
                // Only one currently
                if (assocLabServers.Count > 0)
                {
                    foreach (int ls in assocLabServers)
                    {
                        status = Math.Min(status, dissociateLS(ls, ref message));
                    }
                }
            }
            if (!registered)
            {
                // set the server/client relation
                AdministrativeAPI.LabServerClient_Insert(labServerID, labClientID, 0);

                //1. find groups that can currently access this lab client
                int[] groupIDs = AdministrativeUtilities.GetLabClientGroups(labClientID);

                // assign uselabserver grants for each group
                int qID = AuthorizationAPI.GetQualifierID(labServerID, Qualifier.labServerQualifierTypeID);
                foreach (int groupID in groupIDs)
                {
                    AuthorizationAPI.AddGrant(groupID, Function.useLabServerFunctionType, qID);
                }
            }
            // If USS is specified add ExperinmentInfo
            return status;
        }

        protected int dissociateLS(int serverId, ref StringBuilder message)
        {
            int status = 1;
            int result = 1;
            List<int> assocLabServers = new List<int>(AdministrativeAPI.GetLabServerIDsForClient(labClientID));
            if (assocLabServers.Contains(serverId))
            {
                result = AdministrativeAPI.LabServerClient_Delete(serverId, labClientID);
                if (result < 1)
                {
                    message.AppendLine("Association between labServer & Client was not found.<br/>");
                    status = 0;
                }
                else{
                    int ussId = Convert.ToInt32(hdnUssID);
                    if (ussId > 0)
                    {
                        result = dissociateUSS(ussId, ref message);
                        if (result < 0)
                            status = result;
                    }
                }
            }
            else
            {
                message.AppendLine("LabServer is not currently associated with the client.<br/>");
                status = 0;
            }
            return status;
        }

        protected int removeSchedulingInfo(int clientId, int serverId, int ussId, int lssId, ref StringBuilder message)
        {
            int status = 1;
            int result = 1;
            DateTime start = DateTime.UtcNow;
            DateTime end = DateTime.MaxValue;
            ProcessAgent ls = ticketing.GetProcessAgent(serverId);
            ProcessAgentInfo lss = ticketing.GetProcessAgentInfo(lssId);
            ProcessAgentInfo uss = ticketing.GetProcessAgentInfo(ussId);
            if (ls == null)
            {
                message.AppendLine("LabServer is not specified!<br/>");
                status = 0;
            }
            if (lss == null)
            {
                message.AppendLine("LSS is not specified!<br/>");
                status = 0;
            }
            if (uss == null)
            {
                message.AppendLine("USS is not specified!<br/>");
                status = 0;
            }
            if (status < 1)
            {
                return status;
            }
            TicketLoadFactory tlf = TicketLoadFactory.Instance();
            string payload = tlf.createRevokeReservationPayload("ISB");
            Coupon coupon = ticketing.CreateTicket(TicketTypes.REVOKE_RESERVATION, lss.agentGuid, ProcessAgentDB.ServiceGuid, 300L, payload);
            ticketing.AddTicket(coupon, TicketTypes.REVOKE_RESERVATION, uss.agentGuid, ProcessAgentDB.ServiceGuid, 300L, payload);

            LabSchedulingProxy lssProxy = new LabSchedulingProxy();
            AgentAuthHeader agentHeader = new AgentAuthHeader();
            agentHeader.agentGuid = ProcessAgentDB.ServiceGuid;
            agentHeader.coupon = lss.identOut;
            lssProxy.AgentAuthHeaderValue = agentHeader;
            OperationAuthHeader opHeader = new OperationAuthHeader();
            opHeader.coupon = coupon;
            lssProxy.OperationAuthHeaderValue = opHeader;
            lssProxy.Url = lss.webServiceUrl;
            int count = lssProxy.RemoveReservation(ProcessAgentDB.ServiceGuid, "", uss.agentGuid, ls.agentGuid, labClient.clientGuid, start, end);
            result = lssProxy.RemoveExperimentInfo(ls.agentGuid, labClient.clientGuid);
            if (result > 0)
            {
                status = Math.Min(status, result);
            }

            UserSchedulingProxy ussProxy = new UserSchedulingProxy();
            AgentAuthHeader header = new AgentAuthHeader();
            header.agentGuid = ProcessAgentDB.ServiceGuid;
            header.coupon = uss.identOut;
            ussProxy.AgentAuthHeaderValue = header;
            OperationAuthHeader op2Header = new OperationAuthHeader();
            op2Header.coupon = coupon;
            ussProxy.OperationAuthHeaderValue = op2Header;
            ussProxy.Url = uss.webServiceUrl;
            int num = ussProxy.RevokeReservation(ProcessAgentDB.ServiceGuid, "", ls.agentGuid, labClient.clientGuid, start, end,
                "The USS is being removed from this lab client!");
            result = ussProxy.RemoveExperimentInfo(ls.agentGuid, labClient.clientGuid, lss.agentGuid);
            if (result > 0)
            {
                status = Math.Min(status, result);
            }
            return status;
        }

        /*
                // this from the popup and needs to be changed 2011/01/10 PHB
                /// <summary>
                /// Save Button.
                /// </summary>
                /// <param name="sender"></param>
                /// <param name="e"></param>
                protected void btnSaveLabServerChanges_Click(object sender, System.EventArgs e)
                {
                    //load the labServerIDs integer array from the Associated Lab Servers listbox
                    labServerIDs = new int[lbxAssociated.Items.Count];
                    for (int i = 0; i < lbxAssociated.Items.Count; i++)
                    {
                        labServerIDs[i] = int.Parse(lbxAssociated.Items[i].Value);
                    }

                    // Update the Lab Client Record with the new Lab Server list (labServerIDs)
                    try
                    {
                        //1. find groups that can access this lab server
                        int[] groupIDs = AdministrativeUtilities.GetLabClientGroups(labClientID);

                        ArrayList oldUseLSGrants = new ArrayList();
                        // First delete all "uselabserver" grants for the groups that can access the old set of lab servers
                        foreach (int groupID in groupIDs)
                        {
                            foreach (ProcessAgentInfo pa in AssocLabServers)
                            {
                                int qID = AuthorizationAPI.GetQualifierID(pa.agentId, Qualifier.labServerQualifierTypeID);
                                int[] oldLSGrants = AuthorizationAPI.FindGrants(groupID, Function.useLabServerFunctionType, qID);
                                foreach (int oldGrant in oldLSGrants)
                                    oldUseLSGrants.Add(oldGrant);
                            }
                        }

                        // remove all the grants
                        int[] unremovedGrants = AuthorizationAPI.RemoveGrants(Utilities.ArrayListToIntArray(oldUseLSGrants));

                        ////Change the labclient's list of lab servers
                        //wrapper.ModifyLabClientWrapper(labClientID, labClient.clientName, labClient.version, 
                        //    labClient.clientShortDescription, labClient.clientLongDescription, 
                        //    labClient.notes, labClient.loaderScript, labClient.clientType, 
                        //    labServerIDs, labClient.contactEmail, labClient.contactFirstName, 
                        //    labClient.contactLastName, labClient.needsScheduling, labClient.needsESS,
                        //    labClient.IsReentrant, labClient.clientInfos);

                        // Create the javascript which will cause a page refresh event to fire on the popup's parent page
                        string jScript;
                        jScript = "<script language=javascript> window.opener.Form1.hiddenPopupOnSave.value='1';";
                        jScript += "window.close();</script>";
                        Page.RegisterClientScriptBlock("postbackScript", jScript);

                        // add uselabserver grants for agents that have uselabclientgrants


                        //1. assign uselabserver grants for each group
                        foreach (int labServerID in labServerIDs)
                        {
                            int qID = AuthorizationAPI.GetQualifierID(labServerID, Qualifier.labServerQualifierTypeID);
                            foreach (int groupID in groupIDs)
                                AuthorizationAPI.AddGrant(groupID, Function.useLabServerFunctionType, qID);
                        }

                        lblResponse.Visible = true;
                        lblResponse.Text = Utilities.FormatConfirmationMessage("The labclient '" + labClient + "' has successfully been modified.");
                        btnSaveChanges.Enabled = false;
                    }
                    catch (Exception ex)
                    {
                        divErrorMessage.Visible = true;
                        lblResponse.Visible = true;
                        lblResponse.Text = "Cannot update Lab Client. " + ex.Message;
                    }
                }
         * */

        protected void btnRegisterUSS_Click(object sender, EventArgs e)
        {
            int status = 1;
            int result = 1;
            labClientID = Convert.ToInt32(ddlLabClient.SelectedValue);
            if (labClientID > 0)
                labClient = AdministrativeAPI.GetLabClient(labClientID);
            StringBuilder message = new StringBuilder();
            lblResponse.Visible = false;
            lblResponse.Text = "";
            int oldUssId = int.Parse(hdnUssID.Value);
            int ussId = int.Parse(ddlAssociatedUSS.SelectedValue);
            if (btnRegisterUSS.Text.CompareTo("Register") == 0)
            {
                if (oldUssId != ussId)
                {
                    if (oldUssId > 0)
                    {
                        result = dissociateUSS(oldUssId, ref message);
                        status = Math.Min(status, result);
                    }
                    status = registerUSS(ussId, ref message);
                }
            }
            else if (btnRegisterUSS.Text.CompareTo("Dissociate") == 0)
            {
                result = dissociateUSS(oldUssId, ref message);
                status = Math.Min(status, result);
            }
            if (message.Length > 0)
            {
                lblResponse.Visible = true;
                if (status > 0)
                {
                    lblResponse.Text = Utilities.FormatConfirmationMessage(message.ToString());
                }
                else if (status < 0)
                {
                    lblResponse.Text = Utilities.FormatErrorMessage(message.ToString());
                }
                else
                {
                    lblResponse.Text = Utilities.FormatWarningMessage(message.ToString());
                }
            }
            LoadFormFields();
        }

        protected void btnRegisterESS_Click(object sender, EventArgs e)
        {
            int status = 0;
            int result = 1;
            labClientID = Convert.ToInt32(ddlLabClient.SelectedValue);
            if (labClientID > 0)
                labClient = AdministrativeAPI.GetLabClient(labClientID);
            StringBuilder message = new StringBuilder();
            lblResponse.Visible = false;
            lblResponse.Text = "";
            int oldEssId = int.Parse(hdnEssID.Value);
            int essId = int.Parse(ddlAssociatedESS.SelectedValue);
            if (btnRegisterESS.Text.CompareTo("Register") == 0)
            {
                if (oldEssId != essId)
                {
                    if (oldEssId > 0)
                    {
                        result = dissociateESS(oldEssId, ref message);
                        status = Math.Min(status, result);
                    }
                    status = registerESS(essId, ref message);
                }
            }
            else if (btnRegisterESS.Text.CompareTo("Dissociate") == 0)
            {
                result = dissociateESS(oldEssId, ref message);
                status = Math.Min(status, result);
            }
            if (message.Length > 0)
            {
                lblResponse.Visible = true;
                if (status > 0)
                {
                    lblResponse.Text = Utilities.FormatConfirmationMessage(message.ToString());
                }
                else if (status < 0)
                {
                    lblResponse.Text = Utilities.FormatErrorMessage(message.ToString());
                }
                else
                {
                    lblResponse.Text = Utilities.FormatWarningMessage(message.ToString());
                }
            }
            LoadFormFields();
        }

        //protected int findAssociatedID(int paID, ProcessAgentType paType)
        //{
        //    int targetID = 0;
        //    ResourceMappingValue[] values = new ResourceMappingValue[1];
        //    values[0] = new ResourceMappingValue(ResourceMappingTypes.RESOURCE_TYPE, paType);
        //    //values[1] = new ResourceMappingValue(ResourceMappingTypes.TICKET_TYPE,
        //    //    TicketTypes.GetTicketType(TicketTypes.MANAGE_LAB));

        //    List<ResourceMapping> maps = ResourceMapManager.Find(new ResourceMappingKey(ResourceMappingTypes.PROCESS_AGENT, lsId), values);
        //    if (maps != null && maps.Count > 0)
        //    {
        //        foreach (ResourceMapping rm in maps)
        //        {
        //            for (int i = 0; i < rm.values.Length; i++)
        //            {
        //                if (rm.values[i].Type == ResourceMappingTypes.PROCESS_AGENT)
        //                {
        //                    targetID = (int)rm.values[i].Entry;
        //                    break;
        //                }
        //            }
        //        }
        //    }
        //    return targetID;
        //}

        protected int registerESS(int essID, ref StringBuilder message)
        {
            int status = 1;
            try
            {
                if (labClientID <= 0)
                {
                    message.AppendLine("Please save the Lab Client information before attempting to associate it with a resource.");
                    status = 0;
                }

                if (essID == 0)
                {
                    message.AppendLine("Please select a desired ESS to be associated with the client.");
                    status = 0;
                }
                if (status < 1)
                {
                    return status;
                }

                List<ResourceMappingValue> valuesList = new List<ResourceMappingValue>();
              
                ResourceMappingKey key = new ResourceMappingKey(ResourceMappingTypes.CLIENT, labClientID);
                valuesList.Add(new ResourceMappingValue(ResourceMappingTypes.RESOURCE_TYPE, ProcessAgentType.EXPERIMENT_STORAGE_SERVER));
                valuesList.Add(new ResourceMappingValue(ResourceMappingTypes.PROCESS_AGENT, essID));
                valuesList.Add(new ResourceMappingValue(ResourceMappingTypes.TICKET_TYPE,TicketTypes.GetTicketType(TicketTypes.ADMINISTER_EXPERIMENT)));

                ResourceMapping newMapping = ticketing.AddResourceMapping(key, valuesList.ToArray());

                // add mapping to qualifier list
                int qualifierType = Qualifier.resourceMappingQualifierTypeID;
                string name = ticketing.ResourceMappingToString(newMapping);
                int qualId = AuthorizationAPI.AddQualifier(newMapping.MappingID, qualifierType, name, Qualifier.ROOT);

                // No Grant required for ESS
                hdnEssID.Value = essID.ToString();
                btnRegisterESS.Text = "Dissociate";
                //ddlAssociatedESS.Enabled = false;

                message.AppendLine("Experiment Storage Server \"" + ddlAssociatedESS.SelectedItem.Text + "\" succesfully "
                    + "associated with client \"" + ddlLabClient.SelectedItem.Text + "\".");
                status = 1;
                return status;
            }
            catch
            {
                throw;
            }
        }



        protected int dissociateESS(int essID, ref StringBuilder message)
        {
            int status = 1;

            try
            {
                if (labClientID <= 0)
                {
                    message.AppendLine("Please save the Lab Client information before attempting to dissociate it from a resource");
                    status = 0;
                }

                if (essID == 0)
                {
                    message.AppendLine("Please select a desired ESS to be dissociated from the client.");
                    status = 0;
                }

                if (status < 1)
                    return status;

                int mapId = ResourceMapManager.FindMapID(ResourceMappingTypes.CLIENT, labClientID, 
                    ResourceMappingTypes.RESOURCE_TYPE, ProcessAgentType.EXPERIMENT_STORAGE_SERVER);
                //int qualID = AuthorizationAPI.GetQualifierID(mapId, Qualifier.resourceMappingQualifierTypeID);
                //AuthorizationAPI.RemoveQualifiers(new int[] { qualID });
                ticketing.DeleteResourceMapping(mapId);
                btnRegisterESS.Visible = true;
                btnRegisterESS.Text = "Register";
                message.AppendLine("Experiment Storage Server \"" + ddlAssociatedESS.SelectedItem.Text + "\" successfully "
                    + "dissociated from client \"" + ddlLabClient.SelectedItem.Text + "\".");

                ddlAssociatedESS.Enabled = true;
                ddlAssociatedESS.SelectedIndex = 0;
                hdnEssID.Value = zero;
                return 1;
            }
            catch
            {
                throw;
            }
        }

        protected int registerUSS(int ussID, ref StringBuilder message)
        {
            int status = 1;
            ProcessAgentInfo uss = null;
            try
            {
                if (labClientID <= 0)
                {
                    message.AppendLine("Please save the Lab Client information before attempting to associate it with a resource.");
                    status = 0;
                }

                if (ddlAssociatedUSS.SelectedIndex == 0)
                {
                    message.AppendLine("Please select a desired USS to be associated with the client.");
                    status = 0;
                }
                if (status < 1)
                    return status;

              

                if (ussID > 0)
                {
                    uss = ticketing.GetProcessAgentInfo(ussID);
                    if (uss != null)
                    {
                        if (uss.retired)
                        {
                            message.AppendLine("The specified USS is retired.<br/>");
                            return 0;
                        }

                        TicketLoadFactory factory = TicketLoadFactory.Instance();

                        //this should be in a loop
                        int[] labServerIDs = AdministrativeAPI.GetLabServerIDsForClient(labClientID);
                        if (labServerIDs != null && labServerIDs.Length > 0)
                        {
                            for (int i = 0; i < labServerIDs.Length; i++)
                            {

                                if (labServerIDs[i] > 0)
                                {
                                    ProcessAgentInfo labServer = ticketing.GetProcessAgentInfo(labServerIDs[i]);
                                    if (labServer.retired)
                                    {
                                        message.AppendLine("The lab server: " + labServer.agentName + " is retired!<br/>");
                                    }
                                    int lssId = ticketing.FindProcessAgentIdForAgent(labServerIDs[i], ProcessAgentType.LAB_SCHEDULING_SERVER);

                                    if (lssId > 0)
                                    {

                                        ProcessAgentInfo lss = ticketing.GetProcessAgentInfo(lssId);
                                        if (lss.retired)
                                        {
                                            message.AppendLine("The LSS: " + lss.agentName + " is retired!<br/>");
                                            return 0;
                                        }
                                        // The REVOKE_RESERVATION ticket
                                        bool needUssRevoke = true;
                                        bool needLssRevoke = true;
                                        Coupon ussRevokeCoupon = null;
                                        Coupon lssRevokeCoupon = null;
                                        Ticket ussRevoke = null;
                                        Ticket lssRevoke = null;
                                        Ticket[] ussRevokeTickets = ticketing.RetrieveIssuedTickets(-1L,
                                                TicketTypes.REVOKE_RESERVATION, uss.agentGuid, lss.agentGuid);
                                        if (ussRevokeTickets != null && ussRevokeTickets.Length > 0){
                                            ussRevoke = ussRevokeTickets[0];
                                            needUssRevoke = false;
                                        }

                                        Ticket[] lssRevokeTickets = ticketing.RetrieveIssuedTickets(-1L,
                                                TicketTypes.REVOKE_RESERVATION, lss.agentGuid, uss.agentGuid);
                                        if (lssRevokeTickets != null && lssRevokeTickets.Length > 0){
                                            lssRevoke = lssRevokeTickets[0];
                                            needLssRevoke = false;
                                        }

                                        if (ussRevoke == null && lssRevoke == null)
                                        {
                                            ussRevokeCoupon = ticketing.CreateCoupon();
                                            lssRevokeCoupon = ussRevokeCoupon;
                                        }
                                        if (needUssRevoke)
                                        {
                                            string ussRevokePayload = factory.createRevokeReservationPayload("LSS");
                                            if (ussRevokeCoupon == null)
                                            {
                                                ussRevokeCoupon = ticketing.CreateTicket(TicketTypes.REVOKE_RESERVATION,
                                                    uss.agentGuid, lss.agentGuid, -1L, ussRevokePayload);
                                            }
                                            else
                                            {
                                                ticketing.AddTicket(ussRevokeCoupon, TicketTypes.REVOKE_RESERVATION,
                                                    uss.agentGuid, lss.agentGuid, -1L, ussRevokePayload);
                                            }

                                            // Is this in the domain or cross-domain
                                            if (lss.domainGuid.Equals(ProcessAgentDB.ServiceGuid))
                                            {
                                                // this domain
                                                //Add USS on LSS

                                                LabSchedulingProxy lssProxy = new LabSchedulingProxy();
                                                lssProxy.Url = lss.webServiceUrl;
                                                lssProxy.AgentAuthHeaderValue = new AgentAuthHeader();
                                                lssProxy.AgentAuthHeaderValue.coupon = lss.identOut;
                                                lssProxy.AgentAuthHeaderValue.agentGuid = ProcessAgentDB.ServiceGuid;
                                                int ussAdded = lssProxy.AddUSSInfo(uss.agentGuid, uss.agentName, uss.webServiceUrl, ussRevokeCoupon);
                                                lssProxy.AddExperimentInfo(labServer.agentGuid, labServer.agentName, labClient.clientGuid, labClient.clientName, labClient.version, labClient.contactEmail);
                                                status = 1;
                                            }
                                            else
                                            {
                                                // cross-domain
                                                // send consumerInfo to remote SB
                                                int remoteSbId = ticketing.GetProcessAgentID(lss.domainGuid);
                                                message.AppendLine(RegistrationSupport.RegisterClientUSS(remoteSbId, null, lss.agentId, null, labServer.agentId,
                                                    ussRevokeCoupon, uss.agentId, null, labClient.clientID));
                                            }
                                        }
                                        UserSchedulingProxy ussProxy = new UserSchedulingProxy();
                                        ussProxy.Url = uss.webServiceUrl;
                                        ussProxy.AgentAuthHeaderValue = new AgentAuthHeader();
                                        ussProxy.AgentAuthHeaderValue.coupon = uss.identOut;
                                        ussProxy.AgentAuthHeaderValue.agentGuid = ProcessAgentDB.ServiceGuid;
                                        if (needLssRevoke)
                                        {
                                            //ADD LSS on USS
                                            string ussPayload = factory.createAdministerUSSPayload(Convert.ToInt32(Session["UserTZ"]));
                                            string lssRevokePayload = factory.createRevokeReservationPayload("USS");
                                            if (lssRevokeCoupon == null)
                                            {
                                                lssRevokeCoupon = ticketing.CreateTicket(TicketTypes.REVOKE_RESERVATION, lss.agentGuid, uss.agentGuid, -1L, lssRevokePayload);
                                            }
                                            else
                                            {
                                                ticketing.AddTicket(lssRevokeCoupon, TicketTypes.REVOKE_RESERVATION, lss.agentGuid, uss.agentGuid, -1L, lssRevokePayload);
                                            }
                                           
                                            int lssAdded = ussProxy.AddLSSInfo(lss.agentGuid, lss.agentName, lss.webServiceUrl, lssRevokeCoupon);
                                        }
                                        //Add Experiment Information on USS
                                        int expInfoAdded = ussProxy.AddExperimentInfo(labServer.agentGuid, labServer.agentName,
                                            labClient.clientGuid, labClient.clientName, labClient.version, labClient.contactEmail, lss.agentGuid);

                                        //Ceate resource Map
                                        ResourceMappingKey key = new ResourceMappingKey(ResourceMappingTypes.CLIENT, labClientID);
                                        List<ResourceMappingValue> valuesList = new List<ResourceMappingValue>();
                                        valuesList.Add(new ResourceMappingValue(ResourceMappingTypes.RESOURCE_TYPE, ProcessAgentType.SCHEDULING_SERVER));
                                        valuesList.Add(new ResourceMappingValue(ResourceMappingTypes.PROCESS_AGENT, ussID));
                                        valuesList.Add(new ResourceMappingValue(ResourceMappingTypes.TICKET_TYPE,
                                            TicketTypes.GetTicketType(TicketTypes.SCHEDULE_SESSION)));
                                        // Add the mapping to the database & cache
                                        ResourceMapping newMapping = ticketing.AddResourceMapping(key, valuesList.ToArray());

                                        // add mapping to qualifier list
                                        int qualifierType = Qualifier.resourceMappingQualifierTypeID;
                                        string name = ticketing.ResourceMappingToString(newMapping);
                                        int qualifierID = AuthorizationAPI.AddQualifier(newMapping.MappingID, qualifierType, name, Qualifier.ROOT);

                                        // Group Credentials MOVED TO THE MANAGE LAB GROUPS PAGE
                                    }
                                    else
                                    {
                                        message.AppendLine("You must assign a LSS to the lab server before you may assign a USS!<br/>");
                                        return 0;
                                    }
                                }
                            }
                        }
                        else
                        {
                            message.AppendLine("You must assign a Lab Server before assigning a USS!<br/>");
                            return 0;
                        }
                        btnRegisterUSS.Visible = true;
                        btnRegisterUSS.Text = "Dissociate";
                        ddlAssociatedUSS.Visible = false;
                        txtAssociatedUSS.Visible = true;
                        txtAssociatedUSS.Text = ddlAssociatedUSS.SelectedItem.Text;
                        hdnUssID.Value = ussID.ToString();


                        message.AppendLine("User-side Scheduling Server \"" + ddlAssociatedUSS.SelectedItem.Text + "\" succesfully "
                            + "associated with client \"" + ddlLabClient.SelectedItem.Text + "\".");
                    }
                }
                else
                {
                    message.AppendLine("USS was not found!<br/>");
                    return 0;
                }
            }
            catch (Exception e)
            {
                message.AppendLine("Exception: " + e.Message);
                return -1;
            }
            return status;
        }


        protected int dissociateUSS(int ussID, ref StringBuilder message)
        {
            ProcessAgentInfo lss = null;
            ProcessAgentInfo uss = null;
            int status = 1;
            int result = 1;
            int labServerID = Convert.ToInt32(hdnLabServerID.Value);
            try
            {
                if (labClientID <= 0)
                {
                    message.AppendLine("Please save the Lab Client information before attempting to dissociate it from a resource<br/>");
                    status = 0;
                }
                if (labServerID <= 0)
                {
                    message.AppendLine("The labserver is currently not set.<br/>");
                    status = 0;
                }

                if (ussID <= 0)
                {
                    message.AppendLine("Please select a desired USS to be dissociated from the client.<br/>");
                    status = 0;
                }
                if (status < 1)
                {
                    return status;
                }

                // Check if USS & LSS are assigned
                int uss_Id = ResourceMapManager.FindResourceProcessAgentID(ResourceMappingTypes.CLIENT, labClientID, ProcessAgentType.SCHEDULING_SERVER);
                if (ussID != uss_Id)
                {
                    message.AppendLine("The uss_Id and currently assigned resource do not match!<br/>");
                    return -1;
                }
                int ussMapId = ResourceMapManager.FindMapID(ResourceMappingTypes.CLIENT, labClientID, 
                    ResourceMappingTypes.RESOURCE_TYPE, ProcessAgentType.SCHEDULING_SERVER);
                int lssId = ResourceMapManager.FindResourceProcessAgentID(ResourceMappingTypes.PROCESS_AGENT, Convert.ToInt32(hdnLabServerID.Value), ProcessAgentType.LAB_SCHEDULING_SERVER);
                if (ussID > 0 || lssId > 0)
                {
                    //remove ExperimentInfo
                    result = removeSchedulingInfo(labClientID, labServerID, ussID, lssId, ref message);
                    status = Math.Min(status, result);

                }
                //LabClient lc = new LabClient();
                //lc = labClients[ddlLabClient.SelectedIndex - 1];
                UserSchedulingProxy ussProxy = new UserSchedulingProxy();
                ticketing.DeleteResourceMapping(ussMapId);
                btnRegisterUSS.Text = "Register";
                btnRegisterUSS.Visible = true;
                hdnUssID.Value = zero;

                message.AppendLine("User-side Scheduling Server \"" + ddlAssociatedUSS.SelectedItem.Text + "\" succesfully "
                    + "dissociated from client \"" + ddlLabClient.SelectedItem.Text + "\".");

                ddlAssociatedUSS.Visible = true;
                ddlAssociatedUSS.SelectedIndex = 0;
                txtAssociatedUSS.Visible = false;
                status = 1;
            }
            catch
            {
                throw;
            }
            return status;
        }

        protected void btnAssociateGroups_Click(object sender, EventArgs e)
        {

        }
        protected void btnEditList_Click(object sender, EventArgs e)
        {

        }

        protected void btnGuid_Click(object sender, System.EventArgs e)
        {
            Guid guid = System.Guid.NewGuid();
            txtClientGuid.Text = Utilities.MakeGuid();
            //valGuid.Validate();
        }

        protected void checkGuid(object sender, ServerValidateEventArgs args)
        {
            if (args.Value.Length > 0 && args.Value.Length <= 50)
                args.IsValid = true;
            else
                args.IsValid = false;
        }

    }
}
