/*
 * Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
 * Please see license.txt in top level directory for full license.
 */

/* $Id$ */

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.Xml;
using System.Threading ;
using System.Web.Services.Protocols;
using System.Net;
using System.Web.SessionState;
using System.Reflection;
using System.Configuration;

using iLabs.Core;
using iLabs.DataTypes;
using iLabs.DataTypes.BatchTypes;
using iLabs.DataTypes.ProcessAgentTypes;
using iLabs.DataTypes.StorageTypes;
using iLabs.DataTypes.TicketingTypes;
using iLabs.DataTypes.SoapHeaderTypes;
using iLabs.Proxies.BatchLS;
using iLabs.Proxies.PAgent;
using iLabs.Proxies.ESS;
using iLabs.Proxies.LSS;
using iLabs.Proxies.USS;
using iLabs.ServiceBroker;
using iLabs.ServiceBroker.DataStorage ;
using iLabs.ServiceBroker.Administration;
using iLabs.ServiceBroker.Authorization;
using iLabs.ServiceBroker.Internal;
using iLabs.ServiceBroker.Batch;
using iLabs.ServiceBroker.Services;

using iLabs.Ticketing;
using iLabs.UtilLib;



namespace iLabs.ServiceBroker.iLabSB
{
	/// <summary>
	/// MergedSB contains all of the 6.1 Batch Service Broker Web Service Calls.
	/// All of the Service Broker to Lab Server passthrough calls run in the context of a user's 
	/// Service Broker Web Interface session, and consequently have session enabled. This works
	/// because they are submitted from the Client, through one or more of the Service Broker
	/// passthrough methods, on to the corresponding method on the Lab Server. 
	/// There is one Method, Notify(), that is called from the Lab Server directly. This
	/// runs outside of the session context. 
	/// </summary>
	/// 
	[XmlType (Namespace="http://ilab.mit.edu")]
    [WebServiceBinding(Name = "IBatchSB", Namespace = "http://ilab.mit.edu"),
   WebServiceBinding(Name = "IServiceBroker", Namespace = "http://ilab.mit.edu/iLabs/Services"),
    WebServiceBinding(Name = "IProcessAgent", Namespace = "http://ilab.mit.edu/iLabs/Services"),
    WebServiceBinding(Name = "ITicketIssuer", Namespace = "http://ilab.mit.edu/iLabs/Services"),
   WebService(Name = "iLabServiceBroker", Namespace = "http://ilab.mit.edu/iLabs/Services")]
	public class iLabServiceBroker : InteractiveSB
	{
        /// <summary>
        /// Instantiated to send sbAuthHeader objects in SOAP requests
        /// </summary>
        public sbAuthHeader sbHeader = new sbAuthHeader();
		private BatchLSProxy batchLS_Proxy = new BatchLSProxy();

		public iLabServiceBroker()
		{
			//CODEGEN: This call is required by the ASP.NET Web Services Designer
			InitializeComponent();
		}

		#region Component Designer generated code
		
		//Required by the Web Services Designer 
		private IContainer components = null;
				
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if(disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);		
		}
		
