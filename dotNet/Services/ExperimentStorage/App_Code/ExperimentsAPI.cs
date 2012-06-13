/* $Id$ */

using System;
using System.Data;
using System.Configuration;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;

using iLabs.Core;
using iLabs.DataTypes;
using iLabs.DataTypes.TicketingTypes;
using iLabs.DataTypes.StorageTypes;
using iLabs.Ticketing;
using iLabs.UtilLib;

namespace iLabs.ExpStorage
{
    public class ExperimentsAPI
    {
        public ExperimentsAPI()
        {

        }

        /// <summary>
        /// Opens an Experiment object on the ESS, if the experiment does not exist a record is created.
        /// 
        /// </summary>
        /// <param name="duration">the number of minutes before the experiment times out and is closed</param>
        /// <returns>the ID of the new Experiment object</returns>
        /// <remarks>a duration of value -1 indicates a indefinite duration</remarks>
        public int OpenExperiment(long duration, long sbExperimentId, string sbGuid)
        {
            long experimentId = -1;
            int code = -1;

            DbConnection myConnection = FactoryDB.GetConnection();
            try
            {
                myConnection.Open();
                StorageStatus status = GetExperimentStatus(myConnection, sbExperimentId, sbGuid);
                if (status == null)
                {
                    experimentId = CreateExperiment(myConnection, duration, sbExperimentId, sbGuid, StorageStatus.OPEN);
                    code = StorageStatus.OPEN;
                }
                else
                {
                    DateTime scheduledClose = DateTime.MinValue;
                    if (duration > 0)
                        scheduledClose = DateTime.UtcNow.AddTicks(duration * TimeSpan.TicksPerSecond);
                    code = UpdateExperiment(myConnection, sbExperimentId, sbGuid, StorageStatus.REOPENED,
                        scheduledClose, DateTime.MinValue);
                }
            }

            catch
            {
                throw;
            }
            finally
            {
                myConnection.Close();
            }

            return code;
        }

