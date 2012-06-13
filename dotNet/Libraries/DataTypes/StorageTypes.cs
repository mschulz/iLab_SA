using System;
using System.Collections.Generic;
using System.Text;

namespace iLabs.DataTypes.StorageTypes
{

    
    /// <summary>
    /// A single record of an experiment.
    /// </summary>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://ilab.mit.edu/iLabs/type")]
    public class ExperimentRecord
    {
        /// <summary>
        /// A tag to identify the type of content represented by a record
        /// </summary>
        public string type;

        /// <summary>
        /// optional source of the record
        /// </summary>
        public string submitter;    

        /// <summary>
        /// The ordinal number of this record (starting with 0)
        /// </summary>
        public int sequenceNum;

        /// <summary>
        /// A timestamp issued by the ESS when the record is added
        /// </summary>
        public DateTime timestamp;

        /// <summary>
        /// True if the record's contents field is XML encoded and the contained attributes may be searched
        /// </summary>
        public bool xmlSearchable;

        /// <summary>
        /// The payload of this record
        /// </summary>
        public string contents;
    }

    /// <summary>
    /// a collection of all experiment records for a single experiment.
    /// </summary>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://ilab.mit.edu/iLabs/type")]
    public class Experiment
    {
        /// <summary>
        /// A unique non-negative long that identifies this experiment on the issueing ServiceBroker
        /// </summary>
        public long experimentId;

        /// <summary>
        /// The issueing ServiceBroker
        /// </summary>
        public string issuerGuid;

        /// <summary>
        /// The array of records that represents the experiment log
        /// </summary>
        public ExperimentRecord[] records;
    }

    /// <summary>
    /// A ServiceBroker data type that models the ServiceBroker's Experiments 
    /// table and is designed to provide information about an experiment to requesting clients or services.
    /// </summary>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://ilab.mit.edu/iLabs/type")]
    public class ExperimentSummary
    {
        public long experimentId;
        public long duration;
        public DateTime scheduledStart;
        public DateTime creationTime;
        public DateTime closeTime;
        public int status;
        public int recordCount;
        public string essGuid;        
        public string serviceBrokerGuid;
        public string userName;
        public string groupName;
        public string labServerGuid;
        public string labServerName;
        public string clientName;
        public string clientVersion;
        public string annotation;

        public bool HasEss
        {
            get
            {
                return (essGuid != null && essGuid.Length > 0);
            }
        }
    }

    /// <summary>
    /// A ServiceBroker data type that models the ServiceBroker's Experiments table
    /// </summary>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://ilab.mit.edu/iLabs/type")]
    public class StorageStatus
    {
        public const int BATCH_QUEUED = 1; // if waiting in the execution queue
        public const int BATCH_RUNNING = 2; //if currently running
        public const int BATCH_TERMINATED = 3; // if terminated normally
        public const int BATCH_TERMINATED_ERROR = 4; // if terminated with errors (this includes cancellation by user in mid-execution)
        public const int BATCH_CANCELLED = 5; // if cancelled by user before execution had begun
        public const int BATCH_UNKNOWN = 6; // if unknown labExperimentID. 
        public const int BATCH_NOT_VALID = 7; // Assigned by Service Broker if experiment is not valid (done in submit call)
        public const int BATCH_MASK = 0xf;

        // This version has shifted the interactive status codes 4 bits, to make room for the Batch codes
        
        public const int UNKNOWN = 0x10;
        public const int INITIALIZED = 0x20;
        public const int OPEN = 0x80;
        public const int REOPENED = 0xc0;
        public const int RUNNING = 0x100;
        public const int CLOSED = 0x200;
        public const int TIMEOUT = 0x400;
        public const int USER_ACTION = 0x800;
        public const int ERROR = 0x1000;
        public const int INTERACTIVE_MASK = 0xfff0;
     
        public const int CLOSED_TIMEOUT = CLOSED | TIMEOUT;
        public const int CLOSED_USER = CLOSED | USER_ACTION;
        public const int CLOSED_ERROR = CLOSED | ERROR;

