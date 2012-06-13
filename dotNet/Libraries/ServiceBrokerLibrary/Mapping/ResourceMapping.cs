using System;
using System.Collections.Generic;
using iLabs.DataTypes.TicketingTypes;
using iLabs.Ticketing;
using iLabs.TicketIssuer;

namespace iLabs.ServiceBroker.Mapping
{

    public class ResourceMappingTypes
    {
        public const string PROCESS_AGENT = "PROCESS_AGENT";
        public const string CLIENT = "CLIENT";
        public const string RESOURCE_MAPPING = "RESOURCE_MAPPING";
        public const string STRING = "STRING";
        public const string TICKET_TYPE = "TICKET_TYPE";
        public const string GROUP = "GROUP";
        public const string RESOURCE_TYPE = "RESOURCE_TYPE";

        static string[] Types = { "PROCESS_AGENT", "CLIENT", "RESOURCE_MAPPING", "STRING", "TICKET_TYPE", "GROUP", "RESOURCE_TYPE" };


        static Dictionary<string, string> typeNames = new Dictionary<string, string>();
        
        static ResourceMappingTypes() {
            typeNames.Add(PROCESS_AGENT, "Process Agent");
            typeNames.Add(CLIENT, "Client");
            typeNames.Add(RESOURCE_MAPPING, "Resource Mapping");
            typeNames.Add(STRING, "String");
            typeNames.Add(TICKET_TYPE, "Ticket Type");
            typeNames.Add(GROUP, "Group");
            typeNames.Add(RESOURCE_TYPE, "Resource Type");
        }
    
        public static bool IsResourceMappingType(string type) {
            if (type.Equals(PROCESS_AGENT) ||
                type.Equals(CLIENT) ||
                type.Equals(RESOURCE_MAPPING) ||
                type.Equals(STRING) ||
                type.Equals(TICKET_TYPE) ||
                type.Equals(GROUP) ||
                type.Equals(RESOURCE_TYPE))
                return true;

            return false;
        }

        public static string GetResourceMappingType(int id)
        {
            //if (id < 1 || id > 6)
            //    return null;
            if (id < 1 || id > 7)
                return null;

            return Types[id-1];
        }

        public static int GetResourceMappingTypeID(string type)
        {
            if (type.Equals(PROCESS_AGENT))
                return 1;
            else if (type.Equals(CLIENT))
                return 2;
            else if (type.Equals(RESOURCE_MAPPING))
                return 3;
            else if (type.Equals(STRING))
                return 4;
            else if (type.Equals(TICKET_TYPE))
                return 5;
            else if (type.Equals(GROUP))
                return 6;
            else if (type.Equals(RESOURCE_TYPE))
                return 7;

            return -1;
        }

        public static string GetName(string type)
        {
            string typeName;
            typeNames.TryGetValue(type, out typeName);
            return typeName;
        }
    }

    public abstract class ResourceMappingEntry : IEquatable<ResourceMappingEntry>
    {
        protected internal string type;
        protected internal object entry;

        public ResourceMappingEntry()
        {
        }

        public ResourceMappingEntry(string type, object entry)
        {
            // check that the mapping type is valid
            if (!ResourceMappingTypes.IsResourceMappingType(type))
                throw new ArgumentException("Agrument \"" + type + "\" is an illegal resource mapping type", "type");

            // check that the mapping entry is of the appropriate type
            if (!((type.Equals(ResourceMappingTypes.PROCESS_AGENT) && entry is int)||
                (type.Equals(ResourceMappingTypes.CLIENT) && entry is int) ||
                (type.Equals(ResourceMappingTypes.RESOURCE_MAPPING) && entry is int) ||
                (type.Equals(ResourceMappingTypes.STRING) && entry is string) ||
                (type.Equals(ResourceMappingTypes.TICKET_TYPE) && entry is TicketType) ||
                (type.Equals(ResourceMappingTypes.GROUP) && entry is int) ||
                (type.Equals(ResourceMappingTypes.RESOURCE_TYPE) && entry is string)))
                throw new ArgumentException("Agrument \"entry\" is not of the correct type", "entry");

            this.type = type;
            this.entry = entry;
        }

        public static int GetId(object entry)
        {
            // PRocessAgent, client or group or resource mapping
            if (entry is int)
                return (int)entry;
            else if (entry is TicketType)
                return ((TicketType)entry).ticketTypeId;
            return -1;
        }

