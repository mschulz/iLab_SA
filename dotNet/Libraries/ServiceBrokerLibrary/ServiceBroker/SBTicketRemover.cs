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

using iLabs.Core;

using iLabs.DataTypes.StorageTypes;
using iLabs.DataTypes.SoapHeaderTypes;
using iLabs.DataTypes.TicketingTypes;
using iLabs.Proxies.ESS;
using iLabs.Proxies.PAgent;
using iLabs.ServiceBroker.DataStorage;
using iLabs.Ticketing;
using iLabs.UtilLib;




namespace iLabs.ServiceBroker
{
	/// <summary>
	/// Thread that queries for expired Tickets, and performs clean-up of the database.
    /// This method tries to Cancel any remote tickes that have been issued.
	/// </summary>
	public class SBTicketRemover
	{
        private Thread theThread;
        // waitTime in milliseconds
        //private int waitTime = 3600000; // once an hour
        private int waitTime = 600000; // 10 minutes - current default
        //private int waitTime = 60000; // 1 minute for debugging
        //private int count = 0;  // Was used to output a log message every N times
        private bool go = true;
 
        
		public SBTicketRemover()
		{
            Logger.WriteLine("SBTicketRemover created waitTime = " + waitTime);
			
            theThread = new Thread(new ThreadStart(Run));
            theThread.IsBackground = true;
            theThread.Start();
			
		}

        public SBTicketRemover(int delay){
            waitTime = delay;
            Logger.WriteLine("SBTicketRemover created waitTime = " + waitTime);
            theThread = new Thread(new ThreadStart(Run));
            theThread.IsBackground = true;
            theThread.Start();
        }


        /// <summary>
        /// Wait the timeout period before checking for Expired issued Tickets,
        /// pause and process External tickets
        /// </summary>
        public void Run()
        {
            Logger.WriteLine("SBTicketRemover Starting Run");
            while (go)
            {
                // Wait the delay amount
                Thread.Sleep(waitTime);
                ProcessIssuedTickets();
                // sleep 2 minutes then process the external tickets
                Thread.Sleep(120000);
                //Thread.Sleep(3000); // wait 3 seconds - For Debugging
                ProcessAgentDB paDB = new ProcessAgentDB();
                paDB.ProcessExpiredTickets();
                
            }
            Logger.WriteLine("SBTicketRemover Exiting Run");
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
        /// Queries the IssuedTicket table, for any 'Expired' tickets trys to notify any users
        /// close any active data references and to delete the Issued Ticket. Then checks to 
        /// see if any of the ticket coupons no longer are needed and removes them.
        /// May be called directly as the standard Run method waits until the timeout is 
        /// done before calling this, useful for debugging.
        /// </summary>
        public void ProcessIssuedTickets()
        {
            BrokerDB brokerDb = new BrokerDB();
            
            try
            {
                List<Ticket> expired = brokerDb.GetExpiredIssuedTickets();
                if (expired.Count > 0)
                {
                    RemoveTickets(expired, brokerDb);
                }
                
            }
            catch (Exception e)
            {
               Logger.WriteLine("TicketRemover: " + e.Message + ": " + Utilities.DumpException(e));
            }
        }

        public int RemoveTickets(List<Ticket> ticketList, BrokerDB brokerDb)
        {
            ArrayList coupons = new ArrayList();
            Coupon coupon = null;
            int ticketCount = 0;
            int couponCount = 0;
            if (ticketList.Count > 0)
            {
               Logger.WriteLine("RemoveTickets: expired count = " + ticketList.Count);

                

                foreach (Ticket ticket in ticketList)
                {
                    if (!coupons.Contains(ticket.couponId))
                    {
                        coupons.Add(ticket.couponId);
                    }
                    if (coupon == null || coupon.couponId != ticket.couponId)
                    {
                        coupon = brokerDb.GetIssuedCoupon(ticket.couponId);
                    }
                    switch (ticket.type)
                    {
                        case TicketTypes.ADMINISTER_EXPERIMENT:


                            string payload = ticket.payload;
                            if (payload != null)
                            {
                                XmlQueryDoc xDoc = new XmlQueryDoc(payload);
                                string url = xDoc.Query("AdministerExperimentPayload/essURL");
                                string expStr = xDoc.Query("AdministerExperimentPayload/experimentID");
                                long expID = Convert.ToInt64(expStr);
                                ExperimentStorageProxy essProxy = new ExperimentStorageProxy();
                                essProxy.OperationAuthHeaderValue = new OperationAuthHeader();
                                essProxy.OperationAuthHeaderValue.coupon = coupon;
                                essProxy.Url = url;
                                StorageStatus expStatus = essProxy.SetExperimentStatus(expID, (int)StorageStatus.CLOSED_TIMEOUT);
                                DataStorageAPI.UpdateExperimentStatus(expStatus);
                            }
                            break;
                        case TicketTypes.RETRIEVE_RECORDS:
                        case TicketTypes.STORE_RECORDS:
                            break;
                        case TicketTypes.EXECUTE_EXPERIMENT:
                        case TicketTypes.ALLOW_EXPERIMENT_EXECUTION:
                            break;
                        default: // Every other Ticket type
                            break;
                    }
                    bool statusR = false;

                    if (ticket.redeemerGuid != brokerDb.GetIssuerGuid())
                    {
                        ProcessAgentInfo redeemer = brokerDb.GetProcessAgentInfo(ticket.redeemerGuid);
                        if ((redeemer != null) && !redeemer.retired)
                        {
                            ProcessAgentProxy paProxy = new ProcessAgentProxy();
                            paProxy.AgentAuthHeaderValue = new AgentAuthHeader();
                            paProxy.Url = redeemer.webServiceUrl;
                            paProxy.AgentAuthHeaderValue.coupon = redeemer.identOut;
                            paProxy.AgentAuthHeaderValue.agentGuid = ProcessAgentDB.ServiceGuid;
                            statusR = paProxy.CancelTicket(coupon, ticket.type, ticket.redeemerGuid);
                        }
                    }
                    if (ticket.issuerGuid == brokerDb.GetIssuerGuid())
                    {
                        brokerDb.DeleteIssuedTicket(ticket.ticketId);
                        ticketCount++;
                    }
                }
                foreach (long id in coupons)
                {
                    int count = brokerDb.GetIssuedCouponCollectionCount(id);
                    if (count == 0)
                    {
                        brokerDb.DeleteIssuedCoupon(id);
                        couponCount++;
                    }
                }
               Logger.WriteLine("SB RemoveTickets: ticketCount=" + ticketCount + " \tcouponCount=" + couponCount);
            }
            return ticketCount;
        }
	}
}
