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
using System.Drawing;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using iLabs.Core;
using iLabs.DataTypes;
using iLabs.DataTypes.SoapHeaderTypes;
using iLabs.DataTypes.TicketingTypes;
using iLabs.Ticketing;
using iLabs.UtilLib;

using iLabs.LabServer.Interactive;


namespace iLabs.LabServer.LabView
{
	/// <summary>
	/// Summary description for RunLab.
	/// </summary>
	public partial class labExperiments : System.Web.UI.Page
	{
        bool secure = false;
		protected System.Web.UI.WebControls.Label lblCoupon;
		protected System.Web.UI.WebControls.Label lblTicket;
		protected System.Web.UI.WebControls.Label lblGroupNameTitle;
	    LabDB dbManager = new LabDB();

		protected void Page_Load(object sender, System.EventArgs e)
		{


            if (!IsPostBack)
            {
                if (secure)
                {
                    // Query values from the request
                    string couponId = Request.QueryString["coupon_id"];
                    string passkey = Request.QueryString["passkey"];
                    string issuerGuid = Request.QueryString["issuer_guid"];
                    string returnTarget = Request.QueryString["sb_url"];
                    if ((returnTarget != null) && (returnTarget.Length > 0))
                        Session["returnURL"] = returnTarget;

                    // this should be the Operation Coupon
                    if (!(passkey != null && passkey != "" && couponId != null && couponId != "" && issuerGuid != null && issuerGuid != ""))
                    {

                        Coupon coupon = new Coupon(issuerGuid, long.Parse(couponId), passkey);

                        ProcessAgentDB dbTicketing = new ProcessAgentDB();
                        Ticket ticket = dbTicketing.RetrieveAndVerify(coupon, TicketTypes.MANAGE_LAB);

                        if (ticket.IsExpired() || ticket.isCancelled)
                        {
                            
                            Response.Redirect("Unauthorized.aspx", false);
                        }

                        Session["couponID"] = couponId;
                        Session["passkey"] = passkey;
                        Session["issuerID"] = issuerGuid;
                        Session["sbUrl"] = returnTarget;
                    }
                }
                btnDelete.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to remove this Lab application?')== false) return false;");

                LoadLabList();
                String returnURL = (string)Session["returnURL"];
                if ((returnURL != null) && (returnURL.Length > 0))
                {
                    lnkBackSB.NavigateUrl = returnURL;
                    lnkBackSB.Visible = true;
                }
                else
                {
                    lnkBackSB.Visible = false;
                }
                lblErrorMessage.Visible = false;
            }
            
		}

        private void LoadLabList()
        {
            ddlApplications.Items.Clear();
            ddlApplications.Items.Add(new ListItem(" -- Select Application --", "0"));
            IntTag[] labs = dbManager.GetLabAppTags();
            foreach (IntTag lab in labs)
            {
                ddlApplications.Items.Add(new ListItem(lab.tag, lab.id.ToString()));
            }
        }
        void ClearFormFields(){
            txtAppKey.Text = "";
            txtApplication.Text = "";
            txtApplicationPath.Text = "";
            txtClientGuid.Text = "";
            txtURL.Text = "";
            txtComment.Text = "";
            txtContactEmail.Text = "";
            txtDataSources.Text = "";
            txtDescription.Text = "";
            txtExtra.Text = "";
            txtHeigth.Text = "";
            txtInfoUrl.Text = "";
            txtPageUrl.Text = "";
            txtPort.Text = "";
            txtRev.Text = "";
            txtServer.Text = "";
            txtTitle.Text = "";
            txtVersion.Text = "";
            txtWidth.Text = "";
        }

        void DisplayApplication(int appId)
        {
            LabAppInfo app = dbManager.GetLabApp(appId);
            if (app == null)
            {
                ClearFormFields();
            }
            else
            {
                txtAppKey.Text = app.appKey;
                txtApplication.Text = app.application;
                txtApplicationPath.Text = app.path;
                txtClientGuid.Text = app.appGuid;
                txtURL.Text = app.appURL;
                txtComment.Text = app.comment;
                txtContactEmail.Text = app.contact;
                txtDataSources.Text = app.dataSources;
                txtDescription.Text = app.description;
                txtExtra.Text = Server.HtmlEncode(app.extraInfo);
                txtHeigth.Text = app.height.ToString();
                txtInfoUrl.Text = "";
                txtPageUrl.Text = app.page;
                txtPort.Text = app.port.ToString();
                txtRev.Text = app.rev;
                txtServer.Text = app.server;
                txtTitle.Text = app.title;
                txtVersion.Text = app.version;
                txtWidth.Text = app.width.ToString();
            }
        }
        /// <summary>
        /// This method fires when the Lab Server dropdown changes.
        /// If the index is greater than zero, the specified ProcessAgent will be looked up
        /// and its values will be used to populate the text fields on the form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlApplications_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            int id = Convert.ToInt32(ddlApplications.SelectedValue);
            if (id == 0)
            // prepare for a new record
            {
                ClearFormFields();
                btnDelete.Visible = false;
                //SetInputMode(false);
                //SetBtnScripts(null);
            }
            else
            //retrieve an existing record
            {

                DisplayApplication(id);
                btnDelete.Visible = true;
            }
        }

