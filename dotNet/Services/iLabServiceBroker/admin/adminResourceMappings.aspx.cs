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
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using iLabs.ServiceBroker.Administration;
using iLabs.ServiceBroker.Authorization;
using iLabs.ServiceBroker.Internal;
using iLabs.ServiceBroker.Mapping;
using iLabs.ServiceBroker;
using iLabs.DataTypes;
using iLabs.DataTypes.TicketingTypes;
using iLabs.UtilLib;
using iLabs.Ticketing;

namespace iLabs.ServiceBroker.admin
{
	/// <summary>
	/// Summary description for adminsterGroups.
	/// </summary>
	public partial class adminResourceMappings : System.Web.UI.Page
	{
	
		protected string actionCmd = "Remove";
		protected string target;
		protected static ArrayList valuesList;
        protected static ResourceMappingValue curValue;
        protected static ResourceMapping curMapping;
        protected static ArrayList mappingsList;

		AuthorizationWrapperClass wrapper = new AuthorizationWrapperClass ();
        protected BrokerDB brokerDB;

		protected void Page_Load(object sender, System.EventArgs e)
		{
            brokerDB = new BrokerDB();
            //valuesList = new ArrayList();

			if (Session["UserID"]==null)
				Response.Redirect("../login.aspx");

			// Controls that enable the popup to fire an event on the caller when the Save button is hit.
			btnRefresh.CausesValidation = false;
			
			// This is a hidden input tag. The associatedLabServers popup changes its value using a window.opener call in javascript,
			// then the GetPostBackEventReference fires the event associated with the btnRefresh button.
			// The result is that the LabServer repeater (repLabServers) is refreshed when the Save button is clicked
			// on the popup.
			hiddenPopupOnSave.Attributes.Add("onpropertychange", Page.GetPostBackEventReference(btnRefresh));
			
			if(!IsPostBack )			// populate with all the group IDs
			{
                valuesList = new ArrayList();
                InitializeDropdowns();

                // refresh values repeater
                repValues.DataSource = valuesList;
                repValues.DataBind();

                refreshMappingsRepeater();
			}
		}

