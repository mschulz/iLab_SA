<%@ WebService Language="C#" Class="iLabs.LabServer.TimeOfDay.TimeOfDayWebService" %>

using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;

using System.IO;
using System.Data;
using System.Configuration;
using System.Xml;
using System.Xml.Serialization;

using iLabs.Core;
using iLabs.DataTypes;
using iLabs.DataTypes.ProcessAgentTypes;
using iLabs.DataTypes.StorageTypes;
using iLabs.DataTypes.SoapHeaderTypes;
using iLabs.DataTypes.TicketingTypes;
using iLabs.UtilLib;
using iLabs.Ticketing;
using iLabs.Proxies.PAgent;
using iLabs.Proxies.ESS;
using iLabs.Web;


namespace iLabs.LabServer.TimeOfDay
{
    [WebService(Namespace = "http://ilab.mit.edu/iLabs/Services")]
    [XmlType(Namespace = "http://ilab.mit.edu/iLabs/type")]
    [WebServiceBinding(Name = "TimeOfDayWebService", Namespace = "http://ilab.mit.edu/iLabs/Services")]
    [WebServiceBinding(Name = "IProcessAgent", Namespace = "http://ilab.mit.edu/iLabs/Services")]
    [WebServiceBinding(Name = "ITimeOfDay", Namespace = "http://ilab.mit.edu/iLabs/Services")]
    public class TimeOfDayWebService : WS_ILabCore
    {
        public static long experimentId = -1;
        public static long blobId = -1;

        public string essUrl = null;

        public OperationAuthHeader opHeader = new OperationAuthHeader();
        ExperimentStorageProxy essInterface = new ExperimentStorageProxy();
        ProcessAgentDB dbTicketing = new ProcessAgentDB();

        [WebMethod(EnableSession=true)]
        [SoapDocumentMethod(Binding = "ITimeOfDay")]
        [SoapHeader("opHeader", Direction = SoapHeaderDirection.In)]
        public long GetExperimentId()
        {
            return experimentId;
        }

        [WebMethod(EnableSession = true)]
        [SoapDocumentMethod(Binding = "ITimeOfDay")]
        [SoapHeader("opHeader", Direction = SoapHeaderDirection.In)]
        public long GetBlobId()
        {
            return blobId;
        }

        [WebMethod(EnableSession = true)]
        [SoapDocumentMethod(Binding = "ITimeOfDay")]
        [SoapHeader("opHeader", Direction=SoapHeaderDirection.In)]
        public string ReturnTimeOfDay()
        {
            Coupon opCoupon = new Coupon(opHeader.coupon.issuerGuid, opHeader.coupon.couponId,
                 opHeader.coupon.passkey);

            string ticketType = TicketTypes.EXECUTE_EXPERIMENT;
            //string processAgentType = ProcessAgentType.LAB_SERVER;

            try
            {
                Ticket retrievedTicket = dbTicketing.RetrieveAndVerify(opCoupon, ticketType);
                
                XmlDocument payload = new XmlDocument();
                payload.LoadXml(retrievedTicket.payload);
                essUrl = payload.GetElementsByTagName("essWebAddress")[0].InnerText;

                essInterface.Url = essUrl;
                
                experimentId = Int64.Parse(payload.GetElementsByTagName("experimentID")[0].InnerText);
                
                string time = DateTime.Now.ToString();

                StoreTimeOfDay(opCoupon, time);
                
                return time;
            }

            catch
            {
                throw;
            }

        }
        
        ///////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////


        private int CreateExperiment(Coupon opCoupon, long duration)
        {
            int status = 1;
            //try
            //{
            //    //opHeader.coupon = opCoupon;
                
            //    //expStorage.OperationAuthHeaderValue = opHeader;
            //    //essInterface.OperationAuthHeaderValue = opHeader;
                
            //    //long experimentID = expStorage.CreateExperiment(duration);
            //    //StorageStatus status = essInterface.OpenExperiment(experimentId, duration);
                
            //}

            //catch (Exception ex)
            //{
            //    throw new Exception("Could not create experiment on ESS", ex);
            //}
            return status;
        }

        private int AddRecord(Coupon opCoupon, long experimentID, string type, bool xmlSearchable, string contents,
            RecordAttribute[] attributes)
        {       
            try
            {
                opHeader.coupon = opCoupon;
                //expStorage.OperationAuthHeaderValue = opHeader;
                essInterface.OperationAuthHeaderValue = opHeader;
                
                //int seqNo = expStorage.AddRecord(experimentID,
                //       type, xmlSearchable, contents, attributes);
                int seqNo = essInterface.AddRecord(experimentID,"TOD",
                       type, xmlSearchable, contents, attributes);
                return seqNo;
            }

            catch (Exception ex)
            {
                throw new Exception("Could not add record to experiment on ESS", ex);
            }
        }

