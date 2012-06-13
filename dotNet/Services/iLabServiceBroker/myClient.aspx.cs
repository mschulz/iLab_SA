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
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Security;
using System.Xml;

using iLabs.Core;
using iLabs.DataTypes.ProcessAgentTypes;
using iLabs.ServiceBroker;
using iLabs.ServiceBroker.Administration;
using iLabs.ServiceBroker.Authorization;
using iLabs.ServiceBroker.Internal;
using iLabs.UtilLib;


using iLabs.Ticketing;

//using iLabs.Services;
using iLabs.DataTypes.SchedulingTypes;
using iLabs.DataTypes.SoapHeaderTypes;
using iLabs.DataTypes.TicketingTypes;
using iLabs.Proxies.USS;

namespace iLabs.ServiceBroker.iLabSB
{
    /// <summary>
    /// iLab Client Launch Page.
    /// </summary>
    public partial class myClient : System.Web.UI.Page
    {
        //protected System.Web.UI.WebControls.Label lblDebug;
        protected System.Web.UI.HtmlControls.HtmlAnchor urlEmail;
        protected System.Web.UI.HtmlControls.HtmlAnchor urlDocumentation;

        protected LabClient lc;
        protected ProcessAgentInfo labServer;
        protected ClientInfo[] clientInfos;

        AuthorizationWrapperClass wrapper = new AuthorizationWrapperClass();

        public string couponId = null;
        public string passkey = null;
        public string issuerGuid = null;
        public string auto = null;
        Coupon opCoupon = null;

        BrokerDB issuer = new BrokerDB();

        DateTime startExecution;
        long duration = -1;
        bool autoLaunch = false;
        string effectiveGroupName = null;
        int effectiveGroupID = 0;
        protected CultureInfo culture;
        protected int userTZ;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            int groupID = 0;
            Coupon opCoupon = null;
            string groupName = null;
            lblResponse.Visible = false;

