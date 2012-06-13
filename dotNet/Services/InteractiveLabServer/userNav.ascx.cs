/*
 * Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
 * Please see license.txt in top level directory for full license.
 * 
 * $Id$
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
	public partial class userNav : System.Web.UI.UserControl
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
            aTasks.Attributes.Add("class", "last");
			switch(currentPage)
			{
				case "home.aspx":
					aHome.Attributes.Add("class", "topactive");
					break;
                case "administer.aspx": aHome.Attributes.Add("class", "first");
                    aAdminister.Attributes.Add("class", "topactive");
                    break;
                case "selfRegistration.aspx":aHome.Attributes.Add("class", "first");
                    aSelfRegistration.Attributes.Add("class", "topactive");
                    break;
				case "localGroups.aspx":aHome.Attributes.Add("class", "first");
					aGroups.Attributes.Add("class", "topactive");
					break;
				case "groupPermissions.aspx":
					//Note: the myLabs page determines which clients a user/group can access,
					// then redirects to myClient.aspx. So myLabs.aspx is never displayed, though
					// it looks as though it is the page to be linked to.
                    aGroupPermissions.Attributes.Add("class", "topactive");
					break;
				case "labExperiments.aspx":
                    aExperiments.Attributes.Add("class", "topactive");
					break;
				case "manageTasks.aspx":
                    aTasks.Attributes.Add("class", "topactive");
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
