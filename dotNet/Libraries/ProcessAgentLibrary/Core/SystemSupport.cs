using System;
using System.Text;
using System.Xml;
using System.Xml.XPath;

using iLabs.DataTypes.ProcessAgentTypes;
using iLabs.DataTypes.TicketingTypes;
using iLabs.UtilLib;


namespace iLabs.Core
{

    public class SystemSupport
    {
        public string agentGuid;
        public string contactEmail;
        public string bugEmail;
        public string infoUrl;
        public string description;
        public string location;

        public static SystemSupport Parse(string xml)
        {

            XmlQueryDoc xdoc = new XmlQueryDoc(xml);
            return Parse(xdoc);
        }

        public static SystemSupport Parse(XmlQueryDoc xdoc)
        {
            SystemSupport ss = new SystemSupport();
            try
            {
                ss.agentGuid = xdoc.Query("/systemSupport/agentGuid");
                ss.bugEmail = xdoc.Query("/systemSupport/bugEmail");
                ss.contactEmail = xdoc.Query("/systemSupport/contactEmail");
                ss.infoUrl = xdoc.Query("/systemSupport/infoUrl");
                ss.description = xdoc.Query("/systemSupport/description");
                ss.location = xdoc.Query("/systemSupport/location");
                return ss;
            }
            catch (Exception e)
            {
                ;
            }
            return null;
        }
      

        public SystemSupport() { }

        public SystemSupport(string guid, string contactEmail, string bugEmail, string infoUrl, string description, string location)
        {
            agentGuid = guid;
            this.bugEmail = bugEmail;
            this.contactEmail = contactEmail;
            this.infoUrl = infoUrl;
            this.description = description;
            this.location = location;
        }

        public string ToXML()
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
                xmlWriter.WriteStartElement("systemSupport");
                //xmlWriter.WriteAttributeString("xmlns", "ns", null, nameSpace);

                if (agentGuid != null && agentGuid.Length > 0)
                    xmlWriter.WriteElementString("agentGuid", agentGuid);
                if (bugEmail != null && bugEmail.Length > 0)
                    xmlWriter.WriteElementString("bugEmail", bugEmail);
                if (contactEmail != null && contactEmail.Length > 0)
                    xmlWriter.WriteElementString("contactEmail", contactEmail);
                if (infoUrl != null && infoUrl.Length > 0)
                    xmlWriter.WriteElementString("infoUrl", infoUrl);
                if (description != null && description.Length > 0)
                    xmlWriter.WriteElementString("description", description);
                if (location != null && location.Length > 0)
                    xmlWriter.WriteElementString("location", location);
                xmlWriter.WriteEndElement();
                xmlWriter.Flush();
                return stringBuilder.ToString();
            }
            catch (Exception e)
            {
               Logger.WriteLine("SystemSupport.ToXML(): " + e.Message);
            }
            return null;
        }
    }
}
