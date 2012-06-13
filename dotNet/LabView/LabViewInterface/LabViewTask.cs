/*
 * Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
 * Please see license.txt in top level directory for full license.
 * 
 * $Id$
 */

using System;
using System.Data;
using System.Configuration;
using System.Text;
using System.Web;
//using System.Web.Security;
//using System.Web.UI;
//using System.Web.UI.WebControls;
//using System.Web.UI.WebControls.WebParts;
//using System.Web.UI.HtmlControls;

using iLabs.Core;
using iLabs.DataTypes.ProcessAgentTypes;
using iLabs.DataTypes.TicketingTypes;
using iLabs.DataTypes.StorageTypes;
using iLabs.DataTypes.SoapHeaderTypes;
using iLabs.Proxies.ESS;
using iLabs.Proxies.ISB;
using iLabs.Proxies.Ticketing;
using iLabs.Ticketing;
using iLabs.UtilLib;
//using iLabs.Ticketing;
using iLabs.LabServer.Interactive;

#if LabVIEW_2011
using LabVIEW.lv2011;

namespace iLabs.LabView.LV2011
#endif
#if LabVIEW_2010
using LabVIEW.lv2010;

namespace iLabs.LabView.LV2010
#endif
#if LabVIEW_2009
using LabVIEW.lv2009;

namespace iLabs.LabView.LV2009
#endif
#if LabVIEW_86
using LabVIEW.lv86;

namespace iLabs.LabView.LV86
#endif
#if LabVIEW_82
using LabVIEW.lv821;

namespace iLabs.LabView.LV82
#endif

