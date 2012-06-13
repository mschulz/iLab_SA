using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using iLabs.Core;
using iLabs.DataTypes.SchedulingTypes;
using iLabs.Ticketing;
using iLabs.DataTypes.TicketingTypes;
using System.Xml;
using iLabs.UtilLib;
using System.Globalization;

namespace iLabs.Scheduling.LabSide
{
	/// <summary>
	/// Summary description for TimeBlockManagement.
	/// </summary>
	public partial class TimeBlockManagement : System.Web.UI.Page
	{
		//int[] groupIDs=dbManager.ListCredentialSetIDs();
		 int[] credentialSetIDs;
		 LssCredentialSet[] credentialSets;
	   
        string couponID = null, passkey = null, issuerID = null, sbUrl = null;
        string labServerGuid = null;
        string labServerName = null;
        int userTZ = 0;
        int localTzOffset = 0;
        CultureInfo culture;
        LabSchedulingDB dbManager = new LabSchedulingDB();
        
	
		protected void Page_Load(object sender, System.EventArgs e)
		{

            culture = DateUtil.ParseCulture(Request.Headers["Accept-Language"]);
            localTzOffset = DateUtil.LocalTzOffset;
			credentialSetIDs = dbManager.ListCredentialSetIDs();
			credentialSets=dbManager.GetCredentialSets(credentialSetIDs);
			btnRemove.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to remove this recurring time block?')== false) return false;");
           
            btnNew.Attributes.Add("onclick","javascript:window.open('NewTimeBlockPopUp.aspx','NewTimeBlockPopUp','width=910,height=600,left=270,top=180,modal=yes,resizable=yes').focus()");
            hiddenPopupOnNewTB.Attributes.Add("onpropertychange", Page.GetPostBackEventReference(btnSaveChanges));
           
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
                        Ticket ticket = dbTicketing.RetrieveAndVerify(coupon, TicketTypes.MANAGE_LAB);

                        if ( ticket == null || ticket.IsExpired() || ticket.isCancelled)
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
                        userTZ = Convert.ToInt32(payload.GetElementsByTagName("userTZ")[0].InnerText);
                        Session["userTZ"] = userTZ;
                        labServerGuid = payload.GetElementsByTagName("labServerGuid")[0].InnerText;
                        Session["labServerGuid"] = labServerGuid;
                        labServerName = payload.GetElementsByTagName("labServerName")[0].InnerText;
                        Session["labServerName"] = labServerName;

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


                    BuildRecurrenceListBox();
                }

            }
            else
            {
                userTZ = (int) Session["userTZ"];
                labServerGuid = (string) Session["labServerGuid"];
                labServerName = (string) Session["labServerName"];

            }
            StringBuilder buf = new StringBuilder("Create, modify or delete recurring time blocks.<br/><br/>Times shown are Local LSS time UTC&nbsp;&nbsp;&nbsp;");
            if (localTzOffset > 0)
                buf.Append("+");
            buf.Append(localTzOffset / 60.0);
            lblDescription.Text = buf.ToString();
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
		 * Builds the Select Recurrence List box.
		 * By default, the box gets filled with all the Recurrences of a labserver (resource) in the database
		 */
		private void BuildRecurrenceListBox()
		{
            lbxSelectTimeBlock.Items.Clear();
            lbxPermittedExperiments.Items.Clear();
            lbxSelectExperiment.Items.Clear();
            LSResource[] resources = dbManager.GetLSResources(Session["labServerGuid"].ToString());
            int[] recurrenceIDs = dbManager.ListRecurrenceIDsByResourceID(DateTime.UtcNow, DateTime.MaxValue, resources[0].resourceID);
            Recurrence[] recurs = dbManager.GetRecurrences(recurrenceIDs);
			//if related recurrence have been found
			if (recurs.Length > 0)
			{
				BuildRecurrenceListBox(recurs);
			}
			else //no related recurrence exist
			{

                string msg = "no recurring time blocks assigned to the Lab server " + Session["labServerName"].ToString() + ".";
				lblErrorMessage.Text = Utilities.FormatWarningMessage(msg);
				lblErrorMessage.Visible=true;
			}
			
		}
		
