using System;
using System.Collections;

using iLabs.Core;

namespace iLabs.Ticketing
{
    public class TicketTypes
    {

        // abstract tickets
        public const string ADMINISTER_PA = "ADMINISTER PA";
        public const string MANAGE_PA = "MANAGE PA";

        public const string AUTHENTICATE = "AUTHENTICATE";
        public const string LS = "LS";
        public const string LSS = "LSS";
        public const string USS = "USS";
        public const string ESS = "ESS";


        // authentication tickets
        public const string AUTHENTICATE_SERVICE_BROKER = "AUTHENTICATE SERVICE BROKER";
        public const string AUTHENTICATE_AGENT = "AUTHENTICATE AGENT";

        // session redemption tickets
        public const string REDEEM_SESSION = "REDEEM SESSION";

        // ESS tickets
        public const string ADMINISTER_ESS = "ADMINISTER ESS";
        public const string ADMINISTER_EXPERIMENT = "ADMINISTER EXPERIMENT";
        public const string STORE_RECORDS = "STORE RECORDS";
        public const string RETRIEVE_RECORDS = "RETRIEVE RECORDS";

        // USS tickets
        public const string ADMINISTER_USS = "ADMINISTER USS";
        public const string MANAGE_USS_GROUP = "MANAGE USS GROUP";
        public const string SCHEDULE_SESSION = "SCHEDULE SESSION";
        public const string REDEEM_RESERVATION = "REDEEM RESERVATION";
        public const string REVOKE_RESERVATION = "REVOKE RESERVATION"; //Permanent ticket, issuesd by LSS domainServer
        public const string ALLOW_EXPERIMENT_EXECUTION = "ALLOW EXPERIMENT EXECUTION";

        // LSS tickets
        public const string ADMINISTER_LSS = "ADMINISTER LSS";
        public const string MANAGE_LAB = "MANAGE LAB";
        public const string REQUEST_RESERVATION = "REQUEST RESERVATION"; //Permanent ticket, Issued by USS domainServer
        public const string REGISTER_LS = "REGISTER LS";

        // LS tickets
        public const string ADMINISTER_LS = "ADMINISTER LS";
        public const string EXECUTE_EXPERIMENT = "EXECUTE EXPERIMENT";
        public const string CREATE_EXPERIMENT = "CREATE EXPERIMENT";

        public const string AUTHORIZE_ACCESS = "AUTHORIZE ACCESS";
        public const string AUTHORIZE_CLIENT = "AUTHORIZE CLIENT";

        /// <summary>
        /// static array of ticket types that exist in the DB
        /// </summary>
        protected static TicketType[] ticketTypes;

        static TicketTypes()
        {

            // if the ticket types have not been retrieved from the database, retrieve them now
            if (ticketTypes == null)
            {
                ProcessAgentDB dbTicketing = new ProcessAgentDB();
                ticketTypes = dbTicketing.RetrieveTicketTypes();
            }
        }

        public static TicketType GetTicketType(int ticketTypeId)
        {
            for (int i = 0; i < ticketTypes.Length; i++)
            {
                if (ticketTypes[i].ticketTypeId == ticketTypeId)
                    return ticketTypes[i];
            }
            return null;
        }

        public static TicketType GetTicketType(string ticketTypeName)
        {
            for (int i = 0; i < ticketTypes.Length; i++)
            {
                if (String.Compare(ticketTypes[i].name, ticketTypeName, true) == 0)
                    return ticketTypes[i];
            }
            return null;
        }

        public static TicketType[] GetTicketTypes()
        {
            return ticketTypes;
        }

        public static TicketType[] GetNonAbstractTicketTypes()
        {
            ArrayList list = new ArrayList();
            for (int i = 0; i < ticketTypes.Length; i++)
            {
                if (!ticketTypes[i].isAbstract)
                    list.Add(ticketTypes[i]);
            }
            TicketType[] types = (TicketType[])list.ToArray((new TicketType()).GetType());
            return types;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns><code>true</code> if a ticket type with called <code>name</code> exists, <code>false</code> otherwise</returns>
        public static bool TicketTypeExists(string name)
        {
            for (int i = 0; i < ticketTypes.Length; i++)
            {
                if (String.Compare(ticketTypes[i].name, name, true) == 0)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns><code>true</code> if the argument is a ticket type that is a derived type of the type ADMINISTER_PA, false otherwise</returns>
        public static bool IsAdministerPAType(string typeName)
        {
            //
            // change
            //
            if (typeName == null)
                return false;

            switch (typeName)
            {
                case ADMINISTER_LSS:
                    return true;
                case ADMINISTER_ESS:
                    return true;
                case ADMINISTER_LS:
                    return true;
                case ADMINISTER_USS:
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns><code>true</code> if the argument is a ticket type that is a derived type of the type MANAGE_PA, false otherwise</returns>
        public static bool IsManagePAType(string typeName)
        {
            //
            // change
            //
            if (typeName == null)
                return false;

            switch (typeName)
            {
                case MANAGE_LAB:
                    return true;
                case REGISTER_LS:
                    return true;
                case ADMINISTER_EXPERIMENT:
                    return true;
                case MANAGE_USS_GROUP:
                    return true;
                case REQUEST_RESERVATION:
                    return true;
            }

            return false;
        }

    }

    public class TicketTypeComparer : IComparer {

        public TicketTypeComparer() : base() { }

        int IComparer.Compare(object x, object y) {

            TicketType ticketTypeX = (TicketType) x;
            TicketType ticketTypeY = (TicketType) y;            

            if (ticketTypeX == null && ticketTypeY == null) 
            {
                return 0;
            }
            else if (ticketTypeX == null && ticketTypeY != null)
            {
                return -1;
            }
            else if (ticketTypeX != null && ticketTypeY == null)
            {
               return 1;
            } 
            else 
            {
                return ticketTypeX.shortDescription.CompareTo(ticketTypeY.shortDescription);
             }
        }
    }


    //[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://ilab.mit.edu/iLabs/type")]
    public class TicketType
    {
        /// <summary>
        /// 
        /// </summary>
        public int ticketTypeId;

        /// <summary>
        /// 
        /// </summary>
        public string name;

        /// <summary>
        /// 
        /// </summary>
        public string shortDescription;

        /// <summary>
        /// 
        /// </summary>
        public bool isAbstract;

        /// <summary>
        /// 
        /// </summary>
        public TicketType()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ticketTypeId"></param>
        /// <param name="name"></param>
        /// <param name="shortDescription"></param>
        public TicketType(int ticketTypeId, string name, string shortDescription, bool isAbstract)
        {
            this.ticketTypeId = ticketTypeId;
            this.name = name;
            this.shortDescription = shortDescription;
            this.isAbstract = isAbstract;
        }

        public string Name
        {
            get
            {
                return name;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>A string desctiption of the ticket type</returns>
        public override string ToString()
        {
            return "Ticket Type: id=" + ticketTypeId + " name=" + name + " description=" + shortDescription + " isAbstract=" + isAbstract;
        }
    }
}