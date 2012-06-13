using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;

namespace iLabs.Ticketing
{
    /// <summary>
    /// Base for TicketLoadFactory, singleton 
    /// </summary>
    public class BasicTicketLoadFactory
    {
        protected static string ticketNS = "http://ilab.mit.edu/iLabs/tickets";        
        
        /// <summary>
        /// level of indentation of xml document produced
        /// </summary>
        protected static int indentation = 4;

        /// <summary>
        /// protected constructor
        /// </summary>
        protected BasicTicketLoadFactory()
        {
        }

        /// <summary>
        /// singleton instance
        /// </summary>
        protected static BasicTicketLoadFactory instance;

        public static BasicTicketLoadFactory Instance()
        {
            if (instance == null)
                instance = new BasicTicketLoadFactory();

            return instance;
        }

        /// <summary>
        /// Constructs the ticket payload.
        /// </summary>
        /// <param name="rootElement"></param>
        /// <param name="ticketType"></param>
        /// <param name="keyValueDictionary"></param>
        /// <returns></returns>
        public string writeTicketLoad(string rootElement, string ticketType, Dictionary<string, object> keyValueDictionary)
        {
            try
            {
                StringWriter stringWriter = new StringWriter();
                XmlTextWriter xmlWriter = new XmlTextWriter(stringWriter);
                //xmlWriter.Formatting = Formatting.Indented;
                //xmlWriter.Indentation = indentation;

                // write root element
                xmlWriter.WriteStartElement(rootElement);
                xmlWriter.WriteAttributeString("xmlns", "ns", null, ticketNS);

                // write ticket type
                xmlWriter.WriteStartAttribute("ticketType");
                xmlWriter.WriteString(ticketType);
                xmlWriter.WriteEndAttribute();

                foreach (string s in keyValueDictionary.Keys)
                {
                    xmlWriter.WriteStartElement(s);
                    object value = new object();
                    keyValueDictionary.TryGetValue(s, out value);
                    xmlWriter.WriteString(value.ToString());
                    xmlWriter.WriteEndElement();                 
                }

                xmlWriter.WriteEndElement();
                return stringWriter.ToString();            
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }

            return null;
        }
    }
}
