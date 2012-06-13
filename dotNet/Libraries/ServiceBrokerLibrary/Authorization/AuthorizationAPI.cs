/*
 * Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
 * Please see license.txt in top level directory for full license.
 */

/* $Id$ */

using System;
using System.Data;
using System.Collections;

using iLabs.Core;
//using iLabs.ServiceBroker.DataStorage;
using iLabs.ServiceBroker.Internal;
using iLabs.ServiceBroker.Mapping;
using iLabs.DataTypes;
using iLabs.DataTypes.ProcessAgentTypes;
using iLabs.DataTypes.TicketingTypes;
using iLabs.Ticketing;
using iLabs.TicketIssuer;
using iLabs.ServiceBroker;
using iLabs.UtilLib;

namespace iLabs.ServiceBroker.Authorization
{
	/// <summary>
	/// Summary description for Authorization.
	/// </summary>
	/// 

    public class ExperimentAccess
    {
        public const int READ = 1;
        public const int WRITE = 2;
        public const int ADMINISTER = 4;
        public const int ALL = 7;
    }

	/// <summary>
	/// Structure which holds information pertaining to Service Broker permissions
	/// </summary>
    /// <param name="agentID">the ID of the agent to which the grant is to be added</param>
    /// <param name="function">the Function of grant to be added; function must be one of the fixed set of FunctionTypes recognized by the Service Broker</param>
    /// <param name="qualifierID">the ID of a previously created Qualifier whose QualifierType is that required by the function, if the FunctionType requires one; otherwise null</param>
		
	public struct Grant
	{
		public int grantID;
		public int agentID;
		public string function;
		public int qualifierID;

        public string Function
        {
            get
            {
                return function;
            }
        }
	}

	public struct Qualifier
	{
		public int qualifierID;
        /// <summary>
        /// The Id of the object this Qualifier refers to
        /// </summary>
		public int qualifierReferenceID;
        /// <summary>
        /// The type of object
        /// </summary>
		public int qualifierType;
		public string qualifierName;
		public Qualifier[] parents;

		public const int nullQualifierID = 0;
		public const int ROOT = 1;
		public const int superUserQualifierID=2;

		// Hard coded in the database - make sure this gets changed if the database changes
		public const int labClientQualifierTypeID = 2;
		public const int labServerQualifierTypeID = 3;
		public const int experimentQualifierTypeID = 4;
		public const int groupQualifierTypeID = 5;
		public const int experimentCollectionQualifierTypeID = 6;
		public const int labSchedulingQualifierTypeID = 7;
		public const int userSchedulingQualifierTypeID = 8;
		public const int serviceBrokerQualifierTypeID = 9;
        public const int storageServerQualifierTypeID = 10;
        public const int resourceMappingQualifierTypeID = 11;


        public static int ToTypeID(string serviceType)
        {
            int qualifierTypeID = -1;

            if (serviceType.Equals(ProcessAgentType.SCHEDULING_SERVER))
                qualifierTypeID = Qualifier.userSchedulingQualifierTypeID;

            else if (serviceType.Equals(ProcessAgentType.LAB_SCHEDULING_SERVER))
                qualifierTypeID = Qualifier.labSchedulingQualifierTypeID;
            else if (serviceType.Equals(ProcessAgentType.SERVICE_BROKER)|| serviceType.Equals(ProcessAgentType.REMOTE_SERVICE_BROKER))
                qualifierTypeID = Qualifier.serviceBrokerQualifierTypeID;

            else if (serviceType.Equals(ProcessAgentType.EXPERIMENT_STORAGE_SERVER))
                qualifierTypeID = Qualifier.storageServerQualifierTypeID;

            else if (serviceType.Equals(ProcessAgentType.LAB_SERVER)|| serviceType.Equals(ProcessAgentType.BATCH_LAB_SERVER))
                qualifierTypeID = Qualifier.labServerQualifierTypeID;

            return qualifierTypeID;
        }


	}

	
	public struct Function 
	{
		public string function;

