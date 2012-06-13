using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;

using iLabs.DataTypes.TicketingTypes;
using iLabs.DataTypes;
using iLabs.Core;
using iLabs.Ticketing;
using iLabs.UtilLib;

//using iLabs.Services;



namespace iLabs.TicketIssuer
{
    /// <summary>
    /// Interface for the DB Layer class
    /// </summary>
    public class TicketIssuerDB : ProcessAgentDB
    {
       
        /// <summary>
        /// 
        /// </summary>
        public TicketIssuerDB()
        {
    
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetIssuerGuid()
        {
            if (serviceAgent != null)
                return serviceAgent.agentGuid;
            else
                return null;
        }

        //public bool AuthenticateProcessAgent(Coupon agentCoupon, string agentGuid)
        //{
        //    // check that the agent record exists
        //    int id = GetProcessAgentID(agentGuid);
        //    if (id == 0)
        //        return false;

        //    // check that the agentCoupon parameter corresponds to the In coupon of the process agent
        //    if (AuthenticateCoupon(agentCoupon) && GetProcessAgentInfo(id).identIn.couponId == agentCoupon.couponId)
        //        return true;

        //    return false;
        //}

        /// <summary>
        /// Uses the reference ticketCollection to find a REDEEM_SESSION ticket and parse it
        /// </summary>
        /// <param name="sessionCoupon"></param>
        /// <param name="userId"></param>
        /// <param name="groupId"></param>
        /// <param name="clientId"></param>
        /// <returns>True if ticket found</returns>
        public bool RedeemSessionInfo(Coupon sessionCoupon, out int userId, out int groupId, out int clientId)
        {
            bool status = false;
            userId = -1;
            groupId = -1;
            clientId = -1;
            if (sessionCoupon != null)
            {
                if (sessionCoupon.issuerGuid.CompareTo(GetIssuerGuid()) == 0)
                {
                    status = AuthenticateIssuedCoupon(sessionCoupon);
                    if (status)
                    {
                        XmlQueryDoc xDoc = null;
                        Ticket sessionTicket = RetrieveIssuedTicket(sessionCoupon, TicketTypes.REDEEM_SESSION, GetIssuerGuid());
                        if (sessionTicket != null)
                        {
                            if (!sessionTicket.isCancelled && !sessionTicket.IsExpired())
                            {
                                xDoc = new XmlQueryDoc(sessionTicket.payload);
                                string user = xDoc.Query("RedeemSessionPayload/userID");
                                string group = xDoc.Query("RedeemSessionPayload/groupID");
                                string client = xDoc.Query("RedeemSessionPayload/clientID");
                                if (user != null && user.Length > 0)
                                    userId = Convert.ToInt32(user);
                                if (group != null && group.Length > 0)
                                    groupId = Convert.ToInt32(group);
                                if (client != null && client.Length > 0)
                                    clientId = Convert.ToInt32(client);
                            }
                        }
                    }
                }
            }
            return status;
        }


        /// <summary>
        /// Verifies that an issued  coupon corresponding to the argument exists, and is not cancelled
        /// </summary>
        /// <param name="couponID"></param>
        /// <param name="passkey"></param>
        /// <returns></returns>
        public bool AuthenticateIssuedCoupon(long couponID, string passkey)
        {
            bool status = false;
            DbConnection connection = FactoryDB.GetConnection();
            try
            {
                connection.Open();
                status = AuthenticateIssuedCoupon(connection, couponID, passkey);
            }
            finally
            {
                connection.Close();
            }
            return status;
        }

        /// <summary>
        /// Verifies that an issued  coupon corresponding to the argument exists, and is not cancelled
        /// </summary>
        /// <param name="coupon"></param>
        /// <returns></returns>
        public bool AuthenticateIssuedCoupon(Coupon coupon)
        {
            bool status = false;
            if (coupon != null)
            {
                if (String.Compare(coupon.issuerGuid, GetIssuerGuid(), true) == 0)
                {
                    DbConnection connection = FactoryDB.GetConnection();
                    try
                    {
                        connection.Open();
                        status = AuthenticateIssuedCoupon(connection, coupon.couponId, coupon.passkey);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            return status;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="couponID"></param>
        /// <param name="passkey"></param>
        /// <returns></returns>
        protected bool AuthenticateIssuedCoupon(DbConnection connection, long couponID, string passkey)
        {
            bool status = false;
            
                DbCommand cmd = FactoryDB.CreateCommand("AuthenticateIssuedCoupon", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                // populate parameters
                cmd.Parameters.Add(FactoryDB.CreateParameter("@couponID", couponID,DbType.Int64));
                cmd.Parameters.Add(FactoryDB.CreateParameter("@passKey", passkey,DbType.AnsiString, 100));
                
                try
                {
                    DbDataReader reader = cmd.ExecuteReader();
                    status = reader.HasRows;
                    reader.Close();
                }
                catch (DbException e)
                {
                    writeEx(e);
                    throw;
                }
            
            return status;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="coupon"></param>
        /// <returns></returns>
        protected bool AuthenticateIssuedCoupon(DbConnection connection, Coupon coupon)
        {
            bool status = false;
            if (String.Compare(coupon.issuerGuid, GetIssuerGuid(), true) == 0)
            {
                DbCommand cmd = FactoryDB.CreateCommand("AuthenticateIssuedCoupon", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                // populate parameters
                cmd.Parameters.Add(FactoryDB.CreateParameter("@couponID", coupon.couponId, DbType.Int64));
                cmd.Parameters.Add(FactoryDB.CreateParameter("@passKey", coupon.passkey, DbType.AnsiString, 100));

                try
                {
                    DbDataReader reader = cmd.ExecuteReader();
                    status = reader.HasRows;
                    reader.Close();
                }
                catch (DbException e)
                {
                    writeEx(e);
                    throw e;
                }
            }
            return status;
        }

       /// <summary>
       /// 
       /// </summary>
       /// <returns></returns>
        public Coupon CreateCoupon()
        {
            DbConnection connection = FactoryDB.GetConnection();

            try
            {
                connection.Open();
                Coupon coupon = CreateCoupon(connection);
                return coupon;

            }

            catch (DbException e)
            {
                writeEx(e);
                throw;
            }

            finally
            {
                connection.Close();
            }
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="passcode"></param>
        /// <returns></returns>
        public Coupon CreateCoupon(string passcode)
        {
            DbConnection connection = FactoryDB.GetConnection();

            try
            {
                connection.Open();
                Coupon coupon = CreateCoupon(connection,passcode);
                return coupon;

            }

            catch (DbException e)
            {
                writeEx(e);
                throw;
            }

            finally
            {
                connection.Close();
            }

        }

        /// <summary>
        /// Create a new coupon in the issued_Coupon Table, the coupon is assigned 
        /// a generated passkey and the service GUID. 
        /// </summary>
        /// <returns>Created Coupon</returns>
        protected Coupon CreateCoupon(DbConnection connection)
        {
            long couponID = -1;
           

            // create sql command
            // command executes the "CreateCoupon" stored procedure
            DbCommand cmd = FactoryDB.CreateCommand("CreateCoupon", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            string pass = Utilities.MakeGuid("N");
            // populate parameters
            cmd.Parameters.Add(FactoryDB.CreateParameter("@passKey", pass,DbType.AnsiString, 100));

            try
            {
                couponID = Convert.ToInt64(cmd.ExecuteScalar());
            }
            catch (DbException e)
            {
                writeEx(e);
                throw;
            }
            Coupon coupon = new Coupon(GetIssuerGuid(), couponID, pass);

            return coupon;
        }

        /// <summary>
        /// Create a new coupon in the issued_Coupon Table, the coupon is assigned 
        /// a generated passkey and the service GUID. 
        /// </summary>
        /// <returns>Created Coupon</returns>
        protected Coupon CreateCoupon(DbConnection connection, string pass)
        {
            long couponID = -1;


            // create sql command
            // command executes the "CreateCoupon" stored procedure
            DbCommand cmd = FactoryDB.CreateCommand("CreateCoupon", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            // populate parameters
            cmd.Parameters.Add(FactoryDB.CreateParameter("@passKey", pass, DbType.AnsiString, 100));

            //DbDataReader dataReader = null;
            try
            {
                couponID = Convert.ToInt64(cmd.ExecuteScalar());
            }
            catch (DbException e)
            {
                writeEx(e);
                throw;
            }
            Coupon coupon = new Coupon(GetIssuerGuid(), couponID, pass);

            return coupon;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="couponID"></param>
        public void CancelIssuedCoupon(long couponID)
        {
            bool status = false;
            DbConnection connection = FactoryDB.GetConnection();
            DbCommand cmd = FactoryDB.CreateCommand("CancelIssuedCoupon", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            // populate parameters
            cmd.Parameters.Add(FactoryDB.CreateParameter("@couponID", couponID,DbType.Int64));

            try
            {
                connection.Open();
                status = Convert.ToBoolean(cmd.ExecuteScalar());
            }
            catch (DbException e)
            {
                writeEx(e);
                throw;
            }

            finally
            {
                connection.Close();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="couponID"></param>
        public void DeleteIssuedCoupon(long couponID)
        {
            bool status = false;
            DbConnection connection = FactoryDB.GetConnection();
            DbCommand cmd = FactoryDB.CreateCommand("DeleteIssuedCoupon", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            // populate parameters
            cmd.Parameters.Add(FactoryDB.CreateParameter( "@couponID", couponID, DbType.Int64));

            try
            {
                connection.Open();
                status = Convert.ToBoolean(cmd.ExecuteScalar());
            }
            catch (DbException e)
            {
                writeEx(e);
                throw;
            }

            finally
            {
                connection.Close();
            }
        }
         /// <summary>
        /// Checks the IssuedCoupon table and constructs a full Coupon if an 
        /// Issued coupon is found and is not cancelled.
        /// </summary>
        /// <param name="couponID"></param>
        /// <returns>Coupon if found,  null if cancelled or not found</returns>
        public Coupon GetIssuedCoupon(long couponID)
        {

            Coupon coupon = null;
            DbConnection connection = FactoryDB.GetConnection();
            try
            {
                
                connection.Open();
                coupon = GetIssuedCoupon(connection, couponID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
            return coupon;

        }

        /// <summary>
        /// Checks the IssuedCoupon table and constructs a full Coupon if an 
        /// Issued coupon is found and is not cancelled.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="couponID"></param>
        /// <returns>Coupon if found,  null if cancelled or not found</returns>
        protected Coupon GetIssuedCoupon(DbConnection connection, long couponID)
        {
            Coupon coupon = null;
            DbCommand cmd = FactoryDB.CreateCommand("GetIssuedCoupon", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            // populate parameters
            cmd.Parameters.Add(FactoryDB.CreateParameter("@couponID", couponID,DbType.Int64));

            DbDataReader dataReader = null;
            try
            {
                dataReader = cmd.ExecuteReader();
            }
            catch (DbException e)
            {
                writeEx(e);
                throw;
            }
            while (dataReader.Read())
            {

                bool cancel = dataReader.GetBoolean(0);
                string pass = dataReader.GetString(1);
                if (!cancel)
                {
                    coupon = new Coupon(GetIssuerGuid(), couponID, pass);
                }
            }

            dataReader.Close();
            return coupon;
        }

        /// <summary>
        /// Checks the IssuedCoupon table and constructs a full Coupon if an 
        /// Issued coupon is found and is not cancelled. Passkeys must be unique
        /// </summary>
        /// <param name="passkey"
        /// <returns>Coupons if found,  null if cancelled or not found</returns>
        public Coupon[] GetIssuedCoupons(string passkey)
        {

            Coupon[] coupons = null;
            DbConnection connection = FactoryDB.GetConnection();
            try
            {

                connection.Open();
                coupons = GetIssuedCoupons(connection, passkey);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                connection.Close();
            }
            return coupons;

        }

        /// <summary>
        /// Checks the IssuedCoupon table and constructs a full Coupon if a 
        /// matching Issued coupon is found and is not cancelled. May reurm multiple Coupons but there should only be one.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="passkey"></param>
        /// <returns>Coupons if found,  null if cancelled or not found</returns>
        protected Coupon[] GetIssuedCoupons(DbConnection connection, string passkey)
        {
            List<Coupon> coupons = new List<Coupon>();
            DbCommand cmd = FactoryDB.CreateCommand("GetIssuedCouponByPasskey", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            // populate parameters
            cmd.Parameters.Add(FactoryDB.CreateParameter("@passkey", passkey, DbType.AnsiString));

            DbDataReader dataReader = null;
            try
            {
                dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    long couponID = dataReader.GetInt64(0);
                    bool cancel = dataReader.GetBoolean(1);
                    if (!cancel)
                    {
                        Coupon coupon = new Coupon(GetIssuerGuid(), couponID, passkey);
                        coupons.Add(coupon);
                    }
                }
            }
            catch (DbException e)
            {
                writeEx(e);
                throw;
            }
            finally
            {
                dataReader.Close();
            }
            return coupons.ToArray();
        }

        /// <summary>
        /// Counts the number of tickets remaining in the coupon collection, returns -1 on error;
        /// </summary>
        /// <param name="couponID"></param>
        /// <returns>the ticket count, -1 on error</returns>
        public int GetIssuedCouponCollectionCount(long couponID)
        {
            int count = -1;
            DbConnection connection = FactoryDB.GetConnection();
            try
            {
                connection.Open();
                count = GetIssuedCouponCollectionCount(connection, couponID);
            }
            catch (Exception e)
            { 
                throw;
            }
            finally{
                connection.Close();
            }
            return count;

        }

        protected int GetIssuedCouponCollectionCount(DbConnection connection, long couponID)
        {
            int count = -1;
            DbCommand cmd = FactoryDB.CreateCommand("GetIssuedCollectionCount", connection);
            cmd.CommandType = CommandType.StoredProcedure;


            // populate parameters 
            cmd.Parameters.Add(FactoryDB.CreateParameter("@couponID", couponID,DbType.Int64));
           
            count = Convert.ToInt32(cmd.ExecuteScalar());
            return count;
        }


        protected Ticket InsertIssuedTicket(DbConnection connection, long couponID, string redeemerGUID, string sponsorGUID,
            string type, long duration, string payload)
        {

            // creation time in seconds
            DateTime creation = DateTime.UtcNow;

            // command executes the "InsertIssuedTicket" stored procedure
            DbCommand cmd = FactoryDB.CreateCommand("InsertIssuedTicket", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            // populate parameters
            cmd.Parameters.Add(FactoryDB.CreateParameter("@ticketType", type,DbType.AnsiString, 100));
            cmd.Parameters.Add(FactoryDB.CreateParameter("@couponID", couponID,DbType.Int64));
            cmd.Parameters.Add(FactoryDB.CreateParameter("@redeemerGUID", redeemerGUID,DbType.AnsiString, 50));
            cmd.Parameters.Add(FactoryDB.CreateParameter("@sponsorGUID", sponsorGUID,DbType.AnsiString, 50));
            cmd.Parameters.Add(FactoryDB.CreateParameter("@payload", payload, DbType.String));
            cmd.Parameters.Add(FactoryDB.CreateParameter("@cancelled", 0,DbType.Boolean));
            cmd.Parameters.Add(FactoryDB.CreateParameter("@creationTime", creation, DbType.DateTime)); 
            cmd.Parameters.Add(FactoryDB.CreateParameter("@duration", duration,DbType.Int64));
         
            long id = -1;
            try
            {
                id = Convert.ToInt64(cmd.ExecuteScalar());
            }
            catch (DbException e)
            {
                writeEx(e);
                throw;
            }

            string issuerGuid = GetIssuerGuid();

            // construct the Ticket object and return it
            return new Ticket (id, type, couponID, issuerGuid, sponsorGUID, redeemerGUID, creation, duration, false, payload);  

        }

    
        /// <summary>
        /// 
        /// </summary>
        /// <param name="coupon"></param>
        /// <param name="type"></param>
        /// <param name="redeemerGuid"></param>
        /// <param name="sponsorGuid"></param>
        /// <param name="expiration"></param>
        /// <param name="payload"></param>
        /// <returns>The added Ticket, or null of the ticket cannot be added</returns>
        public Ticket AddTicket(Coupon coupon, 
            string type, string redeemerGuid, string sponsorGuid, long expiration, string payload)
        {
            Ticket ticket = null;
            // create sql connection
            DbConnection connection = FactoryDB.GetConnection();

            try
            {
                connection.Open();
                if (AuthenticateIssuedCoupon(connection, coupon))
                {
                    ticket = InsertIssuedTicket(connection, coupon.couponId, redeemerGuid, sponsorGuid,
                        type, expiration, payload);
                }
            }

            finally
            {
                connection.Close();
            }

            return ticket;
        }
        /// <summary>
        /// Creates a new coupon and adds a new ticket to it.
        /// </summary>
        /// <param name="ticketType"></param>
        /// <param name="redeemerGUID"></param>
        /// <param name="sponsorGUID"></param>
        /// <param name="duration"></param>
        /// <param name="payload"
        /// <returns>Coupon corresponding to the created Ticket</returns>
        public Coupon CreateTicket(string ticketType, string redeemerGUID, string sponsorGUID,
             long duration, string payload)
        {
            DbConnection connection = FactoryDB.GetConnection();

            try
            {
                // create a new coupon
                connection.Open();
                Coupon newCoupon = CreateCoupon(connection);

                Ticket ticket = InsertIssuedTicket(connection, newCoupon.couponId, redeemerGUID, sponsorGUID, ticketType, duration, payload);
                return newCoupon;

            }

            finally
            {
                connection.Close();
            }

        }



        /// <summary>
        /// Mark the ticket as cancelled in the DB
        /// </summary>
        /// <param name="ticket"></param>
        /// <returns></returns>
        public bool CancelIssuedTicket(Coupon coupon, Ticket ticket)
        {
            bool status = false;
            // create sql connection
            DbConnection connection = FactoryDB.GetConnection();

            // create sql command
            // command executes the "CancelTicket" stored procedure
            DbCommand cmd = FactoryDB.CreateCommand("CancelIssuedTicket", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            // populate the parameters
            cmd.Parameters.Add(FactoryDB.CreateParameter("@couponID", coupon.couponId, DbType.Int64));
            cmd.Parameters.Add(FactoryDB.CreateParameter("@redeemer", ticket.redeemerGuid, DbType.AnsiString, 50));
            cmd.Parameters.Add(FactoryDB.CreateParameter("@ticketType", ticket.type,DbType.AnsiString, 100));

            // execute the command
            try
            {
                connection.Open();
                status = Convert.ToBoolean(cmd.ExecuteScalar());
            }
            catch (DbException e)
            {
                writeEx(e);
                throw;
            }
            finally
            {
                connection.Close();
            }

            return status;
        }

        public void DeleteIssuedTicket(long ticketID)
        {
            bool status = false;
            DbConnection connection = FactoryDB.GetConnection();
            DbCommand cmd = FactoryDB.CreateCommand("DeleteIssuedTicket", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            // populate parameters
            cmd.Parameters.Add(FactoryDB.CreateParameter("@ticketID", ticketID,DbType.Int64));
            

            try
            {
                connection.Open();
                status = Convert.ToBoolean(cmd.ExecuteScalar());
            }
            catch (DbException e)
            {
                writeEx(e);
                throw;
            }

            finally
            {
                connection.Close();
            }
        }

        /// <summary>
        /// Retrieve a ticket coupon from the database.
        /// The triple (type,redeemerGuid,sponsorGuid) identifies the ticket.
        /// </summary>
        /// <param name="typeType"></param>
        /// <param name="redeemerGuid"></param>
        /// <param name="sponsorGuid"></param>
        /// <returns>Retrieved Coupon, or null if  the ticket cannot be found</returns>
        public  Coupon []  RetrieveIssuedTicketCoupon(string ticketType, string redeemerGuid,string sponsorGuid)
        {
            List<Coupon> results = new List<Coupon>(); ;
            // create sql connection
            DbConnection connection = FactoryDB.GetConnection();

            // create sql command
            // command executes the "RetrieveTicket" stored procedure
            DbCommand cmd = FactoryDB.CreateCommand("GetIssuedTicketCoupon", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            // populate the parameters
           
            cmd.Parameters.Add(FactoryDB.CreateParameter("@type", ticketType,DbType.AnsiString, 100));
            cmd.Parameters.Add(FactoryDB.CreateParameter("@redeemer", redeemerGuid,DbType.AnsiString, 50));
            cmd.Parameters.Add(FactoryDB.CreateParameter("@sponsor", sponsorGuid,DbType.AnsiString, 50));

            // execute the command
            DbDataReader dataReader = null;
            try
            {
                connection.Open();
                dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    Coupon coupon = new Coupon();
                  
                    // read id
                    coupon.couponId = dataReader.GetInt64(0);
                    coupon.passkey = dataReader.GetString(1);
                    coupon.issuerGuid = ServiceGuid;
                    results.Add(coupon);
                   
                }

            }
            catch (DbException e)
            {
                writeEx(e);
                throw;
            }

            finally
            {
                connection.Close();
            }
            if (results.Count > 0)
            {
                return results.ToArray();
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// Retrieve a ticket from the database. TicketIssuerDB version
        /// The triple (couponID, redeemerID, type) uniquely identifies the ticket.
        /// If the ticket was issued here try the issuedTickets,
        /// Note the ProcessAgent must store the tickets, a null return is a valid value.
        /// </summary>
        /// <param name="coupon"></param>
        /// <param name="redeemerGUID"></param>
        /// <param name="type"></param>
        /// <returns>Retrieved Ticket, or null if  the ticket cannot be found</returns>
        public override Ticket RetrieveTicket(Coupon coupon, string ticketType, string redeemerGUID)
        {
            Ticket result = null;
            if (coupon.issuerGuid == ServiceGuid)
            {
                result = RetrieveIssuedTicket(coupon, ticketType, redeemerGUID);
            }
            else
            {
                base.RetrieveTicket(coupon, ticketType, redeemerGUID);
            }
          
            return result;
        }

        /// <summary>
        /// Retrieve a ticket from the database.
        /// The triple 
        /// </summary>
        /// <param name="duration"> minimum remaining time (seconds) to expire or -1 for never expiring tickets</param>
        /// <param name="ticketType"></param>
        /// <param name="redeemerGuid"></param>
        /// <param name="sponsorGuid"></param>
        /// <returns>Retrieved Tickets, or null if  the ticket cannot be found</returns>
        public Ticket[] RetrieveIssuedTickets(long duration, string ticketType, 
            string redeemerGuid, string sponsorGuid)
        {
            List<Ticket> tickets = new List<Ticket>();
            // create sql connection
            DbConnection connection = FactoryDB.GetConnection();

            // create sql command
            // command executes the "RetrieveTicket" stored procedure
            DbCommand cmd = FactoryDB.CreateCommand("GetIssuedTicketByFunction", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            // populate the parameters
            cmd.Parameters.Add(FactoryDB.CreateParameter("@duration", duration, DbType.Int64));
            cmd.Parameters.Add(FactoryDB.CreateParameter("@ticketType", ticketType, DbType.AnsiString, 100));
            cmd.Parameters.Add(FactoryDB.CreateParameter("@redeemerGuid", redeemerGuid, DbType.AnsiString, 50));
            cmd.Parameters.Add(FactoryDB.CreateParameter("@sponsorGuid", sponsorGuid, DbType.AnsiString, 50));

            // execute the command
            DbDataReader dataReader = null;
            try
            {
                connection.Open();
                dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    Ticket ticket = new Ticket();
                    ticket.issuerGuid = ServiceGuid;
                    // read ticket id
                    ticket.ticketId = (long)dataReader.GetInt64(0);
                    ticket.type = dataReader.GetString(1);
                    // read coupon id
                    ticket.couponId = (long)dataReader.GetInt64(2);
                    // read redeemer id
                    ticket.redeemerGuid = dataReader.GetString(3);
                    // read sponsor id
                    ticket.sponsorGuid = dataReader.GetString(4);
                    // read expiration
                    ticket.creationTime = dataReader.GetDateTime(5);
                    ticket.duration = dataReader.GetInt64(6);
                    // read payload
                    if (!DBNull.Value.Equals(dataReader.GetValue(7)))
                        ticket.payload = dataReader.GetString(7);
                    // read Cancelled
                    bool cancelled = dataReader.GetBoolean(8);
                    if (!cancelled)
                        tickets.Add(ticket);
                }

            }
            catch (DbException e)
            {
                writeEx(e);
                throw;
            }

            finally
            {
                connection.Close();
            }
            if (tickets.Count > 0)
                return tickets.ToArray();
            else
                return null;
        }


        /// <summary>
        /// Retrieve a ticket from the database.
        /// The triple (couponID, redeemerID, type) uniquely identifies the ticket.
        /// </summary>
        /// <param name="coupon"></param>
        /// <param name="redeemerID"></param>
        /// <param name="type"></param>
        /// <returns>Retrieved Ticket, or null if  the ticket cannot be found</returns>
        public virtual Ticket RetrieveIssuedTicket(Coupon coupon, string ticketType, string redeemerID)
        {
            Ticket result = null;
            // create sql connection
            DbConnection connection = FactoryDB.GetConnection();

            // create sql command
            // command executes the "RetrieveTicket" stored procedure
            DbCommand cmd = FactoryDB.CreateCommand("GetIssuedTicketByRedeemer", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            // populate the parameters
            cmd.Parameters.Add(FactoryDB.CreateParameter("@couponID",coupon.couponId, DbType.Int64));
            cmd.Parameters.Add(FactoryDB.CreateParameter("@ticketType", ticketType,DbType.AnsiString, 100));
            cmd.Parameters.Add(FactoryDB.CreateParameter("@redeemer", redeemerID, DbType.AnsiString, 50));
           
            // execute the command
            DbDataReader dataReader = null;
            try
            {
                connection.Open();
                dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    Ticket ticket = new Ticket();
                    ticket.issuerGuid = ServiceGuid;
                    // read ticket id
                    ticket.ticketId = (long)dataReader.GetInt64(0);
                    ticket.type = dataReader.GetString(1);
                    // read coupon id
                    ticket.couponId = (long)dataReader.GetInt64(2);
                    // read redeemer id
                    ticket.redeemerGuid = dataReader.GetString(3);
                    // read sponsor id
                    ticket.sponsorGuid = dataReader.GetString(4);
                    // read expiration
                    ticket.creationTime = dataReader.GetDateTime(5);
                    ticket.duration = dataReader.GetInt64(6);
                    // read payload
                    if (!DBNull.Value.Equals(dataReader.GetValue(7)))
                    ticket.payload = dataReader.GetString(7);
                    // read Cancelled
                    bool cancelled = dataReader.GetBoolean(8);
                    if (!cancelled)
                        result = ticket;
                }

            }
            catch (DbException e)
            {
                writeEx(e);
                throw;
            }

            finally
            {
                connection.Close();
            }

            return result;
        }

        public Ticket RetrieveTicket(Coupon coupon, string type)
        {
            return GetIssuedTicket(coupon, type);
        }
/*
        public Ticket RedeemTicket(Coupon coupon, string type, string redeemerGuid)
        {
            return GetIssuedTicket(coupon, type, redeemerGuid);
        }
*/
        protected Ticket GetIssuedTicket(Coupon coupon, string type)
        {
            Ticket results = null;

            // create sql connection
            DbConnection connection = FactoryDB.GetConnection();

            // create sql command
            // command executes the "CancelTicket" stored procedure
            DbCommand cmd = FactoryDB.CreateCommand("GetIssuedTicket", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            // populate the parameters
            cmd.Parameters.Add(FactoryDB.CreateParameter("@couponID",coupon.couponId, DbType.Int64));
            cmd.Parameters.Add(FactoryDB.CreateParameter("@ticketType",  type, DbType.AnsiString, 100));

            DbDataReader dataReader = null;
            try
            {
                connection.Open();
                dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    Ticket ticket = new Ticket();
                    ticket.couponId = coupon.couponId;
                    ticket.issuerGuid = coupon.issuerGuid;
                    ticket.ticketId = dataReader.GetInt64(0);
                    ticket.type = dataReader.GetString(1);
                    ticket.redeemerGuid = dataReader.GetString(3);
                    ticket.sponsorGuid = dataReader.GetString(4);
                    ticket.creationTime = dataReader.GetDateTime(5);
                    ticket.duration = dataReader.GetInt64(6);
                    if (!DBNull.Value.Equals(dataReader.GetValue(7)))
                    ticket.payload = dataReader.GetString(7);
                    bool cancelled = dataReader.GetBoolean(8);
                    if (!cancelled)
                    {
                        results = ticket;
                    }
                }
                dataReader.Close();
            }
            catch (DbException e)
            {
                writeEx(e);
                throw;
            }


            finally
            {
                connection.Close();
            }

            return results;
        }

        protected Ticket GetIssuedTicket(Coupon coupon, string type, string redeemer)
        {
            Ticket results = null;

            // create sql connection
            DbConnection connection = FactoryDB.GetConnection();

            // create sql command
            // command executes the "CancelTicket" stored procedure
            DbCommand cmd = FactoryDB.CreateCommand("GetIssuedTicketByRedeemer", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            // populate the parameters
            cmd.Parameters.Add(FactoryDB.CreateParameter("@couponID", coupon.couponId,DbType.Int64));
            cmd.Parameters.Add(FactoryDB.CreateParameter("@ticketType", type, DbType.AnsiString, 100));
            cmd.Parameters.Add(FactoryDB.CreateParameter("@redeemer", redeemer,DbType.AnsiString, 50));

            DbDataReader dataReader = null;
            try
            {
                connection.Open();
                dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    Ticket ticket = new Ticket();
                    ticket.couponId = coupon.couponId;
                    ticket.issuerGuid = coupon.issuerGuid;
                    ticket.ticketId = dataReader.GetInt64(0);
                    ticket.type = dataReader.GetString(1);
                    ticket.redeemerGuid = dataReader.GetString(3);
                    ticket.sponsorGuid = dataReader.GetString(4);
                    ticket.creationTime = dataReader.GetDateTime(5);
                    ticket.duration = dataReader.GetInt64(6);
                    if (!DBNull.Value.Equals(dataReader.GetValue(7)))
                    ticket.payload = dataReader.GetString(7);
                    bool cancelled = dataReader.GetBoolean(8);
                    if (!cancelled)
                    {
                        results = ticket;
                    }
                }
                dataReader.Close();
            }
            catch (DbException e)
            {
                writeEx(e);
                throw;
            }


            finally
            {
                connection.Close();
            }

            return results;
        }

        public bool RequestTicketCancellation(Coupon coupon, string type, string redeemerGuid)
        {
            // create sql connection
            DbConnection connection = FactoryDB.GetConnection();
            DbCommand cmd = FactoryDB.CreateCommand("CancelIssuedTicket", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            // populate the parameters
            cmd.Parameters.Add(FactoryDB.CreateParameter("@couponID",coupon.couponId, DbType.Int64));
            cmd.Parameters.Add(FactoryDB.CreateParameter("@ticketType", type,DbType.AnsiString, 100));
            cmd.Parameters.Add(FactoryDB.CreateParameter("@redeemer", redeemerGuid,DbType.AnsiString, 50));
            
            try{
                connection.Open();
                Object status = cmd.ExecuteScalar();
                if (status != null)
                {
                    return Convert.ToInt32(status) == 1;
                }
                else
                    return false;
            }
            catch(Exception e){
                throw;
            }
            finally{
                connection.Close();
            }

        }

        public List<Ticket> GetExpiredIssuedTickets()
        {
            List<Ticket> tickets = new List<Ticket>();

              // create sql connection
            DbConnection connection = FactoryDB.GetConnection();

            // create sql command
            // command executes the "CancelTicket" stored procedure
            DbCommand cmd = FactoryDB.CreateCommand("GetExpiredIssuedTickets", connection);
            cmd.CommandType = CommandType.StoredProcedure;

         

            DbDataReader dataReader = null;
            try{
                connection.Open();
                dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    Ticket ticket = new Ticket();
                    ticket.issuerGuid = this.GetIssuerGuid();
                    ticket.ticketId = dataReader.GetInt64(0);
                    ticket.type = dataReader.GetString(1);
                    ticket.couponId = dataReader.GetInt64(2);
                    ticket.redeemerGuid = dataReader.GetString(3);
                    ticket.sponsorGuid = dataReader.GetString(4);
                    ticket.creationTime = dataReader.GetDateTime(5);
                    ticket.duration = dataReader.GetInt64(6);
                    if (!DBNull.Value.Equals(dataReader.GetValue(7)))
                        ticket.payload = dataReader.GetString(7);
                    ticket.isCancelled = !dataReader.GetBoolean(8);
                    tickets.Add(ticket);

                }
                dataReader.Close();
            }
            catch (DbException e)
            {
                writeEx(e);
                throw;
            }


            finally
            {
                connection.Close();
            }

            return tickets;
        }



    }
}
