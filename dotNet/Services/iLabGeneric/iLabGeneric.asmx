<%@ WebService Language="C#" Class="iLabs.Generic.iLabGeneric" %>

using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;

using System.IO;
using System.Data;
using System.Configuration;
using System.Xml;
using System.Xml.Serialization;

using iLabs.Core;
using iLabs.DataTypes;
using iLabs.DataTypes.ProcessAgentTypes;
using iLabs.DataTypes.StorageTypes;
using iLabs.DataTypes.SoapHeaderTypes;
using iLabs.DataTypes.TicketingTypes;
using iLabs.UtilLib;
using iLabs.Ticketing;
using iLabs.Proxies.PAgent;
using iLabs.Proxies.ESS;
using iLabs.Web;


namespace iLabs.Generic
{
    [WebService(Namespace = "http://ilab.mit.edu/iLabs/Services")]
    [XmlType(Namespace = "http://ilab.mit.edu/iLabs/type")]
    [WebServiceBinding(Name = "IProcessAgent", Namespace = "http://ilab.mit.edu/iLabs/Services")]

    public class iLabGeneric : WS_ILabCore
    {
  
        public OperationAuthHeader opHeader = new OperationAuthHeader();

    }
}
