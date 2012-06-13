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
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Security;

using iLabs.Core;
using iLabs.DataTypes;
using iLabs.DataTypes.ProcessAgentTypes;
using iLabs.DataTypes.StorageTypes;
using iLabs.Proxies.ESS;
using iLabs.ServiceBroker;
using iLabs.ServiceBroker.Internal;
using iLabs.ServiceBroker.Administration;
using iLabs.ServiceBroker.Authorization;
using iLabs.ServiceBroker.Authentication;
using iLabs.ServiceBroker.DataStorage;
using iLabs.ServiceBroker;

using iLabs.Ticketing;
using iLabs.DataTypes;
using iLabs.DataTypes.StorageTypes;
//using iLabs.DataTypes.LabServerTypes;
using iLabs.DataTypes.SoapHeaderTypes;
using iLabs.DataTypes.TicketingTypes;
using iLabs.UtilLib;


namespace iLabs.ServiceBroker.iLabSB
{
	/// <summary>
	/// User Experiments Page
	/// </summary>
	public partial class myExperiments : System.Web.UI.Page
	{
        CultureInfo culture;
        int userTZ;
		AuthorizationWrapperClass wrapper = new AuthorizationWrapperClass();
        int userID = -1;
        int groupID = -1;
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
           if (Session["UserID"]==null)
		        Response.Redirect("../login.aspx");

            if (ddlTimeAttribute.SelectedIndex != 4)
            {
                txtTime2.ReadOnly = true;
                txtTime2.BackColor = Color.Lavender;
            }
            else
            {
                txtTime2.ReadOnly = false;
                txtTime2.BackColor = Color.White;
            }
 			userTZ = Convert.ToInt32(Session["UserTZ"]);
            culture = DateUtil.ParseCulture(Request.Headers["Accept-Language"]);
            if (Session["UserID"] != null)
            {
                userID = Convert.ToInt32(Session["UserID"]);
            }
           
            if (Session["GroupID"] != null)
            {
                groupID = Convert.ToInt32(Session["GroupID"]);
            }

			if(! IsPostBack )
			{
                culture = DateUtil.ParseCulture(Request.Headers["Accept-Language"]);
                //txtTime2.Enabled = false;
				List<Criterion> cList = new List<Criterion> ();
                if (Session["UserID"] != null)
                {
                    cList.Add(new Criterion("User_ID", "=", Session["UserID"].ToString()));

                }
                //if (Session["GroupID"] != null)
                //{
                //    cList.Add(new Criterion("Group_ID", "=", Session["GroupID"].ToString()));
                //}
                long[] eIDs = DataStorageAPI.RetrieveAuthorizedExpIDs(userID, groupID, cList.ToArray());
				LongTag [] expTags = DataStorageAPI.RetrieveExperimentTags(eIDs, userTZ, culture, false, false, true, false, true, false, true, false);
			
				for(int i =0; i< expTags.Length ; i++)
				{
					//System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem(eIDs[i].ToString () +" on "+eIDsinfo[i].submissionTime.ToString(),eIDs[i].ToString());
                    System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem(expTags[i].tag, expTags[i].id.ToString());
					lbxSelectExperiment.Items .Add(item);
				}

				if(eIDs.Length == 0)
				{
					string msg = "No experiment records were found for user '"+Session["UserName"]+"' in group '"+Session["GroupName"]+"'.";
					lblResponse.Text = Utilities.FormatErrorMessage(msg);
					lblResponse.Visible = true;
				}
                // "Are you sure" javascript for DeleteExperiment button
                btnDeleteExperiment.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete this experiment?')== false) return false;");
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
            trDeleteExperiment.Visible = false;
        }
		
		protected void btnGo_Click(object sender, System.EventArgs e)
		{
			clearExperimentDisplay();
            lbxSelectExperiment.Items.Clear();
            List<Criterion> cList = new List<Criterion>();
            if (Session["UserID"] != null)
            {
                cList.Add(new Criterion("User_ID", "=", Session["UserID"].ToString()));
            }

            //if (Session["GroupID"] != null)
            //{
            //    cList.Add(new Criterion("Group_ID", "=", Session["GroupID"].ToString()));
            //}
			
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
			
           // cList.Add(new Criterion("Record_Count", ">", "0"));

            long[] eIDs = DataStorageAPI.RetrieveAuthorizedExpIDs(userID,groupID, cList.ToArray());
                                            //RetrieveExperimentTags(eIDs, userTZ, culture, false, false, true, false, true, false, true, false);
            LongTag[] expTags = DataStorageAPI.RetrieveExperimentTags(eIDs, userTZ, culture, false, false, true, false, true, false, true, false);

            for (int i = 0; i < expTags.Length; i++)
            {
                System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem(expTags[i].tag, expTags[i].id.ToString());
                lbxSelectExperiment.Items.Add(item);
            }
            if (eIDs.Length == 0)
            {
                string msg = "No experiment records were found for user '" + Session["UserName"] + "' in group '" + Session["GroupName"] + "'.";
                lblResponse.Text = Utilities.FormatErrorMessage(msg);
                lblResponse.Visible = true;
            }
		}

