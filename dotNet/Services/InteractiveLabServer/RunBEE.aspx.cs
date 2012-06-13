/*
 * Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
 * Please see license.txt in top level directory for full license.
 * 
 * $Id: RunExperiment.aspx.cs 450 2011-09-07 20:33:00Z phbailey $
 */

using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using iLabs.Ticketing;
using iLabs.UtilLib;
using iLabs.LabServer.Interactive;

namespace iLabs.LabServer.LabView
{
	/// <summary>
    /// RunExperiment is a generic page for presenting LabVIEW applications in 
    /// embedded RemotePanels. LabApp field appUrl should be the address of the 
    /// LabVIEW WebServer and it should be configured for Stand-Alone. 
    /// Default is the LabServer machine port 81.
	/// </summary>
	public partial class RunBEE : System.Web.UI.Page
	{
        protected bool showTime;
		protected System.Web.UI.WebControls.Label lblCoupon;
		protected System.Web.UI.WebControls.Label lblTicket;
		protected System.Web.UI.WebControls.Label lblGroupNameTitle;
        protected string title = "Building Energy Efficiency iLab";
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
            //Use Session payload vs. URL params to validate
			// if credentials pass / Get Cookie
			//HttpCookie cookie = (HttpCookie) Request.Cookies.Get("viData");
			// if credentials pass / Get Cookie
            int tz = 0;
            if(Session["userTZ"] != null)
                tz = Convert.ToInt32(Session["userTZ"]);
            String returnURL = (string)Session["returnURL"];
            if ((returnURL != null) && (returnURL.Length > 0))
            {
                lnkBackSB.NavigateUrl = returnURL;
                lnkBackSB.Visible = true;
            }
            else{
                lnkBackSB.Visible = false;
            }

			string payload = (string) Session["payload"];
			XmlQueryDoc xDoc = new XmlQueryDoc(payload);
			String viidStr= xDoc.Query("payload/appId");
			String viname = xDoc.Query("payload/application");
            String revision = xDoc.Query("payload/revision");  
            String appUrl = xDoc.Query("payload/appUrl");
			String widthStr = xDoc.Query("payload/width");
			String heightStr= xDoc.Query("payload/height");
            String startStr = xDoc.Query("payload/startTime");
            String durationStr= xDoc.Query("payload/duration");
			int width = Convert.ToInt32(widthStr);
			int height = Convert.ToInt32(heightStr);
            long duration = -1;
            if((durationStr != null) && (durationStr.Length > 0))
                duration = Convert.ToInt64(durationStr);
            if ((appUrl != null) && (appUrl.Length > 0))
            {
                thePanel.serverURL = appUrl;
            }
            else
            {
                thePanel.serverURL = @"http://" + this.ApplicationInstance.Server.MachineName + @":81";
            }
   
			thePanel.viName = viname;
            thePanel.version = revision;
			thePanel.width = width;
			thePanel.height= height;
            if (divTimeRemaining.Visible)
            {
                if (duration > 0)
                {
                    RegisterStartupScript("timer", LabUtils.TimerScript(startStr, duration, tz,
                        DateUtil.ParseCulture(Request.Headers["Accept-Language"]), returnURL, 1000, "txtTimeRemaining"));
                }
                else
                {
                    divTimeRemaining.Visible = false;
                }

            }
						
            /* Use Session payload vs. URL params to validate
			}
			else
			{
				Response.Redirect("AccessDenied.aspx", true);
			}
            */
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
