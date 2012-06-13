/*
 * Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
 * Please see license.txt in top level directory for full license.
 * 
 * $Id$
 */
using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using iLabs.ServiceBroker.Internal;


namespace iLabs.ServiceBroker.iLabSB
{
	
	/// <summary>
	///	User Control/Include File
	///	Standard header that provides User Name and Effective Group.
	/// </summary>
	public partial class banner : System.Web.UI.UserControl
	{
		string referringPage;

		//AuthorizationWrapperClass wrapper = new AuthorizationWrapperClass();

		protected void Page_Load(object sender, System.EventArgs e)
		{
			// Get User Name
			if(Session["UserName"] != null)
			{
				lblUserNameBanner.Visible=true;
				lblUserNameBanner.Text="User: " + Session["UserName"].ToString();
			}
			else
			{
				lblUserNameBanner.Visible = false;
			}
			
			// Get Group Name
			if(Session["GroupName"] != null)
			{
				lblGroupNameBanner.Text="Group: " +Session["GroupName"].ToString();
				lblGroupNameBanner.Visible = true;
			}
			else
			{
				lblGroupNameBanner.Visible = false;
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
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{

		}
		#endregion
	}
}
