using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Services;
using System.Web.Services.Protocols;

using iLabs.Core;
using iLabs.ServiceBroker;
using iLabs.ServiceBroker.DataStorage;
using iLabs.ServiceBroker.Administration;
using iLabs.ServiceBroker.Authorization;
using iLabs.ServiceBroker.Internal;
using iLabs.ServiceBroker;

using iLabs.DataTypes;
using iLabs.DataTypes.BatchTypes;
using iLabs.DataTypes.StorageTypes;
using iLabs.DataTypes.TicketingTypes;
using iLabs.DataTypes.SoapHeaderTypes;
using iLabs.Proxies.BatchLS;
using iLabs.Proxies.ESS;
using iLabs.Ticketing;

namespace iLabs.ServiceBroker.Batch
{
    /// <summary>
    /// Provides an assembly for all Batch specific ServiceBroker API methods and classes
    /// used to isolate the interactive ServiceBroker code from Batch 6.1 dependancies.
    /// </summary>
    public class BatchAPI
    {
        public static ExperimentInformation[] GetExperimentInformation(int[] experimentIDs)
        {
            List<ExperimentInformation> list = new List<ExperimentInformation>();
            try
            {
               long[] expIDs = new long[experimentIDs.Length];
               for (int i = 0; i < experimentIDs.Length; i++)
               {
                   expIDs[i] = (long)experimentIDs[i];
               }

                ExperimentAdminInfo[] expSummaries = InternalDataDB.SelectExperimentAdminInfos(expIDs);

                if (expSummaries != null)
                    foreach (ExperimentAdminInfo expSum in expSummaries)
                    {
                        ExperimentInformation info = new ExperimentInformation();
                        info.experimentID = expSum.experimentID;
                        info.userID = expSum.userID;
                        info.effectiveGroupID = expSum.groupID;
                        info.labServerID = expSum.agentID;
                        info.statusCode = expSum.status & StorageStatus.BATCH_MASK;
                        info.submissionTime = expSum.creationTime;
                        info.completionTime = expSum.closeTime;
                        DateTime endTime = expSum.startTime.AddSeconds(expSum.duration);
                        info.expirationTime = endTime;
                        double hours = new TimeSpan(endTime.Ticks - DateTime.UtcNow.Ticks).TotalHours;
                        info.minTimeToLive = hours > 0 ? hours : 0;
                        info.annotation = expSum.annotation;

                      //Get the Experiment records from the ESS if one is used
                        if (expSum.essID > 0)
                        {

                            // This operation should happen within the Wrapper
                            BrokerDB brokerDB = new BrokerDB();
                            ProcessAgentInfo ess = brokerDB.GetProcessAgentInfo(expSum.essID);
                            Coupon opCoupon = brokerDB.GetEssOpCoupon( expSum.experimentID, TicketTypes.RETRIEVE_RECORDS, 60, ess.agentGuid);
                            
                            ExperimentStorageProxy essProxy = new ExperimentStorageProxy();
                            OperationAuthHeader header = new OperationAuthHeader();
                            header.coupon = opCoupon;
                            essProxy.Url = ess.webServiceUrl;
                            essProxy.OperationAuthHeaderValue = header;
                            Criterion errors = new Criterion("record_type", "like", "*Message");
                            Criterion extensions = new Criterion("record_type", "like", "*Extension");
                            Criterion [] criteria = new Criterion[] {errors,extensions };
                            ExperimentRecord[] records = brokerDB.RetrieveExperimentRecords(expSum.experimentID, criteria);
                            if (records != null)
                            {
                                List<String> valWarnings = new List<String>();
                                List<String> execWarnings = new List<String>();
                                foreach (ExperimentRecord rec in records)
                                {
                                    if (rec.type.CompareTo(BatchRecordType.EXECUTION_ERROR) == 0)
                                    {
                                        info.executionErrorMessage = rec.contents;
                                    }
                                    else if (rec.type.CompareTo(BatchRecordType.VALIDATION_ERROR) == 0)
                                    {
                                        info.validationErrorMessage = rec.contents;
                                    }
                                    else if (rec.type.CompareTo(BatchRecordType.BLOB_EXTENSION) == 0)
                                    {
                                        info.xmlBlobExtension = rec.contents;
                                    }
                                    else if (rec.type.CompareTo(BatchRecordType.RESULT_EXTENSION) == 0)
                                    {
                                        info.xmlResultExtension = rec.contents;
                                    }
                                    else if (rec.type.CompareTo(BatchRecordType.EXECUTION_WARNING) == 0)
                                    {
                                        execWarnings.Add(rec.contents);
                                    }
                                    else if (rec.type.CompareTo(BatchRecordType.VALIDATION_WARNING) == 0)
                                    {
                                        valWarnings.Add(rec.contents);
                                    }

                                }
                                if (execWarnings.Count > 0)
                                {
                                    info.executionWarningMessages = execWarnings.ToArray();
                                }
                                if (valWarnings.Count > 0)
                                {
                                    info.validationWarningMessages = valWarnings.ToArray();
                                }
                            }
                        }
                        list.Add(info);
                    }
            }
            catch
            {
                throw;
            }
            if (list.Count > 0)
            {
                return list.ToArray();
            }
            else
                return null;
        }

