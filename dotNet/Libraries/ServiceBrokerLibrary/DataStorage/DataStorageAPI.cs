using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;
using System.Xml;

using iLabs.Core;
using iLabs.DataTypes;
using iLabs.DataTypes.BatchTypes;
using iLabs.DataTypes.StorageTypes;
using iLabs.DataTypes.SoapHeaderTypes;
using iLabs.DataTypes.TicketingTypes;

using iLabs.ServiceBroker;
using iLabs.ServiceBroker.Authorization;
using iLabs.ServiceBroker.Internal;
using iLabs.Ticketing;
using iLabs.Proxies.ESS;
using iLabs.UtilLib;

namespace iLabs.ServiceBroker.DataStorage
{

 
    public class DataStorageAPI
    {
         /// <summary>
        /// Closes an Experiment in the ServiceBroker database
        /// </summary>
        /// <param name="experimentID">the ID of the Experiment to be closed</param>
        /// <returns>true if the Experiment was successfully closed</returns>
        public static bool CloseExperiment(long experimentID, int status)
        {
            return InternalDataDB.CloseExperiment(experimentID, status);
        }

        public static string getStatusString(int status)
        {
            return StorageStatus.GetStatusString(status);
        }


        public static StorageStatus RetrieveExperimentStatus(long experimentId)
        {
            ExperimentSummary summary = RetrieveExperimentSummary(experimentId);
            if (summary != null)
            {
                StorageStatus status = new StorageStatus();
                status.experimentId = experimentId;
                status.closeTime = summary.closeTime;
                status.creationTime = summary.creationTime;
                status.experimentId = experimentId;
                status.issuerGuid = summary.serviceBrokerGuid;
                status.recordCount = summary.recordCount;
                status.status = summary.status;
                return status;

            }
            else
            {
                return null;
            }


        }
        public static ExperimentSummary RetrieveExperimentSummary(long experimentID)
        {
            ExperimentSummary summary = InternalDataDB.SelectExperimentSummary(experimentID);
            if (summary.HasEss && ((summary.status | StorageStatus.CLOSED) == 0))
            {
                ProcessAgentDB ticketing = new ProcessAgentDB();
                // Retrieve the ESS Status info and update as needed
                //This uses a generic ReadRecords ticket created for the ESS
                ProcessAgentInfo ess = ticketing.GetProcessAgentInfo(summary.essGuid);
                if ((ess != null) && !ess.retired)
                {
                    ExperimentStorageProxy essProxy = new ExperimentStorageProxy();
                    essProxy.Url = ess.webServiceUrl;
                    essProxy.OperationAuthHeaderValue = new OperationAuthHeader();
                    essProxy.OperationAuthHeaderValue.coupon = ess.identOut;
                    StorageStatus status = essProxy.GetExperimentStatus(experimentID);
                    bool needsUpdate = false;
                    if (status != null)
                    {
                        if (summary.closeTime != status.closeTime)
                        {
                            summary.closeTime = status.closeTime;
                            needsUpdate = true;
                        }
                        if (summary.recordCount != status.recordCount)
                        {
                            summary.recordCount = status.recordCount;
                            needsUpdate = true;
                        }
                        if (summary.status != status.status)
                        {
                            summary.status = status.status;
                            needsUpdate = true;
                        }
                    }
                    if (needsUpdate)
                    {
                        InternalDataDB.UpdateExperimentStatus(status);
                    }
                }
            }
            return summary;
        }

        public static ExperimentSummary [] RetrieveExperimentSummaries(long []experimentIDs)
        {
             ExperimentSummary[] summaries = InternalDataDB.SelectExperimentSummaries(experimentIDs);
             for (int i = 0; i < summaries.Length; i++)
             {
                 if (summaries[i].HasEss && ((summaries[i].status | StorageStatus.CLOSED) == 0))
                 {
                     ProcessAgentDB ticketing = new ProcessAgentDB();
                     // Retrieve the ESS Status info and update as needed
                     //This uses a generic ReadRecords ticket created for the ESS
                     ProcessAgentInfo ess = ticketing.GetProcessAgentInfo(summaries[i].essGuid);
                     if ((ess != null) && !ess.retired)
                     {
                         ExperimentStorageProxy essProxy = new ExperimentStorageProxy();
                         essProxy.Url = ess.webServiceUrl;
                         essProxy.OperationAuthHeaderValue = new OperationAuthHeader();
                         essProxy.OperationAuthHeaderValue.coupon = ess.identOut;
                         StorageStatus status = essProxy.GetExperimentStatus(summaries[i].experimentId);
                         bool needsUpdate = false;
                         if (status != null)
                         {
                             if (summaries[i].closeTime != status.closeTime)
                             {
                                 summaries[i].closeTime = status.closeTime;
                                 needsUpdate = true;
                             }
                             if (summaries[i].recordCount != status.recordCount)
                             {
                                 summaries[i].recordCount = status.recordCount;
                                 needsUpdate = true;
                             }
                             if (summaries[i].status != status.status)
                             {
                                 summaries[i].status = status.status;
                                 needsUpdate = true;
                             }
                         }
                         if (needsUpdate)
                         {
                             InternalDataDB.UpdateExperimentStatus(status);
                         }
                     }
                 }
             }
             return summaries;
        }

