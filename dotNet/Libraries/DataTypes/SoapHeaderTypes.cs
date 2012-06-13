/* $Id$ */

using System;
using System.Web.Services.Protocols;

using iLabs.DataTypes.TicketingTypes;

namespace iLabs.DataTypes.SoapHeaderTypes
{

    /// <summary>
    /// Header type used for the initial registration of domain credentials.
    /// </summary>
	[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://ilab.mit.edu/iLabs/type")]
	[System.Xml.Serialization.XmlRootAttribute(Namespace="http://ilab.mit.edu/iLabs/type", IsNullable=false)]
	public class InitAuthHeader : SoapHeader 
	{
		/// <remarks/>
		public string initPasskey;
	}


    /// <summary>
    /// Generic header with a single coupon.
    /// </summary>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://ilab.mit.edu/iLabs/type")]
	[System.Xml.Serialization.XmlRootAttribute(Namespace="http://ilab.mit.edu/iLabs/type", IsNullable=false)]
    public class AuthenticationHeader : SoapHeader
    {
        public Coupon coupon;

    }
    /// <summary>
    /// Agent authorization
    /// </summary>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://ilab.mit.edu/iLabs/type")]
	[System.Xml.Serialization.XmlRootAttribute(Namespace="http://ilab.mit.edu/iLabs/type", IsNullable=false)]
    public class AgentAuthHeader : AuthenticationHeader
    {
        public string agentGuid;
    }

    /// <summary>
    /// Broker authorization.
    /// </summary>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://ilab.mit.edu/iLabs/type")]
	[System.Xml.Serialization.XmlRootAttribute(Namespace="http://ilab.mit.edu/iLabs/type", IsNullable=false)]
    public class BrokerAuthHeader : AuthenticationHeader
    {

    }
    /// <summary>
    /// Header for operation requests.
    /// </summary>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://ilab.mit.edu/iLabs/type")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://ilab.mit.edu/iLabs/type", IsNullable = false)]
    public class OperationAuthHeader : AuthenticationHeader
    {
    }


}

