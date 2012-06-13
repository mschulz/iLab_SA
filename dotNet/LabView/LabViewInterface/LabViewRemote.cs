/*
 * Copyright (c) 2004-2006 The Massachusetts Institute of Technology. All rights reserved.
 * Please see license.txt in top level directory for full license.
 */

/* $Id$ */

using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Web;

using System.Runtime.InteropServices;

//using iLabs.DataTypes.SoapHeaderTypes;
//using iLabs.DataTypes.TicketingTypes;
//using iLabs.Services;
using iLabs.UtilLib;

// Specify the LabView application version and application via these resources
using CWDataServer;
using CWDSLib;

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
    /// Summary description for LabViewInterface. This version is an attempt to use a stand-alone LabView Application/Service.
    /// As the Application will require the automatic loading of several VI's this implementation should be 
    /// somewhat faster and simpler than using the standard LabVIEW exe. Also hope to make the Interface 
    /// lighter weight as to constructer start-up. long term goal allow for constructer arguments, to access remote systems.
    /// </summary>
    public class LabViewRemote : LabViewInterface
    {
        protected string host = "";  // Default for 'localhost'
        protected int port = 3363;  //LabVIEW default

        /// <summary>
        /// Connect to the local VI application, at port 3363 the 'LabView' reference will determine 
        /// what version and LabView application will be the target
        /// </summary>
        public LabViewRemote()
        {
            viServer = new ApplicationClass();
            appDir = viServer.ApplicationDirectory;
            viServer.AutomaticClose = false;
        }
        /// <summary>
        ///  Connect to the labView application running on the 
        /// specified host on port. If host is null or the empty string
        /// the localhost will be the target.
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        public LabViewRemote(string host, int port)
            : base()
        {
            if (host != null)
                this.host = host;
            this.port = port;
        }



        protected _Application getRemoteAppRef(string hostName, int portNumber)
        {
            CWDataSocket theSocket;
            _Application remoteApp = null;
            // Get the local VI
            VirtualInstrument viTmp = (VirtualInstrument)viServer.GetVIReference(@"ILAB_RemoteAppRef.vi", "", true, 0);

            string[] connectors = new String[4];
            connectors[0] = @"machineName";
            connectors[1] = @"port";
            connectors[2] = "appReference";
            connectors[3] = "errorOut";
            object param1 = (object)connectors;

            object[] data = new object[4];
            data[0] = (object)hostName;
            data[1] = (object)portNumber;
            object param2 = (object)data;

            //Call the VI
            viTmp.Call(ref param1, ref param2);
            //status returned
            if (((object[])param2)[2] != null)
            {
                remoteApp = (_Application)((object[])param2)[2];
            }
            else
            {
                StringBuilder buf = new StringBuilder("Could not resolve VIServer! at: " + hostName + ":" + portNumber);
                if (((object[])param2)[3] != null)
                {
                    string err = (string)((object[])param2)[3];
                    buf.Append("\n\tError: ");
                    buf.Append(err);
                }
                throw new Exception(buf.ToString());
            }
            return remoteApp;
        }

        public override int GetLockState(string viName)
        {
            CWDataSocket theSocket;
            int status = -1;
            VirtualInstrument vi = GetVI(viName);
            if (vi != null)
            {
                status = GetLockState(vi);
            }
            return status;
        }

        // This is not working
        public override int SetLockState(string viName, Boolean state)
        {
            // should check that the dataSocketServer is running
            // Get the local writer
            VirtualInstrument dsWriter = null;
            if (!base.IsLoaded("ILAB_DSWriter.vi"))
                dsWriter = base.GetVI("ILAB_DSWriter.vi");
            else
                dsWriter = (VirtualInstrument)viServer.GetVIReference(appDir + viPath + @"\ILAB_DSWriter.vi", "", true, 0);

            string url;
            string data;
            if ((host != null) && (host.Length > 0))

                url = "dstp://" + host + "/iLabsReader";
            else
                url = "dstp://localhost" + "/iLabsReader";
            StringBuilder buf = new StringBuilder();
            if (state)
                data = "lockvi|" + viName;
            else
                data = "unlockvi|" + viName;
            string[] connectors = new String[2];
            connectors[0] = @"URL";
            connectors[1] = @"data";

            object param1 = (object)connectors;

            object[] values = new object[2];
            values[0] = (object)url;
            values[1] = (object)data;
            object param2 = (object)values;

            //Call the VI
            dsWriter.Call(ref param1, ref param2);
            return 0;
        }
        /*
        public override void SetLockState(string viName, Boolean state)
        {
            // Get the local writer
            VirtualInstrument dsWriter = null;
            if (!base.IsLoaded("ILAB_DSWriter.vi"))
                dsWriter = base.GetVI("ILAB_DSWriter.vi");
            else
                dsWriter = (VirtualInstrument)viServer.GetVIReference(appDir + viPath + @"\ILAB_DSWriter.vi", "", true, 0);
            // Get the remote
            if (!IsLoaded("ILAB_LockControl.vi"))
            {
                LoadVI(@"user.lib\iLabs.llb", "ILAB_LockControl.vi");
                RunVI("ILAB_LockControl.vi");
            }
            CWDataSocket theSocket;
            theSocket = new CWDataSocketClass();

            if ((host != null) && (host.Length > 0))

                theSocket.ConnectTo("dstp://" + host + "/iLabsReader", CWDSAccessModes.cwdsWrite);
            else
                theSocket.ConnectTo("dstp://localhost" + "/iLabsReader", CWDSAccessModes.cwdsWrite);
            StringBuilder buf = new StringBuilder();
            if (state)
                buf.Append("lockvi|");
            else
                buf.Append("unlockvi|");
            buf.Append(viName);
            theSocket.SyncWrite(buf.ToString(), 1000);
            //theSocket.Data = buf.ToString();
            theSocket.Update();
            //theSocket.AutoConnect = false;
            //theSocket.OnStatusUpdated += new _INIDSCtlEvents_OnStatusUpdatedEventHandler(this.OnStatusUpdated);
            //theSocket.OnDataUpdated += new _INIDSCtlEvents_OnDataUpdatedEventHandler(this.OnDataUpdated);

        } */


        public override string QuitLabView()
        {
            StringBuilder message = new StringBuilder("QuitLabview: ");

            //message.Append(" SubmitAction Exit: " + submitAction("exit","testing exit"));

            //removeVI(actionVI);
            //removeVI(templateVI);
            //removeVI(isLoadedVI);
            //removeVI(cgiVI);
            if (viServer != null)
            {
                message.Append(" Process=" + viServer._ProcessID);

                try
                {
                    viServer.Quit();
                }
                catch (Exception e)
                {
                    message.Append(" ERROR on Quit(): " + e.Message);
                }

                viServer = null;
            }
            return message.ToString();
        }

        public override bool IsLoaded(string viname)
        {
            bool status = false;
            VirtualInstrument isLoadedVI = getIsLoaded();
            if (isLoadedVI != null)
            {
                string[] connectors = new String[4];
                connectors[0] = "name";
                connectors[1] = "status";
                connectors[2] = "machineName";
                connectors[3] = "port";
                object param1 = (object)connectors;

                object[] data = new object[4];
                data[0] = (object)viname;
                data[2] = (object)host;
                data[3] = (object)port;


                object param2 = (object)data;

                //Call the VI
                isLoadedVI.Call(ref param1, ref param2);
                //status returned
                if (((object[])param2)[1] != null)
                {
                    Object obj = ((object[])param2)[1];
                    status = Convert.ToBoolean(obj);
                }
            }
            return status;
        }


        /// <summary>
        /// Returns the VI if in memory does not try and load it from its location, 
        /// note if the location is application execution directory, it will be loaded.
        /// </summary>
        /// <param name="viName"></param>
        /// <returns></returns>
        public override VirtualInstrument GetVI(string viName, bool resvForCall, int options)
        {
            VirtualInstrument vi = null;
            VirtualInstrument getterVI = getGetVI();
            StringBuilder msg = new StringBuilder();
            try
            {
                string[] connectors = new string[5];
                object[] data = new object[5];
                connectors[0] = "path";
                data[0] = viName;
                connectors[1] = "viReference";
                connectors[2] = "errorOut";
                connectors[3] = "machineName";
                data[3] = (object)host;
                connectors[4] = "port";
                data[4] = (object)port;

                object param1 = (object)connectors;
                object param2 = (object)data;
                //Call the VI
                getterVI.Call(ref param1, ref param2);
                //Display the result
                //Data returned
                if (((object[])param2)[1] != null)
                    vi = (VirtualInstrument)((object[])param2)[1];
                //Error returned
                if (((object[])param2)[2] != null)
                    msg.Append(((object[])param2)[2].ToString());
            }
            catch (Exception e)
            {
               Logger.WriteLine("VI Not Found GetVI: " + viName + " " + msg.ToString());
            }
            return vi;
        }

        public override string CreateFromTemplate(string path, string templateBase, string suffix)
        {
            string buf = null;
            VirtualInstrument templateVI = getFromTemplate();
            if (templateVI != null)
            {
                string[] connectors = new String[6];
                object[] data = new object[6];
                connectors[0] = "path";
                data[0] = path;
                connectors[1] = "templateName";
                data[1] = templateBase;
                connectors[2] = "suffix";
                data[2] = suffix;
                connectors[3] = "name";
                connectors[4] = "machineName";
                data[4] = (object)host;
                connectors[5] = "port";
                data[5] = (object)port;
                object param1 = (object)connectors;
                object param2 = (object)data;

                //Call the VI
                templateVI.Call(ref param1, ref param2);

                if (((object[])param2)[3] != null)
                    buf = (string)((object[])param2)[3];
            }
            return buf;
        }



        /*
                /// <summary>
                /// Provides an interface to the LabView WebService page for testing.
                /// </summary>
                /// <param name="actionStr"></param>
                /// <param name="valueStr"></param>
                /// <returns></returns>
                public string SubmitAction(string actionStr, string valueStr)
                {
                    StringBuilder buf = new StringBuilder(actionStr);
                    VirtualInstrument vi = null;
                    switch (actionStr)
                    {
                        //case "listvis":
                        //    buf.Append(": " + ListAllVIs());
                        //    break;
                        case "statusvi":
                            buf.Append(" - " + valueStr + ": ");
                            buf.Append(GetVIStatus(valueStr));
                            break;
                        case "loadvi":
                            buf.Append(" - " + valueStr + ": ");
                            buf.Append(GetVIStatus(GetVI(valueStr)));
                            break;
                        case "runvi":
                            buf.Append(" - " + valueStr + ": ");
                            vi = GetVI(valueStr);
                            SetLockState(vi, true);
                            break;
                        case "stopvi":
                            buf.Append(" - " + valueStr + ": ");
                            vi = GetVI(valueStr);
                            SetLockState(vi, true);
                            break;
                        case "lockvi":
                            buf.Append(" - " + valueStr + ": ");
                            vi = GetVI(valueStr);
                            SetLockState(vi, true);
                            break;
                        case "unlockvi":
                            buf.Append(" - " + valueStr + ": ");
                            vi = GetVI(valueStr);
                            SetLockState(vi, false);
                            break;

                        case "closevi":
                            buf.Append(" - " + valueStr + ": ");
                            vi = GetVI(valueStr);
                            SetLockState(vi, true);
                            break;
                        case "disconnectuser":
                            buf.Append(" - " + valueStr + ": ");
                            vi = GetVI(valueStr);
                            SetLockState(vi, true);
                            break;

                        default:
                            break;
                    }
                    return buf.ToString();
                }
 
        */

        public override string submitAction(string actionStr, string valueStr)
        {
            /*
             * Initialize the variables and define the strings corresponding to
             * the VI connector labels. Note the strings are case sensitive
             **/
            StringBuilder message = new StringBuilder(actionStr + ": ");
            string dataStr = valueStr;
            VirtualInstrument actionVI = getCaseHandler();
            if (actionVI != null)
            {

                string[] connectors = new String[6];
                object[] data = new object[6];

                connectors[0] = "action";
                connectors[1] = "data";
                connectors[2] = "response";
                connectors[3] = "errorOut";
                connectors[4] = "machineName";
                data[4] = (object)host;
                connectors[5] = "port";
                data[5] = (object)port;

                //The wrapper function expects to be passed a object type by reference.
                //We pass the string array to the object type here
                object param1 = (object)connectors;

                //Define the variable that will pass the expression to be evaluated to 
                //LabVIEW and typecast it to type object


                data[0] = (object)actionStr;
                if (dataStr == null)
                    data[1] = (object)"";
                else
                    data[1] = (object)dataStr;

                object param2 = (object)data;


                //Call the VI
                actionVI.Call(ref param1, ref param2);
                //Display the result
                //Data returned
                if (((object[])param2)[2] != null)
                    message.Append(((object[])param2)[2].ToString());
                //Error returned
                if (((object[])param2)[3] != null)
                    message.Append(((object[])param2)[3].ToString());

            }
            else
            {
                message.Append(" actionVI not found");
            }
            return message.ToString();

        }

        public override string GetLabConfiguration(string group)
        {
            StringBuilder message = new StringBuilder("getConfiguration: ");
            try
            {
                if (viServer != null)
                {
                    message.Append(submitAction("listvis", ""));
                }
            }
            catch (Exception ex)
            {
                message.Append("<pre>Error: " + ex.Message + ex.StackTrace + "</pre>");
            }
            return message.ToString();
        }



        VirtualInstrument getCGI()
        {
            return (VirtualInstrument)viServer.GetVIReference(appDir + @"\www\cgi-bin\ILAB_FrameContentCGI.vi", "", true, 0);

        }
        VirtualInstrument getCaseHandler()
        {
            return (VirtualInstrument)viServer.GetVIReference(appDir + viPath + @"\ILABR_CaseHandler.vi", "", true, 0);

        }

        VirtualInstrument getVIStatus()
        {
            return (VirtualInstrument)viServer.GetVIReference(appDir + viPath + @"\ILABR_VIStatus.vi", "", true, 0);

        }

        VirtualInstrument getIsLoaded()
        {
            return (VirtualInstrument)viServer.GetVIReference(appDir + viPath + @"\ILABR_IsLoaded.vi", "", true, 0);

        }
        VirtualInstrument getFromTemplate()
        {
            return (VirtualInstrument)viServer.GetVIReference(appDir + viPath + @"\ILABR_CreateFromTemplate.vi", "", true, 0);

        }
        VirtualInstrument getSetBounds()
        {
            return (VirtualInstrument)viServer.GetVIReference(appDir + viPath + @"\ILABR_SetBounds.vi", "", true, 0);

        }
        VirtualInstrument getGetVI()
        {
            return (VirtualInstrument)viServer.GetVIReference(appDir + viPath + @"\ILABR_GetVI.vi", "", true, 0);

        }


        /****************************************************************
        public LabViewExp prepareExperiment(Coupon expCoupon, string viPath, string viName,
             int xOffset, int yOffset, int width, int height,
             string essService, long experimentID, string dataSockets, string extra)
        {


            LabViewExp exp = new LabViewExp();
            if (((essService != null) && (essService.Length > 0)) && ((dataSockets != null) && (dataSockets.Length > 0)))
            {
                string[] sockets = dataSockets.Split(';');
                // set up an experiment storage handler
                ExperimentStorageProxy ess = new ExperimentStorageProxy();
                ess.OperationAuthHeaderValue = new OperationAuthHeader();
                ess.OperationAuthHeaderValue.coupon = expCoupon;
                ess.Url = essService;
                exp.ESS = ess;
                exp.ExperimentID = experimentID;

                // Use the experimentID as the storage parameter
                foreach (string s in sockets)
                {
                    LVDataSocket reader = new LVDataSocket();
                    reader.ExperimentID = experimentID;
                    reader.ESS = ess;
                    reader.ConnectAutoUpdate(s);
                    exp.AddDataSource(reader);
                }
            }

            int status = GetVIStatus(viName);
            //Global.WriteLog("LVPortal - VIstatus: " + status);
            switch (status)
            {
                case -10:
                    throw new Exception("Error GetVIStatus: " + status);
                    break;
                case -1:
                     // VI not in memory
                    string msg = submitAction("loadvi", viPath + @"\" + viName);
                    submitAction("unlockvi", viName);
                    //submitAction("stopvi",viName);
                    break;
                case 0:
                    // eBad = 0
                    break;
                case 1: 
                    // eIdle = 1
                    // vi in memory but not running
                    submitAction("unlockvi", viName);
                    //submitAction("stopvi",viName);
                    break;
                case 1:
                    submitAction("unlockvi", viName);
                    //submitAction("stopvi",viName);
                    break;
                case 2:
                    // eRunTopLevel = 2,
                    break;
                case 3:
                    // eRunning = 3
                    break;
                case 4:
                    break;
                default:
                    throw new Exception("Error GetVIStatus: unknown status: " + status);
                    break;
            }
            SetBounds(viName, 0, 0, width, height);
            //Global.WriteLog("SetBounds: " + viName);
            // Create the task & store in database;
            exp.VI = GetVI(viName);
            return exp;
        }

        ***************************************************************/
        /*
                public LabViewExp PrepareExperiment(Coupon expCoupon, string viPath, string viName,
                     int xOffset, int yOffset, int width, int height,
                     string essService, long experimentID, string dataSockets, string extra)
                {
                    LabViewExp exp = null;
                    VirtualInstrument vi = null;
                    vi = GetVI(viPath, viName);
                    if (vi != null)
                    {
                        exp = new LabViewExp();
                        exp.VI = vi;
                        SetBounds(vi, xOffset, yOffset, width, height);
                        SetLockState(vi, false);

                        if (((essService != null) && (essService.Length > 0)) && ((dataSockets != null) && (dataSockets.Length > 0)))
                        {
                            string[] sockets = dataSockets.Split(';');
                            // set up an experiment storage handler
                            ExperimentStorageProxy ess = new ExperimentStorageProxy();
                            ess.OperationAuthHeaderValue = new OperationAuthHeader();
                            ess.OperationAuthHeaderValue.coupon = expCoupon;
                            ess.Url = essService;
                            exp.ESS = ess;
                            exp.ExperimentID = experimentID;

                            // Use the experimentID as the storage parameter
                            foreach (string s in sockets)
                            {
                                LVDataSocket reader = new LVDataSocket();
                                reader.ExperimentID = experimentID;
                                reader.ESS = ess;
                                reader.ConnectAutoUpdate(s);
                                exp.AddDataSource(reader);
                            }
                        }
                        vi.TBShowRunButton = true;
                        //RunVI(vi);
                    }
                    return exp;

                }
               
        */

        /*
                public LabViewExp prepareExperiment(long experimentID, string viPath, string viName,
                 int xOffset, int yOffset, int width, int height, string dataSockets, string extra,
                 string essService)
                {


                    LabViewExp exp = new LabViewExp();
                    if (((essService != null) && (essService.Length > 0)) && ((dataSockets != null) && (dataSockets.Length > 0)))
                    {
                        string[] sockets = dataSockets.Split(';');
                        // set up an experiment storage handler
                        ExperimentStorageProxy ess = new ExperimentStorageProxy();
                        ess.OperationAuthHeaderValue = new OperationAuthHeader();
                        ess.OperationAuthHeaderValue.coupon = expCoupon;
                        ess.Url = essService;
                        exp.ESS = ess;
                        exp.ExperimentID = experimentID;

                        // Use the experimentID as the storage parameter
                        foreach (string s in sockets)
                        {
                            LVDataSocket reader = new LVDataSocket();
                            reader.ExperimentID = experimentID;
                            reader.ESS = ess;
                            reader.ConnectAutoUpdate(s);
                            exp.AddDataSource(reader);
                        }
                    }

                    int status = GetVIStatus(viName);
                    //Global.WriteLog("LVPortal - VIstatus: " + status);
                    switch (status)
                    {
                        case -10:
                            throw new Exception("Error GetVIStatus: " + status);
                            break;
                        case -1:
                            // VI not in memory
                            string msg = submitAction("loadvi", viPath + @"\" + viName);
                            submitAction("unlockvi", viName);
                            //submitAction("stopvi",viName);
                            break;

                        case 0:
                            // vi in memory but not running
                            submitAction("unlockvi", viName);
                            //submitAction("stopvi",viName);
                            break;
                        case 1:
                            submitAction("unlockvi", viName);
                            //submitAction("stopvi",viName);
                            break;
                        case 2:
                            break;
                        case 3:
                            break;
                        case 4:
                            break;
                        default:
                            throw new Exception("Error GetVIStatus: unknown status: " + status);
                            break;
                    }
                    SetBounds(viName, 0, 0, width, height);
                    //Global.WriteLog("SetBounds: " + viName);
                    // Create the task & store in database;
                    exp.VI = GetVI(viName);
                    return exp;
                }
        */
        /*
        public LabViewExp PrepareExperiment(long experimentID, string viPath, string viName,
         int xOffset, int yOffset, int width, int height, string dataSockets, string extra,
         ExperimentStorageProxy ess)
        {
            checkCGI();
            LabViewExp exp = new LabViewExp();
            exp.ExperimentID = experimentID;
            VirtualInstrument vi = null;
            vi = GetVI(viPath, viName);
            ExecStateEnum state = vi.ExecState;
            object inCache = false;
            VILockStateEnum lockState = vi.GetLockState(out inCache);
            if (vi != null)
            {
                exp.VI = vi;
                SetBounds(vi, xOffset, yOffset, width, height);
                

                if ((ess != null) && (dataSockets != null) && (dataSockets.Length >0))
                {
                    string[] sockets = dataSockets.Split(';');
                    // set up an experiment storage handler
                    
                    exp.ESS = ess;
                    

                    // Use the experimentID as the storage parameter
                    foreach (string s in sockets)
                    {
                        LVDataSocket reader = new LVDataSocket();
                        reader.ExperimentID = experimentID;
                        reader.ESS = ess;
                        reader.ConnectAutoUpdate(s);
                        exp.AddDataSource(reader);
                    }
                }
                vi.TBShowRunButton = true;
                vi.TBVisible = true;
                //vi.Run(true);
                //vi.RunOnOpen = true;
                //vi.OpenFrontPanel(true, FPStateEnum.eFPStandard);
                SetLockState(vi, false);
                vi.Abort();
                //RunVI(vi);
                state = vi.ExecState;
            }
            return exp;

        }
 */

    }
}
