/* $Id$ */

using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Configuration;
using System.Web;
using System.IO;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using System.Text;
using System.Text.RegularExpressions;
using System.Collections;

using iLabs.Core;
using iLabs.Ticketing;
using iLabs.DataTypes.StorageTypes;

namespace iLabs.ExpStorage
{

    public class BlobsAPI
    {
        public BlobsAPI()
        {
        }

        /// <summary>
        /// Lists the protocols that the ESS can use to retrieve a BLOB from a source
        /// </summary>
        /// <returns>an array of strings specifying available protocols</returns>
        public string[] GetSupportedBlobImportProtocols()
        {
            string protocolList = ConfigurationManager.AppSettings["blobImportProtocols"];
            string[] protocols = protocolList.Split(new char[] { ',' });

            return protocols;
        }

        /// <summary>
        /// Lists the protocols that a process agent can use to retrieve a BLOB from the ESS
        /// </summary>
        /// <returns>an array of strings specifying available protocols</returns>
        public string[] GetSupportedBlobExportProtocols()
        {
            string protocolList = ConfigurationManager.AppSettings["blobExportProtocols"];
            string[] protocols = protocolList.Split(new char[] { ',' });

            return protocols;
        }

        /// <summary>
        /// Lists the checksum algorithms that a process agent can use to store a BLOB on the ESS
        /// </summary>
        /// <returns>an array of strings specifying available algorithms</returns>
        public string[] GetSupportedChecksumAlgorithms()
        {
            string algorithmList = ConfigurationManager.AppSettings["checksumAlgorithms"];
            string[] checksumAlgorithms = algorithmList.Split(new char[] { ',' });

            return checksumAlgorithms;
        }

