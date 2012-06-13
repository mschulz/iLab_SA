using System;
using System.Collections;
using iLabs.ServiceBroker;
using iLabs.ServiceBroker.Authorization;
using iLabs.ServiceBroker.Internal;
using iLabs.UtilLib;

namespace iLabs.ServiceBroker.Administration
{

	/// <summary>
	/// A structure which holds Group Types
	/// </summary>
	public struct GroupType
	{
        /// <summary>
		/// Named constant for the Group type.
		/// </summary>
       public const string NON_EXISTANT = "Non-existent Group";

		/// <summary>
		/// Named constant for the Group type.
		/// </summary>
		public const string REGULAR = "Regular Group";

		/// <summary>
		/// Named constant for the Group type.
		/// </summary>
		public const string REQUEST = "Request Group";

		/// <summary>
		/// Named constant for the Group type.
		/// </summary>
		public const string COURSE_STAFF = "Course Staff Group";

        /// <summary>
        /// Named constant for the Group type.
        /// </summary>
        public const string SERVICE_ADMIN = "Service Administration Group";

        /// <summary>
		/// Named constant for the Group type.
		/// </summary>
        public const string BUILT_IN = "Built-in Group";

        /// <summary>
		/// Summary description for AdministrativeUtilities.
		/// </summary>
		/// 
	}
	public class AdministrativeUtilities
	{
		public AdministrativeUtilities()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		/// <summary>
		/// This returns an array of Lab Client IDs that a group has permission to use
		/// </summary>
		/// <param name="groupID"></param>
		/// <returns></returns>
 		public static int[] GetGroupLabClients (int groupID)
		{
			// get all grants of useLabClient for this group
			// ..........
			ArrayList aListGrants = new ArrayList();

			// find the grants for this group, with function useLabClient
			int[] grantIDs = Authorization.AuthorizationAPI.FindGrants (groupID, Function.useLabClientFunctionType,-1);

			for (int i=0; i< grantIDs.Length ; i++)
			{
				aListGrants.Add(grantIDs[i]);
			}

			// find the grants for all the parent groups, with function useLabClient
			int[] parentGroups =AdministrativeAPI.ListParentGroupsForGroupRecursively (groupID);

			for(int i=0; i< parentGroups.Length ; i++)
			{
				grantIDs = Authorization.AuthorizationAPI.FindGrants (parentGroups[i], Function.useLabClientFunctionType,-1);

				for (int j=0; j< grantIDs.Length ; j++)
				{
					aListGrants.Add(grantIDs[j]);
				}
			}

			grantIDs = Utilities.ArrayListToIntArray(aListGrants);

			//get the full Grant objects given a list of grantIDs
			Grant[] useLCGrantsList = Authorization.AuthorizationAPI.GetGrants(grantIDs);
				
			//get a list of qualifier reference IDs (which are lab clients here)
			ArrayList lcIDsList = new ArrayList();
			if ((useLCGrantsList !=null) && (useLCGrantsList.Length >0))
			{
				foreach(Grant grant in useLCGrantsList)
				{
					lcIDsList.Add(Authorization .AuthorizationAPI.GetQualifier (((Grant) grant).qualifierID).qualifierReferenceID );
				}
			}

			// return the list of lab client IDs
			return Utilities.ArrayListToIntArray(lcIDsList);
		}

		/// <summary>
		///  This returns an array of Lab Server IDs that a group has permission to use
		/// </summary>
		/// <param name="groupID"></param>
		/// <returns></returns>
		public static int[] GetGroupLabServers (int groupID)
		{
			// get all grants of useLabServer for this group
			// ..........

			ArrayList aListGrants = new ArrayList();

			// find the grants for this group, with function useLabServer
			int[] grantIDs = Authorization.AuthorizationAPI.FindGrants (groupID, Function.useLabServerFunctionType,-1);

			for (int i=0; i< grantIDs.Length ; i++)
			{
				aListGrants.Add(grantIDs[i]);
			}

			// find the grants for all the parent groups, with function useLabServer
			int[] parentGroups = AdministrativeAPI.ListParentGroupsForGroupRecursively (groupID);

			for(int i=0; i< parentGroups.Length ; i++)
			{
				grantIDs = Authorization.AuthorizationAPI.FindGrants (parentGroups[i], Function.useLabServerFunctionType,-1);

				for (int j=0; j< grantIDs.Length ; j++)
				{
					aListGrants.Add(grantIDs[j]);
				}
			}

			grantIDs = Utilities.ArrayListToIntArray(aListGrants);

			//get the full Grant objects given a list of grantIDs
			Grant[] useLSGrantsList = Authorization.AuthorizationAPI.GetGrants (grantIDs);
				
			ArrayList lsIDsList = new ArrayList();

			//get a list of qualifier reference IDs (which are lab servers here)
			if ((useLSGrantsList !=null) && (useLSGrantsList.Length >0))
			{
				foreach(Grant grant in useLSGrantsList)
				{
					lsIDsList.Add(Authorization .AuthorizationAPI.GetQualifier (((Grant) grant).qualifierID).qualifierReferenceID );
				}
			}

			// return the list of lab server IDs
			return Utilities.ArrayListToIntArray(lsIDsList);
		}


