using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;


using iLabs.Core;
using iLabs.DataTypes;
using iLabs.DataTypes.ProcessAgentTypes;
using iLabs.DataTypes.StorageTypes;
//using iLabs.DataTypes.BatchServerTypes;
using iLabs.DataTypes.SoapHeaderTypes;
using iLabs.DataTypes.TicketingTypes;
using iLabs.Proxies.ESS;
using iLabs.ServiceBroker;
using iLabs.ServiceBroker.Internal;
using iLabs.ServiceBroker.Administration;
using iLabs.ServiceBroker.Authorization;
using iLabs.ServiceBroker.Authentication;
using iLabs.ServiceBroker.DataStorage;

using iLabs.Ticketing;

using iLabs.UtilLib;

namespace iLabs.ServiceBroker.iLabSB
{

public partial class displayExperiment : System.Web.UI.UserControl
{
    protected System.Data.DataTable dtRecords;
    protected System.Data.DataTable dtBlobs;

    CultureInfo culture;
    int userTZ;
    AuthorizationWrapperClass wrapper = new AuthorizationWrapperClass();
    protected string back_Url;

    public string backUrl
    {
        get
        {
            return back_Url;
        }
        set
        {
            back_Url = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        userTZ = Convert.ToInt32(Session["UserTZ"]);
        culture = DateUtil.ParseCulture(Request.Headers["Accept-Language"]);

        if (!IsPostBack)
        {
            culture = DateUtil.ParseCulture(Request.Headers["Accept-Language"]);
            ArrayList aList = new ArrayList();


            if (Session["UserID"] == null)
            {
                //aList.Add(new Criterion ("User_ID", "=", Session["UserID"].ToString() ));
            }

            if (Session["GroupID"] == null)
            {
                //aList.Add(new Criterion ("Group_ID", "=", Session["GroupID"].ToString()));
            }

            string expIdStr = Request.QueryString["expid"];
            if (expIdStr != null && expIdStr.Length > 0)
            {
                try
                {
                    long expId = Int64.Parse(expIdStr);
                    selectExperiment(expId);
                }
                catch (Exception ex)
                {
                    string msg = "Error retrieving experiment: '" + expIdStr + "'. " + ex.Message;
                    lblResponse.Text = Utilities.FormatErrorMessage(msg);
                    lblResponse.Visible = true;
                }
            }
            else
            {
                string msg = "No experiment was specified.";
                lblResponse.Text = Utilities.FormatErrorMessage(msg);
                lblResponse.Visible = true;

            }
            // "Are you sure" javascript for DeleteExperiment button
            btnDeleteExperiment.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete this experiment?')== false) return false;");

        }
    }
        
		protected void selectExperiment(long experimentID)
		{
			lblResponse.Visible = false;

			txtExperimentID.Text = null;
			txtUsername.Text = null ;
			txtLabServerName.Text = null ;
            txtClientName.Text = null;
			txtGroupName.Text = null ;
			txtStatus.Text = null;
			txtSubmissionTime.Text = null;
			txtCompletionTime.Text = null;
			txtRecordCount.Text = null ;
			txtAnnotation.Text = null;
            txtExperimentRecords.Text = null;
            btnDisplayRecords.Visible = false;
            divRecords.Visible = false;
            divBlobs.Visible = false;
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
                    

                    // getting records at this time:
                    
                    //Get the Experiment records from the ESS if one is used
                    if (expInfo[0].essGuid != null)
                    {
                        Session["EssGuid"] = expInfo[0].essGuid;
                        btnDisplayRecords.Visible = true;
                        //displayRecords(experimentID, expInfo[0].essGuid);

                    }
                    else
                    {
                        Session.Remove("EssGuid");
                    }
                    txtStatus.Text = DataStorageAPI.getStatusString(expInfo[0].status);

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
					
				}
				
				
			}
			catch(Exception ex)
			{
				lblResponse.Text = Utilities.FormatErrorMessage("Error retrieving experiment information. " + ex.Message);
				lblResponse.Visible = true;
			}
		}

