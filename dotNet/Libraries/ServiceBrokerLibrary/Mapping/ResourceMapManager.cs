using System;
using System.Collections.Generic;
using System.Text;

using iLabs.UtilLib;

namespace iLabs.ServiceBroker.Mapping
{
    /// <summary>
    /// In memory cache of all ResourceMapping objects, this does not perform database calls, except on static constructor and Refresh().
    /// </summary>
    public static class ResourceMapManager
    {
        private static Dictionary<int,ResourceMapping> mappingCache;
        private static Dictionary<ResourceMappingEntry, List<int>> mapKeyCache;
        private static IEqualityComparer<ResourceMappingEntry>  entryComparer;
        private static bool needsRefresh = true;

        static ResourceMapManager()
        {
            entryComparer = new ResourceEntryComparer();
            mappingCache = new Dictionary<int, ResourceMapping>();
            mapKeyCache = new Dictionary<ResourceMappingEntry, List<int>>();
            
            Refresh();
        }

        static public bool NeedsRefresh
        {
            get
            {
                return needsRefresh;
            }
        }

        public static int Refresh()
        {
            int count = 0;
            mappingCache.Clear();
            mapKeyCache.Clear();
            BrokerDB db = new BrokerDB();
            List<ResourceMapping> list = db.RetrieveResourceMapping();
            if (list != null && list.Count > 0)
            {
                Add(list);
                count = list.Count;
            }
            needsRefresh = false;
            Logger.WriteLine("Refreshed ResourceMapManager: count= " + count);
            return count;
        }

        //public static int Refresh(int rmId)
        //{
        //    int count = 0;
        //    BrokerDB db = new BrokerDB();
        //    ResourceMapping rm = db.GetResourceMapping(rmId);
        //    if (list != null && list.Count > 0)
        //    {
        //        Add(list);
        //    }
        //    needsRefresh = false;
        //}

        public static void Add(ResourceMapping map)
        {
            if (map == null)
                return;

            if (mappingCache.ContainsKey(map.MappingID))
            {
                return;
            }
            else
            {
                mappingCache.Add(map.MappingID, map);
                if (mapKeyCache.ContainsKey(map.key))
                {
                    List<int> list = mapKeyCache[map.key];
                    if (list != null)
                    {
                        if(list.Contains(map.MappingID)){
                            throw new Exception("ResourceManager keyCache already contains mappinID " + map.MappingID);
                        }
                        else{
                            list.Add(map.MappingID);
                            mapKeyCache[map.key] = list;
                        }

                    }
                    else {
                        throw new Exception("ResourceManager keyCache key without list");
                    }
                }
                else
                {
                    List<int> newList = new List<int>();
                    newList.Add(map.MappingID);
                    mapKeyCache.Add(map.key,newList);
                }
            }
        }

        public static void Add(List<ResourceMapping> mappings)
        {
            foreach (ResourceMapping rm in mappings)
            {
                Add(rm);
            }

        }

        public static bool Contains(int mapId)
        {
            return mappingCache.ContainsKey(mapId);
        }

        public static bool Remove(int mapId)
        {
            bool status = false;
            if(mappingCache.ContainsKey(mapId)){
                ResourceMapping map = mappingCache[mapId];
                mappingCache.Remove(mapId);
                List<int> list = mapKeyCache[map.key];
                foreach (int rmId in list)
                {
                    if (rmId == map.MappingID)
                    {
                        list.Remove(rmId);
                        status = true;
                        break;
                    }
                }
                if (list.Count == 0)
                {
                    mapKeyCache.Remove(map.key);
                }
                else{
                    mapKeyCache[map.key] = list;
                }
            }

            return status;

        }
        public static bool Remove(ResourceMappingEntry key)
        {
            bool status = false;
            if(mapKeyCache.ContainsKey(key)){
                List<int> list = mapKeyCache[key];
                foreach (int rmId in list)
                {
                   mappingCache.Remove(rmId);
                }
                mapKeyCache.Remove(key);
                status = true;
            }
            return status;
        }

        /// <summary>
        /// Get a single ResourceMapping from memory
        /// </summary>
        /// <param name="mapId"></param>
        /// <returns></returns>
        public static ResourceMapping GetMap(int mapId)
        {
            if (mappingCache.ContainsKey(mapId))
                return mappingCache[mapId];
            else
                return null;
        }

        public static List<ResourceMapping> Get(ResourceMappingEntry key)
        {
            if (mapKeyCache.ContainsKey(key))
            {
                List<int> mapIds = mapKeyCache[key];
                List<ResourceMapping> maps = null;
                if (mapIds.Count > 0)
                {
                    maps = new List<ResourceMapping>();
                    foreach (int i in mapIds)
                    {
                        maps.Add(mappingCache[i]);
                    }
                }
                return maps;
            }
            else
                return null;
        }

        /// <summary>
        /// Returns a list of all ResourceMappings in memory.
        /// </summary>
        /// <returns></returns>
        public static List<ResourceMapping> Get()
        {
            List<ResourceMapping> list = new List<ResourceMapping>();
            list.AddRange(mappingCache.Values);
            return list;
        }

        public static void Clear()
        {
            mappingCache.Clear();
            mapKeyCache.Clear();
            needsRefresh = true;
        }

