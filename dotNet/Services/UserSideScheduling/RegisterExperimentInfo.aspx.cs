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
using iLabs.DataTypes.SchedulingTypes;
using iLabs.Ticketing;
using iLabs.DataTypes.TicketingTypes;
using iLabs.DataTypes.SoapHeaderTypes;
using iLabs.UtilLib;

namespace iLabs.Scheduling.UserSide
{
	/// <summary>
	/// Summary description for RegisterExperimentInfo.
	/// </summary>
	public partial class RegisterExperimentInfo : System.Web.UI.Page
	{
	
		 int experimentInfoId;
		 int[] experimentInfoIds;
		 UssExperimentInfo[] experimentInfos;
		 int[] lssInfoIds;
		 LSSInfo[] lssInfos;
        UserSchedulingDB dbManager = new UserSchedulingDB();

        string couponID = null, passkey = null, issuerID = null, sbUrl = null; 

		protected void Page_Load(object sender, System.EventArgs e)
		{
			btnRemove.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to remove this experiment Information?')== false) return false;");
			experimentInfoIds=dbManager.ListExperimentInfoIDs();
			experimentInfos=dbManager.GetExperimentInfos(experimentInfoIds);
			lssInfoIds=dbManager.ListLSSInfoIDs();
			lssInfos=dbManager.GetLSSInfos(lssInfoIds);

			if(!IsPostBack)
			{
                if (Session["couponID"] == null)
                    couponID = Request.QueryString["coupon_id"];
                else
                    couponID = Session["couponID"].ToString();

                if (Session["passkey"] == null)
                    passkey = Request.QueryString["passkey"];
                else
                    passkey = Session["passkey"].ToString();

                if (Session["issuerID"] == null)
                    issuerID = Request.QueryString["issuer_guid"];
                else
                    issuerID = Session["issuerID"].ToString();

                if (Session["sbUrl"] == null)
                    sbUrl = Request.QueryString["sb_url"];
                else
                    sbUrl = Session["sbUrl"].ToString();

                bool unauthorized = false;

                if (couponID != null && passkey != null && issuerID != null)
                {
                    try
                    {
                        Coupon coupon = new Coupon(issuerID, long.Parse(couponID), passkey);
                        Ticket ticket = dbManager.RetrieveAndVerify(coupon, TicketTypes.ADMINISTER_USS);

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
                    // Load the Experiment list box
                    ddlExperiment.Items.Add(new ListItem(" ---------- select Experiment ---------- "));
                    for (int i = 0; i < experimentInfos.Length; i++)
                    {
                        string exper = experimentInfos[i].labClientName + " : " + experimentInfos[i].labClientVersion;
                        ddlExperiment.Items.Add(new ListItem(exper, experimentInfos[i].experimentInfoId.ToString()));
                    }
                    // Load the LSSlist box
                    ddlLSS.Items.Add(new ListItem(" ---------- select Lab side scheduling server ---------- "));
                    for (int i = 0; i < lssInfos.Length; i++)
                    {
                        ddlLSS.Items.Add(new ListItem(lssInfos[i].lssName, lssInfos[i].lssGuid));
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
		/// Clears the Experiment dropdown and reloads it from the array of ExperimentInfo objects
		/// </summary>
		private void InitializeExperimentDropDown()
		{
			experimentInfoIds = dbManager.ListExperimentInfoIDs();
			experimentInfos = dbManager.GetExperimentInfos(experimentInfoIds);
			
			ddlExperiment.Items.Clear();

			ddlExperiment.Items.Add(new ListItem(" ---------- select Experiment ---------- "));
			for(int i=0; i< experimentInfos.Length; i++)
			{
				string exper=experimentInfos[i].labClientName+" : "+experimentInfos[i].labClientVersion;
				ddlExperiment.Items.Add(new ListItem(exper, experimentInfos[i].experimentInfoId.ToString()));
			}
		}
		/// <summary>
		/// The labClientName + labClientVersion cannot be edited in an existing record,
		/// but they must be specified for a new record.
		/// This method resets the ReadOnly state and background colors of these fields.
		/// </summary>
		/// <param name="readOnlySwitch">true if ReadOnly, false if not</param>
		private void SetReadOnly(bool readOnlySwitch)
		{
			//243,239,229 - light green
			//174,155,138 - brown
			if(readOnlySwitch)
			{
				txtLabClientName.ReadOnly = true;
				txtLabClientName.BackColor = Color.FromArgb(243,239,229);
				txtLabClinetVersion.ReadOnly = true;
				txtLabClinetVersion.BackColor = Color.FromArgb(243,239,229);
                txtClientGuid.ReadOnly = true;
                txtClientGuid.BackColor = Color.FromArgb(243, 239, 229);
			}
			else
			{
				txtLabClientName.ReadOnly = false;
				txtLabClinetVersion.ReadOnly = false;
				txtLabClientName.BackColor = Color.White;
				txtLabClinetVersion.BackColor = Color.White;
                txtClientGuid.ReadOnly = false;
                txtClientGuid.BackColor = Color.White;
			}
		}
		
		/// <summary>
		/// This method clears the form fields.
		/// </summary>
		private void ClearFormFields()
		{
			txtLabClientName.Text = "";
			txtLabClinetVersion.Text = "";
            txtClientGuid.Text = "";
			txtLabServerID.Text = "";
			txtLabServerName.Text = "";
			txtProviderName.Text = "";
			SetReadOnly(false);
			ddlLSS.ClearSelection();
			ddlLSS.Items[0].Selected=true;
		  
		}
		/// <summary>
		/// This method fires when the Experiment dropdown changes.
		/// If the index is greater than zero, the specified Experiment will be looked up
		/// and its values will be used to populate the text fields on the form.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void ddlExperiment_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if(ddlExperiment.SelectedIndex == 0) 
				// prepare for a new record
			{
				ClearFormFields();
				SetReadOnly(false);
				
			}
			else
				//retrieve an existing record
			{
				UssExperimentInfo expInfo = new UssExperimentInfo();
				expInfo = experimentInfos[ddlExperiment.SelectedIndex-1];
				txtLabClientName.Text = expInfo.labClientName;
				txtLabClinetVersion.Text = expInfo.labClientVersion;
                txtClientGuid.Text = expInfo.labClientGuid;
                txtClientGuid.ReadOnly = true;
                txtClientGuid.Enabled = false;
				txtLabServerID.Text = expInfo.labServerGuid;
				txtLabServerName.Text = expInfo.labServerName;
				txtProviderName.Text = expInfo.providerName;
				ddlLSS.ClearSelection();
                ddlLSS.Items.FindByValue(expInfo.lssGuid).Selected = true;
				// Make the serverice broker id field ReadOnly
				SetReadOnly(true);
			
			}
		}

		protected void btnNew_Click(object sender, System.EventArgs e)
		{
			ddlExperiment.SelectedIndex = 0;
			ClearFormFields();
			SetReadOnly(false);
		}
		/// <summary>
		/// The Save Button method. If the labclientversion and labclientname field is not set to ReadOnly, this method
		/// will assume that a new record is being created. Otherwise, it will assume that
		/// an existing record is being edited.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void btnSaveChanges_Click(object sender, System.EventArgs e)
		{
			//Error checking for empty fields
			if(txtLabClientName.Text.CompareTo("")==0 )
			{
				lblErrorMessage.Text = Utilities.FormatWarningMessage("Please enter a lab client name.");
				lblErrorMessage.Visible=true;
				return;
			}

			if(txtLabClinetVersion.Text.CompareTo("")==0 )
			{
				lblErrorMessage.Text = Utilities.FormatWarningMessage("You must enter the lab client version.");
				lblErrorMessage.Visible=true;
				return;
			}

			if(txtLabServerID.Text.CompareTo("")==0 )
			{
				lblErrorMessage.Text = Utilities.FormatWarningMessage("You must enter the GUID of lab server.");
				lblErrorMessage.Visible=true;
				return;
			}

			if(txtLabServerName.Text.CompareTo("")==0 )
			{
				lblErrorMessage.Text = Utilities.FormatWarningMessage("You must enter the lab server name.");
				lblErrorMessage.Visible=true;
				return;
			}

			if(txtProviderName.Text.CompareTo("")==0 )
			{
				lblErrorMessage.Text = Utilities.FormatWarningMessage("You must enter the provider name.");
				lblErrorMessage.Visible=true;
				return;
			}


			if(ddlLSS.SelectedIndex==0 )
			{
				lblErrorMessage.Text = Utilities.FormatWarningMessage("You must select a lab side scheduling server.");
				lblErrorMessage.Visible=true;
				return;
			}
			
			///////////////////////////////////////////////////////////////
			/// ADD a new Credential Set                                            //
			///////////////////////////////////////////////////////////////
			if(txtLabClientName.ReadOnly == false) // add new record
			{
				
				// see if this lab client already exists
				foreach (UssExperimentInfo expInfo in experimentInfos)
				{
					if((txtLabClientName.Text == expInfo.labClientName )&& (txtLabClinetVersion.Text == expInfo.labClientVersion))
					{
						lblErrorMessage.Visible = true;
						lblErrorMessage.Text = Utilities.FormatWarningMessage("Experiment " + txtLabClientName.Text + " : " + txtLabClinetVersion.Text+ " exists, choose another one");
						
						return;
					}
				}

				// Add the Credential Set
				int savedIndexforLSS=ddlLSS.SelectedIndex;
				try
				{
                    experimentInfoId = dbManager.AddExperimentInfo(txtLabServerID.Text, txtLabServerName.Text, txtClientGuid.Text, txtLabClientName.Text, txtLabClinetVersion.Text, txtProviderName.Text, ddlLSS.SelectedValue);
				}
				catch (Exception ex)
				{
					lblErrorMessage.Visible = true;
					lblErrorMessage.Text = Utilities.FormatErrorMessage(ex.Message);
					return;
				}

				// If successful...
				if (experimentInfoId != -1)
				{
					lblErrorMessage.Visible = true;
					lblErrorMessage.Text =Utilities.FormatConfirmationMessage("Experiment " + txtLabClientName.Text + " : " + txtLabClinetVersion.Text + " has been added.");
					// set dropdown to newly created credential set.
					InitializeExperimentDropDown();
					ddlExperiment.Items.FindByValue(experimentInfoId.ToString()).Selected = true;
					ddlLSS.ClearSelection();
					ddlLSS.Items[savedIndexforLSS].Selected=true;
		  
					SetReadOnly(true);	
				}
				else // cannot create Credential Set
				{
					lblErrorMessage.Visible = true;
					lblErrorMessage.Text = Utilities.FormatErrorMessage("Cannot create Experiment" +txtLabClientName.Text + " " + txtLabClinetVersion.Text + ".");
					return;
				}
			}
			else // if ReadOnly is true, modify existing record
			{
				///////////////////////////////////////////////////////////////
				/// MODIFY an existing Credential set                        //
				///////////////////////////////////////////////////////////////
				
				// Save the index
				int savedSelectedIndex = ddlExperiment.SelectedIndex;
				int savedIndexForLSS=ddlLSS.SelectedIndex;
				experimentInfoId = experimentInfos[ddlExperiment.SelectedIndex-1].experimentInfoId;
				try
				{
					// Modify the Experiment
                    dbManager.ModifyExperimentInfo(experimentInfoId, txtLabServerID.Text, txtClientGuid.Text, txtLabServerName.Text, txtLabClientName.Text, txtLabClinetVersion.Text, txtProviderName.Text, ddlLSS.SelectedValue);
					lblErrorMessage.Visible = true;
					lblErrorMessage.Text = Utilities.FormatConfirmationMessage("Experiment " + txtLabClientName.Text + " : " + txtLabClinetVersion.Text + " has been modified.");
					
					// Reload the Experiment dropdown
					InitializeExperimentDropDown();
					ddlExperiment.SelectedIndex = savedSelectedIndex;
					ddlLSS.ClearSelection();
					ddlLSS.SelectedIndex=savedIndexForLSS;
                 
				}
				catch(Exception ex)
				{
					lblErrorMessage.Visible = true;
					lblErrorMessage.Text = Utilities.FormatErrorMessage("Experiment " + txtLabClientName.Text + " : " + txtLabClinetVersion.Text + " " + " cannot be modified."+ex.Message);
					return;
				}
			}
		}

		protected void btnRemove_Click(object sender, System.EventArgs e)
		{
			if(ddlExperiment.SelectedIndex == 0)
			{
				lblErrorMessage.Visible = true;
				lblErrorMessage.Text = Utilities.FormatWarningMessage("Please select a group from dropdown list to delete");
				return;
			}
			else
			{
				experimentInfoId = experimentInfos[ddlExperiment.SelectedIndex-1].experimentInfoId;
				try
				{
					dbManager.RemoveExperimentInfo(new int[]{experimentInfoId});
					lblErrorMessage.Visible = true;
					lblErrorMessage.Text = Utilities.FormatConfirmationMessage("Experiment '" +txtLabClientName.Text + " : " + txtLabClinetVersion.Text + "' has been deleted");
					InitializeExperimentDropDown();
					ClearFormFields();
				}
				catch
				{
					lblErrorMessage.Visible = true;
					lblErrorMessage.Text = Utilities.FormatErrorMessage("Experiment '" +txtLabClientName.Text + " : " + txtLabClinetVersion.Text + "' cannot be deleted");
				}

			}
		}



	}
}
