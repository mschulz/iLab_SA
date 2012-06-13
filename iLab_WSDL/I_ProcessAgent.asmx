<%@ WebService Language="C#" Class="I_ProcessAgent" %>

using System;
using System.Xml.Serialization;
using System.Data;
using System.Data.Common;
using System.Collections;
using System.Configuration;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Text.RegularExpressions;
using System.Net;
using System.Xml;
using System.Threading;

using iLabs.DataTypes.ProcessAgentTypes;
using iLabs.DataTypes.TicketingTypes;
using iLabs.DataTypes.SoapHeaderTypes;

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[SoapDocumentService(RoutingStyle = SoapServiceRoutingStyle.RequestElement)]
[XmlType(Namespace = "http://ilab.mit.edu/iLabs/type")]
[WebServiceBinding(Name = "IProcessAgent", Namespace = "http://ilab.mit.edu/iLabs/Services")]
[WebService(Name = "ProcessAgentProxy", Namespace = "http://ilab.mit.edu/iLabs/Services",
    Description="The ProcessAgent Interface is implemented by all iLab interactive services and is responsible for "
    + "managing the credentials used for authentication and authorization between services. See Ticketing API for details."
    )]
public abstract class I_ProcessAgent  : System.Web.Services.WebService {


    /// <summary>
    /// 
    /// </summary>
    public InitAuthHeader initAuthHeader = new InitAuthHeader();
    public AgentAuthHeader agentAuthHeader = new AgentAuthHeader();
    public BrokerAuthHeader brokerAuthHeader = new BrokerAuthHeader();

    ////////////////////////////////////////////////////
    ///    IProcessAgent Methods                     ///
    ////////////////////////////////////////////////////

    
    /// <summary>
    /// Get the services local DateTime.
    /// </summary>
    /// <returns></returns>
    [WebMethod(Description = "Get the services local DateTime. This is not UTC"),
    SoapDocumentMethod(Binding = "IProcessAgent")]
    public abstract DateTime GetServiceTime();
   
    
    /// <summary>
    /// Get a simple StatusReport from the service.
    /// </summary>
    [WebMethod(Description = "Get a simple StatusReport from the service."),
    SoapDocumentMethod(Binding = "IProcessAgent")]
    public abstract StatusReport GetStatus();
    
    
    /// <summary>
    /// Send a status notification to this service, it is up to the service to take action.
    /// </summary>
    [WebMethod(Description = "Send a status notification to this service, it is up to the implementing "
    + "service to take action. Currently no action is taken."),
    SoapDocumentMethod(Binding = "IProcessAgent"),
    SoapHeader("agentAuthHeader", Direction = SoapHeaderDirection.In)]
    public abstract void StatusNotification(StatusNotificationReport report);
  
    /// <summary>
    /// Install the Domain credentials on this process agent.
    /// Please note this method depends on a 'bootstrap' ProcessAgent record in the 
    /// database which must be created via the SelfRegistration page.
    /// Also uses the initialPasskey value from Web.config as the header value.
    /// This service's connection and additional information is returned to the ServiceBroker.
    /// </summary>
    /// <param name="service" Description="used to provide information for the registering service"></param>
    /// <param name="inIdentCoupon" Description="For messages from the ServiceBroker"></param>
    /// <param name="outIdentCoupon" Description="For messages to the ServiceBroker"></param>
    /// <returns>The bootstrap ProcessAgent 'record' which describes this service.</returns>
    [WebMethod(Description="Install the Domain credentials on this ProcessAgent. "
    + "Please note this method depends on a 'bootstrap' ProcessAgent record in the "
    + "database which must be created via the SelfRegistration page. This may only be called from a ServiceBroker. "
    + "Also uses the initialPasskey value from Web.config as the header value.<br/>"
    + "This service's connection and additional information is returned to the ServiceBroker."),
    SoapDocumentMethod(Binding = "IProcessAgent"),
    SoapHeader("initAuthHeader", Direction = SoapHeaderDirection.In)]
    public abstract ProcessAgent InstallDomainCredentials(ProcessAgent service, Coupon inIdentCoupon, Coupon outIdentCoupon);