        public static List<ResourceMapping> Find(ResourceMappingEntry key, ResourceMappingValue[] values)
        {
            List<ResourceMapping> returnList = null;
            if (needsRefresh)
                Refresh();
            List<ResourceMapping> list = Get(key);
            if (list != null && list.Count > 0)
            {
                returnList = new List<ResourceMapping>();
                foreach (ResourceMapping rm in list)
                {
                    if (CheckMappingValues(values, rm.values))
                    {
                        returnList.Add(rm);
                    }
                }
            }
            return returnList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rmt">ResourceMappingType</param>
        /// <param name="sourceID">the id of the source object</param>
        /// <param name="pat">The ProcessAgentType of the associated object</param>
        /// <returns></returns>
        //public static int FindMapID(string rmt, int sourceID, string pat)
        //{
        //    return FindMapID(rmt, sourceID, ResourceMappingTypes.RESOURCE_TYPE, pat);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rmt">ResourceMappingType</param>
        /// <param name="sourceID">the id of the source object</param>
        /// <param name="pat">The ProcessAgentType of the associated object</param>
        /// <returns></returns>
        public static int FindMapID(string rmt, int sourceID, string targetType, string target)
        {
            int targetId = 0;
            ResourceMappingValue[] values = new ResourceMappingValue[1];

            values[0] = new ResourceMappingValue(targetType, target);
           
            List<ResourceMapping> mapList = ResourceMapManager.Find(new ResourceMappingKey(rmt, sourceID), values);
            if (mapList != null && mapList.Count > 0)
            {
                foreach (ResourceMapping rm in mapList)
                {
                    for (int i = 0; i < rm.values.Length; i++)
                    {
                        if (rm.values[i].Type == ResourceMappingTypes.PROCESS_AGENT)
                        {
                            targetId = (int)rm.MappingID;
                            break;
                        }
                    }
                }
            }
            return targetId;
        }
        
        public static List<int> FindMapIds(ResourceMappingEntry key, ResourceMappingValue[] values)
        {
            List<int> returnList = null;
            List<ResourceMapping> list = Get(key);
            if (list != null && list.Count > 0)
            {
                returnList = new List<int>();
                foreach (ResourceMapping rm in list)
                {
                    if (CheckMappingValues(values, rm.values))
                    {
                        returnList.Add(rm.MappingID);
                    }
                }
            }
            return returnList;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rmt">ResourceMappingType</param>
        /// <param name="sourceID">the id of the source object</param>
        /// <param name="pat">The ProcessAgentType of the associated object</param>
        /// <returns>AZero if not found</returns>
        public static int FindResourceProcessAgentID(string rmt, int sourceID, string pat)
        {
            int targetId = 0;
            ResourceMappingValue[] values = new ResourceMappingValue[1];

            values[0] = new ResourceMappingValue(ResourceMappingTypes.RESOURCE_TYPE, pat);
            //values[1] = new ResourceMappingValue(ResourceMappingTypes.TICKET_TYPE,
            //       TicketTypes.GetTicketType(TicketTypes.SCHEDULE_SESSION));
            List<ResourceMapping> mapList = ResourceMapManager.Find(new ResourceMappingKey(rmt, sourceID), values);
            if (mapList != null && mapList.Count > 0)
            {
                foreach (ResourceMapping rm in mapList)
                {
                    for (int i = 0; i < rm.values.Length; i++)
                    {
                        if (rm.values[i].Type == ResourceMappingTypes.PROCESS_AGENT)
                        {
                            targetId = (int)rm.values[i].Entry;
                            if (targetId > 0)
                                break;
                        }
                    }
                }
            }
            return targetId;
        }

        public static bool Update(ResourceMapping rm){
            bool status = false;
            if(mappingCache.ContainsKey(rm.MappingID)){
                mappingCache.Remove(rm.MappingID);
                mappingCache.Add(rm.MappingID,rm);
                status = true;
            }
            else{
                throw new Exception("Update ResourceMapping rm not in cache");
            }
            return status;
        }


        /// <summary>
        /// Checks if the first array of Resource Mapping values finds matches in the second array of values.
        /// Arrays may have different counts but array two's length must be greater than or equal to array one's length.
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static bool CheckMappingValues(ResourceMappingValue[] v1, ResourceMappingValue[] v2)
        {
            if (v1.Length > v2.Length)
            {
                return false;
            }
            else
            {
                int num1Values = v1.Length;
                int[] found = new int[num1Values];
                for (int n = 0; n < num1Values; n++)
                {
                    found[n] = -1;
                }

                for (int i = 0; i < num1Values; i++)
                {
                    for (int j = 0; j < v2.Length; j++)
                    {
                        if (v1[i].Equals(v2[j]))
                        {
                            found[i] = j;
                            break;
                        }
                    }
                }
                bool areEqual = true;
                foreach (int k in found)
                {
                    if (k == -1)
                    {
                        areEqual = false;
                        break;
                    }
                }
                return areEqual;
            }
        }


        /// <summary>
        /// Checks if an array of Resource Mapping values is Equal to another one
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static bool EqualMappingValues(ResourceMappingValue[] v1, ResourceMappingValue[] v2)
        {
            if (v1.Length != v2.Length)
                return false;
            else
                return CheckMappingValues(v1, v2);
        }


    }
}