		public const string addMemberFunctionType = "addMember";
		public const string administerGroupFunctionType = "administerGroup";
		public const string readExperimentFunctionType = "readExperiment";
		public const string writeExperimentFunctionType = "writeExperiment";
		public const string createExperimentFunctionType = "createExperiment";
		public const string useLabClientFunctionType = "useLabClient";
		public const string useLabServerFunctionType = "useLabServer";
		public const string useLabSchedulingFunctionType = "useLabScheduling";
		public const string useUserSchedulingFunctionType = "useUserScheduling";

        /* Functions that are also ticket types */
        /* Administer PA Functions*/
        public const string administerLSS = "ADMINISTER LSS";
        public const string administerESS = "ADMINISTER ESS";
        public const string administerLS = "ADMINISTER LS";
        public const string administerUSS = "ADMINISTER USS";
 
        /* Manage PA Functions */
        public const string manageLAB = "MANAGE LAB";
        public const string administerExperiment = "ADMINISTER EXPERIMENT";
        public const string manageUSSGroup = "MANAGE USS GROUP";
        public const string requestReservation = "REQUEST RESERVATION";
	}

	public class AuthorizationAPI
	{
		
		/// <summary>
		/// Authorization Constructor
		/// </summary>
		public AuthorizationAPI()
		{
		}

		/// <summary>
		/// adds a grant to the agent (User or Group) agentID 
		/// </summary>
		/// <param name="agentID">the ID of the agent to which the grant is to be added</param>
		/// <param name="function">the Function of grant to be added; function must be one of the fixed set of FunctionTypes recognized by the Service Broker</param>
		/// <param name="qualifierID">the ID of a previously created Qualifier whose QualifierType is that required by the function, if the FunctionType requires one; otherwise null</param>
		/// <returns>the ID of the new Grant if agentID is a registered user or group, if function is a legitimate FunctionType, and, in the case where qualifierID is specified, if qualifierID is a recognized ID of the appropriate QualifierType for the function. In addition, the specified grant must not already have been made to the specified agent. If any of these conditions fails, then this method returns null. Note that most of the other Service Broker AddXXX() methods specify the ID of the object being added; the AddGrant() method does not do this; grantIDs are arbitrary strings generated by the Service Broker implementation</returns>
		public static int AddGrant( int agentID, string function, int qualifierID )
		{
			Grant g = new Grant();
			g.agentID = agentID;
			g.function= function;
			g.qualifierID = qualifierID;

            return InternalAuthorizationDB.InsertGrant(g);
		}

		/// <summary>
		/// removes previously registered grants. The removal of a grant should not affect any Service Broker calls currently in progress, but it can prevent the execution of later related method calls. For instance, if the revoked grant permits a group to use a particular lab server, then any experiments in progress should run to completion, but users will be unable to retrieve the corresponding results through the Service Broker.
		/// </summary>
		/// <param name="grantIDs">an array of the IDs of previously registered grants</param>
		/// <returns>an array of the IDs of the grants that could not be removed, i.e., those for which the operation failed</returns>
		public static int[] RemoveGrants ( int[] grantIDs )
		{
             return InternalAuthorizationDB.DeleteGrants ( grantIDs );
		}

		/// <summary>
		/// lists the IDs of all grants registered with the Service Broker
		/// </summary>
		/// <returns>an array of IDs for all registered grants</returns>		
		public static int[] ListGrantIDs ()
		{
			return InternalAuthorizationDB.ListGrantIDsFromDS ();
		}

