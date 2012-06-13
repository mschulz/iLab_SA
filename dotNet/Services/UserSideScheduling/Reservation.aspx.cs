using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Xml;
using System.IO;
using System.Configuration;

using iLabs.Core;
using iLabs.DataTypes.ProcessAgentTypes;
using iLabs.DataTypes.TicketingTypes;
using iLabs.DataTypes.SchedulingTypes;
using iLabs.DataTypes.SoapHeaderTypes;
using iLabs.Proxies.LSS;
using iLabs.Proxies.Ticketing;
using iLabs.Ticketing;
using iLabs.UtilLib;

namespace iLabs.Scheduling.UserSide
{
    /// <summary>
    /// Summary description for ReservationInfo.
    /// </summary>
    public partial class Reservation : System.Web.UI.Page
    {
         string serviceBrokerGuid = null;
         string groupName = null;
         string clientGuid = null;
        string labServerGuid = null;
         string labClientName = null;
         string labClientVersion = null;
         string userName = null;
         string lssURL = null;
         string lssGuid = null;

        int userTZ;
        long couponID = -1;
        string passKey = null;
        string issuerID = null;
        CultureInfo culture = null;
        //string tzOff = null;

         long expirationTime;
        UserSchedulingDB dbManager = new UserSchedulingDB();
        System.Timers.Timer timer1;
        int n = 0;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            bool needsBuildList = false;
            culture = DateUtil.ParseCulture(Request.Headers["Accept-Language"]);
            
            Page.ClientScript.RegisterStartupScript( this.GetType(), "onClose", 
            "function ReloadPage(){" + Page.ClientScript.GetPostBackEventReference(this.hiddenPopupOnMakeRev, hiddenPopupOnMakeRev.ID.ToString()) + "}", true);

            //txtStartTimePeriod.Attributes.Add("OnKeyPress", "return false;");
            //txtEndTimePeriod.Attributes.Add("OnKeyPress", "return false;");
            btnRemoveReservation.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to remove this reservation?')== false) return false;");
            hiddenPopupOnMakeRev.Attributes.Add("onpropertychange", Page.GetPostBackEventReference(btnReserve));

            lblDescription.Text = "Select the number of days and start date you would like to view for available reservations."
                          + "<br/><br/>Times shown are GMT:&nbsp;&nbsp;&nbsp;" + userTZ / 60.0;
            //calDate.SelectionMode = CalendarSelectionMode.Day;
            if (!IsPostBack)
            {
                if (Request.QueryString["refresh"] != null && Session["coupon"] != null)
                {
                    needsBuildList = true;
                }
                else
                {
                    if (Request.QueryString["coupon_id"] != null)
                        couponID = long.Parse(Request.QueryString["coupon_id"]);
                    passKey = Request.QueryString["passkey"];
                    issuerID = Request.QueryString["issuer_guid"];

                    //Url of the client-execution page on the Service Broker
                    string sbUrl = Request.QueryString["sb_url"];
                    if(sbUrl != null)
                        Session["sbUrl"] = sbUrl;
                    if (couponID <= 0 || passKey == null || issuerID == null)
                    {
                        Response.Redirect("UnauthorizedReg.aspx", true);
                    }

                    Coupon coupon = new Coupon(issuerID, couponID, passKey);
                    Session["coupon"] = coupon;

                    Ticket ticket = dbManager.RetrieveAndVerify(coupon, TicketTypes.SCHEDULE_SESSION);
                    if (ticket.IsExpired())
                    {
                        string msg = "The reservation ticket has expired, Please re-login.";
                        lblErrorMessage.Text = Utilities.FormatWarningMessage(msg);
                        lblErrorMessage.Visible = true;
                        this.btnRemoveReservation.Enabled = false;
                        //this.btnReserve.Enabled = false;
                        //this.btnShowAvailableTimePeriod.Enabled = false;
                        return;
                    }
                    XmlDocument payload = new XmlDocument();
                    payload.LoadXml(ticket.payload);
                    //Session["couponID"] = couponID;
                    //Session["passKey"] = passKey;
                    //Session["issuerID"] = issuerID;

                    Session["serviceBrokerGuid"] = payload.GetElementsByTagName("sbGuid")[0].InnerText;
                    Session["groupName"] = payload.GetElementsByTagName("groupName")[0].InnerText;
                    clientGuid = payload.GetElementsByTagName("clientGuid")[0].InnerText;
                    Session["clientGuid"] = clientGuid;
                    labServerGuid = payload.GetElementsByTagName("labServerGuid")[0].InnerText;
                    Session["labServerGuid"] = labServerGuid;
                    Session["labClientName"] = payload.GetElementsByTagName("labClientName")[0].InnerText;
                    Session["labClientVersion"] = payload.GetElementsByTagName("labClientVersion")[0].InnerText;
                    Session["lssURL"] = dbManager.ListLssUrlByExperiment(clientGuid, labServerGuid);
                    Session["lssGuid"] = dbManager.ListLssIdByExperiment(clientGuid, labServerGuid);
                    Session["userName"] = payload.GetElementsByTagName("userName")[0].InnerText;
                    Session["userTZ"] = payload.GetElementsByTagName("userTZ")[0].InnerText;

                    //lblTimeSlotsInfo.Visible = false;
                    this.lblTitleofSchedule.Text = "Scheduling for " + Session["labClientName"];
                    needsBuildList = true;
                    //this.lblUserName.Text ="User: " + Session["userName"].ToString() + "   ";
                    //showDefaultAvailableTime();
                    //lbldatetimeformat1.Text = culture.DateTimeFormat.ShortDatePattern;
                    //lbldatetimeformat2.Text = culture.DateTimeFormat.ShortDatePattern;
                }
                if(Session["userTZ"] != null){
                    calDate.TodaysDate = DateTime.UtcNow.AddMinutes(Convert.ToInt32(Session["userTZ"])).Date;
                }
            }
            //coupon = (Coupon)Session["coupon"];
            lssURL = Session["lssURL"].ToString();
            lssGuid = Session["lssGuid"].ToString();
            serviceBrokerGuid = Session["serviceBrokerGuid"].ToString();
            groupName = Session["groupName"].ToString();
            clientGuid = Session["clientGuid"].ToString();
            labServerGuid = Session["labServerGuid"].ToString();
            labClientName = Session["labClientName"].ToString();
            labClientVersion = Session["labClientVersion"].ToString();
            lssURL = Session["lssURL"].ToString();
            lssGuid = Session["lssGuid"].ToString();
            userName = Session["userName"].ToString();
            if (Session["userTZ"] != null)
            {
                userTZ = Convert.ToInt32(Session["userTZ"]);
            }
         
