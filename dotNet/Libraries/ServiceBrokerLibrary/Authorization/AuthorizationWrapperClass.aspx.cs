/*
 * Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
 * Please see license.txt in top level directory for full license.
 */

/* $Id$ */

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using iLabs.ServiceBroker;
using iLabs.ServiceBroker.Administration;
using iLabs.ServiceBroker.Authorization;
using iLabs.ServiceBroker.Authentication;
using iLabs.ServiceBroker.DataStorage;
using iLabs.ServiceBroker.Internal;


using iLabs.DataTypes;
using iLabs.DataTypes.StorageTypes;
//using iLabs.DataTypes.BatchTypes;
using iLabs.DataTypes.TicketingTypes;
using iLabs.Core;
using iLabs.Ticketing;
using iLabs.TicketIssuer;
using iLabs.UtilLib;


namespace iLabs.ServiceBroker.Authorization
{
	/// <summary>
	/// Summary description for AuthorizationWrapperClass.
	/// </summary>
	public class AuthorizationWrapperClass : System.Web.UI.Page
	{
        
        private static int superuserGroupID;
        private static int superuserID;

        // Load the superuser group ID
        static AuthorizationWrapperClass()
        {
            superuserGroupID = Administration.AdministrativeAPI.GetGroupID(Group.SUPERUSER);
            superuserID = Administration.AdministrativeAPI.GetUserID("superUser",0);
        }

		private void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.Load += new System.EventHandler(this.Page_Load);
		}
		#endregion

		/////////////////////////////////////////////
		///
		public void AuthenticateSuperuser(string message)
		{
			
			if(!IsSuperuserGroup())
			{
				throw new AccessDeniedException (message);
			}
		}

        public bool IsSuperuserGroup()
        {
            int sessionGroupID = Convert.ToInt32(Session["GroupID"]);
            return IsSuperuserGroup(sessionGroupID);
        }

        private bool IsSuperuserGroup(int groupID)
        {
            return (superuserGroupID == groupID);
        }







//////////////////////////////////////////////////////////////////////////////////
		/*
		/// <summary>
		/// super user access privilege
		/// </summary>
		/// <param name="labServerGUID"></param>
		/// <param name="labServerName"></param>
		/// <param name="webServiceURL"></param>
		/// <param name="labServerDescription"></param>
		/// <param name="labInfoURL"></param>
		/// <param name="contactEmail"></param>
		/// <param name="firstName"></param>
		/// <param name="lastName"></param>
		/// <returns>LabServerID</returns>
		public int AddLabServerWrapper (string labServerGUID, string labServerName, string webServiceURL, string labServerDescription, string labInfoURL, string contactEmail, string firstName, string lastName)
		{
			int sessionGroupID = Convert.ToInt32(Session["GroupID"]);
			string sessionGroupName = Session["GroupName"].ToString();
			if(sessionGroupName.CompareTo(Group.SUPERUSER)==0)
			{
				return Administration.Administration .AddLabServer (labServerName, labServerGUID, webServiceURL, labServerDescription, labInfoURL, contactEmail, firstName, lastName);
			}
			else
			{
				throw new AccessDeniedException ("Access denied adding labServer.");
			}
		}
        */
        /*
		/// <summary>
		/// super user access privilege
		/// </summary>
		/// <param name="labServerIDs"></param>
		/// <returns></returns>
		public int[] RemoveLabServersWrapper( int[] labServerIDs)
		{
			int sessionGroupID =  Convert.ToInt32(Session["GroupID"]);
			string sessionGroupName = Session["GroupName"].ToString();
			if(sessionGroupName.CompareTo(Group.SUPERUSER)==0)
			{
				return Administration.Administration .RemoveLabServers (labServerIDs);
			}
			else
			{
				throw new AccessDeniedException ("Access denied removing labServer.");
			}

		}
        */
        /*
		/// <summary>
		/// super user access privilege
		/// </summary>
		/// <param name="labServerID"></param>
		/// <param name="labServerName"></param>
		/// <param name="labServerGUID"></param>
		/// <param name="webServiceURL"></param>
		/// <param name="labServerDescription"></param>
		/// <param name="labInfoURL"></param>
		/// <param name="contactEmail"></param>
		/// <param name="contactFirstName"></param>
		/// <param name="contactLastName"></param>
		public void ModifyLabServerWrapper (int labServerID, string labServerName, string labServerGUID, string webServiceURL, string labServerDescription, string labInfoURL, string contactEmail, string contactFirstName, string contactLastName)
		{
			int sessionGroupID = Convert.ToInt32(Session["GroupID"]);
			string sessionGroupName = Session["GroupName"].ToString();
			if(sessionGroupName.CompareTo(Group.SUPERUSER)==0)
			{
				Administration.Administration .ModifyLabServer (labServerID, labServerName,  labServerGUID, webServiceURL, labServerDescription, labInfoURL,  contactEmail,contactFirstName,  contactLastName);
			}
			else
			{
				throw new AccessDeniedException ("Access denied modifying labServer.");
			}

		}
        */
		/// <summary>
		/// anyone access
		/// </summary>
		/// <returns></returns>
		public int[] ListLabServerIDsWrapper()
		{
			return new ProcessAgentDB().GetProcessAgentIDsByType((int) ProcessAgentType.AgentType.LAB_SERVER);
		}
/*
		/// <summary>
		/// anyone access
		/// </summary>
		/// <param name="labServerIDs"></param>
		/// <returns></returns>
		public LabServer[] GetLabServersWrapper(int[] labServerIDs)
		{
			return Administration.Administration .GetLabServers (labServerIDs);
		}
*/
		/// <summary>
		/// anyone access
		/// </summary>
		/// <param name="labServerGUID"></param>
		/// <returns></returns>
		public int GetLabServerIDWrapper (string labServerGUID)
		{
			return Administration.AdministrativeAPI.GetLabServerID(labServerGUID);
		}
		

// SERVICES 	
			
		/// <summary>
		/// super user access privilege
		/// </summary>
		/// <param name="agentGuid"></param>
		/// <param name="name"></param>
		/// <param name="type"></param>
		/// <param name="issuerGuid"></param>
		/// <param name="codeBaseUrl"></param>
		/// <param name="webServiceUrl"></param>
		/// <param name="inCoupon"></param>
		/// <param name="outCoupon"></param>
		/// <returns>LabServerID</returns>
		public int AddProcessAgentWrapper (string agentGuid, string name, string type, string domainGuid, string codeBaseUrl, string webServiceURL, Coupon inCoupon,Coupon outCoupon)
		{
			AuthenticateSuperuser("Access denied adding ProcessAgent.");
				return  Administration.AdministrativeAPI.AddProcessAgent(agentGuid, name,type, domainGuid, 
					codeBaseUrl, webServiceURL, inCoupon, outCoupon);
			
		}

		/// <summary>
		/// super user access privilege
		/// </summary>
		/// <param name="labServerIDs"></param>
		/// <returns></returns>
		public int[] RemoveProcessAgentWrapper( int[] agentIDs)
		{
			AuthenticateSuperuser("Access denied removing processAgent.");
             return Administration.AdministrativeAPI.RemoveProcessAgents(agentIDs);
		}

		/// <summary>
		/// super user access privilege
		/// </summary>
		/// <param name="labServerID"></param>
		/// <param name="labServerName"></param>
		/// <param name="labServerGUID"></param>
		/// <param name="webServiceURL"></param>
		/// <param name="labServerDescription"></param>
		/// <param name="labInfoURL"></param>
		/// <param name="contactEmail"></param>
		/// <param name="contactFirstName"></param>
		/// <param name="contactLastName"></param>
		public void ModifyProcessAgentWrapper ( string GUID, string name, string type, string domain, string applicationURL, string webServiceURL )
		{
			AuthenticateSuperuser("Access denied modifying processAgent.");
			new ProcessAgentDB().UpdateProcessAgent ( GUID, name, type, domain, applicationURL,  webServiceURL);
		}
/*
		/// <summary>
		/// anyone access
		/// </summary>
		/// <returns></returns>
		public long[] ListProcessAgentIDsWrapper()
		{
			return Administration.Administration .ListProcessAgentIDs ();
		}
*/
	
		/*
		/// <summary>
		/// anyone access
		/// </summary>
		/// <returns></returns>
		public long[] ListProcessAgentIDsWrapper(string type)
		{
			return Administration.Administration.ListProcessAgentIDs (type);
		}
*/

		/// <summary>
		/// anyone access
		/// </summary>
		/// <param name="labServerIDs"></param>
		/// <returns></returns>
		public ProcessAgentInfo[] GetProcessAgentInfosWrapper()
		{
			ProcessAgentDB ticketing = new ProcessAgentDB();
			return ticketing.GetProcessAgentInfos();
		}

		/// <summary>
		/// anyone access
		/// </summary>
		/// <param name="labServerIDs"></param>
		/// <returns></returns>
		public ProcessAgentInfo[] GetProcessAgentInfosWrapper(string type)
		{
			ProcessAgentDB ticketing = new ProcessAgentDB();
			return ticketing.GetProcessAgentInfos(type);
		}

		/// <summary>
		/// anyone access
		/// </summary>
		/// <param name="labServerIDs"></param>
		/// <returns></returns>
		public ProcessAgentInfo[] GetProcessAgentInfosWrapper(int[] ids)
		{
			ProcessAgentDB ticketing = new ProcessAgentDB();
			return ticketing.GetProcessAgentInfos(ids);
		}

		/// <summary>
		/// anyone access
		/// </summary>
		/// <param name="labServerGUID"></param>
		/// <returns></returns>
		public int GetProcessAgentIDWrapper (string agentGUID)
		{
			return new ProcessAgentDB().GetProcessAgentID(agentGUID);
		}
		
		public IntTag[] GetProcessAgentTagsByTypesWrapper (int [] types)
		{
            ProcessAgentDB db = new ProcessAgentDB();
			return db.GetProcessAgentTagsByType(types);
		}

