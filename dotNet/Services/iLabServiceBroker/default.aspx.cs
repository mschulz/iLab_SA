/*
 * Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
 * Please see license.txt in top level directory for full license.
 */

/* $Id$ */

using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Configuration;

using iLabs.Core;

namespace iLabs.ServiceBroker.iLabSB
{
	/// <summary>
	/// Summary description for _default.
	/// </summary>
	public partial class _default : System.Web.UI.Page
	{
		protected string showUrl;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			bool requireSSL = Convert.ToBoolean(ConfigurationManager.AppSettings["haveSSL"]);
			if (requireSSL)
				showUrl = Global.FormatSecureURL(Request,"home.aspx");
			else
				showUrl = Global.FormatRegularURL(Request,"home.aspx");

            if (Request.Params["login"] != null)
            {
                string query = Request.QueryString["ReturnUrl"];
                showUrl = "login.aspx";
            }
            if (Request.Params["sso"] != null)
            {
                 StringBuilder buf = new StringBuilder("ssoAuth.aspx");
                NameValueCollection nValues = Request.QueryString;
                string[] keys= nValues.AllKeys;
                int count = 0;
                foreach(string k in keys){
                    if (count > 0)
                        buf.Append("&");
                    else buf.Append("?");
                   string v = nValues.Get(k);
                    buf.Append(k);
                    buf.Append("=");
                    buf.Append(v);
          
                }
                showUrl = buf.ToString();
            }
			// Put user code to initialize the page here
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
            if (Request.Params["sso"] != null)
            {
                StringBuilder buf = new StringBuilder("ssoAuth.aspx");
                NameValueCollection nValues = Request.QueryString;
                string[] keys = nValues.AllKeys;
                int count = 0;
                foreach (string k in keys)
                {
                    if (count > 0)
                        buf.Append("&");
                    else buf.Append("?");
                    string v = nValues.Get(k);
                    buf.Append(k);
                    buf.Append("=");
                    buf.Append(v);
                    count++;

                }
                showUrl = buf.ToString();
                frmUser.Attributes["src"] = showUrl;
                //string jScript = @"<script language='javascript'>theuser.location.href = '"
                //        + ProcessAgentDB.ServiceAgent.codeBaseUrl + @"/" +showUrl + @"'</script>";
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "ReloadFrame", jScript);
            } 
		}
		#endregion
	}
}
