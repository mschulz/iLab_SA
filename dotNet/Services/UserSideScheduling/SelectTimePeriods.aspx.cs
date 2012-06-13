/* $Id$ */

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Xml;

using iLabs.Controls.Scheduling;
using iLabs.Core;
using iLabs.DataTypes.TicketingTypes;
using iLabs.DataTypes.SoapHeaderTypes;
using iLabs.DataTypes.SchedulingTypes;
using iLabs.Proxies.LSS;
using iLabs.Proxies.Ticketing;
using iLabs.Ticketing;
using iLabs.UtilLib;

namespace iLabs.Scheduling.UserSide
{
	/// <summary>
	/// Summary description for ConfirmReservation.
	/// </summary>
	public partial class SelectTimePeriods: System.Web.UI.Page
	{
        public bool useQuirk = false;
		 DateTime startTime;
         DateTime endTime;
         string serviceBrokerGuid = null;
         string groupName = null;
        string clientGuid = null;
        string labServerGuid = null;
         string labClientName = null;
         string labClientVersion = null;
         string userName = null;
         string lssURL = null;
         TimePeriod[] timeSlots;
         DateTime startReserveTime;
         DateTime endReserveTime;
         string lssGuid = null;
         TicketIssuerProxy ticketIssuer;
         Coupon coupon;
         int userTZ;
        CultureInfo culture;
        UserSchedulingDB dbManager = new UserSchedulingDB();
        List<TimePeriod> periods = null;
        int defaultRange = 30;
        int quantum;
        DateTime endTimePeriod;
        TimeSpan maxAllowTime;
        TimeSpan minRequiredTime;
        TimeSpan minTime;

        protected override void LoadViewState(object savedState)
        {
            if (savedState == null)
                return;

            object[] vs = (object[])savedState;

            if (vs.Length != 3)
                throw new ArgumentException("Wrong savedState object.");

            if (vs[0] != null)
                base.LoadViewState(vs[0]);

            if (vs[1] != null)
                quantum = (int)vs[1];
            if (vs[2] != null)
                endTimePeriod = (DateTime)vs[2];
        }

        /// <summary>
        /// Saves ViewState.
        /// </summary>
        /// <returns></returns>
        protected override object SaveViewState()
        {
            object[] vs = new object[3];
            vs[0] = base.SaveViewState();
            vs[1] = quantum;
            vs[2] = endTimePeriod;
            return vs;
        }

