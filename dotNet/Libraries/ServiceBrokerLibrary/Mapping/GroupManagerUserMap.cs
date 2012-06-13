using System;
using System.Collections.Generic;
using System.Text;

namespace iLabs.ServiceBroker.Mapping
{
   
    public class GroupManagerUserMap
    {
        public GroupManagerUserMap()
        {
        }

        public GroupManagerUserMap(string managerGroupName, string userGroupName, int grantID, int resourceMappingID)
        {
            ManagerGroupName = managerGroupName;
            UserGroupName = userGroupName;
            GrantID = grantID;
            ResourceMappingID = resourceMappingID;

        }
        public static GroupManagerUserMap Parse(string str){
            string [] param = str.Split(',');
            if(param.Length !=4){
                throw new IndexOutOfRangeException("Incorrect argument parsing GroupManagerUserMap");
            }
            return new GroupManagerUserMap(param[0].Trim(), param[1].Trim(), Int32.Parse(param[2]), Int32.Parse(param[3]));
        }

        public string ToCSV()
        {
            StringBuilder buf = new StringBuilder();
            buf.Append((managerGroupName != null) ? managerGroupName : "");
            buf.Append(",");
            buf.Append((userGroupName != null) ? userGroupName : "");
            buf.Append("," + grantID + "," + resourceMappingID);
            return buf.ToString();
        }

        public string ManagerGroupName
        {
            get
            {
                return managerGroupName;
            }

            set
            {
                managerGroupName = value;
            }
        }

        public string UserGroupName
        {
            get
            {
                return userGroupName;
            }

            set
            {
                userGroupName = value;
            }

        }

        public int GrantID
        {
            get
            {
                return grantID;
            }
            set
            {
                grantID = value;
            }
        }

        public int ResourceMappingID
        {
            get
            {
                return resourceMappingID;
            }
            set
            {
                resourceMappingID = value;
            }
        }

        private string managerGroupName;
        private string userGroupName;
        private int grantID;
        private int resourceMappingID;
    }

}
