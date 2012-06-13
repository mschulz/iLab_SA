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
using iLabs.ServiceBroker.Internal;
using iLabs.ServiceBroker.Authorization;
using iLabs.ServiceBroker.Administration;

namespace iLabs.ServiceBroker.iLabSB
{
	/// <summary>
	/// Summary description for myLabs.
	/// </summary>
	public partial class myLabs : System.Web.UI.Page
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
			AuthorizationWrapperClass wrapper = new AuthorizationWrapperClass();

			// To see how many labs the group chosen has

			int userID = Convert.ToInt32(Session["UserID"]);
			int groupID = Convert.ToInt32(Session["GroupID"]);

			int[] labClientIDList = AdministrativeUtilities.GetGroupLabClients(groupID);

			if (labClientIDList != null)
			{
				Session["ClientCount"]=labClientIDList.Length;
				if (labClientIDList.Length>1)
				{
                    Session["LabClientList"] = labClientIDList;
					Response.Redirect("myClientList.aspx");
				}
				else if (labClientIDList.Length ==1)
				{
					// get the lab client
                    Session["ClientID"] = labClientIDList[0];
                    AdministrativeAPI.ModifyUserSession(Convert.ToInt64(Session["SessionID"]), groupID, labClientIDList[0], Session.SessionID);
					Response.Redirect("myClient.aspx");
				}
				else if (labClientIDList.Length ==0)
				{
					Response.Redirect("myClient.aspx");
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