        public static bool UpdateExperimentStatus(long experimentID, int statusCode)
        {
            return InternalDataDB.UpdateExperimentStatus(experimentID, statusCode);
        }

        public static bool UpdateExperimentStatus(StorageStatus status)
        {
            return InternalDataDB.UpdateExperimentStatus(status);
        }
   
        public static LongTag[] RetrieveExperimentTags(long[] experimentIDs, int userTZ, CultureInfo culture)
        {
            return RetrieveExperimentTags(experimentIDs, userTZ, culture, false, false, true, true, true, false, true, true);
        }

        public static LongTag[] RetrieveExperimentTags(long[] experimentIDs, int userTZ, CultureInfo culture,
            bool showUser, bool showGroup, bool showClient, bool showStatus,
            bool showStart, bool showClose, bool showAnnotation, bool showID)
        {
            
            ExperimentSummary [] exp = RetrieveExperimentSummaries(experimentIDs);
            LongTag [] tags = new LongTag[exp.Length];
           
            for (int i = 0; i < exp.Length; i++)
            {
                StringBuilder buf = new StringBuilder();
          
                tags[i] = new LongTag();
                tags[i].id = exp[i].experimentId;

                if (showID)
                        {
                            buf.Append(exp[i].experimentId.ToString("0000") + " ");
                        }
                        //Create
                        if (showStart)
                        {
                            buf.Append(DateUtil.ToUserTime(exp[i].creationTime, culture, userTZ) + " ");
                        }
                        //Close
                        if (showClose)
                        {
                            if (exp[i].closeTime != null)
                            {
                                buf.Append(DateUtil.ToUserTime(exp[i].closeTime, culture, userTZ) + " ");
                            }
                            else
                            {
                                buf.Append("Experiment Not Closed ");
                            }
                        }
                        
                        // User
                        if (showUser)
                        {
                          if (exp[i].userName != null)
                            {
                                buf.Append(exp[i].userName + " ");
                            }
                        }
                        //  Group
                        if (showGroup)
                        {
                            if (exp[i].groupName != null)
                            {
                                buf.Append(exp[i].groupName + " ");
                            }
                        }
                        //Client
                        if (showClient)
                        {
                            if (exp[i].clientName != null)
                            {
                                buf.Append(exp[i].clientName + " ");
                            }
                        }
                        // Status
                        if (showStatus)
                        {
                                buf.Append(exp[i].status.ToString("000") + " ");
                        }
                       
                        //Annotation
                        if (showAnnotation)
                        {
                            if (exp[i].annotation != null)
                            {
                                buf.Append(exp[i].annotation);
                            }
                        }
                    tags[i].tag = buf.ToString();
            }
                    
                
            return tags;
        }
        /// <summary>
        /// Deletes a single experiment, checks for ESS, deletes from the ESS if needed, deletes Qualifier and the experiment administrative info.
        /// This does not check for authorization the experiment ID should be authorized for deletion before this is called.
        /// </summary>
        /// <param name="experimentID"></param>
        /// <returns></returns>
        public static bool DeleteExperiment(long experimentID)
        {
            bool deleted = false;
             ExperimentSummary summary = InternalDataDB.SelectExperimentSummary(experimentID);
             if (summary.HasEss)
             {
                 ProcessAgentDB ticketing = new ProcessAgentDB();
                 // Retrieve the ESS Status info and update as needed
                 //This uses a generic ReadRecords ticket created for the ESS
                 ProcessAgentInfo ess = ticketing.GetProcessAgentInfo(summary.essGuid);
                 if ((ess != null) && !ess.retired)
                 {
                     ExperimentStorageProxy essProxy = new ExperimentStorageProxy();
                     essProxy.Url = ess.webServiceUrl;
                     essProxy.AgentAuthHeaderValue = new AgentAuthHeader();
                     essProxy.AgentAuthHeaderValue.coupon = ess.identOut;
                     essProxy.AgentAuthHeaderValue.agentGuid = ProcessAgentDB.ServiceGuid;
                     essProxy.DeleteExperiment(experimentID);
                 }
             }
            int qualID = Authorization.AuthorizationAPI.GetQualifierID((int)experimentID, Qualifier.experimentQualifierTypeID);
            if (qualID > 0)
                Authorization.AuthorizationAPI.RemoveQualifiers(new int[] { qualID });
            deleted = InternalDataDB.DeleteExperiment(experimentID);
            return deleted;
        }