        protected void displayRecords(long experimentId, string essGuid)
        {
           
            BrokerDB ticketIssuer = new BrokerDB();
            ProcessAgentInfo ess = ticketIssuer.GetProcessAgentInfo(essGuid);
            if (ess == null || ess.retired)
            {
                throw new Exception("The ESS is not registered or is retired");
            }
            Coupon opCoupon = ticketIssuer.GetEssOpCoupon(experimentId, TicketTypes.RETRIEVE_RECORDS, 60, ess.agentGuid);
            if (opCoupon != null)
            {

                ExperimentStorageProxy essProxy = new ExperimentStorageProxy();
                OperationAuthHeader header = new OperationAuthHeader();
                header.coupon = opCoupon;
                essProxy.Url = ess.webServiceUrl;
                essProxy.OperationAuthHeaderValue = header;
               
                ExperimentRecord[] records = essProxy.GetRecords(experimentId,null);
                if (records != null)
                {
                   
                    StringBuilder buf = null;
                    if (cbxContents.Checked)
                    {
                        buf = new StringBuilder();
                        foreach (ExperimentRecord rec in records)
                        {
                            buf.AppendLine(rec.contents);
                        }
                        txtExperimentRecords.Text = buf.ToString();
                        txtExperimentRecords.Visible = true;
                        grvExperimentRecords.Visible = false;
                    }
                    else
                    {
                        dtRecords = new DataTable();
                        dtRecords.Columns.Add("Seq_Num", typeof(System.Int32));
                        dtRecords.Columns.Add("Record Type", typeof(System.String));
                        dtRecords.Columns.Add("Contents", typeof(System.String));
                        foreach (ExperimentRecord rec in records)
                        {
                            DataRow recTmp = dtRecords.NewRow();
                            recTmp["Seq_Num"] = rec.sequenceNum;
                            recTmp["Record Type"] = rec.type;
                            recTmp["Contents"] = rec.contents;
                            dtRecords.Rows.InsertAt(recTmp, dtRecords.Rows.Count);
                        }
                        grvExperimentRecords.DataSource = dtRecords;
                       
                        grvExperimentRecords.DataBind();
                        grvExperimentRecords.Visible = true;
                        txtExperimentRecords.Visible = false;

                    }
                 
                    divRecords.Visible = true;
                }
                Blob[] blobs = essProxy.GetBlobs(experimentId);
                if (blobs != null)
                {
                    
                  
                    
                    dtBlobs = new DataTable();
                    dtBlobs.Columns.Add("Blob_ID", typeof(System.Int64));
                    dtBlobs.Columns.Add("Seq_Num", typeof(System.Int32));
                    dtBlobs.Columns.Add("MimeType", typeof(System.String));
                    dtBlobs.Columns.Add("Description", typeof(System.String));
                    foreach (Blob b in blobs)
                    {
                        DataRow blobTmp = dtBlobs.NewRow();
                        blobTmp["Blob_ID"] = b.blobId;
                        blobTmp["Seq_Num"] = b.recordNumber;
                        blobTmp["MimeType"] = b.mimeType;
                        blobTmp["Description"] = b.description;
                        dtBlobs.Rows.InsertAt(blobTmp, dtBlobs.Rows.Count);
                    }
                    grvBlobs.DataSource = dtBlobs;
                    grvBlobs.DataBind();
                    divBlobs.Visible = true;

                }
            }
        }
                        
		protected void btnSaveAnnotation_Click(object sender, System.EventArgs e)
		{
			lblResponse.Visible=false;

			try
			{
				wrapper.SaveExperimentAnnotationWrapper(Int32.Parse(txtExperimentID.Text), txtAnnotation.Text);

				lblResponse.Text = Utilities.FormatConfirmationMessage("Annotation saved for experiment ID " + txtExperimentID.Text);
				lblResponse.Visible = true;
			}
			catch (Exception ex)
			{
				lblResponse.Text = Utilities.FormatErrorMessage("Error saving experiment annotation. " + ex.Message);
				lblResponse.Visible = true;
			}
		}

        protected void btnBack_Click(object sender, System.EventArgs e)
        {
            Response.Redirect(backUrl);
        }

