using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using iLabs.Core;
using iLabs.DataTypes.SchedulingTypes;
using iLabs.Ticketing;
using iLabs.DataTypes.TicketingTypes;
using iLabs.UtilLib;
using System.Xml;

namespace iLabs.Scheduling.LabSide
{
    /// <summary>
    /// Summary description for ExperimentInfoManagement.
    /// </summary>
    public partial class Manage : System.Web.UI.Page
    {
         string labServerGuid;
        string labServerName = null;

        string couponID = null, passkey = null, issuerID = null, sbUrl = null;
        LabSchedulingDB dbManager = new LabSchedulingDB();
        int userTZ = 0;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            

            // Put user code to initialize the page here
            btnRemove.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to remove this experiment Information?')== false) return false;");
            if (!IsPostBack)
            {

                if (Session["couponID"] == null || Request.QueryString["coupon_id"] != null)
                    couponID = Request.QueryString["coupon_id"];
                else
                    couponID = Session["couponID"].ToString();

                if (Session["passkey"] == null || Request.QueryString["passkey"] != null)
                    passkey = Request.QueryString["passkey"];
                else
                    passkey = Session["passkey"].ToString();

                if (Session["issuerID"] == null || Request.QueryString["issuer_guid"] != null)
                    issuerID = Request.QueryString["issuer_guid"];
                else
                    issuerID = Session["issuerID"].ToString();

                if (Session["sbUrl"] == null || Request.QueryString["sb_url"] != null)
                    sbUrl = Request.QueryString["sb_url"];
                else
                    sbUrl = Session["sbUrl"].ToString();

                bool unauthorized = false;

                if (couponID != null && passkey != null && issuerID != null)
                {
                    try
                    {
                        Coupon coupon = new Coupon(issuerID, long.Parse(couponID), passkey);


                        Ticket ticket = dbManager.RetrieveAndVerify(coupon, TicketTypes.MANAGE_LAB);

                        if (ticket.IsExpired() || ticket.isCancelled)
                        {
                            unauthorized = true;
                            Response.Redirect("Unauthorized.aspx", false);
                        }

                        Session["couponID"] = couponID;
                        Session["passkey"] = passkey;
                        Session["issuerID"] = issuerID;
                        Session["sbUrl"] = sbUrl;

                        XmlDocument payload = new XmlDocument();
                        payload.LoadXml(ticket.payload);

                        labServerGuid = payload.GetElementsByTagName("labServerGuid")[0].InnerText;
                        Session["labServerGuid"] = labServerGuid;
                        labServerName = payload.GetElementsByTagName("labServerName")[0].InnerText;
                        Session["labServerName"] = labServerName;
                        string sbGuid = payload.GetElementsByTagName("sbGuid")[0].InnerText;
                        Session["adminSbGuid"] = sbGuid;
                        string adminGroup = payload.GetElementsByTagName("adminGroup")[0].InnerText;
                        Session["adminGroup"] = adminGroup;
                        userTZ = Convert.ToInt32(payload.GetElementsByTagName("userTZ")[0].InnerText);
                        Session["userTZ"] = userTZ;
                    }

                    catch (Exception ex)
                    {
                        unauthorized = true;
                        Response.Redirect("Unauthorized.aspx", false);
                    }
                }

                else
                {
                    unauthorized = true;
                    Response.Redirect("Unauthorized.aspx", false);
                }

                if (!unauthorized)
                {
                    ProcessAgentInfo paInfo = null;
                    //if (labServerID != null)
                    //    paInfo = dbTicketing.GetProcessAgentInfo(labServerID);
                    //lblLabServerName.Text = Session["labServerName"].ToString();
                   // lblLabServerName.Text = labServerName;

                    BuildExperimentInfoListBox();
                }
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
        /* 
		 * Builds the Select Experiment List box. 
		 * By default, the box gets filled with all the experiment on the related labserver in the database
		 */
        private void BuildExperimentInfoListBox()
        {
            LssExperimentInfo[] experimentInfos = null;
            int[] experimentInfoIDs = dbManager.ListExperimentInfoIDsByLabServer(Session["labServerGuid"].ToString());
            if(experimentInfoIDs != null && experimentInfoIDs.Length > 0)
                experimentInfos = dbManager.GetExperimentInfos(experimentInfoIDs);
            //if related experiment experiments have been found
            if (experimentInfos != null && experimentInfos.Length > 0)
            {
                BuildExperimentInfoListBox(experimentInfos);
            }
            else //no related experiment exist
            {
                //BuildExperimentInfoListBox();
                lbxSelectExperiment.Items.Clear();
                string msg = "No experiments on Lab server " + Session["labServerName"].ToString() + " have been registered.";
                lblErrorMessage.Text = Utilities.FormatConfirmationMessage( msg);
                lblErrorMessage.Visible = true;
            }

        }

        /* 
         * Builds the Select Experiments List using a specified array of experiments. 
         * This is used to return the results of a search
         */
        private void BuildExperimentInfoListBox(LssExperimentInfo[] experimentInfos)
        {
            lbxSelectExperiment.Items.Clear();

            foreach (LssExperimentInfo experimentInfo in experimentInfos)
            {
                ListItem experimentInfoItem = new ListItem();
                experimentInfoItem.Text = experimentInfo.labClientName + ", " + experimentInfo.labClientVersion;
                experimentInfoItem.Value = experimentInfo.experimentInfoId.ToString();
                lbxSelectExperiment.Items.Add(experimentInfoItem);
            }
        }
        private void ResetState()
        {
            txtLabClientName.Text = "";
            txtLabClientName.Enabled = true;
            txtLabClientName.BackColor = Color.White;
            txtLabClientVersion.Text = "";
            txtLabClientVersion.Enabled = true;
            txtLabClientVersion.BackColor = Color.White;
            txtClientGuid.Text = "";
            txtClientGuid.ReadOnly = false;
            txtClientGuid.Enabled = true;
            txtClientGuid.BackColor = Color.White;
            //txtLabServerID.Text = "";
            //txtLabServerID.Enabled = true;
            //txtLabServerID.BackColor = Color.White;
            //txtLabServerName.Text = "";
            txtMinimumTime.Text = "";
            txtPrepareTime.Text = "";
            txtProviderName.Text = "";
            txtRecoverTime.Text = "";
            txtEarlyArriveTime.Text = "";
        }

        private void DisplayExperimentInfo(LssExperimentInfo experimentInfo)
        {
            ResetState();
            txtLabClientName.Text = experimentInfo.labClientName;
            txtLabClientName.Enabled = false;
            txtLabClientName.BackColor = Color.FromArgb(243, 239, 229);
            txtLabClientVersion.Text = experimentInfo.labClientVersion;
            txtLabClientVersion.Enabled = false;
            txtLabClientVersion.BackColor = Color.FromArgb(243, 239, 229);
            txtClientGuid.Text = experimentInfo.labClientGuid;
            txtClientGuid.ReadOnly = true;
            txtClientGuid.Enabled = false;
            txtClientGuid.BackColor = Color.FromArgb(243, 239, 229);
            //txtLabServerID.Text = experimentInfo.labServerID;
            //txtLabServerID.Enabled = false;
            //txtLabServerID.BackColor = Color.FromArgb(243,239,229);
            //txtLabServerName.Text = experimentInfo.labServerName;
            txtMinimumTime.Text = experimentInfo.minimumTime.ToString();
            txtPrepareTime.Text = experimentInfo.prepareTime.ToString();
            txtProviderName.Text = experimentInfo.providerName;
            txtContactEmail.Text = experimentInfo.contactEmail;
            txtRecoverTime.Text = experimentInfo.recoverTime.ToString();
            txtEarlyArriveTime.Text = experimentInfo.earlyArriveTime.ToString();
        }

        protected void lbxSelectExperiment_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (lbxSelectExperiment.SelectedIndex < 0)
            {
                lblErrorMessage.Text = Utilities.FormatWarningMessage("You must select a experiment!");
                lblErrorMessage.Visible = true;
            }
            else
            {
                try
                {
                    LssExperimentInfo[] experimentInfo = dbManager.GetExperimentInfos(new int[] { Int32.Parse(lbxSelectExperiment.SelectedValue) });
                    DisplayExperimentInfo(experimentInfo[0]);
                }
                catch (Exception ex)
                {
                    string msg = "Exception: Cannot retrieve experiment's information. " + ex.Message + ". " + ex.GetBaseException();
                    lblErrorMessage.Text = Utilities.FormatErrorMessage(msg);
                    lblErrorMessage.Visible = true;
                }
            }
        }




