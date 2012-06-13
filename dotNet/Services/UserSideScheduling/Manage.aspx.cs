using System;
using System.Data;
using System.Drawing;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml;
using System.IO;

using iLabs.Core;
using iLabs.DataTypes.ProcessAgentTypes;
using iLabs.DataTypes.TicketingTypes;
using iLabs.DataTypes.SchedulingTypes;

using iLabs.Ticketing;

using iLabs.UtilLib;


namespace iLabs.Scheduling.UserSide
{
    /// <summary>
    /// Summary description for RegisterLSS.
    /// </summary>
    public partial class Manage : System.Web.UI.Page
    {
         int policyID;
         int[] policyIDs;
         USSPolicy[] policies;
         int[] experimentInfoIds;
         UssExperimentInfo[] experimentInfos;
         int[] credentialSetIds;
         UssCredentialSet[] credentials;
        UserSchedulingDB dbManager = new UserSchedulingDB();

        string couponID = null, passkey = null, issuerID = null, sbUrl = null, groupName = null, sbGuid = null;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            btnRemove.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to remove this experiment Information?')== false) return false;");
            policyIDs = dbManager.ListUSSPolicyIDs();
            policies = dbManager.GetUSSPolicies(policyIDs);
            experimentInfoIds = dbManager.ListExperimentInfoIDs();
            experimentInfos = dbManager.GetExperimentInfos(experimentInfoIds);
            credentialSetIds = dbManager.ListCredentialSetIds();
            credentials = dbManager.GetCredentialSets(credentialSetIds);
            
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

                        ProcessAgentDB dbTicketing = new ProcessAgentDB();
                        Ticket ticket = dbTicketing.RetrieveAndVerify(coupon, TicketTypes.MANAGE_USS_GROUP);

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

                        groupName = payload.GetElementsByTagName("groupName")[0].InnerText;
                        Session["groupName"] = groupName;
                        sbGuid = payload.GetElementsByTagName("sbGuid")[0].InnerText;
                        Session["sbGuid"] = sbGuid;
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
                    
                    // Load the Policy list box
                    ddlPolicy.Items.Add(new ListItem(" ---------- select Policy ---------- "));
                    for (int i = 0; i < policies.Length; i++)
                    {
                        UssExperimentInfo exp = dbManager.GetExperimentInfos(new int[] { policies[i].experimentInfoId })[0];
                        string expStr = exp.labClientName + " " + exp.labClientVersion;
                        UssCredentialSet cre = dbManager.GetCredentialSets(new int[] { policies[i].credentialSetId })[0];
                        string creStr = cre.serviceBrokerName + " " + cre.groupName;
                        string pol = creStr + " _ " + expStr;
                        ddlPolicy.Items.Add(new ListItem(pol, policies[i].ussPolicyId.ToString()));
                    }
                    // Load the Experiment box
                    ddlExperiment.Items.Add(new ListItem(" ---------- select Experiment ---------- "));
                    for (int i = 0; i < experimentInfos.Length; i++)
                    {
                        string exper = experimentInfos[i].labClientName + " : " + experimentInfos[i].labClientVersion;
                        ddlExperiment.Items.Add(new ListItem(exper, experimentInfos[i].experimentInfoId.ToString()));
                    }

                    int index = -1;

                    // Load the Credential box
                    ddlGroup.Items.Add(new ListItem(" ---------- select Group ---------- "));
                    for (int i = 0; i < credentials.Length; i++)
                    {
                        string creStr = credentials[i].serviceBrokerName + " : " + credentials[i].groupName;
                        if (credentials[i].groupName.Equals(groupName) && credentials[i].serviceBrokerGuid.Equals(sbGuid))
                            index = credentials[i].credentialSetId;

                        ddlGroup.Items.Add(new ListItem(creStr, credentials[i].credentialSetId.ToString()));

                    }

                    ddlGroup.SelectedValue = index.ToString();
                    ddlGroup.Enabled = false;

