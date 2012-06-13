/*
 * Copyright (c) 2004-2006 The Massachusetts Institute of Technology. All rights reserved.
 * Please see license.txt in top level directory for full license.
 */

/* $Id$ */
#define LabVIEW_86
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Web;

using System.Runtime.InteropServices;

//using iLabs.DataTypes.SoapHeaderTypes;
//using iLabs.DataTypes.TicketingTypes;
//using iLabs.Services;
using iLabs.UtilLib;
using iLabs.LabView;

// Specify the LabView application version and application via these resources

//using LabVIEW.lv86;
using CWDataServer;
using CWDSLib;

#if LabVIEW_86
using LabVIEW;
namespace iLabs.LabView.LV86
#endif
#if LabVIEW_82
using LabVIEW;
namespace iLabs.LabView.LV82
#endif
{

	/// <summary>
	/// Summary description for LabViewInterface. This version is an attempt to use a stand-alone LabView Application/Service.
	/// As the Application will require the automatic loading of several VI's this implementation should be 
	/// somewhat faster and simpler than using the standard LabVIEW exe. Also hope to make the Interface 
	/// lighter weight as to constructer start-up. long term goal allow for constructer arguments, to access remote systems.
    /// <br />Note: many changes have been required to support the addition of Libary Name to the GetVIReference calling parameter.
	/// </summary>
	public class LabViewInterface : I_LabViewInterface
	{
 	
	public static bool CheckDsServer()
        {
            bool status = false;
            try
            {
                CWDataServer.CWDataServer server = new CWDataServer.CWDataServer();
                if (server != null)
                {
                    status = true;
                }
            }
            catch (Exception e)
            {
                Logger.WriteLine("Error accessing DataSocketServer: " + e.Message);
            }
            return status;
        }

		protected _Application viServer;

        protected string appDir = null;
        protected string viPath = @"\user.lib\iLabs";
	
		/// <summary>
		/// Connect to the local VI application,  the 'LabView' reference will detremine 
		/// what version and LabView application will be the target
		/// </summary>
		public LabViewInterface()
		{	
			viServer = new ApplicationClass();
            appDir = viServer.ApplicationDirectory;
			viServer.AutomaticClose = false;
		}

	public string GetLabViewVersion(){
	return viServer.Version.ToString();
}
        public virtual string GetViServerStatus()
        {
            return GetViServerStatus(viServer);
        }

		protected internal string GetViServerStatus(_Application app)
		{
			StringBuilder message = new StringBuilder("AppStatus: ");
			try
			{				
				if(app != null)
				{
					//viServer.CheckConnection();				
					//message.Append(app.ToString());
                    message.Append("\tAppName: " + app.AppName);
                    message.Append("\tDirectory: " + app.ApplicationDirectory);
					message.Append("\tVersion: " + app.Version);
					message.Append("\tUserName: " +app.UserName);
					message.Append("\tProcessID: " + app._ProcessID);
				}
				else
				{
					message.Append("labViewApp = null");
				}
			}
			catch(Exception ex)
			{
				message.Append("Error: " + ex.Message + ex.StackTrace);
				throw(ex);
			}
			message.ToString();
			return message.ToString();
		}

        protected string stripName(string name)
        {
            string viname = null;
            if (name.Contains("\\") | name.Contains(":"))
            {
                char[] key = { '\\', ':' };
                string[] parts = name.Split(key, StringSplitOptions.None);
                int i = parts.Length;
                if (i > 0)
                    viname = parts[i - 1];
            }
            else
            {
                viname = name;
            }
            return viname;
        }

        public string qualifiedName(VirtualInstrument vi)
        {
            string qName = null;
            if (vi != null)
            {
                StringBuilder buf = new StringBuilder();
                string libraryName = null;
                try
                {
                    Library lib = vi.Library;
                    Project proj = null;
                    if (lib != null)
                    {
                        libraryName = lib.Name;
                        proj = lib.Project;
                        if (proj != null)
                        {
                            if ((proj.Name != null) && (proj.Name.Length > 0))
                                buf.Append(proj.Name + "/");
                            if ((proj.MyComputer != null) && (proj.MyComputer.Name != null) && (proj.MyComputer.Name.Length > 0))
                                buf.Append(proj.MyComputer.Name + "/");
                        }
                        if ((libraryName != null) && libraryName.Length > 0)
                        {
                            buf.Append(libraryName + ":");
                        }
                    }
                }
                catch (InvalidCastException ivcEx)
                {
                    Utilities.WriteLog("Library error: " + ivcEx.Message);
                }
                buf.Append(vi.Name);
                qName = buf.ToString(); 
   
            }
            return qName;
        }

        public virtual int GetVIStatus(String viName)
        {
            int status = -10;
            if (IsLoaded(viName))
            {
                VirtualInstrument vi = GetVI(viName);
                status = (int)GetVIStatus(vi);
                vi = null;
            }
            else
            {
                status = -1;
            }
            return status;
        }
        public int GetVIStatus(VirtualInstrument vi)
        {
            // Not loaded == -1
            int status = -1;

            if (vi != null)
            {
                ExecStateEnum state = vi.ExecState;
                status = (int)state;
            }
            return status;
        }

        public int GetFPState(VirtualInstrument vi)
        { 
            int status = -1;
            if (vi != null)
            {
                FPStateEnum state = vi.FPState;
                status = (int)state;
            }
            return status;
        }
       

        public virtual int GetLockState(string viName)
        {
            int status = -1;
            VirtualInstrument vi = GetVI(viName);
            if (vi != null)
            {
                status = GetLockState(vi);
                vi = null;
            }
            return status;
        }

        public  int GetLockState(VirtualInstrument vi)
        {
            bool cached = false;
            int status = -1;
            if (vi != null)
            {
                status = ( int) vi.GetLockState(out cached);   
            }
            return status;
        }

        public virtual int SetLockState(string viName, Boolean state)
        {
            if (state)
            {
                SubmitAction("lockvi", viName);
            }
            else
            {
                SubmitAction("unlockvi", viName);
            }
            return GetLockState(viName);
        }

        public virtual int SetLockState(VirtualInstrument vi, Boolean state)
        {
            if (state)
            {
                SubmitAction("lockvi", qualifiedName(vi));
            }
            else
            {
                SubmitAction("unlockvi", qualifiedName(vi));
            }
            return GetLockState(vi);

        }


        public int SetBounds(string viName, int left, int top, int right, int bottom)
        {
            int status = -1;
            VirtualInstrument vi = null;
            vi = GetVI(viName);
            status = SetBounds(vi,left,top,right,bottom);
            vi = null;
            return status;
        }

        public int SetBounds(VirtualInstrument vi, int left, int top, int right, int bottom)
        {
            int status = -1;
            if (vi != null)
            {

                object[] bounds = new object[4];
                bounds[0] = (object)left;
                bounds[1] = (object)top;
                bounds[2] = (object)right;
                bounds[3] = (object)bottom;
                vi.FPWinBounds = (object)bounds;
                status = 1;
             }
             return status;
        }
        public bool HasControl(VirtualInstrument vi, string name)
        {
            bool status = false;
            object value = null;
            try
            {
                value = vi.GetControlValue(name);
                status = true;
            }
            catch (Exception e)
            {
                Utilities.WriteLog(name + " control not found: " + e.Message);
            }
            return status;
        }

        public object GetControlValue(VirtualInstrument vi, string name)
        {
            object value = null;
            if (vi != null)
            {
          
                try
                {
                    value = vi.GetControlValue(name);
                    
                }
                catch (Exception e)
                {
                    Utilities.WriteLog(name + " control not found: " + e.Message);
                    throw new Exception(name + " control not found:", e);
                }
            }
            return value;
        }
        public int SetControlValue(VirtualInstrument vi,string name, object value)
        {
            int status = -1;
            if (vi != null)
            {
                object state = null;
                bool found = false;
                try
                {
                    state = vi.GetControlValue(name);
                    if (state != null)
                    {
                        status = 0;
                        found = true;
                    }
                }
                catch (Exception e)
                {
                    Utilities.WriteLog(name + " control not found: " + e.Message);
                }
                if (found)
                {
                    vi.SetControlValue(name, value);
                    status = (int)GetVIStatus(vi);
                }
            }
            return status;
        }

        public int ResetVI(string viName)
        {
            int status = -1;
            VirtualInstrument vi = GetVI(viName);

            if (vi != null)
            {
                status = ResetVI(vi);
                vi = null;
            }
            return status;
        }


        public int ResetVI(VirtualInstrument vi)
        {
            int status = -1;
            if (vi != null)
            {
                vi.ReinitializeAllToDefault();
                status = (int)GetVIStatus(vi);
             }
             return status;
        }

        public int RunVI(string viName)
        {
            int status = -1;
            VirtualInstrument vi = GetVI(viName);
            if (vi != null)
            {
                status = RunVI(vi);
                vi = null;
            }
            return status;
        }

        public int RunVI(VirtualInstrument vi)
        {
            int status = -1;
            if (vi != null)
            {
                vi.OpenFrontPanel(true, FPStateEnum.eVisible);
                vi.Run(true);
                status = (int)GetVIStatus(vi);
            }
            return status;
        }

        public int StopVI(string viName)
        {
            int status = -1;
            VirtualInstrument vi = GetVI(viName);
            if (vi != null)
            {
                status = StopVI(vi);
                vi = null;
            }
            return status;
        }

        public int StopVI(VirtualInstrument vi)
        {
            int status = -1;
            if (vi != null)
            {
                status = 0;
                object state = null;
                bool found = false;
                try
                {
                    state = vi.GetControlValue("stop");
                    if (state != null)
                    {
                        status = 1;
                        found = true;
                    }
                    else
                    {
                        Utilities.WriteLog("stop control not found: null returned");
                    }
                }
                catch (Exception e)
                {
                    Exception stopEx = new Exception("Control NotFound: stop", e);
                    Utilities.WriteLog("stop control not found: " + e.Message);
                    throw stopEx;
                }
                if (found)
                {
                    try
                    {
                        vi.SetControlValue("stop", true);
                        Thread.Sleep(10);
                        vi.SetControlValue("stop", false);
                        Utilities.WriteLog("stopping VI: " + vi.Name + " status=" + (int)GetVIStatus(vi));
                        status = 2;
                    }
                    catch (Exception ex)
                    {
                        Exception setControl = new Exception("setControl: stop", ex);
                        Utilities.WriteLog("Error: setControl stop: " + ex.Message);
                        throw setControl;
                    }
                }
            }
            return status;
        }

        public int CloseVI(string viName)
        {
            int status = -1;
            VirtualInstrument vi = GetVI(viName);
            if (vi != null)
            {
                status = CloseVI(vi);
                vi = null;
            }
            return status;
        }

        public int CloseVI(VirtualInstrument vi)
        {
            int status = -1;
            if (vi != null)
            {
                StopVI(vi);
                
                vi.Abort();
                vi.CloseFrontPanel();
                status = (int)GetVIStatus(vi);
            }
            return status;
        }


		public virtual string QuitLabView()
		{
			StringBuilder message = new StringBuilder("QuitLabview: ");
			

			if(viServer != null)
			{
				message.Append(" Process=" +viServer._ProcessID);
			
				try
				{
					viServer.Quit();
				}
				catch(Exception e)
				{
					message.Append(" ERROR on Quit(): " + e.Message);
				}
				
				viServer = null;
			}
			return message.ToString();
		}

		public virtual bool IsLoaded(string name)
		{
			bool status = false;
            string viname = stripName(name);
         
            object obj = viServer.ExportedVIs;
            string [] vis = (string[]) obj;
            for(int i = 0;i< vis.Length;i++){
                if(vis[i] == viname){
                    status = true;
                    break;
                }
            }
           
			return status;
		}

        //public virtual bool IsLoaded(string viname)
        //{
        //    bool status = false;
        //    VirtualInstrument isLoadedVI = getIsLoaded();
        //    if (isLoadedVI != null)
        //    {
        //        string[] connectors = new String[2];
        //        connectors[0] = "name";
        //        connectors[1] = "status";
        //        object param1 = (object)connectors;

        //        object[] data = new object[2];
        //        data[0] = (object)viname;
        //        object param2 = (object)data;

        //        //Call the VI
        //        isLoadedVI.Call(ref param1, ref param2);
        //        //status returned
        //        if (((object[])param2)[1] != null)
        //        {
        //            Object obj = ((object[])param2)[1];
        //            status = Convert.ToBoolean(obj);
        //        }
        //    }
        //    return status;
        //}

        public VirtualInstrument GetVI(string path, string name)
        {
            VirtualInstrument vi = null;
            try
            {
                vi = GetVI(path + @"\" + name);
            }
            catch (Exception e)
            {
                Exception notFound = new Exception("VI NotFound: " + path + @"\" + name, e);
                Utilities.WriteLog("VI not Found: " + " Path: " + path + " Name: " + name + " Exception: " + e.Message);
                throw notFound;
            }
            return vi;
        }

        public VirtualInstrument GetVI(string viName)
        {
            return GetVI(viName, false, 0);
        }

        /// <summary>
        /// Returns the VI if in memory does not try & load it from its location, 
        /// note if the location is application execution directory, it will be loaded.
        /// </summary>
        /// <param name="viName"></param>
        /// <returns></returns>
        public virtual VirtualInstrument GetVI(string viName, bool resvForCall, int options)
        {
            VirtualInstrument vi = null;
            try
            {
                vi = (VirtualInstrument)viServer.GetVIReference(viName, "", resvForCall, options);
                //string path = vi.Path;
                //Library library = vi.Library;
                //Utilities.WriteLog("GetVI Found VI: " + viName + " Path = " + path + " Library: '" + library.Name + "'");
            }
            catch (Exception e)
            {
                Exception notFound = new Exception("VI Not Found GetVI: " + viName,e);
                Utilities.WriteLog("VI Not Found GetVI: " + viName + " Exception: " + e.Message);
                throw notFound;
            }
            return vi;
        }

        public bool HasSubVI(VirtualInstrument parent, string name)
        {
            bool subvi = false;

            // This call does not work it returns an array of strings
            string[] subs = (string[]) parent.Callees;
            foreach( string viName in subs){
                if (viName.CompareTo(name) == 0)
                {
                    subvi = true;
                    break;
                }
            }

            return subvi;
        }

        public VirtualInstrument GetSubVI(VirtualInstrument parent, string name)
        {
            VirtualInstrument subvi = null;

            // This call does not work it returns an array of strings
            //string[] subs = (string[])parent.Callees;
            //subvi.
            //foreach (string viName in subs)
            //{
            //    if (viName.CompareTo(name) == 0)
            //    {
            //        subvi = true;
            //        break;
            //    }
            //}

            return subvi;
        }

        public string LoadVI(string path, string name)
        {
            VirtualInstrument vi = loadVI(path,name);
            return qualifiedName(vi);
        }

        public VirtualInstrument loadVI(string path, string name)
        {
            return loadVI(path, name, false, 0);
        }

        public VirtualInstrument loadVI(string path, string name, bool resvForCall,int options)
        {
            VirtualInstrument vi = null;
            try
            {
                if (!IsLoaded(name))
                {
                    vi = (VirtualInstrument)viServer.GetVIReference(path + @"\" + name, "", resvForCall, options);
                    //vi.OpenFrontPanel(true, FPStateEnum.eVisible);
                }
            }
            catch (Exception e)
            {
                Exception notFound = new Exception("loadVI FileStyleUriParser notFound: " + path + @"\" + name, e);
                throw notFound;
            }
            return vi;
        }

        /*
        public int LoadVI(string name)
        {
            int status = -10;
            VirtualInstrument vi = null;
            vi = GetVI(name);
            if (vi == null)
                status = -1;
            else
                status = (int)vi.ExecState;
            return status;
        }
	*/
        public int AbortVI(string viName)
        {
            int status = -1;
            if (IsLoaded(viName))
            {
                VirtualInstrument vi = GetVI(viName);
                status = AbortVI(vi);
            }
            return status;
        }
        public int AbortVI(VirtualInstrument vi)
        {
            int status = -1;
            if (vi != null)
            {
                string message = "Trying to remove " + vi.Name;

                //vi.SetLockState(LabVIEW.VILockStateEnum.eLockedNoPwdState,false,"",false);
                vi.Abort();
            }
            return status;
        }

		public int RemoveVI(string viName)
		{
			int status = -1;
			if(IsLoaded(viName)){
				VirtualInstrument vi =  GetVI(viName);
				status = RemoveVI(vi);
			}
			return status;
		}
        public int RemoveVI(VirtualInstrument vi)
		{
			int status = -1;
			if(vi != null)
			{
				string message = "Trying to remove " + vi.Name;
				
				//vi.SetLockState(LabVIEW.VILockStateEnum.eLockedNoPwdState,false,"",false);
				vi.Abort();
				vi.CloseFrontPanel();
				vi = null;
			}
			return status;
		}

		public bool IsLabViewOpen(){
			return( viServer != null);
		}


       
        public string CreateFromTemplate(string templatePath)
        {
            string buf = null;
            VirtualInstrument vi = createFromTemplate(templatePath);
            if (vi != null)
            {
                Library lib = vi.Library;
                if (lib != null)
                    buf = lib.Name + vi.Name;
                else
                    buf = ":" + vi.Name;
            }
            return buf;
        }

        
        protected internal VirtualInstrument createFromTemplate(string path)
        {
            VirtualInstrument vi = null;
            vi = GetVI(path);
            
            //if (vi != null)
            //{
            //    string newName = templateName + suffix + ".vi";
            //    vi.Name = newName;
            //}
            
            return vi;
        }


		public  virtual string CreateFromTemplate(string path, string templateBase, string index)
		{
            string buf = null;
			
			//Try & get the vi
			//VirtualInstrument templateVI = (VirtualInstrument) viServer.GetVIReference(@"c:Program Files\National Intruments\LabVIEW 8.2\user.lib\iLabs\ILAB_CreateFromTemplate.vi","",true,0);
            VirtualInstrument templateVI = getFromTemplate();
			if(templateVI != null)
			{
                
				string[] connectors = new String[4];
				connectors[0]="templateName";
				connectors[1]="index";
				connectors[2]="name";
				connectors[3]="path";

				object[] data = new object[4];
				data[0]= (object) templateBase;
				data[1]= (object) index;

				object param1 = (object) connectors;
				object param2= (object) data;

				//Call the VI
				templateVI.Call(ref param1,ref param2);
                
			}
            return buf;
		}
        public virtual string testNames()
        {
            StringBuilder buf = new StringBuilder();

            //Try & get the vi
            VirtualInstrument VI = (VirtualInstrument) viServer.GetVIReference(@"C:\Program Files\National Instruments\LabVIEW 8.2\vi.lib\Utility\allVIsInMemory.llb\VIMemory Get VIs in Memory.vi","",true,0);
           
            if (VI != null)
            {

                string[] connectors = new String[2];
          
                connectors[0] = "VIs in memory";
                connectors[1] = "error out";
                

                object[] data = new object[2];
                //data[0] = (object)templateBase;
                //data[1] = (object)index;

                object param1 = (object)connectors;
                object param2 = (object)data;

                //Call the VI
                VI.Call(ref param1, ref param2);
                if ( ((object[])param2)[0] != null){
                    object [] results = (object[]) ((object[])param2)[0];
                    for(int i=0; i< results.Length; i++){
                        object[] item = (object[]) results[i];
                        buf.Append("Project: " + item[0].ToString());
                        buf.Append("CompNode: " + item[1].ToString());
                        buf.Append("viName: " + item[2].ToString());
                        _Application app = (_Application) item[3];
                        buf.AppendLine(this.GetViServerStatus(app));
                    }
                }
            }
            return buf.ToString();
        }


		

		public void DisplayStatus(string statusVIName, string message,string time)
		{
			
			//Try & get the vi
			VirtualInstrument statusVI = (VirtualInstrument) viServer.GetVIReference(statusVIName,"",true,0);
		
			if(statusVI != null)
			{
				
				statusVI.SetControlValue("Status",message);
				statusVI.SetControlValue("Time Remaining",time);
				/*
				string[] connectors = new String[2];
				connectors[0]="Status";
				connectors[1]="Time Remaining";

				object[] data = new object[2];
				data[0]= (object) message;
				data[1]= (object) time;

				object param1 = (object) connectors;
				object param2= (object) data;

				//Call the VI
				statusVI.Call(ref param1,ref param2);
				*/
			}
		}


        /// <summary>
        /// SubmitAction provides access through a VIServer connection to a VI running on the target 
        /// LabVIEW application, making it possible to perform actions not directly availible via the
        /// Application object. As of LabVIEW 8.0 the behavior of the 'Open VI Reference' has changed,
        /// it is no longer possible to just use the name of the VI, but either a fully quallified string,
        /// or a LabVIEW path object with the complete local file path. The ILAB_CaseHandler vi has been changed to use the full path.
        /// </summary>
        /// <param name="actionStr"></param>
        /// <param name="valueStr">if a vi, use the full path</param>
        /// <returns>verbose message</returns>
		public string SubmitAction(string actionStr, string valueStr){
			return submitAction(actionStr,valueStr);
		}

		public virtual string submitAction(string actionStr, string valueStr)
		{	
			/*
			 * Initialize the variables and define the strings corresponding to
			 * the VI connector labels. Note the strings are case sensitive
			 **/
			StringBuilder message = new StringBuilder(actionStr + ": ");
            if(valueStr != null)
                message.Append(" Data=" + valueStr);
			string dataStr = valueStr;
            VirtualInstrument actionVI = getCaseHandler();
			if(actionVI != null)
			{
               
               string[] connectors = new String[4];
               object[] data = new object[4];
               
			connectors[0]="action";
			connectors[1]="data";
			connectors[2]="response";
			connectors[3]="errorOut";

			//The wrapper function expects to be passed a object type by reference.
			//We pass the string array to the object type here
			object param1 = (object) connectors;

			//Define the variable that will pass the expression to be evaluated to 
			//LabVIEW and typecast it to type object

			
			data[0]= (object) actionStr;
            if (dataStr == null)
                data[1] = (object) "";
            else
			    data[1]= (object) dataStr;
			object param2= (object) data;
			
			
				//Call the VI
				actionVI.Call(ref param1,ref param2);
				//Display the result
				//Data returned
				if(((object[])param2)[2] != null)
					message.Append(" response: " + ((object[])param2)[2].ToString());
				//Error returned
				if(((object[])param2)[3] != null)
					message.Append(" Error: " + ((object[])param2)[3].ToString());
				
			}
			else
			{
				message.Append(" ERROR: actionVI not found");
			}
            Utilities.WriteLog(message.ToString());
			return message.ToString();
		
		}

        public virtual string submitRemoteCommand(string actionStr, string valueStr,
            VirtualInstrument viRef)
        {
            return submitRemoteCommand(actionStr, valueStr, viServer, viRef);
        }

        public virtual string submitRemoteCommand(string actionStr, string valueStr,
            _Application appRef, VirtualInstrument viRef)
        {
            /*
             * Initialize the variables and define the strings corresponding to
             * the VI connector labels. Note the strings are case sensitive
             **/
            StringBuilder message = new StringBuilder(actionStr + ": ");
            if (valueStr != null)
                message.Append(" Data=" + valueStr);
            string dataStr = valueStr;
            VirtualInstrument actionVI =  this.getRemoteCommandVI();
            if (actionVI != null)
            {

                string[] connectors = new String[6];
                object[] data = new object[6];

                connectors[0] = "action";
                connectors[1] = "data";
                connectors[2] = "response";
                connectors[3] = "errorOut";
                connectors[4] = "appRef";
                connectors[5] = "viRef";
                

                //The wrapper function expects to be passed a object type by reference.
                //We pass the string array to the object type here
                object param1 = (object)connectors;

                //Define the variable that will pass the expression to be evaluated to 
                //LabVIEW and typecast it to type object


                data[0] = (object)actionStr;
                if (dataStr == null)
                    data[1] = (object)"";
                else
                    data[1] = (object)dataStr;
                
                if (appRef != null)
                {
                    data[4] = appRef;
                }

                if (viRef != null)
                {
                    data[5] = viRef;
                }
                object param2 = (object)data;
                //Call the VI
                actionVI.Call(ref param1, ref param2);
                //Display the result
                //Data returned
                if (((object[])param2)[2] != null)
                    message.Append(" response: " + ((object[])param2)[2].ToString());
                //Error returned
                if (((object[])param2)[3] != null)
                    message.Append(" Error: " + ((object[])param2)[3].ToString());

            }
            else
            {
                message.Append(" ERROR: RemoteCommandMgr not found");
            }
            Utilities.WriteLog(message.ToString());
            return message.ToString();

        }

		public virtual string GetLabConfiguration(string group)
		{
			StringBuilder message = new StringBuilder("getConfiguration: ");
			try
			{
				if(viServer != null)
				{
					message.Append(submitAction("listvis",""));
				}
			}
			catch(Exception ex)
			{
				message.Append("<pre>Error: " + ex.Message + ex.StackTrace + "</pre>");
			}
			return message.ToString();
		}


        VirtualInstrument getCGI()
        {
            return (VirtualInstrument)viServer.GetVIReference(appDir + @"\www\cgi-bin\ILAB_FrameContentCGI.vi", "", true, 0);
            
        }

        VirtualInstrument getCaseHandler()
        {
            return getILabVI("ILAB_CaseHandlerNoPath.vi");
           
        }

                VirtualInstrument getVIStatus()
        {
            return getILabVI("ILAB_VIStatus.vi");
           
        }

                VirtualInstrument getIsLoaded()
        {
            return getILabVI("ILAB_IsLoaded.vi");
            
        }

        VirtualInstrument getFromTemplate()
        {
            return getILabVI("ILAB_CreateFromTemplate.vi");
           
        }

                VirtualInstrument getSetBounds()
        {
            return getILabVI("ILAB_SetBounds.vi");
           
        }

        VirtualInstrument getGetVI()
        {
            return getILabVI("ILAB_GetVI.vi");
        }

        VirtualInstrument getRemoteCommandVI()
        {
            return getILabVI("ILABs_RemoteCommandMgr.vi");
        }

        VirtualInstrument getILabVI(string viName)
        {
            VirtualInstrument vi = null;
            //try
            //{
            //    vi = (VirtualInstrument)viServer.GetVIReference( "iLabs:"+viName, "", true, 0);
            //}
            //catch( Exception e1)
            //{
            //    Utilities.WriteLog("Error: " + viName + " \t" + e1.Message);
                try
                {
                    vi = (VirtualInstrument)viServer.GetVIReference(appDir + viPath + @"\" + viName, "", true, 0);
                    string path = vi.Path;
                    Library library = vi.Library;
                    Utilities.WriteLog("getILabVI Found VI: " + appDir + viPath + @"\" + viName + " Path = " + path + " Library: '" + library.LocalName + "'");
                }
                catch(Exception e2)
                {
                    Utilities.WriteLog("Error: " +  viName + " \t" + e2.Message);
                }
           // }
            return vi; 
        }

   


        /****************************************************************
        public LabViewExp prepareExperiment(Coupon expCoupon, string viPath, string viName,
             int xOffset, int yOffset, int width, int height,
             string essService, long experimentID, string dataSockets, string extra)
        {


            LabViewExp exp = new LabViewExp();
            if (((essService != null) && (essService.Length > 0)) && ((dataSockets != null) && (dataSockets.Length > 0)))
            {
                string[] sockets = dataSockets.Split(';');
                // set up an experiment storage handler
                ExperimentStorageProxy ess = new ExperimentStorageProxy();
                ess.OperationAuthHeaderValue = new OperationAuthHeader();
                ess.OperationAuthHeaderValue.coupon = expCoupon;
                ess.Url = essService;
                exp.ESS = ess;
                exp.ExperimentID = experimentID;

                // Use the experimentID as the storage parameter
                foreach (string s in sockets)
                {
                    LVDataSocket reader = new LVDataSocket();
                    reader.ExperimentID = experimentID;
                    reader.ESS = ess;
                    reader.ConnectAutoUpdate(s);
                    exp.AddDataSource(reader);
                }
            }

            int status = GetVIStatus(viName);
            //Global.WriteLog("LVPortal - VIstatus: " + status);
            switch (status)
            {
                case -10:
                    throw new Exception("Error GetVIStatus: " + status);
                    break;
                case -1:
                     // VI not in memory
                    string msg = submitAction("loadvi", viPath + @"\" + viName);
                    submitAction("unlockvi", viName);
                    //submitAction("stopvi",viName);
                    break;
                case 0:
                    // eBad = 0
                    break;
                case 1: 
                    // eIdle = 1
                    // vi in memory but not running
                    submitAction("unlockvi", viName);
                    //submitAction("stopvi",viName);
                    break;
                case 1:
                    submitAction("unlockvi", viName);
                    //submitAction("stopvi",viName);
                    break;
                case 2:
                    // eRunTopLevel = 2,
                    break;
                case 3:
                    // eRunning = 3
                    break;
                case 4:
                    break;
                default:
                    throw new Exception("Error GetVIStatus: unknown status: " + status);
                    break;
            }
            SetBounds(viName, 0, 0, width, height);
            //Global.WriteLog("SetBounds: " + viName);
            // Create the task & store in database;
            exp.VI = GetVI(viName);
            return exp;
        }

        ***************************************************************/
        /*
                public LabViewExp PrepareExperiment(Coupon expCoupon, string viPath, string viName,
                     int xOffset, int yOffset, int width, int height,
                     string essService, long experimentID, string dataSockets, string extra)
                {
                    LabViewExp exp = null;
                    VirtualInstrument vi = null;
                    vi = GetVI(viPath, viName);
                    if (vi != null)
                    {
                        exp = new LabViewExp();
                        exp.VI = vi;
                        SetBounds(vi, xOffset, yOffset, width, height);
                        SetLockState(vi, false);

                        if (((essService != null) && (essService.Length > 0)) && ((dataSockets != null) && (dataSockets.Length > 0)))
                        {
                            string[] sockets = dataSockets.Split(';');
                            // set up an experiment storage handler
                            ExperimentStorageProxy ess = new ExperimentStorageProxy();
                            ess.OperationAuthHeaderValue = new OperationAuthHeader();
                            ess.OperationAuthHeaderValue.coupon = expCoupon;
                            ess.Url = essService;
                            exp.ESS = ess;
                            exp.ExperimentID = experimentID;

                            // Use the experimentID as the storage parameter
                            foreach (string s in sockets)
                            {
                                LVDataSocket reader = new LVDataSocket();
                                reader.ExperimentID = experimentID;
                                reader.ESS = ess;
                                reader.ConnectAutoUpdate(s);
                                exp.AddDataSource(reader);
                            }
                        }
                        vi.TBShowRunButton = true;
                        //RunVI(vi);
                    }
                    return exp;

                }
               
        */

        /*
                public LabViewExp prepareExperiment(long experimentID, string viPath, string viName,
                 int xOffset, int yOffset, int width, int height, string dataSockets, string extra,
                 string essService)
                {


                    LabViewExp exp = new LabViewExp();
                    if (((essService != null) && (essService.Length > 0)) && ((dataSockets != null) && (dataSockets.Length > 0)))
                    {
                        string[] sockets = dataSockets.Split(';');
                        // set up an experiment storage handler
                        ExperimentStorageProxy ess = new ExperimentStorageProxy();
                        ess.OperationAuthHeaderValue = new OperationAuthHeader();
                        ess.OperationAuthHeaderValue.coupon = expCoupon;
                        ess.Url = essService;
                        exp.ESS = ess;
                        exp.ExperimentID = experimentID;

                        // Use the experimentID as the storage parameter
                        foreach (string s in sockets)
                        {
                            LVDataSocket reader = new LVDataSocket();
                            reader.ExperimentID = experimentID;
                            reader.ESS = ess;
                            reader.ConnectAutoUpdate(s);
                            exp.AddDataSource(reader);
                        }
                    }

                    int status = GetVIStatus(viName);
                    //Global.WriteLog("LVPortal - VIstatus: " + status);
                    switch (status)
                    {
                        case -10:
                            throw new Exception("Error GetVIStatus: " + status);
                            break;
                        case -1:
                            // VI not in memory
                            string msg = submitAction("loadvi", viPath + @"\" + viName);
                            submitAction("unlockvi", viName);
                            //submitAction("stopvi",viName);
                            break;

                        case 0:
                            // vi in memory but not running
                            submitAction("unlockvi", viName);
                            //submitAction("stopvi",viName);
                            break;
                        case 1:
                            submitAction("unlockvi", viName);
                            //submitAction("stopvi",viName);
                            break;
                        case 2:
                            break;
                        case 3:
                            break;
                        case 4:
                            break;
                        default:
                            throw new Exception("Error GetVIStatus: unknown status: " + status);
                            break;
                    }
                    SetBounds(viName, 0, 0, width, height);
                    //Global.WriteLog("SetBounds: " + viName);
                    // Create the task & store in database;
                    exp.VI = GetVI(viName);
                    return exp;
                }
        */
        /*
        public LabViewExp PrepareExperiment(long experimentID, string viPath, string viName,
         int xOffset, int yOffset, int width, int height, string dataSockets, string extra,
         ExperimentStorageProxy ess)
        {
            checkCGI();
            LabViewExp exp = new LabViewExp();
            exp.ExperimentID = experimentID;
            VirtualInstrument vi = null;
            vi = GetVI(viPath, viName);
            ExecStateEnum state = vi.ExecState;
            object inCache = false;
            VILockStateEnum lockState = vi.GetLockState(out inCache);
            if (vi != null)
            {
                exp.VI = vi;
                SetBounds(vi, xOffset, yOffset, width, height);
                

                if ((ess != null) && (dataSockets != null) && (dataSockets.Length >0))
                {
                    string[] sockets = dataSockets.Split(';');
                    // set up an experiment storage handler
                    
                    exp.ESS = ess;
                    

                    // Use the experimentID as the storage parameter
                    foreach (string s in sockets)
                    {
                        LVDataSocket reader = new LVDataSocket();
                        reader.ExperimentID = experimentID;
                        reader.ESS = ess;
                        reader.ConnectAutoUpdate(s);
                        exp.AddDataSource(reader);
                    }
                }
                vi.TBShowRunButton = true;
                vi.TBVisible = true;
                //vi.Run(true);
                //vi.RunOnOpen = true;
                //vi.OpenFrontPanel(true, FPStateEnum.eFPStandard);
                SetLockState(vi, false);
                vi.Abort();
                //RunVI(vi);
                state = vi.ExecState;
            }
            return exp;

        }
 */

	}
}