		protected void btnDeleteExperiment_Click(object sender, System.EventArgs e)
		{
            lblResponse.Visible = false;
            StringBuilder buf = new StringBuilder();
            buf.Append("Experiment ");
            buf.Append(txtExperimentID.Text + " ");
            buf.Append(txtClientName.Text);

            try
            {

                wrapper.RemoveExperimentsWrapper(new long[] { Convert.ToInt64(txtExperimentID.Text) });

                txtExperimentID.Text = null;
                txtUsername.Text = null;
                txtLabServerName.Text = null;
                txtClientName.Text = null;
                txtGroupName.Text = null;
                txtStatus.Text = null;
                txtSubmissionTime.Text = null;
                txtCompletionTime.Text = null;
                txtRecordCount.Text = null;
                txtAnnotation.Text = null;
                txtExperimentRecords.Text = null;
                Session.Remove("EssGuid");
                Response.Redirect(backUrl + "?message=Deleted " + buf.ToString());

            }
            catch (Exception ex)
            {

                lblResponse.Text = Utilities.FormatErrorMessage("Error deleting " + buf.ToString() + ". - " + ex.Message);
                lblResponse.Visible = true;
            }
		}

        protected void btnDisplayRecords_Click(object sender, System.EventArgs e)
        {
            lblResponse.Visible = false;
            if (Session["EssGuid"] != null)
            {
                try
                {
                    displayRecords(Int64.Parse(txtExperimentID.Text), Session["EssGuid"].ToString());


                }
                catch (Exception ex)
                {
                    lblResponse.Text = Utilities.FormatErrorMessage("Error retrieving experiment records. " + ex.Message);
                    lblResponse.Visible = true;
                }
            }
            else
            {
                lblResponse.Text = Utilities.FormatWarningMessage("No ESS is specified, so no records. ");
                lblResponse.Visible = true;
            }
        }

        protected void On_BindBlobRow(object sender, GridViewRowEventArgs e)
        {
            GridViewRowEventArgs args = (GridViewRowEventArgs)e;
            GridView gv = (GridView)sender;
          
            if(e.Row.RowType == DataControlRowType.DataRow)
            {
                // Display the company name in italics.
                e.Row.Cells[1].Text = "<i>" + e.Row.Cells[1].Text + "</i>";
                
                DataRowView blobR = (DataRowView)  e.Row.DataItem;
                long blobid = (long) blobR["Blob_ID"];
                Object o = e.Row.Controls[0];
               // ((ButtonField)gvr.Controls[0]).Text = blobid.ToString();
               // ((ButtonField)gvr.Controls[0]).CommandName = blobid.ToString();

            }

        


            
        }

        protected void On_BlobSelected(object sender, GridViewCommandEventArgs e)
        {
            lblResponse.Visible = false;
            if (Session["EssGuid"] != null)
            {
                long blobId = Convert.ToInt64(e.CommandArgument);

                BrokerDB brokerDB = new BrokerDB();
                ProcessAgentInfo ess = brokerDB.GetProcessAgentInfo(Session["EssGuid"].ToString());
                if (ess == null || ess.retired)
                {
                    throw new Exception("The ESS is not registered or is retired");
                }
                Coupon opCoupon = brokerDB.GetEssOpCoupon(Convert.ToInt64(txtExperimentID.Text), TicketTypes.RETRIEVE_RECORDS, 60, ess.agentGuid);
                if (opCoupon != null)
                {

                    ExperimentStorageProxy essProxy = new ExperimentStorageProxy();
                    OperationAuthHeader header = new OperationAuthHeader();
                    header.coupon = opCoupon;
                    essProxy.Url = ess.webServiceUrl;
                    essProxy.OperationAuthHeaderValue = header;
                    string url = essProxy.RequestBlobAccess(blobId, "http", 30);
                    if (url != null)
                    {

                        string jScript = "<script language='javascript'>" +
                                    "window.open('" + url + "')" + "</script>";
                        Page.RegisterStartupScript("Open New Window", jScript);
                        //Response.Redirect(url);
                    }
                    else{
                        lblResponse.Text = Utilities.FormatWarningMessage("Could not access BLOB. ");
                        lblResponse.Visible = true;
                    }
                }
            }
            else
            {
                lblResponse.Text = Utilities.FormatWarningMessage("No ESS is specified, so no records. ");
                lblResponse.Visible = true;
            }
        }
	
    }
}
