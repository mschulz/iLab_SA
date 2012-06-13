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
using iLabs.DataTypes.ProcessAgentTypes;
using iLabs.DataTypes.TicketingTypes;
using iLabs.DataTypes.SchedulingTypes;
using iLabs.Proxies.PAgent;
using System.Xml;
using System.IO;
using iLabs.Ticketing;
using System.Configuration;
using iLabs.UtilLib;

namespace iLabs.Scheduling.UserSide
{
    /// <summary>
    /// Summary description for RegisterLSS.
    /// </summary>
    public partial class Administer : System.Web.UI.Page
    {
         int lssInfoId;
         int[] lssInfoIds;
         LSSInfo[] lssInfos;
        UserSchedulingDB dbManager = new UserSchedulingDB();
        string couponID = null, passkey = null, issuerID = null, sbUrl = null;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            btnRemove.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to remove this experiment Information?')== false) return false;");
            lssInfoIds = dbManager.ListLSSInfoIDs();
            lssInfos = dbManager.GetLSSInfos(lssInfoIds);

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
                        Ticket ticket = dbTicketing.RetrieveAndVerify(coupon, TicketTypes.ADMINISTER_USS);

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
                    // Load the LSS list box
                    ddlLSS.Items.Add(new ListItem(" ---------- select Lab side scheduling server ---------- "));
                    for (int i = 0; i < lssInfos.Length; i++)
                    {
                        ddlLSS.Items.Add(new ListItem(lssInfos[i].lssName, lssInfos[i].lssInfoId.ToString()));
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
        /// Clears the LSS dropdown and reloads it from the array of LSSInfo objects
        /// </summary>
        private void InitializeDropDown()
        {
            lssInfoIds = dbManager.ListLSSInfoIDs();
            lssInfos = dbManager.GetLSSInfos(lssInfoIds);

            ddlLSS.Items.Clear();

            ddlLSS.Items.Add(new ListItem(" ---------- select user side scheduling server ---------- "));
            for (int i = 0; i < lssInfos.Length; i++)
            {
                ddlLSS.Items.Add(new ListItem(lssInfos[i].lssName, lssInfos[i].lssGuid));
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
                txtLSSID.ReadOnly = true;
                txtLSSID.BackColor = Color.FromArgb(243, 239, 229);
            }
            else
            {
                txtLSSID.ReadOnly = false;
                txtLSSID.BackColor = Color.White;
            }
        }

        /// <summary>
        /// This method clears the form fields.
        /// </summary>
        private void ClearFormFields()
        {
            txtLSSID.Text = "";
            SetReadOnly(false);
            txtLSSName.Text = "";
            txtLSSURL.Text = "";
        }

        /// <summary>
        /// This method fires when the LSS dropdown changes.
        /// If the index is greater than zero, the specified LSS will be looked up
        /// and its values will be used to populate the text fields on the form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlLSS_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (ddlLSS.SelectedIndex == 0)
            // prepare for a new record
            {
                ClearFormFields();
                SetReadOnly(false);
            }
            else
            //retrieve an existing record
            {
                LSSInfo ui = new LSSInfo();
                ui = lssInfos[ddlLSS.SelectedIndex - 1];
                txtLSSID.Text = ui.lssGuid;
                txtLSSName.Text = ui.lssName;
                txtLSSURL.Text = ui.lssUrl;
                // Make the LSSID field ReadOnly
                SetReadOnly(true);
            }
        }