        /// <summary>
        /// Retrieves an experiment's ResultReport from the ESS
        /// </summary>
        /// <param name="experimentID"></param>
        /// <param name="roles"></param>
        /// <returns>THe ResultStatus or an empty report with status set, when the experiment has not terminated that have not</returns>
        public static ResultReport GetResultReport(int experimentID)
        {
            ResultReport report = null;
            BrokerDB brokerDB = new BrokerDB();
            try
            {
                ExperimentAdminInfo expInfo = InternalDataDB.SelectExperimentAdminInfo(experimentID);
                if (expInfo == null || expInfo.experimentID <= 0)
                {
                    //experiment does not exist
                    throw new SoapException("Invalid experiment ID. ", SoapException.ServerFaultCode);
                }
                else
                {
                    ProcessAgentInfo ess = brokerDB.GetProcessAgentInfo(expInfo.essID);
                    if(ess.retired){
                        throw new Exception("The requested ESS has been retired");
                    }
                    ExperimentStorageProxy essProxy = new ExperimentStorageProxy();
                    essProxy.AgentAuthHeaderValue = new AgentAuthHeader();
                    essProxy.AgentAuthHeaderValue.agentGuid = ProcessAgentDB.ServiceGuid;
                    essProxy.AgentAuthHeaderValue.coupon = ess.identOut;
                    essProxy.Url = ess.webServiceUrl;

                    Coupon opCoupon = brokerDB.GetEssOpCoupon(expInfo.experimentID, TicketTypes.RETRIEVE_RECORDS, 60, ess.agentGuid);
                    OperationAuthHeader opHeader = new OperationAuthHeader();
                    opHeader.coupon = opCoupon;
                    essProxy.OperationAuthHeaderValue = opHeader;
                    if ((expInfo.status & StorageStatus.CLOSED) == 0)
                    {
                        ProcessAgentInfo lsInfo = brokerDB.GetProcessAgentInfo(expInfo.agentID);
                        if (lsInfo != null)
                        {
                            if(lsInfo.retired){
                                throw new Exception("The requested batch LabServer has ben retired.");
                            }
                            BatchLSProxy batchLS_Proxy = new BatchLSProxy();
                            batchLS_Proxy.AuthHeaderValue = new AuthHeader();
                            batchLS_Proxy.AuthHeaderValue.identifier = ProcessAgentDB.ServiceGuid;
                            batchLS_Proxy.AuthHeaderValue.passKey = lsInfo.identOut.passkey;
                            batchLS_Proxy.Url = lsInfo.webServiceUrl;
                            // retrieve resultReport from labServer

                            LabExperimentStatus expStatus = batchLS_Proxy.GetExperimentStatus(experimentID);
                            if (expStatus != null)
                            {
                                if ((expStatus.statusReport.statusCode >= 3) && (expStatus.statusReport.statusCode != 6))
                                {
                                    report = batchLS_Proxy.RetrieveResult(experimentID);
                                        if (report != null)
                                        {
                                            ExperimentRecord theRecord = null;
                                            List<ExperimentRecord> recordList = new List<ExperimentRecord>();
                                            if (report.experimentResults != null && report.experimentResults.Length > 0)
                                            {
                                                theRecord = new ExperimentRecord();
                                                theRecord.submitter = lsInfo.agentGuid;
                                                theRecord.type = BatchRecordType.RESULT;
                                                theRecord.contents = report.experimentResults;
                                                theRecord.xmlSearchable = false;
                                                recordList.Add(theRecord);
                                            }
                                            if (report.errorMessage != null && report.errorMessage.Length > 0)
                                            {
                                                theRecord = new ExperimentRecord();
                                                theRecord.submitter = lsInfo.agentGuid;
                                                theRecord.type = BatchRecordType.EXECUTION_ERROR;
                                                theRecord.contents = report.errorMessage;
                                                theRecord.xmlSearchable = false;
                                                recordList.Add(theRecord);
                                            }
                                            if (report.warningMessages != null && report.warningMessages.Length > 0)
                                            {
                                                foreach (string s in report.warningMessages)
                                                {
                                                    if (s.Length > 0)
                                                    {
                                                        theRecord = new ExperimentRecord();
                                                        theRecord.submitter = lsInfo.agentGuid;
                                                        theRecord.type = BatchRecordType.EXECUTION_WARNING;
                                                        theRecord.contents = s;
                                                        theRecord.xmlSearchable = false;
                                                        recordList.Add(theRecord);
                                                    }
                                                }
                                            }
                                            if (report.xmlResultExtension != null && report.xmlResultExtension.Length > 0)
                                            {
                                                theRecord = new ExperimentRecord();
                                                theRecord.submitter = lsInfo.agentGuid;
                                                theRecord.type = BatchRecordType.RESULT_EXTENSION;
                                                theRecord.contents = report.xmlResultExtension;
                                                theRecord.xmlSearchable = true;
                                                recordList.Add(theRecord);
                                            }
                                            if (report.xmlBlobExtension != null && report.xmlBlobExtension.Length > 0)
                                            {
                                                theRecord = new ExperimentRecord();
                                                theRecord.submitter = lsInfo.agentGuid;
                                                theRecord.type = BatchRecordType.BLOB_EXTENSION;
                                                theRecord.contents = report.xmlBlobExtension;
                                                theRecord.xmlSearchable = true;
                                                recordList.Add(theRecord);
                                            }
                                            if (recordList.Count > 0)
                                            {
                                                essProxy.AddRecords(experimentID, recordList.ToArray());
                                            }
                                            StorageStatus sStatus = essProxy.SetExperimentStatus(experimentID, report.statusCode | StorageStatus.CLOSED);
                                            DataStorageAPI.UpdateExperimentStatus(sStatus);
                                        }
                                    }
                                }
                            
                        }
                    }
                    else
                    {
                        report = new ResultReport();
                        ExperimentRecord[] records = essProxy.GetRecords(experimentID, null);
                        if (records != null)
                        {
                            List<String> execWarnings = new List<String>();
                            foreach (ExperimentRecord rec in records)
                            {
                                if (rec.type.CompareTo(BatchRecordType.EXECUTION_ERROR) == 0)
                                {
                                    report.errorMessage = rec.contents;
                                }

                                else if (rec.type.CompareTo(BatchRecordType.BLOB_EXTENSION) == 0)
                                {
                                    report.xmlBlobExtension = rec.contents;
                                }
                                else if (rec.type.CompareTo(BatchRecordType.RESULT_EXTENSION) == 0)
                                {
                                    report.xmlResultExtension = rec.contents;
                                }
                                else if (rec.type.CompareTo(BatchRecordType.EXECUTION_WARNING) == 0)
                                {
                                    execWarnings.Add(rec.contents);
                                }
                                else if (rec.type.CompareTo(BatchRecordType.RESULT) == 0)
                                {
                                    report.experimentResults = rec.contents;
                                }

                            }
                            if (execWarnings.Count > 0)
                            {
                                report.warningMessages = execWarnings.ToArray();
                            }
                        }
                        report.statusCode = expInfo.status & StorageStatus.BATCH_MASK;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new SoapException(ex.Message + ". " + ex.GetBaseException(), SoapException.ServerFaultCode, ex);
            }
            return report;
        }


        //public static ResultReport GetExperimentResultReport(long experimentID, long couponID)
        //{
        //    ProcessAgentInfo lsInfo = brokerDB.GetProcessAgentInfoByInCoupon(sbHeader.couponID, ProcessAgentDB.ServiceGuid);
        //    ExperimentAdminInfo expInfo = InternalDataDB.SelectExperimentAdminInfo(experimentID);

        //    if ((lsInfo != null && expInfo != null) && lsInfo.agentId == expInfo.agentID)
        //    {
        //        batchLS_Proxy.AuthHeaderValue = new AuthHeader();
        //        batchLS_Proxy.AuthHeaderValue.identifier = ProcessAgentDB.ServiceGuid;
        //        batchLS_Proxy.AuthHeaderValue.passKey = lsInfo.identOut.passkey;
        //        batchLS_Proxy.Url = lsInfo.webServiceUrl;
        //        // retrieve resultReport from labServer

        //        report = batchLS_Proxy.RetrieveResult(experimentID);
        //        if (report != null)
        //        {
        //            if (report.statusCode < 3)
        //            {
        //                //Experiment is waiting or running, this should not happen here
        //                return;
        //            }
        //            else
        //            {
        //                ProcessAgentInfo essInfo = brokerDB.GetProcessAgentInfo(expInfo.agentID);
        //                ExperimentStorageProxy essProxy = new ExperimentStorageProxy();
        //                essProxy.AgentAuthHeaderValue.agentGuid = ProcessAgentDB.ServiceGuid;
        //                essProxy.AgentAuthHeaderValue.coupon = essInfo.identOut;
        //                essProxy.Url = essInfo.webServiceUrl;
        //            }

                       
        //    }
        //}

        //public static string RetrieveExperimentResult(int experimentID)
        //{
        //    return null;
        //}
        //public static string RetrieveLabConfiguration(int experimentID)
        //{
        //    return null;
        //}
        //public static string RetrieveExperimentSpecification(int experimentID)
        //{
        //    return null;
        //}


    }
}
