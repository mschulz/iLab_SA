using System;
using System.Collections.Generic;
using System.Text;
using iLabs.DataTypes.TicketingTypes;
using iLabs.UtilLib;

namespace iLabs.Ticketing
{
    public class TicketLoadFactory : BasicTicketLoadFactory
    {
        /// <summary>
        /// protected constructor
        /// </summary>
        protected TicketLoadFactory()
        {
        }

        /// <summary>
        /// singleton instance
        /// </summary>
        protected new static TicketLoadFactory instance;

        public new static TicketLoadFactory Instance()
        {
            if (instance == null)
                instance = new TicketLoadFactory();

            return instance;
        }

        /**
         * Authetication tickets
         * */

        public string createAuthenticateSBPayload()
        {
            string rootElement = "AuthenticateSBPayload";
            Dictionary<string, object> keyValueDictionary = new Dictionary<string, object>();
            return writeTicketLoad(rootElement, TicketTypes.AUTHENTICATE_SERVICE_BROKER, keyValueDictionary);
        }

        public string createAuthenticateAgentPayload()
        {
            string rootElemt = "AuthenticateAgentPayload";
            Dictionary<string, object> keyValueDictionary = new Dictionary<string, object>();
            return writeTicketLoad(rootElemt, TicketTypes.AUTHENTICATE_AGENT, keyValueDictionary);
        }


        public string createAuthenticateAgentPayload(string authGuid, string clientGuid, string userName, string groupName)
        {
            string rootElemt = "AuthenticateAgentPayload";
            Dictionary<string, object> keyValueDictionary = new Dictionary<string, object>();
            if (authGuid != null && authGuid.Length > 0)
            {
                keyValueDictionary.Add("authGuid", authGuid);
            }
            if (clientGuid != null && clientGuid.Length > 0)
            {
                keyValueDictionary.Add("clientGuid", clientGuid);
            }
            if (userName != null && userName.Length > 0)
            {
                keyValueDictionary.Add("userName", userName);
            }
            if (groupName != null && groupName.Length > 0)
            {
                keyValueDictionary.Add("groupName", groupName);
            }

            return writeTicketLoad(rootElemt, TicketTypes.AUTHENTICATE_AGENT, keyValueDictionary);
        }

        /**
         * service broker tickets
         * */

        public string createAuthorizeClientPayload(string clientGuid, string groupName)
        {
            string rootElemt = "AuthorizeClientPayload";
            Dictionary<string, object> keyValueDictionary = new Dictionary<string, object>();
            if (clientGuid != null && clientGuid.Length > 0)
            {
                keyValueDictionary.Add("clientGuid", clientGuid);
            }
            if (groupName != null && groupName.Length > 0)
            {
                keyValueDictionary.Add("groupName", groupName);
            }
            return writeTicketLoad(rootElemt, TicketTypes.AUTHORIZE_CLIENT, keyValueDictionary);
        }

        public string createRedeemSessionPayload(int userID, int groupID, int clientID)
        {
            return createRedeemSessionPayload(userID, groupID, clientID, null, null);
        }

        public string createRedeemSessionPayload(int userID,int groupID,int clientID,string userName,string groupName)
        {
            string rootElemt = "RedeemSessionPayload";
            Dictionary<string, object> keyValueDictionary = new Dictionary<string, object>();
            keyValueDictionary.Add("userID", userID.ToString());
            if (groupID > 0)
            keyValueDictionary.Add("groupID", groupID.ToString());
            if(clientID > 0)
                keyValueDictionary.Add("clientID", clientID.ToString());
            if (userName != null && userName.Length > 0)
            {
                keyValueDictionary.Add("userName", userName);
            }
            if (groupName != null && groupName.Length > 0)
            {
                keyValueDictionary.Add("groupName", groupName);
            }
            return writeTicketLoad(rootElemt, TicketTypes.REDEEM_SESSION, keyValueDictionary);
        }

        /**
         * ESS tickets
         */
        public string createAdministerESSPayload()
        {
            string rootElemt = "AdministerESSPayload";
            Dictionary<string, object> keyValueDictionary = new Dictionary<string, object>();
            return writeTicketLoad(rootElemt, TicketTypes.ADMINISTER_ESS, keyValueDictionary);
        }

        public string createAdministerExperimentPayload(long experimentID, string essURL)
        {
            string rootElemt = "AdministerExperimentPayload";
            Dictionary<string, object> keyValueDictionary = new Dictionary<string, object>();
            keyValueDictionary.Add("experimentID",experimentID);
            keyValueDictionary.Add("essURL", essURL);
            return writeTicketLoad(rootElemt, TicketTypes.ADMINISTER_EXPERIMENT, keyValueDictionary);
        }

        public string StoreRecordsPayload(bool blob, long experimentID, string essURL)
        {
            string rootElemt = "StoreRecordsPayload";
            Dictionary<string, object> keyValueDictionary = new Dictionary<string, object>();
            keyValueDictionary.Add("blob", blob);
            keyValueDictionary.Add("experimentID", experimentID);
            keyValueDictionary.Add("essURL",essURL);
            return writeTicketLoad(rootElemt, TicketTypes.STORE_RECORDS, keyValueDictionary);
        }

