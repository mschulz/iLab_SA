using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.XPath;

using iLabs.Core;
using iLabs.DataTypes;
using iLabs.DataTypes.TicketingTypes;
using iLabs.UtilLib;
using iLabs.ServiceBroker.Administration;
using iLabs.ServiceBroker.Authorization;
using iLabs.ServiceBroker.Mapping;
using iLabs.DataTypes.ProcessAgentTypes;
using iLabs.DataTypes.TicketingTypes;
using iLabs.Ticketing;

namespace iLabs.ServiceBroker
{
    public class GroupCredential
    {
        public string domainGuid;
        public string domainServerName;
        public string groupName;
        public string ussGuid;
        public string lssGuid;
    }

    public class ResourceDescriptorFactory
    {
        /// <summary>
        /// protected constructor
        /// </summary>
        protected ResourceDescriptorFactory()
        {
            brokerDb = new BrokerDB();
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
        protected BrokerDB brokerDb;
        protected string ns = "";

        public string CreateClientDescriptor(int clientId)
        {
            string descriptor = null;
            LabClient[] clients = AdministrativeAPI.GetLabClients(new int[] { clientId });

            if (clients != null && clients.Length > 0)
            {
                descriptor = CreateClientDescriptor(clients[0]);
            }
            return descriptor;
        }

        public string CreateClientDescriptor(LabClient client)
        {
            StringBuilder stringBuilder = new StringBuilder();
            try
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.OmitXmlDeclaration = true;
                settings.NewLineOnAttributes = true;
                settings.CheckCharacters = true;

                XmlWriter xmlWriter = XmlWriter.Create(stringBuilder, settings);

                // write root element
                xmlWriter.WriteStartElement("clientDescriptor");
                //xmlWriter.WriteAttributeString("xmlns", "ns", null, nameSpace);

                if (client.clientGuid != null && client.clientGuid.Length > 0)
                    xmlWriter.WriteElementString("clientGuid", client.clientGuid);
                if (client.clientName != null && client.clientName.Length > 0)

                    xmlWriter.WriteElementString("clientName", client.clientName);
                if (client.version != null && client.version.Length > 0)
                    xmlWriter.WriteElementString("version", client.version);
                if (client.clientShortDescription != null && client.clientShortDescription.Length > 0)

                    xmlWriter.WriteElementString("clientShortDescription", client.clientShortDescription);
                if (client.clientLongDescription != null && client.clientLongDescription.Length > 0)
                    xmlWriter.WriteElementString("clientLongDescription", client.clientLongDescription);
                xmlWriter.WriteElementString("clientType", client.clientType);
                if (client.contactEmail != null && client.contactEmail.Length > 0)
                    xmlWriter.WriteElementString("contactEmail", HttpUtility.HtmlEncode(client.contactEmail));
                if (client.contactFirstName != null && client.contactFirstName.Length > 0)
                    xmlWriter.WriteElementString("contactFirstName", client.contactFirstName);
                if (client.contactLastName != null && client.contactLastName.Length > 0)
                    xmlWriter.WriteElementString("contactLastName", client.contactLastName);
                if (client.loaderScript != null && client.loaderScript.Length > 0)
                    xmlWriter.WriteElementString("loaderScript", client.loaderScript);
                if (client.documentationURL != null && client.documentationURL.Length > 0)
                    xmlWriter.WriteElementString("documentationUrl", client.documentationURL);
                xmlWriter.WriteElementString("needsScheduling", client.needsScheduling.ToString());
                xmlWriter.WriteElementString("needsESS", client.needsESS.ToString());
                xmlWriter.WriteElementString("isReentrant", client.IsReentrant.ToString());
                if (client.notes != null && client.notes.Length > 0)
                    xmlWriter.WriteElementString("notes", client.notes);
                ProcessAgentInfo[] labServers = AdministrativeAPI.GetLabServersForClient(client.clientID);
                if (labServers != null && labServers.Length > 0)
                {
                    xmlWriter.WriteStartElement("labServers");
                    for (int i = 0; i < labServers.Length; i++)
                    {
                        if (labServers[i] != null)
                        {
                            xmlWriter.WriteStartElement("labServer");
                            xmlWriter.WriteAttributeString("guid", labServers[i].agentGuid);
                            xmlWriter.WriteEndElement();
                        }
                    }
                    xmlWriter.WriteEndElement();

                }
                ClientInfo[] clientInfos = AdministrativeAPI.ListClientInfos(client.clientID);
                if (clientInfos != null && clientInfos.Length > 0)
                {
                    xmlWriter.WriteStartElement("clientInfos");
                    foreach (ClientInfo i in clientInfos)
                    {
                        WriteClientInfo(xmlWriter, i);
                    }
                    xmlWriter.WriteEndElement();

                }
                xmlWriter.WriteEndElement();
                xmlWriter.Flush();
                return stringBuilder.ToString();
            }
            catch (Exception e)
            {
               Logger.WriteLine("Create clientDescriptor: " + e.Message);
            }
            return null;
        }
             
       
        public string CreateProcessAgentDescriptor(int agentId)
        {
            string descriptor = null;
            ProcessAgent agent = brokerDb.GetProcessAgent(agentId);
            SystemSupport ss = brokerDb.RetrieveSystemSupport(agentId);
            descriptor = CreateProcessAgentDescriptor(agentId, agent,ss);
            return descriptor;
        }