        protected void btnNew_Click(object sender, System.EventArgs e)
        {
            ddlLSS.SelectedIndex = 0;
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
            if (txtLSSID.Text.CompareTo("") == 0)
            {
                lblErrorMessage.Text = Utilities.FormatWarningMessage("Enter a user side scheduling server GUID.");
                lblErrorMessage.Visible = true;
                return;
            }

            if (txtLSSName.Text.CompareTo("") == 0)
            {
                lblErrorMessage.Text = Utilities.FormatWarningMessage("You must enter the user side scheduling server name.");
                lblErrorMessage.Visible = true;
                return;
            }

            if (txtLSSURL.Text.CompareTo("") == 0)
            {
                lblErrorMessage.Text = Utilities.FormatWarningMessage("You must enter the user side scheduling URL.");
                lblErrorMessage.Visible = true;
                return;
            }

            ///////////////////////////////////////////////////////////////
            /// ADD a new LSS                                            //
            ///////////////////////////////////////////////////////////////
            if (txtLSSID.ReadOnly == false) // add new record
            {

                // see if this User Side Scheduling Server already exists
                foreach (LSSInfo ui in lssInfos)
                {
                    if (this.txtLSSID.Text == ui.lssGuid)
                    {
                        lblErrorMessage.Visible = true;
                        lblErrorMessage.Text = Utilities.FormatWarningMessage("User Side Scheduling Server GUID " + txtLSSID.Text + " exists, choose another one");

                        return;
                    }
                }

                // Add the Lab Side Scheduling Server
                try
                {
                    // ToDo: this is no longer used but needs tosupport the revokeReservation ticket
                    lssInfoId = dbManager.AddLSSInfo(txtLSSID.Text, txtLSSName.Text, txtLSSURL.Text,null);
                }
                catch (Exception ex)
                {
                    lblErrorMessage.Visible = true;
                    lblErrorMessage.Text = Utilities.FormatErrorMessage(ex.Message);
                    return;
                }

                // If successful...
                if (lssInfoId != -1)
                {
                    lblErrorMessage.Visible = true;
                    lblErrorMessage.Text = Utilities.FormatConfirmationMessage("User Side Scheduling Server " + txtLSSName.Text + " has been added.");

                }
                else // cannot create LSS
                {
                    lblErrorMessage.Visible = true;
                    lblErrorMessage.Text = Utilities.FormatErrorMessage("Cannot create User Side Scheduling Server" + txtLSSName.Text + ".");
                }

                // set dropdown to newly created User Side Scheduling Server.
                InitializeDropDown();
                ddlLSS.Items.FindByText(txtLSSName.Text).Selected = true;

                SetReadOnly(true);

            }
            else // if ReadOnly is true, modify existing record
            {
                ///////////////////////////////////////////////////////////////
                /// MODIFY an existing User Side Scheduling Server                            //
                ///////////////////////////////////////////////////////////////

                // Save the index
                int savedSelectedIndex = ddlLSS.SelectedIndex;

                lssInfoId = lssInfos[ddlLSS.SelectedIndex - 1].lssInfoId;
                try
                {
                    // Modify the User Side Scheduling Server
                    dbManager.ModifyLSSInfo(lssInfoId, txtLSSID.Text, txtLSSName.Text, txtLSSURL.Text,null);

                    lblErrorMessage.Visible = true;
                    lblErrorMessage.Text = Utilities.FormatConfirmationMessage("User Side Scheduling Server " + txtLSSID.Text + " has been modified.");

                    // Reload the User Side Scheduling Server dropdown
                    InitializeDropDown();
                    ddlLSS.SelectedIndex = savedSelectedIndex;

                }
                catch
                {
                    lblErrorMessage.Visible = true;
                    lblErrorMessage.Text = Utilities.FormatErrorMessage("User Side Scheduling Server " + txtLSSName.Text + " cannot be modified.");
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
            if (ddlLSS.SelectedIndex == 0)
            {
                lblErrorMessage.Visible = true;
                lblErrorMessage.Text = Utilities.FormatWarningMessage("Please select a user side scheduling server from dropdown list to delete");
                return;
            }
            else
            {
                lssInfoId = lssInfos[ddlLSS.SelectedIndex - 1].lssInfoId;
                try
                {
                    dbManager.RemoveLSSInfo(new int[] { lssInfoId });
                    lblErrorMessage.Visible = true;
                    lblErrorMessage.Text = Utilities.FormatConfirmationMessage("User side Scheduling Server '" + txtLSSName.Text + "' has been deleted");
                    InitializeDropDown();
                    ClearFormFields();
                }
                catch
                {
                    lblErrorMessage.Visible = true;
                    lblErrorMessage.Text = Utilities.FormatErrorMessage("User Side Scheduling Server " + txtLSSName.Text + "' cannot be deleted");
                }

            }

        }

    }
}
