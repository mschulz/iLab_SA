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
using iLabs.DataTypes.ProcessAgentTypes;
using iLabs.DataTypes.SchedulingTypes;
using iLabs.DataTypes.TicketingTypes;
using iLabs.Ticketing;
using iLabs.UtilLib;
using System.Globalization;
using System.Xml;

namespace iLabs.Scheduling.UserSide
{
	/// <summary>
	/// Summary description for ReservationManagement.
	/// </summary>
	public partial class ReservationManagement : System.Web.UI.Page
	{
        string couponID = null, passkey = null, issuerID = null, sbUrl = null;
        CultureInfo culture;
        UserSchedulingDB dbManager = new UserSchedulingDB();
        string dateF = null;
        int userTZ;
	
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
             dateF = DateUtil.DateTime24(culture);
            // Load the Group list box
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
                 if (Session["userTZ"] != null)
                     userTZ = Convert.ToInt32(Session["userTZ"]);
                 bool unauthorized = false;

                 if (couponID != null && passkey != null && issuerID != null)
                 {
                     try
                     {
                         Coupon coupon = new Coupon(issuerID, long.Parse(couponID), passkey);

                         ProcessAgentDB dbTicketing = new ProcessAgentDB();
                         Ticket ticket = dbTicketing.RetrieveAndVerify(coupon, TicketTypes.MANAGE_USS_GROUP);
                         XmlDocument payload = new XmlDocument();
                         payload.LoadXml(ticket.payload);
                         if (ticket.IsExpired() || ticket.isCancelled)
                         {
                             unauthorized = true;
                             Response.Redirect("Unauthorized.aspx", false);
                         }

                         Session["couponID"] = couponID;
                         Session["passkey"] = passkey;
                         Session["issuerID"] = issuerID;
                         Session["sbUrl"] = sbUrl;
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
                     StringBuilder buf = new StringBuilder();
                     buf.Append("Select criteria for the reservations to be displayed.  Enter date values using this format: '");
                     buf.Append(dateF + " [PM]");
                     buf.Append ("' time may be entered as 24 or 12 hour format.");
                     buf.Append("<br/><br/>Times shown are GMT:&nbsp;&nbsp;&nbsp;" + userTZ / 60.0 + "&nbsp;&nbsp; and use a 24 hour clock.");
                     lblDescription.Text = buf.ToString();
                     lblFormat.Text = dateF;
                     LoadGroupListBox();
                     LoadExperimentListBox();
                 }
             }
             else
             {
                 userTZ = userTZ = Convert.ToInt32(Session["userTZ"]);
             }