		/// <summary>
		/// This returns an array of Group IDs which have permission to use a given LabServer 
		/// </summary>
		/// <param name="labServerID"></param>
		/// <returns></returns>
 		public static int[] GetLabServerGroups (int labServerID)
		{
			// get all grants of useLabServer for this group
			// ..........

			ArrayList aListGrants = new ArrayList();

			// Get a qualifier ID given the reference and type (lab server)
			int  qualID = InternalAuthorizationDB.GetQualifierIDFromDS(labServerID,Qualifier.labServerQualifierTypeID);

			// find all grants of type useLabServer
			int [] grantIDs = InternalAuthorizationDB.FindGrantsIDsFromDS(-1,Function.useLabServerFunctionType,qualID);
			
			// get grant objects
			Grant [] grants = Authorization.AuthorizationAPI.GetGrants(grantIDs);

			ArrayList groupList = new ArrayList();
			if (grants !=null)
			{
				foreach(Grant g in grants)
				{
					// these are the groups that can access the lab server
					groupList.Add(g.agentID);
				}
			}

			ArrayList children = new ArrayList();
			ArrayList finalGroups = new ArrayList(groupList);

			foreach (int groupID in groupList)
			{
				// a group's children also have permission to use the lab server
				
				int[]childrenIDs = AdministrativeAPI.ListSubgroupIDsRecursively(groupID);
				foreach (int child in childrenIDs)
				{
					finalGroups.Add(child);
				}
			}

			// return the list of groupIDs
			return Utilities.ArrayListToIntArray(finalGroups);
		}

        /// <summary>
		/// This returns an array of Group IDs which have permission to use a given LabClient
		/// </summary>
		/// <param name="labClientID"></param>
		/// <returns></returns>
        public static int[] GetLabClientGroups(int labClientID)
        {
            return AdministrativeUtilities.GetLabClientGroups(labClientID, true);
        }

		/// <summary>
		/// This returns an array of Group IDs which have permission to use a given LabClient
		/// </summary>
		/// <param name="labClientID"></param>
		/// <returns></returns>
		public static int[] GetLabClientGroups (int labClientID, bool getChildren)
		{
			// get all grants of useLabClient for this group
			// ..........

			ArrayList aListGrants = new ArrayList();

			// Get a qualifier ID given the reference and type (lab client)
			int  qualID = InternalAuthorizationDB.GetQualifierIDFromDS(labClientID,Qualifier.labClientQualifierTypeID);

			// find all grants of type useLabServer
			int [] grantIDs = InternalAuthorizationDB.FindGrantsIDsFromDS(-1,Function.useLabClientFunctionType,qualID);
			
			// get grant objects
			Grant [] grants = Authorization.AuthorizationAPI.GetGrants(grantIDs);

			ArrayList groupList = new ArrayList();
			if (grants !=null)
			{
				foreach(Grant g in grants)
				{
					// these are the groups that can access the lab server
					groupList.Add(g.agentID);
				}
			}
            if (getChildren)
            {
                ArrayList children = new ArrayList();
                ArrayList finalGroups = new ArrayList(groupList);

                foreach (int groupID in groupList)
                {
                    // a group's children also have permission to use the lab server

                    int[] childrenIDs = AdministrativeAPI.ListSubgroupIDsRecursively(groupID);
                    foreach (int child in childrenIDs)
                    {
                        finalGroups.Add(child);
                    }

                }

                // return the list of groupIDs
                return Utilities.ArrayListToIntArray(finalGroups);
            }
            else
            {
                return Utilities.ArrayListToIntArray(groupList);
            }
		}

		/// <summary>
		/// This returns the groupID of the request group for the specified groupID
		/// </summary>
		/// <param name="groupID"></param>
		/// <returns></returns>
		public static int GetGroupRequestGroup (int groupID)
		{
			return InternalAdminDB.SelectGroupRequestGroup(groupID);
		}

		/// <summary>
		/// This returns the groupID of the course staff group for the specified groupID
		/// </summary>
		/// <param name="groupID"></param>
		/// <returns></returns>
		public static int GetGroupAdminGroup (int groupID)
		{
			return InternalAdminDB.SelectGroupAdminGroupID(groupID);
		}


		public class GroupComparer : IComparer
		{
			public int Compare(object o1, object o2)
			{
				string group1 = ((Group) o1).groupName;
				string group2 = ((Group) o2).groupName;

				return group1.CompareTo(group2);
			}
		}
	}
}
