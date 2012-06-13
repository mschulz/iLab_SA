using System;
using System.Configuration;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Runtime.InteropServices;

using iLabs.Core;
using iLabs.DataTypes;
using iLabs.DataTypes.StorageTypes;
using iLabs.DataTypes.SoapHeaderTypes;
using iLabs.DataTypes.TicketingTypes;
using iLabs.Ticketing;
using iLabs.UtilLib;
using iLabs.Proxies.ESS;
using iLabs.Proxies.ISB;

//using iLabs.LabServer.LabView;



namespace iLabs.LabServer.Interactive
{
	/// <summary>
	/// Summary description for LabDB.
	/// </summary>
	public class LabDB : ProcessAgentDB
	{
		public LabDB()
		{
			//
			// TODO: Add constructor logic here
			//
		}

        //public void ProcessExpiredTasks()
        //{
        //}
/*
		public LabAppInfo GetVIforGroup(long groupID)
		{
			return new LabAppInfo();
		}
         public int InsertLabApp(LabAppInfo app)
        {
*/

        public int InsertLabApp(LabAppInfo app)
        {
            return InsertLabApp(app.title, app.appGuid,app.version, app.appKey,
                app.path, app.application, app.page, app.appURL, app.width, app.height,
                app.dataSources, app.server, app.port,
                app.contact, app.description, app.comment, app.extraInfo,
                app.rev, app.type);
        }

        /*
@application varchar (100),
@appKey varchar (100),
@path varchar (256),
@version varchar (50),
@rev varchar (50),
@page varchar (256),
@title varchar (256),
@description varchar (2000),
@comment varchar (256),
@width int,
@height int,
@type int,
@server varchar (256),
@port int,
@contact varchar (256),
@cgi varchar (256),
@datasource varchar (2000),
@extra nvarchar (2000)
*/
        public int InsertLabApp(string title,string appGuid, string version, string appKey,
            string path, string application, string page,string cgi,int width, int height,
            string datasource,  string server,int port,
            string contact,string description, string comment, string extra,
            string rev, int type)
            
