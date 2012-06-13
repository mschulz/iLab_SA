using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

namespace iLabs.Core
{
    public static class FactoryDB
    {
        static private string connectionStr;
        static private string providerStr;
        static private DateTime minDbDate;
        static private DateTime maxDbDate;
        static private DbProviderFactory theFactory;
        static private bool stripName = false;

        static FactoryDB(){
            // try to read connection and provider strings from the app settings
            try
            {
                connectionStr = ConfigurationManager.AppSettings["sqlConnection"];
            }
            catch (ConfigurationErrorsException ce)
            {
                connectionStr = null;
            }
            try
            {
                providerStr = ConfigurationManager.AppSettings["databaseProvider"];
            }
            catch (ConfigurationErrorsException pe)
            {
                connectionStr = null;
            }
           

            if (providerStr != null && !providerStr.Equals(""))
            {

                if (providerStr.CompareTo("System.Data.SqlClient") == 0)
                {
                    minDbDate = new DateTime(1753, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                    maxDbDate = new DateTime(9999, 12, 31, 23, 59, 59, DateTimeKind.Utc);
                    stripName = false;
                }
                else if (providerStr.CompareTo("MySql.Data.MySqlClient") == 0)
                {
                    minDbDate = new DateTime(1000, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                    maxDbDate = new DateTime(9999, 12, 31, 0, 0, 0, DateTimeKind.Utc);
                    stripName = true;
                }

                theFactory = DbProviderFactories.GetFactory(providerStr);
            }
            else
            {
                theFactory = null;
            }
        }

        public static string ConnectionStr
        {
            get
            {
                return connectionStr;
            }
            set
            {
                connectionStr = value;
            }
        }

        public static string ProviderStr
        {
            get
            {
                return providerStr;
            }
            set
            {
                if (value == null || value.Equals(""))
                {
                    providerStr = null;
                    theFactory = null;
                }
                else
                {
                    providerStr = value;
                    theFactory = DbProviderFactories.GetFactory(providerStr);
                }
            }
        }

        /// <summary>
        /// Creates an unopened connection to the database, should return a DbConnection.
        /// </summary>
        /// <returns></returns>
        public static DbConnection GetConnection()
        {
            DbConnection connection = null;
            if (connectionStr == null || connectionStr.Equals(""))
            {
                    throw new NoNullAllowedException(" The connection string is not specified, check configuration");
            }
            // Replace with Connection constructor for the Database used
            // create an DbConnection
            if (theFactory == null)
            {
                throw new NoNullAllowedException(" The DBprovider string is not specified, check configuration");
            }
            connection = theFactory.CreateConnection();
            connection.ConnectionString =connectionStr;
            return connection;
        }

        public static DbCommand CreateCommand(string text, DbConnection connection)
        {
            DbCommand cmd = connection.CreateCommand();
            cmd.CommandText = text;
            return cmd;
        }

        public static DbDataAdapter CreateDataAdapter()
        {
            return theFactory.CreateDataAdapter();
        }

        public static DbParameter CreateParameter( string name, DbType type)
        {
            DbParameter param = theFactory.CreateParameter();
            if (stripName)
                param.ParameterName = StripName(name);
            else
                param.ParameterName = name;
            param.DbType = type;
            return param;
        }

 	/// <summary>
        /// Convenience method to create a parameter
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="name"></param>
        /// <param name="type">DbType</param>
        /// <param name="maxSize"></param>
        /// <returns></returns>
        public static DbParameter CreateParameter( string name, DbType type, int max)
        {
            DbParameter param = theFactory.CreateParameter();
            if (stripName)
                param.ParameterName = StripName(name);
            else
                param.ParameterName = name;
            param.DbType = type;
            param.Size = max;
            return param;
        }

	 /// <summary>
        ///  Convenience method to create a parameter, minimal value checking is done
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="name"></param>
        /// <param name="value">If null or DBNull.Value, param.Value set to DBNull.Value, zero length strings are not converted to DBNull, 
        /// DateTime values are checked and adjusted to Database min or max if needed.</param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static DbParameter CreateParameter( string name, object value, DbType type)
        {
            DbParameter param = theFactory.CreateParameter();
            if (stripName)
                param.ParameterName = StripName(name);
            else
                param.ParameterName = name;
            param.DbType = type;
           if (value == null || value == System.DBNull.Value)
            {
                param.Value = System.DBNull.Value;
            }
            else
            {
                if (value is DateTime)
                {
                    if (((DateTime)value) < MinDbDate)
                        param.Value = MinDbDate;
                    else if (((DateTime)value) > MaxDbDate)
                        param.Value = MaxDbDate;
                    else
                        param.Value = value;
                }
                else
                {
                    param.Value = value;
                }
            }
            return param;
        }


	 /// <summary>
        ///  Convenience method to create a parameter, minimal value checking is done
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value">If null or DBNull.Value, param.Value set to DBNull.Value, zero length strings are not converted to DBNull, 
        /// DateTime values are checked and adjusted to Database min or max if needed.</param>
        /// <param name="type"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static DbParameter CreateParameter( string name, object value, DbType type, int size)
        {
            DbParameter param = CreateParameter( name, value, type);
            param.Size = size;
            return param;
        }

        public static DateTime MinDbDate
        {
            get
            {
                return minDbDate;
            }
        }

        public static DateTime MaxDbDate
        {
            get
            {
                return maxDbDate;
            }
        }
        public static string StripName(string name)
        {
            if (name.StartsWith("@"))
                return name.Substring(1);
            else
                return name;
        }
    }
}
