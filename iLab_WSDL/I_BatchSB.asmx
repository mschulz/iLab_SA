<%@ WebService Language="c#" Class="iLabs.Services.I_BatchSB" %>

/*
 * Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
 * Please see license.txt in top level directory for full license.
 */

/* $Id$ */

using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Threading ;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Web.SessionState;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Serialization;
using System.Net;
using System.Reflection;
using System.Configuration;

//using iLabs.Architecture.DataStorageAPI ;
//using iLabs.Architecture.AdministrativeAPI;
//using iLabs.Architecture.AuthorizationAPI;
//using iLabs.Architecture.LabServerAPI;
//using iLabs.Architecture.InternalAPI;
//using iLabs.Architecture.TicketingAPI;

using iLabs.DataTypes;
using iLabs.DataTypes.BatchTypes;
using iLabs.ServiceBroker.Services;


namespace iLabs.Services{
    
    /// <summary>
	/// ServiceBrokerService contains all of the Service Broker Web Service Calls.
	/// All of the Service Broker to Lab Server passthrough calls run in the context of a user's 
	/// Service Broker Web Interface session, and consequently have session enabled. This works
	/// because they are submitted from the Java Client, through one or more of the Service Broker
	/// passthrough methods, on to the corresponding method on the Lab Server. 
	/// There is one Method, Notify(), that is called from the Lab Server directly. This
	/// runs outside of the session context. 
	/// </summary>
	/// 
	
    [WebServiceBinding(Name = "IBatchSB", Namespace = "http://ilab.mit.edu")]
	[XmlType (Namespace="http://ilab.mit.edu")]
	[WebService (Name="BatchSBProxy", Namespace="http://ilab.mit.edu",
        Description="IBatchSB provides all of the 6.1 batch methods.  Many of these methods are passed through the ServiceBroker from client to LabServer or LabServer to Client."
	+ " All of the ServiceBroker passthrough calls run in the context of a user's "
	+ "ServiceBroker user session, and consequently have session enabled. "
	+ "<br/>There is one Method, Notify(), that is called from the Lab Server directly. This "
	+ "runs outside of the session context." )]
	public abstract class I_BatchSB : System.Web.Services.WebService
	{
		//private LabServerAPI SSService = new LabServerAPI();
		//AuthorizationWrapperClass wrapper = new AuthorizationWrapperClass();

		/// <summary>
		/// Instantiated to send sbAuthHeader objects in SOAP requests
		/// </summary>
		public sbAuthHeader sbHeader = new sbAuthHeader();
		
		
		//////////////////////////////////////////////////////
		///// SERVICE BROKER TO LAB SERVER API           /////
		/////////////////////////////////////////////////////
		
		/* IMPORTANT 
		 * Note that in the following methods the string labServerID refers to the
		 * lab Server GUID and not the internal SB database generated integer ID. 
		 * The integer IDs are referred to by the intLabServerID variable.
		 */
		/// <summary>
		/// Checks on the status of the lab server.
		/// </summary>
		/// <param name="labServerID">A string GUID that uniquely identifies the Lab Server</param>
		/// <returns>A LabStatus object containing a flag indicating whether or not the lab is online, along with a lab status description.</returns>
		/// <seealso cref="LabStatus">LabStatus Class</seealso>
		/// <remarks>Web Method</remarks>
		[WebMethod (Description="Checks on the status of the lab server.", EnableSession = true)]
		[SoapHeader("sbHeader", Direction=SoapHeaderDirection.In)]
        [SoapDocumentMethod(Binding = "IBatchSB")]
		public abstract LabStatus GetLabStatus(string labServerID);
		

		/// <summary>
		/// Checks on the effective queue length of the Lab Server. 
		/// Answers the following question: how many of the experiments currently in the execution queue would run before the new experiment? 
		/// </summary>
		/// <param name="labServerID">A string GUID that uniquely identifies the Lab Server</param>
		/// <param name="priorityHint">Indicates a requested priority for the hypothetical new experiment.  Possible values range from 20 (highest priority) to -20 (lowest priority); 0 is normal.  Priority hints may or may not be considered by the lab server. </param>
		/// <returns>A WaitEstimate object containing information about how many experiments precede this one in the queue, along with an estimate of how long it will be before the submitted experiment will run.</returns>
		/// <seealso cref="WaitEstimate">WaitEstimate Class</seealso>
		/// <remarks>Web Method</remarks>
		[WebMethod(Description="Checks on the effective queue length of the Lab Server.", EnableSession = true)]
		[SoapHeader("sbHeader", Direction=SoapHeaderDirection.In)]
        [SoapDocumentMethod(Binding = "IBatchSB")]
		public abstract WaitEstimate GetEffectiveQueueLength(string labServerID, int priorityHint);
		
