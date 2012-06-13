/*
 * Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
 * Please see license.txt in top level directory for full license.
 * 
 * $Id$
 */


using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Serialization;

using iLabs.Ticketing;
using iLabs.DataTypes.StorageTypes;
using iLabs.DataTypes.SoapHeaderTypes;
using iLabs.DataTypes.TicketingTypes;

using iLabs.LabView;

namespace iLabs.LabServer.LabView
{

    /// <summary>
    /// This class is a LabView LabServer interface, The methods in this 
    /// WebService were defined for testing. The code has not been updated 
    /// and should only be used for debuging the state of the LabView processes.
    /// </summary>
    // [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    //[SoapDocumentService(RoutingStyle=SoapServiceRoutingStyle.RequestElement)]
    [WebService(Name = "InteractiveLabView", Namespace = "http://ilab.mit.edu/iLabs/Services", Description = "LabVIEW Lab Server"),
    WebServiceBindingAttribute(Name = "LabViewLS", Namespace = "http://ilab.mit.edu/iLabs/Services")]
    [WebServiceBinding(Name = "I_ILS", Namespace = "http://ilab.mit.edu/iLabs/Services")]
    [WebServiceBinding(Name = "IProcessAgent", Namespace = "http://ilab.mit.edu/iLabs/Services")]
    public class InteractiveLabView : InteractiveLabServer
    {

        //private LabDB dbManager;

        public InteractiveLabView()
        {
            //CODEGEN: This call is required by the ASP.NET Web Services Designer
            InitializeComponent();

            //dbManager = new LabDB();

        }

        #region Component Designer generated code

        //Required by the Web Services Designer 
        private IContainer components = null;

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion

        /**************************************************************************************************************************
		 * Lab Server API Web Service Methods
		 * ***********************************************************************************************************************/


        /// <summary>
        /// Checks on the status of the lab server.  
        /// </summary>
        [WebMethod(Description = @"Submits an action to the Labview Interface: loadvi, closevi, publishvi, monitorvi, disconnectuser, listvis. C:\Program Files\National Instruments\LabVIEW 7.1\examples\apps\tankmntr.llb\Tank Simulation.vi"),
            //SoapHeader("authHeader", Direction=SoapHeaderDirection.In), 
        SoapDocumentMethod(Binding = "LabViewLS")]
        public string SubmitCommand(string actionStr, string dataStr)
        {
            return LabViewTaskFactory.GetLabViewInterface().SubmitAction(actionStr, dataStr);
        }
        /*
                /// <summary>
                /// Checks on the status of the lab server.  
                /// </summary>
                [WebMethod(Description = @"Submits an action to the Labview Interface: loadvi, closevi, publishvi, monitorvi, disconnectuser, listvis. C:\Program Files\National Instruments\LabVIEW 7.1\examples\apps\tankmntr.llb\Tank Simulation.vi"),
                    //SoapHeader("authHeader", Direction=SoapHeaderDirection.In), 
                SoapDocumentMethod(Binding = "LabViewLS")]
                public string SubmitCommandRemote(string actionStr, string dataStr, string host, int port)
                {
                    return new LabViewRemote(host,port).SubmitAction(actionStr, dataStr);
                }
         */

        /// <summary>
        /// Reports the status of the lab server.  
        /// </summary>
        [WebMethod(Description = "Lab Server Method: Returns the current status of the VIServer connection. "
        + "Members of struct LabStatus are 'online' (boolean) and 'labStatusMessage' (string)."),
       SoapHeader("agentAuthHeader", Direction = SoapHeaderDirection.In),
        SoapDocumentMethod(Binding = "LabViewLS")]
        public string GetLabViewStatus()
        {
            return LabViewTaskFactory.GetLabViewInterface().GetViServerStatus();

        }

        /*
                /// <summary>
                /// Checks on the status of the lab server.  
                /// </summary>
                [WebMethod(Description = "Lab Server Method: Returns the current status of the VIServer connection. "
                + "Members of struct LabStatus are 'online' (boolean) and 'labStatusMessage' (string)."),
               SoapHeader("agentAuthHeader", Direction = SoapHeaderDirection.In),
                SoapDocumentMethod(Binding = "ILabServer")]
                public LabStatus GetLabStatus()
                {
                    return getLabStatus();

                }

        
                protected LabStatus getLabStatus()
                {
                    string report = new LabViewInterface().GetViServerStatus();
                    LabStatus status = new LabStatus();
                    status.online = true;
                    status.labStatusMessage = report;
                    return status;

                }


                [WebMethod(Description = @"Submits an action to the Labview Interface: loadvi, closevi, publishvi, monitorvi, disconnectuser, listvis. C:\Program Files\National Instruments\LabVIEW 7.1\examples\apps\tankmntr.llb\Tank Simulation.vi"),
                    //SoapHeader("authHeader", Direction=SoapHeaderDirection.In), 
               SoapDocumentMethod(Binding = "LabViewLS")]
                public LabStatus getLabStatusRemote(String host, int port)
                {
                    string report = new LabViewRemote(host, port).GetViServerStatus();
                    LabStatus status = new LabStatus();
                    status.online = true;
                    status.labStatusMessage = report;
                    return status;

                }
        */
        [WebMethod(Description = @"GetLockState"),
            //SoapHeader("authHeader", Direction=SoapHeaderDirection.In), 
SoapDocumentMethod(Binding = "LabViewLS")]
        public int GetLockState(string viName)
        {
            I_LabViewInterface lvi = LabViewTaskFactory.GetLabViewInterface();
            return lvi.GetLockState(viName);

        }
        [WebMethod(Description = @"SetLockState"),
            //SoapHeader("authHeader", Direction=SoapHeaderDirection.In), 
      SoapDocumentMethod(Binding = "LabViewLS")]
        public int SetLockState(string viName, bool state)
        {
            I_LabViewInterface lvi = LabViewTaskFactory.GetLabViewInterface();
            return lvi.SetLockState(viName, state);

        }
        /*
                [WebMethod(Description = @"QuitLabView"),
                    //SoapHeader("authHeader", Direction=SoapHeaderDirection.In), 
        SoapDocumentMethod(Binding = "LabViewLS")]
                public string QuitLabView()
                {
                    I_LabViewInterface lvi = new LabViewInterface();
                    return lvi.QuitLabView();

                }
        */

        //protected string getConfiguration(string group)
        //{
        //    return LabViewTaskFactory.GetLabViewInterface().GetLabConfiguration(group);

        //}

    } // END OF LabViewLS Class
}



