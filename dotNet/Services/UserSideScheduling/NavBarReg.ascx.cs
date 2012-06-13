/*
 * Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
 * Please see license.txt in top level directory for full license.
 */ 
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

namespace iLabs.Scheduling.UserSide
{
	/// <summary>
	///		Summary description for NavBarReg.
	/// </summary>
	public partial class NavBarReg : System.Web.UI.UserControl
	{

    protected string currentPage;
	protected string helpURL = "helpReg.aspx";

	public string HelpURL
	{
		get {return helpURL;}
		set {helpURL = value; }
	}

	protected void Page_Load(object sender, System.EventArgs e)
	{
        // Get the current page name w/o path name or slashes
        currentPage = Request.Url.Segments[Request.Url.Segments.Length - 1];
	    aHelp.HRef = helpURL;
        aReservation.Attributes.Add("class", "first");
 		aReservation.Attributes.Add("class", "last");
		// Put user code to initialize the page here
            switch (currentPage)
            {
                case "Reservation.aspx":
                    aReservation.Attributes.Add("class", "topactive");
                    break;
            }
            if(Session["sbUrl"] != null){
		        liBackToSB.Visible = true;
                aBackToSB.HRef = Session["sbUrl"].ToString();
		}
		else{
			liBackToSB.Visible = false;
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