		/// <summary>
		/// returns an array of the immutable Grant objects for the registered grants whose IDs
        /// are supplied in grantIDs
		/// </summary>
		/// <param name="grantIDs">returns an array of the immutable Grant objects for the registered grants whose IDs are supplied in grantIDs</param>
		/// <returns>an array of Grant objects describing the registered grants specified in grantIDs; if the nth grant ID does not correspond to a grant, the nth entry in the return array will be null</returns>
		public static Grant[] GetGrants ( int[] grantIDs )
		{
			return InternalAuthorizationDB.GetGrantsFromDS (grantIDs);
		}
		
		
		/// <summary>
		/// enumerates the IDs of all explicit grants matching the specification of the arguments; 
        /// any or all of the arguments may be null, in which case, the method does a wildcard 
        /// match on the null argument(s). (A FindGrants() with all the arguments null is equivalent 
        /// to a ListGrantIDs().
		/// </summary>
		/// <param name="agentID">the ID of the agent for which matching grants are sought;</param>
		/// <param name="function">the Function for which matching grants are sought; function must
        /// be one of the fixed set of FunctionTypes recognized by the Service Broker;</param>
		/// <param name="qualifierID">the ID of the Qualifier (associated with the function, 
        /// if the FunctionType requires one; otherwise null) for which matching grants are sought</param>
		/// <returns>an array of grantIDs of any grants that match the specification.</returns>
		public static int[] FindGrants (int agentID, string function, int qualifierID )
		{
			return InternalAuthorizationDB.FindGrantsIDsFromDS (  agentID, function, qualifierID );
		}


