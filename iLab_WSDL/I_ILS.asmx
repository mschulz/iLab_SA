<%@ WebService Language="c#" Class="I_ILS" %>


/* $Id$ */
using System;
using System.Collections;
using System.Configuration;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;

using System.Web.Services.Protocols;
using System.Xml;
using System.Xml.Serialization;
using System.Threading;

using iLabs.DataTypes;
using iLabs.DataTypes.SoapHeaderTypes;
using iLabs.DataTypes.TicketingTypes;

	/// <summary>
	/// The minimum interface definition for an InteractiveLabServer. Currently only one method, 
    /// additional methods may be added as needed.
	/// </summary>
    [XmlType(Namespace = "http://ilab.mit.edu/iLabs/Type")]
    [WebServiceBinding(Name = "I_ILS", Namespace = "http://ilab.mit.edu/iLabs/Services"),
    WebService(Name = "InteractiveLSProxy", Namespace = "http://ilab.mit.edu/iLabs/Services",
        Description="The minimum interface definition for an InteractiveLabServer. Currently only one method, "
        + "additional methods may be added as needed.")]
    public abstract class I_ILS : System.Web.Services.WebService
	{
    
        public AgentAuthHeader agentAuthHeader = new AgentAuthHeader();
       

        /// <summary>
        /// Alert is used by the LabScheduling Server to notify the lab server about a scheduled event other than an experiment 
        /// execution. The actual payload format and response to this method is be determined by the LSS and LabServer implementation.
        /// </summary>
        /// <param name="payload">Defines the alert parameters<wakeup><groupName></groupName><guid></guid><executionTime></executionTime></wakeup></param>
        /// <returns></returns>
        [WebMethod(Description = "Alert is used by the LabScheduling Server to notify the lab server about a scheduled event other than an experiment execution."
            + " The actual payload format and response to this method is be determined by the LSS and LabServer implementation. Currently NOP in reference implementation."),
        SoapHeader("agentAuthHeader", Direction = SoapHeaderDirection.In),
        SoapDocumentMethod(Binding = "I_ILS")]
        public abstract void Alert(string payload);
	}
