using System;
using System.Collections;
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
using System.Xml;

using iLabs.Core;
using iLabs.DataTypes;
using iLabs.DataTypes.SchedulingTypes;
using iLabs.DataTypes.TicketingTypes;

using iLabs.Ticketing;


using iLabs.UtilLib;


namespace iLabs.Scheduling.LabSide
{
	/// <summary>
	/// Summary description for ReservationManagement.
	/// </summary>
	public partial class ReservationManagement : System.Web.UI.Page
	{
	    string labServerGuid;
        CultureInfo culture;
        string labServerName = null;
        string couponID = null, passkey = null, issuerID = null, sbUrl = null;
        int userTZ = 0;
        int localTzOffset;
        LabSchedulingDB dbManager = new LabSchedulingDB();
        


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

		protected void Page_Load(object sender, System.EventArgs e)
		{
            culture = DateUtil.ParseCulture(Request.Headers["Accept-Language"]);
            localTzOffset = DateUtil.LocalTzOffset;
            if (!IsPostBack)
            {
                lblDateTimeFormat.Text = culture.DateTimeFormat.ShortDatePattern;
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
                        userTZ = Convert.ToInt32(payload.GetElementsByTagName("userTZ")[0].InnerText);
                        Session["userTZ"] = userTZ;

                        StringBuilder buf = new StringBuilder("Select criteria for the reservations displayed.<br/><br/>Times shown are LSS local time GMT:&nbsp;&nbsp;&nbsp;");
                        if(localTzOffset > 0)
                            buf.Append("+");
                        buf.Append(localTzOffset / 60.0);
                        buf.Append(".");
                        lblDescription.Text =  buf.ToString();

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
                    LoadResourceListBox(Session["labServerGuid"].ToString());
                    // Load the Group list box
                    LoadGroupListBox(Session["labServerGuid"].ToString());
                    // Load the Experiment list box
                    LoadExperimentListBox(Session["labServerGuid"].ToString());
                    // load the reservation List box.
                    BuildReservationListBox(Session["labServerGuid"].ToString());
                }
            }