        public IntTag[] GetProcessAgentTagsByTypeWrapper(string agentType)
		{
            ProcessAgentDB db = new ProcessAgentDB();
			return db.GetProcessAgentTagsByType(agentType);
		}
		
				
/* 
		/// <summary>
		/// super user privilege
		/// </summary>
		/// <param name="labServerID"></param>
		/// <returns></returns>
		public string GenerateIncomingServerPasskeyWrapper(int labServerID)
		{
			int sessionGroupID =  Convert.ToInt32(Session["GroupID"]);
			string sessionGroupName = Session["GroupName"].ToString();
			if(sessionGroupName.CompareTo(Group.SUPERUSER)==0)
			{
				return Administration.Administration .GenerateIncomingServerPasskey (labServerID);
			}
			else
			{
				throw new AccessDeniedException ("Access denied generating incoming server passkey.");
			}
		}

		/// <summary>
		/// super user privilege
		/// </summary>
		/// <param name="labServerID"></param>
		/// <returns></returns>
		public string GetIncomingServerPasskeyWrapper(int labServerID)
		{
			int sessionGroupID = Convert.ToInt32(Session["GroupID"]);
			string sessionGroupName = Session["GroupName"].ToString();
			if(sessionGroupName.CompareTo(Group.SUPERUSER)==0)
			{
				return Administration.Administration .GetIncomingServerPasskey (labServerID);
			}
			else
			{
				throw new AccessDeniedException ("Access denied getting incoming server passkey.");
			}

		}

		/// <summary>
		/// super user privilege
		/// </summary>
		/// <param name="labServerID"></param>
		/// <param name="passkey"></param>
		public void RegisterOutgoingServerPasskeyWrapper(int labServerID, string passkey)
		{
			int sessionGroupID = Convert.ToInt32(Session["GroupID"]);
			string sessionGroupName = Session["GroupName"].ToString();
			if(sessionGroupName.CompareTo(Group.SUPERUSER)==0)
			{
				Administration.Administration .RegisterOutgoingServerPasskey (labServerID, passkey);
			}
			else
			{
				throw new AccessDeniedException ("Access denied registering outgoing server passkey.");
			}
		}

		/// <summary>
		/// super user privilege
		/// </summary>
		/// <param name="labServerID"></param>
		/// <returns></returns>
		public string GetOutgoingServerPasskeyWrapper(int labServerID)
		{
			int sessionGroupID = Convert.ToInt32(Session["GroupID"]);
			string sessionGroupName = Session["GroupName"].ToString();

			if(sessionGroupName.CompareTo(Group.SUPERUSER)==0)
			{
				return Administration.Administration .GetOutgoingServerPasskey (labServerID);
			}
			else
			{
				throw new AccessDeniedException ("Access denied getting outgoing server passkey.");
			}
		}
*/
		/// <summary>
		/// super user privilege
		/// </summary>
        /// <param name="clientGuid"></param>
		/// <param name="clientName"></param>
		/// <param name="version"></param>
		/// <param name="clientShortDescription"></param>
		/// <param name="clientLongDescription"></param>
		/// <param name="notes"></param>
		/// <param name="loaderScript"></param>
		/// <param name="labServerIDs"></param>
		/// <param name="contactEmail"></param>
		/// <param name="contactFirstName"></param>
		/// <param name="contactLastName"></param>
		/// <param name="clientInfos"></param>
		/// <returns></returns>
        public int AddLabClientWrapper(string clientGuid, string clientName, string version, string clientShortDescription,
            string clientLongDescription, string clientType, string loaderScript, string documentationURL,
            string contactEmail, string contactFirstName, string contactLastName, string notes,
            bool needsESS, bool needsScheduling, bool isReentrant)
		{
			AuthenticateSuperuser("Access denied adding Lab Client.");
			return Administration.AdministrativeAPI .AddLabClient (clientGuid, clientName,  version, clientShortDescription,
                    clientLongDescription, clientType, loaderScript, documentationURL, contactEmail, contactFirstName,
                    contactLastName, notes, needsESS, needsScheduling, isReentrant);
		}

		/// <summary>
		/// super user privilege
		/// </summary>
		/// <param name="labClientIDs"></param>
		/// <returns></returns>
		public int[] RemoveLabClientsWrapper (int[] labClientIDs)
		{
			AuthenticateSuperuser("Access denied removing lab clients.");
			return Administration.AdministrativeAPI .RemoveLabClients (labClientIDs);
			
		}

		/// <summary>
		/// super user privilege
		/// </summary>
		/// <param name="clientID"></param>
		/// <param name="clientName"></param>
		/// <param name="version"></param>
		/// <param name="clientShortDescription"></param>
		/// <param name="clientLongDescription"></param>
		/// <param name="notes"></param>
		/// <param name="loaderScript"></param>
		/// <param name="labServerIDs"></param>
		/// <param name="contactEmail"></param>
		/// <param name="contactFirstName"></param>
		/// <param name="contactLastName"></param>
		/// <param name="clientInfos"></param>
		public void ModifyLabClientWrapper (int clientID, string clientGuid, string clientName, string version, string clientShortDescription,
            string clientLongDescription, string clientType, string loaderScript, string documentationURL,
            string contactEmail, string contactFirstName, string contactLastName, string notes,
            bool needsESS, bool needsScheduling, bool isReentrant)
		{
			AuthenticateSuperuser("Access denied modifying lab client.");
            AdministrativeAPI.ModifyLabClient(clientID, clientGuid, clientName, version, clientShortDescription, clientLongDescription,
                    clientType, loaderScript, documentationURL, contactEmail, contactFirstName, contactLastName, notes, needsESS, needsScheduling, isReentrant);
			
		}

		/// <summary>
		/// anyone privilege
		/// </summary>
		/// <returns></returns>
		public int[] ListLabClientIDsWrapper()
		{
			return Administration.AdministrativeAPI .ListLabClientIDs ();
		}

		/// <summary>
		/// anyone access
		/// </summary>
		/// <param name="labClientIDs"></param>
		/// <returns></returns>
		public LabClient[] GetLabClientsWrapper(int[] labClientIDs)
		{
			return Administration.AdministrativeAPI .GetLabClients (labClientIDs);
		}

		/// <summary>
		/// addMember, adminsterGroup and superUser privilege
		/// </summary>
		/// <param name="userName"></param>
		/// <param name="principalString"></param>
		/// <param name="authenticationType"></param>
		/// <param name="firstName"></param>
		/// <param name="lastName"></param>
		/// <param name="email"></param>
		/// <param name="affiliation"></param>
		/// <param name="reason"></param>
		/// <param name="xmlExtension"></param>
		/// <param name="initialGroupID"></param>
		/// <param name="lockAccount"></param>
		/// <returns></returns>
		public int AddUserWrapper (string userName, int authID, int authenticationTypeID, string firstName, string lastName, string email, string affiliation,string reason, string xmlExtension, int initialGroupID, bool lockAccount)
		{
			int loginUserID = Convert.ToInt32(Session["UserID"]);

			if(IsSuperuserGroup())
			{
				return AdministrativeAPI .AddUser ( userName,  authID, authenticationTypeID, firstName,  lastName, email,  affiliation, reason,xmlExtension,  initialGroupID,  lockAccount);
			}
			else
			{
				// can only add user to group that you have permission for
				int associatedGroupID = AdministrativeAPI.GetAssociatedGroupID(Convert.ToInt32(Session["GroupID"]));
				if (associatedGroupID > 0)
				{
					int qID = AuthorizationAPI .GetQualifierID (associatedGroupID, Qualifier .groupQualifierTypeID );
					if ((AuthorizationAPI .CheckAuthorization (loginUserID, Function .administerGroupFunctionType , qID))||(Authorization.AuthorizationAPI.CheckAuthorization(loginUserID, Function .addMemberFunctionType , qID)))
					{
						return AdministrativeAPI .AddUser ( userName,  authID, authenticationTypeID, firstName,  lastName, email,  affiliation, reason,xmlExtension,  associatedGroupID,  lockAccount);
					}
					else
						throw new AccessDeniedException ("Access denied adding users.");
				}
				else throw new Exception ("Cannot add user.");
			}
		}

		/// <summary>
		/// superUser and administerGroup privilege
		/// Note: if one has administerGroup privileges
		/// then one can only delete members from all groups one has permission to manage
		/// </summary>
		/// <param name="userIDs"></param>
		/// <returns></returns>
		public int[] RemoveUsersWrapper (int[] userIDs)
		{
			
			int loginUserID = Convert.ToInt32(Session["UserID"]);

			if(IsSuperuserGroup())
			{
				return AdministrativeAPI .RemoveUsers (userIDs);
			}
			else
			{
                int sessionGroupID = Convert.ToInt32(Session["GroupID"]);
				//current implementation
				//deletes each user from the group (& subgroups of that group) that one is administering

				ArrayList notRemovedList = new ArrayList();

				foreach (int userID in userIDs)
				{
					// get groups the user belongs to
					int[] userGroups = AdministrativeAPI.ListGroupIDsForUser(userID);

					ArrayList groupsToBeRemovedFrom = new ArrayList();
					
					foreach (int groupID in userGroups)
					{
						int qID = AuthorizationAPI .GetQualifierID (groupID, Qualifier .groupQualifierTypeID );
						if (AuthorizationAPI.CheckAuthorization(sessionGroupID, Function.administerGroupFunctionType, qID))
						{
							groupsToBeRemovedFrom.Add(groupID);
						}
					}

					if (groupsToBeRemovedFrom.Count>0)
						foreach (int groupID in groupsToBeRemovedFrom)
                            AdministrativeAPI.RemoveUserFromGroup(userID, groupID);
					else
						notRemovedList.Add(userID);
				}

				if (notRemovedList.Count == userIDs.Length)
					throw new AccessDeniedException ("Access denied removing users.");
				else
					return Utilities.ArrayListToIntArray(notRemovedList);
			}
		}

		/// <summary>
		/// superUser and owner privilege
		/// </summary>
		/// <param name="userID"></param>
		/// <param name="userName"></param>
		/// <param name="principalString"></param>
		/// <param name="authenticationType"></param>
		/// <param name="firstName"></param>
		/// <param name="lastName"></param>
		/// <param name="email"></param>
		/// <param name="affiliation"></param>
		/// <param name="reason"></param>
		/// <param name="xmlExtension"></param>
		/// <param name="lockAccount"></param>
		/// <returns></returns>
		public void ModifyUserWrapper (int userID,string userName, int authorityID, int authenticationTypeID, string firstName, string lastName, string email, string affiliation, string reason,string xmlExtension, bool lockAccount)
		{
			int loginUserID = Convert.ToInt32(Session["UserID"]);
			if ((IsSuperuserGroup())||(loginUserID.CompareTo(userID)==0))
			{
				 Administration.AdministrativeAPI.ModifyUser( userID, userName, authorityID, authenticationTypeID, firstName,  lastName, email,  affiliation,  reason, xmlExtension, lockAccount);
			}
			else
			{
				throw new AccessDeniedException ("Access denied modifying user.");
			}
		}

		/// <summary>
		/// superUser and administerGroup Access
		/// </summary>
		/// <returns></returns>
		public int[] ListUserIDsWrapper()
		{
			
			int loginUserID = Convert.ToInt32(Session["UserID"].ToString());

			try
			{
				if(IsSuperuserGroup())
				{
					return Administration.AdministrativeAPI.ListUserIDs();
				}
				else
				{
					bool haveAccess=false;
					int groupID = Convert.ToInt32(Session["GroupID"]);
					
					// get users of the course that you have adminster group privileges for

					//get associated group
					int mainGroupID = Administration.AdministrativeAPI.GetAssociatedGroupID(groupID);
					
					if (mainGroupID>0)
					{
						int qID = Authorization.AuthorizationAPI .GetQualifierID (mainGroupID, Qualifier .groupQualifierTypeID );
						if(Authorization.AuthorizationAPI .CheckAuthorization (loginUserID, Function .administerGroupFunctionType , qID))
						{
							haveAccess=true;
						}
					}
				
					if (haveAccess)
						//get users in the associated group
						return Administration.AdministrativeAPI.ListUserIDsInGroupRecursively(mainGroupID);
					else throw new AccessDeniedException("Access to user list denied.");
				}
			}
			catch (Exception ex)
			{
				throw;
			}
		}	