            //lblTimeSlotsInfo.Visible = false;
            lblErrorMessage.Visible = false;
            //showDefaultAvailableTime();

            if (needsBuildList)
                buildReservationListBox();
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
            this.timer1 = new System.Timers.Timer();
            ((System.ComponentModel.ISupportInitialize)(this.timer1)).BeginInit();
            // 
            // timer1
            // 
            this.timer1.Enabled = false;
            this.timer1.Interval = 1000;
            this.timer1.Elapsed += new System.Timers.ElapsedEventHandler(this.timer1_Elapsed);
            this.Load += new System.EventHandler(this.Page_Load);
            ((System.ComponentModel.ISupportInitialize)(this.timer1)).EndInit();


        }
        #endregion

        private void timer1_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            n++;
            if (n >= expirationTime)
            {
                timer1.Stop();
                string msg = "The session ticket has expired, Please re-login.";
                lblErrorMessage.Text = msg;
                lblErrorMessage.Visible = true;
                this.btnRemoveReservation.Enabled = false;
                //this.btnReserve.Enabled = false;
                //this.btnShowAvailableTimePeriod.Enabled = false;
                return;
            }
        }

        private void buildReservationListBox()
        {
            lbxReservation.Items.Clear();
            try
            {
                ReservationInfo[] res = dbManager.GetReservationInfos(serviceBrokerGuid, userName, groupName,
                    labServerGuid, clientGuid, DateTime.UtcNow,DateTime.MaxValue);
                if (res != null)
                {
                    for (int i = 0; i < res.Length; i++)
                    {
                        ListItem reservationItem = new ListItem();
                        reservationItem.Text = DateUtil.ToUserTime(res[i].startTime, culture, userTZ) + " <---> " + DateUtil.ToUserTime(res[i].endTime, culture, userTZ);
                        reservationItem.Value = res[i].reservationId.ToString();
                        lbxReservation.Items.Add(reservationItem);
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = "Exception: Cannot retrieve the reserved time for you. " + ex.GetBaseException() + ".";
                lblErrorMessage.Text = msg;
                lblErrorMessage.Visible = true;
            }

        }

 /*
        // startTime and endTime are all UTC
        private void showAvailableTimeBlock(DateTime startTime, DateTime endTime)
        {
            try
            {
                lblTimeSlotsInfo.Visible = true;
                repTimeSlotsInfo.Controls.Clear();


                OperationAuthHeader opHeader = new OperationAuthHeader();
                opHeader.coupon = coupon;
                LabSchedulingProxy lssProxy = new LabSchedulingProxy();
                lssProxy.Url = lssURL;
                lssProxy.OperationAuthHeaderValue = opHeader;

                TimeBlock[] availableTimeBlocks = lssProxy.RetrieveAvailableTimeBlocks(serviceBrokerGuid, groupName, ProcessAgentDB.ServiceGuid, clientGuid, labServerGuid, startTime, endTime);
                if (availableTimeBlocks.Length == 0)
                {
                    lblTimeSlotsInfo.Text = "There is no available time block for " + labClientName + ":" + labClientVersion + "from " + DateUtil.ToUserTime(startTime, culture, userTZ) + " to " + DateUtil.ToUserTime(endTime, culture, userTZ);
                }
                else
                {
                    lblTimeSlotsInfo.Text = "The available time block for " + labClientName + ":" + labClientVersion + " from " + DateUtil.ToUserTime(startTime, culture, userTZ) + " to " + DateUtil.ToUserTime(endTime, culture, userTZ) + " are :";
                    int count = 0;
                    foreach (TimeBlock tb in availableTimeBlocks)
                    {

                        HyperLink h1 = new HyperLink();

                        if (tb.startTime < startTime && tb.endTime > endTime)
                        {

                            h1.Text = DateUtil.ToUserTime(startTime, culture, userTZ) + "---" + DateUtil.ToUserTime(endTime, culture, userTZ);
                            h1.NavigateUrl = "javascript:PopupReserve('" + DateUtil.ToUtcString(startTime) + "', '" + DateUtil.ToUtcString(endTime) + "', 450,700);";
                        }
                        if (tb.startTime >= startTime && tb.endTime > endTime)
                        {

                            h1.Text = DateUtil.ToUserTime(tb.startTime, culture, userTZ) + "---" + DateUtil.ToUserTime(endTime, culture, userTZ);
                            h1.NavigateUrl = "javascript:PopupReserve('" + DateUtil.ToUtcString(tb.startTime) + "', '" + DateUtil.ToUtcString(endTime) + "', 450,700);";
                        }
                        if (tb.startTime >= startTime && tb.endTime <= endTime)
                        {

                            h1.Text = DateUtil.ToUserTime(tb.startTime, culture, userTZ) + "---" + DateUtil.ToUserTime(tb.endTime, culture, userTZ);
                            h1.NavigateUrl = "javascript:PopupReserve('" + DateUtil.ToUtcString(tb.startTime) + "', '" + DateUtil.ToUtcString(tb.endTime) + "', 450,700);";
                        }
                        if (tb.startTime < startTime && tb.endTime < endTime)
                        {

                            h1.Text = DateUtil.ToUserTime(startTime, culture, userTZ) + "---" + DateUtil.ToUserTime(tb.endTime, culture, userTZ);
                            h1.NavigateUrl = "javascript:PopupReserve('" + DateUtil.ToUtcString(tb.startTime) + "', '" + DateUtil.ToUtcString(tb.endTime) + "', 450,700);";
                        }
                        repTimeSlotsInfo.Controls.AddAt(count, h1);
                        repTimeSlotsInfo.Controls.AddAt(count + 1, new LiteralControl("<br></br>"));
                        count = count + 2;
                    }
                }

            }
            catch (Exception ex)
            {
                string msg = "Exception: Cannot retrive the available time block. " + ex.GetBaseException() + ".";
                lblErrorMessage.Text = Utilities.FormatErrorMessage(msg);
                lblErrorMessage.Visible = true;
                
            }
        }

       /* private void showAvailableTime(DateTime startTime, DateTime endTime)
        {
            try
            {
                lblTimeSlotsInfo.Visible = true;
                repTimeSlotsInfo.Controls.Clear();

               
                OperationAuthHeader opHeader = new OperationAuthHeader();
                opHeader.coupon = coupon;
                LssProxy lssProxy = new LssProxy(lssURL);
                lssProxy.OperationAuthHeaderValue = opHeader;

                TimePeriod[] availableTimePeriods = lssProxy.RetrieveAvailableTimePeriods(serviceBrokerGuid, groupName, ussGuid, labClientName, labClientVersion, startTime, endTime);
                if (availableTimePeriods.Length == 0)
                {
                    lblTimeSlotsInfo.Text = "There is no available time for " + labClientName + ":" + labClientVersion + "from " + DateUtil.ToUserTime(startTime, culture, userTZ) + " to " + DateUtil.ToUserTime(endTime, culture, userTZ);
                }
                else
                {
                    lblTimeSlotsInfo.Text = "The available time period for " + labClientName + ":" + labClientVersion + " from " + DateUtil.ToUserTime(startTime, culture, userTZ) + " to " + DateUtil.ToUserTime(endTime, culture, userTZ) + " are :";
                    int count = 0;
                    foreach (TimePeriod ts in availableTimePeriods)
                    {
                        
                        HyperLink h1 = new HyperLink();
                        h1.Text = DateUtil.ToUserTime(ts.startTime, culture, userTZ) + "---" + DateUtil.ToUserTime(ts.endTime, culture, userTZ);
                        h1.NavigateUrl = "javascript:PopupReserve('" + DateUtil.ToUtcString(ts.startTime) + "', '" + DateUtil.ToUtcString(ts.endTime) + "', 450,700);";

                        repTimeSlotsInfo.Controls.AddAt(count, h1);
                        repTimeSlotsInfo.Controls.AddAt(count + 1, new LiteralControl("<br></br>"));
                        count = count + 2;
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = "Exception: Cannot retrive the available time. " + ex.GetBaseException() + ".";
                lblErrorMessage.Text = msg;
                lblErrorMessage.Visible = true;
            }
        }
        * */

        protected void btnReserve_Click(object sender, System.EventArgs e)
        {
            string msg = "The reservation has been made successfully!";
            lblErrorMessage.Text = Utilities.FormatConfirmationMessage(msg);
            lblErrorMessage.Visible = true;
            calDate.SelectedDates.Clear();
            buildReservationListBox();
            //showDefaultAvailableTime();
            return;
        }

        protected void btnRemoveReservation_Click(object sender, System.EventArgs e)
        {
            DateTime startTime = DateTime.MinValue;
            DateTime endTime = DateTime.MinValue;
            DateTime startTimeUTC = startTime.ToUniversalTime();
            DateTime endTimeUTC = endTime.ToUniversalTime();
            int experimentInfoId = -1;

            if (lbxReservation.SelectedIndex < 0)
            {
                string msg = "Please select the reservation to be removed. ";
                lblErrorMessage.Text = Utilities.FormatWarningMessage(msg);
                lblErrorMessage.Visible = true;
                return;
            }

            try
            {
                int[] resIDs = new int[] { Int32.Parse(lbxReservation.SelectedValue) };
                if (resIDs != null && resIDs.Length > 0)
                {
                    ReservationInfo[] remove = dbManager.GetReservations(resIDs);
                    if (remove != null && remove.Length > 0)
                    {
                        startTimeUTC = remove[0].startTime;
                        endTimeUTC = remove[0].endTime;
                        experimentInfoId = remove[0].experimentInfoId;
                        UssExperimentInfo[] exp = dbManager.GetExperimentInfos(new int[] { remove[0].experimentInfoId });
                        UssCredentialSet[] cSet = dbManager.GetCredentialSets(new int[] { remove[0].credentialSetId });
                        LSSInfo lss = dbManager.GetLSSInfo(exp[0].lssGuid);
                        
                        if (exp != null && exp.Length > 0 && cSet != null && cSet.Length > 0 && lss.lssUrl != null)
                        {
                            Coupon coupon = dbManager.GetCoupon(lss.revokeCouponID, lss.domainGuid);
                            if (coupon != null)
                            {
                                OperationAuthHeader opHeader = new OperationAuthHeader();
                                opHeader.coupon = coupon;
                                LabSchedulingProxy lssProxy = new LabSchedulingProxy();
                                lssProxy.OperationAuthHeaderValue = opHeader;
                                lssProxy.Url = lss.lssUrl;

                                int count = lssProxy.RemoveReservation(cSet[0].serviceBrokerGuid, cSet[0].groupName, ProcessAgentDB.ServiceGuid,
                                    exp[0].labServerGuid, exp[0].labClientGuid, startTimeUTC, endTimeUTC);
                                if (count > 0)
                                {
                                    dbManager.RemoveReservation(resIDs);
                                    string msg = "The reservation has been removed successfully! ";
                                    lblErrorMessage.Text = Utilities.FormatConfirmationMessage(msg);
                                    lblErrorMessage.Visible = true;
                                }
                                else
                                {
                                    string msg = "The reservation has not been removed successfully.";
                                    lblErrorMessage.Text = Utilities.FormatErrorMessage(msg);
                                    lblErrorMessage.Visible = true;
                                }
                            }
                            else
                            {
                                string msg = "Access denied: The reservation has not been removed successfully.";
                                lblErrorMessage.Text = Utilities.FormatErrorMessage(msg);
                                lblErrorMessage.Visible = true;
                            }
                        }
                        buildReservationListBox();
                    }
                }
            }

            catch (Exception ex)
            {
                string msg = "Exception: reservation can not be removed. " + ex.GetBaseException() + ".";
                lblErrorMessage.Text = Utilities.FormatErrorMessage(msg);
                lblErrorMessage.Visible = true;
            }
           
        }

        protected void calDayChanged(object sender, System.EventArgs e)
        {
            lblErrorMessage.Visible = false;

            DateTime targetDay = DateUtil.SpecifyUTC(calDate.SelectedDate).AddMinutes(-userTZ);
            DateTime current = DateTime.UtcNow; 
            DateTime startTime;
            DateTime endTime;
            int days = Convert.ToInt32(ddlDays.SelectedValue);
            
            if(targetDay < current){
                if(targetDay.AddDays(1) <current){
                    lblErrorMessage.Text = "You must select a date which is today or later!";
                    lblErrorMessage.Visible = true;
                    calDate.SelectedDates.Clear();
                    return;
                }
                else{
                    startTime = new DateTime(current.Year,current.Month,current.Day,current.Hour,
                        current.Minute,0,0,current.Kind);
                }
            }else{
                startTime = targetDay;
               
            }
            //if (calDate.SelectionMode == CalendarSelectionMode.Day)
            //    endTime = startTime.AddDays(1);
            //else
            endTime = DateUtil.SpecifyUTC(calDate.SelectedDate).AddMinutes(-userTZ).AddDays(days);
            calDate.SelectedDates.Clear();
            ClientScriptManager cs = Page.ClientScript;
            // 
            int pageWidth = 50 + (100 * days) + 50 + 450;//HourColum, days, spacer, text 
            string js =
            "PopupReserve('" + DateUtil.ToUtcString(startTime) + "', '" + DateUtil.ToUtcString(endTime) + "', " + pageWidth +",700);";
            cs.RegisterStartupScript(this.GetType(), "dayClick", js, true);
            return;
            //string url = "SelectTimePeriods.aspx?start=" + DateUtil.ToUtcString(startTime) + "&end=" + DateUtil.ToUtcString(endTime);
            //Response.Redirect(url, false);

        }

        //protected void calDayClicked(object sender, System.EventArgs e)
        //{
        //    DateTime targetDay = calDate.SelectedDate;
        //    string js =
        //    "PopupReserve('" + DateUtil.ToUtcString(targetDay) + "', '" + DateUtil.ToUtcString(targetDay.AddDays(1.0)) + "', 450,700);";
        //    //lblErrorMessage.Text = Utilities.FormatConfirmationMessage(msg);
        //    //lblErrorMessage.Visible = true;
        //    //buildReservationListBox();
        //    //showDefaultAvailableTime();
        //    ClientScriptManager cs = Page.ClientScript;
        //    cs.RegisterStartupScript(this.GetType(), "dayClick", js, true);
        //    return;
        //}
        

        protected void btnBackToSB_Click(object sender, EventArgs e)
        {
            Response.Redirect(Session["sbUrl"].ToString(), false);
        }

        protected void ReservationSelected(object sender, EventArgs e)
        {
            int i = lbxReservation.SelectedIndex;
        }

        protected void btnRedeemReservation_Click(object sender, EventArgs e)
        {

            if (lbxReservation.SelectedIndex < 0)
            {
                string msg = "Please select the reservation to be redeemed. ";
                lblErrorMessage.Text = Utilities.FormatWarningMessage(msg);
                lblErrorMessage.Visible = true;
                return;
            }
            try
            {
                TimeSpan ts = dbManager.GetReservationWaitTime(Int32.Parse(lbxReservation.SelectedValue));

                string msg = null;
                if (ts.Ticks > 0)
                {
                    if (ts.Minutes == 0)
                    {
                        msg = "It is " + ts.Seconds.ToString() + " seconds earlier than the reservation";
                    }
                    else
                    {
                        if (ts.TotalDays == 0)
                        {
                            if (ts.Hours == 0)
                            {
                                msg = "It is " + ts.Minutes.ToString() + " minutes and " + ts.Seconds.ToString() + " seconds earlier than the reservation";
                            }
                            else
                            {
                                msg = "It is " + ts.Hours.ToString() + " hours and " + ts.Minutes.ToString() + " minutes and " + ts.Seconds.ToString() + " seconds earlier than the reservation";
                            }
                        }
                        else
                        {
                            msg = "It is " + ts.Days.ToString() + " days and " + ts.Hours.ToString() + " hours and " + ts.Minutes.ToString() + " minutes and " + ts.Seconds.ToString() + " seconds earlier than the reservation";
                        }
                    }
                    lblErrorMessage.Text = Utilities.FormatConfirmationMessage(msg);
                    lblErrorMessage.Visible = true;
                    return;
                }
                if (ts.Milliseconds <= 0)
                {
                    msg = "You can execute the experiment now!";
                    ReservationInfo[] res = dbManager.GetReservations(new int[] { Int32.Parse(lbxReservation.SelectedValue) });
                    //res[0].
                    lblErrorMessage.Text = Utilities.FormatConfirmationMessage(msg);
                    lblErrorMessage.Visible = true;

                    

                    redeemReservation(res[0]);
                }

                //RedirectBackToSB();

            }
            catch (Exception ex)
            {
                string msg = "Exception: reservation can not be redeemed. " + ex.GetBaseException() + ".";
                lblErrorMessage.Text = Utilities.FormatErrorMessage(msg);
                lblErrorMessage.Visible = true;
            }
        }

        private void redeemReservation(ReservationInfo res)
        {
            long duration = (res.endTime.Ticks - res.startTime.Ticks) / TimeSpan.TicksPerSecond;
            TicketLoadFactory factory = TicketLoadFactory.Instance();
            ProcessAgentDB ticketing = new ProcessAgentDB();
            

            string payload = factory.createAllowExperimentExecutionPayload(
                res.startTime, duration, Session["groupName"].ToString(),clientGuid);
            DateTime tmpTime = res.startTime.AddTicks(duration * TimeSpan.TicksPerSecond);
            DateTime utcNow = DateTime.UtcNow;
            long ticketDuration = (tmpTime.Ticks - utcNow.Ticks) / TimeSpan.TicksPerSecond;
            
            TicketIssuerProxy ticketIssuer = new TicketIssuerProxy();

            //get the SB web service URL, and set the proxy's URL accordingly
            ProcessAgentInfo sbInfo = ticketing.GetServiceBrokerInfo();
            ticketIssuer.Url = sbInfo.webServiceUrl;

            //Get the agent coupon Coupon, to be embedded in the SOAP header of the web service call to the SB
            Coupon agentOutCoupon = sbInfo.identOut;

            iLabs.DataTypes.SoapHeaderTypes.AgentAuthHeader agentAuthHeader = new iLabs.DataTypes.SoapHeaderTypes.AgentAuthHeader();

            //set the SOAP header (of the proxy class) to the agentCoupon
            agentAuthHeader.coupon = agentOutCoupon;
            agentAuthHeader.agentGuid = ProcessAgentDB.ServiceGuid;
            ticketIssuer.AgentAuthHeaderValue = agentAuthHeader;

            //call the CreateTicket web service method on the SB (ticket issuer)
            Coupon allowExecutionCoupon = ticketIssuer.CreateTicket(TicketTypes.ALLOW_EXPERIMENT_EXECUTION, 
                sbInfo.agentGuid, ticketDuration, payload);
            
            if (allowExecutionCoupon != null)
            {
                string couponId = allowExecutionCoupon.couponId.ToString();
                string passkey = allowExecutionCoupon.passkey;
                string issuerGuid = allowExecutionCoupon.issuerGuid;

                string backToSbUrl = Session["sbUrl"].ToString() +
                    "?coupon_id=" + couponId +
                    "&passkey=" + passkey +
                    "&issuer_guid=" + issuerGuid;

                Response.Redirect(backToSbUrl, false);
            }
            else
            {
                string msg = "Exception: ExperimentExecution is not allowed.";
                lblErrorMessage.Text = Utilities.FormatErrorMessage(msg);
                lblErrorMessage.Visible = true;
            }
        }

        protected void OnMakeReservation_Click(object sender, EventArgs e)
        {
            buildReservationListBox();
        }
    }
}