           /// <summary>
        /// Clicking this button deletes a lab server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNew_Click(object sender, System.EventArgs e)
        {
            ClearFormFields();
            btnDelete.Visible = false;
            ddlApplications.SelectedIndex = 0;
            
        }


        /// <summary>
        /// Clicking this button deletes a lab server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDelete_Click(object sender, System.EventArgs e)
        {
            if (ddlApplications.SelectedIndex != 0)
            {
                int appId = Convert.ToInt32(ddlApplications.SelectedValue);
                dbManager.DeleteLabApp(appId);
                ClearFormFields();
                LoadLabList();
                btnDelete.Visible = false;
                ddlApplications.SelectedIndex = 0;
                lblErrorMessage.Text=Utilities.FormatConfirmationMessage("Experiment was deleted!");
                lblErrorMessage.Visible = true;

            }

        }

        /// <summary>
        /// Clicking this button deletes a lab server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSaveChanges_Click(object sender, System.EventArgs e)
        {
            // TODO:
            // Check for required fields

           if (ddlApplications.SelectedIndex == 0)
            {
                // Assume that this is a new LabApp
                int port = 0;
                int width = 0;
                int height = 0;
                int type = 0;
               
                if (txtWidth.Text != null && txtWidth.Text.Length > 0)
                    width = Int32.Parse(txtWidth.Text);
                if (txtHeigth.Text != null && txtHeigth.Text.Length > 0)
                    height = Int32.Parse(txtHeigth.Text);
                if (txtPort.Text != null && txtPort.Text.Length > 0)
                    port = Int32.Parse(txtPort.Text);
                string extra = null;
                if (txtExtra.Text != null && txtExtra.Text.Length > 0)
                    extra = Server.HtmlEncode(txtExtra.Text);

                int appId = dbManager.InsertLabApp(
                    txtTitle.Text,txtClientGuid.Text, txtVersion.Text,txtAppKey.Text,           
                    txtApplicationPath.Text,txtApplication.Text,txtPageUrl.Text,
                    txtURL.Text,width,height,txtDataSources.Text,
                    txtServer.Text,port,txtContactEmail.Text,
                    txtDescription.Text,txtComment.Text,extra,txtRev.Text,0);
               
                LoadLabList();
                if (appId > 0)
                {
                    ddlApplications.SelectedValue = appId.ToString();
                    btnDelete.Visible = true;
                    lblErrorMessage.Text = Utilities.FormatConfirmationMessage("Experiment was created!");
                    lblErrorMessage.Visible = true;
                }
            }
            else
            {
                // Modify the existing LabApp
                int appId = Convert.ToInt32(ddlApplications.SelectedValue);
                int port = 0;
                int width = 0;
                int height = 0;
                int type = 0;
                if (txtWidth.Text != null && txtWidth.Text.Length > 0)
                    width = Int32.Parse(txtWidth.Text);
                if (txtHeigth.Text != null && txtHeigth.Text.Length > 0)
                    height = Int32.Parse(txtHeigth.Text);
                if (txtPort.Text != null && txtPort.Text.Length > 0)
                    port = Int32.Parse(txtPort.Text);
                string extra = null;
                if (txtExtra.Text != null && txtExtra.Text.Length > 0)
                    extra = Server.HtmlEncode(txtExtra.Text);

                dbManager.ModifyLabApp(appId,
                    txtTitle.Text, txtClientGuid.Text, txtVersion.Text, txtAppKey.Text,
                    txtApplicationPath.Text, txtApplication.Text, txtPageUrl.Text,
                    txtURL.Text, width, height, txtDataSources.Text,
                    txtServer.Text, port, txtContactEmail.Text,
                    txtDescription.Text, txtComment.Text, Server.HtmlEncode(txtExtra.Text), txtRev.Text, 0);
                lblErrorMessage.Text = Utilities.FormatConfirmationMessage("Experiment was modified!");
                lblErrorMessage.Visible = true;
                DisplayApplication(appId);
            }
            return;
        }
        protected void btnGuid_Click(object sender, System.EventArgs e)
        {
            Guid guid = System.Guid.NewGuid();
            txtClientGuid.Text = Utilities.MakeGuid();
            valGuid.Validate();
        }

        protected void checkGuid(object sender, ServerValidateEventArgs args)
        {
            if (args.Value.Length > 0 && args.Value.Length <= 50)
                args.IsValid = true;
            else
                args.IsValid = false;
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
	}
}
