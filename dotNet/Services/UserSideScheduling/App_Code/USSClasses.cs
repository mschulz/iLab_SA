using System;
using System.Data;

using iLabs.DataTypes.SchedulingTypes;

namespace iLabs.Scheduling.UserSide
{
    /// <remarks/>

    public class ReservationInfo
    {

        private int reservationIdField;

        private string userNameField;

        private int credentialSetIdField;

        private System.DateTime startTimeField;

        private System.DateTime endTimeField;

        private int experimentInfoIdField;

        /// <remarks/>
        public int reservationId
        {
            get
            {
                return this.reservationIdField;
            }
            set
            {
                this.reservationIdField = value;
            }
        }

        /// <remarks/>
        public string userName
        {
            get
            {
                return this.userNameField;
            }
            set
            {
                this.userNameField = value;
            }
        }

        /// <remarks/>
        public int credentialSetId
        {
            get
            {
                return this.credentialSetIdField;
            }
            set
            {
                this.credentialSetIdField = value;
            }
        }

        /// <remarks/>
        public System.DateTime startTime
        {
            get
            {
                return this.startTimeField;
            }
            set
            {
                this.startTimeField = value;
            }
        }

        /// <remarks/>
        public System.DateTime endTime
        {
            get
            {
                return this.endTimeField;
            }
            set
            {
                this.endTimeField = value;
            }
        }

        /// <remarks/>
        public int experimentInfoId
        {
            get
            {
                return this.experimentInfoIdField;
            }
            set
            {
                this.experimentInfoIdField = value;
            }
        }
    }

    public class ReservationData : ReservationInfo
    {
        private string sbGuidField;
        private string groupNameField;
        private string clientGuidField;
        private string lsGuidField;
        private string lssGuidField;

        public string sbGuid
        {
            get
            {
                return sbGuidField;
            }
            set
            {
                sbGuidField = value;
            }
        }
        public string groupName
        {
            get
            {
                return groupNameField;
            }
            set
            {
                groupNameField = value;
            }
        }

        public string clientGuid
        {
            get
            {
                return clientGuidField;
            }
            set
            {
                clientGuidField = value;
            }
        }
        public string lsGuid
        {
            get
            {
                return lsGuidField;
            }
            set
            {
                lsGuidField = value;
            }
        }
        public string lssGuid
        {
            get
            {
                return lssGuidField;
            }
            set
            {
                lssGuidField = value;
            }
        }
    }

    /// <summary>
    /// a class which hold lab side scheduling server information
    /// </summary>
    public class LSSInfo
    {
        /// <summary>
        /// the ID of the lab side scheduling server information
        /// </summary>
        public int lssInfoId;
        /// <summary>
        /// The coupon ID of the persitant REVOKE_RESERVATION ticket
        /// </summary>
        public long revokeCouponID;
        /// <summary>
        /// The domain of the revokeCoupon
        /// </summary>
        public string domainGuid;
        /// <summary>
        /// the GUID of the lab side scheduling server 
        /// </summary>
        public string lssGuid;
        /// <summary>
        /// the name of the lab side scheduling server
        /// </summary>
        public string lssName;
        /// <summary>
        /// teh url of the lab side scheduling server
        /// </summary>
        public string lssUrl;

    }

    /// <summary>
    /// a structure which holds USS Credential Set
    /// </summary>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://ilab.mit.edu/iLabs/type")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://ilab.mit.edu/iLabs/type", IsNullable = false)]
    public class UssCredentialSet
    {
        /// <summary>
        /// the ID of the Credential Set
        /// </summary>
        public int credentialSetId;
        /// <summary>
        /// the GUID of the Service Broker whose domain the group belongs to
        /// </summary>
        public string serviceBrokerGuid;
        /// <summary>
        /// the name of the service broker whose domain the group belongs to
        /// </summary>
        public string serviceBrokerName;
        /// <summary>
        /// the name of the group
        /// </summary>
        public string groupName;
    }

    /// <summary>
    /// a structure which holds user side scheduling policy
    /// </summary>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://ilab.mit.edu/iLabs/type")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://ilab.mit.edu/iLabs/type", IsNullable = false)]
    public class USSPolicy
    {
        /// <summary>
        /// the ID of the USSPolicy
        /// </summary>
        public int ussPolicyId;
        /// <summary>
        /// the ID of the information of the experiment that the policy employes to
        /// </summary>
        public int experimentInfoId;
        /// <summary>
        /// the rule 
        /// </summary>
        public string rule;
        /// <summary>
        /// the ID of the credential set, user with which should obey the rule.
        /// </summary>
        public int credentialSetId;

    }
    /// <summary>
    /// a structuer which holds experiment inforamtion
    /// </summary>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://ilab.mit.edu/iLabs/type")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://ilab.mit.edu/iLabs/type", IsNullable = false)]
    public class UssExperimentInfo
    {
        /// <summary>
        /// the ID of the experiment information
        /// </summary>
        public int experimentInfoId;
        /// <summary>
        /// the GUID of the lab server which issues the experiment 
        /// </summary>
        public string labServerGuid;
        /// <summary>
        /// the name of the lab server which issues the experiment
        /// </summary>
        public string labServerName;
        /// <summary>
        /// the GUID of the lab client 
        /// </summary>
        public string labClientGuid;
        /// <summary>
        /// the version of the client through which the experiment can be executed
        /// </summary>
        public string labClientVersion;
        /// <summary>
        /// the name of the client through which the experiment can be executed
        /// </summary>
        public string labClientName;
        /// <summary>
        /// the name of the experiment's provider
        /// </summary>
        public string providerName;
        /// <summary>
        /// the GUID of the lab side scheculing server through which the lab server can be accessed
        /// </summary>
        public string lssGuid;

    }
}