		/// <summary>
		/// Gets general information about a Lab Server.
		/// </summary>
		/// <param name="labServerID">A string GUID that uniquely identifies the Lab Server.</param>
		/// <returns>A URL to a lab-specific information resource, e.g. a lab information page.</returns>
		/// <remarks>Web Method</remarks>
		[WebMethod(Description="Gets general information about a lab server.", EnableSession = true)]
		[SoapHeader("sbHeader", Direction=SoapHeaderDirection.In)]
        [SoapDocumentMethod(Binding = "IBatchSB")]
		public abstract string GetLabInfo(string labServerID);
		

		/// <summary>
		/// Gets the configuration of a lab server.
		/// </summary>
		/// <param name="labServerID">A string GUID that uniquely identifies the Lab Server.</param>
		/// <returns>An opaque, domain-dependent lab configuration.</returns>
		/// <remarks>Web Method</remarks>
		[WebMethod(Description="Gets the lab configuration of a LabServer. The configuration specification returned is in a format specific to that LabServer.", EnableSession = true)]
		[SoapHeader("sbHeader", Direction=SoapHeaderDirection.In)]
        [SoapDocumentMethod(Binding = "IBatchSB")]
		public abstract string GetLabConfiguration (string labServerID);
		

		/// <summary>
		/// Submits an experiment specification to the lab server for execution.
		/// </summary>
		/// <param name="labServerID">A string GUID that uniquely identifies the Lab Server.</param>
		/// <param name="experimentSpecification">An opaque, domain-dependent experiment specification. </param>
		/// <returns>A ValidationReport object containing information about whether or not the experiment would be accepted for execution, how long it would be before it could be run, and any error or warning messages.</returns>
		/// <seealso cref="ValidationReport">ValidationReport Class</seealso>
		/// <remarks>Web Method</remarks>
		[WebMethod(Description="Submits an experiment specification to the lab server for validation.", EnableSession = true)]
		[SoapHeader("sbHeader", Direction=SoapHeaderDirection.In)]
        [SoapDocumentMethod(Binding = "IBatchSB")]
		public abstract ValidationReport Validate (string labServerID, string experimentSpecification);
		
		/// <summary>
		/// Submits an experiment specification to the lab server for execution.  
		/// </summary>
		/// <param name="labServerID">A string GUID that uniquely identifies the Lab Server.</param>
		/// <param name="experimentSpecification">An opaque, domain-dependent experiment specification.</param>
		/// <param name="priorityHint">Indicates a requested priority for this experiment.  Possible values range from 20 (highest priority) to -20 (lowest priority); 0 is normal.  Priority hints may or may not be considered by the lab server.</param>
		/// <param name="emailNotification">If true, the service broker will make one attempt to notify the user (by email to the address with which the user’s account on the service broker is registered) when this experiment terminates.</param>
		/// <returns>A ClientSubmissionReport object containing a ValidationReport object, the experimentID, time to live information, and a WaitEstimate object.</returns>
		/// <seealso cref="ClientSubmissionReport">ClientSubmissionReport Class</seealso>
		/// <seealso cref="ValidationReport">ValidationReport Class</seealso>
		/// <seealso cref="WaitEstimate">WaitEstimate Class</seealso>
		/// <remarks>Web Method</remarks>
		[WebMethod(Description="Submits an experiment specification to the lab server for exection.", EnableSession = true)]
		[SoapHeader("sbHeader", Direction=SoapHeaderDirection.In)]
        [SoapDocumentMethod(Binding = "IBatchSB")]
		public abstract ClientSubmissionReport Submit (string labServerID, string experimentSpecification, int priorityHint, bool emailNotification);
		

		/// <summary>
		/// Checks on the status of a previously submitted experiment.
		/// </summary>
		/// <param name="experimentID">A token that identifies the experiment.</param>
		/// <returns>A LabExperimentStatus object containing an ExperimentStatus object and time to live information.</returns>
		/// <seealso cref="LabExperimentStatus">LabExperimentStatus Class</seealso>
		/// <seealso cref="ExperimentStatus">ExperimentStatus Class</seealso>
		/// <remarks>Web Method</remarks>
		[WebMethod (Description="Checks on the status of a previously submitted experiment.", EnableSession = true)]
		[SoapHeader("sbHeader", Direction=SoapHeaderDirection.In)]
        [SoapDocumentMethod(Binding = "IBatchSB")]
		public abstract LabExperimentStatus GetExperimentStatus(int experimentID);
		