		/* 
		 * Builds the Select Recurrence List using a specified array of recurrence. 
		 * This is used to return the results of a search
		 */
		private void BuildRecurrenceListBox(Recurrence[] recurs)
		{
            StringBuilder buf = null;
			lbxSelectTimeBlock.Items .Clear ();
			btnEdit.Visible = false;
			foreach (Recurrence recur in recurs) 
			{
                buf = new StringBuilder();
                //buf.Append("<pre>");
				ListItem recurItem = new ListItem();
				//string ussName = dbManager.GetUSSInfos(new int[]{dbManager.ListUSSInfoID(cs.ussGuid)})[0].ussName;
               // string labServerName = dbManager.RetrieveLabServerName(recur.labServerGuid);
                buf.Append(String.Format("{0,-30}",Session["labServerName"].ToString() + ": "));
               
                buf.Append(String.Format("{0,-10}",DateUtil.ToUserTime(recur.startDate, culture, localTzOffset)));
                buf.Append(" -- " );
                buf.Append(String.Format("{0,10}", DateUtil.ToUserTime(recur.startDate.AddDays(recur.numDays), culture, localTzOffset)));
                
                buf.Append(String.Format(" {0,-15}",recur.recurrenceType + ":"));
                TimeSpan tzOffset = TimeSpan.FromMinutes(localTzOffset);
                DateTime localStart = recur.startDate.AddMinutes(localTzOffset);
                buf.Append(localStart.Add(recur.startOffset).TimeOfDay + " -- " + localStart.Add(recur.endOffset).TimeOfDay);
                //buf.Append(recur.startOffset + " -- " + recur.endOffset);
                if(recur.recurrenceType == Recurrence.RecurrenceType.Weekly){
                    buf.Append(" Days: ");
                   buf.Append(DateUtil.ListDays(recur.dayMask,culture));
                }
                //buf.Append("</pre>");
				recurItem.Text = buf.ToString();
                recurItem.Value = recur.recurrenceId.ToString();
				lbxSelectTimeBlock.Items .Add(recurItem);
			}
		}

        private void DisplayPermittedExperiments(int recurrenceID)
        {
            lbxSelectExperiment.Items.Clear();
            lbxPermittedExperiments.Items.Clear();
            int[] permittedExperimentInfoIDs = dbManager.ListExperimentInfoIDsByRecurrence(recurrenceID);
            LssExperimentInfo[] permittedExperimentInfos = dbManager.GetExperimentInfos(permittedExperimentInfoIDs);
            // the experiments available in the lab server which the recurrence belongs to
            //String labServerID = dbManager.GetRecurrence(new int[]{recurrenceID})[0].labServerGuid;
            int[] availableExperimentInfoIDs = dbManager.ListExperimentInfoIDsByLabServer(Session["labServerGuid"].ToString());
            ArrayList unPermittedExperimentIDs = new ArrayList();
            foreach (int aeID in availableExperimentInfoIDs)
            {
                bool isIn = false;
                for (int i = 0; i < permittedExperimentInfoIDs.Length; i++)
                {
                    if (permittedExperimentInfoIDs[i] == aeID)
                    {
                        isIn = true;
                        break;
                    }
                }
                if (!isIn)
                {
                    unPermittedExperimentIDs.Add(aeID);
                }
            }
            int[] unPEIDs = Utilities.ArrayListToIntArray(unPermittedExperimentIDs);
            LssExperimentInfo[] unPermittedExperimentInfos = dbManager.GetExperimentInfos(unPEIDs);
            if (permittedExperimentInfos != null && permittedExperimentInfos.Length > 0)
            {
                foreach (LssExperimentInfo pep in permittedExperimentInfos)
                {
                    ListItem permittedExperimentItem = new ListItem();
                    permittedExperimentItem.Text = pep.labClientName + ", " + pep.labClientVersion;
                    permittedExperimentItem.Value = pep.experimentInfoId.ToString();
                    lbxPermittedExperiments.Items.Add(permittedExperimentItem);
                }
            }
            if (unPermittedExperimentInfos != null && unPermittedExperimentInfos.Length > 0)
            {
                foreach (LssExperimentInfo upep in unPermittedExperimentInfos)
                {
                    ListItem unPermittedExperimentItem = new ListItem();
                    unPermittedExperimentItem.Text = upep.labClientName + ", " + upep.labClientVersion;
                    unPermittedExperimentItem.Value = upep.experimentInfoId.ToString();
                    lbxSelectExperiment.Items.Add(unPermittedExperimentItem);
                }
            }
        }