                    // Set the GUID field to not ReadOnly
                    SetReadOnly(false);
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
        /// <summary>
        /// Clears the Experiment dropdown and reloads it from the array of Policy objects
        /// </summary>
        private void InitializePolicyDropDown()
        {
            policyIDs = dbManager.ListUSSPolicyIDs();
            policies = dbManager.GetUSSPolicies(policyIDs);

            ddlPolicy.Items.Clear();

            ddlPolicy.Items.Add(new ListItem(" ---------- select Policy ---------- "));
            for (int i = 0; i < policies.Length; i++)
            {
                UssExperimentInfo exp = dbManager.GetExperimentInfos(new int[] { policies[i].experimentInfoId })[0];
                string expStr = exp.labClientName + " " + exp.labClientVersion;
                UssCredentialSet cre = dbManager.GetCredentialSets(new int[] { policies[i].credentialSetId })[0];
                string creStr = cre.serviceBrokerName + " " + cre.groupName;
                string pol = creStr + " _ " + expStr;
                ddlPolicy.Items.Add(new ListItem(pol, policies[i].ussPolicyId.ToString()));
            }
        }
        /// <summary>
        /// The experiemnt + group cannot be edited in an existing record,
        /// but they must be specified for a new record.
        /// This method resets the ReadOnly state and background colors of these fields.
        /// </summary>
        /// <param name="readOnlySwitch">true if ReadOnly, false if not</param>
        private void SetReadOnly(bool readOnlySwitch)
        {
            //243,239,229 - light green
            //174,155,138 - brown
            if (readOnlySwitch)
            {
                ddlExperiment.Enabled = false;
                ddlExperiment.BackColor = Color.FromArgb(243, 239, 229);
                ddlGroup.Enabled = false;
                ddlGroup.BackColor = Color.FromArgb(243, 239, 229);
            }
            else
            {
                ddlGroup.Enabled = true;
                ddlExperiment.Enabled = true;
                ddlExperiment.BackColor = Color.White;
                ddlGroup.BackColor = Color.White;
            }
        }

        /// <summary>
        /// This method clears the form fields.
        /// </summary>
        private void ClearFormFields()
        {
            txtMaxReservableTimeSlots.Text = "";
            txtMinReservableTimeSlots.Text = "";
            SetReadOnly(false);
            ddlGroup.ClearSelection();
            ddlGroup.Items[0].Selected = true;
            ddlExperiment.ClearSelection();
            ddlExperiment.Items[0].Selected = true;
        }
        /// <summary>
        /// This method fires when the Policy dropdown changes.
        /// If the index is greater than zero, the specified Policy will be looked up
        /// and its values will be used to populate the text fields on the form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlPolicy_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (ddlPolicy.SelectedIndex == 0)
            // prepare for a new record
            {
                ClearFormFields();
                SetReadOnly(false);

            }
            else
            //retrieve an existing record
            {
                USSPolicy policy = new USSPolicy();
                policy = policies[ddlPolicy.SelectedIndex - 1];

                txtMaxReservableTimeSlots.Text = PolicyParser.getProperty(policy.rule, lblfield2.Text);
                txtMinReservableTimeSlots.Text = PolicyParser.getProperty(policy.rule, lblfield3.Text);
                ddlGroup.ClearSelection();
                ddlGroup.Items.FindByValue(policy.credentialSetId.ToString()).Selected = true;
                ddlExperiment.ClearSelection();
                ddlExperiment.Items.FindByValue(policy.experimentInfoId.ToString()).Selected = true;
                SetReadOnly(true);

            }
        }

