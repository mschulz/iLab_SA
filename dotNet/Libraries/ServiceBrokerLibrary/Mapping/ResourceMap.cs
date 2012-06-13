using System;
using System.Collections.Generic;
using iLabs.DataTypes.TicketingTypes;
using iLabs.Ticketing;
using iLabs.TicketIssuer;

namespace iLabs.ServiceBroker.Mapping
{

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
    public class ResourceMap
    {
        public enum ResourceType {
             GROUP = 1, PROCESS_AGENT, CLIENT, RESOURCE_MAPPING, TICKET_TYPE,
             RESOURCE_TYPE, STRING 
        };

        //static string[] Types = { "PROCESS_AGENT", "CLIENT", "RESOURCE_MAPPING", "STRING", "TICKET_TYPE", "GROUP" };
       // static string[] Types = { "PROCESS_AGENT", "CLIENT", "RESOURCE_MAPPING", "STRING", "TICKET_TYPE", "GROUP", "RESOURCE_TYPE" };


        static Dictionary<ResourceType, string> typeNames = new Dictionary<ResourceType, string>();
        
        static ResourceMap() {
            typeNames.Add(ResourceType.PROCESS_AGENT, "Process Agent");
            typeNames.Add(ResourceType.CLIENT, "Client");
            typeNames.Add(ResourceType.GROUP, "Group");
            typeNames.Add(ResourceType.RESOURCE_MAPPING, "Resource Mapping");
            typeNames.Add(ResourceType.TICKET_TYPE, "Ticket Type");
            typeNames.Add(ResourceType.STRING, "String");
            
            typeNames.Add(ResourceType.GROUP, "Group");
            typeNames.Add(ResourceType.RESOURCE_TYPE, "Resource Type");
        }
    
        public static bool IsResourceMapType(string type) {
            if (type.Equals(ResourceType.PROCESS_AGENT.ToString()) ||
                type.Equals(ResourceType.CLIENT.ToString()) ||
                type.Equals(ResourceType.RESOURCE_MAPPING.ToString()) ||
                type.Equals(ResourceType.STRING.ToString()) ||
                type.Equals(ResourceType.TICKET_TYPE.ToString()) ||
                type.Equals(ResourceType.GROUP.ToString()) ||
                type.Equals(ResourceType.RESOURCE_TYPE.ToString()))
                return true;

            return false;
        }

        public static bool IsResourceMapType(ResourceType type)
        {
            if (type.Equals(ResourceType.PROCESS_AGENT.ToString()) ||
                type.Equals(ResourceType.CLIENT.ToString()) ||
                type.Equals(ResourceType.RESOURCE_MAPPING.ToString()) ||
                type.Equals(ResourceType.STRING.ToString()) ||
                type.Equals(ResourceType.TICKET_TYPE.ToString()) ||
                type.Equals(ResourceType.GROUP.ToString()) ||
                type.Equals(ResourceType.RESOURCE_TYPE.ToString()))
                return true;

            return false;
        }
        public static bool IsResourceMapType(int type)
        {
            return (type > 0 && type <= (int)ResourceType.STRING);
        }

        public static string GetResourceMapType(int id)
        {
            if (IsResourceMapType(id))
                return null;

            return ((ResourceType)id).ToString();
        }

        public static ResourceType GetResourceMapType(string type)
        {
            ResourceType typeId = 0;
            //if (type.Equals(PROCESS_AGENT))
            //    typeId = ResourceType.PROCESS_AGENT;
            //else if (type.Equals(CLIENT))
            //    typeId = ResourceType.CLIENT;
            //else if (type.Equals(RESOURCE_MAPPING))
            //    typeId = ResourceType.RESOURCE_MAPPING;
            //else if (type.Equals(STRING))
            //    typeId = ResourceType.STRING;
            //else if (type.Equals(TICKET_TYPE))
            //    typeId = ResourceType.TICKET_TYPE;
            //else if (type.Equals(GROUP))
            //    typeId = ResourceType.GROUP;
            //else if (type.Equals(RESOURCE_TYPE))
            //    typeId = ResourceType.RESOURCE_TYPE;

            return typeId;
        }

        public static string GetTypeName(int type)
        {
            string typeName;
            typeNames.TryGetValue((ResourceType) type, out typeName);
            return typeName;
        }

  
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
        public ResourceMapKey key;

