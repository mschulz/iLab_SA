/*
 * Copyright (c) 2004-2006 The Massachusetts Institute of Technology. All rights reserved.
 * Please see license.txt in top level directory for full license.
 */

/* $Id$ */

using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Web;

using System.Runtime.InteropServices;

namespace iLabs.LabView
{

    /// <summary>
    /// Summary description for I_LabViewInterface. This version is an attempt to use a stand-alone LabView Application/Service.
    /// As the Application will require the automatic loading of several VI's this implementation should be 
    /// somewhat faster and simpler than using the standard LabVIEW exe. Also hope to make the Interface 
    /// lighter weight as to constructer start-up. long term goal allow for constructer arguments, to access remote systems.
    /// </summary>
    public interface I_LabViewInterface
    {
        /// <summary>
        /// Sends messages to the status and remaining time fields of the named VI, 
        /// it must be in memory and publish the controls as connectors.
        /// </summary>
        /// <param name="statusVIName"></param>
        /// <param name="message"></param>
        /// <param name="time"></param>
        void DisplayStatus(string statusVIName, string message, string time);

        /// <summary>
        /// Simple status of the vi server's application
        /// </summary>
        /// <returns></returns>
        string GetViServerStatus();

        /// <summary>
        /// The local LabVIEW version
        /// </summary>
        /// <returns></returns>
        string GetLabViewVersion();

        bool IsLabViewOpen();

        /// <summary>
        /// Tries to kill the running labVIEW process, should be used with care.
        /// </summary>
        /// <returns></returns>
        string QuitLabView();

        /// <summary>
        /// Sends a command and targetData to the ILAB_CaseHandler.vi
        /// </summary>
        /// <param name="actionStr"></param>
        /// <param name="valueStr"></param>
        /// <returns>a status string</returns>
        string SubmitAction(string actionStr, string valueStr);

        /// <summary>
        /// Checks if the Exported ( i.e. FrontPanel VI ) is loaded
        /// </summary>
        /// <param name="viname">Library:viName.vi format or just the viName.vi format.</param>
        /// <returns></returns>
        bool IsLoaded(string viname);

        /// <summary>
        /// Tests if VI is in memory, if so returns executionState
        /// </summary>
        /// <param name="viName">Library:viName.vi format or just the viName.vi format.</param>
        /// <returns>-10 on error, -1 not in memory, or executionState</returns>
        int GetVIStatus(String viName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="templatePath">full directory path</param>
        /// <param name="templateBase">template name without '.vit'</param>
        /// <param name="suffix">added to base name</param>
        /// <returns>The created viName, may include 'Library:' if LabView 8.0 or greater.</returns>
        string CreateFromTemplate(string templatePath, string templateBase, string suffix);

        /// <summary>
        /// Forces the stopping of a VI, by calling abort, should only be used if StopVI fails, does not close FrontPanel.
        /// </summary>
        /// <param name="viName">Library:viName.vi format or just the viName.vi format.</param>
        int AbortVI(string viName);

        /// <summary>
        /// Stops a running vi and closes the front panel, this may remove the VI from memory if no other references exist.
        /// </summary>
        /// <param name="viName">Library:viName.vi format or just the viName.vi format.</param>
        int CloseVI(string viName);

        /// <summary>
        /// Loads the specified VI and opens the front panel, if the 
        /// panel is already in memory it is not reloaded.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="viName"></param>
        /// <returns>The in memeory name, may include 'Library:' if LabView 8.0 or greater.</returns>
        string LoadVI(string path, string viName);
        /*
        /// <summary>
        /// Loads the specified VI and opens the front panel, if the 
        /// panel is already in memory it is not reloaded.
        /// </summary>
        /// <param name="viName"></param>
        /// <returns></returns>
        int LoadVI(string viName);
        */
        /// <summary>
        /// Attempt to reset all default values.
        /// </summary>
        /// <param name="viName"></param>
        int ResetVI(string viName);

        /// <summary>
        /// Attempt to close all references to vi causing it to go out of memory.
        /// </summary>
        /// <param name="viName"></param>
        /// <returns>Library:viName.vi format or just the viName.vi format.</returns>
        int RemoveVI(string viName);

        /// <summary>
        ///  Start the Front Panel, vi must be in memory.
        /// </summary>
        /// <param name="viName">Library:viName.vi format or just the viName.vi format.</param>
        int RunVI(string viName);

        /// <summary>
        /// Stops the running of the VI by trying to set the value of an internal contol 
        /// named 'stop' to true, does not close FrontPanel.
        /// </summary>
        /// <param name="viName">Library:viName.vi format or just the viName.vi format.</param>
        int StopVI(string viName);

        /// <summary>
        /// Set the VI's displayed rectangle.
        /// </summary>
        /// <param name="viName">Library:viName.vi format or just the viName.vi format.</param>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <param name="right"></param>
        /// <param name="bottom"></param>
        int SetBounds(string viName, int left, int top, int right, int bottom);

        /// <summary>
        /// Get current RemotePanel 'lock' state. This is not the VI lock state which refers to the edit state.
        /// </summary>
        /// <param name="viName">Library:viName.vi format or just the viName.vi format.</param>
        /// <returns></returns>
        int GetLockState(string viName);

        /// <summary>
        /// Force the server to change what is in control of the RemotePanel.
        /// </summary>
        /// <param name="viName">Library:viName.vi format or just the viName.vi format.</param>
        /// <param name="state">true forces control back to the server, 
        /// false lets the next user 'request control' to take effect.</param>
        int SetLockState(string viName, Boolean state);
       
      
    }
}