		protected void lbxSelectExperiment_SelectedIndexChanged(object sender, System.EventArgs e)
		{
            clearExperimentDisplay();
			long experimentID = Int64.Parse (lbxSelectExperiment.Items [lbxSelectExperiment.SelectedIndex ].Value);
			try
			{		
				ExperimentSummary[] expInfo = wrapper.GetExperimentSummaryWrapper (new long[] {experimentID});
				if(expInfo[0] != null)
				{
					txtExperimentID.Text = expInfo[0].experimentId.ToString();
					txtUsername.Text = expInfo[0].userName ;
                    txtGroupName.Text = expInfo[0].groupName;
					txtLabServerName.Text = expInfo[0].labServerName;
                    txtClientName.Text = expInfo[0].clientName;
                    //Check if update needed from the ESS if one is used
                    if( expInfo[0].essGuid != null){
                        int expStatus = expInfo[0].status;
                        if((expStatus == StorageStatus.UNKNOWN || expStatus == StorageStatus.INITIALIZED
                        || expStatus == StorageStatus.OPEN || expStatus == StorageStatus.REOPENED
                        ||expStatus == StorageStatus.RUNNING 
                        ||expStatus == StorageStatus.BATCH_QUEUED ||expStatus == StorageStatus.BATCH_RUNNING 
                        ||expStatus == StorageStatus.BATCH_TERMINATED ||expStatus == StorageStatus.BATCH_TERMINATED_ERROR))
                        {

                        // This operation should happen within the Wrapper
                        BrokerDB ticketIssuer = new BrokerDB();
                        ProcessAgentInfo ess = ticketIssuer.GetProcessAgentInfo(expInfo[0].essGuid);
                        if (ess == null || ess.retired)
                        {
                            throw new Exception("The ESS is not registered or is retired");
                        }
                        Coupon opCoupon = ticketIssuer.GetEssOpCoupon(expInfo[0].experimentId, TicketTypes.RETRIEVE_RECORDS,60,ess.agentGuid);
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
                    txtStatus.Text =  DataStorageAPI.getStatusString(expInfo[0].status);
					txtSubmissionTime.Text = DateUtil.ToUserTime(expInfo[0].creationTime,culture,userTZ);
                    if ((expInfo[0].closeTime != null) && (expInfo[0].closeTime != DateTime.MinValue))
                    {
                        txtCompletionTime.Text = DateUtil.ToUserTime(expInfo[0].closeTime, culture, userTZ);
                    }
                    else{
                        txtCompletionTime.Text = "Experiment Not Closed!";
                    }
                    txtRecordCount.Text = expInfo[0].recordCount.ToString("    0");
                    txtAnnotation.Text = expInfo[0].annotation;
                    trSaveAnnotation.Visible = true;
                    trDeleteExperiment.Visible = true;
                    trShowExperiment.Visible = (expInfo[0].recordCount > 0);
				}			
			}
			catch(Exception ex)
			{
				lblResponse.Text = Utilities.FormatErrorMessage("Error retrieving experiment information. " + ex.Message);
				lblResponse.Visible = true;
			}
		}

		protected void btnSaveAnnotation_Click(object sender, System.EventArgs e)
		{
			lblResponse.Visible=false;
			try{
				wrapper.SaveExperimentAnnotationWrapper(Int32.Parse(txtExperimentID.Text), txtAnnotation.Text);
                LongTag[] updateEXP = DataStorageAPI.RetrieveExperimentTags( new long[] { Convert.ToInt64(txtExperimentID.Text) },
                    userTZ, culture, false, false, true, false, true, false, true, false);
                if (updateEXP.Length == 1)
                {
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
            Response.Redirect("showExperiment.aspx?expid=" + txtExperimentID.Text, true);
        }

		protected void btnDeleteExperiment_Click(object sender, System.EventArgs e)
		{
			ArrayList aList = new ArrayList();

			try
			{
				lbxSelectExperiment.Items.Clear();
				wrapper.RemoveExperimentsWrapper(new long[] {Convert.ToInt32(txtExperimentID.Text)});
				if(Session["UserID"] != null)
				{
					aList.Add(new Criterion ("User_ID", "=", Session["UserID"].ToString() ));
				}

				if(Session["GroupID"] != null)
				{
					aList.Add(new Criterion ("Group_ID", "=", Session["GroupID"].ToString()));
				}

				Criterion[] carray = new Criterion [aList.Count];
			
				for(int i=0; i< aList.Count ; i++)
				{
					carray[i] = ( Criterion ) aList[i];
				}

				long[] eIDs = wrapper.FindExperimentIDsWrapper(carray);
                LongTag[] eTags = DataStorageAPI.RetrieveExperimentTags(eIDs, userTZ, culture, false, false, true, false, true, false, true, false);
			
				for(int i =0; i< eTags.Length ; i++)
				{
					System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem(eTags[i].tag, eTags[i].id.ToString()) ;
					lbxSelectExperiment.Items .Add(item);
				}

				if(eIDs.Length == 0)
				{
					string msg = "No experiment records were found for user '"+Session["UserName"]+"' in group '"+Session["GroupName"]+"'.";
					lblResponse.Text = Utilities.FormatErrorMessage(msg);
					lblResponse.Visible = true;
				}
			}
			catch (Exception ex)
			{
				lblResponse.Text = Utilities.FormatErrorMessage("Error deleting experiment. "+ ex.Message);
				lblResponse.Visible = true;
			}
		}
	}
}
