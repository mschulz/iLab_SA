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

using iLabs.Core;
using iLabs.DataTypes;
using iLabs.DataTypes.ProcessAgentTypes;
using iLabs.DataTypes.TicketingTypes;
using iLabs.ServiceBroker;
using iLabs.ServiceBroker.Administration;
using iLabs.ServiceBroker.Authorization;
using iLabs.ServiceBroker.Internal;
using iLabs.ServiceBroker;
using iLabs.Ticketing;
using iLabs.UtilLib;

namespace iLabs.ServiceBroker.admin
{
	/// <summary>
	/// Summary description for addAdminURLPopup.
	/// </summary>
	public partial class addAdminURLPopup : System.Web.UI.Page
	{

        protected IntTag paTag;
        protected ArrayList adminUrlList;
        protected AdminUrl curAdminURL; 

        protected void Page_Load(object sender, System.EventArgs e)
		{
          
            if (Request.Params["paguid"] != null)
            {
                string paGuid = Request.Params["paguid"];
                ProcessAgentDB ticketing = new ProcessAgentDB();
                paTag = ticketing.GetProcessAgentTag(paGuid);
            }

            if (!Page.IsPostBack)			// populate with all the group IDs
            {
                // populate ticket type drop down
                PopulateTicketTypeDropDown();

                refreshUrlRepeater();
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
            //this.btnRefresh.ServerClick += new System.EventHandler(this.btnRefresh_ServerClick);
            //
            // The following is added to support window popups from within the repeater.
            //
            //this.repAdminURLs.ItemCreated += new RepeaterItemEventHandler(this.repAdminURLs_ItemCreated);
            this.repAdminURLs.ItemDataBound += new RepeaterItemEventHandler(this.repAdminURLs_ItemBound);
            this.repAdminURLs.ItemCommand += new RepeaterCommandEventHandler(this.repAdminURLs_ItemCommand);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {

        }
        #endregion

        private void PopulateTicketTypeDropDown() 
        {
            TicketType[] ticketTypes = TicketTypes.GetNonAbstractTicketTypes();

            ListItem[] li = new ListItem[ticketTypes.Length + 1];
            li[0] = new ListItem("--- Select Ticket Type ---");
            for (int i = 1; i < li.Length; i++)
            {
                li[i] = new ListItem(ticketTypes[i-1].name);
            }
            ttDropDownList.Items.AddRange(li);
        }

        private void refreshUrlRepeater()
        {
            try
            {
                BrokerDB ticketIssuer = new BrokerDB();
                AdminUrl[] adminUrls = ticketIssuer.RetrieveAdminURLs(paTag.id);
                adminUrlList = new ArrayList();
                foreach (AdminUrl url in adminUrls)
                {
                    adminUrlList.Add(url);
                }
                repAdminURLs.DataSource = adminUrlList;
                repAdminURLs.DataBind();
            }
            catch (Exception ex)
            {
                lblResponse.Text = Utilities.FormatErrorMessage("Cannot list administration URLs. " + ex.GetBaseException());
            }          
        }

        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRefresh_ServerClick(object sender, System.EventArgs e)
        {
            refreshUrlRepeater();
        }

        /// <summary>
        /// save the new Admin URL 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSaveChanges_Click(object sender, EventArgs e)
        {
            if (ttDropDownList.SelectedIndex == 0)
            {
                // Show Error Message
                string jScript = "<script language='javascript'>" +
                    "alert('Please select a ticket type')" + "</script>";
                Page.RegisterStartupScript("Error Window", jScript);
                return;
            }

            if (txtURL == null || txtURL.Equals(""))
            {
                // URL Text box should be filled
                // Show Error Message
                string jScript = "<script language='javascript'>" +
                    "alert('Please provide a value for the URL')" + "</script>";
                Page.RegisterStartupScript("Error Window", jScript);
                return;
            }

            // create new Admin URL
            BrokerDB issuer = new BrokerDB();
            issuer.InsertAdminURL(paTag.id, txtURL.Text, ttDropDownList.SelectedItem.Value);

            // refresh repeater
            refreshUrlRepeater();

            // reset Gui
            ttDropDownList.SelectedIndex = 0;
            txtURL.Text = "";
        }

        protected void repAdminURLs_ItemBound(object source, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                // get the current grant
                Object o = e.Item.DataItem;
                curAdminURL = (AdminUrl)o;
                Button curBtn = (Button)e.Item.FindControl("btnRemove");
                curBtn.CommandArgument = ((AdminUrl)o).Id.ToString();
            }
        }

        protected void repAdminURLs_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            refreshUrlRepeater();
            if (e.CommandName.Equals("Remove"))
            {
                // delete the admin URL
                BrokerDB issuer = new BrokerDB();
                issuer.DeleteAdminURL(Int32.Parse(e.CommandArgument.ToString()));
            }
        }


    }
}