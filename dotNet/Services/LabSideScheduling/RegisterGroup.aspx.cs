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

namespace iLabs.Scheduling.LabSide
{
	/// <summary>
	/// Summary description for Register.
	/// </summary>
	public partial class Register : System.Web.UI.Page
	{
	
		 int credentialSetID;
		 int[] credentialSetIDs;
	     LssCredentialSet[] credentialSets;
        string couponID = null, passkey = null, issuerID = null, sbUrl = null;
        LabSchedulingDB dbManager = new LabSchedulingDB();

		protected void Page_Load(object sender, System.EventArgs e)
		{
			btnRemove.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to remove this experiment Information?')== false) return false;");
			credentialSetIDs=dbManager.ListCredentialSetIDs();
			credentialSets=dbManager.GetCredentialSets(credentialSetIDs);
			
			if(!IsPostBack)
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
                    // Load the Group list box
                    ddlGroup.Items.Add(new ListItem(" ---------- select Group ---------- "));
                    for (int i = 0; i < credentialSets.Length; i++)
                    {
                        string cred = credentialSets[i].groupName + " " + credentialSets[i].serviceBrokerName;
                        ddlGroup.Items.Add(new ListItem(cred, credentialSets[i].credentialSetId.ToString()));
                    }

                    // Set the GUID field to not ReadOnly
                    SetReadOnly(false);
                    //SetDDLUnable(false);
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
		/// Clears the Group dropdown and reloads it from the array of CredentialSet objects
		/// </summary>
		private void InitializeGroupDropDown()
		{
			credentialSetIDs = dbManager.ListCredentialSetIDs();
			credentialSets = dbManager.GetCredentialSets(credentialSetIDs);
			
			ddlGroup.Items.Clear();

			ddlGroup.Items.Add(new ListItem(" ---------- select Group ---------- "));
			for(int i=0; i< credentialSets.Length; i++)
			{
				string cred=credentialSets[i].groupName+" "+credentialSets[i].serviceBrokerName;
				ddlGroup.Items.Add(new ListItem(cred, credentialSets[i].credentialSetId.ToString()));
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
			if(readOnlySwitch)
			{
				txtServiceBrokerID.ReadOnly = true;
				txtServiceBrokerID.BackColor = Color.FromArgb(243,239,229);
			}
			else
			{
				txtServiceBrokerID.ReadOnly = false;
				txtServiceBrokerID.BackColor = Color.White;
			}
		}
		
		/// <summary>
		/// This method clears the form fields.
		/// </summary>
		private void ClearFormFields()
		{
			txtGroupName.Text = "";
			txtServiceBrokerID.Text = "";
			SetReadOnly(false);
			txtServiceBrokerName.Text = "";
		}
		/// <summary>
		/// This method fires when the GROUP dropdown changes.
		/// If the index is greater than zero, the specified Group will be looked up
		/// and its values will be used to populate the text fields on the form.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void ddlGroup_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if(ddlGroup.SelectedIndex == 0) 
				// prepare for a new record
			{
				ClearFormFields();
				SetReadOnly(false);
				
			}
			else
				//retrieve an existing record
			{
				LssCredentialSet cr = new LssCredentialSet();
				cr = credentialSets[ddlGroup.SelectedIndex-1];
				txtGroupName.Text = cr.groupName;
				txtServiceBrokerID.Text = cr.serviceBrokerGuid;
				txtServiceBrokerName.Text = cr.serviceBrokerName;
				
				// Make the serverice broker id field ReadOnly
				SetReadOnly(true);
			
			}
		}

		protected void btnNew_Click(object sender, System.EventArgs e)
		{
			ddlGroup.SelectedIndex = 0;
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
			if(txtGroupName.Text.CompareTo("")==0 )
			{
				lblErrorMessage.Text = Utilities.FormatWarningMessage("Please enter a group name.");
				lblErrorMessage.Visible=true;
				return;
			}

			if(txtServiceBrokerID.Text.CompareTo("")==0 )
			{
				lblErrorMessage.Text = Utilities.FormatWarningMessage("You must enter the service broker ID.");
				lblErrorMessage.Visible=true;
				return;
			}

			if(txtServiceBrokerName.Text.CompareTo("")==0 )
			{
				lblErrorMessage.Text = Utilities.FormatWarningMessage("You must enter the service broker name.");
				lblErrorMessage.Visible=true;
				return;
			}

			
			///////////////////////////////////////////////////////////////
			/// ADD a new Credential Set                                            //
			///////////////////////////////////////////////////////////////
			if(txtServiceBrokerID.ReadOnly == false) // add new record
			{
				
				// see if this Credential Set already exists
				foreach (LssCredentialSet cr in credentialSets)
				{
					if((txtGroupName.Text == cr.groupName )&& (txtServiceBrokerID.Text == cr.serviceBrokerGuid) )
					{
						lblErrorMessage.Visible = true;
						lblErrorMessage.Text = Utilities.FormatWarningMessage("Group " + txtGroupName.Text + " " + txtServiceBrokerName.Text + " exists, choose another one");
						
						return;
					}
				}

				// Add the Credential Set
				try
				{
					credentialSetID = dbManager.AddCredentialSet(txtServiceBrokerID.Text, txtServiceBrokerName.Text, txtGroupName.Text);
				}
				catch (Exception ex)
				{
					lblErrorMessage.Visible = true;
					lblErrorMessage.Text = Utilities.FormatErrorMessage(ex.Message);
					return;
				}

				// If successful...
				if (credentialSetID != -1)
				{
					lblErrorMessage.Visible = true;
					lblErrorMessage.Text =Utilities.FormatConfirmationMessage("Group " + txtGroupName.Text + " " + txtServiceBrokerName.Text + " has been added.");
					// set dropdown to newly created credential set.
					InitializeGroupDropDown();
					ddlGroup.Items.FindByValue(credentialSetID.ToString()).Selected = true;
					SetReadOnly(true);	
				}
				else // cannot create Credential Set
				{
					lblErrorMessage.Visible = true;
					lblErrorMessage.Text = Utilities.FormatErrorMessage("Cannot create Group" +txtGroupName.Text + " " + txtServiceBrokerName.Text + ".");
				    return;
				}
			}
			else // if ReadOnly is true, modify existing record
			{
				///////////////////////////////////////////////////////////////
				/// MODIFY an existing Credential set                        //
				///////////////////////////////////////////////////////////////
				
				// Save the index
				int savedSelectedIndex = ddlGroup.SelectedIndex;
				credentialSetID = credentialSets[ddlGroup.SelectedIndex-1].credentialSetId;
				try
				{
					// Modify the Credential
					dbManager.ModifyCredentialSet(credentialSetID, txtServiceBrokerID.Text, txtServiceBrokerName.Text, txtGroupName.Text);
					
					lblErrorMessage.Visible = true;
					lblErrorMessage.Text = Utilities.FormatConfirmationMessage("Group " + txtGroupName.Text + " " + txtServiceBrokerName.Text + " has been modified.");
					
					// Reload the Group dropdown
					InitializeGroupDropDown();
					ddlGroup.SelectedIndex = savedSelectedIndex;
                 
				}
				catch(Exception ex)
				{
					lblErrorMessage.Visible = true;
					lblErrorMessage.Text = Utilities.FormatErrorMessage("Group " + txtGroupName.Text + " " + txtServiceBrokerName.Text + " cannot be modified."+ex.Message);
					return;
				}
			}
		}

		protected void btnRemove_Click(object sender, System.EventArgs e)
		{
			if(ddlGroup.SelectedIndex == 0)
			{
				lblErrorMessage.Visible = true;
				lblErrorMessage.Text = Utilities.FormatWarningMessage("Please select a group from dropdown list to delete");
				return;
			}
			else
			{
				credentialSetID = credentialSets[ddlGroup.SelectedIndex-1].credentialSetId;
				try
				{
					dbManager.RemoveCredentialSets(new int[]{credentialSetID});
					lblErrorMessage.Visible = true;
					lblErrorMessage.Text = Utilities.FormatConfirmationMessage("Group '" +txtGroupName.Text + " " + txtServiceBrokerName.Text + "' has been deleted");
					InitializeGroupDropDown();
					ClearFormFields();
				}
				catch
				{
					lblErrorMessage.Visible = true;
					lblErrorMessage.Text = Utilities.FormatErrorMessage("Group '" +txtGroupName.Text + " " + txtServiceBrokerName.Text + "' cannot be deleted");
				}

			}
		}



	}
}
