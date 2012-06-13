using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Text;
using System.Web.Mail;

using iLabs.DataTypes;
using iLabs.DataTypes.SchedulingTypes;
using iLabs.DataTypes.SoapHeaderTypes;
using iLabs.Core;
using iLabs.UtilLib;

using iLabs.Proxies.USS;

namespace iLabs.Scheduling.LabSide
{


    #region LSS API Classes
    /*
    /// <summary>
    /// a structure which holds time block
    /// </summary>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://ilab.mit.edu/iLabs/type")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://ilab.mit.edu/iLabs/type", IsNullable = false)]
    public class TimeBlockInfo
    {
        /// <summary>
        /// the ID of the time block
        /// </summary>
        public int timeBlockId;
        /// <summary>
        /// the resourceID , the LabServer and resource the time block was assigned to. May not be needed see recurrenceID.
        /// </summary>
        public int resourceId;
        /// <summary>
        /// the start time of the time block, in UTC
        /// </summary>
        public DateTime startTime;
        /// <summary>
        /// the end time of the time block, in UTC
        /// </summary>
        public DateTime endTime;
        /// <summary>
        /// the GUID of the lab server that the time block belongs to. May not be needed see recurrenceID.
        /// </summary>
        public String labServerGuid;
        /// <summary>
        /// the ID of the recurrence that the time block belongs to
        /// </summary>
        public int recurrenceID;
    }
*/

    /// <summary>
    /// a structure which holds experiment information
    /// </summary>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://ilab.mit.edu/iLabs/type")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://ilab.mit.edu/iLabs/type", IsNullable = false)]
    public class LssExperimentInfo
    {
        /// <summary>
        /// the ID of the experiment information
        /// </summary>
        public int experimentInfoId;
        /// <summary>
        /// the GUID of the lab Client 
        /// </summary>
        public string labClientGuid;
        /// <summary>
        /// the GUID of the lab server which provide the experiment
        /// </summary>
        public string labServerGuid;
        /// <summary>
        /// the Name of the lab server which provide the experiment
        /// </summary>
        public string labServerName;
        /// <summary>
        /// the name of the lab client through which the experiment can be executed
        /// </summary>
        public string labClientName;
        /// <summary>
        /// the version of the lab client through which the experiment can be executed
        /// </summary>
        public string labClientVersion;
        /// <summary>
        /// the name of the provider of the experiment
        /// </summary>
        public string providerName;
        public string contactEmail;
    
        /// <summary>
        /// the start up time needed for the execution of the experiment
        /// </summary>
        public int prepareTime;
        /// <summary>
        /// the cool down time needed after the execution of the experiment
        /// </summary>
        public int recoverTime;
        /// <summary>
        /// the experiment's minimum exection time
        /// </summary>
        public int minimumTime;
        /// <summary>
        /// the maxium time the user is allowed to arrive before the excution time of the experiment 
        /// </summary>
        public int earlyArriveTime;
    }
    /// <summary>
    /// a structure which holds Lab server side policy
    /// </summary>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://ilab.mit.edu/iLabs/type")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://ilab.mit.edu/iLabs/type", IsNullable = false)]
    public class LSSPolicy
    {
        /// <summary>
        /// the ID of the lab side scheduling server policy
        /// </summary>
        public int lssPolicyId;
        /// <summary>
        /// the ID of the credential set, the goup with which should obey this policy
        /// </summary>
        public int credentialSetId;
        /// <summary>
        /// the rule
        /// </summary>
        public string rule;
        /// <summary>
        /// the ID of the information of the experiment which the policy is applied to
        /// </summary>
        public int experimentInfoId;
    }

    public class LSResource{
        public int resourceID;
        public string labServerGuid;
        public string labServerName;
        public string description;
    }
    /// <summary>
    /// a structure which holds a permitted experiment for a time block
    /// </summary>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://ilab.mit.edu/iLabs/type")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://ilab.mit.edu/iLabs/type", IsNullable = false)]
    public class PermittedExperiment
    {
        /// <summary>
        /// the ID of the permission
        /// </summary>
        public int permittedExperimentId;
        /// <summary>
        /// the ID of the informaiton of the experiment which is permitted to be executed 
        /// </summary>
        public int experimentInfoId;
        /// <summary>
        /// the ID of the recurrence whose permission is given to the experiment
        /// </summary>
        public int recurrenceId;
    }

