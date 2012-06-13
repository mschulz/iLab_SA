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
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace iLabs.ServiceBroker.admin
{
	/// <summary>
	/// Summary description for messagePreviewPopup.
	/// </summary>
	public partial class messagePreviewPopup : System.Web.UI.Page
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (Session["UserID"]==null)
				Response.Redirect("../login.aspx");

			lblMessage.Text=Request.Params["msg"];
			lblTitle.Text=Request.Params["title"];
			if(lblMessage.Text=="")
			{
				lblDatePosted.Text="";

			}
			else
			{
				if(Request.Params["date"]=="")
				{
					lblDatePosted.Text="DatePosted: "+DateTime.Now.ToString();

				}
				else
				{
					lblDatePosted.Text="DatePosted: "+Request.Params["date"];
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
