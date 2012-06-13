/*
 * Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
 * Please see license.txt in top level directory for full license.
 * 
 * $Id$
 */

using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
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
using iLabs.DataTypes.ProcessAgentTypes;
using iLabs.DataTypes.SoapHeaderTypes;
using iLabs.DataTypes.TicketingTypes;
using iLabs.Proxies.PAgent;
using iLabs.Ticketing;
using iLabs.UtilLib;

namespace iLabs.WebService
{
	/// <summary>
	/// Summary description for administration.
	/// </summary>
    public partial class SelfRegistration : System.Web.UI.UserControl
	{

        private Color disabled = Color.FromArgb(243, 239, 229);
        private Color enabled = Color.White;
        private string agentType = ProcessAgentType.LAB_SERVER;
        private int maxGuidLength = 50;
        private string guidMessage = "The Guid must be globally unique and have no more than 50 characters";
        private string nameScript;
         private StringBuilder code;
         private string modifyMessage= "\\nThis will require changes for all iLab Services!\\nThe changes will not take effect until you click the Modify button";
         private string resetScript;
        private bool secure = false;


        ProcessAgentDB dbTicketing = new ProcessAgentDB();
       
        
        public event ServerValidateEventHandler ServerValidate;

        public string AgentType
        {
            get
            {
                return agentType;
            }
            set
            {
                agentType = value;
            }
        }

        protected void Page_Load(object sender, System.EventArgs e)
        {

            string couponId;
            string passkey;
            string issuerGuid;
            //code = new StringBuilder();
            //code.AppendLine("<script>");
            //code.AppendLine("function EnableModify() {");
            //code.AppendLine("if(document.getElementById('selfReg_btnModifyService').disabled)");
            //code.AppendLine("{document.getElementById('selfReg_btnModifyService').disabled = false;");
            //code.AppendLine("}");
            //code.AppendLine("}");
            //code.AppendLine("function ConfirmModify() {");
            //code.AppendLine("var msg= 'Are you sure you want to modify this WebService.\\nIf you proceed all references to this site will be modified.';");
            //code.AppendLine("var state = confirm(msg);return state;}");
            //code.AppendLine("function ConfirmRetire() {");
            //code.AppendLine("var msg= 'Are you sure you want to retire this WebService.\\nIf you proceed all references to this site will be retired.';");
            //code.AppendLine("var state = confirm(msg);return state;}");
            //code.AppendLine("</script>");
            //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "OnNameChange", code.ToString());
            if (!IsPostBack)
            {
          
                // try & test for local access or not configured
                if (secure)
                {
                    // retrieve parameters from URL
                    couponId = Request.QueryString["coupon_id"];
                    passkey = Request.QueryString["passkey"];
                    issuerGuid = Request.QueryString["issuer_guid"];
                    Ticket allowAdminTicket = null;
                    if (couponId != null && passkey != null && issuerGuid != null)
                    {
                        allowAdminTicket = dbTicketing.RetrieveAndVerify(
                            new Coupon(issuerGuid, Int64.Parse(couponId), passkey),
                            TicketTypes.ADMINISTER_LS);
                    }
                    else
                    {
                        Response.Redirect("AccessDenied.aspx", true);
                    }
                }

                string returnUrl = Request.QueryString["returnURL"];
                if (returnUrl != null && returnUrl.Length > 0)
                    Session["returnURL"] = returnUrl;
                txtDomainServer.ReadOnly = true;
                txtDomainServer.BackColor = disabled;
                txtOutPassKey.ReadOnly = true;
                txtOutPassKey.BackColor = disabled;
                DisplayForm();
            }
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
        }