        public string RetrieveRecordsPayload(long experimentID, string essURL)
        {
            string rootElemt = "RetrieveRecordsPayload";
            Dictionary<string, object> keyValueDictionary = new Dictionary<string, object>();
            keyValueDictionary.Add("experimentID", experimentID);
            keyValueDictionary.Add("essURL", essURL);
            return writeTicketLoad(rootElemt, TicketTypes.RETRIEVE_RECORDS, keyValueDictionary);
        }

        /**
         * USS tickets
         */
        public string createAdministerUSSPayload(int userTZ)
        {
            string rootElemt = "AdministerUSSPayload";
            Dictionary<string, object> keyValueDictionary = new Dictionary<string, object>();
            keyValueDictionary.Add("userTZ", userTZ);
            return writeTicketLoad(rootElemt, TicketTypes.ADMINISTER_USS, keyValueDictionary);
        }

        public string createManageUSSGroupPayload(string groupName, string sbGuid, string clientGuid, int userTZ)
        {
            string rootElemt = "ManageUSSGroupPayload";
            Dictionary<string, object> keyValueDictionary = new Dictionary<string, object>();        
            keyValueDictionary.Add("groupName", groupName);
            keyValueDictionary.Add("sbGuid", sbGuid);
	        if(clientGuid != null)
                keyValueDictionary.Add("clientGuid", clientGuid);
            keyValueDictionary.Add("userTZ", userTZ);
            return writeTicketLoad(rootElemt, TicketTypes.MANAGE_USS_GROUP, keyValueDictionary);
        }

        public string createScheduleSessionPayload(string userName, int userID, string groupName, string sbGuid,
            string labServerGUID, string clientGuid, string labClientName, string labClientVersion, string ussURL, int userTZ)
        {
            string rootElemt = "ScheduleSessionPayload";
            Dictionary<string, object> keyValueDictionary = new Dictionary<string, object>();
            if (userName != null && userName.Length > 0)
                keyValueDictionary.Add("userName", userName);
            keyValueDictionary.Add("userID", userID.ToString());
            if (groupName != null && groupName.Length > 0)
                keyValueDictionary.Add("groupName", groupName);
            if (sbGuid != null && sbGuid.Length > 0)
                keyValueDictionary.Add("sbGuid", sbGuid);
            if (labServerGUID != null && labServerGUID.Length > 0)
                keyValueDictionary.Add("labServerGuid", labServerGUID);
            if (clientGuid != null && clientGuid.Length > 0)
                keyValueDictionary.Add("clientGuid", clientGuid);
            if (labClientName != null && labClientName.Length > 0)
                keyValueDictionary.Add("labClientName", labClientName);
            if (labClientVersion != null && labClientVersion.Length > 0)
                keyValueDictionary.Add("labClientVersion", labClientVersion);
            if (ussURL != null && ussURL.Length > 0)
                keyValueDictionary.Add("ussURL", ussURL);
            keyValueDictionary.Add("userTZ", userTZ);
            return writeTicketLoad(rootElemt, TicketTypes.SCHEDULE_SESSION, keyValueDictionary);
        }

       


        public string createRedeemReservationPayload(DateTime startTime,DateTime endTime, string userName, int userID, string groupName, string clientGuid)
        {
            string rootElemt = "RedeemReservationPayload";
            Dictionary<string, object> keyValueDictionary = new Dictionary<string, object>();
            keyValueDictionary.Add("start", DateUtil.ToUtcString(startTime));
            keyValueDictionary.Add("end", DateUtil.ToUtcString(endTime));
            keyValueDictionary.Add("userName", userName);
            keyValueDictionary.Add("groupName", groupName);
            keyValueDictionary.Add("userID", userID.ToString());
            keyValueDictionary.Add("clientGuid", clientGuid);
            return writeTicketLoad(rootElemt, TicketTypes.REDEEM_RESERVATION, keyValueDictionary);
        }
        public string createRevokeReservationPayload(string source)
        {
             return createRevokeReservationPayload(source,null, 0, null,null,null,null);
        }

        public string createRevokeReservationPayload(string source, string userName, int userID, string groupName, 
            string authorityGuid, string clientGuid, string ussURL)
        {
            string rootElemt = "RevokeReservationPayload";
            Dictionary<string, object> keyValueDictionary = new Dictionary<string, object>();
            if (source != null && source.Length > 0)
                keyValueDictionary.Add("source", source);
            if (userName != null && userName.Length > 0)
                keyValueDictionary.Add("userName", userName);
            keyValueDictionary.Add("userID", userID.ToString());
            if (groupName != null && groupName.Length > 0)
                keyValueDictionary.Add("groupName", groupName);
            if (authorityGuid != null && authorityGuid.Length > 0)
                keyValueDictionary.Add("authorityGuid", authorityGuid);
            if (clientGuid != null && clientGuid.Length > 0)
                keyValueDictionary.Add("clientGuid", clientGuid);
            if (ussURL != null && ussURL.Length > 0)
                keyValueDictionary.Add("ussURL", ussURL);
            return writeTicketLoad(rootElemt, TicketTypes.REVOKE_RESERVATION, keyValueDictionary);
        }