        protected void btnSave_Click(object sender, System.EventArgs e)
        {
            int min = -1;
            int quan = -1;
            int pt = -1;
            int rt = -1;
            int et = -1;
            lblErrorMessage.Text = "";
            lblErrorMessage.Visible = false;
            //Error checking for empty fields
            if (txtLabClientName.Text.CompareTo("") == 0)
            {
                lblErrorMessage.Text = Utilities.FormatWarningMessage("Enter a LabClientName.");
                lblErrorMessage.Visible = true;
                return;
            }

            if (txtLabClientVersion.Text.CompareTo("") == 0)
            {
                lblErrorMessage.Text = Utilities.FormatWarningMessage("You must enter the lab Clinet's Version.");
                lblErrorMessage.Visible = true;
                return;
            }

            if (txtClientGuid.Text.CompareTo("") == 0)
            {
                lblErrorMessage.Text = Utilities.FormatWarningMessage("You must enter the lab Clinet's GUID.");
                lblErrorMessage.Visible = true;
                return;
            }
            

            if (txtMinimumTime.Text.CompareTo("") == 0)
            {
                lblErrorMessage.Text = Utilities.FormatWarningMessage("You must enter the Minimum Excution Time of the experiment.");
                lblErrorMessage.Visible = true;
                return;
            }
            
            try
            {
                min = Int32.Parse(txtMinimumTime.Text);
              

            }
            catch (Exception ex)
            {
                lblErrorMessage.Text = Utilities.FormatWarningMessage("Please enter a positive integer in the MinimumTime text box.");
                lblErrorMessage.Visible = true;
                return;
            }
            if (min <= 0)
            {
                lblErrorMessage.Text = Utilities.FormatWarningMessage("Please enter an integer value greater than Zero in the MinimumTime text box.");
                lblErrorMessage.Visible = true;
                return;
            }
            
            if (txtEarlyArriveTime.Text.CompareTo("") == 0)
            {
                lblErrorMessage.Text = Utilities.FormatWarningMessage("You must enter the early arrive Time of the experiment.");
                lblErrorMessage.Visible = true;
                return;
            }
            try
            {
                et = Int32.Parse(txtEarlyArriveTime.Text);

            }
            catch (Exception ex)
            {
                lblErrorMessage.Text = Utilities.FormatWarningMessage("Please enter an integer in the EarlyArriveTime text box.");
                lblErrorMessage.Visible = true;
                return;
            }
            if (txtPrepareTime.Text.CompareTo("") == 0)
            {
                lblErrorMessage.Text = Utilities.FormatWarningMessage("You must enter the Prepare Time of the experiment.");
                lblErrorMessage.Visible = true;
                return;
            }
            try
            {
                pt = Int32.Parse(txtPrepareTime.Text);

            }
            catch (Exception ex)
            {
                lblErrorMessage.Text = Utilities.FormatWarningMessage("Please enter an integer in the PrepareTime text box.");
                lblErrorMessage.Visible = true;
                return;
            }
            //if (txtProviderName.Text.CompareTo("") == 0)
            //{
            //    lblErrorMessage.Text = "You must enter the USS's provider name.";
            //    lblErrorMessage.Visible = true;
            //    return;
            //}
            
            if (txtRecoverTime.Text.CompareTo("") == 0)
            {
                lblErrorMessage.Text = Utilities.FormatWarningMessage("You must enter the Recover Time of the experiment.");
                lblErrorMessage.Visible = true;
                return;
            }
            try
            {
                rt = Int32.Parse(txtRecoverTime.Text);

            }
            catch (Exception ex)
            {
                lblErrorMessage.Text = Utilities.FormatWarningMessage("Please enter an integer in the RecoverTime text box.");
                lblErrorMessage.Visible = true;
                return;
            }


            //If all the error checks are cleared

            //if adding a new ExperimentInfo
            if (txtLabClientName.Enabled)
            {
                try
                {
                    // the database will also throw an exception if the combination of LabClientName and LabClientVersion exists
                    // since combination of LabClientName and LabClientVersion must be unique .
                    // this is just another check to throw a meaningful exception
                    int eID = dbManager.ListExperimentInfoIDByExperiment( labServerGuid, txtClientGuid.Text);
                    if (eID >= 0) // then the experiment already exists in database
                    {
                        string msg = "The experiment '" + txtLabClientName.Text + " " + txtLabClientVersion.Text + "' already exists. Choose another lab client name or lab client version.";
                        lblErrorMessage.Text = Utilities.FormatWarningMessage(msg);
                        lblErrorMessage.Visible = true;
                        lbxSelectExperiment.Items.FindByValue(eID.ToString()).Selected = true;
                        return;
                    }
                    else
                    {

                        //Add ExperimentInfo
                        int experimentInfoID = dbManager.AddExperimentInfo(Session["labServerGuid"].ToString(),
                            Session["labServerName"].ToString(),txtClientGuid.Text, txtLabClientName.Text,
                            txtLabClientVersion.Text, txtProviderName.Text,txtContactEmail.Text,
                            pt, rt, min, et);
                        string msg = "The record for the experiment '" + txtLabClientName.Text + " " + txtLabClientVersion.Text + "' has been created successfully.";
                        lblErrorMessage.Text = Utilities.FormatConfirmationMessage(msg);
                        lblErrorMessage.Visible = true;
                        txtLabClientName.Enabled = false;
                        txtLabClientName.BackColor = Color.FromArgb(243, 239, 229);
                        txtLabClientVersion.Enabled = false;
                        txtLabClientVersion.BackColor = Color.FromArgb(243, 239, 229);
                        BuildExperimentInfoListBox();
                        //select the recently added ExperimentInfo in the list box
                        lbxSelectExperiment.Items.FindByValue(experimentInfoID.ToString()).Selected = true;
                        return;
                    }
                }
                catch (Exception ex)
                {
                    string msg = "Exception: Cannot add the experiment '" + txtLabClientName.Text + " " + txtLabClientVersion.Text + "'. " + ex.Message + ". " + ex.GetBaseException() + ".";
                    lblErrorMessage.Text = Utilities.FormatErrorMessage(msg);
                    lblErrorMessage.Visible = true;
                }
            }
            else // if updating an old ExperimentInfo
            {
                try
                {
                    //Update Experiment information
                    dbManager.ModifyExperimentInfo(dbManager.ListExperimentInfoIDByExperiment( Session["labServerGuid"].ToString(), txtClientGuid.Text),
                        Session["labServerGuid"].ToString(), Session["labServerName"].ToString(),
                        txtClientGuid.Text, txtLabClientName.Text, txtLabClientVersion.Text,
                        txtProviderName.Text, txtContactEmail.Text, pt, rt, min, et);
                    string msg = "The record for the experiment '" + txtLabClientName.Text + " " + txtLabClientVersion.Text + "' has been updated.";
                    lblErrorMessage.Text = Utilities.FormatConfirmationMessage(msg);
                    lblErrorMessage.Visible = true;
                    return;
                }
                catch (Exception ex)
                {
                    string msg = "Exception: Cannot update the experiment '" + txtLabClientName.Text + " " + txtLabClientVersion.Text + "'. " + ex.Message + ". " + ex.GetBaseException() + ".";
                    lblErrorMessage.Text = msg;
                    lblErrorMessage.Visible = true;
                }
            }
        }