		#endregion


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
		[WebMethod (Description="Checks on the status of the lab server", EnableSession = true)]
		[SoapHeader("sbHeader", Direction=SoapHeaderDirection.In)]
        [SoapDocumentMethod(Action="http://ilab.mit.edu/GetLabStatus", Binding = "IBatchSB",
            RequestNamespace = "http://ilab.mit.edu", ResponseNamespace = "http://ilab.mit.edu")]
		public LabStatus GetLabStatus(string labServerID)
		{
			try
			{
      
				// Checking if user has permission to use the lab server. The method will set headers for lab server calls
				//if authorization is successful.
                CheckAndSetLSAuthorization(labServerID);
			
				// call the webservice
				return batchLS_Proxy.GetLabStatus();
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Checks on the effective queue length of the Lab Server. 
		/// Answers the following question: how many of the experiments currently in the execution queue would run before the new experiment? 
		/// </summary>
		/// <param name="labServerID">A string GUID that uniquely identifies the Lab Server</param>
		/// <param name="priorityHint">Indicates a requested priority for the hypothetical new experiment.  Possible values range from 20 (highest priority) to -20 (lowest priority); 0 is normal.  Priority hints may or may not be considered by the lab server. </param>
		/// <returns>A WaitEstimate object containing information about how many experiments precede this one in the queue, along with an estimate of how long it will be before the submitted experiment will run.</returns>
		/// <seealso cref="WaitEstimate">WaitEstimate Class</seealso>
		/// <remarks>Web Method</remarks>
		[WebMethod(Description="Checks on the effective queue length of the Lab Server", EnableSession = true)]
		[SoapHeader("sbHeader", Direction=SoapHeaderDirection.In)]
        [SoapDocumentMethod(Action = "http://ilab.mit.edu/GetEffectiveQueueLength", Binding = "IBatchSB", 
		RequestNamespace = "http://ilab.mit.edu", ResponseNamespace = "http://ilab.mit.edu")]
		public WaitEstimate GetEffectiveQueueLength(string labServerID, int priorityHint)
		{
			try
			{
 
				// Checking if user has permission to use the lab server. The method will set headers for lab server calls
				//if authorization is successful.
                CheckAndSetLSAuthorization(labServerID);
	
				string userGroup = Session["GroupName"].ToString();
				return batchLS_Proxy.GetEffectiveQueueLength(userGroup, priorityHint);
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Gets general information about a Lab Server.
		/// </summary>
		/// <param name="labServerID">A string GUID that uniquely identifies the Lab Server.</param>
		/// <returns>A URL to a lab-specific information resource, e.g. a lab information page.</returns>
		/// <remarks>Web Method</remarks>
		[WebMethod(Description="Gets general information about a lab server", EnableSession = true)]
		[SoapHeader("sbHeader", Direction=SoapHeaderDirection.In)]
        [SoapDocumentMethod(Action = "http://ilab.mit.edu/GetLabInfo",Binding = "IBatchSB", RequestNamespace = "http://ilab.mit.edu", ResponseNamespace = "http://ilab.mit.edu")]
		public string GetLabInfo(string labServerID)
		{
			try 
			{
				// Checking if user has permission to use the lab server. The method will set headers for lab server calls
				//if authorization is successful.
                CheckAndSetLSAuthorization(labServerID);

				return batchLS_Proxy.GetLabInfo();
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Gets the configuration of a lab server.
		/// </summary>
		/// <param name="labServerID">A string GUID that uniquely identifies the Lab Server.</param>
		/// <returns>An opaque, domain-dependent lab configuration.</returns>
		/// <remarks>Web Method</remarks>
		[WebMethod(Description="Gets the lab configuration of a lab server", EnableSession = true)]
		[SoapHeader("sbHeader", Direction=SoapHeaderDirection.In)]
        [SoapDocumentMethod(Action = "http://ilab.mit.edu/GetLabConfiguration", Binding = "IBatchSB", RequestNamespace = "http://ilab.mit.edu", ResponseNamespace = "http://ilab.mit.edu")]
		public string GetLabConfiguration (string labServerID)
		{
			try
			{   
              
				// Checking if user has permission to use the lab server. The method will set headers for lab server calls
				//if authorization is successful.
                CheckAndSetLSAuthorization(labServerID);

				string userGroup = Session["GroupName"].ToString();
				string lc= batchLS_Proxy.GetLabConfiguration(userGroup);
				if ((lc==null)||(lc.Length==0))
					throw new SoapException("Lab configuration does not exist.",SoapException.ServerFaultCode);
				else
					return lc;
			}
			catch (AccessDeniedException ade)
			{
				throw new SoapException(ade.Message+". "+ade.GetBaseException(), SoapException.ServerFaultCode,ade);
			}
			catch (Exception ex)
			{
			
				throw new SoapException(ex.Message+". "+ex.GetBaseException(), SoapException.ServerFaultCode,ex);
			}
		}

		/// <summary>
		/// Submits an experiment specification to the lab server for execution.
		/// </summary>
		/// <param name="labServerID">A string GUID that uniquely identifies the Lab Server.</param>
		/// <param name="experimentSpecification">An opaque, domain-dependent experiment specification. </param>
		/// <returns>A ValidationReport object containing information about whether or not the experiment would be accepted for execution, how long it would be before it could be run, and any error or warning messages.</returns>
		/// <seealso cref="ValidationReport">ValidationReport Class</seealso>
		/// <remarks>Web Method</remarks>
		[WebMethod(Description="Submits an experiment specification to the lab server for exection", EnableSession = true)]
		[SoapHeader("sbHeader", Direction=SoapHeaderDirection.In)]
        [SoapDocumentMethod(Action = "http://ilab.mit.edu/Validate", Binding = "IBatchSB", RequestNamespace = "http://ilab.mit.edu", ResponseNamespace = "http://ilab.mit.edu")]
		public ValidationReport Validate (string labServerID, string experimentSpecification)
		{
			try 
			{
                // Checking if user has permission to use the lab server. The method will set headers for lab server calls
				//if authorization is successful.
                CheckAndSetLSAuthorization(labServerID);

				string userGroup = Session["GroupName"].ToString();
				ValidationReport vReport = batchLS_Proxy.Validate(experimentSpecification, userGroup);
				return vReport;
			}
			catch
			{
				throw;
			}
		}

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
        [WebMethod(Description = "Submits an experiment specification to the lab server for exection", EnableSession = true)]
        [SoapHeader("sbHeader", Direction = SoapHeaderDirection.In)]
        [SoapDocumentMethod(Action = "http://ilab.mit.edu/Submit", Binding = "IBatchSB", RequestNamespace = "http://ilab.mit.edu", ResponseNamespace = "http://ilab.mit.edu")]
        public ClientSubmissionReport Submit(string labServerID, string experimentSpecification, int priorityHint, bool emailNotification)
        {
            // Default to 24 hours duration
            long duration = TimeSpan.TicksPerDay/TimeSpan.TicksPerSecond;
            int seqNo = 0;
            ClientSubmissionReport clientSReport = null;
            try{
               
                // Checking if user has permission to use the lab server. The method will set headers for lab server calls
                //if authorization is successful.
                CheckAndSetLSAuthorization(labServerID);

                //Retrieve variables from session
                int userID = Convert.ToInt32(Session["UserID"]);
                int effectiveGroupID = Convert.ToInt32(Session["GroupID"]);
                int clientID = 0;
                if (Session["ClientID"] != null )
                    clientID = Convert.ToInt32(Session["ClientID"]);
                string effectiveGroup = Session["GroupName"].ToString();
                ProcessAgentInfo infoLS = brokerDB.GetProcessAgentInfo(labServerID);

                //if (infoLS.retired)
                //{
                //    throw new Exception("The Batch Lab Server is retired");
                //}
                //// get qualifier ID of labServer
                //int qualifierID = AuthorizationAPI.GetQualifierID(infoLS.agentId, Qualifier.labServerQualifierTypeID);

                ///* End collecting information */

                //// Checking if user has permission to use the lab server
                //if (!AuthorizationAPI.CheckAuthorization(effectiveGroupID, Function.useLabServerFunctionType, qualifierID))
                //{
                //    // check fails

                //    throw new AccessDeniedException("Access denied using labServer '" + infoLS.agentName + "'.");
                //}
                //else
                //{
                    int[] groupIDs = new int[1];
                    groupIDs[0] = effectiveGroupID;

                    SubmissionReport sReport = new SubmissionReport();

                    clientSReport = new ClientSubmissionReport();
                    clientSReport.vReport = new ValidationReport();
                    clientSReport.wait = new WaitEstimate();

                    // 1. Create Coupon for ExperimentCollection
                    Coupon coupon = brokerDB.CreateCoupon();

                    int essID = brokerDB.FindProcessAgentIdForClient(clientID, ProcessAgentType.EXPERIMENT_STORAGE_SERVER);
                    //
                    // 2. create ServiceBroker experiment record and get corresponding experiment id
                    // This checks authorization.
                    long experimentID = wrapper.CreateExperimentWrapper(StorageStatus.INITIALIZED, userID, effectiveGroupID, infoLS.agentId, clientID,
                        essID, DateTime.UtcNow, duration);

                    // Store a record of the Experiment Collection Coupon
                    DataStorageAPI.InsertExperimentCoupon(experimentID, coupon.couponId);

                    //3.A create ESS administer experiment ticket, Add 10 minutes to duration
                    // This must be created before the ESS experiment records may be created
                    ProcessAgentInfo essAgent = brokerDB.GetProcessAgentInfo(essID);
                    if (essAgent.retired)
                    {
                        throw new Exception("The Batch Lab Server is retired");
                    }
                    TicketLoadFactory factory = TicketLoadFactory.Instance();

                    brokerDB.AddTicket(coupon,
                           TicketTypes.ADMINISTER_EXPERIMENT, essAgent.AgentGuid, ProcessAgentDB.ServiceGuid, duration, factory.createAdministerExperimentPayload(experimentID, essAgent.webServiceUrl));

                    //3.B create store record ticket, in the MergedSB the records are all saved via the serviceBroker
                    brokerDB.AddTicket(coupon,
                           TicketTypes.STORE_RECORDS, essAgent.agentGuid, ProcessAgentDB.ServiceGuid, duration, factory.StoreRecordsPayload(true, experimentID, essAgent.webServiceUrl));

                    //3.C create retrieve experiment ticket, retrieve Experiment Records never expires, unless experiment deleted
                    //    This should be changed to a long but finite period once eadExisting Expermint is in place.
                    brokerDB.AddTicket(coupon,
                           TicketTypes.RETRIEVE_RECORDS, essAgent.agentGuid, ProcessAgentDB.ServiceGuid, -1, factory.RetrieveRecordsPayload(experimentID, essAgent.webServiceUrl));


                    // 3.D Create the ESS Experiment Records
                    ExperimentStorageProxy ess = new ExperimentStorageProxy();
                    ess.AgentAuthHeaderValue = new AgentAuthHeader();
                    ess.AgentAuthHeaderValue.coupon = essAgent.identOut;
                    ess.AgentAuthHeaderValue.agentGuid = ProcessAgentDB.ServiceGuid;
                    ess.OperationAuthHeaderValue = new OperationAuthHeader();
                    ess.OperationAuthHeaderValue.coupon = coupon;
                    ess.Url = essAgent.webServiceUrl;

                    // Call the ESS to create the ESS Records and open the experiment
                    StorageStatus status = ess.OpenExperiment(experimentID, duration);
                    if (status != null)
                        DataStorageAPI.UpdateExperimentStatus(status);

                    seqNo = ess.AddRecord(experimentID, ProcessAgentDB.ServiceGuid,
                              BatchRecordType.SPECIFICATION, true, experimentSpecification, null);

                    // save lab configuration
                    string labConfiguration = batchLS_Proxy.GetLabConfiguration(effectiveGroup);
                    seqNo = ess.AddRecord(experimentID, labServerID,
                        BatchRecordType.LAB_CONFIGURATION, true, labConfiguration, null);

                    // call labServer submit
                    sReport = batchLS_Proxy.Submit(Convert.ToInt32(experimentID), experimentSpecification, effectiveGroup, priorityHint);

                    // save submission report
                    //wrapper.SaveSubmissionReportWrapper(experimentID, sReport);
                    if (sReport.vReport != null)
                        if ((sReport.vReport.errorMessage != null) && (sReport.vReport.errorMessage.CompareTo("") != 0))
                        {
                            seqNo = ess.AddRecord(experimentID, labServerID, BatchRecordType.VALIDATION_ERROR, false, sReport.vReport.errorMessage, null);

                        }

                    if (sReport.vReport.warningMessages != null)
                        foreach (string s in sReport.vReport.warningMessages)
                        {
                            if ((s != null) && (s.CompareTo("") != 0))
                                seqNo = ess.AddRecord(experimentID, labServerID, BatchRecordType.VALIDATION_WARNING, false, s, null);
                        }

                    // return clientSubmissionReport
                    if (sReport.vReport != null)
                    {
                        clientSReport.vReport.accepted = sReport.vReport.accepted;
                        clientSReport.vReport.errorMessage = sReport.vReport.errorMessage;
                        // if error exists then change status to "an experiment with a problem"
                        if ((sReport.vReport.errorMessage != null) && (!sReport.vReport.errorMessage.Equals("")))
                        {
                            StorageStatus sStatus = new StorageStatus();
                            sStatus.experimentId = experimentID;
                            //sStatus.estRuntime=sReport.vReport.estRuntime;
                            sStatus.status = StorageStatus.BATCH_TERMINATED_ERROR;
                            DataStorageAPI.UpdateExperimentStatus(sStatus);
                        }
                        clientSReport.vReport.estRuntime = sReport.vReport.estRuntime;
                        clientSReport.vReport.warningMessages = sReport.vReport.warningMessages;
                    }
                    clientSReport.experimentID = Convert.ToInt32(experimentID);
                    clientSReport.minTimeToLive = sReport.minTimetoLive;
                    if (sReport.wait != null)
                    {
                        clientSReport.wait.effectiveQueueLength = sReport.wait.effectiveQueueLength;
                        clientSReport.wait.estWait = sReport.wait.estWait;
                    }
                //}
                return clientSReport;
            }

            catch
            {
                throw;
            }
        }

		/// <summary>
		/// Checks on the status of a previously submitted experiment.
		/// </summary>
		/// <param name="experimentID">A token that identifies the experiment.</param>
		/// <returns>A LabExperimentStatus object containing an ExperimentStatus object and time to live information.</returns>
		/// <seealso cref="LabExperimentStatus">LabExperimentStatus Class</seealso>
		/// <seealso cref="ExperimentStatus">ExperimentStatus Class</seealso>
		/// <remarks>Web Method</remarks>
        [WebMethod(Description = "Checks on the status of a previously submitted experiment", EnableSession = true)]
        [SoapHeader("sbHeader", Direction = SoapHeaderDirection.In)]
        [SoapDocumentMethod(Action = "http://ilab.mit.edu/GetExperimentStatus", Binding = "IBatchSB", RequestNamespace = "http://ilab.mit.edu", ResponseNamespace = "http://ilab.mit.edu")]
        public LabExperimentStatus GetExperimentStatus(int experimentID)
        {
            LabExperimentStatus status = null;
            try
            {
                int intLabServerID = AdministrativeAPI.GetLabServerID(experimentID);
                // Checking if user has permission to use the lab server. The method will set headers for lab server calls
                //if authorization is successful.
                CheckAndSetLSAuthorization(intLabServerID);
             
                ExperimentSummary summary = DataStorageAPI.RetrieveExperimentSummary(experimentID);
                if ((summary != null) && ((summary.status & StorageStatus.CLOSED) == StorageStatus.CLOSED))
                {
                    status = new LabExperimentStatus();
                    status.minTimetoLive = 0;
                    ExperimentStatus report2 = new ExperimentStatus();
                    report2.estRemainingRuntime = 0;
                    report2.estRuntime = 0;
                    report2.statusCode = summary.status & StorageStatus.BATCH_MASK;
                    report2.wait = new WaitEstimate();
                    status.statusReport = report2;
                }
                else
                {
                    status = batchLS_Proxy.GetExperimentStatus(experimentID);
                    DataStorageAPI.UpdateExperimentStatus((long)experimentID, status.statusReport.statusCode);
                }
            }
            catch
            {
                throw;
            }
            return status;
        }

		/// <summary>
		/// Cancels a previously submitted experiment. If the experiment is already running, makes best efforts to abort execution, but there is no guarantee that the experiment will not run to completion. 
		/// </summary>
		/// <param name="experimentID">A token tha identifies the experiment.</param>
		/// <returns>true if experiment was successfully removed from the queue (before execution had begun).  If false, user may want to call GetExperimentStatus() for more detailed information.</returns>
		/// <remarks>Web Method</remarks>
		[WebMethod (Description="Cancels a previously submitted experiment. If the experiment is already running, makes best efforts to abort execution, but there is no guarantee that the experiment will not run to completion", EnableSession = true)]
		[SoapHeader("sbHeader", Direction=SoapHeaderDirection.In)]
        [SoapDocumentMethod(Action = "http://ilab.mit.edu/Cancel", Binding = "IBatchSB", RequestNamespace = "http://ilab.mit.edu", ResponseNamespace = "http://ilab.mit.edu")]
		public bool Cancel(int experimentID)
		{
			try
			{
                
                int intLabServerID = AdministrativeAPI.GetLabServerID(experimentID);

				// Checking if user has permission to use the lab server. The method will set headers for lab server calls
				//if authorization is successful.
				CheckAndSetLSAuthorization(intLabServerID);
                bool status = batchLS_Proxy.Cancel(experimentID);
                //if (status)
                //{
                DataStorageAPI.RetrieveExperimentSummary(experimentID);
                DataStorageAPI.CloseExperiment(experimentID, StorageStatus.BATCH_CANCELLED);
                //}
                return status;
			}
			catch
			{
				throw;
			}
		}
		
		/// <summary>
		/// Retrieves the results from (or errors generated by) a previously submitted experiment from the 
        /// ExperimentStorageServer.
		/// </summary>
		/// <param name="experimentID">A token which identifies the experiment.</param>
		/// <returns>A ResultReport object containing the status code, result string, warning and error messages, and some optional XML information.</returns>
		/// <seealso cref="ResultReport">ResultReport class</seealso>
		/// <remarks>Web Method</remarks>
		[WebMethod(Description="Retrieves the results from (or errors generated by) a previously submitted experiment", EnableSession = true)]
		[SoapHeader("sbHeader", Direction=SoapHeaderDirection.In)]
        [SoapDocumentMethod(Action = "http://ilab.mit.edu/RetrieveResult", Binding = "IBatchSB", RequestNamespace = "http://ilab.mit.edu", ResponseNamespace = "http://ilab.mit.edu")]
		public ResultReport RetrieveResult (int experimentID)
		{
            /* Status Code key: 
			1: if waiting in the execution queue
			2: if currently running
			3: if terminated normally
			4: if terminated with errors (this includes cancellation by user in mid-execution)
			5: if cancelled by user before execution had begun
			6: if unknown labExperimentID. 
			7: Assigned by Service Broker if experiment is not valid (done in submit call)
			*/
            ResultReport sbReport = null;
            try
			{
                long expID = (long) experimentID;
				
                //first try to recreate session if using an html client
                if (Session == null || (Session["UserID"] == null) || (Session["UserID"].ToString() == ""))
                {
                    if (sbHeader.couponID > 0)
                    {
                        wrapper.SetServiceSession(new Coupon(ProcessAgentDB.ServiceGuid, sbHeader.couponID, sbHeader.couponPassKey));
                    }
                }
                // Checking if user has permission to use the lab server. The method will set headers for lab server calls
                //if authorization is successful.
                
				int roles = wrapper.GetExperimentAuthorizationWrapper(expID);
                if((roles & 1) != 1){
                    throw new AccessDeniedException("The experiment does not exist or you do not have read permission.");
                }
                sbReport = BatchAPI.GetResultReport(experimentID);
            }
			catch(Exception ex)
			{
				throw new SoapException(ex.Message+". "+ex.GetBaseException(), SoapException.ServerFaultCode,ex);
			}
            return sbReport;
		}

		/// <summary>
		/// Notifies the Service Broker that a previously submitted experiment has terminated. 
		/// Unlike the other methods in this class, this one is invoked by the Lab Server, not the Lab Client. 
		/// Consequently (and also unlike the others), it has no session context.
        /// The ServiceBroker will download the experiment results from the Lab Server and store them on the ESS 
        /// and update the experimentAdministrative record on the serviceBroker
		/// </summary>
		/// <param name="experimentID">A token that identifies the experiment.</param>
		/// <remarks>Web Method</remarks>
		[WebMethod(Description="Notification from the Lab Server that a previously submitted experiment has terminated.")]
		[SoapHeader("sbHeader", Direction=SoapHeaderDirection.In)]
        [SoapDocumentMethod(Action = "http://ilab.mit.edu/Notify", Binding = "IBatchSB", RequestNamespace = "http://ilab.mit.edu", ResponseNamespace = "http://ilab.mit.edu")]
		public void Notify(int experimentID)
		{
			try 
			{
                //// No session state for this call, must process header information
                // It seems several of the older batch labs do not populate the header so commented out
                //if(sbHeader.couponID > 0 && sbHeader.couponPassKey != null){ 
                //    if(brokerDB.AuthenticateIssuedCoupon(sbHeader.couponID,sbHeader.couponPassKey)){
                        ResultReport report = BatchAPI.GetResultReport(experimentID);
                //    }
                //}
            }
			catch
			{
				throw;
			}
		}




      
         
                //////////////////////////////////////////////////////
                ///// CLIENT TO SERVICE BROKER API               /////
                /////////////////////////////////////////////////////

		 /// <summary>
        /// Sets a client item value in the user's opaque data store.
        /// </summary>
        /// <param name="name">The name of the client item whose value is to be saved.</param>
        /// <param name="itemValue">The value that is to be saved with name.</param>
        /// <remarks>Web Method</remarks>
        [WebMethod(Description = "Sets a client item value in the user's opaque data store", EnableSession = true)]
        [SoapHeader("sbHeader", Direction = SoapHeaderDirection.In, Required = true)]
        [SoapDocumentMethod("http://ilab.mit.edu/SaveClientItem",Binding = "IBatchSB", 
            RequestNamespace = "http://ilab.mit.edu", ResponseNamespace = "http://ilab.mit.edu")]
        public void SaveClientItem(string name, string itemValue)
        {
            //first try to recreate session if using an html client
            if (Session == null || (Session["UserID"] == null) || (Session["UserID"].ToString() == ""))
            {
                if(sbHeader != null)
                wrapper.SetServiceSession(new Coupon(ProcessAgentDB.ServiceGuid, sbHeader.couponID, sbHeader.couponPassKey));
            }
            int userID = Convert.ToInt32(Session["UserID"]);
            int clientID = Convert.ToInt32(Session["ClientID"]);
            DataStorageAPI.SaveClientItemValue(clientID, userID, name, itemValue);
        }


        /// <summary>
        /// Returns the value of a client item in the user's opaque data store.  
        /// </summary>
        /// <param name="name">The name of the client item whose value is to be returned.</param>
        /// <returns>The value of a client item in the user's opaque data store.</returns>
        /// <remarks>Web Method</remarks>
        [WebMethod(Description = "Returns the value of an client item in the user's opaque data store", EnableSession = true)]
        [SoapHeader("sbHeader", Direction = SoapHeaderDirection.In, Required = true)]
        [SoapDocumentMethod("http://ilab.mit.edu/LoadClientItem", Binding = "IBatchSB",
            RequestNamespace = "http://ilab.mit.edu", ResponseNamespace = "http://ilab.mit.edu")]
        public string LoadClientItem(string name)
        {
            //first try to recreate session if using an html client
            if (Session == null || (Session["UserID"] == null) || (Session["UserID"].ToString() == ""))
            {
                if (sbHeader != null)
                wrapper.SetServiceSession(new Coupon(ProcessAgentDB.ServiceGuid, sbHeader.couponID, sbHeader.couponPassKey));
            }
            int userID = Convert.ToInt32(Session["UserID"]);
            int clientID = Convert.ToInt32(Session["ClientID"]);

            return DataStorageAPI.GetClientItemValue(clientID, userID, new string[] { name })[0].ToString();
        }


        /// <summary>
        /// Removes a client item from the user's opaque data store.
        /// </summary>
        /// <param name="name">The name of the client item to be removed.</param>
        /// <remarks>Web Method</remarks>
        [WebMethod(Description = "Removes an client item from the user's opaque data store", EnableSession = true)]
        [SoapHeader("sbHeader", Direction = SoapHeaderDirection.In, Required = true)]
        [SoapDocumentMethod("http://ilab.mit.edu/DeleteClientItem", Binding = "IBatchSB",
            RequestNamespace = "http://ilab.mit.edu", ResponseNamespace = "http://ilab.mit.edu")]
        public void DeleteClientItem(string name)
        {
            //first try to recreate session if using an html client
            if (Session == null || (Session["UserID"] == null) || (Session["UserID"].ToString() == ""))
            {
                if (sbHeader != null)
                wrapper.SetServiceSession(new Coupon(ProcessAgentDB.ServiceGuid, sbHeader.couponID, sbHeader.couponPassKey));
            }
            int userID = Convert.ToInt32(Session["UserID"]);
            int clientID = Convert.ToInt32(Session["ClientID"]);

            DataStorageAPI.RemoveClientItems(clientID, userID, new string[] { name });
        }



        /// <summary>
        /// Enumerates the names of all client items in the user's opaque data store.
        /// </summary>
        /// <returns>An array of client items.</returns>
        /// <remarks>Web Method</remarks>
        [WebMethod(Description = "Enumerates the names of all client items in the user's opaque data store", EnableSession = true)]
        [SoapHeader("sbHeader", Direction = SoapHeaderDirection.In, Required = true)]
        [SoapDocumentMethod("http://ilab.mit.edu/ListAllClientItems", Binding = "IBatchSB",
            RequestNamespace = "http://ilab.mit.edu", ResponseNamespace = "http://ilab.mit.edu")]
        public string[] ListAllClientItems()
        { //first try to recreate session if using an html client
            if (Session == null || (Session["UserID"] == null) || (Session["UserID"].ToString() == ""))
            {
                if (sbHeader != null)
                wrapper.SetServiceSession(new Coupon(ProcessAgentDB.ServiceGuid, sbHeader.couponID, sbHeader.couponPassKey));
            }
            int userID = Convert.ToInt32(Session["UserID"]);
            int clientID = Convert.ToInt32(Session["ClientID"]);

            return DataStorageAPI.ListClientItems(clientID, userID);
        }

        
        //////////////////////////////////////////////////////
		///// New methods added for David - CV, 8/5/2005 /////
		/////////////////////////////////////////////////////

		/// <summary>
		/// Retrieves a previously saved experiment specification.
		/// </summary>
		/// <param name="experimentID">A token which identifies the experiment.</param>
		/// <returns> The experimentSpecification, a domain-dependent experiment specification originally created by the Lab Client.</returns>
		/// <remarks>Web Method</remarks>
		[WebMethod(Description="Retrieves a previously saved experiment specification", EnableSession = true)]
		[SoapHeader("sbHeader", Direction=SoapHeaderDirection.In)]
        [SoapDocumentMethod(Action = "http://ilab.mit.edu/RetrieveSpecification", Binding = "IBatchSB", RequestNamespace = "http://ilab.mit.edu", ResponseNamespace = "http://ilab.mit.edu")]
		public string RetrieveSpecification (int experimentID)
		{
			try
			{
				//first try to recreate session if using a html client
                if (Session == null || (Session["UserID"] == null) || (Session["UserID"].ToString() == ""))
                {
                    if (sbHeader != null)
                    wrapper.SetServiceSession(new Coupon(ProcessAgentDB.ServiceGuid, sbHeader.couponID, sbHeader.couponPassKey));
                }
				Criterion[] carray = new Criterion[] {new Criterion("record_type", "=", BatchRecordType.SPECIFICATION) };
                ExperimentRecord[] records = brokerDB.RetrieveExperimentRecords(experimentID, carray);
                if ((records != null) && (records.Length >= 1) && (records[0].contents != null) && (records[0].contents.Length > 0))
                {
                    return records[0].contents;
                }
                else
                {
                    throw new SoapException("Experiment specification does not exist.", SoapException.ServerFaultCode);
                }
			}
			catch
			{
				throw;
			}
		}
        
                /// <summary>
                /// Retrieves the result from a previously executed experiment.
                /// </summary>
                /// <param name="experimentID">A token which identifies the experiment.</param>
                /// <returns> The experimentResult, an opaque string generated by the Lab Server to describe the result of a previously executed experiment.</returns>
                /// <remarks>Web Method</remarks>
        [WebMethod(Description = "Retrieves the result from a previously executed experiment", EnableSession = true)]
        [SoapHeader("sbHeader", Direction = SoapHeaderDirection.In)]
        [SoapDocumentMethod(Action = "http://ilab.mit.edu/RetrieveExperimentResult", Binding = "IBatchSB", RequestNamespace = "http://ilab.mit.edu", ResponseNamespace = "http://ilab.mit.edu")]
        public string RetrieveExperimentResult(int experimentID)
        {
            try
            {
                //first try to recreate session if using a html client
                if (Session == null || (Session["UserID"] == null) || (Session["UserID"].ToString() == ""))
                {
                    if (sbHeader != null)
                        wrapper.SetServiceSession(new Coupon(ProcessAgentDB.ServiceGuid, sbHeader.couponID, sbHeader.couponPassKey));
                }
                //Criterion result = new Criterion("type", "=", BatchRecordType.RESULT);
                Criterion[] carray = new Criterion[] { new Criterion("record_type", "=", BatchRecordType.RESULT) };
                ExperimentRecord[] records = brokerDB.RetrieveExperimentRecords(experimentID, carray);
                if ((records != null) && (records.Length >= 1) && (records[0].contents != null) && (records[0].contents.Length > 0))
                {
                    return records[0].contents;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        
        /// <summary>
		/// Retrieves a previously saved lab configuration for a particular experiment.
		/// </summary>
		/// <param name="experimentID">A token which identifies the experiment.</param>
		/// <returns> The labConfiguration, an opaque string included in the ResultReport by the Lab Server to specify the configuration in which an experiment is executed.</returns>
		/// <remarks>Web Method</remarks>
		[WebMethod(Description="Retrieves a previously saved lab configuration for a particular experiment", EnableSession = true)]
		[SoapHeader("sbHeader", Direction=SoapHeaderDirection.In)]
        [SoapDocumentMethod(Action = "http://ilab.mit.edu/RetrieveLabConfiguration", Binding = "IBatchSB", RequestNamespace = "http://ilab.mit.edu", ResponseNamespace = "http://ilab.mit.edu")]
		public string RetrieveLabConfiguration (int experimentID)
		{
			try
			{
				//first try to recreate session if using a html client
                if (Session == null || (Session["UserID"] == null) || (Session["UserID"].ToString() == ""))
                {
                    if (sbHeader != null)
                        wrapper.SetServiceSession(new Coupon(ProcessAgentDB.ServiceGuid, sbHeader.couponID, sbHeader.couponPassKey));
                }
				Criterion[] carray = new Criterion[] {new Criterion("record_type", "=", BatchRecordType.LAB_CONFIGURATION) };
                ExperimentRecord[] records = brokerDB.RetrieveExperimentRecords(experimentID, carray);
                if ((records != null) && (records.Length >= 1) && (records[0].contents != null) && (records[0].contents.Length > 0))
                {
                    return records[0].contents;
                }
                else
                {
                    throw new SoapException("Lab configuration does not exist.", SoapException.ServerFaultCode);
                }
			}
			catch (AccessDeniedException ade)
			{
				throw new SoapException(ade.Message+". "+ade.GetBaseException(), SoapException.ServerFaultCode,ade);
			}
			catch (Exception ex)
			{
				throw new SoapException(ex.Message+". "+ex.GetBaseException(), SoapException.ServerFaultCode,ex);
			}
		}

		/// <summary>
		/// Saves or modifies an optional user defined annotation to the experiment record.
		/// </summary>
		/// <param name="experimentID">A token which identifies the experiment.</param>
		/// <param name="annotation">The annotation to be saved with the experiment.</param>
		/// <returns>The previous annotation or null if there wasn't one.</returns>
		/// <remarks>Web Method</remarks>
		[WebMethod(Description="Saves or modifies an optional user defined annotation to the experiment record.", EnableSession = true)]
		[SoapHeader("sbHeader", Direction=SoapHeaderDirection.In)]
        [SoapDocumentMethod(Action = "http://ilab.mit.edu/SaveAnnotation", Binding = "IBatchSB", RequestNamespace = "http://ilab.mit.edu", ResponseNamespace = "http://ilab.mit.edu")]
		public string SaveAnnotation(int experimentID, string annotation)
		{
			try
			{
				//first try to recreate session if using a html client
                if (Session == null || (Session["UserID"] == null) || (Session["UserID"].ToString() == ""))
                {
                    if (sbHeader != null)
                        wrapper.SetServiceSession(new Coupon(ProcessAgentDB.ServiceGuid, sbHeader.couponID, sbHeader.couponPassKey));
                }


				return wrapper.SaveExperimentAnnotationWrapper(experimentID, annotation);
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Retrieves a previously saved experiment annotation.
		/// </summary>
		/// <param name="experimentID">A token which identifies the experiment.</param>
		/// <returns>The annotation, a string originally created by the user via the Lab Client.</returns>
		/// <remarks>Web Method</remarks>
		[WebMethod(Description="Retrieves a previously saved experiment annotation.", EnableSession = true)]
		[SoapHeader("sbHeader", Direction=SoapHeaderDirection.In)]
        [SoapDocumentMethod(Action = "http://ilab.mit.edu/RetrieveAnnotation", Binding = "IBatchSB", RequestNamespace = "http://ilab.mit.edu", ResponseNamespace = "http://ilab.mit.edu")]
		public string RetrieveAnnotation(int experimentID)
		{
			try
			{
				//first try to recreate session if using a html client
				if (Session==null||(Session["UserID"]==null)||(Session["UserID"].ToString()=="")){
                    if(sbHeader != null)
					    wrapper.SetServiceSession(new Coupon(ProcessAgentDB.ServiceGuid, sbHeader.couponID,sbHeader.couponPassKey));
                }
				return wrapper.SelectExperimentAnnotationWrapper(experimentID);
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Retrieves experiment metadata for experiments specified by an array of experiment IDs.
		/// </summary>
		/// <param name="experimentIDs">An array of tokens which identify experiments</param>
		/// <returns>An array of instances of the specified ExperimentInformation objects.</returns>
		/// <remarks>Web Method</remarks>
        [WebMethod(Description = "Retrieves experiment metadata for experiments specified by an array of experiment IDs.", EnableSession = true)]
        [SoapHeader("sbHeader", Direction = SoapHeaderDirection.In)]
        [SoapDocumentMethod(Action = "http://ilab.mit.edu/GetExperimentInformation", Binding = "IBatchSB", RequestNamespace = "http://ilab.mit.edu", ResponseNamespace = "http://ilab.mit.edu")]
        public ExperimentInformation[] GetExperimentInformation(int[] experimentIDs)
        {
                //first try to recreate session if using a html client
            if (Session == null || (Session["UserID"] == null) || (Session["UserID"].ToString() == ""))
            {
                if (sbHeader != null)
                    wrapper.SetServiceSession(new Coupon(ProcessAgentDB.ServiceGuid, sbHeader.couponID, sbHeader.couponPassKey));
            }
            return BatchAPI.GetExperimentInformation(experimentIDs);
        }

		/////////////////////////////////////
		///    Local Helper Methods       ///
		////////////////////////////////////


 
		/// <summary>
		/// Checks if the current session's user has permission to access the labserver.
		/// An access denied exception is thrown if the authorization check fails;
		/// otherwise the soap header for calls to the lab server is set to allow the SB to access the lab server
		/// </summary>
		/// <param name="labServerID"></param>
        public void CheckAndSetLSAuthorization(string labServerGuid)
        {
            ProcessAgentInfo info = brokerDB.GetProcessAgentInfo(labServerGuid);
            if (info.retired)
            {
                throw new Exception("The Batch Lab Server is retired");
            }
            CheckAndSetLSAuthorization(info.agentId);
        }
			
        /// <summary>
        /// Checks if the current session's user has permission to access the labserver.
        /// An access denied exception is thrown if the authorization check fails;
        /// otherwise the soap header for calls to the lab server is set to allow the SB to access the lab server
        /// </summary>
        /// <param name="labServerID"></param>
        public void CheckAndSetLSAuthorization(int labServerID)
        {
            try
            {
                /* Collecting information to check for authorization */

                //first try to recreate session if using a html client
                if (Session == null || (Session["UserID"] == null) || (Session["UserID"].ToString() == ""))
                {
                    if (sbHeader != null)
                    {
                        wrapper.SetServiceSession(new Coupon(ProcessAgentDB.ServiceGuid, sbHeader.couponID, sbHeader.couponPassKey));
                    }
                    else
                    {
                        HttpCookie authCookie = Context.Request.Cookies[ConfigurationManager.AppSettings["isbAuthCookieName"]];
                        if (authCookie != null)
                        {
                            if (authCookie.Value != null)
                            {
                                long sid = Convert.ToInt64(authCookie.Value);
                                wrapper.SetServiceSession(sid);
                            }
                        }
                    }
                }

                //retrieve userID from the session
                int userID = Convert.ToInt32(Session["UserID"]);
                int groupID = Convert.ToInt32(Session["GroupID"]);
                ProcessAgentInfo info = brokerDB.GetProcessAgentInfo(labServerID);
                if (info.retired)
                {
                    throw new Exception("The ProcessAgent is retired");
                }
                // get qualifier ID of labServer
                int qualifierID = AuthorizationAPI.GetQualifierID(info.agentId, Qualifier.labServerQualifierTypeID);

                /* End collecting information */

                // Checking if user has permission to use the lab server
                if (!AuthorizationAPI.CheckAuthorization(groupID, Function.useLabServerFunctionType, qualifierID))
                {
                    // check fails

                    throw new AccessDeniedException("Access denied using labServer '" + info.agentName + "'.");
                }
                else
                {
                    // set soap header authen values
                    batchLS_Proxy.AuthHeaderValue = new AuthHeader();
                    batchLS_Proxy.AuthHeaderValue.identifier = ProcessAgentDB.ServiceGuid;
                    batchLS_Proxy.AuthHeaderValue.passKey = info.identOut.passkey;
                    batchLS_Proxy.Url = info.webServiceUrl;
                }
            }
            catch
            {
                throw;
            }
            
        }
	}




}