        private void InitializeDropdowns()
        {
            //ServiceIDs = wrapper.ListServiceIDsWrapper();
            IntTag[] tags = brokerDB.GetProcessAgentTagsWithType();

            ///////////////////////////
            // initialize key dropdowns
            ///////////////////////////

            // initialize keytype dropdown
            keyTypeDropdown.Items.Clear();
            keyTypeDropdown.Items.Add(new ListItem(" ------------ select Key Type ------------ ", "0"));
            keyTypeDropdown.Items.Add(new ListItem("Process Agent", ResourceMappingTypes.PROCESS_AGENT));
            keyTypeDropdown.Items.Add(new ListItem("Client", ResourceMappingTypes.CLIENT));
            keyTypeDropdown.Items.Add(new ListItem("Ticket Type", ResourceMappingTypes.TICKET_TYPE));
            keyTypeDropdown.Items.Add(new ListItem("Group", ResourceMappingTypes.GROUP));

            // make key dropdowns invisible
            key_ProcessAgentDropdown.Visible = false;
            key_ClientDropdown.Visible = false;
            key_TicketTypeDropdown.Visible = false;
            key_GroupDropdown.Visible = false;

            // initialize controls for key entry
            // process agent dropdown
            key_ProcessAgentDropdown.Items.Clear();
            key_ProcessAgentDropdown.Items.Add(new ListItem(" ------------- select Process Agent ------------ ", "0"));
            // Do not load the "Unknown Lab Server" record into the dropdown. Hence we start at 1, not 0
            foreach (IntTag t in tags)
            {
                key_ProcessAgentDropdown.Items.Add(new ListItem(t.tag, t.id.ToString()));
            }

            // client dropdown
            // Load Lab Client dropdown
            key_ClientDropdown.Items.Clear();
            key_ClientDropdown.Items.Add(new ListItem(" ---------- select Lab Client ---------- ", "0"));

            int[] labClientIDs = wrapper.ListLabClientIDsWrapper();
            LabClient[] labClients = wrapper.GetLabClientsWrapper(labClientIDs);
            foreach (LabClient lc in labClients)
            {
                key_ClientDropdown.Items.Add(new ListItem(lc.clientName, lc.clientID.ToString()));
            }

            // ticket type dropdown
            key_TicketTypeDropdown.Items.Clear();
            key_TicketTypeDropdown.Items.Add(new ListItem(" ------------ select Ticket Type ------------ ", "0"));
            TicketType[] ticketTypes = TicketTypes.GetTicketTypes();
            for (int i = 0; i < ticketTypes.Length; i++)
            {
                key_TicketTypeDropdown.Items.Add(new ListItem(ticketTypes[i].shortDescription, ticketTypes[i].Name));
            }

            // group dropdown
            key_GroupDropdown.Items.Clear();
            key_GroupDropdown.Items.Add(new ListItem(" ------------ select Group ------------ ", "0"));
            int[] groupIDs = wrapper.ListGroupIDsWrapper();
            Group[] groups = wrapper.GetGroupsWrapper(groupIDs);
            foreach (Group g in groups)
            {
                key_GroupDropdown.Items.Add(new ListItem(g.groupName, g.groupID.ToString()));
            }

            ///////////////////////////
            // initialize value dropdowns
            ///////////////////////////

            // initialize valuetype dropdown
            valueTypeDropdown.Items.Clear();
            valueTypeDropdown.Items.Add(new ListItem(" ------------ select Value Type ------------ ", "0"));
            valueTypeDropdown.Items.Add(new ListItem("Process Agent", ResourceMappingTypes.PROCESS_AGENT));
            valueTypeDropdown.Items.Add(new ListItem("Client", ResourceMappingTypes.CLIENT));
            valueTypeDropdown.Items.Add(new ListItem("Resource Mapping", ResourceMappingTypes.RESOURCE_MAPPING));
            valueTypeDropdown.Items.Add(new ListItem("String", ResourceMappingTypes.STRING));
            valueTypeDropdown.Items.Add(new ListItem("Ticket Type", ResourceMappingTypes.TICKET_TYPE));
            valueTypeDropdown.Items.Add(new ListItem("Group", ResourceMappingTypes.GROUP));
            valueTypeDropdown.Items.Add(new ListItem("Resource Type", ResourceMappingTypes.RESOURCE_TYPE));

            // make value dropdowns invisible
            value_ProcessAgentDropdown.Visible = false;
            value_ClientDropdown.Visible = false;
            value_MappingDropdown.Visible = false;
            value_StringText.Visible = false;
            value_TicketTypeDropdown.Visible = false;
            value_GroupDropdown.Visible = false;
            value_ResourceTypeText.Visible = false;


            // initialize controls for value entry
            // process agent dropdown
            value_ProcessAgentDropdown.Items.Clear();
            value_ProcessAgentDropdown.Items.Add(new ListItem(" ------------- select Process Agent ------------ ", "0"));
            // Do not load the "Unknown Lab Server" record into the dropdown. Hence we start at 1, not 0
            foreach (IntTag t in tags)
            {
                value_ProcessAgentDropdown.Items.Add(new ListItem(t.tag, t.id.ToString()));
            }

            // client dropdown
            // Load Lab Client dropdown
            value_ClientDropdown.Items.Add(new ListItem(" ---------- select Lab Client ---------- ", "0"));

            foreach (LabClient lc in labClients)
            {
                value_ClientDropdown.Items.Add(new ListItem(lc.clientName, lc.clientID.ToString()));
            }

            // mappings dropdown
            value_MappingDropdown.Items.Clear();
            value_MappingDropdown.Items.Add(new ListItem(" ------------- select Resource Mapping ------------ ", "0"));
            ResourceMapManager.Refresh();
            List<ResourceMapping> mappings = ResourceMapManager.Get();
            foreach (ResourceMapping rm in mappings)
            {
                ListItem rmItem = new ListItem(brokerDB.ResourceMappingToString(rm), rm.MappingID.ToString());
                value_MappingDropdown.Items.Add(rmItem);
            }


            // ticket type dropdown
            value_TicketTypeDropdown.Items.Clear();
            value_TicketTypeDropdown.Items.Add(new ListItem(" ------------ select Ticket Type ------------ ", "0"));
            for (int i = 0; i < ticketTypes.Length; i++)
            {
                value_TicketTypeDropdown.Items.Add(new ListItem(ticketTypes[i].shortDescription, ticketTypes[i].Name));
            }

            // group dropdown
            value_GroupDropdown.Items.Clear();
            value_GroupDropdown.Items.Add(new ListItem(" ------------ select Group ------------ ", "0"));
            foreach (Group g in groups)
            {
                value_GroupDropdown.Items.Add(new ListItem(g.groupName, g.groupID.ToString()));
            }


        }