			if (ddlTimeIs.SelectedIndex!=4)
			{
				txtTime2.ReadOnly=true;
				txtTime2.BackColor=Color.Lavender;
			}
		}

	
		protected void ddlTimeIs_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			txtTime1.Text=null;
			txtTime2.Text=null;
			if(ddlTimeIs.SelectedIndex==4)
			{
				txtTime2.ReadOnly=false;
				txtTime2.BackColor=Color.White;
			}
		}
	
		//list the reservation information according to the labserverGuid
		private void BuildReservationListBox(string labServerGuid)
		{
           
			txtDisplay.Text=null;
			try
			{
                IntTag[] reservations = dbManager.ListReservationTags(labServerGuid, DateTime.MinValue, DateTime.MinValue, culture, localTzOffset);
                if (reservations == null || reservations.Length == 0)
				{
					lblErrorMessage.Text =Utilities.FormatConfirmationMessage("no reservations have been found.");
					lblErrorMessage.Visible=true;
				}
				else
				{   
                    StringBuilder buf = new StringBuilder();
                    foreach (IntTag t in reservations)
                    {
                        buf.AppendLine(t.tag);
                    }
                    txtDisplay.Text = buf.ToString();

				}
			}
			catch(Exception ex)
			{
				lblErrorMessage.Text =Utilities.FormatErrorMessage("can not retrieve reservations  "+ex.Message);
				lblErrorMessage.Visible=true;
			}
		}
		//list the reservation information according to the selected criterion
		private void BuildReservationListBox(int resourceID, int ExperimentInfoID, int CredentialSetID, DateTime time1, DateTime time2)
		{
			
			try
			{
				txtDisplay.Text=null;
                IntTag[] reservations = dbManager.ListReservationTags(resourceID, ExperimentInfoID, CredentialSetID, time1, time2, culture, localTzOffset);
                if (reservations == null || reservations.Length == 0)
                {
                    lblErrorMessage.Text = Utilities.FormatConfirmationMessage("no reservations have been found.");
                    lblErrorMessage.Visible = true;
                }
                else
                {
                    StringBuilder buf = new StringBuilder();
                    foreach (IntTag t in reservations)
                    {
                        buf.AppendLine(t.tag);
                    }
                    txtDisplay.Text = buf.ToString();
                }
			}
			catch(Exception ex)
			{
				lblErrorMessage.Text =Utilities.FormatErrorMessage("can not retrieve reservationInfos  " + ex.Message);
				lblErrorMessage.Visible=true;
			}
		}
		protected void btnGo_Click(object sender, System.EventArgs e)
		{
			lblErrorMessage.Text ="";
			lblErrorMessage.Visible=false;
            int resourceID = 0;
            int expID = 0;
            int credID = 0;
            DateTime start = FactoryDB.MinDbDate;
            DateTime end = FactoryDB.MaxDbDate;

            if (ddlResource.SelectedIndex > 0)
            {
                resourceID = Convert.ToInt32(ddlResource.SelectedValue);
            }
            if (ddlGroup.SelectedIndex > 0)
            {
                credID = Convert.ToInt32(ddlGroup.SelectedValue);
            }
            if (ddlExperiment.SelectedIndex > 0)
            {
                expID = Convert.ToInt32(ddlExperiment.SelectedValue);
            }

           
                if (ddlTimeIs.SelectedIndex > 0)
                {
                    DateTime time1;
                    try
                    {
                        time1 = DateUtil.ParseUserToUtc(txtTime1.Text, culture, localTzOffset);
                    }
                    catch
                    {
                        lblErrorMessage.Text = Utilities.FormatWarningMessage("Please enter a valid time");
                        lblErrorMessage.Visible = true;
                        return;
                    }
                    if (ddlTimeIs.SelectedIndex == 1) //Equal To
                    {
                        start = time1;
                        end = time1.AddDays(1);


                    }
                    else if (ddlTimeIs.SelectedIndex == 2) // Before
                    {
                        //start = FactoryDB.MinDbDate;
                        end = time1;

                    }
                    else if (ddlTimeIs.SelectedIndex == 3) // After
                    {
                        start = time1;
                        //end = FactoryDB.MaxDbDate;

                    }
                    else if (ddlTimeIs.SelectedIndex == 4) //Between
                    {
                        DateTime time2;
                        try
                        {
                            time2 = DateUtil.ParseUserToUtc(txtTime2.Text, culture, localTzOffset);
                            start = time1;
                            end = time2;
                        }
                        catch
                        {
                            lblErrorMessage.Text = Utilities.FormatWarningMessage("Please enter a valid time");
                            lblErrorMessage.Visible = true;
                            return;
                        }
                    }
                }
                BuildReservationListBox(resourceID,expID, credID, start, end);
            		
		}

        private void LoadResourceListBox(string lsGuid)
        {
            ddlResource.Items.Clear();
            ddlResource.Items.Add(new ListItem(" ---------- All Resources ---------- "));
            IntTag[] resources = dbManager.GetLSResourceTags(lsGuid);
            foreach (IntTag it in resources)
            {
                ddlResource.Items.Add(new ListItem(it.tag, it.id.ToString()));
            }

        }

		private void LoadGroupListBox(string lsGuid)
		{
			ddlGroup.Items.Clear();
			try
			{
				ddlGroup.Items.Add(new ListItem(" ---------- All Groups ---------- "));
				LssCredentialSet[] credentialSets=dbManager.GetCredentialSetsByLS(lsGuid);
				for(int i=0; i< credentialSets.Length; i++)
				{
                    string cred = credentialSets[i].groupName + " : " + credentialSets[i].serviceBrokerName;
                    ddlGroup.Items.Add(new ListItem(cred, credentialSets[i].credentialSetId.ToString()));
				}
			}
			catch(Exception ex)
			{
				lblErrorMessage.Text = Utilities.FormatErrorMessage("can not load the Group List Box"+ex.Message);
				lblErrorMessage.Visible=true;
			}
		}

		private void LoadExperimentListBox(string labServerID)
		{
			ddlExperiment.Items.Clear();
			try
			{
				ddlExperiment.Items.Add(new ListItem(" ---------- Any Experiment ---------- "));
				int[] experimentInfoIDs = dbManager.ListExperimentInfoIDsByLabServer(labServerID);
				LssExperimentInfo[] experimentInfos = dbManager.GetExperimentInfos(experimentInfoIDs);
				for(int i=0; i< experimentInfoIDs.Length; i++)
				{
					
					string exper=experimentInfos[i].labClientName + "  " + experimentInfos[i].labClientVersion;
					ddlExperiment.Items.Add(new ListItem(exper, experimentInfos[i].experimentInfoId.ToString()));
				}
			}
			catch(Exception ex)
			{
				lblErrorMessage.Text = Utilities.FormatErrorMessage("can not load the Experiment List Box"+ex.Message);
				lblErrorMessage.Visible=true;
			}

		}
		/*private void btnSelectByLabServer_Click(object sender, System.EventArgs e)
		{
			if (txtLabServerID.Text == "" )
			{
				lblErrorMessage.Text ="Please enter the lab server ID";
				lblErrorMessage.Visible=true;
			}
			else
			{
				labServerIDG = txtLabServerID.Text;
				// Load the Experiment list box
				LoadExperimentListBox(labServerIDG);
				// load the reservation List box.
				BuildReservatoinListBox(labServerIDG);
			}
		}*/

        protected void NavBar1_Load(object sender, EventArgs e)
        {

        }
}
	}

