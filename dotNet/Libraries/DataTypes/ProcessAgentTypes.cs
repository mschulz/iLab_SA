/* $Id$ */

using System;

using iLabs.DataTypes.TicketingTypes;

namespace iLabs.DataTypes.ProcessAgentTypes
{
	
    /// <summary>
    /// Minimum information about a processAgent. Once in a domain this data should be immutable.
    /// </summary>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://ilab.mit.edu/iLabs/type")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://ilab.mit.edu/iLabs/type")]
    public class ProcessAgent : IComparable
    {

        /// <summary>
        /// The agents Guid - limit to 50 characters
        /// </summary>
        public string agentGuid;

        /// <summary>
        /// Human readable name.
        /// </summary>
        public string agentName;

        /// <summary>
        /// On of the pre-defined types
        /// </summary>
        public string type;

        /// <summary>
        /// The domain server Guid - limit to 50 characters, may be null during WebService Calls.
        /// </summary>
        public string domainGuid;

        /// <summary>
        /// The fully qualified URL for the site's root directory
        /// </summary>
        public string codeBaseUrl;

        /// <summary>
        /// The fully qualified URL for the Web Service Page
        /// </summary>
        public string webServiceUrl;

        /// <summary>
        /// Default constructor
        /// </summary>
        public ProcessAgent()
        {

        }

        public ProcessAgent(string agentGuid, string agentName, string type,
           string domainGuid, string codeBaseUrl, string webServiceUrl)
        {
            this.agentGuid = agentGuid;
            this.agentName = agentName;
            this.type = type;
            this.domainGuid = domainGuid;
            this.codeBaseUrl = codeBaseUrl;
            this.webServiceUrl = webServiceUrl;

        }

        public override bool Equals(object obj)
        {
            ProcessAgent pa = (ProcessAgent)obj;
            return (pa.agentGuid.Equals(this.agentGuid) && pa.webServiceUrl.Equals(this.webServiceUrl));

        }

        public override int GetHashCode()
        {
            return agentGuid.GetHashCode() + webServiceUrl.GetHashCode();
        }

        #region IComparable Members

        public int CompareTo(object obj)
        {
            return this.agentName.CompareTo(((ProcessAgent)obj).agentName);
        }

        #endregion

    }

    /// <summary>
    /// Provides a class to contain all parts of a domainCredential set, used to return any changes on a ModifyCredentialSet call.
    /// </summary>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://ilab.mit.edu/iLabs/type")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://ilab.mit.edu/iLabs/type")]
    public class DomainCredentials
    {
        /// <summary>
        /// The ProcessAgent
        /// </summary>
        public ProcessAgent agent;
        /// <summary>
        /// The coupon that will be used on all incoming messages from the domain server, it may be null.
        /// </summary>
        public Coupon inCoupon;
        /// <summary>
        /// The coupon that will be used on all outgoing messages to the domain server, it may be null.
        /// </summary>
        public Coupon outCoupon;

        public DomainCredentials()
        {
            agent = null;
            inCoupon = null;
            outCoupon = null;
        }

        public DomainCredentials(ProcessAgent processAgent,Coupon inCoupon, Coupon outCoupon)
        {
            agent = processAgent;
            this.inCoupon = inCoupon;
            this.outCoupon = outCoupon;
        }
    }


    /// <summary>
    /// Information about available services.
    /// </summary>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://ilab.mit.edu/iLabs/type")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://ilab.mit.edu/iLabs/type")]
    public class ServiceDescription
    {
        /// <summary>
        /// An XML-encoded string that describes the services provided by the service provider described by this registry entry.
        /// </summary>
        public string serviceProviderInfo;
        /// <summary>
        /// An optional coupon that may be used to redeem the described service.
        /// </summary>
        public Coupon coupon;
        /// <summary>
        /// An optional XML-encoded string that provides information required by the consumer. 
        /// This information may be parsed by the consuming site and may be reformated for use.
        /// Part of this field may be used by a remote service broker to route the service call.
        /// </summary>
        public string consumerInfo;

        /// <summary>
        /// 
        /// </summary>
        public ServiceDescription()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceProviderInfo"></param>
        /// <param name="coupon"></param>
        /// <param name="consumerInfo"></param>
        public ServiceDescription(string serviceProviderInfo, Coupon coupon, string consumerInfo)
        {
            this.serviceProviderInfo = serviceProviderInfo;
            this.coupon = coupon;
            this.consumerInfo = consumerInfo;
        }
    }

    /// <summary>
    /// Report returned after a call to a processAgent's GetStatus method.
    /// </summary>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://ilab.mit.edu/iLabs/type")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://ilab.mit.edu/iLabs/type")]
    public class StatusReport
    {
        public bool online;
        public string serviceGuid;
        public string payload;
    }

    /// <summary>
    /// Information sent to a processAgent via a StatusNotification call. 
    /// Currently processing of the notification is not specified. 
    /// </summary>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://ilab.mit.edu/iLabs/type")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://ilab.mit.edu/iLabs/type")]
    public class StatusNotificationReport
    {
        public int alertCode;
        public string serviceGuid;
        public DateTime time;
        public string payload;
    }
}