        protected void DisplayForm()
        {
            lblResponse.Visible = false;
            StringBuilder message = new StringBuilder();
            ProcessAgent pai = ProcessAgentDB.ServiceAgent;
            if (pai != null)
            {
                lblServiceType.Text = pai.type;
                txtServiceName.Text = pai.agentName;
                bakServiceName.Value = pai.agentName;
                txtServiceGUID.Text = pai.agentGuid;

                txtCodebaseUrl.Text = pai.codeBaseUrl;
                bakCodebase.Value = pai.codeBaseUrl;
                txtWebServiceUrl.Text = pai.webServiceUrl;
                bakServiceUrl.Value = pai.webServiceUrl;


                if (agentType.Equals(ProcessAgentType.SERVICE_BROKER))
                {
                    trDomainSB.Visible = false;
                    IntTag[] pas = dbTicketing.GetProcessAgentTags();
                    // If additional processAgents are registered the Fields may not be modified
                    SetFormMode(pas != null && pas.Length > 1);
                }
                else
                { 
                    // Check to see if a ServiceBroker is registered
                    trDomainSB.Visible = true;
                    ProcessAgentInfo sbInfo = dbTicketing.GetServiceBrokerInfo();
                    if (sbInfo != null)
                    {

                        txtDomainServer.Text = sbInfo.ServiceUrl;

                        // May not modify any fields that define the service since
                        // It is registered with its domainServiceBroker

                        SetFormMode(true);

                    }
                    else
                    {
                        // May modify fields that define the service since
                        // It is not registered registered with a domainServiceBroker
                        message.Append("A domain ServiceBroker has not been registered!");
                        lblResponse.Text = Utilities.FormatWarningMessage(message.ToString());
                        lblResponse.Visible = true;
                        Utilities.WriteLog("administration: DomainServerNotFound");
                        SetFormMode(false);
                    }
                }
               
            }
            else
            {
                message.Append("The self Registration information has not been saved to the database.");
                message.Append(" Displaying default values from Web.Config. Please modify & save.");
                btnSaveChanges.Visible = true;
                btnModifyService.Visible = false;
                // Need to call selfRegister
                //lblServiceType.Text = ConfigurationManager.AppSettings["serviceType"];
                lblServiceType.Text = agentType;
                string serviceGUID = ConfigurationManager.AppSettings["serviceGUID"];
                if(serviceGUID != null)
                txtServiceGUID.Text = serviceGUID;

                string serviceURL = ConfigurationManager.AppSettings["serviceURL"];
                if (serviceURL != null)
                txtWebServiceUrl.Text = serviceURL;

                string serviceName = ConfigurationManager.AppSettings["serviceName"];
                if (serviceName != null)
                txtServiceName.Text = serviceName;

                string codebaseURL = ConfigurationManager.AppSettings["codebaseURL"];
                if (codebaseURL != null)
                txtCodebaseUrl.Text = codebaseURL;
            lblResponse.Text = Utilities.FormatWarningMessage(message.ToString());
            lblResponse.Visible = true;
            }
            txtOutPassKey.Text = ConfigurationManager.AppSettings["defaultPasskey"];
           
        }

        protected void SetFormMode(bool hasDomain)
        {
	    lblResponse.Visible = false;
        if (hasDomain)
        {
            //tdServiceName.Attributes.Add();
            //txtServiceName.Attributes.Add("onKeyPress", "EnableModify();");
            //txtCodebaseUrl.Attributes.Add("onKeyPress", "EnableModify();");
            //txtWebServiceUrl.Attributes.Add("onKeyPress", "EnableModify();");
  
            btnSaveChanges.Visible = false;
            btnModifyService.Visible = true;
            btnModifyService.Enabled = true;
            //btnModifyService.Attributes.Add("onClientClick", "ConfirmModify();");
            btnRetire.Visible = false;
            
        }
        else
        {
            //txtServiceName.Attributes.Remove("onKeyPress");
            //txtCodebaseUrl.Attributes.Remove("onKeypress");
            //txtWebServiceUrl.Attributes.Remove("onKeypress");
           
            btnSaveChanges.Visible = true;
            btnModifyService.Visible = false;
            btnRetire.Visible = false;
        }
            txtServiceGUID.ReadOnly = hasDomain;
            txtServiceGUID.BackColor = hasDomain ? disabled : enabled;
            btnGuid.Visible = !hasDomain;
        }


