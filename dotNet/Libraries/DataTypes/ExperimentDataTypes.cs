using System;

namespace iLabs.DataTypes.ExperimentDataTypes
{
	////////////////////////////////////////
	// WaitEstimate                       //
	////////////////////////////////////////
	/// <summary>
	/// Information regarding the length of time before a submitted experiment will be executed.
	/// This is the structure returned by the GetEffectiveQueueLength method.
	/// </summary>
	[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://ilab.mit.edu/iLabs/type")]
	public class WaitEstimate
	{
		/// <summary>
		/// Number of experiments currently in the execution queue that would run before the hypothetical new experiment.
		/// </summary>
		public int effectiveQueueLength;
        
		/// <summary>
		/// [OPTIONAL, < 0 if not supported]. Estimated wait (in seconds) until the hypothetical new experiment would begin, based on the other experiments currently in the execution queue.
		/// </summary>
		public double estWait;
	}

	////////////////////////////////////////
	// ExperimentInformation              //
	////////////////////////////////////////
	/// <summary>
	/// The transparent ExperimentInformation object associated with each experiment record contains 
	/// all the information about the experiment that is accessible to the Service Broker. 
	/// Most of the fields of the ExperimentInformation class are fixed, for example experimentID, effectiveGroupID, userID, etc. 
	/// They are permanently set by the time the lab server executes the experiment. 
	/// The Service Broker can, however, change the annotation field for the client after experiment execution. 
	/// This allows the user to supply a string title or caption to a particular experiment.
	/// </summary>
	[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://ilab.mit.edu/iLabs/type")]
	public class ExperimentInformation
	{
		/// <summary>
		/// A token identifying the experiment.
		/// </summary>
		public long experimentID;

		/// <remarks/>
		public int labServerID;

		/// <remarks/>
		public int userID;

		/// <remarks/>
		public int effectiveGroupID;

		/// <summary>
		/// Date/Time the experiment was submitted.
		/// </summary>
		public System.DateTime submissionTime;
        
		/// <summary>
		/// Date/Time the experiment was completed.
		/// </summary>
		public System.DateTime completionTime;
        
		/// <summary>
		/// 
		/// </summary>
		public System.DateTime expirationTime;
        
		/// <summary>
		/// Guaranteed minimum remaining time (in hours) before this experiment's data will be purged from the lab server. 
		/// </summary>
		public double minTimeToLive;
        
		/// <summary>
		/// Indicates a requested priority for the experiment.
		/// Possible values range from 20 (highest priority) to -20 (lowest priority); 0 is normal.
		/// Priority hints may or may not be considered by the lab server.
		/// </summary>
		public int priorityHint;
        
		/// <summary>
		/// Indicates the status of this experiment.  
		/// 1: if waiting in the execution queue
		/// 2: if currently running
		/// 3: if terminated normally
		/// 4: if terminated with errors (this includes cancellation by user in mid-execution)
		/// 5: if cancelled by user before execution had begun
		/// 6: if unknown labExperimentID. 
		/// </summary>
		public int statusCode;
        
		/// <summary>
		/// Domain-dependent human-readable text containing non-fatal warnings about the experiment.
		/// </summary>
		public string[] validationWarningMessages;
        
		/// <summary>
		/// Domain-dependent human-readable text describing why the experiment specification would not be accepted.
		/// Used only in event that the experiment is not accepted by the Lab Server.
		/// </summary>
		public string validationErrorMessage;
        
		/// <summary>
		/// Domain-dependent human-readable text containing non-fatal warnings about the experiment including runtime warnings. 
		/// </summary>
		public string[] executionWarningMessages;
        
		/// <summary>
		/// [REQUIRED if statusCode == 4]. Domain-dependent human-readable text describing why the experiment terminated abnormally including runtime errors.
		/// </summary>
		public string executionErrorMessage;
        
		/// <summary>
		/// Allows the user to apply a string title or caption to a particular experiment
		/// </summary>
		public string annotation;
        
		/// <summary>
		/// Transparent XML string that defines a set of attribute-value pairs that are set by the Lab Server and read by the Service Broker.
		/// This field should conform to XML Schemas defined by the lab server for a particular type of experiment. 
		/// The intent is to allow searches for experiments and blobs by attribute, but this capability is not currently implemented.
		/// </summary>
		/// <preliminary/>
		public string xmlResultExtension;
        
		/// <summary>
		/// Transparent XML string that defines a set of attribute-value pairs that are set by the Lab Server and read by the Service Broker.
		/// This field should conform to XML Schemas defined by the lab server for a particular type of experiment. 
		/// The intent is to allow searches for experiments and blobs by attribute, but this capability is not currently implemented.
		/// </summary>
		/// <preliminary/>
		public string xmlBlobExtension;
	}
	////////////////////////////////////////
	// ValidationReport                   //
	////////////////////////////////////////
	/// <summary>
	/// A structure containing an evaluation from the Lab Server on the validity of the submitted experiment specification.
	/// </summary>
	[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://ilab.mit.edu/iLabs/type")]
	public class ValidationReport
	{
		/// <summary>
		/// true if the experiment specification would be (is) accepted for execution. 
		/// </summary>
		public bool accepted;
        
		/// <summary>
		/// Domain-dependent human-readable text containing non-fatal warnings about the experiment.
		/// </summary>
		public string[] warningMessages;
        
		/// <summary>
		/// [If accepted == false]. Domain-dependent human-readable text describing why the experiment specification would not be accepted.
		/// </summary>
		public string errorMessage;
        
		/// <summary>
		/// [OPTIONAL, < 0 if not supported]. Estimated runtime (in seconds of this experiment.
		/// </summary>
		public double estRuntime;
	}
	////////////////////////////////////////
	// ResultReport                       //
	////////////////////////////////////////
	/// <summary>
	/// Contains the results from (or errors generated by) a previously submitted experiment.
	/// </summary>
	[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://ilab.mit.edu/iLabs/type")]
	public class ResultReport
	{
		/// <summary>
		/// Indicates the status of this experiment.  
		/// 1: if waiting in the execution queue
		/// 2: if currently running
		/// 3: if terminated normally
		/// 4: if terminated with errors (this includes cancellation by user in mid-execution)
		/// 5: if cancelled by user before execution had begun
		/// 6: if unknown labExperimentID. 
		/// </summary>
		public int statusCode;
        
		/// <summary>
		/// [REQUIRED if experimentStatus == 3, 
		/// OPTIONAL if experimentStatus == 4].
		/// An opaque, domain-dependent set of experiment results.
		/// </summary>
		public string experimentResults;
        
		/// <summary>
		/// [OPTIONAL, null if unused]. A transparent XML string that helps to identify this experiment.  Used for indexing and querying in generic components which can't understand the opaque experimentSpecification and experimentResults.
		/// </summary>
		public string xmlResultExtension;
        
		/// <summary>
		/// [OPTIONAL, null if unused]. A transparent XML string that helps to identify any blobs saved as part of this experiment's results. 
		/// </summary>
		public string xmlBlobExtension;
        
		/// <summary>
		/// Domain-dependent human-readable text containing non-fatal warnings about the experiment including runtime warnings. 
		/// </summary>
		public string[] warningMessages;
        
		/// <summary>
		/// [REQUIRED if experimentStatus == 4]. Domain-dependent human-readable text describing why the experiment terminated abnormally including runtime errors.
		/// </summary>
		public string errorMessage;
	}


	////////////////////////////////////////
	// LabExperimentStatus                //
	////////////////////////////////////////
	/// <summary>
	/// Contains information on the status of a previously submitted experiment.
	/// </summary>
	/// <seealso cref="ExperimentStatus">ExperimentStatus Structure</seealso>
	[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://ilab.mit.edu/iLabs/type")]
	public class LabExperimentStatus
	{
		/// <summary>
		/// An instance of an ExperimentStatus class which indicates the status of this experiment.
		/// </summary>
		/// <seealso cref="ExperimentStatus">ExperimentStatus Structure</seealso>
		public ExperimentStatus statusReport;
        
		/// <summary>
		/// Guaranteed minimum remaining time (in hours) before this labExperimentID and associated data will be purged from the lab server. 
		/// </summary>
		public double minTimetoLive;
	}
	////////////////////////////////////////
	// ExperimentStatus                   //
	////////////////////////////////////////
	/// <summary>
	/// Contains information regarding the status of an experiment. 
	/// </summary>
	/// <seealso cref="WaitEstimate">WaitEstimate Structure</seealso>
	[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://ilab.mit.edu/iLabs/type")]
	public class ExperimentStatus
	{
		/// <summary>
		/// 1: if waiting in the execution queue
		/// 2: if currently running
		/// 3: if terminated normally
		/// 4: if terminated with errors (this includes cancellation by user in mid-execution)
		/// 5: if cancelled by user before execution had begun
		/// 6: if unknown experimentID 
		/// </summary>
		public int statusCode;
        
		/// <summary>
		/// An instance of a WaitEstimate class containing the estimated wait time before this experiment will execute.
		/// </summary>
		/// <seealso cref="WaitEstimate">WaitEstimate Structure</seealso>
		public WaitEstimate wait;
        
		/// <summary>
		/// [OPTIONAL <0 if not used]. Estimated runtime (in seconds) of this experiment.
		/// </summary>
		public double estRuntime;
        
		/// <summary>
		/// [OPTIONAL <0 if not used]. Estimated remaining runtime (in seconds) of this experiment, if the experiment is currently running.
		/// </summary>
		public double estRemainingRuntime;
	}
	////////////////////////////////////////
	// SubmissionReport                   //
	////////////////////////////////////////
	/// <summary>
	/// A report returned to the Service Broker from the Lab Server containing information about a submitted experiment.
	/// </summary>
	/// <seealso cref="ValidationReport">ValidationReport Structure</seealso>
	/// <seealso cref="WaitEstimate">WaitEstimate Structure</seealso>
	[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://ilab.mit.edu/iLabs/type")]
	public class SubmissionReport
	{
		/// <summary>
		/// An instance of a ValidationReport class containing an evaluation from the Lab Server on the validity of the submitted experiment specification.
		/// </summary>
		/// <seealso cref="ValidationReport">ValidationReport Structure</seealso>
		public ValidationReport vReport;
        
		/// <summary>
		/// A token that identifies the experiment.
		/// </summary>
		public long labExperimentID;		//Imad: Renamed experimentID
        
		/// <summary>
		/// Guaranteed minimum time (in hours, starting now) before this experimentID and associated data will be purged from the lab server.
		/// </summary>
		public double minTimetoLive;
        
		/// <summary>
		/// An instance of a WaitEstimate class.
		/// Contains information regarding the length of time before a submitted experiment will be executed.
		/// </summary>
		/// <seealso cref="WaitEstimate">WaitEstimate Structure</seealso>
		public WaitEstimate wait;
	}
	
	////////////////////////////////////////
	// LabStatus                          //
	////////////////////////////////////////
	/// <summary>
	/// Provides information regarding the status of a Lab Server, 
	/// such as whether or not it is online and accepting experiments.
	/// </summary>
	[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://ilab.mit.edu/iLabs/type")]
	public class LabStatus
	{
		/// <summary>
		/// true if lab is accepting experiments.
		/// </summary>
		public Boolean online;

		/// <summary>
		/// Domain-dependent human-readable text describing status of lab server.
		/// </summary>
		public String labStatusMessage;
	}

 
}