        public string CreateProcessAgentDescriptor(int agentId, ProcessAgent agent, SystemSupport ss)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.OmitXmlDeclaration = true;
            settings.NewLineOnAttributes = true;
            settings.CheckCharacters = true;

            StringBuilder stringBuilder = new StringBuilder();
            XmlWriter xmlWriter = XmlWriter.Create(stringBuilder, settings);

            if (agent != null)
            {
                // write root element
                xmlWriter.WriteStartElement("processAgentDescriptor");
                //xmlWriter.WriteAttributeString("xmlns", "ns", null, nameSpace);
             
                xmlWriter.WriteElementString("agentGuid", agent.agentGuid);
                xmlWriter.WriteElementString("type", agent.type);
                xmlWriter.WriteElementString("agentName", agent.agentName);
                xmlWriter.WriteElementString("domainGuid", agent.domainGuid);
                xmlWriter.WriteElementString("codeBaseUrl", agent.codeBaseUrl);
                xmlWriter.WriteElementString("webServiceUrl", agent.webServiceUrl);
                if (ss != null)
                    xmlWriter.WriteRaw(ss.ToXML());
                if(agent.type.Equals(ProcessAgentType.LAB_SERVER)){
                    int lssId = brokerDb.FindProcessAgentIdForAgent(agentId, ProcessAgentType.LAB_SCHEDULING_SERVER);
                    if(lssId > 0){
                        ProcessAgent lss = brokerDb.GetProcessAgent(lssId);
                        if(lss != null){

                            xmlWriter.WriteElementString("lssGuid",lss.agentGuid);
                        }
                    }
                }
                
                Hashtable resourceTags = brokerDb.GetResourceStringTags(agentId, 1);
                if (resourceTags != null && resourceTags.Count > 0)
                {
                    xmlWriter.WriteStartElement("resources");
                    foreach (string s in resourceTags.Keys)
                    {
                        xmlWriter.WriteStartElement("resource");
                        xmlWriter.WriteAttributeString("key", s);
                        xmlWriter.WriteAttributeString("value",((IntTag)resourceTags[s]).tag);
                        xmlWriter.WriteEndElement();
                    }
                    xmlWriter.WriteEndElement();

                }
                xmlWriter.WriteEndElement();
                xmlWriter.Flush();
            }
            return stringBuilder.ToString(); ;
        }
        /// <summary>
        /// Loads a new ProcessAgent into the database, without any Ident coupons, creates Qualifier.
        /// </summary>
        /// <param name="xdoc"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public ProcessAgentInfo LoadProcessAgent(XmlQueryDoc xdoc, ref StringBuilder message)
        {
            ProcessAgent pa = new ProcessAgent();
            pa.agentGuid = xdoc.Query("/processAgentDescriptor/agentGuid");
            pa.type = xdoc.Query("/processAgentDescriptor/type");
            pa.agentName = xdoc.Query("/processAgentDescriptor/agentName");
            pa.domainGuid = xdoc.Query("/processAgentDescriptor/domainGuid");
            pa.codeBaseUrl = xdoc.Query("/processAgentDescriptor/codeBaseUrl");
            pa.webServiceUrl = xdoc.Query("/processAgentDescriptor/webServiceUrl");
            int newID = brokerDb.InsertProcessAgent(pa, null, null);
           
            SystemSupport systemSupport = new SystemSupport();
            systemSupport.agentGuid = xdoc.Query("/processAgentDescriptor/systemSupport/agentGuid");
            systemSupport.bugEmail = xdoc.Query("/processAgentDescriptor/systemSupport/bugEmail");
            systemSupport.contactEmail = xdoc.Query("/processAgentDescriptor/systemSupport/contactEmail");
            systemSupport.infoUrl = xdoc.Query("/processAgentDescriptor/systemSupport/infoUrl");
            systemSupport.description = xdoc.Query("/processAgentDescriptor/systemSupport/desciption");
            systemSupport.location = xdoc.Query("/processAgentDescriptor/systemSupport/loction");
            if (systemSupport != null && systemSupport.agentGuid.CompareTo(pa.agentGuid) == 0)
            {
                brokerDb.SaveSystemSupport(systemSupport.agentGuid, systemSupport.contactEmail, systemSupport.bugEmail,
                    systemSupport.infoUrl, systemSupport.description, systemSupport.location);
            }
          
            // deal with resources later, need to decode resource Names
            XPathNodeIterator pathIter = xdoc.Select("/processAgentDescriptor/resources/*");
            if (pathIter != null && pathIter.Count > 0)
            {
                while (pathIter.MoveNext())
                {
                    string key = pathIter.Current.GetAttribute("key", ns);
                    string value = pathIter.Current.GetAttribute("value", ns);
                    // Create ResourceMapping
                    ResourceMappingKey mapKey = new ResourceMappingKey(ResourceMappingTypes.PROCESS_AGENT, newID);

                    // create values
                    ResourceMappingValue[] values = new ResourceMappingValue[2];
                    values[0] = new ResourceMappingValue(ResourceMappingTypes.RESOURCE_TYPE, key);
                    values[1] = new ResourceMappingValue(ResourceMappingTypes.STRING, value);
                    ResourceMapping newMapping = brokerDb.AddResourceMapping(mapKey, values);
                }
            }
            message.AppendLine("Registered a new " + pa.type + ": " + newID + ": "
                    + pa.agentName + " -> " + pa.codeBaseUrl);
            //Add Qualifiers
            if (pa.type.Equals(ProcessAgentType.LAB_SCHEDULING_SERVER))
            {
                int lssQualID = AuthorizationAPI.AddQualifier(newID, Qualifier.labSchedulingQualifierTypeID,
                    pa.agentName, Qualifier.ROOT);
                
            }
            else if (pa.type.Equals(ProcessAgentType.LAB_SERVER))
            {
                int lsQualID = AuthorizationAPI.AddQualifier(newID, Qualifier.labServerQualifierTypeID, pa.agentName, Qualifier.ROOT);
                string lssGuid = xdoc.Query("/processAgentDescriptor/lssGuid");
                if (lssGuid != null && lssGuid.Length > 0)
                {
                    int lssForLsId = brokerDb.GetProcessAgentID(lssGuid);
                    if (lssForLsId > 0)
                    {
                        brokerDb.AssociateLSS(newID, lssForLsId);
                    }
                }
            }
            return brokerDb.GetProcessAgentInfo(pa.agentGuid);
        }

