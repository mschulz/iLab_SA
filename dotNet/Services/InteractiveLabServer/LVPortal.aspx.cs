/*
 * Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
 * Please see license.txt in top level directory for full license.
 * 
 * $Id$
 */

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
using System.Web.Security;
using System.Xml.XPath;

using iLabs.DataTypes.SoapHeaderTypes;
using iLabs.DataTypes.TicketingTypes;
using iLabs.Ticketing;
using iLabs.UtilLib;

using iLabs.LabServer.Interactive;

namespace iLabs.LabServer.LabView
{
    /// <summary>
    /// Summary description for Portal.
    /// </summary>
    public partial class LVPortal : System.Web.UI.Page
    {

        protected void Page_Load(object sender, System.EventArgs e)
        {
            LabDB dbManager = new LabDB();
            int appID = 0;
            long expID = 0;
            LabTask task = null;

            // Query values from the request
            
            string appKey = Request.QueryString["app"];
            string coupon_Id = Request.QueryString["coupon_id"];
            string passkey = Request.QueryString["passkey"];
            string issuerGUID = Request.QueryString["issuer_guid"];
            string returnTarget = Request.QueryString["sb_url"];

            if ((returnTarget != null) && (returnTarget.Length > 0))
                Session["returnURL"] = returnTarget;
            

           Logger.WriteLine("LVPortal: " + Request.Url.ToString());

            // this should be the Experiment Coupon data
            if (!(passkey != null && passkey != "" && coupon_Id != null && coupon_Id != "" && issuerGUID != null && issuerGUID != ""))
            {
               Logger.WriteLine("LVPortal: " + "AccessDenied missing Experiment credentials");
                Response.Redirect("AccessDenied.aspx?text=missing+Experiment+credentials.", true);
            }
            
            long expCoupId = Convert.ToInt64(coupon_Id);
            Coupon expCoupon = new Coupon(issuerGUID, expCoupId, passkey);

            //Check the database for ticket and coupon, if not found Redeem Ticket from
            // issuer and store in database.
            //This ticket should include group, experiment id and be valid for this moment in time??
            Ticket expTicket = dbManager.RetrieveAndVerify(expCoupon, TicketTypes.EXECUTE_EXPERIMENT);

            if (expTicket != null)
            {
                if (expTicket.IsExpired())
                {
                    Response.Redirect("AccessDenied.aspx?text=The ExperimentExecution+ticket+has+expired.", true);

                }
               

                ////Parse experiment payload, only get what is needed 	
                string payload = expTicket.payload;
                XmlQueryDoc expDoc = new XmlQueryDoc(payload);
                string tzStr = expDoc.Query("ExecuteExperimentPayload/userTZ");
                if ((tzStr != null) && (tzStr.Length > 0))
                    Session["userTZ"] = tzStr;
                string groupName = expDoc.Query("ExecuteExperimentPayload/groupName");
                Session["groupName"] = groupName;
                string sbStr = expDoc.Query("ExecuteExperimentPayload/sbGuid");
                Session["brokerGUID"] = sbStr;

                //Get Lab specific info for this URL or group
                LabAppInfo appInfo = null;
                // Experiment is specified by 'app=appKey'
                if (appKey != null && appKey.Length > 0)
                {
                    appInfo = dbManager.GetLabApp(appKey);
                }
                else // Have to use groupName & Servicebroker THIS REQUIRES groups & permissions are set in database
                {    // This is no longer the case as the USS handles groups and permissions
                    appInfo = dbManager.GetLabAppForGroup(groupName, sbStr);
                }
                if (appInfo == null)
                {
                    Response.Redirect("AccessDenied.aspx?text=Unable+to+find+application+information,+please+notify+your+administrator.", true);
                }

                // Use taskFactory to create a new task, return an existing reentrant task or null if there is an error
                LabViewTaskFactory factory = new LabViewTaskFactory();
                task = factory.CreateLabTask(appInfo, expCoupon, expTicket);

                if (task != null)
                {

                    //Useful for debugging overloads the use of a field in the banner
                    //Session["GroupName"] = "TaskID: " + task.taskID.ToString();

                    //Utilities.WriteLog("TaskXML: " + task.taskID + " \t" + task.data);

                    //Construct the information to be passed to the target page
                    TimeSpan taskDur = task.endTime - task.startTime;
                    string vipayload = LabTask.constructSessionPayload(appInfo,
                        task.startTime, taskDur.Ticks / TimeSpan.TicksPerSecond, task.taskID,
                        returnTarget, null, null);

                    //Utilities.WriteLog("sessionPayload: " + payload);
                    //Store Session information
                    Session["payload"] = vipayload;
                    if (appInfo.rev != null && appInfo.rev.Length > 0)
                    {
                        Session["lvversion"] = appInfo.rev;
                    }
                    else
                    {
                        Session.Remove("lvversion");
                    }

                    //redirect to Presentation page...
                    Response.Redirect(appInfo.page, true);
                }
                else
                {
                    Response.Redirect("AccessDenied.aspx?text=Unable+to+launch++application,+please+notify+your+administrator.", true);
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
    }
}
