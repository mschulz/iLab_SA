using System;
//using iLabs.Services;

using iLabs.DataTypes.TicketingTypes;

namespace iLabs.Ticketing
{
	/// <summary>
	/// Summary description for Ticketing.
	/// </summary>
	public class TicketUtil
	{
		private static Random autoRand = new Random( );

		public static string NewPasskey()
		{
			// generate the Service Broker passkey
	
			Double d = autoRand.NextDouble();
			// change to a string
			return((long)(d*1000000000000000)).ToString("000000000000000");
		}
	
	}

    public class TicketingAutheticationFailedException : System.ApplicationException
    {
        public TicketingAutheticationFailedException(string message)
            : base(message)
        {

        }
    }

    public class TicketNotFoundException : TicketingAutheticationFailedException
    {
        public TicketNotFoundException(string message)
            : base(message)
        {

        }
    }

    public class TicketExpiredException : TicketingAutheticationFailedException
    {
        public TicketExpiredException(string message)
            : base(message)
        {

        }
    }
}