        public static int DeleteExperimentCoupon(long experimentID, long couponID)
        {
            return InternalDataDB.DeleteExperimentCoupon(experimentID, couponID);
        }
        public static void InsertExperimentCoupon(long experimentID, long couponID)
        {
            InternalDataDB.InsertExperimentCoupon(experimentID, couponID);
        }
        public static long [] RetrieveExperimentCouponIDs(long experimentID)
        {
            long [] coupons = InternalDataDB.RetrieveExperimentCouponIDs(experimentID);
            return coupons;
        }
                     
        public static long[] RetrieveAuthorizedExpIDs(int userID, int groupID, Criterion[] carray)
        {
            long[] expIDs = null;
            List<Criterion> sbList = new List<Criterion>();
            List<Criterion> essList = new List<Criterion>();
           

            if (carray != null && carray.Length > 0)
            {
                // Parse the criterion
                for (int i = 0; i < carray.Length; i++)
                {
                    switch (carray[i].attribute.ToLower())
                    {
                        // ServiceBroker Experiment fields
                        // Actual SB experiment column names
                        case "agent_id":    
                        case "annotation":
                        case "client_id":
                        case "creationtime":
                        case "duration":
                        case "ess_id":
                        case "experiment_id":
                        case "group_id":
                        case "labsession_id":
                        case "scheduledstart":
                        case "status":
                        case "user_id":
                        //these criterion are based on external values, processed by InternalDB methods.
                        case "username":
                        case "groupname":
                        case "clientguid":
                        case "clientname":
                        case "labservername":
                            sbList.Add(carray[i]);
                            break;
                        // Rename the Criterion
                        case "start":
                            sbList.Add(new Criterion("creationtime", carray[i].predicate, carray[i].value));
                            break;
                        // ESS targets
                        case "record_count":
                        case "record_type": // Individual record criterion send to ESS
                        case "contents":                            
                        default: // any unhandled attributes are record attributes
                            essList.Add(carray[i]);
                            break;
                    }
                } //parsing of Criterion done
            }
            if (sbList.Count == 0 && essList.Count == 0)
            {
                // No search items - Get all experiments allowed
                expIDs = InternalDataDB.RetrieveAuthorizedExperimentIDs(userID, groupID);
            }
            else
            { // Query SB database to find all possible Experiments and related ESS's
                //As there are criteria only experiments with hits will be returned
                bool hasEssCriteria = (essList.Count > 0);
                 List<long> workExp = new List<long>();
                 Hashtable essLists = null;

                // DataSet contains all experimentID and ess ids that pass SB criteria,
                // and a set of all ess_ids for the authorized experiments
                DataSet results = InternalDataDB.RetrieveExperimentIDsCriteria(userID, groupID, sbList.ToArray());
                if (results != null)
                {
                    if (hasEssCriteria)
                    {
                        
                        DataTable essids = results.Tables["ess"];
                        if (essids != null && essids.Rows != null && essids.Rows.Count > 0)
                        {
                            essLists = new Hashtable();
                            foreach (DataRow er in essids.Rows)
                            {
                                if (er[0] != DBNull.Value)
                                {
                                    List<Int64> exps = new List<Int64>();
                                    essLists.Add(Convert.ToInt32(er[0]), exps);
                                }
                            }
                        }
                    }
                    DataTable hits = results.Tables["sbHits"];
                    if (hits != null)
                    {
                        // Add SB hits to list
                        foreach (DataRow r in hits.Rows)
                        {
                            workExp.Add(Convert.ToInt64(r[0]));
                            if (hasEssCriteria)
                            {
                                if (r[1] != DBNull.Value)
                                {
                                    ((List<Int64>)essLists[Convert.ToInt32(r[1])]).Add(Convert.ToInt64(r[0]));
                                }
                            }
                        }
                    }
                    if (hasEssCriteria)
                    { // Have ESS criteria, use the workList and further filter
                        List<Int64> essHits = new List<Int64>();
                        BrokerDB brokerDB = new BrokerDB();

                        //Process ess criteria
                        foreach (object obj in essLists.Keys)
                        {
                            List<Int64> essExps = (List<Int64>)essLists[obj];
                            if (essExps.Count > 0)
                            {
                                int essId = Convert.ToInt32(obj);

                                ProcessAgentInfo info = brokerDB.GetProcessAgentInfo(essId);
                                if ((info != null) && !info.retired)
                                {
                                    ExperimentStorageProxy essProxy = new ExperimentStorageProxy();
                                    AgentAuthHeader authHeader = new AgentAuthHeader();
                                    authHeader.agentGuid = ProcessAgentDB.ServiceGuid;
                                    authHeader.coupon = info.identOut;
                                    essProxy.AgentAuthHeaderValue = authHeader;
                                    essProxy.Url = info.webServiceUrl;
                                    long[] essExpids = essProxy.GetExperimentIDs(essExps.ToArray(), essList.ToArray());
                                    if (essExpids != null && essExpids.Length > 0)
                                    {
                                        foreach (long e in essExpids)
                                        {
                                            essHits.Add(e);
                                        }
                                    }
                                }
                            }
                        }// End of ESS processing
                        expIDs = essHits.ToArray();
                    }
                    else
                    {
                        expIDs = workExp.ToArray();
                    }
                }
               
            }
            return expIDs;
        }

