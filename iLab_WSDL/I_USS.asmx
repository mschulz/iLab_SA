<%@ WebService Language="c#" Class="I_USS" %>

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

using iLabs.DataTypes;
using iLabs.DataTypes.SoapHeaderTypes;
using iLabs.DataTypes.TicketingTypes;
using iLabs.DataTypes.SchedulingTypes;

	/// <summary>
	/// Summary description for User Scheduling Service (USS).
	/// </summary>
    [WebServiceBinding(Name = "IUSS", Namespace = "http://ilab.mit.edu/iLabs/Services"),
    WebService(Name = "UserSchedulingProxy", Namespace = "http://ilab.mit.edu/iLabs/Services",
       Description = "Interface description for User Scheduling Service (USS).")]
    public abstract class I_USS : System.Web.Services.WebService
	{

        public OperationAuthHeader opHeader = new OperationAuthHeader();
        public AgentAuthHeader agentAuthHeader = new AgentAuthHeader();

        
        /// <summary>
        /// Retrieve available time periods(UTC) This is a pass-through method that gets the information from the LSS.
        /// </summary>
        /// <param name="serviceBrokerGuid"></param>
        /// <param name="groupName"></param>
        /// <param name="clientGuid"></param>
        /// <param name="labServerGuid"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>return an array of time periods (UTC), each of the time periods is longer than the experiment's minimum time 
        [WebMethod]
        [SoapDocumentMethod(Binding = "IUSS"),
        SoapHeader("opHeader", Direction = SoapHeaderDirection.In)]
        public abstract TimePeriod[] RetrieveAvailableTimePeriods(string serviceBrokerGuid, string groupName,
            string labServerGuid, string clientGuid, DateTime startTime, DateTime endTime);
       
        
        
        /// <summary>
        /// Returns all resevations that match the search criteria.
        /// </summary>
        /// <param name="serviceBrokerGuid">The User's SB</param>
        /// <param name="userName">User Name</param>
        /// <param name="labServerGuid">LabServer GUID</param>
        /// <param name="labClientGuid">Client GUID</param>
        /// <param name="startTime">startTime(UTC)</param>
        /// <param name="endTime">endTime(UTC)</param>
        /// <returns></returns>
        [WebMethod(Description = "Returns a list of all reservations that match the search parameters.")]
        [SoapDocumentMethod(Binding = "IUSS"),
        SoapHeader("opHeader", Direction = SoapHeaderDirection.In)]
        public abstract Reservation[] ListReservations(string serviceBrokerGuid, string userName,
            string labServerGuid, string labClientGuid, DateTime startTime, DateTime endTime);
       
        /// <summary>
        /// Add a reservation for a lab server for the specified user, group, client and time. 
        /// If the reservation is confirmed by the LSS, the reservation will be added to the USS.
        /// </summary>
        /// <param name="serviceBrokerGuid"></param>
        /// <param name="userName"></param>
        /// <param name="groupName"></param>
        /// <param name="clientGuid"></param>
        /// <param name="labServerGuid"></param>
        /// <param name="labClientGuid"></param>
        /// <param name="startTime">UTC</param>
        /// <param name="endTime">UTC</param>
        /// <returns>Returns a message of succes or a simple description of the reason for failure.</returns>
        [WebMethod(Description = "Add a reservation for a lab server for the specified user, group, client and time. "
        + "If the reservation is confirmed by the LSS, the reservation will be added to the USS.")]
        [SoapDocumentMethod(Binding = "IUSS"),
        SoapHeader("opHeader", Direction = SoapHeaderDirection.In)]
        public abstract string AddReservation(string serviceBrokerGuid, string userName, string groupName,
            string labServerGuid, string labClientGuid, DateTime startTime, DateTime endTime);
		
		/// <summary>
		/// Remove all the reservations for a lab server  that match the specification.. 
        /// The serviceBroker is notified of each reservation so that an email is sent 
        /// to each of the owners of the removed reservations.
        /// If the method is not called by an LSS the LSS is forwarded a RemoveReservation request."
		/// </summary>
        /// <param name="serviceBrokerGuid"></param>
        /// <param name="groupName"></param>
		/// <param name="clientGuid"></param>
        /// <param name="labServerGuid"></param>
        /// <param name="labClientGuid"></param>
		/// <param name="startTime">UTC</param>
		/// <param name="endTime">UTC</param>
        /// <param name="message"></param>
        /// <returns>The number of reservations removed or -1 in an error</returns>
        [WebMethod(Description = "Remove all the reservations for a lab server that match the specification."
            + " The serviceBroker is notified of each reservation so that an email is sent "
            + "to each of the owners of the removed reservations."
            + " If the method is not called by an LSS the LSS is forwarded a RemoveReservation request.")]
        [SoapDocumentMethod(Binding = "IUSS"),
        SoapHeader("opHeader", Direction = SoapHeaderDirection.In)]
        public abstract int RevokeReservation(string serviceBrokerGuid, string groupName,
            string labServerGuid, string labClientGuid, DateTime startTime, DateTime endTime,
            string message);
		
		/// <summary>
		/// Returns an existing reservation for the current time for 
        /// a particular user to execute a particular experiment, if it exists. 
        /// This does not create the AllowExperiment ticket.
		/// </summary>
		/// <param name="userName"></param>
		/// <param name="serviceBrokerGuid"></param>
        /// <param name="clientGuid"></param>
        /// <param name="labServerGuid"></param>
        /// <returns>the existing reservation if it is the right time for a particular
        /// user to execute a particular experiment, or null.</returns>
        [WebMethod(Description = "Returns an existing reservation for the current time for "
        + "a particular user to execute a particular experiment, if it exists. "
        + "This does not create the AllowExperiment ticket.")]
        [SoapDocumentMethod(Binding = "IUSS"),
        SoapHeader("opHeader", Direction = SoapHeaderDirection.In)]
        public abstract Reservation RedeemReservation(string serviceBrokerGuid, string userName, 
            string labServerGuid, string clientGuid);
		
		
		/// <summary>
		///  Add a credential set
		/// </summary>
        /// <param name="serviceBrokerGuid"></param>
		/// <param name="groupName"></param>
        /// <returns>1 if the CredentialSet is added successfully, 0 otherwise</returns>1 if the credential set has been added 
        /// successfully, 0 otherwise
        [WebMethod(Description = "Add a credential set. A credentialSet defines a specific group from a specific ServiceBroker.")]
        [SoapDocumentMethod(Binding = "IUSS"),
        SoapHeader("agentAuthHeader", Direction = SoapHeaderDirection.In)]
		public abstract int AddCredentialSet(string serviceBrokerGuid,
            string serviceBrokerName,string groupName);

        /// <summary>
        ///  Modify a credential set.
        /// </summary>
        /// <param name="serviceBrokerGuid"></param>
        /// <param name="groupName"></param>
        /// <returns>1 if the CredentialSet is modified successfully, 0 otherwise</returns>
        [WebMethod(Description = "Modify a credential set. This may be called if a ServiceBroker or group name is changed on the ServiceBroker")]
        [SoapDocumentMethod(Binding = "IUSS"),
        SoapHeader("agentAuthHeader", Direction = SoapHeaderDirection.In)]
        public abstract int ModifyCredentialSet(string serviceBrokerGuid,
            string serviceBrokerName, string groupName);

        /// <summary>
		///  Remove a credential set
		/// </summary>
        /// <param name="serviceBrokerGuid"></param>
		/// <param name="groupName"></param>
        /// <returns>1 if the CredentialSet is removed successfully, 0 otherwise</returns>
        [WebMethod(Description = "Remove a credential set.")]
        [SoapDocumentMethod(Binding = "IUSS"),
        SoapHeader("agentAuthHeader", Direction = SoapHeaderDirection.In)]
        public abstract int RemoveCredentialSet(string serviceBrokerGuid, string groupName);
        
		/// <summary>
		/// Add information of a particular experiment
		/// </summary>
        /// <param name="labServerGuid"></param>
		/// <param name="labServerName"></param>
        /// <param name="labClientGuid"></param>
        /// <param name="labClientName"></param>
		/// <param name="labClientVersion"></param>		
		/// <param name="providerName"></param>
        /// <param name="lssGuid"></param>
        /// <returns>1 if the experimentInfo is added successfully, 0 otherwise</returns>
        [WebMethod(Description = "Add iformation about an experiment that will have its users managed by the service.")]
        [SoapDocumentMethod(Binding = "IUSS"),
        SoapHeader("agentAuthHeader", Direction = SoapHeaderDirection.In)]
		public abstract int AddExperimentInfo(string labServerGuid, string labServerName,
            string labClientGuid, string labClientName, string labClientVersion,
            string providerName, string lssGuid);


        /// <summary>
        /// Modify information of a particular experiment
        /// </summary>
        /// <param name="labServerGuid"></param>
        /// <param name="labServerName"></param>
        /// <param name="labClientGuid"></param>
        /// <param name="labClientName"></param>
        /// <param name="labClientVersion"></param>		
        /// <param name="providerName"></param>
        /// <param name="lssGuid"></param>
        /// <returns>1 if the experimentInfo is modified successfully, 0 otherwise</returns>
        [WebMethod(Description = "Modify information of a particular experiment")]
        [SoapDocumentMethod(Binding = "IUSS"),
        SoapHeader("agentAuthHeader", Direction = SoapHeaderDirection.In)]
        public abstract int ModifyExperimentInfo(string labServerGuid, string labServerName,
            string labClientGuid, string labClientName, string labClientVersion,
            string providerName, string lssGuid);
        
        /// <summary>
        /// Remove a particular experiment
        /// </summary>
        /// <param name="labServerGuid"></param>      
        /// <param name="labClientGuid"></param>
        /// <param name="lssGuid"></param>
        /// <returns>1 if the experimentInfo is removed successfully, 0 otherwise</returns>
        [WebMethod(Description = "Remove a particular experiment")]
        [SoapDocumentMethod(Binding = "IUSS"),
        SoapHeader("agentAuthHeader", Direction = SoapHeaderDirection.In)]
        public abstract int RemoveExperimentInfo(string labServerGuid, string labClientGuid,string lssGuid);
		
		/// <summary>
		/// Add information of a particular lab side scheduling server identified by lssID.
		/// </summary>
        /// <param name="lssGuid"></param>
        /// <param name="lssName"></param>
		/// <param name="lssUrl"></param>
        /// <returns>1 if the LSSInfo is added successfully, 0 otherwise</returns>
        [WebMethod(Description = "Add information of a particular lab side scheduling server identified by lssID.")]
        [SoapDocumentMethod(Binding = "IUSS"),
        SoapHeader("agentAuthHeader", Direction = SoapHeaderDirection.In)]
        public abstract int AddLSSInfo(string lssGuid, string lssName, string lssUrl, Coupon coupon);
        
        /// <summary>
        /// Modify information of a particular lab side scheduling server identified by lssID
        /// </summary>
        /// <param name="lssGuid"></param>
        /// <param name="lssName"></param>
        /// <param name="lssUrl"></param>
        /// <returns>1 if the LSSInfo is modified successfully, 0 otherwise</returns>
        [WebMethod(Description = "Modify information of a particular lab side scheduling server identified by lssID")]
        [SoapDocumentMethod(Binding = "IUSS"),
        SoapHeader("agentAuthHeader", Direction = SoapHeaderDirection.In)]
        public abstract int ModifyLSSInfo(string lssGuid, string lssName, string lssUrl, Coupon coupon);
        
        /// <summary>
        /// rEMOVE information of a particular lab side scheduling server identified by lssID
        /// </summary>
        /// <param name="lssGuid"></param>
        /// <param name="lssName"></param>
        /// <param name="lssUrl"></param>
        /// <returns>1 if the LSSInfo is removed successfully, 0 otherwise</returns>
        [WebMethod(Description = "Remove information of a particular lab side scheduling server identified by lssID")]
        [SoapDocumentMethod(Binding = "IUSS"),
        SoapHeader("agentAuthHeader", Direction = SoapHeaderDirection.In)]
        public abstract int RemoveLSSInfo(string lssGuid);


	}


