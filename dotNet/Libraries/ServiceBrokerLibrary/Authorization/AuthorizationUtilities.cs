using System;
using System.Collections;
using iLabs.ServiceBroker;
using iLabs.ServiceBroker.Internal;

namespace iLabs.ServiceBroker.Authorization
{
	/// <summary>
	/// Summary description for AuthorizationUtilities.
	/// </summary>
	public class AuthorizationUtilities
	{
		public AuthorizationUtilities()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		/* !------------------------------------------------------------------------------!
		 *					CALLS FOR QUALIFIERS - HELPER METHODS
		 * !------------------------------------------------------------------------------!
		 */

		/// <summary>
		/// returns the ancestors of a given qualifier
		/// </summary>
		/// <param name="descendantID">The qualifier whose ancestors are to be found</param>
		/// <param name="a">ArrayList that is dynanmically changed</param>
		public static void GetQualifierAncestors(int descendantID, ArrayList a)
		{
			int[] ancestorArray = InternalAuthorizationDB.ListQualifierParentsFromDS(descendantID);
			foreach(int ancestor in ancestorArray)
			{
				if(!a.Contains(ancestor))
				{
					GetQualifierAncestors(ancestor, a); // Done inside the loop to prevent cyclic looping.
					a.Add(ancestor);
				}
			}
		}

		/// <summary>
		/// Does a depth first retrieval of the qualifier descendants - helper method for ListQualifierDescendants
		/// </summary>
		/// <param name="ancestorQualifierID">The qualifier whose descendants are to be found</param>
		/// <param name="a">ArrayList that is dynanmically changed</param>
		public static void QualifierPostOrder(int ancestorQualifierID, ArrayList a)
		{
			int[] descendantArray = InternalAuthorizationDB.ListQualifierChildren(ancestorQualifierID);
			foreach(int descendant in descendantArray)
			{
				if(!a.Contains(descendant))
				{
					a.Add(descendant);
				}
				QualifierPostOrder(descendant, a);
			}
		}

		/* !------------------------------------------------------------------------------!
		 *							CALLS FOR AGENTS - HELPER METHODS
		 * !------------------------------------------------------------------------------!
		 */

		/// <summary>
		///  returns all ancestors of a given agent
		/// </summary>
		/// <param name="descendantID"></param>
		/// <param name="a"></param>
		public static void GetGroupAncestors(int descendantID, ArrayList a)
		{
			int[] ancestorArray = InternalAuthorizationDB.ListAgentParentsFromDS(descendantID);
			foreach(int ancestor in ancestorArray)
			{
				GetGroupAncestors(ancestor, a);
				if(!a.Contains(ancestor))
				{
					a.Add(ancestor);
				}
			}
		}

		/// <summary>
		/// returns all descendants of a given agent
		/// </summary>
		/// <param name="ancestorID"></param>
		/// <param name="a">this is required because the method is called recursively</param>
		public static void GetGroupDescendants(int ancestorID, ArrayList a)
		{
			int[] descendantArray = InternalAuthorizationDB.ListAgentChildrenFromDS(ancestorID);
			foreach(int descendant in descendantArray)
			{
				GetGroupDescendants(descendant, a);
				if(!a.Contains(descendant))
				{
					a.Add(descendant);
				}
			}
		}

	}

	
}
