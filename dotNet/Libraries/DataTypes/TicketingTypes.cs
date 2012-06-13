using System;
using System.Collections.Generic;
using System.Text;

namespace iLabs.DataTypes.TicketingTypes
{
    /// <summary>
    /// Coupons convey passcodes and domain authorization information.
    /// </summary>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://ilab.mit.edu/iLabs/type")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://ilab.mit.edu/iLabs/type")]
    public class Coupon
    {
        /// <summary>
        /// 
        /// </summary>
        public long couponId;

        /// <summary>
        /// 
        /// </summary>
        public string issuerGuid;

        /// <summary>
        /// 
        /// </summary>
        public string passkey;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Coupon()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="couponId"></param>
        /// <param name="issuerGuid"></param>
        /// <param name="passkey"></param>
        public Coupon(string issuerGuid, long couponId, string passkey)
        {
            this.couponId = couponId;
            this.issuerGuid = issuerGuid;
            this.passkey = passkey;
        }
        public static bool operator ==(Coupon c1, Coupon c2)
        {
            if((object)c1 == null && (object)c2==null)
                return true;
            else if((object)c1 == null || (object)c2==null)
                return false;
            else
               return (c1.couponId == c2.couponId) && (c1.passkey == c2.passkey) && (c1.issuerGuid == c2.issuerGuid);
          
        }

        public static bool operator !=(Coupon c1, Coupon c2)
        {
            if ((object)c1 == null && (object)c2 == null)
                return false;
            else if ((object)c1 == null || (object)c2 == null)
                return true;
            else
                return (c1.couponId != c2.couponId) || (c1.passkey != c2.passkey) || (c1.issuerGuid != c2.issuerGuid);
        }
        

        public override bool Equals(object obj)
        {
            return (this == (Coupon)obj);
        }

        public override int GetHashCode()
        {
            return couponId.GetHashCode() + ( issuerGuid != null ? issuerGuid.GetHashCode() : 0) + ( passkey != null ? passkey.GetHashCode() : 0);
        }
    }

    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://ilab.mit.edu/iLabs/type")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://ilab.mit.edu/iLabs/type")]
    public class Ticket
    {
        /// <summary>
        /// Unique ticket ID
        /// </summary>
        public long ticketId;

        /// <summary>
        /// a string defining the type of ticket
        /// </summary>
        public string type;

        /// <summary>
        /// The ID of the coupon defining which ticket collection this ticket is a member of.
        /// </summary>
        public long couponId;

        /// <summary>
        /// The Ticket issuer
        /// </summary>
        public string issuerGuid;

        /// <summary>
        /// Guid of the processAgent requesting the creation of this ticket.
        /// </summary>
        public string sponsorGuid;

        /// <summary>
        /// Guid of the processAgent that will be processing the specified operation.
        /// </summary>
        public string redeemerGuid;

        /// <summary>
        /// Creation time of the ticket stored as UTC
        /// </summary>
        public DateTime creationTime;

        /// <summary>
        /// Ticket duration, the number of seconds before the ticket expires. Negative one ( -1 )
        /// is used to define a ticket that never expires.
        /// </summary>
        public long duration;

        /// <summary>
        /// 
        /// </summary>
        public bool isCancelled;

        /// <summary>
        /// The ticket body.
        /// </summary>
        public string payload;


        /// <summary>
        /// 
        /// </summary>
        public Ticket()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ticketId"></param>
        /// <param name="type"></param>
        /// <param name="couponId"></param>
        /// <param name="issuerGuid"></param>
        /// <param name="sponsorGuid"></param>
        /// <param name="redeemerGuid"></param>
        /// <param name="createTime"></param>
        /// <param name="duration"></param>
        /// <param name="isCancelled"></param>
        /// <param name="payload"></param>
        public Ticket(long ticketId, string type, long couponId, string issuerGuid, string sponsorGuid,
            string redeemerGuid, DateTime createTime, long duration, bool isCancelled, string payload)
        {
            this.ticketId = ticketId;
            this.type = type;
            this.couponId = couponId;
            this.issuerGuid = issuerGuid;
            this.sponsorGuid = sponsorGuid;
            this.redeemerGuid = redeemerGuid;

            if ((createTime.Kind == DateTimeKind.Local) || (createTime.Kind == DateTimeKind.Unspecified))
                this.creationTime = createTime.ToUniversalTime();
            else
                this.creationTime = createTime;
            this.duration = duration;
            this.isCancelled = isCancelled;
            this.payload = payload;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ticketId"></param>
        /// <param name="type"></param>
        /// <param name="couponId"></param>
        /// <param name="issuerGuid"></param>
        /// <param name="sponsorGuid"></param>
        /// <param name="redeemerGuid"></param>
        /// <param name="creationTime"></param>
        /// <param name="expirationTime"></param>
        /// <param name="isCancelled"></param>
        /// <param name="payload"></param>
        public Ticket(long ticketId, string type, long couponId, string issuerGuid, string sponsorGuid,
            string redeemerGuid, long duration, string payload)
        {
            this.ticketId = ticketId;
            this.type = type;
            this.couponId = couponId;
            this.issuerGuid = issuerGuid;
            this.sponsorGuid = sponsorGuid;
            this.redeemerGuid = redeemerGuid;
            this.creationTime = DateTime.UtcNow;
            this.duration = duration;
            this.isCancelled = false;
            this.payload = payload;
        }

        public bool IsExpired()
        {
            bool state = false;
            if (duration != -1)
            {
                // Documentation states that Add does not alter the original value
                if (creationTime.AddTicks(duration * TimeSpan.TicksPerSecond) < DateTime.UtcNow)
                    state = true;
            }
            return state;
        }

        public long SecondsToExpire()
        {
            long remaining = long.MaxValue;
            if (duration != -1)
            {
                TimeSpan ts = creationTime.AddTicks(duration * TimeSpan.TicksPerSecond).Subtract(DateTime.UtcNow);
                remaining = ts.Ticks / TimeSpan.TicksPerSecond;
            }
            return remaining;
        }


    }

 
 

}