        public int LoadLabClient(XmlQueryDoc xdoc, ref StringBuilder message)
        {
           
            string guid = xdoc.Query("/clientDescriptor/clientGuid");
            string type = xdoc.Query("/clientDescriptor/clientType");
            string name = xdoc.Query("/clientDescriptor/clientName");
            string version = xdoc.Query("/clientDescriptor/version");
            string shortDescription = xdoc.Query("/clientDescriptor/clientShortDescription");
            string longDescription = xdoc.Query("/clientDescriptor/clientLongDescription");
            string contactEmail = HttpUtility.HtmlDecode(xdoc.Query("/clientDescriptor/contactEmail"));
            string contactFirstName = xdoc.Query("/clientDescriptor/contactFirstName");
            string contactLastName = xdoc.Query("/clientDescriptor/contactLastName");
            string loaderScript = xdoc.Query("/clientDescriptor/loaderScript");
            string documentationURL = xdoc.Query("/clientDescriptor/documentationUrl");
            string tmp = xdoc.Query("/clientDescriptor/needsScheduling");
            bool needsScheduling = Convert.ToBoolean(tmp);
            tmp = xdoc.Query("/clientDescriptor/needsESS");
            bool needsESS = Convert.ToBoolean(tmp);
            tmp = xdoc.Query("/clientDescriptor/isReentrant");
            bool isReentrant = Convert.ToBoolean(tmp);
            string notes = xdoc.Query("/clientDescriptor/notes");

            // Insert the Client, Qualifier is created internally
            int newClientId = AdministrativeAPI.AddLabClient(guid, name, version, shortDescription,
                longDescription, type, loaderScript, documentationURL,
                contactEmail, contactFirstName, contactLastName,notes, needsESS, needsScheduling, isReentrant);
            
            // parse the LabServer list
            XPathNodeIterator iter = xdoc.Select("/clientDescriptor/labServers/*");
            if (iter != null && iter.Count > 0)
            {
                int order = 0;
                while (iter.MoveNext())
                {
                    string lsGuid = iter.Current.GetAttribute("guid", ns);
                    int serverID = brokerDb.GetProcessAgentID(lsGuid);
                    if (serverID > 0)
                    {
                        AdministrativeAPI.LabServerClient_Insert(serverID, newClientId, order);
                        order++;
                    }
                }
            }

            // deal with resources 
            iter = xdoc.Select("/clientDescriptor/clientInfos/*");
            if (iter != null && iter.Count > 0)
            {
                while (iter.MoveNext())
                {
                    ClientInfo clientInfo = new ClientInfo();
                    clientInfo.infoURL = iter.Current.GetAttribute("infoUrl", ns);
                    clientInfo.infoURLName = iter.Current.GetAttribute("infoUrlName", ns);
                    clientInfo.description = iter.Current.GetAttribute("description", ns);
                    clientInfo.displayOrder = Int32.Parse(iter.Current.GetAttribute("displayOrder", ns));
                    AdministrativeAPI.InsertLabClientInfo(newClientId, clientInfo.infoURL, clientInfo.infoURLName,
                        clientInfo.description, clientInfo.displayOrder);
                }
            }
          
            message.Append(" Registered a new LabClient. ");
            message.AppendLine(" GUID: " + guid + " -> " + name);
            return newClientId;
        }
    

