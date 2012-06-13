/*
 * Copyright (c) 2012 The Massachusetts Institute of Technology. All rights reserved.
 * Please see license.txt in top level directory for full license.
 * 
 * $Id: userNav.ascx.cs 450 2011-09-07 20:33:00Z phbailey $
 */
namespace iLabs.LabServer
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using System.Web.Security;

	/// <summary>
	///		Summary description for userNav.
	/// </summary>
	public partial class labNav : System.Web.UI.UserControl
	{


		protected string currentPage;
		protected string helpURL = "help.aspx";

		public string HelpURL
		{
			get {return helpURL;}
			set {helpURL = value; }
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			// Get the current page name w/o path name or slashes
			currentPage = Request.Url.Segments[Request.Url.Segments.Length -1];
			aHelp.HRef = helpURL;
			SetNavList();
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

		/// <summary>
		/// Sets the button state on the items in the unorderd list "ulNavList"
		/// </summary>
		private void SetNavList()
		{
            aHome.Attributes.Add("class", "first");
            aHome.Attributes.Add("class", "last");
			switch(currentPage)
			{
				case "home.aspx":
					aHome.Attributes.Add("class", "topactive");
					break;
				case "help.aspx":
					aHelp.Attributes.Add("class", "topactive");
					break;
				default:
					break;
			}
            if (Session["sbUrl"] != null)
            {
                liBackToSB.Visible = true;
                aBackToSB.HRef = Session["sbUrl"].ToString();
            }
            else
            {
                liBackToSB.Visible = false;
            }
		}
	}
}