        /// <summary>
        /// Creates a new BLOB record on the ESS
        /// </summary>
        /// <param name="experimentId">the ID of the Experiment with which the BLOB is to be associated</param>
        /// <param name="description">a string description suitable for table listing</param>
        /// <param name="byteCount">the byte count of the BLOB used to validate a future download of the binary data</param>
        /// <param name="checksum">a string checksum used to validate a future download of the binary data</param>
        /// <param name="checksumAlgorithm">a string designating the supported algorithm used to calculate the checksum</param>
        /// <returns>the ID of the created BLOB object</returns>
        public long CreateBlob(long experimentId, string sbGuid, string description, int byteCount, string checksum, string checksumAlgorithm)
        {
            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("CreateBlob", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;
            myConnection.Open();

            myCommand.Parameters.Add(FactoryDB.CreateParameter("@experimentId", experimentId, DbType.Int64));
            myCommand.Parameters.Add(FactoryDB.CreateParameter("@issuerGuid", sbGuid, DbType.AnsiString, 50));
            myCommand.Parameters.Add(FactoryDB.CreateParameter("@description", description, DbType.String,2048));
            myCommand.Parameters.Add(FactoryDB.CreateParameter("@byteCount", byteCount, DbType.Int32));
            myCommand.Parameters.Add(FactoryDB.CreateParameter("@checksum", checksum, DbType.AnsiString,256));
            myCommand.Parameters.Add(FactoryDB.CreateParameter("@checksumAlgorithm", checksumAlgorithm, DbType.AnsiString,256));

            try
            {
                long blobId = Convert.ToInt64(myCommand.ExecuteScalar());
                return blobId;
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
        /// Reports the ServiceBroker ExperimentID that owns the BLOB
        /// </summary>
        /// <param name="blobId">the ID of the BLOB whose Experiment is sought</param>
        /// <returns>the experimentId of the Experiment that owns the BLOB with the ID blobId; -1 if there is no such experiment</returns>
        public long GetBlobExperiment(long blobId)
        {
            long experimentId = -1;

            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("RetrieveBlobExperiment", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.Parameters.Add(FactoryDB.CreateParameter("@blobId", blobId,DbType.Int64));

            try
            {
                myConnection.Open();

                Object obj = myCommand.ExecuteScalar();

                if (obj != null)
                    experimentId = Convert.ToInt64(myCommand.ExecuteScalar());

                return experimentId;

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
        /// Returns the associated record of the BLOB
        /// </summary>
        /// <param name="blobId">the ID of the BLOB whose association is being queried</param>
        /// <returns>the sequence number of the ExperimentRecord with which the BLOB is associated; -1 if there is no such record; note that a BLOB can be associated with at most one ExperimentRecord</returns>
        public int GetBlobAssociation(long blobId)
        {
            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("RetrieveBlobAssociation", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.Parameters.Add(FactoryDB.CreateParameter("@blobId", blobId, DbType.Int64));

            try
            {
                myConnection.Open();

                Object obj = myCommand.ExecuteScalar();
                int seqNo = -1;

                if (obj != null)
                    seqNo = Convert.ToInt32(myCommand.ExecuteScalar());

                return seqNo;
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
        /// StoredProcedure does not Exist.
        /// Reports on the download status of the BLOB record
        /// </summary>
        /// <param name="blobId">the ID of the BLOB whose status is being queried</param>
        /// <returns>a status code</returns>
        public int GetBlobStatus(long blobId)
        {
            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("RetrieveBlobDownloadStatus", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@blobId", blobId, DbType.Int64));

            int statusCode = -1;

            try
            {
                myConnection.Open();

                Object obj = myCommand.ExecuteScalar();

                if (obj != null)
                    statusCode = Convert.ToInt32(myCommand.ExecuteScalar());
            }
            catch
            {
                throw;
            }
            finally
            {
                myConnection.Close();
            }

            return statusCode;
        }

        public bool SetBlobStatus(long blobId, int status)
        {
            bool statusCode = false;
            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("SetBlobDownloadStatus", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@blobId", blobId, DbType.Int64));
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@blobStatus", status, DbType.Int32));

            try
            {
                myConnection.Open();

                Object obj = myCommand.ExecuteScalar();

                if (obj != null)
                    statusCode = Convert.ToInt32(myCommand.ExecuteScalar()) > 0;
            }
            catch
            {
                throw;
            }
            finally
            {
                myConnection.Close();
            }

            return statusCode;
          
        }
        
            /// <summary>
            /// Requests the download of the binary data associated with a previously created BLOB record
            /// </summary>
            /// <param name="BlobID">the ID of the BLOB for which the download of binary data is requested</param>
            /// <param name="blobURL">the source URL for the binary data</param>
            /// <returns>the sequence number of the ExperimentRecord with which the BLOB is associated; -1 if there is no such record</returns>
            /// <remarks>the protocol specified in the URL must match one of the ESS's supported BLOB import protocols</remarks>
            public bool RequestBlobStorage(long blobId, string blobUrl)
            {
                try
                {
                    string[] temp = Regex.Split(blobUrl, "://");
                    string importProtocol = temp[0];

                    string[] supportedImportedProtocols = GetSupportedBlobImportProtocols();
                    bool supported = false;

                    foreach (string s in supportedImportedProtocols)
                    {
                        if (String.Compare(s, importProtocol, true) == 0)
                        {
                            supported = true;
                            break;
                        }
                    }

                    if (supported)
                    {
                        DownloadAPI.DownloadDataInBackground(blobId, blobUrl);
                        return true;
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    throw new Exception("Exception thrown uploading binary data to ESS", ex);
                }
            }
        
        /*
        /// <summary>
        /// Currently stores the data in a file and possibly in the database.
        /// </summary>
        /// <param name="blobId"></param>
        /// <param name="blobData"></param>
        /// <returns></returns>
        public static bool StoreBlob(long blobId, byte[] blobData, string blobUrl)
        {
            try
            {
                if (StoreBlobData(blobId, blobData) && StoreBlobAccess(blobId, blobUrl))
                {
                    string userBlobsPath = ConfigurationManager.AppSettings["userBlobsPath"];

                    //NEED TO GET THE EXTENSION FROM THE INCOMING BLOBURL
                    FileStream fs = new FileStream(userBlobsPath + "\\blob_" + blobId.ToString() + ".jpg", FileMode.Create, FileAccess.Write);
                    BinaryWriter bw = new BinaryWriter(fs);
                    bw.Write(blobData);
                    bw.Close();
                    fs.Close();

                    return true;
                }

                return false;
            }
            catch
            {
                throw;
            }
        }
        */
        /// <summary>
        /// 
        /// </summary>
        /// <param name="blobId"></param>
        /// <param name="blobUrl"></param>
        /// <returns></returns>
        public static bool StoreBlobAccess(long blobId, string blobUrl)
        {
            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("AddBlobAccess", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@blobId", blobId, DbType.Int64));
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@blobUrl", blobUrl, DbType.String,512));

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
        /// Adds the actual blob data to the blob record in the database, 
        /// this should not be called until the blob data has been downloaded 
        /// and it passes the Checksum test.
        /// </summary>
        /// <param name="blobData">the blob data</param>
        /// <returns>true if the data is successfully stored</returns>
        public byte[] RetrieveBlobData(long blobId)
        {
            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("RetrieveBlobData", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@blobId", blobId, DbType.Int64));
            byte[] data = null;
            try
            {
                myConnection.Open();
                DbDataReader myReader = myCommand.ExecuteReader();
                
                while (myReader.Read())
                {
                    if (!myReader.IsDBNull(0))
                    {
                        int byteCount = myReader.GetInt32(0);
                        // BLOB warning
                        int bufSize = 8000;
                        Byte[] buffer = new byte[byteCount];
                        long count = myReader.GetBytes(1, 0L, buffer, 0, byteCount);
                        //data = new byte[count];
                        //for (long i = 0L; i < count;i++ )
                        //{
                        //    data[i] = buffer[i];
                        //} 
                        return buffer;
                    }
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
            return data;
        }
        /// <summary>
        /// Adds the actual blob data to the blob record in the database, 
        /// this should not be called until the blob data has been downloaded 
        /// and it passes the Checksum test.
        /// </summary>
        /// <param name="blobData">the blob data</param>
        /// <returns>true if the data is successfully stored</returns>
        public bool StoreBlobData(long blobId, string mimeType, byte[] blobData)
        {
            SqlConnection myConnection = (SqlConnection) FactoryDB.GetConnection();
            SqlCommand myCommand = myConnection.CreateCommand();
            myCommand.CommandText= "AddBlobData";
            myCommand.CommandType = CommandType.StoredProcedure;

            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@blobId", blobId, DbType.Int64));
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@blobStatus", Blob.eStatus.COMPLETE, DbType.Int32));
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@mimeType", mimeType, DbType.AnsiString,1024));
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@byteCount", blobData.Length, DbType.Int32));
            // BLOB Warning
            SqlParameter blobParam = new SqlParameter("@blobData",SqlDbType.Image, blobData.Length);
            blobParam.Value = blobData;
            myCommand.Parameters.Add(blobParam);
            

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


        //public static bool StoreBlobUrl(long blobID)
        //{
        //    DbConnection myConnection = FactoryDB.GetConnection();
        //    DbCommand myCommand = FactoryDB.CreateCommand("AddBlobAccess", myConnection);
        //    myCommand.CommandType = CommandType.StoredProcedure;

        //    myCommand.Parameters.Add(FactoryDB.CreateParameter("@blobId", blobID));

        //    try
        //    {
        //        myConnection.Open();
        //        int rows = myCommand.ExecuteNonQuery();

        //        return (rows > 0);

        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        myConnection.Close();
        //    }
        //}

        /// <summary>
        /// NO OP
        /// Cancels the download of binary data associated with a BLOB or deletes the corrupt data of an attempted download
        /// </summary>
        /// <param name="blobId">the ID of the BLOB for which the download of binary data is being cancelled</param>
        /// <returns>the sequence number of the ExperimentRecord with which the BLOB is assoicated; -1 if there is no such record</returns>
        public int CancelBlobStorage(long blobId)
        {
            return 0;
        }

        /// <summary>
        /// Returns a URL from which the specified BLOB data can be downloaded
        /// </summary>
        /// <param name="blobId">the ID of the requested BLOB data</param>
        /// <param name="protocol">a string specifying one of the ESS's supported protocols for BLOB export</param>
        /// <param name="duration">the number of minutes for which the BLOB will be made available at the returned URL</param>
        /// <returns>the URL from which the invoking process agent may download the BLOB from the ESS; it must match the requested protocol, if the ESS supports it; null if error</returns>
        public string RequestBlobAccess(long blobId, string protocol, int duration)
        {
            string key = "any";
            string pass = "1234";
            StringBuilder blobURL = new StringBuilder();
            //blobURL.Append("http://");
            // Should deal more directly with the protocol and URL
            blobURL.Append(ProcessAgentDB.ServiceAgent.codeBaseUrl + "/BlobHandler.ashx");
            blobURL.Append("?bid=" + blobId);
            blobURL.Append("&key=" + key + "&pid=" + pass);
            return blobURL.ToString();


            /* Totally punt for the moment
            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("RetrieveBlobAccess", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.Parameters.Add(FactoryDB.CreateParameter("@blobId", blobId));
            myCommand.Parameters.Add(FactoryDB.CreateParameter("@duration", duration));

            try
            {
                myConnection.Open();
                string url = (string)myCommand.ExecuteScalar();

                //check if the requested protocol matches the URL protocol
                string[] temp = Regex.Split(url, "://");
                string protocolStored = temp[0];
                if (String.Compare(protocol, protocolStored, true) == 0)
                    blobURL = url;

                return blobURL;
            }
            catch
            {
                throw;
            }
            finally
            {
                myConnection.Close();
            }
             */
        }

        /// <summary>
        /// Associates the BLOB with the specified ExperimentRecord
        /// </summary>
        /// <param name="blobId">the ID of the BLOB to be associated</param>
        /// <param name="experimentId">the ID of the Experiment containing the associated ExperimentRecord</param>
        /// <param name="sequenceNum">the sequence number of the associated ExperimentRecord</param>
        /// <returns>true if the association was successful</returns>
        public bool AddBlobToRecord(long blobId, long experimentId, string sbGuid, int sequenceNum)
        {
            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("AddBlobToExperimentRecord", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.Parameters.Add(FactoryDB.CreateParameter("@experimentId", experimentId, DbType.Int64));
            myCommand.Parameters.Add(FactoryDB.CreateParameter("@issuerGuid", sbGuid, DbType.AnsiString,50));
            myCommand.Parameters.Add(FactoryDB.CreateParameter("@blobId", blobId, DbType.Int64));
            myCommand.Parameters.Add(FactoryDB.CreateParameter("@sequenceNo", sequenceNum, DbType.Int32));

            try
            {
                myConnection.Open();

                int recs = myCommand.ExecuteNonQuery();
                return (recs > 0);

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
        /// Retrieves the BLOB records associated with a particular ExperimentRecord
        /// </summary>
        /// <param name="experimentId">the ID of the Experiment containing the specified ExperimentRecord</param>
        /// <param name="sequenceNum">the sequence number of the specified ExperimentRecord</param>
        /// <returns>An array of BLOB objects assoicated with the specified ExperimentRecord; if the sequenceNum is -1, 
        /// then all the BLOB objects associated with all the ExperimentRecords of the Experiment designated by experimentId are returned</returns>
        public Blob[] GetBlobsForRecord(long experimentId, string sbGuid, int sequenceNum)
        {
            if (sequenceNum == -1)
            {
                return GetBlobsForExperiment(experimentId, sbGuid);
            }
            else
            {
                DbConnection myConnection = FactoryDB.GetConnection();
                DbCommand myCommand = FactoryDB.CreateCommand("RetrieveBlobsForExperimentRecord", myConnection);
                myCommand.CommandType = CommandType.StoredProcedure;
                myConnection.Open();

                ArrayList blobList = new ArrayList();

                try
                {
                    myCommand.Parameters.Add(FactoryDB.CreateParameter("@experimentId", experimentId, DbType.Int64));
                    myCommand.Parameters.Add(FactoryDB.CreateParameter("@issuerGuid", sbGuid, DbType.AnsiString,50));
                    myCommand.Parameters.Add(FactoryDB.CreateParameter("@sequenceNo", sequenceNum, DbType.Int32));

                    DbDataReader myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        Blob b = new Blob();

                        b.experimentId = experimentId;
                        b.recordNumber = sequenceNum;

                        if (myReader["blob_id"] != System.DBNull.Value)
                            b.blobId = Convert.ToInt64(myReader["blob_id"]);

                        if (myReader["date_created"] != System.DBNull.Value)
                            b.timestamp = (DateTime)(myReader["date_created"]);

                        if (myReader["description"] != System.DBNull.Value)
                            b.description = (string)myReader["description"];

                        if (myReader["byte_Count"] != System.DBNull.Value)
                            b.byteCount = Convert.ToInt32(myReader["byte_Count"]);

                        if (myReader["check_sum"] != System.DBNull.Value)
                            b.checksum = (string)(myReader["check_sum"]);

                        if (myReader["check_sum_algorithm"] != System.DBNull.Value)
                            b.checksumAlgorithm = (string)myReader["check_sum_algorithm"];

                        if (myReader["mime_type"] != System.DBNull.Value)
                            b.mimeType = (string)myReader["mime_type"];
                        blobList.Add(b);
                    }

                    myReader.Close();

                    if (blobList.Count == 0)
                        return null;

                    Blob[] blobs = new Blob[blobList.Count];
                    for (int i = 0; i < blobList.Count; i++)
                    {
                        blobs[i] = (Blob)blobList[i];
                    }

                    return blobs;
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


        /// <summary>
        /// Retrieves the BLOB records associated with a particular Experiment
        /// </summary>
        /// <param name="experimentId">the ID of the Experiment containing the specified ExperimentRecord</param>
        /// <returns>An array of BLOB objects assoicated with the specified ExperimentRecord; if the sequenceNum is -1, 
        /// then all the BLOB objects associated with all the ExperimentRecords of the Experiment designated by experimentId are returned</returns>
        public Blob[] GetBlobsForExperiment(long experimentId, string sbGuid)
        {
            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("RetrieveBlobsForExperiment", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;
            myConnection.Open();

            ArrayList blobList = new ArrayList();

            try
            {
                myCommand.Parameters.Add(FactoryDB.CreateParameter("@experimentId", experimentId, DbType.Int64));
                myCommand.Parameters.Add(FactoryDB.CreateParameter("@issuerGuid", sbGuid, DbType.AnsiString,50));

                DbDataReader myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    Blob b = new Blob();
                    
                    b.experimentId = experimentId;

                    if (myReader["blob_id"] != System.DBNull.Value)
                        b.blobId = Convert.ToInt64(myReader["blob_id"]);

                    if (myReader["date_created"] != System.DBNull.Value)
                        b.timestamp = (DateTime)(myReader["date_created"]);

                    if (myReader["description"] != System.DBNull.Value)
                        b.description = (string)myReader["description"];

                    if (myReader["byte_Count"] != System.DBNull.Value)
                        b.byteCount = Convert.ToInt32(myReader["byte_Count"]);

                    if (myReader["check_sum"] != System.DBNull.Value)
                        b.checksum = (string)(myReader["check_sum"]);

                    if (myReader["check_sum_algorithm"] != System.DBNull.Value)
                        b.checksumAlgorithm = (string)myReader["check_sum_algorithm"];

                    if (myReader["seq_num"] != System.DBNull.Value)
                        b.recordNumber = Convert.ToInt32(myReader["seq_num"]);

                    if (myReader["mime_type"] != System.DBNull.Value)
                        b.mimeType = (string)myReader["mime_type"];

                    blobList.Add(b);
                }

                myReader.Close();

                if (blobList.Count == 0)
                    return null;

                Blob[] blobs = new Blob[blobList.Count];
                for (int i = 0; i < blobList.Count; i++)
                {
                    blobs[i] = (Blob)blobList[i];
                }

                return blobs;
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
        /// Retrieves the complete BLOB object, using the BLOB ID
        /// </summary>
        /// <param name="blobId">the ID of the BLOB to be completely retrieved</param>
        /// <returns>the complete BLOB object</returns>
        public Blob GetBlob(long blobId)
        {
            Blob b = new Blob();

            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("RetrieveBlob", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            myCommand.Parameters.Add(FactoryDB.CreateParameter("@blobId", blobId, DbType.Int64));

            try
            {
                myConnection.Open();
                DbDataReader myReader = myCommand.ExecuteReader();

                while (myReader.Read())
                {
                    b.blobId = blobId;
                    if (myReader["experiment_ID"] != System.DBNull.Value)
                        b.experimentId = Convert.ToInt64(myReader["experiment_ID"]);

                    if (myReader["date_created"] != System.DBNull.Value)
                        b.timestamp = Convert.ToDateTime(myReader["date_created"]);

                    if (myReader["description"] != System.DBNull.Value)
                        b.description = Convert.ToString(myReader["description"]);

                    if (myReader["byte_count"] != System.DBNull.Value)
                        b.byteCount = Convert.ToInt32(myReader["byte_count"]);

                    if (myReader["check_sum"] != System.DBNull.Value)
                        b.checksum = Convert.ToString(myReader["check_sum"]);

                    if (myReader["check_sum_algorithm"] != System.DBNull.Value)
                        b.checksumAlgorithm = Convert.ToString(myReader["check_sum_algorithm"]);

                    if (myReader["seq_num"] != System.DBNull.Value)
                        b.recordNumber = Convert.ToInt32(myReader["seq_num"]);

                    if (myReader["mime_type"] != System.DBNull.Value)
                        b.mimeType = (string)myReader["mime_type"];

                }

                myReader.Close();

                if (b.experimentId > 0)
                    return b;

                return null;

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
