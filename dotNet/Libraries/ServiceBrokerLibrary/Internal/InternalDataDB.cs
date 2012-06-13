using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text;

using iLabs.Core;
using iLabs.DataTypes;
using iLabs.DataTypes.BatchTypes;
using iLabs.DataTypes.StorageTypes;
using iLabs.DataTypes.SoapHeaderTypes;
using iLabs.DataTypes.TicketingTypes;

using iLabs.ServiceBroker.Administration;
using iLabs.ServiceBroker.Authorization;
using iLabs.ServiceBroker.DataStorage;
using iLabs.ServiceBroker;
using iLabs.Ticketing;
using iLabs.UtilLib;
using iLabs.Proxies.ESS;


namespace iLabs.ServiceBroker.Internal
{
    public class InternalDataDB
    {
        public static DateTime MinDbDateTime = new DateTime(1800, 1, 1);

        /// <summary>
        /// Closes an Experiment in the ServiceBroker database
        /// </summary>
        /// <param name="experimentID">the ID of the Experiment to be closed</param>
        /// <returns>true if the Experiment was successfully deleted</returns>
        public static bool CloseExperiment(long experimentID, int status)
        {
            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("Experiment_Close", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.Parameters.Add(FactoryDB.CreateParameter("@experimentID", experimentID,DbType.Int64));
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@status", status, DbType.Int32));

            try
            {
                myConnection.Open();
                int rows = myCommand.ExecuteNonQuery();

                return (rows != -1);

                //alternatively
                //return true;
            }
            catch (Exception ex)
            {
                //return false;
                throw new Exception("Exception thrown deleting experiment", ex);
            }
            finally
            {
                myConnection.Close();
            }
        }


        /// <summary>
        /// deletes an Experiment object and all its associated ExperimentRecords and BLOBs on the ESS
        /// </summary>
        /// <param name="experimentID">the ID of the Experiment to be deleted</param>
        /// <returns>true if the Experiment was successfully deleted</returns>
        public static bool DeleteExperiment(long experimentID)
        {
            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("Experiment_Delete", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.Parameters.Add(FactoryDB.CreateParameter("@experimentID", experimentID,DbType.Int64));

            try
            {
                myConnection.Open();
                int rows = myCommand.ExecuteNonQuery();

                return (rows != -1);

                //alternatively
                //return true;
            }
            catch (Exception ex)
            {
                //return false;
                throw new Exception("Exception thrown deleting experiment", ex);
            }
            finally
            {
                myConnection.Close();
            }
        }
  
        
        /// <summary>
        /// Returns the current
        /// </summary>
        /// <param name="experimentID"></param>
        /// <returns></returns>
        public Coupon GetExperimentCoupon(long experimentID)
        {
            Coupon expCoupon = null;

            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("ExperimentCoupon_Retrive", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.Parameters.Add(FactoryDB.CreateParameter("@experimentID", experimentID,DbType.Int64));
            try
            {
                myConnection.Open();
                DbDataReader myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                   
                    bool cancelled = myReader.GetBoolean(0);
                    if (cancelled)
                        return null;
                    expCoupon = new Coupon();
                    expCoupon.couponId = myReader.GetInt64(1);
                    expCoupon.passkey = myReader.GetString(2);
                    expCoupon.issuerGuid = ProcessAgentDB.ServiceGuid;

                }
            }
            catch(DbException ex) {
                throw new Exception("Exception thrown retrieving experiment coupon", ex);
            }
            finally
            {
                myConnection.Close();
            }
            return expCoupon;
        }
        
        /// <summary>
        /// Creates the Experiment_information record on the ServiceBroker
        /// </summary>
        /// <param name="status"></param>
        /// <param name="userid"></param>
        /// <param name="groupid"></param>
        /// <param name="ls_id"></param>
        /// <param name="client_id"></param>
        /// <param name="ess_id"></param>
        /// <param name="duration">Time in seconds that the Experiment will be available, normally -1 ?</param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns>the experiments ID</returns>
        public static long InsertExperiment(int status, int userid, int groupid,
            int ls_id, int client_id, int essID, DateTime start, long duration)
        {
            long experimentID = -1;

            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("Experiment_Create", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@status", status, DbType.Int32));
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@user", userid, DbType.Int32));
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@group", groupid, DbType.Int32));
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@ls", ls_id, DbType.Int32));
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@client", client_id, DbType.Int32));
            if (essID > 0)
                myCommand.Parameters.Add(FactoryDB.CreateParameter( "@ess", essID, DbType.Int32));
            else
                myCommand.Parameters.Add(FactoryDB.CreateParameter( "@ess", null, DbType.Int32));
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@start", start, DbType.DateTime));
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@duration", duration,DbType.Int64));
           
            try
            {
                myConnection.Open();
                experimentID = Convert.ToInt64(myCommand.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw new Exception("Exception thrown creating experiment", ex);
            }
            finally
            {
                myConnection.Close();
            }

            
            return experimentID;
        }
        /// <summary>
        /// Saves the record on the experiment's ESS
        /// </summary>
        /// <param name="experimentID"></param>
        /// <param name="submitter"></param>
        /// <param name="type"></param>
        /// <param name="contents"></param>
        /// <param name="xmlSearchable"></param>
        /// <param name="attributes"></param>
        /// <returns>experiment Sequence number or -1 if the record may not be saved</returns>
        public static int SaveExperimentRecord(long experimentID, string submitter,
            string type, bool xmlSearchable, string contents, RecordAttribute[] attributes)
        {
            int seqNum = -1;
            ExperimentStorageProxy essProxy = GetEssProxy(experimentID);
            if (essProxy != null)
            {
                seqNum = essProxy.AddRecord(experimentID, submitter, type, xmlSearchable,
                        contents, attributes);
            }
            return seqNum;
        }

        //public static ExperimentRecord RetrieveExperimentRecord(long experimentID, int sequenceNum)
        //{
        //    ExperimentRecord record = null;
        //    ExperimentStorageProxy essProxy = GetEssProxy(experimentID);
           
        //    if (essProxy != null)
        //    {
        //        record = essProxy.GetRecord(experimentID, sequenceNum);
        //    }
        //    return record;
        //}

        //public static ExperimentRecord[] RetrieveExperimentRecords(long experimentID, Criterion[] criteria)
        //{
        //    ExperimentRecord[] records = null;
        //    ExperimentStorageProxy essProxy = GetEssProxy(experimentID);

        //    if (essProxy != null)
        //    {
        //        records = essProxy.GetRecords(experimentID, criteria);
        //    }
        //    return records;
        //}

        //public static ExperimentRecord[] RetrieveExperimentRecords(long experimentID,int essID, Criterion[] criteria)
        //{
        //    ExperimentRecord [] records = null;

        //    // This operation should happen within the Wrapper
        //    BrokerDB ticketIssuer = new BrokerDB();
        //    ProcessAgentInfo ess = ticketIssuer.GetProcessAgentInfo(essID);
        //    Coupon opCoupon = null;

        //    long[] couponIDs = RetrieveExperimentCouponIDs(experimentID);
        //    if (couponIDs != null && couponIDs.Length >= 0)
        //    {   // An experiment ticket collection exists, try and find an active
        //        // Retrieve_Records ticket
        //        for (int i = 0; i < couponIDs.Length; i++)
        //        {
        //            Coupon tmpCoupon = ticketIssuer.GetIssuedCoupon(couponIDs[i]);
        //            Ticket ticket = ticketIssuer.RetrieveTicket(tmpCoupon, TicketTypes.RETRIEVE_RECORDS);
        //            if (ticket != null && !(ticket.SecondsToExpire() > 60))
        //            {
                        
        //                opCoupon = tmpCoupon;
        //                break;
        //            }
        //        }
        //    }
        //    if (opCoupon == null)
        //    {
        //        TicketLoadFactory factory = TicketLoadFactory.Instance();
        //        string payload = factory.RetrieveRecordsPayload(experimentID, ess.webServiceUrl);
        //        // Create a ticket to read records
        //        opCoupon = ticketIssuer.CreateTicket(TicketTypes.RETRIEVE_RECORDS, ess.agentGuid, ticketIssuer.GetServiceBrokerInfo().agentGuid, 600, payload);
        //        InternalDataDB.InsertExperimentCoupon(experimentID, opCoupon.couponId);
        //    }
        //    ExperimentStorageProxy essProxy = new ExperimentStorageProxy();
        //    OperationAuthHeader header = new OperationAuthHeader();
        //    header.coupon = opCoupon;
        //    essProxy.Url = ess.webServiceUrl;
        //    essProxy.OperationAuthHeaderValue = header;
            
        //    records = essProxy.GetRecords(experimentID, criteria);
        //    return records;
        //}
/*
        /// <summary>
        /// to record an experiment specification
        /// </summary>
        public static void SaveExperimentInformation(long experimentID, int userID, int effectiveGroupID, int labServerID, string annotation)
        {
            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("ExperimentInformation", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            //SQL Server doesn't throw an error if we try to update a row that doesn't exist. Perhaps a better way to check this would be to see if the row 
            // exists(by calling retrieve experiment spec. from where you're calling it and then proceed if not null.
            myCommand.Parameters.Add(FactoryDB.CreateParameter("@experimentID", experimentID));
            myCommand.Parameters.Add(FactoryDB.CreateParameter("@userID", userID));
            myCommand.Parameters.Add(FactoryDB.CreateParameter("@effectiveGroupID", effectiveGroupID));
            myCommand.Parameters.Add(FactoryDB.CreateParameter("@labServerID", labServerID));
            myCommand.Parameters.Add(FactoryDB.CreateParameter("@annotation", annotation));

            try
            {
                myConnection.Open();
                myCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Exception thrown saving experiment information", ex);
            }
            finally
            {
                myConnection.Close();
            }
        }
*/
        /// <summary>
        /// saves or modifies an optional user defined annotation to the experiment record
        /// </summary>
        public static string SaveExperimentAnnotation(long experimentID, string annotation)
        {
            string previousAnnotation = null;
            previousAnnotation = SelectExperimentAnnotation(experimentID);
            if (previousAnnotation != annotation)
            {
                DbConnection myConnection = FactoryDB.GetConnection();
                DbCommand myCommand = FactoryDB.CreateCommand("Experiment_UpdateAnnotation", myConnection);
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.Parameters.Add(FactoryDB.CreateParameter("@experimentID", experimentID, DbType.Int64));
                myCommand.Parameters.Add(FactoryDB.CreateParameter("@annotation", annotation,DbType.String));
                try
                {
                    myConnection.Open();
                    myCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("Exception thrown SaveExperimentAnnotation", ex);
                }
                finally
                {
                    myConnection.Close();
                }
            }
            return previousAnnotation;
        }

        /// <summary>
        /// to retrieve a previously saved experiment annotation
        /// </summary>
        public static string SelectExperimentAnnotation(long experimentID)
        {
            string annotation = null;

            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("Experiment_RetrieveAnnotation", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.Parameters.Add(FactoryDB.CreateParameter("@experimentID", experimentID,DbType.Int64));
            try
            {
                myConnection.Open();
                annotation = myCommand.ExecuteScalar().ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("Exception thrown retrieving experiment annotation. " + ex.Message, ex);
            }
            finally
            {
                myConnection.Close();
            }

            return annotation;
        }

        /// <summary>
        /// to retrieve the owner (currently a user) of an experiment
        /// </summary>
        public static int SelectExperimentOwner(long experimentID)
        {
            int userID = -1;

            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("Experiment_RetrieveOwner", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.Parameters.Add(FactoryDB.CreateParameter("@experimentID", experimentID,DbType.Int64));
            try
            {
                myConnection.Open();
                userID = Convert.ToInt32(myCommand.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw new Exception("Exception thrown retrieving experiment owner", ex);
            }
            finally
            {
                myConnection.Close();
            }

            return userID;
        }

        /// <summary>
        /// to retrieve the effective group an experiment was run under
        /// </summary>
        public static int SelectExperimentGroup(long experimentID)
        {
            int groupID = -1;

            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("Experiment_RetrieveGroup", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.Parameters.Add(FactoryDB.CreateParameter("@experimentID", experimentID, DbType.Int64));
            try
            {
                myConnection.Open();
                groupID = Convert.ToInt32(myCommand.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw new Exception("Exception thrown retrieving experiment group", ex);
            }
            finally
            {
                myConnection.Close();
            }

            return groupID;
        }

        /// <summary>
        /// to change the owner of an experiment.
        /// </summary>
        public static void UpdateExperimentOwner(long experimentID, int newUserID)
        {
            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("Experiment_UpdateOwner", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@experimentID", experimentID, DbType.Int64));
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@newUserID", newUserID, DbType.Int32));

            try
            {
                myConnection.Open();
                myCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Exception thrown ModifyExperimentOwner", ex);
            }
            finally
            {
                myConnection.Close();
            }
        }
        /*
		/// <summary>
		/// to delete information about an experiment (such as owner, submitted time etc.)
		/// </summary>
		public static void DeleteExperimentInformation (long experimentID)
		{
			DbConnection myConnection = new DbConnection(ConfigurationManager.AppSettings ["sqlConnection"]);
			DbCommand myCommand = FactoryDB.CreateCommand("Experiment_Delete", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure ;
			myCommand.Parameters.Add(FactoryDB.CreateParameter("@experimentID", experimentID));

			try 
			{
				myConnection.Open();
				myCommand.ExecuteNonQuery();
			}
			catch (Exception ex)
			{
				throw new Exception("Exception thrown Deleting Experiment Information",ex);
			}
			finally
			{
				myConnection.Close();
			}
		}
        */
        /// <summary>
        /// deletes all record of the experiments specified by the array of experiment IDs
        /// </summary>
        /// <param name="experimentIDs">an array that identifies the experiments to be deleted</param>
        /// <returns>an array containing the subset of the specified experiment IDs for which the delete operation failed</returns>

        public static long[] DeleteExperiments(long[] experimentIDs)
        {
            /*
            * Note : Alternately ADO.NET could be used. However, the disconnected DataAdapter object might prove
            * extremely inefficient and hence this method was chosen
            */

            ArrayList arrayList = new ArrayList();

            try
            {
                // this is very inefficient and cannot have a transaction

                foreach (long experimentID in experimentIDs)
                {
                    int qualID = Authorization.AuthorizationAPI.GetQualifierID((int)experimentID, Qualifier.experimentQualifierTypeID);
                    if (qualID > 0)
                        Authorization.AuthorizationAPI.RemoveQualifiers(new int[] { qualID });
                    bool deleted = DeleteExperiment(experimentID);

                    // Deleting from table Experiments
                    /*  IMPORTANT ! - The database if currently set to Cascade delete, where deleting an experiment will automatically
                    *  delete the relevant Experiment_Results records and consequentially,that will automatically 
                    *  delete all the Result Message Records. If Cascade Delete is not to be used, then the code to delete the extra records
                    *  in these 2 tables when an experiment is deleted should be added in the stored procedure
                    */
                    if (deleted)
                    {
                        arrayList.Add(experimentID);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception thrown in Removing Experiments", ex);
            }

            //Shaomin's Code
            ArrayList qualifiersToRemove = new ArrayList();

            for (int i = 0; i < experimentIDs.Length; i++)
            {
                if (!arrayList.Contains(experimentIDs[i]))
                {
                    int qualifierID =
                        Authorization.AuthorizationAPI.GetQualifierID((int)experimentIDs[i],
                        Qualifier.experimentQualifierTypeID);
                    qualifiersToRemove.Add(qualifierID);
                }
            }

            int[] qualifierIDs = new int[qualifiersToRemove.Count];
            for (int i = 0; i < qualifiersToRemove.Count; i++)
            {
                qualifierIDs[i] = Convert.ToInt32(qualifiersToRemove[i]);
            }

            Authorization.AuthorizationAPI.RemoveQualifiers(qualifierIDs);

            long[] expIDs = new long[arrayList.Count];
            for (int i = 0; i < arrayList.Count; i++)
            {
                expIDs[i] = (long)arrayList[i];
            }

            return expIDs;
        }
        /// <summary>
        /// Retrieves the current Experiment summary form the ServiceBroker's database.
        /// </summary>
        /// <param name="experimentID"></param>
        /// <returns></returns>
        public static ExperimentSummary SelectExperimentSummary(long experimentID)
        {
            //select ei.coupon_ID, u.user_Name, g.group_Name, c.Lab_Client_Name,status, essGuid, 
            //   scheduledStart,duration, creationTime, closeTime, annotation

            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("ExperimentSummary_Retrieve", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@experimentID", experimentID, DbType.Int64));
            ExperimentSummary exp = null;
            try
            {
                myConnection.Open();
                DbDataReader myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                    {
                        exp = new ExperimentSummary();
                        exp.experimentId = experimentID;
                       
                        exp.userName = myReader.GetString(0);
                        exp.groupName = myReader.GetString(1);
                        exp.labServerGuid = myReader.GetString(2);
                        exp.labServerName = myReader.GetString(3);
                        exp.clientName = myReader.GetString(4);
                        exp.clientVersion = myReader.GetString(5);
                        exp.status = myReader.GetInt32(6); 
                        if (!myReader.IsDBNull(7))
                         exp.essGuid = myReader.GetString(7);
                        else 
                          exp.essGuid = null;
                        if (!myReader.IsDBNull(8))
                            exp.scheduledStart = DateUtil.SpecifyUTC(myReader.GetDateTime(8));
                        if (!myReader.IsDBNull(9))
                            exp.duration = myReader.GetInt64(9);
                        if (!myReader.IsDBNull(10))
                        exp.creationTime = DateUtil.SpecifyUTC(myReader.GetDateTime(10));
                        if (!myReader.IsDBNull(11))
                            exp.closeTime = DateUtil.SpecifyUTC(myReader.GetDateTime(11));
                        if (!myReader.IsDBNull(12))
                            exp.annotation = myReader.GetString(12);
                    if (!myReader.IsDBNull(13))
                        exp.recordCount = myReader.GetInt32(13);
                     
                    }
                    myReader.Close();
                }
            
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                myConnection.Close();
            }
            return exp;
        }
        /// <summary>
        /// retrieves all Active ExperimentIds for the user, group, client and server. 
        /// Currently active uses scheduledStartTime and duration, status and closetime are ignored.
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="groupID"
        /// <returns></returns>
        public static long[] RetrieveActiveExperimentIDs(int userID, int groupID,int serverID, int clientID)
        {

            StringBuilder whereClause = null;
            List<long> expIDs = new List<long>();

            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand cmd = FactoryDB.CreateCommand("Experiment_RetrieveActiveIDs", myConnection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(FactoryDB.CreateParameter( "@userId", userID, DbType.Int32));
            cmd.Parameters.Add(FactoryDB.CreateParameter( "@groupId", groupID, DbType.Int32));
            cmd.Parameters.Add(FactoryDB.CreateParameter( "@serverId", serverID, DbType.Int32));
            cmd.Parameters.Add(FactoryDB.CreateParameter( "@clientId", clientID, DbType.Int32));

            try
            {
                myConnection.Open();
                DbDataReader myReader = cmd.ExecuteReader();
                while (myReader.Read())
                {
                    expIDs.Add(myReader.GetInt64(0));
                }
            }
            catch { }
            finally
            {
                myConnection.Close();
            }
            return expIDs.ToArray();
        }

        /// <summary>
        /// retrieves all ExperimentIds that the specified user and group has access to
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="groupID"
        /// <returns></returns>
        public static long[] RetrieveAuthorizedExperimentIDs(int userID, int groupID)
        {
            StringBuilder whereClause = null;
            List<long> expIDs = new List<long>();

            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand cmd = FactoryDB.CreateCommand("Authorization_RetrieveExpIDs", myConnection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(FactoryDB.CreateParameter("@userId", userID,DbType.Int32));
            cmd.Parameters.Add(FactoryDB.CreateParameter("@groupId", groupID, DbType.Int32));

            try
            {
                myConnection.Open();
                DbDataReader myReader = cmd.ExecuteReader();
                while (myReader.Read())
                {
                    expIDs.Add(myReader.GetInt64(0));
                }
            }
            catch { }
            finally
            {
                myConnection.Close();
            }
            return expIDs.ToArray();
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
            List<long> expIDs = new List<long>();

            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand cmd = FactoryDB.CreateCommand("ExperimentIDs_Retrieve", myConnection);
            cmd.CommandType = CommandType.StoredProcedure;
            if(userID > 0)
                cmd.Parameters.Add(FactoryDB.CreateParameter("@userId", userID, DbType.Int32));
            if(groupID > 0)
                cmd.Parameters.Add(FactoryDB.CreateParameter("@groupId", groupID, DbType.Int32));
            if(clientID > 0)
                cmd.Parameters.Add(FactoryDB.CreateParameter("@clientId", clientID, DbType.Int32));

            try
            {
                myConnection.Open();
                DbDataReader myReader = cmd.ExecuteReader();
                while (myReader.Read())
                {
                    expIDs.Add(myReader.GetInt64(0));
                }
            }
            catch { }
            finally
            {
                myConnection.Close();
            }
            return expIDs.ToArray();
        }
/*
        /// <summary>
        /// retrieves all ExperimentIds that the specified user and group has access to
        /// and that match the Criterion. The criterion has been limited to actual ISB experiment fields.
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="groupID"
        /// <param name="carray">The criterion have been limited to ServiceBroker fields</param>
        /// <returns></returns>
        public static DataSet RetrieveExperimentIds(long[] experiments, Criterion[] carray)
        {
            DataSet results = new DataSet();
            StringBuilder whereClause = null;
            List<long> expIDs = new List<long>();
  
              if(carray != null && carray.Length > 0){     

                whereClause = new StringBuilder();
                   
                for (int i = 0; i < carray.Length; i++)
                {
                    if(i > 0)
                        whereClause.Append(" AND ");

                    switch (carray[i].attribute.ToLower())
                    {
                      
                        case "agent_id":    // Actual SB experiment column names
                        case "annotation":
                        case "client_id":
                        case "creationtime":
                        case "ess_id":
                        case "group_id":
                        case "record_count":
                        case "scheduledstart":
                        case "status":
                        case "user_id":
                            whereClause.Append(carray[i].ToSQL());
                            break;
                        default: // any unhandled attributes are ignored
                            break;
                    }
                }
                if(whereClause.Length > 7000){
                    throw new Exception("Please reduce the number of criteria for this query, too many arguments!");
                }

              }
                DbConnection myConnection = FactoryDB.GetConnection();
                DbCommand cmd = FactoryDB.CreateCommand("Authorization_RetrieveExpIDsCriteria", myConnection);
                cmd.CommandType = CommandType.StoredProcedure;
                DbParameter userParam = cmd.Parameters.Add(FactoryDB.CreateParameter("@userID", DbType.Int);
				userParam.Value = userID;
                DbParameter groupParam = cmd.Parameters.Add(FactoryDB.CreateParameter("@groupID", DbType.Int);
				groupParam.Value = userID;
                DbParameter whereParam = cmd.Parameters.Add(FactoryDB.CreateParameter(FactoryDB.CreateParameter("@whereClause",DbType.AnsiString,7000));
                if(whereClause != null && whereClause.Length == 0)
                    whereParam.Value = DBNull.Value;
                else
                    whereParam.Value = whereClause.ToString();
	
            try
            {
                myConnection.Open();
                DbDataReader myReader = cmd.ExecuteReader();
                while (myReader.Read())
                {
                    expIDs.Add(myReader.GetInt64(0));
                }
            }
            catch{}
            finally{
                myConnection.Close();
            }
           return expIDs.ToArray();
        }
*/
        /// <summary>
        /// retrieves all ExperimentIds that the specified user and group has access to
        /// and that match the Criterion. The criterion has been limited to actual ISB experiment fields.
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="groupID"
        /// <param name="carray">The criterion have been limited to ServiceBroker fields</param>
        /// <returns></returns>
        public static DataSet RetrieveExperimentIDsCriteria(int userID, int groupID, Criterion[] carray)
        {
            //if (carray == null || carray.Length == 0)
            //{
            //    throw new Exception("Method requires Criteria");
            //}
            StringBuilder whereClause = null;
            DataSet results = new DataSet();

            if (carray != null && carray.Length > 0)
            {
                whereClause = new StringBuilder();

                for (int i = 0; i < carray.Length; i++)
                {
                    if (i > 0)
                        whereClause.Append(" AND ");

                    switch (carray[i].attribute.ToLower())
                    {
                        case "agent_id":    // Actual SB experiment column names  
                        case "annotation":
                        case "client_id":
                        case "creationtime":
                        case "duration":
                        case "ess_id":
                        case "experiment_id":
                        case "group_id":
                        case "record_count":
                        case "scheduledstart":
                        case "status":
                        case "user_id":
                            whereClause.Append(carray[i].ToSQL());
                            break;
                        //these criterion are based on external values, requiring special processing of the fields.
                        case "username":
                            whereClause.Append("User_ID " + carray[i].predicate);
                            whereClause.Append(" (select user_id from users where user_name='" + carray[i].value + "')");
                            break;
                        case "groupname":
                            whereClause.Append("Group_ID " + carray[i].predicate);
                            whereClause.Append(" (select group_id from group where group_name='" + carray[i].value + "')");
                            break;
                        case "clientguid":
                            whereClause.Append("Client_ID " +carray[i].predicate);
                            whereClause.Append(" (select client_id from lab_clients where client_guid='" + carray[i].value + "')");
                            break;
                        case "clientname":
                            whereClause.Append("Client_ID " + carray[i].predicate);
                            whereClause.Append(" (select client_id from lab_clients where lab_client_name='" + carray[i].value + "')");
                            break;
                        case "labservername":
                            whereClause.Append("Agent_ID " + carray[i].predicate);
                            whereClause.Append(" (select agent_id from processAgent where agent_name='" + carray[i].value + "')");
                            break;
                        case "start":
                            whereClause.Append("creationtime " + carray[i].predicate + " " + carray[i].value);
                            break;
                        // any unhandled attributes are ignored
                        default: 
                            break;
                    }
                }
                if (whereClause.Length > 7000)
                {
                    throw new Exception("Please reduce the number of criteria for this query, too many arguments!");
                }


                DbConnection myConnection = FactoryDB.GetConnection();
                DbCommand cmd = FactoryDB.CreateCommand("Authorization_RetrieveExpIDsCriteria", myConnection);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(FactoryDB.CreateParameter("@userID",userID, DbType.Int32));
                cmd.Parameters.Add(FactoryDB.CreateParameter("@groupID", groupID,DbType.Int32));
                cmd.Parameters.Add(FactoryDB.CreateParameter("@criteria", whereClause.ToString(), DbType.AnsiString, 7000));
                
                try
                {
                    myConnection.Open();
                    DbDataReader myReader = cmd.ExecuteReader();
                    if (myReader.HasRows)
                    {

                        DataTable exp = new DataTable("sbHits");
                        exp.Columns.Add("expid", typeof(System.Int64));
                        exp.Columns.Add("essid", typeof(System.Int32));
                        results.Tables.Add(exp);

                        while (myReader.Read())
                        {
                            DataRow row = exp.NewRow();
                            row["expid"] = myReader.GetInt64(0);
                            if (!myReader.IsDBNull(1))
                                row["essid"] = myReader.GetInt32(1);
                            else
                                row["essid"] = DBNull.Value;
                            exp.Rows.Add(row);
                        }
                    }
                    
                    if (myReader.NextResult())
                    {
                        if (myReader.HasRows)
                        {
                            DataTable ess = new DataTable("ess");
                            ess.Columns.Add("essid", typeof(System.Int32));
                            results.Tables.Add(ess);

                            while (myReader.Read())
                            {
                                if (!myReader.IsDBNull(0))
                                {
                                    DataRow essRow = ess.NewRow();
                                    essRow["essid"] = myReader.GetInt32(0);
                                    ess.Rows.Add(essRow);
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                   Logger.WriteLine("Error: " + e.Message);
                    throw;
                }
                finally
                {
                    myConnection.Close();
                }
                
            }
            return results;
            
        }

        public static ExperimentAdminInfo SelectExperimentAdminInfo(long experimentID)
        {
            ExperimentAdminInfo[] infos = SelectExperimentAdminInfos(new long[] { experimentID });
            if (infos != null && infos.Length > 0)
                return infos[0];
            else 
                return null;
        }

        public static ExperimentAdminInfo[] SelectExperimentAdminInfos(long[] experimentIDs)
        {
            //select u.user_Name,g.group_Name,pa.Agent_Guid,pa.Agent_Name,
            //c.Lab_Client_Name,c.version, status, ess_ID, scheduledStart, duration, creationTime, closeTime, annotation, record_count
            //from Experiments ei,ProcessAgent pa, Groups g, Lab_Clients c, Users u
            List<ExperimentAdminInfo> list = new List<ExperimentAdminInfo>();

            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("Experiment_RetrieveAdminInfos", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            myCommand.Parameters.Add(FactoryDB.CreateParameter("@ids", Utilities.ToCSV(experimentIDs),DbType.AnsiString,7000));
            
            //myCommand.Parameters.Add(FactoryDB.CreateParameter("@userId", DbType.Int));
            //myCommand.Parameters["@userID"].Value = userID;
            //myCommand.Parameters.Add(FactoryDB.CreateParameter("@groupId", DbType.Int));
            //myCommand.Parameters["@groupID"].Value = groupID;
            try
            {
                myConnection.Open();

           
                    DbDataReader myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        ExperimentAdminInfo exp = new ExperimentAdminInfo();
                   
                        exp.experimentID = myReader.GetInt64(0);
                        exp.userID = myReader.GetInt32(1);
                        exp.groupID = myReader.GetInt32(2);
                        exp.agentID = myReader.GetInt32(3);
                        exp.clientID = myReader.GetInt32(4);
                        if (!myReader.IsDBNull(5))
                        exp.essID = myReader.GetInt32(5);
                        exp.status = myReader.GetInt32(6);
                        exp.recordCount = myReader.GetInt32(7);
                        exp.duration = myReader.GetInt64(8);
                        if (!myReader.IsDBNull(9))
                        exp.startTime = DateUtil.SpecifyUTC(myReader.GetDateTime(9));
                        exp.creationTime = DateUtil.SpecifyUTC(myReader.GetDateTime(10));
                        if (!myReader.IsDBNull(11))
                        exp.closeTime = DateUtil.SpecifyUTC(myReader.GetDateTime(11));
                    if (!myReader.IsDBNull(12))
                        exp.annotation = myReader.GetString(12);

                      list.Add(exp);
                       
                    }
                    myReader.Close();
                }
            
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                myConnection.Close();
            }
            return list.ToArray();
        }

           
        public static ExperimentSummary[] SelectExperimentSummaries(long[] experimentIDs)
        {
            //select u.user_Name,g.group_Name,pa.Agent_Guid,pa.Agent_Name,
            //c.Lab_Client_Name,c.version, status, ess_ID, scheduledStart, duration, creationTime, closeTime, annotation, record_count
            //from Experiments ei,ProcessAgent pa, Groups g, Lab_Clients c, Users u
            ExperimentSummary[] exp = new ExperimentSummary[experimentIDs.Length];
            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("ExperimentSummary_Retrieve", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            myCommand.Parameters.Add(FactoryDB.CreateParameter("@experimentID", null,DbType.Int64));
            try
            {
                myConnection.Open();

                for (int i = 0; i < experimentIDs.Length; i++)
                {
                    myCommand.Parameters["@experimentID"].Value = experimentIDs[i];

                    // get experimentInfo from table Experiments
                   // select u.user_Name,g.group_Name,pa.Agent_Guid,pa.Agent_Name,
                   // c.Lab_Client_Name,c.version, status, ess_ID, scheduledStart, duration, 
                   // creationTime, closeTime, annotation, record_count
                   // from Experiments ei,ProcessAgent pa, Groups g, Lab_Clients c, Users u
                    DbDataReader myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        exp[i] = new ExperimentSummary();
                        exp[i].experimentId = experimentIDs[i];
                       
                        exp[i].userName = myReader.GetString(0);
                        exp[i].groupName = myReader.GetString(1);
                        exp[i].labServerGuid = myReader.GetString(2);
                        exp[i].labServerName = myReader.GetString(3);
                        exp[i].clientName = myReader.GetString(4);
                        exp[i].clientVersion = myReader.GetString(5);
                        exp[i].status = myReader.GetInt32(6);
                        if (!myReader.IsDBNull(7))
                            exp[i].essGuid = myReader.GetString(7);
                        else exp[i].essGuid = null;
                        if (!myReader.IsDBNull(8))
                            exp[i].scheduledStart = DateUtil.SpecifyUTC(myReader.GetDateTime(8));
                        if (!myReader.IsDBNull(9))
                            exp[i].duration = myReader.GetInt64(9);
                        if (!myReader.IsDBNull(10))
                        exp[i].creationTime = DateUtil.SpecifyUTC(myReader.GetDateTime(10));
                        if (!myReader.IsDBNull(11))
                            exp[i].closeTime = DateUtil.SpecifyUTC(myReader.GetDateTime(11));
                        if (!myReader.IsDBNull(12))
                            exp[i].annotation = myReader.GetString(12);
                        if (!myReader.IsDBNull(13))
                        exp[i].recordCount = myReader.GetInt32(13);
                    }
                    myReader.Close();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                myConnection.Close();
            }
            return exp;
        }

        public static bool UpdateExperimentStatus(long experimentID, int statusCode)
        {
            bool ok = false;
            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("ExperimentStatus_UpdateCode", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.Parameters.Add(FactoryDB.CreateParameter("@experimentID", experimentID, DbType.Int64));
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@status", statusCode, DbType.Int32));
            try{
                myConnection.Open();
                object obj = myCommand.ExecuteScalar();
                if(obj != null){
                    int i = Convert.ToInt32(obj);
                    ok = i > 0;
                }
            }
            catch{
                throw;
            }
            finally{
                myConnection.Close();
            }
            return ok;
        }

        public static bool UpdateExperimentStatus(StorageStatus status)
        {
            bool ok = false;
            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("ExperimentStatus_Update", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@experimentID", status.experimentId, DbType.Int64));
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@status", status.status, DbType.Int32));
            if (status.closeTime != null && (status.closeTime > MinDbDateTime))
            {
                myCommand.Parameters.Add(FactoryDB.CreateParameter("@closeTime", status.closeTime, DbType.DateTime));
            }
            else
            {
                myCommand.Parameters.Add(FactoryDB.CreateParameter("@closeTime", null, DbType.DateTime));
            }
            if (status.recordCount != null)
                myCommand.Parameters.Add(FactoryDB.CreateParameter( "@recordCount",status.recordCount, DbType.Int32));
            else
                myCommand.Parameters.Add(FactoryDB.CreateParameter( "@recordCount",null, DbType.Int32));
          
            try
            {
                myConnection.Open();
                myCommand.ExecuteNonQuery();
                ok = true;
            }
            catch (Exception e)
            {
               Logger.WriteLine("UpdateExperimentStatus: " + e.Message);
            }
            finally
            {
                myConnection.Close();
            }
            return ok;
        }




        /*
        Warning this is not updated to cu=reent Stored procedures
                /// <summary>
                /// to retrieve information about an experiment such as owner, submitted time, etc.
                /// </summary>
                public static ExperimentInformation RetrieveExperimentInformation (long experimentID)
                {
                    ExperimentInformation ei = new ExperimentInformation();
            
			
                    DbConnection myConnection = FactoryDB.GetConnection()
                    DbCommand myCommand = FactoryDB.CreateCommand("Experiment_RetrieveInformation", myConnection);
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.Parameters .Add(new DbParameter ("@experimentID",experimentID));

                    try 
                    {
                        myConnection.Open ();
				
                        // get experiment info from table Experiment_Information
                        DbDataReader myReader = myCommand.ExecuteReader ();
                        while(myReader.Read ())
                        {	
                            ei.experimentID = experimentID;
                   
			
                            if(myReader["status"] !=System.DBNull.Value)
                                ei.statusCode = Convert.ToInt32(myReader["status"]);
                            if(myReader["user_id"] != System.DBNull.Value )
                                ei.userID = Convert.ToInt32(myReader["user_id"]);
                            if(myReader["group_id"] != System.DBNull.Value )
                                ei.effectiveGroupID= Convert.ToInt32(myReader["group_id"]);
                            if(myReader["lab_server_id"] != System.DBNull.Value )
                                ei.labServerID = Convert.ToInt32(myReader["agent_id"]);
                    
                            if(myReader["creation_time"] != System.DBNull.Value )
                                ei.submissionTime = Convert.ToDateTime(myReader["creation_time"]);
                            if(myReader["duration"] != System.DBNull.Value )
                                ei.submissionTime = Convert.ToDateTime(myReader["duration"]);
                            if(myReader["close_time"] != System.DBNull.Value )
                                ei.completionTime= Convert.ToDateTime( myReader["close_time"]);
                            if(myReader["annotation"] != System.DBNull.Value )
                                ei.annotation= (string) myReader["annotation"];
                            ei.
					
					
                        }
                        myReader.Close ();
				
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Exception thrown getting experiment information",ex);
                    }
                    finally 
                    {
                        myConnection.Close ();
                    }
			
                    return ei;
                }
              */

        /*
        /// <summary>
        /// to retrieve information about a set of experiments 
        /// </summary>
        public static ExperimentInformation[] RetrieveExperimentInformation (long[] experimentIDs)
        {
            ExperimentInformation[]ei = new ExperimentInformation[experimentIDs.Length];
            for (int i = 0;i<experimentIDs.Length;i++)
            {
                ei[i]= new ExperimentInformation();
            }
			
            DbConnection myConnection = FactoryDB.GetConnection()
            DbCommand myCommand = FactoryDB.CreateCommand("Experiment_RetrieveInformation", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.Parameters .Add(new DbParameter ("@experimentID",DbType.Int64));

            try 
            {
                myConnection.Open ();
				
                for (int i=0;i<experimentIDs.Length;i++)
                {
                    myCommand.Parameters["@experimentID"].Value = experimentIDs[i];

												   
                    // get experiment info from table Experiment_Information
                    DbDataReader myReader = myCommand.ExecuteReader ();
                    while(myReader.Read ())
                    {	
                        ei[i].experimentID = experimentIDs[i];

                        if(myReader["user_id"] != System.DBNull.Value )
                            ei[i].userID = Convert.ToInt32(myReader["user_id"]);
                        if(myReader["group_id"] != System.DBNull.Value )
                            ei[i].effectiveGroupID= Convert.ToInt32(myReader["group_id"]);
                        if(myReader["agent_id"] != System.DBNull.Value )
                            ei[i].labServerID = Convert.ToInt32(myReader["agent_id"]);
                        //if (myReader["scheduledStart"] != System.DBNull.Value)
                        //    ei[i].submissionTime = Convert.ToDateTime(myReader["scheduledStart"]);
                        //if (myReader["duration"] != System.DBNull.Value)
                        //    ei[i].submissionTime = Convert.ToDateTime(myReader["duration"]);
                        //if(myReader["essID"] != System.DBNull.Value )
                        //    ei[i]. = Convert.ToDateTime(myReader["essID"]);
                        if(myReader["annotation"] != System.DBNull.Value )
                            ei[i].annotation= (string) myReader["annotation"];
                        if(myReader["closeTime"] != System.DBNull.Value )
                            ei[i].completionTime= Convert.ToDateTime( myReader["closeTime"]);
                        if(myReader["status"] !=System.DBNull.Value)
                            ei[i].statusCode = Convert.ToInt32(myReader["status"]);
                    }
                    myReader.Close ();
                }
				
            }
            catch (Exception ex)
            {
                throw new Exception("Exception thrown getting experiment information",ex);
            }
            finally 
            {
                myConnection.Close ();
            }
			
            return ei;
        }
*/
        /// <summary>
        /// to retrieve all the experiments that were run under the specified groups
        /// </summary>
        public static long[] SelectGroupExperimentIDs(int[] groupIDs)
        {
            List<long> eIDs = new List<long>();

            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("ExperimentIDs_RetrieveByGroup", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.Parameters.Add(FactoryDB.CreateParameter("@groupID", null, DbType.Int32));

            try
            {
                myConnection.Open();

                for (int i = 0; i < groupIDs.Length; i++)
                {
                    myCommand.Parameters["@groupID"].Value = groupIDs[i];

                    // get experiment id from table Experiment_Information
                    DbDataReader myReader = myCommand.ExecuteReader();

                    while (myReader.Read())
                    {
                        if (myReader["experiment_id"] != System.DBNull.Value)
                            eIDs.Add(Convert.ToInt64(myReader["experiment_id"]));
                    }
                    myReader.Close();
                }

                return eIDs.ToArray();

            }
            catch (Exception ex)
            {
                throw new Exception("Exception thrown getting experiments that were run in this group", ex);
            }
            finally
            {
                myConnection.Close();
            }
        }

        /// <summary>
        /// finds all of those Experiments specified in experimentIDs which possess attributes that match the logical AND of conditions expressed in the Criterion array. The search is limited to those experiments that the current user created or for which he or she has a “ReadExperiment” grant. The Criterion conditions must be satisfied elements of the Experiment’s administrative data model such as its ownerID or by the RecordAttributes of a single ExperimentRecord belonging to the experiment for it to qualify. 
        /// </summary>
        /// <param name="criteria">The array of Criterion objects that specify the attributes of the requested experiments; all experimentIDs match if null.</param>
        /// <returns>an array of the IDs of Experiments that match the search criteria</returns>
        // Need to stub in alternate predicates and attribute hash maps. - CV 07/08/04
        public static long[] SelectExperimentIDs(Criterion[] criteria)
        {
            StringBuilder sqlQuery = new StringBuilder("select experiment_id from experiments");

            long[] experimentIDs;

          
            for (int i = 0; i < criteria.Length; i++)
            {
                if (i == 0)
                {
                    sqlQuery.Append(" where ");
                }
                else {
                    sqlQuery.Append(" AND ");
                }
                sqlQuery.Append(criteria[i].attribute + " " + criteria[i].predicate + " '" + criteria[i].value + "'");

            }

            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = myConnection.CreateCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = sqlQuery.ToString();

            try
            {
                myConnection.Open();
                // get experiment ids from table experiments
                DbDataReader myReader = myCommand.ExecuteReader();
                ArrayList eIDs = new ArrayList();

                while (myReader.Read())
                {
                    if (myReader["experiment_id"] != System.DBNull.Value)
                        eIDs.Add(myReader["experiment_id"]);
                }

                myReader.Close();
                // Converting to a string array
                experimentIDs = new long[eIDs.Count];
                for (int i = 0; i < eIDs.Count; i++)
                {
                    experimentIDs[i] = Convert.ToInt64(eIDs[i]);
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Exception thrown finding experiment", ex);
            }
            finally
            {
                myConnection.Close();
            }

            return experimentIDs;


        }
/*
        /// <summary>
        /// finds all of those Experiments specified in experimentIDs which possess attributes that match the logical AND of conditions expressed in the Criterion array. The search is limited to those experiments that the current user created or for which he or she has a “ReadExperiment” grant. The Criterion conditions must be satisfied elements of the Experiment’s administrative data model such as its ownerID or by the RecordAttributes of a single ExperimentRecord belonging to the experiment for it to qualify. 
        /// </summary>
        /// <param name="criteria">The array of Criterion objects that specify the attributes of the requested experiments; all experimentIDs match if null.</param>
        /// <returns>an array of the IDs of Experiments that match the search criteria</returns>
        // Need to stub in alternate predicates and attribute hash maps. - CV 07/08/04
        public static ExperimentSummary[] SelectExperiments(Criterion[] criteria)
        {
            StringBuilder sqlQuery = new StringBuilder();
            long[] experimentIDs;

            sqlQuery.Append("select experiment_id ");
            sqlQuery.Append(" from experiments where ");
            for (int i = 0; i < criteria.Length; i++)
            {
                if (i != 0)
                {
                    sqlQuery.Append(" AND ");
                }
                sqlQuery.Append(criteria[i].attribute + " " + criteria[i].predicate + " '" + criteria[i].value + "'");

            }

            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.GetConnection();
            myCommand.Connection = myConnection;
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = sqlQuery.ToString();

            try
            {
                myConnection.Open();
                // get experiment ids from table experiments
                DbDataReader myReader = myCommand.ExecuteReader();
                ArrayList eIDs = new ArrayList();

                while (myReader.Read())
                {
                    if (myReader["experiment_id"] != System.DBNull.Value)
                        eIDs.Add(myReader["experiment_id"]);
                }

                myReader.Close();
                // Converting to a string array
                experimentIDs = new long[eIDs.Count];
                for (int i = 0; i < eIDs.Count; i++)
                {
                    experimentIDs[i] = Convert.ToInt64(eIDs[i]);
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Exception thrown finding experiment", ex);
            }
            finally
            {
                myConnection.Close();
            }

            return SelectExperimentSummary(myConnection, experimentIDs);
        }
 * */
/*
        /// <summary>
        /// finds all of those Experiments specified in experimentIDs which possess attributes that match the logical AND of conditions expressed in the Criterion array. The search is limited to those experiments that the current user created or for which he or she has a “ReadExperiment” grant. The Criterion conditions must be satisfied elements of the Experiment’s administrative data model such as its ownerID or by the RecordAttributes of a single ExperimentRecord belonging to the experiment for it to qualify. 
        /// </summary>
        /// <param name="criteria">The array of Criterion objects that specify the attributes of the requested experiments; all experimentIDs match if null.</param>
        /// <returns>an array of the IDs of Experiments that match the search criteria</returns>
        // Need to stub in alternate predicates and attribute hash maps. - CV 07/08/04
        public static ExperimentSummary[] SelectExperimentInfo(long[] expIDs)
        {
            ExperimentSummary[] experiments = null;
            DbConnection myConnection = FactoryDB.GetConnection();
            try
            {
                experiments = SelectExperimentSummary(myConnection, expIDs);
            }
            catch (Exception e)
            {
            }
            finally
            {
                myConnection.Close();
            }
            return experiments;

        }
     */  
        /// <summary>
        /// Creates a populated ESS proxy if the specified experiment has an associated ESS.
        /// </summary>
        /// <param name="experimentID"></param>
        /// <returns>a valid ESS proxy, null if the experiment does not have an ESS</returns>
        public static ExperimentStorageProxy GetEssProxy(long experimentID){

            ExperimentStorageProxy proxy = null;
            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("Experiment_RetrieveEssInfo", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.Parameters.Add(FactoryDB.CreateParameter("@experimentID", experimentID,DbType.Int64));
            
            try
            {
                myConnection.Open();
                DbDataReader myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    proxy = new ExperimentStorageProxy();
                    proxy.AgentAuthHeaderValue = new AgentAuthHeader();
                    Coupon coupon = new Coupon();
                    coupon.couponId = (long) myReader.GetInt64(0);
                    coupon.issuerGuid = (string)myReader.GetString(1);
                    coupon.passkey = (string) myReader.GetString(2);
                    proxy.AgentAuthHeaderValue.coupon = coupon;
                    proxy.AgentAuthHeaderValue.agentGuid = ProcessAgentDB.ServiceGuid;
                    proxy.Url = (string) myReader.GetString(3);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception thrown updateing ESSinfo", ex);
            }
            finally
            {
                myConnection.Close();
            }
            return proxy;
        }
     

        public static bool UpdateExperimentESSInfo(long experimentID, long essExpID, int agentId)
        {
            bool status = false;
            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("Experiment_UpdateEssInfo", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.Parameters.Add(FactoryDB.CreateParameter("@experimentID", experimentID, DbType.Int64));
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@essExpID", essExpID, DbType.Int64));
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@essID", agentId, DbType.Int32));
            try
            {
                myConnection.Open();
                myCommand.ExecuteNonQuery();
                status = true;
            }
            catch (Exception ex)
            {
                throw new Exception("Exception thrown updateing ESSinfo", ex);
            }
            finally
            {
                myConnection.Close();
            }
            return status;
        }

        /** ExperimentCoupon **/

        public static int DeleteExperimentCoupon(long experimentID, long couponID)
        {
            int count = 0;
            DbConnection connection = FactoryDB.GetConnection();
            try
            {
                count = DeleteExperimentCoupon(connection, experimentID, couponID);
            }
            catch { }
            finally
            {
                connection.Close();
            }
            return count;
        }

        public static int DeleteExperimentCoupon(DbConnection connection, long experimentID, long couponID)
        {
            DbCommand myCommand = FactoryDB.CreateCommand("ExperimentCoupon_Delete", connection);
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@experimentID", experimentID, DbType.Int64));
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@couponID", couponID, DbType.Int64));
            connection.Open();
            int count = Convert.ToInt32(myCommand.ExecuteScalar());
            return count;
        }

        public static void InsertExperimentCoupon(long experimentID, long couponID)
        {
            DbConnection connection = FactoryDB.GetConnection();
            try
            {
                InsertExperimentCoupon(connection, experimentID, couponID);
            }
            catch { }
            finally
            {
                connection.Close();
            }
        }
        public static void InsertExperimentCoupon(DbConnection connection, long experimentID, long couponID)
        {
            DbCommand myCommand = FactoryDB.CreateCommand("ExperimentCoupon_Insert", connection);
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@experimentID", experimentID, DbType.Int64));
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@couponID", couponID, DbType.Int64));
            connection.Open();
            myCommand.ExecuteNonQuery();

        }

        public static long[] RetrieveExperimentCouponIDs(long experimentID)
        {
            long[] coupons = null;
            DbConnection connection = FactoryDB.GetConnection();
            try
            {
                coupons = RetrieveExperimentCouponIDs(connection, experimentID);
            }
            catch { }
            finally
            {
                connection.Close();
            }

            return coupons;
        }

        public static long[] RetrieveExperimentCouponIDs(DbConnection connection, long experimentID)
        {
            long[] coupons = null;
            ArrayList ids = new ArrayList();
            DbCommand myCommand = FactoryDB.CreateCommand("ExperimentCouponID_Retrieve", connection);
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@experimentID", experimentID, DbType.Int64));
            connection.Open();
            DbDataReader reader = myCommand.ExecuteReader();
            while (reader.Read())
            {
                long id = reader.GetInt64(0);
                ids.Add(id);
            }
            coupons = Utilities.ArrayListToLongArray(ids);
            return coupons;
        }


        /* !------------------------------------------------------------------------------!
         *							CALLS FOR CLIENT ITEMS
         * !------------------------------------------------------------------------------!
         */

        /// <summary>
        /// to add a client item
        /// </summary>
        public static void SaveClientItem(int clientID, int userID, string itemName, string itemValue)
        {
            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("ClientItem_Save", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@clientID", clientID, DbType.Int64));
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@userID", userID, DbType.Int32));
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@itemName", itemName, DbType.String,256));
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@itemValue", itemValue, DbType.String));

            try
            {
                myConnection.Open();
                myCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Exception thrown in inserting client item", ex);
            }
            finally
            {
                myConnection.Close();
            }
        }

        /// <summary>
        /// to delete the client item specified by the combination of clientID, userID and itemName
        /// </summary>
        public static void DeleteClientItem(int clientID, int userID, string itemName)
        {
            //string previousItem =  SelectClientItemValue(clientID,userID,itemName);
            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("ClientItem_Delete", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@clientID", clientID, DbType.Int64));
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@userID", userID, DbType.Int32));
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@itemName", itemName, DbType.String,256));

            try
            {
                myConnection.Open();
                int i = myCommand.ExecuteNonQuery();
                if (i == 0)
                    throw new Exception("No record exists exception");
            }
            catch (Exception ex)
            {
                throw new Exception("Exception thrown in DeleteClientItem", ex);
            }
            finally
            {
                myConnection.Close();
            }
            //return previousItem;
        }

        /// <summary>
        /// to retrieve a list of all the item names in the database for a client -user combo
        /// </summary>
        public static string[] SelectClientItems(int clientID, int userID)
        {
            string[] clientItemIDs;

            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("ClientItem_RetrieveNames", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@clientID", clientID, DbType.Int32));
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@userID", userID, DbType.Int32));

            try
            {
                myConnection.Open();


                // get ClientItem Names from table stored_item_summary
                DbDataReader myReader = myCommand.ExecuteReader();
                ArrayList citems = new ArrayList();

                while (myReader.Read())
                {
                    if (myReader["item_name"] != System.DBNull.Value)
                        citems.Add((string)myReader["item_name"]);
                    //	clientItemIDs [i] = (string) myReader["item_name"];
                }
                myReader.Close();

                // Converting to a string array
                clientItemIDs = Utilities.ArrayListToStringArray(citems);
            }
            catch (Exception ex)
            {
                throw new Exception("Exception thrown SelectClientItemIDs", ex);
            }
            finally
            {
                myConnection.Close();
            }

            return clientItemIDs;
        }


        /// <summary>
        /// to retrieve user the item value for client items specified the combination of clientID, userID and itemName 
        /// </summary>
        public static string SelectClientItemValue(int clientID, int userID, string itemName)
        {
            ArrayList clientItems = new ArrayList();

            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("ClientItem_RetrieveValue", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@clientID", clientID, DbType.Int32));
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@userID", userID, DbType.Int32));
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@itemName", itemName, DbType.String,256));

            try
            {
                myConnection.Open();

                // get ClientItem info from table client_items
                return myCommand.ExecuteScalar().ToString();

            }
            catch (Exception ex)
            {
                throw new Exception("Exception thrown SelectClientItemValue", ex);
            }
            finally
            {
                myConnection.Close();
            }
        }

 
    }
}
