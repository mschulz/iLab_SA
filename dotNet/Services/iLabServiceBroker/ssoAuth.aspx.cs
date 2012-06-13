/*
 * Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
 * Please see license.txt in top level directory for full license.
 * 
 * $Id$
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Security;
using System.Xml;

using iLabs.Core;
using iLabs.DataTypes.ProcessAgentTypes;
using iLabs.DataTypes.SchedulingTypes;
using iLabs.DataTypes.SoapHeaderTypes;
using iLabs.DataTypes.TicketingTypes;
using iLabs.ServiceBroker;
using iLabs.ServiceBroker.Administration;
using iLabs.ServiceBroker.Authentication;
using iLabs.ServiceBroker.Authorization;
using iLabs.ServiceBroker.Internal;
using iLabs.Ticketing;
using iLabs.UtilLib;

using iLabs.Proxies.USS;

using iLabs.Ticketing;
using iLabs.DataTypes.TicketingTypes;

namespace iLabs.ServiceBroker.iLabSB
{
    /// <summary>
    /// ssoAuth - Single Sign On Authorization, currently this is a work around since there are no supported 3rd party authorization services.
    /// Query properties: auth - authorization guid, key - optional key, usr - userName, grp - groupName, cid - clientGUID
    /// On initial call each query property is checked for,
    /// If the call is authorized try & retrieve the session either from existing session state of via the session cookie.
    /// If needed display page with login fields; process to create authorization and session cookies and capture users TZ.
    /// Depending on the properties specified and session state make  best-case selection of the action to take.
    /// If have user, group & client try & launch client
    /// if hae user & client try & resolve group, then launch client.
    /// If group &/or client can not be resolved go to appropriate page.
    /// </summary>
    /// 
    public partial class ssoAuth : System.Web.UI.Page
    {
        AuthorizationWrapperClass wrapper = new AuthorizationWrapperClass();
        string supportMailAddress = ConfigurationManager.AppSettings["supportMailAddress"];
        int uTZ = -2000;

        BrokerDB brokerDB = new BrokerDB();

        //DateTime startExecution;
        //long duration = -1;

        protected void Page_Load(object sender, System.EventArgs e)
        {          
            if (!IsPostBack)
            {
                //Session.Remove("userTZ");
                lblMessages.Text = "";
                lblMessages.Visible = false;
                hdnAuthority.Value = Request.QueryString["auth"];
                hdnKey.Value = Request.QueryString["key"];
                hdnUser.Value = Request.QueryString["usr"];
                hdnGroup.Value = Request.QueryString["grp"];
                if (hdnGroup.Value != null && hdnGroup.Value.Length > 0)
                {
                    int g_id = AdministrativeAPI.GetGroupID(hdnGroup.Value);
                    if( g_id <= 0){
                        lblMessages.Text = "The specified group does not exist!";
                        lblMessages.Visible = true;
                        return;
                    }
                }
                hdnClient.Value = Request.QueryString["cid"];
                if (hdnClient.Value != null && hdnClient.Value.Length > 0)
                {
                     int c_id = AdministrativeAPI.GetLabClientID(hdnClient.Value);
                    if( c_id <= 0){
                        lblMessages.Text = "The specified client does not exist!";
                        lblMessages.Visible = true;
                        return;
                    }
                }
                StringBuilder sbScript = new StringBuilder();
                sbScript.AppendLine("<script language='JavaScript' type='text/javascript'>");
                sbScript.AppendLine("<!--");
                sbScript.AppendLine((this.ClientScript.GetPostBackEventReference(this, "PBArg") + ";"));
                sbScript.AppendLine("// -->");
                sbScript.AppendLine("</script>");
                this.RegisterStartupScript("AutoPostBackScript", sbScript.ToString());
            }
            if (Session["userTZ"] == null || Session["userTZ"].ToString().Length == 0)
            {
                if (Request.Params["userTZ"] != null && Request.Params["userTZ"].Length > 0)
                {
                    Session["UserTZ"] = Request.Params["userTZ"];
                    if (Session["SessionID"] != null && Session["SessionID"].ToString().Length > 0)
                    {
                        AdministrativeAPI.SetSessionTimeZone(Convert.ToInt64(Session["SessionID"]), Convert.ToInt32(Session["UserTZ"]));
                    }
                }
            }
            //Check if user specified and matches logged in user
            if (hdnUser.Value != null && hdnUser.Value.Length > 0)
            {
                // Check that the specified user & current user match
                if (Session["UserName"] != null && (Session["UserName"].ToString().Length > 0))
                {
                    if (hdnUser.Value.ToLower().CompareTo(Session["UserName"].ToString().ToLower()) != 0)
                    {
                        lblMessages.Visible = true;
                        lblMessages.Text = "You are currently logged in as a different user than the specified user. Please logout and login as " + hdnUser.Value;
                    }
                }
            }
           
            if (Session["UserName"] == null)
            {
                if (Request.IsAuthenticated)
                {
                    // Get Session info
                    // this needs work
                    SessionInfo sessionInfo = null;
                    divLogin.Visible = false;
                    HttpCookie cookie = Request.Cookies.Get(ConfigurationManager.AppSettings["isbAuthCookieName"]);
                    if (cookie != null)
                    {
                        long sessionid = Convert.ToInt64(cookie.Value);
                        sessionInfo = AdministrativeAPI.GetSessionInfo(sessionid);
                        if (sessionInfo != null && hdnUser.Value != null && hdnUser.Value.Length > 0)
                        {
                            // Check that the specified user & current user match
                            if (sessionInfo.userName != null && (sessionInfo.userName.Length > 0))
                            {
                                if (hdnUser.Value.ToLower().CompareTo(sessionInfo.userName.ToLower()) != 0)
                                {
                                    lblMessages.Visible = true;
                                    lblMessages.Text = "You are currently logged in as a different user than the specified user. Please logout and login as " + hdnUser.Value;
                                    return;
                                }
                            }
                        }
                        Session["UserID"] = sessionInfo.userID;
                        Session["UserName"] = sessionInfo.userName;
                        Session["GroupName"] = sessionInfo.groupName;
                        Session["GroupID"] = sessionInfo.groupID;
                        Session["UserTZ"] = sessionInfo.tzOffset;

                    }
                }
                // Find out who you are
                else if(hdnUser.Value != null && hdnUser.Value.Length > 0
                    && hdnKey.Value != null && hdnKey.Value.Length > 0)
                { 
                        divLogin.Visible = !authenticateUser(hdnUser.Value,hdnKey.Value, hdnAuthority.Value);
                }
                else
                {
                        // Use 3rd party Auth Service - not implmented
                        divLogin.Visible = true;
                }
            }
            else // We have a user Session
            {
                ResolveAction();
            }
               
        }
        
      

        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            //Page.ClientScript.RegisterStartupScript
        }
        #endregion

        private void logout()
        {
            AdministrativeAPI.SaveUserSessionEndTime(Convert.ToInt64(Session["SessionID"]));
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.Abandon();
        }

        /// <summary>
        /// This examines the specified parameters and current session state to resolve the next action.
        /// This may only be reached after a user is Authenticated.
        /// </summary>
        private void  ResolveAction(){
                int user_ID = 0;
                int client_ID = 0;
                int group_ID = 0;
                string client_Guid = null;
                string group_Name = null;
                string user_Name = null;
                StringBuilder buf = new StringBuilder();
                Session["IsAdmin"] = false;
                Session["IsServiceAdmin"] = false;
                lblMessages.Visible = false;
                lblMessages.Text = "";

                if (hdnUser.Value != null && hdnUser.Value.Length > 0)
                {
                    // Check that the specified user & current user match
                    if (hdnUser.Value.ToLower().CompareTo(Session["UserName"].ToString().ToLower()) == 0)
                    {
                        user_Name = hdnUser.Value;
                        Authority auth = brokerDB.AuthorityRetrieve(hdnAuthority.Value);
                        if(auth!= null)
                            user_ID = AdministrativeAPI.GetUserID(user_Name,auth.authorityID);
                        else 
                            user_ID = AdministrativeAPI.GetUserID(user_Name, 0);
                    }
                    else
                    {
                        //logout();
                        lblMessages.Visible = true;
                        lblMessages.Text = "You are not the user that was specified!";
                        return;
                    }
                }
                else // User is current user
                {
                    user_Name = Session["UserName"].ToString();
                    user_ID = Convert.ToInt32(Session["UserID"]);
                }

                //Get Client_ID
                if (hdnClient.Value != null && hdnClient.Value.Length > 0)
                {
                    client_ID = AdministrativeAPI.GetLabClientID(hdnClient.Value);
                    //Session["clientID"] = client_ID;
                }

                //{ // Note: The existing session client should not be concidered?
                
                if (hdnGroup.Value != null && hdnGroup.Value.Length > 0)
                {
                    group_Name = hdnGroup.Value;
                }
                
                // Check that the user & is a member of the group
                if (group_Name != null)
                {
                    int gid = AdministrativeAPI.GetGroupID(group_Name);
                    if (gid > 0)
                    {
                        if (AdministrativeAPI.IsUserMember(user_ID, gid))
                        {
                            group_ID = gid;
                            //Session["GroupID"] = group_ID;
                            //Session["GroupName"] = group_Name;
                        }
                        else
                        {
                            // user is not a member of the group
                            group_ID = -1;
                            group_Name = null;
                            
                        }
                    }
                }

                // Session and parameters are parsed, do we have enough info to launch
                int[] clientGroupIDs = null;
                int[] userGroupIDs = null;

                // Try and resolve any unspecified parameters
                if (client_ID <= 0 && group_ID <= 0)
                {
                    userGroupIDs = AdministrativeAPI.ListGroupIDsForUserRecursively(user_ID);
                    Group[] groups = AdministrativeAPI.GetGroups(userGroupIDs);
                    Dictionary<int, int[]> clientMap = new Dictionary<int, int[]>();
                    foreach (Group g in groups)
                    {
                        if ((g.groupType.CompareTo(GroupType.REGULAR) == 0))
                        {
                            int[] clientIDs = AdministrativeUtilities.GetGroupLabClients(g.groupID);
                            if (clientIDs != null & clientIDs.Length > 0)
                            {
                                clientMap.Add(g.groupID, clientIDs);
                            }
                        }
                    }
                    if (clientMap.Count > 1) //more than one group with clients
                    {
                        modifyUserSession(group_ID, client_ID);
                        Response.Redirect(Global.FormatRegularURL(Request, "myGroups.aspx"),true);
                    }
                    if (clientMap.Count == 1) // get the group with clients
                    {
                        Dictionary<int, int[]>.Enumerator en = clientMap.GetEnumerator();
                        int gid = -1;
                        int[] clients = null;
                        while (en.MoveNext())
                        {
                            gid = en.Current.Key;
                            clients = en.Current.Value;
                        }
                        if (AdministrativeAPI.IsUserMember(user_ID, gid))
                        {
                            group_ID = gid;
                            group_Name = AdministrativeAPI.GetGroupName(gid);
                            

                            if (clients == null || clients.Length > 1)
                            {
                               modifyUserSession(group_ID, client_ID);
                                Response.Redirect(Global.FormatRegularURL(Request, "myLabs.aspx"),true);
                            }
                            else
                            {
                                client_ID = clients[0];
                            }
                        }
                    }
                }
            
                else if (client_ID > 0 && group_ID <= 0)
                {
                    int gid = -1;
                    clientGroupIDs = AdministrativeUtilities.GetLabClientGroups(client_ID);
                    if (clientGroupIDs == null || clientGroupIDs.Length == 0)
                    {
                      modifyUserSession( group_ID, client_ID);
                        Response.Redirect(Global.FormatRegularURL(Request, "myGroups.aspx"),true);
                    }
                    else if (clientGroupIDs.Length == 1)
                    {
                        gid = clientGroupIDs[0];
                    }
                    else
                    {
                        userGroupIDs = AdministrativeAPI.ListGroupIDsForUserRecursively(user_ID);
                        int count = 0;
                        foreach (int ci in clientGroupIDs)
                        {
                            foreach (int ui in userGroupIDs)
                            {
                                if (ci == ui)
                                {
                                    count++;
                                    gid = ui;
                                }
                            }
                        }
                        if (count != 1)
                        {
                            gid = -1;
                        }
                    }
                    if (gid > 0 && AdministrativeAPI.IsUserMember(user_ID, gid))
                    {
                        group_ID = gid;
                       
                    }
                    else
                    {
                        modifyUserSession( group_ID, client_ID);
                    }
                }
                else if (client_ID <= 0 && group_ID > 0)
                {
                    int[] clients = AdministrativeUtilities.GetGroupLabClients(group_ID);
                    if (clients == null || clients.Length != 1)
                    {
                        modifyUserSession(group_ID, client_ID);
                        Response.Redirect(Global.FormatRegularURL(Request, "myLabs.aspx"),true);
                    }
                    else
                    {
                        client_ID = clients[0];
                    }
                }
                if (user_ID > 0 && group_ID > 0 && client_ID > 0)
                {
                    int gid = -1;
                    clientGroupIDs = AdministrativeUtilities.GetLabClientGroups(client_ID);
                    foreach (int g_id in clientGroupIDs)
                    {
                        if (g_id == group_ID)
                        {
                            gid = g_id;
                            break;
                        }
                    }
                    if (gid == -1)
                    {
                        buf.Append("The specified group does not have permission to to run the specified client!");
                        lblMessages.Visible = true;
                        lblMessages.Text = Utilities.FormatErrorMessage(buf.ToString());
                        return;
                    }
                    if (!AdministrativeAPI.IsUserMember(user_ID, group_ID))
                    {
                        buf.Append("You do not have permission to to run the specified client!");
                        lblMessages.Visible = true;
                        lblMessages.Text = Utilities.FormatErrorMessage(buf.ToString());
                        return;
                    }

                    // is authorized ?
                    
                    modifyUserSession(group_ID, client_ID);
                    launchLab(user_ID, group_ID, client_ID);
                   
                }
            }


        protected void launchLab(int userID, int groupID, int clientID)
        {
            // Currently there is not a good solution for checking for an AllowExperiment ticket, will check the USS for reservation
            StringBuilder buf = new StringBuilder();
            buf.Append(Global.FormatRegularURL(Request, "myClient.aspx"));
            buf.Append("?auto=t");
          
            string userName = null;
            Coupon opCoupon = null;
            Ticket allowTicket = null;
            int effectiveGroupID = 0;
            if (Session["UserName"] != null && Session["UserName"].ToString().Length > 0)
            {
                userName = Session["UserName"].ToString();
            }
            else
            {
                userName = AdministrativeAPI.GetUserName(userID);
            }

              LabClient client = AdministrativeAPI.GetLabClient(clientID);
              if (client != null && client.clientID > 0) // need to test for valid value
              {
                    DateTime start = DateTime.UtcNow;
                    long duration = 36000L; // default is ten hours
                    ProcessAgentInfo[] labServers = null;
                    labServers = AdministrativeAPI.GetLabServersForClient(clientID);
                    if (labServers.Length > 0)
                    {
                        //labServer = labServers[0];
                    }
                    else
                    {
                        throw new Exception("The lab server is not specified for lab client " + client.clientName + " version: " + client.version);
                    }
                    // Find efective group
                    string effectiveGroupName = null;
                    effectiveGroupID = AuthorizationAPI.GetEffectiveGroupID(groupID, clientID,
                        Qualifier.labClientQualifierTypeID, Function.useLabClientFunctionType);
                    if (effectiveGroupID == groupID)
                    {
                        if (Session["groupName"] != null)
                        {
                            effectiveGroupName = Session["groupName"].ToString();
                        }
                        else
                        {
                            effectiveGroupName = AdministrativeAPI.GetGroupName(groupID);
                            Session["groupName"] = effectiveGroupName;
                        }
                    }
                    else if (effectiveGroupID > 0)
                    {
                        effectiveGroupName = AdministrativeAPI.GetGroupName(effectiveGroupID);
                    }

                    //Check for Scheduling: Moved to myCLent
                    
                    Session["ClientID"] = client.clientID;
                  //Response.Redirect(Global.FormatRegularURL(Request, "myClient.aspx"), true);
                  Response.Redirect(buf.ToString(), true);
                  } // End if valid client
              else{
                throw new Exception("The specified lab client could not be found");
              }
        }

       

        protected void btnLogIn_Click(object sender, System.EventArgs e)
        {
            if(authenticateUser(txtUsername.Text, txtPassword.Text, null))
                ResolveAction();
        }

        protected bool authenticateUser(string userName, string passwd, string authority)
        {
            bool status = false;
            if (userName == null || userName.Length == 0 || passwd == null || passwd.Length == 0)
            {
                lblLoginErrorMessage.Text = Utilities.FormatErrorMessage("Missing user ID and/or password field.");
                lblLoginErrorMessage.Visible = true;
                return status;
            }
            if (hdnUser.Value != null && hdnUser.Value.Length > 0)
            {
                // Check that the specified user & current user match
                if (hdnUser.Value.ToLower().CompareTo(userName.ToLower()) != 0)
                {
                    lblMessages.Visible = true;
                    lblMessages.Text = Utilities.FormatWarningMessage("You are currently trying to login  in as a different user than the specified user. Please login as " + hdnUser.Value); ;
                    return status;
                }
            }
          

            int userID = -1;
            Authority auth = brokerDB.AuthorityRetrieve(hdnAuthority.Value);
            if (auth != null)
                userID = wrapper.GetUserIDWrapper(userName, auth.authorityID);
            else
                userID = wrapper.GetUserIDWrapper(userName, 0);

            if (userID > 0)
            {
                User user = wrapper.GetUsersWrapper(new int[] { userID })[0];

                if (userID != -1 && user.lockAccount == true)
                {
                    lblLoginErrorMessage.Text = Utilities.FormatErrorMessage("Account locked - Email " + supportMailAddress + ".");
                    lblLoginErrorMessage.Visible = true;
                    return status;
                }
                bool authOK = false;
                //Test for an authorized request for a third party agent
                if(authority != null && authority.Length >0 && authority.CompareTo(ProcessAgentDB.ServiceGuid) !=0){
                    Coupon[] authCoupon = brokerDB.GetIssuedCoupons(passwd);
                    if (authCoupon != null && authCoupon.Length >0)
                    {
                        Ticket authTicket = brokerDB.RetrieveTicket(authCoupon[0], TicketTypes.AUTHENTICATE_AGENT);
                        if (authTicket != null && authTicket.sponsorGuid.CompareTo(authority) == 0)
                        {
                            XmlQueryDoc authDoc =  new XmlQueryDoc(authTicket.payload);
                            string authAgent = authDoc.Query("AuthenticateAgentPayload/authGuid");
                            string authGuid = authDoc.Query("AuthenticateAgentPayload/clientGuid");
                            string authUser = authDoc.Query("AuthenticateAgentPayload/userName");
                            string authGroup = authDoc.Query("AuthenticateAgentPayload/groupName");



                        }
                    }
                    else
                    {
                        throw new AccessDeniedException("AccessDenied!");
                    }
                }
                else{
                    authOK = AuthenticationAPI.Authenticate(userID, passwd);
                }
                if (authOK)
                {
                    FormsAuthentication.SetAuthCookie(txtUsername.Text, false);
                    Session["UserID"] = userID;
                    Session["UserName"] = user.userName;
                    hdnUser.Value = user.userName;
                    if (Request.Params["userTZ"] != null && Request.Params["userTZ"].ToString().Length > 0)
                    {
                        Session["UserTZ"] = Request.Params["userTZ"];
                        uTZ = Convert.ToInt32(Request.Params["userTZ"]);
                    }
                
                    Session["SessionID"] = AdministrativeAPI.InsertUserSession(userID, 0, uTZ, Session.SessionID.ToString()).ToString();
                    Object tst = Session["SessionID"];
                    HttpCookie cookie = new HttpCookie(ConfigurationManager.AppSettings["isbAuthCookieName"], Session["SessionID"].ToString());
                    Response.AppendCookie(cookie);
                    divLogin.Visible = false;
                    status = true;
                    return status;
                }
                else
                {
                    lblLoginErrorMessage.Text = Utilities.FormatErrorMessage("Invalid user ID and/or password.");
                    lblLoginErrorMessage.Visible = true;
                    return status;
                }
            }
            else
            {
                lblLoginErrorMessage.Text = Utilities.FormatErrorMessage("Username does not exist.");
                lblLoginErrorMessage.Visible = true;
            }
            return status;
        }

        protected void modifyUserSession(int group_ID, int client_ID)
        {
            if (group_ID > 0)
            {
                string group_Name = AdministrativeAPI.GetGroupName(group_ID); ;
                Session["GroupID"] = group_ID;
                Session["GroupName"] = group_Name;
            }
            else
            {
                Session.Remove("GroupName");
                Session.Remove("GroupID");
            }
            if (client_ID > 0)
            {
                 Session["ClientID"] = client_ID;
                 Session["ClientCount"] = 1;
            }
            else
            {
                Session.Remove("ClientID");
                Session.Remove("ClientCount");
            }
            AdministrativeAPI.ModifyUserSession(Convert.ToInt64(Session["SessionID"]), group_ID, client_ID,
                Session.SessionID.ToString());
        }
    }
}