			if (ddlTimeIs.SelectedIndex!=4)
			{
				txtTime2.ReadOnly=true;
				txtTime2.BackColor=Color.Lavender;
			}			
			
		}

	
		public void ddlTimeIs_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			txtTime1.Text=null;
			txtTime2.Text=null;
			if(ddlTimeIs.SelectedIndex==4)
			{
				txtTime2.ReadOnly=false;
				txtTime2.BackColor=Color.White;
			}
		}
	
		
		//list the reservation information according to the selected criterion
		private void BuildReservationListBox(string userName, int ExperimentInfoID, int credentialSetId, DateTime time1, DateTime time2)
		{
			
			try
			{
				txtDisplay.Text=null;
				ReservationInfo[] reservations = dbManager.GetReservationInfos(userName, ExperimentInfoID, credentialSetId,  time1,  time2);						
				if (reservations.Length==0)
				{
					lblErrorMessage.Text =Utilities.FormatConfirmationMessage("no reservations have been made.");
					lblErrorMessage.Visible=true;
				}
				else
				{
                    StringBuilder buf = new StringBuilder();
					for(int j = reservations.Length-1; j > -1  ; j--)
					{
						string uName = reservations[j].userName;
						UssExperimentInfo exinfo = dbManager.GetExperimentInfos(new int[]{reservations[j].experimentInfoId})[0];
						string experimentName = exinfo.labClientName + "  " + exinfo.labClientVersion;
						buf.AppendLine(DateUtil.ToUserTime(reservations[j].startTime, culture, userTZ,dateF) + " <-> " + DateUtil.ToUserTime(reservations[j].endTime, culture, userTZ,dateF) + " " + uName + ", " + experimentName);
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
		protected void btnGo_Click(object sender, System.EventArgs e)
		{
            string userName = null;
            int credentialSetId = -1;
            int experimentInfoId = -1;
			lblErrorMessage.Text ="";
			lblErrorMessage.Visible=false;

            userName = txtUserName.Text;
            if (ddlGroup.SelectedIndex > 0)
            {
                credentialSetId = Int32.Parse(ddlGroup.SelectedValue);
            }
				
				

                if (ddlExperiment.SelectedIndex > 0)
                {
                    experimentInfoId = Int32.Parse(ddlExperiment.SelectedValue);
                }
                
				
				if (ddlTimeIs.SelectedIndex<1) // select-Time
				{
                    BuildReservationListBox(userName, experimentInfoId, credentialSetId, FactoryDB.MinDbDate, FactoryDB.MaxDbDate);

				}
				else 
				{
					DateTime time1;
					try
					{
                        time1 = DateUtil.ParseUserToUtc(txtTime1.Text, culture, userTZ);
					}
					catch
					{	
						lblErrorMessage.Text = Utilities.FormatWarningMessage("Please enter a valid time");
						lblErrorMessage.Visible=true;
						return;
					}
					if(ddlTimeIs.SelectedIndex==1) // Date
					{
						BuildReservationListBox(userName, experimentInfoId,credentialSetId, time1.Date,time1.Date.AddDays(1));

					}
					else if(ddlTimeIs.SelectedIndex==2) // before
					{
                        BuildReservationListBox(userName, experimentInfoId, credentialSetId, FactoryDB.MinDbDate, time1);

					}				
					else if(ddlTimeIs.SelectedIndex==3) // after
					{
                        BuildReservationListBox(userName, experimentInfoId, credentialSetId, time1, FactoryDB.MaxDbDate);

					}
					else if(ddlTimeIs.SelectedIndex==4) // between
					{
						DateTime time2;
						try
						{
                            time2 = DateUtil.ParseUserToUtc(txtTime2.Text, culture, userTZ);
						}
						catch
						{	
							lblErrorMessage.Text = Utilities.FormatWarningMessage("Please enter a valid time");
							lblErrorMessage.Visible=true;
							return;
						}
						BuildReservationListBox(userName, experimentInfoId,credentialSetId,time1,time2);

					}
				}
					
		}
		private void LoadGroupListBox()
		{
			ddlGroup.Items.Clear();
			try
			{
				ddlGroup.Items.Add(new ListItem(" ---------- select Group ---------- "));
				int[] credentialSetIds = dbManager.ListCredentialSetIds();
				UssCredentialSet[] credentialSets=dbManager.GetCredentialSets(credentialSetIds);
				for(int i=0; i< credentialSets.Length; i++)
				{
					
					string cred=credentialSets[i].groupName+" "+credentialSets[i].serviceBrokerName;
					ddlGroup.Items.Add(new ListItem(cred, credentialSets[i].credentialSetId.ToString()));
				}
			}
			catch(Exception ex)
			{
				lblErrorMessage.Text ="can not load the Group List Box"+ex.Message;
				lblErrorMessage.Visible=true;
			}
		}

		private void LoadExperimentListBox()
		{
			ddlExperiment.Items.Clear();
			try
			{
				ddlExperiment.Items.Add(new ListItem(" ---------- select Experiment ---------- "));
				int[] experimentInfoIds = dbManager.ListExperimentInfoIDs();
				UssExperimentInfo[] experimentInfos = dbManager.GetExperimentInfos(experimentInfoIds);
				for(int i=0; i< experimentInfoIds.Length; i++)
				{
					
					string exper=experimentInfos[i].labClientName + "  " + experimentInfos[i].labClientVersion;
					ddlExperiment.Items.Add(new ListItem(exper, experimentInfos[i].experimentInfoId.ToString()));
				}
			}
			catch(Exception ex)
			{
				lblErrorMessage.Text =Utilities.FormatErrorMessage("can not load the Experiment List Box"+ex.Message);
				lblErrorMessage.Visible=true;
			}

		}
	
	}
	}
