using System;
using System.Data;
using System.Configuration;
using System.IO;
using System.Web;

using CWDataServer;
using CWDSLib;

using iLabs.DataTypes;
using iLabs.DataTypes.ProcessAgentTypes;
using iLabs.DataTypes.SoapHeaderTypes;
using iLabs.DataTypes.StorageTypes;
using iLabs.DataTypes.TicketingTypes;
using iLabs.LabServer.Interactive;
using iLabs.Proxies.ESS;
using iLabs.Proxies.ISB;

namespace iLabs.LabServer.LabView
{
    /// <summary>
    /// Provide FileWatcher EventHandlers for the BEE Lab. Each instance 
    /// of the class should support one experiment and use a FileDataSource
    /// to interface with the file.
    /// </summary>
    public class BeeEventHandler
    {

        private Coupon opCoupon;
        private long experimentID;
        private string submitter;
        private string socketUrl;
        private string essUrl;
        private string recordType;
        CWDataSocketClass dataSocket;
        //
        // TODO: Add constructor logic here
        //

        public BeeEventHandler()
        {
        }

        public BeeEventHandler(Coupon opCoupon, long experimentId,
            string dsUrl, string essUrl, string recordType, string submitter)
        {
            this.opCoupon = opCoupon;
            this.experimentID = experimentId;
            socketUrl = dsUrl;
            this.essUrl = essUrl;
            this.recordType = recordType;
            this.submitter = submitter;

        }

        void initSocket()
        {
            dataSocket = new CWDataSocketClass();
            dataSocket.URL = socketUrl;
            dataSocket.AccessMode = CWDSAccessModes.cwdsWrite;
        }


        // Define the event handlers.
        public void OnChanged(object source, FileSystemEventArgs e)
        {
            bool ok = false;
            // Specify what is done when a file is changed, created, or deleted.
            switch (e.ChangeType)
            {
                case WatcherChangeTypes.Created:
                    // Console.WriteLine("File Created: " + e.FullPath + " " + e.ChangeType);
                    break;
                case WatcherChangeTypes.Changed:

                    // DateTime lastWrite = File.GetLastWriteTimeUtc(e.FullPath);
                    FileInfo fInfo = new FileInfo(e.FullPath);
                    long len = fInfo.Length;
                    if (len > 0)
                    {
                        ExperimentStorageProxy essProxy = new ExperimentStorageProxy();
                        essProxy.OperationAuthHeaderValue = new OperationAuthHeader();
                        essProxy.OperationAuthHeaderValue.coupon = opCoupon;
                        essProxy.Url = essUrl;
                        if (dataSocket == null)
                            initSocket();
                        dataSocket.Connect();
                        CWDSStatus status = dataSocket.Status;
                        bool dsOK = !(status == CWDSStatus.cwdsConnectionError | status == CWDSStatus.cwdsUnconnected);
                        string[] records = File.ReadAllLines(e.FullPath);
                        using (FileStream inFile = fInfo.Open(FileMode.Truncate)) { }

                        foreach (string record in records)
                        {
                            try
                            {
                                essProxy.AddRecord(experimentID, submitter, recordType, false, record, null);
                            }
                            catch(Exception essEx)
                            {
                                int i = 1;
                            }
                            try
                            {
                                if (dsOK)
                                    dataSocket.SyncWrite(record, 1000);
                            }
                            catch (Exception dsEx)
                            {
                                int j = 1;
                            }
                        }
                        dataSocket.Disconnect();
                    }
                    break;
                case WatcherChangeTypes.Deleted:
                    Console.WriteLine("File: " + e.FullPath + " " + e.ChangeType);
                    break;
            }
        }

    }
}
