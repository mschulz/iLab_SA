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
using System.Web.Security;
using System.Configuration;
using iLabs.ServiceBroker;
using iLabs.ServiceBroker.Administration;
using iLabs.ServiceBroker.Authentication;
using iLabs.ServiceBroker.Authorization;
using iLabs.UtilLib;


namespace iLabs.ServiceBroker.iLabSB
{
	/// <summary>
	/// Summary description for login.
	/// </summary>
	public partial class login : System.Web.UI.Page
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			AuthorizationWrapperClass wrapper = new AuthorizationWrapperClass();

			if(Session["UserID"] != null)
			{
				Response.Redirect(Global.FormatRegularURL(Request,"myGroups.aspx"));
			}
			else
			{
				bool requireSSL = Convert.ToBoolean(ConfigurationManager.AppSettings["haveSSL"]);
				string Url;
				if ((requireSSL)&&(!Request.IsSecureConnection))
				{
					Url = Global.FormatSecureURL(Request,"login.aspx");
					Response.Redirect(Url);
				}
				else if ((!requireSSL)&&(Request.IsSecureConnection))
				{
					Url = Global.FormatRegularURL(Request,"login.aspx");
					Response.Redirect(Url);
				}
				
			}

			ArrayList messagesList = new ArrayList();
			SystemMessage[] messages = wrapper.GetSystemMessagesWrapper(SystemMessage.SYSTEM,0,0,0);
			foreach(SystemMessage message in messages)
			{
				messagesList.Add(message);
			}

			messagesList.Sort(new DateComparer());
			messagesList.Reverse();

			repSystemMessage.DataSource = messagesList;
			repSystemMessage.DataBind();

			if (messagesList==null)
				lblSystemMessage.Text +="<p>No Messages at this time</p>";
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
