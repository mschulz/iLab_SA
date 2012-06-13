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
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web .Security ;

using iLabs.ServiceBroker;
using iLabs.ServiceBroker.Internal;
using iLabs.ServiceBroker.Authentication;
using iLabs.ServiceBroker.Administration;
using iLabs.ServiceBroker.Authorization;
using iLabs.ServiceBroker.DataStorage;
using iLabs.ServiceBroker;
using iLabs.Ticketing;
using iLabs.UtilLib;

//using iLabs.Services;
using iLabs.DataTypes;
using iLabs.Core;
using iLabs.DataTypes.ProcessAgentTypes;
using iLabs.DataTypes.StorageTypes;
using iLabs.DataTypes.SoapHeaderTypes;
using iLabs.DataTypes.TicketingTypes;
using iLabs.Proxies.ESS;

namespace iLabs.ServiceBroker.admin
{
	/// <summary>
	/// Summary description for experimentRecords.
	/// </summary>
	public partial class experimentRecords : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.DropDownList ddlTimeis;
        int userTZ;
        CultureInfo culture = null;
        string dateF = null;
        AuthorizationWrapperClass wrapper = new AuthorizationWrapperClass();
        int userId;
		protected void Page_Load(object sender, System.EventArgs e)
		{
            if (Session["UserID"] == null)
            {
                Response.Redirect("../login.aspx");
            }
            userId = Convert.ToInt32(Session["UserID"]);
            culture = DateUtil.ParseCulture(Request.Headers["Accept-Language"]);
            dateF = DateUtil.DateTime24(culture);

            if (Session["UserTZ"] != null)
                userTZ = Convert.ToInt32(Session["UserTZ"]);
            if (!IsPostBack)
            {
                // "Are you sure" javascript for DeleteExperiment button
                btnDeleteExperiment.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete this experiment?')== false) return false;");

                StringBuilder buf = new StringBuilder();
                buf.Append("Select criteria for the experiments to be displayed.  Enter date values using this format: '");
                buf.Append(dateF + " [PM]");
                buf.Append("' time may be entered as 24 or 12 hour format.");
                buf.Append("<br/><br/>Times shown are GMT:&nbsp;&nbsp;&nbsp;" + userTZ / 60.0 + "&nbsp;&nbsp; and use a 24 hour clock.");
                lblDescription.Text = buf.ToString();
                LoadAuthorityList();
            }
			if(ddlTimeAttribute.SelectedValue!="Between")                
			{
				txtTime2.ReadOnly=true;
				txtTime2.BackColor=Color.Lavender;
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
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    

		}
		#endregion

        private void LoadAuthorityList()
        {
            ddlAuthority.Items.Clear();
            //ListItem liHeaderAdminGroup = new ListItem("--- All Authorities ---", "-1");
            //ddlAuthorities.Items.Add(liHeaderAdminGroup);
            BrokerDB brokerDB = new BrokerDB();
            IntTag[] authTags = brokerDB.GetAuthorityTags();
            if (authTags != null && authTags.Length > 0)
            {
                foreach (IntTag t in authTags)
                {
                    ListItem li = new ListItem(t.tag, t.id.ToString());
                    ddlAuthority.Items.Add(li);
                }
                ddlAuthority.SelectedValue = "0";
            }
        }
        protected void clearExperimentDisplay()
        {
            txtExperimentID.Text = null;
            txtUserName1.Text = null;
            txtLabServerName.Text = null;
            txtClientName.Text = null;
            txtGroupName1.Text = null;
            txtStatus.Text = null;
            txtSubmissionTime.Text = null;
            txtCompletionTime.Text = null;
            txtRecordCount.Text = null;
            txtAnnotation.Text = null;
            txtAnnotation.ReadOnly = true;
            lblResponse.Text = null;

            trSaveAnnotation.Visible = false;
            trShowExperiment.Visible = false;
            trDeleteExperiment.Visible = false;
        }

        protected void  selectExperiments(){
			//	 get all criteria in place
			lbxSelectExperiment.Items .Clear ();
            clearExperimentDisplay();
            int authorityID = 0;

			int sessionGroupID = Convert.ToInt32(Session["GroupID"]);
            DateTime time1 = FactoryDB.MinDbDate;
            
			
				List<Criterion> cList = new List<Criterion>();
				if(txtGroupname.Text != "")
				{
					int gID = wrapper.GetGroupIDWrapper(txtGroupname.Text);
					cList.Add(new Criterion ("Group_ID", "=",gID.ToString()));
				}

				if(txtUsername.Text != "")
				{
                    authorityID = Convert.ToInt32(ddlAuthority.SelectedValue);
					int uID = wrapper.GetUserIDWrapper(txtUsername.Text, authorityID);
					cList.Add(new Criterion ("User_ID", "=", uID.ToString() ));
				}
			if((ddlTimeAttribute.SelectedIndex > 0))
			{
                try
                {
                    time1 = DateUtil.ParseUserToUtc(txtTime1.Text, culture, Convert.ToInt32(Session["UserTZ"]));
                }
                catch
                {
                    lblResponse.Text = Utilities.FormatErrorMessage("Please enter a valid date & time in the first time field.");
                    lblResponse.Visible = true;
                    return;
                }
            }
            if (ddlTimeAttribute.SelectedIndex ==1){ //Date
               
                cList.Add(new Criterion ("CreationTime", ">=",time1.Date.ToString()));
					cList.Add(new Criterion ("CreationTime", "<", time1.Date.AddDays(1).ToString()));

            }
            else if(ddlTimeAttribute.SelectedIndex ==2){ // Before
                 cList.Add(new Criterion ("CreationTime", "<", time1.ToString()));
            }
            else if(ddlTimeAttribute.SelectedIndex ==3){ // After
                 cList.Add(new Criterion ("CreationTime", ">=", time1.ToString()));
            }
            else if(ddlTimeAttribute.SelectedIndex ==4){ // Between
                DateTime time2 = FactoryDB.MaxDbDate;
                try{
						    time2 = DateUtil.ParseUserToUtc(txtTime2.Text,culture,Convert.ToInt32(Session["UserTZ"]));
					    }
                        catch{	
					        lblResponse.Text = Utilities.FormatErrorMessage("Please enter a valid date & time in the second time field.");
					        lblResponse.Visible = true;
					        return;
                        }
					cList.Add(new Criterion ("CreationTime", ">=",time1.ToString()));
					cList.Add(new Criterion ("CreationTime", "<", time2.ToString()));
            }
		 long[] eIDs = DataStorageAPI.RetrieveAuthorizedExpIDs(userId,sessionGroupID, cList.ToArray());
            LongTag[] expTags = DataStorageAPI.RetrieveExperimentTags(eIDs, userTZ, culture,true,true,true,false,true,false,true,false);

            for (int i = 0; i < expTags.Length; i++)
            {
              
                System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem(expTags[i].tag, expTags[i].id.ToString());
                lbxSelectExperiment.Items.Add(item);
            }

            if (eIDs.Length == 0)
            {
                string msg = "No experiments were found for the selection criteria!";
                lblResponse.Text = Utilities.FormatErrorMessage(msg);
                lblResponse.Visible = true;
            }
		
		}

        protected void btnGo_Click(object sender, System.EventArgs e)
        {
            selectExperiments();
        }

		protected void lbxSelectExperiment_SelectedIndexChanged(object sender, System.EventArgs e)
		{

            clearExperimentDisplay();
			try
			{
				ExperimentSummary[] expInfo = wrapper.GetExperimentSummaryWrapper (new long[] {Int64.Parse (lbxSelectExperiment.Items [lbxSelectExperiment.SelectedIndex ].Value)});
			
				if(expInfo[0] != null)
				{
                    if( expInfo[0].essGuid != null){
                        int expStatus = expInfo[0].status;
                        if ((expStatus == StorageStatus.UNKNOWN || expStatus == StorageStatus.INITIALIZED
                        || expStatus == StorageStatus.OPEN || expStatus == StorageStatus.REOPENED
                        || expStatus == StorageStatus.RUNNING
                        || expStatus == StorageStatus.BATCH_QUEUED || expStatus == StorageStatus.BATCH_RUNNING
                        || expStatus == StorageStatus.BATCH_TERMINATED || expStatus == StorageStatus.BATCH_TERMINATED_ERROR))
                        {

                            // This operation should happen within the Wrapper
                            BrokerDB ticketIssuer = new BrokerDB();
                            ProcessAgentInfo ess = ticketIssuer.GetProcessAgentInfo(expInfo[0].essGuid);
                            if (ess.retired)
                            {
                                throw new Exception("The experiments ESS has been retired");
                            }
                            Coupon opCoupon = ticketIssuer.GetEssOpCoupon(expInfo[0].experimentId, TicketTypes.RETRIEVE_RECORDS, 60, ess.agentGuid);
                            if (opCoupon != null)
                            {

                                ExperimentStorageProxy essProxy = new ExperimentStorageProxy();
                                OperationAuthHeader header = new OperationAuthHeader();
                                header.coupon = opCoupon;
                                essProxy.Url = ess.webServiceUrl;
                                essProxy.OperationAuthHeaderValue = header;

                                StorageStatus curStatus = essProxy.GetExperimentStatus(expInfo[0].experimentId);
                                if (expInfo[0].status != curStatus.status || expInfo[0].recordCount != curStatus.recordCount
                                    || expInfo[0].closeTime != curStatus.closeTime)
                                {
                                    DataStorageAPI.UpdateExperimentStatus(curStatus);
                                    expInfo[0].status = curStatus.status;
                                    expInfo[0].recordCount = curStatus.recordCount;
                                    expInfo[0].closeTime = curStatus.closeTime;

                                }
                            }
                        }

                    }
					txtExperimentID.Text = expInfo[0].experimentId.ToString () ;
					txtUserName1.Text = expInfo[0].userName ;
					txtLabServerName.Text =expInfo[0].labServerName;
                    txtClientName.Text = expInfo[0].clientName;
					txtGroupName1.Text = expInfo[0].groupName;

                    txtStatus.Text = DataStorageAPI.getStatusString(expInfo[0].status);
					txtSubmissionTime.Text = DateUtil.ToUserTime(expInfo[0].creationTime,culture,userTZ);
                    if ((expInfo[0].closeTime != null) && (expInfo[0].closeTime != DateTime.MinValue))
                        txtCompletionTime.Text = DateUtil.ToUserTime(expInfo[0].closeTime, culture, userTZ);
                    else
                        txtCompletionTime.Text = "Not Closed!";
                    txtRecordCount.Text = expInfo[0].recordCount.ToString();
			
					txtAnnotation.Text = expInfo[0].annotation;
                    txtAnnotation.ReadOnly = false;

                    trShowExperiment.Visible = (expInfo[0].recordCount >0);
                    trSaveAnnotation.Visible = true;
                    trDeleteExperiment.Visible = true;

				}
			
				//txtExperimentSpecification.Text = wrapper.RetrieveExperimentSpecificationWrapper (Int32.Parse (lbxSelectExperiment.Items [lbxSelectExperiment.SelectedIndex ].Value));
				//txtExperimentResult.Text = wrapper.RetrieveExperimentResultWrapper (Int32.Parse (lbxSelectExperiment.Items [lbxSelectExperiment.SelectedIndex ].Value));
				//txtLabconfig.Text = wrapper.RetrieveLabConfigurationWrapper (Int32.Parse (lbxSelectExperiment.Items [lbxSelectExperiment.SelectedIndex ].Value));
			
			}
			catch(Exception ex)
			{
				lblResponse.Text ="<div class=errormessage><p>Exception: Error retrieving experiment information. "+ex.Message+"</p></div>";
				lblResponse.Visible=true;
			}
		}

		protected void ddlTimeAttribute_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			//txtTime1.Text=null;
			//txtTime2.Text=null;
			if (ddlTimeAttribute.SelectedValue.ToString().CompareTo("Between")==0)
			{
				txtTime2.ReadOnly=false;
				txtTime2.BackColor=Color.White;
			}
		}