        private void DisplayPermittedGroups(int recurrenceID)
        {
            lbxSelectGroup.Items.Clear();
            lbxPermittedGroups.Items.Clear();
            int[] permittedGroupIDs = dbManager.ListCredentialSetIDsByRecurrence(recurrenceID);
            LssCredentialSet[] permittedGroups = dbManager.GetCredentialSets(permittedGroupIDs);
            // the Groups available in the lab server which the recurrence belongs to
            //String labServerID = dbManager.GetRecurrence(new int[]{recurrenceID})[0].labServerGuid;
            int[] availableGroupIDs = dbManager.ListCredentialSetIDs();
            ArrayList unPermittedGroupIDs = new ArrayList();
            foreach (int aeID in availableGroupIDs)
            {
                bool isIn = false;
                for (int i = 0; i < permittedGroupIDs.Length; i++)
                {
                    if (permittedGroupIDs[i] == aeID)
                    {
                        isIn = true;
                        break;
                    }
                }
                if (!isIn)
                {
                    unPermittedGroupIDs.Add(aeID);
                }
            }
            int[] unPGIDs = Utilities.ArrayListToIntArray(unPermittedGroupIDs);
            LssCredentialSet[] unPermittedGroups = dbManager.GetCredentialSets(unPGIDs);

            foreach (LssCredentialSet pep in permittedGroups)
            {
                ListItem permittedGroupItem = new ListItem();
                permittedGroupItem.Text = pep.groupName + ", " + pep.serviceBrokerName;
                permittedGroupItem.Value = pep.credentialSetId.ToString();
                lbxPermittedGroups.Items.Add(permittedGroupItem);
            }

            foreach (LssCredentialSet upep in unPermittedGroups)
            {
                ListItem unPermittedGroupItem = new ListItem();
                unPermittedGroupItem.Text = upep.groupName + ", " + upep.serviceBrokerName;
                unPermittedGroupItem.Value = upep.credentialSetId.ToString();
                lbxSelectGroup.Items.Add(unPermittedGroupItem);
            }
        }
		protected void lbxSelectTimeBlock_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if(lbxSelectTimeBlock.SelectedIndex < 0)
			{
				lblErrorMessage.Text = Utilities.FormatWarningMessage("You must select a time block!");
				lblErrorMessage.Visible = true;
			}
			else
			{
				try
				{
                    string script = "javascript:window.open('NewTimeBlockPopUp.aspx?rid=" + Int32.Parse(lbxSelectTimeBlock.SelectedValue)
                    + " ','NewTimeBlockPopUp','width=910,height=600,left=270,top=180,modal=yes,resizable=yes').focus()";
                   // btnEdit.Attributes.Add("onclick",script);
                   // btnEdit.Visible = true;
                    DisplayPermittedExperiments(Int32.Parse(lbxSelectTimeBlock.SelectedValue));
                    DisplayPermittedGroups(Int32.Parse(lbxSelectTimeBlock.SelectedValue));
				}
				catch(Exception ex)
				{
					string msg = "Exception: Cannot retrieve time block's information. " +ex.Message+". "+ex.GetBaseException();
                    lblErrorMessage.Text = Utilities.FormatErrorMessage(msg);
					lblErrorMessage.Visible = true;
				}
			}
		}

	

		protected void btnSaveChanges_Click(object sender, System.EventArgs e)
		{
				try
				{
					
						BuildRecurrenceListBox();
						//select the recently added Time Recurrence in the list box
                        lbxSelectTimeBlock.Items.FindByValue(Session["newOccurrenceID"].ToString()).Selected = true;
                        DisplayPermittedExperiments(Int32.Parse(Session["newOccurrenceID"].ToString()));
                        DisplayPermittedGroups(Int32.Parse(Session["newOccurrenceID"].ToString()));
					    string msg1 = "The record for the recurring time block '"+ lbxSelectTimeBlock.SelectedItem + "' has been created successfully.";
					    lblErrorMessage.Text= Utilities.FormatConfirmationMessage(msg1);
					    lblErrorMessage.Visible = true;
						return;
					}
				catch (Exception ex)
				{
                    string msg = "Exception:can not show the permitted experiments";
					lblErrorMessage.Text= Utilities.FormatErrorMessage(msg);
					lblErrorMessage.Visible=true;
				}
		
		}

        protected void btnEdit_Click(object sender, System.EventArgs e)
        {
        }

		protected void btnRemove_Click(object sender, System.EventArgs e)
		{
			if (lbxSelectTimeBlock.SelectedIndex<0)
			{
				lblErrorMessage.Text = Utilities.FormatWarningMessage("Select a recurring time block to be deleted.");
				lblErrorMessage.Visible=true;
				return;
			}
			try
			{
				if(dbManager.RemoveRecurrence(Int32.Parse(lbxSelectTimeBlock.SelectedValue)) <= 0)
				{
					string msg = "The time block '"+ lbxSelectTimeBlock.SelectedItem.Text + "' was not deleted.";
                    lblErrorMessage.Text = Utilities.FormatErrorMessage(msg);
					lblErrorMessage.Visible=true;
				}
				else
				{
					string msg = "The recurring time block '"+ lbxSelectTimeBlock.SelectedItem.Text + "' has been deleted.";
                    lblErrorMessage.Text = Utilities.FormatConfirmationMessage(msg);
					lblErrorMessage.Visible=true;
				}
			}
			catch (Exception ex)
			{
                string msg = "Exception: Cannot delete the recurring time block '" + lbxSelectTimeBlock.SelectedItem.Text + "'. " + ex.Message + ". " + ex.GetBaseException();
                lblErrorMessage.Text = Utilities.FormatErrorMessage(msg);
				lblErrorMessage.Visible=true;
			}
			finally
			{
				
				BuildRecurrenceListBox();
			}
		}

		protected void btnNew_Click(object sender, System.EventArgs e)
		{
            //ResetState();
            //BuildRecurrenceListBox();
		
		}

		protected void btnPermit_Click(object sender, System.EventArgs e)
		{
		  //if there is no experiment selected, throw out an error message
			if (lbxSelectExperiment.SelectedIndex<0)
			{
				string msg = "Please select an experiment";
                lblErrorMessage.Text = Utilities.FormatWarningMessage(msg);
				lblErrorMessage.Visible=true;
				return;
			}
			string savedExprimentText=lbxSelectExperiment.SelectedItem.Text;
			try
			{
				int ptbID = dbManager.AddPermittedExperiment(Int32.Parse(lbxSelectExperiment.SelectedValue),Int32.Parse(lbxSelectTimeBlock.SelectedValue));
				DisplayPermittedExperiments(Int32.Parse(lbxSelectTimeBlock.SelectedValue));
				string msg = "The permission has been given to the experiment ' " + savedExprimentText + ". ";
                lblErrorMessage.Text = Utilities.FormatConfirmationMessage(msg);
				lblErrorMessage.Visible=true;
                
			}
			catch (Exception ex)
			{
				string msg = "Exception: Cannot give the permission to the experiment ' " + savedExprimentText + "'. "+ex.Message+". "+ex.GetBaseException();
                lblErrorMessage.Text = Utilities.FormatErrorMessage(msg);
				lblErrorMessage.Visible=true;
			}


		}

		protected void btnUnPermit_Click(object sender, System.EventArgs e)
		{
			//if there is no experiment selected, throw out an error message
			if (lbxPermittedExperiments.SelectedIndex<0)
			{
				string msg = "Please select an experiment";
                lblErrorMessage.Text = Utilities.FormatWarningMessage(msg);
				lblErrorMessage.Visible=true;
				return;
			}
			string savedExprimentText=lbxPermittedExperiments.SelectedItem.Text;
			try
			{
                int recurrenceID = Int32.Parse(lbxSelectTimeBlock.SelectedValue);
				int permittedExperimentID=dbManager.ListPermittedExperimentIDByRecur(Int32.Parse(lbxPermittedExperiments.SelectedValue),Int32.Parse(lbxSelectTimeBlock.SelectedValue));
				int[] eID = dbManager.RemovePermittedExperiments(new int[]{permittedExperimentID},recurrenceID);
				if (eID.Length>0)
				{
					string msg = "The permission to the experiment ' " + savedExprimentText + "' can not been deleted. ";
                    lblErrorMessage.Text = Utilities.FormatErrorMessage(msg);
					lblErrorMessage.Visible=true;
				}
				else
				{
					DisplayPermittedExperiments(Int32.Parse(lbxSelectTimeBlock.SelectedValue));
					string msg = "The permission to the experiment ' " + savedExprimentText + "' has been deleted. ";
                    lblErrorMessage.Text = Utilities.FormatConfirmationMessage(msg);
					lblErrorMessage.Visible=true;
				}
			}
			catch (Exception ex)
			{
				string msg = "Exception: Cannot delete the permission to the experiment ' " + lbxPermittedExperiments.SelectedItem.Text + "'. "+ex.Message+". "+ex.GetBaseException();
                lblErrorMessage.Text = Utilities.FormatErrorMessage(msg);
				lblErrorMessage.Visible=true;
			}
		}

        protected void btnPermitGroup_Click(object sender, System.EventArgs e)
        {
            //if there is no experiment selected, throw out an error message
            if (lbxSelectGroup.SelectedIndex < 0)
            {
                string msg = "Please select a group";
                lblErrorMessage.Text = Utilities.FormatWarningMessage(msg);
                lblErrorMessage.Visible = true;
                return;
            }
            string savedGroupText = lbxSelectGroup.SelectedItem.Text;
            try
            {
                int ptbID = dbManager.AddPermittedCredentialSet(Int32.Parse(lbxSelectGroup.SelectedValue), Int32.Parse(lbxSelectTimeBlock.SelectedValue));
                DisplayPermittedGroups(Int32.Parse(lbxSelectTimeBlock.SelectedValue));
                string msg = "The permission has been given to the group ' " + savedGroupText + ". ";
                lblErrorMessage.Text = Utilities.FormatConfirmationMessage(msg);
                lblErrorMessage.Visible = true;

            }
            catch (Exception ex)
            {
                string msg = "Exception: Cannot give the permission to the group ' " + savedGroupText + "'. " + ex.Message + ". " + ex.GetBaseException();
                lblErrorMessage.Text = Utilities.FormatErrorMessage(msg);
                lblErrorMessage.Visible = true;
            }


        }

        protected void btnUnPermitGroup_Click(object sender, System.EventArgs e)
        {
            //if there is no experiment selected, throw out an error message
            if (lbxPermittedGroups.SelectedIndex < 0)
            {
                string msg = "Please select a group";
                lblErrorMessage.Text = Utilities.FormatWarningMessage(msg);
                lblErrorMessage.Visible = true;
                return;
            }
            string savedGroupText = lbxPermittedGroups.SelectedItem.Text;
            try
            {
                int recurrenceId = Int32.Parse(lbxSelectTimeBlock.SelectedValue);
                int permittedGroupID = Int32.Parse(lbxPermittedGroups.SelectedValue);
                int[] eID = dbManager.RemovePermittedCredentialSets(new int[] { permittedGroupID },recurrenceId);
                if (eID.Length > 0)
                {
                    string msg = "The permission to the group ' " + savedGroupText + "' can not been deleted. ";
                    lblErrorMessage.Text = Utilities.FormatErrorMessage(msg);
                    lblErrorMessage.Visible = true;
                }
                else
                {
                    DisplayPermittedGroups(Int32.Parse(lbxSelectTimeBlock.SelectedValue));
                    string msg = "The permission to the group ' " + savedGroupText + "' has been deleted. ";
                    lblErrorMessage.Text = Utilities.FormatConfirmationMessage(msg);
                    lblErrorMessage.Visible = true;
                }
            }
            catch (Exception ex)
            {
                string msg = "Exception: Cannot delete the permission for the group ' " + lbxPermittedGroups.SelectedItem.Text + "'. " + ex.Message + ". " + ex.GetBaseException();
                lblErrorMessage.Text = Utilities.FormatErrorMessage(msg);
                lblErrorMessage.Visible = true;
            }
        }
	}
}