            userTZ = Convert.ToInt32(Session["UserTZ"]);
            culture = DateUtil.ParseCulture(Request.Headers["Accept-Language"]);
            lc = null;
            if (Session["ClientID"] != null && Session["ClientID"].ToString().Length > 0)
            {
                lc = wrapper.GetLabClientsWrapper(new int[] { Convert.ToInt32(Session["ClientID"]) })[0];


            }
            if (Session["GroupID"] != null && Session["GroupID"].ToString().Length > 0)
            {
                groupID = Convert.ToInt32(Session["GroupID"]);
            }
            if (groupID > 0 && lc != null)
            {
                setEffectiveGroup(groupID, lc.clientID);

            }
            if (Session["GroupName"] != null && Session["GroupName"].ToString().Length > 0)
            {
                groupName = Session["GroupName"].ToString();

                lblGroupNameTitle.Text = groupName;
                //lblBackToLabs.Text = groupName;
            }
            if (!IsPostBack)
            {
                // retrieve parameters from URL
                couponId = Request.QueryString["coupon_id"];
                passkey = Request.QueryString["passkey"];
                issuerGuid = Request.QueryString["issuer_guid"];
                if (couponId != null && passkey != null && issuerGuid != null)
                {
                    opCoupon = new Coupon(issuerGuid, Int64.Parse(couponId), passkey);
                }
                pReenter.Visible = false;
                btnReenter.Visible = false;
                auto = Request.QueryString["auto"];
                if (auto != null && auto.Length > 0)
                {
                    if (auto.ToLower().Contains("t"))
                    {
                        autoLaunch = true;
                    }
                }

               
            }
            if (lc != null)
            {
                labServer = getLabServer(lc.clientID, effectiveGroupID);
                clientInfos = AdministrativeAPI.ListClientInfos(lc.clientID);
                if (lc.clientType == LabClient.INTERACTIVE_APPLET || lc.clientType == LabClient.INTERACTIVE_HTML_REDIRECT)
                {
                    if (lc.needsScheduling)
                    {
                        Ticket allowExperimentExecutionTicket = null;
                        if (opCoupon != null)
                        {

                            // First check for an Allow Execution Ticket
                            allowExperimentExecutionTicket = issuer.RetrieveTicket(
                                opCoupon, TicketTypes.ALLOW_EXPERIMENT_EXECUTION);
                        }
                        if (allowExperimentExecutionTicket == null)
                        {
                            // Try for a reservation
                            int ussId = issuer.FindProcessAgentIdForClient(lc.clientID, ProcessAgentType.SCHEDULING_SERVER);
                            if (ussId > 0)
                            {
                                ProcessAgent uss = issuer.GetProcessAgent(ussId);


                                // check for current reservation

                                //create a collection & redeemTicket
                                string redeemPayload = TicketLoadFactory.Instance().createRedeemReservationPayload(DateTime.UtcNow, DateTime.UtcNow, Session["UserName"].ToString(), Convert.ToInt32(Session["UserID"]),groupName, lc.clientGuid);
                                if (opCoupon == null)
                                    opCoupon = issuer.CreateCoupon();

                                issuer.AddTicket(opCoupon, TicketTypes.REDEEM_RESERVATION, uss.agentGuid, ProcessAgentDB.ServiceGuid, 600, redeemPayload);

                                UserSchedulingProxy ussProxy = new UserSchedulingProxy();
                                OperationAuthHeader op = new OperationAuthHeader();
                                op.coupon = opCoupon;
                                ussProxy.Url = uss.webServiceUrl;
                                ussProxy.OperationAuthHeaderValue = op;
                                Reservation reservation = ussProxy.RedeemReservation(ProcessAgentDB.ServiceGuid, Session["UserName"].ToString(), labServer.agentGuid, lc.clientGuid);

                                if (reservation != null)
                                {
                                    // Find efective group
                                    setEffectiveGroup(groupID, lc.clientID);

                                    // create the allowExecution Ticket
                                    DateTime start = reservation.Start;
                                    long duration = reservation.Duration;
                                    string payload = TicketLoadFactory.Instance().createAllowExperimentExecutionPayload(
                                        start, duration, effectiveGroupName,lc.clientGuid);
                                    DateTime tmpTime = start.AddTicks(duration * TimeSpan.TicksPerSecond);
                                    DateTime utcNow = DateTime.UtcNow;
                                    long ticketDuration = (tmpTime.Ticks - utcNow.Ticks) / TimeSpan.TicksPerSecond;
                                    allowExperimentExecutionTicket = issuer.AddTicket(opCoupon, TicketTypes.ALLOW_EXPERIMENT_EXECUTION,
                                            ProcessAgentDB.ServiceGuid, ProcessAgentDB.ServiceGuid, ticketDuration, payload);
                                }
                            }

                        }
                        if (allowExperimentExecutionTicket != null)
                        {
                            XmlDocument payload = new XmlDocument();
                            payload.LoadXml(allowExperimentExecutionTicket.payload);
                            startExecution = DateUtil.ParseUtc(payload.GetElementsByTagName("startExecution")[0].InnerText);
                            duration = Int64.Parse(payload.GetElementsByTagName("duration")[0].InnerText);

                            Session["StartExecution"] = DateUtil.ToUtcString(startExecution);
                            Session["Duration"] = duration;

                            //groupId = payload.GetElementsByTagName("groupID")[0].InnerText;


                            // Display reenter button if experiment is reentrant & a current experiment exists
                            if (lc.IsReentrant)
                            {

                                long[] ids = InternalDataDB.RetrieveActiveExperimentIDs(Convert.ToInt32(Session["UserID"]),
                                    effectiveGroupID, labServer.agentId, lc.clientID);
                                if (ids.Length > 0)
                                {
                                    btnLaunchLab.Text = "Launch New Experiment";
                                    btnLaunchLab.Visible = true;
                                    pLaunch.Visible = true;
                                    pReenter.Visible = true;
                                    btnReenter.Visible = true;
                                    btnReenter.CommandArgument = ids[0].ToString();
                                }
                                else
                                {

                                    //pReenter.Visible = false;
                                    //btnReenter.Visible = false;
                                    btnLaunchLab.Text = "Launch Lab";
                                    if (autoLaunch)
                                    {
                                        launchLabClient(lc.clientID);
                                    }
                                    else
                                    {
                                        pLaunch.Visible = true;
                                        btnLaunchLab.Visible = true;
                                    }
                                }
                            }
                            else
                            {
                                pLaunch.Visible = true;
                                btnLaunchLab.Visible = true;
                                if (autoLaunch)
                                {
                                    launchLabClient(lc.clientID);
                                }
                            }
                        }
                        else
                        {
                            pLaunch.Visible = false;
                            btnLaunchLab.Visible = false;
                        }
                    }

                    else
                    {
                        pLaunch.Visible = true;
                        btnLaunchLab.Visible = true;
                        if (autoLaunch)
                        {
                            launchLabClient(lc.clientID);
                        }
                    }
                }
                else if (lc.clientType == LabClient.BATCH_APPLET || lc.clientType == LabClient.BATCH_HTML_REDIRECT)
                {
                    pLaunch.Visible = true;
                    btnLaunchLab.Visible = true;
                    if (autoLaunch)
                    {
                        launchLabClient(lc.clientID);
                    }

                }


                btnSchedule.Visible = lc.needsScheduling;
                lblClientName.Text = lc.clientName;
                lblVersion.Text = lc.version;
                lblLongDescription.Text = lc.clientLongDescription;
                if (lc.notes != null && lc.notes.Length > 0)
                {
                    pNotes.Visible = true;
                    lblNotes.Text = lc.notes;
                }
                else
                {
                    pNotes.Visible = false;
                    lblNotes.Text = null;
                }
                if (lc.contactEmail != null && lc.contactEmail.Length > 0)
                {
                    pEmail.Visible = true;
                    string emailCmd = "mailto:" + lc.contactEmail;
                    lblEmail.Text = "<a href=" + emailCmd + ">" + lc.contactEmail + "</a>";
                }
                else
                {
                    pEmail.Visible = false;
                    lblEmail.Text = null;
                }
                if (lc.documentationURL != null && lc.documentationURL.Length > 0)
                {
                    pDocURL.Visible = true;
                    lblDocURL.Text = "<a href=" + lc.documentationURL + ">" + lc.documentationURL + "</a>";
                }
                else
                {
                    pDocURL.Visible = false;
                    lblDocURL.Text = null;
                }

                btnLaunchLab.Command += new CommandEventHandler(this.btnLaunchLab_Click);
                btnLaunchLab.CommandArgument = lc.clientID.ToString();

                int count = 0;

                if (clientInfos != null && clientInfos.Length > 0)
                {
                    //repClientInfos.DataSource = clientInfos;
                    //repClientInfos.DataBind();
                    foreach (ClientInfo ci in clientInfos)
                    {
                        System.Web.UI.WebControls.Button b = new System.Web.UI.WebControls.Button();
                        b.Visible = true;
                        b.CssClass = "button";
                        b.Text = ci.infoURLName;
                        b.CommandArgument = ci.infoURL;
                        b.CommandName = ci.infoURLName;
                        b.ToolTip = ci.description;
                        b.Command += new CommandEventHandler(this.HelpButton_Click);
                        repClientInfos.Controls.Add(b);
                        repClientInfos.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));
                    }
                }
            }
            else
            {
                // No LabClient
                btnSchedule.Visible = false;
                pLaunch.Visible = false;
                btnLaunchLab.Visible = false;
                string msg = "There are no labs assigned to group: " + Session["GroupName"].ToString() + "!";
                lblResponse.Text = Utilities.FormatErrorMessage(msg);
                lblResponse.Visible = true;
            }
            // System_Messages block

            SystemMessage[] groupMessages = null;
            SystemMessage[] serverMessages = null;
            groupMessages = AdministrativeAPI.SelectSystemMessagesForGroup(Convert.ToInt32(Session["GroupID"]));
            if (lc != null && labServer != null && labServer.agentId > 0)
            {
                serverMessages = wrapper.GetSystemMessagesWrapper(SystemMessage.LAB, 0, 0, labServer.agentId);
            }
            if ((groupMessages == null || groupMessages.Length == 0) && (serverMessages == null || serverMessages.Length == 0))
            {

                lblGroupNameSystemMessage.Text += "No Messages at this time!";
                lblGroupNameSystemMessage.Visible = true;
                lblServerSystemMessage.Visible = false;
            }
            else
            {
                if (groupMessages != null && groupMessages.Length > 0)
                {
                    //lblGroupNameSystemMessage.Text = "Messages for " + groupName;
                    lblGroupNameSystemMessage.Text = "Group Messages:";
                    lblGroupNameSystemMessage.Visible = true;
                    repGroupMessage.DataSource = groupMessages;
                    repGroupMessage.DataBind();
                }
                else
                {
                    lblGroupNameSystemMessage.Visible = false;
                }

                if (serverMessages != null && serverMessages.Length > 0)
                {
                    //lblGroupNameSystemMessage.Text = "Messages for " + groupName;
                    lblServerSystemMessage.Text = "Client/Server Messages:";
                    lblServerSystemMessage.Visible = true;
                    repServerMessage.DataSource = serverMessages;
                    repServerMessage.DataBind();
                }
                else
                {
                    lblServerSystemMessage.Visible = false;
                }
            }
        }

        

        //protected void ClientInfos_Bind(EventArgs args)
        //{
        //    if (repClientInfos.DataSource == null) { }
        //    else
        //    {
        //        repClientInfos.ClearChildViewState();
        //        repClientInfos.Controls.Clear();
        //        ClientInfo[] infos = (ClientInfo[]) repClientInfos.DataSource;
        //        int count = 0;
        //        foreach (ClientInfo ci in infos)
        //        {
        //            System.Web.UI.WebControls.Button b = new System.Web.UI.WebControls.Button();
        //            b.Visible = true;
        //            b.CssClass = "button";
        //            b.Text = ci.infoURLName;
        //            b.CommandArgument = ci.infoURL;
        //            b.CommandName = ci.infoURLName;
        //            b.ToolTip = ci.description;
        //            b.Command += new CommandEventHandler(this.HelpButton_Click);
        //            repClientInfos.Controls.Add(b);
        //            count++;
        //            repClientInfos.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));
        //            count++;
        //        }
        //        repClientInfos.ViewState["count"] = count;
        //        repClientInfos.ChildControlsCreated = true;
        //        repClientInfos.TrackViewState();

        //    }
        //}

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
        public string userFormatTime(DateTime dt)
        {
            return iLabs.UtilLib.DateUtil.ToUserTime(dt, culture, userTZ);
        }   

        private void urlEmail_Click(object sender, System.EventArgs e)
        {
            string email = wrapper.GetLabClientsWrapper(new int[] { Convert.ToInt32(Session["ClientID"]) })[0].contactEmail;
            Response.Redirect("emailTo:" + email);
        }

        protected void HelpButton_Click(object sender, CommandEventArgs e)
        {
            string jScript = @"<script language='javascript'> window.open ('" + e.CommandArgument.ToString() + "')</script>";
            Page.RegisterStartupScript("DocsPopup", jScript);
        }

        protected void urlDocumentation_Click(object sender, System.EventArgs e)
        {
            LabClient lc = wrapper.GetLabClientsWrapper(new int[] { Convert.ToInt32(Session["ClientID"]) })[0];
            clientInfos = AdministrativeAPI.ListClientInfos(lc.clientID);
            string docURL = null;
            if (clientInfos != null)
            {
                foreach (ClientInfo ci in clientInfos)
                {
                    if (ci.infoURLName.CompareTo("Documentation") == 0)
                    {
                        docURL = ci.infoURL;
                        break;
                    }
                }
            }
            string jScript = @"<script language='javascript'> window.open ('" + docURL + "')</script>";
            Page.RegisterStartupScript("DocsPopup", jScript);
        }
        /// <summary>
        /// Utility to set effectiveGroup fields
        /// </summary>
        /// <param name="currentGroup"></param>
        /// <param name="clientID"></param>
        void setEffectiveGroup(int currentGroup, int clientID)
        {
            // Find efective group
            effectiveGroupName = null;
            effectiveGroupID = AuthorizationAPI.GetEffectiveGroupID(currentGroup, clientID,
                Qualifier.labClientQualifierTypeID, Function.useLabClientFunctionType);
            if (effectiveGroupID == currentGroup)
            {
                effectiveGroupName = Session["GroupName"].ToString();
            }
            else if (effectiveGroupID > 0)
            {
                effectiveGroupName = AdministrativeAPI.GetGroupName(effectiveGroupID);
            }
        }

        /// <summary>
        /// This returns the LabServer for the client. In future may use effectiveGroupID to select if there are multiple lab servers.
        /// </summary>
        /// <returns>primary labServer for group or null</returns>
        ProcessAgentInfo getLabServer(int clientID, int effectiveGrpID)
        {
            ProcessAgentInfo pai = null;
            if(lc != null){
                ProcessAgentInfo[] paInfos = AdministrativeAPI.GetLabServersForClient(clientID);
                if (paInfos != null && paInfos.Length > 0)
                {
                    pai = paInfos[0];
                }
            }
            return pai;
        }
      

        private void launchLabClient(int c_id)
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("login.aspx");
            }
            int userID = Convert.ToInt32(Session["UserID"]);
            StringBuilder message = new StringBuilder("Message: clientID = ");
            message.Append(btnLaunchLab.CommandArgument + " ");
            LabClient client = AdministrativeAPI.GetLabClient(c_id);
            if (client!= null)
            {
                // [GeneralTicketing] get lab servers metadata from lab server ids
                ProcessAgentInfo labServer = getLabServer(client.clientID,effectiveGroupID);
                if (labServer != null)
                {
                        TicketLoadFactory factory = TicketLoadFactory.Instance();
                        // 1. Create Coupon for ExperimentCollection
                        Coupon coupon = issuer.CreateCoupon();

                        iLabProperties properties = new iLabProperties();
                        properties.Add("sb", ProcessAgentDB.ServiceAgent);
                        properties.Add("ls", labServer);
                        properties.Add("op", coupon);

                        Session["ClientID"] = client.clientID;

                        DateTime start = DateTime.UtcNow;
                        long duration = 7L * 24L * 60L *60L; // default is one week

                        //Check for Scheduling: 
                        //The scheduling Ticket should exist and been parsed into the session
                        if (lc.needsScheduling)
                        {
                            start = DateUtil.ParseUtc(Session["StartExecution"].ToString());
                            duration = Convert.ToInt64(Session["Duration"]);
                        }

                        //payload includes username and current group name & client id.
                        string sessionPayload = factory.createRedeemSessionPayload(userID, Convert.ToInt32(Session["GroupID"]),
                                   Convert.ToInt32(Session["ClientID"]), (string)Session["UserName"], (string)Session["GroupName"]);
                        // SB is the redeemer, ticket type : session_identifcation, no expiration time, payload,SB as sponsor ID, redeemer(SB) coupon
                        issuer.AddTicket(coupon, TicketTypes.REDEEM_SESSION, ProcessAgentDB.ServiceGuid,
                                     ProcessAgentDB.ServiceGuid, duration, sessionPayload);

                        AdministrativeAPI.ModifyUserSession(Convert.ToInt64(Session["SessionID"]), Convert.ToInt32(Session["GroupID"]), client.clientID, Session.SessionID);

                        if (client.clientType == LabClient.INTERACTIVE_HTML_REDIRECT)
                        {
                            // execute the "experiment execution recipe
                            RecipeExecutor executor = RecipeExecutor.Instance();
                            string redirectURL = null;

                            // loaderScript not parsed in Recipe
                           redirectURL = executor.ExecuteExperimentExecutionRecipe(coupon, labServer, client,
                            start, duration, Convert.ToInt32(Session["UserTZ"]), userID,
                            effectiveGroupID, effectiveGroupName);
                           
                            // Add the return url to the redirect
                            if (redirectURL.IndexOf("?") == -1)
                                redirectURL += "?";
                            else
                                redirectURL += "&";
                            redirectURL += "sb_url=" + Utilities.ExportUrlPath(Request.Url);
                            // Parse & check that the default auth tokens are added
                            string tmpUrl = iLabParser.Parse(redirectURL, properties, true);

                            // Now open the lab within the current Window/frame
                            Response.Redirect(tmpUrl, true);
                        }


                        else if (client.clientType == LabClient.INTERACTIVE_APPLET)
                        {

                            // Note: Currently Interactive applets
                            // use the Loader script for Batch experiments
                            // Applets do not use default query string parameters, parametes must be in the loader script
                            Session["LoaderScript"] = iLabParser.Parse(client.loaderScript, properties);
                            Session.Remove("RedirectURL");

                            string jScript = @"<script language='javascript'>parent.theapplet.location.href = '"
                                + "applet.aspx" + @"'</script>";
                            Page.RegisterStartupScript("ReloadFrame", jScript);
                        }

                        // Support for Batch 6.1 Lab Clients
                        else if (client.clientType == LabClient.BATCH_HTML_REDIRECT)
                        {
                            // use the Loader script for Batch experiments

                            //use ticketing & redirect to url in loader script

                            // [GeneralTicketing] retrieve static process agent corresponding to the first
                            // association lab server */


                            // New comments: The HTML Client is not a static process agent, so we don't search for that at the moment.
                            // Presumably when the interactive SB is merged with the batched, this should check for a static process agent.
                            // - CV, 7/22/05
                            {
                                Session.Remove("LoaderScript");

                                /* This is the original batch-redirect using a pop-up */
                                // check that the default auth tokens are added
                                string jScript = @"<script language='javascript'> window.open ('" + iLabParser.Parse(client.loaderScript, properties, true) + "')</script>";
                                Page.RegisterStartupScript("HTML Client", jScript);

                                /* This is the batch-redirect with a simple redirect, this may not work as we need to preserve session-state */
                                //string redirectURL = lc.loaderScript + "?couponID=" + coupon.couponId + "&passkey=" + coupon.passkey;
                                //Response.Redirect(redirectURL,true);
                            }
                        }
                        // use the Loader script for Batch experiments
                        else if (client.clientType == LabClient.BATCH_APPLET)
                        {
                            // Do not append defaults
                            Session["LoaderScript"] = iLabParser.Parse(client.loaderScript, properties);
                            Session.Remove("RedirectURL");

                            string jScript = @"<script language='javascript'>parent.theapplet.location.href = '"
                                + ProcessAgentDB.ServiceAgent.codeBaseUrl + @"/applet.aspx" + @"'</script>";
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "ReloadFrame", jScript);
                        }
                } // labserver != null
            }
            else
            {
                message.Append(" LabServer = null");
            }
            //lblDebug.Text = message.ToString();
            Logger.WriteLine(message.ToString());
        }

        protected void btnLaunchLab_Click(object sender, System.EventArgs e)
        {
            launchLabClient(Convert.ToInt32(btnLaunchLab.CommandArgument));
        }

        private void reenterLabClient()
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("login.aspx");
            }
            BrokerDB brokerDB = new BrokerDB();
            StringBuilder message = new StringBuilder("Message: clientID = ");
            int[] labIds = new int[1];
            message.Append(btnReenter.CommandArgument + " ");
            long expid = Convert.ToInt64(btnReenter.CommandArgument);
            LabClient client = AdministrativeAPI.GetLabClient(Convert.ToInt32(Session["ClientID"]));
            if (client.clientID > 0)
            {
                if (client.clientType == LabClient.INTERACTIVE_HTML_REDIRECT)
                {
                    iLabProperties properties = new iLabProperties();
                    properties.Add("sb",ProcessAgentDB.ServiceAgent);
                    long[] coupIDs = InternalDataDB.RetrieveExperimentCouponIDs(expid);
                    Coupon coupon = brokerDB.GetIssuedCoupon(coupIDs[0]);
                    if(coupon != null)
                        properties.Add("op", coupon);
                    // construct the redirect query
                    ProcessAgentInfo lsInfo = getLabServer(client.clientID,effectiveGroupID);
                    if(lsInfo != null)
                        properties.Add("ls",lsInfo);
                    
                    StringBuilder url = new StringBuilder(client.loaderScript.Trim());
                    // Add the return url to the redirect
                    if (url.ToString().IndexOf("?") == -1)
                        url.Append('?');
                    else
                        url.Append('&');
                    url.Append("&sb_url=");
                    url.Append(Utilities.ExportUrlPath(Request.Url));
                    string targetURL = iLabParser.Parse(url,properties,true);
                    // Now open the lab within the current Window/frame
                    Response.Redirect(targetURL, true);
                }
            }
        }

        protected void btnReenter_Click(object sender, System.EventArgs e)
        {
            reenterLabClient();
        }

        private void doScheduling()
        {
            string username = Session["UserName"].ToString();
            string groupName = Session["GroupName"].ToString();
            string labClientName = lc.clientName;
            string labClientVersion = lc.version;
            
            ProcessAgentInfo labServer = getLabServer(lc.clientID,effectiveGroupID);
            int ussId = issuer.FindProcessAgentIdForClient(lc.clientID, ProcessAgentType.SCHEDULING_SERVER);

            if (ussId > 0)
            {

                ProcessAgent uss = issuer.GetProcessAgent(ussId);
                int lssId = issuer.FindProcessAgentIdForAgent(labServer.agentId, ProcessAgentType.LAB_SCHEDULING_SERVER);
                ProcessAgent lss = issuer.GetProcessAgent(lssId);

                //Default duration ????
                long duration = 36000;

                RecipeExecutor recipeExec = RecipeExecutor.Instance();
                string schedulingUrl = recipeExec.ExecuteExerimentSchedulingRecipe(uss, lss, username, Convert.ToInt32(Session["UserID"]),effectiveGroupName,
                    labServer.agentGuid, lc,
                    Convert.ToInt64(ConfigurationManager.AppSettings["scheduleSessionTicketDuration"]), Convert.ToInt32(Session["UserTZ"]));

                schedulingUrl += "&sb_url=" + Utilities.ExportUrlPath(Request.Url);
                Response.Redirect(schedulingUrl, false);
            }
        }

        protected void btnSchedule_Click(object sender, EventArgs e)
        {
            doScheduling();
        }

    }
}