    /// <summary>
    /// Modify the specified services domain credentials on this static process agent. 
    /// Agent_guid is key to the service to be modified. The agentAuthorizationHeader must use 
    /// the old values. This will only be called from a serviceBroker to members of its domain, or to a different domains ServiceBroker.
    /// </summary>
    /// <param name="originalGuid">The current guid of the target service
    /// <param name="agent" Description="Used to provide information for the service to be modified, the agent guid must remain the same as the original for the specified service"></param>
    /// <param name="inCoupon" Description="New coupon for messages from the Service"></param>
    /// <param name="outCoupon" Description="New coupon for messages to the Service"></param>
    /// <returns>A status value, negative values indicate errors, zero indicates unknown service, positive indicates level of success.</returns>
    [WebMethod(Description="Modify the specified services domain credentials on this ProcessAgent. "
    + "Agent_guid is key to the service to be modified. The agentAuthorizationHeader must use "
   + "the old values. This will only be called from a ServiceBroker to members of its domain, or to a different domain's ServiceBroker."),
    SoapDocumentMethod(Binding = "IProcessAgent"),
    SoapHeader("agentAuthHeader", Direction = SoapHeaderDirection.In)]
    public abstract int ModifyDomainCredentials(string originalGuid, ProcessAgent agent, string extra, Coupon inCoupon, Coupon outCoupon);

    [WebMethod(Description = "Remove this service from a domain."),
    SoapDocumentMethod(Binding = "IProcessAgent"),
    SoapHeader("agentAuthHeader", Direction = SoapHeaderDirection.In)]
    public abstract int RemoveDomainCredentials(string domainGuid, string serviceGuid);

    /// <summary>
    /// Informs this processAgent that it should modify all references to a specific processAent. 
    /// This is used to propagate modifications, between ProcessAgents.
    /// </summary>
    /// <param name="agent">The processAgent data to be modified, the guid may not be different than the targets's guid</param>
    /// <param name="extra">an optional xml encoded set of extra named values</param>
    /// <returns>A status value, negative values indicate errors, zero indicates unknown service, positive indicates level of success.</returns>
    [WebMethod(Description = "Informs this processAgent that it should modify all references to a specific ProcessAgent. "
    + "This is used to propagate modifications, between ProcessAgents."),
    SoapDocumentMethod(Binding = "IProcessAgent"),
    SoapHeader("agentAuthHeader", Direction = SoapHeaderDirection.In)]
    public abstract int ModifyProcessAgent(string originalGuid, ProcessAgent agent, string extra);

    /// <summary>
    /// Informs a processAgent that it should retire/un-retire all references to a specific processAent. 
    /// This is used to propagate retire calls, for this release un-retire is not supported.
    /// </summary>
    /// <param name="domainGuid">The guid of the services domain ServiceBroker</param>
    /// <param name="serviceGuid">The guid of the service</param>
    /// <param name="state">The retired state to be set</param>
    /// <returns>A status value, negative values indicate errors, zero indicates unknown service, positive indicates level of success.</returns>
    [WebMethod(Description = "Informs a ProcessAgent that it should retire/un-retire all references to a specific ProcessAgent. "
    + "This is used to propagate retire calls, for this release un-retire is not supported."),
    SoapDocumentMethod(Binding = "IProcessAgent"),
    SoapHeader("agentAuthHeader", Direction = SoapHeaderDirection.In)]
    public abstract int RetireProcessAgent(string domainGuid, string serviceGuid,bool state);
    
    /// <summary>
    /// Cancel the ticket. This call is always from a serviceBroker, 
    /// if the receiver is a serviceBroker but is not the redeemer the 
    /// header is repackaged and forwarded to the local redeemer.
    /// </summary>
    /// <param name="coupon"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    [WebMethod(Description = "Cancel the ticket. This call is always from a ServiceBroker, "
    + "if the receiver is a ServiceBroker but is not the redeemer the "
    + "header is repackaged and forwarded to the redeemer if known."),
    SoapHeader("agentAuthHeader", Direction = SoapHeaderDirection.In),
    SoapDocumentMethod(Binding = "IProcessAgent")]
    public abstract bool CancelTicket(Coupon coupon, string type, string redeemer);


    [WebMethod(Description = "Register, is used to convey additional information between services. "
        + "The ServiceBroker implementation is also used in Cross-Domain registration of services and clients"),
    SoapDocumentMethod(Binding = "IProcessAgent"),
    SoapHeader("agentAuthHeader", Direction = SoapHeaderDirection.In)]
    public abstract void Register(string registerGuid, ServiceDescription[] info);
   
}

