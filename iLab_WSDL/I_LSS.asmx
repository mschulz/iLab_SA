<%@ WebService Language="c#" Class="I_LSS" %>

/*
 * Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
 * Please see license.txt in top level directory for full license.
 */

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
using iLabs.DataTypes.SchedulingTypes;



	/// <summary>
	/// The interface specification for Lab Scheduling Servers.
	/// </summary>
    [XmlType(Namespace = "http://ilab.mit.edu/iLabs/Type")]
    [WebServiceBinding(Name = "ILSS", Namespace = "http://ilab.mit.edu/iLabs/Services"),
    WebService(Name = "LabSchedulingProxy", Namespace = "http://ilab.mit.edu/iLabs/Services",
        Description="The interface specification for Lab Scheduling Servers (LSS).")]
    public abstract class I_LSS : System.Web.Services.WebService
	{
        public OperationAuthHeader opHeader = new OperationAuthHeader();
        public AgentAuthHeader agentAuthHeader = new AgentAuthHeader();
       
		/// <summary>
        /// Retrieve an array of available time periods(UTC) for a particular group 
        /// and particular experiment delimited by a time period. 
        /// TimePeriods returned ordered by start time and may contain differing quantums and length.
		/// </summary>
        /// <param name="serviceBrokerGuid">the Groups ServiceBroker</param>
		/// <param name="groupName">the group requesting the available time</param>
        /// <param name="ussGuid">the USS</param>
		/// <param name="clientGuid">the client GUID</param>
		/// <param name="labServerGuid">the LabServer GUID</param>
        /// <param name="startTime">UTC start time</param>
        /// <param name="endTime">UTC end time</param>
        /// <returns>An array of time periods (UTC), ordered by time, each of the 
        /// time periods is longer than the experiment's minimum time and may contain 
        /// differing quantums and length.</returns> 
        [WebMethod(Description=" Retrieve an array of available time periods(UTC) for a particular group"
        + " and particular experiment delimited by a time period. " 
        + "Time periods should be ordered by start time and may contain differing quantums and length.")]
        [SoapDocumentMethod(Binding = "ILSS"),
        SoapHeader("opHeader", Direction = SoapHeaderDirection.In)]
        public abstract TimePeriod[] RetrieveAvailableTimePeriods(string serviceBrokerGuid,
            string groupName, string ussGuid,
            string labServerGuid, string clientGuid, DateTime startTime, DateTime endTime);
 
		/// <summary>
		/// Returns an Boolean indicating whether a particular reservation from
        /// a USS is confirmed and added to the database in LSS successfully.
        /// If it fails, exception will be throw out indicating
		///	the reason for rejection.
		/// </summary>
        /// <param name="serviceBrokerGuid"></param>
		/// <param name="groupName"></param>
        /// <param name="ussGuid"></param>
        /// <param name="labServerGuid"></param>
        /// <param name="clientGuid"></param>param>
        /// <param name="startTime">UTC start time</
        /// <param name="endTime">UTC end time</param>
        /// <returns>the notification whether the reservation is confirmed. If not, 
        /// notification will give a reason</returns>
        [WebMethod(Description="Returns a string indicating whether a particular reservation "
        + "from a USS is confirmed and added to the database in LSS successfully."
        + "If it fails, a exception will be throw indicating the reason for rejection.")]
        [SoapDocumentMethod(Binding = "ILSS"),
        SoapHeader("opHeader", Direction = SoapHeaderDirection.In)]
        public abstract string ConfirmReservation(string serviceBrokerGuid, string groupName,
            string ussGuid, string labServerGuid, string clientGuid, 
            DateTime startTime, DateTime endTime);

        /// <summary>
        /// Redeem the reservation, this just changes the status of the reservation
        /// </summary>
        /// <param name="serviceBrokerGuid"></param>
        /// <param name="groupName"></param>
        /// <param name="ussGuid"></param>
        /// <param name="labServerGuid"></param>
        /// <param name="clientGuid"></param>
        /// <param name="startTime">UTC start time</param>
        /// <param name="endTime">UTC end time</param>
        /// <returns>1 if updated successfully, 0 otherwise</returns>
        [WebMethod(Description="Used to redeem the reservation status on LSS,")]
        [SoapDocumentMethod(Binding = "ILSS"),
        SoapHeader("opHeader", Direction = SoapHeaderDirection.In)]
        public abstract int RedeemReservation(string serviceBrokerGuid, string groupName,
            string ussGuid, string labServerGuid, string clientGuid,
            DateTime startTime, DateTime endTime);
        
		/// <summary>
        /// remove the ReservationInfos that intersect the specification, 
        /// wraps the API method which sends a single email to the experiment contact email address.
        /// Note: should only be called from the USS, this does not forward the RemoveReservation method call to the USS or ServiceBroker.
		/// </summary>
        /// <param name="serviceBrokerGuid">Must be specified if groupName specified</param>
		/// <param name="groupName"></param>
        /// <param name="ussGuid">Must be specified</param>
        /// <param name="labServerGuid">Must be specified</param>
        /// <param name="clientGuid"></param>
        /// <param name="startTime">UTC start time, trucated within the method to UTCnow</param>
        /// <param name="endTime">UTC end time</param>
        /// <returns>The number of reservationInfos deleted, or -1 on error</returns>
        [WebMethod(Description="Removes all reservations which intersect the parameters, should only be called from the USS, the LSS does not notify the USS.")]
        [SoapDocumentMethod(Binding = "ILSS"),
        SoapHeader("opHeader", Direction = SoapHeaderDirection.In)]
        public abstract int RemoveReservation(string serviceBrokerGuid, string groupName,
            string ussGuid, string labServerGuid, string clientGuid, 
            DateTime startTime, DateTime endTime);
   
		/// <summary>
		/// Add information of a particular user side scheduling server identified by ussID 
		/// </summary>
        /// <param name="ussGuid"></param>
		/// <param name="ussName"></param>
		/// <param name="ussUrl"></param>
        /// <param name="coupon">The coupon for a persistant Revoke Reservation Ticket</param>
        /// <returns>1 if the USSInfo is added successfully, 0 otherwise</returns>
        [WebMethod(Description="Add information of a particular user side scheduling server. "
        + "This is called from the ServiceBroker, it may be called many times, duplicate entries are not added to the database")]
        [SoapDocumentMethod(Binding = "ILSS"),
        SoapHeader("agentAuthHeader", Direction = SoapHeaderDirection.In)]
        public abstract int AddUSSInfo(string ussGuid, string ussName, string ussUrl, Coupon coupon);

        /// <summary>
        /// Modify information about a registered USS. 
        /// </summary>
        /// <param name="ussGuid"></param>
        /// <param name="ussName"></param>
        /// <param name="ussUrl"></param>
        /// <param name="coupon">The coupon for a persistant Revoke Reservation Ticket</param>
        /// <returns>1 if the USSInfo is modified successfully, 0 otherwise</returns>
        [WebMethod(Description="Modify information about a registered USS.")]
        [SoapDocumentMethod(Binding = "ILSS"),
        SoapHeader("agentAuthHeader", Direction = SoapHeaderDirection.In)]
        public abstract int ModifyUSSInfo(string ussGuid, string ussName, string ussUrl, Coupon coupon);
        
        /// <summary>
        /// Remove information of a particular user side scheduling server identified by ussGuid. 
        /// Currently this does not remove the USS information, as all reservations 
        /// that are related to the USS would also be deleted.
        /// </summary>
        /// <param name="ussGuid"></param>
        /// <param name="ussName"></param>
        /// <param name="ussUrl"></param>
        /// <returns>1 if the USSInfo is removed successfully, 0 otherwise</returns>
        [WebMethod(Description="Remove information of a particular user side scheduling server identified by ussGuid."
           + " Currently this does not remove the USS information, as all reservations"
           + " that are related to the USS would also be deleted.")]
        [SoapDocumentMethod(Binding = "ILSS"),
        SoapHeader("agentAuthHeader", Direction = SoapHeaderDirection.In)]
        public abstract int RemoveUSSInfo(string ussGuid);
        
		/// <summary>
		/// Add a credential set for a particular group, this may be called multiple times 
        /// for the same group, only one instance will be created.
        /// CredentialSets represent a group of users from a specific 
        /// ServiceBroker, and the USS that schedules them.
		/// </summary>
        /// <param name="serviceBrokerGuid"></param>
        /// <param name="serviceBrokerName"></param>
		/// <param name="groupName"></param>
        /// <param name="ussGuid"></param>
        /// <returns>1 if the CredentialSet is added successfully, 0 otherwise</returns>
        [WebMethod(Description="Add a credential set for a particular group, this may be called multiple times for the same group, only one instance will be created. CredentialSets represent a group of users from a specific ServiceBroker, and the USS that schedules them.")]
        [SoapDocumentMethod(Binding = "ILSS"),
       SoapHeader("agentAuthHeader", Direction = SoapHeaderDirection.In)]
        public abstract int AddCredentialSet(string serviceBrokerGuid, string serviceBrokerName, 
            string groupName, string ussGuid);


        /// <summary>
        /// Modify the ServiceBroker's server name for a credential set. This is triggered 
        /// by a ModifyDomainCredentals that changes parameters for an existing service.
        /// </summary>
        /// <param name="serviceBrokerGuid"></param>
        /// <param name="serviceBrokerName"></param>
        /// <param name="groupName"></param>
        /// <param name="ussGuid"></param>
        /// <returns>1 if the CredentialSet is added successfully, 0 otherwise</returns>
        [WebMethod(Description="Modify the ServiceBroker's server name for a credential set.  "
        + "This is triggered by a ModifyDomainCredentals that changes parameters for an existing service.")]
        [SoapDocumentMethod(Binding = "ILSS"),
       SoapHeader("agentAuthHeader", Direction = SoapHeaderDirection.In)]
        public abstract int ModifyCredentialSet(string serviceBrokerGuid, string serviceBrokerName,
            string groupName, string ussGuid);

        /// <summary>
        /// Remove the credential set of a particular group.
        /// </summary>
        /// <param name="serviceBrokerGuid"></param>
        /// <param name="groupName"></param>
        /// <param name="ussGuid"></param>
        /// <returns></returns>1 if the CredentialSet is removed successfully, 0 otherwise
        [WebMethod(Description="Remove a credential set of a particular group.")]
        [SoapDocumentMethod(Binding = "ILSS"),
       SoapHeader("agentAuthHeader", Direction = SoapHeaderDirection.In)]
        public abstract int RemoveCredentialSet(string serviceBrokerGuid,
            string groupName, string ussGuid);

        /// <summary>
        /// Add information of a particular experiment
        /// </summary>
        /// <param name="labServerGuid"></param>
        /// <param name="labServerName"></param>
        /// <param name="clientGuid"></param>
        /// <param name="clientVersion"></param>
        /// <param name="clientName"></param>
        /// <param name="providerName"></param>
        /// <returns>1 if the experimentInfo is added successfully, 0 otherwise</returns>
        [WebMethod(Description="Add information for a particular experiment.")]
        [SoapDocumentMethod(Binding = "ILSS"),
        SoapHeader("agentAuthHeader", Direction = SoapHeaderDirection.In)]
        public abstract int AddExperimentInfo(string labServerGuid, string labServerName,
            string clientGuid, string clientName, string clientVersion,
            string providerName);

        /// <summary>
        /// Modify information of a particular experiment
        /// </summary>
        /// <param name="labServerGuid"></param>
        /// <param name="labServerName"></param>
        /// <param name="clientGuid"></param>
        /// <param name="clientName"></param>
        /// <param name="clientVersion"></param>
        /// <param name="providerName"></param>
        /// <returns>1 if the experimentInfo is added successfully, 0 otherwise</returns>
        [WebMethod(Description="Modify information of a particular experiment.")]
        [SoapDocumentMethod(Binding = "ILSS"),
        SoapHeader("agentAuthHeader", Direction = SoapHeaderDirection.In)]
        public abstract int ModifyExperimentInfo(string labServerGuid, string labServerName,
            string clientGuid, string clientName, string clientVersion,
            string providerName);

        /// <summary>
        /// Remove a particular experiment
        /// </summary>
        /// <param name="labServerGuid"></param>
        /// <param name="clientGuid"></param>
        /// <returns>1 if the experimentInfo is removed successfully, 0 otherwise</returns>
        [WebMethod(Description="Remove a particular experiment.")]
        [SoapDocumentMethod(Binding = "ILSS"),
        SoapHeader("agentAuthHeader", Direction = SoapHeaderDirection.In)]
        public abstract int RemoveExperimentInfo(string labServerGuid, string clientGuid);


	}