{
    /// <summary>
    /// Summary description for LabViewTask
    /// </summary>
    public class LabViewTask : LabTask
    {

        public static LabTask CreateLabTask(LabAppInfo appInfo, Coupon expCoupon, Ticket expTicket)
        {
            // set defaults
            DateTime startTime = DateTime.UtcNow;
            long duration = -1L;
            long experimentID = 0;
            int status = -1;

            string statusViName = null;
            string statusTemplate = null;
            string templatePath = null;
            LabDB dbManager = new LabDB();
            string qualName = null;
            string fullName = null;  // set defaults
           
            LabTask labTask = null;
            LabViewTask task = null;
            VirtualInstrument vi = null;
            LabViewInterface lvi = null;
            

            ////Parse experiment payload, only get what is needed 	
            string payload = expTicket.payload;
            XmlQueryDoc expDoc = new XmlQueryDoc(payload);
            string essService = expDoc.Query("ExecuteExperimentPayload/essWebAddress");
            string startStr = expDoc.Query("ExecuteExperimentPayload/startExecution");
            string durationStr = expDoc.Query("ExecuteExperimentPayload/duration");
            string groupName = expDoc.Query("ExecuteExperimentPayload/groupName");
            string userName = expDoc.Query("ExecuteExperimentPayload/userName");
            string expIDstr = expDoc.Query("ExecuteExperimentPayload/experimentID");

            if ((startStr != null) && (startStr.Length > 0))
            {
                startTime = DateUtil.ParseUtc(startStr);
            }
            if ((durationStr != null) && (durationStr.Length > 0) && !(durationStr.CompareTo("-1") == 0))
            {
                duration = Convert.ToInt64(durationStr);
            }
            if ((expIDstr != null) && (expIDstr.Length > 0))
            {
                experimentID = Convert.ToInt64(expIDstr);
            }


            if (appInfo.extraInfo != null && appInfo.extraInfo.Length > 0)
            {
                // Note should have either statusVI or template pair
                // Add Option for VNCserver access
                try
                {
                    XmlQueryDoc viDoc = new XmlQueryDoc(appInfo.extraInfo);
                    statusViName = viDoc.Query("extra/status");
                    statusTemplate = viDoc.Query("extra/statusTemplate");
                    templatePath = viDoc.Query("extra/templatePath");
                }
                catch (Exception e)
                {
                    string err = e.Message;
                }
            }

            // log the experiment for debugging

           Logger.WriteLine("Experiment: " + experimentID + " Start: " + DateUtil.ToUtcString(startTime) + " \tduration: " + duration);
            long statusSpan = DateUtil.SecondsRemaining(startTime, duration);



            if ((appInfo.server != null) && (appInfo.server.Length > 0) && (appInfo.port > 0))
            {
                lvi = new LabViewRemote(appInfo.server, appInfo.port);
            }
            else
            {
                lvi = new LabViewInterface();
            }
            if (!lvi.IsLoaded(appInfo.application))
            {
                vi = lvi.loadVI(appInfo.path, appInfo.application);
                if (false) // Check for controls first
                {
                    string[] names = new string[4];
                    object[] values = new object[4];
                    names[0] = "CouponId";
                    values[0] = expCoupon.couponId;
                    names[1] = "Passcode";
                    values[1] = expCoupon.passkey;
                    names[2] = "IssuerGuid";
                    values[2] = expCoupon.issuerGuid;
                    names[3] = "ExperimentId";
                    values[3] = experimentID;
                    lvi.SetControlValues(vi, names, values);
                }
                vi.OpenFrontPanel(true, FPStateEnum.eVisible);
            }
            else
            {
                vi = lvi.GetVI(appInfo.path, appInfo.application);
            }
            if (vi == null)
            {
                status = -1;
                string err = "Unable to Find: " + appInfo.path + @"\" + appInfo.application;
               Logger.WriteLine(err);
                throw new Exception(err);
            }
            // Get qualifiedName
            qualName = lvi.qualifiedName(vi);
            fullName = appInfo.path + @"\" + appInfo.application;


            status = lvi.GetVIStatus(vi);

           Logger.WriteLine("CreateLabTask - " + qualName + ": VIstatus: " + status);
            switch (status)
            {
                case -10:
                    throw new Exception("Error GetVIStatus: " + status);
                    break;
                case -1:
                    // VI not in memory
                    throw new Exception("Error GetVIStatus: " + status);

                    break;
                case 0: // eBad == 0
                    break;
                case 1: // eIdle == 1 vi in memory but not running 
                    FPStateEnum fpState = vi.FPState;
                    if (fpState != FPStateEnum.eVisible)
                    {
                        vi.OpenFrontPanel(true, FPStateEnum.eVisible);
                    }
                    vi.ReinitializeAllToDefault();
                    break;
                case 2: // eRunTopLevel: this should be the LabVIEW application
                    break;
                case 3: // eRunning
                    //Unless the Experiment is reentrant it should be stopped and be reset.
                    if(!appInfo.reentrant){
                        int stopStatus = lvi.StopVI(vi);
                        if (stopStatus != 0)
                        {
                            lvi.AbortVI(vi);
                        }
                        vi.ReinitializeAllToDefault();
                    }
                    break;
                default:
                    throw new Exception("Error GetVIStatus: unknown status: " + status);
                    break;
            }
            try
            {
                lvi.SetBounds(vi, 0, 0, appInfo.width, appInfo.height);
               Logger.WriteLine("SetBounds: " + appInfo.application);
            }
            catch (Exception sbe)
            {
               Logger.WriteLine("SetBounds exception: " + Utilities.DumpException(sbe));
            }
            lvi.SubmitAction("unlockvi", lvi.qualifiedName(vi));
           Logger.WriteLine("unlockvi Called: ");


           

            // Create the labTask & store in database;
            labTask = dbManager.InsertTask(appInfo.appID, experimentID,
           groupName, startTime, duration,
           LabTask.eStatus.Scheduled, expTicket.couponId, expTicket.issuerGuid, null);
            if (labTask != null)
            {
                //Convert the generic LabTask to a LabViewTask
                task = new LabViewTask(labTask);
            }
            if ((statusTemplate != null) && (statusTemplate.Length > 0))
            {
                statusViName = lvi.CreateFromTemplate(templatePath, statusTemplate, task.taskID.ToString());
            }


            if (((essService != null) && (essService.Length > 0)) )
            {
                // Create DataSourceManager to manage dataSocket connections
                DataSourceManager dsManager = new DataSourceManager();

                // set up an experiment storage handler
                ExperimentStorageProxy ess = new ExperimentStorageProxy();
                ess.OperationAuthHeaderValue = new OperationAuthHeader();
                ess.OperationAuthHeaderValue.coupon = expCoupon;
                ess.Url = essService;
                dsManager.essProxy = ess;
                dsManager.ExperimentID = experimentID;
                dsManager.AppKey = qualName;
                // Note these dataSources are written to by the application and sent to the ESS
                if ((appInfo.dataSources != null) && (appInfo.dataSources.Length > 0))
                {
                    string[] sockets = appInfo.dataSources.Split(',');
                    // Use the experimentID as the storage parameter
                    foreach (string s in sockets)
                    {
                        LVDataSocket reader = new LVDataSocket();
                        dsManager.AddDataSource(reader);
                        if (s.Contains("="))
                        {
                            string[] nv = s.Split('=');
                            reader.Type = nv[1];
                            reader.Connect(nv[0], LabDataSource.READ_AUTOUPDATE);

                        }
                        else
                        {
                            reader.Connect(s, LabDataSource.READ_AUTOUPDATE);
                        }

                    }
                }
                TaskProcessor.Instance.AddDataManager(task.taskID, dsManager);
            }
            string taskData = null;
            taskData = LabTask.constructTaskXml(appInfo.appID, fullName,appInfo.rev, statusViName, essService);
            dbManager.SetTaskData(task.taskID, taskData);
            task.data = taskData;
            TaskProcessor.Instance.Add(task);
            return task;
        }

        public LabViewTask()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        /// <summary>
        /// Copy constructor to create LabView Task from task retrieved from database
        /// </summary>
        /// <param name="task"></param>
        public LabViewTask(LabTask task)
        {
            taskID = task.taskID;
            couponID = task.couponID;
            experimentID = task.experimentID;
            labAppID = task.labAppID;
            status = task.Status;
            groupName = task.groupName;
            issuerGUID = task.issuerGUID;
            startTime = task.startTime;
            endTime = task.endTime;
            data = task.data;
            storage = task.storage;
        }

        public override eStatus Expire()
        {
           Logger.WriteLine("Task expired: " + taskID);

            Close(eStatus.Expired);
            status |= eStatus.Expired;
            return base.Expire();
        }

        public override eStatus Close()
        {
            return Close(eStatus.Closed);
        }

        public override eStatus Close(eStatus reason)
        {
            LabDB dbService = new LabDB();
            LabViewInterface lvi = null;
            try
            {
                if (data != null)
                {
                    XmlQueryDoc taskDoc = new XmlQueryDoc(data);
                    string viName = taskDoc.Query("task/application");
                    string statusVI = taskDoc.Query("task/status");
                    string server = taskDoc.Query("task/server");
                    string portStr = taskDoc.Query("task/serverPort");
                    if ((portStr != null) && (portStr.Length > 0) && (portStr.CompareTo("0") != 0) )
                    {
                        lvi = new LabViewRemote(server, Convert.ToInt32(portStr));
                    }
                    else
                    {
                        lvi = new LabViewInterface();
                    }
                    // Status VI not used 
                    if ((statusVI != null) && statusVI.Length != 0)
                    {
                        try
                        {
                            if(reason == eStatus.Expired)
                                lvi.DisplayStatus(statusVI, "You are out of time!", "0:00");
                            else
                                lvi.DisplayStatus(statusVI, "Your experiment has been cancelled", "0:00");
                        }
                        catch (Exception ce)
                        {
                           Logger.WriteLine("Trying StatusVI: " + ce.Message);
                        }
                    }


                    //Get the VI and send version specfic call to get control of the VI
                    VirtualInstrument vi = lvi.GetVI(viName);
                    // LV 8.2.1
                    //Server takes control of RemotePanel, connection not broken
                    
                    lvi.SubmitAction("lockvi", lvi.qualifiedName(vi));
                    int stopStatus = lvi.StopVI(vi);
                    if (stopStatus != 0)
                    { //VI found but no stop control
                        vi.Abort();
                       Logger.WriteLine("Expire: AbortVI() called because no stop control");
                    }

                    // Also required for LV 8.2.0 and 7.1, force disconnection of RemotePanel
#if LabVIEW_82
                    lvi.SubmitAction("closevi", lvi.qualifiedName(vi));
#endif

                    vi = null;

                }
               Logger.WriteLine("TaskID = " + taskID + " has expired");
                dbService.SetTaskStatus(taskID, (int)reason);
                status = eStatus.Closed;
               
                    DataSourceManager dsManager = TaskProcessor.Instance.GetDataManager(taskID);
                    if (dsManager != null)
                    {
                        dsManager.CloseDataSources();
                        TaskProcessor.Instance.RemoveDataManager(taskID);
                    }
                
                dbService.SetTaskStatus(taskID, (int)status);
                if (couponID > 0)
                { // this task was created with a valid ticket, i.e. not a test.
                    Coupon expCoupon = dbService.GetCoupon(couponID, issuerGUID);

                    // Only use the domain ServiceBroker, do we need a test
                    // Should only be one
                    ProcessAgentInfo[] sbs = dbService.GetProcessAgentInfos(ProcessAgentType.SERVICE_BROKER);

                    if ((sbs == null) || (sbs.Length < 1))
                    {
                       Logger.WriteLine("Can not retrieve ServiceBroker!");
                        throw new Exception("Can not retrieve ServiceBroker!");
                    }
                    ProcessAgentInfo domainSB = null;
                    foreach (ProcessAgentInfo dsb in sbs)
                    {
                        if (!dsb.retired)
                        {
                            domainSB = dsb;
                            break;
                        }
                    }
                    if (domainSB == null)
                    {
                       Logger.WriteLine("Can not retrieve ServiceBroker!");
                        throw new Exception("Can not retrieve ServiceBroker!");
                    }
                    InteractiveSBProxy iuProxy = new InteractiveSBProxy();
                    iuProxy.AgentAuthHeaderValue = new AgentAuthHeader();
                    iuProxy.AgentAuthHeaderValue.coupon = sbs[0].identOut;
                    iuProxy.AgentAuthHeaderValue.agentGuid = ProcessAgentDB.ServiceGuid;
                    iuProxy.Url = sbs[0].webServiceUrl;
                    StorageStatus storageStatus = iuProxy.AgentCloseExperiment(expCoupon, experimentID);
                   Logger.WriteLine("AgentCloseExperiment status: " + storageStatus.status + " records: " + storageStatus.recordCount);


                    // currently RequestTicketCancellation always returns false
                    // Create ticketing service interface connection to TicketService
                    TicketIssuerProxy ticketingInterface = new TicketIssuerProxy();
                    ticketingInterface.AgentAuthHeaderValue = new AgentAuthHeader();
                    ticketingInterface.Url = sbs[0].webServiceUrl;
                    ticketingInterface.AgentAuthHeaderValue.coupon = sbs[0].identOut;
                    ticketingInterface.AgentAuthHeaderValue.agentGuid = ProcessAgentDB.ServiceGuid;
                    if (ticketingInterface.RequestTicketCancellation(expCoupon, TicketTypes.EXECUTE_EXPERIMENT, ProcessAgentDB.ServiceGuid))
                    {
                        dbService.CancelTicket(expCoupon, TicketTypes.EXECUTE_EXPERIMENT, ProcessAgentDB.ServiceGuid);
                       Logger.WriteLine("Canceled ticket: " + expCoupon.couponId);
                    }
                    else
                    {
                       Logger.WriteLine("Unable to cancel ticket: " + expCoupon.couponId);
                    }
                }
            }
            catch (Exception e1)
            {
               Logger.WriteLine("ProcessTasks Cancelled: exception:" + e1.Message + e1.StackTrace);
            }
            finally
            {
                lvi = null;
            }
            return base.Close();
        }


        public override eStatus HeartBeat()
        {

            try
            {
                if (status == eStatus.Running)
                {
                    if (data != null)
                    {
                        XmlQueryDoc taskDoc = new XmlQueryDoc(data);
                        string vi = taskDoc.Query("task/application");
                        string statusVI = taskDoc.Query("task/status");

                        if ((statusVI != null) && (statusVI.Length > 0))
                        {
                            I_LabViewInterface lvi = null;
                            try
                            {
                                string server = taskDoc.Query("task/server");
                                string portStr = taskDoc.Query("task/serverPort");

                                if (((server != null) && (server.Length > 0)) && ((portStr != null) && (portStr.Length > 0)))
                                {
                                    lvi = new LabViewRemote(server, Convert.ToInt32(portStr));
                                }
                                else
                                {
                                    lvi = new LabViewInterface();
                                }
                                long ticks = endTime.Ticks - DateTime.UtcNow.Ticks;
                                TimeSpan val = new TimeSpan(ticks);
                                lvi.DisplayStatus(statusVI, "TaskID: " + taskID, val.Minutes + ":" + val.Seconds);
                            }
                            catch (Exception ce2)
                            {
                               Logger.WriteLine("Status: " + ce2.Message);
                                throw;
                            }
                            finally
                            {
                                lvi = null;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
               Logger.WriteLine("ProcessTasks Status: " + e.Message);
            }
            return status;
        }

    }
}