    /// <summary>
    /// a structure which holds user side scheduling information
    /// </summary>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://ilab.mit.edu/iLabs/type")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://ilab.mit.edu/iLabs/type", IsNullable = false)]
    public class USSInfo
    {
        /// <summary>
        /// the ID of the user side scheduling server information
        /// </summary>
        public int ussInfoId;
        public long revokeCouponId;
        public string domainGuid;
        /// <summary>
        /// the GUID of the user side scheduling server
        /// </summary>
        public string ussGuid;
        /// <summary>
        /// the name of the user side scheduling server
        /// </summary>
        public string ussName;
        /// <summary>
        ///  the URL of the user side scheduling server
        /// </summary>
        public string ussUrl;
        
    }

    /// <summary>
    /// a structure which holds credential set
    /// </summary>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://ilab.mit.edu/iLabs/type")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://ilab.mit.edu/iLabs/type", IsNullable = false)]
    public class LssCredentialSet
    {
        /// <summary>
        /// the ID of the credential set
        /// </summary>
        public int credentialSetId;
        /// <summary>
        /// the GUID of the service broker whose domain the group belongs to
        /// </summary>
        public string serviceBrokerGuid;
        /// <summary>
        /// the Name of the service broker whose domain the group belongs to
        /// </summary>
        public string serviceBrokerName;
        /// <summary>
        /// the Name of the goup with this credential set
        /// </summary>
        public string groupName;
       
    }
    /// <summary>
    /// a structure which holds reservation information
    /// </summary>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://ilab.mit.edu/iLabs/type")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://ilab.mit.edu/iLabs/type", IsNullable = false)]
    public class ReservationInfo : ITimeBlock
    {
        /// <summary>
        /// the ID of the reservation information
        /// </summary>
        public int reservationInfoId;
        /// <summary>
        /// the ID of the resource which is reserved for execution
        /// </summary>
        public int resourceId;
        public int experimentInfoId;
        /// <summary>
        /// the ID of the credentialSet. the user from the group with this credential set made the reservation
        /// </summary>
        public int credentialSetId;
        public int ussId;
        /// <summary>
        /// the start time of the reservation
        /// </summary>
        public DateTime startTime;
        /// <summary>
        /// the end time of the reservation
        /// </summary>
        public DateTime endTime;
        public int statusCode;

        public int CompareTo(object that)
        {
            return CompareTo((ITimeBlock)that);
        }

        public int CompareTo(ITimeBlock b)
        {
            int status = 0;
            if (Start > b.Start)
            {
                status = 1;
            }
            else if (Start < b.Start)
            {
                status = -1;
            }
            else if (Duration > b.Duration)
            {
                status = 1;
            }
            else if (Duration < b.Duration)
            {
                status = -1;
            }
            return status;
        }

        public DateTime Start
        {
            get
            {
                return startTime;
            }
        }

        public DateTime End
        {
            get
            {
                return endTime;
            }
        }
        public int Duration
        {
            get
            {
                return (int)(endTime - startTime).TotalSeconds;
            }
        }
        public bool Intersects(ITimeBlock target)
        {
            return (this.Start < target.End && this.End > target.Start);
        }

        public TimeBlock Intersection(ITimeBlock target)
        {
            if (Intersects(target))
            {
                DateTime start = (Start > target.Start) ? Start : target.Start;
                DateTime end = (End < target.End) ? End : target.End;
                return new TimeBlock(start, end);
            }
            else
                return null;
        }
       
    }
    /// <summary>
    /// Defines a view of a reservation for efficient revoking of reservations.
    /// </summary>
    public class ReservationData : TimeBlock
    {
        //R.Reservation_Info_ID,R.Start_Time, R.End_Time,
        //E.Lab_Client_GUID,E.Lab_Server_GUID, C.Group_Name,C.Service_Broker_Guid,R.USS_Info_ID,R.status
        public int reservationID;
        public int ussId;
        public int status;
        //public DateTime start;
        //public DateTime end;
        public string clientGuid;
        public string labServerGuid;
        public string groupName;
        public string sbGuid;

        public ReservationData(DateTime start, DateTime end)
            : base(start, end)
        { }
    }

#endregion
}