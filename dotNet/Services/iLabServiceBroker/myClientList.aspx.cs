/*
 * Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
 * Please see license.txt in top level directory for full license.
 */

/* $Id$ */

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Security;

using iLabs.ServiceBroker.Administration;
using iLabs.ServiceBroker.Authorization;
using iLabs.ServiceBroker.Internal;
using iLabs.UtilLib;

namespace iLabs.ServiceBroker.iLabSB
{
	/// <summary>
	/// Summary description for myClientList.
	/// </summary>
	public partial class myClientList : System.Web.UI.Page
	{
        AuthorizationWrapperClass wrapper = new AuthorizationWrapperClass();
		protected LabClient[] lcList = null;
        public CultureInfo culture;
        public int userTZ;
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(! IsPostBack)
			{
				if(Session["GroupName"] != null)
				{
					string groupName = Session["GroupName"].ToString();
					lblGroupNameTitle.Text = groupName;
					lblGroupNameSystemMessage.Text = groupName;
					lblGroupNameLabList.Text = groupName;
				}

			}
            userTZ = Convert.ToInt32(Session["UserTZ"]);
            culture = DateUtil.ParseCulture(Request.Headers["Accept-Language"]);
			// This doesn't work - is it possible to stick an int array in the session?
			//int[] lcIDList = (int[])(Session["LabClientList"]);

			//Temporarily getting the list again from using the Utilities class
			int[] lcIDList = AdministrativeUtilities.GetGroupLabClients (Convert.ToInt32(Session["GroupID"]));
			lcList = wrapper.GetLabClientsWrapper(lcIDList);
				
			repLabs.DataSource = lcList;
			repLabs.DataBind();

         
            SystemMessage[] groupMessages = null;
            groupMessages = AdministrativeAPI.SelectSystemMessagesForGroup(Convert.ToInt32(Session["GroupID"]));
          
            if (groupMessages != null && groupMessages.Length > 0)
            {
                lblGroupNameSystemMessage.Text = "Group Messages:";
                repSystemMessage.DataSource = groupMessages;
                repSystemMessage.DataBind();
            }
            else
            {
                lblGroupNameSystemMessage.Text = "No Messages at this time!";
            }
		}
      
            public string userFormatTime(DateTime dt)
            {
                return iLabs.UtilLib.DateUtil.ToUserTime(dt, culture, userTZ);
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
			this.repLabs.ItemCommand += new System.Web.UI.WebControls.RepeaterCommandEventHandler(this.repLabs_ItemCommand);

		}
		#endregion

		private void repLabs_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
		{
			if (Session["UserID"] ==null)
				Response.Redirect("login.aspx");
			else
			{
				if(e.CommandName=="SetLabClient")
				{
					// get the labClientID from the lcList.
					// The indexer of the List will match the index of the repeater
					// since the repeater was loaded from the List.
					int clientID = ((LabClient)lcList[e.Item.ItemIndex]).clientID;
				
					// Set the LabClient session value and redirect
					Session["ClientID"] = clientID;
                    AdministrativeAPI.ModifyUserSession(Convert.ToInt64(Session["SessionID"]), Convert.ToInt32(Session["GroupID"]), clientID, Session.SessionID);
					Response.Redirect("myClient.aspx");
				}
			}
		}

		public void navLogout_Click(object sender, System.EventArgs e)
		{
			AdministrativeAPI.SaveUserSessionEndTime (Convert.ToInt64 (Session["SessionID"]));
			Session.RemoveAll();
			FormsAuthentication.SignOut ();
			Response.Redirect ("login.aspx");
		}
	}
}
