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
using iLabs.DataTypes.TicketingTypes;
using System.Xml;
using System.IO;

using iLabs.Core;
using iLabs.Ticketing;
using System.Configuration;
using iLabs.DataTypes.SchedulingTypes;
using iLabs.UtilLib;


namespace iLabs.Scheduling.LabSide
{
    /// <summary>
    /// Summary description for RegisterUSS.
    /// </summary>
    public partial class Administer : System.Web.UI.Page
    {
         int ussInfoID;
         int[] ussInfoIDs;
         USSInfo[] ussInfos;
        LabSchedulingDB dbManager = new LabSchedulingDB();
        string couponID = null, passkey = null, issuerID = null, sbUrl = null;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            btnRemove.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to remove this experiment Information?')== false) return false;");
            ussInfoIDs = dbManager.ListUSSInfoIDs();
            ussInfos = dbManager.GetUSSInfos(ussInfoIDs);

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
                        Ticket ticket = dbTicketing.RetrieveAndVerify(coupon, TicketTypes.ADMINISTER_LSS);

                        if (ticket.IsExpired() || ticket.isCancelled)
                        {
                            unauthorized = true;
                            Response.Redirect("Unauthorized.aspx", false);
                        }

                        Session["couponID"] = couponID;
                        Session["passkey"] = passkey;
                        Session["issuerID"] = issuerID;
                        Session["sbUrl"] = sbUrl;

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
                    // Load the USS list box
                    ddlUSS.Items.Add(new ListItem(" ---------- select User side scheduling server ---------- "));
                    for (int i = 0; i < ussInfos.Length; i++)
                    {
                        ddlUSS.Items.Add(new ListItem(ussInfos[i].ussName, ussInfos[i].ussInfoId.ToString()));
                    }

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
        /// Clears the USS dropdown and reloads it from the array of USSInfo objects
        /// </summary>
        private void InitializeDropDown()
        {
            ussInfoIDs = dbManager.ListUSSInfoIDs();
            ussInfos = dbManager.GetUSSInfos(ussInfoIDs);

            ddlUSS.Items.Clear();

            ddlUSS.Items.Add(new ListItem(" ---------- select user side scheduling server ---------- "));
            for (int i = 0; i < ussInfos.Length; i++)
            {
                ddlUSS.Items.Add(new ListItem(ussInfos[i].ussName, ussInfos[i].ussGuid.ToString()));
            }
        }

