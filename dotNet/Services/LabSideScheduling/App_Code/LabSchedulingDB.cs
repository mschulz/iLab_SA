/*
 * Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
 * Please see license.txt in top level directory for full license.
 * 
 * $Id$
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text;
using System.Web.Mail;

using iLabs.Core;
using iLabs.DataTypes;
using iLabs.DataTypes.ProcessAgentTypes;
using iLabs.DataTypes.SchedulingTypes;
using iLabs.DataTypes.SoapHeaderTypes;
using iLabs.DataTypes.TicketingTypes;
using iLabs.UtilLib;

using iLabs.Proxies.USS;

namespace iLabs.Scheduling.LabSide
{
	public class LabSchedulingDB : ProcessAgentDB
	{

		public LabSchedulingDB()
		{
        }

        public override int ModifyProcessAgent(string originalGuid, ProcessAgent agent, string extra)
        {
            int status = 0;
            status = base.ModifyProcessAgent(originalGuid, agent, extra);
            if (agent.type == ProcessAgentType.LAB_SERVER)
            {
                // Labserver Names in Experiment info's
                // Labserver Names in LS_Resource
                if (ModifyExperimentLabServer(originalGuid, agent.agentGuid, agent.agentName))
                    status++;
            }
            if (agent.type == ProcessAgentType.SCHEDULING_SERVER)
            {
                // USS path, & name in USS_Info

                int ussId = ListUSSInfoID(agent.agentGuid);
                if (ussId > 0)
                {
                    USSInfo[] uss = GetUSSInfos(new int[] { ussId });
                    if (uss != null && uss.Length > 0)

                        status += ModifyUSSInfo(ussId, agent.agentGuid, agent.agentName, agent.webServiceUrl,
                             uss[0].revokeCouponId, uss[0].domainGuid);
                }
            }
            if (agent.type == ProcessAgentType.SERVICE_BROKER || agent.type == ProcessAgentType.REMOTE_SERVICE_BROKER)
            {
                status += ModifyCredentialSetServiceBroker(originalGuid, agent.agentGuid, agent.agentName);
            }
            return status;
        }

        #region LSSPolicy Methods
        /* !------------------------------------------------------------------------------!
		 *							CALLS FOR LSSPolicy
		 * !------------------------------------------------------------------------------!
		 */
		/// <summary>
		/// add lab side scheduling policy to determine whether a reservation from a particular group for a particular experiment shoulc be accepted or not
		/// </summary>
		/// <param name="credentialSetID"></param>
		/// <param name="experimentInfoID"></param>
		/// <param name="rule"></param>
		/// <returns></returns>the unique ID which identifies the lab scheduling policy added,>0 was successfully added,==-1 otherwise
		public int AddLSSPolicy(int credentialSetID, int experimentInfoID,string rule)
		{
			//create a connection
            DbConnection connection = FactoryDB.GetConnection();
			//create a command
			//command executes the "AddLSSPolicy" store procedure
            DbCommand cmd = FactoryDB.CreateCommand("LSSPolicy_Add", connection);
			cmd.CommandType=CommandType.StoredProcedure;
			//populate the parameters
			cmd.Parameters.Add(FactoryDB.CreateParameter("@credentialSetID", credentialSetID,DbType.Int32));
			cmd.Parameters.Add(FactoryDB.CreateParameter("@experimentInfoID", experimentInfoID, DbType.Int32));
			cmd.Parameters.Add(FactoryDB.CreateParameter("@rule",rule,  DbType.AnsiString, 2048));
			
			int i=-1;

			// execute the command
			try
			{
				connection.Open();
				Object ob=cmd.ExecuteScalar();
				if(ob != null && ob!=System.DBNull.Value)
				{
					i= Int32.Parse(ob.ToString());
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Exception thrown in add LSSPolicy",ex);
			}
			finally
			{
				connection.Close();
			}	
			return i;
			
		}
		/// <summary>
		/// delete the lab side scheduling policies specified by the lssPolicyIDs
		/// </summary>
		/// <param name="lssPolicyIDs"></param>
		/// <returns></returns>the IDs of all LssPolicies not successfully removed
		
		public int[] RemoveLSSPolicy(int[] lssPolicyIDs)
		{
			ArrayList arrayList=new ArrayList();
			//create a connection
            DbConnection connection = FactoryDB.GetConnection();
			//create a command
			//command executes the "deleteLSSPolicy" store procedure
            DbCommand cmd = FactoryDB.CreateCommand("LSSPolicy_Delete", connection);
			cmd.CommandType=CommandType.StoredProcedure;
            cmd.Parameters.Add(FactoryDB.CreateParameter("@lssPolicyID", null, DbType.Int32));
			
			// execute the command
			try
			{
				connection.Open();
				//populate the parameters
				foreach(int lssPolicyID in lssPolicyIDs)
				{
					cmd.Parameters["@lssPolicyID"].Value = lssPolicyID;
					if (cmd.ExecuteNonQuery()==0)
					{
						arrayList.Add(lssPolicyID);
					}
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Exception thrown in remove LSS policies",ex);
			}
			finally
			{
				connection.Close();
			}	
			int[] uIDs = Utilities.ArrayListToIntArray(arrayList);
			return uIDs;			
		}
		/// <summary>
		/// update the data fields for the lab side scheduling policy specified by the lssPolicyID
		/// </summary>
		/// <param name="lssPolicyID"></param>
		/// <param name="credentialSetID"></param>
		/// <param name="experimentInfoID"></param>
		/// <param name="rule"></param>
		/// <returns></returns>if updated succesfully, return ture, otherwise, return false

		public bool ModifyLSSPolicy(int lssPolicyID,int credentialSetID,int experimentInfoID,string rule)
		{
			//create a connection
            DbConnection connection = FactoryDB.GetConnection();
			//create a command
			//command executes the "modifyLSSPolicy" store procedure
            DbCommand cmd = FactoryDB.CreateCommand("LSSPolicy_Modify", connection);
			cmd.CommandType=CommandType.StoredProcedure;
			//populate the parameters
            cmd.Parameters.Add(FactoryDB.CreateParameter( "@lssPolicyID", lssPolicyID, DbType.Int32));
			cmd.Parameters.Add(FactoryDB.CreateParameter("@credentialSetID", credentialSetID, DbType.Int32));
			cmd.Parameters.Add(FactoryDB.CreateParameter("@experimentInfoID", experimentInfoID, DbType.Int32));
			cmd.Parameters.Add(FactoryDB.CreateParameter("@rule", rule, DbType.AnsiString, 2048));
			
			bool i=false;

			// execute the command
			try
			{
				int m=0;
				connection.Open();
				Object ob=cmd.ExecuteScalar();
				if(ob != null && ob!=System.DBNull.Value)
				{
					m = Int32.Parse(ob.ToString());
				}
				if (m!=0)
				{
					i=true;
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Exception thrown in add ModifyLSSPolicy",ex);
			}
			finally
			{
				connection.Close();
			}		
			return i;
             
		}
		/// <summary>
		/// enumerates all the IDs of the lab side scheduling policies for  a particular experiment identified by the experimentInfoIDs
		/// </summary>
		/// <param name="experimentInfoID"></param>
		/// <returns></returns>an array of ints containing the IDs of all the lab side scheduling policies of specified experiment
		public int[] ListLSSPolicyIDsByExperiment(int experimentInfoID)
		{
			int[] lssPolicyIDs;
			// create sql connection
            DbConnection connection = FactoryDB.GetConnection();
			// create sql command
			// command executes the "RetrieveLSSPolicyIDsByExperiment" stored procedure
            DbCommand cmd = FactoryDB.CreateCommand("LSSPolicy_RetrieveIDsByExperiment", connection);
			cmd.CommandType = CommandType.StoredProcedure;

			// populate the parameters
			cmd.Parameters.Add(FactoryDB.CreateParameter("@experimentInfoID", experimentInfoID,DbType.Int32));

			// execute the command
			DbDataReader dataReader = null;
			ArrayList arrayList=new ArrayList();
			try 
			{
				connection.Open();
				dataReader = cmd.ExecuteReader ();
				//store the lssPolicyIDs retrieved in arraylist
				
				while(dataReader.Read ())
				{	
					if(dataReader["LSS_Policy_ID"] != System.DBNull.Value )
						arrayList.Add(Convert.ToInt32(dataReader["LSS_Policy_ID"]));
				}
				dataReader.Close();
			} 

			catch (Exception ex)
			{
				throw new Exception("Exception thrown in retrieve lSSPolicyIDs by group",ex);
			}
			finally
			{
				connection.Close();
			}
			lssPolicyIDs=Utilities.ArrayListToIntArray(arrayList);
			return lssPolicyIDs;
		}
		/// <summary>
		/// returns an array of the immutable LSSPolicy objects that correspond to the supplied lssPolicyIDs
		/// </summary>
		/// <param name="lssPolicyIDs"></param>
		/// <returns></returns>
		public LSSPolicy[] GetLSSPolicies(int[] lssPolicyIDs)
		{
			LSSPolicy[] lssPolicies=new LSSPolicy[lssPolicyIDs.Length];
			for(int i=0; i<lssPolicyIDs.Length;i++)
			{
				lssPolicies[i]=new LSSPolicy();
			}
			// create sql connection
            DbConnection connection = FactoryDB.GetConnection();
			// create sql command
			// command executes the "RetrieveLSSPolicyByID" stored procedure
            DbCommand cmd = FactoryDB.CreateCommand("LSSPolicy_RetrieveByID", connection);
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add(FactoryDB.CreateParameter("@lssPolicyID", null, DbType.Int32));
			//execute the command
			try 
			{
				connection.Open();
				for(int i=0;i<lssPolicyIDs.Length;i++)
				{
					// populate the parameters
					cmd.Parameters["@lssPolicyID"].Value = lssPolicyIDs[i];	
					DbDataReader dataReader = null;
					dataReader=cmd.ExecuteReader();
					while(dataReader.Read())
					{	
						lssPolicies[i].lssPolicyId=lssPolicyIDs[i];
						if(dataReader[0] != System.DBNull.Value )
							lssPolicies[i].experimentInfoId=(int)dataReader.GetInt32(0);
						if(dataReader[1] != System.DBNull.Value )
							lssPolicies[i].rule=dataReader.GetString(1);
						if(dataReader[2] != System.DBNull.Value )
							lssPolicies[i].credentialSetId=(int)dataReader.GetInt32(2);
					}
					dataReader.Close();
				}	
			} 

			catch (Exception ex)
			{
				throw new Exception("Exception thrown in get lsspolicies",ex);
			}
			finally
			{
				connection.Close();
			}	
			return lssPolicies;
        }
        #endregion

        #region TimeBlock Methods
        /* !------------------------------------------------------------------------------!
			 *							CALLS FOR Time Block
			 * !------------------------------------------------------------------------------!
			 */
        /*
		/// <summary>
		/// add a time block in which users with a particular credential set are allowed to access a particular lab server
		/// </summary>
		/// <param name="labServerGuid"></param>
		/// <param name="resourceID"></param>
		/// <param name="startTime"></param>
		/// <param name="endTime"></param>
        /// <param name="recurrenceID"></param>
        /// <returns>the uniqueID which identifies the time block added, >0 was successfully added; ==-1 otherwise</returns>
		public int AddTimeBlock(string labServerGuid,int resourceID, DateTime startTime, DateTime endTime, int recurrrenceID)
		{
			//create a connection
            DbConnection connection = FactoryDB.GetConnection();
			//create a command
			//command executes the "AddTimeBlock" store procedure
			DbCommand cmd=FactoryDB.CreateCommand("AddTimeBlock",connection);
			cmd.CommandType=CommandType.StoredProcedure;
			//populate the parameters
			DbParameter labServerIDParam = cmd.Parameters.Add(FactoryDB.CreateParameter("@labServerGUID", DbType.AnsiString,50);
			labServerIDParam.Value = labServerGuid;
			DbParameter credentialSetIDParam = cmd.Parameters.Add(FactoryDB.CreateParameter("@resourceID", DbType.Int);
			credentialSetIDParam.Value = resourceID;
			DbParameter startTimeParam = cmd.Parameters.Add(FactoryDB.CreateParameter("@startTime", DbType.DateTime);
			startTimeParam.Value = startTime;
			DbParameter endTimeParam = cmd.Parameters.Add(FactoryDB.CreateParameter("@endTime", DbType.DateTime);
			endTimeParam.Value = endTime;
            DbParameter recurrenceIDParam = cmd.Parameters.Add(FactoryDB.CreateParameter("@recurrenceID", DbType.Int);
            recurrenceIDParam.Value = recurrrenceID;
			int i=-1;

			// execute the command
			try
			{
				connection.Open();
				Object ob=cmd.ExecuteScalar();
				if(ob != null && ob!=System.DBNull.Value)
				{
					i= Int32.Parse(ob.ToString());
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Exception thrown in add time block",ex);
			}
			finally
			{
				connection.Close();
			}	
			return i;		
		}
		
		/// <summary>
		/// delete the time blocks specified by the timeBlockIDs
		/// </summary>
		/// <param name="timeBlockIDs"></param>
		/// <returns></returns>an array of ints containning the IDs of all time blocks not successfully removed
		public int[] RemoveTimeBlock(int[] timeBlockIDs)
		{
			ArrayList arrayList=new ArrayList();
			//create a connection
            DbConnection connection = FactoryDB.GetConnection();
			//create a command
			//command executes the "DeleteTimeBlock" store procedure
			DbCommand cmd=FactoryDB.CreateCommand("DeleteTimeBlock",connection);
			cmd.CommandType=CommandType.StoredProcedure;
			cmd.Parameters.Add(FactoryDB.CreateParameter(FactoryDB.CreateParameter("@timeBlockID", DbType.Int));
			
			// execute the command
			try
			{
				connection.Open();
				//populate the parameters
				foreach(int timeBlockID in timeBlockIDs)
				{
					cmd.Parameters["@timeBlockID"].Value = timeBlockID;
					if (cmd.ExecuteNonQuery()==0)
					{
						arrayList.Add(timeBlockID);
					}
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Exception thrown in remove time block",ex);
			}
			finally
			{
				connection.Close();
			}	
			int[] uIDs = Utilities.ArrayListToIntArray(arrayList);
			return uIDs;			
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="timeBlockID"></param>
		/// <param name="labServerID"></param>
		/// <param name="resourceID"></param>
		/// <param name="startTime"></param>
		/// <param name="endTime"></param>
		/// <returns></returns> true if updated sucessfully, false otherwise
		public bool ModifyTimeBlock(int timeBlockID, string labServerGuid,int resourceID, DateTime startTime, DateTime endTime)
		{
			//create a connection
            DbConnection connection = FactoryDB.GetConnection();
			//create a command
			//command executes the "ModifyTimeBlock" store procedure
			DbCommand cmd=FactoryDB.CreateCommand("ModifyTimeBlock",connection);
			cmd.CommandType=CommandType.StoredProcedure;
			//populate the parameters
			DbParameter timeBlockIDParam = cmd.Parameters.Add(FactoryDB.CreateParameter("@timeBlockID", DbType.Int);
			timeBlockIDParam.Value = timeBlockID;
			DbParameter labServerIDParam = cmd.Parameters.Add(FactoryDB.CreateParameter("@labServerGUID", DbType.AnsiString,50);
			labServerIDParam.Value = labServerGuid;
			DbParameter credentialSetIDParam = cmd.Parameters.Add(FactoryDB.CreateParameter("@resourceID", DbType.Int);
			credentialSetIDParam.Value = resourceID;
			DbParameter startTimeParam = cmd.Parameters.Add(FactoryDB.CreateParameter("@startTime", DbType.DateTime);
			startTimeParam.Value = startTime;
			DbParameter endTimeParam = cmd.Parameters.Add(FactoryDB.CreateParameter("@endTime", DbType.DateTime);
			endTimeParam.Value = endTime;
			bool i=false;

			// execute the command
			try
			{
				connection.Open();
				int m=0 ;
				Object ob=cmd.ExecuteScalar();
				if(ob != null && ob!=System.DBNull.Value)
				{
					m= Int32.Parse(ob.ToString());
				}
				if (m!=0)
				{
					i=true;
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Exception thrown in add ModifyTime",ex);
			}
			finally
			{
				connection.Close();
			}		
			return i;
             
		}
		
		/// <summary>
		/// enumerates all IDs of the time blocks belonging to a particular lab server identified by the labserverID
		/// </summary>
		/// <param name="labServerGuid"></param>
		/// <returns></returns>an array of ints containing the IDs of all the time blocks of specified lab server
		public int[] ListTimeBlockIDsByLabServer(string labServerGuid)
		{
			int[] timeBlockIDs;
			// create sql connection
            DbConnection connection = FactoryDB.GetConnection();

			// create sql command
			// command executes the "RetrieveTimeBlockIDsByLabServer" stored procedure
			DbCommand cmd = FactoryDB.CreateCommand("RetrieveTimeBlockIDsByLabServer", connection);
			cmd.CommandType = CommandType.StoredProcedure;

			// populate the parameters
			DbParameter labServerIDParam = cmd.Parameters.Add(FactoryDB.CreateParameter("@labServerGUID", DbType.AnsiString,50);
			labServerIDParam.Value = labServerGuid;

			// execute the command
			DbDataReader dataReader = null;
			ArrayList arrayList=new ArrayList();
			try 
			{
				connection.Open();
				dataReader = cmd.ExecuteReader ();
				//store the timeBlockIDs retrieved in arraylist
				
				while(dataReader.Read ())
				{	
					if(dataReader["Time_Block_ID"] != System.DBNull.Value )
						arrayList.Add(Convert.ToInt32(dataReader["Time_Block_ID"]));
				}
				dataReader.Close();
			} 

			catch (Exception ex)
			{
				throw new Exception("Exception thrown in retrieve timeBlockIDs by lab server",ex);
			}
			finally
			{
				connection.Close();
			}
			timeBlockIDs=Utilities.ArrayListToIntArray(arrayList);
			return timeBlockIDs;
		}
        
		/// <summary>
		/// enumerates all IDs of the time blocks during which the members of a particular group identified by the credentialSetID are allowed to access a particular lab server identified by the labServerID
		/// </summary>
		/// <param name="labServerGuid"></param>
		/// <param name="credentialSetID"></param>
		/// <returns></returns>an array of ints containing the IDs of all the time blocks during which the members of a particular grou[ are allowed to access a particular lab server
		public int[] ListTimeBlockIDsByGroup(string labServerGuid,int credentialSetID)
		{
			int[] timeBlockIDs;
			// create sql connection
            DbConnection connection = FactoryDB.GetConnection();

			// create sql command
			// command executes the "RetrieveTimeBlockIDsByGroup" stored procedure
			DbCommand cmd = FactoryDB.CreateCommand("RetrieveTimeBlockIDsByGroup", connection);
			cmd.CommandType = CommandType.StoredProcedure;

			// populate the parameters
			DbParameter labServerIDParam = cmd.Parameters.Add(FactoryDB.CreateParameter("@labServerGUID", DbType.AnsiString,50);
			labServerIDParam.Value = labServerGuid;
			DbParameter credentialSetIDParam = cmd.Parameters.Add(FactoryDB.CreateParameter("@credentialSetID", DbType.Int);
			credentialSetIDParam.Value = credentialSetID;

			// execute the command
			DbDataReader dataReader = null;
			ArrayList arrayList=new ArrayList();
			try 
			{
				connection.Open();
				dataReader = cmd.ExecuteReader ();
				//store the timeBlockIDs retrieved in arraylist
				
				while(dataReader.Read ())
				{	
					if(dataReader["Time_Block_ID"] != System.DBNull.Value )
						arrayList.Add(Convert.ToInt32(dataReader["Time_Block_ID"]));
				}
				dataReader.Close();
			} 

			catch (Exception ex)
			{
				throw new Exception("Exception thrown in retrieve timeBlockIDs by group",ex);
			}
			finally
			{
				connection.Close();
			}
			timeBlockIDs=Utilities.ArrayListToIntArray(arrayList);
			return timeBlockIDs;
		}
		/// <summary>
		/// list the IDs of a particular lab server's time blocks which are assigned to particular group in the time chunk defined by the start time and end time
		/// </summary>
		/// <param name="serviceBrokerGuid"></param>
		/// <param name="groupName"></param>
        /// <param name="ussGuid"></param>
        /// <param name="labServerGuid"></param>
		/// <param name="startTime"></param>
		/// <param name="endTime"></param>
		/// <returns></returns>
        public int[] ListTimeBlockIDsByTimeChunk(string serviceBrokerGuid, string groupName, string ussGuid, 
            string clientGuid, string labServerGuid, DateTime startTime, DateTime endTime)
		{
			int[] timeBlockIDs;
			// create sql connection
            DbConnection connection = FactoryDB.GetConnection();

			// create sql command
			// command executes the "RetrieveTimeBlockIDs" stored procedure
			DbCommand cmd = FactoryDB.CreateCommand("RetrieveTimeBlockIDsByTimeChunk", connection);
			cmd.CommandType = CommandType.StoredProcedure;

			// populate the parameters
			DbParameter serviceBrokerIDParam = cmd.Parameters.Add(FactoryDB.CreateParameter("@serviceBrokerGUID", DbType.AnsiString,50);
			serviceBrokerIDParam.Value = serviceBrokerGuid;
			DbParameter groupNameParam = cmd.Parameters.Add(FactoryDB.CreateParameter("@groupName", DbType.AnsiString,128);
			groupNameParam.Value = groupName;
			DbParameter ussIDParam = cmd.Parameters.Add(FactoryDB.CreateParameter("@ussGUID", DbType.AnsiString,50);
            ussIDParam.Value = ussGuid;
            DbParameter clientParam = cmd.Parameters.Add(FactoryDB.CreateParameter("@labClientGuid", DbType.AnsiString, 50);
            clientParam.Value = clientGuid;
			DbParameter labServerIDParam = cmd.Parameters.Add(FactoryDB.CreateParameter("@labServerGuid", DbType.AnsiString,50);
            labServerIDParam.Value = labServerGuid;
			DbParameter startTimeParam = cmd.Parameters.Add(FactoryDB.CreateParameter("@startTime", DbType.DateTime);
			startTimeParam.Value = startTime;
			DbParameter endTimeParam = cmd.Parameters.Add(FactoryDB.CreateParameter("@endTime", DbType.DateTime);
			endTimeParam.Value = endTime;

		

			// execute the command
			DbDataReader dataReader = null;
			ArrayList arrayList=new ArrayList();
			try 
			{
				connection.Open();
				dataReader = cmd.ExecuteReader ();
				//store the timeBlockIDs retrieved in arraylist
				
				while(dataReader.Read ())
				{	
					if(dataReader["Time_Block_ID"] != System.DBNull.Value )
						arrayList.Add(Convert.ToInt32(dataReader["Time_Block_ID"]));
				}
				dataReader.Close();
			} 

			catch (Exception ex)
			{
				throw new Exception("Exception thrown in retrieve timeBlockIDs ",ex);
			}
			finally
			{
				connection.Close();
			}
			timeBlockIDs=Utilities.ArrayListToIntArray(arrayList);
			return timeBlockIDs;
		}

		/// Enumerates the IDs of the information of all the time blocks 
		/// </summary>
		/// <returns></returns>the array  of ints contains the IDs of the information of all the time blocks
		public int[] ListTimeBlockIDs()
		{
			int[] timeBlockIDs;
			// create sql connection
            DbConnection connection = FactoryDB.GetConnection();

			// create sql command
			// command executes the "RetrieveTimeBlockIDs" stored procedure
			DbCommand cmd = FactoryDB.CreateCommand("RetrieveTimeBlockIDs", connection);
			cmd.CommandType = CommandType.StoredProcedure;
			// execute the command
			DbDataReader dataReader = null;
			ArrayList arrayList=new ArrayList();
			try 
			{
				connection.Open();
				dataReader = cmd.ExecuteReader ();
				//store the timeBlockIDs retrieved in arraylist
				
				while(dataReader.Read ())
				{	
					if(dataReader["Time_Block_ID"] != System.DBNull.Value )
						arrayList.Add(Convert.ToInt32(dataReader["Time_Block_ID"]));
				}
				dataReader.Close();
			} 

			catch (Exception ex)
			{
				throw new Exception("Exception thrown in retrieve timeBlockIDs ",ex);
			}
			finally
			{
				connection.Close();
			}
			timeBlockIDs=Utilities.ArrayListToIntArray(arrayList);
			return timeBlockIDs;
		}
        
		/// <summary>
		/// Returns an array of the immutable TimeBlock objects that correspond ot the supplied time block IDs
		/// </summary>
		/// <param name="timeBlockIDs"></param>
		/// <returns></returns>an array of immutable objects describing the specified time blocks
		public TimeBlock[] GetTimeBlocks(int[] timeBlockIDs)
		{
			TimeBlock[] timeBlocks=new TimeBlock[timeBlockIDs.Length];
			for(int i=0; i<timeBlockIDs.Length;i++)
			{
				timeBlocks[i]=new TimeBlock();
			}
			// create sql connection
            DbConnection connection = FactoryDB.GetConnection();

			// create sql command
			// command executes the "RetrieveTimeBlockByID" stored procedure
			DbCommand cmd = FactoryDB.CreateCommand("RetrieveTimeBlockByID", connection);
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add(FactoryDB.CreateParameter("@timeBlockID", DbType.Int);
			//execute the command
			try 
			{
				connection.Open();
				for(int i=0; i<timeBlockIDs.Length;i++)
				{
					// populate the parameters
					cmd.Parameters["@timeBlockID"].Value = timeBlockIDs[i];	
					DbDataReader dataReader = null;
					dataReader=cmd.ExecuteReader();
					while(dataReader.Read())
					{	
						//timeBlocks[i].timeBlockId=timeBlockIDs[i];
                        if (dataReader[0] != System.DBNull.Value)
                        {
                            DateTime temp = dataReader.GetDateTime(0);
                            timeBlocks[i].startTime = DateUtil.SpecifyUTC(temp);
                        }
						if(dataReader[1] != System.DBNull.Value )
							timeBlocks[i].endTime=DateUtil.SpecifyUTC(dataReader.GetDateTime(1));
						//if(dataReader[2] != System.DBNull.Value )
						//	timeBlocks[i].labServerGuid=dataReader.GetString(2);
                        //if(dataReader[3] != System.DBNull.Value )
                        //    timeBlocks[i].credentialSetId=(int)dataReader.GetInt32(3);
                        //if (dataReader[4] != System.DBNull.Value)
                        //    timeBlocks[i].recurrenceID = (int)dataReader.GetInt32(4);
					}
					dataReader.Close();
				}	
			} 

			catch (Exception ex)
			{
				throw new Exception("Exception thrown in get time blocks",ex);
			}
			finally
			{
				connection.Close();
			}	
			return timeBlocks;
		}
         * */

#endregion

        #region ExperimentInfo Methods

        /* !------------------------------------------------------------------------------!
			 *							CALLS FOR Experiment Information
			 * !------------------------------------------------------------------------------!
			 */
		// <summary>
		/// add information of particular experiment
		/// </summary>
        /// <param name="labClientGuid"></param>
        /// <param name="labServerGuid"></param>
		/// <param name="labServerName"></param>
		/// <param name="labClientVersion"></param>
		/// <param name="labClientName"></param>
		/// <param name="providerName"></param>
		/// <param name="prepareTime"></param>
		/// <param name="recoverTime"></param>
		/// <param name="minimumTime"></param>
		/// <param name="earlyArriveTime"></param>
		/// <returns></returns>the unique ID which identifies the experiment information added, >0 was successfully added, ==-1 otherwise
        public int AddExperimentInfo(string labServerGuid, string labServerName, string labClientGuid, string labClientName, string labClientVersion,
            string providerName, string contactEmail, int prepareTime, int recoverTime, int minimumTime, int earlyArriveTime)
		{
			//create a connection
            DbConnection connection = FactoryDB.GetConnection();
			//create a command
			//command executes the "AddExperimentInfo" store procedure
            DbCommand cmd = FactoryDB.CreateCommand("ExperimentInfo_Add", connection);
			cmd.CommandType=CommandType.StoredProcedure;
			//populate the parameters
           
			cmd.Parameters.Add(FactoryDB.CreateParameter("@labServerGUID", labServerGuid,DbType.AnsiString,50));
			cmd.Parameters.Add(FactoryDB.CreateParameter("@labServerName", labServerName,DbType.String,256));
			cmd.Parameters.Add(FactoryDB.CreateParameter("@labClientGUID", labClientGuid, DbType.AnsiString, 50));
            cmd.Parameters.Add(FactoryDB.CreateParameter("@labClientName", labClientName,DbType.String,256));
			cmd.Parameters.Add(FactoryDB.CreateParameter("@labClientVersion", labClientVersion, DbType.String, 50));
            cmd.Parameters.Add(FactoryDB.CreateParameter("@providerName", providerName, DbType.String, 256));
            cmd.Parameters.Add(FactoryDB.CreateParameter( "@contactEmail", contactEmail, DbType.String, 256));
			cmd.Parameters.Add(FactoryDB.CreateParameter("@prepareTime", prepareTime, DbType.Int32));
			cmd.Parameters.Add(FactoryDB.CreateParameter("@minimumTime", minimumTime, DbType.Int32));
			cmd.Parameters.Add(FactoryDB.CreateParameter("@recoverTime", recoverTime, DbType.Int32));
			cmd.Parameters.Add(FactoryDB.CreateParameter("@earlyArriveTime", earlyArriveTime, DbType.Int32));
			
			int i=-1;

			// execute the command
			try
			{
				connection.Open();
				Object ob=cmd.ExecuteScalar();
				if(ob!=null && ob!=System.DBNull.Value)
				{
					i= Int32.Parse(ob.ToString());
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Exception thrown in add experiment Infomation",ex);
			}
			finally
			{
				connection.Close();
			}	
			return i;		
		}
		
		/// <summary>
		/// delete the experiment information specified by the experimentInfoIDs
		/// </summary>
		/// <param name="experimentInfoIDs"></param>
		/// <returns></returns>an array of ints containing the IDs of all experiment information not successfully removed
		public int[] RemoveExperimentInfo(int[] experimentInfoIDs)
		{
			ArrayList arrayList=new ArrayList();
			//create a connection
            DbConnection connection = FactoryDB.GetConnection();
			//create a command
			//command executes the "DeleteExperimentInfo" store procedure
            DbCommand cmd = FactoryDB.CreateCommand("ExperimentInfo_Delete", connection);
			cmd.CommandType=CommandType.StoredProcedure;
			cmd.Parameters.Add(FactoryDB.CreateParameter("@experimentInfoID", null, DbType.Int32));
			// execute the command
			try
			{
				connection.Open();
				//populate the parameters
				foreach(int experimentInfoID in experimentInfoIDs)
				{
					cmd.Parameters["@experimentInfoID"].Value = experimentInfoID;
					if (cmd.ExecuteNonQuery()==0)
					{
						arrayList.Add(experimentInfoID);
					}
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Exception thrown in remove experiment information",ex);
			}
			finally
			{
				connection.Close();
			}	
			int[] uIDs = Utilities.ArrayListToIntArray(arrayList);
			return uIDs;			
		}

        /// <summary>
        /// update the data fields for the experiment information specified by the experimentInfoID
        /// </summary>
        /// <param name="experimentInfoID"></param>
        /// <param name="labServerGuid"></param>
        /// <param name="labServerName"></param>
        /// <param name="labClientVersion"></param>
        /// <param name="labClientName"></param>
        /// <param name="providerName"></param>
        /// <param name="prepareTime"></param>
        /// <param name="recoverTime"></param>
        /// <param name="minimumTime"></param>
        /// <param name="earlyArriveTime"></param>
        /// <returns></returns>true if modified successfully, falso otherwise
        public bool ModifyExperimentInfo(int experimentInfoID, string labServerGuid, string labServerName,
            string labClientGuid, string labClientName, string labClientVersion, string providerName)
        {
            //create a connection
            DbConnection connection = FactoryDB.GetConnection();
            //create a command
            //command executes the "ModifyExperimentInfo" store procedure
            DbCommand cmd = FactoryDB.CreateCommand("ExperimentInfo_ModifyCore", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            //populate the parameters
            cmd.Parameters.Add(FactoryDB.CreateParameter( "@experimentInfoID", experimentInfoID, DbType.Int32));
            cmd.Parameters.Add(FactoryDB.CreateParameter( "@labClientGUID", labClientGuid, DbType.AnsiString, 50));
            cmd.Parameters.Add(FactoryDB.CreateParameter( "@labServerGUID", labServerGuid, DbType.AnsiString, 50));
            cmd.Parameters.Add(FactoryDB.CreateParameter( "@labServerName", labServerName, DbType.String, 256));
            cmd.Parameters.Add(FactoryDB.CreateParameter( "@labClientVersion", labClientVersion, DbType.String, 50));
            cmd.Parameters.Add(FactoryDB.CreateParameter( "@labClientName", labClientName, DbType.String, 256));
            cmd.Parameters.Add(FactoryDB.CreateParameter( "@providerName", providerName, DbType.String, 256));
            

            bool i = false;

            // execute the command
            try
            {
                connection.Open();
                int m = 0;
                Object ob = cmd.ExecuteScalar();
                if (ob != null && ob != System.DBNull.Value)
                {
                    m = Int32.Parse(ob.ToString());
                }
                if (m != 0)
                {
                    i = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception thrown in add Modify Experiment Infomation", ex);
            }
            finally
            {
                connection.Close();
            }
            return i;

        }


		/// <summary>
		/// update the data fields for the experiment information specified by the experimentInfoID
		/// </summary>
		/// <param name="experimentInfoID"></param>
        /// <param name="labServerGuid"></param>
		/// <param name="labServerName"></param>
		/// <param name="labClientVersion"></param>
		/// <param name="labClientName"></param>
		/// <param name="providerName"></param>
		/// <param name="prepareTime"></param>
		/// <param name="recoverTime"></param>
		/// <param name="minimumTime"></param>
		/// <param name="earlyArriveTime"></param>
		/// <returns></returns>true if modified successfully, falso otherwise
        public bool ModifyExperimentInfo(int experimentInfoID, string labServerGuid, string labServerName,
            string labClientGuid, string labClientName, string labClientVersion, string providerName,
            string contactEmail, int prepareTime, int recoverTime, int minimumTime, int earlyArriveTime)
		{
			//create a connection
            DbConnection connection = FactoryDB.GetConnection();
			//create a command
			//command executes the "ModifyExperimentInfo" store procedure
            DbCommand cmd = FactoryDB.CreateCommand("ExperimentInfo_Modify", connection);
			cmd.CommandType=CommandType.StoredProcedure;
			//populate the parameters
			cmd.Parameters.Add(FactoryDB.CreateParameter("@experimentInfoID", experimentInfoID,DbType.Int32));
			cmd.Parameters.Add(FactoryDB.CreateParameter("@labClientGUID", labClientGuid, DbType.AnsiString, 50));
            cmd.Parameters.Add(FactoryDB.CreateParameter("@labServerGUID", labServerGuid, DbType.AnsiString,50));
            cmd.Parameters.Add(FactoryDB.CreateParameter("@labServerName", labServerName,DbType.String,256));
			cmd.Parameters.Add(FactoryDB.CreateParameter("@labClientVersion", labClientVersion, DbType.String,50));
			cmd.Parameters.Add(FactoryDB.CreateParameter("@labClientName", labClientName, DbType.String,256));
            cmd.Parameters.Add(FactoryDB.CreateParameter("@providerName", providerName, DbType.String, 256));
            cmd.Parameters.Add(FactoryDB.CreateParameter("@contactEmail", contactEmail, DbType.String, 256));
			cmd.Parameters.Add(FactoryDB.CreateParameter("@prepareTime", prepareTime, DbType.Int32));
			cmd.Parameters.Add(FactoryDB.CreateParameter("@recoverTime", recoverTime, DbType.Int32));
			cmd.Parameters.Add(FactoryDB.CreateParameter("@minimumTime",  minimumTime, DbType.Int32));
			cmd.Parameters.Add(FactoryDB.CreateParameter("@earlyArriveTime", earlyArriveTime, DbType.Int32));
			
			bool i=false;

			// execute the command
			try
			{
				connection.Open();
				int m=0;
				Object ob=cmd.ExecuteScalar();
				if(ob!=null && ob!=System.DBNull.Value)
				{
					m= Int32.Parse(ob.ToString());
				}
				if (m!=0)
				{
					i=true;
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Exception thrown in add Modify Experiment Infomation",ex);
			}
			finally
			{
				connection.Close();
			}		
			return i;
             
		}

        /// <summary>
        /// update the data fields for the experiment information specified by the experimentInfoID
        /// </summary>
        /// <returns></returns>true if modified successfully, falso otherwise
        public bool ModifyExperimentLabServer(string originalGuid, string labServerGuid, string labServerName)
        {
            //create a connection
            DbConnection connection = FactoryDB.GetConnection();
            //create a command
            //command executes the "ModifyExperimentInfo" store procedure
            DbCommand cmd = FactoryDB.CreateCommand("ExperimentInfo_ModifyLabServer", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            //populate the parameters
            cmd.Parameters.Add(FactoryDB.CreateParameter("@originalGUID", originalGuid, DbType.AnsiString, 50));
            cmd.Parameters.Add(FactoryDB.CreateParameter("@labServerGUID", labServerGuid, DbType.AnsiString, 50));
            cmd.Parameters.Add(FactoryDB.CreateParameter("@labServerName", labServerName, DbType.String, 256));
                       
            bool i = false;

            // execute the command
            try
            {
                connection.Open();
                int m = 0;
                Object ob = cmd.ExecuteScalar();
                if (ob != null && ob != System.DBNull.Value)
                {
                    m = Int32.Parse(ob.ToString());
                }
                if (m != 0)
                {
                    i = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception thrown in add Modify Experiment Infomation", ex);
            }
            finally
            {
                connection.Close();
            }
            return i;

        }
		
		/// <summary>
		/// enumerates IDs of the information of all the experiments belonging to certain lab server identified by the labserverID
		/// </summary>
		/// <param name="labServerID"></param>
		/// <returns></returns>an array of ints containing the IDs of the information of all the experiments belonging to specified lab server
		public int[] ListExperimentInfoIDsByLabServer(string labServerGuid)
		{
			int[] experimentInfoIDs;
			// create sql connection
            DbConnection connection = FactoryDB.GetConnection();

			// create sql command
			// command executes the "RetrieveExperimentInfoIDsByLabServer" stored procedure
            DbCommand cmd = FactoryDB.CreateCommand("ExperimentInfo_RetrieveIDsByLabServer", connection);
			cmd.CommandType = CommandType.StoredProcedure;

			// populate the parameters
            cmd.Parameters.Add(FactoryDB.CreateParameter( "@labServerGUID", labServerGuid, DbType.AnsiString, 50));
			
            // execute the command
			DbDataReader dataReader = null;
			ArrayList arrayList=new ArrayList();
			try 
			{
				connection.Open();
				dataReader = cmd.ExecuteReader();
				//store the experimetnInfoIDs retrieved in arraylist
				
				while(dataReader.Read ())
				{	
					if(dataReader["Experiment_Info_ID"] != System.DBNull.Value )
						arrayList.Add(Convert.ToInt32(dataReader["Experiment_Info_ID"]));
				}
				dataReader.Close();
			} 

			catch (Exception ex)
			{
				throw new Exception("Exception thrown in retrieve experimentInfoIDs by lab server",ex);
			}
			finally
			{
				connection.Close();
			}
			experimentInfoIDs=Utilities.ArrayListToIntArray(arrayList);
			return experimentInfoIDs;
		}

        /// <summary>
        /// get the labserver name according to the labserver ID
        /// </summary>
        /// <param name="labServerGuid"></param>
        /// <returns></returns>
        public string RetrieveLabServerName(string labServerGuid)
        {
            string labServerName = null;
            // create sql connection
            DbConnection connection = FactoryDB.GetConnection();

            // create sql command
            // command executes the "RetrieveExperimentInfoIDsByLabServer" stored procedure
            DbCommand cmd = FactoryDB.CreateCommand("LabServer_RetrieveName", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(FactoryDB.CreateParameter("@labServerGUID", labServerGuid, DbType.AnsiString, 50));
            
            // execute the command
            DbDataReader dataReader = null;
            ArrayList arrayList = new ArrayList();
            try
            {
                connection.Open();
                Object ob = cmd.ExecuteScalar();
                if (ob!=null && ob != System.DBNull.Value)
                {
                    labServerName = ob.ToString();
                }
                return labServerName;
            }

            catch (Exception ex)
            {
                throw new Exception("Exception thrown in retrieve experimentInfoIDs by lab server", ex);
            }
            finally
            {
                connection.Close();
            }
            
           
        }
		/// <summary>
		/// retrieve the ids of all the experiment information
		/// </summary>
		/// <returns></returns>an array of ints containing the IDs of the information of all the experiments 
		public int[] ListExperimentInfoIDs()
		{
			int[] experimentInfoIDs;
			// create sql connection
            DbConnection connection = FactoryDB.GetConnection();

			// create sql command
			// command executes the "RetrieveExperimentInfoIDs" stored procedure
            DbCommand cmd = FactoryDB.CreateCommand("ExperimentInfo_RetrieveIDs", connection);
			cmd.CommandType = CommandType.StoredProcedure;

			

			// execute the command
			DbDataReader dataReader = null;
			ArrayList arrayList=new ArrayList();
			try 
			{
				connection.Open();
				dataReader = cmd.ExecuteReader ();
				//store the experimetnInfoIDs retrieved in arraylist
				
				while(dataReader.Read ())
				{	
					if(dataReader["Experiment_Info_ID"] != System.DBNull.Value )
						arrayList.Add(Convert.ToInt32(dataReader["Experiment_Info_ID"]));
				}
				dataReader.Close();
			} 

			catch (Exception ex)
			{
				throw new Exception("Exception thrown in retrieve experimentInfoIDs ",ex);
			}
			finally
			{
				connection.Close();
			}
			experimentInfoIDs=Utilities.ArrayListToIntArray(arrayList);
			return experimentInfoIDs;
		}
		/// <summary>
		/// enumerates the ID of the information of a particular experiment specified by labClientName and labClientVersion
		/// </summary>
		/// <param name="labClientName"></param>
		/// <param name="labClientVersion"></param>
		/// <returns></returns>the ID of the information of a particular experiment, -1 if such a experiment info can not be retrieved
		public int ListExperimentInfoIDByExperiment(string labServerGuid, string clientGuid)
		{
			int experimentInfoID=-1;
			// create sql connection
            DbConnection connection = FactoryDB.GetConnection();

			// create sql command
			// command executes the "RetrieveExperimentInfoIDByExperiment" stored procedure
            DbCommand cmd = FactoryDB.CreateCommand("ExperimentInfo_RetrieveIDByExperiment", connection);
			cmd.CommandType = CommandType.StoredProcedure;

			// populate the parameters
            cmd.Parameters.Add(FactoryDB.CreateParameter( "@clientGuid", clientGuid, DbType.AnsiString, 50));
			cmd.Parameters.Add(FactoryDB.CreateParameter("@labServerGuid", labServerGuid, DbType.AnsiString,50));
			
			// execute the command
			
			try 
			{
				connection.Open();
				Object ob=cmd.ExecuteScalar();
                if (ob != null && ob != System.DBNull.Value)
				{
					experimentInfoID=Int32.Parse(ob.ToString());
				}
				
			} 

			catch (Exception ex)
			{
				throw new Exception("Exception thrown in retrieve experimentInfoIDs by experiment",ex);
			}
			finally
			{
				connection.Close();
			}
			return experimentInfoID;
		}
        
		/// <summary>
		/// Return an array of the immutable USSInfo objects thta correspond to the supplied USS information IDs
		/// </summary>
		/// <param name="experimentInfoIDs"></param>
		/// <returns></returns>
		
		public LssExperimentInfo[] GetExperimentInfos(int[] experimentInfoIDs)
		{
            List<LssExperimentInfo> experimentInfos = new List<LssExperimentInfo>();
			
			// create sql connection
            DbConnection connection = FactoryDB.GetConnection();

			// create sql command
			// command executes the "RetrieveExperimentInfoByID" stored procedure
            DbCommand cmd = FactoryDB.CreateCommand("ExperimentInfo_RetrieveByID", connection);
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters .Add(FactoryDB.CreateParameter("@experimentInfoID", null, DbType.Int32));
			//execute the command
			try 
			{
				connection.Open();
				for(int i=0;i<experimentInfoIDs.Length;i++)
				{
					// populate the parameters
					cmd.Parameters["@experimentInfoID"].Value = experimentInfoIDs[i];	
					DbDataReader dataReader = null;
					dataReader=cmd.ExecuteReader();
					while(dataReader.Read())
                    {
                        LssExperimentInfo experimentInfo = new LssExperimentInfo();
						experimentInfo.experimentInfoId=experimentInfoIDs[i];
						if(dataReader[0] != System.DBNull.Value )
							experimentInfo.labClientGuid=dataReader.GetString(0);
                        if (dataReader[1] != System.DBNull.Value)
                            experimentInfo.labServerGuid = dataReader.GetString(1);
						if(dataReader[2] != System.DBNull.Value )
							experimentInfo.labServerName=dataReader.GetString(2);
						if(dataReader[3] != System.DBNull.Value )
							experimentInfo.labClientVersion=dataReader.GetString(3);
						if(dataReader[4] != System.DBNull.Value )
							experimentInfo.labClientName=dataReader.GetString(4);
						if(dataReader[5] != System.DBNull.Value )
							experimentInfo.providerName=dataReader.GetString(5);
                        if (dataReader[6] != System.DBNull.Value)
                            experimentInfo.contactEmail= dataReader.GetString(6);
						if(dataReader[7] != System.DBNull.Value )
							experimentInfo.prepareTime=dataReader.GetInt32(7);
						if(dataReader[8] != System.DBNull.Value )
							experimentInfo.recoverTime=dataReader.GetInt32(8);
						if(dataReader[9] != System.DBNull.Value )
							experimentInfo.minimumTime=dataReader.GetInt32(9);
						if(dataReader[10] != System.DBNull.Value )
							experimentInfo.earlyArriveTime=dataReader.GetInt32(10);
                        experimentInfos.Add(experimentInfo);
					}
					dataReader.Close();
				}	
			} 

			catch (Exception ex)
			{
				throw new Exception("Exception thrown in get experiment inforamtion",ex);
			}
			finally
			{
				connection.Close();
			}
	        if(experimentInfos.Count >0)
			    return experimentInfos.ToArray();
            else 
                return null;
        }
        #endregion

        #region LabServerResource Methods

        /* !------------------------------------------------------------------------------!
			 *							CALLS FOR LabServer Resources
			 * !------------------------------------------------------------------------------!
			 */

        public int CheckForLSResource(string labServerGuid, string labServerName){
            //create a connection
            DbConnection connection = FactoryDB.GetConnection();
            //create a command
            DbCommand cmd = FactoryDB.CreateCommand("Resource_AddGetID", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            //populate the parameters
            cmd.Parameters.Add(FactoryDB.CreateParameter( "@guid", labServerGuid, DbType.AnsiString, 50));
            cmd.Parameters.Add(FactoryDB.CreateParameter("@name", labServerName, DbType.String,256));
            
            int i = -1;

            // execute the command
            try
            {
                connection.Open();
                Object ob = cmd.ExecuteScalar();
                if (ob != null && ob != System.DBNull.Value)
                {
                    i = Convert.ToInt32(ob);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception thrown in add permitted experiment", ex);
            }
            finally
            {
                connection.Close();
            }
            return i;
        }
   /*      */

        public LSResource[] GetLSResources()
        {
            List<LSResource> resources = null;
            //create a connection
            DbConnection connection = FactoryDB.GetConnection();
            //create a command
            DbCommand cmd = FactoryDB.CreateCommand("Resource_GetAll", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            // execute the command
            try
            {
                connection.Open();
                DbDataReader dataReader = null;
                dataReader = cmd.ExecuteReader();

                if (dataReader.HasRows)
                {
                    resources = new List<LSResource>();
                    while (dataReader.Read())
                    {
                        LSResource resource = new LSResource();
                        resource.resourceID = dataReader.GetInt32(0);
                        resource.labServerGuid = dataReader.GetString(1);
                        resource.labServerName = dataReader.GetString(2);
                        if (!dataReader.IsDBNull(3))
                            resource.description = dataReader.GetString(3);
                        resources.Add(resource);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception thrown in add permitted experiment", ex);
            }
            finally
            {
                connection.Close();
            }
            return resources.ToArray();
        }
        public LSResource GetLSResource(int id)
        {
            LSResource resource = null;
            //create a connection
            DbConnection connection = FactoryDB.GetConnection();
            //create a command
            DbCommand cmd = FactoryDB.CreateCommand("Resource_Get", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(FactoryDB.CreateParameter("@id", id,DbType.Int32));
            
            // execute the command
            try
            {
                connection.Open();
                DbDataReader dataReader = null;
                dataReader = cmd.ExecuteReader();

                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        resource = new LSResource();
                        resource.resourceID = dataReader.GetInt32(0);
                        resource.labServerGuid = dataReader.GetString(1);
                        resource.labServerName = dataReader.GetString(2);
                        if (!dataReader.IsDBNull(3))
                            resource.description = dataReader.GetString(3);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception thrown in add permitted experiment", ex);
            }
            finally
            {
                connection.Close();
            }
            return resource;
        }

        public LSResource[] GetLSResources(string guid)
        {
            List<LSResource> resources = new List<LSResource>();
            //create a connection
            DbConnection connection = FactoryDB.GetConnection();
            //create a command
            DbCommand cmd = FactoryDB.CreateCommand("Resource_GetByGuid", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(FactoryDB.CreateParameter( "@guid", guid, DbType.AnsiString, 50));
            
            // execute the command
            try
            {
                connection.Open();
                DbDataReader dataReader = null;
                dataReader = cmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        LSResource resource = new LSResource();
                        resource.resourceID = dataReader.GetInt32(0);
                        resource.labServerGuid = dataReader.GetString(1);
                        resource.labServerName = dataReader.GetString(2);
                        if (!dataReader.IsDBNull(3))
                            resource.description = dataReader.GetString(3);
                        resources.Add(resource);
                    }
                }
               
            }
            catch (Exception ex)
            {
                throw new Exception("Exception thrown in add permitted experiment", ex);
            }
            finally
            {
                connection.Close();
            }
            return resources.ToArray();
        }

        public IntTag[] GetLSResourceTags()
        {
            List<IntTag> tags = new List<IntTag>();
            //create a connection
            DbConnection connection = FactoryDB.GetConnection();
            //create a command
            DbCommand cmd = FactoryDB.CreateCommand("Resource_GetTags", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            
            // execute the command
            try
            {
                connection.Open();
               DbDataReader dataReader = null;
					dataReader=cmd.ExecuteReader();
                    while (dataReader.Read())
                    {
                        IntTag tag = new IntTag();
                        tag.id = dataReader.GetInt32(0);
                        tag.tag = dataReader.GetString(1);
                        tags.Add(tag);
                    }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception thrown in GetResourceTags", ex);
            }
            finally
            {
                connection.Close();
            }
            return tags.ToArray();
        }

        public IntTag[] GetLSResourceTags(string guid)
        {
            List<IntTag> tags = new List<IntTag>();
            //create a connection
            DbConnection connection = FactoryDB.GetConnection();
            //create a command
            DbCommand cmd = FactoryDB.CreateCommand("Resource_GetTagsByGuid", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            //populate the parameters
            cmd.Parameters.Add(FactoryDB.CreateParameter( "@guid", guid, DbType.AnsiString, 50));

            // execute the command
            try
            {
                connection.Open();
                DbDataReader dataReader = null;
                dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    IntTag tag = new IntTag();
                    tag.id = dataReader.GetInt32(0);
                    tag.tag = dataReader.GetString(1);
                    tags.Add(tag);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception thrown in add permitted experiment", ex);
            }
            finally
            {
                connection.Close();
            }
            return tags.ToArray();
        }

        public int InsertLSResource(string guid,string name,string description)
        {
            //create a connection
            DbConnection connection = FactoryDB.GetConnection();
            //create a command
            DbCommand cmd = FactoryDB.CreateCommand("Resource_Insert", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            //populate the parameters
            cmd.Parameters.Add(FactoryDB.CreateParameter("@guid", guid, DbType.AnsiString, 50));
            cmd.Parameters.Add(FactoryDB.CreateParameter("@name", name, DbType.String, 256));
            cmd.Parameters.Add(FactoryDB.CreateParameter("@description", description, DbType.String, 2048));
            
            // execute the command
            try
            {
                connection.Open();
                DbDataReader dataReader = null;
                return Convert.ToInt32(cmd.ExecuteScalar());
               
            }
            catch (Exception ex)
            {
                throw new Exception("Exception thrown in add InsertResource", ex);
            }
            finally
            {
                connection.Close();
            }
           
        }

        public int SetLSResourceDescription(int id, string description)
        {
            
            //create a connection
            DbConnection connection = FactoryDB.GetConnection();
            //create a command
            DbCommand cmd = FactoryDB.CreateCommand("Resource_SetDescription", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            //populate the parameters
            cmd.Parameters.Add(FactoryDB.CreateParameter("@id", id, DbType.Int32));
            cmd.Parameters.Add(FactoryDB.CreateParameter("@description", description, DbType.String, 2048));
            
            // execute the command
            try
            {
                connection.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw new Exception("Exception thrown in add InsertResource", ex);
            }
            finally
            {
                connection.Close();
            }
        }

        #endregion

        /* !------------------------------------------------------------------------------!
			 *							CALLS FOR Permited Experiment
			 * !------------------------------------------------------------------------------!
			 */
		/// <summary>
		/// add permission of a particular experiment being executed in a particular recurrence
		/// </summary>
		/// <param name="experimentInfoID"></param>
		/// <param name="recurrenceID"></param>
		/// <returns></returns>the unique ID which identifies the permission added. >0 was successfully added;==-1 otherwise
		public int AddPermittedExperiment(int experimentInfoID, int recurrenceID)
		{
			//create a connection
			DbConnection connection= FactoryDB.GetConnection();
			//create a command
			//command executes the "AddPermittedExperiment" store procedure
            DbCommand cmd = FactoryDB.CreateCommand("PermittedExperiment_Add", connection);
			cmd.CommandType=CommandType.StoredProcedure;
			//populate the parameters
			cmd.Parameters.Add(FactoryDB.CreateParameter("@experimentInfoID", experimentInfoID,DbType.Int32));
			cmd.Parameters.Add(FactoryDB.CreateParameter("@recurrenceID", recurrenceID, DbType.Int32));
			
			int i=-1;

			// execute the command
			try
			{
				connection.Open();
				Object ob=cmd.ExecuteScalar();
				if(ob!=null && ob!=System.DBNull.Value)
				{
					i= Int32.Parse(ob.ToString());
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Exception thrown in add permitted experiment",ex);
			}
			finally
			{
				connection.Close();
			}	
			return i;		
		}
		
		/// <summary>
		/// delete permission of  a particular experiment being executed in a particular time block
		/// </summary>
		/// <param name="permittedExperimentIDs"></param>
		/// <returns></returns>an array of ints containing the IDs of all permissions not successfully removed
		public int[] RemovePermittedExperiments(int[] permittedExperimentIDs,int recurrenceID)
		{
			ArrayList arrayList=new ArrayList();
			//create a connection
			DbConnection connection= FactoryDB.GetConnection();
			//create a command
			//command executes the "DeletePermittedExperiment" store procedure
            DbCommand cmd = FactoryDB.CreateCommand("PermittedExperiment_Delete", connection);
			cmd.CommandType=CommandType.StoredProcedure;
            cmd.Parameters.Add(FactoryDB.CreateParameter("@experimentID", null, DbType.Int32));
            cmd.Parameters.Add(FactoryDB.CreateParameter("@recurrenceID", recurrenceID, DbType.Int32));
           
			// execute the command
			try
			{
				connection.Open();
				//populate the parameters
				foreach(int permittedExperimentID in permittedExperimentIDs)
				{
                    cmd.Parameters["@experimentID"].Value = permittedExperimentID;
					if (cmd.ExecuteNonQuery()==0)
					{
						arrayList.Add(permittedExperimentID);
					}
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Exception thrown in remove permitted experiments",ex);
			}
			finally
			{
				connection.Close();
			}	
			int[] uIDs = Utilities.ArrayListToIntArray(arrayList);
			return uIDs;			
		}
	/*	
		/// <summary>
		/// enumerates the IDs of information of the permitted experiments for a particular time block identified by the timeBlockID
		/// </summary>
		/// <param name="timeBlockID"></param>
		/// <returns></returns>an array of ints containing the IDs of the information of the permitted experiments for a particular time block identified by the timeBlockID
		public int[] ListPermittedExperimentInfoIDsByTimeBlock(int timeBlockID)
		{
			
			// create sql connection
			DbConnection connection = FactoryDB.GetConnection();

			// create sql command
			// command executes the "RetrievePermittedExperimentInfoIDsByTimeBlock" stored procedure
            DbCommand cmd = FactoryDB.CreateCommand("PermittedExperimentInfo_RetrieveIDsByTimeBlock", connection);
			cmd.CommandType = CommandType.StoredProcedure;

			// populate the parameters
			DbParameter timeBlockIDParam = cmd.Parameters.Add(FactoryDB.CreateParameter("@timeBlockID", DbType.Int);
			timeBlockIDParam.Value = timeBlockID;

			// execute the command
			DbDataReader dataReader = null;
			ArrayList arrayList=new ArrayList();
			try 
			{
				connection.Open();
				dataReader = cmd.ExecuteReader ();
				//store the permitteExperimentIDs retrieved in arraylist
				
				while(dataReader.Read ())
				{	
					if(dataReader["Experiment_Info_ID"] != System.DBNull.Value )
						arrayList.Add(Convert.ToInt32(dataReader["Experiment_Info_ID"]));
				}
				dataReader.Close();
			} 

			catch (Exception ex)
			{
				throw new Exception("Exception thrown in retrieve permittedExperimentInfoIDs by timeBlock",ex);
			}
			finally
			{
				connection.Close();
			}
			int[] permittedExperimentInfoIDs=Utilities.ArrayListToIntArray(arrayList);
			return permittedExperimentInfoIDs;
		}
     * */


        /// <summary>
        /// enumerates the IDs of information of the permitted experiments for a particular recurrence identified by the recurrenceID
        /// </summary>
        /// <param name="recurrenceID"></param>
        /// <returns></returns>an array of ints containing the IDs of the information of the permitted experiments for a particular recurrence identified by the recurrenceID
        public int[] ListExperimentInfoIDsByRecurrence(int recurrenceID)
        {

            // create sql connection
            DbConnection connection = FactoryDB.GetConnection();

            // create sql command
            // command executes the "RetrievePermittedExperimentInfoIDsByRecurrence" stored procedure
            DbCommand cmd = FactoryDB.CreateCommand("PermittedExperiment_RetrieveIDsByRecurrence", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            // populate the parameters
            cmd.Parameters.Add(FactoryDB.CreateParameter("@recurrenceID", recurrenceID,DbType.Int32));
            
            // execute the command
            DbDataReader dataReader = null;
            ArrayList arrayList = new ArrayList();
            try
            {
                connection.Open();
                dataReader = cmd.ExecuteReader();
                //store the permitteExperimentIDs retrieved in arraylist

                while (dataReader.Read())
                {
                    if (dataReader["Experiment_Info_ID"] != System.DBNull.Value)
                        arrayList.Add(Convert.ToInt32(dataReader["Experiment_Info_ID"]));
                }
                dataReader.Close();
            }

            catch (Exception ex)
            {
                throw new Exception("Exception thrown in retrieve permittedExperimentInfoIDs by recurrence", ex);
            }
            finally
            {
                connection.Close();
            }
            int[] permittedExperimentInfoIDs = Utilities.ArrayListToIntArray(arrayList);
            return permittedExperimentInfoIDs;
        }

		/// <summary>
		/// returns an array of the immutable PermittedExperiment objects that correspond to the supplied permittedExperimentIDs
		/// </summary>
		/// <param name="permittedExperimentIDs"></param>
		/// <returns></returns>an array of immutable objects describing the specified PermittedExperiments
		public PermittedExperiment[] GetPermittedExperiments(int[] permittedExperimentIDs)
		{
			PermittedExperiment[] permittedExperiments=new  PermittedExperiment[permittedExperimentIDs.Length];
			for(int i=0; i<permittedExperimentIDs.Length;i++)
			{
				permittedExperiments[i]=new  PermittedExperiment();
			}
			// create sql connection
			DbConnection connection = FactoryDB.GetConnection();

			// create sql command
			// command executes the "RetrievePermittedExperimentByID" stored procedure
            DbCommand cmd = FactoryDB.CreateCommand("PermittedExperiment_RetrieveByID", connection);
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add(FactoryDB.CreateParameter("@permittedExperimentID", null, DbType.Int32));
			//execute the command
			try 
			{
				connection.Open();
				for(int i=0;i<permittedExperimentIDs.Length;i++)
				{
					// populate the parameters
					
					cmd.Parameters["@permittedExperimentID"].Value = permittedExperimentIDs[i];	
					DbDataReader dataReader = null;
					dataReader=cmd.ExecuteReader();
					while(dataReader.Read())
					{	
						permittedExperiments[i].permittedExperimentId=permittedExperimentIDs[i];
						if(dataReader[0] != System.DBNull.Value )
							permittedExperiments[i].experimentInfoId=(int)dataReader.GetInt32(0);
						if(dataReader[1] != System.DBNull.Value )
							permittedExperiments[i].recurrenceId=(int)dataReader.GetInt32(1);
					}
					dataReader.Close();
				}	
			} 

			catch (Exception ex)
			{
				throw new Exception("Exception thrown in get permittedExperiment",ex);
			}
			finally
			{
				connection.Close();
			}	
			return permittedExperiments;
		}
/*
		/// <summary>
		/// retrieve unique ID of the PerimmttiedExperiment which represents the permission of executing a particular experiment in a particular time block
		/// </summary>
		/// <param name="experimentInfoID"></param>
		/// <param name="timeBlockID"></param>
		/// <returns></returns>-1 if the permission can not be retrieved
		public int ListPermittedExperimentID(int experimentInfoID, int timeBlockID)
		{
			int permittedExperimentID=-1;
			// create sql connection
			DbConnection connection = FactoryDB.GetConnection();

			// create sql command
			// command executes the "RetrievePermittedExperimentID" stored procedure
            DbCommand cmd = FactoryDB.CreateCommand("PermittedExperiment_RetrieveID", connection);
			cmd.CommandType = CommandType.StoredProcedure;

			// populate the parameters
			DbParameter experimentInfoIDParam = cmd.Parameters.Add(FactoryDB.CreateParameter("@experimentInfoID", DbType.Int);
			experimentInfoIDParam.Value = experimentInfoID;
			DbParameter timeBlockIDParam = cmd.Parameters.Add(FactoryDB.CreateParameter("@timeBlockID", DbType.Int);
			timeBlockIDParam.Value = timeBlockID;

			// execute the command
			
			try 
			{
				connection.Open();
				Object ob=cmd.ExecuteScalar();
                if (ob != null && ob != System.DBNull.Value)
				{
					permittedExperimentID = Int32.Parse(ob.ToString());
				}
					
			} 

			catch (Exception ex)
			{
				throw new Exception("Exception thrown in retrieve permittedExperimentIDs by timeBlock",ex);
			}
			finally
			{
				connection.Close();
			}
			
			return permittedExperimentID;
		}
 * 
 * */

        /// <summary>
        /// retrieve unique ID of the PerimmttiedExperiment which represents the permission of executing a particular experiment in a particular recurrence
        /// </summary>
        /// <param name="experimentInfoID"></param>
        /// <param name="recurrenceID"></param>
        /// <returns></returns>-1 if the permission can not be retrieved
        public int ListPermittedExperimentIDByRecur(int experimentInfoID, int recurrenceID)
        {
            int permittedExperimentID = -1;
            // create sql connection
            DbConnection connection = FactoryDB.GetConnection();

            // create sql command
            // command executes the "RetrievePermittedExperimentID" stored procedure
            DbCommand cmd = FactoryDB.CreateCommand("PermittedExperiment_RetrieveIDByRecur", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            // populate the parameters
            cmd.Parameters.Add(FactoryDB.CreateParameter("@experimentInfoID", experimentInfoID, DbType.Int32));
            cmd.Parameters.Add(FactoryDB.CreateParameter("@recurrenceID", recurrenceID, DbType.Int32));
            
            // execute the command

            try
            {
                connection.Open();
                Object ob = cmd.ExecuteScalar();
                if (ob != null && ob != System.DBNull.Value)
                {
                    permittedExperimentID = Int32.Parse(ob.ToString());
                }

            }

            catch (Exception ex)
            {
                throw new Exception("Exception thrown in retrieve permittedExperimentIDs by recurrence", ex);
            }
            finally
            {
                connection.Close();
            }

            return permittedExperimentID;
        }

        /* !------------------------------------------------------------------------------!
			 *							CALLS FOR Permited CredentialSet
			 * !------------------------------------------------------------------------------!
			 */
        /// <summary>
        /// add permission of a particular experiment being executed in a particular recurrence
        /// </summary>
        /// <param name="experimentInfoID"></param>
        /// <param name="recurrenceID"></param>
        /// <returns></returns>the unique ID which identifies the permission added. >0 was successfully added;==-1 otherwise
        public int AddPermittedCredentialSet(int credentialSetID, int recurrenceID)
        {
            //create a connection
            DbConnection connection = FactoryDB.GetConnection();
            //create a command
            //command executes the "AddPermittedExperiment" store procedure
            DbCommand cmd = FactoryDB.CreateCommand("PermittedGroup_Add", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            //populate the parameters
            cmd.Parameters.Add(FactoryDB.CreateParameter("@credentialSetID", credentialSetID, DbType.Int32));
            cmd.Parameters.Add(FactoryDB.CreateParameter("@recurrenceID",  recurrenceID, DbType.Int32));
            
            int i = -1;

            // execute the command
            try
            {
                connection.Open();
                Object ob = cmd.ExecuteScalar();
                if (ob != null && ob != System.DBNull.Value)
                {
                    i = Int32.Parse(ob.ToString());
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception thrown in add permitted experiment", ex);
            }
            finally
            {
                connection.Close();
            }
            return i;
        }

        /// <summary>
        /// delete permission of  a particular experiment being executed in a particular time block
        /// </summary>
        /// <param name="permittedExperimentIDs"></param>
        /// <returns></returns>an array of ints containing the IDs of all permissions not successfully removed
        public int[] RemovePermittedCredentialSets(int[] permittedCredentialSetIDs, int recurrenceID)
        {
            ArrayList arrayList = new ArrayList();
            //create a connection
            DbConnection connection = FactoryDB.GetConnection();
            //create a command
            //command executes the "DeletePermittedExperiment" store procedure
            DbCommand cmd = FactoryDB.CreateCommand("PermittedGroup_Delete", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(FactoryDB.CreateParameter("@groupID", null,DbType.Int32));
            cmd.Parameters.Add(FactoryDB.CreateParameter( "@recurrenceID", recurrenceID, DbType.Int32));
          
            // execute the command
            try
            {
                connection.Open();
                //populate the parameters
                foreach (int permittedCredentialSetID in permittedCredentialSetIDs)
                {
                    cmd.Parameters["@groupID"].Value = permittedCredentialSetID;
                    if (cmd.ExecuteNonQuery() == 0)
                    {
                        arrayList.Add(permittedCredentialSetID);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception thrown in remove permitted experiments", ex);
            }
            finally
            {
                connection.Close();
            }
            int[] uIDs = Utilities.ArrayListToIntArray(arrayList);
            return uIDs;
        }

        /*
        /// <summary>
        /// enumerates the IDs of information of the permitted experiments for a particular time block identified by the timeBlockID
        /// </summary>
        /// <param name="timeBlockID"></param>
        /// <returns></returns>an array of ints containing the IDs of the information of the permitted experiments for a particular time block identified by the timeBlockID
        public int[] ListCredentialSetIDsByTimeBlock(int timeBlockID)
        {

            // create sql connection
            DbConnection connection = FactoryDB.GetConnection();

            // create sql command
            // command executes the "RetrievePermittedExperimentInfoIDsByTimeBlock" stored procedure
            DbCommand cmd = FactoryDB.CreateCommand("PermittedCredentialSet_RetrieveIDsByTimeBlock", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            // populate the parameters
            DbParameter timeBlockIDParam = cmd.Parameters.Add(FactoryDB.CreateParameter("@timeBlockID", DbType.Int);
            timeBlockIDParam.Value = timeBlockID;

            // execute the command
            DbDataReader dataReader = null;
            ArrayList arrayList = new ArrayList();
            try
            {
                connection.Open();
                dataReader = cmd.ExecuteReader();
                //store the permitteExperimentIDs retrieved in arraylist

                while (dataReader.Read())
                {
                    if (dataReader["Credential_Set_ID"] != System.DBNull.Value)
                        arrayList.Add(Convert.ToInt32(dataReader["Credential_Set_ID"]));
                }
                dataReader.Close();
            }

            catch (Exception ex)
            {
                throw new Exception("Exception thrown in retrieve permittedCredentialSetIDs by timeBlock", ex);
            }
            finally
            {
                connection.Close();
            }
            int[] permittedCredentialSetIDs = Utilities.ArrayListToIntArray(arrayList);
            return permittedCredentialSetIDs;
        }
         * */

        public bool IsPermittedCredentialSet(int credentialSetID, int recurrenceID)
        {
              // create sql connection
            DbConnection connection = FactoryDB.GetConnection();

            // create sql command
            // command executes the "RetrievePermittedExperimentInfoIDsByRecurrence" stored procedure
            DbCommand cmd = FactoryDB.CreateCommand("IsPermittedGroup", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            // populate the parameters
            cmd.Parameters.Add(FactoryDB.CreateParameter("@credentialID", credentialSetID, DbType.Int32));
            cmd.Parameters.Add(FactoryDB.CreateParameter("@recurrenceID", recurrenceID, DbType.Int32));
            
            int count = 0;
            try
            {
                connection.Open();
                count = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                connection.Close();
            }
            return count > 0;
        }

        /// <summary>
        /// enumerates the IDs of information of the permitted experiments for a particular recurrence identified by the recurrenceID
        /// </summary>
        /// <param name="recurrenceID"></param>
        /// <returns></returns>an array of ints containing the IDs of the information of the permitted experiments for a particular recurrence identified by the recurrenceID
        public int[] ListCredentialSetIDsByRecurrence(int recurrenceID)
        {

            // create sql connection
            DbConnection connection = FactoryDB.GetConnection();

            // create sql command
            // command executes the "RetrievePermittedExperimentInfoIDsByRecurrence" stored procedure
            DbCommand cmd = FactoryDB.CreateCommand("PermittedGroup_RetrieveIDsForRecurrence", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            // populate the parameters
            cmd.Parameters.Add(FactoryDB.CreateParameter("@recurrenceID", recurrenceID, DbType.Int32));

            // execute the command
            DbDataReader dataReader = null;
            ArrayList arrayList = new ArrayList();
            try
            {
                connection.Open();
                dataReader = cmd.ExecuteReader();
                //store the permitteExperimentIDs retrieved in arraylist

                while (dataReader.Read())
                {
                    if (dataReader["Credential_Set_ID"] != System.DBNull.Value)
                        arrayList.Add(Convert.ToInt32(dataReader["Credential_Set_ID"]));
                }
                dataReader.Close();
            }

            catch (Exception ex)
            {
                throw new Exception("Exception thrown in retrieve permittedCredentialSetIDs by recurrence", ex);
            }
            finally
            {
                connection.Close();
            }
            int[] permittedCredentialSetIDs = Utilities.ArrayListToIntArray(arrayList);
            return permittedCredentialSetIDs;
        }

      /*  
        /// <summary>
        /// returns an array of the immutable PermittedExperiment objects that correspond to the supplied permittedExperimentIDs
        /// </summary>
        /// <param name="permittedExperimentIDs"></param>
        /// <returns></returns>an array of immutable objects describing the specified PermittedExperiments
        public CredentialSet[] GetPermittedCredentialSet(int[] permittedCredentialSetIDs)
        {
            CredentialSet[] permittedCredentialSets = new PermittedExperiment[permittedCredentialSetIDs.Length];
            for (int i = 0; i < permittedCredentialSetIDs.Length; i++)
            {
                permittedCredentialSets[i] = new PermittedExperiment();
            }
            // create sql connection
            DbConnection connection = FactoryDB.GetConnection();

            // create sql command
            // command executes the "RetrievePermittedExperimentByID" stored procedure
            DbCommand cmd = FactoryDB.CreateCommand("RetrievePermittedExperimentByID", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(FactoryDB.CreateParameter(FactoryDB.CreateParameter("@permittedExperimentID", DbType.Int));
            //execute the command
            try
            {
                connection.Open();
                for (int i = 0; i < permittedExperimentIDs.Length; i++)
                {
                    // populate the parameters

                    cmd.Parameters["@permittedExperimentID"].Value = permittedExperimentIDs[i];
                    DbDataReader dataReader = null;
                    dataReader = cmd.ExecuteReader();
                    while (dataReader.Read())
                    {
                        permittedExperiments[i].permittedExperimentId = permittedExperimentIDs[i];
                        if (dataReader[0] != System.DBNull.Value)
                            permittedExperiments[i].experimentInfoId = (int)dataReader.GetInt32(0);
                        if (dataReader[1] != System.DBNull.Value)
                            permittedExperiments[i].recurrenceId = (int)dataReader.GetInt32(1);
                    }
                    dataReader.Close();
                }
            }

            catch (Exception ex)
            {
                throw new Exception("Exception thrown in get permittedExperiment", ex);
            }
            finally
            {
                connection.Close();
            }
            return permittedExperiments;
        }
        
        /// <summary>
        /// retrieve unique ID of the PerimmttiedExperiment which represents the permission of executing a particular experiment in a particular time block
        /// </summary>
        /// <param name="experimentInfoID"></param>
        /// <param name="timeBlockID"></param>
        /// <returns></returns>-1 if the permission can not be retrieved
        public int ListPermittedExperimentID(int experimentInfoID, int timeBlockID)
        {
            int permittedExperimentID = -1;
            // create sql connection
            DbConnection connection = FactoryDB.GetConnection();

            // create sql command
            // command executes the "RetrievePermittedExperimentID" stored procedure
            DbCommand cmd = FactoryDB.CreateCommand("RetrievePermittedExperimentID", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            // populate the parameters
            DbParameter experimentInfoIDParam = cmd.Parameters.Add(FactoryDB.CreateParameter("@experimentInfoID", DbType.Int);
            experimentInfoIDParam.Value = experimentInfoID;
            DbParameter timeBlockIDParam = cmd.Parameters.Add(FactoryDB.CreateParameter("@timeBlockID", DbType.Int);
            timeBlockIDParam.Value = timeBlockID;

            // execute the command

            try
            {
                connection.Open();
                Object ob = cmd.ExecuteScalar();
                if (ob != null && ob != System.DBNull.Value)
                {
                    permittedExperimentID = Int32.Parse(ob.ToString());
                }

            }

            catch (Exception ex)
            {
                throw new Exception("Exception thrown in retrieve permittedExperimentIDs by timeBlock", ex);
            }
            finally
            {
                connection.Close();
            }

            return permittedExperimentID;
        }
         * /
         * 
        /// <summary>
        /// retrieve unique ID of the PerimmttiedExperiment which represents the permission of executing a particular experiment in a particular recurrence
        /// </summary>
        /// <param name="experimentInfoID"></param>
        /// <param name="recurrenceID"></param>
        /// <returns></returns>-1 if the permission can not be retrieved
        public int ListPermittedExperimentIDByRecur(int experimentInfoID, int recurrenceID)
        {
            int permittedExperimentID = -1;
            // create sql connection
            DbConnection connection = FactoryDB.GetConnection();

            // create sql command
            // command executes the "RetrievePermittedExperimentID" stored procedure
            DbCommand cmd = FactoryDB.CreateCommand("RetrievePermittedExperimentIDByRecur", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            // populate the parameters
            DbParameter experimentInfoIDParam = cmd.Parameters.Add(FactoryDB.CreateParameter("@experimentInfoID", DbType.Int);
            experimentInfoIDParam.Value = experimentInfoID;
            DbParameter recurrenceIDParam = cmd.Parameters.Add(FactoryDB.CreateParameter("@recurrenceID", DbType.Int);
            recurrenceIDParam.Value = recurrenceID;

            // execute the command

            try
            {
                connection.Open();
                Object ob = cmd.ExecuteScalar();
                if (ob != null && ob != System.DBNull.Value)
                {
                    permittedExperimentID = Int32.Parse(ob.ToString());
                }

            }

            catch (Exception ex)
            {
                throw new Exception("Exception thrown in retrieve permittedExperimentIDs by recurrence", ex);
            }
            finally
            {
                connection.Close();
            }

            return permittedExperimentID;
        }
        */

		/* !------------------------------------------------------------------------------!
			 *							CALLS FOR ReservationInfo
			 * !------------------------------------------------------------------------------!
			 */
		/// <summary>
		/// add reservation information
		/// </summary>
		/// <param name="serviceBrokerGuid"></param>
		/// <param name="groupName"></param>
		/// <param name="ussGuid"></param>
		/// <param name="clientGuid"></param>
		/// <param name="labServerGuid"></param>
		/// <param name="startTime"></param>
		/// <param name="endTime"></param>
        /// <param name="statusCode"></param>
		/// <returns></returns>the unique ID identifying the reservation information added, >0 successfully added, -1 otherwise
		public int AddReservationInfo(string serviceBrokerGuid, string groupName, string ussGuid,
            string labServerGuid, string clientGuid, DateTime startTime, DateTime endTime, int statusCode)
		{
            int status = -1;
			//create a connection
			DbConnection connection= FactoryDB.GetConnection();
           
			//create a command
			//command executes the "AddReservationInfo" store procedure
                DbCommand cmd = FactoryDB.CreateCommand("ReservationInfo_Add", connection);
			cmd.CommandType=CommandType.StoredProcedure;
			//populate the parameters
			cmd.Parameters.Add(FactoryDB.CreateParameter("@serviceBrokerGUID", serviceBrokerGuid,DbType.AnsiString,50));
			cmd.Parameters.Add(FactoryDB.CreateParameter("@groupName", groupName, DbType.String,256));
            DbParameter ussParam = FactoryDB.CreateParameter("@ussGUID", DbType.AnsiString,50);
            if (ussGuid == null || ussGuid.Length == 0)
                ussParam.Value = DBNull.Value;
            else
                ussParam.Value = ussGuid;
			cmd.Parameters.Add(ussParam);
			cmd.Parameters.Add(FactoryDB.CreateParameter("@clientGuid", clientGuid,DbType.AnsiString,50));
			cmd.Parameters.Add(FactoryDB.CreateParameter("@labServerGuid", labServerGuid, DbType.AnsiString,50));
			cmd.Parameters.Add(FactoryDB.CreateParameter("@startTime", startTime, DbType.DateTime));
			cmd.Parameters.Add(FactoryDB.CreateParameter("@endTime", endTime, DbType.DateTime));
			cmd.Parameters.Add(FactoryDB.CreateParameter("@status", statusCode, DbType.Int32));
            
            // execute the command
			connection.Open();
            try
            {
                Object ob = cmd.ExecuteScalar();
                if (ob != null && ob != System.DBNull.Value)
                {
                    status = Convert.ToInt32(ob);
                }
			}
			catch (Exception ex)
			{
				throw new Exception("Exception thrown in add ReservationInfo: "+ex.Message,ex);
			}
			finally
			{
				connection.Close();
			}	
			return status;		
		}
		
        /// <summary>
        /// add reservationInfo
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="credentialSetID"></param>
        /// <param name="experimentInfoID"></param>
        /// <param name="resourceID"></param>
        /// <param name="status"></param>
        /// <returns></returns>the unique ID identifying the reservation information added, >0 successfully added, -1 otherwise
        public int AddReservationInfo(DateTime startTime, DateTime endTime, int credentialSetID, int experimentInfoID, int resourceID,
            int ussID, int status)
		{
			//create a connection
			DbConnection connection= FactoryDB.GetConnection();
			//create a command
			//command executes the "AddReservation" store procedure
            DbCommand cmd = FactoryDB.CreateCommand("Reservation_Add", connection);
			cmd.CommandType=CommandType.StoredProcedure;
			//populate the parameters
			cmd.Parameters.Add(FactoryDB.CreateParameter("@startTime",startTime, DbType.DateTime));
			cmd.Parameters.Add(FactoryDB.CreateParameter("@endTime", endTime, DbType.DateTime));
			cmd.Parameters.Add(FactoryDB.CreateParameter("@credentialSetID", credentialSetID, DbType.Int32));
			cmd.Parameters.Add(FactoryDB.CreateParameter("@experimentInfoID", experimentInfoID, DbType.Int32));
            cmd.Parameters.Add(FactoryDB.CreateParameter( "@resourceID", resourceID, DbType.Int32));
            DbParameter ussParam = FactoryDB.CreateParameter( "@ussID", DbType.Int32);
            if (ussID > 1)
                ussParam.Value = ussID;
            else
                ussParam.Value = 0;
            cmd.Parameters.Add(ussParam);
			cmd.Parameters.Add(FactoryDB.CreateParameter("@status", status, DbType.Int32));
            
			int i=-1;

			// execute the command
			try
			{
				connection.Open();
				Object ob=cmd.ExecuteScalar();
				if(ob!=null && ob!=System.DBNull.Value)
				{
					i= Int32.Parse(ob.ToString());
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Exception thrown in add ReservationInfo: " + ex.Message,ex);
			}
			finally
			{
				connection.Close();
			}	
			return i;		
		}


        /// <summary>
        /// Returns an Boolean indicating whether a particular reservation from a USS is confirmed and added to the database in LSS successfully. If it fails, exception will be throw out indicating
        ///	the reason for rejection.
        /// </summary>
        /// <param name="serviceBrokerGuid"></param>
        /// <param name="groupName"></param>
        /// <param name="ussGuid"></param>
        /// <param name="labServerGuid"></param>
        /// <param name="clientGuid"></param>
        /// <param name="startTime"></param>the local time of LSS
        /// <param name="endTime"></param>the local time of LSS
        /// <returns></returns>the notification whether the reservation is confirmed. If not, notification will give a reason
        public string ConfirmReservation(string serviceBrokerGuid, string groupName, string ussGuid,
             string labServerGuid, string clientGuid, DateTime startTime, DateTime endTime)
        {
            StringBuilder notification = new StringBuilder();
            try
            {

                //get the experiment configuration
                
                int eID = ListExperimentInfoIDByExperiment(labServerGuid, clientGuid);
                LssExperimentInfo exInfo = GetExperimentInfos(new int[] { eID })[0];
                int preTime = exInfo.prepareTime;
                int recTime = exInfo.recoverTime;
                int minTime = exInfo.minimumTime;
                //check whether the reservation is executable
                if (((TimeSpan)endTime.Subtract(startTime)).TotalMinutes < minTime)
                {
                    notification.Append("The reservation time is less than the minimum time the experiment required");
                    return notification.ToString();
                }
                //the start time for the experiment equipment
                DateTime expStartTime = startTime.AddMinutes(-preTime);
                //the end time for the experiment equipment
                DateTime expEndTime = endTime.AddMinutes(recTime);
                int status = MakeReservation(serviceBrokerGuid, groupName, ussGuid, labServerGuid, clientGuid,
                    startTime, endTime, ref notification);

                if (status > 0)
                {
                    notification.AppendLine("The reservation is confirmed successfully");
                    if (exInfo.contactEmail != null && exInfo.contactEmail.Length > 0)
                    {
                        // Send a mail Message
                        StringBuilder message = new StringBuilder();

                        MailMessage mail = new MailMessage();
                        mail.To = exInfo.contactEmail;
                        mail.From = ConfigurationManager.AppSettings["genericFromMailAddress"];
                        mail.Subject = "New Reservation: " + exInfo.labClientName;

                        message.Append("A new reservation for " + exInfo.labClientName + " version: " + exInfo.labClientVersion);
                        message.AppendLine(" on " + exInfo.labServerName + " has been made.");
                        message.AppendLine("\tGroup: " + groupName + " ServiceBroker: " + serviceBrokerGuid);
                        message.Append("\tFrom: " + DateUtil.ToUserTime(startTime, CultureInfo.CurrentCulture, DateUtil.LocalTzOffset));
                        message.AppendLine("\t\tTo: " + DateUtil.ToUserTime(endTime, CultureInfo.CurrentCulture, DateUtil.LocalTzOffset));
                        message.AppendLine("\tDateFormat: " + CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern + " \tUTC Offset: " + (DateUtil.LocalTzOffset / 60.0));
                        message.AppendLine();
                        mail.Body = message.ToString();
                        SmtpMail.SmtpServer = "127.0.0.1";

                        try
                        {
                            SmtpMail.Send(mail);
                        }
                        catch (Exception ex)
                        {
                            // Report detailed SMTP Errors
                            StringBuilder smtpErrorMsg = new StringBuilder();
                            smtpErrorMsg.Append("Exception: " + ex.Message);
                            //check the InnerException
                            if (ex.InnerException != null)
                            {
                                smtpErrorMsg.Append("<br>Inner Exceptions:");
                                while (ex.InnerException != null)
                                {
                                    smtpErrorMsg.Append("<br>" + ex.InnerException.Message);
                                    ex = ex.InnerException;
                                }
                                Logger.WriteLine(smtpErrorMsg.ToString());
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return notification.ToString();
        }

        public int MakeReservation(string serviceBrokerGuid, string groupName, string ussGuid,
      string labServerGuid, string clientGuid, DateTime startTime, DateTime endTime, ref StringBuilder notification)
        {
            int status = -1;
            Recurrence[] recurrences = GetRecurrences(serviceBrokerGuid, groupName,
                labServerGuid, clientGuid, startTime, endTime);
            if ((recurrences == null) || (recurrences.Length == 0))
            {
                notification.AppendLine("This experiment is not available to your group during this time.");
                return status;
            }
            else if (recurrences.Length > 1)
            {
                notification.Append("More than one recurrence was found for this request. Please choose another time.");
                return status;
            }

            else
            {
                TimeBlock[] reserved = ListReservationTimeBlocks(recurrences[0].resourceId, startTime, endTime);
                bool ok = true;
                if ((reserved != null) && (reserved.Length > 0))
                {

                    // Need to process the reservations to make suggestions
                    notification.AppendLine("This reservation time is not available now, please adjust your reservation time.");
                    TimeBlock check = new TimeBlock(startTime, endTime);
                    foreach (TimeBlock tb in reserved)
                    {
                        TimeBlock intersection = check.Intersection(tb);
                        if (intersection != null)
                        {
                            TimeSpan span = TimeSpan.FromSeconds(intersection.Duration);
                            if (intersection.Start < endTime)
                                notification.AppendLine(@"<br />Adjust the time by starting " + span.TotalMinutes +
                                    " minutes earlier, or make the duration shorter by " + span.TotalMinutes + " minutes.");
                            if (intersection.End > startTime)
                                notification.AppendLine(@"<br />Adjust the time by starting " + span.TotalMinutes +
                                    " minutes later.");

                        }

                    }
                    return 0;
                }
                //add the reservation to to reservationInfo table
                status = AddReservationInfo(serviceBrokerGuid, groupName, ussGuid, labServerGuid, clientGuid, startTime.ToUniversalTime(), endTime.ToUniversalTime(), 0);
            }
            return status;
        }


		/// <summary>
		/// delete the reservation information specified by the reservationInfoIDs
		/// </summary>
		/// <param name="reservationInfoIDs"></param>
		/// <returns></returns>an array of ints containning the IDs of all reservation information not successfully removed
		public int[] RemoveReservationInfoByIDs(int[] reservationInfoIDs)
		{
			ArrayList arrayList=new ArrayList();
			//create a connection
			DbConnection connection= FactoryDB.GetConnection();
			//create a command
			//command executes the "DeleteReservationInfoByID" store procedure
            DbCommand cmd = FactoryDB.CreateCommand("ReservationInfo_DeleteByID", connection);
			cmd.CommandType=CommandType.StoredProcedure;
			cmd.Parameters.Add(FactoryDB.CreateParameter("@reservationInfoID", null,DbType.Int32));
			
			// execute the command
			try
			{
				connection.Open();
				//populate the parameters
				foreach(int reservationInfoID in reservationInfoIDs)
				{
					cmd.Parameters["@reservationInfoID"].Value = reservationInfoID;
					if (cmd.ExecuteNonQuery()==0)
					{
						arrayList.Add(reservationInfoID);
					}
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Exception thrown in remove reservationInfobyID",ex);
			}
			finally
			{
				connection.Close();
			}	
			int[] uIDs = Utilities.ArrayListToIntArray(arrayList);
			return uIDs;			
		}
        
		/// <summary>
		/// remove the reservation information
		/// </summary>
        /// <param name="serviceBrokerGuid"></param>
		/// <param name="groupName"></param>
        /// <param name="ussGuid"></param>
		/// <param name="labClientName"></param>
		/// <param name="labClientVersion"></param>
		/// <param name="startTime"></param>
		/// <param name="endTime"></param>
		/// <returns>The number of reservationInfos deleted or -1 if an error</returns>
        public int RemoveReservationInfo(string serviceBrokerGuid, string groupName, string ussGuid,
            string labServerGuid, string clientGuid, DateTime startTime, DateTime endTime)
		{
            int status = -1;
			ArrayList arrayList=new ArrayList();
			//create a connection
			DbConnection connection= FactoryDB.GetConnection();
			//create a command
			//command executes the "DeleteReservationInfo" store procedure
            DbCommand cmd = FactoryDB.CreateCommand("ReservationInfo_Delete", connection);
			cmd.CommandType=CommandType.StoredProcedure;
			
			// execute the command
			try
			{
				connection.Open();
				//populate the parameters
			
				cmd.Parameters.Add(FactoryDB.CreateParameter("@serviceBrokerGUID", serviceBrokerGuid, DbType.AnsiString,50));
                cmd.Parameters.Add(FactoryDB.CreateParameter("@groupName", groupName, DbType.String,256));
				cmd.Parameters.Add(FactoryDB.CreateParameter("@ussGUID", ussGuid, DbType.AnsiString,50));
                cmd.Parameters.Add(FactoryDB.CreateParameter("@clientGuid", clientGuid, DbType.AnsiString,256));
				cmd.Parameters.Add(FactoryDB.CreateParameter("@labServerGuid", labServerGuid, DbType.AnsiString,50));
				cmd.Parameters.Add(FactoryDB.CreateParameter("@startTime", startTime, DbType.DateTime));
				cmd.Parameters.Add(FactoryDB.CreateParameter("@endTime", endTime, DbType.DateTime));

                //Return number of reservationInfos deleted
                status = cmd.ExecuteNonQuery();
				
				
			}
			catch (Exception ex)
			{
				throw new Exception("Exception thrown in remove reservationInfo",ex);
			}
			finally
			{
				connection.Close();
			}	
			
			return status;			
		}


        public ReservationData[] RetrieveReservationData(int resourceId, int expId, int credId, DateTime start, DateTime end)
        {

            List<ReservationData> data = new List<ReservationData>();
            DbConnection connection = FactoryDB.GetConnection();

            // create sql command
            // command executes the "Reserva" stored procedure
            DbCommand cmd = FactoryDB.CreateCommand("ReservationData_Retrieve", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            //if (resourceId < 1)
            //    cmd.Parameters.Add(FactoryDB.CreateParameter( "@resourceid", null, DbType.Int32));
            //else
                cmd.Parameters.Add(FactoryDB.CreateParameter( "@resourceid", resourceId, DbType.Int32));
            //if (expId < 1)
            //    cmd.Parameters.Add(FactoryDB.CreateParameter( "@expid", null, DbType.Int32));
            //else
                cmd.Parameters.Add(FactoryDB.CreateParameter( "@expid", expId, DbType.Int32));
            //if (credId < 1)
            //    cmd.Parameters.Add(FactoryDB.CreateParameter( "@credid", null, DbType.Int32));
            //else
                cmd.Parameters.Add(FactoryDB.CreateParameter( "@credid", credId, DbType.Int32));
            //if (start == DateTime.MinValue)
            //    cmd.Parameters.Add(FactoryDB.CreateParameter( "@start", null, DbType.DateTime));
            //else
                cmd.Parameters.Add(FactoryDB.CreateParameter( "@start", start, DbType.DateTime));
            //if (end == DateTime.MinValue)
            //    cmd.Parameters.Add(FactoryDB.CreateParameter( "@end", null, DbType.DateTime));
            //else
                cmd.Parameters.Add(FactoryDB.CreateParameter( "@end", end, DbType.DateTime));
 /*
            //if (resourceId > 0)
                cmd.Parameters.Add(FactoryDB.CreateParameter( "@resourceid", resourceId, DbType.Int32));
            //if (expId >0)
                cmd.Parameters.Add(FactoryDB.CreateParameter( "@expid", expId, DbType.Int32));
            //if (credId > 0)
                cmd.Parameters.Add(FactoryDB.CreateParameter( "@credid", credId, DbType.Int32));
            if (start == DateTime.MinValue)
                cmd.Parameters.Add(FactoryDB.CreateParameter( "@start", null, DbType.DateTime));
            else
                cmd.Parameters.Add(FactoryDB.CreateParameter( "@start", start, DbType.DateTime));
            if (end == DateTime.MinValue)
                cmd.Parameters.Add(FactoryDB.CreateParameter( "@end", null, DbType.DateTime));
            else
                cmd.Parameters.Add(FactoryDB.CreateParameter( "@end", end, DbType.DateTime));
  * ***/
            DbDataReader dataReader = null;
            try
            {
                connection.Open();
                dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    ReservationData rd = new ReservationData(DateUtil.SpecifyUTC(dataReader.GetDateTime(1)),
                        DateUtil.SpecifyUTC(dataReader.GetDateTime(2)));
                    rd.reservationID = dataReader.GetInt32(0);
                    //rd.start = DateUtil.SpecifyUTC(dataReader.GetDateTime(1));
                    //rd.end = DateUtil.SpecifyUTC(dataReader.GetDateTime(2));
                    rd.clientGuid = dataReader.GetString(3);
                    rd.labServerGuid = dataReader.GetString(4);
                    rd.groupName = dataReader.GetString(5);
                    rd.sbGuid = dataReader.GetString(6);
                    if (dataReader.IsDBNull(7))
                        rd.ussId = 0;
                    else
                        rd.ussId = dataReader.GetInt32(7);
                    rd.status = dataReader.GetInt32(8);
                    data.Add(rd);
                }

            }
            catch
            {
                throw;
            }
            finally
            {
                connection.Close();
            }
            return data.ToArray();
        }
		
		/// <summary>
		/// enumerates all IDs of the reservations made to a particular experiment identified by the experimentInfoID
		/// </summary>
		/// <param name="experimentInfoID"></param>
		/// <returns></returns>an array of ints containing the IDs of all the reservation information made to the specified experiment
		public int[] ListReservationInfoIDsByExperiment(int experimentInfoID)
		{
			int[] reservationInfoIDs;
			// create sql connection
			DbConnection connection = FactoryDB.GetConnection();

			// create sql command
			// command executes the "RetrieveReserveInfoIDsByExperiment" stored procedure
            DbCommand cmd = FactoryDB.CreateCommand("ReserveInfo_RetrieveIDsByExperiment", connection);
			cmd.CommandType = CommandType.StoredProcedure;

			// populate the parameters
			cmd.Parameters.Add(FactoryDB.CreateParameter("@experimentInfoID", experimentInfoID, DbType.Int32));
			
			// execute the command
			DbDataReader dataReader = null;
			ArrayList arrayList=new ArrayList();
			try 
			{
				connection.Open();
				dataReader = cmd.ExecuteReader ();
				//store the reservationInfoIDs retrieved in arraylist
				
				while(dataReader.Read ())
				{	
					if(dataReader["Reservation_Info_ID"] != System.DBNull.Value )
						arrayList.Add(Convert.ToInt32(dataReader["Reservation_Info_ID"]));
				}
				dataReader.Close();
			} 

			catch (Exception ex)
			{
				throw new Exception("Exception thrown in retrieve reservationInfoIDs by experiment",ex);
			}
			finally
			{
				connection.Close();
			}
			reservationInfoIDs=Utilities.ArrayListToIntArray(arrayList);
			return reservationInfoIDs;
		}
		/// <summary>
		/// enumerates all IDs of the reservations made to a particular experiment from a particular group between the start time and the end time
		/// </summary>
        /// <param name="serviceBrokerGuid"></param>
		/// <param name="groupName"></param>
        /// <param name="ussGuid"></param>
		/// <param name="labClientName"></param>
		/// <param name="labClientVersion"></param>
		/// <param name="startTime"></param>
		/// <param name="endTime"></param>
		/// <returns></returns>
        public int[] ListReservationInfoIDs(string serviceBrokerGuid, string groupName, string ussGuid,
            string labServerGuid, string clientGuid, DateTime startTime, DateTime endTime)
		{
			List<int> reservationInfoIDs = new List<int>();
			// create sql connection
			DbConnection connection = FactoryDB.GetConnection();

			// create sql command
			// command executes the "RetrieveReserveInfoIDs" stored procedure
            DbCommand cmd = FactoryDB.CreateCommand("ReservationInfo_RetrieveIDs", connection);
			cmd.CommandType = CommandType.StoredProcedure;

			// populate the parameters
            cmd.Parameters.Add(FactoryDB.CreateParameter( "@serviceBrokerGUID", serviceBrokerGuid, DbType.AnsiString, 50));
            cmd.Parameters.Add(FactoryDB.CreateParameter("@groupName", groupName, DbType.String,256));
            DbParameter ussParameter = FactoryDB.CreateParameter( "@ussGUID", ussGuid, DbType.AnsiString, 50);
            if (ussGuid != null && ussGuid.Length > 0)
                ussParameter.Value = ussGuid;
            else
                ussParameter.Value = DBNull.Value;
            cmd.Parameters.Add(ussParameter);
            cmd.Parameters.Add(FactoryDB.CreateParameter("@clientGuid", clientGuid, DbType.AnsiString,50));
			cmd.Parameters.Add(FactoryDB.CreateParameter("@labServerGuid", labServerGuid, DbType.AnsiString,50));
			cmd.Parameters.Add(FactoryDB.CreateParameter("@startTime", startTime, DbType.DateTime));
			cmd.Parameters.Add(FactoryDB.CreateParameter("@endTime", endTime, DbType.DateTime));
			
			// execute the command
			DbDataReader dataReader = null;
			
			try 
			{
				connection.Open();
				dataReader = cmd.ExecuteReader ();
				//store the reservationInfoIDs retrieved in arraylist
				
				while(dataReader.Read ())
				{	
					if(dataReader["Reservation_Info_ID"] != System.DBNull.Value )
						reservationInfoIDs.Add(dataReader.GetInt32(0));
				}
				dataReader.Close();
			} 

			catch (Exception ex)
			{
				throw new Exception("Exception thrown in retrieve reservationInfoIDs ",ex);
			}
			finally
			{
				connection.Close();
			}
			
			return reservationInfoIDs.ToArray();
		}

/// <summary>
/// retrieve reservation made to a particular labserver during a given time chunk.
/// </summary>
/// <param name="labServerGuid"></param>
/// <param name="startTime"></param>
/// <param name="endTime"></param>
/// <returns></returns>
		public int[] ListReservationInfoIDsByLabServer(string labServerGuid, DateTime startTime, DateTime endTime)
		{
            List<int> reservationInfoIDs = new List<int>();
			// create sql connection
			DbConnection connection = FactoryDB.GetConnection();

			// create sql command
			// command executes the "RetrieveReservationInfoIDByLabServer" stored procedure
            DbCommand cmd = FactoryDB.CreateCommand("ReservationInfo_RetrieveIDByLabServer", connection);
			cmd.CommandType = CommandType.StoredProcedure;

			// populate the parameters
			cmd.Parameters.Add(FactoryDB.CreateParameter("@labServerGUID", labServerGuid, DbType.AnsiString,50));
			cmd.Parameters.Add(FactoryDB.CreateParameter("@startTime", startTime, DbType.DateTime));
			cmd.Parameters.Add(FactoryDB.CreateParameter("@endTime", endTime, DbType.DateTime));
			
			// execute the command
			DbDataReader dataReader = null;
			try 
			{
				connection.Open();
				dataReader = cmd.ExecuteReader ();
				while(dataReader.Read ())
				{	
					if(dataReader["Reservation_Info_ID"] != System.DBNull.Value )
						reservationInfoIDs.Add(dataReader.GetInt32(0));
				}
				dataReader.Close();
			} 
			catch (Exception ex)
			{
				throw new Exception("Exception thrown in retrieve reservationInfoIDsByLabServer ",ex);
			}
			finally
			{
				connection.Close();
			}
			return reservationInfoIDs.ToArray();
		}

        /// <summary>
        /// retrieve reservation made to a particular labserver during a given time chunk.
        /// </summary>
        /// <param name="labServerGuid"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public int[] ListReservationInfoIDsByLabResource(int resourceID, DateTime startTime, DateTime endTime)
        {
            List<int> reservations = new List<int>();
            // create sql connection
            DbConnection connection = FactoryDB.GetConnection();

            // create sql command
            // command executes the "RetrieveReservationInfoIDByLabServer" stored procedure
            DbCommand cmd = FactoryDB.CreateCommand("ReservationInfo_RetrieveIDByResource", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            // populate the parameters
            cmd.Parameters.Add(FactoryDB.CreateParameter("@resourceID", resourceID, DbType.Int32));
            cmd.Parameters.Add(FactoryDB.CreateParameter("@startTime", startTime, DbType.DateTime));
            cmd.Parameters.Add(FactoryDB.CreateParameter("@endTime", endTime, DbType.DateTime));
            
            // execute the command
            DbDataReader dataReader = null;

            try
            {
                connection.Open();
                dataReader = cmd.ExecuteReader();
                //store the reservationInfoIDs retrieved in arraylist

                while (dataReader.Read())
                {
                    if (!dataReader.IsDBNull(0))
                        reservations.Add(dataReader.GetInt32(0));
                }
                dataReader.Close();
            }

            catch (Exception ex)
            {
                throw new Exception("Exception thrown in retrieve reservationInfoIDsByResource ", ex);
            }
            finally
            {
                connection.Close();
            }
           
            return reservations.ToArray();
        }
/*
        /// <summary>
        /// retrieve reservation made to a particular labserver during a given time chunk.
        /// </summary>
        /// <param name="labServerGuid"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public int[] ListReservationInfoIDsByRecurrence(int resourceID, DateTime startTime, DateTime endTime)
        {
            List<int> reservations = new List<int>();
            // create sql connection
            DbConnection connection = FactoryDB.GetConnection();

            // create sql command
            // command executes the "RetrieveReservationInfoIDByLabServer" stored procedure
            DbCommand cmd = FactoryDB.CreateCommand("ReservationInfo_RetrieveIDByRecurrence", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            // populate the parameters
            DbParameter resourceIDParam = cmd.Parameters.Add(FactoryDB.CreateParameter("@recurrenceID", DbType.Int);
            resourceIDParam.Value = resourceID;
            DbParameter startTimeParam = cmd.Parameters.Add(FactoryDB.CreateParameter("@startTime", DbType.DateTime);
            startTimeParam.Value = startTime;
            DbParameter endTimeParam = cmd.Parameters.Add(FactoryDB.CreateParameter("@endTime", DbType.DateTime);
            endTimeParam.Value = endTime;

            // execute the command
            DbDataReader dataReader = null;

            try
            {
                connection.Open();
                dataReader = cmd.ExecuteReader();
                //store the reservationInfoIDs retrieved in arraylist

                while (dataReader.Read())
                {
                    if (!dataReader.IsDBNull(0))
                        reservations.Add(dataReader.GetInt32(0));
                }
                dataReader.Close();
            }

            catch (Exception ex)
            {
                throw new Exception("Exception thrown in retrieve reservationInfoIDsByResource ", ex);
            }
            finally
            {
                connection.Close();
            }

            return reservations.ToArray();
        }
 */
        /// <summary>
        /// This returns all the reservation times that intersect this time span and use the same resource.
        /// </summary>
        /// <param name="resourceId"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public TimeBlock[] ListReservationTimeBlocks(int resourceId, DateTime startTime, DateTime endTime){
            List<TimeBlock> blocks = new List<TimeBlock>();
          
            // create sql connection
            DbConnection connection = FactoryDB.GetConnection();

            // create sql command
            // command executes the "RetrieveReservationInfoIDByLabServer" stored procedure
            DbCommand cmd = FactoryDB.CreateCommand("Reservation_GetTimes", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            // populate the parameters
            cmd.Parameters.Add(FactoryDB.CreateParameter("@resourceID", resourceId, DbType.Int32));
            cmd.Parameters.Add(FactoryDB.CreateParameter("@start", startTime, DbType.DateTime));
            cmd.Parameters.Add(FactoryDB.CreateParameter("@end", endTime, DbType.DateTime));
            
            // execute the command
			DbDataReader dataReader = null;
			try 
			{
				connection.Open();
				dataReader = cmd.ExecuteReader ();
				//store the reservationInfoIDs retrieved in arraylist
				DateTime start;
                DateTime end;
				while(dataReader.Read ())
				{	
                    start = DateTime.SpecifyKind(dataReader.GetDateTime(0),DateTimeKind.Utc);
                    end = DateTime.SpecifyKind(dataReader.GetDateTime(1), DateTimeKind.Utc);
                    blocks.Add(new TimeBlock(start,end));
				}
				dataReader.Close();
			} 

			catch (Exception ex)
			{
				throw new Exception("Exception thrown in retrieve reservation_Times ",ex);
			}
			finally
			{
				connection.Close();
			}
			
			return blocks.ToArray();
		}   
		
		/// <summary>
		/// to select reservation Infos accorrding to given criterion
		/// </summary>
        public ReservationInfo[] SelectReservationInfo(string labServerGuid, int experimentInfoID, int credentialSetID, DateTime timeAfter, DateTime timeBefore)
		{
            List<ReservationInfo> reInfos = new List<ReservationInfo>();

            StringBuilder sqlQuery = new StringBuilder();
            sqlQuery.Append("select Reservation_Info_ID, resource_ID,Start_Time, End_Time, R.Experiment_Info_ID, Credential_Set_ID, Status, uss_Info_ID from Reservation_Info AS R Join Experiment_Info AS E on (R.Experiment_Info_ID = E.Experiment_Info_ID) where E.Lab_server_GUID = " + "'" + labServerGuid + "'");
			if (experimentInfoID!=-1)
			{
				sqlQuery.Append(" and R.Experiment_Info_ID = " + experimentInfoID);
			}
			if (credentialSetID !=-1)
			{
					sqlQuery.Append(" and R.Credential_Set_ID =" + credentialSetID);
			}

			if (timeBefore.CompareTo(DateTime.MinValue)!=0)
			{
				
				sqlQuery.Append(" and R.Start_Time <= '"+timeBefore+"'");
			}

			if (timeAfter.CompareTo(DateTime.MinValue)!=0)
			{
				
				sqlQuery.Append(" and R.Start_Time >= '"+timeAfter+"'");
			}

			sqlQuery.Append("ORDER BY R.Start_Time asc");

			DbConnection myConnection = FactoryDB.GetConnection();
			DbCommand myCommand = myConnection.CreateCommand();
			myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = sqlQuery.ToString(); ;

			try 
			{
				myConnection.Open ();
				
				// get ReservationInfo info from table reservation_Info
				DbDataReader myReader = myCommand.ExecuteReader ();
				while(myReader.Read ())
				{	
					ReservationInfo ri = new ReservationInfo();
					ri.reservationInfoId = Convert.ToInt32( myReader["Reservation_Info_ID"]); //casting to (long) didn't work
                    if (myReader["Resource_ID"] != System.DBNull.Value)
                        ri.resourceId = Convert.ToInt32(myReader["Resource_ID"]);
					if(myReader["Start_Time"] != System.DBNull.Value )
						ri.startTime = DateUtil.SpecifyUTC((DateTime) myReader["Start_Time"]);
					if(myReader["End_Time"] != System.DBNull.Value )
						ri.endTime= DateUtil.SpecifyUTC((DateTime) myReader["End_Time"]);
					if(myReader["Resource_ID"]!=System.DBNull.Value)
						ri.resourceId=Convert.ToInt32(myReader["Resource_ID"]);
					if(myReader["Credential_Set_ID"] != System.DBNull.Value )
						ri.credentialSetId = Convert.ToInt32(myReader["Credential_Set_ID"]);
                    if (myReader["Status"] != System.DBNull.Value)
                        ri.statusCode= Convert.ToInt32(myReader["Status"]);
                    if (myReader["uss_Info_ID"] != System.DBNull.Value)
                        ri.ussId = Convert.ToInt32(myReader["uss_Info_ID"]);
                    else
                        ri.ussId = 0;	
					reInfos.Add(ri);

				}
				myReader.Close ();
				
			
			}
			catch (Exception ex)
			{
				throw new Exception("Exception thrown in selecting reservation information",ex);
			}
			finally
			{
				myConnection.Close();
			}

			return reInfos.ToArray();
		}

		/// <summary>
		/// returns an array of the immutable ReservationInfo objects that correspond to the supplied reservationInfoIDs
		/// </summary>
		/// <param name="reservationInfoIDs"></param>
		/// <returns></returns>an array ofimmutable objects describing the specified reservations
 
		public ReservationInfo[] GetReservationInfos(int[] reservationInfoIDs)
		{
            
            List<ReservationInfo> reservationInfos = null;
            if (reservationInfoIDs.Length > 0)
            {
                reservationInfos = new List<ReservationInfo>();
                // create sql connection
                DbConnection connection = FactoryDB.GetConnection();

                // create sql command
                // command executes the "RetrieveReservationInfoByID" stored procedure
                DbCommand cmd = FactoryDB.CreateCommand("ReservationInfo_RetrieveByID", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(FactoryDB.CreateParameter("@reservationInfoID", null, DbType.Int32));
                //execute the command
                try
                {
                    connection.Open();
                    for (int i = 0; i < reservationInfoIDs.Length; i++)
                    {
                        // populate the parameters
                        cmd.Parameters["@reservationInfoID"].Value = reservationInfoIDs[i];
                        DbDataReader dataReader = null;
                        dataReader = cmd.ExecuteReader();
                        while (dataReader.Read())
                        {
                            ReservationInfo reservationInfo = new ReservationInfo();
                            reservationInfo.reservationInfoId = reservationInfoIDs[i];
                            reservationInfo.resourceId = dataReader.GetInt32(0);
                            if (dataReader[1] != System.DBNull.Value)
                            {
                                DateTime test = dataReader.GetDateTime(1);
                                reservationInfo.startTime = DateUtil.SpecifyUTC(dataReader.GetDateTime(1));
                            }
                            if (dataReader[2] != System.DBNull.Value)
                                reservationInfo.endTime = DateUtil.SpecifyUTC(dataReader.GetDateTime(2));
                            if (dataReader[3] != System.DBNull.Value)
                                reservationInfo.experimentInfoId = (int)dataReader.GetInt32(3);
                            if (dataReader[4] != System.DBNull.Value)
                                reservationInfo.credentialSetId = (int)dataReader.GetInt32(4);
                            if (dataReader[5] != System.DBNull.Value)
                                reservationInfo.ussId = (int)dataReader.GetInt32(5);
                            if (dataReader[6] != System.DBNull.Value)
                                reservationInfo.ussId = (int)dataReader.GetInt32(6);
                            if (dataReader[7] != System.DBNull.Value)
                                reservationInfo.statusCode = (int)dataReader.GetInt32(7);
                            reservationInfos.Add(reservationInfo);
                            

                        }
                        dataReader.Close();
                    }
                }

                catch (Exception ex)
                {
                    throw new Exception("Exception thrown in get reservationInfo", ex);
                }
                finally
                {
                    connection.Close();
                }
            }
			return reservationInfos.ToArray();
		}


        public int RevokeReservations(int[] ids, string message)
        {
            int count = 0;
            ReservationInfo[] info = GetReservationInfos(ids);
            if (info != null & info.Length > 0)
            {
                foreach (ReservationInfo ri in info)
                {
                    count += RevokeReservation(ri, message);
                }
            }
            return count;
        }
        /* TO DO */
        public int RevokeReservation(ReservationInfo ri, string message)
        {
            int count = 0;
            LssCredentialSet[] sets = GetCredentialSets(new int[] { ri.credentialSetId });
            LssExperimentInfo[] exps = GetExperimentInfos(new int[] { ri.experimentInfoId });
            if (sets != null && sets.Length > 0 && exps != null && exps.Length > 0)
            {

                USSInfo uss = GetUSSInfo(ri.ussId);
                if (uss != null)
                {
   
                    UserSchedulingProxy ussProxy = new UserSchedulingProxy();
                    OperationAuthHeader header = new OperationAuthHeader();
                    header.coupon = GetCoupon(uss.revokeCouponId, uss.domainGuid);
                    ussProxy.OperationAuthHeaderValue = header;
                    ussProxy.Url = uss.ussUrl;

                    int num = ussProxy.RevokeReservation(sets[0].serviceBrokerGuid, sets[0].groupName,
                        exps[0].labServerGuid, exps[0].labClientGuid, ri.Start, ri.End, message);
                    if (num > 0)
                    {
                        RemoveReservationInfoByIDs(new int[] { ri.reservationInfoId });
                        count += num;
                    }
                }
            }

            return count;
        }


        public IntTag[] ListReservationTags(string labServerGuid, DateTime start, DateTime end, CultureInfo culture, int userTZ)
        {
            string temp = culture.DateTimeFormat.ShortDatePattern;
             if (!temp.Contains("MM"))
                 temp = temp.Replace("M", "MM");
             if (!temp.Contains("dd"))
                 temp = temp.Replace("d", "dd");
            string dateF = temp + " HH" + culture.DateTimeFormat.TimeSeparator + "mm";
            List<IntTag> tags = new List<IntTag>();
            DbConnection connection = FactoryDB.GetConnection();

            // create sql command
            // command executes the "Reserva" stored procedure
            DbCommand cmd = FactoryDB.CreateCommand("ReservationTags_RetrieveByLabServer", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(FactoryDB.CreateParameter("@guid", labServerGuid,DbType.AnsiString, 50));
            if(start == DateTime.MinValue)
                cmd.Parameters.Add(FactoryDB.CreateParameter("@start",null, DbType.DateTime));
            else
               cmd.Parameters.Add(FactoryDB.CreateParameter("@start",start, DbType.DateTime));
            if (end == DateTime.MinValue)
                cmd.Parameters.Add(FactoryDB.CreateParameter("@end", null,DbType.DateTime));
            else
                cmd.Parameters.Add(FactoryDB.CreateParameter("@end", end, DbType.DateTime));
            DbDataReader dataReader = null;
            try
            {
                connection.Open();
                dataReader = cmd.ExecuteReader();
                tags = ParseReservationTags(dataReader, culture, userTZ);
            }
            catch {
                throw;
            }
            finally
            {
                connection.Close();
            }
            return tags.ToArray();
        }

       
         public IntTag[] ListReservationTags(int resourceId, int expId, int credId, DateTime start, DateTime end, CultureInfo culture, int userTZ)
         {
            
            List<IntTag> tags = null;
            DbConnection connection = FactoryDB.GetConnection();

            // create sql command
            // command executes the "Reserva" stored procedure
            DbCommand cmd = FactoryDB.CreateCommand("ReservationTags_Retrieve", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            if (expId < 1)
                cmd.Parameters.Add(FactoryDB.CreateParameter( "@resourceid", null, DbType.Int32));
            else
                cmd.Parameters.Add(FactoryDB.CreateParameter( "@resourceid", resourceId, DbType.Int32));
             if(expId <1)
                 cmd.Parameters.Add(FactoryDB.CreateParameter("@expid", null, DbType.Int32));
             else
                cmd.Parameters.Add(FactoryDB.CreateParameter("@expid", expId, DbType.Int32));
             if(credId < 1)
                 cmd.Parameters.Add(FactoryDB.CreateParameter("@credid", null, DbType.Int32));
             else
               cmd.Parameters.Add(FactoryDB.CreateParameter("@credid",credId, DbType.Int32));
            if(start == DateTime.MinValue)
                cmd.Parameters.Add(FactoryDB.CreateParameter("@start", null, DbType.DateTime));
            else
                cmd.Parameters.Add(FactoryDB.CreateParameter("@start", start, DbType.DateTime));
            if (end == DateTime.MinValue)
                 cmd.Parameters.Add(FactoryDB.CreateParameter("@end", null, DbType.DateTime));
            else
                 cmd.Parameters.Add(FactoryDB.CreateParameter("@end", end, DbType.DateTime));
            DbDataReader dataReader = null;
            try
            {
                connection.Open();
                dataReader = cmd.ExecuteReader();
                tags = ParseReservationTags(dataReader, culture, userTZ);
               
            }
            catch {
                throw;
            }
            finally
            {
                connection.Close();
            }
            return tags.ToArray();
        }
        private List<IntTag> ParseReservationTags(DbDataReader dataReader, CultureInfo culture, int userTZ)
        {
            string temp = culture.DateTimeFormat.ShortDatePattern;
            if (temp.Contains("MM"))
                ;
            else
                temp = temp.Replace("M", "MM");
            if (temp.Contains("dd"))
                ;
            else
                temp = temp.Replace("d", "dd");
            string dateF = temp + " HH" + culture.DateTimeFormat.TimeSeparator + "mm";
            List<IntTag> tags = new List<IntTag>();
            while (dataReader.Read())
            {
                //t R.Reservation_Info_ID,R.Start_Time, R.End_Time, E.Lab_Client_Name, C.Group_Name,C.Service_Broker_Name
                IntTag t = new IntTag();
                StringBuilder buf = new StringBuilder();
                t.id = dataReader.GetInt32(0);
                buf.Append(DateUtil.ToUserTime(DateUtil.SpecifyUTC(dataReader.GetDateTime(1)), culture, userTZ, dateF) + " - ");
                buf.Append(DateUtil.ToUserTime(DateUtil.SpecifyUTC(dataReader.GetDateTime(2)), culture, userTZ, dateF) + " ");
                buf.Append(dataReader.GetString(3) + " ");
                buf.Append(dataReader.GetString(4) + ":");
                buf.Append(dataReader.GetString(5));
                //buf.Append(" " +dataReader.GetInt32(6));
                t.tag = buf.ToString();
                tags.Add(t);
            }
            return tags;
        }

		/* !------------------------------------------------------------------------------!
			 *							CALLS FOR CredentialSet
			 * !------------------------------------------------------------------------------!
			 */
		/// <summary>
		/// add a credential set of a particular group
		/// </summary>
        /// <param name="serviceBrokerGuid"></param>
		/// <param name="groupName"></param>
        /// <param name="ussGuid"></param>
		/// <returns></returns>the unique ID which identifies the credential set added.>0 was successfully added,-1 otherwise
        public int AddCredentialSet(string serviceBrokerGuid, string serviceBrokerName, string groupName)
		{
			//create a connection
			DbConnection connection= FactoryDB.GetConnection();
			//create a command
            DbCommand cmd = FactoryDB.CreateCommand("CredentialSet_GetID", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            //populate the parameters
            cmd.Parameters.Add(FactoryDB.CreateParameter("@serviceBrokerGUID", serviceBrokerGuid, DbType.AnsiString, 50));
            cmd.Parameters.Add(FactoryDB.CreateParameter("@groupName", groupName, DbType.String, 256));
            int i = -1;
            try
            {
                connection.Open();
                // execute the command
                try
                {

                    Object ob = cmd.ExecuteScalar();
                    if (ob != null && ob != System.DBNull.Value)
                    {
                        i = Convert.ToInt32(ob);
                    }
                    if (i > 0)
                    {
                        return 0;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Exception thrown in GetCredentialSetID", ex);
                }
                
                //command executes the "AddCredentialSet" store procedure
                DbCommand cmd2 = FactoryDB.CreateCommand("CredentialSet_Add", connection);
                cmd2.CommandType = CommandType.StoredProcedure;

                //populate the parameters
                cmd2.Parameters.Add(FactoryDB.CreateParameter("@serviceBrokerGUID", serviceBrokerGuid, DbType.AnsiString, 50));
                cmd2.Parameters.Add(FactoryDB.CreateParameter("@serviceBrokerName", serviceBrokerName, DbType.String, 256));
                cmd2.Parameters.Add(FactoryDB.CreateParameter("@groupName", groupName, DbType.String, 256));

               
                // execute the command
                try
                {
                    Object ob = cmd2.ExecuteScalar();
                    if (ob != null && ob != System.DBNull.Value)
                    {
                        i = Int32.Parse(ob.ToString());
                        if(i > 0)
                            i = 1;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Exception thrown in insert credential set", ex);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception thrown in add credential set", ex);
            }
            finally
            {
                connection.Close();
            }	
			return i;		
		}

        
		/// <summary>
		/// Updates the data fields for the credential set specified by the credentialSetID; note credentialSetID may not be changed 
		/// </summary>
		/// <param name="credentialSetID"></param>
        /// <param name="serviceBrokerGuid"></param>
		/// <param name="serviceBrokerName"></param>
		/// <param name="groupName"></param>
        /// <param name="ussGuid"></param>
		/// <returns></returns>if modified successfully, false otherwise
        public int ModifyCredentialSet(int credentialSetID, string serviceBrokerGuid, string serviceBrokerName, string groupName)
		{
            int status = 0;
			//create a connection
			DbConnection connection= FactoryDB.GetConnection();
			//create a command
			//command executes the "ModifyCredentialSet" store procedure
            DbCommand cmd = FactoryDB.CreateCommand("CredentialSet_Modify", connection);
			cmd.CommandType=CommandType.StoredProcedure;
			//populate the parameters
			cmd.Parameters.Add(FactoryDB.CreateParameter("@credentialSetID", credentialSetID, DbType.Int32));			
			cmd.Parameters.Add(FactoryDB.CreateParameter("@serviceBrokerGUID", serviceBrokerGuid,DbType.AnsiString,50));            
			cmd.Parameters.Add(FactoryDB.CreateParameter("@serviceBrokerName", serviceBrokerName,DbType.String,256));			
			cmd.Parameters.Add(FactoryDB.CreateParameter("@groupName", groupName, DbType.String,256));			
            
			// execute the command
			try
			{
				connection.Open();
				Object ob=cmd.ExecuteScalar();
                if (ob != null && ob != System.DBNull.Value)
				{
					status = Int32.Parse(ob.ToString());
				}
			}
			catch (Exception ex)
			{
                throw new Exception("Exception thrown in  CredentialSet_Modify ", ex);
			}
			finally
			{
				connection.Close();
			}		
			return status;      
		}

        /// <summary>
        /// Updates the data fields for the credential set specified by the credentialSetID; note credentialSetID may not be changed 
        /// </summary>
        /// <param name="credentialSetID"></param>
        /// <param name="serviceBrokerGuid"></param>
        /// <param name="serviceBrokerName"></param>
        /// <param name="groupName"></param>
        /// <param name="ussGuid"></param>
        /// <returns></returns>if modified successfully, false otherwise
        public int ModifyCredentialSetServiceBroker(string originalGuid, string serviceBrokerGuid, string serviceBrokerName)
        {
            int status = 0;
            //create a connection
            DbConnection connection = FactoryDB.GetConnection();
            //create a command
            //command executes the "ModifyCredentialSet" store procedure
            DbCommand cmd = FactoryDB.CreateCommand("CredentialSet_ModifyServiceBroker", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            //populate the parameters

            cmd.Parameters.Add(FactoryDB.CreateParameter("@originalGUID", serviceBrokerGuid,DbType.AnsiString, 50));
            cmd.Parameters.Add(FactoryDB.CreateParameter("@serviceBrokerGUID", serviceBrokerGuid,DbType.AnsiString, 50));
            cmd.Parameters.Add(FactoryDB.CreateParameter("@serviceBrokerName", serviceBrokerName,DbType.String, 256));
            
            // execute the command
            try
            {
                connection.Open();
                Object ob = cmd.ExecuteScalar();
                if (ob != null && ob != System.DBNull.Value)
                {
                    status = Convert.ToInt32(ob);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception thrown in  CredentialSet_ModifyServiceBroker ", ex);
            }
            finally
            {
                connection.Close();
            }
            return status;
        }
		/// <summary>
		///  remove a credential set specified by the credentialsetsIDS
		/// </summary>
		/// <param name="credentialSetIDs"></param>
		/// <returns></returns>An array of ints containing the IDs of all credential sets not successfully removed, i.e., those for which the operation failed. 
		public int[] RemoveCredentialSets(int[] credentialSetIDs)
		{
			ArrayList arrayList=new ArrayList();
			//create a connection
			DbConnection connection= FactoryDB.GetConnection();
			//create a command
			//command executes the "DeleteCredentialSet" store procedure
            DbCommand cmd = FactoryDB.CreateCommand("CredentialSet_Delete", connection);
			cmd.CommandType=CommandType.StoredProcedure;
			cmd.Parameters.Add(FactoryDB.CreateParameter("@credentialSetID", null,DbType.Int32));
			
			// execute the command
			try
			{
				connection.Open();
				//populate the parameters
				foreach(int credentialSetID in credentialSetIDs)
				{
					cmd.Parameters["@credentialSetID"].Value = credentialSetID;
					if (cmd.ExecuteNonQuery()==0)
					{
						arrayList.Add(credentialSetID);
					}
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Exception thrown in remove credential set",ex);
			}
			finally
			{
				connection.Close();
			}	
			int[] uIDs = Utilities.ArrayListToIntArray(arrayList);
			return uIDs;			
		}
        /// <summary>
        /// remove a credential set of a particular group
        /// </summary>
        /// <param name="serviceBrokerGuid"></param>
        /// <param name="groupName"></param>
        /// <param name="ussGuid"></param>
        /// <returns></returns>true, the credentialset is removed successfully, false otherwise
        public int RemoveCredentialSet(string serviceBrokerGuid, string serviceBrokerName, string groupName)
        {
            int status = 0;
            //create a connection
            DbConnection connection = FactoryDB.GetConnection();
            //create a command
            //command executes the "RemoveCredentialSet" store procedure
            DbCommand cmd = FactoryDB.CreateCommand("CredentialSet_Remove", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            //populate the parameters
            cmd.Parameters.Add(FactoryDB.CreateParameter("@serviceBrokerGUID", serviceBrokerGuid,DbType.AnsiString, 50));        
            cmd.Parameters.Add(FactoryDB.CreateParameter("@serviceBrokerName", serviceBrokerName,DbType.String, 256));         
            cmd.Parameters.Add(FactoryDB.CreateParameter("@groupName", groupName,DbType.String, 256));         
           
            // execute the command
            try
            {
                connection.Open();
                status = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Exception thrown in remove credential set", ex);
            }
            finally
            {
                connection.Close();
            }
            return status;
        }
		/// <summary>
		/// Enumerates the IDs of the information of all the credential set 
		/// </summary>
		/// <returns></returns>the array  of ints contains the IDs of all the credential set
		public int[] ListCredentialSetIDs()
		{
			int[] credentialSetIDs;
			// create sql connection
			DbConnection connection = FactoryDB.GetConnection();

			// create sql command
			// command executes the "RetrieveCredentialSetIDs" stored procedure
            DbCommand cmd = FactoryDB.CreateCommand("CredentialSet_RetrieveIDs", connection);
			cmd.CommandType = CommandType.StoredProcedure;
			// execute the command
			DbDataReader dataReader = null;
			ArrayList arrayList=new ArrayList();
			try 
			{
				connection.Open();
				dataReader = cmd.ExecuteReader ();
				//store the credentialSetIDs retrieved in arraylist
				
				while(dataReader.Read ())
				{	
					if(dataReader["Credential_Set_ID"] != System.DBNull.Value )
						arrayList.Add(Convert.ToInt32(dataReader["Credential_Set_ID"]));
				}
				dataReader.Close();
			} 

			catch (Exception ex)
			{
				throw new Exception("Exception thrown in retrieve credentialSetIDs ",ex);
			}
			finally
			{
				connection.Close();
			}
			credentialSetIDs=Utilities.ArrayListToIntArray(arrayList);
			return credentialSetIDs;
		}

/// <summary>
/// Returns an array of the immutable Credential objects that correspond to the supplied credentialSet IDs. 
/// </summary>
/// <param name="credentialSetIDs"></param>
/// <returns></returns>An array of immutable objects describing the specified Credential Set information; if the nth credentialSetID does not correspond to a valid experiment scheduling property, the nth entry in the return array will be null.
		public LssCredentialSet[] GetCredentialSets(int[] credentialSetIDs)
		{
            List<LssCredentialSet> credentialSets = new List<LssCredentialSet>();
			
			// create sql connection
			DbConnection connection = FactoryDB.GetConnection();

			// create sql command
			// command executes the "RetrieveCredentialSetByID" stored procedure
            DbCommand cmd = FactoryDB.CreateCommand("CredentialSet_RetrieveByID", connection);
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add(FactoryDB.CreateParameter("@credentialSetID", null, DbType.Int32));
			//execute the command
			try 
			{
				connection.Open();
				for(int i=0;i<credentialSetIDs.Length;i++)
				{
                   
					// populate the parameters
					cmd.Parameters["@credentialSetID"].Value = credentialSetIDs[i];	
					DbDataReader dataReader = null;
					dataReader=cmd.ExecuteReader();
					while(dataReader.Read())
					{
                        LssCredentialSet credentialSet = new LssCredentialSet();
						credentialSet.credentialSetId=credentialSetIDs[i];
						if(dataReader[0] != System.DBNull.Value )
							credentialSet.serviceBrokerGuid=dataReader.GetString(0);
						if(dataReader[1] != System.DBNull.Value )
							credentialSet.serviceBrokerName=dataReader.GetString(1);
						if(dataReader[2] != System.DBNull.Value )
							credentialSet.groupName=dataReader.GetString(2);
						
                        credentialSets.Add(credentialSet);
					}
					dataReader.Close();
				}	
			} 

			catch (Exception ex)
			{
				throw new Exception("Exception thrown in get credentialSets",ex);
			}
			finally
			{
				connection.Close();
			}	
			return credentialSets.ToArray();
		}

        		
/// <summary>
/// Returns an array of the immutable Credential objects that correspond to the supplied credentialSet IDs. 
/// </summary>
/// <param name="credentialSetIDs"></param>
/// <returns></returns>An array of immutable objects describing the specified Credential Set information; if the nth credentialSetID does not correspond to a valid experiment scheduling property, the nth entry in the return array will be null.
		public LssCredentialSet[] GetCredentialSetsByLS(string lsGuid)
		{
            List<LssCredentialSet> credentialSets = new List<LssCredentialSet>();
			
			// create sql connection
			DbConnection connection = FactoryDB.GetConnection();

			// create sql command
			// command executes the "RetrieveCredentialSetByLS" stored procedure
            DbCommand cmd = FactoryDB.CreateCommand("CredentialSets_RetrieveByLS", connection);
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add(FactoryDB.CreateParameter("@lsGuid", lsGuid, DbType.String,50));
			//execute the command
			try 
			{
				connection.Open();
			
					// populate the parameters
					DbDataReader dataReader = null;
					dataReader=cmd.ExecuteReader();
					while(dataReader.Read())
					{
                        LssCredentialSet credentialSet = new LssCredentialSet();
                        credentialSet.credentialSetId = dataReader.GetInt32(0);
						if(dataReader[1] != System.DBNull.Value )
							credentialSet.serviceBrokerGuid=dataReader.GetString(1);
						if(dataReader[2] != System.DBNull.Value )
							credentialSet.serviceBrokerName=dataReader.GetString(2);
						if(dataReader[3] != System.DBNull.Value )
							credentialSet.groupName=dataReader.GetString(3);
						
                        credentialSets.Add(credentialSet);
					}
					dataReader.Close();
					
			} 

			catch (Exception ex)
			{
				throw new Exception("Exception thrown in get credentialSets",ex);
			}
			finally
			{
				connection.Close();
			}	
			return credentialSets.ToArray();
		}

        /// <summary>
        /// Get a credential set ID of a particular group
        /// </summary>
        /// <param name="serviceBrokerGuid"></param>
        /// <param name="groupName"></param>
        /// <param name="ussGuid"></param>
        /// <returns></returns>the unique ID which identifies the credential set, 0 otherwise
        public int GetCredentialSetID(string serviceBrokerGuid,  string groupName)
        {
            //create a connection
            DbConnection connection = FactoryDB.GetConnection();
            //create a command
            //command executes the "AddCredentialSet" store procedure
            DbCommand cmd = FactoryDB.CreateCommand("CredentialSet_GetID", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            //populate the parameters
            cmd.Parameters.Add(FactoryDB.CreateParameter("@serviceBrokerGUID", serviceBrokerGuid, DbType.AnsiString, 50));
            cmd.Parameters.Add(FactoryDB.CreateParameter("@groupName", groupName, DbType.String, 256));
           
            
            int i = 0;

            // execute the command
            try
            {
                connection.Open();
                Object ob = cmd.ExecuteScalar();
                if (ob != null && ob != System.DBNull.Value)
                {
                    i = Convert.ToInt32(ob);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception thrown in GetCredentialSetID", ex);
            }
            finally
            {
                connection.Close();
            }
            return i;
        }

		/* !------------------------------------------------------------------------------!
			 *							CALLS FOR USSInfo
			 * !------------------------------------------------------------------------------!
			 */
		/// <summary>
		/// Add information of a particular user side scheduling server identified by ussID, theCOupon should have already been Inserted. 
		/// </summary>
        /// <param name="ussGuid"></param>
		/// <param name="ussName"></param>
		/// <param name="ussURL"></param>
        /// <param name="coupon"></param>
		/// <returns></returns>The unique ID which identifies the experiment information added. >0 was successfully added; ==-1 otherwise   
        public int AddUSSInfo(string ussGuid, string ussName, string ussURL,Coupon coupon)
		{
            int i = -1;
			//create a connection
			DbConnection connection= FactoryDB.GetConnection();
            //DbCommand cmd0 = FactoryDB.CreateCommand("InsertCoupon", connection);
            //cmd0.CommandType = CommandType.StoredProcedure;
            //cmd0.Parameters.Add(FactoryDB.CreateParameter("@couponID", coupon.couponId, DbType.Int64));
            //cmd0.Parameters.Add(FactoryDB.CreateParameter("@issuerGUID", coupon.issuerGuid, DbType.AnsiString, 50));
            //cmd0.Parameters.Add(FactoryDB.CreateParameter("@passKey", coupon.passkey, DbType.String, 100));
           
               
            //    cmd0.ExecuteNonQuery();
                //create a command
                //command executes the "AddUSSInfo" store procedure
                DbCommand cmd = FactoryDB.CreateCommand("USSInfo_Add", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                //populate the parameters
                cmd.Parameters.Add(FactoryDB.CreateParameter("@ussGUID", ussGuid, DbType.AnsiString, 50));
                cmd.Parameters.Add(FactoryDB.CreateParameter("@ussName", ussName, DbType.String, 256));
                cmd.Parameters.Add(FactoryDB.CreateParameter("@ussURL", ussURL, DbType.String, 512));
                cmd.Parameters.Add(FactoryDB.CreateParameter("@couponId", coupon.couponId, DbType.Int64));
                cmd.Parameters.Add(FactoryDB.CreateParameter("@domainGuid", coupon.issuerGuid, DbType.AnsiString, 50));



                // execute the command
                try
                {
                    connection.Open();
                Object ob = cmd.ExecuteScalar();
                if (ob != null && ob != System.DBNull.Value)
                {
                    i = Int32.Parse(ob.ToString());
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception thrown in add ussInfo", ex);
            }
			finally
			{
				connection.Close();
			}	
			return i;		
		}

		/// <summary>
		/// Updates the data fields for the USS information specified by the ussInfoID; note ussInfoID may not be changed 
		/// </summary>
		/// <param name="ussInfoID"></param>
        /// <param name="ussGuid"></param>
		/// <param name="ussName"></param>
		/// <param name="ussURL"></param>
		/// <returns></returns>true if modified successfully, false otherwise
        public int ModifyUSSInfo(int ussInfoID, string ussGuid, string ussName, string ussURL,long couponId,string domainGuid)
		{
            int status = 0;
			//create a connection
			DbConnection connection= FactoryDB.GetConnection();
			//create a command
			//command executes the "ModifyUSSInfo" store procedure
            DbCommand cmd = FactoryDB.CreateCommand("USSInfo_Modify", connection);
			cmd.CommandType=CommandType.StoredProcedure;
			//populate the parameters
			cmd.Parameters.Add(FactoryDB.CreateParameter("@ussInfoID", ussInfoID, DbType.Int32));
			cmd.Parameters.Add(FactoryDB.CreateParameter("@ussGUID", ussGuid, DbType.AnsiString,50));
            cmd.Parameters.Add(FactoryDB.CreateParameter("@ussName", ussName, DbType.String,256));
			cmd.Parameters.Add(FactoryDB.CreateParameter("@ussURL", ussURL, DbType.String,512));
			cmd.Parameters.Add(FactoryDB.CreateParameter("@couponId", couponId, DbType.Int64));
            cmd.Parameters.Add(FactoryDB.CreateParameter("@domainGuid", domainGuid, DbType.AnsiString, 50));
            
			// execute the command
			try
			{
				int m=0;
				connection.Open();
				Object ob=cmd.ExecuteScalar();
                if (ob != null && ob != System.DBNull.Value)
				{
					status = Convert.ToInt32(ob);
				}
				
			}
			catch (Exception ex)
			{
				throw new Exception("Exception thrown in ModifyUSSInfo",ex);
			}
			finally
			{
				connection.Close();
			}		
			return status;      
		}
        
		/// <summary>
		/// Delete the uss information specified by the ussInfoIDs. 
		/// </summary>
		/// <param name="ussInfoIDs"></param>
		/// <returns></returns>An array of USS information IDs specifying the USS information to be removed
		public int[] RemoveUSSInfo(int[] ussInfoIDs)
		{
			ArrayList arrayList=new ArrayList();
			//create a connection
			DbConnection connection= FactoryDB.GetConnection();
			//create a command
			//command executes the "DeleteUSSInfo" store procedure
            DbCommand cmd = FactoryDB.CreateCommand("USSInfo_Delete", connection);
			cmd.CommandType=CommandType.StoredProcedure;
			cmd.Parameters.Add(FactoryDB.CreateParameter("@ussInfoID", null, DbType.Int32));
			
			// execute the command
			try
			{
				connection.Open();
				//populate the parameters
				foreach(int ussInfoID in ussInfoIDs)
				{
					cmd.Parameters["@ussInfoID"].Value = ussInfoID;
					if (cmd.ExecuteNonQuery()==0)
					{
						arrayList.Add(ussInfoID);
					}
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Exception thrown in remove ussInfo",ex);
			}
			finally
			{
				connection.Close();
			}	
			int[] uIDs = Utilities.ArrayListToIntArray(arrayList);
			return uIDs;			
		}

		/// <summary>
		/// Enumerates the IDs of the information of all the USS 
		/// </summary>
		/// <returns></returns>the array  of ints contains the IDs of the information of all the USS
		public int[] ListUSSInfoIDs()
		{
			int[] ussInfoIDs;
			// create sql connection
			DbConnection connection = FactoryDB.GetConnection();

			// create sql command
			// command executes the "RetrieveUSSInfoIDs" stored procedure
            DbCommand cmd = FactoryDB.CreateCommand("USSInfo_RetrieveIDs", connection);
			cmd.CommandType = CommandType.StoredProcedure;
			// execute the command
			DbDataReader dataReader = null;
			ArrayList arrayList=new ArrayList();
			try 
			{
				connection.Open();
				dataReader = cmd.ExecuteReader ();
				//store the ussInfoIDs retrieved in arraylist
				
				while(dataReader.Read ())
				{	
					if(dataReader["USS_Info_ID"] != System.DBNull.Value )
						arrayList.Add(Convert.ToInt32(dataReader["USS_Info_ID"]));
				}
				dataReader.Close();
			} 

			catch (Exception ex)
			{
				throw new Exception("Exception thrown in retrieve ussInfoIDs ",ex);
			}
			finally
			{
				connection.Close();
			}
			ussInfoIDs=Utilities.ArrayListToIntArray(arrayList);
			return ussInfoIDs;
		}
		/// <summary>
		/// Enumerates the ID of the information of a particular USS specified by ussID
		/// </summary>
        /// <param name="ussGuid"></param>
		/// <returns></returns>the ID of the information of a particular USS 
        public int ListUSSInfoID(string ussGuid)
		{
			int ussInfoID=-1;
			// create sql connection
			DbConnection connection = FactoryDB.GetConnection();

			// create sql command
			// command executes the "RetrieveUSSInfoID" stored procedure
            DbCommand cmd = FactoryDB.CreateCommand("USSInfo_RetrieveID", connection);
			cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(FactoryDB.CreateParameter( "@ussGuid", ussGuid, DbType.AnsiString, 50));
            
			// execute the command
			
			try 
			{
				connection.Open();
				Object ob=cmd.ExecuteScalar();
				if(ob != null && ob!=System.DBNull.Value)
				{
					ussInfoID=Convert.ToInt32(ob);
				}
				
			} 

			catch (Exception ex)
			{
				throw new Exception("Exception thrown in retrieve ussInfoID",ex);
			}
			finally
			{
				connection.Close();
			}
			return ussInfoID;
		}

        /// <summary>
        /// Returns an array of the immutable USSInfo objects that correspond to the supplied USS information IDs. 
        /// </summary>
        /// <param name="ussInfoIDs"></param>
        /// <returns></returns>An array of immutable objects describing the specified USS information; if the nth ussInfoID does not correspond to a valid experiment scheduling property, the nth entry in the return array will be null
        public USSInfo GetUSSInfo(int id)
        {
            USSInfo ussInfo = null;
         
            // create sql connection
            DbConnection connection = FactoryDB.GetConnection();

            // create sql command
            // command executes the "RetrieveUSSInfoByID" stored procedure
            DbCommand cmd = FactoryDB.CreateCommand("USSInfo_RetrieveByID", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(FactoryDB.CreateParameter("@ussInfoId", id, DbType.Int32));
            
            //execute the command
            try
            {
                connection.Open();
             
                    DbDataReader dataReader = null;
                    dataReader = cmd.ExecuteReader();
                    while (dataReader.Read())
                    {
                        ussInfo = new USSInfo();
                        ussInfo.ussInfoId = id;
                       
                        if (dataReader[0] != System.DBNull.Value)
                            ussInfo.ussGuid = dataReader.GetString(0);
                        if (dataReader[1] != System.DBNull.Value)
                            ussInfo.ussName = dataReader.GetString(1);
                        if (dataReader[2] != System.DBNull.Value)
                            ussInfo.ussUrl = dataReader.GetString(2);
                        if (dataReader[3] != System.DBNull.Value)
                            ussInfo.revokeCouponId = dataReader.GetInt64(3);
                        if (dataReader[4] != System.DBNull.Value)
                            ussInfo.domainGuid = dataReader.GetString(4);

                    }
                    dataReader.Close();
                
            }

            catch (Exception ex)
            {
                throw new Exception("Exception thrown in get ussInfos", ex);
            }
            finally
            {
                connection.Close();
            }
            return ussInfo;
        }

        public USSInfo GetUSSInfo(string guid)
        {
            USSInfo ussInfo = null;

            // create sql connection
            DbConnection connection = FactoryDB.GetConnection();

            // create sql command
            // command executes the "RetrieveUSSInfoByID" stored procedure
            DbCommand cmd = FactoryDB.CreateCommand("USSInfo_RetrieveByGUID", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(FactoryDB.CreateParameter("@guid", guid, DbType.AnsiString, 50));
            
            //execute the command
            try
            {
                connection.Open();

                DbDataReader dataReader = null;
                dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    ussInfo = new USSInfo();
                    if (dataReader[0] != System.DBNull.Value)
                        ussInfo.ussInfoId = dataReader.GetInt32(0);
                    if (dataReader[1] != System.DBNull.Value)
                        ussInfo.ussGuid = dataReader.GetString(1);
                    if (dataReader[2] != System.DBNull.Value)
                        ussInfo.ussName = dataReader.GetString(2);
                    if (dataReader[3] != System.DBNull.Value)
                        ussInfo.ussUrl = dataReader.GetString(3);
                    if (dataReader[4] != System.DBNull.Value)
                        ussInfo.revokeCouponId = dataReader.GetInt64(4);
                    if (dataReader[5] != System.DBNull.Value)
                        ussInfo.domainGuid = dataReader.GetString(5);

                }
                dataReader.Close();

            }

            catch (Exception ex)
            {
                throw new Exception("Exception thrown in get ussInfos", ex);
            }
            finally
            {
                connection.Close();
            }
            return ussInfo;
        }
		
      /// <summary>
      /// Returns an array of the immutable USSInfo objects that correspond to the supplied USS information IDs. 
      /// </summary>
      /// <param name="ussInfoIDs"></param>
      /// <returns></returns>An array of immutable objects describing the specified USS information; if the nth ussInfoID does not correspond to a valid experiment scheduling property, the nth entry in the return array will be null
		public USSInfo[] GetUSSInfos(int[] ussInfoIDs)
		{
            List<USSInfo> ussInfos = new List<USSInfo>();
			
			// create sql connection
			DbConnection connection = FactoryDB.GetConnection();

			// create sql command
			// command executes the "RetrieveUSSInfoByID" stored procedure
            DbCommand cmd = FactoryDB.CreateCommand("USSInfo_RetrieveByID", connection);
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add(FactoryDB.CreateParameter("@ussInfoID", null, DbType.Int32));
			//execute the command
			try 
			{
				connection.Open();
				for(int i=0;i<ussInfoIDs.Length;i++)
				{
					// populate the parameters
					cmd.Parameters["@ussInfoID"].Value = ussInfoIDs[i];	
					DbDataReader dataReader = null;
					dataReader=cmd.ExecuteReader();
					while(dataReader.Read())
                    {
                        USSInfo ussInfo = new USSInfo();
						ussInfo.ussInfoId=ussInfoIDs[i];
						if(dataReader[0] != System.DBNull.Value )
							ussInfo.ussGuid=dataReader.GetString(0);
						if(dataReader[1] != System.DBNull.Value )
							ussInfo.ussName=dataReader.GetString(1);
						if(dataReader[2] != System.DBNull.Value )
							ussInfo.ussUrl=dataReader.GetString(2);
                        if (dataReader[3] != System.DBNull.Value)
                            ussInfo.revokeCouponId = dataReader.GetInt64(3);
                        if (dataReader[4] != System.DBNull.Value)
                            ussInfo.domainGuid = dataReader.GetString(4);
                        ussInfos.Add(ussInfo); 
					}
					dataReader.Close();
				}	
			} 

			catch (Exception ex)
			{
				throw new Exception("Exception thrown in get ussInfos",ex);
			}
			finally
			{
				connection.Close();
			}	
			return ussInfos.ToArray();
		}

        /* !------------------------------------------------------------------------------!
         *							CALLS FOR Recurrence
         * !------------------------------------------------------------------------------!
         */


        public int AddRecurrence(Recurrence recur)
        {
            return AddRecurrence(recur.startDate, recur.numDays, (int)recur.recurrenceType,
                recur.startOffset, recur.endOffset, recur.quantum, recur.resourceId, recur.dayMask);
        }

      /// <summary>
      /// add recurrence
      /// </summary>
      /// <param name="startDate"></param>
      /// <param name="numDays"></param>
      /// <param name="recurrenceType"></param>
      /// <param name="startOffset"></param>
      /// <param name="endOffset"></param>
      /// <param name="quantum"></param>
      /// <param name="resourceId"></param>
      /// <param name="dayMask"></param>
        /// <returns></returns>the uniqueID which identifies the recurrence added, >0 was successfully added; ==-1 otherwise
        public int AddRecurrence(DateTime startDate, int numDays,
            int recurrenceType, TimeSpan startOffset, TimeSpan endOffset, int quantum,
            int resourceId, byte dayMask)
        {
            //create a connection
            DbConnection connection = FactoryDB.GetConnection();
            //create a command
            //command executes the "AddRecurrence" store procedure
            DbCommand cmd = FactoryDB.CreateCommand("Recurrence_Add", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            //populate the parameters
            cmd.Parameters.Add(FactoryDB.CreateParameter("@resourceID", resourceId, DbType.Int32));
            cmd.Parameters.Add(FactoryDB.CreateParameter("@startDate", startDate, DbType.DateTime));
            cmd.Parameters.Add(FactoryDB.CreateParameter("@numDays", numDays, DbType.Int32));
            cmd.Parameters.Add(FactoryDB.CreateParameter("@recurrenceType",recurrenceType, DbType.Int32));
            cmd.Parameters.Add(FactoryDB.CreateParameter("@startOffset", startOffset.TotalSeconds,DbType.Int32));
            cmd.Parameters.Add(FactoryDB.CreateParameter("@endOffset", endOffset.TotalSeconds, DbType.Int32));
            cmd.Parameters.Add(FactoryDB.CreateParameter("@quantum", quantum, DbType.Int32));
            cmd.Parameters.Add(FactoryDB.CreateParameter("@dayMask", dayMask, DbType.Byte));
            
            int i = -1;

            // execute the command
            try
            {
                connection.Open();
                Object ob = cmd.ExecuteScalar();
                if (ob != null && ob != System.DBNull.Value)
                {
                    i = Int32.Parse(ob.ToString());
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception thrown in add recurrence", ex);
            }
            finally
            {
                connection.Close();
            }
            return i;
        }

        /// <summary>
        /// delete the recurrence specified by the recurrenceIDs
        /// </summary>
        /// <param name="recurrenceIDs"></param>
        /// <returns></returns>an array of ints containning the IDs of all recurrence not successfully removed
        public int RemoveRecurrence(int recurrenceID)
        {
            int i = 0;
            //create a connection
            DbConnection connection = FactoryDB.GetConnection();
            //create a command
            //command executes the "DeleteRecurrence" store procedure
            DbCommand cmd = FactoryDB.CreateCommand("Recurrence_Delete", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(FactoryDB.CreateParameter("@recurrenceID", recurrenceID, DbType.Int32));
            
            // execute the command
            try
            {
                connection.Open();
              
                object obj = cmd.ExecuteNonQuery();
                if (obj != null)
                    i = Convert.ToInt32(obj);
            }
            catch (Exception ex)
            {
                throw new Exception("Exception thrown in remove recurrence", ex);
            }
            finally
            {
                connection.Close();
            }
            return i;
        }
        /// <summary>
        /// Returns an array of the immutable Recurrence objects that correspond ot the supplied Recurrence IDs
        /// </summary>
        /// <param name="timeBlockIDs"></param>
        /// <returns></returns>an array of immutable objects describing the specified Recurrence
        public Recurrence[] GetRecurrences(int[] recurrenceIDs)
        {
            Recurrence[] recurrences = new Recurrence[recurrenceIDs.Length];
            for (int i = 0; i < recurrenceIDs.Length; i++)
            {
                recurrences[i] = new Recurrence();
            }
            // create sql connection
            DbConnection connection = FactoryDB.GetConnection();

            // create sql command
            // command executes the "RetrieveRecurrenceByID" stored procedure
            DbCommand cmd = FactoryDB.CreateCommand("Recurrence_RetrieveByID", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(FactoryDB.CreateParameter("@recurrenceID", null, DbType.Int32));
            //execute the command
            try
            {
                connection.Open();
                for (int i = 0; i < recurrenceIDs.Length; i++)
                {
                    // populate the parameters
                    cmd.Parameters["@recurrenceID"].Value = recurrenceIDs[i];
                    DbDataReader dataReader = null;
                    dataReader = cmd.ExecuteReader();
                    while (dataReader.Read())
                    {
                        recurrences[i].recurrenceId = recurrenceIDs[i];
                        if (dataReader[0] != System.DBNull.Value)
                            recurrences[i].startDate = DateUtil.SpecifyUTC(dataReader.GetDateTime(0));
                        if (dataReader[1] != System.DBNull.Value)
                            recurrences[i].numDays = dataReader.GetInt32(1);
                        if (dataReader[2] != System.DBNull.Value)
                            recurrences[i].recurrenceType = (Recurrence.RecurrenceType) dataReader.GetInt32(2);
                        if (dataReader[3] != System.DBNull.Value)
                        {
                            recurrences[i].startOffset = TimeSpan.FromSeconds((double)dataReader.GetInt32(3));
                        }
                        if (dataReader[4] != System.DBNull.Value)
                            recurrences[i].endOffset = TimeSpan.FromSeconds((double)dataReader.GetInt32(4));
                        if (dataReader[5] != System.DBNull.Value)
                            recurrences[i].quantum = dataReader.GetInt32(5);
                        
                        if (dataReader[6] != System.DBNull.Value)
                            recurrences[i].resourceId = (int)dataReader.GetInt32(6);
                        if (dataReader[7] != System.DBNull.Value)
                            recurrences[i].dayMask = dataReader.GetByte(7);
                    }
                    dataReader.Close();
                }
            }

            catch (Exception ex)
            {
                throw new Exception("Exception thrown in get recurrence", ex);
            }
            finally
            {
                connection.Close();
            }
            return recurrences;
        }

        public Recurrence[] GetRecurrences(string serviceBrokerGuid, string groupName,
                    string labServerGuid, string clientGuid, DateTime startTime, DateTime endTime)
        {
            TimeBlock range = new TimeBlock(startTime, endTime);
            List<Recurrence> recurrences = new List<Recurrence>();
            DbConnection connection = FactoryDB.GetConnection();

            // create sql command
            // command executes the "RetrieveRecurrenceIDs" stored procedure
            DbCommand cmd = FactoryDB.CreateCommand("Recurrences_Retrieve", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(FactoryDB.CreateParameter("@sbGuid", serviceBrokerGuid, DbType.AnsiString, 50));
            cmd.Parameters.Add(FactoryDB.CreateParameter("@group", groupName, DbType.String, 256));
            cmd.Parameters.Add(FactoryDB.CreateParameter("@lsGuid", labServerGuid, DbType.AnsiString, 50));
            cmd.Parameters.Add(FactoryDB.CreateParameter("@clientGuid", clientGuid, DbType.AnsiString, 50));
            cmd.Parameters.Add(FactoryDB.CreateParameter("@start", DateUtil.SpecifyUTC(startTime), DbType.DateTime));
            cmd.Parameters.Add(FactoryDB.CreateParameter("@end", DateUtil.SpecifyUTC(endTime), DbType.DateTime));
            
            try
            {
                DbDataReader dataReader = null;
                Recurrence recur = null;
                connection.Open();

                //recurrence_id,resource_id,recurrence_type,day_mask,recurrence_start_date,recurrence_num_days
                //recurrence_start_offset,recurrence_end_offset, quantum
                dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    recur = new Recurrence();
                    recur.recurrenceId = dataReader.GetInt32(0);
                    recur.resourceId = dataReader.GetInt32(1);
                    recur.recurrenceType = (Recurrence.RecurrenceType)dataReader.GetInt32(2);
                    recur.dayMask = dataReader.GetByte(3);
                    recur.startDate = DateUtil.SpecifyUTC(dataReader.GetDateTime(4));
                    recur.numDays = dataReader.GetInt32(5);
                    recur.startOffset = TimeSpan.FromSeconds((double)dataReader.GetInt32(6));
                    recur.endOffset = TimeSpan.FromSeconds((double)dataReader.GetInt32(7));
                    recur.quantum = dataReader.GetInt32(8);
                    TimeBlock[] blocks = recur.GetTimeBlocks();
                    foreach (TimeBlock tb in blocks)
                    {
                        if (range.Intersects(tb))
                        {
                            recurrences.Add(recur);
                            break;
                        }
                    }
                }
                dataReader.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Exception thrown in get recurrences", ex);
            }
            finally
            {
                connection.Close();
            }
            return recurrences.ToArray();
        }

        /// Enumerates the IDs of the information of all the Recurrence 
        /// </summary>
        /// <returns></returns>the array  of ints contains the IDs of the information of all the Recurrence
        public int[] ListRecurrenceIDs()
        {
            int[] recurrenceIDs;
            // create sql connection
            DbConnection connection = FactoryDB.GetConnection();

            // create sql command
            // command executes the "RetrieveRecurrenceIDs" stored procedure
            DbCommand cmd = FactoryDB.CreateCommand("Recurrence_RetrieveIDs", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            // execute the command
            DbDataReader dataReader = null;
            ArrayList arrayList = new ArrayList();
            try
            {
                connection.Open();
                dataReader = cmd.ExecuteReader();
                //store the timeBlockIDs retrieved in arraylist

                while (dataReader.Read())
                {
                    if (dataReader["Recurrence_ID"] != System.DBNull.Value)
                        arrayList.Add(Convert.ToInt32(dataReader["Recurrence_ID"]));
                }
                dataReader.Close();
            }

            catch (Exception ex)
            {
                throw new Exception("Exception thrown in retrieve recurrenceIDs ", ex);
            }
            finally
            {
                connection.Close();
            }
            recurrenceIDs = Utilities.ArrayListToIntArray(arrayList);
            return recurrenceIDs;
        }

        /*
        /// <summary>
        /// enumerates all IDs of the recurrences belonging to a particular lab server identified by the labserverID
        /// </summary>
        /// <param name="labServerGuid"></param>
        /// <returns></returns>an array of ints containing the IDs of all the time blocks of specified lab server
        public int[] ListRecurrenceIDsByLabServer(string labServerGuid)
        {
            int[] recurIDs;
            // create sql connection
            DbConnection connection = FactoryDB.GetConnection();

            // create sql command
            // command executes the "RetrieveRecurrenceIDsByLabServer" stored procedure
            DbCommand cmd = FactoryDB.CreateCommand("RetrieveRecurrenceIDsByLabServer", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            // populate the parameters
            DbParameter labServerIDParam = cmd.Parameters.Add(FactoryDB.CreateParameter("@labServerGUID", DbType.AnsiString, 50);
            labServerIDParam.Value = labServerGuid;

            // execute the command
            DbDataReader dataReader = null;
            ArrayList arrayList = new ArrayList();
            try
            {
                connection.Open();
                dataReader = cmd.ExecuteReader();
                //store the timeBlockIDs retrieved in arraylist

                while (dataReader.Read())
                {
                    if (dataReader["Recurrence_ID"] != System.DBNull.Value)
                        arrayList.Add(Convert.ToInt32(dataReader["Recurrence_ID"]));
                }
                dataReader.Close();
            }


            catch (Exception ex)
            {
                throw new Exception("Exception thrown in retrieve recurrenceIDs by lab server", ex);
            }
            finally
            {
                connection.Close();
            }
            recurIDs = Utilities.ArrayListToIntArray(arrayList);
            return recurIDs;
        }

        /// <summary>
        /// enumerates all IDs of the recurrences belonging to a particular lab server identified by the labserverID
        /// </summary>
        /// <param name="labServerGuid"></param>
        /// <returns></returns>an array of ints containing the IDs of all the time blocks of specified lab server
        public int[] ListRecurrenceIDsByLabServer(DateTime start, DateTime end, string labServerGuid)
        {
            int[] recurIDs;
            // create sql connection
            DbConnection connection = FactoryDB.GetConnection();

            // create sql command
            // command executes the "RetrieveRecurrenceIDsByLabServer" stored procedure
            DbCommand cmd = FactoryDB.CreateCommand("RetrieveRecurrenceIDsByLabServerAndTime", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            // populate the parameters
            DbParameter labServerIDParam = cmd.Parameters.Add(FactoryDB.CreateParameter("@labServerGUID", DbType.AnsiString, 50);
            labServerIDParam.Value = labServerGuid;
            DbParameter startParam = cmd.Parameters.Add(FactoryDB.CreateParameter("@start", DbType.DateTime);
            startParam.Value = start;
            DbParameter endParam = cmd.Parameters.Add(FactoryDB.CreateParameter("@end", DbType.DateTime);
            endParam.Value = end;

            // execute the command
            DbDataReader dataReader = null;
            ArrayList arrayList = new ArrayList();
            try
            {
                connection.Open();
                dataReader = cmd.ExecuteReader();
                //store the timeBlockIDs retrieved in arraylist

                while (dataReader.Read())
                {
                    if (dataReader["Recurrence_ID"] != System.DBNull.Value)
                        arrayList.Add(Convert.ToInt32(dataReader["Recurrence_ID"]));
                }
                dataReader.Close();
            }


            catch (Exception ex)
            {
                throw new Exception("Exception thrown in retrieve recurrenceIDs by lab server", ex);
            }
            finally
            {
                connection.Close();
            }
            recurIDs = Utilities.ArrayListToIntArray(arrayList);
            return recurIDs;
        }
         * */

        /// <summary>
        /// enumerates all IDs of the recurrences belonging to a particular lab server identified by the labserverID
        /// </summary>
        /// <param name="labServerGuid"></param>
        /// <returns></returns>an array of ints containing the IDs of all the time blocks of specified lab server
        public int[] ListRecurrenceIDsByResourceID(DateTime start, DateTime end, int resourceID)
        {
            int[] recurIDs;
            // create sql connection
            DbConnection connection = FactoryDB.GetConnection();

            // create sql command
            // command executes the "RetrieveRecurrenceIDsByLabServer" stored procedure
            DbCommand cmd = FactoryDB.CreateCommand("Recurrence_RetrieveIDsByResourceAndTime", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            // populate the parameters
            cmd.Parameters.Add(FactoryDB.CreateParameter("@resourceID", resourceID, DbType.Int32));
            cmd.Parameters.Add(FactoryDB.CreateParameter("@start", DateUtil.SpecifyUTC(start), DbType.DateTime));
            cmd.Parameters.Add(FactoryDB.CreateParameter("@end", DateUtil.SpecifyUTC(end), DbType.DateTime));
            
            // execute the command
            DbDataReader dataReader = null;
            ArrayList arrayList = new ArrayList();
            try
            {
                connection.Open();
                dataReader = cmd.ExecuteReader();
                //store the timeBlockIDs retrieved in arraylist

                while (dataReader.Read())
                {
                    if (dataReader["Recurrence_ID"] != System.DBNull.Value)
                        arrayList.Add(Convert.ToInt32(dataReader["Recurrence_ID"]));
                }
                dataReader.Close();
            }


            catch (Exception ex)
            {
                throw new Exception("Exception thrown in retrieve recurrenceIDs by lab server", ex);
            }
            finally
            {
                connection.Close();
            }
            recurIDs = Utilities.ArrayListToIntArray(arrayList);
            return recurIDs;
        }

        /// <summary>
        /// retrieve available time periods(local time of LSS) overlaps with a time chrunk for a particular group 
        /// and particular experiment, so that we get the a serials available time periods
        /// which is the minumum available time periods set covering the time chunk
        /// </summary>
        /// <param name="serviceBrokeGUID"></param>
        /// <param name="groupName"></param>
        /// <param name="ussGUID"></param>
        /// <param name="labServerGuid"></param>
        /// <param name="clientGuid"></param>
        /// <param name="startTime"></param>the local time of LSS
        /// <param name="endTime"></param>the local time of LSS
        /// <returns></returns>return an array of time periods, each of the time periods is longer than the experiment's minimum time 
        public TimePeriod[] RetrieveAvailableTimePeriods(string serviceBrokerGUID, string groupName, string ussGUID,
             string labServerGuid, string clientGuid, DateTime startTime, DateTime endTime)
        {
            List<TimePeriod> timePeriods = null;
            TimeBlock[] returnBlocks = null;
            int minTime = 1;
            int quantumMax = 1;
            int resourceID = 0;
            LssExperimentInfo exInfo = null;
            try
            {
                LssExperimentInfo[] exInfos = null;
                //get the experiment configuration
                int id = ListExperimentInfoIDByExperiment(labServerGuid, clientGuid);
                if (id > 0)
                    exInfos = GetExperimentInfos(new int[] { id });
                if (exInfos != null && exInfos.Length > 0)
                    exInfo = exInfos[0];

                //LssExperimentInfo exInfo = ListLssIdByExperimentGetExperimentInfo(serviceBrokerGUID, groupName, clientGuid,
                //    labServerGuid, startTime, endTime);

                Recurrence[] recurrences = GetRecurrences(serviceBrokerGUID, groupName,
                    labServerGuid, clientGuid, startTime, endTime);
                if (exInfo == null || recurrences == null)
                {
                    return null;
                }
                minTime = (exInfo.prepareTime + exInfo.minimumTime + exInfo.recoverTime) * 60;

                List<TimeBlock> recurBlocks = new List<TimeBlock>();
                foreach (Recurrence rec in recurrences)
                {
                    if (resourceID == 0)
                    {
                        resourceID = rec.resourceId;
                    }
                    else if (resourceID != rec.resourceId)
                    {
                        throw new Exception(" multiple resources returned from get Recurrences");
                    }

                    TimeBlock[] recurTbs = rec.GetTimeBlocks(startTime.AddMinutes(-exInfo.prepareTime), endTime.AddMinutes(exInfo.recoverTime));

                    if (recurTbs != null)
                    {
                        quantumMax = Math.Max(quantumMax, rec.quantum);
                        recurBlocks.AddRange(recurTbs);
                    }
                }

                TimeBlock[] reservationTbs = ListReservationTimeBlocks(resourceID, startTime.AddMinutes(-exInfo.prepareTime),
                        endTime.AddMinutes(exInfo.recoverTime));
                if (reservationTbs != null)
                {
                    returnBlocks = TimeBlock.Remaining(recurBlocks.ToArray(), reservationTbs);
                    returnBlocks = TimeBlock.Concatenate(returnBlocks);
                }
                else
                {
                    returnBlocks = recurBlocks.ToArray(); ;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("exception thrown in retrieving available time periods" + ex.Message);
            }
            if (returnBlocks != null)
            {
                timePeriods = new List<TimePeriod>();
                foreach (TimeBlock t in returnBlocks)
                {
                    if (t.duration >= minTime)
                    {
                        TimePeriod tp = new TimePeriod(t.startTime.AddMinutes(exInfo.prepareTime), t.End.AddMinutes(-exInfo.recoverTime));
                        tp.quantum = quantumMax;
                        timePeriods.Add(tp);
                    }
                }
                timePeriods.Sort();
            }
            if (timePeriods != null && timePeriods.Count > 0)
                return timePeriods.ToArray();
            else
                return null;
        }

        public TimeBlock[] GetAvailableTimePeriods(string serviceBrokerGuid, string groupName, string ussGuid,
           string labServerGuid, string clientGuid,  DateTime startTime, DateTime endTime)
        {
            List<Recurrence> recList = new List<Recurrence>();
            List<TimeBlock> tpList = new List<TimeBlock>();
            // create sql connection
            DbConnection connection = FactoryDB.GetConnection();

            // create sql command
            // command executes the "RetrieveRecurrences" stored procedure
            DbCommand cmd = FactoryDB.CreateCommand("Recurrences_Retrieve", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(FactoryDB.CreateParameter("@sbGUID", serviceBrokerGuid, DbType.AnsiString, 50));
            cmd.Parameters.Add(FactoryDB.CreateParameter("@group", groupName, DbType.String, 256));
            //cmd.Parameters.Add(FactoryDB.CreateParameter("@ussGUID", ussGuid, DbType.AnsiString, 50));
            cmd.Parameters.Add(FactoryDB.CreateParameter("@clientGuid", clientGuid, DbType.AnsiString, 50));
            cmd.Parameters.Add(FactoryDB.CreateParameter("@lsGuid", labServerGuid, DbType.AnsiString, 50));
            cmd.Parameters.Add(FactoryDB.CreateParameter("@start", DateUtil.SpecifyUTC(startTime), DbType.DateTime));
            cmd.Parameters.Add(FactoryDB.CreateParameter("@end", DateUtil.SpecifyUTC(endTime), DbType.DateTime));
            
            DbDataReader dataReader = null;
            try
            {
                connection.Open();
                dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    Recurrence rec = new Recurrence();
                    if (dataReader[1] != System.DBNull.Value)
                        rec.recurrenceId = (int)dataReader.GetInt32(1);
                    if (dataReader[2] != System.DBNull.Value)
                        rec.resourceId = (int)dataReader.GetInt32(2);
                    if (dataReader[3] != System.DBNull.Value)
                        rec.recurrenceType = (Recurrence.RecurrenceType)dataReader.GetInt32(3);
                    if (dataReader[4] != System.DBNull.Value)
                        rec.dayMask = dataReader.GetByte(4);
                    if (dataReader[5] != System.DBNull.Value)
                        rec.startDate = DateUtil.SpecifyUTC(dataReader.GetDateTime(5));
                    if (dataReader[6] != System.DBNull.Value)
                        rec.numDays = dataReader.GetInt32(6);
                    if (dataReader[7] != System.DBNull.Value)
                        rec.startOffset = TimeSpan.FromSeconds(dataReader.GetInt32(7));
                    if (dataReader[8] != System.DBNull.Value)
                        rec.endOffset = TimeSpan.FromSeconds(dataReader.GetInt32(8));
                    if (dataReader[9] != System.DBNull.Value)
                        rec.quantum = dataReader.GetInt32(9);
                    recList.Add(rec);
                }
                connection.Close();
            }
            catch (Exception e)
            {
                throw;
            }
            if (recList.Count > 0)
            {
                return tpList.ToArray();
            }
            else return null;


        }
	
	}
}