         /// <summary>
        /// Retrieve experiment IDs that match the intersection of the tree arguments.
        /// Values less than or equal to 0 are not part of the query
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="groupID"></param>
        /// <param name="clientID"></param>
        /// <returns></returns>
        public static long[] RetrieveExperimentIDs(int userID, int groupID, int clientID)
        {
            return InternalDataDB.RetrieveExperimentIDs(userID, groupID, clientID);
        }

        public static int SaveExperimentRecord(long experimentID, string submitter, string recordType,
             bool xmlSearchable, string contents, RecordAttribute[] attributes)
        {
            return InternalDataDB.SaveExperimentRecord(experimentID, submitter, recordType,
              xmlSearchable, contents, attributes);
        }
               


        ///*********************** CLIENT ITEMS **************************///
		

		/// <summary>
		/// Saves the value of a client data item for the specified client and user.
		/// </summary>
		/// <param name="clientID">The ID of the client implementation.</param>
		/// <param name="userID">The ID of the user.</param>
		/// <param name="itemName">The name under which the data item is to be saved.</param>
		/// <param name="itemValue">The value to be saved.</param>
		public static void SaveClientItemValue(int clientID, int userID, string itemName, string itemValue)
		{
			try
			{
                InternalDataDB.SaveClientItem(clientID, userID, itemName, itemValue);
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		/// <summary>
		/// Gets the value of the specified client data items for the designed client and user.
		/// </summary>
		/// <param name="clientID">The name of the client implementation.</param>
		/// <param name="userID">The ID of the user.</param>
		/// <param name="itemNames">An array of item names for the data items to be retrieved.</param>
		/// <returns>The current values of the data items specified in itemNames (and in the same order); if an item named in itemNames is not recognized, a null will be returned at that position in the array.</returns>
		public static string[] GetClientItemValue(int clientID, int userID, string[] itemNames)
		{
			string[] itemValues = new string[itemNames.Length ];

			for(int i = 0; i < itemNames.Length ; i++)
			{
                itemValues[i] = InternalDataDB.SelectClientItemValue(clientID, userID, itemNames[i]);
			}
			return itemValues;
		}

		/// <summary>
		/// Deletes client data items and their values for the specified client and user.
		/// </summary>
		/// <param name="clientID">The ID of the client implementation.</param>
		/// <param name="userID">The ID of the user.</param>
		/// <param name="itemNames">The array of names of items to be removed.</param>
		/// <returns>An array of item names that does not exist for the specified client and user.</returns>
		public static string[] RemoveClientItems(int clientID, int userID, string[] itemNames)
		{
			ArrayList aList = new ArrayList ();
			for (int i=0; i<itemNames.Length ; i++)
			{
				try
				{
                    InternalDataDB.DeleteClientItem(clientID, userID, itemNames[i]);
				}
				catch (Exception ex)
				{
					//aList.Add(itemNames[i]);
					throw;
				}
			}
			string[]  unRemovedItemNames = Utilities.ArrayListToStringArray(aList);
		
			return unRemovedItemNames;
		}

		/// <summary>
		/// List the names of all cient data items for the given client and user.
		/// </summary>
		/// <param name="clientID">The ID of the client implementation.</param>
		/// <param name="userID">The ID of the user.</param>
		/// <returns>The array of string item names for the given client and user.</returns>
		public static string[] ListClientItems(int clientID, int userID)
		{
            return InternalDataDB.SelectClientItems(clientID, userID);
		}


    }
}
