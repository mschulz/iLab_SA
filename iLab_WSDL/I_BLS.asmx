<%@ WebService Language="c#" Class="I_BLS" %>


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
//using iLabs.DataTypes.SoapHeaderTypes;
using iLabs.DataTypes.TicketingTypes;
using iLabs.DataTypes.BatchTypes;

/// <summary>
/// The interface definition for an InteractiveLabServer. Currently only one method, 
/// additional methods will be added as needed.
/// </summary>
/// [System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[SoapDocumentService(RoutingStyle = SoapServiceRoutingStyle.RequestElement)]
[XmlType(Namespace = "http://ilab.mit.edu")]
[WebServiceBinding(Name = "I_BLS", Namespace = "http://ilab.mit.edu"),
WebService(Name = "BatchLSProxy", Namespace = "http://ilab.mit.edu",
    Description="The interface definition for a BatchLabServer. In the merged architecture most of these "
    + "methods are built using the interactive API methods. Methods that in the traditional batch architecture "
    + "automatically saves the messages to the database now save to the Experiment Storage Service.")]
public abstract class I_BLS : System.Web.Services.WebService
{
    public AuthHeader authHeader = new AuthHeader();
    
    ////////////////////////////////////////
    // Cancel (WebMethod)                 //
    ////////////////////////////////////////

    /// <summary>
    /// Cancels a previously submitted experiment. If the experiment is already running, makes best
    /// effort to abort execution, but there is no guarantee that the experiment will not run to completion.
    /// </summary>
    /// <param name="experimentID">A token identifying the experiment returned from a previous call to Submit()</param>
    /// <returns>true if the experiment was successfully removed from the queue
    /// (before execution had begun). If false, the user may want to call GetExperimentStatus() for
    /// more detailed information.
    /// </returns>
    [WebMethod(Description = "Cancels a previously submitted experiment. If the experiment is already running, makes "
    + "best effort to abort execution, but there is no guarantee that the experiment will not run to completion."),
    SoapDocumentMethod(Binding = "I_BLS"),
    SoapHeader("authHeader", Direction = SoapHeaderDirection.In)]
    public abstract bool Cancel(int experimentID);

    ////////////////////////////////////////
    // GetLabStatus (WebMethod)           //
    ////////////////////////////////////////
    /// <summary>
    /// Checks on the status of the lab server.
    /// </summary>
    /// <returns>A LabStatus class containing the results of the call.</returns>
    /// <seealso cref="LabStatus">LabStatus Class</seealso>
    [WebMethod(Description = "Checks on the status of the lab server."),
   SoapDocumentMethod(Binding = "I_BLS"),
   SoapHeader("authHeader", Direction = SoapHeaderDirection.In)]
    public abstract LabStatus GetLabStatus();

    ////////////////////////////////////////
    // GetEffectiveQueueLength (WebMethod)//
    ////////////////////////////////////////
    /// <summary>
    /// Checks on the effective queue length of the lab server. 
    /// Answers the following question: how many of the experiments currently in the execution queue
    /// would run before the new experiment?
    /// </summary>
    /// <param name="userGroup">Effective group of the user submitting the hypothetical new experiment.</param>
    /// <param name="priorityHint">Indicates a requested priority for the hypothetical new experiment.
    ///  Possible values range from 20 (highest priority) to -20 (lowest priority); 0 is normal.  
    ///  Priority hints may or may not be considered by the lab server. 
    /// </param>
    /// <returns>Returns a WaitEstimate class</returns>
    /// <seealso cref="WaitEstimate">WaitEstimate Class</seealso>
    [WebMethod(Description = "Checks on the effective queue length of the lab server. "
     + "Answers the following question: how many of the experiments currently in the execution queue "
     + "would run before the new experiment?"),
   SoapDocumentMethod(Binding = "I_BLS"),
   SoapHeader("authHeader", Direction = SoapHeaderDirection.In)]
    public abstract WaitEstimate GetEffectiveQueueLength(string userGroup, int priorityHint);

    ////////////////////////////////////////
    // GetLabInfo (WebMethod)             //
    ////////////////////////////////////////
    /// <summary>
    /// Gets general information about a lab server.
    /// </summary>
    /// <returns>A URL to a lab-specific information resource, e.g. a lab information page.</returns>
    [WebMethod(Description = "Gets general information about a lab server."),
   SoapDocumentMethod(Binding = "I_BLS"),
   SoapHeader("authHeader", Direction = SoapHeaderDirection.In)]
    public abstract string GetLabInfo();