        {
            int id = -1;
            DbConnection connection = FactoryDB.GetConnection();
            try
            {
                // create sql command
                DbCommand cmd = FactoryDB.CreateCommand("InsertLabApp", connection);
                cmd.CommandType = CommandType.StoredProcedure;
              
                cmd.Parameters.Add(FactoryDB.CreateParameter("@title", title,DbType.String, 256));
                cmd.Parameters.Add(FactoryDB.CreateParameter("@guid", appGuid, DbType.AnsiString, 50));
                cmd.Parameters.Add(FactoryDB.CreateParameter("@version", version, DbType.String, 50));
                cmd.Parameters.Add(FactoryDB.CreateParameter("@appKey", appKey, DbType.String, 100));
                cmd.Parameters.Add(FactoryDB.CreateParameter("@path", path, DbType.String, 256));
                cmd.Parameters.Add(FactoryDB.CreateParameter("@application", application, DbType.String, 100));
                cmd.Parameters.Add(FactoryDB.CreateParameter( "@page", page, DbType.String, 512));
                cmd.Parameters.Add(FactoryDB.CreateParameter("@cgi", cgi,DbType.String, 512));
                cmd.Parameters.Add(FactoryDB.CreateParameter("@width",width, DbType.Int32));
                cmd.Parameters.Add(FactoryDB.CreateParameter("@height", height,DbType.Int32));
                cmd.Parameters.Add(FactoryDB.CreateParameter("@datasource", datasource, DbType.String));
                cmd.Parameters.Add(FactoryDB.CreateParameter("@server", server,DbType.String, 256));
                if (port > 0)
                    cmd.Parameters.Add(FactoryDB.CreateParameter("@port", port,DbType.Int32));
                else
                   cmd.Parameters.Add(FactoryDB.CreateParameter("@port", null, DbType.Int32));
                cmd.Parameters.Add(FactoryDB.CreateParameter("@contact", contact, DbType.String, 256));
                cmd.Parameters.Add(FactoryDB.CreateParameter("@description", description, DbType.String, 2048));
                cmd.Parameters.Add(FactoryDB.CreateParameter("@comment", comment,DbType.String,2048));
                cmd.Parameters.Add(FactoryDB.CreateParameter("@extra", extra,DbType.String));
                cmd.Parameters.Add(FactoryDB.CreateParameter("@rev", rev, DbType.String, 50));
                cmd.Parameters.Add(FactoryDB.CreateParameter("@type", type, DbType.Int32));
                
                connection.Open();
                Object obj = cmd.ExecuteScalar();
                if (obj != null)
                    id = Convert.ToInt32(obj);


            }
            catch (Exception ex)
            {
               Logger.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return id;
        }

        public int ModifyLabApp(LabAppInfo app)
        {
            return ModifyLabApp(app.appID, app.title, app.appGuid, app.version, app.appKey,
                app.path, app.application, app.page, app.appURL, app.width, app.height,
                app.dataSources, app.server, app.port,
                app.contact, app.description, app.comment, app.extraInfo,
                app.rev, app.type);
        }

        public int ModifyLabApp(int appId, string title, string appGuid, string version, string appKey,
            string path, string application, string page, string cgi, int width, int height,
            string datasource, string server, int port,
            string contact, string description, string comment, string extra,
            string rev, int type)
        {
            int count = -1;
            DbConnection connection = FactoryDB.GetConnection();
            try
            {
                // create sql command
                DbCommand cmd = FactoryDB.CreateCommand("ModifyLabApp", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(FactoryDB.CreateParameter("@appId", appId,DbType.Int32));
                cmd.Parameters.Add(FactoryDB.CreateParameter("@title", title,DbType.String, 256));
                cmd.Parameters.Add(FactoryDB.CreateParameter("@guid", appGuid,DbType.AnsiString, 50));
                cmd.Parameters.Add(FactoryDB.CreateParameter("@version", version, DbType.String, 50));
                cmd.Parameters.Add(FactoryDB.CreateParameter("@appKey", appKey,DbType.String, 100));
                cmd.Parameters.Add(FactoryDB.CreateParameter("@path", path,DbType.String, 256));
                cmd.Parameters.Add(FactoryDB.CreateParameter("@application", application,DbType.String, 100));
                cmd.Parameters.Add(FactoryDB.CreateParameter("@page", page, DbType.String, 512));
                cmd.Parameters.Add(FactoryDB.CreateParameter("@cgi", cgi,DbType.String, 512));
                cmd.Parameters.Add(FactoryDB.CreateParameter("@width", width,DbType.Int32));
                cmd.Parameters.Add(FactoryDB.CreateParameter("@height", height, DbType.Int32));
                cmd.Parameters.Add(FactoryDB.CreateParameter("@datasource", datasource, DbType.String));
                cmd.Parameters.Add(FactoryDB.CreateParameter("@server", server, DbType.String, 256));  
                if (port > 0)
                    cmd.Parameters.Add(FactoryDB.CreateParameter("@port", port, DbType.Int32));
                else
                    cmd.Parameters.Add(FactoryDB.CreateParameter("@port", null, DbType.Int32));
                cmd.Parameters.Add(FactoryDB.CreateParameter("@contact", contact, DbType.String, 256));
                cmd.Parameters.Add(FactoryDB.CreateParameter("@description", description, DbType.String, 2048));
                cmd.Parameters.Add(FactoryDB.CreateParameter("@comment", comment,DbType.String, 2048));
                cmd.Parameters.Add(FactoryDB.CreateParameter("@extra", extra,DbType.String));
                cmd.Parameters.Add(FactoryDB.CreateParameter("@rev", rev,DbType.String, 50));
                cmd.Parameters.Add(FactoryDB.CreateParameter("@type", type, DbType.Int32));
               
                connection.Open();
                Object obj = cmd.ExecuteScalar();
                if (obj != null)
                    count = Convert.ToInt32(obj);


            }
            catch (Exception ex)
            {
               Logger.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return count;
        }

        public int ModifyLabPaths(string oldPath, string newPath)
        {
            int count = -1;
            DbConnection connection = FactoryDB.GetConnection();
            try
            {
                // create sql command
                DbCommand cmd = FactoryDB.CreateCommand("ModifyLabPaths", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(FactoryDB.CreateParameter("@oldPath", oldPath,DbType.String, 256));
                cmd.Parameters.Add(FactoryDB.CreateParameter("@newPath", newPath,DbType.String, 256));
            
                connection.Open();
                Object obj = cmd.ExecuteScalar();
                if (obj != null)
                    count = Convert.ToInt32(obj);

             }
            catch (Exception ex)
            {
               Logger.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return count;
        }

        public int DeleteLabApp(int appId)
        {
            int count = -1;
              DbConnection connection = FactoryDB.GetConnection();
            try
            {
                // create sql command
                DbCommand cmd = FactoryDB.CreateCommand("RemoveLabApp", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(FactoryDB.CreateParameter("@appId", appId, DbType.Int32));
               
                connection.Open();
                Object obj = cmd.ExecuteScalar();
                if (obj != null)
                    count = Convert.ToInt32(obj);
            }
            catch (Exception ex)
            {
               Logger.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return count;
        }


        public LabAppInfo GetLabApp(int appId)
        {
            DbConnection connection = FactoryDB.GetConnection();
            connection.Open();
            LabAppInfo info = GetLabApp(connection, appId);
            connection.Close();
            return info;
        }

        public LabAppInfo GetLabApp(string appKey)
        {
            DbConnection connection = FactoryDB.GetConnection();
            connection.Open();
            LabAppInfo info = GetLabApp(connection, appKey);
            connection.Close();
            return info;
        }

        public IntTag [] GetLabAppTags()
        {
            ArrayList list = new ArrayList();
            DbConnection connection = FactoryDB.GetConnection();

           // create sql command
			DbCommand cmd = FactoryDB.CreateCommand("GetLabAppTags", connection);
			cmd.CommandType = CommandType.StoredProcedure;
			DbDataReader dataReader = null;
			try 
			{
                connection.Open();
				dataReader = cmd.ExecuteReader();
                while (dataReader.Read()) 
			    {
                    IntTag tag = new IntTag();
                    tag.id = dataReader.GetInt32(0);
                    tag.tag = dataReader.GetString(1);
                    list.Add(tag);
                }
			} 
			catch (DbException e) 
			{
				writeEx(e);
				throw;
			}

			finally{
                connection.Close();
            }
            IntTag temp = new IntTag();
			return  ( IntTag[]) list.ToArray(temp.GetType());
        }

        public IntTag GetLabAppTag(int appId)
        {
            IntTag tag = null;
            DbConnection connection = FactoryDB.GetConnection();
            // create sql command
			// command executes the "GetVI" stored procedure
			DbCommand cmd = FactoryDB.CreateCommand("GetLabAppTag", connection);
			cmd.CommandType = CommandType.StoredProcedure;

			// populate parameters
			cmd.Parameters.Add(FactoryDB.CreateParameter("@appId", appId, DbType.Int32));
			
			DbDataReader dataReader = null;
			try 
			{
                connection.Open();
				dataReader = cmd.ExecuteReader();
                // id of created coupon
			
			    while (dataReader.Read()) 
			    {
                    tag = new IntTag();
                    tag.id = dataReader.GetInt32(0);
                    tag.tag = dataReader.GetString(1);
                }
			}
			catch (DbException e) 
			{
				writeEx(e);
				throw;
			}

			finally{
                connection.Close();
            }
            return tag;
        }

        public IntTag GetLabAppTag(string appKey)
        {
            IntTag tag = null;
            DbConnection connection = FactoryDB.GetConnection();
           // create sql command
			// command executes the "GetVI" stored procedure
			DbCommand cmd = FactoryDB.CreateCommand("GetLabAppTagByKey", connection);
			cmd.CommandType = CommandType.StoredProcedure;

			// populate parameters
            cmd.Parameters.Add(FactoryDB.CreateParameter( "@appKey", appKey, DbType.String, 100));
			
			DbDataReader dataReader = null;
            try
            {
                connection.Open();
                dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    tag = new IntTag();
                    tag.id = dataReader.GetInt32(0);
                    tag.tag = dataReader.GetString(1);
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
            return tag;
        }

        public LabAppInfo GetLabApp(DbConnection connection, int appId)
        {
	
			// create sql command
			// command executes the "GetVI" stored procedure
			DbCommand cmd = FactoryDB.CreateCommand("GetLabApp", connection);
			cmd.CommandType = CommandType.StoredProcedure;

			// populate parameters
			cmd.Parameters.Add(FactoryDB.CreateParameter("@appId", appId, DbType.Int32));
			
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

			// id of created coupon
			LabAppInfo appInfo = new LabAppInfo();
			appInfo.appID = appId;
			while (dataReader.Read()) 
			{
				appInfo.appID = appId;
				appInfo.path = dataReader.GetString(0);
				appInfo.application= dataReader.GetString(1);
				appInfo.page= dataReader.GetString(2);
                if (!dataReader.IsDBNull(3))
				appInfo.title= dataReader.GetString(3);
            if (!dataReader.IsDBNull(4))
				appInfo.description= dataReader.GetString(4);
				if(!dataReader.IsDBNull(5))
					appInfo.extraInfo= dataReader.GetString(5);
                if (!dataReader.IsDBNull(6))
				appInfo.contact= dataReader.GetString(6);
            if (!dataReader.IsDBNull(7))
				appInfo.comment= dataReader.GetString(7);
				appInfo.width= dataReader.GetInt32(8);
				appInfo.height= dataReader.GetInt32(9);
				if(!dataReader.IsDBNull(10))
					appInfo.dataSources= dataReader.GetString(10);
                if (!dataReader.IsDBNull(11))
                    appInfo.server = dataReader.GetString(11);
                if (!dataReader.IsDBNull(12))
                    appInfo.port = dataReader.GetInt32(12);
                if (!dataReader.IsDBNull(13))
                    appInfo.appURL = dataReader.GetString(13);
                if (!dataReader.IsDBNull(14))
                    appInfo.version = dataReader.GetString(14);
                if (!dataReader.IsDBNull(15))
                    appInfo.rev = dataReader.GetString(15);
                if (!dataReader.IsDBNull(16))
                    appInfo.appKey = dataReader.GetString(16);
                if (!dataReader.IsDBNull(17))
                    appInfo.appGuid = dataReader.GetString(17);
			}

			
	return appInfo;
	}

    public LabAppInfo GetLabApp(DbConnection connection, string appKey)
        {
            LabAppInfo appInfo = null;

            // create sql command
            // command executes the "GetVI" stored procedure
            DbCommand cmd = FactoryDB.CreateCommand("GetLabAppByKey", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            // populate parameters
            cmd.Parameters.Add(FactoryDB.CreateParameter( "@appKey", appKey, DbType.String, 100));

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

            // id of created coupon
           
            while (dataReader.Read())
            {
                appInfo = new LabAppInfo();
                appInfo.appID = dataReader.GetInt32(0);
                appInfo.path = dataReader.GetString(1);
                appInfo.application = dataReader.GetString(2);
                appInfo.page = dataReader.GetString(3);
                if (!dataReader.IsDBNull(4))
                    appInfo.title = dataReader.GetString(4);
                if (!dataReader.IsDBNull(5))
                    appInfo.description = dataReader.GetString(5);
                if (!dataReader.IsDBNull(6))
                    appInfo.extraInfo = dataReader.GetString(6);
                if (!dataReader.IsDBNull(7))
                    appInfo.contact = dataReader.GetString(7);
                if (!dataReader.IsDBNull(8))
                    appInfo.comment = dataReader.GetString(8);
                appInfo.width = dataReader.GetInt32(9);
                appInfo.height = dataReader.GetInt32(10);
                if (!dataReader.IsDBNull(11))
                    appInfo.dataSources = dataReader.GetString(11);
                if (!dataReader.IsDBNull(12))
                    appInfo.server = dataReader.GetString(12);
                if (!dataReader.IsDBNull(13))
                    appInfo.port = dataReader.GetInt32(13);
                if (!dataReader.IsDBNull(14))
                    appInfo.appURL = dataReader.GetString(14);
                if (!dataReader.IsDBNull(15))
                    appInfo.version = dataReader.GetString(15);
                if (!dataReader.IsDBNull(16))
                    appInfo.rev = dataReader.GetString(16);
                if (!dataReader.IsDBNull(17))
                    appInfo.appKey = dataReader.GetString(17);
                if (!dataReader.IsDBNull(18))
                    appInfo.appGuid = dataReader.GetString(18);
            }
            return appInfo;
        }

        public LabAppInfo[] GetLabApps()
        {
            DbConnection connection = FactoryDB.GetConnection();
            connection.Open();
            LabAppInfo[] labs = GetLabApps(connection);
            connection.Close();
            return labs;
        }

        public LabAppInfo[] GetLabApps(DbConnection connection)
        {

            // create sql command
            DbCommand cmd = FactoryDB.CreateCommand("GetLabApps", connection);
            cmd.CommandType = CommandType.StoredProcedure;

         
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

            // id of created coupon
            
           ArrayList list = new ArrayList();
            while (dataReader.Read())
            {
                LabAppInfo viInfo = new LabAppInfo();
                viInfo.appID = dataReader.GetInt32(0);
                viInfo.path = dataReader.GetString(1);
                viInfo.application = dataReader.GetString(2);
                viInfo.page = dataReader.GetString(3);
                if (!dataReader.IsDBNull(4))
                    viInfo.title = dataReader.GetString(4);
                if (!dataReader.IsDBNull(5))
                    viInfo.description = dataReader.GetString(5);
                if (!dataReader.IsDBNull(6))
                    viInfo.extraInfo = dataReader.GetString(6);
                if (!dataReader.IsDBNull(7))
                    viInfo.contact = dataReader.GetString(7);
                if (!dataReader.IsDBNull(8))
                    viInfo.comment = dataReader.GetString(8);
                viInfo.width = dataReader.GetInt32(9);
                viInfo.height = dataReader.GetInt32(10);
                if (!dataReader.IsDBNull(11))
                    viInfo.dataSources = dataReader.GetString(11);
                if (!dataReader.IsDBNull(12))
                    viInfo.server = dataReader.GetString(12);
                if (!dataReader.IsDBNull(13))
                    viInfo.port = dataReader.GetInt32(13);
                if (!dataReader.IsDBNull(14))
                    viInfo.appURL = dataReader.GetString(14);
                if (!dataReader.IsDBNull(15))
                    viInfo.version = dataReader.GetString(15);
                if (!dataReader.IsDBNull(16))
                    viInfo.rev = dataReader.GetString(16);
                if (!dataReader.IsDBNull(17))
                    viInfo.appKey = dataReader.GetString(17);
                if (!dataReader.IsDBNull(18))
                    viInfo.appGuid = dataReader.GetString(18);
                list.Add(viInfo);
            }
            LabAppInfo[] labs = new LabAppInfo[list.Count];
            for(int i= 0;i<list.Count;i++){
                labs[i] = ( LabAppInfo) list[i];
            }
            return labs;

            
        }
 
        public LabAppInfo GetLabAppForGroup(string groupName, string serviceGUID)
		{
			DbConnection connection =  FactoryDB.GetConnection();
			// create sql command
            DbCommand cmd = FactoryDB.CreateCommand("GetLabAppByGroup", connection);
			cmd.CommandType = CommandType.StoredProcedure;

			// populate parameters
            cmd.Parameters.Add(FactoryDB.CreateParameter( "@groupName", groupName,DbType.String, 256));
			cmd.Parameters.Add(FactoryDB.CreateParameter("@guid", serviceGUID, DbType.AnsiString,50));
            
			DbDataReader dataReader = null;
			try 
			{
                connection.Open();
				dataReader = cmd.ExecuteReader();
			} 
			catch (DbException e) 
			{
				writeEx(e);
				throw e;
			}

			LabAppInfo viInfo = new LabAppInfo();
			while (dataReader.Read()) 
			{
				viInfo.appID = dataReader.GetInt32(0);
                viInfo.path = dataReader.GetString(1);
                viInfo.application = dataReader.GetString(2);
                if (!dataReader.IsDBNull(3))
                    viInfo.page = dataReader.GetString(3);
                if (!dataReader.IsDBNull(4))
                    viInfo.title = dataReader.GetString(4);
                if (!dataReader.IsDBNull(5))
                    viInfo.description = dataReader.GetString(4);
                if (!dataReader.IsDBNull(6))
                    viInfo.extraInfo = dataReader.GetString(6);
                if (!dataReader.IsDBNull(7))
                    viInfo.contact = dataReader.GetString(7);
                if (!dataReader.IsDBNull(8))
                    viInfo.comment = dataReader.GetString(8);
                viInfo.width = dataReader.GetInt32(9);
                viInfo.height = dataReader.GetInt32(10);
                if (!dataReader.IsDBNull(11))
                    viInfo.dataSources = dataReader.GetString(11);
                if (!dataReader.IsDBNull(12))
                    viInfo.server = dataReader.GetString(12);
                if (!dataReader.IsDBNull(13))
                    viInfo.port = dataReader.GetInt32(13);
                if (!dataReader.IsDBNull(14))
                    viInfo.appURL = dataReader.GetString(14);
                if (!dataReader.IsDBNull(15))
                    viInfo.version = dataReader.GetString(15);
                if (!dataReader.IsDBNull(16))
                    viInfo.rev = dataReader.GetString(16);
                if (!dataReader.IsDBNull(17))
                    viInfo.appKey = dataReader.GetString(17);
                if (!dataReader.IsDBNull(18))
                    viInfo.appGuid = dataReader.GetString(18);
			}

			// close the sql connection
			connection.Close();
			
			return viInfo;
		}

        public LabTask InsertTask(int app_id, long exp_id, string groupName, DateTime startTime, long duration, LabTask.eStatus status,
			long coupon_ID,string issuerGuidStr, string data)
		{
            LabTask task = new LabTask();

			DbConnection connection =  FactoryDB.GetConnection();
			DbCommand cmd = FactoryDB.CreateCommand("InsertTask", connection);
			cmd.CommandType = CommandType.StoredProcedure;

			// populate parameters
			cmd.Parameters.Add(FactoryDB.CreateParameter("@appid", app_id, DbType.Int32));	
            if(exp_id < 1)
                cmd.Parameters.Add(FactoryDB.CreateParameter("@expid", null, DbType.Int64));
            else
                cmd.Parameters.Add(FactoryDB.CreateParameter("@expid", exp_id, DbType.Int64));
			cmd.Parameters.Add(FactoryDB.CreateParameter("@groupName", groupName,DbType.String,256));
             // This must be in UTC
			cmd.Parameters.Add(FactoryDB.CreateParameter("@startTime", startTime,DbType.DateTime));
            if (duration > 0)
                cmd.Parameters.Add(FactoryDB.CreateParameter("@endTime",startTime.AddTicks(duration * TimeSpan.TicksPerSecond), DbType.DateTime));
            else
                cmd.Parameters.Add(FactoryDB.CreateParameter("@endTime",DateTime.MinValue, DbType.DateTime));
			cmd.Parameters.Add(FactoryDB.CreateParameter("@status", status,DbType.Int32));
			cmd.Parameters.Add(FactoryDB.CreateParameter("@couponID", coupon_ID,DbType.Int64));
			cmd.Parameters.Add(FactoryDB.CreateParameter("@issuerGUID", issuerGuidStr, DbType.AnsiString,50));
			cmd.Parameters.Add(FactoryDB.CreateParameter("@data", data,DbType.String,2048));
			
            // id of created task
            long itemID = -1;

            try
            {
                connection.Open();
                itemID = Convert.ToInt64(cmd.ExecuteScalar());
            }
            catch (DbException e)
            {
                writeEx(e);
                throw e;
            }
            finally
            {
                connection.Close();
            }
            task.taskID = itemID;
            task.labAppID = app_id;
            task.experimentID = exp_id;
            task.groupName = groupName;
            task.startTime = startTime;
            if (duration > 0)
                task.endTime = startTime.AddTicks(duration * TimeSpan.TicksPerSecond);
            else
                task.endTime = DateTime.MinValue;
            task.Status = status;
            task.couponID = coupon_ID;
            task.issuerGUID = issuerGuidStr;
            task.data = data;
            return task;
		}



		public LabTask GetTask(long task_id)
		{
			DbConnection connection =  FactoryDB.GetConnection();
			// create sql command
			DbCommand cmd = FactoryDB.CreateCommand("GetTask", connection);
			cmd.CommandType = CommandType.StoredProcedure;

			// populate parameters
			cmd.Parameters.Add(FactoryDB.CreateParameter("@taskid",task_id, DbType.Int64));
			
			DbDataReader dataReader = null;
            // id of created coupon
            LabTask taskInfo = new LabTask();
            try
            {
                connection.Open();
                dataReader = cmd.ExecuteReader();

                taskInfo.taskID = task_id;
                while (dataReader.Read())
                {
                    taskInfo.labAppID = dataReader.GetInt32(0);
                    if (!dataReader.IsDBNull(1))
                        taskInfo.experimentID = dataReader.GetInt64(1);
                    taskInfo.groupName = dataReader.GetString(2);
                    taskInfo.startTime = dataReader.GetDateTime(3);
                    taskInfo.endTime = dataReader.GetDateTime(4);
                    taskInfo.Status = (LabTask.eStatus)dataReader.GetInt32(5);
                    if (!DBNull.Value.Equals(dataReader.GetValue(6)))
                        taskInfo.couponID = dataReader.GetInt64(6);
                    if (!DBNull.Value.Equals(dataReader.GetValue(7)))
                        taskInfo.issuerGUID = dataReader.GetString(7);
                    if (!dataReader.IsDBNull(8))
                        taskInfo.data = dataReader.GetString(8);
                }
            }
            catch (DbException e)
            {
                writeEx(e);
                throw e;
            }
            finally
            {
                // close the sql connection
                connection.Close();
            }
			return taskInfo;
		}

        public LabTask GetTask(long experiment_id, string sbGUID)
        {
            DbConnection connection = FactoryDB.GetConnection();
            // create sql command
            DbCommand cmd = FactoryDB.CreateCommand("GetTaskByExperiment", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            // populate parameters
            cmd.Parameters.Add(FactoryDB.CreateParameter("@experimentid", experiment_id,DbType.Int64));
            cmd.Parameters.Add(FactoryDB.CreateParameter("@sbguid", sbGUID, DbType.AnsiString,50));
                        
            DbDataReader dataReader = null;
            try
            {
                connection.Open();
                dataReader = cmd.ExecuteReader();
            }
            catch (DbException e)
            {
                writeEx(e);
                throw e;
            }

            // id of created coupon
            LabTask taskInfo = new LabTask();
            while (dataReader.Read())
            {
                taskInfo.taskID = dataReader.GetInt64(0);
                taskInfo.labAppID = dataReader.GetInt32(1);
                if (!dataReader.IsDBNull(2))
                    taskInfo.experimentID = dataReader.GetInt64(2);
                taskInfo.groupName = dataReader.GetString(3);
                taskInfo.startTime = dataReader.GetDateTime(4);
                taskInfo.endTime = dataReader.GetDateTime(5);
                taskInfo.Status = (LabTask.eStatus)dataReader.GetInt32(6);
                if (!DBNull.Value.Equals(dataReader.GetValue(7)))
                    taskInfo.couponID = dataReader.GetInt64(7);
                if (!DBNull.Value.Equals(dataReader.GetValue(8)))
                    taskInfo.issuerGUID = dataReader.GetString(8);
                if (!dataReader.IsDBNull(9))
                    taskInfo.data = dataReader.GetString(9);
            }

            // close the sql connection
            connection.Close();

            return taskInfo;
        }


/*
CREATE PROCEDURE GetActiveTasks

AS
select taskID,Status 
*/
		public LabTask [] GetActiveTasks()
		{
			DbConnection connection =  FactoryDB.GetConnection();
			// create sql command
			DbCommand cmd = FactoryDB.CreateCommand("GetActiveTasks", connection);
			cmd.CommandType = CommandType.StoredProcedure;

			DbDataReader dataReader = null;
			try 
			{
                connection.Open();
				dataReader = cmd.ExecuteReader();
			} 
			catch (DbException e) 
			{
				writeEx(e);
				throw e;
			}

			// id of created coupon
			
			ArrayList list = new ArrayList();
			while (dataReader.Read()) 
			{
				LabTask taskInfo = new LabTask();
				taskInfo.taskID = dataReader.GetInt64(0);
                taskInfo.labAppID = dataReader.GetInt32(1);
                if (!dataReader.IsDBNull(2))
				    taskInfo.experimentID = dataReader.GetInt64(2);
                if (!dataReader.IsDBNull(3))
				    taskInfo.groupName = dataReader.GetString(3);
				taskInfo.startTime= dataReader.GetDateTime(4);
                taskInfo.endTime = dataReader.GetDateTime(5);			
				taskInfo.Status= (LabTask.eStatus) dataReader.GetInt32(6);
				if(!DBNull.Value.Equals(dataReader.GetValue(7)))
				    taskInfo.couponID= dataReader.GetInt64(7);
				if(!DBNull.Value.Equals(dataReader.GetValue(8)))
				    taskInfo.issuerGUID= dataReader.GetString(8);
				if(!dataReader.IsDBNull(9))
				    taskInfo.data= dataReader.GetString(9);
				list.Add(taskInfo);
			}

			// close the sql connection
			connection.Close();
			LabTask taskInfoTemp = new LabTask();
			return  (LabTask[]) list.ToArray(taskInfoTemp.GetType());
		}

/*
CREATE PROCEDURE GetExpiredTasks
@targetTime datetime

AS
select taskID,VIID,GroupID,StartTime,endTime,Status,CouponID,IssuerID,Data 
*/
		public LabTask [] GetExpiredTasks(DateTime targetTime)
		{
			DbConnection connection =  FactoryDB.GetConnection();
			// create sql command
			DbCommand cmd = FactoryDB.CreateCommand("GetExpiredTasks", connection);
            cmd.CommandType = CommandType.StoredProcedure;
			// populate parameters
           
            cmd.Parameters.Add(FactoryDB.CreateParameter( "@targetTime", targetTime, DbType.DateTime));
		
			DbDataReader dataReader = null;
			try 
			{
                connection.Open();
				dataReader = cmd.ExecuteReader();
			} 
			catch (DbException e) 
			{
				writeEx(e);
				throw e;
			}

			// id of created coupon
			
			ArrayList list = new ArrayList();
			while (dataReader.Read()) 
			{
				LabTask taskInfo = new LabTask();
				taskInfo.taskID = dataReader.GetInt64(0);
				taskInfo.labAppID = dataReader.GetInt32(1);
                if (!dataReader.IsDBNull(2))
                taskInfo.experimentID = dataReader.GetInt64(2);
				taskInfo.groupName = dataReader.GetString(3);
				taskInfo.startTime= dataReader.GetDateTime(4);
				taskInfo.endTime= dataReader.GetDateTime(5);
                taskInfo.Status = (LabTask.eStatus)dataReader.GetInt32(6);
				if(!DBNull.Value.Equals(dataReader.GetValue(7)))
				    taskInfo.couponID= dataReader.GetInt64(7);
				if(!dataReader.IsDBNull(8))
				    taskInfo.issuerGUID= dataReader.GetString(8);
				if(!dataReader.IsDBNull(9))
				    taskInfo.data= dataReader.GetString(9);
				list.Add(taskInfo);
			}

			// close the sql connection
			connection.Close();
			LabTask taskInfoTemp = new LabTask();
			return  (LabTask[]) list.ToArray(taskInfoTemp.GetType());
		}

		public void SetTaskData(long task_id,string data)
		{
			DbConnection connection =  FactoryDB.GetConnection();
			// create sql command
			DbCommand cmd = FactoryDB.CreateCommand("SetTaskData", connection);
			cmd.CommandType = CommandType.StoredProcedure;

			// populate parameters
			cmd.Parameters.Add(FactoryDB.CreateParameter("@taskid", task_id, DbType.Int64));
			cmd.Parameters.Add(FactoryDB.CreateParameter("@data", data, DbType.String,2048));
			
            try
            {
                connection.Open();
                cmd.ExecuteNonQuery();

            }
            catch (DbException e)
            {
                writeEx(e);
                throw e;
            }
            finally
            {
                connection.Close();
            }
		}



/*
SetTaskStatus taskID, status
AS
update task set status = @status where taskID = @taskID

*/
		public void SetTaskStatus(long task_id, int status)
		{
			DbConnection connection =  FactoryDB.GetConnection();
			// create sql command
			DbCommand cmd = FactoryDB.CreateCommand("SetTaskStatus", connection);
			cmd.CommandType = CommandType.StoredProcedure;

			// populate parameters
            cmd.Parameters.Add(FactoryDB.CreateParameter( "@taskID", task_id, DbType.Int64));
			cmd.Parameters.Add(FactoryDB.CreateParameter("@status", status, DbType.Int32));
			
			DbDataReader dataReader = null;
            try
            {
                connection.Open();
                dataReader = cmd.ExecuteReader();
            }
            catch (DbException e)
            {
                writeEx(e);
                throw e;
            }
            finally
            {
                connection.Close();
            }
		}
        /*
        // Should move this into the TaskProcessor and have LabVIEW specific methods 
        // happen outside of the database class.
		int count = 0;
		public void ProcessTasks()
		{
			count++;
			if(count >= 10)
			{
				Utilities.WriteLog("ProcessTasks");
				count = 0;
			}
			
			LabTask[] tasks = GetActiveTasks();
			//LabViewInterface lvi = new LabViewInterface();
			DateTime time = DateTime.UtcNow;
			foreach(LabTask task in tasks)
			{
				
				if(time.CompareTo(task.endTime) > 0)
				{ // task has expired
					try
					{

                       Logger.WriteLine("Found expired task: " + task.taskID);
					
						if(task.data != null)
						{
							XmlQueryDoc taskDoc = new XmlQueryDoc(task.data);
							String vi = taskDoc.Query("task/viname");
							string status = taskDoc.Query("task/statusvi");
							if(status != null)
							{
								try
								{
									//lvi.DisplayStatus(status,"You are out of time!","0:00");
									//lvi.submitAction("stopvi",status);
								}
								catch(Exception ce)
								{
                                   Logger.WriteLine("Trying Status: " + ce.Message);
								}
							}
							//lvi.submitAction("lockvi",vi);
							//lvi.submitAction("stopvi",vi);
						}
                       Logger.WriteLine("TaskID = " + task.taskID + " has expired");
						SetTaskStatus(task.taskID,99);
						Coupon expCoupon = this.GetCoupon(task.couponID,task.issuerGUID);
						Coupon identCoupon = this.GetIdentityInCoupon (task.issuerGUID);

						ProcessAgentInfo issuer = GetProcessAgentInfo(task.issuerGUID);
						if(issuer == null)
						{
							//Response.Redirect("AccessDenied.aspx?text=the+specified+ticket+issuer+could+not+be+found.", true);
						}
				
						// Create ticketing service interface connection to TicketService
						ITicketIssuer_Proxy ticketingInterface = new ITicketIssuer_Proxy();
						ticketingInterface.AgentAuthHeaderValue.coupon = identCoupon;
                        ticketingInterface.AgentAuthHeaderValue.agentGuid = ConfigurationManager.AppSettings["ServiceGuid"];
						ticketingInterface.Url = issuer.webServiceUrl;
						if(ticketingInterface.RequestTicketCancellation(expCoupon,TicketTypes.EXECUTE_EXPERIMENT,ConfigurationManager.AppSettings["ServiceGuid"]))
						{
							// Or should this be cancelled from the TicketIssuer
							CancelTicket(expCoupon,TicketTypes.EXECUTE_EXPERIMENT,ConfigurationManager.AppSettings["ServiceGuid"]);
			
						}
						else
						{
                           Logger.WriteLine("Unable to cancel ticket: " + expCoupon.couponId);
						}
					}
				
					catch(Exception e1)
					{
                       Logger.WriteLine("ProcessTasks Expired: " + e1.Message);
					}
				}
				else
				{
					try
					{
						if(task.Status == LabTask.eStatus.Running)
						{
							if(task.data != null)
							{
								XmlQueryDoc taskDoc = new XmlQueryDoc(task.data);
								string status = taskDoc.Query("task/statusvi");
								if(status != null)
								{
									try
									{
										//lvi.DisplayStatus(status,time.ToString() , task.endTime.Subtract(time).ToString());
									}
									catch(Exception ce2)
									{
                                       Logger.WriteLine("Status: " + ce2.Message);
									}
								}
							}
						}
					}
					catch(Exception e2)
					{
                       Logger.WriteLine("ProcessTasks Status: " + e2.Message);
					}
				}
			}
		}
*/
        public LabTask.eStatus ExperimentStatus(long id, string issuer)
        {
            LabTask.eStatus status = LabTask.eStatus.NotFound;
            int result = -10;
            DbConnection connection = FactoryDB.GetConnection();
            // create sql command
            // command executes the "GetExperimentStatus" stored procedure
            DbCommand cmd = FactoryDB.CreateCommand("GetTaskStatusByExperiment", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            // populate parameters
            cmd.Parameters.Add(FactoryDB.CreateParameter("@id", id, DbType.Int64));
            cmd.Parameters.Add(FactoryDB.CreateParameter("@guid", issuer, DbType.AnsiString, 50));
            
            DbDataReader dataReader = null;
            try
            {
                connection.Open();
                dataReader = cmd.ExecuteReader();

                int count = 0;
                while (dataReader.Read())
                {
                    status = (LabTask.eStatus)dataReader.GetInt32(0);
                    count++;
                }
               Logger.WriteLine("ExperimentStatus count: " + count);
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


	}
}