		/// <summary>
		/// superUser privilege
		/// </summary>
		/// <returns></returns>
		public int[] ListOrphanedUserIDsWrapper()
		{
			if(IsSuperuserGroup())
			{
				return Administration.AdministrativeAPI .ListOrphanedUserIDs ();
			}
			else
			{
				throw new AccessDeniedException ("Access denied listing orphaned user.");
			}
		}

		/// <summary>
		/// anyone access
		/// </summary>
		/// <param name="userIDs"></param>
		/// <returns></returns>
		public User[] GetUsersWrapper(int[] userIDs)
		{
			return Administration.AdministrativeAPI .GetUsers (userIDs);
		}

		/// <summary>
		/// anyone access
		/// </summary>
		/// <param name="userName"></param>
		/// <returns></returns>
		public int GetUserIDWrapper (string userName, int authID)
		{
			return Administration.AdministrativeAPI.GetUserID(userName,authID);
		}

		/// <summary>
		/// superUser or administerGroup privilege
		/// </summary>
		/// <param name="groupName"></param>
		/// <param name="parentGroupID"></param>
		/// <param name="description"></param>
		/// <param name="email"></param>
		/// <returns></returns>
		public int AddGroupWrapper (string groupName, int parentGroupID, string description, string email, string groupType, int associatedGroupID)
		{
			bool haveAccess = false;
			
			//string sessionGroupName = Session["GroupName"].ToString();
			if(IsSuperuserGroup())
			{
				haveAccess = true;
			}
			else
			{
                int sessionGroupID = Convert.ToInt32(Session["GroupID"]);
				// can add request groups
				if ((groupType.Equals(GroupType.REQUEST))&&(parentGroupID == Administration.AdministrativeAPI.GetGroupID(Group.NEWUSERGROUP)))
					haveAccess = true;
				else
				{
					// add group to one you have permission to administer
					int qID = Authorization.AuthorizationAPI .GetQualifierID (parentGroupID, Qualifier .groupQualifierTypeID );
					if(Authorization.AuthorizationAPI .CheckAuthorization (sessionGroupID, Function .administerGroupFunctionType , qID))
					{
						haveAccess = true;
					}
				}
			}

			if (haveAccess)
				return Administration.AdministrativeAPI .AddGroup (groupName, parentGroupID, description, email, groupType, associatedGroupID);
			else throw new AccessDeniedException ("Access denied adding group.");
		}

		/// <summary>
		/// superUser and administerGroup privilege
		/// </summary>
		/// <param name="groupID"></param>
		/// <param name="groupName"></param>
		/// <param name="description"></param>
		/// <param name="email"></param>
		public void ModifyGroupWrapper(int groupID,string groupName, string description, string email)
		{
			//int sessionGroupID = Convert.ToInt32(Session["GroupID"]);
			//string sessionGroupName = Session["GroupName"].ToString();
			if(IsSuperuserGroup())
			{
				Administration.AdministrativeAPI .ModifyGroup (groupID,groupName,description, email);
			}
			else
			{
				// modify group if you have permission to administer it
                int sessionGroupID = Convert.ToInt32(Session["GroupID"]);
				int qID = Authorization.AuthorizationAPI .GetQualifierID (groupID, Qualifier .groupQualifierTypeID );
				if(Authorization.AuthorizationAPI .CheckAuthorization (sessionGroupID, Function .administerGroupFunctionType , qID))
				{
					Administration.AdministrativeAPI .ModifyGroup (groupID,groupName,description, email);
				}
				else
					throw new AccessDeniedException ("Access denied modifying group.");
			}
		}

		/// <summary>
		/// superUser and administer group privilege
		/// </summary>
		/// <param name="groupIDs"></param>
		/// <returns></returns>
		public int[] RemoveGroupsWrapper(int[] groupIDs)
		{
			//int sessionGroupID = Convert.ToInt32(Session["GroupID"]);
			//string sessionGroupName = Session["GroupName"].ToString();
			if(IsSuperuserGroup())
			{
				return Administration.AdministrativeAPI .RemoveGroups (groupIDs);
			}
			else
			{
                int sessionGroupID = Convert.ToInt32(Session["GroupID"]);
				ArrayList removeGroups = new ArrayList();
				ArrayList notRemoved = new ArrayList();
				// remove groups to one you have permission to administer
				foreach (int groupID in groupIDs)
				{
					int qID = Authorization.AuthorizationAPI .GetQualifierID (groupID, Qualifier .groupQualifierTypeID );
					if(Authorization.AuthorizationAPI .CheckAuthorization (sessionGroupID, Function .administerGroupFunctionType , qID))
					{
						removeGroups.Add(groupID);
					}
					else
						notRemoved.Add(groupID);
				}
				if (removeGroups.Count>0)
				{
					int[] unremovedGroups = Administration.AdministrativeAPI.RemoveGroups(Utilities.ArrayListToIntArray(removeGroups));
					foreach (int g in unremovedGroups)
						notRemoved.Add(g);
					return Utilities.ArrayListToIntArray(notRemoved);
				}
				else
					throw new AccessDeniedException ("Access denied removing groups.");
			}
		}

		/// <summary>
		/// anyone access
		/// </summary>
		/// <returns></returns>
		public int[] ListGroupIDsWrapper()
		{
			return Administration.AdministrativeAPI .ListGroupIDs ();
		}

        /// <summary>
        /// anyone access
        /// </summary>
        /// <returns></returns>
        public int[] ListGroupIDsByType(string typeName)
        {
            return Administration.AdministrativeAPI.ListGroupIDsByType(typeName);
        }

		/// <summary>
		/// administerGroup, superUser privilege
		/// </summary>
		/// <param name="groupIDs"></param>
		/// <returns></returns>
		public Group[]  GetGroupsWrapper(int[] groupIDs)
		{
			if (groupIDs.Length>0)
			{
				//string sessionGroupName = Session["GroupName"].ToString();
				int loginUserID = Convert.ToInt32(Session["UserID"]);
			
				Group[] allowedGroups;

				if(IsSuperuserGroup())
				{
					allowedGroups= Administration.AdministrativeAPI .GetGroups (groupIDs);
				}
				else
				{
                    int sessionGroupID = Convert.ToInt32(Session["GroupID"]);
					// if you have permission to access group listing (i.e you are part of course staff)
					ArrayList allowedGroupList = new ArrayList();

					foreach (int groupID in groupIDs)
					{
						// If the group you're logged in has permission to access the group then display it.
						
						if (! (groupID == Administration.AdministrativeAPI.GetGroupID(Group.NEWUSERGROUP)))

						{
                            int qID = Authorization.AuthorizationAPI.GetQualifierID(groupID, Qualifier.groupQualifierTypeID);
							if(Authorization.AuthorizationAPI .CheckAuthorization (sessionGroupID, Function .administerGroupFunctionType , qID))
							{
								allowedGroupList.Add(groupID);
							}
						}
						else
						{
							// checking for request groups of course staff.
							//add new user to allowedgroup list if there exist a request group
							int[] newUserSubGroups = Administration.AdministrativeAPI.ListSubgroupIDs(groupID);
							foreach (int sgID in newUserSubGroups)
							{
								int sqID = Authorization.AuthorizationAPI .GetQualifierID (sgID, Qualifier .groupQualifierTypeID );
								if(Authorization.AuthorizationAPI .CheckAuthorization (sessionGroupID, Function .administerGroupFunctionType , sqID))
								{
									allowedGroupList.Add(groupID);
									break;
								}
							}
						}
					}

					allowedGroups = Administration.AdministrativeAPI.GetGroups(Utilities.ArrayListToIntArray(allowedGroupList));
				}

				if ((allowedGroups!=null)&&(allowedGroups.Length>0))
				{
					return allowedGroups;
				}
				else
				{
					throw new AccessDeniedException ("Access denied getting groups.");
				}
			}
			else
				return new Group[0];
		}

		/// <summary>
		/// anyone privilege
		/// </summary>
		/// <param name="groupName"></param>
		/// <returns></returns>
		public int GetGroupIDWrapper(string groupName)
		{
			return Administration.AdministrativeAPI.GetGroupID(groupName);
		}

		/// <summary>
		/// anyone privilege
		/// </summary>
		/// <param name="groupID"></param>
		/// <returns></returns>
		public int GetAssociatedGroupIDWrapper(int groupID)
		{
			return Administration.AdministrativeAPI.GetAssociatedGroupID(groupID);
		}

		/// <summary>
		/// administerGroup, addMemberToGroup and superUser privilege
		/// </summary>
		/// <param name="memberID"></param>
		/// <param name="groupID"></param>
		/// <returns></returns>
		public bool AddGroupToGroupWrapper(int memberID, int groupID)
		{
			/* Check to prevent adding subGroups to superUser Group */
			if (groupID == GetGroupIDWrapper(Group.SUPERUSER))
			{
				throw new Exception ("Cannot add subgroups to the SuperUser Group.");
			}
		
			int sessionGroupID = Convert.ToInt32(Session["GroupID"]);
			string sessionGroupName = Session["GroupName"].ToString();
			int userID = Convert.ToInt32(Session["UserID"]);

			bool haveAccess = false;
			if(IsSuperuserGroup())
			{
				haveAccess = true;
			}
			else
			{
				int qID = AuthorizationAPI .GetQualifierID (groupID, Qualifier .groupQualifierTypeID );
				if ((AuthorizationAPI .CheckAuthorization (userID, Function .administerGroupFunctionType , qID))||(Authorization.AuthorizationAPI .CheckAuthorization (userID, Function .addMemberFunctionType , qID)))
					haveAccess = true;
			}
			
			if (haveAccess)
			{
				return AdministrativeAPI .AddGroupToGroup (memberID, groupID);
			}
			else
			{
				throw new AccessDeniedException ("Access denied adding member.");
			}
		}

