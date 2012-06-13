/*
 * Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
 * Please see license.txt in top level directory for full license.
 */

/* $Id$ */

using System;
using System.Data ;
using System.Data.Common ;
using System.Configuration ;
using System.Collections ;
using System.Collections.Generic;
using System.Text;

using iLabs.Core;
using iLabs.ServiceBroker;
using iLabs.ServiceBroker.Administration;
using iLabs.ServiceBroker.Authorization;
//using iLabs.ServiceBroker.DataStorage;
using iLabs.Ticketing;
using iLabs.UtilLib;

namespace iLabs.ServiceBroker.Internal
{
	/// <summary>
	/// Summary description for InternalAuthorizationDB.
	/// </summary>
	public class InternalAuthorizationDB
	{
	
		public InternalAuthorizationDB()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		/* !------------------------------------------------------------------------------!
		 *							CALLS FOR GRANTS
		 * !------------------------------------------------------------------------------!
		 */
		/// <summary>
		/// to add a grant - returns the grantID
		/// </summary>
		public static int InsertGrant(Grant grant)
		{
			int grantID=-1;

			DbConnection myConnection = FactoryDB.GetConnection();
			DbCommand myCommand = FactoryDB.CreateCommand("Grant_Insert", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;

            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@agentID", grant.agentID, DbType.Int32));
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@functionName", grant.function, DbType.AnsiString,128));
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@qualifierID", grant.qualifierID, DbType.Int32));
			
			try
			{
				myConnection.Open();
				grantID = Convert.ToInt32 ( myCommand.ExecuteScalar());
			}
			catch (Exception ex)
			{
				throw new Exception("Exception thrown in InsertGrant. " + ex.Message, ex);
			}
			finally
			{
				myConnection.Close();
			}

			//Update Grant cache
			AuthCache.GrantSet = InternalAuthorizationDB.RetrieveGrants();
			return grantID;
		}

		/// <summary>
		/// to remove a set of grants
		/// </summary>
		public static int[] DeleteGrants(int[] grantIDs)
		{

			DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("Grant_Delete", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@grantID", null, DbType.Int32));

			/*
			 * Note : Alternately ADO.NET could be used. However, the disconnected  DataAdapter object might prove
			 * extremely inefficient and hence this method was chosen
			 */
            bool status = false;
			ArrayList arrayList = new ArrayList ();

			try 
			{
				myConnection.Open ();
											
				foreach (long grantID in grantIDs) 
				{
					// Deleting from table Grants
					
					myCommand.Parameters["@grantID"].Value = grantID;
					if(myCommand.ExecuteNonQuery () <= 0)
					{
						//grants that were not removed
						arrayList.Add(grantID);
					}
					else
					{
                        //need to update GrantSet.
                        status = true;
                        
					}
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Exception thrown in DeleteGrants. " + ex.Message, ex);
			}
			finally 
			{
				myConnection.Close ();
                if (status) //update GrantSet.
                    AuthCache.GrantSet = InternalAuthorizationDB.RetrieveGrants();
			}

			int[] gIDs = Utilities.ArrayListToIntArray(arrayList);
			
			return gIDs;
			
		}

	
		/* !------------------------------------------------------------------------------!
		 *	CALLS FOR GRANTS - DATASETS (Contains Dataset versions of some of the above methods)
		 * !------------------------------------------------------------------------------!
		 */
	
		/// <summary>
		/// returns the Grants table in a dataset
		/// </summary>
		public static DataSet RetrieveGrants()
		{
			DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("Grants_RetrieveTable", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;
			DataSet ds = new DataSet();

			try
			{
             
				DbDataAdapter dataAdapter = FactoryDB.CreateDataAdapter();
				dataAdapter.SelectCommand = myCommand;
				dataAdapter.TableMappings.Add("Table","Grants");
				dataAdapter.Fill(ds);
			}
			catch (Exception ex)
			{
				throw new Exception("Exception thrown in GetGrantsTable. " + ex.Message, ex);
			}
			finally
			{
				myConnection.Close();
			}
			return ds;
		}

 
		/// <summary>
		/// lists the IDs of all grants registered with the Service Broker
		/// </summary>
		/// <returns>an array of IDs for all registered grants</returns>
		public static int[] ListGrantIDsFromDS ()
		{
			DataTable grantsTable = AuthCache.GrantSet.Tables[0];
			ArrayList grantIDsList = new ArrayList();
	
			foreach(DataRow dataRow in grantsTable.Rows)
			{
				grantIDsList.Add(dataRow["grant_id"]);
			}

			return Utilities.ArrayListToIntArray(grantIDsList);
		}

		/// <summary>
		/// returns an array of the immutable Grant objects for the registered grants whose IDs are supplied in grantIDs
		/// </summary>
		/// <param name="grantIDs">returns an array of the immutable Grant objects for the registered grants whose IDs are supplied in grantIDs</param>
		/// <returns>an array of Grant objects describing the registered grants specified in grantIDs; if the nth grant ID does not correspond to a grant, the nth entry in the return array will be null</returns>

		/* Check to see if converting all the nulls to -1's work. 
		 * Also make sure the correct datarow from the table is taken as in 
		 * function_name and not function_id - CV 12/19/2004*/
		public static Grant[] GetGrantsFromDS ( int[] grantIDs )
		{
			DataTable grantsTable = AuthCache.GrantSet.Tables[0];
			ArrayList grantList = new ArrayList();
				
			foreach(int grantID in grantIDs)
			{
				bool grantIDFound= false;
				foreach(DataRow dataRow in grantsTable.Rows)
				{
					if(Convert.ToInt32(dataRow["grant_id"])== grantID)
					{
						grantIDFound = true;
						Grant g = new Grant();
						g.grantID = Convert.ToInt32(dataRow["grant_id"]);
						g.agentID = Convert.ToInt32(dataRow["agent_id"]);
						g.function = dataRow["function_name"].ToString();
						g.qualifierID = Convert.ToInt32(dataRow["qualifier_id"]);

						grantList.Add(g);

						break;
					}
				}
				if(!grantIDFound)
				{
					Grant g = new Grant();
					g.grantID = -1;
					g.agentID = -1;
					g.function = null;
					g.qualifierID = -1;

					grantList.Add(g);
				}
			}

			// Convert to a grants array
			Grant[] getGrants = new Grant[grantIDs.Length];

			for(int i = 0; i<grantIDs.Length; i++)
			{
				Grant g = (Grant) grantList[i];
				getGrants[i] = (Grant) grantList[i];
			}

			return getGrants;
		}

		/// <summary>
		/// enumerates the IDs of all explicit grants matching the specification of the arguments; any or all of the arguments may be null, in which case, 
		/// the method does a wildcard match on the null argument(s). 
		/// (A FindGrants() with all the arguments null is equivalent to a ListGrantIDs().
		/// This uses the cached GrantSet, without updating it first.
		/// Current implementation assumes that all wildcard searches do not yield implicit grants
		/// </summary>
		/// <param name="agentID">the ID of the agent for which matching grants are sought, wildcard value is -1;</param>
		/// <param name="function">the Function for which matching grants are sought; function must be one of the fixed set of FunctionTypes recognized by the Service Broker, wildcard is null;</param>
		/// <param name="qualifierID">the ID of the Qualifier (associated with the function, if the FunctionType requires one; otherwise null) for which matching grants are sought, wildcard value is -1.</param>
		/// <returns>an array of grantIDs of any grants that match the specification.</returns>

		public static int[] FindGrantsIDsFromDS ( int agentID, string function, int qualifierID )
		{
			DataTable GrantsTable = AuthCache.GrantSet.Tables[0];
			StringBuilder sb = new StringBuilder();
			if(agentID != -1)
			{
				sb.Append("agent_id = " + agentID);
			}
			if(!((function == null)||function.Equals("")))
			{
				if(sb.Length != 0)
					sb.Append(" AND ");
				sb.Append("function_name = '" + function +"'");
			}
			if(qualifierID != -1)
			{
				if(sb.Length != 0)
					sb.Append(" AND ");
				sb.Append("qualifier_id = " + qualifierID);
			}
			if(sb.Length == 0)
			{
				return ListGrantIDsFromDS();
			}
			else
			{
				ArrayList foundGrantsList = new ArrayList();
				DataRow []rows = GrantsTable.Select(sb.ToString());
				foreach(DataRow dataRow in rows)
				{
					foundGrantsList.Add(dataRow["grant_id"]);
				}
				int[] foundGrants = Utilities.ArrayListToIntArray(foundGrantsList);
				return foundGrants;
			}
		}
	
	/* !------------------------------------------------------------------------------!
	 *				CALLS FOR QUALIFIERS - DATABASE CALLS
	 * !------------------------------------------------------------------------------!
	 */
		/// <summary>
		/// to add a qualifier - returns the qualifierID
		/// </summary>
		public static int InsertQualifier(Qualifier qualifier)
		{
			int qID= -1;
			DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("Qualifier_Insert", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;

            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@qualifierTypeID", qualifier.qualifierType, DbType.Int32));
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@qualifierReferenceID", qualifier.qualifierReferenceID, DbType.Int32));
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@qualifierName", qualifier.qualifierName, DbType.String,512));
			
			try
			{
				myConnection.Open();
				qID = Convert.ToInt32(myCommand.ExecuteScalar());
				if (qID>0)
				{
					//Refresh cache
					AuthCache.QualifierSet = InternalAuthorizationDB.RetrieveQualifiers();
					AuthCache.QualifierHierarchySet = InternalAuthorizationDB.RetrieveQualifierHierarchy();
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Exception thrown in InsertQualifier. "+ ex.Message,ex);
			}
			finally
			{
				myConnection.Close();
			}
			return qID;
		}

		/// <summary>
		/// to remove a qualifier. returns a list of all qualifiers that could not be removed.
		/// </summary>
		public static int[] DeleteQualifier(int[] qualifierIDs)
		{
			DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("Qualifier_Delete", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;

            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@qualifierID", 0, DbType.Int32));
			
			/*
			 * Note : Alternately ADO.NET could be used. However, the disconnected  DataAdapter object might prove
			 * extremely inefficient and hence this method was chosen
			 */

			ArrayList arrayList = new ArrayList ();

			try 
			{
				myConnection.Open ();
											
				foreach (int qualifierID in qualifierIDs) 
				{
					// Deleting from table Qualifiers
					
					myCommand.Parameters["@qualifierID"].Value = qualifierID;
					if(myCommand.ExecuteNonQuery () <= 0)
					{
						arrayList.Add(qualifierID);
					}
					else
					{
						//Refresh cache
						AuthCache.QualifierSet = InternalAuthorizationDB.RetrieveQualifiers();
						AuthCache.QualifierHierarchySet = InternalAuthorizationDB.RetrieveQualifierHierarchy();
					}
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Exception thrown in DeleteQualifiers. " + ex.Message, ex);
			}
			finally 
			{
				myConnection.Close ();
			}

			int[] qIDs = Utilities.ArrayListToIntArray(arrayList);
			
			return qIDs;
		}

		/// <summary>
		/// to add a qualifier to the hierarchy (default parent=0)
		/// </summary>
		public static void InsertQualifierHierarchy(int qualifierID, int parentQualifierID)
		{
			if (qualifierID==parentQualifierID)
				throw new Exception("Exception thrown in InsertQualifierHierarchy. Cannot insert node with itself as parent. This will cause an infinite cyclic loop.");
			else
			{
				DbConnection myConnection = FactoryDB.GetConnection();
				DbCommand myCommand = FactoryDB.CreateCommand("QualifierHierarchy_Insert", myConnection);
				myCommand.CommandType = CommandType.StoredProcedure;

                myCommand.Parameters.Add(FactoryDB.CreateParameter( "@qualifierID", qualifierID, DbType.Int32));
                myCommand.Parameters.Add(FactoryDB.CreateParameter( "@parentQualifierID", parentQualifierID, DbType.Int32));
			
				try
				{
					myConnection.Open();
					int i= myCommand.ExecuteNonQuery();
					if (i>=0)
					{
						//Refresh cache
						AuthCache.QualifierHierarchySet = InternalAuthorizationDB.RetrieveQualifierHierarchy();
					}

				}
				catch (Exception ex)
				{
					throw new Exception("Exception thrown in InsertQualifierHierarchy. " + ex.Message, ex);
				}
				finally
				{
					myConnection.Close();
				}
			}
		}

		/// <summary>
		/// to remove a qualifier from the hierarchy
		/// </summary>
		public static bool DeleteQualifierHierarchy(int qualifierID, int parentQualifierID)
		{
			DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("QualifierHierarchy_Delete", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;

            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@qualifierID", qualifierID, DbType.Int32));
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@parentQualifierID", parentQualifierID, DbType.Int32));
			
			try
			{
				myConnection.Open();
				int i=myCommand.ExecuteNonQuery();
				if (i>=0)
				{
                    //Refresh cache
					AuthCache.QualifierHierarchySet = InternalAuthorizationDB.RetrieveQualifierHierarchy();
					return true;
				}
				else return false;
			}
			catch (Exception ex)
			{
				throw new Exception("Exception thrown in DeleteQualifierHierarchy. " + ex.Message, ex);
			}
			finally
			{
				myConnection.Close();
			}
		}

		
	/* !------------------------------------------------------------------------------!
	 *	CALLS FOR QUALIFIERS - DATASET METHODS (Also contains some Dataset versions of the above methods)
	 * !------------------------------------------------------------------------------!
	 */

		/// <summary>
		/// returns the Qualifiers table in a dataset
		/// </summary>
		public static DataSet RetrieveQualifiers()
		{
			DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("Qualifiers_RetrieveTable", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;
			DataSet ds = new DataSet();

			try
			{
				DbDataAdapter dataAdapter = FactoryDB.CreateDataAdapter();
				dataAdapter.SelectCommand = myCommand;
				dataAdapter.TableMappings.Add("Table","Qualifiers");

				dataAdapter.Fill(ds);
			
			}
			catch (Exception ex)
			{
				throw new Exception("Exception thrown in GetQualifiersTable. " + ex.Message, ex);
			}
			finally
			{
				myConnection.Close();
			}
			return ds;
		}

		/// <summary>
		/// returns the QualifierHierarchy table in a dataset
		/// </summary>
		public static DataSet RetrieveQualifierHierarchy()
		{
			DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("QualifierHierarchy_RetrieveTable", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;
			DataSet ds = new DataSet();

			try
			{
				DbDataAdapter dataAdapter = FactoryDB.CreateDataAdapter();
				dataAdapter.SelectCommand = myCommand;
				dataAdapter.TableMappings.Add("Table","Qualifier_Hierarchy");

				dataAdapter.Fill(ds);
			
			}
			catch (Exception ex)
			{
				throw new Exception("Exception thrown in GetQualifierHierarchyTable. " + ex.Message, ex);
			}
			finally
			{
				myConnection.Close();
			}
			return ds;
		}

		/// <summary>
		/// lists the IDs of all qualifiers of the specified type
		/// </summary>
		/// <param name="qualifierTypeIDs">the type of qualifier sought; if qualifierType is null, list all qualifier IDs of all types </param>
		/// <returns>an array of IDs of all matching qualifiers</returns>
		/// 
		public static int[] ListQualifierIDsFromDS ( int[] qualifierTypeIDs )
		{
			DataTable qualifiersTable = 
				AuthCache.QualifierSet.Tables[0];
			ArrayList qualifierIDsList = new ArrayList();
	
			foreach(DataRow dataRow in qualifiersTable.Rows)
			{
				for (int j=0; j< qualifierTypeIDs.Length; j++)
				{
					if(Convert.ToInt32(dataRow["qualifier_type_id"]) == qualifierTypeIDs[j])
					{
						qualifierIDsList.Add(Convert.ToInt32(dataRow["qualifier_id"]));
					}
				}
			}
			return Utilities.ArrayListToIntArray(qualifierIDsList);
		}

		/// <summary>
		/// method returns the qualifier ID associated with the supplied arguments
		/// </summary>
		/// <param name="qualifierReferenceID"></param>
		/// <param name="qualifierType"></param>
		/// <returns>the qualifier ID of qualifier that has the two supplied arguments as parameters; otherwise null</returns>
		/// 
		public static int GetQualifierIDFromDS(int qualifierReferenceID, int qualifierType)
		{
			int qualifierID = -1;
			
			DataTable qualifiersTable = AuthCache.QualifierSet.Tables[0];

			string query = "qualifier_reference_id = " + qualifierReferenceID +" AND "+"qualifier_type_id = "+qualifierType;
			DataRow []rows = qualifiersTable.Select(query);

			foreach(DataRow dataRow in rows)
			{
				qualifierID = Convert.ToInt32(dataRow["qualifier_id"]);
				break;
			}

			return qualifierID;
		}

		/// <summary>
		/// method returns the qualifier instance associated with the supplied argument
		/// </summary>
		/// <param name="qualifierID"></param>
		/// <returns>a qualifier instance pertaining to the supplied qualifier ID; otherwise an empty qualifier instance</returns>
		public static Qualifier GetQualifierFromDS(int qualifierID)
		{
			Qualifier q = new Qualifier();
			DataTable qualifiersTable = AuthCache.QualifierSet.Tables[0];

			string query = "qualifier_id = " + qualifierID;
			DataRow []rows = qualifiersTable.Select(query);

			foreach(DataRow dataRow in rows)
			{
				q.qualifierReferenceID = Convert.ToInt32(dataRow["qualifier_reference_id"]);
				q.qualifierType = Convert.ToInt32(dataRow["qualifier_type_id"]);
				q.qualifierID = Convert.ToInt32(dataRow["qualifier_id"]);
				q.qualifierName = dataRow["qualifier_name"].ToString();
				break;
			}
			return q;
		}

		/// <summary>
		///  Gets the direct parents of a given qualifier
		/// </summary>
		/// <param name="qualifierID"></param>
		/// <returns></returns>
		public static int[] ListQualifierParentsFromDS(int qualifierID)
		{
			ArrayList directParentList = new ArrayList();

			DataTable qualifierHierarchyTable = 
				AuthCache.QualifierHierarchySet.Tables[0];
			string query = "qualifier_id = " + qualifierID +" AND "+"parent_qualifier_id <> "+Qualifier.ROOT;
			DataRow []rows = qualifierHierarchyTable.Select(query);

			foreach(DataRow dataRow in rows)
			{
				directParentList.Add(Convert.ToInt32(dataRow["parent_qualifier_id"]));
			}
			
			return Utilities.ArrayListToIntArray(directParentList);
		}

		/// <summary>
		/// method returns the qualifier children associated with the supplied argument
		/// </summary>
		/// <param name="parentQualifierID"></param>
		/// <returns>an array of qualifier IDs pertaining to the children qualifiers, otherwise an empty array</returns>
		public static int[] ListQualifierChildren(int parentQualifierID)
		{
			ArrayList directChildrenList = new ArrayList();

			DataTable qualifierHierarchyTable = 
				AuthCache.QualifierHierarchySet.Tables[0];

			string query = "parent_qualifier_id = " + parentQualifierID ;
			DataRow []rows = qualifierHierarchyTable.Select(query);

			foreach(DataRow dataRow in rows)
			{
				directChildrenList.Add(Convert.ToInt32(dataRow["qualifier_id"]));
			}

			return Utilities.ArrayListToIntArray(directChildrenList);
		}

        /// <summary>
        /// method returns the number of qualifier instances where the name is changed
        /// </summary>
        /// <param name="qualifierTypeId"></param>
        /// <param name="referenceId"></param>
        /// <param name="name"></param>
        /// <returns>a qualifier instance pertaining to the supplied qualifier ID; otherwise an empty qualifier instance</returns>
        public static int ModifyQualifierName(int qualifierTypeId, int referenceId, string name)
        {
            int status = 0;
            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("Qualifier_UpdateName", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@qualifierTypeId", qualifierTypeId, DbType.Int32));
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@refid", referenceId, DbType.Int32));
            myCommand.Parameters.Add(FactoryDB.CreateParameter( "@newName", name, DbType.String,256));

            try
            {
                myConnection.Open();
                status = Convert.ToInt32(myCommand.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw new Exception("Exception thrown in ModifyQualifierName. " + ex.Message, ex);
            }
            finally
            {
                myConnection.Close();
            }
            if (status > 0)
            {
                //Update cache
                AuthCache.QualifierSet = InternalAuthorizationDB.RetrieveQualifiers();
            }
            return status;
        }

		/* !------------------------------------------------------------------------------!
	     *							CALLS FOR AGENTS
	     * !------------------------------------------------------------------------------!
	     */

	

      

        ///// <summary>
        ///// to retrieve agent metadata for agents specified by array of agent IDs 
        ///// </summary>
        //public static Agent[] SelectAgents ( int[] agentIDs )
        //{
        //    Agent[] a = new Agent[agentIDs.Length ];
        //    for (int i=0; i<agentIDs.Length ; i++)
        //    {
        //        a[i] = new Agent();
        //    }

        //    DbConnection myConnection = FactoryDB.GetConnection();
        //    DbCommand myCommand = FactoryDB.CreateCommand("Agent_Retrieve", myConnection);
        //    myCommand.CommandType = CommandType.StoredProcedure;
        //    myCommand.Parameters .Add(FactoryDB.CreateParameter("@agentID",null,DbType.Int32));

        //    try 
        //    {
        //        myConnection.Open ();
				
        //        for (int i =0; i < agentIDs.Length ; i++) 
        //        {
        //            myCommand.Parameters["@agentID"].Value = agentIDs[i];

        //            // get agent info from table agents
        //            DbDataReader myReader = myCommand.ExecuteReader ();
        //            while(myReader.Read ())
        //            {	
        //                a[i].id = agentIDs[i];

        //                int isGroup=0;
        //                if(myReader["is_group"] != System.DBNull.Value )
        //                    isGroup= Convert.ToByte(myReader["is_group"]);
        //                if (isGroup==1)
        //                    a[i].type=Agent.groupType;
        //                else
        //                    a[i].type = Agent.userType;
        //                if(myReader["agent_name"] != System.DBNull.Value )
        //                    a[i].type= (string)(myReader["agent_name"]);
        //                /*if(myReader["date_created"] != System.DBNull .Value )
        //                    a[i].= (string) myReader["date_created"];*/
        //            }
        //            myReader.Close ();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Exception thrown SelectGroups. " + ex.Message, ex);
        //    }
        //    finally 
        //    {
        //        myConnection.Close ();
        //    }
			
        //    return a;
        //}

       

		/* !------------------------------------------------------------------------------!
		 *	CALLS FOR AGENTS - DATASETS (Also contains some Dataset versions of the above methods)
		 * !------------------------------------------------------------------------------!
		 */

		/// <summary>
		/// returns the Agents table in a dataset
		/// </summary>
		public static DataSet RetrieveAgents()
		{
			DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("Group_RetrieveHierarchyTable", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;
			DataSet ds = new DataSet();

			try
			{
				DbDataAdapter dataAdapter = FactoryDB.CreateDataAdapter();
				dataAdapter.SelectCommand = myCommand;
				dataAdapter.TableMappings.Add("Table","Agents");

				dataAdapter.Fill(ds);
			
			}
			catch (Exception ex)
			{
				throw new Exception("Exception thrown in RetrieveAgents. " + ex.Message, ex);
			}
			finally
			{
				myConnection.Close();
			}
			return ds;
		}

		/// <summary>
		/// returns the direct parents of an agent - dataset method
		/// </summary>
		/// <param name="agentID"></param>
		/// <returns>an array of agent IDs pertaining to the parent agents, otherwise an empty array</returns>

		public static int[] ListAgentParentsFromDS(int agentID)
		{
			List<int> directParentList = new List<int>();

			DataTable agentHierarchyTable = AuthCache.AgentHierarchySet.Tables[0];
			string query = "group_id = " + agentID ;
			DataRow []rows = agentHierarchyTable.Select(query);

			foreach(DataRow dataRow in rows)
				directParentList.Add(Convert.ToInt32(dataRow["parent_group_id"]));

			return directParentList.ToArray();
		}
		
		/// <summary>
		/// returns the direct children of an agent - dataset method
		/// </summary>
		/// <param name="agentID"></param>
		/// <returns></returns>
		public static int[] ListAgentChildrenFromDS(int agentID)
		{
			List<int> directChildrenList = new List<int>();

			DataTable agentHierarchyTable = AuthCache.AgentHierarchySet.Tables[0];

			string query = "parent_group_id = " + agentID ;
			DataRow []rows = agentHierarchyTable.Select(query);

			foreach(DataRow dataRow in rows)
					directChildrenList.Add(Convert.ToInt32(dataRow["group_id"]));	
			
			return directChildrenList.ToArray();
		}

		
		/* !------------------------------------------------------------------------------!
		 *				CHECK AUTHORIZATION HELPER METHOD - USES DATASET
		 * !------------------------------------------------------------------------------!
		 */

		/// <summary>
		/// determines whether the specified agent has the appropriate authorization specified by function and qualifierID; both explicit and implicit grants are checked; there is no wildcard matching and none of the arguments may be null; 
		/// </summary>
		/// <param name="agentID">the ID of the agent for which authorization is being checked;</param>
		/// <param name="function">the Function for which authorization is being checked;</param>
		/// <param name="qualifierID">the qualifierID of a previously created Qualifier whose QualifierType is that required by the function, if the FunctionType requires one; otherwise null;</param>
		/// <returns>true if the agent is authorized to perform the function with the qualifier (if present); false otherwise</returns>
		public static bool CheckAuthorizationFromDS (int agentID, string function, int qualifierID )
		{	
			int[] explicitGrants = FindGrantsIDsFromDS(agentID, function, qualifierID);

            if (explicitGrants != null && explicitGrants.Length > 0)
				return true;

			/* 
			* Check for implicit grants - since grants are inherited through the tree.
			* Find the agent and qualifier ancestors and check their grants
			*/
			ArrayList agentAncestorList = new ArrayList();
				
			AuthorizationUtilities.GetGroupAncestors(agentID, agentAncestorList);
			//also need to add the agentID itself (it might apply to certain explicit grants)
			agentAncestorList.Add(agentID);
	
			ArrayList qualifierAncestorList = new ArrayList();

			AuthorizationUtilities.GetQualifierAncestors(qualifierID, qualifierAncestorList);	
			//also need to add the qualifierID itself (it might apply to certain explicit grants)
			qualifierAncestorList.Add(qualifierID);
	
			// Check for grants with this list of qualifiers and agents
			
			//This is an efficient way of doing it - other option is to check using FindGrants for each combination of
			//agent, function and qualifier. That is not as efficient as this.

			DataTable GrantsTable = AuthCache.GrantSet.Tables[0];

			string query = " function_name = '" + function+"'";
			DataRow []rows = GrantsTable.Select(query);

			//look for the implicit grant
			foreach(DataRow dataRow in rows)
			{
				foreach(int agentAncestor in agentAncestorList)
				{
					foreach(int qualifierAncestor in qualifierAncestorList)
					{
						if( Convert.ToInt32(dataRow["agent_id"]) == agentAncestor &&
							Convert.ToInt32(dataRow["qualifier_id"])==qualifierAncestor )
							return true;
					}
				}
			}
			return false;
		}

	}
}
