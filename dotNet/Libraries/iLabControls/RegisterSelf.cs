/*
 * Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
 * Please see license.txt in top level directory for full license.
 * 
 * $Id$
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using iLabs.Core;
using iLabs.DataTypes;
using iLabs.DataTypes.ProcessAgentTypes;
using iLabs.DataTypes.SoapHeaderTypes;
using iLabs.DataTypes.TicketingTypes;
using iLabs.Ticketing;
using iLabs.UtilLib;
using iLabs.Proxies.PAgent;

namespace iLabs.Controls
{
 
    [ToolboxData("<{0}:RegisterSelf runat=server></{0}:RegisterSelf>")]

    
    public class RegisterSelf : CompositeControl, IPostBackEventHandler
    {
        ProcessAgentDB dbTicketing = new ProcessAgentDB();
        #region properties

        private bool secure = false;

        
        private Color disabled = Color.FromArgb(243, 239, 229);
        private Color enabled = Color.White;
        private int maxGuidLength = 50;
        private string guidWarning = "The guid is too long! Max Length = ";
        private string headerText = "Self Registration for ";
        private string introText = "Configure this service's registration information.";

        private ProcessAgent self = null;
        private SystemSupport sSupport = null;


        public string Header
        {
            get { return headerText; }
            set { headerText = value; }
        }
        public string Intro
        {
            get { return introText; }
            set { introText = value; }
        }
        public string GuidWarning
        {
            get { return guidWarning; }
            set { guidWarning = value; }
        }
        public int MaxGuidLength
        {
            get { return maxGuidLength; }
            set
            {
                if (value > 50 || value <= 0)
                    maxGuidLength = 50;
                else 
                    maxGuidLength = value;
            }
        }
       
        public string Title
        {
            get
            {
                object o = ViewState["Title"];
                if (o == null)
                    return String.Empty;
                return (string)o;
            }
            set { ViewState["Title"] = value; }
        }
     
        public string AgentName
        {
            get
            {
                object o = ViewState["AgentName"];
                if (o == null)
                    return String.Empty;
                return (string)o;
            }
            set { ViewState["AgentName"] = value; }
        }
        public string AgentType
        {
            get
            {
                object o = ViewState["AgentType"];
                if (o == null)
                    return String.Empty;
                return (string)o;
            }
            set { ViewState["AgentType"] = value; }
        }

        public string CodebaseUrl
        {
            get
            {
                object o = ViewState["CodebaseUrl"];
                if (o == null)
                    return String.Empty;
                return (string)o;
            }
            set { ViewState["CodebaseUrl"] = value; }
        }
        public string WebServiceUrl
        {
            get
            {
                object o = ViewState["WebServiceUrl"];
                if (o == null)
                    return String.Empty;
                return (string)o;
            }
            set { ViewState["WebServiceUrl"] = value; }
        }
        public string AgentGuid
        {
            get
            {
                object o = ViewState["AgentGuid"];
                if (o == null)
                    return String.Empty;
                return (string)o;
            }
            set { ViewState["AgentGuid"] = value; }
        }
        public string DomainGuid
        {
            get
            {
                object o = ViewState["DomainGuid"];
                if (o == null)
                    return String.Empty;
                return (string)o;
            }
            set { ViewState["DomainGuid"] = value; }
        }
        public string DomainServer
        {
            get
            {
                object o = ViewState["DomainServer"];
                if (o == null)
                    return String.Empty;
                return (string)o;
            }
            set { ViewState["DomainServer"] = value; }
        }
        public string InfoUrl
        {
            get
            {
                object o = ViewState["InfoUrl"];
                if (o == null)
                    return String.Empty;
                return (string)o;
            }
            set { ViewState["InfoUrl"] = value; }
        }
        public string ContactEmail
        {
            get
            {
                object o = ViewState["ContactEmail"];
                if (o == null)
                    return String.Empty;
                return (string)o;
            }
            set { ViewState["ContactEmail"] = value; }
        }
        public string BugEmail
        {
            get
            {
                object o = ViewState["BugEmail"];
                if (o == null)
                    return String.Empty;
                return (string)o;
            }
            set { ViewState["BugEmail"] = value; }
        }
        public string Location
        {
            get
            {
                object o = ViewState["Location"];
                if (o == null)
                    return String.Empty;
                return (string)o;
            }
            set { ViewState["Location"] = value; }
        }
        public string PassCode
        {
            get
            {
                object o = ViewState["PassCode"];
                if (o == null)
                    return String.Empty;
                return (string)o;
            }
            set { ViewState["PassCode"] = value; }
        }
        public string Description
        {
            get
            {
                object o = ViewState["Description"];
                if (o == null)
                    return String.Empty;
                return (string)o;
            }
            set { ViewState["Description"] = value; }
        }
        public string Response
        {
            get
            {
                object o = ViewState["Response"];
                if (o == null)
                    return String.Empty;
                return (string)o;
            }
            set { ViewState["Response"] = value; }
        }
        public bool ResponseVisible
        {
            get
            {
                object o = ViewState["ResponseVisible"];
                if (o == null)
                    return false;
                else
                return (bool)o;
            }
            set { ViewState["ResponseVisible"] = value; }
        }
        public string GuidMessage
        {
            get
            {
                object o = ViewState["GuidMessage"];
                if (o == null)
                    return String.Empty;
                return (string)o;
            }
            set { ViewState["GuidMessage"] = value; }
        }
#endregion

        public string ModifyMessage
        {
            get
            {
                object o = ViewState["ModifyMessage"];
                if (o == null)
                    return String.Empty;
                return (string)o;
            }
            set { ViewState["ModifyMessage"] = value; }
        }

        public bool HasDomain
        {
            get
            {
                object o = ViewState["HasDomain"];
                if (o == null)
                    return false;
                return (bool)o;
            }
            set { ViewState["HasDomain"] = value; }
        }

  
        

        #region UI Controls

        Unit onePX = new Unit(1);
        Unit width = new Unit(670);
        Unit labelWidth = new Unit(200);
        Unit txtBoxHeight = new Unit(24);
        Unit dataWidth = new Unit(496);
        Unit guidWidth = new Unit(380);
        Unit buttonColumnWidth = new Unit(60);

        HtmlGenericControl pageintro;      
        HtmlGenericControl pagecontent;
        HtmlGenericControl simpleform;

        //HtmlForm frmRegister;
        Table tblMain;
        TableRow trRowPasskey;
        TableRow trDomainSB;
        TableRow trDomainGuid;
  
        Label lblTitle;
        Label lblResponse;
        Label lblIntroduction;
        Label lblServiceType;
        Label lblServiceName;
        Label lblCodebaseUrl;
        Label lblServiceUrl;
        Label lblInfoUrl;
        Label lblServiceGuid;
        Label lblDomainServer;
        Label lblDomainGuid;
        Label lblOutPassKey;
        Label lblDescription;
        Label lblContactInfo;
        Label lblBugEmail;
        Label lblLocation;
        TextBox txtServiceName;
        TextBox txtCodebaseUrl;
        TextBox txtServiceUrl;
        TextBox txtInfoUrl;
        TextBox txtServiceGuid;
        TextBox txtDomainServer;
        TextBox txtDomainGuid;
        TextBox txtOutPasskey;
        TextBox txtDescription;
        TextBox txtContactInfo;
        TextBox txtBugEmail;
        TextBox txtLocation;
      
        Button btnGuid;
        Button btnModify;
        Button btnRefresh;
        Button btnClear;
        Button btnRetire;
        Button btnSave;
        HiddenField hdnServiceName;
        HiddenField hdnCodebaseUrl;
        HiddenField hdnServiceUrl;
        HiddenField hdnServiceGuid;
        #endregion

 


        #region pageCode



         ////btnSave.Enabled = false;
         //           //btnSave.Visible = false;
         //           if (AgentType.Equals(ProcessAgentType.SERVICE_BROKER))
         //           {
         //               trDomainSB.Visible = false;
         //               IntTag[] pas = dbTicketing.GetProcessAgentTags();
         //               // If additional processAgents are registered the Fields may not be modified
         //               SetFormMode(pas != null && pas.Length > 1);
         //           }
         //           else
         //           {
         //               // Check to see if a ServiceBroker is registered
         //               trDomainSB.Visible = true;
         //               ProcessAgentInfo sbInfo = dbTicketing.GetServiceBrokerInfo();
         //               if (sbInfo != null)
         //               {

         //                   txtDomainServer.Text = sbInfo.ServiceUrl;

         //                   // May not modify any fields that define the service since
         //                   // It is registered with its domainServiceBroker

         //                   SetFormMode(true);

         //               }
         //               else
         //               {
         //                   // May modify fields that define the service since
         //                   // It is not registered registered with a domainServiceBroker
         //                   message.Append("A domain ServiceBroker has not been registered!");
         //                   lblResponse.Text = Utilities.FormatWarningMessage(message.ToString());
         //                   lblResponse.Visible = true;
         //                   btnGuid.Enabled = true;
         //                   btnGuid.Visible = true;
         //                  Logger.WriteLine("administration: DomainServerNotFound");
         //                   SetFormMode(false);
         //               }
         //           }

        protected override void OnLoad(EventArgs e)
        {
            
            if (!Page.IsPostBack)
            {
                StringBuilder message = new StringBuilder();
                

                ProcessAgent pai = dbTicketing.GetSelfProcessAgent();// ProcessAgentDB.ServiceAgent;
                if (pai != null)
                {
                    AgentType = pai.type;
                    AgentName = pai.agentName;
                    AgentGuid = pai.agentGuid;
                    CodebaseUrl = pai.codeBaseUrl;
                    WebServiceUrl = pai.webServiceUrl;

                    if (!(pai.type == ProcessAgentType.SERVICE_BROKER || pai.type == ProcessAgentType.BATCH_SERVICE_BROKER))
                    {
                        if (pai.domainGuid != null)
                        {
                            DomainGuid = pai.domainGuid;
                            IntTag tag = dbTicketing.GetProcessAgentTag(pai.domainGuid);
                            if (tag != null && tag.tag != null)
                                DomainServer = tag.tag;
                            HasDomain = true;
                        }
                        else
                        {
                            HasDomain = false;
                        }
                    }
                    else
                    {
                        IntTag[] paTags = dbTicketing.GetProcessAgentTags();
                        if (paTags != null && paTags.Length > 1)
                        {
                            HasDomain = true;
                        }
                        else
                        {
                            HasDomain = false;
                        }
                    }
                    SystemSupport ss = dbTicketing.RetrieveSystemSupport(pai.agentGuid);
                    if (ss != null)
                    {
                        Description = ss.description;
                        InfoUrl = ss.infoUrl;
                        ContactEmail = ss.contactEmail;
                        BugEmail = ss.bugEmail;
                        Location = ss.location;
                    }
                }
                else
                {
                    message.Append("The self Registration information has not been saved to the database.");
                    message.Append(" Displaying default values from Web.Config. Please modify & save.<br/>");
                    message.Append(" You should use the fully qualified hostname or IP adress inplace of the default 'localhost'.");

                    // Need to call selfRegister
                    if(AgentType == null)
                        AgentType = ConfigurationManager.AppSettings["serviceType"];
                    string serviceGUID = ConfigurationManager.AppSettings["serviceGUID"];
                    if (serviceGUID != null)
                        AgentGuid = serviceGUID;

                    string serviceURL = ConfigurationManager.AppSettings["serviceURL"];
                    if (serviceURL != null)
                        WebServiceUrl = serviceURL;

                    string serviceName = ConfigurationManager.AppSettings["serviceName"];
                    if (serviceName != null)
                        AgentName = serviceName;

                    StringBuilder buf = new StringBuilder();
                    buf.Append(this.Page.Request.Url.Scheme + "://");
                    buf.Append(this.Page.Request.Url.Host);
                    if (!this.Page.Request.Url.IsDefaultPort)
                    {
                        buf.Append(":" + this.Page.Request.Url.Port);
                    }
                    buf.Append(this.Page.Request.ApplicationPath);
                     CodebaseUrl = buf.ToString();
                  
                    string str = ConfigurationManager.AppSettings["supportMailAddress"];
                    if (str != null)
                        ContactEmail = str;
                    str = ConfigurationManager.AppSettings["bugReportMailAddress"];
                    if (str != null)
                        BugEmail = str;
                   
                    Response = Utilities.FormatWarningMessage(message.ToString());
                    ResponseVisible = true;
                    
                }
            }
        }


   
        protected void SetFormMode(bool hasDomain)
        {
            lblResponse.Text = Response;
            lblResponse.Visible = ResponseVisible;
            if (hasDomain)
            {

                txtServiceName.Attributes.Add("onChange", "EnableServiceName();");
                txtCodebaseUrl.Attributes.Add("onChange", "EnableCodeBase();");
                txtServiceUrl.Attributes.Add("onChange", "EnableServiceUrl();");

            }
            else
            {
                txtServiceName.Attributes.Remove("onChange");
                txtCodebaseUrl.Attributes.Remove("onChange");
                txtServiceUrl.Attributes.Remove("onChange");

            }
            txtServiceGuid.ReadOnly = hasDomain;
            txtServiceGuid.BackColor = hasDomain ? disabled : enabled;
            btnGuid.Visible = !hasDomain;
            btnGuid.Enabled = !hasDomain;

            btnSave.Visible = !hasDomain;
            btnSave.Enabled = !hasDomain;
            btnModify.Visible = hasDomain;
            btnModify.Enabled = hasDomain;
            btnRetire.Visible = hasDomain;
        }


        protected void btnGuid_Click(object sender, System.EventArgs e)
        {
            
           AgentGuid = Utilities.MakeGuid();
           txtServiceGuid.Text = AgentGuid;
        }
        protected Byte[] parseToUTF8(string str){
             UTF8Encoding utf8 = new UTF8Encoding(false, true);
                Byte[] bytes = null;
                try
                {
                    bytes = utf8.GetBytes(str);
                }
                catch (Exception e)
                {
                    throw new Exception("There are illegal characters in the string.",e);
                }
            return bytes;
        }

        protected bool checkGuid()
        {
            bool status = false;
            StringBuilder buf = new StringBuilder();
            if (txtServiceGuid.Text == null || txtServiceGuid.Text.Trim().Length == 0)
            {
                throw new Exception("You must enter a GUID string!");
            }else{
                Byte[] bytes = null;
                try{
                    
                    bytes = parseToUTF8(txtServiceGuid.Text.Trim());
                }
                catch(Exception e){
                    throw new Exception("Error on GUID string: " + e.Message);
                }
                if(bytes.Length > maxGuidLength){
                    throw new Exception("The GUID string has too many characters. Max length is " + maxGuidLength);
                }
            }
            status = true;
            return status;
        }

        protected void btnModify_Click(object sender, System.EventArgs e)
        {
            bool error = false;
            StringBuilder message = new StringBuilder();
            try
            {
                if (ProcessAgentDB.ServiceAgent != null)
                {
                    string originalGuid = ProcessAgentDB.ServiceAgent.agentGuid;
                    if (!(txtServiceName.Text != null && txtServiceName.Text.Length > 0))
                    {
                        error = true;
                        message.Append(" You must enter a Service Name<br/>");
                    }
                    try
                    {
                        if (!checkGuid())
                        {
                            error = true;
                            message.Append("There is an unspecified error with the GUID string.<br/>");
                        }
                    }
                    catch (Exception ex)
                    {
                        error = true;
                        message.Append("GUID Error: ");
                        message.Append(ex.Message);
                        message.Append("<br/>");
                    }
                    if (!(txtOutPasskey.Text != null && txtOutPasskey.Text.Length > 0))
                    {
                        error = true;
                        message.Append(" You must enter a default passkey<br/>");
                    }
                    if (!(txtCodebaseUrl.Text != null && txtCodebaseUrl.Text.Length > 0))
                    {
                        error = true;
                        message.Append(" You must enter the base URL for the Web Site<br/>");
                    }
                    else if (txtCodebaseUrl.Text.Contains("localhost"))
                    {
                        error = true;
                        message.Append(" You must not use localhost in a codebase URL, if you must test on the local machine please use '127.0.0.1'.<br/>");
                    }
                    if (!(txtServiceUrl.Text != null && txtServiceUrl.Text.Length > 0))
                    {
                        error = true;
                        message.Append(" You must enter the web Ssrvice URL for the Web Site<br/>");
                    }
                    else if (txtServiceUrl.Text.Contains("localhost"))
                    {
                        error = true;
                        message.Append(" You must not use localhost in a web service URL, if you must test only on the local machine please use '127.0.0.1'.<br/>");
                    }
                    else
                    { // Test for valid webService URL
                        ProcessAgentProxy paProxy = new ProcessAgentProxy();
                        paProxy.Url = txtServiceUrl.Text.Trim();
                        try
                        {
                            DateTime serTime = paProxy.GetServiceTime();
                        }
                        catch
                        {
                                error = true;
                                message.Append(" There is an error with the web service URL: " + txtServiceUrl.Text.Trim() + " Please check that it is valid and the web service is configured correctly.<br/>");
                        }
                    }
                    if (error)
                    {
                        lblResponse.Text = Utilities.FormatErrorMessage(message.ToString());
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
                            pa.agentGuid = txtServiceGuid.Text;
                            pa.agentName = txtServiceName.Text;
                            pa.domainGuid = ProcessAgentDB.ServiceAgent.domainGuid;
                            pa.codeBaseUrl = txtCodebaseUrl.Text;
                            pa.webServiceUrl = txtServiceUrl.Text;
                            pa.type = AgentType;
                            //dbTicketing.SelfRegisterProcessAgent(pa.agentGuid, pa.agentName, agentType,
                            //    pa.domainGuid, pa.codeBaseUrl, pa.webServiceUrl);
                            //message.Append("Local information has been saved. ");
                            int returnValue = psProxy.ModifyProcessAgent(originalGuid, pa, null);
                            message.Append("The changes have been sent to the ServiceBroker");
                            if (returnValue > 0)
                            {
                                dbTicketing.SelfRegisterProcessAgent(pa.agentGuid, pa.agentName, AgentType,
                                pa.domainGuid, pa.codeBaseUrl, pa.webServiceUrl);

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
                        string tmpGuid = null;
                        if (AgentType == ProcessAgentType.SERVICE_BROKER)
                        {
                            tmpGuid = ProcessAgentDB.ServiceAgent.agentGuid;
                            AuthorityUpdateSelf(txtServiceName.Text,ProcessAgentDB.ServiceAgent.agentGuid,
                                 txtCodebaseUrl.Text.Trim(), txtDescription.Text, txtContactInfo.Text,
                                 txtBugEmail.Text, txtLocation.Text);
                        }
                        dbTicketing.SelfRegisterProcessAgent(ProcessAgentDB.ServiceAgent.agentGuid, txtServiceName.Text, AgentType,
                                tmpGuid, txtCodebaseUrl.Text.Trim(), txtServiceUrl.Text.Trim());
                        dbTicketing.SaveSystemSupport(ProcessAgentDB.ServiceAgent.agentGuid, txtContactInfo.Text, txtBugEmail.Text,
                           txtInfoUrl.Text, txtDescription.Text, txtLocation.Text);
                    }
                    ProcessAgentDB.RefreshServiceAgent();
                //    if (txtOutPasskey.Text.CompareTo(ConfigurationManager.AppSettings["defaultPasskey"]) != 0)
                //    {
                //        ConfigurationManager.AppSettings.Set("defaultPasskey", txtOutPasskey.Text);
                //    }
                }
            }
            catch (Exception ex)
            {
                Exception ex2 = new Exception("Error in  selfRegistration.modify()", ex);
               Logger.WriteLine(Utilities.DumpException(ex2));
                throw ex2;
            }
        }


        protected void btnSave_Click(object sender, System.EventArgs e)
        {

            bool error = false;
            string webURL = null;
            StringBuilder message = new StringBuilder();
            //Check fields for valid input
            if (!(txtServiceName.Text != null && txtServiceName.Text.Length > 0))
            {
                error = true;
                message.Append(" You must enter a Service Name<br/>");
            }
            try
            {
                if (!checkGuid())
                {
                    error = true;
                    message.Append("There is an unspecified error with the GUID string.<br/>");
                }
            }
            catch (Exception exc)
            {
                error = true;
                message.Append("GUID Error: ");
                message.Append(exc.Message);
                message.Append("<br/>");
            }
            if (!(txtOutPasskey.Text != null && txtOutPasskey.Text.Length > 0))
            {
                error = true;
                message.Append(" You must enter a default passkey<br/>");
            }
            if (!(txtCodebaseUrl.Text != null && txtCodebaseUrl.Text.Length > 0))
            {
                error = true;
                message.Append(" You must enter the base URL for the Web Site<br/>");
            }
            else if (txtCodebaseUrl.Text.Contains("localhost"))
            {
                error = true;
                message.Append(" You must not use localhost in a codebase URL, if you must test on the local machine please use '127.0.0.1'.<br/>");
            }
            if (!(txtServiceUrl.Text != null && txtServiceUrl.Text.Length > 0))
            {
                error = true;
                message.Append(" You must enter full or relative URL of the Web Service page<br/>");
            }
            else if (txtServiceUrl.Text.Contains("localhost"))
            {
                error = true;
                message.Append(" You must not use localhost in a web service URL, if you must test only on the local machine please use '127.0.0.1'.<br/>");
            }
            else
            { 
                //Construct webServiceUrl
                
                string testURL = txtServiceUrl.Text.Trim();
                if (testURL.StartsWith("http://", StringComparison.OrdinalIgnoreCase)
                    || testURL.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                {
                    webURL = testURL;
                }
                else if (testURL.StartsWith("~/", StringComparison.OrdinalIgnoreCase))
                {
                    webURL = txtCodebaseUrl.Text.Trim() + testURL.Substring(1);
                }
                else if (testURL.StartsWith("/", StringComparison.OrdinalIgnoreCase))
                {
                    webURL = txtCodebaseUrl.Text.Trim() + testURL;
                }
                else
                {
                    webURL = txtCodebaseUrl.Text.Trim() + "/" + testURL;
                }
                
                
                // Test for valid webService URL
                ProcessAgentProxy paProxy = new ProcessAgentProxy();
                paProxy.Url = webURL;
                try
                {
                    DateTime serviceTime = paProxy.GetServiceTime();
                }
                catch
                {
                    error = true;
                    message.Append(" There is an error with the web service URL: " + webURL + " Please check that it is valid and the web service is configured correctly.<br/>");
                }
            }
            if (error)
            {
                lblResponse.Text = Utilities.FormatErrorMessage(message.ToString());
                lblResponse.Visible = true;
                return;
            }
            else
            {
                //if(txtOutPasskey.Text.CompareTo(ConfigurationManager.AppSettings["defaultPasskey"]) != 0){
                //    ConfigurationManager.AppSettings.Set("defaultPasskey",txtOutPasskey.Text);
                //}

                // Check if domain is set if so only update mutable Fields
                dbTicketing.SelfRegisterProcessAgent(txtServiceGuid.Text.Trim(),
                    txtServiceName.Text, lblServiceType.Text, null,
                    txtCodebaseUrl.Text.Trim(), webURL);
                txtServiceUrl.Text = webURL;
                if (AgentType == ProcessAgentType.SERVICE_BROKER)
                {
                    dbTicketing.SetDomainGuid(txtServiceGuid.Text.Trim());
                    AuthorityUpdateSelf(txtServiceName.Text, txtServiceGuid.Text.Trim(),
                                 txtCodebaseUrl.Text.Trim(), txtDescription.Text, txtContactInfo.Text,
                                 txtBugEmail.Text, txtLocation.Text);
                }
                dbTicketing.SaveSystemSupport(ProcessAgentDB.ServiceAgent.agentGuid, txtContactInfo.Text, txtBugEmail.Text,
                            txtInfoUrl.Text, txtDescription.Text, txtLocation.Text);

                //DisplayForm();
                lblResponse.Text = Utilities.FormatConfirmationMessage("Self registration has completed!");
                lblResponse.Visible = true;
                ProcessAgentDB.RefreshServiceAgent();
            }
        }

        protected void btnRetire_Click(object sender, System.EventArgs e)
        {
            bool error = false;
            StringBuilder message = new StringBuilder();
            try
            {
                if (ProcessAgentDB.ServiceAgent.domainGuid != null)
                {
                    ProcessAgentInfo originalAgent = dbTicketing.GetProcessAgentInfo(ProcessAgentDB.ServiceAgent.agentGuid);
                    ProcessAgentInfo sb = dbTicketing.GetProcessAgentInfo(ProcessAgentDB.ServiceAgent.domainGuid);
                    if ((sb != null) && !sb.retired)
                    {
                        ProcessAgentProxy psProxy = new ProcessAgentProxy();
                        AgentAuthHeader header = new AgentAuthHeader();
                        header.agentGuid = originalAgent.agentGuid;
                        header.coupon = sb.identOut;
                        psProxy.AgentAuthHeaderValue = header;
                        psProxy.Url = sb.webServiceUrl;

                        int returnValue = psProxy.RetireProcessAgent(originalAgent.domainGuid,
                            originalAgent.agentGuid, true);
                        message.Append("The changes have been sent to the ServiceBroker");
                        if (returnValue > 0)
                        {
                            dbTicketing.SetDomainGuid(null);
                            dbTicketing.SetSelfState(originalAgent.agentGuid, false);
                            dbTicketing.SetProcessAgentRetired(originalAgent.agentGuid, true);
                            dbTicketing.DeleteTickets(originalAgent.agentGuid);

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
                    else
                    {
                        dbTicketing.SelfRegisterProcessAgent(ProcessAgentDB.ServiceAgent.agentGuid, txtServiceName.Text, AgentType,
                                null, txtCodebaseUrl.Text, txtServiceUrl.Text);
                    }
                }
            }
            catch (Exception ex)
            {
                Exception ex2 = new Exception("Error in  selfRegistration.modify()", ex);
               Logger.WriteLine(Utilities.DumpException(ex2));
                throw ex2;
            }
            clearForm();
        }


        protected void clearForm()
        {
            lblResponse.Text = "";
            lblResponse.Visible = false;
            txtServiceName.Text = "";
            txtServiceGuid.Text = "";
            txtCodebaseUrl.Text = "";
            txtServiceUrl.Text = "";
            txtDomainServer.Text = "";
            txtDomainGuid.Text = "";
            txtOutPasskey.Text = "";
            txtDescription.Text = "";
            txtInfoUrl.Text = "";
            txtContactInfo.Text = "";
            txtBugEmail.Text = "";
            txtLocation.Text = "";
            
        }

        protected void btnClear_Click(object sender, System.EventArgs e)
        {
            clearForm();
            btnClear.Enabled = false;
        }

        protected void btnRefresh_Click(object sender, System.EventArgs e)
        {
            //DisplayForm();
            btnClear.Enabled = true;
        }

        /// <summary>
        /// This is a special case that must only be called if the if the Service is a ServiceBroker
        /// </summary>
        /// <param name="name"></param>
        /// <param name="guid"></param>
        /// <param name="url"></param>
        /// <param name="description"></param>
        /// <param name="contactEmail"></param>
        /// <param name="bugEmail"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        protected int AuthorityUpdateSelf(string name, string guid, string url, string description,
           string contactEmail, string bugEmail, string location)
        {
            int count = 0;
            DbConnection connection = FactoryDB.GetConnection();
            DbCommand cmd = FactoryDB.CreateCommand("Authority_UpdateSelf", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            // populate stored procedure parameters
          
            cmd.Parameters.Add(FactoryDB.CreateParameter("@authorityName", name, DbType.String, 256));
            cmd.Parameters.Add(FactoryDB.CreateParameter("@description", description, DbType.String, 512));
            cmd.Parameters.Add(FactoryDB.CreateParameter("@authorityURL", url, DbType.String, 512));
            cmd.Parameters.Add(FactoryDB.CreateParameter("@authorityGuid", guid, DbType.AnsiString, 50));
            cmd.Parameters.Add(FactoryDB.CreateParameter("@contactEmail", contactEmail, DbType.String, 256));
            cmd.Parameters.Add(FactoryDB.CreateParameter("@bugEmail", bugEmail, DbType.String, 256));
            cmd.Parameters.Add(FactoryDB.CreateParameter("@location", location, DbType.String, 256));
            try
            {
                // read the result
                connection.Open();
                object result = cmd.ExecuteScalar();
                if (result != null)
                    count = Convert.ToInt32(result);
            }
            catch (DbException e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                // close the sql connection
                connection.Close();
            }
            return count;
        }

        #endregion

        #region render

        //protected override void Render(HtmlTextWriter output)
        //{
        //    StringBuilder code = new StringBuilder();
        //    code.AppendLine("<script>");
        //    code.AppendLine("function EnableServiceName() {");
        //    code.AppendLine("var msg= 'Are you sure you want to modify the Web Service Name?" + "';");
        //    code.AppendLine("if (confirm(msg)) document.getElementById('btnModifyService').disabled = false;");
        //    code.AppendLine("else{ document.getElementById('txtServiceName').value = document.getElementById('bakServiceName').value;}}");
        //    code.AppendLine();
        //    code.AppendLine("function EnableCodeBase() {");
        //    code.AppendLine("var msg= 'Are you sure you want to modify the CodeBase for this service?" + "';");
        //    code.AppendLine("if (confirm(msg))document.getElementById('btnModifyService').disabled = false;");
        //    code.AppendLine("else{document.getElementById('txtCodebaseUrl').value = document.getElementById('bakCodebase').value;}}");
        //    code.AppendLine();
        //    code.AppendLine("function EnableServiceUrl() {");
        //    code.AppendLine("var msg= 'Are you sure you want to modify the Web Service URL?" + "';");
        //    code.AppendLine("if (confirm(msg))document.getElementById('btnModifyService').disabled = false;");
        //    code.AppendLine("else{document.getElementById('txtWebServiceUrl').value = document.getElementById('bakServiceUrl').value;}}");
        //    code.AppendLine();
        //    code.AppendLine("function ConfirmRetire() {");
        //    code.AppendLine("var msg= 'Are you sure you want to retire this WebService.\\nIf you proceed all references to this site will be retired';");
        //    code.AppendLine("var state = confirm(msg);return state;}");
        //    code.AppendLine("</script>");
        //    output.Write(code.ToString());
        //    base.Render(output);
        //}


        //protected override void Render(HtmlTextWriter output)
        //{
        //    EnsureChildControls();
        //    DisplayForm();
        //    RenderContents(output);
        //}
        protected override void CreateChildControls()
        {
            Controls.Clear();
            CreateControlHierarchy();
            ClearChildViewState();
        }

        protected void CreateControlHierarchy()
        {
           
            pageintro = new HtmlGenericControl("div");
            pageintro.ID ="pageintro";
            lblTitle = new Label();
            lblTitle.Text = "Self Registration for " +  AgentType;
            lblTitle.ID = "lblTitle";
            lblTitle.Visible = true;
            lblResponse = new Label();
            lblResponse.ID = "lblResponse";
            lblResponse.Text = Response;
            lblResponse.Visible = ResponseVisible;

            lblIntroduction = new Label();
            lblIntroduction.ID = "lblIntroduction";
            lblIntroduction.Text = Intro;
            lblIntroduction.Visible = true;
            lblServiceType = new Label();
            lblServiceType.ID = "lblServiceType";
            lblServiceType.Text = AgentType;
            HtmlGenericControl h1 = new HtmlGenericControl("h2");
            h1.Controls.Add(lblTitle);
            pageintro.Controls.Add(h1);
            
            HtmlGenericControl p = new HtmlGenericControl("p");
            p.Controls.Add(lblIntroduction);
            pageintro.Controls.Add(p);
            if (ResponseVisible)
            {
                p = new HtmlGenericControl("p");
                p.Controls.Add(lblResponse);
                pageintro.Controls.Add(p);
            }
            Controls.Add(pageintro);

            pagecontent = new HtmlGenericControl("div");
            pagecontent.ID = "pagecontent";

            //frmRegister = new HtmlForm();
            //frmRegister.Method = "post";
            //frmRegister.Target = "";
            //frmRegister.Name = "RegisterSelf";
            //frmRegister.ID = "frmRegister";
            
            tblMain = new Table();
            tblMain.CssClass = "simpleform";
            tblMain.Width = width;
            
            Literal spacer = new Literal();
            spacer.Text = "&nbsp;&nbsp;";
            Style s1 = new Style();
            s1.Width = labelWidth;
            s1.Height = onePX;
            s1.ForeColor = ForeColor;
            Style s2 = new Style();
            s2.Width = guidWidth;
            s2.ForeColor = BorderColor;
           
            Style s3 = new Style();
            s3.Width = buttonColumnWidth;
            s3.ForeColor = ForeColor;
            Style s4 = new Style();
            s4.Width = dataWidth;

            Style txtAreaStyle = new Style();
            //txtAreaStyle.Height = txtBoxHeight;
            txtAreaStyle.Width = dataWidth;
            txtAreaStyle.BorderColor = BorderColor;

            Style txtBoxStyle = new Style();
            txtBoxStyle.Height = txtBoxHeight;
            txtBoxStyle.Width = dataWidth;
            txtBoxStyle.BorderColor = BorderColor;

            Style txtGuidStyle = new Style();
            txtGuidStyle.Height = txtBoxHeight;
            txtGuidStyle.Width = guidWidth;
            txtGuidStyle.BorderColor = BorderColor;


            TableRow row;
            TableHeaderCell th;
            TableCell td;
            TableCell td2;
            //Label lbl;
            //Create the first row
            row = new TableRow();
            th = new TableHeaderCell();
            th.ApplyStyle(s1);
            td = new TableCell();
            td.ApplyStyle(s2);
            td2 = new TableCell();
            td2.ApplyStyle(s3);
            row.Cells.Add(th);
            row.Cells.Add(td);
            row.Cells.Add(td2);
            tblMain.Rows.Add(row);

            row = new TableRow();
            th = new TableHeaderCell();
            th.ColumnSpan = 3;
            th.Style.Add(HtmlTextWriterStyle.TextAlign, "center");
            th.Text = "Required Credential Information";
            row.Cells.Add(th);
            tblMain.Rows.Add(row);

            row = new TableRow();
            th = new TableHeaderCell(); 
            lblServiceName = new Label();
            lblServiceName.ID = "lblServiceName";
            lblServiceName.Text = "Service Name";
            th.Controls.Add(lblServiceName);
            td = new TableCell();
            td.ColumnSpan = 2;
            txtServiceName = new TextBox();
            txtServiceName.ID = "txtServiceName";
            txtServiceName.ApplyStyle(txtBoxStyle);
            txtServiceName.Text = AgentName;
            td.Controls.Add(txtServiceName);
            row.Cells.Add(th);
            row.Cells.Add(td);
            tblMain.Rows.Add(row);

            row = new TableRow();
            th = new TableHeaderCell();
            lblServiceGuid = new Label();
            lblServiceGuid.ID = "lblServiceGuid";
            lblServiceGuid.Text = "Service GUID";
            th.Controls.Add(lblServiceGuid);

            td = new TableCell();
            td.ColumnSpan = 1;
            txtServiceGuid = new TextBox();
            txtServiceGuid.ID = "txtServiceGuid";
            txtServiceGuid.ApplyStyle(txtGuidStyle);
            txtServiceGuid.Text = AgentGuid;
            td.Controls.Add(txtServiceGuid);
            td2 = new TableCell();
            td2.ApplyStyle(s3);
            btnGuid = new Button();
            btnGuid.Text = "Create GUID";
            btnGuid.ID = "btnGuid";
            btnGuid.Click += btnGuid_Click;
            btnGuid.Enabled = false;
            td2.Controls.Add(btnGuid);
            row.Cells.Add(th);
            row.Cells.Add(td);
            row.Cells.Add(td2);
            tblMain.Rows.Add(row);

            row = new TableRow();
            th = new TableHeaderCell();
            lblCodebaseUrl = new Label();
            lblCodebaseUrl.ID = "lblCodebaseUrl";
            lblCodebaseUrl.Text = "Codebase URL";
            th.Controls.Add(lblCodebaseUrl);
         
            td = new TableCell();
            td.ColumnSpan = 2;
            txtCodebaseUrl = new TextBox();
            txtCodebaseUrl.ID = "txtCodebaseUrl";
            txtCodebaseUrl.ApplyStyle(txtBoxStyle);
            txtCodebaseUrl.Text = CodebaseUrl;
            td.Controls.Add(txtCodebaseUrl);
            row.Cells.Add(th);
            row.Cells.Add(td);
            tblMain.Rows.Add(row);

            row = new TableRow();
            th = new TableHeaderCell();
            lblServiceUrl = new Label();
            lblServiceUrl.ID = "lblServiceUrl";
            lblServiceUrl.Text = "Web Service URL";
            th.Controls.Add(lblServiceUrl);
            
            td = new TableCell();
            td.ColumnSpan = 2;
            txtServiceUrl = new TextBox();
            txtServiceUrl.ID = "txtServiceUrl";
            txtServiceUrl.ApplyStyle(txtBoxStyle);
            txtServiceUrl.Text = WebServiceUrl;
            td.Controls.Add(txtServiceUrl);
            row.Cells.Add(th);
            row.Cells.Add(td);
            tblMain.Rows.Add(row);

            trDomainSB = new TableRow();
            trDomainSB.ID = "trDomainSB";
            th = new TableHeaderCell();
            lblDomainServer = new Label();
            lblDomainServer.ID = "lblDomainServer";
            lblDomainServer.Text = "Domain ServiceBroker";
            th.Controls.Add(lblDomainServer);
           
            td = new TableCell();
            td.ColumnSpan = 2;
            txtDomainServer = new TextBox();
            txtDomainServer.ApplyStyle(txtBoxStyle);
            txtDomainServer.Text = DomainServer;
            txtDomainServer.Enabled = false;
            td.Controls.Add(txtDomainServer);
            trDomainSB.Cells.Add(th);
            trDomainSB.Cells.Add(td);
            tblMain.Rows.Add(trDomainSB);

            trDomainSB.Visible = (AgentType == ProcessAgentType.SERVICE_BROKER || AgentType == ProcessAgentType.BATCH_SERVICE_BROKER) ? false :true;

            trDomainGuid = new TableRow();
            trDomainGuid.ID = "trDomainGuid";
            th = new TableHeaderCell();
            lblDomainGuid = new Label();
            lblDomainGuid.ID = "lblDomainGuid";
            lblDomainGuid.Text = "Domain Guid";
            th.Controls.Add(lblDomainGuid);

            td = new TableCell();
            td.ColumnSpan = 2;
            txtDomainGuid = new TextBox();
            txtDomainGuid.ApplyStyle(txtBoxStyle);
            txtDomainGuid.Text = DomainGuid;
            txtDomainGuid.Enabled = false;
            td.Controls.Add(txtDomainGuid);
            trDomainGuid.Cells.Add(th);
            trDomainGuid.Cells.Add(td);
            tblMain.Rows.Add(trDomainGuid);

            trDomainGuid.Visible = (AgentType == ProcessAgentType.SERVICE_BROKER || AgentType == ProcessAgentType.BATCH_SERVICE_BROKER) ? false : true;


            trRowPasskey = new TableRow();
            th = new TableHeaderCell();
            lblOutPassKey = new Label();
            lblOutPassKey.ID = "lblOutPassKey";
            lblOutPassKey.Text = "Install Credential Passkey";
            th.Controls.Add(lblOutPassKey);
           
            td = new TableCell();
            td.ColumnSpan = 2;
            txtOutPasskey = new TextBox();
            txtOutPasskey.ApplyStyle(txtBoxStyle);
            txtOutPasskey.Text = ConfigurationManager.AppSettings["defaultPasskey"];
            txtOutPasskey.Enabled = false;
            txtOutPasskey.ReadOnly = true;
            txtOutPasskey.ToolTip = "Default passkey may only be modified by editing the web.config file.";
            
            td.Controls.Add(txtOutPasskey);
            td2 = new TableCell();
            td2.ApplyStyle(s3);
            trRowPasskey.Cells.Add(th);
            trRowPasskey.Cells.Add(td);
            tblMain.Rows.Add(trRowPasskey);

            row = new TableRow();
            th = new TableHeaderCell();
            th.ColumnSpan = 3;
            th.Style.Add(HtmlTextWriterStyle.TextAlign, "center");
            th.Text = "Optional Information";
            row.Cells.Add(th);
            tblMain.Rows.Add(row);

            row = new TableRow();
            th = new TableHeaderCell();
            lblDescription = new Label();
            lblDescription.ID = "lblDescription";
            lblDescription.Text = "Description";
            th.Controls.Add(lblDescription);

            td = new TableCell();
            td.ColumnSpan = 2;
            txtDescription = new TextBox();
            txtDescription.TextMode = TextBoxMode.MultiLine;
            txtDescription.Columns = 50;
            txtDescription.Rows = 4;
            
            txtDescription.Wrap = true;
            txtDescription.ApplyStyle(txtAreaStyle);
            txtDescription.Text = Description;
            td.Controls.Add(txtDescription);
            row.Cells.Add(th);
            row.Cells.Add(td);
            tblMain.Rows.Add(row);

            row = new TableRow();
            th = new TableHeaderCell();
            lblInfoUrl = new Label();
            lblInfoUrl.ID = "lblInfoUrl";
            lblInfoUrl.Text = "Information URL";
            th.Controls.Add(lblInfoUrl);

            td = new TableCell();
            td.ColumnSpan = 2;
            txtInfoUrl = new TextBox();
            txtInfoUrl.ApplyStyle(txtBoxStyle);
            txtInfoUrl.Text = InfoUrl;
            td.Controls.Add(txtInfoUrl);
            row.Cells.Add(th);
            row.Cells.Add(td);
            tblMain.Rows.Add(row);

            row = new TableRow();
            th = new TableHeaderCell();
            lblContactInfo = new Label();
            lblContactInfo.ID = "lblContactInfo";
            lblContactInfo.Text = "Contact Email";
            th.Controls.Add(lblContactInfo);
           
            td = new TableCell();
            td.ColumnSpan = 2;
            txtContactInfo = new TextBox();
            txtContactInfo.ApplyStyle(txtBoxStyle);
            txtContactInfo.Text = ContactEmail;
            td.Controls.Add(txtContactInfo);
            row.Cells.Add(th);
            row.Cells.Add(td);
            tblMain.Rows.Add(row);

            row = new TableRow();
            th = new TableHeaderCell();
            lblBugEmail = new Label();
            lblBugEmail.ID = "lblBugEmail";
            lblBugEmail.Text = "Bug Email";
            th.Controls.Add(lblBugEmail);

            td = new TableCell();
            td.ColumnSpan = 2;
            txtBugEmail = new TextBox();
            txtBugEmail.ApplyStyle(txtBoxStyle);
            txtBugEmail.Text = BugEmail;
            td.Controls.Add(txtBugEmail);
            row.Cells.Add(th);
            row.Cells.Add(td);
            tblMain.Rows.Add(row);

            row = new TableRow();
            th = new TableHeaderCell();
            lblLocation = new Label();
            lblLocation.ID = "lblLocation";
            lblLocation.Text = "Location";
            th.Controls.Add(lblLocation);

            td = new TableCell();
            td.ColumnSpan = 2;
            txtLocation = new TextBox();
            txtLocation.ApplyStyle(txtBoxStyle);
            txtLocation.Text = Location;
            td.Controls.Add(txtLocation);
            row.Cells.Add(th);
            row.Cells.Add(td);
            tblMain.Rows.Add(row);


            row = new TableRow();
            th = new TableHeaderCell();
            th.ColumnSpan = 2;

            btnSave = new Button();
            btnSave.ID = "btnSave";
            btnSave.Text = "Save Service";
            btnSave.CommandName = "Save";
            btnSave.Click += btnSave_Click;
            btnSave.Enabled = false;
            th.Controls.Add(btnSave);  

            btnModify = new Button();
            btnModify.ID = "btnModify";
            btnModify.Text = "Modify Service";
            btnModify.CommandName = "Modify";
            btnModify.Click += btnModify_Click;
            btnModify.OnClientClick = "javascript:return confirm('Are you sure you want to modify this WebService?\\n If you proceed all references to this site will be modified.');";
            btnModify.Enabled = false;
            th.Controls.Add(btnModify);  
            th.Controls.Add(spacer);

            btnRefresh = new Button();
            btnRefresh.Text = "Refresh";
            btnRefresh.CommandName="Refresh";
            btnRefresh.ID= "btnRefresh";
            btnRefresh.Click += btnRefresh_Click;
            th.Controls.Add(btnRefresh);
            th.Controls.Add(spacer);

            btnClear = new Button();
            btnClear.Text = "Clear";
            btnClear.CommandName="Clear";
            btnClear.ID="btnClear";
            btnClear.Click += btnClear_Click;
            btnClear.OnClientClick = "javascript:return confirm('Are you sure you want to clear the page?\\n Are you really sure');";
            th.Controls.Add(btnClear);
            
            th.Controls.Add(spacer);
            btnRetire = new Button();
            btnRetire.ID = "btnRetire";
            btnRetire.Text = "Retire";
            btnRetire.CommandName="Retire";
            btnRetire.Click += btnRetire_Click;
            btnRetire.OnClientClick="javascript:return confirm('Are you sure you want to retire this WebService?\\nIf you proceed all references to this site will be retired');";
                                
            th.Controls.Add(btnRetire);
            td = new TableCell();
            row.Cells.Add(th);
            row.Cells.Add(td);
            tblMain.Rows.Add(row);

            pagecontent.Controls.Add(tblMain);
            hdnServiceGuid = new HiddenField();
            hdnServiceGuid.ID = "bakServiceGuid";
            pagecontent.Controls.Add(hdnServiceGuid);
            hdnServiceName = new HiddenField();
            hdnServiceName.ID = "bakServiceName";
            pagecontent.Controls.Add(hdnServiceName);
            hdnServiceUrl = new HiddenField();
            hdnServiceUrl.ID = "bakServiceUrl";
            pagecontent.Controls.Add(hdnServiceUrl);
            hdnCodebaseUrl = new HiddenField();
            hdnCodebaseUrl.ID = "bakCodebaseUrl";
            pagecontent.Controls.Add(hdnCodebaseUrl);
            //pagecontent.Controls.Add(frmRegister);
            Controls.Add(pagecontent);

            ChildControlsCreated = true;
            SetFormMode(HasDomain);
           
        }

#endregion

      #region PostBack

        //bool LoadPostData(string postDataKey, NameValueCollection postCollection)
        //{
        //    //string currentText = Text;
        //    //string postedText = postCollection[postDataKey];
        //    //if (!currentText.Equals(postedText, StringComparison.Ordinal))
        //    //{
        //    //    Text = postedText;
        //    //    return true;
        //    //} 
        //    return false;
        //}
        //void RaisePostDataChangedEvent()
        //{
        //    return;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventArgument"></param>
        public void RaisePostBackEvent(string eventArgument)
        {
            int i = 0;
            //ModifySelf(this, new ModifySelfEventArgs());
        }

        #endregion

    }

    
}