        public ResourceMapKey Key
        {
            get
            {
                return key;
            }
        }

        /// <summary>
        /// Resource Mapping Values
        /// </summary>
        public ResourceMapValue[] values;

        /// <summary>
        /// 
        /// </summary>
        public ResourceMap()
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
        public ResourceMap(int resourceMappingId, ResourceMapKey key, ResourceMapValue[] values)
        {
            this.resourceMappingId = resourceMappingId;
            this.key = key;
            this.values = values;
        }
    }	
    

    public class ResourceMapValue
    {
        protected internal ResourceMap.ResourceType type;
        protected internal object value;

        public ResourceMapValue()
        {
        }

        public ResourceMapValue(string valueType, object entry)
        {
            // check that the mapping type is valid
            if (!ResourceMap.IsResourceMapType(valueType))
                throw new ArgumentException("Agrument \"" + type + "\" is an illegal resource mapping type", "type");
            type = ResourceMap.GetResourceMapType(valueType);
            // check that the mapping entry is of the appropriate type
            if (!((type.Equals(ResourceMap.ResourceType.GROUP) && entry is int) ||
                (type.Equals(ResourceMap.ResourceType.PROCESS_AGENT) && entry is int)||
                (type.Equals(ResourceMap.ResourceType.CLIENT) && entry is int) ||
                (type.Equals(ResourceMap.ResourceType.RESOURCE_MAPPING) && entry is int) ||                
                (type.Equals(ResourceMap.ResourceType.TICKET_TYPE) && entry is TicketType) ||                
                (type.Equals(ResourceMap.ResourceType.RESOURCE_TYPE) && entry is string) ||
                (type.Equals(ResourceMap.ResourceType.STRING) && entry is string)))
                throw new ArgumentException("Agrument \"entry\" is not of the correct type", "entry");

            this.value = entry;
        }

        public int GetId(object entry)
        {
            // ProcessAgent, client or group or resource mapping
            if (value is int)
                return (int)value;
            else if (value is TicketType)
                return ((TicketType)value).ticketTypeId;
            return -1;
        }

        public string TypeName
        {
            get
            {
                return type.ToString() ;
            }
        }

        public ResourceMap.ResourceType Type
        {
            get
            {
                return type;
            }
        }

        public object Value
        {
            get
            {
                return Value;
            }
        }

        public override bool Equals(object obj)
        {
            ResourceMapValue mappingEntry = (ResourceMapValue)obj;
            return (mappingEntry.Type.Equals(this.type) && mappingEntry.Value.Equals(this.value));
        }

    }

    public class ResourceMapKey
    {
        protected internal ResourceMap.ResourceType type;
        protected internal int value;
        public ResourceMapKey()
        {
        }

        /// <summary>
        /// Redefined to disallow mapping keys that are Resource mappings
        /// </summary>
        /// <param name="type"></param>
        /// <param name="entry"></param>
        public ResourceMapKey(ResourceMap.ResourceType type, int key)
        {


            // check that the mapping type is valid
            if (!ResourceMap.IsResourceMapType(type))
                throw new ArgumentException("Agrument \"" + type + "\" is an illegal resource mapping type", "type");

            // check that the mapping entry is of the appropriate type
            if (!(
                (type.Equals(ResourceMap.ResourceType.CLIENT) && value is int) ||
                (type.Equals(ResourceMap.ResourceType.PROCESS_AGENT) && value is int) ||
                (type.Equals(ResourceMap.ResourceType.STRING) && value is string) ||
                (type.Equals(ResourceMap.ResourceType.TICKET_TYPE) && value is TicketType) ||
                (type.Equals(ResourceMap.ResourceType.GROUP) && value is int) ||
                (type.Equals(ResourceMap.ResourceType.RESOURCE_TYPE) && value is string)))
            {
                throw new ArgumentException("Agrument \"entry\" is not of the correct type", "entry");
            }

            this.type = type;
            value = key;

        }

        public ResourceMap.ResourceType Type
        {
            get
            {
                return type;
            }
        }

        public object Key
        {
            get
            {
                return value;
            }
        }

         public override bool Equals(object obj)
        {
            ResourceMapKey mappingKey = (ResourceMapKey)obj;
            return (mappingKey.Type.Equals(this.type) && mappingKey.Key.Equals(this.value));
        }

    }

   

 
}
