/*
 * Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
 * Please see license.txt in top level directory for full license.
 * 
 * $Id$
 */

namespace iLabs.LabServer.LabView
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	/// <summary>
	///		Summary description for LVFrame.
	/// </summary>
	public partial class LVFrame : System.Web.UI.UserControl
	{
        public string cgiURL = "http://localhost:81/cgi-bin/ILAB_FrameContentCGI.vi";
        public string path = null;
		public string viName = "Tank Simulation.vi";
		public string hasControl = "true";
		public string scroll = "true";
		public int width = 50;
		public int fWidth = 70;
		public int height = 50;
		public int fHeight = 70;


		protected void Page_Load(object sender, System.EventArgs e)
		{
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
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{

		}
		#endregion
	}
}