        /// <summary>
        /// administerGroup, addMemberToGroup and superUser privilege
        /// </summary>
        /// <param name="memberID"></param>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public bool MoveGroupToGroupWrapper(int memberID, int fromID, int groupID)
        {
            /* Check to prevent adding subGroups to superUser Group */
            if (IsSuperuserGroup(groupID))
            {
               throw new Exception("Cannot add subgroups to the SuperUser Group.");
            }

            //int sessionGroupID = Convert.ToInt32(Session["GroupID"]);
            //string sessionGroupName = Session["GroupName"].ToString();
            int userID = Convert.ToInt32(Session["UserID"]);

            bool haveAccess = false;
            if (IsSuperuserGroup())
            {
                haveAccess = true;
            }
            else
            {
                int qID = Authorization.AuthorizationAPI.GetQualifierID(groupID, Qualifier.groupQualifierTypeID);
                if ((Authorization.AuthorizationAPI.CheckAuthorization(userID, Function.administerGroupFunctionType, qID)) || (Authorization.AuthorizationAPI.CheckAuthorization(userID, Function.addMemberFunctionType, qID)))
                    haveAccess = true;
            }

            if (haveAccess)
            {
                return Administration.AdministrativeAPI.MoveGroupToGroup(memberID, fromID, groupID);
            }
            else
            {
                throw new AccessDeniedException("Access denied adding member.");
            }
        }

        ///// <summary>
        ///// superUser and administerGroup privilege
        ///// </summary>
        ///// <param name="memberIDs"></param>
        ///// <param name="groupID"></param>
        ///// <returns></returns>
        //public int[] RemoveMembersFromGroupWrapper(int[] memberIDs, int groupID)
        //{
        //    int sessionGroupID = Convert.ToInt32(Session["GroupID"]);
        //    string sessionGroupName = Session["GroupName"].ToString();
        //    int userID = Convert.ToInt32(Session["UserID"]);

        //    bool haveAccess = false;
        //    if(sessionGroupName.CompareTo(Group.SUPERUSER)==0)
        //    {
        //        haveAccess = true;
        //    }
        //    else
        //    {
        //        int qID = Authorization.AuthorizationAPI .GetQualifierID (groupID, Qualifier .groupQualifierTypeID );
        //        if(Authorization.AuthorizationAPI .CheckAuthorization (userID, Function .administerGroupFunctionType , qID))
        //        {
        //            haveAccess = true;
        //        }
        //    }

        //    if (haveAccess)
        //    {
        //        return Administration.AdministrativeAPI .RemoveMembersFromGroup (memberIDs, groupID);
        //    }
        //    else
        //    {
        //        throw new AccessDeniedException ("Access denied removing member from group.");
        //    }
        //}

		/// <summary>
		/// superUser and administerGroup privilege
		/// </summary>
		/// <param name="groupID"></param>
		/// <returns></returns>
		public int[] ListUserIDsInGroupWrapper(int groupID)
		{
			int sessionGroupID = Convert.ToInt32(Session["GroupID"]);
			string sessionGroupName = Session["GroupName"].ToString();
			int userID = Convert.ToInt32(Session["UserID"]);

			bool haveAccess = false;
			bool haveRequestGroupAccess = false;
			if(IsSuperuserGroup())
			{
				haveAccess = true;
			}
			else
			{
				int qID = Authorization.AuthorizationAPI .GetQualifierID (groupID, Qualifier .groupQualifierTypeID );
				if (! (groupID == Administration.AdministrativeAPI.GetGroupID(Group.NEWUSERGROUP)))

				{
					if(Authorization.AuthorizationAPI .CheckAuthorization (userID, Function .administerGroupFunctionType , qID))
					{
						haveAccess=true;
					}
				}
				else
				{
					// checking for request groups of course staff.
					//add new user to allowedgroup list if there exist a request group
					int[] newUserSubGroups = Administration.AdministrativeAPI.ListSubgroupIDs(groupID);
					foreach (int sgID in newUserSubGroups)
					{
						int sqID = Authorization.AuthorizationAPI .GetQualifierID (sgID, Qualifier .groupQualifierTypeID );
						if(Authorization.AuthorizationAPI .CheckAuthorization (sessionGroupID, Function .administerGroupFunctionType , sqID))
						{
							haveRequestGroupAccess = true;
							break;
						}
					}
				}

			}

			if (haveAccess)
			{
				return Administration.AdministrativeAPI .ListUserIDsInGroup (groupID);
			}
			else
			{
				if (haveRequestGroupAccess == true)
					return new int[] {};
				else
				{
					throw new AccessDeniedException ("Access denied listing userIDs from group.");
				}
			}
		}

		/// <summary>
		/// superUser and administerGroup privilege
		/// </summary>
		/// <param name="groupID"></param>
		/// <returns></returns>
		public int[] ListUserIDsInGroupRecursivelyWrapper(int groupID)
		{
			//int sessionGroupID = Convert.ToInt32(Session["GroupID"]);
			//string sessionGroupName = Session["GroupName"].ToString();
			int userID = Convert.ToInt32(Session["UserID"]);

			bool haveAccess = false;
			if(IsSuperuserGroup())
			{
				haveAccess = true;
			}
			else
			{
				int qID = Authorization.AuthorizationAPI .GetQualifierID (groupID, Qualifier .groupQualifierTypeID );
				if(Authorization.AuthorizationAPI .CheckAuthorization (userID, Function .administerGroupFunctionType , qID))
				{
					haveAccess=true;
				}
			}

			if (haveAccess)
			{
				return Administration.AdministrativeAPI .ListUserIDsInGroupRecursively (groupID);
			}
			else
			{
				throw new AccessDeniedException ("Access denied listing userIDs from group.");
			}
		}

		/// <summary>
		/// superUser and administerGroup privilege
		/// </summary>
		/// <param name="groupID"></param>
		/// <returns></returns>
		public int[] ListSubgroupIDsWrapper(int groupID)
		{
			//int sessionGroupID = Convert.ToInt32(Session["GroupID"]);
			//string sessionGroupName = Session["GroupName"].ToString();
			int userID = Convert.ToInt32(Session["UserID"]);

			bool haveAccess = false;
			if(IsSuperuserGroup())
			{
				haveAccess = true;
			}
			else
			{
                int sessionGroupID = Convert.ToInt32(Session["GroupID"]);
				int qID = AuthorizationAPI .GetQualifierID (groupID, Qualifier .groupQualifierTypeID );
				if (! (groupID == AdministrativeAPI.GetGroupID(Group.NEWUSERGROUP)))

				{
					if(AuthorizationAPI.CheckAuthorization (userID, Function .administerGroupFunctionType , qID))
					{
						haveAccess=true;
					}
				}
				else
				{
					// checking for request groups of course staff.
					//add new user to allowedgroup list if there exist a request group
					int[] newUserSubGroups = AdministrativeAPI.ListSubgroupIDs(groupID);
					foreach (int sgID in newUserSubGroups)
					{
						int sqID = AuthorizationAPI .GetQualifierID (sgID, Qualifier .groupQualifierTypeID );
						if(AuthorizationAPI .CheckAuthorization (sessionGroupID, Function .administerGroupFunctionType , sqID))
						{
							haveAccess = true;
							break;
						}
					}
				}
			}
			
			if (haveAccess)
			{
				return AdministrativeAPI.ListSubgroupIDs (groupID);
			}
			else
			{
				//throw new AccessDeniedException ("Access denied listing subgroupIDs from group.");
                return null;
			}
		}

		/// <summary>
		/// superUser and administerGroup privilege
		/// </summary>
		/// <param name="groupID"></param>
		/// <returns></returns>
		public int[] ListSubgroupIDsRecursivelyWrapper(int groupID)
		{
			//int sessionGroupID = Convert.ToInt32(Session["GroupID"]);
			//string sessionGroupName = Session["GroupName"].ToString();
			int userID = Convert.ToInt32(Session["UserID"]);

			bool haveAccess = false;
			if(IsSuperuserGroup())
			{
				haveAccess = true;
			}
			else
			{
                int sessionGroupID = Convert.ToInt32(Session["GroupID"]);
				int qID = Authorization.AuthorizationAPI .GetQualifierID (groupID, Qualifier .groupQualifierTypeID );
				if (! (groupID == Administration.AdministrativeAPI.GetGroupID(Group.NEWUSERGROUP)))

				{
					if(Authorization.AuthorizationAPI .CheckAuthorization (userID, Function .administerGroupFunctionType , qID))
					{
						haveAccess=true;
					}
				}
				else
				{
					// checking for request groups of course staff.
					//add new user to allowedgroup list if there exist a request group
					int[] newUserSubGroups = Administration.AdministrativeAPI.ListSubgroupIDs(groupID);
					foreach (int sgID in newUserSubGroups)
					{
						int sqID = Authorization.AuthorizationAPI .GetQualifierID (sgID, Qualifier .groupQualifierTypeID );
						if(Authorization.AuthorizationAPI .CheckAuthorization (sessionGroupID, Function .administerGroupFunctionType , sqID))
						{
							haveAccess = true;
							break;
						}
					}
				}

			}

			if (haveAccess)
			{
				return Administration.AdministrativeAPI .ListSubgroupIDsRecursively (groupID);
			}
			else
			{
				//throw new AccessDeniedException ("Access denied listing subgroupIDs from group.");
                return null;
			}
		}

		/// <summary>
		/// superUser and administerGroup privilege
		/// </summary>
		/// <param name="groupID"></param>
		/// <returns></returns>
		public int[] ListGroupIDsInGroupWrapper(int groupID)
		{
			//int sessionGroupID = Convert.ToInt32(Session["GroupID"]);
			//string sessionGroupName = Session["GroupName"].ToString();
			

			bool haveAccess = false;
			if(IsSuperuserGroup())
			{
				haveAccess = true;
			}
			else
			{
                int userID = Convert.ToInt32(Session["UserID"]);
				int qID = AuthorizationAPI .GetQualifierID (groupID, Qualifier .groupQualifierTypeID);
				if (AuthorizationAPI .CheckAuthorization (userID, Function .administerGroupFunctionType , qID))
				{
					haveAccess = true;
				}
			}

			if (haveAccess)
			{
				return AdministrativeAPI.ListSubgroupIDs(groupID);
			}
			else
			{
				//throw new AccessDeniedException ("Access denied listing groupIDs in group.");
                return null;
			}
		}
      
		/// <summary>
		/// superUser and administerGroup privilege
		/// </summary>
		/// <param name="groupID"></param>
		/// <returns></returns>
		public int[] ListMemberIDsInGroupFromDSWrapper(int groupID)
		{
			//int sessionGroupID = Convert.ToInt32(Session["GroupID"]);
			//string sessionGroupName = Session["GroupName"].ToString();
			int userID = Convert.ToInt32(Session["UserID"]);

			bool haveAccess = false;
			if(IsSuperuserGroup())
			{
				haveAccess = true;
			}
			else
			{
				int qID = Authorization.AuthorizationAPI .GetQualifierID (groupID, Qualifier .groupQualifierTypeID );
				if (Authorization.AuthorizationAPI .CheckAuthorization (userID, Function .administerGroupFunctionType , qID))
				{
					haveAccess = true;
				}
			}

			if (haveAccess)
			{
				return InternalAdminDB.ListMemberIDsInGroupFromDS (groupID);
			}
			else
			{
				//throw new AccessDeniedException ("Access denied listing memberIDs in group.");
                return null;
			}
		}

