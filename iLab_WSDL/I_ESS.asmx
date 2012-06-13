<%@ WebService Language="C#" Class="I_ESS" %>

/* $Id$ */

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

using iLabs.DataTypes;
using iLabs.DataTypes.StorageTypes;
using iLabs.DataTypes.TicketingTypes;
using iLabs.DataTypes.SoapHeaderTypes;


[XmlType(Namespace = "http://ilab.mit.edu/iLabs/type")]
[WebServiceBinding(Name = "IESS", Namespace = "http://ilab.mit.edu/iLabs/Services")]
[WebService(Name = "ExperimentStorageProxy", Namespace = "http://ilab.mit.edu/iLabs/Services",
    Description="The Experiment Storage Service (ESS) provides iLab services and clients the ability to save, access and query experiment results. "
    + " Experiments are always created by the ServiceBroker, and administrative information is managed by the ServiceBroker.")]
public abstract class I_ESS  : System.Web.Services.WebService {
   
    /// <summary>
    /// 
    /// </summary>
    public OperationAuthHeader opHeader = new OperationAuthHeader();
    public AgentAuthHeader agentAuthHeader = new AgentAuthHeader();

    ////////////////////////////////////////////////////
    ///       IESS Methods - Experiment methods      ///
    ////////////////////////////////////////////////////

/// <summary>
/// Notifies the ESS that it may close the experiment.
/// </summary>
/// <param name="experimentId">The ServiceBrokers experiment ID</param>
/// <returns>StorageStatus</returns>
    [WebMethod(Description = "Closes an Experiment on the ESS so that no further ExperimentRecords "
        + "or BLOBs can be written to it; BLOBs that have been created for this Experiment but "
        + "have not been associated with an ExperimentRecord are deleted by this method. "
        + " Should be called via the ServiceBroker.",
        EnableSession = true)]
    [SoapHeader("agentAuthHeader", Direction = SoapHeaderDirection.In)]
    [SoapDocumentMethod(Binding = "IESS")]
    public abstract StorageStatus CloseExperiment(long experimentId);

    [WebMethod(Description = "Deletes an experiment object and all its associated ExperimentRecords "
            + "and BLOBs on the ESS", EnableSession = true)]
    [SoapDocumentMethod(Binding = "IESS")]
    [SoapHeader("agentAuthHeader", Direction = SoapHeaderDirection.In)]
    public abstract bool DeleteExperiment(long experimentId);
    
    [WebMethod(Description = "Opens an Experiment on the ESS so that ExperimentRecords "
            + "and BLOBs can be written to it. Duration specifies the amount of time ( in seconds )"
           + " before the experiment may be automaticly closed. Should be called via the ServiceBroker.",
            EnableSession = true)]
    [SoapHeader("agentAuthHeader", Direction = SoapHeaderDirection.In)]
        [SoapDocumentMethod(Binding = "IESS")]
    public abstract StorageStatus OpenExperiment(long experimentId, long duration);

    [WebMethod(Description = "Returns the number of minutes since the last \"write action\" "
        + "to the Experiment", EnableSession = true)]
    [SoapDocumentMethod(Binding = "IESS")]
    [SoapHeader("opHeader", Direction = SoapHeaderDirection.In)]
    public abstract int GetIdleTime(long experimentId);
   
    [WebMethod(Description = "Gets the Experiment's status", EnableSession = true)]
    [SoapDocumentMethod(Binding = "IESS")]
    [SoapHeader("opHeader", Direction = SoapHeaderDirection.In)]
    public abstract StorageStatus GetExperimentStatus(long experimentId);


    [WebMethod(Description = "Sets the Experiment's status, returns the resulting status. StatusCode should be one of a defined set, depending on the status action may be performed on the experiment.", EnableSession = true)]
    [SoapDocumentMethod(Binding = "IESS")]
    [SoapHeader("opHeader", Direction = SoapHeaderDirection.In)]
    public abstract StorageStatus SetExperimentStatus(long experimentId, int statusCode);
    