        /// <summary>
        /// The GUID cannot be edited in an existing record,
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
                txtUSSID.ReadOnly = true;
                txtUSSID.BackColor = Color.FromArgb(243, 239, 229);
            }
            else
            {
                txtUSSID.ReadOnly = false;
                txtUSSID.BackColor = Color.White;
            }
        }

        /// <summary>
        /// This method clears the form fields.
        /// </summary>
        private void ClearFormFields()
        {
            txtUSSID.Text = "";
            SetReadOnly(false);
            txtUSSName.Text = "";
            txtUSSURL.Text = "";
        }

        /// <summary>
        /// This method fires when the USS dropdown changes.
        /// If the index is greater than zero, the specified USS will be looked up
        /// and its values will be used to populate the text fields on the form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlUSS_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (ddlUSS.SelectedIndex == 0)
            // prepare for a new record
            {
                ClearFormFields();
                SetReadOnly(false);
            }
            else
            //retrieve an existing record
            {
                USSInfo ui = new USSInfo();
                ui = ussInfos[ddlUSS.SelectedIndex - 1];
                txtUSSID.Text = ui.ussGuid;
                txtUSSName.Text = ui.ussName;
                txtUSSURL.Text = ui.ussUrl;
                // Make the USSID field ReadOnly
                SetReadOnly(true);
            }
        }

        protected void btnNew_Click(object sender, System.EventArgs e)
        {
            ddlUSS.SelectedIndex = 0;
            ClearFormFields();
            SetReadOnly(false);
        }
        /// <summary>
        /// The Save Button method. If the GUID field is not set to ReadOnly, this method
        /// will assume that a new record is being created. Otherwise, it will assume that
        /// an existing record is being edited.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSaveChanges_Click(object sender, System.EventArgs e)
        {
            //Error checking for empty fields
            if (txtUSSID.Text.CompareTo("") == 0)
            {
                lblErrorMessage.Text = Utilities.FormatWarningMessage("Enter a user side scheduling server GUID.");
                lblErrorMessage.Visible = true;
                return;
            }

            if (txtUSSName.Text.CompareTo("") == 0)
            {
                lblErrorMessage.Text = Utilities.FormatWarningMessage("You must enter the user side scheduling server name.");
                lblErrorMessage.Visible = true;
                return;
            }

            if (txtUSSURL.Text.CompareTo("") == 0)
            {
                lblErrorMessage.Text = Utilities.FormatWarningMessage("You must enter the user side scheduling URL.");
                lblErrorMessage.Visible = true;
                return;
            }

            ///////////////////////////////////////////////////////////////
            /// ADD a new USS                                            //
            ///////////////////////////////////////////////////////////////
            if (txtUSSID.ReadOnly == false) // add new record
            {

                // see if this User Side Scheduling Server already exists
                foreach (USSInfo ui in ussInfos)
                {
                    if (this.txtUSSID.Text == ui.ussGuid)
                    {
                        lblErrorMessage.Visible = true;
                        lblErrorMessage.Text = Utilities.FormatWarningMessage("User Side Scheduling Server GUID " + txtUSSID.Text + " exists, choose another one");

                        return;
                    }
                }

                // Add the User Side Scheduling Server
                try
                {
                    ussInfoID = -1; // dbManager.AddUSSInfo(txtUSSID.Text, txtUSSName.Text, txtUSSURL.Text);
                }
                catch (Exception ex)
                {
                    lblErrorMessage.Visible = true;
                    lblErrorMessage.Text = Utilities.FormatErrorMessage(ex.Message);
                    return;
                }

                // If successful...
                if (ussInfoID != -1)
                {
                    lblErrorMessage.Visible = true;
                    lblErrorMessage.Text = Utilities.FormatConfirmationMessage("User Side Scheduling Server " + txtUSSName.Text + " has been added.");

                }
                else // cannot create USS
                {
                    lblErrorMessage.Visible = true;
                    lblErrorMessage.Text = Utilities.FormatErrorMessage("Cannot create User Side Scheduling Server" + txtUSSName.Text + ".");
                }

                // set dropdown to newly created User Side Scheduling Server.
                InitializeDropDown();
                ddlUSS.Items.FindByText(txtUSSName.Text).Selected = true;

                SetReadOnly(true);

            }
            else // if ReadOnly is true, modify existing record
            {
                ///////////////////////////////////////////////////////////////
                /// MODIFY an existing User Side Scheduling Server                            //
                ///////////////////////////////////////////////////////////////

                // Save the index
                int savedSelectedIndex = ddlUSS.SelectedIndex;

                ussInfoID = ussInfos[ddlUSS.SelectedIndex - 1].ussInfoId;
                try
                {
                    // Modify the User Side Scheduling Server
                    //dbManager.ModifyUSSInfo(ussInfoID, txtUSSID.Text, txtUSSName.Text, txtUSSURL.Text);

                    lblErrorMessage.Visible = true;
                    lblErrorMessage.Text = Utilities.FormatConfirmationMessage("User Side Scheduling Server " + txtUSSID.Text + " has been modified.");

                    // Reload the User Side Scheduling Server dropdown
                    InitializeDropDown();
                    ddlUSS.SelectedIndex = savedSelectedIndex;

                }
                catch
                {
                    lblErrorMessage.Visible = true;
                    lblErrorMessage.Text = Utilities.FormatWarningMessage("User Side Scheduling Server " + txtUSSName.Text + " cannot be modified.");
                    return;
                }
            }
        }
        /// <summary>
        /// click the button to delete the user side scheduling server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRemove_Click(object sender, System.EventArgs e)
        {
            if (ddlUSS.SelectedIndex == 0)
            {
                lblErrorMessage.Visible = true;
                lblErrorMessage.Text = Utilities.FormatWarningMessage("Please select a user side scheduling server from dropdown list to delete");
                return;
            }
            else
            {
                ussInfoID = ussInfos[ddlUSS.SelectedIndex - 1].ussInfoId;
                try
                {
                    dbManager.RemoveUSSInfo(new int[] { ussInfoID });
                    lblErrorMessage.Visible = true;
                    lblErrorMessage.Text = Utilities.FormatConfirmationMessage("User side Scheduling Server '" + txtUSSName.Text + "' has been deleted");
                    InitializeDropDown();
                    ClearFormFields();
                }
                catch
                {
                    lblErrorMessage.Visible = true;
                    lblErrorMessage.Text = Utilities.FormatWarningMessage("User Side Scheduling Server " + txtUSSName.Text + "' cannot be deleted");
                }

            }

        }

    }
}