		/// <summary>
		/// determines whether the specified agent has the appropriate authorization specified by function and qualifierID; both explicit and implicit grants are checked; there is no wildcard matching and none of the arguments may be null; 
		/// </summary>
		/// <param name="agentID">the ID of the agent for which authorization is being checked;</param>
		/// <param name="function">the Function for which authorization is being checked;</param>
		/// <param name="qualifierID">the qualifierID of a previously created Qualifier whose QualifierType is that required by the function, if the FunctionType requires one; otherwise null;</param>
		/// <returns>true if the agent is authorized to perform the function with the qualifier (if present); false otherwise</returns>
		public static bool CheckAuthorization ( int agentID, string function, int qualifierID )
		{
              return InternalAuthorizationDB.CheckAuthorizationFromDS ( agentID, function, qualifierID );
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="qualifierReferenceID">the ID of the resource represented by the qualifier (null if not necessary);</param>
		/// <param name="type">one of the statically configured qualifierTypes registered with the Service Broker; the qualifier type is used to check type compatibility between function and qualifier in grants</param>
		/// <param name="name">A semantically meaningful string describing the purpose of the qualifier</param>
		/// <param name="parentQualifierID">the ID of a previously created Qualifier to serve as the parent of the Qualifier being added; the parentID locates the new Qualifier in the hierarchy; additional parents may be added by the AddQualifier-Parent() method</param>
		/// <returns>the ID of the new qualifier if it is successfully created, null otherwise</returns>
		public static int AddQualifier( int qualifierReferenceID, int type, string name, int parentQualifierID )
		{
			Qualifier q = new Qualifier();
			q.qualifierReferenceID = qualifierReferenceID;
			q.qualifierType = type;
			q.qualifierName = name;
			bool qualifierReferenceIDValid = false;

			int qualifierID = -1;			
			ProcessAgentDB db = new ProcessAgentDB();
            ProcessAgent agent = null;
			//need to check whether the qualifierReferenceID for a particular qualifierType 
			//actually exists before it is added to the table
			switch (type)
			{
					// LabServerfo
					// Replace with case for all processAgent types an use exits for the combination of ID & type
				case Qualifier.labServerQualifierTypeID:
				{
                    agent = db.GetProcessAgent(qualifierReferenceID);
                    if(agent != null && ((agent.type.Equals(ProcessAgentType.BATCH_LAB_SERVER)) 
                        ||(agent.type.Equals(ProcessAgentType.LAB_SERVER)))){
							qualifierReferenceIDValid = true;
					}
					break;
				}

					//LabClient
				
            case Qualifier.serviceBrokerQualifierTypeID:
                {
                    agent = db.GetProcessAgent(qualifierReferenceID);
                    if (agent != null && ((agent.type.Equals(ProcessAgentType.SERVICE_BROKER)) 
                          ||(agent.type.Equals(ProcessAgentType.BATCH_SERVICE_BROKER))
                          || (agent.type.Equals(ProcessAgentType.REMOTE_SERVICE_BROKER))))
                    {
                        qualifierReferenceIDValid = true;
                    }
                    break;
                }
           
            case Qualifier.labSchedulingQualifierTypeID:
                {
                    agent = db.GetProcessAgent(qualifierReferenceID);
                    if (agent != null && agent.type.Equals(ProcessAgentType.LAB_SCHEDULING_SERVER))
                    {
                        qualifierReferenceIDValid = true;
                    }
                    break;
                }
            case Qualifier.userSchedulingQualifierTypeID:
                {
                    agent = db.GetProcessAgent(qualifierReferenceID);
                    if (agent != null && agent.type.Equals(ProcessAgentType.SCHEDULING_SERVER))
                    {
                        qualifierReferenceIDValid = true;
                    }
                    break;
                }
            case Qualifier.storageServerQualifierTypeID:
                {
                    agent = db.GetProcessAgent(qualifierReferenceID);
                    if (agent != null && agent.type.Equals(ProcessAgentType.EXPERIMENT_STORAGE_SERVER))
                    {
                        qualifierReferenceIDValid = true;
                    }
                    break;
                }
            case Qualifier.labClientQualifierTypeID:
                {
                    int[] labClientIDs = InternalAdminDB.SelectLabClientIDs();
                    foreach (int labClientID in labClientIDs)
                    {
                        if (labClientID == qualifierReferenceID)
                        {
                            qualifierReferenceIDValid = true;
                            break;
                        }
                    }
                    break;
                }
					//Group
				case Qualifier.groupQualifierTypeID:
				{
					int[] groupIDs = InternalAdminDB.SelectGroupIDs();
					foreach(int groupID in groupIDs)
					{
						if(groupID == qualifierReferenceID)
						{
							qualifierReferenceIDValid = true;
							break;
						}
					}
					break;
				}

					//Experiment Collection
				case Qualifier.experimentCollectionQualifierTypeID:
				{
					int[] groupIDs = InternalAdminDB.SelectGroupIDs();
					foreach(int groupID in groupIDs)
					{
						if(groupID == qualifierReferenceID)
						{
							qualifierReferenceIDValid = true;
							break;
						}
					}
					break;
				}

			    //Experiment
				case Qualifier.experimentQualifierTypeID:
				{
					Criterion c = new Criterion("experiment_id","=",qualifierReferenceID.ToString());
                    if (InternalDataDB.SelectExperimentIDs(new Criterion[] { c }).Length > 0)
						qualifierReferenceIDValid = true;
					break;
				}

                // Resource Mapping
                case Qualifier.resourceMappingQualifierTypeID:
                {
                    BrokerDB brokerDb = new BrokerDB();
                    ResourceMapping mapping = brokerDb.GetResourceMapping(qualifierReferenceID);
                    if (mapping != null)
                    
                            qualifierReferenceIDValid = true;
                            break;                
                }
			}

			if(qualifierReferenceIDValid)
			{
				try
				{
					qualifierID = InternalAuthorizationDB.InsertQualifier(q);

					if(qualifierID != -1)
					{
						InternalAuthorizationDB.InsertQualifierHierarchy(qualifierID,parentQualifierID);
					}
				}
				catch(Exception ex)
				{
					throw;
				}
			}
	
			return qualifierID;	
    	} 

		/// <summary>
		/// adds a parent to the specified node in a qualifier hierarchy
		/// </summary>
		/// <param name="parentQualifierID">the ID of a previously created Qualifier that will serve as an additional parent of the Qualifier designated by childID; the addition of the parent must preserve the acyclic property of the Qualifier graph.</param>
		/// <param name="qualifierID">the ID of a previously created Qualifier to which the new parent is being added.</param>
		/// <returns>true if the parent qualifier was successfully added, false otherwise</returns>
		public static bool AddQualifierParent( int qualifierID, int parentQualifierID )
		{
			//need to preserve acyclical graph hence parentQualifierID cannot already be a 
			//descendant of qualifierID
			if(IsQualifierDescendant(parentQualifierID, qualifierID))
				return false;

			//check if the Qualifier has any parents
			//if it's only parent is root, then it has no parents
			//in which case it is remove from root and added under the other qualifier
			
			bool hasParents = false;

			if (InternalAuthorizationDB.ListQualifierParentsFromDS(qualifierID).Length >0)
				hasParents =true;

			try
			{
				InternalAuthorizationDB.InsertQualifierHierarchy(qualifierID, parentQualifierID);

				//Remove from root if it doesn't have any other parents
				if(!hasParents)
					InternalAuthorizationDB.DeleteQualifierHierarchy(qualifierID, Qualifier.ROOT);

				return true;
			}
			catch(Exception ex)
			{
				throw;
			}
		}

		/// <summary>
		/// removes a parent from the specified node in a qualifier hierarchy; the node losing a parent must possess an additional parent so it does not become derooted (cf. SetQualifierParent())
		/// </summary>
		/// <param name="parentQualifierID">the ID of a previously created Qualifier that is one of the two or more parents of the Qualifier designated by childID;</param>
		/// <param name="qualifierID">the ID of a previously created Qualifier; that is to lose a parent</param>
		/// <returns>true if the parent qualifier was successfully removed, false otherwise</returns>
		public static bool RemoveQualifierParent ( int qualifierID, int parentQualifierID )
		{
			try
			{
				//Lists all the parents (minus ROOT)
				int[] qualifierParents = ListQualifierParents(qualifierID);
			
				if (qualifierParents.Length>1)
					return InternalAuthorizationDB.DeleteQualifierHierarchy(qualifierID, parentQualifierID);
				else
					return false;
				//otherwise either this is the only parent,
				// or this is not even a parent in which case we don't bother removing it
				
			}
			catch(Exception ex)
			{
				throw;
			}
		}

		/// <summary>
		/// makes a previously created qualifier node specified by parentID the single parent of the node specified by key in a qualifier hierarchy. The operation will complete only if it preserves the acyclic character of the Qualifier graph. Note this is the only way to make a previously created Qualifier node of a hierarchy a new root
		/// </summary>
		/// <param name="parentQualifierID">the ID of a previously created Qualifier that will become the new parent of the Qualifier designated by childID;</param>
		/// <param name="qualifierID">the ID of a previously created Qualifier that will receive the new parent;</param>
		/// <returns>true if the parent qualifier was successfully changed, false otherwise</returns>
		public static bool SetQualifierParent ( int qualifierID, int parentQualifierID )
		{
			try
			{
				//need to preserve acyclical graph hence parentQualifierID cannot already be a 
				//descendant of qualifierID
				if(IsQualifierDescendant(parentQualifierID, qualifierID))
					return false;

				bool relationshipExists = false;

				int[] currentParents = InternalAuthorizationDB.ListQualifierParentsFromDS(qualifierID);
				foreach (int parent in currentParents)
					if (parentQualifierID!=parent)
						InternalAuthorizationDB.DeleteQualifierHierarchy(qualifierID, parentQualifierID);
					else
						relationshipExists = true;
			
				//set parent as sole qualifier if it isn't already there
				if(!relationshipExists)
				{
					InternalAuthorizationDB.InsertQualifierHierarchy(qualifierID, parentQualifierID);
				}
		
				return true;
			}
			catch(Exception ex)
			{
				throw new Exception("Exception thrown in SetQualifierParent. "+ex.Message, ex);
			}
			
		}

		/// <summary>
		/// removes previously registered Qualifiers in order from the qualifier graph; all removed qualifiers must be childless nodes of the graph when they are removed.
		/// </summary>
		/// <param name="qualifierIDs">an array of the Qualifier IDs of previously registered qualifiers to be unregistered; the qualifiers are removed in the order in which they are listed in this array</param>
		/// <returns>an array of the Qualifier IDs of the qualifiers that could not be removed, i.e., those for which the operation failed</returns>
		public static int[] RemoveQualifiers ( int[] qualifierIDs )
		{
			//check if any of the qualifierIDs have children.
			//If they have children then they cannot be removed
			ArrayList cannotRemove = new ArrayList();
			ArrayList canRemove = new ArrayList();

			try 
			{
				for(long i = 0; i<qualifierIDs.Length; i++)
				{
					// If a qualifier has children then don't delete.
					if(ListQualifierChildren(qualifierIDs[i]).Length != 0)
					{
						cannotRemove.Add(qualifierIDs[i]);
					}
					else
					{
						canRemove.Add(qualifierIDs[i]);
					}
				}

				int[] notRemoved = InternalAuthorizationDB.DeleteQualifier(Utilities.ArrayListToIntArray(canRemove));

				foreach (int ID in notRemoved)
					cannotRemove.Add(ID);

				return Utilities.ArrayListToIntArray(cannotRemove);
			}
			catch(Exception ex)
			{
				throw;
			}
		}

		/// <summary>
		/// lists the IDs of all qualifiers of the specified type
		/// </summary>
		/// <param name="qualifierTypeIDs">the type of qualifier sought; if qualifierType is null, list all qualifier IDs of all types </param>
		/// <returns>an array of IDs of all matching qualifiers</returns>		
		public static int[] ListQualifierIDs ( int[] qualifierTypeIDs )
		{
             return InternalAuthorizationDB.ListQualifierIDsFromDS (qualifierTypeIDs );
		}

		/// <summary>
		/// lists the IDs of all child qualifier of the specified qualifier
		/// </summary>
		/// <param name="parentQualifierID">the ID of the qualifier whose children are sought</param>
		/// <returns>an array of qualifier IDs pertaining to the children qualifiers, otherwise an empty array</returns>
		public static int[] ListQualifierChildren(int parentQualifierID)
		{
			return InternalAuthorizationDB.ListQualifierChildren(parentQualifierID);
		}

		/// <summary>
		/// lists the IDs of all qualifiers that are descendants of the given qualifier ID
		/// </summary>
		/// <param name="ancestorQualifierID">the ID of the qualifier whose descendants are sought</param>
		/// <param name="breadthFirst">if true, descendants are listed in breadth first order, otherwise depth first postorder, which happens to be the correct order for deleting subtrees in which all nodes are singly parented.</param>
		/// <returns>an array of Qualifier IDs of all descendant qualifiers</returns>
		public static int[] ListQualifierDescendants ( int ancestorQualifierID, bool breadthFirst )
		{
			ArrayList descendantList = new ArrayList();
			
			//use a breadth first order approach
			//this code is still not optimized, i.e. we are searching the WHOLE hierarchy table even after
			//we have traversed it once. - Karim's notes
			if(breadthFirst)
			{
				Queue descendantQueue = new Queue();
				int[] children = ListQualifierChildren(ancestorQualifierID);

				foreach (int child in children)
				{
					descendantList.Add(child);
					descendantQueue.Enqueue(child);
				}
				
				while( descendantQueue.Count > 0 )
				{
					int[] moreDescendants = ListQualifierChildren(Convert.ToInt32(descendantQueue.Dequeue()));

					foreach(int grandchild in moreDescendants)
					{	
						if(!descendantQueue.Contains(grandchild))
						{
							descendantList.Add(grandchild);
							descendantQueue.Enqueue(grandchild);
						}
					}
				}
			}

				//use a depth first postorder method instead
			else
			{
				AuthorizationUtilities.QualifierPostOrder(ancestorQualifierID, descendantList);
			}

			return Utilities.ArrayListToIntArray(descendantList);
		}

		/// <summary>
		/// method checks whether the one qualifier is the descendant of another
		/// </summary>
		/// <param name="qualifierID1">the descendant qualifier in question</param>
		/// <param name="qualifierID2">the ancestor qualifier in question</param>
		/// <returns>true iff qualifier 1 is a descendant of qualifier 2</returns>
		public static bool IsQualifierDescendant ( int qualifierID1, int qualifierID2 )
		{
			//get the descendants of qualifierID2
			int[] descendants = ListQualifierDescendants(qualifierID2, true);

			//check whether one of these descendants is qualifierID1.
			foreach(int descendant in descendants)
			{
				if(qualifierID1 == descendant)
					return true;
			}

			return false;
		}

		/// <summary>
		/// method returns the qualifier ID associated with the supplied arguments
		/// </summary>
		/// <param name="qualifierReferenceID">the ID of the resource represented by the qualifier (null if not necessary)</param>
		/// <param name="qualifierType">one of the statically configured qualifierTypes registered with the Service Broker; iLab does not support a single ID space, so qualifierReference IDs are only unique within a single qualifier</param>
		/// <returns>the qualifier ID of qualifier that has the two supplied arguments as parameters; otherwise null</returns>
		public static int GetQualifierID(int qualifierReferenceID, int qualifierType)
		{
              return InternalAuthorizationDB.GetQualifierIDFromDS(qualifierReferenceID, qualifierType);
		}

		/// <summary>
		/// method returns the qualifier instance associated with the supplied argument
		/// </summary>
		/// <param name="qualifierID">the ID of the qualifier to be retrieved</param>
		/// <returns>a qualifier instance pertaining to the supplied qualifier ID; otherwise an empty qualifier instance</returns>
		public static Qualifier GetQualifier(int qualifierID)
		{
             return InternalAuthorizationDB.GetQualifierFromDS(qualifierID);
		}

        /// <summary>
        /// method returns the number of qualifier instances where the name is changed
        /// </summary>
        /// <param name="qualifierTypeId"></param>
        /// <param name="referenceId"></param>
        /// <param name="name"></param>
        /// <returns>a qualifier instance pertaining to the supplied qualifier ID; otherwise an empty qualifier instance</returns>
        public static int ModifyQualifierName(int qualifierTypeId,int referenceId,string name)
        {
            return InternalAuthorizationDB.ModifyQualifierName(qualifierTypeId, referenceId, name);
        }

		/// <summary>
		/// methods returns the qualifier parents associated with the supplied argument
		/// </summary>
		/// <param name="childQualifierID"></param>
		/// <returns>an array of qualifier IDs pertaining to the parent qualifiers, otherwise an empty array</returns>
		public static int[] ListQualifierParents( int childQualifierID )
		{
			return InternalAuthorizationDB.ListQualifierParentsFromDS(childQualifierID);
		}

        public static int GetEffectiveGroupID(int currentGroupID, int qualRefID, int qualTypeID, string functionType)
        {
            int effectiveID = 0;
            int qualID = InternalAuthorizationDB.GetQualifierIDFromDS(qualRefID, qualTypeID);
            int[] legalGroups = InternalAuthorizationDB.FindGrantsIDsFromDS(currentGroupID, functionType, qualID);
            if (legalGroups.Length == 1)
            {
                effectiveID = currentGroupID;
            }
            else
            {
                ArrayList groups = new ArrayList();
                AuthorizationUtilities.GetGroupAncestors(currentGroupID, groups);
                if (groups.Count > 0)
                {
                    // Inverse order processing
                    for (int i = groups.Count - 1; i >= 0; i--)
                    {
                        int testGroup = Convert.ToInt32(groups[i]);
                        legalGroups = InternalAuthorizationDB.FindGrantsIDsFromDS(testGroup, functionType, qualID);
                        if (legalGroups.Length == 1)
                        {
                            effectiveID = testGroup;
                            break;
                        }
                    }
                }

            }
            return effectiveID;
        }

	}//class Authorization
}
