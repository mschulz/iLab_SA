<%@ WebService Language="c#" Class="I_ServiceBroker" %>

using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services.Protocols;
using System.Web.Services;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.Xml;

using iLabs.DataTypes;
using iLabs.DataTypes.ProcessAgentTypes;
using iLabs.DataTypes.TicketingTypes;
using iLabs.DataTypes.StorageTypes;
using iLabs.DataTypes.SoapHeaderTypes;
using iLabs.ServiceBroker.Services;




	/// <summary>
	/// Summary description Interactive ServiceBroker Interface.
	/// </summary>
	///
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[SoapDocumentService(RoutingStyle = SoapServiceRoutingStyle.RequestElement)]
[WebServiceBinding(Name = "IServiceBroker", Namespace = "http://ilab.mit.edu/iLabs/Services")]
[XmlType(Namespace = "http://ilab.mit.edu/iLabs/type")]
[WebService(Name = "InteractiveSBProxy", Namespace = "http://ilab.mit.edu/iLabs/Services",
    Description = "Interactive ServiceBroker Interface.")]

public abstract class I_ServiceBroker : System.Web.Services.WebService
{

    public AgentAuthHeader agentAuthHeader = new AgentAuthHeader();
    public OperationAuthHeader opHeader = new OperationAuthHeader();


    /// <summary>
    /// Sets a client item value in the user's opaque data store.
    /// </summary>
    /// <param name="name">The name of the client item whose value is to be saved.</param>
    /// <param name="itemValue">The value that is to be saved with name.</param>
    /// <remarks>Web Method</remarks>
    [WebMethod(Description = "Sets a client item value in the user's opaque data store", EnableSession = true),
    SoapHeader("opHeader", Direction = SoapHeaderDirection.In),
   SoapDocumentMethod(Binding = "IServiceBroker")]
    public abstract void SaveClientData(string name, string itemValue);

    /// <summary>
    /// Returns the value of a client item in the user's opaque data store.  
    /// </summary>
    /// <param name="name">The name of the client item whose value is to be returned.</param>
    /// <returns>The value of a client item in the user's opaque data store.</returns>
    /// <remarks>Web Method</remarks>
    [WebMethod(Description = "returns the value of an client item in the user's opaque data store", EnableSession = true),
  SoapHeader("opHeader", Direction = SoapHeaderDirection.In),
   SoapDocumentMethod(Binding = "IServiceBroker")]
    public abstract string LoadClientData(string name);

    /// <summary>
    /// Removes a client item from the user's opaque data store.
    /// </summary>
    /// <param name="name">The name of the client item to be removed.</param>
    /// <remarks>Web Method</remarks>
    [WebMethod(Description = "removes an client item from the user's opaque data store", EnableSession = true),
  SoapHeader("opHeader", Direction = SoapHeaderDirection.In),
   SoapDocumentMethod(Binding = "IServiceBroker")]
    public abstract void DeleteClientData(string name);

    /// <summary>
    /// Enumerates the names of all client items in the user's opaque data store.
    /// </summary>
    /// <returns>An array of client items.</returns>
    /// <remarks>Web Method</remarks>
    [WebMethod(Description = "Enumerates the names of all client items in the user's opaque data store. "
    + "This is performed for the user's current client as stored in the session state.", EnableSession = true),
   SoapHeader("opHeader", Direction = SoapHeaderDirection.In),
   SoapDocumentMethod(Binding = "IServiceBroker")]
    public abstract string[] ListClientDataItems();


    ///////////////////////
    //// Authority Methods
    ///////////////////////

    /// <summary>
    /// An authority requests the 'completeness' status of a user. This will most likely only be supported for requests from a SCORM.
    /// </summary>
    /// <param name="userName">>A string token reperesenting the user, this may be a user name, or an anonymous unique 
    /// id that the authority will always use to identify this user</param>
    /// <param name="authorityKey">May be a URL or GUID, not sure what it will actually be</param>
    /// <returns>An IntTag, depending on the id the tag maybe used differently.</returns>
    [WebMethod(Description = "An authority requests the 'completeness' status of a user. This will most likely only be supported for requests from a SCORM.", EnableSession = true)]
    [SoapHeader("opHeader", Direction = SoapHeaderDirection.In, Required = false)]
    [SoapDocumentMethod(Binding = "IServiceBroker")]
    public abstract IntTag GetUserStatus(string userName, string authorityKey);

    /// <summary>
    /// An authority updates a user. This will most likely only be supported for requests from a SCORM.
    /// </summary>
    /// <param name="userName">>A string token reperesenting the user, this may be a user name, or an anonymous unique 
    /// id that the authority will always use to identify this user</param>
    /// <param name="authorityKey">May be a URL or GUID, not sure what it will actually be</param>
    /// <param name="firstName"></param>
    /// <param name="lastName"></param>
    /// <param name="email"></param>
    /// <param name="affiliation"></param>
    /// <param name="autoCreate">create the user if it does not exist & is true</param>
    /// <returns></returns>
    [WebMethod(Description = "An authority updates a user. This will most likely only be supported for requests from a SCORM.", EnableSession = true)]
    [SoapHeader("opHeader", Direction = SoapHeaderDirection.In, Required = false)]
    [SoapDocumentMethod(Binding = "IServiceBroker")]
    public abstract IntTag ModifyUser(string userName, string authorityKey, string firstName, string lastName,
        string email, string affiliation, bool autoCreate);

