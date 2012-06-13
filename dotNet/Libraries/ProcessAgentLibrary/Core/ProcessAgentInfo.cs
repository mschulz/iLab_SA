using System;
using iLabs.DataTypes.ProcessAgentTypes;
using iLabs.DataTypes.TicketingTypes;


namespace iLabs.Core
{
	/// <summary>
	/// Summary description for ProcessAgentInfo.
	/// </summary>
	public class ProcessAgentInfo
	{
        /// <summary>
        /// The local database primary key for the processAgent entry
        /// </summary>
        public int agentId;
        /// <summary>
        /// A meaningful human readable name for the service, does not need to be unique.
        /// </summary>
        public string agentName;
        /// <summary>
        /// The globally unique identifier for the service, this may not be modified.
        /// </summary>
        public string agentGuid;
        public ProcessAgentType.AgentType agentType;
        public string domainGuid;
        public string codeBaseUrl;
        public string webServiceUrl;
        public string issuerGuid;
        /// <summary>
        /// The coupon for incoming messages from the specified processagent to this local service
        /// </summary>
        public Coupon identIn = null;
        /// <summary>
        /// The coupon for messages from this local service to the specified processAgent
        /// </summary>
        public Coupon identOut = null;
        public bool retired = false;

		public ProcessAgentInfo(){}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="guid">The globally unique identifier for the service, this may not be modified.</param>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="codeBase"></param>
        /// <param name="servicePage"></param>
        /// <param name="inCoupon">The coupon for incoming messages from the processagent to the local service</param>
        /// <param name="outCoupon">The coupon for messages from the local service to this processAgent</param>
        public ProcessAgentInfo(int id, string guid, string name, int type,
             string domainGuid, string codeBase, string servicePage,
            string issuerGuid, Coupon inCoupon, Coupon outCoupon)
		{
			agentId = id;
            agentGuid = guid;
			agentName = name;
			agentType = (ProcessAgentType.AgentType) type;
            this.domainGuid = domainGuid;
            codeBaseUrl = codeBase;
			webServiceUrl = servicePage;
            this.issuerGuid = issuerGuid;
			identIn = inCoupon;
			identOut = outCoupon;
		}

		public string AgentGuid
		{
				get
				{
					return agentGuid;
				}

                set {
                    agentGuid = value; 
                }
			}	
		public int AgentType
		{
			get
			{
				return (int) agentType;
			}

		}
		public string AgentTypeName
		{
			get
			{
                return ProcessAgentType.ToTypeName(agentType);
			}

		}
		public string AgentName
		{
			get
			{
				return agentName;
			}

		}	
		public string ServiceUrl
		{
			get
			{
				return webServiceUrl;
			}

		}
        public string CodeBaseUrl
        {
            get
            {
                return codeBaseUrl;
            }

        }
		public int AgentId
		{
			get
			{
				return agentId;
			}

		}	

		


	}

	
}