        //public string writeClientXml(string nameSpace, string rootElement,
        //    Dictionary<string, object> keyValueDictionary,
        //     int[] servers, ClientInfo[] infos)
        //{
        //    try
        //    {
        //        XmlWriterSettings settings = new XmlWriterSettings();
        //        settings.Indent = true;
        //        settings.OmitXmlDeclaration = true;
        //        settings.NewLineOnAttributes = true;
        //        settings.CheckCharacters = true;

        //        StringBuilder stringBuilder = new StringBuilder();
        //        XmlWriter xmlWriter = XmlWriter.Create(stringBuilder, settings);
               
        //        // write root element
        //        xmlWriter.WriteStartElement(rootElement);
        //        //xmlWriter.WriteAttributeString("xmlns", "ns", null, nameSpace);

        //        foreach (string s in keyValueDictionary.Keys)
        //        {
        //            xmlWriter.WriteStartElement(s);
        //            object value = new object();
        //            keyValueDictionary.TryGetValue(s, out value);
        //            xmlWriter.WriteString(value.ToString());
        //            xmlWriter.WriteEndElement();
        //        }
        //        if(servers != null && servers.Length > 0){
        //            xmlWriter.WriteStartElement("labServers");
        //            for (int i = 0; i < servers.Length; i++)
        //            {
        //                ProcessAgent server = brokerDb.GetProcessAgent(servers[i]);
        //                if (server != null)
        //                {
        //                    xmlWriter.WriteStartElement("serverGuid");
        //                    xmlWriter.WriteString(server.agentGuid);
        //                    xmlWriter.WriteEndElement();
        //                }

        //            }
        //            xmlWriter.WriteEndElement();

        //        }
        //        if (infos != null && infos.Length > 0)
        //        {
        //            xmlWriter.WriteStartElement("clientResources");
        //            foreach (ClientInfo i in infos)
        //            {
        //                WriteClientInfo(xmlWriter, i);
        //            }
        //            xmlWriter.WriteEndElement();

