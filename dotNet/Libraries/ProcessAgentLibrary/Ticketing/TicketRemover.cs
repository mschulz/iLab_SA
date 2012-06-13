/*
 * Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
 * Please see license.txt in top level directory for full license.
 */

/* $Id$ */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Threading;

using iLabs.DataTypes.TicketingTypes;
using iLabs.Core;
using iLabs.UtilLib;

namespace iLabs.Ticketing
{
	/// <summary>
	/// Thread that queries for expired Tickets, and perform clean-up of the database.
    /// This is a simple database clean-up no requests to the service broker are generated.
	/// </summary>
	public class TicketRemover
	{
        private Thread theThread;
        // waitTime in milliseconds
        private int waitTime = 3600000; // Default is once an hour
        //private int waitTime = 600000; // Every 10 minutes
        private bool go = true;
 
        
		public TicketRemover()
		{
           Logger.WriteLine("TicketRemover created waitTime = " + waitTime);
			theThread = new Thread(new ThreadStart(Run));
            theThread.IsBackground = true;
            theThread.Start();
			
		}

        public TicketRemover(int delay){
            waitTime = delay;
            Logger.WriteLine("TicketRemover created waitTime = " + waitTime);
            theThread = new Thread(new ThreadStart(Run));
            theThread.IsBackground = true;
            theThread.Start();
        }

        /// <summary>
        /// Run waits until the delay has timed out then processes the 
        /// expired tickets, before starting to wait again.
        /// </summary>
        public void Run()
        {
            while (go)
            {
                Thread.Sleep(waitTime);
                ProcessTickets();    
            }
        }

        public void Start()
        {
            lock (this)
            {
                go = true;
            } 
            Run();
        }

        public void Stop()
        {
            lock (this)
            {
                go = false;
            }
        }

        /// <summary>
        /// Queries the Ticket table, for any 'Expired' tickets trys to delete the Ticket. Then checks to 
        /// see if any of the ticket coupons are no longer needed and removes them.
        /// </summary>
        public void ProcessTickets()
        {
            
            try
            {
                //Need to move all processing into ProcessAgentDB
                ProcessAgentDB agentDB = new ProcessAgentDB();
                int expiredCount = agentDB.ProcessExpiredTickets();
                if (expiredCount > 0)
                    Logger.WriteLine("TicketRemover: removed " + " expiredCount expired tickets");
            }
            catch (Exception e)
            {
               Logger.WriteLine("TicketRemover: " + e.Message + ": " + Utilities.DumpException(e));
            }
        }

	}
}
