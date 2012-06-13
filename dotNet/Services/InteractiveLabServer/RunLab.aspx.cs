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
	/// RunLab is a generic page for presenting LabVIEW applications via CGI calls
    /// ( See ILAB_RFrameContentCGI.vi ), in LabVIEW 8.2.1 there is a
    /// difference in the configuration of the GWeb Server
    /// which makes CGI calls more difficult. LabApp field appUrl should be the fully qualified address 
    /// of ILAB_FrameContentCGI.vi and the GWeb Server should work in shared mode.
	/// </summary>
	public partial class RunLab : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.Label lblCoupon;
		protected System.Web.UI.WebControls.Label lblTicket;
		protected System.Web.UI.WebControls.Label lblGroupNameTitle;
	
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
                lnkBackSB.NavigateUrl = returnURL;

			string payload = (string) Session["payload"];
			XmlQueryDoc xDoc = new XmlQueryDoc(payload);
			String viidStr= xDoc.Query("payload/appId");
			String viname = xDoc.Query("payload/application");  
            String cgi = xDoc.Query("payload/appUrl");
			String widthStr = xDoc.Query("payload/width");
			String heightStr= xDoc.Query("payload/height");
            String startStr = xDoc.Query("payload/startTime");
            String durationStr= xDoc.Query("payload/duration");
			int width = Convert.ToInt32(widthStr);
			int height = Convert.ToInt32(heightStr);
            long duration = -1;
            if((durationStr != null) && (durationStr.Length > 0))
                duration = Convert.ToInt64(durationStr);
            if ((cgi != null) && (cgi.Length > 0))
            {
                theFrame.cgiURL = cgi;
            }
            else
            {
                theFrame.cgiURL = @"http://" + this.ApplicationInstance.Server.MachineName + @":81/cgi-bin/ILAB_FrameContentCGI.vi";
            }
            
			theFrame.viName = viname;
			theFrame.width = width;
			theFrame.fWidth = width+ 40;
			theFrame.height= height;
			theFrame.fHeight = height + 40;
            if(duration > 0){
                RegisterStartupScript("timer", LabUtils.TimerScript(startStr, duration, tz,
                    DateUtil.ParseCulture(Request.Headers["Accept-Language"]),returnURL,1000));
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
