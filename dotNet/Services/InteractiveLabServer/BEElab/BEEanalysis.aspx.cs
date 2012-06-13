/*
 * Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
 * Please see license.txt in top level directory for full license.
 * 
 * $Id: RunExperiment.aspx.cs 450 2011-09-07 20:33:00Z phbailey $
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
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
using iLabs.DataTypes.StorageTypes;
using iLabs.DataTypes.TicketingTypes;
using iLabs.LabServer.Interactive;
using iLabs.Proxies.ESS;
using iLabs.Proxies.ISB;
using iLabs.Ticketing;
using iLabs.UtilLib;


namespace iLabs.LabServer.LabView
{
	/// <summary>
    /// BEEanalysis is a page presenting the results from a completed BEE lab experiment
    /// The user will have the option of querying for experiments and selecting one to display
	/// </summary>
	public partial class BEEanalysis : System.Web.UI.Page
	{
        protected bool showTime;
		protected System.Web.UI.WebControls.Label lblCoupon;
		protected System.Web.UI.WebControls.Label lblTicket;
		protected System.Web.UI.WebControls.Label lblGroupNameTitle;
        protected string title = "Building Energy Efficiency iLab";

        int userTZ;
        int userID = -1;
        int groupID;
        long couponID = -1;
        string passKey = null;
        string issuerID = null;
        CultureInfo culture = null;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            LabDB dbManager = new LabDB();
            if (!IsPostBack)
            {
                // Query values from the request
                clearSessionInfo();
                hdnAppkey.Value = Request.QueryString["app"];
                hdnCoupon.Value = Request.QueryString["coupon_id"];
                hdnPasscode.Value = Request.QueryString["passkey"];
                hdnIssuer.Value = Request.QueryString["issuer_guid"];
                hdnSbUrl.Value = Request.QueryString["sb_url"];
                string userName = null;
                string userIdStr = null;

              
                int tz = 0;
                if (Session["userTZ"] != null)
                    tz = Convert.ToInt32(Session["userTZ"]);
                String returnURL = (string)Session["returnURL"];
                

                // this should be the RedeemSession & Experiment Coupon data
                if (!(hdnPasscode.Value != null && hdnPasscode.Value != ""
                    && hdnCoupon.Value != null && hdnCoupon.Value != "" 
                    && hdnIssuer.Value != null && hdnIssuer.Value != ""))
                {
                    Logger.WriteLine("BEEanaylsis: " + "AccessDenied missing credentials");
                    Response.Redirect("AccessDenied.aspx?text=missing+credentials.", true);
                }
                Session["opCouponID"] = hdnCoupon.Value;
                Session["opIssuer"] = hdnIssuer.Value;
                Session["opPasscode"]= hdnPasscode.Value;
                Coupon expCoupon = new Coupon(hdnIssuer.Value,  Convert.ToInt64(hdnCoupon.Value), hdnPasscode.Value);

                //Check the database for ticket and coupon, if not found Redeem Ticket from
                // issuer and store in database.
                //This ticket should include group, experiment id and be valid for this moment in time??
                Ticket expTicket = dbManager.RetrieveAndVerify(expCoupon, TicketTypes.EXECUTE_EXPERIMENT);

                if (expTicket != null)
                {
                    if (expTicket.IsExpired())
                    {
                        Response.Redirect("AccessDenied.aspx?text=The ExperimentExecution+ticket+has+expired.", true);

                    }
                    Session["exCoupon"] = expCoupon;

                    ////Parse experiment payload, only get what is needed 	
                    string payload = expTicket.payload;
                    XmlQueryDoc expDoc = new XmlQueryDoc(payload);
                    string expIdStr = expDoc.Query("ExecuteExperimentPayload/experimentID");
                    string tzStr = expDoc.Query("ExecuteExperimentPayload/userTZ");
                    //string userIdStr = expDoc.Query("ExecuteExperimentPayload/userID");
                    string groupName = expDoc.Query("ExecuteExperimentPayload/groupName");
                    Session["groupName"] = groupName;
                    string sbStr = expDoc.Query("ExecuteExperimentPayload/sbGuid");
                    Session["brokerGUID"] = sbStr;

                    if ((tzStr != null) && (tzStr.Length > 0))
                    {
                        Session["userTZ"] = tzStr;
                    }
                    LabAppInfo clientInfo = dbManager.GetLabApp(hdnAppkey.Value);
                    if(clientInfo == null){
                        throw new Exception("Client does not exist!");
                    }
                    long experimentID = Convert.ToInt64(expIdStr);
                    InteractiveSBProxy sbProxy = new InteractiveSBProxy();
                    ProcessAgentInfo sbInfo = dbManager.GetProcessAgentInfo(hdnIssuer.Value);
                    if (sbInfo != null)
                    {
                        sbProxy.OperationAuthHeaderValue = new OperationAuthHeader();
                        sbProxy.OperationAuthHeaderValue.coupon = expCoupon;
                        sbProxy.Url = sbInfo.webServiceUrl;
                        
                        List<Criterion> cri = new List<Criterion>();
                        if (clientInfo.extraInfo != null && clientInfo.extraInfo.Length > 0)
                            cri.Add(new Criterion("clientguid","=", clientInfo.extraInfo));
                        ExperimentSummary[] expSum = sbProxy.RetrieveExperimentSummary(cri.ToArray());
                        listSummaries(expSum);
                    }
                }
            }
        }

        protected void clearSessionInfo()
        {
            Session.Remove("opCouponID");
            Session.Remove("opIssuer");
            Session.Remove("opPasscode");
        }

        
        protected void clearExperimentDisplay(){
            lblResponse.Visible=false;

			// get all criteria in place
			txtExperimentID.Text = null;
			txtUsername.Text = null ;
			txtLabServerName.Text = null ;
            txtClientName.Text = null;
			txtGroupName.Text = null ;
			txtStatus.Text = null;
			txtSubmissionTime.Text = null;
			txtCompletionTime.Text = null;
            txtRecordCount.Text = null;
			txtAnnotation.Text = null;
            trSaveAnnotation.Visible = false;
            trShowExperiment.Visible = false;
            //trDeleteExperiment.Visible = false;
        }

        void displayExperimentSummary(ExperimentSummary expInfo)
        {
            txtExperimentID.Text = expInfo.experimentId.ToString();
            txtUsername.Text = expInfo.userName;
            txtGroupName.Text = expInfo.groupName;
            txtLabServerName.Text = expInfo.labServerName;
            txtClientName.Text = expInfo.clientName;
                       
            txtStatus.Text = StorageStatus.GetStatusString(expInfo.status);
            txtSubmissionTime.Text = DateUtil.ToUserTime(expInfo.creationTime, culture, userTZ);
            if ((expInfo.closeTime != null) && (expInfo.closeTime != DateTime.MinValue))
            {
                txtCompletionTime.Text = DateUtil.ToUserTime(expInfo.closeTime, culture, userTZ);
            }
            else
            {
                txtCompletionTime.Text = "Experiment Not Closed!";
            }
            txtRecordCount.Text = expInfo.recordCount.ToString("    0");
            txtAnnotation.Text = expInfo.annotation;
            trSaveAnnotation.Visible = true;
            //trDeleteExperiment.Visible = true;
            trShowExperiment.Visible = (expInfo.recordCount > 0);
        }
		
		protected void btnGo_Click(object sender, System.EventArgs e)
		{
            clearSessionInfo();
			clearExperimentDisplay();
            List<Criterion> cList = new List<Criterion>();
            LabDB dbManager = new LabDB();
            LabAppInfo clientInfo = dbManager.GetLabApp(hdnAppkey.Value);
            if (clientInfo == null)
            {
                throw new Exception("Client does not exist!");
            }
            if( clientInfo.extraInfo != null &&  clientInfo.extraInfo.Length > 0)
                cList.Add(new Criterion("clientguid", "=", clientInfo.extraInfo));
			if((ddlTimeAttribute.SelectedIndex >0))
			{
				DateTime time1 = FactoryDB.MinDbDate;
				try
				{
                    time1 = DateUtil.ParseUserToUtc(txtTime1.Text,culture,Convert.ToInt32(Session["UserTZ"]));
                }
                catch
				{	
					lblResponse.Text = Utilities.FormatErrorMessage("Please enter a valid date & time in the first time field .");
					lblResponse.Visible = true;
					return;
                }
                if (ddlTimeAttribute.SelectedIndex == 1)
                {
                    cList.Add(new Criterion("CreationTime", ">=", time1.ToString()));
                    cList.Add(new Criterion("CreationTime", "<", time1.AddDays(1).ToString()));
                }
                else if (ddlTimeAttribute.SelectedIndex == 2)
                {
                    cList.Add(new Criterion("CreationTime", "<", time1.ToString()));
                }
                else if (ddlTimeAttribute.SelectedIndex == 3)
                {
                    cList.Add(new Criterion("CreationTime", ">=", time1.ToString()));
                }
                if (ddlTimeAttribute.SelectedIndex == 4)
                {
                    DateTime time2 = FactoryDB.MaxDbDate;
                    try
                    {
                        time2 = DateUtil.ParseUserToUtc(txtTime2.Text, culture, Convert.ToInt32(Session["UserTZ"]));
                    }
                    catch
                    {
                        lblResponse.Text = Utilities.FormatErrorMessage("Please enter a valid date & time in the second time field.");
                        lblResponse.Visible = true;
                        return;
                    }
                    cList.Add(new Criterion("CreationTime", ">=", time1.ToString()));
                    cList.Add(new Criterion("CreationTime", "<=", time2.ToString()));
                }
            }

            InteractiveSBProxy sbProxy = new InteractiveSBProxy();
            ProcessAgentInfo sbInfo = dbManager.GetProcessAgentInfo(hdnIssuer.Value);
            if (sbInfo != null)
            {
                sbProxy.OperationAuthHeaderValue = new OperationAuthHeader();
                sbProxy.OperationAuthHeaderValue.coupon 
                    = new Coupon(hdnIssuer.Value, Convert.ToInt64(hdnCoupon.Value), hdnPasscode.Value);
                sbProxy.Url = sbInfo.webServiceUrl;

                List<Criterion> cri = new List<Criterion>();
                cri.Add(new Criterion("clientguid", "=", clientInfo.extraInfo));
                ExperimentSummary[] expSum = sbProxy.RetrieveExperimentSummary(cri.ToArray());
                if (expSum.Length == 0)
                {
                    string msg = "No experiment records were found for user '" + Session["UserName"] + "' in group '" + Session["GroupName"] + "'.";
                    lblResponse.Text = Utilities.FormatErrorMessage(msg);
                    lblResponse.Visible = true;
                }
                listSummaries(expSum);

            }
		}

        void listSummaries(ExperimentSummary[] summaries)
        {
            lbxSelectExperiment.Items.Clear();
            foreach (ExperimentSummary exp in summaries)
            {
                StringBuilder buf = new StringBuilder();
                buf.Append(exp.experimentId.ToString("0000") + " ");
                buf.Append(DateUtil.ToUserTime(exp.creationTime, culture, userTZ) + " ");
                if (exp.closeTime != null)
                {
                    buf.Append(DateUtil.ToUserTime(exp.closeTime, culture, userTZ) + " ");
                }
                else
                {
                    buf.Append("Experiment Not Closed ");
                }
                // User
                if (false)
                {
                    if (exp.userName != null)
                    {
                        buf.Append(exp.userName + " ");
                    }
                }
                //  Group
                if (exp.groupName != null)
                {
                    buf.Append(exp.groupName + " ");
                }
                buf.Append(exp.clientName + " ");
                // Status
                buf.Append(exp.status.ToString("000") + " ");
                //Annotation
                if (exp.annotation != null)
                {
                    buf.Append(exp.annotation);
                }

                System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem(buf.ToString(), exp.experimentId.ToString());
                lbxSelectExperiment.Items.Add(item);
            }
        }

		protected void lbxSelectExperiment_SelectedIndexChanged(object sender, System.EventArgs e)
		{
            LabDB dbManager = new LabDB();
            clearSessionInfo();
            clearExperimentDisplay();
			long experimentID = Int64.Parse (lbxSelectExperiment.Items [lbxSelectExperiment.SelectedIndex ].Value);
            InteractiveSBProxy sbProxy = new InteractiveSBProxy();
            ProcessAgentInfo sbInfo = dbManager.GetProcessAgentInfo(hdnIssuer.Value);
            if (sbInfo != null)
            {
                sbProxy.OperationAuthHeaderValue = new OperationAuthHeader();
                sbProxy.OperationAuthHeaderValue.coupon
                    = new Coupon(hdnIssuer.Value, Convert.ToInt64(hdnCoupon.Value), hdnPasscode.Value);
                sbProxy.Url = sbInfo.webServiceUrl;
                try
                {
                    //ExperimentSummary[] expInfo = wrapper.GetExperimentSummaryWrapper(new long[] { experimentID });
                    Criterion[] search = new Criterion[] { new Criterion("experiment_ID", "=", experimentID.ToString()) };
                    ExperimentSummary[] expInfo = sbProxy.RetrieveExperimentSummary(search);

                    if (expInfo[0] != null)
                    {
                        displayExperimentSummary(expInfo[0]);
                    }
                }
                catch (Exception ex)
                {
                    lblResponse.Text = Utilities.FormatErrorMessage("Error retrieving experiment information. " + ex.Message);
                    lblResponse.Visible = true;
                }
            }
		}

		protected void btnSaveAnnotation_Click(object sender, System.EventArgs e)
		{
			lblResponse.Visible=false;
            LabDB dbManager = new LabDB();
            try
            {
                InteractiveSBProxy sbProxy = new InteractiveSBProxy();
                ProcessAgentInfo sbInfo = dbManager.GetProcessAgentInfo(hdnIssuer.Value);
                if (sbInfo != null)
                {
                    sbProxy.OperationAuthHeaderValue = new OperationAuthHeader();
                    sbProxy.OperationAuthHeaderValue.coupon
                        = new Coupon(hdnIssuer.Value, Convert.ToInt64(hdnCoupon.Value), hdnPasscode.Value);
                    sbProxy.Url = sbInfo.webServiceUrl;
                    sbProxy.SetAnnotation(Int32.Parse(txtExperimentID.Text), txtAnnotation.Text);
                    Criterion[] search = new Criterion[] { new Criterion("experiment_ID", "=", txtExperimentID.Text) };
                    ExperimentSummary[] expInfo = sbProxy.RetrieveExperimentSummary(search);
                    if (expInfo.Length > 0)
                    {
                        displayExperimentSummary(expInfo[0]);
                    }
                    lblResponse.Text = Utilities.FormatConfirmationMessage("Annotation saved for experiment ID " + txtExperimentID.Text);
                    lblResponse.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblResponse.Text = Utilities.FormatErrorMessage("Error saving experiment annotation. " + ex.Message);
                lblResponse.Visible = true;
            }
		}

        protected void btnShowExperiment_Click(object sender, System.EventArgs e)
        {
            Session["opCouponID"] = hdnCoupon.Value;
            Session["opPasscode"] = hdnPasscode.Value;
            Session["opIssuer"] = hdnIssuer.Value;
            Response.Redirect("BEEgraph.aspx?expid=" + txtExperimentID.Text, true);
        }

	
        protected void ddlTimeAttribute_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            //if (ddlTimeAttribute.SelectedIndex == 4)
            //{
            //    txtTime2.Enabled = true;
            //}
            //else
            //{
            //    txtTime2.Enabled = false;
            //}
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