		/// <summary>
		/// anyone access
		/// </summary>
		/// <param name="agentID"></param>
		/// <returns></returns>
		public int[] ListGroupsForUserWrapper(int agentID)
		{
			return Administration.AdministrativeAPI.ListGroupIDsForUser (agentID);
		}
        /// <summary>
        /// anyone access
        /// </summary>
        /// <param name="agentID"></param>
        /// <returns></returns>
        public int[] ListNonRequestGroupsForUserWrapper(int agentID)
        {
            return Administration.AdministrativeAPI.ListGroupIDsForUser(agentID);
        }

		/// <summary>
		/// anyone access
		/// </summary>
		/// <param name="agentID"></param>
		/// <returns></returns>
		public int[] ListGroupsForGroupRecursivelyWrapper(int agentID)
		{
			return AdministrativeAPI.ListParentGroupsForGroupRecursively (agentID);
		}

		/// <summary>
		/// anyone access
		/// </summary>
        /// <param name="groupID"></param>
		/// <param name="membeID"></param>
		/// <returns></returns>
		public bool IsGroupMemberWrapper( int groupID,int memberID)
		{
			return AdministrativeAPI.IsGroupMember (groupID, memberID);
		}


        /// <summary>
        /// anyone access
        /// </summary>
        /// <param name="groupID"></param>
        /// <param name="agentID"></param>
        /// <returns></returns>
        public bool IsUserMemberWrapper(int groupID, int userID)
        {
            return AdministrativeAPI.IsUserMember(groupID, userID);
        }

		/// <summary>
		/// administerGroup and superUser access  - currently implemented as anyone access
		/// </summary>
		/// <param name="userIDs"></param>
		/// <param name="message"></param>
		/// <returns></returns>
		public int[] NotifyUsersWrapper(int[] userIDs, string message)
		{
			////////////////////////////////////////////////////////////////////////////
			//Imad: implement authorization check --> only admnisterGroup and superUser/
			////////////////////////////////////////////////////////////////////////////
//			string userID = Session["UserID"].ToString();
//			string qID = Authorization.Authorization .SelectQualifierID (groupID, Qualifier .groupQualifierType );
//			if(CheckAuthorization(userID, Function.administerGroupFunctionType, qID)
//				||CheckAuthorization(userID, Function.superUserFunctionType, Qualifier.superUserQualifierID))
//			{
//				return Administration.Administration.NotifyUsers(userIDs, message);
//			}
//
//			else
//			{
//				throw new AccessDeniedException("Access denied notifying user.");
//			}

			// check authze not implemented yet
			return Administration.AdministrativeAPI.NotifyUsers (userIDs, message);
		}

		////////////////////////////////////////////////////////
		//Imad: implement the 4 methods related to Client items/
		////////////////////////////////////////////////////////
		//What to do with the "owner" Function? don't implement the methods?

		
		/*public string SaveClientItemValueWrapper(string clientID, string userID, string itemName, string itemValue)
		{
		}

		public string[] GetClientItemValueWrapper(string clientID, string userID, string[] itemNames)
		{
		}

		public string[] RemoveClientItemsWrapper(string clientID, string userID, string[] itemNames)
		{
		}

		public string[] ListClientItemsWrapper(string clientID, string userID)
		{
		}*/


		/// <summary>
		/// superUser, limited administerGroup privilege
		/// </summary>
		/// <param name="agentID"></param>
		/// <param name="function"></param>
		/// <param name="qualifierID"></param>
		/// <returns></returns>
		public int AddGrantWrapper( int agentID, string function, int qualifierID )
		{
			int sessionGroupID = Convert.ToInt32(Session["GroupID"]);
			//string sessionGroupName = Session["GroupName"].ToString();

			Qualifier q = AuthorizationAPI.GetQualifier(qualifierID);
			if ((q.qualifierName.Equals(Group.NEWUSERGROUP))||(q.qualifierType.Equals(Group.SUPERUSER))
				||(q.qualifierName.Equals(Group.ORPHANEDGROUP))||(q.qualifierName.Equals(Group.ROOT)))
				throw new AccessDeniedException ("Cannot add grant. Insufficient permission.");

			if(IsSuperuserGroup())
			{
				return Authorization.AuthorizationAPI .AddGrant (agentID, function, qualifierID);
			}
			else
			{
				bool haveAccess = false;
				// special case. give administrator group privilege to add request group grants
				int mainGroupID = AdministrativeAPI.GetAssociatedGroupID(sessionGroupID);
				if (mainGroupID >0)
				{
					int reqQualID = AuthorizationAPI.GetQualifierID(AdministrativeUtilities.GetGroupRequestGroup(mainGroupID),Qualifier.groupQualifierTypeID);
					if (qualifierID == reqQualID)
						haveAccess = true;
				}

				if ((sessionGroupID != agentID)&& (!haveAccess))
				{
					// whatever grant the administrator group has, can be assigned to 
					// all agents that it is administering
					// e.g.  If agent A has the “administer group”  privilege on the Q group, 
					//then any agent that belongs to A can assign any grant that they possess 
					//(i.e. from agent A over any function F, to Q group or subgroups).
					if(Authorization.AuthorizationAPI .CheckAuthorization (sessionGroupID, function , qualifierID))
						haveAccess = true;
					else
					{
						// if it has administer group privileges over the agent.
						int agentQualID = Authorization.AuthorizationAPI.GetQualifierID(agentID, Qualifier.groupQualifierTypeID);
						if (agentQualID >0)
							if (Authorization.AuthorizationAPI.CheckAuthorization(sessionGroupID, Function.administerGroupFunctionType,agentQualID))
								haveAccess = true;
					}
				}
				
				if (haveAccess)
					return Authorization.AuthorizationAPI.AddGrant(agentID,function, qualifierID);
				else
					throw new AccessDeniedException ("Access denied adding grant.");
			}
		}

		/// <summary>
		/// superUser privilege
		/// </summary>
		/// <param name="grantIDs"></param>
		/// <returns></returns>
		public int[] RemoveGrantsWrapper ( int[] grantIDs )
		{
			//int sessionGroupID = Convert.ToInt32(Session["GroupID"]);
			//string sessionGroupName = Session["GroupName"].ToString();

			if(IsSuperuserGroup())
			{
				return Authorization.AuthorizationAPI .RemoveGrants (grantIDs);
			}
			else
			{
                int sessionGroupID = Convert.ToInt32(Session["GroupID"]);
				ArrayList grantsToBeRemoved = new ArrayList();
				ArrayList notRemoved = new ArrayList();
				Grant[] grants = Authorization.AuthorizationAPI.GetGrants(grantIDs);
				
				foreach (Grant g in grants)
				{
					bool haveAccess = false;
					// cant' remove grants from itself
					if ((sessionGroupID != g.agentID))
					{
						// if it has administer group privileges over the agent.
						int agentQualID = Authorization.AuthorizationAPI.GetQualifierID(g.agentID, Qualifier.groupQualifierTypeID);
						if (agentQualID >0)
							if (Authorization.AuthorizationAPI.CheckAuthorization(sessionGroupID, Function.administerGroupFunctionType,agentQualID))
								haveAccess = true;
					}
					if (haveAccess)
						grantsToBeRemoved.Add(g.grantID);
					else
						notRemoved.Add(g.grantID);
				}
				
				if (grantsToBeRemoved.Count>0)
				{
					int[] unRemoved = Authorization.AuthorizationAPI.RemoveGrants(Utilities.ArrayListToIntArray(grantsToBeRemoved));
					foreach (int gID in unRemoved)
						notRemoved.Add(gID);
					return Utilities.ArrayListToIntArray(notRemoved);
				}
				else
					throw new AccessDeniedException ("Access denied removing grant.");
			}
		}

		/// <summary>
		/// anyone access
		/// </summary>
		/// <returns></returns>
		public int[] ListGrantIDsWrapper ()
		{
			return Authorization.AuthorizationAPI .ListGrantIDs ();
		}

		/// <summary>
		/// anyone access
		/// </summary>
		/// <param name="grantIDs"></param>
		/// <returns></returns>
		public Grant[] GetGrantsWrapper ( int[] grantIDs )
		{
			return Authorization.AuthorizationAPI .GetGrants (grantIDs);
		}

		/// <summary>
		/// anyone access
		/// </summary>
		/// <param name="agentID"></param>
		/// <param name="function"></param>
		/// <param name="qualifierID"></param>
		/// <returns></returns>
		public int[] FindGrantsWrapper (int agentID, string function, int qualifierID)
		{
			return Authorization.AuthorizationAPI .FindGrants (agentID, function, qualifierID);
		}

		/// <summary>
		/// superUser and owner privilege
		/// </summary>
		/// <param name="userID"></param>
		/// <param name="password"></param>
		/// <returns></returns>
		public bool SetNativePasswordWrapper (int userID, string password)
		{
            //int sessionGroupID = Convert.ToInt32(Session["GroupID"]);
            //string sessionGroupName = "";
            //if (sessionGroupID>0)
            //    sessionGroupName = Session["GroupName"].ToString();

			int loginUserID = Convert.ToInt32(Session["UserID"]);

			if((IsSuperuserGroup())||(loginUserID.CompareTo(userID)==0))
			{
				return Authentication.AuthenticationAPI .SetNativePassword (userID, password);
			}
			else
			{
				throw new AccessDeniedException ("Access denied setting password.");
			}
		}

        /// <summary>
        /// Checks for authorization, and inserts the serviceBroker administrative record for the experiment.
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="effectiveGroupID"></param>
        /// <returns></returns>
        public bool AllowExperimentWrapper(int status, int userID, int effectiveGroupID, int lsID, int clientID,
            int essID, DateTime start, long duration)
        {
            //Find experiment collection for the group
            int expCollQualID = Authorization.AuthorizationAPI.GetQualifierID(effectiveGroupID, Qualifier.experimentCollectionQualifierTypeID);

            //Explicit permission only
            //int[] grantIDs = Authorization.Authorization.FindGrants(effectiveGroupID,Function.createExperimentFunctionType,expCollQualID);
            //if ((grantIDs.Length>0)&&(grantIDs[0]>0))

            //Implicit permission included here
            //Check if user has write permission for the experiment collection (explicit or implicit)
            return Authorization.AuthorizationAPI.CheckAuthorization(effectiveGroupID, Function.createExperimentFunctionType, expCollQualID);
        }