        protected void btnSaveAnnotation_Click(object sender, System.EventArgs e)
        {
            lblResponse.Visible = false;

            try
            {
                wrapper.SaveExperimentAnnotationWrapper(Int64.Parse(txtExperimentID.Text), txtAnnotation.Text);
                LongTag[] updateEXP = DataStorageAPI.RetrieveExperimentTags(new long[] { Convert.ToInt64(txtExperimentID.Text) },
                    userTZ, culture, true, true, true, false, false, false, true, false);
                if(updateEXP.Length == 1){
                    lbxSelectExperiment.Items.FindByValue(txtExperimentID.Text).Text = updateEXP[0].tag;
                }
                lblResponse.Text = Utilities.FormatConfirmationMessage("Annotation saved for experiment ID " + txtExperimentID.Text);
                lblResponse.Visible = true;
            }
            catch (Exception ex)
            {
                lblResponse.Text = Utilities.FormatErrorMessage("Error saving experiment annotation. " + ex.Message);
                lblResponse.Visible = true;
            }
        }

        protected void btnShowExperiment_Click(object sender, System.EventArgs e)
        {
            Response.Redirect("showAdminExperiment.aspx?expid=" + txtExperimentID.Text, true);
        }

        protected void btnDeleteExperiment_Click(object sender, System.EventArgs e)
        {
            lblResponse.Visible = false;
            try
            {
                wrapper.RemoveExperimentsWrapper(new long[] { Convert.ToInt32(txtExperimentID.Text) });
                selectExperiments();
                lblResponse.Text = Utilities.FormatConfirmationMessage("Deleted experiment ID " + txtExperimentID.Text);
                lblResponse.Visible = true;
            }
            catch (Exception ex)
            {
                lblResponse.Text = Utilities.FormatErrorMessage("Error deleting experiment. " + ex.Message);
                lblResponse.Visible = true;
            }
        }
	}
}