        //        }

        //        xmlWriter.WriteEndElement();
        //        xmlWriter.Flush();
        //        return stringBuilder.ToString();
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.Message);
        //        Console.WriteLine(e.StackTrace);
        //    }

        //    return null;
        //}

        protected void WriteClientInfo(XmlWriter xmlWriter, ClientInfo info)
        {
            xmlWriter.WriteStartElement("clientInfo");
            if (info.infoURL != null)
            {
                xmlWriter.WriteAttributeString("infoUrl",info.infoURL);
            }
            if (info.infoURLName != null)
            {
                xmlWriter.WriteAttributeString("infoUrlName", info.infoURLName);
            }
            if (info.description != null)
            {
                xmlWriter.WriteAttributeString("description", info.description);
            }
            if (info.displayOrder != null)
            {
                xmlWriter.WriteAttributeString("displayOrder", info.displayOrder.ToString());
            }

            xmlWriter.WriteEndElement();

        }


        public string writeAgentXml(string nameSpace, string rootElement,
            Dictionary<string, object> keyValueDictionary,
            string resourceBlockName, Dictionary<string, object> resources)
        {
            try
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.OmitXmlDeclaration = true;
                settings.NewLineOnAttributes = true;
                settings.CheckCharacters = true;

                StringBuilder stringBuilder = new StringBuilder();
                XmlWriter xmlWriter = XmlWriter.Create(stringBuilder, settings);

                // write root element
                xmlWriter.WriteStartElement(rootElement);
                //xmlWriter.WriteAttributeString("xmlns", "ns", null, nameSpace);

                foreach (string s in keyValueDictionary.Keys)
                {
                    xmlWriter.WriteStartElement(s);
                    object value = new object();
                    keyValueDictionary.TryGetValue(s, out value);
                    xmlWriter.WriteString(value.ToString());
                    xmlWriter.WriteEndElement();
                }
                if (resources != null && resources.Count > 0)
                {
                    xmlWriter.WriteStartElement(resourceBlockName);
                    foreach (string s in resources.Keys)
                    {
                        string tmp = s.Replace(' ', '_');
                        //tmp.Replace(' ', '_');
                        xmlWriter.WriteStartElement(HttpUtility.HtmlEncode(tmp));
                        xmlWriter.WriteString(resources[s].ToString());
                        xmlWriter.WriteEndElement();
                    }
                    xmlWriter.WriteEndElement();

                }

                xmlWriter.WriteEndElement();
                xmlWriter.Flush();
                return stringBuilder.ToString();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }

            return null;
        }

        public string CreateGroupCredentialDescriptor(string sbGuid, string sbName, string groupName,string ussGuid, string lssGuid)
        {
            StringBuilder stringBuilder = new StringBuilder();
            try
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.OmitXmlDeclaration = true;
                settings.NewLineOnAttributes = true;
                settings.CheckCharacters = true;


                XmlWriter xmlWriter = XmlWriter.Create(stringBuilder, settings);


                // write root element
                xmlWriter.WriteStartElement("credentialDescriptor");
                //xmlWriter.WriteAttributeString("xmlns", "ns", null, nameSpace);

                xmlWriter.WriteElementString("domainGuid", sbGuid);
                xmlWriter.WriteElementString("serviceBrokerName", sbName);
                xmlWriter.WriteElementString("groupName", groupName);
                xmlWriter.WriteElementString("ussGuid", ussGuid);
                if(lssGuid != null)
                    xmlWriter.WriteElementString("lssGuid", lssGuid);
                xmlWriter.WriteEndElement();
                xmlWriter.Flush();
                return stringBuilder.ToString();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }

            return null;
        }

        public GroupCredential ParseCredential(XmlQueryDoc xdoc, ref StringBuilder message)
        {
            GroupCredential credential = new GroupCredential();
            credential.domainGuid = xdoc.Query("/credentialDescriptor/domainGuid");
            credential.domainServerName = xdoc.Query("/credentialDescriptor/serviceBrokerName");
            credential.groupName = xdoc.Query("/credentialDescriptor/groupName");
            credential.ussGuid = xdoc.Query("/credentialDescriptor/ussGuid");
            credential.lssGuid = xdoc.Query("/credentialDescriptor/lssGuid");
            return credential;
        }
        
    }
}