		private void refreshMappingsRepeater()
		{
            try
            {
                ResourceMapManager.Refresh();
                List<ResourceMapping> mappingsList = ResourceMapManager.Get();
                repRsrcMappings.DataSource = mappingsList;
                repRsrcMappings.DataBind();
            }
            catch (Exception e)
            {
                lblResponse.Text = Utilities.FormatErrorMessage("Cannot list Resource Mappings. " + e.GetBaseException());
            }           
		}
		/// <summary>
		/// This is a hidden HTML button that is "clicked" by an event raised 
		/// by the closing of the associatedLabServers popup.
		/// It causes the Lab Servers repeater to be refreshed.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnRefresh_ServerClick(object sender, System.EventArgs e)
		{
			refreshMappingsRepeater();
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
			this.btnRefresh.ServerClick += new System.EventHandler(this.btnRefresh_ServerClick);
			//
			// The following is added to support window popups from within the repeater.
			//
			this.repValues.ItemCreated += new RepeaterItemEventHandler(this.repValues_ItemCreated);
            this.repValues.ItemDataBound += new RepeaterItemEventHandler(this.repValues_ItemBound);
            this.repValues.ItemCommand += new RepeaterCommandEventHandler(this.repValues_ItemCommand);

            this.repRsrcMappings.ItemCreated += new RepeaterItemEventHandler(this.repRsrcMappings_ItemCreated);
            this.repRsrcMappings.ItemDataBound += new RepeaterItemEventHandler(this.repRsrcMappings_ItemBound);
            this.repRsrcMappings.ItemCommand += new RepeaterCommandEventHandler(this.repRsrcMappings_ItemCommand);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    

		}
		#endregion

		private void repValues_ItemCreated(Object sender, RepeaterItemEventArgs e)
		{
			if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
			{
				Button curBtn = (Button) e.Item.FindControl("btnRemoveValue");
				//curBtn.Attributes.Add("onClick", "return confirmDelete();");
			}
		}

		private void repValues_ItemBound(Object sender, RepeaterItemEventArgs e)
		{		
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                // get the current mapping value
                Object o = e.Item.DataItem;
                curValue = (ResourceMappingValue)o;             

                // set value label
                Label lbl = (Label)e.Item.FindControl("valueLabel");
                lbl.Text = brokerDB.GetMappingEntryString(curValue,false);
            }			
		}
		private void repValues_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
		{            
            if (e.CommandName.Equals("Remove"))
            {
                // delete the mapping value from the values list                
                valuesList.Remove(curValue);
                // refresh values repeater
                repValues.DataSource = valuesList;
                repValues.DataBind();
            }                
		}