        public string TypeName
        {
            get
            {
                return ResourceMappingTypes.GetName(type);
            }
        }

        public string Type
        {
            get
            {
                return type;
            }
        }

        public object Entry
        {
            get
            {
                return entry;
            }
        }

        public override bool Equals(object obj)
        {
            ResourceMappingEntry mappingEntry = (ResourceMappingEntry)obj;
            return (mappingEntry.Type.Equals(this.type) && mappingEntry.Entry.Equals(this.entry));
        }

        public bool Equals(ResourceMappingEntry obj)
        {
            
            return this.type.Equals(obj.type) && this.entry.Equals(obj.entry);
        }

        public override int GetHashCode()
        {
            return type.GetHashCode() ^ entry.GetHashCode();
        }

    }

    public class ResourceEntryComparer : EqualityComparer<ResourceMappingEntry>
    {
        public override bool Equals(ResourceMappingEntry x, ResourceMappingEntry y)
        {
            return x.Equals(y);
        }

        public override int GetHashCode(ResourceMappingEntry obj)
        {
            return obj.GetHashCode();
        }
    
    }

    public class ResourceMappingKey : ResourceMappingEntry
    {
        public ResourceMappingKey()
        {
        }

        /// <summary>
        /// Redefined to disallow mapping keys that are Resource mappings
        /// </summary>
        /// <param name="type"></param>
        /// <param name="entry"></param>
        public ResourceMappingKey(string type, object entry)
            : base(type, entry)
        {
            // check that the mapping type is valid
            if (!ResourceMappingTypes.IsResourceMappingType(type))
                throw new ArgumentException("Agrument \"" + type + "\" is an illegal resource mapping type", "type");

            // check that the mapping entry is of the appropriate type
            if (!((type.Equals(ResourceMappingTypes.CLIENT) && entry is int) ||
                (type.Equals(ResourceMappingTypes.PROCESS_AGENT) && entry is int) ||
                (type.Equals(ResourceMappingTypes.STRING) && entry is string) ||
                (type.Equals(ResourceMappingTypes.TICKET_TYPE) && entry is TicketType) ||
                (type.Equals(ResourceMappingTypes.GROUP) && entry is int) ||
                (type.Equals(ResourceMappingTypes.RESOURCE_TYPE) && entry is string)))
                throw new ArgumentException("Agrument \"entry\" is not of the correct type", "entry");

            base.type = type;
            base.entry = entry;
        }

        public object GetKey()
        {
            return base.Entry;
        }

    }

    public class ResourceMappingValue : ResourceMappingEntry
    {

        public ResourceMappingValue()
        {
        }

        public ResourceMappingValue(string type, object entry)
            : base(type, entry)
        {
        }

        public object GetValue()
        {
            return base.Entry;
        }

    }

    /// <summary>
    /// A mapping between different resources in the architecture.
    /// It maps a resource (called the key) to one or more other resources (called the values). 
    /// A type field species the type of the key. Another type field specifies the type of the value
    /// For example the mapping (n, (LS1, PROCESS_AGENT), (LSS1, PROCESS_AGENT)
    ///                                                   ("ALLOCATE_TIME", TICKET_TYPE)                                                           
    /// specifies that the process agent LS1 (which happens to be a lab server in this case) 
    /// can allocate time on the server side scheduling server LSS1
    /// The exact interpretation of the mapping is left to the business logic that uses it.
    /// </summary>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://ilab.mit.edu/iLabs/type")]
    public class ResourceMapping
    {
        /// <summary>
        /// ID of the resource mapping
        /// </summary>
        protected int resourceMappingId;

        public int MappingID
        {
            get
            {
                return resourceMappingId;
            }

        }

        /// <summary>
        /// Resource Mapping Key
        /// </summary>
        public ResourceMappingKey key;

        public ResourceMappingKey Key
        {
            get
            {
                return key;
            }
        }

        /// <summary>
        /// Resource Mapping Values
        /// </summary>
        public ResourceMappingValue[] values;

        /// <summary>
        /// 
        /// </summary>
        public ResourceMapping()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resourceMappingId"></param>
        /// <param name="resourceKey"></param>
        /// <param name="resourceKeyType"></param>
        /// <param name="resourceValue"></param>
        /// <param name="resourceValueType"></param>
        public ResourceMapping(int resourceMappingId, ResourceMappingKey key, ResourceMappingValue[] values)
        {
            this.resourceMappingId = resourceMappingId;
            this.key = key;
            this.values = values;
        }
    }	
}
