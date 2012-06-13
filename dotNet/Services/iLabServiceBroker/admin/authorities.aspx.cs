/*
 * Copyright (c) 2011 The Massachusetts Institute of Technology. All rights reserved.
 * Please see license.txt in top level directory for full license.
 * 
 * $Id:$
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Security;

using iLabs.Core;
using iLabs.DataTypes;
using iLabs.DataTypes.ProcessAgentTypes;
using iLabs.DataTypes.SoapHeaderTypes;
using iLabs.DataTypes.TicketingTypes;
using iLabs.Proxies.PAgent;
using iLabs.Proxies.LSS;
using iLabs.ServiceBroker;
using iLabs.ServiceBroker.Internal;
using iLabs.ServiceBroker.Administration;
using iLabs.ServiceBroker.Authorization;
using iLabs.ServiceBroker.Mapping;
using iLabs.ServiceBroker;
using iLabs.Ticketing;

using iLabs.DataTypes;
using iLabs.DataTypes.SoapHeaderTypes;
using iLabs.DataTypes.TicketingTypes;
using iLabs.ServiceBroker;
using iLabs.ServiceBroker.Administration;
using iLabs.UtilLib;

namespace iLabs.ServiceBroker.admin
{
    /// <summary>
    /// Allows an Administrator to add, modify, or delete a Lab Server database record
    /// </summary>
    public partial class authorities : System.Web.UI.Page
    {
        protected bool isNew = false;
        protected bool hasAdminGroups = true;
        protected bool hasManageLSSGroups = false;
        protected BrokerDB brokerDB = new BrokerDB();
        protected AuthorizationWrapperClass wrapper = new AuthorizationWrapperClass();

        //243,239,229 - light green
        //174,155,138 - brown
        private Color disabled = Color.FromArgb(243, 239, 229);
        private Color enabled = Color.White;

        //private int currMappingID = -1;


        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (Session["UserID"] == null)
                Response.Redirect("../login.aspx");
            
            if (!IsPostBack)
            {
                txtServiceName.ReadOnly = true;
                
                // Get list of services
                LoadAuthorityList();
                LoadProtocolList();
                LoadGroupList();
                SetInputMode(false);
                
               
            }
            //else // Do any stuff that needs to be done on Postback PageOpen calls
            

            // Reset error/confirmation message
            lblErrorMessage.Visible = false;

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
            brokerDB = new BrokerDB();

        }
        #endregion


        private void LoadAuthorityList()
        {
            ddlAuthorities.Items.Clear();
            ListItem liHeaderAdminGroup = new ListItem("--- Select Authority ---", "-1");
            ddlAuthorities.Items.Add(liHeaderAdminGroup);
            IntTag[] authTags = brokerDB.GetAuthorityTags();
            if(authTags != null && authTags.Length >0){
                foreach(IntTag t in authTags){
                    ListItem li = new ListItem(t.tag, t.id.ToString());
                    ddlAuthorities.Items.Add(li);
                }
            }
        }

        private void LoadProtocolList()
        {
            ddlAuthProtocol.Items.Clear();
            ListItem li = new ListItem("--- Select ---", "0");
            ddlAuthProtocol.Items.Add(li);
            
            IntTag[] authTags = brokerDB.GetAuthProtocolTags();
            if (authTags != null && authTags.Length > 0)
            {
                foreach (IntTag t in authTags)
                {
                    li = new ListItem(t.tag, t.id.ToString());
                    ddlAuthProtocol.Items.Add(li);
                }
            }
        }

        private void LoadGroupList()
        {
            ddlGroup.Items.Clear();
           // ListItem li = new ListItem("--- Select ---", "0");
           // ddlGroup.Items.Add(li);
            int newUserGrpID = AdministrativeAPI.GetGroupID(Group.NEWUSERGROUP);
            ListItem li = new ListItem(Group.NEWUSERGROUP, newUserGrpID.ToString());
            ddlGroup.Items.Add(li);
            IntTag[] tags = brokerDB.GetIntTags("Group_RetrieveUserGroupTags",null);
            if (tags != null && tags.Length > 0)
            {
                foreach (IntTag t in tags)
                {
                    li = new ListItem(t.tag, t.id.ToString());
                    ddlGroup.Items.Add(li);
                }
            }
        }

     
      

   

        /// <summary>
        /// The GUID and Incoming Passkey fields cannot be edited in an existing record,
        /// but they must be specified for a new record.
        /// This method resets the ReadOnly state and background colors of these fields.
        /// </summary>
        /// <param name="readOnlySwitch">true if displaying a service, false if not</param>
        private void SetInputMode(bool isDisplay)
        {
            txtServiceName.ReadOnly = isDisplay;
            //txtServiceURL.ReadOnly = isDisplay;
            txtServiceGuid.ReadOnly = isDisplay;
            //txtServiceDescription.ReadOnly = isDisplay;
            //txtGroup.ReadOnly = isDisplay;
            //txtEmailProxy.ReadOnly = isDisplay;
            //txtContactEmail.ReadOnly = isDisplay;
            //txtBugEmail.ReadOnly = isDisplay;
            //txtLocation.ReadOnly = isDisplay;
            //txtPassPhrase.ReadOnly = isDisplay;
            txtServiceName.BackColor = isDisplay ? disabled : enabled;
            //txtServiceURL.BackColor = isDisplay ? disabled : enabled;
            txtServiceGuid.BackColor = isDisplay ? disabled : enabled;
            //txtServiceDescription.BackColor = isDisplay ? disabled : enabled;
            //txtGroup.BackColor = isDisplay ? disabled : enabled;
            //txtEmailProxy.BackColor = isDisplay ? disabled : enabled;
            //txtContactEmail.BackColor = isDisplay ? disabled : enabled;
            //txtBugEmail.BackColor = isDisplay ? disabled : enabled;
            //txtLocation.BackColor = isDisplay ? disabled : enabled;
            //txtPassPhrase.BackColor = isDisplay ? disabled : enabled;
        }

        /// <summary>
        /// This method clears the form fields.
        /// </summary>
        private void ClearFormFields()
        {
            txtServiceName.Text = "";
            txtServiceURL.Text = "";
            txtServiceGuid.Text = "";
            txtServiceDescription.Text = "";
            ddlGroup.SelectedIndex = 0;
            txtEmailProxy.Text = "";
            txtContactEmail.Text = "";
            txtBugEmail.Text = "";
            txtLocation.Text = "";
            txtPassPhrase.Text = "";
            ddlAuthorities.SelectedValue = "-1";
            ddlAuthProtocol.SelectedValue = "0";
            ddlGroup.SelectedIndex = 0;
        }

        /// <summary>
        /// This method fires when the Service dropdown changes.
        /// If the index is greater than zero, the specified ProcessAgent will be looked up
        /// and its values will be used to populate the text fields on the form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlAuthorities_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            int id = Convert.ToInt32(ddlAuthorities.SelectedValue);
            displayService(id);
        }

        protected void ddlAuthProtocol_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            int id = Convert.ToInt32(ddlAuthProtocol.SelectedValue);
            displayService(id);
        }

        protected void displayService(int id)
        {
            if (id > -1)
            {
                Authority auth = brokerDB.AuthorityRetrieve(id);
                if (auth != null)
                {
                    SetInputMode(true);
                    txtServiceName.Text = auth.authName;
                    txtServiceURL.Text = auth.authURL;
                    txtServiceGuid.Text = auth.authGuid;
                    txtServiceDescription.Text = auth.description;
                    ddlGroup.SelectedValue = auth.defaultGroupID.ToString();
                    txtEmailProxy.Text = auth.emailProxy;
                    txtContactEmail.Text = auth.contactEmail;
                    txtBugEmail.Text = auth.bugEmail;
                    txtLocation.Text = auth.location;
                    txtPassPhrase.Text = auth.passphrase;
                    ddlAuthorities.SelectedValue = auth.authorityID.ToString();
                    ddlAuthProtocol.SelectedValue = auth.authTypeID.ToString();
                    return;
                }
            }
            ClearFormFields();
            SetInputMode(false);
        }

          

       

        protected void btnNew_Click(object sender, System.EventArgs e)
        {
            ddlAuthorities.SelectedIndex = 0;
            ddlAuthProtocol.SelectedIndex = 0;
            ddlGroup.SelectedIndex = 0;
            ClearFormFields();
            SetInputMode(false);
        }

        protected void btnSave_Click(object sender, System.EventArgs e)
        {
            int id = -1;
            // check for required fields
            id = Convert.ToInt32(ddlAuthorities.SelectedValue);
            if (id >-1)
            {
                brokerDB.AuthorityUpdate(id, Convert.ToInt32(ddlAuthProtocol.SelectedValue), Convert.ToInt32(ddlGroup.SelectedValue),
                    txtServiceName.Text, txtServiceGuid.Text, txtServiceURL.Text, txtServiceDescription.Text, txtPassPhrase.Text,
                    txtEmailProxy.Text, txtContactEmail.Text, txtBugEmail.Text, txtLocation.Text);
            }
            else {
                id = brokerDB.AuthorityInsert(Convert.ToInt32(ddlAuthProtocol.SelectedValue), Convert.ToInt32(ddlGroup.SelectedValue),
                    txtServiceName.Text, txtServiceGuid.Text, txtServiceURL.Text, txtServiceDescription.Text, txtPassPhrase.Text,
                    txtEmailProxy.Text, txtContactEmail.Text, txtBugEmail.Text, txtLocation.Text);
            }
            if(id > -1){
                LoadAuthorityList();
                ddlAuthorities.SelectedValue = id.ToString();
                displayService(id);
            }
            
        }


        protected void btnRemove_Click(object sender, System.EventArgs e)
        {
            ddlAuthorities.SelectedIndex = 0;
            ddlAuthProtocol.SelectedIndex = 0;
            ddlGroup.SelectedIndex = 0;

            ClearFormFields();
            SetInputMode(false);
        }

  

    }
}
