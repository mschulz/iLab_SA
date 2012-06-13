/*
 * Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
 * Please see license.txt in top level directory for full license.
 * 
 * $Id$
 */

using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Web;
using System.Web.SessionState;
using System.Runtime.InteropServices;
using System.Threading;

using iLabs.Core;
using iLabs.Ticketing;
using iLabs.UtilLib;

using iLabs.LabServer.Interactive;
using iLabs.LabServer.LabView; 

namespace iLabs.LabServer.LabView 
{
	/// <summary>
	/// Summary description for Global.
	/// </summary>
	public class Global : System.Web.HttpApplication
	{
		
		static protected Thread threadLV;
        //static public Hashtable taskTable;
        public static TaskHandler taskThread;
        //public static TaskProcessor tasks;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private TicketRemover ticketRemover;
		
        

	

		public static void ThreadProc() 
		{
			//LVDLLStatus(IntPtr.Zero, 0, IntPtr.Zero);
			//PumpMessages();
		}

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
            ProcessAgentDB.RefreshServiceAgent();
			//threadLV = new Thread(new ThreadStart(ThreadProc));
			//threadLV.Start();
          
			
            //taskTable = new Hashtable();
            //tasks = new TaskProcessor();
           Logger.WriteLine("Global Static ended");
		}
/*
		public static LabViewInterface GetLVI()
		{
				return Global.labView.GetLVI();
		}

*/

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
               Logger.WriteLine("Application_Start: starting");
            }
            ProcessAgentDB.RefreshServiceAgent();
            //Should load any active tasks and update any expired tasks
            LabDB dbService = new LabDB();
            LabTask[] activeTasks = dbService.GetActiveTasks();
            foreach (LabTask task in activeTasks)
            {
                TaskProcessor.Instance.Add(task);
            }
            taskThread = new TaskHandler(TaskProcessor.Instance);
            ticketRemover = new TicketRemover();
		}
 
		protected void Session_Start(Object sender, EventArgs e)
		{
           Logger.WriteLine("Session_Start: " + sender.ToString() + " \t : " + e.ToString());
            Exception ex = new Exception("Session_Start:");
            string tmp = Utilities.DumpException(ex);
           Logger.WriteLine(Utilities.DumpException(ex));

		}

		protected void Application_BeginRequest(Object sender, EventArgs e)
		{
			// In InteractiveLabView
           Logger.WriteLine("Request: " + sender.ToString() + " \t : " + e.ToString());

		}

		protected void Application_EndRequest(Object sender, EventArgs e)
		{

		}

		protected void Application_AuthenticateRequest(Object sender, EventArgs e)
		{

		}

		protected void Application_Error(Object sender, EventArgs e)
		{
           Logger.WriteLine("Application_Error: " + sender.ToString() + " \t : " + e.ToString());
            Exception ex = new Exception("Application_Error: ");
            string tmp = Utilities.DumpException(ex);
           Logger.WriteLine(Utilities.DumpException(ex));
		}

		protected void Session_End(Object sender, EventArgs e)
		{
           Logger.WriteLine("Session_End: " + sender.ToString() + " \t : " + e.ToString());
            Exception ex = new Exception("Session_End:");
            string tmp = Utilities.DumpException(ex);
           Logger.WriteLine(Utilities.DumpException(ex));

		}
		

		protected void Application_End(Object sender, EventArgs e)
		{
            if (ticketRemover != null)
                ticketRemover.Stop();
			System.Console.WriteLine("Application_End Called:");
           Logger.WriteLine("Application_End: closing");
			
		}
		override public void Dispose()
		{
           Logger.WriteLine("GLOBAL:Dispose Called:");
			Application_End(this, null);
			base.Dispose();
		}
	

        public static string FormatRegularURL(HttpRequest r, string relativePath)
        {
            string protocol = ConfigurationManager.AppSettings["regularProtocol"];
            string serverName =
                HttpUtility.UrlEncode(r.ServerVariables["SERVER_NAME"]);
            string vdirName = r.ApplicationPath;
            string formattedURL = protocol + "://" + serverName + vdirName + "/" + relativePath;
            return formattedURL;
        }

        public static string FormatSecureURL(HttpRequest r, string relativePath)
        {
            string protocol = ConfigurationManager.AppSettings["secureProtocol"];
            string serverName =
                HttpUtility.UrlEncode(r.ServerVariables["SERVER_NAME"]);
            string vdirName = r.ApplicationPath;
            string formattedURL = protocol + "://" + serverName + vdirName + "/" + relativePath;
            return formattedURL;
        }

     
			
		#region Web Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.components = new System.ComponentModel.Container();
		}
		#endregion
	}


	

}