		/// <summary>
		/// Checks for authorization, and inserts the serviceBroker administrative record for the experiment.
		/// </summary>
		/// <param name="userID"></param>
		/// <param name="effectiveGroupID"></param>
		/// <returns></returns>
		public long CreateExperimentWrapper(int status, int userID, int effectiveGroupID,int lsID,int clientID,
            int essID, DateTime start,long duration)
		{
			//Find experiment collection for the group
			int expCollQualID = Authorization.AuthorizationAPI.GetQualifierID(effectiveGroupID, Qualifier.experimentCollectionQualifierTypeID);
				
			//Explicit permission only
			//int[] grantIDs = Authorization.Authorization.FindGrants(effectiveGroupID,Function.createExperimentFunctionType,expCollQualID);
			//if ((grantIDs.Length>0)&&(grantIDs[0]>0))

			//Implicit permission included here
			//Check if user has write permission for the experiment collection (explicit or implicit)
			if (Authorization.AuthorizationAPI.CheckAuthorization(effectiveGroupID,Function.createExperimentFunctionType,expCollQualID))
			{
                return InternalDataDB.InsertExperiment( status, userID, effectiveGroupID,
                    lsID, clientID, essID, start, duration);
			}
			else
				throw (new Exception("Insufficient permission to create experiment. "));
		}
       /// <summary>
       /// Authenticates users permissions to save a record and forwards the request to the ESS
       /// </summary>
       /// <param name="loginUserID"></param>
       /// <param name="sessionGroupID"></param>
       /// <param name="experimentID"></param>
       /// <param name="submitter"></param>
       /// <param name="recordType"></param>
       /// <param name="contents"></param>
       /// <param name="xmlSearchable"></param>
       /// <param name="attributes"></param>
        public void SaveExperimentRecordWrapper(int loginUserID, int sessionGroupID, long experimentID, string submitter, string recordType, bool xmlSearchable, string contents, RecordAttribute[] attributes)
		{
            //int loginUserID = Convert.ToInt32(Session["UserID"]);
            //int sessionGroupID = Convert.ToInt32(Session["GroupID"]);
            //string sessionGroupName = Session["GroupName"].ToString();
			int userID = InternalDataDB.SelectExperimentOwner (experimentID);
			int effectiveGroupID = InternalDataDB.SelectExperimentGroup(experimentID);
	
			bool haveAccess = false;

			//superUser or owner check
			if(IsSuperuserGroup() ||(loginUserID==userID))
			{
				haveAccess = true;
			}
			else
			{
				//writeExperiment check

				// get qualifier ID of experimentID
				int qualifierID = Authorization.AuthorizationAPI .GetQualifierID ((int) experimentID, Qualifier .experimentQualifierTypeID );
				if (Authorization.AuthorizationAPI .CheckAuthorization (loginUserID, Function .writeExperimentFunctionType , qualifierID))
				{
					haveAccess = true;
				}
			}

			if (haveAccess)
			{
                DataStorage.DataStorageAPI.SaveExperimentRecord(experimentID, submitter, recordType, xmlSearchable, contents, attributes);
			}
			else
			{
				throw new AccessDeniedException ("Access denied saving experimentSpecification.");
			}
		}

/*
		/// <summary>
		/// superUser, owner and writeExperiment privilege
		/// </summary>
		/// <param name="experimentID"></param>
		/// <param name="labServerID"></param>
		/// <param name="experimentSpecification"></param>
		/// <param name="annotation"></param>
		public void SaveExperimentSpecificationWrapper(long experimentID,int labServerID, string experimentSpecification, string annotation)
		{
			int loginUserID = Convert.ToInt32(Session["UserID"]);
			int sessionGroupID = Convert.ToInt32(Session["GroupID"]);
			string sessionGroupName = Session["GroupName"].ToString();
			int userID = InternalAdminDB.SelectExperimentOwner (experimentID);
			int effectiveGroupID = InternalAdminDB.SelectExperimentGroup(experimentID);
	
			bool haveAccess = false;

			//superUser or owner check
			if((sessionGroupName.CompareTo(Group.SUPERUSER)==0)||(loginUserID==userID))
			{
				haveAccess = true;
			}
			else
			{
				//writeExperiment check

				// get qualifier ID of experimentID
				int qualifierID = Authorization.Authorization .GetQualifierID ((int) experimentID, Qualifier .experimentQualifierTypeID );
				if (Authorization.Authorization .CheckAuthorization (loginUserID, Function .writeExperimentFunctionType , qualifierID))
				{
					haveAccess = true;
				}
			}

			if (haveAccess)
			{
				ExperimentRecordsAPI .SaveExperimentSpecification(experimentID, userID, effectiveGroupID, labServerID, experimentSpecification, annotation);
			}
			else
			{
				throw new AccessDeniedException ("Access denied saving experimentSpecification.");
			}
		}
*/
        /*
		/// <summary>
		/// superUser, owner and readExperiment privilege
		/// </summary>
		/// <param name="experimentID"></param>
		/// <returns></returns>
		public string RetrieveExperimentSpecificationWrapper(long experimentID)
		{
			int loginUserID =Convert.ToInt32( Session["UserID"]);
			int sessionGroupID = Convert.ToInt32(Session["GroupID"]);
			string sessionGroupName = Session["GroupName"].ToString();
			int userID = InternalAdminDB.SelectExperimentOwner (experimentID);

			bool haveAccess = false;
			
			//superUser or owner check
			if ((sessionGroupName.CompareTo(Group.SUPERUSER)==0)||(loginUserID == userID))
			{
				haveAccess = true;
			}
			else
			{
				//readExperiment check

				// get qualifier ID of experimentID
				int qualifierID = Authorization.Authorization .GetQualifierID ((int) experimentID, Qualifier .experimentQualifierTypeID );
				if (Authorization.Authorization .CheckAuthorization (loginUserID, Function .readExperimentFunctionType , qualifierID))
				{
					haveAccess = true;
				}
			}

			if (haveAccess)
			{
				return ExperimentRecordsAPI .RetrieveExperimentSpecification (experimentID);
			}
			else
			{
				throw new AccessDeniedException ("Access denied retrieving experiment specification.");
			}
		}
@/		
		/// <summary>
		/// superUser, owner and writeExperiment privilege
		/// </summary>
		/// <param name="experimentID"></param>
		/// <param name="sReport"></param>
		public void SaveSubmissionReportWrapper (long experimentID, SubmissionReport sReport )
		{
			int loginUserID =Convert.ToInt32( Session["UserID"].ToString());
			int sessionGroupID = Convert.ToInt32(Session["GroupID"]);
			string sessionGroupName = Session["GroupName"].ToString();
			int userID = InternalAdminDB.SelectExperimentOwner (experimentID);

			bool haveAccess = false;
			//superUser or owner check
			if ((sessionGroupName.CompareTo(Group.SUPERUSER)==0)||(loginUserID == userID))
			{
				haveAccess = true;
			}
			else
			{
				//writeExperiment check

				// get qualifier ID of experimentID
				int qualifierID = Authorization.Authorization .GetQualifierID ((int) experimentID, Qualifier .experimentQualifierTypeID );

				if (Authorization.Authorization .CheckAuthorization (loginUserID, Function .writeExperimentFunctionType , qualifierID))
				{
					haveAccess = true;
				}
			}

			if (haveAccess)
			{
				ExperimentRecordsAPI .SaveSubmissionReport (experimentID, sReport);
			}
			else
			{
				throw new AccessDeniedException ("Access denied saving submission report.");
			}
		}			
*/
		/*
		/// <summary>
		/// superUser, owner and writeExperiment privilege
		/// </summary>
		/// <param name="experimentID"></param>
		/// <param name="rReport"></param>
		public void SaveResultReportWrapper (long experimentID, ResultReport rReport )
		{
			int loginUserID =Convert.ToInt32(Session["UserID"]);
			int sessionGroupID = Convert.ToInt32(Session["GroupID"]);
			string sessionGroupName = Session["GroupName"].ToString();
			int userID = InternalAdminDB.SelectExperimentOwner (experimentID);

			bool haveAccess = false;
			//superUser or owner check
			if ((sessionGroupName.CompareTo(Group.SUPERUSER)==0)||(loginUserID == userID))
			{
				haveAccess = true;
			}
			else
			{
				//writeExperiment check
	
				// get qualifier ID of experimentID
				int qualifierID = Authorization.Authorization .GetQualifierID ((int) experimentID, Qualifier .experimentQualifierTypeID );
				if (Authorization.Authorization .CheckAuthorization (loginUserID, Function .writeExperimentFunctionType , qualifierID))
				{
					haveAccess = true;
				}
			}

			if (haveAccess)
			{
				ExperimentRecordsAPI .SaveResultReport (experimentID, rReport);
			}
			else
			{
				throw new AccessDeniedException ("Access denied saving result report.");
			}
		}
		/*	
		/// <summary>
		/// superUser, owner and readExperiment privilege
		/// </summary>
		/// <param name="experimentID"></param>
		/// <returns></returns>
		public string RetrieveExperimentResultWrapper( long experimentID )
		{
			int loginUserID = Convert.ToInt32(Session["UserID"]);
			int sessionGroupID = Convert.ToInt32(Session["GroupID"]);
			string sessionGroupName = Session["GroupName"].ToString();
			int userID = InternalAdminDB.SelectExperimentOwner (experimentID);

			bool haveAccess = false;
			
			//superUser or owner check
			if ((sessionGroupName.CompareTo(Group.SUPERUSER)==0)||(loginUserID == userID))
			{
				haveAccess = true;
			}
			else
			{
				//readExperiment check

				// get qualifier ID of experimentID
				int qualifierID = Authorization.Authorization .GetQualifierID ((int) experimentID, Qualifier .experimentQualifierTypeID );
				if (Authorization.Authorization .CheckAuthorization (loginUserID, Function .readExperimentFunctionType , qualifierID))
				{
					haveAccess = true;
				}
			}

			if (haveAccess)
			{
				return ExperimentRecordsAPI .RetrieveExperimentResult (experimentID);
			}
			else
			{
				throw new AccessDeniedException ("Access denied retrieving experiment result.");
			}
		}
        */
        /*
		/// <summary>
		/// superUser, owner and writeExperiment
		/// </summary>
		/// <param name="experimentID"></param>
		/// <param name="labConfiguration"></param>
		/// <returns></returns>
		public void SaveLabConfigurationWrapper( long experimentID, string labConfiguration )
		{
			int loginUserID = Convert.ToInt32(Session["UserID"]);
			int sessionGroupID = Convert.ToInt32(Session["GroupID"]);
			string sessionGroupName = Session["GroupName"].ToString();
			int userID = InternalAdminDB.SelectExperimentOwner (experimentID);
	
			bool haveAccess = false;
			
			//superUser or owner check
			if ((sessionGroupName.CompareTo(Group.SUPERUSER)==0)||(loginUserID == userID))
			{
				haveAccess = true;
			}
			else
			{
				//writeExperiment check

				// get qualifier ID of experimentID
				int qualifierID = Authorization.Authorization .GetQualifierID ((int) experimentID, Qualifier .experimentQualifierTypeID );
				if (Authorization.Authorization .CheckAuthorization (loginUserID, Function .writeExperimentFunctionType , qualifierID))
				{
					haveAccess = true;
				}
			}

			if (haveAccess)
			{
				ExperimentRecordsAPI .SaveLabConfiguration (experimentID, labConfiguration);
			}
			else
			{
				throw new AccessDeniedException ("Access denied saving lab configuration.");
			}
		}
		*/	
        /*
		/// <summary>
		/// superUser, owner and readExperiment
		/// </summary>
		/// <param name="experimentID"></param>
		/// <returns></returns>
		public string RetrieveLabConfigurationWrapper( long experimentID )
		{
			int loginUserID = Convert.ToInt32(Session["UserID"]);
			int sessionGroupID = Convert.ToInt32(Session["GroupID"]);
			string sessionGroupName = Session["GroupName"].ToString();
			int userID = InternalAdminDB.SelectExperimentOwner (experimentID);
	
			bool haveAccess = false;
			
			//superUser or owner check
			if ((sessionGroupName.CompareTo(Group.SUPERUSER)==0)||(loginUserID == userID))
			{
				haveAccess = true;
			}
			else
			{
				//readExperiment check

				// get qualifier ID of experimentID
				int qualifierID = Authorization.Authorization .GetQualifierID ((int) experimentID, Qualifier .experimentQualifierTypeID );
				if (Authorization.Authorization .CheckAuthorization (loginUserID, Function .readExperimentFunctionType , qualifierID))
				{
					haveAccess = true;
				}
			}

			if (haveAccess)
			{
				return ExperimentRecordsAPI .RetrieveLabConfiguration (experimentID);
			}
			else
			{
				throw new AccessDeniedException ("Access denied retrieving lab configuration.");
			}
		}
        */