        private void repRsrcMappings_ItemCreated(Object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                //Button curBtn = (Button)e.Item.FindControl("btnRemove");
                //curBtn.Attributes.Add("onClick", btnRemove_Click);
            }
        }

        private void repRsrcMappings_ItemBound(Object sender, RepeaterItemEventArgs e)
        {           
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                // get the current mapping 
                Object o = e.Item.DataItem;
                curMapping = (ResourceMapping)o;
                Button curBtn = (Button)e.Item.FindControl("btnRemove");
                //curBtn.Attributes.Add("onClick", btnRemove_Click);
                curBtn.CommandArgument = curMapping.MappingID.ToString();
                // set key label
                Label lbl = (Label)e.Item.FindControl("keyLabel");
                lbl.Text = brokerDB.GetMappingEntryString(curMapping.Key,false);

                ////////
                ArrayList mappingValuesList = new ArrayList(curMapping.values.Length);
                for (int i = 0; i < curMapping.values.Length; i++)
                    mappingValuesList.Add(curMapping.values[i]);

                Repeater repeater = (Repeater)e.Item.FindControl("repValues2");
                repeater.ItemCreated += new RepeaterItemEventHandler(this.repValues2_ItemCreated);
                repeater.ItemDataBound += new RepeaterItemEventHandler(this.repValues2_ItemBound);
                repeater.ItemCommand += new RepeaterCommandEventHandler(this.repValues2_ItemCommand);
                repeater.DataSource = mappingValuesList;
                repeater.DataBind();
            }
        }
        private void repRsrcMappings_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
        {
            if (e.CommandName.Equals("Remove"))
            {
                // delete the resource mapping from the database
                brokerDB.DeleteResourceMapping(curMapping);
                
                // refresh repeater
                List<ResourceMapping> mappingsList = ResourceMapManager.Get();
                
                repRsrcMappings.DataSource = mappingsList;
                repRsrcMappings.DataBind();

                // refresh mappings dropdown
                value_MappingDropdown.Items.Clear();
                value_MappingDropdown.Items.Add(new ListItem(" ------------- select Resource Mapping ------------ ", "0"));
                foreach (ResourceMapping rm in mappingsList)
                {
                    value_MappingDropdown.Items.Add(new ListItem(brokerDB.ResourceMappingToString(rm), rm.MappingID.ToString()));
                }
            }
        }

        private void repValues2_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
        {
        }

        private void repValues2_ItemBound(Object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                // get the current mapping 
                Object o = e.Item.DataItem;
                curValue = (ResourceMappingValue)o;

                // set value label 
                Label lbl = (Label)e.Item.FindControl("valueLabel2");
                lbl.Text = brokerDB.GetMappingEntryString(curValue,false);
            }
        }

        private void repValues2_ItemCreated(Object sender, RepeaterItemEventArgs e)
        {
        }

        /// <summary>
        /// Add a new value entry to the list of temporary values
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddValue_Click(object sender, System.EventArgs e)
        {
            if (valueTypeDropdown.SelectedIndex == 0)
            {
                // Show Error Message
                string jScript = "<script language='javascript'>" +
                    "alert('Please select a value type')" + "</script>";
                Page.RegisterStartupScript("Error Window", jScript);
                return;
            }

            // create a new Resource Mapping Value
            string selectedValueType = valueTypeDropdown.SelectedValue;
            Object valueObj = null;
            if (selectedValueType.Equals(ResourceMappingTypes.PROCESS_AGENT))
            {
                if (value_ProcessAgentDropdown.SelectedIndex == 0)
                {
                    // Show Error Message
                    string jScript = "<script language='javascript'>" +
                        "alert('Please select a process agent')" + "</script>";
                    Page.RegisterStartupScript("Error Window", jScript);
                    return;
                }
                valueObj = int.Parse(value_ProcessAgentDropdown.SelectedValue);
            }
            else if (selectedValueType.Equals(ResourceMappingTypes.CLIENT))
            {
                if (value_ClientDropdown.SelectedIndex == 0)
                {
                    // Show Error Message
                    string jScript = "<script language='javascript'>" +
                        "alert('Please select a client')" + "</script>";
                    Page.RegisterStartupScript("Error Window", jScript);
                    return;
                }
                valueObj = int.Parse(value_ClientDropdown.SelectedValue);
            }
            else if (selectedValueType.Equals(ResourceMappingTypes.RESOURCE_MAPPING))
            {
                if (value_MappingDropdown.SelectedIndex == 0)
                {
                    // Show Error Message
                    string jScript = "<script language='javascript'>" +
                        "alert('Please select a Resource Mapping')" + "</script>";
                    Page.RegisterStartupScript("Error Window", jScript);
                    return;
                }
                // value object is resource mapping ID
                valueObj = int.Parse(value_MappingDropdown.SelectedValue);
            }
            else if (selectedValueType.Equals(ResourceMappingTypes.STRING))
            {
                if (value_StringText.Text == null || value_StringText.Text.Equals(""))
                {
                    // Show Error Message
                    string jScript = "<script language='javascript'>" +
                        "alert('Please enter a string value')" + "</script>";
                    Page.RegisterStartupScript("Error Window", jScript);
                    return;
                }
                valueObj = value_StringText.Text;
            }
            else if (selectedValueType.Equals(ResourceMappingTypes.TICKET_TYPE))
            {
                if (value_TicketTypeDropdown.SelectedIndex == 0)
                {
                    // Show Error Message
                    string jScript = "<script language='javascript'>" +
                        "alert('Please select a ticket type')" + "</script>";
                    Page.RegisterStartupScript("Error Window", jScript);
                    return;
                }
                valueObj = TicketTypes.GetTicketType(value_TicketTypeDropdown.SelectedValue);
            }
            else if (selectedValueType.Equals(ResourceMappingTypes.GROUP))
            {
                if (value_GroupDropdown.SelectedIndex == 0)
                {
                    // Show Error Message
                    string jScript = "<script language='javascript'>" +
                        "alert('Please select a Group')" + "</script>";
                    Page.RegisterStartupScript("Error Window", jScript);
                    return;
                }
                valueObj = int.Parse(value_GroupDropdown.SelectedValue);
            }

            else if (selectedValueType.Equals(ResourceMappingTypes.RESOURCE_TYPE))
            {
                if (value_ResourceTypeText.Text == null || value_ResourceTypeText.Text.Equals(""))
                {
                    // Show Error Message
                    string jScript = "<script language='javascript'>" +
                        "alert('Please enter a string value')" + "</script>";
                    Page.RegisterStartupScript("Error Window", jScript);
                    return;
                }
                valueObj = value_ResourceTypeText.Text;
            }

            ResourceMappingValue value = new ResourceMappingValue(valueTypeDropdown.SelectedValue, valueObj);
            
            // add value to list of values and refresh the value repeater
            valuesList.Add(value);
            repValues.DataSource = valuesList;
            repValues.DataBind();

            // reset dropdown lists
            valueTypeDropdown.SelectedIndex = 0;
            value_ProcessAgentDropdown.SelectedIndex = 0;
            value_TicketTypeDropdown.SelectedIndex = 0;
            value_ClientDropdown.SelectedIndex = 0;
            value_TicketTypeDropdown.SelectedIndex = 0;
            value_GroupDropdown.SelectedIndex = 0;
            value_StringText.Text = "";
            value_ResourceTypeText.Text = "";

            value_ProcessAgentDropdown.Visible = false;
            value_ClientDropdown.Visible = false;
            value_StringText.Visible = false;
            value_TicketTypeDropdown.Visible = false;
            value_GroupDropdown.Visible = false;
            value_ResourceTypeText.Visible = false;
        }

        /// <summary>
        /// Add new resource mapping to the database 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddResource_Click(object sender, EventArgs e)
        {
            // check that a key type is selected
            if (keyTypeDropdown.SelectedIndex == 0)
            {
                // Show Error Message
                string jScript = "<script language='javascript'>" +
                    "alert('Please select a key type')" + "</script>";
                Page.RegisterStartupScript("Error Window", jScript);
                return;
            }

            // if a value is selected but not added to the list of values, add it
            if (valueTypeDropdown.SelectedValue.Equals(ResourceMappingTypes.PROCESS_AGENT) && value_ProcessAgentDropdown.SelectedIndex != 0 ||
                valueTypeDropdown.SelectedValue.Equals(ResourceMappingTypes.CLIENT) && value_ClientDropdown.SelectedIndex != 0 ||
                valueTypeDropdown.SelectedValue.Equals(ResourceMappingTypes.TICKET_TYPE) && value_TicketTypeDropdown.SelectedIndex != 0 ||
                valueTypeDropdown.SelectedValue.Equals(ResourceMappingTypes.RESOURCE_MAPPING) && value_MappingDropdown.SelectedIndex != 0 ||
                valueTypeDropdown.SelectedValue.Equals(ResourceMappingTypes.GROUP) && value_GroupDropdown.SelectedIndex != 0 ||
                valueTypeDropdown.SelectedValue.Equals(ResourceMappingTypes.STRING) && !value_StringText.Text.Equals("") && value_StringText.Text != null ||
                valueTypeDropdown.SelectedValue.Equals(ResourceMappingTypes.RESOURCE_TYPE) && !value_ResourceTypeText.Text.Equals("") && value_ResourceTypeText.Text != null)
            {

                // create a new Resource Mapping Value
                string selectedValueType = valueTypeDropdown.SelectedValue;
                Object valueObj = null;
                if (selectedValueType.Equals(ResourceMappingTypes.PROCESS_AGENT))
                {
                    if (value_ProcessAgentDropdown.SelectedIndex == 0)
                    {
                        // Show Error Message
                        string jScript = "<script language='javascript'>" +
                            "alert('Please select a process agent')" + "</script>";
                        Page.RegisterStartupScript("Error Window", jScript);
                        return;
                    }
                    valueObj = int.Parse(value_ProcessAgentDropdown.SelectedValue);
                }
                else if (selectedValueType.Equals(ResourceMappingTypes.CLIENT))
                {
                    if (value_ClientDropdown.SelectedIndex == 0)
                    {
                        // Show Error Message
                        string jScript = "<script language='javascript'>" +
                            "alert('Please select a client')" + "</script>";
                        Page.RegisterStartupScript("Error Window", jScript);
                        return;
                    }
                    valueObj = int.Parse(value_ClientDropdown.SelectedValue);
                }
                else if (selectedValueType.Equals(ResourceMappingTypes.RESOURCE_MAPPING))
                {
                    if (value_MappingDropdown.SelectedIndex == 0)
                    {
                        // Show Error Message
                        string jScript = "<script language='javascript'>" +
                            "alert('Please select a Resource Mapping')" + "</script>";
                        Page.RegisterStartupScript("Error Window", jScript);
                        return;
                    }
                    valueObj = brokerDB.GetResourceMapping(int.Parse(value_MappingDropdown.SelectedValue));
                }
                else if (selectedValueType.Equals(ResourceMappingTypes.STRING))
                {
                    if (value_StringText.Text == null || value_StringText.Text.Equals(""))
                    {
                        // Show Error Message
                        string jScript = "<script language='javascript'>" +
                            "alert('Please enter a string value')" + "</script>";
                        Page.RegisterStartupScript("Error Window", jScript);
                        return;
                    }
                    valueObj = value_StringText.Text;
                }
                else if (selectedValueType.Equals(ResourceMappingTypes.TICKET_TYPE))
                {
                    if (value_TicketTypeDropdown.SelectedIndex == 0)
                    {
                        // Show Error Message
                        string jScript = "<script language='javascript'>" +
                            "alert('Please select a ticket type')" + "</script>";
                        Page.RegisterStartupScript("Error Window", jScript);
                        return;
                    }
                    valueObj = TicketTypes.GetTicketType(value_TicketTypeDropdown.SelectedValue);
                }
                else if (selectedValueType.Equals(ResourceMappingTypes.GROUP))
                {
                    if (value_GroupDropdown.SelectedIndex == 0)
                    {
                        // Show Error Message
                        string jScript = "<script language='javascript'>" +
                            "alert('Please select a group')" + "</script>";
                        Page.RegisterStartupScript("Error Window", jScript);
                        return;
                    }
                    valueObj = int.Parse(value_GroupDropdown.SelectedValue);
                }

                else if (selectedValueType.Equals(ResourceMappingTypes.RESOURCE_TYPE))
                {
                    if (value_ResourceTypeText.Text == null || value_ResourceTypeText.Text.Equals(""))
                    {
                        // Show Error Message
                        string jScript = "<script language='javascript'>" +
                            "alert('Please enter a string value')" + "</script>";
                        Page.RegisterStartupScript("Error Window", jScript);
                        return;
                    }
                    valueObj = value_ResourceTypeText.Text;
                }

                ResourceMappingValue value = new ResourceMappingValue(valueTypeDropdown.SelectedValue, valueObj);

                // add value to list of values and refresh the value repeater
                if (valuesList == null)
                    valuesList = new ArrayList();
                valuesList.Add(value);
            }

            if (valuesList.Count < 1)
            {          
                // Show Error Message
                string jScript = "<script language='javascript'>" +
                    "alert('Please create a mapping value')" + "</script>";
                Page.RegisterStartupScript("Error Window", jScript);
                return;
            }
            
            // create key object
            string selectedKeyType = keyTypeDropdown.SelectedValue;
            Object keyObj = null;
            if (selectedKeyType.Equals(ResourceMappingTypes.PROCESS_AGENT))
            {
                if (key_ProcessAgentDropdown.SelectedIndex == 0)
                {
                    // Show Error Message
                    string jScript = "<script language='javascript'>" +
                        "alert('Please select a process agent')" + "</script>";
                    Page.RegisterStartupScript("Error Window", jScript);
                    return;
                }
                keyObj = int.Parse(key_ProcessAgentDropdown.SelectedValue);
            }
            else if (selectedKeyType.Equals(ResourceMappingTypes.CLIENT))
            {
                if (key_ClientDropdown.SelectedIndex == 0)
                {
                    // Show Error Message
                    string jScript = "<script language='javascript'>" +
                        "alert('Please select a client')" + "</script>";
                    Page.RegisterStartupScript("Error Window", jScript);
                    return;
                }
                keyObj = int.Parse(key_ClientDropdown.SelectedValue);
            }
            else if (selectedKeyType.Equals(ResourceMappingTypes.TICKET_TYPE))
            {
                if (key_TicketTypeDropdown.SelectedIndex == 0)
                {
                    // Show Error Message
                    string jScript = "<script language='javascript'>" +
                        "alert('Please select a ticket type')" + "</script>";
                    Page.RegisterStartupScript("Error Window", jScript);
                    return;
                }
                keyObj = TicketTypes.GetTicketType(key_TicketTypeDropdown.SelectedValue);
            }
            else if (selectedKeyType.Equals(ResourceMappingTypes.GROUP))
            {
                if (key_GroupDropdown.SelectedIndex == 0)
                {
                    // Show Error Message
                    string jScript = "<script language='javascript'>" +
                        "alert('Please select a group')" + "</script>";
                    Page.RegisterStartupScript("Error Window", jScript);
                    return;
                }
                keyObj = int.Parse(key_GroupDropdown.SelectedValue);
            }
            
            ResourceMappingKey key = new ResourceMappingKey(selectedKeyType, keyObj);

            // create values
            ResourceMappingValue[] values = (ResourceMappingValue[])valuesList.ToArray((new ResourceMappingValue()).GetType());

            //create resource mapping
            ResourceMapping newMapping = brokerDB.AddResourceMapping(key, values);
            // test to see if a qualifier is needed
            if (true)
            {
                // add mapping to qualifier list
                int qualifierType = Qualifier.resourceMappingQualifierTypeID;
                string name = brokerDB.ResourceMappingToString(newMapping);
                int qualId = AuthorizationAPI.AddQualifier(newMapping.MappingID, qualifierType, name, Qualifier.ROOT);
            }
            // reset value list
            valuesList = new ArrayList();
            // refresh values repeater
            repValues.DataSource = valuesList;
            repValues.DataBind();

            // refresh mappings dropdown
            value_MappingDropdown.Items.Clear();
            value_MappingDropdown.Items.Add(new ListItem(" ------------- select Resource Mapping ------------ ", "0"));
            //ResourceMapping[] mappings = brokerDB.GetResourceMappings();
            //for (int i = 0; i < mappings.Length; i++)
            //    value_MappingDropdown.Items.Add(new ListItem(brokerDB.ResourceMappingToString(mappings[i]), mappings[i].MappingID.ToString()));
            List<ResourceMapping> mappings = ResourceMapManager.Get();
            foreach (ResourceMapping rm in mappings)
            {
                value_MappingDropdown.Items.Add(new ListItem(brokerDB.ResourceMappingToString(rm), rm.MappingID.ToString()));
            }

            // refresh repeater
            refreshMappingsRepeater();

            // reset controls
            keyTypeDropdown.SelectedIndex = 0;
            key_ProcessAgentDropdown.Visible = false;
            key_ClientDropdown.Visible = false;
            key_TicketTypeDropdown.Visible = false;
            key_GroupDropdown.Visible = false;

            valueTypeDropdown.SelectedIndex = 0;
            value_ClientDropdown.Visible = false;
            value_MappingDropdown.Visible = false;
            value_StringText.Visible = false;
            value_TicketTypeDropdown.Visible = false;
            value_GroupDropdown.Visible = false;
            value_ResourceTypeText.Visible = false;
        }

        protected void btnRemove_Click(object sender, EventArgs e)
        {
            Button curBtn = (Button)sender;
            int i = Convert.ToInt32(curBtn.CommandArgument);
            if (i > 0)
            {
                brokerDB.DeleteResourceMapping(i);
                refreshMappingsRepeater();
            }
        }

        /// <summary>
        /// Make the appropriate control visible, based on the selected key type
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void keyType_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            // make key dropdowns invisible
            key_ProcessAgentDropdown.Visible = false;
            key_ClientDropdown.Visible = false;
            key_TicketTypeDropdown.Visible = false;
            key_GroupDropdown.Visible = false;

            // make the appropriate dropdown visible
            if (keyTypeDropdown.SelectedValue.Equals(ResourceMappingTypes.PROCESS_AGENT))
                key_ProcessAgentDropdown.Visible = true;
            else if (keyTypeDropdown.SelectedValue.Equals(ResourceMappingTypes.CLIENT))
                key_ClientDropdown.Visible = true;
            else if (keyTypeDropdown.SelectedValue.Equals(ResourceMappingTypes.TICKET_TYPE))
                key_TicketTypeDropdown.Visible = true;
            else if (keyTypeDropdown.SelectedValue.Equals(ResourceMappingTypes.GROUP))
                key_GroupDropdown.Visible = true;      
        }

        /// <summary>
        /// Make the appropriate control visible, based on the selected key value
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void valueType_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            // make key dropdowns invisible
            value_ProcessAgentDropdown.Visible = false;
            value_ClientDropdown.Visible = false;
            value_MappingDropdown.Visible = false;
            value_StringText.Visible = false;
            value_TicketTypeDropdown.Visible = false;
            value_GroupDropdown.Visible = false;
            value_ResourceTypeText.Visible = false;

            // make the appropriate dropdown visible
            if (valueTypeDropdown.SelectedValue.Equals(ResourceMappingTypes.PROCESS_AGENT))
                value_ProcessAgentDropdown.Visible = true;
            else if (valueTypeDropdown.SelectedValue.Equals(ResourceMappingTypes.CLIENT))
                value_ClientDropdown.Visible = true;
            else if (valueTypeDropdown.SelectedValue.Equals(ResourceMappingTypes.RESOURCE_MAPPING))
                value_MappingDropdown.Visible = true;
            else if (valueTypeDropdown.SelectedValue.Equals(ResourceMappingTypes.STRING))
                value_StringText.Visible = true;
            else if (valueTypeDropdown.SelectedValue.Equals(ResourceMappingTypes.TICKET_TYPE))
                value_TicketTypeDropdown.Visible = true;
            else if (valueTypeDropdown.SelectedValue.Equals(ResourceMappingTypes.GROUP))
                value_GroupDropdown.Visible = true;
            else if (valueTypeDropdown.SelectedValue.Equals(ResourceMappingTypes.RESOURCE_TYPE))
                value_ResourceTypeText.Visible = true;
        }

        protected void key_ProcessAgent_SelectedIndexChanged(object sender, System.EventArgs e)
        {
        }

        protected void key_Client_SelectedIndexChanged(object sender, System.EventArgs e)
        {
        }

        protected void key_TicketType_SelectedIndexChanged(object sender, System.EventArgs e)
        {
        }

        protected void value_ProcessAgent_SelectedIndexChanged(object sender, System.EventArgs e)
        {
        }

        protected void value_Client_SelectedIndexChanged(object sender, System.EventArgs e)
        {
        }

        protected void value_TicketType_SelectedIndexChanged(object sender, System.EventArgs e)
        {
        }

       

    }
}