    [WebMethod(Description = "Adds a new ExperimentRecord to a pre-existing Experiment object",
        EnableSession = true)]
    [SoapDocumentMethod(Binding = "IESS")]
    [SoapHeader("opHeader", Direction = SoapHeaderDirection.In)]
    public abstract int AddRecord(long experimentId, string submitter, string type, bool xmlSearchable,
        string contents, RecordAttribute[] attributes);

    [WebMethod(Description = "Adds ExperimentRecords to a pre-existing Experiment object",
       EnableSession = true)]
    [SoapDocumentMethod(Binding = "IESS")]
    [SoapHeader("opHeader", Direction = SoapHeaderDirection.In)]
    public abstract int AddRecords(long experimentId, ExperimentRecord[] records);
   

    [WebMethod(Description = "Returns the specified ExperimentRecord", EnableSession = true)]
    [SoapDocumentMethod(Binding = "IESS")]
    [SoapHeader("opHeader", Direction = SoapHeaderDirection.In)]
    public abstract ExperimentRecord GetRecord(long experimentId, int sequenceNum);

    [WebMethod(Description = "Returns the specified ExperimentRecords"
       + " Criterian are OR'ed together records returned match any criteria.", EnableSession = true)]
    [SoapDocumentMethod(Binding = "IESS")]
    [SoapHeader("opHeader", Direction = SoapHeaderDirection.In)]
    public abstract ExperimentRecord[] GetRecords(long experimentId, Criterion[] target);
    
    [WebMethod(Description = "Returns the specified Experiment including the array of associated "
        + "ExperimentRecords", EnableSession = true)]
    [SoapDocumentMethod(Binding = "IESS")]
    [SoapHeader("opHeader", Direction = SoapHeaderDirection.In)]
    public abstract Experiment GetExperiment(long experimentId);

    [WebMethod(Description = "Returns an array of ExperimentIDs from the specified set which match the criterian."
        + " Criterian are AND'ed together experiments returned must match all criteria.", EnableSession = true)]
    [SoapDocumentMethod(Binding = "IESS")]
    [SoapHeader("agentAuthHeader", Direction = SoapHeaderDirection.In)]
    public abstract long[] GetExperimentIDs(long[] expSet, Criterion[] filter);
    
    [WebMethod(Description = "Adds the specified RecordAttributes to an ExperimentRecord", EnableSession = true)]
    [SoapDocumentMethod(Binding = "IESS")]
    [SoapHeader("opHeader", Direction = SoapHeaderDirection.In)]
    public abstract int[] AddAttributes(long experimentId, int sequenceNum, RecordAttribute[] attributes);
    

    [WebMethod(Description = "Retrieves the specified RecordAttributes of an ExperimentRecord", EnableSession = true)]
    [SoapDocumentMethod(Binding = "IESS")]
    [SoapHeader("opHeader", Direction = SoapHeaderDirection.In)]
    public abstract RecordAttribute[] GetRecordAttributes(long experimentId, int sequenceNum, int[] attributeIds);
   
    [WebMethod(Description = "Retrieves the IDs of the specified RecordAttributes of an ExperimentRecord",
        EnableSession = true)]
    [SoapDocumentMethod(Binding = "IESS")]
    [SoapHeader("opHeader", Direction = SoapHeaderDirection.In)]
    public abstract RecordAttribute[] GetRecordAttributesByName(long experimentId, int sequenceNum, string attributeName);
   
    [WebMethod(Description = "Deletes the specified RecordAttributes of an ExperimentRecord", EnableSession = true)]
    [SoapDocumentMethod(Binding = "IESS")]
    [SoapHeader("opHeader", Direction = SoapHeaderDirection.In)]
    public abstract RecordAttribute[] DeleteRecordAttributes(long experimentId, int sequenceNum, int[] attributeIds);
  
    ////////////////////////////////////////////////////
    ///       IESS Methods - BLOB methods            ///
    ////////////////////////////////////////////////////

    [WebMethod(Description = "Associates the BLOB with the specified ExperimentRecord", EnableSession = true)]
    [SoapDocumentMethod(Binding = "IESS")]
    [SoapHeader("opHeader", Direction = SoapHeaderDirection.In)]
    public abstract bool AddBlobToRecord(long blobId, long experimentId, int sequenceNum);

