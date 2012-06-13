using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml;

using iLabs.Core;
using iLabs.DataTypes;
using iLabs.DataTypes.ProcessAgentTypes;
using iLabs.DataTypes.TicketingTypes;
using iLabs.Ticketing;
using iLabs.DataTypes.SoapHeaderTypes;
using iLabs.Proxies.ESS;

namespace iLabs.LabServer.TimeOfDay
{

    public partial class Redirect : System.Web.UI.Page
    {
        public static ExperimentStorageProxy essInterface = new ExperimentStorageProxy();
        public static TodInterface todInterface = new TodInterface();

        OperationAuthHeader essOpHeader = new OperationAuthHeader();
        OperationAuthHeader lsOpHeader = new OperationAuthHeader();

        ProcessAgentDB dbTicketing = new ProcessAgentDB();

        public static string couponId = null;
        public static string passkey = null;
        public static string issuerGuid = null;
        public static string sbUrl = null;

        public string essUrl = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                todInterface.Url = ProcessAgentDB.ServiceAgent.webServiceUrl;

                // retrieve parameters from URL
                couponId = Request.QueryString["coupon_id"];
                passkey = Request.QueryString["passkey"];
                issuerGuid = Request.QueryString["issuer_guid"];
                sbUrl = Request.QueryString["sb_url"];


                if (passkey != null && couponId != null && issuerGuid != null)
                {
                    try
                    {
                        //set execution coupon and ticket type
                        Coupon executionCoupon = new Coupon(issuerGuid, Int64.Parse(couponId), passkey);
                        string type = TicketTypes.EXECUTE_EXPERIMENT;

                        // retrieve the ticket and verify it
                        Ticket retrievedTicket = dbTicketing.RetrieveAndVerify(executionCoupon, type);

                        XmlDocument payload = new XmlDocument();
                        payload.LoadXml(retrievedTicket.payload);
                        essUrl = payload.GetElementsByTagName("essWebAddress")[0].InnerText;

                        essInterface.Url = essUrl;

                        HyperLinkSB.NavigateUrl = sbUrl;
                    }

                    catch
                    {
                        Response.Redirect("Default.aspx" + "?sb_url=" + sbUrl);
                    }

                }

                else
                {
                    Response.Redirect("Default.aspx" + "?sb_url=" + sbUrl);
                }
            }

        }
        protected void btnGetTimeOfDay_Click(object sender, EventArgs e)
        {
            Coupon opCoupon = new Coupon(issuerGuid, Int64.Parse(couponId), passkey);

            lsOpHeader.coupon = opCoupon;
            todInterface.OperationAuthHeaderValue = lsOpHeader;

            string time = todInterface.ReturnTimeOfDay();
            lblGetTimeOfDay.Text = time;
        }

        protected void btnRequestBlobAccess_Click(object sender, EventArgs e)
        {
            Coupon opCoupon = new Coupon(issuerGuid, Int64.Parse(couponId), passkey);

            lsOpHeader.coupon = opCoupon;
            todInterface.OperationAuthHeaderValue = lsOpHeader;

            long experimentId = todInterface.GetExperimentId();
            long blobId = todInterface.GetBlobId();

            essOpHeader.coupon = opCoupon;
            //blobStorage.OperationAuthHeaderValue = essOpHeader;
            essInterface.OperationAuthHeaderValue = essOpHeader;

            //string blobUrl = blobStorage.RequestBlobAccess(blobId, "http", 30);
            string blobUrl = essInterface.RequestBlobAccess(blobId, "http", 30);

            Image1.Visible = true;
            Image1.ImageUrl = blobUrl;

            Label1.Text = lblGetTimeOfDay.Text;


        }
    }
}