        protected void btnGuid_Click(object sender, System.EventArgs e)
        {
            Guid guid = System.Guid.NewGuid();
            txtServiceGUID.Text = Utilities.MakeGuid();
            valGuid.Validate();
        }

        protected void checkGuid(object sender, ServerValidateEventArgs args)
        {
            if(args.Value.Length >0 && args.Value.Length <=50)
                args.IsValid = true;
            else 
                args.IsValid = false;
        }

        protected void modifyService(object sender, System.EventArgs e)
        {
            bool error = false;
            StringBuilder message = new StringBuilder();
            try
            {
                if (ProcessAgentDB.ServiceAgent != null)
                {
                    string originalGuid = ProcessAgentDB.ServiceAgent.agentGuid;
                    if (!(txtServiceName.Text != null && txtServiceName.Text.Trim().Length > 0))
                    {
                        error = true;
                        message.Append(" You must enter a Service Name<br/>");
                    }

                    if (!(txtCodebaseUrl.Text != null && txtCodebaseUrl.Text.Trim().Length > 0))
                    {
                        error = true;
                        message.Append(" You must enter the base URL for the Web Site<br/>");
                    }
                    if (!(txtWebServiceUrl.Text != null && txtWebServiceUrl.Text.Trim().Length > 0))
                    {
                        error = true;
                        message.Append(" You must enter full URL of the Web Service page<br/>");
                    }
                    if (error)
                    {
                        lblResponse.Text = Utilities.FormatErrorMessage(message.ToString());
                        lblResponse.Visible = true;
                        return;
                    }
                    if ((txtServiceName.Text.Trim().CompareTo(bakServiceName.Value) == 0)
                        && (txtCodebaseUrl.Text.Trim().CompareTo(bakCodebase.Value) == 0)
                        && (txtWebServiceUrl.Text.Trim().CompareTo(bakServiceUrl.Value) == 0))
                    {
                        lblResponse.Text = Utilities.FormatWarningMessage("No user editable fields have been changed, modify aborted!");
                        lblResponse.Visible = true;
                        return;
                    }
                    if (ProcessAgentDB.ServiceAgent.domainGuid != null)
                    {
                        ProcessAgentInfo originalAgent = dbTicketing.GetProcessAgentInfo(originalGuid);
                        ProcessAgentInfo sb = dbTicketing.GetProcessAgentInfo(ProcessAgentDB.ServiceAgent.domainGuid);
                        if ((sb != null) && !sb.retired)
                        {
                            ProcessAgentProxy psProxy = new ProcessAgentProxy();
                            AgentAuthHeader header = new AgentAuthHeader();
                            header.agentGuid = ProcessAgentDB.ServiceAgent.agentGuid;
                            header.coupon = sb.identOut;
                            psProxy.AgentAuthHeaderValue = header;
                            psProxy.Url = sb.webServiceUrl;
                            ProcessAgent pa = new ProcessAgent();
                            pa.agentGuid = txtServiceGUID.Text.Trim();
                            pa.agentName = txtServiceName.Text.Trim();
                            pa.domainGuid = ProcessAgentDB.ServiceAgent.domainGuid;
                            pa.codeBaseUrl = txtCodebaseUrl.Text.Trim();
                            pa.webServiceUrl = txtWebServiceUrl.Text.Trim();
                            pa.type = agentType;
                            //dbTicketing.SelfRegisterProcessAgent(pa.agentGuid, pa.agentName, agentType,
                            //    pa.domainGuid, pa.codeBaseUrl, pa.webServiceUrl);
                            //message.Append("Local information has been saved. ");
                            int returnValue = psProxy.ModifyProcessAgent(originalGuid, pa, null);
                            message.Append("The changes have been sent to the ServiceBroker");
                            if (returnValue > 0)
                            {
                                dbTicketing.SelfRegisterProcessAgent(pa.agentGuid, pa.agentName, agentType,
                                pa.domainGuid, pa.codeBaseUrl, pa.webServiceUrl);
                                bakServiceName.Value = pa.agentName;
                                bakCodebase.Value = pa.codeBaseUrl;
                                bakServiceUrl.Value = pa.webServiceUrl;
                                message.Append(".<br />Local information has been saved. ");
                                lblResponse.Text = Utilities.FormatConfirmationMessage(message.ToString());
                                lblResponse.Visible = true;

                            }
                            else
                            {
                                message.Append(", but did not process correctly!");
                                message.Append("<br />Local information has not been saved. ");
                                lblResponse.Text = Utilities.FormatErrorMessage(message.ToString());
                                lblResponse.Visible = true;
                            }
                        }
                    }
                    else
                    {
                        dbTicketing.SelfRegisterProcessAgent(ProcessAgentDB.ServiceAgent.agentGuid, txtServiceName.Text, agentType,
                                null, txtCodebaseUrl.Text, txtWebServiceUrl.Text);
                    }
                }
            }
            catch (Exception ex)
            {
                Exception ex2 = new Exception("Error in  selfRegistration.modify()",ex);
                Utilities.WriteLog(Utilities.DumpException(ex2));
                throw ex2;
            }
        }