        protected void btnRemove_Click(object sender, System.EventArgs e)
        {
            if (lbxSelectExperiment.SelectedIndex < 0)
            {
                lblErrorMessage.Text = "Select a experimentInfo to be deleted.";
                lblErrorMessage.Visible = true;
                return;
            }
            try
            {
                if (dbManager.RemoveExperimentInfo(new int[] { dbManager.ListExperimentInfoIDByExperiment(Session["labServerGuid"].ToString(), txtClientGuid.Text) }).Length > 0)
                {
                    string msg = "The experiment '" + txtLabClientName.Text + " " + txtLabClientVersion.Text + "' was not deleted.";
                    lblErrorMessage.Text = Utilities.FormatErrorMessage(msg);
                    lblErrorMessage.Visible = true;
                }
                else
                {
                    string msg = "The experiment '" + txtLabClientName.Text + " " + txtLabClientVersion.Text + "' has been deleted.";
                    lblErrorMessage.Text = Utilities.FormatConfirmationMessage(msg);
                    lblErrorMessage.Visible = true;
                }
            }
            catch (Exception ex)
            {
                string msg = "Exception: Cannot delete the experiment '" + txtLabClientName.Text + " " + txtLabClientVersion.Text + "'. " + ex.Message + ". " + ex.GetBaseException();
                lblErrorMessage.Text = Utilities.FormatErrorMessage(msg);
                lblErrorMessage.Visible = true;
            }
            finally
            {
                ResetState();
                //txtSearchBy.Text = "";
                BuildExperimentInfoListBox();
            }
        }

        protected void btnNew_Click(object sender, System.EventArgs e)
        {
            ResetState();
            //txtSearchBy.Text = "";
            BuildExperimentInfoListBox();
        }



    }

}