        //
        //Do we need the ServiceBroker ID??
        //
        public string createAllowExperimentExecutionPayload(DateTime startExecution, long duration,
            string groupName, string clientGuid)//, string sbGuid)
        {
            string rootElemt = "AllowExperimentExecutionPayload";
            Dictionary<string, object> keyValueDictionary = new Dictionary<string, object>();
            keyValueDictionary.Add("startExecution", DateUtil.ToUtcString(startExecution));
            keyValueDictionary.Add("duration", duration);
            keyValueDictionary.Add("groupName", groupName);
            if(clientGuid != null && clientGuid.Length > 0)
                keyValueDictionary.Add("clientGuid", clientGuid);
            return writeTicketLoad(rootElemt, TicketTypes.ALLOW_EXPERIMENT_EXECUTION, keyValueDictionary);
        }

        /**
         * LSS tickets
         */
        public string createAdministerLSSPayload(int userTZ)
        {
            string rootElemt = "AdministerLSSPayload";
            Dictionary<string, object> keyValueDictionary = new Dictionary<string, object>();
            keyValueDictionary.Add("userTZ", userTZ);
            return writeTicketLoad(rootElemt, TicketTypes.ADMINISTER_LSS, keyValueDictionary);
        }

        public string createManageLabPayload(string labServerGuid, string labServerName, string sbGuid, string adminGroupName,int userTZ)
        {
            string rootElemt = "ManageLabPayload";
            Dictionary<string, object> keyValueDictionary = new Dictionary<string, object>();
            keyValueDictionary.Add("labServerGuid", labServerGuid);
            keyValueDictionary.Add("labServerName", labServerName);
            keyValueDictionary.Add("sbGuid", sbGuid);
            keyValueDictionary.Add("adminGroup", adminGroupName);
            keyValueDictionary.Add("userTZ", userTZ);
            return writeTicketLoad(rootElemt, TicketTypes.MANAGE_LAB, keyValueDictionary);
        }

        public string createRequestReservationPayload()
        {
            string rootElemt = "RequestReservationPayload";
            Dictionary<string, object> keyValueDictionary = new Dictionary<string, object>();
            return writeTicketLoad(rootElemt, TicketTypes.REQUEST_RESERVATION, keyValueDictionary);
        }

        public string createRegisterLSPayload()
        {
            string rootElemt = "RegisterLSPayload";
            Dictionary<string, object> keyValueDictionary = new Dictionary<string, object>();
            return writeTicketLoad(rootElemt, TicketTypes.REGISTER_LS, keyValueDictionary);
        }

        ///*
        // * LS tickets
        // */
        public string createAdministerLSPayload(int userTZ)
        {
            string rootElemt = "AdministerLSPayload";
            Dictionary<string, object> keyValueDictionary = new Dictionary<string, object>();
            keyValueDictionary.Add("userTZ", userTZ);
            return writeTicketLoad(rootElemt, TicketTypes.ADMINISTER_LS, keyValueDictionary);
        }

        public string createCreateExperimentPayload(DateTime startExecution, long duration,
            string userName, int userID, string groupName, string lsGuid, string clientGuid)
        {
            string rootElemt = "CreateExperimentPayload";
            Dictionary<string, object> keyValueDictionary = new Dictionary<string, object>();
            keyValueDictionary.Add("startExecution", DateUtil.ToUtcString(startExecution));
            keyValueDictionary.Add("duration", duration);
            keyValueDictionary.Add("userName", userName);
            keyValueDictionary.Add("userID", userID.ToString());
            keyValueDictionary.Add("groupName", groupName);
            keyValueDictionary.Add("labGuid", lsGuid);
            keyValueDictionary.Add("clientGuid", clientGuid);
            return writeTicketLoad(rootElemt, TicketTypes.CREATE_EXPERIMENT, keyValueDictionary);
        }

        public string createExecuteExperimentPayload(string essWebAddress, DateTime startExecution, long duration, 
            int userTZ, int userID, string groupName, string sbGuid, long experimentID)
        {
            string rootElemt = "ExecuteExperimentPayload";
            Dictionary<string, object> keyValueDictionary = new Dictionary<string, object>();
            if(essWebAddress != null)
                keyValueDictionary.Add("essWebAddress", essWebAddress);
            keyValueDictionary.Add("startExecution", DateUtil.ToUtcString(startExecution));
            keyValueDictionary.Add("duration", duration);
            
            // Use ID to hide username and deal with authority
           
            keyValueDictionary.Add("userID", userID);
            keyValueDictionary.Add("groupName", groupName);
            // changed to "sbGuid"
            keyValueDictionary.Add("sbGuid", sbGuid);
            keyValueDictionary.Add("experimentID", experimentID);
            keyValueDictionary.Add("userTZ", userTZ);
            return writeTicketLoad(rootElemt, TicketTypes.EXECUTE_EXPERIMENT, keyValueDictionary);
        }
        
    }
}