        public long CreateExperiment(DbConnection myConnection, long duration, long sbExperimentId, string sbGuid, int status)
        {
            long experimentId = -1;

            DbCommand myCommand = FactoryDB.CreateCommand("CreateExperiment", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            myCommand.Parameters.Add(FactoryDB.CreateParameter("@experimentID", sbExperimentId, DbType.Int64));
            myCommand.Parameters.Add(FactoryDB.CreateParameter("@issuerGuid", sbGuid, DbType.AnsiString,50));
            
            if (duration != -1)
            {
                DateTime scheduledClose = DateTime.UtcNow.AddTicks(duration * TimeSpan.TicksPerSecond);
                myCommand.Parameters.Add(FactoryDB.CreateParameter("@scheduledClose", scheduledClose, DbType.DateTime));
            }
            else
            {
                myCommand.Parameters.Add(FactoryDB.CreateParameter("@scheduledClose", null, DbType.DateTime));
            }
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@status", status, DbType.Int32));
            try
            {
                experimentId = Convert.ToInt64(myCommand.ExecuteScalar());
            }
            catch
            {
                throw;
            }
            return experimentId;
        }

        public int UpdateExperiment(DbConnection myConnection, long sbExperimentId, string sbGuid,
            int statusCode, DateTime scheduledClose, DateTime closeTime)
        {
            DateTime close_time = new DateTime();
            int status = -1;
            DbCommand myCommand = FactoryDB.CreateCommand("UpdateExperiment", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.Parameters.Add(FactoryDB.CreateParameter("@experimentID", sbExperimentId, DbType.Int64));
            myCommand.Parameters.Add(FactoryDB.CreateParameter("@issuerGuid", sbGuid, DbType.AnsiString,50));
            myCommand.Parameters.Add(FactoryDB.CreateParameter("@status", statusCode, DbType.Int32));
            if (scheduledClose != DateTime.MinValue && scheduledClose != FactoryDB.MinDbDate)
            {
                 myCommand.Parameters.Add(FactoryDB.CreateParameter("@scheduledClose", scheduledClose, DbType.DateTime));
            }
            else
            {
                 myCommand.Parameters.Add(FactoryDB.CreateParameter("@scheduledClose", null, DbType.DateTime));
            }
            if (closeTime != null && closeTime != DateTime.MinValue && closeTime != FactoryDB.MinDbDate)
            {
                 myCommand.Parameters.Add(FactoryDB.CreateParameter("@closeTime", closeTime, DbType.DateTime));
            }
            else
            {
                 myCommand.Parameters.Add(FactoryDB.CreateParameter("@closeTime", null, DbType.DateTime));
            }
            try
            {
                status = Convert.ToInt32(myCommand.ExecuteScalar());
            }
            catch
            {
                throw;
            }
            return status;
        }

        public bool CloseExperiment(long experimentId, string guid)
        {
            return CloseExperiment(experimentId, guid, StorageStatus.CLOSED);
        }
        /// <summary>
        /// Closes an Experiment on the ESS so that no further ExperimentRecords or BLOBs can be written to it; 
        /// BLOBs that have been created for this Experiment but have not been associated with an ExperimentRecord 
        /// are deleted by this method 
        /// </summary>
        /// <param name="experimentId">the ID of the experiment to be closed</param>
        /// <returns>true if the experiment was successfully closed; false otherwise</returns>
        public bool CloseExperiment(long experimentId, string guid, int statusCode)
        {
            DbConnection myConnection = FactoryDB.GetConnection();

            try
            {
                myConnection.Open();
                return CloseExperiment(myConnection, experimentId, guid, statusCode);
            }
            catch
            {
                throw;
            }
            finally
            {
                myConnection.Close();
            }
        }

        public bool CloseExperiment(DbConnection myConnection, long experimentId, string guid, int statusCode)
        {
            DbCommand myCommand = FactoryDB.CreateCommand("CloseExperiment", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.Parameters.Add(FactoryDB.CreateParameter("@experimentId", experimentId, DbType.Int64));
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@issuerGuid", guid, DbType.AnsiString,50));
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@statusCode", statusCode, DbType.Int32));

            try
            {

                int rows = myCommand.ExecuteNonQuery();

                return (rows > 0);
            }
            catch
            {
                throw;
            }
        }

        public StorageStatus GetExperimentStatus(long experimentID, string guid)
        {
            StorageStatus status = null;
            DbConnection connection = FactoryDB.GetConnection();
            try
            {
                connection.Open();
                status = GetExperimentStatus(connection, experimentID, guid);
            }
            finally
            {
                connection.Close();
            }
            return status;
        }



        public StorageStatus GetExperimentStatus(DbConnection connection, long experimentID, string guid)
        {
            StorageStatus status = null;

            try
            {
                DbCommand myCommand = FactoryDB.CreateCommand("GetExperimentStatus", connection);
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.Parameters.Add(FactoryDB.CreateParameter( "@experimentId", experimentID, DbType.Int64));
                myCommand.Parameters.Add(FactoryDB.CreateParameter( "@issuerGuid", guid, DbType.AnsiString,50));
                DbDataReader reader = myCommand.ExecuteReader();
                while (reader.Read())
                {
                    status = new StorageStatus();

                    status.experimentId = reader.GetInt64(0);
                    status.status = reader.GetInt32(1);
                    status.recordCount = reader.GetInt32(2);
                    if (!reader.IsDBNull(3))
                        status.creationTime = DateUtil.SpecifyUTC(reader.GetDateTime(3));
                    if (!reader.IsDBNull(4))
                        status.closeTime = DateUtil.SpecifyUTC(reader.GetDateTime(4));
               ;
                    if (!reader.IsDBNull(5))
                        status.lastModified = DateUtil.SpecifyUTC(reader.GetDateTime(5));
                    status.issuerGuid = reader.GetString(6);
                }
                reader.Close();

            }
            catch (Exception e)
            {
               Logger.WriteLine("GetExperimentStatus: " + e.Message);
                throw;
            }


            return status;
        }

        public StorageStatus SetExperimentStatus(long experimentID, string guid, int statusCode)
        {
            StorageStatus status = null;
            DbConnection connection = FactoryDB.GetConnection();
            try
            {
                connection.Open();
                setExperimentStatus(connection, experimentID, guid, statusCode);
                status = GetExperimentStatus(connection, experimentID, guid);
            }
            finally
            {
                connection.Close();
            }
            return status;
        }
        private bool setExperimentStatus(DbConnection connection, long experimentID, string guid, int statusCode)
        {

            DbCommand myCommand = FactoryDB.CreateCommand("SetExperimentStatus", connection);
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@experimentId", experimentID, DbType.Int64));
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@issuerGuid", guid, DbType.AnsiString,50));
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@statusCode", statusCode, DbType.Int32));
            try
            {

                int count = (int)myCommand.ExecuteScalar();
                return count > 0;

            }
            catch
            {
                throw;
            }
        }

        public long[] GetEssExpIDs(DbConnection connection, long[] expIds, string issuer)
        {
            List<long> essIds = new List<long>();
            StringBuilder sql = new StringBuilder("select essExp_Id from experiments where issuer_GUID ='");
            sql.Append(issuer + "' ");
            if (expIds != null && expIds.Length > 0)
            {
                sql.Append("AND experiment_ID IN (");
                for (int i = 0; i < expIds.Length; i++)
                {
                    if (i > 0)
                    {
                        sql.Append(", ");
                    }
                    sql.Append(expIds[i]);
                }
                sql.Append(")");
            }

            DbCommand myCommand = FactoryDB.CreateCommand(sql.ToString(), connection);


            try
            {
                connection.Open();
                // get experiment ids from table experiments
                DbDataReader myReader = myCommand.ExecuteReader();

                while (myReader.Read())
                {
                    if (!myReader.IsDBNull(0))
                        essIds.Add(myReader.GetInt64(0));
                }

                myReader.Close();

            }
            finally
            {
                connection.Close();
            }
            return essIds.ToArray();

        }

        /// <summary>
        /// Get an array of experimentIDs from the specified set that match the criterian.
        /// </summary>
        /// <param name="expSet"></param>
        /// <param name="issuerGuid"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public long[] GetExperimentIDs(long[] expSet, string issuerGuid, Criterion[] filter)
        {
            string expIds = Utilities.ToCSV(expSet);
            List<long> essIds = new List<long>();
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("select Experiment_id from experiments where EssExp_id in ");

           
            // Only use the expSet
            sql.AppendLine("( select EssExp_ID from experiments where experiment_id in ( ");
            sql.Append(expIds);
            sql.Append(") AND issuer_guid = '");
            sql.Append(issuerGuid);
            sql.AppendLine("' ) ");

           
            if (filter != null && filter.Length > 0)
            {
              

                StringBuilder fields = new StringBuilder();
                StringBuilder attributes = new StringBuilder();
                int fieldCount = 0;
                int attributeCount = 0;
                foreach (Criterion c in filter)
                {
                    if (c.attribute.ToLower().CompareTo("record_count") == 0){
                        sql.Append(" AND EssExp_id in ( select essExp_id from experiments where Current_Sequence_No ");
                        sql.Append(c.predicate);
                        sql.Append( c.value );
                        sql.AppendLine(")");
                    }
                    else if ((c.attribute.ToLower().CompareTo("record_type") == 0)
                        || (c.attribute.ToLower().CompareTo("contents") == 0))
                    {
                        if (fieldCount > 0)
                        {
                            fields.Append(" AND");
                        }
                        fields.Append(" " + c.attribute);
                        fields.Append(c.predicate + " ");
                        fields.Append("'" + c.value + "' ");
                        fieldCount++;
                    }
                    else
                    {
                        if (attributeCount > 0)
                        {
                            attributes.Append(" AND ");
                        }
                        attributes.Append(" ( attribute_name = '" + c.attribute + "' AND attribute_value ");
                        attributes.Append(c.predicate + " ");
                        attributes.Append("'" + c.value + "' )");
                        attributeCount++;
                    }


                }
                if (fieldCount > 0)
                {
                    sql.AppendLine(" AND EssExp_id in ( Select Distinct ESSExp_id from Experiment_Records where ");
                    sql.Append(fields.ToString());
                    sql.AppendLine(")");
                }
                
                if (attributeCount > 0)
                {
                    sql.AppendLine(" AND EssExp_id in ( Select Distinct ESSExp_id from Record_Attributes where ");
                   
                    sql.AppendLine(attributes.ToString() + " )");
                }


                //sql.AppendLine(") ");
            }
            //sql.AppendLine(")");
            DbConnection connection = FactoryDB.GetConnection();
           Logger.WriteLine(sql.ToString());
            DbCommand myCommand = FactoryDB.CreateCommand(sql.ToString(), connection);


            try
            {
                connection.Open();
                // get experiment ids from table experiments
                DbDataReader myReader = myCommand.ExecuteReader();

                while (myReader.Read())
                {
                    if (!myReader.IsDBNull(0))
                        essIds.Add(myReader.GetInt64(0));
                }

                myReader.Close();

            }
            finally
            {
                connection.Close();
            }
            return essIds.ToArray();

        }



        /// <summary>
        /// Returns the number of minutes since the last "write action" to the Experiment
        /// </summary>
        /// <param name="experimentId">the ID of the experiment whose idle time is sought</param>
        /// <returns>the idle time in minutes</returns>
        public int GetIdleTime(long experimentId, string sbGuid)
        {
            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("RetrieveExperimentIdleTime", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.Parameters.Add(FactoryDB.CreateParameter("@experimentId", experimentId, DbType.Int64));
            myCommand.Parameters.Add(FactoryDB.CreateParameter("@issuerGuid", sbGuid, DbType.AnsiString,50));
            try
            {
                myConnection.Open();
                DateTime lastmod = (DateTime)myCommand.ExecuteScalar();
                TimeSpan ts = (DateTime.Now - lastmod);
                return (int)ts.TotalMinutes;
            }
            catch
            {
                throw;
            }
            finally
            {
                myConnection.Close();
            }
        }

        /// <summary>
        /// Deletes an experiment object and all its associated ExperimentRecords and BLOBs on the ESS
        /// </summary>
        /// <param name="experimentId">the ID of the experiment to be deleted</param>
        /// <returns>true if the Experiment was successfully deleted</returns>
        public bool DeleteExperiment(long experimentId, string sbGuid)
        {
            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("DeleteExperiment", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.Parameters.Add(FactoryDB.CreateParameter("@experimentId", experimentId, DbType.Int64));
            myCommand.Parameters.Add(FactoryDB.CreateParameter("@issuerGuid", sbGuid, DbType.AnsiString,50));

            try
            {
                myConnection.Open();
                int rows = myCommand.ExecuteNonQuery();
                return (rows > 0);

            }
            catch
            {
                throw;
            }
            finally
            {
                myConnection.Close();
            }
        }

        /// <summary>
        /// Adds a new ExperimentRecord to a pre-existing Experiment object
        /// </summary>
        /// <param name="experimentId">the ID of the Experiment to receive the new ExperimentRecord</param>
        /// <param name="type">a string that designates the type of record</param>
        /// <param name="xmlSearchable">true if the contents field can be searched to find attributes to match Criterion conditions</param>
        /// <param name="contents">the payload of the record</param>
        /// <param name="attributes">an array of attributes/values pairs to be attached to the record for search purposes</param>
        /// <returns>the sequence number of the newly added ExperimentRecord</returns>
        public int AddRecord(long experimentId, string sbGuid, string submitter, string type, bool xmlSearchable,
            string contents, RecordAttribute[] attributes)//, recordCreatorName
        {
            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("AddExperimentRecord", myConnection);
            DbTransaction tran = null;
            myCommand.CommandType = CommandType.StoredProcedure;

            ////Get the Service Broker record from the database
            //ProcessAgentInfo[] paInfo = dbTicketing.GetProcessAgentInfos(ProcessAgentType.SERVICE_BROKER);

            ////Get the Service Broker name - This is the sponsor of the "ADMINISTER EXPERIMENT" ticket
            //string sponsorGuid = paInfo[0].agentGuid;

            //myCommand.Parameters.Add(FactoryDB.CreateParameter("@sponsorGuid", sponsorGuid));

            myCommand.Parameters.Add(FactoryDB.CreateParameter("@experimentId", experimentId, DbType.Int64));
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@issuerGuid", sbGuid, DbType.AnsiString,50));
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@submitter", submitter, DbType.String,256));
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@type", type, DbType.AnsiString,100));
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@xmlSearchable", xmlSearchable, DbType.Boolean));
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@contents", contents, DbType.String));

            //NOTE: TEMPORARY PARAMETER
            //myCommand.Parameters.Add(FactoryDB.CreateParameter("@submitterName", "Service Broker"));
            //myCommand.Parameters.Add(FactoryDB.CreateParameter("@submitterName", recordCreatorName));


            try
            {
                myConnection.Open();
                tran = myConnection.BeginTransaction();
                myCommand.Transaction = tran;

                int sequenceNo = Convert.ToInt32(myCommand.ExecuteScalar());
                if (sequenceNo == -1)
                    return -1;

                if (attributes != null)
                {
                    myCommand.Parameters.Clear();
                    myCommand.CommandText = "AddRecordAttribute";
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.Parameters.Add(FactoryDB.CreateParameter( "@experimentId", experimentId, DbType.Int64));
                    myCommand.Parameters.Add(FactoryDB.CreateParameter("@issuerGuid", sbGuid, DbType.AnsiString,50));
                    myCommand.Parameters.Add(FactoryDB.CreateParameter("@sequenceNo", sequenceNo, DbType.Int32));
                    myCommand.Parameters.Add(FactoryDB.CreateParameter("@attributeName", null, DbType.String,256));
                    myCommand.Parameters.Add(FactoryDB.CreateParameter("@attributeValue", null, DbType.String,256));

                    foreach (RecordAttribute a in attributes)
                    {
                        myCommand.Parameters["@attributeName"].Value = a.name;
                        myCommand.Parameters["@attributeValue"].Value = a.value;
                        myCommand.ExecuteNonQuery();
                    }
                }

                tran.Commit();
                return sequenceNo;
            }
            catch
            {
                try
                {
                    tran.Rollback();
                }
                catch
                {
                    throw;
                }

                throw;
            }
            finally
            {
                myConnection.Close();
            }
        }

        /*
         * declare @essID bigint
         * select @essID=EssExp_ID from experiments where Experiment_id = @exp_id and issuer_guid = @issuerGuid
         * 
         select sequence_no from Experiment_Records 
         * where (EssExp_ID = @essID AND ( (contents like 'data%') OR (record_type = "image"))
         * OR ( record_id in ( select record_id from record_attributes 
         * where record_id in ( select record_id from experiment_records where essExp_id=@essID
         * AND( (attribute_name='attribute' and attribute_value ='value')
         *     OR (attribute_name='attributeN' and attribute_value ='valueN')
         * */

        /// <summary>
        /// Returns the specified ExperimentRecord sequence numbers
        /// </summary>
        /// <param name="experimentId">the ID of the Experiment whose record is to be retrieved</param>
        /// <param name="sequenceNum">the sequence number of the record to be retrieved</param>
        /// <returns>the specified ExperimentRecord</returns>
        public int[] GetRecordNumbers(long experimentId, string sbGuid, Criterion[] criteria)
        {
            List<int> records = new List<int>();
            
            if (criteria != null && criteria.Length > 0)
            {
                bool hasFields = false;
                bool hasAttributes = false;
                StringBuilder sql = new StringBuilder();             
                StringBuilder attributes = new StringBuilder();
                
                for (int i = 0; i < criteria.Length; i++)
                {
                    switch (criteria[i].attribute.ToLower())
                    {
                        //these criterion are based on fields.
                        case "record_type":
                        case "contents":
                        case "submitter_name":
                            if(hasFields)
                                sql.Append(" AND ");
                            sql.Append(criteria[i].ToSQL());
                            hasFields = true;
                            break;
                        default: // it's an attribute
                            if(hasAttributes)
                            attributes.Append(" AND ");
                            attributes.Append("(attribute_name = '" + criteria[i].attribute + "' ");
                            attributes.Append(" AND attribute_value ");
                           attributes.Append(criteria[i].predicate);
                            attributes.Append(" '" + criteria[i].value + "')");
                            hasAttributes = true;
                            break;
                    }
                 }
                 DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("RetrieveExperimentRecordNumbersCriteria", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.Parameters.Add(FactoryDB.CreateParameter("@experimentId", experimentId, DbType.Int64));
            myCommand.Parameters.Add(FactoryDB.CreateParameter("@issuerGuid", sbGuid, DbType.AnsiString,50));
            if (hasFields)
                myCommand.Parameters.Add(FactoryDB.CreateParameter("@fieldQuery",sql.ToString(), DbType.AnsiString,2000));
            else
                myCommand.Parameters.Add(FactoryDB.CreateParameter("@fieldQuery",null, DbType.AnsiString,2000));
            if (hasAttributes)
                myCommand.Parameters.Add(FactoryDB.CreateParameter("@attributeQuery", attributes.ToString(), DbType.AnsiString, 2000));
            else
                myCommand.Parameters.Add(FactoryDB.CreateParameter("@attributeQuery", null, DbType.AnsiString, 2000));
            try{
                myConnection.Open();

                // get experiment record information from table experiment_records
                DbDataReader myReader = myCommand.ExecuteReader();

                while (myReader.Read())
                {
                    records.Add(myReader.GetInt32(0));
                }
                }
                catch(Exception ex){
                    throw ex;
                }
                finally{
                    myConnection.Close();
                }
            }
            else{ // get all the sequence numbers
            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("RetrieveExperimentRecordNumbers", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.Parameters.Add(FactoryDB.CreateParameter("@experimentId", experimentId, DbType.Int64));
            myCommand.Parameters.Add(FactoryDB.CreateParameter("@issuerGuid", sbGuid, DbType.AnsiString,50));

            try
            {
                myConnection.Open();

                // get experiment record information from table experiment_records
                DbDataReader myReader = myCommand.ExecuteReader();

                while (myReader.Read())
                {
                     records.Add(myReader.GetInt32(0));
                }

                myReader.Close();

            }
            catch
            {
                throw;
            }
            finally
            {
                myConnection.Close();
            }
            }
            return records.ToArray(); ;
        }

        /// <summary>
        /// Returns the specified ExperimentRecord sequence numbers
        /// </summary>
        /// <param name="experimentId">the ID of the Experiment whose record is to be retrieved</param>
        /// <param name="sequenceNum">the sequence number of the record to be retrieved</param>
        /// <returns>the specified ExperimentRecord</returns>
        public ExperimentRecord[] GetRecords(long experimentId, string sbGuid, Criterion[] criteria)
        {
            List<ExperimentRecord> records = new List<ExperimentRecord>();
            ExperimentRecord expRecord = null;
            if (criteria != null && criteria.Length > 0)
            {
                bool hasFields = false;
                bool hasAttributes = false;
                StringBuilder sql = new StringBuilder();
                StringBuilder attributes = new StringBuilder();

                for (int i = 0; i < criteria.Length; i++)
                {
                    switch (criteria[i].attribute.ToLower())
                    {
                        //these criterion are based on fields.
                        case "record_type":
                        case "contents":
                        case "submitter_name":
                            if (hasFields)
                                sql.Append(" AND ");
                            sql.Append(criteria[i].ToSQL());
                            hasFields = true;
                            break;
                        default: // it's an attribute
                            if (hasAttributes)
                                attributes.Append(" AND ");
                            attributes.Append("(attribute_name = '" + criteria[i].attribute + "' ");
                            attributes.Append(" AND attribute_value ");
                            attributes.Append(criteria[i].predicate);
                            attributes.Append(" '" + criteria[i].value + "')");
                            hasAttributes = true;
                            break;
                    }
                }
                DbConnection myConnection = FactoryDB.GetConnection();
                DbCommand myCommand = FactoryDB.CreateCommand("RetrieveExperimentRecordsCriteria", myConnection);
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.Parameters.Add(FactoryDB.CreateParameter("@experimentId", experimentId, DbType.Int64));
                myCommand.Parameters.Add(FactoryDB.CreateParameter("@issuerGuid", sbGuid, DbType.AnsiString,50));
                if (hasFields)
                    myCommand.Parameters.Add(FactoryDB.CreateParameter("@fieldQuery", sql.ToString(), DbType.AnsiString,2000));
                else
                    myCommand.Parameters.Add(FactoryDB.CreateParameter("@fieldQuery", null, DbType.AnsiString,2000));
                if (hasAttributes)
                    myCommand.Parameters.Add(FactoryDB.CreateParameter("@attributeQuery", attributes.ToString(), DbType.AnsiString,2000));
                else
                    myCommand.Parameters.Add(FactoryDB.CreateParameter("@attributeQuery", null, DbType.AnsiString,2000));
                try
                {
                    myConnection.Open();

                    // get experiment record information from table experiment_records
                    DbDataReader myReader = myCommand.ExecuteReader();

                    while (myReader.Read())
                    {
                        expRecord = new ExperimentRecord();
                        if (myReader["record_type"] != System.DBNull.Value)
                            expRecord.type = (string)myReader["record_type"];


                        if (myReader["submitter_Name"] != System.DBNull.Value)
                            expRecord.submitter = (string)myReader["submitter_Name"];

                        if (myReader["contents"] != System.DBNull.Value)
                            expRecord.contents = (string)myReader["contents"];

                        if (myReader["time_stamp"] != System.DBNull.Value)
                            expRecord.timestamp = DateUtil.SpecifyUTC((DateTime)myReader["time_stamp"]);

                        if (myReader["is_xml_searchable"] != System.DBNull.Value)
                            expRecord.xmlSearchable = (bool)myReader["is_xml_searchable"];

                        if (myReader["sequence_no"] != System.DBNull.Value)
                            expRecord.sequenceNum = (int)myReader["sequence_no"];
                        records.Add(expRecord);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    myConnection.Close();
                }
            }
            else
            { // get all the sequence numbers
                DbConnection myConnection = FactoryDB.GetConnection();
                DbCommand myCommand = FactoryDB.CreateCommand("RetrieveExperimentRecords", myConnection);
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.Parameters.Add(FactoryDB.CreateParameter("@experimentId", experimentId,DbType.Int64));
                myCommand.Parameters.Add(FactoryDB.CreateParameter("@issuerGuid", sbGuid, DbType.AnsiString, 50));

                try
                {
                    myConnection.Open();

                    // get experiment record information from table experiment_records
                    DbDataReader myReader = myCommand.ExecuteReader();

                    while (myReader.Read())
                    {
                        expRecord = new ExperimentRecord();

                        if (myReader["sequence_no"] != System.DBNull.Value)
                            expRecord.sequenceNum = (int)myReader["sequence_no"];

                        if (myReader["record_type"] != System.DBNull.Value)
                            expRecord.type = (string)myReader["record_type"];
                     
                        if (myReader["submitter_Name"] != System.DBNull.Value)
                            expRecord.submitter = (string)myReader["submitter_Name"];

                        if (myReader["contents"] != System.DBNull.Value)
                            expRecord.contents = (string)myReader["contents"];

                        if (myReader["time_stamp"] != System.DBNull.Value)
                            expRecord.timestamp = DateUtil.SpecifyUTC((DateTime)myReader["time_stamp"]);

                        if (myReader["is_xml_searchable"] != System.DBNull.Value)
                            expRecord.xmlSearchable = (bool)myReader["is_xml_searchable"];

                        
                        records.Add(expRecord);
                    }

                    myReader.Close();

                }
                catch
                {
                    throw;
                }
                finally
                {
                    myConnection.Close();
                }
            }
            return records.ToArray(); ;
        }




        /// <summary>
        /// Returns the specified ExperimentRecord
        /// </summary>
        /// <param name="experimentId">the ID of the Experiment whose record is to be retrieved</param>
        /// <param name="sequenceNum">the sequence number of the record to be retrieved</param>
        /// <returns>the specified ExperimentRecord</returns>
        public ExperimentRecord GetRecord(long experimentId, string sbGuid, int sequenceNum)
        {
            ExperimentRecord expRecord = new ExperimentRecord();

            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("RetrieveExperimentRecord", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.Parameters.Add(FactoryDB.CreateParameter("@experimentId", experimentId, DbType.Int64));
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@issuerGuid", sbGuid, DbType.AnsiString,50));
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@sequenceNo", sequenceNum, DbType.Int32));

            try
            {
                myConnection.Open();

                // get experiment record information from table experiment_records
                DbDataReader myReader = myCommand.ExecuteReader();

                while (myReader.Read())
                {
                    if (myReader["record_type"] != System.DBNull.Value)
                        expRecord.type = (string)myReader["record_type"];

                    //Note: Temporary parameter
                    //if(myReader["sponsor_GUID"] != System.DBNull.Value )
                    //	expRecord.sponsorGuid = (string) myReader["sponsor_GUID"];

                    if (myReader["submitter"] != System.DBNull.Value)
                        expRecord.submitter = (string)myReader["submitter"];

                    //if (myReader["submitter_name"] != System.DBNull.Value)
                    //    expRecord.  submitterName = (string)myReader["submitter_name"];

                    if (myReader["contents"] != System.DBNull.Value)
                        expRecord.contents = (string)myReader["contents"];

                    if (myReader["time_stamp"] != System.DBNull.Value)
                        expRecord.timestamp = DateUtil.SpecifyUTC((DateTime)myReader["time_stamp"]);

                    if (myReader["is_xml_searchable"] != System.DBNull.Value)
                        expRecord.xmlSearchable = (bool)myReader["is_xml_searchable"];

                    if (myReader["sequence_no"] != System.DBNull.Value)
                        expRecord.sequenceNum = (int)myReader["sequence_no"];
                }

                myReader.Close();

            }
            catch
            {
                throw;
            }
            finally
            {
                myConnection.Close();
            }

            return expRecord;
        }

        /// <summary>
        /// Deletes the experiment record
        /// </summary>
        /// <param name="experimentId"></param>
        /// <param name="sequenceNO"></param>
        public void DeleteRecord(long experimentId, string sbGuid, int sequenceNO)
        {
            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("DeleteExperimentRecord", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;
            myConnection.Open();
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@experimentId", experimentId, DbType.Int64));
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@issuerGuid", sbGuid, DbType.AnsiString,50));
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@seqNo", sequenceNO, DbType.Int32));
            try
            {
                myCommand.ExecuteNonQuery();

            }
            catch
            {
                throw;
            }
            finally
            {
                myConnection.Close();
            }

        }

        /// <summary>
        /// Returns the specified Experiment including the array of associated ExperimentRecords
        /// </summary>
        /// <param name="experimentId">the ID of the Experiment to be retrieved</param>
        /// <returns>the specified Experiment</returns>
        public Experiment GetExperiment(long experimentId, string sbGuid)
        {
            Experiment exp = new Experiment();
            exp.experimentId = experimentId;
            int recordCt = 0;

            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("RetrieveExperiment", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@experimentId", experimentId, DbType.Int64));
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@issuerGuid", sbGuid, DbType.AnsiString,50));
            try
            {
                myConnection.Open();

                // get experiment record information from table experiments
                DbDataReader myReader = myCommand.ExecuteReader();

                int count = 0;

                while (myReader.Read())
                {
                    count++;
                    //if (myReader["creator_name"] != System.DBNull.Value)
                    //    exp.creatorName = (string)myReader["creator_name"];

                    if (myReader["Issuer_Guid"] != System.DBNull.Value)
                        exp.issuerGuid = (string)myReader["Issuer_Guid"];

                    if (myReader["current_sequence_no"] != System.DBNull.Value)
                        recordCt = Convert.ToInt32(myReader["current_sequence_no"]);
                }
                myReader.Close();

                if (count == 0)
                    return null;

                //Retrieve records for an experiment

                ExperimentRecord[] expRecs = new ExperimentRecord[recordCt];

                DbCommand myCommand2 = FactoryDB.CreateCommand("RetrieveRecordsForExperiment", myConnection);
                myCommand2.CommandType = CommandType.StoredProcedure;
                myCommand2.Parameters.Add(FactoryDB.CreateParameter("@experimentId", experimentId, DbType.Int64));
                myCommand2.Parameters.Add(FactoryDB.CreateParameter("@issuerGuid", sbGuid, DbType.AnsiString,50));
                DbDataReader myReader2 = myCommand2.ExecuteReader();

                int i = 0;
                //Not calling GetExperimentRecords as this is more efficient
                while (myReader2.Read())
                {
                    ExperimentRecord expRecord = new ExperimentRecord();

                    if (myReader2["sequence_no"] != System.DBNull.Value)
                        expRecord.sequenceNum = Convert.ToInt32(myReader2["sequence_no"]);

                    if (myReader2["record_type"] != System.DBNull.Value)
                        expRecord.type = (string)myReader2["record_type"];

                    if (myReader2["submitter_name"] != System.DBNull.Value)
                        expRecord.submitter = (string)myReader2["submitter_name"];

                    //if (myReader2["sponsor_GUID"] != System.DBNull.Value)
                    //   expRecord.sponsorGuid = (string)myReader2["sponsor_GUID"];

                    //if (myReader2["submitter_name"] != System.DBNull.Value)
                    //    expRecord.submitterName = (string)myReader2["submitter_name"];

                    if (myReader2["contents"] != System.DBNull.Value)
                        expRecord.contents = (string)myReader2["contents"];

                    if (myReader2["time_stamp"] != System.DBNull.Value)
                        expRecord.timestamp = DateUtil.SpecifyUTC((DateTime)myReader2["time_stamp"]);

                    if (myReader2["is_xml_searchable"] != System.DBNull.Value)
                        expRecord.xmlSearchable = (bool)myReader2["is_xml_searchable"];

                    if (recordCt > 0)
                        expRecs[i] = expRecord;
                    i++;
                }

                myReader2.Close();
                exp.records = expRecs;

            }
            catch
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
        /// Adds the specified RecordAttributes to an ExperimentRecord
        /// </summary>
        /// <param name="experimentId">the ID of the Experiment whose ExperimentRecord is to receive the new attributes</param>
        /// <param name="sequenceNum">the sequence number of the ExperimentRecord to receive the new attributes</param>
        /// <param name="attributes">the attributes to be added</param>
        /// <returns>an array of the IDs of the newly added attributes</returns>
        public int[] AddAttributes(long experimentId, string sbGuid, int sequenceNum, RecordAttribute[] attributes)
        {
            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("AddRecordAttribute", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            int[] attIDs = new int[attributes.Length];

            try
            {
                myConnection.Open();
                if (attributes != null)
                {
                    myCommand.Parameters.Add(FactoryDB.CreateParameter("@experimentId", experimentId, DbType.Int64));
                    myCommand.Parameters.Add(FactoryDB.CreateParameter("@issuerGuid", sbGuid, DbType.AnsiString,50));
                    myCommand.Parameters.Add(FactoryDB.CreateParameter("@sequenceNo", sequenceNum, DbType.Int32));
                    myCommand.Parameters.Add(FactoryDB.CreateParameter("@attributeName", null,DbType.AnsiString));
                    myCommand.Parameters.Add(FactoryDB.CreateParameter("@attributeValue", null, DbType.AnsiString));
                    int i = 0;

                    foreach (RecordAttribute a in attributes)
                    {
                        myCommand.Parameters["@attributeName"].Value = a.name;
                        myCommand.Parameters["@attributeValue"].Value = a.value;
                        attIDs[i] = Convert.ToInt32(myCommand.ExecuteScalar());
                        i = i + 1;
                    }
                }

                return attIDs;
            }
            catch
            {
                throw;
            }
            finally
            {
                myConnection.Close();
            }
        }

        /// <summary>
        /// Retrieves the specified RecordAttributes of an ExperimentRecord
        /// </summary>
        /// <param name="experimentId">the ID of the Experiment containing the ExperimentRecord whose attributes are to be retrieved</param>
        /// <param name="sequenceNum">the sequence number of the ExperimentRecord whose attributes are to be retrieved</param>
        /// <param name="attributeIDs">the IDs of the attributes to be retrieved; null gets all RecordAttributes</param>
        /// <returns>an array of the requested RecordAttributes</returns>
        public RecordAttribute[] GetRecordAttributes(long experimentId, string sbGuid, int sequenceNum, int[] attributeIDs)
        {
            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("RetrieveRecordAttributeByID", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            RecordAttribute[] attributes = new RecordAttribute[attributeIDs.Length];

            try
            {
                myConnection.Open();
                if (attributes != null)
                {
                    myCommand.Parameters.Add(FactoryDB.CreateParameter("@experimentId", experimentId, DbType.Int64));
                    myCommand.Parameters.Add(FactoryDB.CreateParameter("@issuerGuid", sbGuid, DbType.AnsiString,50));
                    myCommand.Parameters.Add(FactoryDB.CreateParameter("@sequenceNo", sequenceNum, DbType.Int32));
                    myCommand.Parameters.Add(FactoryDB.CreateParameter("@attributeID", null, DbType.Int32));

                    for (int i = 0; i < attributeIDs.Length; i++)
                    {
                        myCommand.Parameters["@attributeID"].Value = attributeIDs[i];
                        DbDataReader myReader = myCommand.ExecuteReader();
                        RecordAttribute att = new RecordAttribute();

                        while (myReader.Read())
                        {
                            if (myReader["attribute_name"] != System.DBNull.Value)
                                att.name = (string)myReader["attribute_name"];
                            if (myReader["attribute_value"] != System.DBNull.Value)
                                att.value = (string)myReader["attribute_value"];
                        }

                        attributes[i] = att;
                        myReader.Close();
                    }
                }

                return attributes;
            }
            catch
            {
                throw;
            }
            finally
            {
                myConnection.Close();
            }
        }

        /// <summary>
        /// Retrieves the IDs of the specified RecordAttributes of an ExperimentRecord
        /// </summary>
        /// <param name="experimentId">the ID of the Experiment containing the ExperimentRecord whose attributes are to be retrieved</param>
        /// <param name="sequenceNum">the sequence number of the ExperimentRecord whose attributes are to be retrieved</param>
        /// <param name="attributeName">the name of the attributes to be retrieved (a record may possess multiple attributes with the same name)</param>
        /// <returns>an array of the IDs of the requested RecordAttributes</returns>
        public RecordAttribute[] GetRecordAttributes(long experimentId, string sbGuid, int sequenceNum, string attributeName)
        {
            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("RetrieveRecordAttributeByName", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            ArrayList attList = new ArrayList();

            try
            {
                myConnection.Open();
                if (attributeName != null)
                {
                    myCommand.Parameters.Add(FactoryDB.CreateParameter( "@experimentId", experimentId, DbType.Int32));
                    myCommand.Parameters.Add(FactoryDB.CreateParameter( "@issuerGuid", sbGuid, DbType.AnsiString,50));
                    myCommand.Parameters.Add(FactoryDB.CreateParameter( "@sequenceNo", sequenceNum, DbType.Int32));
                    myCommand.Parameters.Add(FactoryDB.CreateParameter( "@attributeName", attributeName, DbType.String,256));

                    DbDataReader myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        RecordAttribute att = new RecordAttribute();
                        //						if(myReader["attribute_name"] != System.DBNull.Value )
                        //							att.name = (string) myReader["attribute_name"];
                        att.name = attributeName;
                        if (myReader["attribute_value"] != System.DBNull.Value)
                            att.value = (string)myReader["attribute_value"];
                        attList.Add(att);
                    }


                    myReader.Close();

                }

                RecordAttribute[] attributes = new RecordAttribute[attList.Count];
                int[] attributeIDs = new int[attList.Count];

                for (int i = 0; i < attList.Count; i++)
                {
                    attributes[i] = (RecordAttribute)attList[i];
                }

                return attributes;
            }
            catch
            {
                throw;
            }
            finally
            {
                myConnection.Close();
            }
        }

        /// <summary>
        /// Deletes the specified RecordAttributes of an ExperimentRecord
        /// </summary>
        /// <param name="experimentId">the ID of the Experiment containing the ExperimentRecord whose attributes are to be deleted</param>
        /// <param name="sequenceNum">the sequence number of the ExperimentRecord whose attributes are to be deleted</param>
        /// <param name="attributeIDs">the IDs of the attributes to be deleted</param>
        /// <returns>an array of the deleted RecordAttributes</returns>
        public RecordAttribute[] DeleteRecordAttributes(long experimentId, string sbGuid, int sequenceNum, int[] attributeIDs)
        {
            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("DeleteRecordAttribute", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            RecordAttribute[] attributes = new RecordAttribute[attributeIDs.Length];

            try
            {
                myConnection.Open();
                if (attributes != null)
                {
                    myCommand.Parameters.Add(FactoryDB.CreateParameter("@experimentId", experimentId, DbType.Int64));
                    myCommand.Parameters.Add(FactoryDB.CreateParameter( "@issuerGuid", sbGuid, DbType.AnsiString,50));
                    myCommand.Parameters.Add(FactoryDB.CreateParameter( "@sequenceNo", sequenceNum, DbType.Int32));
                    myCommand.Parameters.Add(FactoryDB.CreateParameter( "@attributeID", null, DbType.Int32));

                    for (int i = 0; i < attributeIDs.Length; i++)
                    {
                        myCommand.Parameters["@attributeID"].Value = attributeIDs[i];
                        DbDataReader myReader = myCommand.ExecuteReader();
                        RecordAttribute att = new RecordAttribute();
                        while (myReader.Read())
                        {
                            if (myReader["attribute_name"] != System.DBNull.Value)
                                att.name = (string)myReader["attribute_name"];
                            if (myReader["attribute_value"] != System.DBNull.Value)
                                att.value = (string)myReader["attribute_value"];
                        }

                        attributes[i] = att;
                        myReader.Close();
                    }
                }

                return attributes;
            }
            catch
            {
                throw;
            }
            finally
            {
                myConnection.Close();
            }
        }
    }
}