		/// <summary>
		/// Cancels a previously submitted experiment. If the experiment is already running, makes best efforts to abort execution, but there is no guarantee that the experiment will not run to completion. 
		/// </summary>
		/// <param name="experimentID">A token tha identifies the experiment.</param>
		/// <returns>true if experiment was successfully removed from the queue (before execution had begun).  If false, user may want to call GetExperimentStatus() for more detailed information.</returns>
		/// <remarks>Web Method</remarks>
		[WebMethod (Description="Cancels a previously submitted experiment. If the experiment is already running, makes best efforts to abort execution, but there is no guarantee that the experiment will not run to completion.", EnableSession = true)]
		[SoapHeader("sbHeader", Direction=SoapHeaderDirection.In)]
        [SoapDocumentMethod(Binding = "IBatchSB")]
		public abstract bool Cancel(int experimentID);
		
		
		/// <summary>
		/// Retrieves the results from (or errors generated by) a previously submitted experiment from the lab server.
		/// </summary>
		/// <param name="experimentID">A token which identifies the experiment.</param>
		/// <returns>A ResultReport object containing the status code, result string, warning and error messages, and some optional XML information.</returns>
		/// <seealso cref="ResultReport">ResultReport class</seealso>
		/// <remarks>Web Method</remarks>
		[WebMethod(Description="Retrieves the results from (or errors generated by) a previously submitted experiment.", EnableSession = true)]
		[SoapHeader("sbHeader", Direction=SoapHeaderDirection.In)]
        [SoapDocumentMethod(Binding = "IBatchSB")]
		public abstract ResultReport RetrieveResult (int experimentID);
		
		/// <summary>
		/// Notifies the Service Broker that a previously submitted experiment has terminated. 
		/// Unlike the other methods in this class, this one is invoked by the Lab Server, not the Lab Client. 
		/// Consequently (and also unlike the others), it has no session context.
		/// </summary>
		/// <param name="experimentID">A token that identifies the experiment.</param>
		/// <remarks>Web Method</remarks>
		[WebMethod(Description="Notification from the Lab Server that a previously submitted experiment has terminated.")]
		[SoapHeader("sbHeader", Direction=SoapHeaderDirection.In)]
        [SoapDocumentMethod(Binding = "IBatchSB")]
		public abstract void Notify(int experimentID);
		

		//////////////////////////////////////////////////////
		///// CLIENT TO SERVICE BROKER API               /////
		/////////////////////////////////////////////////////
		
		
		/// <summary>
		/// Sets a client item value in the user's opaque data store.
		/// </summary>
		/// <param name="name">The name of the client item whose value is to be saved.</param>
		/// <param name="itemValue">The value that is to be saved with name.</param>
		/// <remarks>Web Method</remarks>
		[WebMethod(Description="Sets a client item value in the user's opaque data store.", EnableSession = true)]
		[SoapHeader("sbHeader", Direction=SoapHeaderDirection.In)]
        [SoapDocumentMethod(Binding = "IBatchSB")]
		public abstract void SaveClientItem(string name, string itemValue);
		

		/// <summary>
		/// Returns the value of a client item in the user's opaque data store.  
		/// </summary>
		/// <param name="name">The name of the client item whose value is to be returned.</param>
		/// <returns>The value of a client item in the user's opaque data store.</returns>
		/// <remarks>Web Method</remarks>
		[WebMethod(Description="Returns the value of an client item in the user's opaque data store.", EnableSession = true)]
		[SoapHeader("sbHeader", Direction=SoapHeaderDirection.In)]
        [SoapDocumentMethod(Binding = "IBatchSB")]
		public abstract string LoadClientItem(string name);
		

		/// <summary>
		/// Removes a client item from the user's opaque data store.
		/// </summary>
		/// <param name="name">The name of the client item to be removed.</param>
		/// <remarks>Web Method</remarks>
		[WebMethod(Description = "Removes an client item from the user's opaque data store.", EnableSession = true)]
		[SoapHeader("sbHeader", Direction=SoapHeaderDirection.In)]
        [SoapDocumentMethod(Binding = "IBatchSB")]
		public abstract void DeleteClientItem(string name);
		