    ////////////////////////////////////////
		// GetLabConfiguration (WebMethod)    //
		////////////////////////////////////////
		/// <summary>
		/// Gets the configuration of a lab server.
		/// </summary>
		/// <param name="userGroup">Effective group of the user requesting the lab configuration.</param>
		/// <returns>An opaque, domain-dependent lab configuration.</returns>
    [WebMethod(Description = "Gets the configuration of a lab server."),
SoapDocumentMethod(Binding = "I_BLS"),
SoapHeader("authHeader", Direction = SoapHeaderDirection.In)]
    public abstract string GetLabConfiguration(string userGroup);
    
    ////////////////////////////////////////
    // Validate (WebMethod)               //
    ////////////////////////////////////////
    /// <summary>
    /// Checks whether an experiment specification would be accepted if submitted for execution.
    /// </summary>
    /// <param name="experimentSpecification">/* An opaque, domain-dependent experiment specification.</param>
    /// <param name="userGroup">Effective group of the user submitting this experiment.</param>
    /// <returns>Returns a ValidationReport class</returns>
    /// <seealso cref="ValidationReport">ValidationReport Class</seealso>
    [WebMethod(Description = "Checks whether an experiment specification would be accepted if submitted for execution."),
   SoapDocumentMethod(Binding = "I_BLS"),
   SoapHeader("authHeader", Direction = SoapHeaderDirection.In)]
    public abstract ValidationReport Validate(string experimentSpecification, string userGroup);

    ////////////////////////////////////////
    // Submit (WebMethod)                 //
    ////////////////////////////////////////
    /// <summary>
    /// Submits an experiment specification to the lab server for execution.
    /// </summary>
    /// <param name="experimentID">The identifying token that can be used to inquire about the status of this experiment and to retrieve the results when ready.</param>
    /// <param name="experimentSpecification">An opaque, domain-dependent experiment specification.</param>
    /// <param name="userGroup">Effective group of the user submitting this experiment.</param>
    /// <param name="priorityHint">Indicates a requested priority for this experiment.  Possible values range from 20 (highest priority) to -20 (lowest priority); 0 is normal.  Priority hints may or may not be considered by the lab server.</param>
    /// <returns>Returns a Submission Report class</returns>
    /// <seealso cref="SubmissionReport">SubmissionReport Class</seealso>
    [WebMethod(Description = "Submits an experiment specification to the lab server for execution."),
   SoapDocumentMethod(Binding = "I_BLS"),
   SoapHeader("authHeader", Direction = SoapHeaderDirection.In)]
    public abstract SubmissionReport Submit(int experimentID, string experimentSpecification, string userGroup, int priorityHint);

    ////////////////////////////////////////
    // GetExperimentStatus (WebMethod)    //
    ////////////////////////////////////////
    /// <summary>
    /// Checks on the status of a previously submitted experiment.
    /// </summary>
    /// <param name="experimentID">A token that identifies the experiment.</param>
    /// <returns>Returns a LabExperimentStatus class</returns>
    /// <seealso cref="LabExperimentStatus">LabExperimentStatus Class</seealso>
    [WebMethod(Description = "Checks on the status of a previously submitted experiment."),
   SoapDocumentMethod(Binding = "I_BLS"),
   SoapHeader("authHeader", Direction = SoapHeaderDirection.In)]
    public abstract LabExperimentStatus GetExperimentStatus(int experimentID);

    ////////////////////////////////////////
    // RetrieveResult (WebMethod)         //
    ////////////////////////////////////////
    /// <summary>
    /// Retrieves the results from (or errors generated by) a previously submitted experiment.
    /// </summary>
    /// <param name="experimentID">A token identifying the experiment.</param>
    /// <returns>Returns a ResultReport class</returns>
    /// <seealso cref="ResultReport">ResultReport Class</seealso>
    [WebMethod(Description = "Retrieves the results from (or errors generated by) a previously submitted experiment."),
   SoapDocumentMethod(Binding = "I_BLS"),
   SoapHeader("authHeader", Direction = SoapHeaderDirection.In)]
    public abstract ResultReport RetrieveResult(int experimentID);

}