        protected void saveChanges(object sender, System.EventArgs e)
        {
           
            bool error = false;
            StringBuilder message = new StringBuilder();
            //Check fields for valid input
            if (!(txtServiceName.Text != null && txtServiceName.Text.Trim().Length > 0))
            {
                error = true;
                message.Append(" You must enter a Service Name<br/>");
            }
            if (!(txtServiceGUID.Text != null && txtServiceGUID.Text.Trim().Length > 0))
            {
                error = true;
                message.Append(" You must enter a Guid for the service<br/>");
            }
            if (!valGuid.IsValid)
            {
                error = true;
                message.Append(valGuid.Text+ "<br />");
            }
            if (!(txtCodebaseUrl.Text != null && txtCodebaseUrl.Text.Trim().Length > 0))
            {
                error = true;
                message.Append(" You must enter the base URL for the Web Site<br/>");
            }
            if (!(txtWebServiceUrl.Text != null && txtWebServiceUrl.Text.Trim().Length > 0))
            {
                error = true;
                message.Append(" You must enter full URL of the Web Service page<br/>");
            }

            if(error){
                lblResponse.Text = Utilities.FormatErrorMessage(message.ToString());
		        lblResponse.Visible = true;
                return;
            }
            else{
                // Check if domain is set if so only update mutable Fields
                dbTicketing.SelfRegisterProcessAgent(txtServiceGUID.Text.Trim(),
                    txtServiceName.Text.Trim(), lblServiceType.Text.Trim(), txtServiceGUID.Text.Trim(),
                    txtCodebaseUrl.Text.Trim(), txtWebServiceUrl.Text.Trim());
               
                DisplayForm();
                lblResponse.Text = Utilities.FormatConfirmationMessage("Self registration has completed!");
                lblResponse.Visible = true;
            }
        }

        protected void btnRetire_Click(object sender, System.EventArgs e)
        {
            bool error = false;
            StringBuilder message = new StringBuilder();
            txtServiceName.Text = "";
            txtServiceGUID.Text = "";
            txtCodebaseUrl.Text = "";
            txtWebServiceUrl.Text = "";
            lblResponse.Text = "";
            lblResponse.Visible = false;
            txtDomainServer.Text = "";
        }


        protected void btnNew_Click(object sender, System.EventArgs e)
        {
            txtServiceName.Text = "";
            txtServiceGUID.Text = "";
            txtCodebaseUrl.Text = "";
            txtWebServiceUrl.Text = "";
            lblResponse.Text = "";
	        lblResponse.Visible = false;
            txtDomainServer.Text = "";
        }

        protected void btnRefresh_Click(object sender, System.EventArgs e)
        {
            txtServiceName.Text = "";
            txtServiceGUID.Text = "";
            txtCodebaseUrl.Text = "";
            txtWebServiceUrl.Text = "";
            txtDomainServer.Text = "";
            
            DisplayForm();
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
            Utilities.WriteLog("InitializeComponent");
            dbTicketing = new ProcessAgentDB();
		}
		#endregion
	}
}