        protected void btnNew_Click(object sender, System.EventArgs e)
        {
            ddlPolicy.SelectedIndex = 0;
            ClearFormFields();
            SetReadOnly(false);
        }
        /// <summary>
        /// The Save Button method. If the Experiment and group field is not set to ReadOnly, this method
        /// will assume that a new record is being created. Otherwise, it will assume that
        /// an existing record is being edited.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSaveChanges_Click(object sender, System.EventArgs e)
        {
            //Error checking for empty fields
            string [] ruleFields = new string[2] { lblfield2.Text, lblfield3.Text };
            string[] properties = new string[2] { txtMaxReservableTimeSlots.Text, txtMinReservableTimeSlots.Text };
            int max = 0;
            int min = 0;
            if (txtMaxReservableTimeSlots.Text.CompareTo("") == 0)
            {
                lblErrorMessage.Text = Utilities.FormatWarningMessage("Please enter the maximum amount of time that may be reserved.");
                lblErrorMessage.Visible = true;
                return;
            }
            try
            {
                max = Int32.Parse(txtMaxReservableTimeSlots.Text);

            }
            catch (Exception ex)
            {
                lblErrorMessage.Text = Utilities.FormatWarningMessage("Please enter an integer in the maximum time text box.");
                lblErrorMessage.Visible = true;
                return;
            }
            if (txtMinReservableTimeSlots.Text.CompareTo("") == 0)
            {
                lblErrorMessage.Text = Utilities.FormatWarningMessage("Please enter the minimum required experiment time.");
                lblErrorMessage.Visible = true;
                return;
            }

            try
            {
               min = Int32.Parse(txtMinReservableTimeSlots.Text);

            }
            catch (Exception ex)
            {
                lblErrorMessage.Text = Utilities.FormatWarningMessage("Please enter an integer in the Minimum Reservable Time text box.");
                lblErrorMessage.Visible = true;
                return;
            }
            if(!(min > 0 && max >= min)){
                lblErrorMessage.Text = Utilities.FormatWarningMessage("The minimum amount of time must be greater than zero and the maximum must be greater than or equal to the minimum.");
                lblErrorMessage.Visible = true;
                return;
            }

            if (ddlGroup.SelectedIndex == 0)
            {
                lblErrorMessage.Text = Utilities.FormatWarningMessage("You must select a group.");
                lblErrorMessage.Visible = true;
                return;
            }

            if (ddlExperiment.SelectedIndex == 0)
            {
                lblErrorMessage.Text = Utilities.FormatWarningMessage("You must select a experiment.");
                lblErrorMessage.Visible = true;
                return;
            }

            ///////////////////////////////////////////////////////////////
            /// ADD a new Policy                                          //
            ///////////////////////////////////////////////////////////////
            if (ddlGroup.Enabled == true || ddlPolicy.SelectedIndex <= 0) // add new record
            {

                // see if this policy already exists
                foreach (USSPolicy policy in policies)
                {
                    if ((ddlGroup.SelectedValue == policy.credentialSetId.ToString()) && (ddlExperiment.SelectedValue == policy.experimentInfoId.ToString()))
                    {
                        lblErrorMessage.Visible = true;
                        string mesHead = setMessageHeader();
                        lblErrorMessage.Text = Utilities.FormatWarningMessage(mesHead + " exists, choose another one");
                        return;
                    }
                }

                // Add the Policy
                int savedIndexforGroup = ddlGroup.SelectedIndex;
                int savedIndexforExperiment = ddlExperiment.SelectedIndex;
                try
                {

                    UssCredentialSet cre = dbManager.GetCredentialSets(new int[] { Int32.Parse(ddlGroup.SelectedValue) })[0];
                    policyID = dbManager.AddUSSPolicy(cre.groupName, cre.serviceBrokerGuid, Int32.Parse(ddlExperiment.SelectedValue), PolicyParser.GenerateRule(ruleFields, properties));
                }
                catch (Exception ex)
                {
                    lblErrorMessage.Visible = true;
                    lblErrorMessage.Text = Utilities.FormatErrorMessage(ex.Message);
                    return;
                }

                // If successful...
                if (policyID != -1)
                {
                    lblErrorMessage.Visible = true;
                    // set dropdown to newly created credential set.
                    InitializePolicyDropDown();
                    ddlPolicy.Items.FindByValue(policyID.ToString()).Selected = true;
                    ddlGroup.ClearSelection();
                    ddlGroup.Items[savedIndexforGroup].Selected = true;
                    ddlExperiment.ClearSelection();
                    ddlExperiment.Items[savedIndexforExperiment].Selected = true;
                    string mesHead = setMessageHeader();
                    lblErrorMessage.Text =Utilities.FormatConfirmationMessage(mesHead + " been added.");
                    SetReadOnly(true);
                }
                else // cannot create policy	
                {
                    lblErrorMessage.Visible = true;
                    lblErrorMessage.Text = Utilities.FormatErrorMessage("Cannot create the policy.");
                    return;
                }
            }
            else // if ReadOnly is true, modify existing record
            {
                ///////////////////////////////////////////////////////////////
                /// MODIFY an existing Credential set                        //
                ///////////////////////////////////////////////////////////////

                // Save the index
                int savedSelectedIndex = ddlPolicy.SelectedIndex;
                int savedIndexForGroup = ddlGroup.SelectedIndex;
                int savedIndexForExperiment = ddlExperiment.SelectedIndex;
                policyID = policies[ddlPolicy.SelectedIndex - 1].ussPolicyId;
                try
                {
                    // Modify the Policy
                    dbManager.ModifyUSSPolicy(policyID, Int32.Parse(ddlExperiment.SelectedValue), PolicyParser.GenerateRule(ruleFields, properties), Int32.Parse(ddlGroup.SelectedValue));
                    lblErrorMessage.Visible = true;
                    string mesHead = setMessageHeader();
                    lblErrorMessage.Text = Utilities.FormatConfirmationMessage(mesHead + " has been modified.");

                    // Reload the Experiment dropdown
                    InitializePolicyDropDown();
                    ddlPolicy.SelectedIndex = savedSelectedIndex;
                    ddlGroup.ClearSelection();
                    ddlGroup.SelectedIndex = savedIndexForGroup;
                    ddlExperiment.ClearSelection();
                    ddlExperiment.SelectedIndex = savedIndexForExperiment;

                }
                catch (Exception ex)
                {
                    lblErrorMessage.Visible = true;
                    string mesHead = setMessageHeader();
                    lblErrorMessage.Text = Utilities.FormatErrorMessage(mesHead + " cannot be modified. " + ex.Message);
                    return;
                }
            }
        }