    [WebMethod(Description = "Cancels the download of binary data associated with a BLOB or deletes "
      + "the corrupt data of an attempted download", EnableSession = true)]
    [SoapDocumentMethod(Binding = "IESS")]
    [SoapHeader("opHeader", Direction = SoapHeaderDirection.In)]
    public abstract int CancelBlobStorage(long blobId);

    [WebMethod(Description = "Creates a new BLOB record on the ESS", EnableSession = true)]
    [SoapDocumentMethod(Binding = "IESS")]
    [SoapHeader("opHeader", Direction = SoapHeaderDirection.In)]
    public abstract long CreateBlob(long experimentId, string description, int byteCount, string checksum, string checksumAlgorithm);

    [WebMethod(Description = "Reports on the association of the BLOB record", EnableSession = true)]
    [SoapDocumentMethod(Binding = "IESS")]
    [SoapHeader("opHeader", Direction = SoapHeaderDirection.In)]
    public abstract int GetBlobAssociation(long blobId);
    
    [WebMethod(Description = "Reports the Experiment that owns the BLOB", EnableSession = true)]
    [SoapDocumentMethod(Binding = "IESS")]
    [SoapHeader("opHeader", Direction = SoapHeaderDirection.In)]
    public abstract long GetBlobExperiment(long blobId);

    [WebMethod(Description = "Retrieves the BLOB records associated with a particular ExperimentRecord", EnableSession = true)]
    [SoapDocumentMethod(Binding = "IESS")]
    [SoapHeader("opHeader", Direction = SoapHeaderDirection.In)]
    public abstract Blob[] GetBlobs(long experimentId);

    [WebMethod(Description = "Retrieves the BLOB records associated with a particular ExperimentRecord", EnableSession = true)]
    [SoapDocumentMethod(Binding = "IESS")]
    [SoapHeader("opHeader", Direction = SoapHeaderDirection.In)]
    public abstract Blob[] GetBlobsForRecord(long experimentId, int sequenceNum);

    //Report on the download status of the BLOB record- incomplete ! need to store status code
    [WebMethod(Description = "Reports on the download status of the BLOB record", EnableSession = true)]
    [SoapDocumentMethod(Binding = "IESS")]
    [SoapHeader("opHeader", Direction = SoapHeaderDirection.In)]
    public abstract int GetBlobStatus(long blobId);
   


    [WebMethod(Description = "Lists the protocols that the ESS can use to retrieve a BLOB from a source", EnableSession = true)]
    [SoapDocumentMethod(Binding = "IESS")]
    [SoapHeader("opHeader", Direction = SoapHeaderDirection.In)]
    public abstract string[] GetSupportedBlobImportProtocols();


    [WebMethod(Description = "Lists the protocols that a process agent can use to retrieve a BLOB from "
        + "the ESS", EnableSession = true)]
    [SoapDocumentMethod(Binding = "IESS")]
    [SoapHeader("opHeader", Direction = SoapHeaderDirection.In)]
    public abstract string[] GetSupportedBlobExportProtocols();


    [WebMethod(Description = "Lists the checksum algorithms that a process agent can use to store a BLOB "
        + "on the ESS", EnableSession = true)]
    [SoapDocumentMethod(Binding = "IESS")]
    [SoapHeader("opHeader", Direction = SoapHeaderDirection.In)]
    public abstract string[] GetSupportedChecksumAlgorithms();



    [WebMethod(Description = "Requests a URL from which the specified BLOB data can be downloaded", EnableSession = true)]
    [SoapDocumentMethod(Binding = "IESS")]
    [SoapHeader("opHeader", Direction = SoapHeaderDirection.In)]
    public abstract string RequestBlobAccess(long blobId, string protocol, int duration);

    [WebMethod(EnableSession = true)]
    [SoapDocumentMethod(Binding = "IESS")]
    [SoapHeader("opHeader", Direction = SoapHeaderDirection.In)]
    public abstract bool RequestBlobStorage(long blobId, string blobUrl);
   

  
    
   
    


    
    
    
   

}