        public static string GetStatusString(int status)
        {
            string statStr = null;
            switch (status)
            {
                case StorageStatus.BATCH_QUEUED:
                    statStr = "Queued";
                    break;
                case StorageStatus.BATCH_CANCELLED:
                    statStr = "Cancelled";
                    break;
                case StorageStatus.BATCH_NOT_VALID:
                    statStr = "Not Valid";
                    break;
                case StorageStatus.UNKNOWN:
                case StorageStatus.BATCH_UNKNOWN:
                    statStr = "Unknown";
                    break;
                case StorageStatus.INITIALIZED:
                    statStr = "Initialized";
                    break;
                case StorageStatus.OPEN:
                    statStr = "Open";
                    break;
                case StorageStatus.REOPENED:
                    statStr = "Re-Opened";
                    break;
                case StorageStatus.BATCH_RUNNING:
                case StorageStatus.RUNNING:
                    statStr = "Running";
                    break;
                case StorageStatus.BATCH_TERMINATED_ERROR:
                case StorageStatus.CLOSED:
                    statStr = "Closed";
                    break;
                case StorageStatus.BATCH_TERMINATED:
                case StorageStatus.CLOSED_ERROR:
                    statStr = "Closed Error";
                    break;
                case StorageStatus.BATCH_CANCELLED | StorageStatus.CLOSED:
                    statStr = "Cancelled";
                    break;
                case StorageStatus.BATCH_TERMINATED_ERROR | StorageStatus.CLOSED:
                    statStr = "Closed Error";
                    break;
                case StorageStatus.BATCH_TERMINATED | StorageStatus.CLOSED_ERROR:
                    statStr = "Closed -Terminated";
                    break;
                case StorageStatus.CLOSED_TIMEOUT:
                    statStr = "Closed Timeout";
                    break;
                case StorageStatus.CLOSED_USER:
                    statStr = "Closed By User";
                    break;
                default:
                    if ((status & StorageStatus.CLOSED) == StorageStatus.CLOSED)
                        statStr = "Closed";
                    else
                        statStr = "StorageStatus Error";
                    break;
            }
            return statStr;
        }

        
        public long experimentId;
        public int status;
        public int recordCount;
        public DateTime creationTime;
        public DateTime closeTime;
        public DateTime lastModified;
        public string issuerGuid;
    }


    /// <summary>
    /// An optional attribute which may be added to an experiment record.
    /// </summary>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://ilab.mit.edu/iLabs/type")]
    public class RecordAttribute
    {
        /// <summary>
        /// The attribute name
        /// </summary>
        public string name;

        /// <summary>
        /// The attribute value
        /// </summary>
        public string value;
    }

    /// <summary>
    /// Byte Large Object, a class to manage BLOBs on the ESS.
    /// </summary>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://ilab.mit.edu/iLabs/type")]
    public class Blob
    {
        public enum eStatus : int
        {
            UNKNOWN = -1, NOT_REQUESTED = 0, REQUESTED, DOWNLOADING, CANCELLED, FAILED, CORRUPT, COMPLETE, DELETED
        }

        /// <summary>
        /// The unique ID of this BLOB
        /// </summary>
        public long blobId;

        /// <summary>
        /// The ID of the associated Experiment
        /// </summary>
        /// <remarks>A BLOB must always be associated with an Experiment whether or not it is associated with an ExperimentRecord</remarks>
        public long experimentId;

        /// <summary>
        /// The sequenceNumber of the associated Experiment record
        /// </summary>
        /// <remarks>A BLOB may be associated with an ExperimentRecord</remarks>
        public int recordNumber = -1;

        /// <summary>
        /// A timestamp issued by the ESS when the BLOB is created
        /// </summary>
        /// <remarks>It is independent of the time when the actual download of the BLOB data is requested or completed</remarks>
        public DateTime timestamp;

        /// <summary>
        /// A brief description suitable for listing in a table
        /// </summary>
        public string description;

        /// <summary>
        /// Optional mimeType
        /// </summary>
        public string mimeType;

        /// <summary>
        /// The length of the actual binary data in bytes
        /// </summary>
        public int byteCount;

        /// <summary>
        /// The string result of the checksum or hashing algorithm
        /// </summary>
        public string checksum;

        /// <summary>
        /// A string designation of a supported checksum algorithm
        /// </summary>
        /// <remarks>Implementations must support CRC32 at a minimum, but may also support other hashing 
        /// or checksum schemes e.g., MD5</remarks>
        public string checksumAlgorithm;

        public bool IsAssociated()
        {
            return recordNumber >= 0;
        }
    }

}
