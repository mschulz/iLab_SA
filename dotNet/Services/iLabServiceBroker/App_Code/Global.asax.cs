/*
 * Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
 * Please see license.txt in top level directory for full license.
 */

/* $Id$ */

using System;
using System.Collections;

using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Web.Security;
using System.Web;
using System.Web.SessionState;

using iLabs.Core;
using iLabs.ServiceBroker.DataStorage;
using iLabs.ServiceBroker.Administration;
using iLabs.ServiceBroker.Authorization;
using iLabs.ServiceBroker.Internal;
using iLabs.ServiceBroker.Mapping;
using iLabs.ServiceBroker;
using iLabs.UtilLib;


namespace iLabs.ServiceBroker.iLabSB 
{
	/// <summary>
	/// Summary description for Global.
	/// </summary>
	public class Global : System.Web.HttpApplication
	{
        private SBTicketRemover ticketRemover;

        static Global()
        {
            if (ConfigurationManager.AppSettings["logPath"] != null
               && ConfigurationManager.AppSettings["logPath"].Length > 0)
            {
               Logger.LogPath = ConfigurationManager.AppSettings["logPath"];
               Logger.WriteLine("");
               Logger.WriteLine("#############################################################################");
               Logger.WriteLine("");
               Logger.WriteLine("Global Static started: " + iLabGlobal.Release + " -- " + iLabGlobal.BuildDate);
            }
        }

		public Global()
		{
			InitializeComponent();
		}	
		
		protected void Application_Start(Object sender, EventArgs e)
		{
			string path = ConfigurationManager.AppSettings["logPath"];
            if (path != null && path.Length > 0)
            {
               Logger.LogPath = path;
               Logger.WriteLine("");
               Logger.WriteLine("#############################################################################");
               Logger.WriteLine(iLabGlobal.Release);
               Logger.WriteLine("ISB Application_Start: starting");
            }
            RefreshApplicationCaches();
            ticketRemover = new SBTicketRemover(60000);
            ticketRemover.ProcessIssuedTickets();
		}
 
		protected void Session_Start(Object sender, EventArgs e)
		{
            object obj = Request;
            // Check for cookie added to Applet page
            HttpCookie cookie = Request.Cookies[ConfigurationManager.AppSettings["isbAuthCookieName"]];
            if (cookie != null)
            {
                object cValue = cookie.Value;
                if (cValue != null)
                {
                    long sesID = Convert.ToInt64(cValue);
                    SessionInfo info = AdministrativeAPI.GetSessionInfo(sesID);
                    if (info != null)
                    {
                        AdministrativeAPI.ModifyUserSession(sesID, info.groupID,info.clientID, Session.SessionID);
                        Session["SessionID"] = sesID;
                        Session["UserID"] = info.userID;
                        int[] myGrps = AdministrativeAPI.ListNonRequestGroupsForUser(info.userID);
                        if (myGrps != null)
                            Session["GroupCount"] = myGrps.Length;
                        Session["UserName"] = info.userName;
                        if (info.clientID > 0)
                        {
                            Session["ClientID"] = info.clientID;
                        }
                        else
                        {
                            Session.Remove("ClientID");
                        }
                        Session["UserTZ"] = info.tzOffset;
                        Session["IsAdmin"] = false;
                        Session["IsServiceAdmin"] = false;
                        Group []grps = null;
                        if (info.groupID > 0){
                            grps = AdministrativeAPI.GetGroups(new int[]{info.groupID});
                        }
                        if (grps != null && grps.Length == 1)
                        {
                            Session["GroupID"] = info.groupID;
                            Session["GroupName"] = grps[0].groupName;
                            if ((grps[0].groupName.Equals(Group.SUPERUSER)) || (grps[0].groupType.Equals(GroupType.COURSE_STAFF)))
                            {
                                Session["IsAdmin"] = true;
                            }
                            // if the effective group is a service admin group, then redirect to the service admin page.
                            // the session variable is used in the userNav page to check whether to make the corresponing tab visible
                            else if (grps[0].groupType.Equals(GroupType.SERVICE_ADMIN))
                            {
                                Session["IsServiceAdmin"] = true;
                            }
                        }
                        else
                        {
                            Session.Remove("GroupID");
                            Session.Remove("GroupName");
                            Response.Redirect("myGroups.aspx");
                        }
                    }
                }
            }
		}