        protected void btnRemove_Click(object sender, System.EventArgs e)
        {
            if (ddlPolicy.SelectedIndex == 0)
            {
                lblErrorMessage.Visible = true;
                lblErrorMessage.Text = Utilities.FormatWarningMessage("Please select a policy from dropdown list to delete");
                return;
            }
            else
            {
                policyID = policies[ddlExperiment.SelectedIndex - 1].ussPolicyId;
                try
                {
                    dbManager.RemoveUSSPolicy(new int[] { policyID });
                    lblErrorMessage.Visible = true;
                    string mesHead = setMessageHeader();
                    lblErrorMessage.Text = Utilities.FormatConfirmationMessage(mesHead + " has been deleted. ");
                    InitializePolicyDropDown();
                    ClearFormFields();
                }
                catch
                {
                    lblErrorMessage.Visible = true;
                    string mesHead = setMessageHeader();
                    lblErrorMessage.Text = Utilities.FormatErrorMessage(mesHead + " cannot be deleted. ");
                }

            }
        }

        private string setMessageHeader()
        {
            UssExperimentInfo exp = dbManager.GetExperimentInfos(new int[] { Int32.Parse(ddlExperiment.SelectedValue) })[0];
            string expStr = exp.labClientName + " " + exp.labClientVersion;
            UssCredentialSet cre = dbManager.GetCredentialSets(new int[] { Int32.Parse(ddlGroup.SelectedValue) })[0];
            string creStr = cre.serviceBrokerName + " " + cre.groupName;
            string mesHead = "The policy for " + creStr + " executing " + expStr;
            return mesHead;
        }


    }
}





