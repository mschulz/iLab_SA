/*
 * Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
 * Please see license.txt in top level directory for full license.
 */

/* $Id$ */

using System;
using System.Web.Security;

using iLabs.Core;
using iLabs.DataTypes.TicketingTypes;
using iLabs.ServiceBroker.Administration;
using iLabs.ServiceBroker.Internal;
using iLabs.Ticketing;


namespace iLabs.ServiceBroker.Authentication
{
	public class AuthenticationType
	{
        // Warning if additional types are added need to update the database
        public enum AuthTypeID : int
        {
            Undefined = 0, Native = 1,
            Kerberos = 2, ThirdParty = 3

        }
		public const string NativeAuthentication = "Native";
		public const string Kerberos = "Kerberos_MIT";
        public const string ThirdParty = "Third_Party";
	}


	/// <summary>
	/// Summary description for Authentication.
	/// </summary>
	public class AuthenticationAPI
	{
		public AuthenticationAPI()
		{
		}

		/// <summary>
		/// performs whatever actions including GUI interaction to identify and authenticate the user who has initiated the current session
		/// </summary>
		/// <param name="userID">the ID of user</param>
		/// <param name="password">the user's password</param>
		/// <returns>true if the user has been authenticated; false otherwise</returns>
		public static bool Authenticate (int userID, string password)
		{
            bool status = false;
			string hashedDBPassword = InternalAuthenticationDB.ReturnNativePassword (userID);
            if (hashedDBPassword != null && hashedDBPassword.Length > 0)
            {
                string hashedPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(password, "sha1");
                if (hashedPassword == hashedDBPassword)
                {
                    status = true;
                }
            }
            return status;
		}

        /// <summary>
        /// performs whatever actions including GUI interaction to identify and authenticate the user who has initiated the current session
        /// </summary>
        /// <param name="userName">the local of user</param>
        /// <param name="authGuid">an optional authority guid, if empty use local pasword</param>
        /// <param name="password">the user's password</param>
        /// <returns>true if the user has been authenticated; false otherwise</returns>
        public static bool Authenticate(string userName, string authGuid, string password)
        {
            BrokerDB brokerDB = new BrokerDB();
            Authority auth = null;
            bool status = false;
            int userID = -1;
            if (authGuid != null && authGuid.Length > 0)
            {
                auth = brokerDB.AuthorityRetrieve(authGuid);
            }
            else{
                auth = brokerDB.AuthorityRetrieve(0);
            }
            if(auth != null){
                userID = InternalAdminDB.SelectUserID(userName,auth.authorityID);
                if (auth.authorityID == 0 && auth.authTypeID == 1) //Test for local authentication
                {
                    string hashedDBPassword = InternalAuthenticationDB.ReturnNativePassword(userID);
                    if (hashedDBPassword != null && hashedDBPassword.Length > 0)
                    {
                        string hashedPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(password, "sha1");
                        if (hashedPassword == hashedDBPassword)
                        {
                            status = true;
                        }
                    }
                }
                else
                {
                    // For now accept third party authentication
                    if(userID > 0)
                        status = true;
                }
            }
            return status;
        }

        public static Ticket GetAuthenticateAgentTicket(string authGuid, string passkey)
        {
            Ticket authTicket = null;
            BrokerDB brokerDB = new BrokerDB();
            Coupon[] authCoupons = brokerDB.GetIssuedCoupons(passkey);
            if (authCoupons != null && authCoupons.Length > 0)
            {
                authTicket = brokerDB.RetrieveTicket(authCoupons[0], TicketTypes.AUTHENTICATE_AGENT);
                if (authTicket == null || authTicket.sponsorGuid.CompareTo(authGuid) != 0)
                {
                    //TODO: Parse Ticket

                    throw new AccessDeniedException("AccessDenied!");
                }

            }
            return authTicket;
        }
		/*		
		public static string Authenticate (string type)
		{
			// how to do this?
		}

		public static bool CreateNativePrincipal (string principalID)
		{

		}

		public static string[] RemoveNativePrincipals (string[] principalIDs)
		{
		
		}

		public static string[] ListNativePrincipals()
		{

		}
*/
		/// <summary>
		/// Set the password of the specified native principal
		/// </summary>
		/// <param name="userID">the ID of the native principal whose password is to be changed</param>
		/// <param name="password">the new password</param>
		/// <returns>true if the change was successful; false if the new password was of inappropriate form or the native userID unknown.</returns>
		public static bool SetNativePassword (int userID, string password)
		{
			string hashedPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(password, "sha1");
			try
			{
				InternalAuthenticationDB.SaveNativePassword (userID, hashedPassword);

			}
			catch
			{
				return false;
			}
			return true;

		}

	
	}
}