    /// <summary>
    /// An authority requests the launching of a specific client for a user.
    /// </summary>
    /// <param name="clientGuid">The GUID of the client, this client must be registered on the serviceBroker.</param>  
    /// <param name="groupName">For now this should be a group that exisits on the serviceBroker, it may be null</param>
    /// <param name="userName">A string token reperesenting the user, this may be a user name, or an anonymous unique 
    /// id that the authority will always use to identify this user</param>
    /// <param name="authorityKey">May be a URL or GUID, not sure what it will actually be</param>
    /// <param name="duration"></param>
    /// <param name="autoStart">If the client is resolved for the user and may be executed now, the myClient page is not displayed. Default is true(1).</param>
    /// <returns>an IntTag with a status code in the interger and a URLto be used by the authority to redirect the request.</returns>
    [WebMethod(Description = "An authority requests the launching of a specific client for a user.", EnableSession = true)]
    [SoapHeader("opHeader", Direction = SoapHeaderDirection.In, Required = true)]
    [SoapDocumentMethod(Binding = "IServiceBroker")]
    public abstract IntTag LaunchLabClient(string clientGuid, string groupName,
           string userName, string authorityKey, DateTime start, long duration, int autoStart);


    /// <summary>
    /// Request authorization for the specified types of access, for the specified group and optional user.
    /// While several fields may be null, enough imformation must be supplied by a combination of the headers 
    /// and the fields to authenticate & authorize the requested actions.
    /// This method supports both an AgentAuthHeader and an OperationHeader in the SOAP header, at least one must be used.
    /// Currently support for Scheduling tickets only.
    /// </summary>
    /// <param name="types">an array of the ticket types requested</param>
    /// <param name="duration">minimum duration of the created tickets in seconds, durations less than 2 minutes will be converted to two minutes.</param>
    /// <param name="userName">User name on the ticketIssuer service, may in the future support validation from the service making the request, may be null</param>
    /// <param name="groupName">group name on the ticketIssuer service, may be null.  May in the future support validation from the service making the request</param>
    /// <param name="serviceGuid">TThe lab server or other service that authorization is being requested for. May be null</param>
    /// <param name="clientGuid">May be null</param>
    /// <returns>An operationCoupon or null</returns>
    [WebMethod(Description = "Request authorization for the specified types of access, for the specified group, user, service and  client. Currently support for Scheduling tickets only.")]
    [SoapDocumentMethod(Binding = "IServiceBroker")]
    [SoapHeader("agentAuthHeader", Direction = SoapHeaderDirection.In)]
    [SoapHeader("opHeader", Direction = SoapHeaderDirection.In)]
    public abstract Coupon RequestAuthorization(string[] types, long duration, string userName, string groupName, string serviceGuid, string clientGuid);

    ///////////////////////////////////
    ////   EXPERIMENT METHODS      ////
    ///////////////////////////////////

    [WebMethod(Description = "Creates an Experiment on the ServiceBroker, if an ESS is "
+ "associated with this experiment the ESS experiment is opened so that ExperimentRecords "
+ "or BLOBs can be written to the ESS.",
EnableSession = true)]
    [SoapHeader("opHeader", Direction = SoapHeaderDirection.In)]
    [SoapDocumentMethod(Binding = "IServiceBroker")]
    public abstract StorageStatus CreateExperiment(DateTime startTime, long duration, 
        string labServerGuid, string clientGuid, string groupName, string userName);


    [WebMethod(Description = "Opens an existing Experiment, if an ESS is "
+ "associated with this experiment the ESS experiment is opened so that ExperimentRecords "
+ "or BLOBs can be written to the ESS.",
EnableSession = true)]
    [SoapHeader("opHeader", Direction = SoapHeaderDirection.In)]
    [SoapDocumentMethod(Binding = "IServiceBroker")]
    public abstract StorageStatus OpenExperiment(long experimentId, long duration);

    /// <summary>
    /// Closes an Experiment on the ServiceBroker, if an ESS is
    /// associated with this experiment the ESS experiment is closed so that no further ExperimentRecords
    /// or BLOBs can be written to the ESS.
    /// </summary>
    /// <param name="coupon">coupon issued as part of ExperimentExecution collection, used to route to the issueing SB</param>
    /// <param name="experimentId"></param>
    /// <returns></returns>
    [WebMethod(Description = "Agent to ServiceBroker call, coupon is from the original ExecuteExperiment collection and is used to route to the issueing SB. "
   + "Closes an Experiment on the ServiceBroker, if an ESS is "
   + "associated with this experiment the ESS experiment is closed so that no further ExperimentRecords "
   + "or BLOBs can be written to the ESS.",
EnableSession = true)]
    [SoapHeader("agentAuthHeader", Direction = SoapHeaderDirection.In)]
    [SoapDocumentMethod(Binding = "IServiceBroker")]
    public abstract StorageStatus AgentCloseExperiment(Coupon coupon, long experimentId);