		protected void Application_BeginRequest(Object sender, EventArgs e)
		{
            
			if (Request.Path.IndexOf('\\') >= 0 ||
				System.IO.Path.GetFullPath(Request.PhysicalPath) != Request.PhysicalPath) 
			{
				throw new HttpException(404, "not found");
			}
		}

		protected void Application_EndRequest(Object sender, EventArgs e)
		{

		}

		protected void Application_AuthenticateRequest(Object sender, EventArgs e)
		{

		}

		protected void Application_Error(Object sender, EventArgs e)
		{
			Exception ex = Server.GetLastError();
			if(ex is HttpUnhandledException)
			{
				EventLog.WriteEntry(this.Application.ToString(),ex.Message,EventLogEntryType.Error);
				
				Server.Transfer("reportBug.aspx?ex=true");
			}

		}

		protected void Session_End(Object sender, EventArgs e)
		{
			AdministrativeAPI.SaveUserSessionEndTime (Convert.ToInt64 (Session["SessionID"]));
			Session.RemoveAll();
			FormsAuthentication.SignOut ();
		}

		protected void Application_End(Object sender, EventArgs e)
		{
            if (ticketRemover != null)
                ticketRemover.Stop();
           Logger.WriteLine("ISB Application_End:");

            HttpRuntime runtime = (HttpRuntime)typeof(System.Web.HttpRuntime).InvokeMember("_theRuntime",
                                                                                            BindingFlags.NonPublic
                                                                                            | BindingFlags.Static
                                                                                            | BindingFlags.GetField,
                                                                                            null,
                                                                                            null,
                                                                                            null);
            if (runtime == null)
                return;
            string shutDownMessage = (string)runtime.GetType().InvokeMember("_shutDownMessage",
                                                                             BindingFlags.NonPublic
                                                                             | BindingFlags.Instance
                                                                             | BindingFlags.GetField,
                                                                             null,
                                                                             runtime,
                                                                             null);
            string shutDownStack = (string)runtime.GetType().InvokeMember("_shutDownStack",
                                                                          BindingFlags.NonPublic
                                                                           | BindingFlags.Instance
                                                                           | BindingFlags.GetField,
                                                                           null,
                                                                           runtime,
                                                                           null);
            if (!EventLog.SourceExists(".NET Runtime"))
            {
                EventLog.CreateEventSource(".NET Runtime", "Application");
            }
            EventLog log = new EventLog();
            log.Source = ".NET Runtime";
            log.WriteEntry(String.Format("\r\n\r\n_shutDownMessage={0}\r\n\r\n_shutDownStack={1}",
                                         shutDownMessage,
                                         shutDownStack),
                                         EventLogEntryType.Error);
        }

		public static string FormatRegularURL(HttpRequest r, string relativePath)
		{
			string protocol = ConfigurationManager.AppSettings["regularProtocol"];
			string serverName = 
				HttpUtility.UrlEncode(r.ServerVariables["SERVER_NAME"]);
			string vdirName = r.ApplicationPath;
			string formattedURL = protocol+"://"+serverName+vdirName+"/"+relativePath;
			return formattedURL;
		}

		public static string FormatSecureURL(HttpRequest r, string relativePath)
		{
			string protocol = ConfigurationManager.AppSettings["secureProtocol"];
			string serverName = 
				HttpUtility.UrlEncode(r.ServerVariables["SERVER_NAME"]);
			string vdirName = r.ApplicationPath;
			string formattedURL = protocol+"://"+serverName+vdirName+"/"+relativePath;
			return formattedURL;
		}

        public static void RefreshApplicationCaches()
        {
            Logger.WriteLine("Refreshing Application Caches");
            ProcessAgentDB.RefreshServiceAgent();
            // The AuthCache class is defined in the Authorization
            AuthCache.Refresh();
            ResourceMapManager.Refresh();
        }
			
		#region Web Form Designer generated code
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