		protected void Page_Load(object sender, System.EventArgs e)
		{
            culture = DateUtil.ParseCulture(Request.Headers["Accept-Language"]);
            cntrScheduling.setBrowser(Request.Headers["User-Agent"]);
            if (Session["serviceBrokerGuid"] != null)
			serviceBrokerGuid = Session["serviceBrokerGuid"].ToString();
            if(Session["groupName"] != null)
			groupName = Session["groupName"].ToString();
            if(Session["clientGuid"] != null)
            clientGuid = Session["clientGuid"].ToString();
            if(Session["labServerGuid"] != null)
            labServerGuid = Session["labServerGuid"].ToString();
            if(Session["labClientName"] != null)
			labClientName = Session["labClientName"].ToString();
            if(Session["labClientVersion"] != null)
			labClientVersion = Session["labClientVersion"].ToString();
            if(Session["userName"] != null)
			userName = Session["userName"].ToString();
            if(Session["lssURL"] != null)
			lssURL = Session["lssURL"].ToString();
            if(Session["lssGuid"] != null)
            lssGuid = Session["lssGuid"].ToString();
            if(Session["userTZ"] != null)
            userTZ = Convert.ToInt32(Session["userTZ"]);
            if(Session["coupon"] != null)
            coupon = (Coupon) Session["coupon"];

            if (IsPostBack){
                if(Session["startTime"]!= null)
                   startTime = (DateTime)Session["startTime"] ;
                if(Session["endTime"]!= null)
                    endTime = (DateTime)Session["endTime"];
                if (Session["minAllowTime"] != null)
                {
                    minRequiredTime = TimeSpan.FromMinutes((int)Session["minAllowTime"]);
                }
                else
                {
                    minRequiredTime = TimeSpan.FromMinutes(1);
                }
                if (Session["maxAllowTime"] != null)
                {
                    maxAllowTime = TimeSpan.FromMinutes((int)Session["maxAllowTime"]);
                }
                else
                {
                    maxAllowTime = TimeSpan.FromMinutes(42);
                }
            }

            else{
                string value1 = Request.QueryString["start"];
                string value2 = Request.QueryString["end"];
                if(value1 != null)
                startTime = DateUtil.ParseUtc(value1);
                Session["startTime"] = startTime;
                if (value2 != null)
                endTime = DateUtil.ParseUtc(value2);
                Session["endTime"] = endTime;
                int[] policyIDs = dbManager.ListUSSPolicyIDsByGroupAndExperiment(groupName, serviceBrokerGuid, clientGuid, labServerGuid);
                for (int i = 0; i < policyIDs.Length; i++)
                {
                    USSPolicy pol = dbManager.GetUSSPolicies(new int[] { policyIDs[i] })[0];
                    string maxstr = PolicyParser.getProperty(pol.rule, "Maximum reservable time");
                    if (maxstr != null)
                    {
                        maxAllowTime = TimeSpan.FromMinutes(Int32.Parse(maxstr));
                        Session["maxAllowTime"] = (int) maxAllowTime.TotalMinutes; ;
                    }
                    string minstr = PolicyParser.getProperty(pol.rule, "Minimum time required");
                    if (minstr != null)
                    {
                        minRequiredTime = TimeSpan.FromMinutes(Int32.Parse(minstr));
                        Session["minAllowTime"] = (int) minRequiredTime.TotalMinutes;
                    }
                    lblUssPolicy.Text = "Minimum time required: " + minRequiredTime + "<br />Maximum time allowed: " + maxAllowTime;
                }
                lblTimezone.Text = "Times are GMT " + userTZ / 60.0;
            }
            getTimePeriods();
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


        void getTimePeriods()
        {
            OperationAuthHeader opHeader = new OperationAuthHeader();
            opHeader.coupon = coupon;
            LabSchedulingProxy lssProxy = new LabSchedulingProxy();
            lssProxy.Url = lssURL;
            lssProxy.OperationAuthHeaderValue = opHeader;

            TimePeriod[] availablePeriods = lssProxy.RetrieveAvailableTimePeriods(serviceBrokerGuid, groupName,
                ProcessAgentDB.ServiceGuid, labServerGuid, clientGuid, startTime, endTime);
            if (availablePeriods == null)
            {
                string msg = "There are no available time slots for this experiment.";
                lblErrorMessage.Text = Utilities.FormatWarningMessage(msg);
                lblErrorMessage.Visible = true;
                btnMakeReservation.Visible = false;
               // btnMakeReservation1.Visible = false;
                cntrScheduling.Visible=false;
            }
            else{
                if (availablePeriods.Length > 0)
                {
                    minTime = availablePeriods[0].quantum > minRequiredTime.TotalMinutes
                        ? TimeSpan.FromMinutes(availablePeriods[0].quantum) : minRequiredTime;
                }
                cntrScheduling.Visible = true;
                cntrScheduling.StartTime = startTime;
                cntrScheduling.EndTime = endTime;
                cntrScheduling.UserTZ = userTZ;
                cntrScheduling.Culture = culture;
               
                cntrScheduling.DataSource = availablePeriods;
                cntrScheduling.DataBind();
            }
        }

        protected void TimePeriod_Click(object sender, System.EventArgs e)
        {
            AvailableClickEventArgs args = (AvailableClickEventArgs)e;
            quantum = args.Quantum;
            TimeSpan quantTS = TimeSpan.FromMinutes(quantum);
            TimeSpan duration = TimeSpan.FromSeconds(args.Duration);
            endTimePeriod = args.Start.Add(duration);
            DateTime endTime = args.Start.Add(duration).Subtract(minRequiredTime);
            TimeSpan span;
            StringBuilder buf = new StringBuilder();
            //buf.Append("StartTime: " + args.Start.ToString("o") + "<br />&nbsp;&nbsp;Duration: " + duration.ToString() + " Quant: " + quantum + "<br />");
            buf.Append("The minimum time required for this experiment is: " + minRequiredTime.ToString() + ".<br />");
            buf.Append("The maximum time allowed is: " + maxAllowTime.ToString() + ".");
            lblUssPolicy.Text = buf.ToString();

            ddlSelectTime.Items.Clear();
            ddlDuration.Items.Clear();
            DateTime wrkTime = args.Start;
            int count = 0;
            int qOff = wrkTime.Minute % quantum;
            int quantOffset = 0; 
            if (qOff != 0)
            {
                quantOffset = quantum - qOff;
                ddlSelectTime.Items.Add(new ListItem(DateUtil.ToUserTime(wrkTime, culture, userTZ)));
                wrkTime = wrkTime.AddMinutes(quantOffset);
                //count += quantOffset;
            }
            while ((count <= defaultRange) && (wrkTime <= endTime))
            {
                ddlSelectTime.Items.Add(new ListItem(DateUtil.ToUserTime(wrkTime, culture, userTZ)));
                wrkTime = wrkTime.AddMinutes(quantum);
                count += quantum;
            }
            if(quantOffset != 0){
               
                if (minRequiredTime.TotalMinutes <= quantOffset)
                {
                    span = TimeSpan.FromMinutes(quantOffset);
                }
                else
                {
                    int off = (Convert.ToInt32(minRequiredTime.TotalMinutes) + qOff) % quantum;
                    span = TimeSpan.FromMinutes(minRequiredTime.TotalMinutes + off);
                }
            }
            else{
                if(minRequiredTime <= quantTS){
                    span = quantTS;
                }
                else{
                    int offset = quantum - (Convert.ToInt32(minRequiredTime.TotalMinutes) % quantum);
                    span = minRequiredTime.Add(TimeSpan.FromMinutes(offset));
                }
            }
            ddlDuration.Items.Add(new ListItem(DateUtil.TimeSpanTrunc(span), Convert.ToInt32(span.TotalSeconds).ToString()));
            span = span.Add(quantTS);
            span = span.Subtract(TimeSpan.FromMinutes((double)(span.Minutes % quantum)));
            while ((span <= maxAllowTime) && (span <duration))
            {
                ddlDuration.Items.Add(new ListItem(DateUtil.TimeSpanTrunc(span), Convert.ToInt32(span.TotalSeconds).ToString()));
                span = span.Add(quantTS);
            }
            //if( (span < maxAllowTime) && (endTime >= args.Start.Add(span)))
                //ddlDuration.Items.Add(new ListItem(DateUtil.TimeSpanTrunc(maxAllowTime), Convert.ToInt32(maxAllowTime.TotalSeconds).ToString()));
            if( (span < maxAllowTime) && (maxAllowTime >= duration))
                ddlDuration.Items.Add(new ListItem(DateUtil.TimeSpanTrunc(duration), Convert.ToInt32(duration.TotalSeconds).ToString()));
        }


        protected void btnMakeReservation_Click(object sender, System.EventArgs e)
        {
            // ToDo: Add error checking
            DateTime startReserveTime = DateUtil.ParseUserToUtc(ddlSelectTime.SelectedItem.Text, culture, userTZ);
            DateTime endReserveTime = startReserveTime.AddSeconds(Double.Parse(ddlDuration.SelectedValue));
            if ((endReserveTime.Minute % quantum) != 0)
            {
                DateTime dt = endReserveTime.AddMinutes(quantum - (endReserveTime.Minute % quantum));
                if (dt <= endTimePeriod)
                {
                    endReserveTime = dt;
                }
                else
                {
                    endReserveTime = endTimePeriod;
                }
            }
            lblErrorMessage.Text = ddlSelectTime.SelectedItem.Text + " " + DateUtil.ToUserTime(endReserveTime, culture, -userTZ);
            lblErrorMessage.Visible = true;
            string notification = null;
            LabSchedulingProxy lssProxy = new LabSchedulingProxy();
            lssProxy.Url = lssURL;
            try
            {
                // create "REQUEST RESERVATION" ticket in SB and get the coupon for the ticket.
                //iLabs.DataTypes.TicketingTypes.Coupon authCoupon = ticketIssuer.CreateTicket(lssGuid, "REQUEST RESERVATION", 300, "");

                //assign the coupon from ticket to the soap header;
                OperationAuthHeader opHeader = new OperationAuthHeader();
                opHeader.coupon = coupon;
                lssProxy.OperationAuthHeaderValue = opHeader;
                notification = lssProxy.ConfirmReservation(serviceBrokerGuid, groupName, ProcessAgentDB.ServiceGuid, labServerGuid, clientGuid,
                    startReserveTime, endReserveTime);
                if (!notification.Contains("success"))
                {
                    lblErrorMessage.Text = Utilities.FormatErrorMessage(notification);
                }
                else
                {
                    try
                    {
                        int status = dbManager.AddReservation(userName, serviceBrokerGuid, groupName, labServerGuid, clientGuid, 
                            startReserveTime, endReserveTime);
                        string confirm = "The reservation from<br />" + DateUtil.ToUserTime(startReserveTime,culture,userTZ)
                            + " to " + DateUtil.ToUserTime(endReserveTime, culture, userTZ) + "<br />is confirmed.";
                        lblErrorMessage.Text = Utilities.FormatConfirmationMessage(confirm);
                    }
                    catch(Exception insertEx){
                        lblErrorMessage.Text = Utilities.FormatErrorMessage(notification);
                        lblErrorMessage.Visible = true;
                    }
                     getTimePeriods();
                }
                lblErrorMessage.Visible = true;
                    return;
            }
            catch (Exception ex)
            {
                string msg = "Exception: reservation can not be confirmed. " + ex.GetBaseException() + ".";
                lblErrorMessage.Text = Utilities.FormatErrorMessage(msg);
                lblErrorMessage.Visible = true;

            }
            try
            {
                if (notification == "The reservation is confirmed successfully")
                {
                    int experimentInfoId = dbManager.ListExperimentInfoIDByExperiment(labServerGuid, clientGuid);
                    DateTime startTimeUTC = startReserveTime.ToUniversalTime();
                    DateTime endTimeUTC = endReserveTime.ToUniversalTime();
                }
                return;
            }
            catch (Exception ex)
            {
                string msg = "Exception: reservation can not be added successfully. " + ex.GetBaseException() + ".";
                lblErrorMessage.Text = Utilities.FormatErrorMessage(msg);
                lblErrorMessage.Visible = true;
                lssProxy.RemoveReservation(serviceBrokerGuid, groupName, ProcessAgentDB.ServiceGuid, labServerGuid, clientGuid, startReserveTime, endReserveTime);
            }
        }
        
        protected void btnReturn_Click(Object Src, EventArgs E)
        {
            // This routine will create a javascript block to refresh the client & close the page.
            Page.ClientScript.RegisterStartupScript( this.GetType(), "Success", "ReloadParent();", true);
        }
 
	}
	
}