    [WebMethod(Description = "Client call that requests that the ServiceBroker Close an Experiment , if an ESS is "
        + "associated with this experiment the ESS experiment is closed so that no further ExperimentRecords "
        + "or BLOBs can be written to the ESS.",
        EnableSession = true)]
    [SoapHeader("opHeader", Direction = SoapHeaderDirection.In)]
    [SoapDocumentMethod(Binding = "IServiceBroker")]
    public abstract StorageStatus ClientCloseExperiment(long experimentId);


    [WebMethod(Description = "Qualifies operation access and forwards request to the ESS",
    EnableSession = true)]
    [SoapHeader("opHeader", Direction = SoapHeaderDirection.In)]
    [SoapDocumentMethod(Binding = "IServiceBroker")]
    public abstract Experiment RetrieveExperiment(long experimentID);

    [WebMethod(Description = "Uses the users qualifiers to select Experiment summaries, no write permissions are created."
   + " Valid field names include all Experiment Table field names and userName, groupName, labServerName, clientGuid, clientName, scheduledStart,creationTime, status "
   + " and experimentID.",
   EnableSession = true)]
    [SoapHeader("opHeader", Direction = SoapHeaderDirection.In)]
    [SoapDocumentMethod(Binding = "IServiceBroker")]
    public abstract ExperimentSummary[] RetrieveExperimentSummary(Criterion[] carray);

    [WebMethod(Description = "Uses the users qualifiers to select Experiment records, no write permissions are created."
   + " Valid field names include; userName, groupName, labServerName,clientName, scheduledStart,creationTime, status "
   + " and experimentID.",
   EnableSession = true)]
    [SoapHeader("opHeader", Direction = SoapHeaderDirection.In)]
    [SoapDocumentMethod(Binding = "IServiceBroker")]
    public abstract ExperimentRecord[] RetrieveExperimentRecords(long experimentID, Criterion[] carray);

    [WebMethod(Description = "Uses the credentials granted the experiment specified by the opHeader to check "
        + "access to the requested experiment, if allowed a new ticket collection is started "
    + "to access the requested experiment and optional ESS records. Returns null if access denied.",
    EnableSession = true)]
    [SoapHeader("opHeader", Direction = SoapHeaderDirection.In)]
    [SoapDocumentMethod(Binding = "IServiceBroker")]
    public abstract Coupon RequestExperimentAccess(long experimentId);

    /// <summary>
    /// Saves or modifies an optional user defined annotation to the experiment record.
    /// </summary>
    /// <param name="experimentId">A token which identifies the experiment.</param>
    /// <param name="annotation">The annotation to be saved with the experiment.</param>
    /// <returns>The previous annotation or null if there wasn't one.</returns>
    /// <remarks>Web Method</remarks>
    [WebMethod(Description = "Saves or modifies an optional user defined annotation to the experiment record.", EnableSession = true)]
    [SoapHeader("opHeader", Direction = SoapHeaderDirection.In)]
    [SoapDocumentMethod(Binding = "IServiceBroker")]
    public abstract string SetAnnotation(int experimentId, string annotation);

    /// <summary>
    /// Retrieves a previously saved experiment annotation.
    /// </summary>
    /// <param name="experimentId">A token which identifies the experiment.</param>
    /// <returns>The annotation, a string originally created by the user via the Lab Client.</returns>
    /// <remarks>Web Method</remarks>
    [WebMethod(Description = "Retrieves a previously saved experiment annotation.", EnableSession = true)]
    [SoapHeader("opHeader", Direction = SoapHeaderDirection.In)]
    [SoapDocumentMethod(Binding = "IServiceBroker")]
    public abstract string GetAnnotation(int experimentId);

    /// <summary>
    /// Revokes reservations that intersect the specifications, may be called from the LSS or ServiceBroker
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="groupName"></param>
    /// <param name="labServerGuid"></param>
    /// <param name="clientCuid"></param>
    /// <param name="startTime"></param>
    /// <param name="endTime"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    [WebMethod(Description = "Revokes reservations that intersect the specifications, may be called from the LSS or USS, but normally is called from the USS.", EnableSession = true)]
    [SoapHeader("agentAuthHeader", Direction = SoapHeaderDirection.In)]
    [SoapDocumentMethod(Binding = "IServiceBroker")]
    public abstract bool RevokeReservation(string serviceBrokerGuid, string userName, string groupName, string labServerGuid, string labClientGuid,
        DateTime startTime, DateTime endTime, string message);
}


 

