/*
 * Copyright (c) 2004-2006 The Massachusetts Institute of Technology. All rights reserved.
 * Please see license.txt in top level directory for full license.
 */

/* $Id$ */

using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Services.Protocols;

using iLabs.DataTypes.StorageTypes;

using iLabs.UtilLib;
using iLabs.LabServer.Interactive;

using iLabs.Proxies.ESS;

using CWDataServer;
using CWDSLib;


namespace iLabs.LabView
{

    public class LVDataSocket : LabDataSource
    {
        private CWDataSocket theSocket;
        private bool firstRead = true;

        public static bool CheckServer()
        {
           
            bool status = false;
            try
            {
                CWDataServer.CWDataServer server = new CWDataServer.CWDataServer();
                if (server != null)
                {
                    status = true;
                }
            }
            catch (Exception e)
            {
                Logger.WriteLine("Error accessing DataSocketServer: " + e.Message);
            }
            return status;
        }

        public LVDataSocket()
        {
            theSocket = new CWDataSocketClass();
            theSocket.AutoConnect = false;
            theSocket.OnStatusUpdated += new _INIDSCtlEvents_OnStatusUpdatedEventHandler(this.OnStatusUpdated);
            theSocket.OnDataUpdated += new _INIDSCtlEvents_OnDataUpdatedEventHandler(this.OnDataUpdated);
        }


        public override void Connect(string TargetUrl, int accessMode)
        {
            if (manager == null)
            {
                throw new Exception("DataManager has not been assigned");
            }

            if (!(theSocket.Status == CWDSStatus.cwdsUnconnected))
                theSocket.Disconnect();
            // Note 0x20 or 64 stops the zero first entry on read?
            CWDSAccessModes mode = CWDSAccessModes.cwdsRead;
            switch (accessMode)
            {
                case LabDataSource.READ:
                    mode = CWDSAccessModes.cwdsRead;
                    break;
                case LabDataSource.READ_AUTOUPDATE:
                    mode = CWDSAccessModes.cwdsReadAutoUpdate;
                    break;
                case LabDataSource.READ_BUFFERED:
                    mode = CWDSAccessModes.cwdsReadBuffered;
                    break;
                case LabDataSource.READ_BUFFERED_AUTOUPDATE:
                    mode = CWDSAccessModes.cwdsReadBufferedAutoUpdate;
                    break;
                case LabDataSource.WRITE:
                    mode = CWDSAccessModes.cwdsWrite;
                    break;
                case LabDataSource.WRITE_AUTOUPDATE:
                    mode = CWDSAccessModes.cwdsWriteAutoUpdate;
                    break;
                case LabDataSource.READ_WRITE_AUTOUPDATE:
                    mode = CWDSAccessModes.cwdsReadWriteAutoUpdate;
                    break;
                case LabDataSource.READ_WRITE_BUFFERED_AUTOUPDATE:
                    mode = CWDSAccessModes.cwdsReadWriteBufferedAutoUpdate;
                    break;
                default:
                    break;
            }
            this.accessMode = accessMode;
            theSocket.ConnectTo(TargetUrl, mode);
        }

        public override void Update()
        {
            theSocket.Update();
        }

        public override bool Write(object data, int timeout)
        {
            bool status = false;
            if ((accessMode & LabDataSource.WRITE) == LabDataSource.WRITE)
            {
                status = theSocket.SyncWrite(data, timeout);
            }
            return status;
        }

        public override void Disconnect()
        {
            theSocket.Disconnect();
        }

        public override void Dispose()
        {
            if (!(theSocket.Status == CWDSStatus.cwdsUnconnected))
                theSocket.Disconnect();
        }



        private void OnStatusUpdated(int i, int k, string message)
        {
            Console.WriteLine(message);

        }


        private void OnDataUpdated(CWData e)
        {
            Object obj = e.Value;
            if ((accessMode & LabDataSource.READ) == LabDataSource.READ)
            {
                if (firstRead)
                {
                    firstRead = false;
                    if (Convert.ToInt32(e.Value) == 0)
                        return;
                }
                manager.essProxy.AddRecord(manager.experimentID, "", recordType, true, e.Value.ToString(), null);
            }

        }


    }
}