        /// <summary>
        /// Based on the User and group what actions are authorized for the experiment
        /// </summary>
        /// <param name="experimentID"></param>
        /// <returns>Bitmask 1=Read, 2=Write, 4=Delete</returns>
        public int GetExperimentAuthorizationWrapper(long experimentID)
        {
            int loginUserID = Convert.ToInt32(Session["UserID"]);
            int sessionGroupID = Convert.ToInt32(Session["GroupID"]);
            return GetExperimentAuthorizationWrapper(experimentID, loginUserID, sessionGroupID);
        }

        /// <summary>
        /// Based on the User and group what actions are authorized for the experiment
        /// </summary>
        /// <param name="experimentID"></param>
        /// <param name="loginUserID"></param>
        /// <param name="groupID"></param>
        /// <returns>Bitmask 1=Read, 2=Write, 4=Administer</returns>
        public int GetExperimentAuthorizationWrapper(long experimentID,int loginUserID, int groupID)
        {
            
            int userID = InternalDataDB.SelectExperimentOwner(experimentID);

            int haveAccess = 0;

            //superUser or owner check
            if (IsSuperuserGroup(groupID) || (loginUserID == userID))
            {
                haveAccess = 7; // All
            }
            else
            {
                //readExperiment check

                // get qualifier ID of experimentID
                int qualifierID = Authorization.AuthorizationAPI.GetQualifierID((int)experimentID, Qualifier.experimentQualifierTypeID);
                if (Authorization.AuthorizationAPI.CheckAuthorization(loginUserID, Function.readExperimentFunctionType, qualifierID))
                {
                    haveAccess = 1;
                }
                if (Authorization.AuthorizationAPI.CheckAuthorization(loginUserID, Function.writeExperimentFunctionType, qualifierID))
                {
                    haveAccess |= 2;
                }
            }

            return haveAccess;
        }


		/// <summary>
		/// superUser, owner and writeExperiment privilege
		/// </summary>
		/// <param name="experimentID"></param>
		/// <param name="annotation"></param>
		/// <returns></returns>
		public string SaveExperimentAnnotationWrapper ( long experimentID, string annotation )
		{
			int loginUserID = Convert.ToInt32(Session["UserID"]);
			int sessionGroupID = Convert.ToInt32(Session["GroupID"]);
            return SaveExperimentAnnotationWrapper(experimentID, annotation, loginUserID, sessionGroupID);
		}

        /// <summary>
        /// superUser, owner and writeExperiment privilege
        /// </summary>
        /// <param name="experimentID"></param>
        /// <param name="annotation"></param>
        /// <returns></returns>
        public string SaveExperimentAnnotationWrapper(long experimentID, string annotation, int loginUserID, int groupID)
        {
           
            int userID = InternalDataDB.SelectExperimentOwner(experimentID);

            bool haveAccess = false;
            //superUser or owner check
            if ((IsSuperuserGroup(groupID)) || (loginUserID == userID))
            {
                haveAccess = true;
            }
            else
            {
                //writeExperiment check

                // get qualifier ID of experimentID
                int qualifierID = Authorization.AuthorizationAPI.GetQualifierID((int)experimentID, Qualifier.experimentQualifierTypeID);
                if (Authorization.AuthorizationAPI.CheckAuthorization(loginUserID, Function.writeExperimentFunctionType, qualifierID))
                {
                    haveAccess = true;
                    
                }
                else // Test for group permissions
                {
                    int grpQualID = 0;
                    ExperimentAdminInfo expInfo = InternalDataDB.SelectExperimentAdminInfo(experimentID);
                    if (expInfo.groupID == groupID)
                    {
                        //Check for group collection rights
                        grpQualID = Authorization.AuthorizationAPI.GetQualifierID(groupID, Qualifier.experimentCollectionQualifierTypeID);
                        if (Authorization.AuthorizationAPI.CheckAuthorization(groupID, Function.writeExperimentFunctionType, grpQualID))
                        {
                            haveAccess = true;
                        }
                    }
                    else
                    {
                        // Check if group administer
                        int adminID = InternalAdminDB.SelectGroupAdminGroupID(expInfo.groupID);
                        if (adminID == groupID)
                        {
                            //Check for group collection rights
                            grpQualID = Authorization.AuthorizationAPI.GetQualifierID(expInfo.groupID, Qualifier.experimentCollectionQualifierTypeID);
                            if (Authorization.AuthorizationAPI.CheckAuthorization(groupID, Function.writeExperimentFunctionType, grpQualID))
                            {
                                haveAccess = true;
                            }
                        }
                    }

                }
            }

            if (haveAccess)
            {
                return InternalDataDB.SaveExperimentAnnotation(experimentID, annotation);
            }
            else
            {
                throw new AccessDeniedException("Access denied saving experiment annotation.");
            }
        }
		/// <summary>
		/// superUser, owner and readExperiment privilege
		/// </summary>
		/// <param name="experimentID"></param>
		/// <returns></returns>
		public string SelectExperimentAnnotationWrapper(long experimentID)
		{
			int loginUserID = Convert.ToInt32(Session["UserID"]);
			int sessionGroupID = Convert.ToInt32(Session["GroupID"]);
            return SelectExperimentAnnotationWrapper(experimentID, loginUserID, sessionGroupID);
		}
        /// <summary>
        /// superUser, owner and readExperiment privilege
        /// </summary>
        /// <param name="experimentID"></param>
        /// <returns></returns>
        public string SelectExperimentAnnotationWrapper(long experimentID,int loginUserID, int  groupID)
        {
            
            int userID = InternalDataDB.SelectExperimentOwner(experimentID);

            bool haveAccess = false;

            //superUser or owner check
            if ((IsSuperuserGroup(groupID)) || (loginUserID == userID))
            {
                haveAccess = true;
            }
            else
            {
                //readExperiment check

                // get qualifier ID of experimentID
                int qualifierID = Authorization.AuthorizationAPI.GetQualifierID((int)experimentID, Qualifier.experimentQualifierTypeID);
                if (Authorization.AuthorizationAPI.CheckAuthorization(loginUserID, Function.readExperimentFunctionType, qualifierID))
                {
                    haveAccess = true;
                }
            }

            if (haveAccess)
            {
                return InternalDataDB.SelectExperimentAnnotation(experimentID);
            }
            else
            {
                throw new AccessDeniedException("Access denied retrieving experiment annotation.");
            }
        }

		/// <summary>
		/// superUser, owner and writeExperiment privilege
		/// </summary>
		/// <param name="experimentIDs"></param>
		/// <returns>An array of experimentIDs that were not removed</returns>
		public long[] RemoveExperimentsWrapper ( long[] experimentIDs )
		{
			ArrayList aList = new ArrayList ();

			int loginUserID = Convert.ToInt32(Session["UserID"]);
			int sessionGroupID = Convert.ToInt32(Session["GroupID"]);
            List<long> expIDs = FilterExperimentIDs(loginUserID, sessionGroupID, Function.writeExperimentFunctionType, experimentIDs);
            
            List<long> unremovedExpIDs = new List<long>();
            foreach (long exp in experimentIDs)
            {
                if(!expIDs.Contains(exp))
                    unremovedExpIDs.Add(exp);
            }
            foreach (long expID in expIDs)
            {
                if (!DataStorageAPI.DeleteExperiment(expID))
                {
                    unremovedExpIDs.Add(expID);
                }
            }	
			return unremovedExpIDs.ToArray();
		}
        /*
        public Coupon GetExperimentCouponWrapper(long experimentID)
        {
            //first check permisson
            int loginUserID = Convert.ToInt32(Session["UserID"]);
            int sessionGroupID = Convert.ToInt32(Session["GroupID"]);
            string sessionGroupName = Session["GroupName"].ToString();

            bool haveAccess = true;

            //superUser check
            if (sessionGroupName.CompareTo(Group.SUPERUSER) == 0)
            {
                haveAccess = true;
            }

            for (long i = 0; i < experimentIDs.Length; i++)
            {
                int userID = InternalDataDB.SelectExperimentOwner(experimentIDs[i]);

                //owner check
                if (loginUserID == userID)
                {
                    haveAccess = true;
                }
                else
                {
                    // get qualifier ID of experimentID
                    int qualifierID = Authorization.Authorization.GetQualifierID((int)experimentIDs[i], Qualifier.experimentQualifierTypeID);

                    //readExperiment check
                    if (Authorization.Authorization.CheckAuthorization(loginUserID, Function.readExperimentFunctionType, qualifierID))
                    {
                        haveAccess = true;
                    }
                }

                if (!haveAccess)
                {
                    throw new AccessDeniedException("Access denied retrieving experiment information. Insufficient permission to access experiment " + i + ".");
                }

                //reset haveAccess to false if not superUser
                if (sessionGroupName.CompareTo(Group.SUPERUSER) != 0)
                {
                    haveAccess = false;
                }
            }
        }
        */

		/// <summary>
		/// superUser, owner and readExperiment Privilege
		/// </summary>
		/// <param name="experimentIDs"></param>
		/// <returns></returns>
		public ExperimentSummary[] GetExperimentSummaryWrapper ( long[] experimentIDs )
		{
			//first check permisson
			int loginUserID = Convert.ToInt32(Session["UserID"]);
			int sessionGroupID = Convert.ToInt32(Session["GroupID"]);
            List<long> allowedExpIDs = FilterExperimentIDs(loginUserID, sessionGroupID, Function.readExperimentFunctionType, experimentIDs);
			//if successfully out of loop.
            return DataStorageAPI.RetrieveExperimentSummaries(allowedExpIDs.ToArray());
		}

