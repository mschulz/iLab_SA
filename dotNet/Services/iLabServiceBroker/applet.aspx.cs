/*
 * Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
 * Please see license.txt in top level directory for full license.
 */

/* $Id$ */

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
//using iLabs.ServiceBroker.Ticketing;

namespace iLabs.ServiceBroker.iLabSB
{
	/// <summary>
	/// Summary description for applet.
	/// </summary>
	public partial class applet : System.Web.UI.Page
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
            object obj = Request;
            if (Session == null)
            {
                int i = 0;
            }
			// Put user code to initialize the page here			
		}

		protected void RedirectToLabClient() 
		{

			// get redirect from the session
			string redirectURL = (string)Session["RedirectURL"];
			Session.Remove("RedirectURL");
				
			string script;
			script = "<script language=JavaScript>window.top.location.href = '" + redirectURL +"';</script>";
			this.RegisterStartupScript("redirect", script);
		}

		protected void RedirectToMyClient() 
		{

			// get redirect from the session
			string redirectURL = "MyClient.aspx";
				
			string script;
			script = "<script language=JavaScript>window.top.location.href = '" + redirectURL +"';</script>";
			this.RegisterStartupScript("redirect", script);
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
            this.Load += new System.EventHandler(this.Page_Load);
		}
		#endregion
	}
}
