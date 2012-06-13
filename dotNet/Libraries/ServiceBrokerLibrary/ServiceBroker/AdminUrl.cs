using System;

using iLabs.DataTypes.TicketingTypes;
using iLabs.Ticketing;


namespace iLabs.ServiceBroker
{
    /// <summary>
    /// A URL for administering a process agent
    /// </summary>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://ilab.mit.edu/iLabs/type")]
    public class AdminUrl
    {

        private int id;
        private int processAgentId;
        private string url;
        private TicketType ticketType; 


        public AdminUrl()
        {
        }

        public AdminUrl(int urlId, int processAgentId, string url, string ticketType)
        {
            if (!TicketTypes.TicketTypeExists(ticketType))
                throw new Exception("\"" + ticketType + "\" is not a legal ticket type.");

            this.id = urlId;
            this.processAgentId = processAgentId;
            this.url = url;
            this.ticketType = TicketTypes.GetTicketType(ticketType);
        }

        /// <summary>
        /// id of the admin URL
        /// </summary>
        public int Id
        {
            get
            {
                return id;
            }
        }

        /// <summary>
        /// ID of the process agent 
        /// </summary>
        public int ProcessAgentId
        {
            get
            {
                return processAgentId;
            }
        }

        /// <summary>
        /// Administration URL
        /// </summary>
        public string Url
        {
            get
            {
                return url;
            }
        }
        /// <summary>
        /// The ticket type associated with the administration function that the designated URL points to
        /// </summary>
        public TicketType TicketType
        {
            get
            {
                return ticketType;
            }
        }
    }
}