        private long CreateBlob(Coupon opCoupon, long experimentID)
        {
                        
            try
            {

                
                string imageBase = ConfigurationManager.AppSettings["imageBaseName"];

                string relativePath = "/images/" + imageBase + ".jpg";
                string filePath = Server.MapPath("images") + "\\" + imageBase + ".jpg";


                //string[] essSupportedChecksumAlgorithms = blobStorage.GetSupportedChecksumAlgorithms();
                //string checksumAlgorithm = ConfigurationManager.AppSettings["checksumAlgorithms"];

                //bool supported = false;

                //foreach (string s in essSupportedChecksumAlgorithms)
                //{
                //    if (String.Compare(s, checksumAlgorithm, true) == 0)
                //        supported = true;
                //}

                //if (!supported)
                //{
                //    lblResponse.Text = "CREATE BLOB - Could not create BLOB: Checksum Algorithm ("
                //        + checksumAlgorithm + ") not supported by the ESS";
                //    return;
                //}

                string checksum = ChecksumUtil.ComputeMD5(filePath);
                string checksumAlgorithm = "md5";

                FileInfo fi = new FileInfo(filePath);
                int byteCount = (int)(fi.Length);

                string description = imageBase + " image";

                opHeader.coupon = opCoupon;
                //blobStorage.OperationAuthHeaderValue = opHeader;
                essInterface.OperationAuthHeaderValue = opHeader;

                //long blobId = blobStorage.CreateBlob(experimentID, description, byteCount,
                //    checksum, checksumAlgorithm);
                long blobId = essInterface.CreateBlob(experimentID, description, byteCount,
                     checksum, checksumAlgorithm);

                return blobId;
            }

            catch (Exception ex)
            {
                throw new Exception("Could not create Blob on ESS", ex);
            }
        }

        private bool AddBlobToRecord(Coupon opCoupon, long blobId, long experimentId, int seqNo)
        {            
            try
            {
                opHeader.coupon = opCoupon;
                //blobStorage.OperationAuthHeaderValue = opHeader;
                essInterface.OperationAuthHeaderValue = opHeader;
                
                //bool isAdded = blobStorage.AddBlobToRecord(blobId, experimentId, seqNo);
                bool isAdded = essInterface.AddBlobToRecord(blobId, experimentId, seqNo);
                return isAdded;
            }

            catch (Exception ex)
            {
                throw new Exception("could not add blob to record", ex);
            }
        }


        private bool StoreBlob(Coupon opCoupon, long blobId)
        {
            string imageBase = ConfigurationManager.AppSettings["imageBaseName"];

            string relativePath = "/images/" + imageBase + ".jpg";

            string blobUrl = ProcessAgentDB.ServiceAgent.codeBaseUrl
                + relativePath;

            try
            {
                opHeader.coupon = opCoupon;
                //blobStorage.OperationAuthHeaderValue = opHeader;
                essInterface.OperationAuthHeaderValue = opHeader;
                                
                //bool isStored = blobStorage.RequestBlobStorageTest(blobId, blobUrl);
                bool isStored = essInterface.RequestBlobStorage(blobId, blobUrl);
                
                return isStored;
            }

            catch (Exception ex)
            {
                throw new Exception("could not store blob on ESS", ex);
            }

        }



        private void StoreTimeOfDay(Coupon opCoupon, string time)
        {
            //Create Experiment on ESS
            long expDuration = 180;
            CreateExperiment(opCoupon, expDuration);

            //Add the result record to the ESS
            int seqNo = AddRecord(opCoupon, experimentId, "Experiment Result", false, time, null);

            //Store a Blob = 1.add new record to experiment 2. create blob 3. associate blob to record 4. store data
            StoreTimeOfDayBlob(opCoupon, experimentId);

        }

        private void StoreTimeOfDayBlob(Coupon opCoupon, long experimentId)
        {
            //Add a "BLOB" record to the ESS
            int seqNo = AddRecord(opCoupon, experimentId, "Image", false, "Binary Data", null);

            //Create a BLOB on the ESS
            blobId = CreateBlob(opCoupon, experimentId);

            //Associate the blob to the record create above
            bool isAdded = AddBlobToRecord(opCoupon, blobId, experimentId, seqNo);

            //Store the actual binary data in the ESS database
            bool isStored = StoreBlob(opCoupon, blobId);
        }


    }
}

