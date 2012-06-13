using System;
using System.Collections.Generic;
using System.Text;
using iLabs.DataTypes.TicketingDataTypes;
using iLabs.UtilLib;

namespace iLabs.TicketingAPI
{
    public class ResourceDescriptorFactory : BasicXmlFactory
    {
        /// <summary>
        /// protected constructor
        /// </summary>
        protected ResourceDescriptorFactory()
        {
        }

        /// <summary>
        /// singleton instance
        /// </summary>
        protected new static ResourceDescriptorFactory instance;

        public new static ResourceDescriptorFactory Instance()
        {
            if (instance == null)
                instance = new ResourceDescriptorFactory();

            return instance;
        }
        string nameSpace = "http://ilab.mit.edu/iLabs/resources";


        public string createProcessAgentDescriptor(ProcessAgent agent, ArrayList resources)
        {
            string rootElemt = "processAgentDescriptor";
            Dictionary<string, object> keyValueDictionary = new Dictionary<string, object>();
            keyValueDictionary.Add("agentGuid",agent.agentGuid);
            keyValueDictionary.Add("type", agent.type);
            keyValueDictionary.Add("agentName", agent.agentName);
            keyValueDictionary.Add("issuerGuid", agent.issuerGuid);
            keyValueDictionary.Add("codeBaseUrl", agent.codeBaseUrl);
            keyValueDictionary.Add("webServiceUrl", agent.webServiceUrl);
        }

        public string createExecuteExperimentPayload(string essWebAddress, DateTime startExecution, long duration, 
            int userTZ, string groupName, string sbGuid, long experimentID)
        {
            string rootElemt = "ExecuteExperimentPayload";
            Dictionary<string, object> keyValueDictionary = new Dictionary<string, object>();
            if(essWebAddress != null)
                keyValueDictionary.Add("essWebAddress", essWebAddress);
            keyValueDictionary.Add("startExecution", DateUtil.ToUtcString(startExecution));
            keyValueDictionary.Add("duration", duration);
            
            // remove
            //keyValueDictionary.Add("userName", groupName);
            keyValueDictionary.Add("groupName", groupName);
            // changed to "sbGuid"
            keyValueDictionary.Add("sbGuid", sbGuid);
            keyValueDictionary.Add("experimentID", experimentID);
            keyValueDictionary.Add("userTZ", userTZ);
            return writeTicketLoad(rootElemt, TicketTypes.EXECUTE_EXPERIMENT, keyValueDictionary);
        }
        
    }
}