        public List<long> FilterExperimentIDs(int loginUserID, int sessionGroupID, string function, long[] expIDs)
        {
            List<long> allowedExpIDs = new List<long>();
            //check permissons
			//superUser check
            if (IsSuperuserGroup(sessionGroupID))
            {
                List<long> ids = new List<long>();
                ids.AddRange(expIDs);
                return ids;
            }
            else
            {
                for (int i = 0; i < expIDs.Length; i++)
                {
                    //owner check
                    int userID = InternalDataDB.SelectExperimentOwner(expIDs[i]);
                    if (loginUserID == userID)
                    {
                        allowedExpIDs.Add(expIDs[i]);
                    }
                    else
                    {
                        // get qualifier ID of experimentID
                        int qualifierID = Authorization.AuthorizationAPI.GetQualifierID((int)expIDs[i], Qualifier.experimentQualifierTypeID);

                        //readExperiment check
                        //need to check for session Group instead of login user. Otherwise it'll show experiments from other groups too.
                        if (Authorization.AuthorizationAPI.CheckAuthorization(sessionGroupID, function, qualifierID))
                        {
                            allowedExpIDs.Add(expIDs[i]);
                        }
                    }
                }
            }
			return allowedExpIDs;
        }

		/// <summary>
		/// owner, readExperiment and superUser privilege
		/// </summary>
		/// <param name="criteria"></param>
		public long[] FindExperimentIDsWrapper(Criterion [] criteria)
		{
			// first, retrieve all experiment IDs which satisfy criteria
            //long[] expIDs = InternalDataDB.SelectExperimentIDs(criteria);

			//then check permissons
			int loginUserID = Convert.ToInt32(Session["UserID"]);
			int sessionGroupID = Convert.ToInt32(Session["GroupID"]);
            return FindExperimentIDsWrapper(loginUserID, sessionGroupID, criteria);
		}


        /// <summary>
        /// owner, readExperiment and superUser privilege
        /// </summary>
        /// <param name="criteria"></param>
        public long[] FindExperimentIDsWrapper(int loginUserID, int sessionGroupID, Criterion[] criteria)
        {
            // first, retrieve all experiment IDs which satisfy criteria
            long[] expIDs = InternalDataDB.SelectExperimentIDs(criteria);
            return FilterExperimentIDs(loginUserID, sessionGroupID,Function.readExperimentFunctionType, expIDs).ToArray();
        }


		/// <summary>
		/// superUser privilege
		/// </summary>
		/// <param name="experimentID"></param>
		/// <param name="newUserID"></param>
		public void ModifyExperimentOwnerWrapper (long experimentID, int newUserID)
		{
            //int sessionGroupID = Convert.ToInt32(Session["GroupID"]);
            //string sessionGroupName = Session["GroupName"].ToString();

			if(IsSuperuserGroup())
			{
                InternalDataDB.UpdateExperimentOwner(experimentID, newUserID);
			}
			else
			{
				throw new AccessDeniedException ("Access denied modifying experiment owner.");
			}
		}

		/// <summary>
		/// superUser and administerGroup privilege
		/// </summary>
		/// <param name="messageType"></param>
		/// <param name="toBeDisplayed"></param>
		/// <param name="groupID"></param>
		/// <param name="clientID"></param>
        /// <param name="agentID"></param>
		/// <param name="messageBody"></param>
		/// <param name="messageTitle"></param>
		/// <returns></returns>
        public int AddSystemMessageWrapper(string messageType, bool toBeDisplayed, int groupID, int clientID, int agentID, string messageTitle, string messageBody)
		{
            //int sessionGroupID = Convert.ToInt32(Session["GroupID"]);
            //string sessionGroupName = Session["GroupName"].ToString();
			int loginUserID = Convert.ToInt32(Session["UserID"]);

			int qID = Authorization.AuthorizationAPI.GetQualifierID(groupID, Qualifier.groupQualifierTypeID);
			if((IsSuperuserGroup())||(Authorization.AuthorizationAPI.CheckAuthorization(loginUserID, Function.administerGroupFunctionType, qID)))			
			{
                return Administration.AdministrativeAPI.AddSystemMessage(messageType, toBeDisplayed, groupID, clientID, agentID, messageTitle, messageBody);
			}
			else
				throw new AccessDeniedException("Access denied adding system message.");
		}

		/// <summary>
		/// anyone privilege
		/// </summary>
		/// <param name="messageIDs"></param>
		/// <returns></returns>
		public int[] RemoveSystemMessagesWrapper (int[] messageIDs)
		{
			return Administration.AdministrativeAPI.RemoveSystemMessages(messageIDs);
		}

		/// <summary>
		/// superUser and administerGroup privilege
		/// </summary>
		/// <param name="messageID"></param>
		/// <param name="messageType"></param>
		/// <param name="toBeDisplayed"></param>
		/// <param name="groupID"></param>
		/// <param name="labServerID"></param>
		/// <param name="messageBody"></param>
		/// <param name="messageTitle"></param>
		public void ModifySystemMessageWrapper (int messageID, string messageType, bool toBeDisplayed, int groupID, int clientID, int agentID, string messageBody, string messageTitle)
		{
            //int sessionGroupID = Convert.ToInt32(Session["GroupID"]);
            //string sessionGroupName = Session["GroupName"].ToString();
			int loginUserID = Convert.ToInt32(Session["UserID"]);

			int qID = Authorization.AuthorizationAPI.GetQualifierID(groupID, Qualifier.groupQualifierTypeID);
			if((IsSuperuserGroup())||(Authorization.AuthorizationAPI.CheckAuthorization(loginUserID, Function.administerGroupFunctionType, qID)))
			{
				Administration.AdministrativeAPI.ModifySystemMessage(messageID, messageType, toBeDisplayed, groupID, clientID, agentID, messageBody, messageTitle);
			}
			else
				throw new AccessDeniedException("Access denied modifying system message.");
		}

		/// <summary>
		/// anyone privilege
		/// </summary>
		/// <param name="messageType"></param>
		/// <param name="groupID"></param>
		/// <param name="labServerID"></param>
		/// <returns></returns>
		public SystemMessage[] GetSystemMessagesWrapper (string messageType, int groupID, int clientID, int agentID)
		{
			return Administration.AdministrativeAPI.GetSystemMessages(messageType, groupID, clientID, agentID);
		}

        /// <summary>
        /// Retrieves session information via the sessionID and resets session variables.
        /// </summary>
        /// <param name="coupon"></param>
        public void SetServiceSession(long sessionID)
        {
            SessionInfo sessionInfo = AdministrativeAPI.GetSessionInfo(sessionID);
            SessionInfoToSession(sessionInfo);
        }
 
        /// <summary>
        /// Retrieves session information via the coupon's RedeemSesssion ticket and resets session variables.
        /// </summary>
        /// <param name="coupon"></param>
        public void SetServiceSession(Coupon coupon)
        {
            SessionInfo sessionInfo = AdministrativeAPI.GetSessionInfo(coupon);
            SessionInfoToSession(sessionInfo);

        }

        private void SessionInfoToSession(SessionInfo sessionInfo)
        {

            if (sessionInfo != null)
            {
                //Establish session credentials with the new information.
                Session["UserID"] = sessionInfo.userID;
                if (sessionInfo.groupID > 0)
                {
                    Session["GroupID"] = sessionInfo.groupID;
                    Session["GroupName"] = sessionInfo.groupName;
                }
                else
                {
                    Session.Remove("GroupID");
                    Session.Remove("GroupName");
                }

                if (sessionInfo.clientID > 0)
                {
                    Session["ClientID"] = sessionInfo.clientID;
                }
                else
                    Session.Remove("ClientID");


            }
            else
            {
                Session.Remove("UserID");
                Session.Remove("GroupID");
                Session.Remove("ClientID");
                Session.Remove("UserName");
                Session.Remove("GroupName");
            }
        }
 


		/// <summary>
		/// superUser and administerGroup Access
		/// </summary>
		/// <returns></returns>
		public UserSession[] GetUserSessionsWrapper(int userID, int groupID, DateTime timeAfter, DateTime timeBefore)
		{
            int sessionGroupID = Convert.ToInt32(Session["GroupID"]);
            //string sessionGroupName=Session["GroupName"].ToString();
			int loginUserID = Convert.ToInt32(Session["UserID"].ToString());

			try
			{
				if(IsSuperuserGroup())
				{
					return Administration.AdministrativeAPI.GetUserSessions(userID, groupID, timeAfter, timeBefore);
				}
				else
				{
					if (groupID >=0)
					{
						int qID = Authorization.AuthorizationAPI .GetQualifierID (groupID, Qualifier .groupQualifierTypeID );
						if(Authorization.AuthorizationAPI .CheckAuthorization (sessionGroupID, Function .administerGroupFunctionType , qID))
						{
							return Administration.AdministrativeAPI.GetUserSessions(userID, groupID, timeAfter, timeBefore);
						}
						else
							throw new AccessDeniedException("Cannot list user sessions. Insufficient permission.");
					}
					else
					{
						ArrayList allowedGroups = new ArrayList();
						ArrayList userSessions = new ArrayList();
						if (userID>=0)
						{
							int[] userParents = Administration.AdministrativeAPI.ListGroupIDsForUser(userID);
							foreach (int parent in userParents)
							{
								int qID = Authorization.AuthorizationAPI .GetQualifierID (parent, Qualifier .groupQualifierTypeID );
								if(Authorization.AuthorizationAPI .CheckAuthorization (sessionGroupID, Function .administerGroupFunctionType , qID))
								{
									allowedGroups.Add(parent);
								}
							}
						}
						else
						{
							int[] gIDs = Administration.AdministrativeAPI.ListGroupIDs();	
							foreach (int gID in gIDs)
							{
								if (gID>0)
								{
									int qID = Authorization.AuthorizationAPI .GetQualifierID (gID, Qualifier .groupQualifierTypeID );
									if(Authorization.AuthorizationAPI .CheckAuthorization (sessionGroupID, Function .administerGroupFunctionType , qID))
									{
										allowedGroups.Add(gID);
									}
								}
							}
						}

						if (allowedGroups.Count>0)
						{
							foreach (int allowedParent in allowedGroups)
							{
								UserSession[] temp = Administration.AdministrativeAPI.GetUserSessions(userID, allowedParent, timeAfter, timeBefore);
								foreach (UserSession us in temp)
								{
									userSessions.Add(us);
								}
							}

							UserSession[] sessionArray = new UserSession[userSessions.Count];
							for (int i = 0;i <userSessions.Count; i++)
							{
								sessionArray[i]= (UserSession)userSessions[i];
							}
							return sessionArray;
						}
						else 
							throw new AccessDeniedException("Cannot list user sessions. Insufficient Permission.");
					}
				}
			}
			catch (Exception ex)
			{
				throw;
			}
		}	

	}

}