		/// <summary>
		/// Enumerates the names of all client items in the user's opaque data store.
		/// </summary>
		/// <returns>An array of client items.</returns>
		/// <remarks>Web Method</remarks>
		[WebMethod(Description="Enumerates the names of all client items in the user's opaque data store. "
        + "The list is specific to the current client in the user's session state.", EnableSession = true)]
		[SoapHeader("sbHeader", Direction=SoapHeaderDirection.In)]
        [SoapDocumentMethod(Binding = "IBatchSB")]
		public abstract string[] ListAllClientItems();
		

		//////////////////////////////////////////////////////
		///// New methods added for David - CV, 8/5/2005 /////
		/////////////////////////////////////////////////////

		/// <summary>
		/// Retrieves a previously saved experiment specification.
		/// </summary>
		/// <param name="experimentID">A token which identifies the experiment.</param>
		/// <returns> The experimentSpecification, a domain-dependent experiment specification originally created by the Lab Client.</returns>
		/// <remarks>Web Method</remarks>
		[WebMethod(Description="Retrieves a previously saved experiment specification. "
            + "A domain-dependent experiment specification originally created by the Lab Client.", EnableSession = true)]
		[SoapHeader("sbHeader", Direction=SoapHeaderDirection.In)]
        [SoapDocumentMethod(Binding = "IBatchSB")]
		public abstract string RetrieveSpecification (int experimentID);
		
/*
		/// <summary>
		/// Retrieves the result from a previously executed experiment.
		/// </summary>
		/// <param name="experimentID">A token which identifies the experiment.</param>
		/// <returns> The experimentResult, an opaque string generated by the Lab Server to describe the result of a previously executed experiment.</returns>
		/// <remarks>Web Method</remarks>
		[WebMethod(Description="Retrieves the result from a previously executed experiment", EnableSession = true)]
		[SoapHeader("sbHeader", Direction=SoapHeaderDirection.In)]
        [SoapDocumentMethod(Binding = "IBatchSB")]
		public abstract string RetrieveExperimentResult (int experimentID);
*/
		/// <summary>
		/// Retrieves a previously saved lab configuration for a particular experiment.
		/// </summary>
		/// <param name="experimentID">A token which identifies the experiment.</param>
		/// <returns> The labConfiguration, an opaque string included in the ResultReport by the Lab Server to specify the configuration in which an experiment is executed.</returns>
		/// <remarks>Web Method</remarks>
		[WebMethod(Description="Retrieves a previously saved lab configuration for a particular experiment.", EnableSession = true)]
		[SoapHeader("sbHeader", Direction=SoapHeaderDirection.In)]
        [SoapDocumentMethod(Binding = "IBatchSB")]
		public abstract string RetrieveLabConfiguration (int experimentID);
		

		/// <summary>
		/// Saves or modifies an optional user defined annotation to the experiment record.
		/// </summary>
		/// <param name="experimentID">A token which identifies the experiment.</param>
		/// <param name="annotation">The annotation to be saved with the experiment.</param>
		/// <returns>The previous annotation or null if there wasn't one.</returns>
		/// <remarks>Web Method</remarks>
		[WebMethod(Description="Saves or modifies an optional user defined annotation to the experiment record.", EnableSession = true)]
		[SoapHeader("sbHeader", Direction=SoapHeaderDirection.In)]
        [SoapDocumentMethod(Binding = "IBatchSB")]
		public abstract string SaveAnnotation(int experimentID, string annotation);
		

		/// <summary>
		/// Retrieves a previously saved experiment annotation.
		/// </summary>
		/// <param name="experimentID">A token which identifies the experiment.</param>
		/// <returns>The annotation, a string originally created by the user via the Lab Client.</returns>
		/// <remarks>Web Method</remarks>
		[WebMethod(Description="Retrieves a previously saved experiment annotation.", EnableSession = true)]
		[SoapHeader("sbHeader", Direction=SoapHeaderDirection.In)]
        [SoapDocumentMethod(Binding = "IBatchSB")]
		public abstract string RetrieveAnnotation(int experimentID);
		

		/// <summary>
		/// Retrieves experiment metadata for experiments specified by an array of experiment IDs.
		/// </summary>
		/// <param name="experimentIDs">An array of tokens which identify experiments</param>
		/// <returns>An array of instances of the specified ExperimentInformation objects.</returns>
		/// <remarks>Web Method</remarks>
		[WebMethod(Description="Retrieves experiment metadata for experiments specified by an array of experiment IDs.", EnableSession = true)]
		[SoapHeader("sbHeader", Direction=SoapHeaderDirection.In)]
        [SoapDocumentMethod(Binding = "IBatchSB")]
		public abstract ExperimentInformation[] GetExperimentInformation(int[] experimentIDs);
		

	}
}
