/*
 * Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
 * Please see license.txt in top level directory for full license.
 */

/* $Id$ */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text;

using iLabs.Core;
using iLabs.ServiceBroker;
using iLabs.ServiceBroker.Administration;
using iLabs.ServiceBroker.Authorization;
using iLabs.DataTypes;
using iLabs.DataTypes.ProcessAgentTypes;
using iLabs.DataTypes.TicketingTypes;
//using iLabs.DataTypes.BatchTypes;
using iLabs.Ticketing;
using iLabs.TicketIssuer;
using iLabs.UtilLib;
using iLabs.ServiceBroker;

namespace iLabs.ServiceBroker.Internal
{
	/// <summary>
	/// Summary description for InternalAdminDB.
	/// </summary>
	public class InternalAdminDB
	{
		public InternalAdminDB()
		{
			//
			// TODO: Add constructor logic here
			//
		}


		/* !------------------------------------------------------------------------------!
		 *							CALLS FOR ProcessAgents and Services
		 * !------------------------------------------------------------------------------!
		 */

	
		/// <summary>
		/// to delete all lab servers records specified by the array of lab server IDs
		/// </summary>
		public static int[] DeleteProcessAgents ( int[] agentIDs )
		{
			
			DbConnection myConnection = FactoryDB.GetConnection();
			DbCommand myCommand = FactoryDB.CreateCommand("DeleteProcessAgents", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;
			myCommand.Parameters.Add(FactoryDB.CreateParameter("@agentID",null,DbType.Int32));

			/*
			 * Note : Alternately ADO.NET could be used. However, the disconnected  DataAdapter object might prove
			 * extremely inefficient and hence this method was chosen
			 */
			 
			ArrayList arrayList = new ArrayList ();

			try 
			{
				myConnection.Open ();
											
				foreach (int agentID in agentIDs) 
				{
					
					// Deleting from table LabServers
					/*	IMPORTANT ! - The database if currently set to Cascade delete, where deleting an experiment will automatically
					 *  delete the relevant Lab_Server_to_Client_Map records. If Cascade Delete is not to be used, then the code to delete the extra records
					 *  in the map table when a lab server is deleted should be added in the stored procedure
					 * 
					 * Also, the qualifiers pertaining to the lab server are automatically
					 * deleted in the 'DeleteLabServer' stored procedure. This preserves consistency
					 * and gets rid of unnecessary rollback mechanisms that would otherwise have 
					 * to be implemented. - CV, 4/29/05
					 */

					myCommand.Parameters["@agentID"].Value = agentID;
					if(myCommand.ExecuteNonQuery () == 0)
					{
						arrayList.Add(agentID);
					}
				}

			}
			catch (Exception ex)
			{
				throw new Exception("Exception thrown in DeleteLabServers",ex);
			}
			finally 
			{
				myConnection.Close ();
			}

			// refresh in memory Q & Q-H cache - since the lab server qualifiers are automatically deleted
			AuthCache.QualifierSet  = InternalAuthorizationDB.RetrieveQualifiers ();
			AuthCache.QualifierHierarchySet = InternalAuthorizationDB.RetrieveQualifierHierarchy ();

			int[] lsIDs = Utilities.ArrayListToIntArray(arrayList);
			return lsIDs;
		}


		/* !------------------------------------------------------------------------------!
		 *							CALLS FOR LAB SERVERS
		 * !------------------------------------------------------------------------------!
		 */
     

		/// <summary>
		/// Gets the integer LabServerID given the experimentID
		/// </summary>
		public static int SelectLabServerID(long experimentID)
		{
			
			int intLabServerID;

			DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("Experiment_RetrieveRawData", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;

			myCommand.Parameters.Add(FactoryDB.CreateParameter("@experimentID", experimentID,DbType.Int64));
			
			try
			{
				myConnection.Open();
				DbDataReader r = myCommand.ExecuteReader();
				if (r.Read()) 
				{
					intLabServerID = r.GetInt32(2);
				}
				else
				{
					throw new Exception("Cannot retrieve Lab Server ID from the database");
				}

			}
			catch (Exception ex)
			{
				throw new Exception("Exception thrown in retrieving labServerID", ex);
			}
			finally
			{
				myConnection.Close();
			}
			return intLabServerID;
		}

		/* !------------------------------------------------------------------------------!
		 *							CALLS FOR LAB CLIENTS
		 * !------------------------------------------------------------------------------!
		 */
		
		/// <summary>
		/// Inserts a LabClient into the database, this does not reate any ClientInfo items or asociate a LabServer.
		/// </summary>
		/// <param name="guid"></param>
		/// <param name="name"></param>
		/// <param name="shortDescription"></param>
		/// <param name="longDescription"></param>
		/// <param name="version"></param>
		/// <param name="loaderScript"></param>
		/// <param name="clientType"></param>
		/// <param name="email"></param>
		/// <param name="firstName"></param>
		/// <param name="lastName"></param>
		/// <param name="notes"></param>
		/// <param name="needsScheduling"></param>
		/// <param name="needsESS"></param>
		/// <param name="isReentrant"></param>
		/// <returns>the client ID</returns>
        public static int InsertLabClient(string guid, string name, string version, string shortDescription, string longDescription,
             string clientType, string loaderScript, string documentationURL, string email, string firstName, string lastName, string notes,
             bool needsESS, bool needsScheduling, bool isReentrant)
		{
			int clientID = -1;

			DbConnection myConnection = FactoryDB.GetConnection();
			DbCommand myCommand = FactoryDB.CreateCommand("Client_Insert", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;

            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@guid", guid, DbType.AnsiString,50));
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@labClientName", name, DbType.String,256));
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@shortDescription", shortDescription, DbType.String,256));
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@longDescription", longDescription, DbType.String));
			myCommand.Parameters.Add(FactoryDB.CreateParameter("@version", version,DbType.String,50));
			myCommand.Parameters.Add(FactoryDB.CreateParameter("@loaderScript", loaderScript, DbType.String,2000));
            myCommand.Parameters.Add(FactoryDB.CreateParameter("@docURL", documentationURL, DbType.String, 512));
			myCommand.Parameters.Add(FactoryDB.CreateParameter("@clientType", clientType, DbType.AnsiString,100));
			myCommand.Parameters.Add(FactoryDB.CreateParameter("@email",email, DbType.String,256 ));	
			myCommand.Parameters.Add(FactoryDB.CreateParameter("@firstName",firstName, DbType.String,128 ));	
			myCommand.Parameters.Add(FactoryDB.CreateParameter("@lastName",lastName, DbType.String,128 ));
			myCommand.Parameters.Add(FactoryDB.CreateParameter("@notes", notes, DbType.String));
            myCommand.Parameters.Add(FactoryDB.CreateParameter("@needsScheduling", needsScheduling, DbType.Boolean));
            myCommand.Parameters.Add(FactoryDB.CreateParameter("@needsESS", needsESS, DbType.Boolean));
            myCommand.Parameters.Add(FactoryDB.CreateParameter("@isReentrant", isReentrant, DbType.Boolean));
	
			//Encoding transaction here
			//Alternatively a dataset can be used, but it's not preferred bcos it's inefficient?
			
            //DbTransaction transaction;	
			myConnection.Open();
			//transaction = myConnection.BeginTransaction();
			//myCommand.Transaction = transaction;

			try
			{

				clientID = Convert.ToInt32(myCommand.ExecuteScalar());
				//transaction.Commit();
			}
			catch (Exception ex)
			{
				//transaction.Rollback();	
				throw new Exception("Exception thrown in inserting lab server to client",ex);
			}
			finally
			{
				myConnection.Close();
			}
			return clientID;
		}

        /// <summary>
        /// Inserts an association between the LabClient and the labServer, sorting order may be specified.
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="serverID"></param>
        /// <param name="displayOrder"></param>
        /// <returns>count of affected rows</returns>
        public static int LabServerClientInsert(int serverID, int clientID, int displayOrder)
        {
            /*-- ASSOCIATED LAB SERVERS --*/
            int i = 0;
			DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("LabServerClient_Insert",myConnection);
			
					myCommand.CommandType = CommandType.StoredProcedure ;

                    myCommand.Parameters.Add(FactoryDB.CreateParameter( "@labClientID", clientID, DbType.Int32));
					myCommand.Parameters.Add(FactoryDB.CreateParameter("@labServerID", serverID, DbType.Int32));
					myCommand.Parameters.Add(FactoryDB.CreateParameter("@displayOrder", displayOrder, DbType.Int32));
				try{
                    myConnection.Open();
				     i = myCommand.ExecuteNonQuery();
                }
				 catch(Exception e){
                     throw new Exception("Exception thrown in inserting lab server to client",e);
                }
				finally
			{
				myConnection.Close();
			}
			return i;
        }
        /// <summary>
        /// Inserts a LabCIlentInfo
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="url"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="displayOrder"></param>
        /// <returns>the ID of the LabClientInfo</returns>
       public static int InsertLabClientInfo(int clientID, string url, string name, string description, int displayOrder)
        {
                DbConnection myConnection = FactoryDB.GetConnection();
                DbCommand myCommand = FactoryDB.CreateCommand("ClientInfo_Insert", myConnection);
                myCommand.CommandType = CommandType.StoredProcedure;

                myCommand.Parameters.Add(FactoryDB.CreateParameter( "@labClientID", clientID, DbType.Int32));
                myCommand.Parameters.Add(FactoryDB.CreateParameter( "@infoURL", url, DbType.String, 512));
                myCommand.Parameters.Add(FactoryDB.CreateParameter( "@infoName", name, DbType.String, 256));
                myCommand.Parameters.Add(FactoryDB.CreateParameter( "@description", description, DbType.String));
                myCommand.Parameters.Add(FactoryDB.CreateParameter( "@displayOrder", displayOrder, DbType.Int32));
                int id = 0;
            try{
                myConnection.Open();
                id = Convert.ToInt32(myCommand.ExecuteScalar());
            }
            catch(Exception e){
                     throw new Exception("Exception thrown inserting clientInfo",e);
            }
			finally
			{
				myConnection.Close();
			}
             return id;
        }

		/// <summary>
		/// Updates the lab client core fields. This does not change LabServerClient map or ClientItems
		/// </summary>
		/// <param name="clientID"></param>
		/// <param name="guid"></param>
		/// <param name="name"></param>
		/// <param name="version"></param>
		/// <param name="shortDescription"></param>
		/// <param name="longDescription"></param>
		/// <param name="clientType"></param>
		/// <param name="loaderScript"></param>
		/// <param name="email"></param>
		/// <param name="firstName"></param>
		/// <param name="lastName"></param>
		/// <param name="notes"></param>
		/// <param name="needsESS"></param>
		/// <param name="needsScheduling"></param>
		/// <param name="isReentrant"></param>
		/// <returns></returns>
        public static int UpdateLabClient(int clientID, string guid, string name, string version, string shortDescription, string longDescription,
            string clientType, string loaderScript, string documentationURL,string email, string firstName, string lastName, string notes,
             bool needsESS, bool needsScheduling, bool isReentrant)
		{
            int status = -1;
			DbConnection myConnection = FactoryDB.GetConnection();
			DbCommand myCommand = FactoryDB.CreateCommand("Client_Update", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;
			myCommand.Parameters.Add(FactoryDB.CreateParameter("@labClientID", clientID, DbType.Int32 ));
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@clientGUID", guid, DbType.String,50));
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@labClientName", name, DbType.String,256));
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@shortDescription", shortDescription, DbType.String,256));
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@longDescription", longDescription, DbType.String));
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@version", version, DbType.String,50));
			myCommand.Parameters.Add(FactoryDB.CreateParameter("@loaderScript", loaderScript, DbType.String,2000));
			myCommand.Parameters.Add(FactoryDB.CreateParameter("@clientType", clientType, DbType.AnsiString,100));
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@docURL", documentationURL, DbType.String, 512));
			myCommand.Parameters.Add(FactoryDB.CreateParameter("@email",email, DbType.String,256 ));
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@firstName", firstName, DbType.String,128));
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@lastName", lastName, DbType.String, 128));
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@notes", notes, DbType.String));
            myCommand.Parameters.Add(FactoryDB.CreateParameter("@needsScheduling", needsScheduling, DbType.Boolean));
            myCommand.Parameters.Add(FactoryDB.CreateParameter("@needsESS", needsESS, DbType.Boolean));
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@isReentrant", isReentrant, DbType.Boolean));

			//Encoding transaction here
			//Alternatively a dataset can be used, but it's not preferred bcos it's inefficient?
			//DbTransaction transaction;
		
			myConnection.Open();
			//transaction = myConnection.BeginTransaction();
            //myCommand.Transaction = transaction;

			try
			{
				status = myCommand.ExecuteNonQuery();
				if(status == 0)
					throw new Exception ("No record modified exception");
				

				//transaction.Commit();
			}
			catch (Exception ex)
			{
				//transaction.Rollback();	
				throw new Exception("Exception thrown in updating lab client: " + ex.Message, ex);
			}
			finally
			{
				myConnection.Close();
			}
            return status;
		}

        public static int LabServerClientUpdate(int serverID, int clientID, int displayOrder)
        {
            /*-- ASSOCIATED LAB SERVERS --*/
            // update table Lab_Server_To_Client_Map

            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("LabServerClient_Update", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@labClientID", clientID, DbType.Int32));
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@labServerID", serverID, DbType.Int32));
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@displayOrder", displayOrder, DbType.Int32));
            int x = 0;
            try
            {
                myConnection.Open();
                x = myCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //transaction.Rollback();	
                throw new Exception("Exception thrown in inserting lab server to client: " + ex.Message, ex);
            }
            finally
            {
                myConnection.Close();
            }
            return x;
        }

        public static ProcessAgentInfo[] GetLabServersForClient(int clientID)
        {
            List<ProcessAgentInfo> servers = null;
            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("LabServerClient_RetrieveServerIDs", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@labClientID", clientID, DbType.Int32));
            DbDataReader reader = null;
            try
            {
                myConnection.Open();
                reader = myCommand.ExecuteReader();
                if (reader != null && reader.HasRows)
                {
                    servers = new List<ProcessAgentInfo>();
                    ProcessAgentDB paDB = new ProcessAgentDB();
                    while (reader.Read())
                    {

                        int serverID = reader.GetInt32(0);
                        ProcessAgentInfo pai = paDB.GetProcessAgentInfo(serverID);
                        if (pai != null && !pai.retired)
                        {
                            servers.Add(pai);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                throw new Exception("Exception thrown in getLabServersForClient: " + ex.Message, ex);
            }
            finally
            {
                myConnection.Close();
            }


            if (servers != null)
                return servers.ToArray();
            else
                return null;
        }

        public static int[] GetLabServerIDsForClient(int clientID)
        {
            List<int> servers = new List<int>();
            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("LabServerClient_RetrieveServerIDs", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@labClientID", clientID, DbType.Int32));
            DbDataReader reader = null;
            try
            {
                myConnection.Open();
                reader = myCommand.ExecuteReader();
                if (reader != null && reader.HasRows)
                {
                   
                    ProcessAgentDB paDB = new ProcessAgentDB();
                    while (reader.Read())
                    {
                        int serverID = reader.GetInt32(0);
                        servers.Add(serverID);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception thrown in getLabServersForClient: " + ex.Message, ex);
            }
            finally
            {
                myConnection.Close();
            }
            return servers.ToArray();
           
        }

        public static int UpdateLabClientInfoOrder(int[] clientInfoIDs)
        {
            int count = 0;
            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("ClientInfo_UpdateOrder",myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;
            DbParameter infoID = FactoryDB.CreateParameter( "@clientInfoID", DbType.Int32);
            DbParameter order = FactoryDB.CreateParameter( "@displayOrder", DbType.Int32);
            myCommand.Parameters.Add(infoID);
            myCommand.Parameters.Add(order);
            try
            {
                myConnection.Open();
                for (int i = 0; i < clientInfoIDs.Length; i++)
                {
                    infoID.Value = clientInfoIDs[i];
                    order.Value = i;
                    int status = myCommand.ExecuteNonQuery();
                    if (status > 0)
                    {
                        count += status;
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("error: " + e.Message);
            }
            finally
            {
                myConnection.Close();
            }
            return count;
        }

        /// <summary>
        /// Updates a ClientInfo that has the same ClientInfoID and clientID.
        /// </summary>
        /// <param name="clientInfoID"></param>
        /// <param name="clientID"></param>
        /// <param name="url"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="displayOrder"></param>
        /// <returns></returns>
        public static int UpdateLabClientInfo(int clientInfoID, int clientID, string url, string name, string description, int displayOrder)
        {
            int count = 0;
            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("ClientInfo_Update", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure ;

            myCommand.Parameters.Add(FactoryDB.CreateParameter("@clientInfoID", clientInfoID, DbType.Int32));
			myCommand.Parameters.Add(FactoryDB.CreateParameter("@clientID", clientID,DbType.Int32));
			myCommand.Parameters.Add(FactoryDB.CreateParameter("@infoURL", url, DbType.String,512));
			myCommand.Parameters.Add(FactoryDB.CreateParameter("@infoName", name, DbType.String,256));
			myCommand.Parameters.Add(FactoryDB.CreateParameter("@description", description,DbType.String,2048));
			myCommand.Parameters.Add(FactoryDB.CreateParameter("@displayOrder", displayOrder, DbType.Int32));
			try{
                myConnection.Open();
                int x = myCommand.ExecuteNonQuery();
                if (x != 0)
                    count += x;
            }
            catch (Exception e)
            {
                throw new Exception("error: " + e.Message);
            }
            finally
            {
                myConnection.Close();
            }
            return count;
        }

        public static ClientInfo[] ListClientInfos(int clientID){
            List<ClientInfo> infos = new List<ClientInfo>();
            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("ClientInfo_Retrieve", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure ;

			myCommand.Parameters.Add(FactoryDB.CreateParameter("@labClientID", clientID,DbType.Int32));

            DbDataReader reader = null;

            try
            {
                myConnection.Open();
                reader = myCommand.ExecuteReader();
                // select client_info_id, info_URL, info_name, display_order, description
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        ClientInfo info = new ClientInfo();
                        info.clientID = clientID;
                        info.clientInfoID = reader.GetInt32(0);
                        info.infoURL = reader.GetString(1);
                        info.infoURLName = reader.GetString(2);
                        info.displayOrder = reader.GetInt32(3);
                        info.description = reader.GetString(4);
                        infos.Add(info);

                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("error: " + e.Message);
            }
            finally
            {
                myConnection.Close();
            }
            return infos.ToArray();
        }

		/// <summary>
		/// to delete all lab clients records specified by the array of lab client IDs
        /// ResourceMapping (ESS,USS & LabServer) are removeded as will as ClientInfo Items
		/// </summary>
		public static int[] DeleteLabClients ( int[] clientIDs )
		{

            DbConnection myConnection = FactoryDB.GetConnection();
			DbCommand myCommand = FactoryDB.CreateCommand("Client_Delete", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure ;
			myCommand.Parameters.Add(FactoryDB.CreateParameter("@labClientID",null,DbType.Int32));

			/*
			 * Note : Alternately ADO.NET could be used. However, the disconnected  DataAdapter object might prove
			 * extremely inefficient and hence this method was chosen
			 */
			 
			ArrayList arrayList = new ArrayList ();

			try 
			{
				myConnection.Open ();
											
				foreach (int clientID in clientIDs) 
				{
					// Deleting from table LabClients
					/*	IMPORTANT ! - The database is currently set to Cascade delete, where deleting an experiment will automatically
					 *  delete the relevant Lab_Server_to_Client_Map records. If Cascade Delete is not to be used, then the code to delete the extra records
					 *  in the map table when a lab client is deleted should be added in the stored procedure
					 *  
					 * Also, the qualifiers pertaining to the lab client are automatically
					 * deleted in the 'DeleteLabClient' stored procedure. This preserves consistency
					 * and gets rid of unnecessary rollback mechanisms that would otherwise have 
					 * to be implemented. - CV, 4/29/05
					 */
					myCommand.Parameters["@labClientID"].Value = clientID;
					if(myCommand.ExecuteNonQuery () == 0)
					{
						arrayList.Add(clientID);
					}
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Exception thrown in DeleteLabClients",ex);
			}
			finally 
			{
				myConnection.Close ();
			}

			// refresh in memory Q & Q-H cache - since the lab server qualifiers are automatically deleted
			AuthCache.QualifierSet  = InternalAuthorizationDB.RetrieveQualifiers ();
			AuthCache.QualifierHierarchySet = InternalAuthorizationDB.RetrieveQualifierHierarchy ();

			int[] lcIDs = Utilities.ArrayListToIntArray(arrayList);
						
			return lcIDs;
		}

        public static int DeleteLabClient(int clientID)
        {
            int count = -1;
            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("Client_Delete", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.Parameters.Add(FactoryDB.CreateParameter("@labClientID", clientID,DbType.Int32));

            /*
             * Note : Alternately ADO.NET could be used. However, the disconnected  DataAdapter object might prove
             * extremely inefficient and hence this method was chosen
             */

            try
            {
                myConnection.Open();

                // Deleting from table LabClients
                /*	IMPORTANT ! - The database if currently set to Cascade delete, where deleting an experiment will automatically
                 *  delete the relevant Lab_Server_to_Client_Map records. If Cascade Delete is not to be used, then the code to delete the extra records
                 *  in the map table when a lab client is deleted should be added in the stored procedure
                 *  
                 * Also, the qualifiers pertaining to the lab client are automatically
                 * deleted in the 'DeleteLabClient' stored procedure. This preserves consistency
                 * and gets rid of unnecessary rollback mechanisms that would otherwise have 
                 * to be implemented. - CV, 4/29/05
                 */
              
                count = myCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Exception thrown in DeleteLabClients", ex);
            }
            finally
            {
                myConnection.Close();
            }
            if (count > 0)
            {
                // refresh in memory Q & Q-H cache - since the lab server qualifiers are automatically deleted
                AuthCache.QualifierSet = InternalAuthorizationDB.RetrieveQualifiers();
                AuthCache.QualifierHierarchySet = InternalAuthorizationDB.RetrieveQualifierHierarchy();
            }
            return count;
        }

        public static int LabServerClientDelete(int serverID, int clientID)
        {
            int count = 0;
            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("LabServerClient_Delete", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@labClientID", clientID, DbType.Int32));
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@labServerID", serverID, DbType.Int32));

            try
            {
                myConnection.Open();
                count = Convert.ToInt32(myCommand.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw new Exception("Exception thrown in LabServerClientDelete", ex);
            }
            finally
            {
                myConnection.Close();
            }
            return count;
        }
        public static int DeleteLabClientInfo(int clientID)
        {
            int count = -1;
            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("ClientInfo_DeleteByClient", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@labClientID", clientID, DbType.Int32));

            try
            {
                myConnection.Open();
                count = myCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Exception thrown in DeleteLabClients", ex);
            }
            finally
            {
                myConnection.Close();
            }
            return count;
        }
        public static int DeleteLabClientInfo(int clientID, int infoID)
        {
            int count = -1;
            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("ClientInfo_Delete", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@clientID", clientID, DbType.Int32));
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@infoID", infoID, DbType.Int32));

            try
            {
                 myConnection.Open();
                count = Convert.ToInt32(myCommand.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw new Exception("Exception thrown in DeleteLabClients", ex);
            }
            finally
            {
                myConnection.Close();
            }
            return count;
        }


        public static int SelectLabClientId(string guid)
        {
            int id = -1;
            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("Client_RetrieveID", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@guid", guid, DbType.AnsiString, 50));
		    
            try
            {
                myConnection.Open();

                // get labclient id from table lab_clients
                DbDataReader myReader = myCommand.ExecuteReader();

                while (myReader.Read())
                {
                    if (!myReader.IsDBNull(0))
                        id = myReader.GetInt32(0);

                }
            }
            catch (Exception e)
            {
               Logger.WriteLine("Error getting clientID: " + e.Message);
            }
            finally
            {
                myConnection.Close();
            }
            return id;

        }

		/// <summary>
		/// to retrieve a list of all the lab clients in the database
		/// </summary>

		public static int[] SelectLabClientIDs ()
		{
            List<int> clientIDs = new List<int>();
            DbConnection myConnection = FactoryDB.GetConnection();
			DbCommand myCommand = FactoryDB.CreateCommand("ClientIDs_Retrieve", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;

			try 
			{
				myConnection.Open ();
				

				// get labclient ids from table lab_clients
				DbDataReader myReader = myCommand.ExecuteReader ();
				

				while(myReader.Read ())
				{	
					if(myReader["client_ID"] != System.DBNull.Value )
                        clientIDs.Add(Convert.ToInt32(myReader["client_ID"]));
				}
				myReader.Close ();

			}
			catch (Exception ex)
			{
				throw new Exception("Exception thrown SelectLabClientIDs",ex);
			}
			finally 
			{
				myConnection.Close ();
			}

            return clientIDs.ToArray();
		}

        /// <summary>
        /// to retrieve a list of all the lab clients in the database
        /// </summary>

        public static int[] SelectLabClientIDsForServer(int serverID)
        {
            List<int> clientIDs = new List<int>();
            if (serverID > 0)
            {
                DbConnection myConnection = FactoryDB.GetConnection();
                DbCommand myCommand = FactoryDB.CreateCommand("LabServerClient_ClientIDs", myConnection);
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.Parameters.Add(FactoryDB.CreateParameter( "@serverID", serverID, DbType.Int32));
                try
                {
                    myConnection.Open();


                    // get labclient ids from table lab_clients
                    DbDataReader myReader = myCommand.ExecuteReader();


                    while (myReader.Read())
                    {
                        if (myReader["client_ID"] != System.DBNull.Value)
                            clientIDs.Add(Convert.ToInt32(myReader["client_ID"]));
                    }
                    myReader.Close();

                }
                catch (Exception ex)
                {
                    throw new Exception("Exception thrown SelectLabClientIDs", ex);
                }
                finally
                {
                    myConnection.Close();
                }
            }
            return clientIDs.ToArray();
        }


        public static IntTag[] SelectLabClientTags()
        {
            List<IntTag> clients = new List<IntTag>();
            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("Client_RetrieveTags", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                myConnection.Open();


                // get labclient ids from table lab_clients
                DbDataReader myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    IntTag tag = new IntTag();
                    tag.id = myReader.GetInt32(0);
                    if (!myReader.IsDBNull(1))
                    {
                        tag.tag = myReader.GetString(1);
                    }
                    clients.Add(tag);
                }
                myReader.Close();

            }
            catch (Exception ex)
            {
                throw new Exception("Exception thrown SelectLabClientTagss", ex);
            }
            finally
            {
                myConnection.Close();
            }
            clients.Sort();
            return clients.ToArray();
        }

        	/// <summary>
		/// to retrieve lab client metadata for lab clients specified by array of lab client IDs 
		/// </summary>
		public static LabClient SelectLabClient( int clientID )
		{
            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("Client_Retrieve", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@labClientID", clientID, DbType.Int32));
            LabClient lc = new LabClient();
            try
            {
                myConnection.Open();

              
                    // get labclient info from table lab_clients
                    DbDataReader myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        lc.clientID = clientID;

                        if (myReader["lab_client_name"] != System.DBNull.Value)
                            lc.clientName = (string)myReader["lab_client_name"];
                        if (myReader["short_description"] != System.DBNull.Value)
                            lc.clientShortDescription = (string)myReader["short_description"];
                        if (myReader["long_description"] != System.DBNull.Value)
                            lc.clientLongDescription = (string)myReader["long_description"];
                        if (myReader["version"] != System.DBNull.Value)
                            lc.version = (string)myReader["version"];
                        if (myReader["loader_script"] != System.DBNull.Value)
                            lc.loaderScript = (string)myReader["loader_script"];
                        if (myReader["description"] != System.DBNull.Value)
                            lc.clientType = (string)myReader["description"];
                        if (myReader["contact_email"] != System.DBNull.Value)
                            lc.contactEmail = (string)myReader["contact_email"];
                        if (myReader["contact_first_name"] != System.DBNull.Value)
                            lc.contactFirstName = (string)myReader["contact_first_name"];
                        if (myReader["contact_last_name"] != System.DBNull.Value)
                            lc.contactLastName = (string)myReader["contact_last_name"];
                        if (myReader["notes"] != System.DBNull.Value)
                            lc.notes = (string)myReader["notes"];
                        lc.needsScheduling = Convert.ToBoolean(myReader["needsScheduling"]);
                        lc.needsESS = Convert.ToBoolean(myReader["needsESS"]);
                        lc.IsReentrant = Convert.ToBoolean(myReader["isReentrant"]);
                        if (myReader["Client_Guid"] != System.DBNull.Value)
                            lc.clientGuid = (string)myReader["Client_Guid"];
                        if (myReader["Documentation_URL"] != System.DBNull.Value)
                            lc.documentationURL = (string)myReader["Documentation_URL"];
                        
                    }
                    myReader.Close();
                
                ////Retrieve  lab servers for a client

                //ArrayList lsIDs = new ArrayList();

                //DbCommand myCommand2 = FactoryDB.CreateCommand("LabServerClient_RetrieveServerIDs", myConnection);
                //myCommand2.CommandType = CommandType.StoredProcedure;
                //myCommand2.Parameters.Add(FactoryDB.CreateParameter(myCommand2, "@labClientID", clientID, DbType.Int32));

                //    DbDataReader myReader2 = myCommand2.ExecuteReader();

                //    while (myReader2.Read())
                //    {
                //        if (myReader2["agent_id"] != System.DBNull.Value)
                //            lsIDs.Add(Convert.ToInt32(myReader2["agent_id"]));
                //    }

                //    myReader2.Close();

                //    // Convert to an int array and add to the current LabClient object
                //    lc.labServerIDs = Utilities.ArrayListToIntArray(lsIDs);
                

                ////Retrieve info urls for a client
                //ArrayList infoURLs = new ArrayList();

                //myCommand2 = FactoryDB.CreateCommand("ClientInfo_Retrieve", myConnection);
                //myCommand2.CommandType = CommandType.StoredProcedure;
                //myCommand2.Parameters.Add(FactoryDB.CreateParameter(myCommand2, "@labClientID", clientID, DbType.Int32));

                //    DbDataReader myReader3 = myCommand2.ExecuteReader();

                //    while (myReader3.Read())
                //    {
                //        ClientInfo c = new ClientInfo();
                //        if (myReader3["info_url"] != System.DBNull.Value)
                //            c.infoURL = (string)myReader3["info_url"];
                //        if (myReader3["info_name"] != System.DBNull.Value)
                //            c.infoURLName = (string)myReader3["info_name"];
                //        if (myReader3["client_info_id"] != System.DBNull.Value)
                //            c.clientInfoID = Convert.ToInt32(myReader3["client_info_id"]);
                //        if (myReader3["description"] != System.DBNull.Value)
                //            c.description = ((string)myReader3["description"]);
                //        if (myReader3["display_order"] != System.DBNull.Value)
                //            c.displayOrder = (int)myReader3["display_order"];

                //        infoURLs.Add(c);
                //    }

                //    myReader2.Close();

                //    // Converting to a clientInfo array
                //    lc.clientInfos = new ClientInfo[infoURLs.Count];
                //    for (int j = 0; j < infoURLs.Count; j++)
                //    {
                //        lc.clientInfos[j] = (ClientInfo)(infoURLs[j]);
                //    }
                }
            
            catch (Exception ex)
            {
                throw new Exception("Exception thrown SelectLabClients", ex);
            }
            finally
            {
                myConnection.Close();
            }

            return lc;
        }

        /// <summary>
        /// to retrieve lab client metadata for lab clients specified by array of lab client IDs 
        /// </summary>
        public static LabClient SelectLabClient(string clientGuid)
        {
            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("Client_RetrieveByGuid", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@clientGuid",clientGuid, DbType.String,50));
            LabClient lc = new LabClient();
            try
            {
                myConnection.Open();


                // get labclient info from table lab_clients
                DbDataReader myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    if (myReader["client_ID"] != System.DBNull.Value)
                        lc.clientID= (int)myReader["client_ID"];
                    if (myReader["lab_client_name"] != System.DBNull.Value)
                        lc.clientName = (string)myReader["lab_client_name"];
                    if (myReader["short_description"] != System.DBNull.Value)
                        lc.clientShortDescription = (string)myReader["short_description"];
                    if (myReader["long_description"] != System.DBNull.Value)
                        lc.clientLongDescription = (string)myReader["long_description"];
                    if (myReader["version"] != System.DBNull.Value)
                        lc.version = (string)myReader["version"];
                    if (myReader["loader_script"] != System.DBNull.Value)
                        lc.loaderScript = (string)myReader["loader_script"];
                    if (myReader["description"] != System.DBNull.Value)
                        lc.clientType = (string)myReader["description"];
                    if (myReader["contact_email"] != System.DBNull.Value)
                        lc.contactEmail = (string)myReader["contact_email"];
                    if (myReader["contact_first_name"] != System.DBNull.Value)
                        lc.contactFirstName = (string)myReader["contact_first_name"];
                    if (myReader["contact_last_name"] != System.DBNull.Value)
                        lc.contactLastName = (string)myReader["contact_last_name"];
                    if (myReader["notes"] != System.DBNull.Value)
                        lc.notes = (string)myReader["notes"];
                    lc.needsScheduling = Convert.ToBoolean(myReader["needsScheduling"]);
                    lc.needsESS = Convert.ToBoolean(myReader["needsESS"]);
                    lc.IsReentrant = Convert.ToBoolean(myReader["isReentrant"]);
                    if (myReader["Client_Guid"] != System.DBNull.Value)
                        lc.clientGuid = (string)myReader["Client_Guid"];
                    if (myReader["Documentation_URL"] != System.DBNull.Value)
                        lc.documentationURL = (string)myReader["Documentation_URL"];
                   
                }
                myReader.Close();

                ////Retrieve  lab servers for a client

                //ArrayList lsIDs = new ArrayList();

                //DbCommand myCommand2 = FactoryDB.CreateCommand("LabServerClient_RetrieveServerIDs", myConnection);
                //myCommand2.CommandType = CommandType.StoredProcedure;
                //myCommand2.Parameters.Add(FactoryDB.CreateParameter(myCommand2, "@labClientID", lc.clientID, DbType.Int32));

                //DbDataReader myReader2 = myCommand2.ExecuteReader();

                //while (myReader2.Read())
                //{
                //    if (myReader2["agent_id"] != System.DBNull.Value)
                //        lsIDs.Add(Convert.ToInt32(myReader2["agent_id"]));
                //}

                //myReader2.Close();

                //// Convert to an int array and add to the current LabClient object
                //lc.labServerIDs = Utilities.ArrayListToIntArray(lsIDs);


                ////Retrieve info urls for a client

                //ArrayList infoURLs = new ArrayList();

                //myCommand2 = FactoryDB.CreateCommand("ClientInfo_Retrieve", myConnection);
                //myCommand2.CommandType = CommandType.StoredProcedure;
                //myCommand2.Parameters.Add(FactoryDB.CreateParameter(myCommand2, "@labClientID", lc.clientID, DbType.Int32));

                //DbDataReader myReader3 = myCommand2.ExecuteReader();

                //while (myReader3.Read())
                //{
                //    ClientInfo c = new ClientInfo();
                //    if (myReader3["info_url"] != System.DBNull.Value)
                //        c.infoURL = (string)myReader3["info_url"];
                //    if (myReader3["info_name"] != System.DBNull.Value)
                //        c.infoURLName = (string)myReader3["info_name"];
                //    if (myReader3["client_info_id"] != System.DBNull.Value)
                //        c.clientInfoID = Convert.ToInt32(myReader3["client_info_id"]);
                //    if (myReader3["description"] != System.DBNull.Value)
                //        c.description = ((string)myReader3["description"]);
                //    if (myReader3["display_order"] != System.DBNull.Value)
                //        c.displayOrder = (int)myReader3["display_order"];

                //    infoURLs.Add(c);
                //}

                //myReader2.Close();

                //// Converting to a clientInfo array
                //lc.clientInfos = new ClientInfo[infoURLs.Count];
                //for (int j = 0; j < infoURLs.Count; j++)
                //{
                //    lc.clientInfos[j] = (ClientInfo)(infoURLs[j]);
                //}
            }

            catch (Exception ex)
            {
                throw new Exception("Exception thrown SelectLabClients", ex);
            }
            finally
            {
                myConnection.Close();
            }

            return lc;
        }

		/// <summary>
		/// to retrieve lab client metadata for lab clients specified by array of lab client IDs 
		/// </summary>
		public static LabClient[] SelectLabClients ( int[] clientIDs )
		{
            List<LabClient> clients = new List<LabClient>();

            DbConnection myConnection = FactoryDB.GetConnection();
			DbCommand myCommand = FactoryDB.CreateCommand("Client_Retrieve", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;
			myCommand.Parameters .Add(FactoryDB.CreateParameter( "@labClientID",null, DbType.Int32));

			try 
			{
				myConnection.Open ();
				
				for (int i =0; i < clientIDs.Length ; i++) 
				{
					myCommand.Parameters["@labClientID"].Value = clientIDs[i];

					// get labclient info from table lab_clients
					DbDataReader myReader = myCommand.ExecuteReader ();
					while(myReader.Read ())
                    {
                        LabClient lc = new LabClient();
						lc.clientID = clientIDs[i];

						if(myReader["lab_client_name"] != System.DBNull.Value )
							lc.clientName = (string) myReader["lab_client_name"];
						if(myReader["short_description"] != System.DBNull.Value )
							lc.clientShortDescription = (string) myReader["short_description"];
						if(myReader["long_description"] != System.DBNull.Value )
							lc.clientLongDescription = (string) myReader["long_description"];
						if(myReader["version"] != System.DBNull.Value )
							lc.version = (string) myReader["version"];
						if(myReader["loader_script"] != System.DBNull.Value )
							lc.loaderScript= (string) myReader["loader_script"];
						if(myReader["description"] != System.DBNull.Value )
							lc.clientType= (string) myReader["description"];
						if(myReader["contact_email"] != System.DBNull .Value )
							lc.contactEmail= (string) myReader["contact_email"];
						if(myReader["contact_first_name"]!= System.DBNull.Value)
							lc.contactFirstName = (string) myReader["contact_first_name"];
						if(myReader["contact_last_name"]!= System.DBNull.Value)
							lc.contactLastName = (string) myReader["contact_last_name"];
						if(myReader["notes"]!= System.DBNull.Value)
							lc.notes = (string) myReader["notes"];
                        lc.needsScheduling = Convert.ToBoolean(myReader["needsScheduling"]);
                        lc.needsESS = Convert.ToBoolean(myReader["needsESS"]);
                        lc.IsReentrant = Convert.ToBoolean(myReader["isReentrant"]);
                        if (myReader["Client_Guid"] != System.DBNull.Value)
                            lc.clientGuid = (string)myReader["Client_Guid"];
                        if (myReader["Documentation_URL"] != System.DBNull.Value)
                            lc.documentationURL = (string)myReader["Documentation_URL"];
                        clients.Add(lc);
						
					}
					myReader.Close ();
				}
 
			}
			catch (Exception ex)
			{
				throw new Exception("Exception thrown SelectLabClients",ex);
			}
			finally 
			{
				myConnection.Close ();
			}
            clients.Sort();
			return clients.ToArray();;
		}
       
        /// <summary>
        /// Returns the number of clients for a LabServer, If the group is greater than zero only the grups clients will be counted
        /// </summary>
        /// <param name="groupID">Greater than zero, or not used</param>
        /// <param name="labServerID">LabServer id</param>
        /// <returns></returns>
        public static int CountServerClients(int groupID, int labServerID){
            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("LabServerClient_Count", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.Parameters.Add(FactoryDB.CreateParameter("@groupID", groupID, DbType.Int32));
            myCommand.Parameters.Add(FactoryDB.CreateParameter("@serverID", labServerID, DbType.Int32));
            
            try
            {
                myConnection.Open();
                return Convert.ToInt32(myCommand.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                myConnection.Close();
            }
        }

public static int CountScheduledClients(int labServerID){
            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("LabServerClient_CountScheduledClients", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.Parameters.Add(FactoryDB.CreateParameter("@serverID", labServerID, DbType.Int32));
            
            try
            {
                myConnection.Open();
                return Convert.ToInt32(myCommand.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                myConnection.Close();
            }
        }
        /// <summary>
        /// Get the local labClientID from it's name. 
        /// </summary>
        /// <param name="name"></param>
        /// <returns>ClientID</returns>
        public static int GetLabClientIDFromName(string name)
        {
            int id = -1;
            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("LabClient_SelectByName", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@name",name, DbType.Single,256));
            myConnection.Open();
            try
            {
                int value = Convert.ToInt32(myCommand.ExecuteScalar());
                if (value > 0)
                {
                    id = value;
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
            return id;
        }


 /// <summary>
        /// Get the local labClientName from it's ID. Note this does not use a stored procedure
        /// as it was added as part of the service patch and did not want to require a database update.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>ClientID</returns>
        public static string GetLabClientName(int clientID)
        {
            string name = null;
            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("SELECT Lab_Client_Name from Lab_Clients where Client_ID ='" + clientID + "';", myConnection);
            myConnection.Open();
            try
            {
                name = Convert.ToString(myCommand.ExecuteScalar());
             
            }
            catch
            {
                throw;
            }
            finally
            {
                myConnection.Close();
            }
            return name;
        }

        /// <summary>
        /// Get the local labClient Guid from it's ID. Note this does not use a stored procedure
        /// as it was added as part of the service patch and did not want to require a database update.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>ClientID</returns>
        public static string GetLabClientGUID(int clientID)
        {
            string guid = null;
            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("SELECT Client_GUID from Lab_Clients where Client_ID ='" + clientID + "';", myConnection);
            myConnection.Open();
            try
            {
                guid = Convert.ToString(myCommand.ExecuteScalar());

            }
            catch
            {
                throw;
            }
            finally
            {
                myConnection.Close();
            }
            return guid;
        }
		/// <summary>
		/// to retrieve a list of all the client types in the database
		/// </summary>

		public static string[] SelectLabClientTypes ()
		{
			string[] clientTypes;

            DbConnection myConnection = FactoryDB.GetConnection();
			DbCommand myCommand = FactoryDB.CreateCommand("ClientTypes_Retrieve", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;

			try 
			{
				myConnection.Open ();
				

				// get labclient types from table clients_types
				DbDataReader myReader = myCommand.ExecuteReader ();
				ArrayList lcTypes = new ArrayList();

				while(myReader.Read ())
				{	
					if(myReader["description"] != System.DBNull.Value )
						lcTypes.Add((string)(myReader["description"]));
				}
				myReader.Close ();

				// Converting to a string array
				clientTypes = Utilities.ArrayListToStringArray(lcTypes);

			}
			catch (Exception ex)
			{
				throw new Exception("Exception thrown SelectLabClientTypes",ex);
			}
			finally 
			{
				myConnection.Close ();
			}
			
			return clientTypes;
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

            myCommand.Parameters.Add(FactoryDB.CreateParameter("@clientID", clientID, DbType.Int32));
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@userID", userID, DbType.Int32));
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@itemName", itemName, DbType.String,256));
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@itemValue", itemValue, DbType.String,2048));

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

            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@clientID", clientID, DbType.Int32));
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

            // ClientItem_Retrieve currently only returns the value
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



        /* !------------------------------------------------------------------------------!
         *							CALLS FOR USERS
         * !------------------------------------------------------------------------------!
         */

		/// <summary>
		/// to add a user
		/// The previous call used to be
		/// public static void InsertUser(User user, string principalID, string authenticationType, string initialGroupID)
		/// principal ID is automatically generated by the principals table.
		/// </summary>
        public static int InsertUser(User user, int authenticationTypeID, int initialGroupID)
        {
            return InsertUser(user.userName,user.authID,user.firstName,user.lastName,user.email,user.affiliation,user.reason,
                user.xmlExtension,false, authenticationTypeID, initialGroupID);
        }


		public static int InsertUser(string userName, int authID, string firstName, string lastName, string email,
            string affiliation, string reason, string xmlExtension, bool lockAccount,
            int authenticationTypeID, int initialGroupID)
		{
			// The Add User stored procedure first inserts a user into the Agents table
			//& then Agent Hierarchy, with the specified parent group id
			// The AgentID is then used as the primary key in the Users table
			// Finally user is also entered into the principals table

			int userID = -1;

			DbConnection myConnection = FactoryDB.GetConnection();
			DbCommand myCommand = FactoryDB.CreateCommand("User_Insert", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;

            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@userName", userName, DbType.String,256));
            myCommand.Parameters.Add(FactoryDB.CreateParameter("@authID", authID, DbType.Int32));
            if(firstName != null && firstName.Length >0)
			myCommand.Parameters.Add(FactoryDB.CreateParameter( "@firstName",firstName, DbType.String, 256 ));
            if (lastName != null && lastName.Length > 0)
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@lastName", lastName, DbType.String,256));
            if (email != null && email.Length > 0)
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@email", email, DbType.String,256));
            if (affiliation != null && affiliation.Length > 0)
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@affiliation", affiliation, DbType.String,256));
            if (reason != null && reason.Length > 0)
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@reason", reason, DbType.String,2048));
            if (xmlExtension != null && xmlExtension.Length > 0)
			myCommand.Parameters.Add(FactoryDB.CreateParameter( "@XMLExtension", xmlExtension, DbType.AnsiString));
			myCommand.Parameters.Add(FactoryDB.CreateParameter( "@lockUser",lockAccount,DbType.Boolean));
			myCommand.Parameters.Add(FactoryDB.CreateParameter( "@authenTypeID", authenticationTypeID,DbType.Int32));
			myCommand.Parameters.Add(FactoryDB.CreateParameter( "@initialGroupID", initialGroupID,DbType.Int32));

			try
			{
				myConnection.Open();
				userID = Int32.Parse ( myCommand.ExecuteScalar().ToString ());
			}
			catch (DbException sex)
			{
				throw new Exception("SQLException thrown in inserting user", sex);
			}
			catch (Exception ex)
			{
				throw new Exception("Exception thrown in inserting user",ex);
			}
			finally
			{
				myConnection.Close();
			}

			// refresh A & A-H in memory cache
			AuthCache.AgentsSet = InternalAuthorizationDB.RetrieveAgents();
			AuthCache.AgentHierarchySet = InternalAdminDB.RetrieveGroupHierarchy();

			return userID;
		}

		/// <summary>
		/// to modify a user
		/// The previous call used to be
		/// public static void UpdateUser(User user, string principalID, string authenticationType)
		/// principalID was changed to principalString a while ago
		/// </summary>
		public static void UpdateUser(User user, int authorityID, int authenticationTypeID)
		{
            UpdateUser(user.userID,user.userName,authorityID,authenticationTypeID,user.firstName,user.lastName,user.email,
                user.affiliation,user.reason,user.xmlExtension,user.lockAccount);
           
		}
        /// <summary>
        /// to modify a user
        /// The previous call used to be
        /// public static void UpdateUser(User user, string principalID, string authenticationType)
        /// principalID was changed to principalString a while ago
        /// </summary>
        public static void UpdateUser(int userID,string userName, int authorityID, int authenticationTypeID,
            string firstName,string lastName, string email, string affiliation, string reason, string xmlExtension,bool lockAccount)
        {
            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("User_Update", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            myCommand.Parameters.Add(FactoryDB.CreateParameter("@userID", userID, DbType.Int32));
            myCommand.Parameters.Add(FactoryDB.CreateParameter("@userName", userName, DbType.String, 256));
            myCommand.Parameters.Add(FactoryDB.CreateParameter("@firstName", firstName, DbType.String, 256));
            myCommand.Parameters.Add(FactoryDB.CreateParameter("@lastName", lastName, DbType.String, 256));
            myCommand.Parameters.Add(FactoryDB.CreateParameter("@email", email, DbType.String, 256));
            myCommand.Parameters.Add(FactoryDB.CreateParameter("@affiliation", affiliation, DbType.String, 256));
            myCommand.Parameters.Add(FactoryDB.CreateParameter("@reason", reason, DbType.String, 2048));
            myCommand.Parameters.Add(FactoryDB.CreateParameter("@XMLExtension", xmlExtension, DbType.AnsiString));
            myCommand.Parameters.Add(FactoryDB.CreateParameter("@lockUser", lockAccount, DbType.Boolean));
            myCommand.Parameters.Add(FactoryDB.CreateParameter("@authorityID", authorityID, DbType.Int32));
            myCommand.Parameters.Add(FactoryDB.CreateParameter("@authenTypeID", authenticationTypeID, DbType.Int32));

            try
            {
                myConnection.Open();
                int i = myCommand.ExecuteNonQuery();
                if (i == 0)
                    throw new Exception("No record exists exception");   //throws an exception if No records can be modified
            }
            catch (Exception ex)
            {
                throw new Exception("Exception thrown in updating user record", ex);
            }
            finally
            {
                myConnection.Close();
            }
        }


		/// <summary>
		/// to delete all user records specified by the array of user IDs
		/// </summary>
		public static int[] DeleteUsers ( int[] userIDs )
		{

            DbConnection myConnection = FactoryDB.GetConnection();
			DbCommand myCommand = FactoryDB.CreateCommand("User_Delete", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure ;
			myCommand.Parameters.Add(FactoryDB.CreateParameter("@userID",null,DbType.Int32));

			/*
			 * Note : Alternately ADO.NET could be used. However, the disconnected  DataAdapter object might prove
			 * extremely inefficient and hence this method was chosen
			 */
			 
			ArrayList arrayList = new ArrayList ();

			try 
			{
				myConnection.Open ();
											
				foreach (int userID in userIDs) 
				{
					// Deleting from table Users
					/*	
					 * IMPORTANT ! - The database if currently not set to Cascade delete for Agents and Users.
					 * Hence the stored procedure implements the following functionality:
					 * 1. When a user (specified by userID) is to be deleted, the agent is first deleted.
					 * 2. This cascade deletes the records in the Agent Hierarchy and Grants tables (which has to be manually done in the code if cascade delete
																				doesn't work)
					 * 3. Then the User is deleted from the Users Table
					 * 4. This cascade deletes the entries in the Principals table (which has to be manually done in the code if cascade delete doesn't work).
					 * 5. This cascade deletes the entries in the Experiment_Information 
					 *		Client_Items & User_sessions tables
					 */
					myCommand.Parameters["@userID"].Value = userID;
					if(myCommand.ExecuteNonQuery () == 0)
					{
						arrayList.Add(userID);
					}
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Exception thrown in DeleteUsers",ex);
			}
			finally 
			{
				myConnection.Close ();
			}

			// need refresh agent datasets in memory
			AuthCache.AgentHierarchySet = InternalAdminDB.RetrieveGroupHierarchy();
			AuthCache.AgentsSet = InternalAuthorizationDB.RetrieveAgents();

			int[] uIDs = Utilities.ArrayListToIntArray(arrayList);
			
			return uIDs;
		}

		/// <summary>
		/// to retrieve a list of all the users in the database
		/// </summary>
		public static int[] SelectUserIDs ()
		{
			int[] userIDs;

            DbConnection myConnection = FactoryDB.GetConnection();
			DbCommand myCommand = FactoryDB.CreateCommand("User_RetrieveIDs", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;

			try 
			{
				myConnection.Open ();

				// get User ids from table users
				DbDataReader myReader = myCommand.ExecuteReader ();
				ArrayList uIDs = new ArrayList();

				while(myReader.Read ())
				{	
					if(myReader["user_id"] != System.DBNull.Value )
						uIDs.Add(Convert.ToInt32(myReader["user_id"]));
				}
				myReader.Close ();

				// Converting to an int array
				userIDs = Utilities.ArrayListToIntArray(uIDs);
			}
			catch (Exception ex)
			{
				throw new Exception("Exception thrown SelectUserIDs",ex);
			}
			finally 
			{
				myConnection.Close ();
			}
			
			return userIDs;
		}

        /// <summary>
        /// to retrieve a list of all the users in the database
        /// </summary>
        public static int[] SelectUserIDsInGroup(int groupID)
        {
            List<int> userIDs = new List<int>();

            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("Group_RetrieveUserIDs", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.Parameters.Add(FactoryDB.CreateParameter("@groupID", groupID, DbType.Int32));
            try
            {
                myConnection.Open();

                // get User ids from table users
                DbDataReader myReader = myCommand.ExecuteReader();
               

                while (myReader.Read())
                {
                    if (myReader["user_id"] != System.DBNull.Value)
                        userIDs.Add(Convert.ToInt32(myReader["user_id"]));
                }
                myReader.Close();

               
            }
            catch (Exception ex)
            {
                throw new Exception("Exception thrown SelectUserIDs", ex);
            }
            finally
            {
                myConnection.Close();
            }

            return userIDs.ToArray();
        }


		/// <summary>
		/// to retrieve a list of all the orphaned users in the database i.e. users which no longer belong to a group
		/// </summary>
		public static int[] SelectOrphanedUserIDs ()
		{
			int[] userIDs;

            DbConnection myConnection = FactoryDB.GetConnection();
			DbCommand myCommand = FactoryDB.CreateCommand("User_RetrieveOrphanedIDs", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;

			try 
			{
				myConnection.Open ();
				

				// get User ids from table users
				DbDataReader myReader = myCommand.ExecuteReader ();
				ArrayList uIDs = new ArrayList();

				while(myReader.Read ())
				{	
					if(myReader["user_id"] != System.DBNull.Value )
						uIDs.Add(Convert.ToInt32(myReader["user_id"]));
				}
				myReader.Close ();

				// Converting to an int array
				userIDs = Utilities.ArrayListToIntArray(uIDs);
			}
			catch (Exception ex)
			{
				throw new Exception("Exception thrown SelectOrphanedUserIDs",ex);
			}
			finally 
			{
				myConnection.Close ();
			}
			
			return userIDs;
		}

        /// <summary>
        /// to retrieve user metadata for user specified by userID
        /// </summary>
        public static User SelectUser(int userID)
        {
            User u = null;
            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("User_Retrieve", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.Parameters.Add(FactoryDB.CreateParameter("@userID", userID, DbType.Int32));

            try
            {
                myConnection.Open();
                // get User info from table users 
                DbDataReader myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    u = new User();
                    u.userID = userID;
                    if (myReader["auth_id"] != System.DBNull.Value)
                        u.authID = Convert.ToInt32(myReader["auth_id"]);
                    if (myReader["user_name"] != System.DBNull.Value)
                        u.userName = (string)myReader["user_name"];
                    if (myReader["first_name"] != System.DBNull.Value)
                        u.firstName = (string)myReader["first_name"];
                    if (myReader["last_name"] != System.DBNull.Value)
                        u.lastName = (string)myReader["last_name"];
                    if (myReader["email"] != System.DBNull.Value)
                        u.email = (string)myReader["email"];
                    if (myReader["affiliation"] != System.DBNull.Value)
                        u.affiliation = (string)myReader["affiliation"];
                    if (myReader["xml_extension"] != System.DBNull.Value)
                        u.xmlExtension = (string)myReader["xml_extension"];
                    if (myReader["signup_reason"] != System.DBNull.Value)
                        u.reason = (string)myReader["signup_reason"];
                    if (myReader["date_created"] != System.DBNull.Value)
                        u.registrationDate = (DateTime)myReader["date_created"];
                    if (Convert.ToInt16(myReader["lock_user"]) == 0)
                        u.lockAccount = false;
                    else u.lockAccount = true;
                }
                myReader.Close();
            }

            catch (Exception ex)
            {
                throw new Exception("Exception thrown SelectUser", ex);
            }
            finally
            {
                myConnection.Close();
            }

            return u;
        }

		/// <summary>
		/// to retrieve user metadata for users specified by array of users 
		/// </summary>
		public static User[] SelectUsers ( int[] userIDs )
		{
            List<User> users = new List<User>();
			

            DbConnection myConnection = FactoryDB.GetConnection();
			DbCommand myCommand = FactoryDB.CreateCommand("User_Retrieve", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;
			myCommand.Parameters .Add(FactoryDB.CreateParameter( "@userID",null, DbType.Int32));

			try 
			{
				myConnection.Open ();
				
				for (int i =0; i < userIDs.Length ; i++) 
				{
					myCommand.Parameters["@userID"].Value = userIDs[i];

					// get User info from table users 
					DbDataReader myReader = myCommand.ExecuteReader ();
					while(myReader.Read ())
					{
                        User u = new User();
						u.userID = userIDs[i];

                        if (myReader["auth_id"] != System.DBNull.Value)
                            u.authID = Convert.ToInt32(myReader["auth_id"]);
						if(myReader["user_name"] != System.DBNull.Value )
							u.userName= (string) myReader["user_name"];
						if(myReader["first_name"] != System.DBNull.Value )
							u.firstName= (string) myReader["first_name"];
						if(myReader["last_name"] != System.DBNull.Value )
							u.lastName= (string) myReader["last_name"];
						if(myReader["email"] != System.DBNull .Value )
							u.email= (string) myReader["email"];
						if(myReader["affiliation"] != System.DBNull.Value )
							u.affiliation= (string) myReader["affiliation"];
						if(myReader["xml_extension"] != System.DBNull.Value )
							u.xmlExtension = (string) myReader["xml_extension"];
						if(myReader["signup_reason"] != System.DBNull .Value )
							u.reason= (string) myReader["signup_reason"];
						if(myReader["date_created"] != System.DBNull .Value)
							u.registrationDate = (DateTime) myReader["date_created"];
						if (Convert.ToInt16(myReader["lock_user"]) == 0)
							u.lockAccount = false;
						else u.lockAccount = true;
                        users.Add(u);
					}
					myReader.Close ();
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Exception thrown SelectUsers",ex);
			}
			finally 
			{
				myConnection.Close ();
			}
            users.Sort();
            return users.ToArray(); ;
		}

		/// <summary>
		/// to get a user's email given userName
		/// </summary>
		public static string SelectUserEmail(string userName)
		{
			
			DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("User_RetrieveEmail", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;

			myCommand.Parameters.Add(FactoryDB.CreateParameter("@userName", userName,DbType.String,256));
			
			try
			{
				myConnection.Open();
				return  myCommand.ExecuteScalar().ToString();
			}
			catch (Exception ex)
			{
				throw new Exception("Exception thrown in retrieving user email",ex);
			}
			finally
			{
				myConnection.Close();
			}
		}

		/// <summary>
		/// to get a user's ID given userName
		/// </summary>
		public static int SelectUserID(string userName, int authorityID)
		{
			
			DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("User_RetrieveID", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;

			myCommand.Parameters.Add(FactoryDB.CreateParameter("@userName", userName,DbType.String,256));
            myCommand.Parameters.Add(FactoryDB.CreateParameter("@authID", authorityID, DbType.Int32));
			
			try
			{
				myConnection.Open();
				int userID = Convert.ToInt32(myCommand.ExecuteScalar());

				//If user record doesn't exist return -1
				if (userID == 0)
						userID=-1;
				
				return userID;
			}
			catch (Exception ex)
			{
				throw new Exception("Exception thrown in retrieving user ID",ex);
			}
			finally
			{
				myConnection.Close();
			}
		}

        /// <summary>
        /// to get a user's ID given userName
        /// </summary>
        public static string SelectUserName(int userID)
        {

            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("User_RetrieveName", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@userID", userID, DbType.Int32));

            try
            {
                myConnection.Open();
                object obj = myCommand.ExecuteScalar();

                //If user record doesn't exist return null
                if (obj != null)
                    return obj.ToString();
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Exception thrown in retrieving user ID", ex);
            }
            finally
            {
                myConnection.Close();
            }
        }

		/* !------------------------------------------------------------------------------!
		 *							CALLS FOR USER SESSIONS
		 * !------------------------------------------------------------------------------!
		 */

		/// <summary>
		/// to insert a user session record. returns a database generated session id.
		/// </summary>
		public static long InsertUserSession(UserSession us)
		{
			
			DbConnection myConnection = FactoryDB.GetConnection();
			DbCommand myCommand = FactoryDB.CreateCommand("UserSession_insert", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;
			myCommand.Parameters.Add(FactoryDB.CreateParameter("@userID", us.userID,DbType.Int32));
            DbParameter paramGroup = FactoryDB.CreateParameter("@groupID",DbType.Int32);
            //if(us.groupID >0)
                paramGroup.Value = us.groupID;
            //else
            //    paramGroup.Value =DBNull.Value;
			myCommand.Parameters.Add( paramGroup);
             DbParameter paramClient = FactoryDB.CreateParameter("@clientID", DbType.Int32);
            //if(us.clientID >0)
                paramClient.Value = us.clientID;
            //else
            //    paramClient.Value =DBNull.Value;
            myCommand.Parameters.Add(paramClient);
            myCommand.Parameters.Add(FactoryDB.CreateParameter("@tzOffset", us.tzOffset,DbType.Int32));
			myCommand.Parameters.Add(FactoryDB.CreateParameter("@sessionKey", us.sessionKey,DbType.AnsiString,512));
			
			try
			{
				myConnection.Open();
				return Convert.ToInt64( myCommand.ExecuteScalar());
			}
			catch (Exception ex)
			{
				throw new Exception("Exception thrown in inserting user session",ex);
			}
			finally
			{
				myConnection.Close();
			}
		}

        /// <summary>
        /// to insert a user session record. returns a database generated session id.
        /// </summary>
        public static bool ModifyUserSession(long sessionID,int groupID,int clientID, string sessionKey)
        {

            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("UserSession_Update", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.Parameters.Add(FactoryDB.CreateParameter("@sessionID", sessionID,DbType.Int64));
            DbParameter paramGroup = FactoryDB.CreateParameter( "@groupID", DbType.Int32);
            //if (groupID > 0)
                paramGroup.Value = groupID;
            //else
            //    paramGroup.Value = DBNull.Value;
            myCommand.Parameters.Add(paramGroup);
            DbParameter paramClient = FactoryDB.CreateParameter( "@clientID", DbType.Int32);
            //if (clientID > 0)
                paramClient.Value = clientID;
            //else
                //paramClient.Value = DBNull.Value;
            myCommand.Parameters.Add(paramClient);
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@sessionKey", sessionKey, DbType.AnsiString,512));

            try
            {
                myConnection.Open();
                return Convert.ToBoolean(myCommand.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw new Exception("Exception thrown in inserting user session", ex);
            }
            finally
            {
                myConnection.Close();
            }
        }

        /// <summary>
        /// to insert a user session record. returns a database generated session id.
        /// </summary>
        public static bool SetSessionGroup(long sessionID, int groupID)
        {

            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("UserSession_UpdateGroup", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@sessionID", sessionID, DbType.Int64));
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@groupID", groupID, DbType.Int32));
            try
            {
                myConnection.Open();
                return Convert.ToBoolean(myCommand.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw new Exception("Exception thrown in inserting user session", ex);
            }
            finally
            {
                myConnection.Close();
            }
        }
        /// <summary>
        /// to insert a user session record. returns a database generated session id.
        /// </summary>
        public static bool SetSessionClient(long sessionID, int clientID)
        {
            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("UserSession_UpdateClient", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@sessionID", sessionID, DbType.Int64));
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@clientID", clientID, DbType.Int32));
            try
            {
                myConnection.Open();
                return Convert.ToBoolean(myCommand.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw new Exception("Exception thrown setting user session client", ex);
            }
            finally
            {
                myConnection.Close();
            }
        }

        /// <summary>
        /// to insert a user session record. returns a database generated session id.
        /// </summary>
        public static bool SetSessionKey(long sessionID, string key)
        {

            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("UserSession_UpdateKey", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.Parameters.Add(FactoryDB.CreateParameter("@sessionID", sessionID,DbType.Int64));
            myCommand.Parameters.Add(FactoryDB.CreateParameter("@sessionKey", key,DbType.AnsiString,512));
            try
            {
                myConnection.Open();
                object obj = myCommand.ExecuteScalar();
                return (Convert.ToInt32(obj) > 0);
            }
            catch (Exception ex)
            {
                throw new Exception("Exception thrown in inserting user session", ex);
            }
            finally
            {
                myConnection.Close();
            }
        }
        /// <summary>
        /// to insert a user session record. returns a database generated session id.
        /// </summary>
        public static bool SetSessionTimeZone(long sessionID, int tzOffset)
        {
            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("UserSession_UpdateTzOffset", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@sessionID", sessionID, DbType.Int64));
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@tzOffset", tzOffset, DbType.Int32));
            try
            {
                myConnection.Open();
                return Convert.ToBoolean(myCommand.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw new Exception("Exception thrown setting user session timezone", ex);
            }
            finally
            {
                myConnection.Close();
            }
        }

		/// <summary>
		/// to update the session end time in the user's session record -returns the user's session end time.
		/// </summary>
		public static bool SaveUserSessionEndTime(long sessionID)
		{
			
			DbConnection myConnection = FactoryDB.GetConnection();
			DbCommand myCommand = FactoryDB.CreateCommand("UserSession_UpdateEndTime", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;

            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@sessionID", sessionID, DbType.Int64));
			
			try
			{
				myConnection.Open();
				return Convert.ToBoolean( myCommand.ExecuteScalar());
			}
			catch (Exception ex)
			{
				throw new Exception("Exception thrown in updating user's session end time",ex);
			}
			finally
			{
				myConnection.Close();
			}
		}

        /// <summary>
        /// to select a user's sessions given the session IDs
        /// </summary>
        public static SessionInfo SelectSessionInfo(long sessionID)
        {
            SessionInfo sessionInfo = null;
           
            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("SessionInfo_Retrieve", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.Parameters.Add(FactoryDB.CreateParameter("@sessionID",sessionID,  DbType.Int64));
           
            try
            {
                myConnection.Open();
                // get session info from table user_sessions
                DbDataReader myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    sessionInfo = new SessionInfo();

                    sessionInfo.sessionID = sessionID;
                    sessionInfo.userID = myReader.GetInt32(0);
                    sessionInfo.groupID = myReader.GetInt32(1);
                    if (!myReader.IsDBNull(2))
                        sessionInfo.clientID = myReader.GetInt32(2);
                    else
                        sessionInfo.clientID = 0;
                    sessionInfo.userName = myReader.GetString(3);
                    sessionInfo.groupName = myReader.GetString(4);
                    sessionInfo.tzOffset = myReader.GetInt32(5);
                }
                myReader.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Exception thrown in selecting sessions given sessionIDs", ex);
            }
            finally
            {
                myConnection.Close();
            }
            return sessionInfo;
        }

		/// <summary>
		/// to select a user's sessions given the session IDs
		/// </summary>
		public static UserSession[] SelectUserSessions(long[] sessionIDs)
		{
			UserSession[] us = new UserSession[sessionIDs.Length ];
			for (int i=0; i<sessionIDs.Length ; i++)
			{
				us[i] = new UserSession();
			}

			DbConnection myConnection = FactoryDB.GetConnection();
			DbCommand myCommand = FactoryDB.CreateCommand("UserSession_Retrieve", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;
			myCommand.Parameters .Add(FactoryDB.CreateParameter("@sessionID",null,DbType.Int64));

			try 
			{
				myConnection.Open ();
				
				for (int i =0; i < sessionIDs.Length ; i++) 
				{
					myCommand.Parameters["@sessionID"].Value = sessionIDs[i];

					// get session info from table user_sessions
					DbDataReader myReader = myCommand.ExecuteReader ();
					while(myReader.Read ())
					{	
						us[i].sessionID = sessionIDs[i];
                        if (myReader["user_id"] != System.DBNull.Value)
                            us[i].userID = Convert.ToInt32(myReader["user_id"]);
                        if (myReader["effective_group_id"] != System.DBNull.Value)
                            us[i].groupID = Convert.ToInt32(myReader["effective_group_id"]);
                        if (myReader["client_id"] != System.DBNull.Value)
                            us[i].groupID = Convert.ToInt32(myReader["client_id"]);
						if(myReader["session_start_time"] != System.DBNull.Value )
							us[i].sessionStartTime = DateUtil.SpecifyUTC((DateTime) myReader["session_start_time"]);
						if(myReader["session_end_time"] != System.DBNull.Value )
							us[i].sessionEndTime= DateUtil.SpecifyUTC((DateTime) myReader["session_end_time"]);
						if(myReader["session_key"] != System.DBNull.Value )
							us[i].sessionKey= ((string)myReader["session_key"]);
                        if (myReader["tz_offset"] != System.DBNull.Value)
                            us[i].tzOffset = Convert.ToInt32(myReader["tz_offset"]);
					}
					myReader.Close ();
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Exception thrown in selecting sessions given sessionIDs",ex);
			}
			finally
			{
				myConnection.Close();
			}
			return us;
		}

		/// <summary>
		/// to select all the sessions of a given user
		/// </summary>
		public static UserSession[] SelectUserSessions(int userID, int groupID, DateTime timeAfter, DateTime timeBefore)
		{
			//UserSession[] userSessions = null;
			List<UserSession> sessions = new List<UserSession>();
			StringBuilder sqlQuery = new StringBuilder();
            int whereCount = 0;
						
			sqlQuery.Append("select session_ID, session_start_time, session_end_time,user_ID, effective_group_ID, session_key from user_sessions");
			if (userID > 0)
			{
                if (whereCount== 0 )
                    sqlQuery.Append(" WHERE");
                else
                     sqlQuery.Append(" AND");
                sqlQuery.Append(" user_ID = "); 
                sqlQuery.Append(userID);
                whereCount++;
			}
			if (groupID > 0)
			{
                if (whereCount == 0)
                    sqlQuery.Append(" WHERE");
                else
                    sqlQuery.Append(" AND");
                sqlQuery.Append(" effective_group_ID = ");
                sqlQuery.Append(groupID);
                whereCount++;
			}

			if (timeBefore.CompareTo(DateTime.MinValue)!=0)
			{
				if (whereCount == 0)
                    sqlQuery.Append(" WHERE");
                else
                    sqlQuery.Append(" AND");
				sqlQuery.Append(" session_start_time <= '");
                sqlQuery.Append(timeBefore);
                sqlQuery.Append("'");
                whereCount++;
			}

			if (timeAfter.CompareTo(DateTime.MinValue)!=0)
			{
			    if (whereCount == 0)
                    sqlQuery.Append(" WHERE");
                else
                    sqlQuery.Append(" AND");
				sqlQuery.Append(" session_start_time >= '");
                sqlQuery.Append(timeAfter);
                sqlQuery.Append("'");
                whereCount++;
			}

			DbConnection myConnection = FactoryDB.GetConnection();
			DbCommand myCommand = myConnection.CreateCommand();
            myCommand.CommandText = sqlQuery.ToString();
			myCommand.CommandType = CommandType.Text;
			

//			DbConnection myConnection = FactoryDB.GetConnection();
//			DbCommand myCommand = FactoryDB.CreateCommand("UserSessions_RetrieveAll", myConnection);
//			myCommand.CommandType = CommandType.StoredProcedure;
//			myCommand.Parameters .Add(new DbParameter ("@userID",userID));
//			myCommand.Parameters .Add(new DbParameter ("@groupID",groupID));
//			myCommand.Parameters .Add(new DbParameter ("@TimeAfter",timeAfter));
//			myCommand.Parameters .Add(new DbParameter ("@TimeBefore",timeBefore));

			try 
			{
				myConnection.Open ();
				
					// get session info from table user_sessions
					DbDataReader myReader = myCommand.ExecuteReader ();
					while(myReader.Read ())
					{	
						UserSession us = new UserSession();
						us.sessionID = Convert.ToInt64( myReader["session_id"]); //casting to (long) didn't work
						if(myReader["session_start_time"] != System.DBNull.Value )
							us.sessionStartTime = DateUtil.SpecifyUTC((DateTime) myReader["session_start_time"]);
						if(myReader["session_end_time"] != System.DBNull.Value )
							us.sessionEndTime= DateUtil.SpecifyUTC((DateTime) myReader["session_end_time"]);
						if(myReader["user_id"]!=System.DBNull.Value)
							us.userID=Convert.ToInt32(myReader["user_id"]);
						if(myReader["effective_group_id"] != System.DBNull.Value )
							us.groupID= Convert.ToInt32(myReader["effective_group_id"]);
						if(myReader["session_key"] != System.DBNull.Value )
							us.sessionKey= ((string)myReader["session_key"]);
							
						sessions.Add(us);

					}
					myReader.Close ();
				
				
			}
			catch (Exception ex)
			{
				throw new Exception("Exception thrown in selecting user session",ex);
			}
			finally
			{
				myConnection.Close();
			}
			return sessions.ToArray();
		}

        /// <summary>
        /// to select all the sessions of a given user
        /// </summary>
        public static DataSet SelectSessionHistory(int userID, int groupID, DateTime timeAfter, DateTime timeBefore)
        {

//            select s.Session_ID,u.User_name, h.modify_time,g.Group_Name, c.Lab_Client_Name,s.Session_Start_Time,s.Session_End_Time,h.Session_key
//from user_sessions s, session_history h, Users u, Groups g, Lab_Clients c
//where s.session_id = h.session_id and s.user_ID = u.user_id and h.group_ID = g.Group_ID and h.CLient_ID = c.client_ID
//order by s.session_id, h.modify_time
            int orderCode = 0;
            //UserSession[] userSessions = null;
            //ArrayList sessions = new ArrayList();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable("sessionHistory");
            StringBuilder sqlQuery = new StringBuilder("select s.Session_ID,u.User_name,s.Session_Start_Time,s.Session_End_Time, h.modify_time,");
            sqlQuery.Append("g.Group_Name, c.Lab_Client_Name,h.Session_key from user_sessions s, session_history h, Users u, Groups g, Lab_Clients c");
            
            StringBuilder sqlWhere = new StringBuilder(" where s.session_id = h.session_id and s.user_ID = u.user_id and h.group_ID = g.Group_ID and h.CLient_ID = c.client_ID");
           
 
            if (userID > 0)
            {
                sqlWhere.Append(" AND s.user_ID = ");
                sqlWhere.Append(userID);
                orderCode = 1;
            }
            if (groupID > 0)
            {
    //            select ah.agent_ID, ag.is_group, ag.agent_name
    //from   agent_hierarchy ah, agents ag
    //where ah.parent_group_ID = @groupID and ah.agent_id=ag.agent_id
                sqlWhere.Append(" AND h.Group_ID = " + groupID);
                if (orderCode == 0)
                {
                    sqlWhere.Append(" AND s.user_ID in (select ug.user_ID from User_Groups ug");
                    sqlWhere.Append(" where ug.group_ID = ");
                    sqlWhere.Append(groupID);
                }
                sqlWhere.Append(") ");
               // orderCode |= 2;
            }

            if (timeBefore.CompareTo(DateTime.MinValue) != 0)
            {
                sqlWhere.Append(" AND session_start_time <= '");
                sqlWhere.Append(timeBefore);
                sqlWhere.Append("'");
                //orderCode |= 4;
            }

            if (timeAfter.CompareTo(DateTime.MinValue) != 0)
            {

                sqlWhere.Append(" AND session_start_time >= '");
                sqlWhere.Append(timeAfter);
                sqlWhere.Append("'");
              //orderCode |= 4;
            }

             StringBuilder sqlOrder = new StringBuilder();
           if(orderCode == 1)
               sqlOrder.Append(" ORDER BY s.Session_ID,s.Session_Start_Time, h.modify_time ");
           else
                sqlOrder.Append(" ORDER BY u.User_name,s.Session_ID,s.Session_Start_Time, h.modify_time ");
            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = myConnection.CreateCommand();
            myCommand.CommandText = sqlQuery.ToString() + sqlWhere.ToString() + sqlOrder.ToString();
            myCommand.CommandType = CommandType.Text;


            //			DbConnection myConnection = FactoryDB.GetConnection();
            //			DbCommand myCommand = FactoryDB.CreateCommand("UserSessions_RetrieveAll", myConnection);
            //			myCommand.CommandType = CommandType.StoredProcedure;
            //			myCommand.Parameters .Add(new DbParameter ("@userID",userID));
            //			myCommand.Parameters .Add(new DbParameter ("@groupID",groupID));
            //			myCommand.Parameters .Add(new DbParameter ("@TimeAfter",timeAfter));
            //			myCommand.Parameters .Add(new DbParameter ("@TimeBefore",timeBefore));

            try
            {
                myConnection.Open();

                // get session info from table user_sessions
                DbDataReader myReader = myCommand.ExecuteReader();
               
                ds.Tables.Add(dt);
                dt.BeginLoadData();
                dt.Load(myReader);
                dt.EndLoadData();
                //while (myReader.Read())
                //{
                //    UserSession us = new UserSession();
                //    us.sessionID = Convert.ToInt64(myReader["session_id"]); //casting to (long) didn't work
                //    if (myReader["session_start_time"] != System.DBNull.Value)
                //        us.sessionStartTime = DateUtil.SpecifyUTC((DateTime)myReader["session_start_time"]);
                //    if (myReader["session_end_time"] != System.DBNull.Value)
                //        us.sessionEndTime = DateUtil.SpecifyUTC((DateTime)myReader["session_end_time"]);
                //    if (myReader["user_id"] != System.DBNull.Value)
                //        us.userID = Convert.ToInt32(myReader["user_id"]);
                //    if (myReader["effective_group_id"] != System.DBNull.Value)
                //        us.groupID = Convert.ToInt32(myReader["effective_group_id"]);
                //    if (myReader["session_key"] != System.DBNull.Value)
                //        us.sessionKey = ((string)myReader["session_key"]);

                //    sessions.Add(us);

                //}
                myReader.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Exception thrown in selecting user session", ex);
            }
            finally
            {
                myConnection.Close();
            }

            return ds;
        }
		
		// Will probably need new methods for user sessions by group and by time

		/* !------------------------------------------------------------------------------!
		 *							CALLS FOR GROUPS
		 * !------------------------------------------------------------------------------!
		 */

		/// <summary>
		/// to add a group
		/// </summary>
		public static int InsertGroup(Group grp, int parentGroupID, int associatedGroupID)
		{

			// The Add Group stored procedure first inserts a group into the Agents table
			//& then Agent Hierarchy, with the specified parent group id
			// The AgentID is then used as the primary key in the groups table

			// Corresponding qualifiers  are NOT added here since they
			//	usually only created after the actual group record has been created 

			int groupID = -1;

			DbConnection myConnection = FactoryDB.GetConnection();
			DbCommand myCommand = FactoryDB.CreateCommand("Group_Insert", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;

			myCommand.Parameters.Add(FactoryDB.CreateParameter("@groupName", grp.groupName,DbType.String,256));
			myCommand.Parameters.Add(FactoryDB.CreateParameter("@description", grp.description,DbType.String,2048));
			myCommand.Parameters.Add(FactoryDB.CreateParameter("@email", grp.email,DbType.String,256));
			myCommand.Parameters.Add(FactoryDB.CreateParameter("@parentGroupID",parentGroupID,DbType.Int32));
			myCommand.Parameters.Add(FactoryDB.CreateParameter("@groupType", grp.groupType,DbType.AnsiString,100));
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@associatedGroupID", associatedGroupID, DbType.Int32));
			
			try
			{
				myConnection.Open();
				groupID = Int32.Parse ( myCommand.ExecuteScalar().ToString ());
			}
			catch (Exception ex)
			{
				throw new Exception("Exception thrown in inserting group",ex);
			}
			finally
			{
				myConnection.Close();
			}

			// refresh A & A-H in memory
			AuthCache.AgentsSet = InternalAuthorizationDB.RetrieveAgents();
			AuthCache.AgentHierarchySet = InternalAdminDB.RetrieveGroupHierarchy();
				
			return groupID;
		}

		/// <summary>
		/// to modify a group
		/// </summary>
		public static void UpdateGroup(Group grp)
		{
			DbConnection myConnection = FactoryDB.GetConnection();
			DbCommand myCommand = FactoryDB.CreateCommand("Group_Update", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;

            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@groupID", grp.groupID, DbType.Int32));
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@groupName", grp.groupName, DbType.String,256));
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@description", grp.description, DbType.String, 2048));
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@email", grp.email, DbType.String, 256));
			
			try
			{
				myConnection.Open();
				int i = myCommand.ExecuteNonQuery();
				if(i == 0)
					throw new Exception ("No record modified exception");
			}
			catch (Exception ex)
			{
				throw new Exception("Exception thrown in modifying group",ex);
			}
			finally
			{
				myConnection.Close();
			}
		}

		/// <summary>
		/// to delete all group records specified by the array of group IDs
		/// </summary>
		/* IMPORTANT NOTE !
		 *  This method assumes that a group is empty. 
		 * So the admin API is responsible for calling the 
		 * RemoveMembersFromGroup method before this*/
		public static int[] DeleteGroups ( int[] groupIDs )
		{
            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("Group_Delete", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure ;
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@groupID", null, DbType.Int32));

			/*
			 * Note : Alternately ADO.NET could be used. However, the disconnected  DataAdapter object might prove
			 * extremely inefficient and hence this method was chosen
			 */
			 
			ArrayList arrayList = new ArrayList ();

			try 
			{
				myConnection.Open ();
											
				foreach (int groupID in groupIDs) 
				{
					// Deleting from table Groups
					/*	
					 * IMPORTANT ! - The database if currently not set to Cascade delete for Agents and Groups.
					 * Hence the stored procedure implements the following functionality:
					 * 1. When a group (specified by groupID) is to be deleted, the agent is first deleted.
					 * 2. This cascade deletes the records in the Agent Hierarchy and Grants tables (which has to be manually done in the code if cascade delete
																				doesn't work)
					 * 3. Then the Group is deleted from the Groups Table
					 * 4. This cascade deletes the entries in the Experiment_Information 
					 *		System_Messages & User_sessions tables
					 * 5. The corresponding group entries are deleted from the Qualifiers Table (
					 *		& hence Qualifier_Hierarchy by cascade delete)
					 * 6. The Experiment_Collection qualifier of the group is also deleted from the Qualifiers Table
					 */
					myCommand.Parameters["@groupID"].Value = groupID;
					if(myCommand.ExecuteNonQuery () == 0)
					{
						arrayList.Add(groupID);
					}
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Exception thrown in DeleteGroups",ex);
			}
			finally 
			{
				myConnection.Close ();
			}

			// refresh A-H in memory
			AuthCache.AgentHierarchySet = InternalAdminDB.RetrieveGroupHierarchy();
			AuthCache.AgentsSet = InternalAuthorizationDB.RetrieveAgents();

			// refresh in memory Q & Q-H
			AuthCache.QualifierSet  = InternalAuthorizationDB.RetrieveQualifiers ();
			AuthCache.QualifierHierarchySet = InternalAuthorizationDB.RetrieveQualifierHierarchy ();

			//Converting to int array
			int[] gIDs = Utilities.ArrayListToIntArray(arrayList);
				
			return gIDs;
		}

		/// <summary>
		/// to retrieve a list of all the group IDs in the database
		/// </summary>

		public static int[] SelectGroupIDs ()
		{
			int[] groupIDs;

            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("Group_RetrieveIDs", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;

			try 
			{
				myConnection.Open ();
				

				// get group ids from table groups
				DbDataReader myReader = myCommand.ExecuteReader ();
				ArrayList grpIDs = new ArrayList();

				while(myReader.Read ())
				{	
					if(myReader["group_id"] != System.DBNull.Value )
						grpIDs.Add(Convert.ToInt32(myReader["group_id"]));
				}
				myReader.Close ();

				// Converting to an int array
				groupIDs = Utilities.ArrayListToIntArray(grpIDs);
			}
			catch (Exception ex)
			{
				throw new Exception("Exception thrown SelectGroupIDs",ex);
			}
			finally 
			{
				myConnection.Close ();
			}
			
			return groupIDs;
		}

        public static int[] SelectGroupIDsByType(string typeName)
        {
            List<int> groupIDs = new List<int>();

            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("Group_RetrieveIDsByType", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@typeName", typeName, DbType.String, 256));

            try
            {
                myConnection.Open();
                // get group ids from table groups
                DbDataReader myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    if (myReader["group_id"] != System.DBNull.Value)
                        groupIDs.Add(Convert.ToInt32(myReader["group_id"]));
                }
                myReader.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Exception thrown SelectGroupIDs", ex);
            }
            finally
            {
                myConnection.Close();
            }

            return groupIDs.ToArray();
        }
        /// <summary>
        /// to retrieve a list of all the admin group IDs in the database
        /// </summary>

        public static int[] SelectAdminGroupIDs()
        {
            int[] groupIDs;

            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("Group_RetrieveAdminGroupIDs", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                myConnection.Open();


                // get group ids from table groups
                DbDataReader myReader = myCommand.ExecuteReader();
                ArrayList grpIDs = new ArrayList();

                while (myReader.Read())
                {
                    if (myReader["group_id"] != System.DBNull.Value)
                        grpIDs.Add(Convert.ToInt32(myReader["group_id"]));
                }
                myReader.Close();

                // Converting to an int array
                groupIDs = Utilities.ArrayListToIntArray(grpIDs);
            }
            catch (Exception ex)
            {
                throw new Exception("Exception thrown SelectAdminGroupIDs", ex);
            }
            finally
            {
                myConnection.Close();
            }

            return groupIDs;
        }

        protected static Group readGroup(DbDataReader reader)
        {
            // select  g.Group_ID, g.Group_Type_ID,g.Associated_Group_ID, g.Date_Created, g.group_name, 
            // g.description AS description, g.email, gt.description AS group_type
            Group grp = new Group();
            grp.groupID = reader.GetInt32(0);
            grp.groupTypeID = reader.GetInt32(1);
            grp.associatedGroupID = reader.GetInt32(2);
            grp.createTime = DateUtil.SpecifyUTC(reader.GetDateTime(3));
            grp.groupName = reader.GetString(4);
            if(!reader.IsDBNull(5))
                grp.description = reader.GetString(5);
            if (!reader.IsDBNull(6))
                grp.email = reader.GetString(6);
            grp.groupType = reader.GetString(7);

            return grp;
        }

        /// <summary>
        /// to retrieve group metadata for groups specified by array of group IDs 
        /// </summary>
        public static Group SelectGroup(int groupID)
        {
            Group g = null;
            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("Group_Retrieve", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.Parameters.Add(FactoryDB.CreateParameter("@groupID", groupID, DbType.Int32));

            try
            {
                myConnection.Open();
                    DbDataReader myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        g = readGroup(myReader);
                    }
                    myReader.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Exception thrown SelectGroups", ex);
            }
            finally
            {
                myConnection.Close();
            }

            return g;
        }


		/// <summary>
		/// to retrieve group metadata for groups specified by array of group IDs 
		/// </summary>
		public static Group[] SelectGroups ( int[] groupIDs )
		{
            List<Group> groups = new List<Group>();

            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("Group_Retrieve", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;
			myCommand.Parameters .Add(FactoryDB.CreateParameter("@groupID",null,DbType.Int32));

			try 
			{
				myConnection.Open ();
				
				for (int i =0; i < groupIDs.Length ; i++) 
				{
					myCommand.Parameters["@groupID"].Value = groupIDs[i];

					// get labserver info from table lab_servers
					DbDataReader myReader = myCommand.ExecuteReader ();
					while(myReader.Read ())
					{
                        groups.Add(readGroup(myReader));
					}
					myReader.Close ();
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Exception thrown SelectGroups",ex);
			}
			finally 
			{
				myConnection.Close ();
			}
            groups.Sort();
			return groups.ToArray();
		}

        /// <summary>
        /// to retrieve group metadata for groups specified by Type 
        /// </summary>
        public static Group[] SelectGroupsByType(string typeName)
        {
            List<Group> groups = new List<Group>();

            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("Group_RetrieveByType", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.Parameters.Add(FactoryDB.CreateParameter("@typeName", typeName, DbType.String));

            try
            {
                myConnection.Open();
                DbDataReader myReader = myCommand.ExecuteReader();
               while (myReader.Read())
                {
                    groups.Add(readGroup(myReader));
                }
                myReader.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Exception thrown SelectGroupsByType", ex);
            }
            finally
            {
                myConnection.Close();
            }
            groups.Sort();
            return groups.ToArray();
        }

		/// <summary>
		/// to get a group's ID given groupName
		/// </summary>
		public static int SelectGroupID(string groupName)
		{
			DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("Group_RetrieveID", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;

			myCommand.Parameters.Add(FactoryDB.CreateParameter("@groupName", groupName,DbType.String,256));
			
			try
			{
				myConnection.Open();

				int groupID = Convert.ToInt32(myCommand.ExecuteScalar());

				//If group record doesn't exist return -1
				if (groupID == 0)
					groupID=-1;

				return groupID;
			}
			catch (Exception ex)
			{
				throw new Exception("Exception thrown in retrieving group id",ex);
			}
			finally
			{
				myConnection.Close();
			}
		}

        /// <summary>
        /// to get a group's ID given groupName
        /// </summary>
        public static string SelectGroupName(int groupID)
        {
            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("Group_RetrieveName", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@groupID", groupID, DbType.Int32));

            try
            {
                myConnection.Open();

                object obj = myCommand.ExecuteScalar();
                if (obj != null)
                {
                    return obj.ToString();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception thrown in retrieving group name", ex);
            }
            finally
            {
                myConnection.Close();
            }
        }


		/// <summary>
		/// to get a group's associated ID given groupID
		/// </summary>
		public static int SelectAssociatedGroupID(int groupID)
		{
			DbConnection myConnection = FactoryDB.GetConnection();
			DbCommand myCommand = FactoryDB.CreateCommand("Group_RetrieveAssociatedGroupID", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;

			myCommand.Parameters.Add(FactoryDB.CreateParameter("@groupID", groupID,DbType.Int32));
			
			try
			{
				myConnection.Open();

				int associatedGroupID = Convert.ToInt32(myCommand.ExecuteScalar());

				//If group record doesn't exist return -1
				if (associatedGroupID == 0)
					associatedGroupID=-1;

				return associatedGroupID;
			}
			catch (Exception ex)
			{
				throw new Exception("Exception thrown in retrieving associated group id",ex);
			}
			finally
			{
				myConnection.Close();
			}
		}

		/// <summary>
		/// to get a group's request group ID given groupID
		/// </summary>
		public static int SelectGroupRequestGroup(int groupID)
		{
			DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("Group_RetrieveRequestGroupID", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;

            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@groupID", groupID, DbType.Int32));
			
			try
			{
				myConnection.Open();

				int reqGroupID = Convert.ToInt32(myCommand.ExecuteScalar());

				//If group record doesn't exist return -1
				if (reqGroupID == 0)
					reqGroupID=-1;

				return reqGroupID;
			}
			catch (Exception ex)
			{
				throw new Exception("Exception thrown in retrieving reqGroup id",ex);
			}
			finally
			{
				myConnection.Close();
			}
		}

		/// <summary>
		/// to get a group's course staff group ID given groupID
		/// </summary>
		public static int SelectGroupAdminGroupID(int groupID)
		{
			DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("Group_RetrieveAdminGroupID", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;

            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@groupID", groupID, DbType.Int32));
			
			try
			{
				myConnection.Open();

				int adminGroupID = Convert.ToInt32(myCommand.ExecuteScalar());

				//If group record doesn't exist return -1
				if (adminGroupID == 0)
					adminGroupID=-1;

				return adminGroupID;
			}
			catch (Exception ex)
			{
				throw new Exception("Exception thrown in retrieving adminGroup id",ex);
			}
			finally
			{
				myConnection.Close();
			}
		}

		/* !------------------------------------------------------------------------------!
		 *							CALLS FOR GROUPS - MEMBERS
		 * !------------------------------------------------------------------------------!
		 */

		/// <summary>
		/// to add a member to a group
		/// </summary>
		public static bool AddGroupToGroup(int memberID, int groupID)
		{
			DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("Group_AddGroup", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;

            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@groupID", groupID, DbType.Int32));
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@memberID", memberID, DbType.Int32));
			
			try
			{
				myConnection.Open();

				// Adding a member to the group
				/*	
				* IMPORTANT ! 
				* The stored procedure implements the following functionality:
				* 1. When a user is to be added a group, he/she is
					- Removed into orphaned users group (if they exist there), AND
					- Added to the group in the agent hierarchy
						
				* 2. When a subgroup is to be added to a group it is,
					- The qualifier corresponding to the subgroup is added under the qualifier corresponding to the parent group
					- If the subgroup has an experiment collection node, it is moved under the experiment collection node of the parent(if one exists)
						except if the parent is ROOT, in which case the subgroup experiment collection node is added under Qualifier ROOT
					- Added to the group in the agent hierarchy
				*/

				int i = myCommand.ExecuteNonQuery();
				if (i<=0) 
					return false;
				else
				{
					// refresh Agents & A-H in memory
					AuthCache.AgentHierarchySet = InternalAdminDB.RetrieveGroupHierarchy();
					AuthCache.AgentsSet = InternalAuthorizationDB.RetrieveAgents();

					// refresh in memory Q & Q-H
					AuthCache.QualifierSet  = InternalAuthorizationDB.RetrieveQualifiers ();
					AuthCache.QualifierHierarchySet = InternalAuthorizationDB.RetrieveQualifierHierarchy ();

					return true;
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Exception thrown in inserting member to group. ",ex);
			}
			finally
			{
				myConnection.Close();
			}
		}
        /// <summary>
        /// to add a member to a group
        /// </summary>
        public static bool AddUserToGroup(int userID, int groupID)
        {
            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("Group_AddUser", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            myCommand.Parameters.Add(FactoryDB.CreateParameter("@groupID", groupID, DbType.Int32));
            myCommand.Parameters.Add(FactoryDB.CreateParameter("@userID", userID, DbType.Int32));

            try
            {
                myConnection.Open();

                // Adding a member to the group
                /*	
                * IMPORTANT ! 
                * The stored procedure implements the following functionality:
                * 1. When a user is to be added a group, he/she is
                    - Removed into orphaned users group (if they exist there), AND
                    - Added to the group in the agent hierarchy
						
                * 2. When a subgroup is to be added to a group it is,
                    - The qualifier corresponding to the subgroup is added under the qualifier corresponding to the parent group
                    - If the subgroup has an experiment collection node, it is moved under the experiment collection node of the parent(if one exists)
                        except if the parent is ROOT, in which case the subgroup experiment collection node is added under Qualifier ROOT
                    - Added to the group in the agent hierarchy
                */

                int i = myCommand.ExecuteNonQuery();
                if (i <= 0)
                    return false;
                else
                {
                    //// refresh Agents & A-H in memory
                    //AuthCache.AgentHierarchySet = InternalAdminDB.RetrieveGroupHierarchy();
                    //AuthCache.AgentsSet = InternalAuthorizationDB.RetrieveAgents();

                    //// refresh in memory Q & Q-H
                    //AuthCache.QualifierSet = InternalAuthorizationDB.RetrieveQualifiers();
                    //AuthCache.QualifierHierarchySet = InternalAuthorizationDB.RetrieveQualifierHierarchy();

                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception thrown in inserting member to group. ", ex);
            }
            finally
            {
                myConnection.Close();
            }
        }

        public static bool IsGroupMember(int groupID, int memberID)
        {
            bool status = false;
            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("Group_IsGroupMember", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;

            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@groupID", groupID, DbType.Int32));
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@memberID", memberID, DbType.Int32));

            try
            {
                myConnection.Open();
                status = Convert.ToBoolean(myCommand.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw new Exception("Exception thrown in IsGroupMember. ", ex);
            }
            finally
            {
                myConnection.Close();
            }
            return status;
        }

        public static bool IsUserMember(int groupID, int userID)
        {
            bool status = false;
            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("Group_IsUserMember", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            myCommand.Parameters.Add(FactoryDB.CreateParameter("@groupID", groupID, DbType.Int32));
            myCommand.Parameters.Add(FactoryDB.CreateParameter("@userID", userID, DbType.Int32));

            try
            {
                myConnection.Open();
                status = Convert.ToBoolean(myCommand.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw new Exception("Exception thrown in IsUserMember. ", ex);
            }
            finally
            {
                myConnection.Close();
            }
            return status;
        }
		/// <summary>
		/// to remove a member from a group
		/// </summary>
		public static bool RemoveGroupFromGroup(int memberID, int groupID)
		{
			DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("Group_RemoveGroup", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;

            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@groupID", groupID, DbType.Int32));
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@memberID", memberID, DbType.Int32));
			
			try
			{
				myConnection.Open();

				// Deleting a member from the group
				/*	
				* IMPORTANT ! 
				* The stored procedure implements the following functionality:
				* 1. When a user is to be removed from a group, he/she is either
					- Moved into orphaned users group (if they are part of only 1 group,
						 from which they're being removed) OR,
					- Just removed from the group (relationship severed in agent hierarchy table)
						if they're part of multiple groups
						
				* 2. When a subgroup is to be removed from a group it is either,
					- Moved to ROOT (if it is part of only one group from which it's being removed).
						Consequently, the corresponding group qualifier is also moved under the 
						Qualifier root & the corresponding experiment collection qualifier 
						is also moved under Qualifier root, OR
					- Just removed from the group (relationship severed in agent hierarchy table)
						if they're part of multiple groups. The corresponding group and
						experiment collection qualifier relationships are also removed.
				*/
				int i = myCommand.ExecuteNonQuery();
				if (i <=0)
					return false;
				else
				{
					// refresh Agents & A-H in memory
					AuthCache.AgentHierarchySet = InternalAdminDB.RetrieveGroupHierarchy();
					AuthCache.AgentsSet = InternalAuthorizationDB.RetrieveAgents();

					// refresh in memory Q & Q-H
					AuthCache.QualifierSet  = InternalAuthorizationDB.RetrieveQualifiers ();
					AuthCache.QualifierHierarchySet = InternalAuthorizationDB.RetrieveQualifierHierarchy ();

					return true;
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Exception thrown in removing member from group. ",ex);
			}
			finally
			{
				myConnection.Close();
			}
		}

        /// <summary>
		/// to remove a usser from a group
		/// </summary>
		public static bool RemoveUserFromGroup(int userID, int groupID)
		{
            bool status = false;
			DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("Group_RemoveUser", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;

            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@groupID", groupID, DbType.Int32));
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@userID", userID, DbType.Int32));
			
			try
			{
				myConnection.Open();

                int i = myCommand.ExecuteNonQuery();
                status = (i > 0);
            }
            catch (Exception ex)
            {
                throw new Exception("Exception thrown in inserting member to group. ", ex);
            }
            finally
            {
                myConnection.Close();
            }
            return status;
        }

        /// <summary>
        /// to add a member to a group
        /// </summary>
        public static bool MoveGroupToGroup(int memberID, int fromID, int groupID)
        {
            bool status = false;
            try
            {
                status = AddGroupToGroup(memberID, groupID);
                if (!status)
                {
                    return false;
                }
                else
                {
                    int orphanID = -1;
                    DbConnection myConnection = FactoryDB.GetConnection();
                    try
                    {
                        DbCommand isOrphanCommand = FactoryDB.CreateCommand("Group_RetrieveID", myConnection);
                        isOrphanCommand.CommandType = CommandType.StoredProcedure;
                        isOrphanCommand.Parameters.Add(FactoryDB.CreateParameter("@groupName", Group.ORPHANEDGROUP, DbType.AnsiString, 100));
                        myConnection.Open();
                        orphanID = Convert.ToInt32(isOrphanCommand.ExecuteScalar());
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Exception thrown in moving member to group. ", e);
                    }
                    finally
                    {
                        myConnection.Close();
                    }
                    if (orphanID == fromID)
                    {
                        // if the from group is Orphan the member should already have been removed
                        // Rebuild the Cache
                        status = true;
                        // refresh Agents & A-H in memory
                        AuthCache.AgentHierarchySet = InternalAdminDB.RetrieveGroupHierarchy();
                        AuthCache.AgentsSet = InternalAuthorizationDB.RetrieveAgents();

                        // refresh in memory Q & Q-H
                        AuthCache.QualifierSet = InternalAuthorizationDB.RetrieveQualifiers();
                        AuthCache.QualifierHierarchySet = InternalAuthorizationDB.RetrieveQualifierHierarchy();
                    }
                    else
                    {
                        status = RemoveGroupFromGroup(memberID, fromID);
                    }
                    return status;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception thrown in moving member to group. ", ex);
            }

        }
        /// <summary>
        /// to add a member to a group
        /// </summary>
        public static bool MoveUserToGroup(int memberID, int fromID, int groupID)
        {
            bool status = false;
            try
            {
                status = AddUserToGroup(memberID, groupID);
                if (!status)
                {
                    return status;
                }
                else
                {
                    int orphanID = -1;
                    DbConnection myConnection = FactoryDB.GetConnection();
                    try
                    {
                        DbCommand isOrphanCommand = FactoryDB.CreateCommand("Group_RetrieveID", myConnection);
                        isOrphanCommand.CommandType = CommandType.StoredProcedure;
                        isOrphanCommand.Parameters.Add(FactoryDB.CreateParameter("@groupName", Group.ORPHANEDGROUP,DbType.AnsiString,100));
                        myConnection.Open();
                        orphanID = Convert.ToInt32(isOrphanCommand.ExecuteScalar());
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Exception thrown in moving member to group. ", e);
                    }
                    finally
                    {
                        myConnection.Close();
                    }
                    if (orphanID == fromID)
                    {
                        // if the from group is Orphan the member should already have been removed
                        // Rebuild the Cache
                        status = true;
                        // refresh Agents & A-H in memory
                        AuthCache.AgentHierarchySet = InternalAdminDB.RetrieveGroupHierarchy();
                        AuthCache.AgentsSet = InternalAuthorizationDB.RetrieveAgents();

                        // refresh in memory Q & Q-H
                        AuthCache.QualifierSet = InternalAuthorizationDB.RetrieveQualifiers();
                        AuthCache.QualifierHierarchySet = InternalAuthorizationDB.RetrieveQualifierHierarchy();
                    }
                    else
                    {
                        status = RemoveUserFromGroup(memberID, fromID);
                    }
                    return status;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception thrown in moving member to group. ", ex);
            }

        }
        /// <summary>
        /// returns the GroupHierarchy table in a dataset
        /// </summary>
        public static DataSet RetrieveGroupHierarchy()
        {
            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("Group_RetrieveHierarchyTable", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;
            DataSet ds = new DataSet();

            try
            {
                DbDataAdapter dataAdapter = FactoryDB.CreateDataAdapter();
                dataAdapter.SelectCommand = myCommand;
                dataAdapter.TableMappings.Add("Table", "Agent_Hierarchy");

                dataAdapter.Fill(ds);

            }
            catch (Exception ex)
            {
                throw new Exception("Exception thrown in Group_RetrieveHierarchyTable. " + ex.Message, ex);
            }
            finally
            {
                myConnection.Close();
            }
            return ds;
        }

        /// <summary>
        /// to find all the parent groups of an agent
        /// </summary>
        public static int[] ListNonRequestGroupIDs(int userID)
        {
            List<Int32> aList = new List<Int32>();

            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("User_RetrieveNonRequestGroupIDs", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            myCommand.Parameters.Add(FactoryDB.CreateParameter("@userID", userID, DbType.Int32));

            try
            {
                myConnection.Open();
                DbDataReader myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    if (!myReader.IsDBNull(0))
                        aList.Add(myReader.GetInt32(0));
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception thrown in finding nonRequestGroupIDs", ex);
            }
            finally
            {
                myConnection.Close();
            }
            return aList.ToArray();
        }

        /// <summary>
        /// to find all the parent groups of an group
        /// </summary>
        public static int[] ListGroupParentIDs(int groupID)
        {
            ArrayList aList = new ArrayList();

            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("Group_RetrieveParentIDs", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            myCommand.Parameters.Add(FactoryDB.CreateParameter("@groupID", groupID, DbType.Int32));

            try
            {
                myConnection.Open();
                DbDataReader myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    if (myReader["parent_group_ID"] != System.DBNull.Value)
                        aList.Add(Convert.ToInt32(myReader["parent_group_ID"]));
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception thrown in finding parent groups of agent", ex);
            }
            finally
            {
                myConnection.Close();
            }

            //Convert to an int array
            int[] st = Utilities.ArrayListToIntArray(aList);

            return st;
        }

		/// <summary>
		/// to retrieve a list of all the members of a group
		/// </summary>
		public static int[] SelectGroupIDsInGroup (int groupID)
		{
            List<int> mIDs = new List<int>();
            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("Group_RetrieveChildrenGroupIDs", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;

            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@groupID", groupID, DbType.Int32));

			try 
			{
				myConnection.Open ();

				// get Member IDs from table Agent_Hierarchy
				DbDataReader myReader = myCommand.ExecuteReader ();
				
                int id = 0;
				while(myReader.Read ())
				{
                    id = myReader.GetInt32(0); 
                    mIDs.Add(id);
				}
				myReader.Close ();

			}
			catch (Exception ex)
			{
				throw new Exception("Exception thrown Select Group's Members",ex);
			}
			finally 
			{
				myConnection.Close ();
			}
			return mIDs.ToArray();
		}

        /// <summary>
        /// </summary>
        /// <param name="groupID">the ID of the Group whose members are to be listed</param>
        /// <returns>a string array containing the member IDs of all members of the Group. A subgroup is listed by the subgroup ID, not by enumerating all members of the subgroup</returns>
        public static int[] ListGroupIDsForUser(int userID)
        {
           List<int> groupIds = new List<int>();
            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("User_RetrieveGroupIDs", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;

            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@userID", userID, DbType.Int32));

			try 
			{
				myConnection.Open ();

				// get Member IDs from table Agent_Hierarchy
				DbDataReader myReader = myCommand.ExecuteReader ();
				while(myReader.Read ())
				{	
					groupIds.Add(myReader.GetInt32(0));
				}
				myReader.Close ();
			}
			catch (Exception ex)
			{
				throw new Exception("Exception thrown Select User's Groups",ex);
			}
			finally 
			{
				myConnection.Close ();
			}
            return groupIds.ToArray();
        }
	

		/// <summary>
		/// Method is equivalent to "ListMemberIDsInGroup", except that it acts upon a DataSet rather than on the Database
		/// </summary>
		/// <param name="groupID">the ID of the Group whose members are to be listed</param>
		/// <returns>a string array containing the member IDs of all members of the Group. A subgroup is listed by the subgroup ID, not by enumerating all members of the subgroup</returns>
		public static int[] ListMemberIDsInGroupFromDS(int groupID)
		{
			DataTable hierarchyTable = AuthCache.AgentHierarchySet.Tables[0];
			ArrayList memberIDsList = new ArrayList();
			foreach(DataRow dataRow in hierarchyTable.Rows)
			{
				if(Convert.ToInt32(dataRow["parent_group_id"])== groupID)
				{
					memberIDsList.Add(Convert.ToInt32(dataRow["agent_id"]));
				}
			}
			int[] memberIDs = new int[memberIDsList.Count];
			int i = 0;
			foreach(int memberID in memberIDsList)
			{
				memberIDs[i] = memberID;
				i++;
			}
			return memberIDs;
		}
	

		/* !------------------------------------------------------------------------------!
		 *							CALLS FOR SYSTEM MESSAGES
		 * !------------------------------------------------------------------------------!
		 */
		/// <summary>
		/// to add a system message
		/// </summary>
		
		public static int InsertSystemMessage(SystemMessage sm)
		{
			int messageID =0;

			DbConnection myConnection = FactoryDB.GetConnection();
			DbCommand myCommand = FactoryDB.CreateCommand("SystemMessage_Insert", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;

			myCommand.Parameters.Add(FactoryDB.CreateParameter("@messageType", sm.messageType, DbType.AnsiString,100 ));
			myCommand.Parameters.Add(FactoryDB.CreateParameter("@messageTitle", sm.messageTitle, DbType.String,256 ));
            
			myCommand.Parameters.Add(FactoryDB.CreateParameter("@toBeDisplayed", sm.toBeDisplayed,DbType.Boolean));
            
            if(sm.clientID >0)
                myCommand.Parameters.Add(FactoryDB.CreateParameter( "@clientID", sm.clientID, DbType.Int32));
            else
                myCommand.Parameters.Add(FactoryDB.CreateParameter( "@clientID", null, DbType.Int32));
            if(sm.agentID >0)
                myCommand.Parameters.Add(FactoryDB.CreateParameter( "@agentID", sm.agentID, DbType.Int32));
            else
                myCommand.Parameters.Add(FactoryDB.CreateParameter( "@agentID", null, DbType.Int32));
            if(sm.groupID >0)
                myCommand.Parameters.Add(FactoryDB.CreateParameter( "@groupID", sm.groupID, DbType.Int32));
            else
                myCommand.Parameters.Add(FactoryDB.CreateParameter( "@groupID", null, DbType.Int32));
           
            myCommand.Parameters.Add(FactoryDB.CreateParameter("@messageBody", sm.messageBody, DbType.String,3000));	

			try
			{
				myConnection.Open();
				messageID = Int32.Parse ( myCommand.ExecuteScalar().ToString ());
			}
			catch (Exception ex)
			{
				throw new Exception("Exception thrown in inserting system message",ex);
			}
			finally
			{
				myConnection.Close();
			}

			return messageID;
		}

		/// <summary>
		/// to modify a system message
		/// </summary>
		
		public static void UpdateSystemMessage(SystemMessage sm)
		{
			DbConnection myConnection = FactoryDB.GetConnection();
			DbCommand myCommand = FactoryDB.CreateCommand("SystemMessage_Update", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;

            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@messageID", sm.messageID, DbType.Int32));
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@messageType", sm.messageType, DbType.AnsiString,100));
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@messageTitle", sm.messageTitle, DbType.String,256));
			myCommand.Parameters.Add(FactoryDB.CreateParameter("@toBeDisplayed", sm.toBeDisplayed, DbType.Boolean));
            if (sm.clientID > 0)
                myCommand.Parameters.Add(FactoryDB.CreateParameter( "@clientID", sm.clientID, DbType.Int32));
            else
                myCommand.Parameters.Add(FactoryDB.CreateParameter( "@clientID", null, DbType.Int32));
            if (sm.agentID > 0)
                myCommand.Parameters.Add(FactoryDB.CreateParameter( "@agentID", sm.agentID, DbType.Int32));
            else
                myCommand.Parameters.Add(FactoryDB.CreateParameter( "@agentID", null, DbType.Int32));
            if (sm.groupID > 0)
                myCommand.Parameters.Add(FactoryDB.CreateParameter( "@groupID", sm.groupID, DbType.Int32));
            else
                myCommand.Parameters.Add(FactoryDB.CreateParameter( "@groupID", null, DbType.Int32));
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@messageBody", sm.messageBody, DbType.String));	

			try
			{
				myConnection.Open();
				int i = myCommand.ExecuteNonQuery();
	
				if(i == 0)
					throw new Exception ("No record modified exception");
			}
			catch (Exception ex)
			{
				throw new Exception("Exception thrown in updating system message",ex);
			}
			finally
			{
				myConnection.Close();
			}
		}

		/// <summary>
		/// to delete all system message records specified by the array of system message IDs
		/// </summary>
		public static int[] DeleteSystemMessages ( int[] systemMessageIDs )
		{

            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("SystemMessage_DeleteByID", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure ;
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@messageID", null, DbType.Int32));

			/*
			 * Note : Alternately ADO.NET could be used. However, the disconnected  DataAdapter object might prove
			 * extremely inefficient and hence this method was chosen
			 */
			 
			ArrayList arrayList = new ArrayList ();

			try 
			{
				myConnection.Open ();
											
				foreach (int messageID in systemMessageIDs) 
				{
					// Deleting from table SystemMessages
					
					myCommand.Parameters["@messageID"].Value = messageID;
					if(myCommand.ExecuteNonQuery () == 0)
					{
						arrayList.Add(messageID);
					}
				}

			}
			catch (Exception ex)
			{
				throw new Exception("Exception thrown in DeleteSystemMessages",ex);
			}
			finally 
			{
				myConnection.Close ();
			}

			int[] smIDs = Utilities.ArrayListToIntArray(arrayList);

			return smIDs;
		}

		/// <summary>
		/// to delete all system message records specified by the messageType and groupID and labServerID
		/// not sure if this method is being used though
		/// </summary>
		public static void DeleteSystemMessages ( string messageType,int groupID, int labServerID )
		{

            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("SystemMessages_Delete", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure ;
			myCommand.Parameters.Add(FactoryDB.CreateParameter("@messageType",messageType, DbType.AnsiString,100));
            if (groupID == 0)
                myCommand.Parameters.Add(FactoryDB.CreateParameter("@groupID", System.DBNull.Value, DbType.Int32));
            else
                myCommand.Parameters.Add(FactoryDB.CreateParameter( "@groupID", groupID, DbType.Int32));
            if(labServerID==0)
                myCommand.Parameters.Add(FactoryDB.CreateParameter( "@labServerID", System.DBNull.Value, DbType.Int32));
                else
                myCommand.Parameters.Add(FactoryDB.CreateParameter( "@labServerID", labServerID, DbType.Int32));

			try
			{
				myConnection.Open();
				int i = myCommand.ExecuteNonQuery();
				if(i == 0)
					throw new Exception ("No record deleted exception");
			}
			catch (Exception ex)
			{
				throw new Exception("Exception thrown in DeleteSystemMessages",ex);
			}
			finally 
			{
				myConnection.Close ();
			}

		}

		/// <summary>
		/// to retrieve system message metadata for systemMessages specified by array of systemMessage IDs 
		/// This method only displays the messages where the to_be_displayed is set to true (0)
		/// </summary>
		public static SystemMessage[] SelectSystemMessages ( int[] systemMessageIDs )
		{
			SystemMessage[] sm = new SystemMessage[systemMessageIDs.Length];
			for (int i=0; i<systemMessageIDs.Length ; i++)
			{
				sm[i] = new SystemMessage();
			}

            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("SystemMessage_RetrieveByID", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;
			myCommand.Parameters .Add(FactoryDB.CreateParameter("@messageID",null,DbType.Int32));

			try 
			{
				myConnection.Open ();
				
				for (int i =0; i < systemMessageIDs.Length ; i++) 
				{
					myCommand.Parameters["@messageID"].Value = systemMessageIDs[i];

					// get systemMessage info from table system_messages
					DbDataReader myReader = myCommand.ExecuteReader ();
					while(myReader.Read ())
					{	
						sm[i].messageID = systemMessageIDs[i];

						if(myReader["message_body"] != System.DBNull.Value )
							sm[i].messageBody = (string) myReader["message_body"];
						byte tbd = 0;
						if(myReader["to_be_displayed"] != System.DBNull.Value ) 
						{
							tbd = Convert.ToByte( myReader["to_be_displayed"]);
						}
						if (tbd ==1)
							sm[i].toBeDisplayed = true;
						else 
							sm[i].toBeDisplayed = false;
						if(myReader["description"] != System.DBNull.Value )
							sm[i].messageType = (string) myReader["description"];
						if(myReader["last_modified"] != System.DBNull.Value )
							sm[i].lastModified = DateUtil.SpecifyUTC(Convert.ToDateTime(myReader["last_modified"]));
                        if (myReader["agent_id"] != System.DBNull.Value)
                            sm[i].agentID = Convert.ToInt32(myReader["agent_id"]);
                        if (myReader["group_id"] != System.DBNull.Value)
						sm[i].groupID = Convert.ToInt32(myReader["group_id"]);
                    if (myReader["client_id"] != System.DBNull.Value)
						sm[i].clientID = Convert.ToInt32(myReader["client_id"]);
						if(myReader["message_title"] != System.DBNull.Value )
							sm[i].messageTitle = (string) myReader["message_title"];
					}
					myReader.Close ();
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Exception thrown SelectSystemMessages",ex);
			}
			finally 
			{
				myConnection.Close ();
			}
			
			return sm;
		}
        public static SystemMessage[] SelectSystemMessagesForGroup(int groupID)
        {
            ArrayList groups = new ArrayList();

            //int[] groups = InternalAuthorizationDB.ListAgentParents(groupID);
            AuthorizationUtilities.GetGroupAncestors(groupID, groups);
            if(!groups.Contains(groupID))
                groups.Add(groupID);
            List<SystemMessage> systemMessages = new List<SystemMessage>();
            if (groups != null && groups.Count > 0)
            {
                DbConnection myConnection = FactoryDB.GetConnection();
                DbCommand myCommand = FactoryDB.CreateCommand("SystemMessages_RetrieveByGroup", myConnection);
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.Parameters.Add(FactoryDB.CreateParameter( "@groupIds", Utilities.ToCSV(Utilities.ArrayListToIntArray(groups)), DbType.AnsiString, 4000));
                try
                {
                    myConnection.Open();

                    // get systemMessage info from table system_messages
                    DbDataReader myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        SystemMessage sm = new SystemMessage();
                        sm.messageType = "Group";
                        if (myReader["system_message_id"] != System.DBNull.Value)
                            sm.messageID = Convert.ToInt32(myReader["system_message_id"]);

                        if (myReader["message_body"] != System.DBNull.Value)
                            sm.messageBody = (string)myReader["message_body"];
                        byte tbd = 0;
                        if (myReader["to_be_displayed"] != System.DBNull.Value)
                        {
                            tbd = Convert.ToByte(myReader["to_be_displayed"]);
                        }
                        if (tbd == 1)
                            sm.toBeDisplayed = true;
                        else
                            sm.toBeDisplayed = false;
                        if (myReader["last_modified"] != System.DBNull.Value)
                            sm.lastModified = DateUtil.SpecifyUTC(Convert.ToDateTime(myReader["last_modified"]));
                        if (myReader["message_title"] != System.DBNull.Value)
                            sm.messageTitle = (string)myReader["message_title"];

                        systemMessages.Add(sm);

                    }
                    myReader.Close();
                }
                catch (Exception ex)
                {
                    throw new Exception("Exception thrown SelectSystemMessages", ex);
                }
                finally
                {
                    myConnection.Close();
                }
            }
            return systemMessages.ToArray();
        }
        

		/// <summary>
		/// to retrieve system message metadata for systemMessages specified by messageType and group and labServerID
		/// </summary>
        public static SystemMessage[] SelectSystemMessages(string messageType, int groupID, int clientID, int agentID)
		{
            List<SystemMessage> systemMessages = new List<SystemMessage>();

            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("SystemMessages_Retrieve", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure ;
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@messageType", messageType, DbType.AnsiString,100));
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@clientID", clientID, DbType.Int32));
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@agentID", agentID, DbType.Int32));
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@groupID", groupID, DbType.Int32));
             
            //DbParameter client = FactoryDB.CreateParameter( "@clientID", DbType.Int32);
            //if (clientID > 0)
            //    myCommand.Parameters.Add(FactoryDB.CreateParameter( "@clientID", clientID, DbType.Int32));
            //else
            //    myCommand.Parameters.Add(FactoryDB.CreateParameter( "@clientID", null, DbType.Int32));
            //if (agentID > 0)
            //    myCommand.Parameters.Add(FactoryDB.CreateParameter( "@agentID", agentID, DbType.Int32));
            //else
            //    myCommand.Parameters.Add(FactoryDB.CreateParameter( "@agentID", null, DbType.Int32));
            //if (groupID > 0)
            //    myCommand.Parameters.Add(FactoryDB.CreateParameter( "@groupID", groupID, DbType.Int32));
            //else
            //    myCommand.Parameters.Add(FactoryDB.CreateParameter( "@groupID", null, DbType.Int32));
            
			try 
			{
				myConnection.Open ();
				

				// get systemMessage info from table system_messages
				DbDataReader myReader = myCommand.ExecuteReader ();
				while(myReader.Read ())
				{	
					SystemMessage sm = new SystemMessage();
					sm.messageType = messageType;
					sm.groupID = groupID;
					sm.clientID = clientID;
                    sm.agentID = agentID;

					if(myReader["system_message_id"] != System.DBNull.Value )
						sm.messageID = Convert.ToInt32(myReader["system_message_id"]);

					if(myReader["message_body"] != System.DBNull.Value )
						sm.messageBody = (string) myReader["message_body"];
					byte tbd = 0;
					if(myReader["to_be_displayed"] != System.DBNull.Value ) 
					{
						tbd = Convert.ToByte( myReader["to_be_displayed"]);
					}
					if (tbd ==1)
						sm.toBeDisplayed = true;
					else 
						sm.toBeDisplayed = false;
					if(myReader["last_modified"] != System.DBNull.Value )
						sm.lastModified = DateUtil.SpecifyUTC(Convert.ToDateTime(myReader["last_modified"]));
					if(myReader["message_title"] != System.DBNull.Value )
						sm.messageTitle = (string) myReader["message_title"];

                    systemMessages.Add(sm);
						
				}
				myReader.Close ();
			}
			catch (Exception ex)
			{
				throw new Exception("Exception thrown SelectSystemMessages",ex);
			}
			finally 
			{
				myConnection.Close ();
			}
			return systemMessages.ToArray();
		}

		
		/// <summary>
		/// to retrieve system message metadata for systemMessages specified by array of systemMessage IDs 
		/// This was a method added later by Shaomin - it was required to display all the system messages for the admin pages
		/// 
		/// Its name was changed from SelectSystemMessagesSuperUser to SelectAdminSystemMessages by Charu on 5/22/04 during the converstion
		/// to the new database
		/// </summary>
		public static SystemMessage[] SelectAdminSystemMessages ( int[] systemMessageIDs )
		{
			List<SystemMessage> messages = new List<SystemMessage>();
			
            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("SystemMessage_RetrieveByIDForAdmin", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;
			myCommand.Parameters .Add(FactoryDB.CreateParameter("@messageID",null, DbType.Int32));

			try 
			{
				myConnection.Open ();
				
				for (int i =0; i < systemMessageIDs.Length ; i++) 
				{
					myCommand.Parameters["@messageID"].Value = systemMessageIDs[i];

					// get systemMessage info from table system_messages
					DbDataReader myReader = myCommand.ExecuteReader ();
					while(myReader.Read ())
                    {
                        SystemMessage sm = new SystemMessage();
						sm.messageID = systemMessageIDs[i];

						if(myReader["message_body"] != System.DBNull.Value )
							sm.messageBody = (string) myReader["message_body"];
						byte tbd = 0;
						if(myReader["to_be_displayed"] != System.DBNull.Value ) 
						{
							sm.toBeDisplayed = Convert.ToBoolean( myReader["to_be_displayed"]);
						}
						if(myReader["description"] != System.DBNull.Value )
							sm.messageType = (string) myReader["description"];
						if(myReader["last_modified"] != System.DBNull.Value )
							sm.lastModified = DateUtil.SpecifyUTC(Convert.ToDateTime(myReader["last_modified"]));
                        if (myReader["client_id"] != System.DBNull.Value)
                            sm.clientID = Convert.ToInt32(myReader["client_id"]);
                        if (myReader["group_id"] != System.DBNull.Value)
						sm.groupID = Convert.ToInt32(myReader["group_id"]);
                    if (myReader["agent_id"] != System.DBNull.Value)
						sm.agentID = Convert.ToInt32(myReader["agent_id"]);
						if(myReader["message_title"] != System.DBNull.Value )
							sm.messageTitle= (string) myReader["message_title"];
                        messages.Add(sm);
					}
					myReader.Close ();
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Exception thrown SelectSystemMessages",ex);
			}
			finally 
			{
				myConnection.Close ();
			}
			
			return messages.ToArray();
		}

		/// <summary>
		/// to retrieve system message metadata for systemMessages specified by messageType and group for a superUser (gets all system messages)
		/// This was a method added later by Shaomin - it was required to display all the system messages for the admin pages
		/// 
		/// Its name was changed from SelectSystemMessagesSuperUser to SelectAdminSystemMessages by Charu on 5/22/04, during the conversion
		/// to the new database
		/// </summary>
        public static SystemMessage[] SelectAdminSystemMessages(string messageType, int groupID, int clientID, int agentID)
		{
			ArrayList arrayList = new ArrayList();

            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("SystemMessages_RetrieveForAdmin", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure ;

            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@messageType", messageType, DbType.AnsiString,100));
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@clientID", clientID, DbType.Int32));
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@groupID", groupID, DbType.Int32));
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@agentID", agentID, DbType.Int32));

			try 
			{
				myConnection.Open ();
				

				// get systemMessage info from table system_messages
				DbDataReader myReader = myCommand.ExecuteReader ();
				while(myReader.Read ())
				{	
					SystemMessage sm = new SystemMessage();
					sm.messageType = messageType;
                    sm.clientID = clientID;
					sm.groupID = groupID;
					sm.agentID = agentID;

					if(myReader["system_message_id"] != System.DBNull.Value )
						sm.messageID = Convert.ToInt32(myReader["system_message_id"]);

					if(myReader["message_body"] != System.DBNull.Value )
						sm.messageBody = (string) myReader["message_body"];
					byte tbd = 0;
					if(myReader["to_be_displayed"] != System.DBNull.Value ) 
					{
						tbd = Convert.ToByte( myReader["to_be_displayed"]);
					}
					if (tbd ==1)
						sm.toBeDisplayed = true;
					else 
						sm.toBeDisplayed = false;
					if(myReader["last_modified"] != System.DBNull.Value )
						sm.lastModified = DateUtil.SpecifyUTC(Convert.ToDateTime(myReader["last_modified"]));
					if(myReader["message_title"] != System.DBNull.Value )
						sm.messageTitle= (string) myReader["message_title"];

					arrayList.Add(sm);
						
				}
				myReader.Close ();
			}
			catch (Exception ex)
			{
				throw new Exception("Exception thrown SelectSystemMessages",ex);
			}
			finally 
			{
				myConnection.Close ();
			}

			// Converting to a SystemMessage array
			SystemMessage[] systemMessages = new SystemMessage[arrayList.Count];
			for (int i=0;i <arrayList.Count ; i++) 
			{
				systemMessages[i] = (SystemMessage) arrayList[i];
			}
			
			return systemMessages;
		}

	}
}

