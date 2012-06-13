using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Web.SessionState;
using System.Globalization;

using iLabs.Core;
using iLabs.DataTypes;
using iLabs.DataTypes.ProcessAgentTypes;
using iLabs.DataTypes.SoapHeaderTypes;
using iLabs.DataTypes.TicketingTypes;
using iLabs.DataTypes.StorageTypes;
using iLabs.Proxies.PAgent;
using iLabs.ServiceBroker;
using iLabs.Ticketing;
using iLabs.Proxies.ESS;

using iLabs.ServiceBroker.Internal;
using iLabs.ServiceBroker;
using iLabs.ServiceBroker.Administration;
using iLabs.ServiceBroker.Authentication;
using iLabs.ServiceBroker.Authorization;
using iLabs.ServiceBroker.DataStorage;
using iLabs.UtilLib;



namespace iLabs.ServiceBroker.admin
{
    /// <summary>
    /// Summary description for service Broker Stats.
    /// </summary>
    /// ***************************************************
    public partial class sbReport : System.Web.UI.Page
    {
        private Color disabled = Color.FromArgb(243, 239, 229);
        private bool secure = false;
        int userTZ;
        CultureInfo culture = null;
        AuthorizationWrapperClass wrapper = new AuthorizationWrapperClass();

        BrokerDB dbTicketing = new BrokerDB();

        protected void Page_Load(object sender, EventArgs e)
        {
            string couponId;
            string passkey;
            string issuerGuid;
            if (!wrapper.IsSuperuserGroup())
            {
                string msg = "You must be in the Superuser Group to run these reports.";
                lblResponse.Text = Utilities.FormatWarningMessage(msg);
                lblResponse.Visible = true;
                btnSubmit.Enabled = false;
                btn_ExportCVS.Enabled = false;
                return;
            }
            culture = DateUtil.ParseCulture(Request.Headers["Accept-Language"]);
            if (Session["UserTZ"] != null)
                userTZ = Convert.ToInt32(Session["UserTZ"]);


            string returnUrl = Request.QueryString["returnURL"];
            if (returnUrl != null && returnUrl.Length > 0)
                Session["returnURL"] = returnUrl;



            if (!IsPostBack)
            {

                // try & test for local access or not configured
                if (secure)
                {
                    // retrieve parameters from URL
                    couponId = Request.QueryString["coupon_id"];
                    passkey = Request.QueryString["passkey"];
                    issuerGuid = Request.QueryString["issuer_guid"];
                    Ticket allowAdminTicket = null;
                    if (couponId != null && passkey != null && issuerGuid != null)
                    {
                        allowAdminTicket = dbTicketing.RetrieveAndVerify(
                            new Coupon(issuerGuid, Int64.Parse(couponId), passkey),
                            TicketTypes.ADMINISTER_LS);
                    }
                    else
                    {
                        Response.Redirect("AccessDenied.aspx", true);
                    }
                }
                DisplayForm();

            }
        }

        /// ***************************************************

        protected void DisplayForm()
        {
            try
			{
				ddlGroupTarget.Items .Clear ();
				//ddlGroupTarget.Items .Add (new ListItem("--Select one--","0"));
				int[] groupIDs = wrapper.ListGroupIDsWrapper();
				Group[] groups=wrapper.GetGroupsWrapper(groupIDs);
                foreach (Group g in groups)
                {
                    if (g.groupID > 0)
                    {
                        if (
                            (!g.groupName.Equals(Group.ROOT))
                          && (!g.groupName.Equals(Group.ORPHANEDGROUP)) && (!g.groupName.Equals(Group.NEWUSERGROUP) && (!g.groupName.Equals(Group.SUPERUSER)))
                          && (g.groupType.Equals(GroupType.REGULAR)))
                        {
                            //Response.Write(g.groupID);
                            //Response.Write(g.groupType);
                            ddlGroupTarget.Items.Add(new ListItem(g.groupName, g.groupID.ToString()));
                        }
                    }
                }
                //ddlGroupTarget.Items.Add (new ListItem("All Groups","0"));

				} // end try
				catch(Exception ex)
				{
				    string msg = "Exception: Cannot list groups. "+ex.Message+". "+ex.GetBaseException()+".";
                    lblResponse.Text = Utilities.FormatErrorMessage(msg);
					lblResponse.Visible = true; 
				}

                ddlReportTarget.Items.Add(new ListItem("Group Roster", "1"));
                ddlReportTarget.Items.Add(new ListItem("Group Overview Report", "2"));
                ddlReportTarget.Items.Add(new ListItem("Group Client Report", "3"));
                ddlReportTarget.Items.Add(new ListItem("Group Client Column Report", "5"));
                ddlReportTarget.Items.Add(new ListItem("Group Experiment List", "4"));
                ddlReportTarget.Items.Add(new ListItem("All Group Summary Report", "6"));
                //ddlReportTarget.Items.Add(new ListItem("Group Stats Report", "7"));

			} //end displayform

        /// ***************************************************
        
        protected void btnSubmit_Click(object sender, System.EventArgs e)
        {
            // check to make sure that both a group and report type are selected
            if (ddlGroupTarget.Text == "" && ddlReportTarget.Text == "")
            {
                lblResponse.Text = "<div class=errormessage><p>Please select both a group and a report </p></div>";
                lblResponse.Visible = true;
                return;
            }
            else
            {
                switch (ddlReportTarget.Text )
                {
                    case "1":
                        ReportDisplayArea.Text = GroupRoster(Convert.ToInt32(ddlGroupTarget.Text));
                        ReportDisplayArea.Visible = true;
                        btn_ExportCVS.Visible = true;
                        break;
                    case "2":
                        ReportDisplayArea.Text = GroupSummaryReport(Convert.ToInt32(ddlGroupTarget.Text));
                        ReportDisplayArea.Visible = true;
                        btn_ExportCVS.Visible = true;
                        break;
                    case "3":
                        ReportDisplayArea.Text = GroupDetailReport(Convert.ToInt32(ddlGroupTarget.Text));
                        ReportDisplayArea.Visible = true;
                        btn_ExportCVS.Visible = true;
                        break;
                    case "4":
                        ReportDisplayArea.Text = GroupExperimentReport(Convert.ToInt32(ddlGroupTarget.Text));
                        ReportDisplayArea.Visible = true;
                        btn_ExportCVS.Visible = true;
                        break;
                    case "5":
                        ReportDisplayArea.Text = GroupDetailReport2(Convert.ToInt32(ddlGroupTarget.Text));
                        ReportDisplayArea.Visible = true;
                        btn_ExportCVS.Visible = true;
                        break;
                    case "6":
                        ReportDisplayArea.Text = GroupAllReport(Convert.ToInt32(ddlGroupTarget.Text));
                        ReportDisplayArea.Visible = true;
                        btn_ExportCVS.Visible = true;
                        break;

                    case "7":
                        ReportDisplayArea.Text = GroupStatReport(Convert.ToInt32(ddlGroupTarget.Text));
                        ReportDisplayArea.Visible = true;
                        btn_ExportCVS.Visible = true;
                        break;

                } //end switch statement
            } //end else

        } // end btnSubmit_Click

        /// ***************************************************

        protected string GroupRoster(int groupID)
        {
            string reportDate = System.DateTime.Now.ToString();
            string groupReportTXT = "";
            string lSession = "";
            int[] gIDs;
            gIDs = new int[1] { groupID };
            Group[] groups = wrapper.GetGroupsWrapper(gIDs);
            foreach (Group g in groups)
            {
                // Get GroupName from GroupID
                groupReportTXT = "<br><br><hr><br><center><h1><b>" + g.groupName + "</b></h1></center><br><br>Report Date: " + reportDate + " <br>Description: " + g.description + "<br><br>\n";
            }
            int[] userIDs = wrapper.ListUserIDsInGroupWrapper(groupID);
            if (userIDs != null && userIDs.Length > 0)
            {
                groupReportTXT = groupReportTXT + "<p><table border=1 width=650 ><tr><th>Last Name</th><th>First Name</th><th>Username</th><th>Email</th><th>Last Login</th></tr> \n";

                // Get Group user information (name, username, last_login)

                User[] users = wrapper.GetUsersWrapper(userIDs);
                foreach (User u in users)
                {
                    lSession = GetLastLogin(u.userID, groupID);
                    groupReportTXT = groupReportTXT + "<tr><td>" + u.lastName + "</td><td>" + u.firstName + "</td><td>" + u.userName + "</td><td>" + u.email + "</td><td>" + lSession + "</td></tr>\n";
                }

                groupReportTXT = groupReportTXT + "</table></p>";
            }
            else
            {
                groupReportTXT = groupReportTXT + "<p><strong>No users are members of this group!</strong></p>";
            }
            return groupReportTXT;

        } // end GroupRoster

        /// ***************************************************

        protected string GroupSummaryReport(int groupID)
        {
            string reportDate = System.DateTime.Now.ToString();
            string groupExpTXT = "";
            string groupExpTXT2 = "";
            int numSession = 0;
            int totalSessions = 0;
            int numExp = 0;
            int totalExp = 0;
            int[] gIDs;
            gIDs = new int[1] { groupID };
            Group[] groups = wrapper.GetGroupsWrapper(gIDs);
            foreach (Group g in groups)
            {
                // Get GroupName from GroupID
                groupExpTXT = "<br><br><hr><br><center><h1><b>" + g.groupName + "</b></h1></center><br><br>Report Date: " + reportDate + " <br>Description: " + g.description +"<br><br>\n";

                // get the associated lab clients
                int[] lcIDsList = AdministrativeUtilities.GetGroupLabClients(g.groupID);
                LabClient[] lcList = wrapper.GetLabClientsWrapper(lcIDsList);
                groupExpTXT += "Associated Clients: <ul>";
                if (lcList != null && lcList.Length > 0)
                {
                    for (int i = 0; i < lcList.Length; i++)
                    {
                        groupExpTXT += "<li><strong class=lab>" + lcList[i].clientName + "</strong> - " + lcList[i].clientShortDescription + "</li>";
                    }
                }
                else
                {
                    groupExpTXT += "<li><strong class=lab>No Clients are assigned to this group!</strong></li>";
                }
                groupExpTXT += "</ul><br>";
            }

            // Get the number of user logins
            // Get the number of experiments submitted
            groupExpTXT2 = "<p><table border=1 width=650 ><tr><th>Last Name</th><th>First Name</th><th>Username</th><th>Logins</th><th>Experiment Runs</th></tr> \n";
            // Get Group user information (name, username, last_login, number of experiments submitted)
            int[] userIDs = wrapper.ListUserIDsInGroupWrapper(groupID);
            User[] users = wrapper.GetUsersWrapper(userIDs);
            foreach (User u in users)
            {
                numSession = GetNumLogin(u.userID, groupID);
                totalSessions = totalSessions + numSession;
                numExp = GetNumExp(u.userID, groupID);
                totalExp = totalExp + numExp;
                groupExpTXT2 = groupExpTXT2 + "<tr><td>" + u.lastName + "</td><td>" + u.firstName + "</td><td>" + u.userName + "</td><td>" + numSession + "</td><td>" + numExp + "</td></tr>\n";
            }
            groupExpTXT2 = groupExpTXT2 + "</table></p>";
            groupExpTXT = groupExpTXT + "Number of users: " + users.Length + "\n<br>Number of user logins: " + totalSessions + "\n<br>Number of experiment runs: " + totalExp + "\n<br><br>";
            groupExpTXT = groupExpTXT + groupExpTXT2;
            return groupExpTXT;

        } // end GroupSummaryReport

        /// ***************************************************

        protected string GroupDetailReport(int groupID)
        {
            string reportDate = System.DateTime.Now.ToString();
            string groupExpTXT = "";
            string groupExpTXT2 = "";
            string groupClientTXT = "";
            string userExpTXT = "";
            string totExpTXT = "";
            int numSession = 0;
            int totalSessions = 0;
            int numExp = 0;
            int totalExp = 0;
            int numClients = 0;
            int numClientExp = 0;
            int[] gIDs;
            int[] totalExpByClient;
            string[] totClientName;
            gIDs = new int[1] { groupID };
            Group[] groups = wrapper.GetGroupsWrapper(gIDs);
            foreach (Group g in groups)
            {
                // Get GroupName from GroupID
                groupExpTXT = "<br><br><hr><br><center><h1><b>" + g.groupName + "</b></h1></center><br><br>Report Date: " + reportDate + " <br>Description: " + g.description + "<br><br>\n";

                // get the associated lab clients
                int[] lcIDsList = AdministrativeUtilities.GetGroupLabClients(g.groupID);
                LabClient[] lcList = wrapper.GetLabClientsWrapper(lcIDsList);
                numClients = lcList.Length;
            }
            totalExpByClient = new int[numClients];
            totClientName = new string[numClients];

            // Get the number of user logins
            // Get the number of experiment Launches
            groupExpTXT2 = "<p><table border=1 width=650 ><tr><th>Last Name</th><th>First Name</th><th>Username</th><th>Logins</th><th>Experiment Runs</th></tr> \n";
            // Get Group user information (name, username, last_login, number of experiments submitted)
            int[] userIDs = wrapper.ListUserIDsInGroupWrapper(groupID);
            User[] users = wrapper.GetUsersWrapper(userIDs);
            foreach (User u in users)
            {
                // get the number of logins for this user
                numSession = GetNumLogin(u.userID, groupID);
                totalSessions = totalSessions + numSession;
                //get the number of experiments launches for this user
                numExp = GetNumExp(u.userID, groupID);
                totalExp = totalExp + numExp;

                // for each client get the number of experiment for that client
                foreach (Group gr in groups)
                {
                    userExpTXT = "";  //clear client list
                    // get the associated lab clients
                    int[] lcIDsList = AdministrativeUtilities.GetGroupLabClients(gr.groupID);
                    LabClient[] lcList = wrapper.GetLabClientsWrapper(lcIDsList);
                    for (int j = 0; j < lcList.Length; j++)
                    {
                        numClientExp = GetNumExpByClient(u.userID, groupID, lcList[j].clientID);
                        userExpTXT += "<br>" + lcList[j].clientName + " - " + numClientExp;
                        totalExpByClient[j] += numClientExp;
                        totClientName[j] = lcList[j].clientName;
                    }
                }

                // Create user table row
                groupExpTXT2 = groupExpTXT2 + "<tr><td>" + u.lastName + "</td><td>" + u.firstName + "</td><td>" + u.userName + "</td><td>" + numSession + "</td><td>Total: " + numExp + userExpTXT + "</td></tr>\n";
            }
            for (int k = 0; k < numClients; k++)
            {
                totExpTXT += "<li>" + totClientName[k] + ": " + totalExpByClient[k];
            }

            groupExpTXT2 = groupExpTXT2 + "</table></p>";
            groupExpTXT = groupExpTXT + "Number of users: " + users.Length + "\n<br>Number of user logins: " + totalSessions + "\n<br>Number of experiment runs: " + totalExp + "\n<br><ul>" + totExpTXT + "</ul><br>";
            groupExpTXT = groupExpTXT + groupExpTXT2;

            return groupExpTXT;

        } // end GroupDetailReport

        /// ***************************************************

        protected string GroupExperimentReport(int groupID)
        {
            string reportDate = System.DateTime.Now.ToString();
            StringBuilder grpBuf = new StringBuilder();
            StringBuilder grpBuf2 = new StringBuilder();
            string groupExpTXT = "";
            string groupExpTXT2 = "";
            string groupExpRows = "";
            int numSession = 0;
            int totalSessions = 0;
            int numExp = 0;
            int totalExp = 0;
            int[] gIDs;
            gIDs = new int[1] { groupID };

            Group[] groups = wrapper.GetGroupsWrapper(gIDs);
            foreach (Group g in groups)
            {

                // Get GroupName from GroupID
                grpBuf.AppendLine("<br><br><hr><br><center><h1><b>" + g.groupName + "</b></h1></center><br><br>Report Date: " + reportDate + " <br>Description: " + g.description + "<br><br>");

                // get the associated lab clients
                int[] lcIDsList = AdministrativeUtilities.GetGroupLabClients(g.groupID);
                LabClient[] lcList = AdministrativeAPI.GetLabClients(lcIDsList);
                //LabClient[] lcList = wrapper.GetLabClientsWrapper(lcIDsList);
                grpBuf.AppendLine("Associated Clients: <ul>");
                for (int i = 0; i < lcList.Length; i++)
                {
                    grpBuf.AppendLine("<li><strong class=lab>" + lcList[i].clientName + "</strong> - " + lcList[i].clientShortDescription + "</li>");
                }
                grpBuf.AppendLine("</ul><br>");
            }

            // Get the number of experiments submitted

            long[] eIDs = DataStorageAPI.RetrieveExperimentIDs(-1, groupID, -1);
            //LongTag[] expTags = DataStorageAPI.RetrieveExperimentTags(eIDs, userTZ, culture, true, true, true, true, true, true, true, true);
            if (eIDs.Length == 0)
            {
                string msg = "No experiments were found for the selection criteria!";
                lblResponse.Text = Utilities.FormatWarningMessage(msg);
                lblResponse.Visible = true;
            }
            else
            {
                grpBuf.AppendLine("<p><table border=1 width=650 ><tr><th>Experiment ID</th><th>Client Name</th><th>Username</th><th>Submission Time</th><th>Completion Time</th></tr>");

                ExperimentSummary[] expInfo = wrapper.GetExperimentSummaryWrapper(eIDs);
                int l = expInfo.Length;
                int j;
                for (j = 0; j < l; j++)
                {
                    if (expInfo[j] != null)
                    {

                        string a = expInfo[j].experimentId.ToString();
                        string b = expInfo[j].userName;
                        string c = "";
                        string d = DateUtil.ToUserTime(expInfo[j].creationTime, culture, userTZ);
                        string e = expInfo[j].clientName;
                        if ((expInfo[j].closeTime != null) && (expInfo[j].closeTime != DateTime.MinValue))
                            c = DateUtil.ToUserTime(expInfo[j].closeTime, culture, userTZ);
                        else
                            c = "Not Closed!";

                        grpBuf.AppendLine("<tr><td>" + a + "</td><td>" + e + "</td><td>" + b + "</td><td>" + d + "</td><td>" + c + "</td></tr>");
                    }
                }
                grpBuf.AppendLine("</table></p>");
            }

            return grpBuf.ToString();

        }// end GroupExperimentReport



        /// ***************************************************

        protected string GroupDetailReport2(int groupID)
        {
            string reportDate = System.DateTime.Now.ToString();
            string groupExpTXT = "";
            string groupExpTXT2 = "";
            string groupClientTXT = "";
            string groupExpTXTheader = "";
            string userExpTXT = "";
            string totExpTXT = "";
            string expListTable = "";
            string expListName = "";
            int numSession = 0;
            int totalSessions = 0;
            int numExp = 0;
            int totalExp = 0;
            int numClients = 0;
            int numClientExp = 0;
            int[] gIDs;
            int[] totalExpByClient;
            string[] totClientName;
            gIDs = new int[1] { groupID };
            Group[] groups = wrapper.GetGroupsWrapper(gIDs);
            foreach (Group g in groups)
            {
                // Get GroupName from GroupID
                groupExpTXT = "<br><br><hr><br><center><h1><b>" + g.groupName + "</b></h1></center><br><br>Report Date: " + reportDate + " <br>Description: " + g.description + "<br><br>\n";

                // get the associated lab clients
                int[] lcIDsList = AdministrativeUtilities.GetGroupLabClients(g.groupID);
                LabClient[] lcList = wrapper.GetLabClientsWrapper(lcIDsList);
                numClients = lcList.Length;
            }
            totalExpByClient = new int[numClients];
            totClientName = new string[numClients];

            // Get the number of user logins
            // Get the number of experiment Launches
            // Get Group user information (name, username, last_login, number of experiments submitted)
            int[] userIDs = wrapper.ListUserIDsInGroupWrapper(groupID);
            User[] users = wrapper.GetUsersWrapper(userIDs);
            foreach (User u in users)
            {
                // get the number of logins for this user
                numSession = GetNumLogin(u.userID, groupID);
                totalSessions = totalSessions + numSession;
                //get the number of experiments launches for this user
                numExp = GetNumExp(u.userID, groupID);
                totalExp = totalExp + numExp;

                // for each client get the number of experiment for that client
                foreach (Group gr in groups)
                {
                    userExpTXT = "";  //clear client list
                    expListTable = "";
                    // get the associated lab clients
                    int[] lcIDsList = AdministrativeUtilities.GetGroupLabClients(gr.groupID);
                    LabClient[] lcList = wrapper.GetLabClientsWrapper(lcIDsList);
                    for (int j = 0; j < lcList.Length; j++)
                    {
                        numClientExp = GetNumExpByClient(u.userID, groupID, lcList[j].clientID);
                        userExpTXT += "<br>" + lcList[j].clientName + " - " + numClientExp;
                        expListTable += "<td>" + numClientExp + "</td>";
                        totalExpByClient[j] += numClientExp;
                        totClientName[j] = lcList[j].clientName;
                    }
                }

                // Create user table row
                groupExpTXT2 = groupExpTXT2 + "<tr><td>" + u.lastName + "</td><td>" + u.firstName + "</td><td>" + u.userName + "</td><td>" + numSession + "</td><td>" + numExp + "</td>" + expListTable + "</tr>\n";
            }
            for (int k = 0; k < numClients; k++)
            {
                totExpTXT += "<li>" + totClientName[k] + ": " + totalExpByClient[k];
                expListName += "<th>" + totClientName[k] + "</th>";

            }
            groupExpTXTheader = "<p><table border=1><tr><th>Last Name</th><th>First Name</th><th>Username</th><th>Logins</th><th>Total Experiments</th>" + expListName + "</tr> \n";

            groupExpTXT2 = groupExpTXT2 + "</table></p>";
            groupExpTXT = groupExpTXT + "Number of users: " + users.Length + "\n<br>Number of user logins: " + totalSessions + "\n<br>Number of experiment runs: " + totalExp + "\n<br><ul>" + totExpTXT + "</ul><br>";
            groupExpTXT = groupExpTXT + groupExpTXTheader;
            groupExpTXT = groupExpTXT + groupExpTXT2;

            return groupExpTXT;

        } // end GroupDetailReport

        /// ***************************************************

        /// ***************************************************

        protected string GroupStatReport(int groupID)
        {
            string var1 = "Group Statistic Report " + groupID;
            return var1;

        } // end GroupStatReport

        /// ***************************************************

        //last log-in sessions according to the selected criterion
        private string GetLastLogin(int userID, int groupID)
        {
            string lastSession = "";
            try
            {
                UserSession[] sessions = wrapper.GetUserSessionsWrapper(userID, groupID, DateTime.MinValue, DateTime.MaxValue);
                if (sessions.Length == 0)
                {
                    lastSession = "No logins.";
                }
                else
                {
                    int pos = 0;
                    DateTime lastLogin = sessions[0].sessionStartTime;
                    for (int j = sessions.Length - 1; j > -1; j--)
                    {
                        if (lastLogin < sessions[j].sessionStartTime)
                        {
                            lastLogin = sessions[j].sessionStartTime;
                            pos = j;
                        }
                    }
                    //string gName = wrapper.GetGroupsWrapper(new int[] { sessions[pos].groupID })[0].groupName;
                    lastSession += sessions[pos].sessionStartTime.ToString() + " \n\n" ;
                }
            }
            catch (Exception ex)
            {
                lblResponse.Text = "<div class=errormessage><p>Cannot retrieve UserSessions. " + ex.GetBaseException() + "</p></div>";
                lblResponse.Visible = true;
            }
            return lastSession;
        }

        /// ***************************************************

        //return the number log-in sessions based on the selected criterion
        private int GetNumLogin(int userID, int groupID)
        {
            int num = 0;
            try
            {
                UserSession[] sessions = wrapper.GetUserSessionsWrapper(userID, groupID, DateTime.MinValue, DateTime.MaxValue);
                num = sessions.Length;
            }
            catch (Exception ex)
            {
                lblResponse.Text = "<div class=errormessage><p>Cannot retrieve UserSessions. " + ex.GetBaseException() + "</p></div>";
                lblResponse.Visible = true;
            }
            return num;
        }

        /// ***************************************************

        private int GetNumExp(int userID, int groupID)
        {
            int num = 0;
            List<Criterion> cList = new List<Criterion>();
            cList.Add(new Criterion("Group_ID", "=", groupID.ToString()));
            cList.Add(new Criterion("User_ID", "=", userID.ToString()));

            try
            {
                long[] eIDs = DataStorageAPI.RetrieveAuthorizedExpIDs(userID, groupID, cList.ToArray());
                num = eIDs.Length;
            }
            catch (Exception ex)
            {
                lblResponse.Text = "<div class=errormessage><p>Cannot retrieve UserSessions. " + ex.GetBaseException() + "</p></div>";
                lblResponse.Visible = true;
            }
            return num;
        }

        /// ***************************************************

        private int GetNumExpByClient(int userID, int groupID, int clientID)
        {
            int num = 0;
            List<Criterion> cList = new List<Criterion>();
            cList.Add(new Criterion("Group_ID", "=", groupID.ToString()));
            cList.Add(new Criterion("User_ID", "=", userID.ToString()));
            cList.Add(new Criterion("client_id", "=", clientID.ToString()));

            try
            {
                long[] eIDs = DataStorageAPI.RetrieveAuthorizedExpIDs(userID, groupID, cList.ToArray());
                num = eIDs.Length;
            }
            catch (Exception ex)
            {
                lblResponse.Text = "<div class=errormessage><p>Cannot retrieve UserSessions for this client. " + ex.GetBaseException() + "</p></div>";
                lblResponse.Visible = true;
            }
            return num;
        }

        /// ***************************************************

        //private long[] GetGroupExpIDs(int userID, int groupID)
        //{
        //    long[] eIDs = null;
        //    List<Criterion> cList = new List<Criterion>();
        //    cList.Add(new Criterion("Group_ID", "=", groupID.ToString()));
        //    //cList.Add(new Criterion("User_ID", "=", userID.ToString()));

        //    try
        //    {
        //        eIDs = DataStorageAPI.RetrieveAuthorizedExpIDs(userID, groupID, cList.ToArray());
        //    }
        //    catch (Exception ex)
        //    {
        //        lblResponse.Text = "<div class=errormessage><p>Cannot retrieve UserSessions for this group. " + ex.GetBaseException() + "</p></div>";
        //        lblResponse.Visible = true;
        //    } 
        //    return eIDs;
        //}

        protected void btnExportCVS_Click(object sender, System.EventArgs e)
        {
            //
            // Export html table data to CVS file for user to save locally.
            //
            // generate report
            string reportString = "";

            switch (ddlReportTarget.Text)
            {
                case "1":
                    reportString = GroupRoster(Convert.ToInt32(ddlGroupTarget.Text));
                    break;
                case "2":
                    reportString = GroupSummaryReport(Convert.ToInt32(ddlGroupTarget.Text));
                    break;
                case "3":
                    reportString = GroupDetailReport(Convert.ToInt32(ddlGroupTarget.Text));
                    break;
                case "4":
                    reportString = GroupExperimentReport(Convert.ToInt32(ddlGroupTarget.Text));
                    break;
                case "5":
                    reportString = GroupDetailReport2(Convert.ToInt32(ddlGroupTarget.Text));
                    break;
                case "6":
                    reportString = GroupAllReport(Convert.ToInt32(ddlGroupTarget.Text));
                    break;

                case "7":
                    reportString = GroupStatReport(Convert.ToInt32(ddlGroupTarget.Text));
                    break;

            } //end switch statement  
          
            // convert to CVS
            String CVSstring = convertCVS(reportString);

            // Export to downloadble file
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment; filename=export"+ DateTime.Now.ToString("MMddyyyyHHmmss")+".txt");
            Response.AddHeader("content-type", "text/csv");
            Response.Write(CVSstring);
            Response.End();
        }

        protected string convertCVS(string inputS)
        {
            // take an input HTML string with a table and convert to CVS format
            //

            StringBuilder outputS = new StringBuilder(inputS);
            outputS.Replace("<table>",string.Empty);
            outputS.Replace("<table border=1>", string.Empty);
            outputS.Replace("</table>", string.Empty);
            outputS.Replace("</tr>",string.Empty);
            outputS.Replace("</td>",string.Empty);
            outputS.Replace("</th>", string.Empty);
            outputS.Replace("</p>", string.Empty);
            outputS.Replace("<h1>", string.Empty);
            outputS.Replace("</h1>", string.Empty);
            outputS.Replace("<strong class=lab>", string.Empty);
            outputS.Replace("</strong>", string.Empty);
            outputS.Replace("</center>", string.Empty);
            outputS.Replace("<center>", string.Empty);
            outputS.Replace("<b>", string.Empty);
            outputS.Replace("</b>", string.Empty);
            outputS.Replace("<hr>", string.Empty);
            outputS.Replace("<ul>", string.Empty);
            outputS.Replace("</ul>", string.Empty);
            outputS.Replace("\n", string.Empty);
            outputS.Replace("<table border=1 width=650 >", string.Empty);
            outputS.Replace("<li>", Environment.NewLine);
            outputS.Replace("</li>",string.Empty);
            outputS.Replace("<p>", Environment.NewLine);
            outputS.Replace("<tr>", Environment.NewLine);
            outputS.Replace("<br>", Environment.NewLine);
            outputS.Replace("<br/>", Environment.NewLine);
            outputS.Replace("<th>", "\t");
            outputS.Replace("<td>", "\t");
            return outputS.ToString();
        }


        /// ***************************************************

        protected string GroupAllReport(int groupID)
        {
            string groupExpTXT;
            string reportDate = System.DateTime.Now.ToString();
            string groupRows = "";
            string groupExpTXTheader;

            groupExpTXT = "<br><br><hr><br><center><h1><b>Groups Summary Report</b></h1></center><br><br>Report Date: " + reportDate + "<br><br>\n";


            try
			{
				int[] groupIDs = wrapper.ListGroupIDsWrapper();
				Group[] groups=wrapper.GetGroupsWrapper(groupIDs);
                foreach (Group gp in groups)
                {
                    if (gp.groupID > 0)
                    {
                        if (
                            (!gp.groupName.Equals(Group.ROOT))
                          && (!gp.groupName.Equals(Group.ORPHANEDGROUP)) && (!gp.groupName.Equals(Group.NEWUSERGROUP) && (!gp.groupName.Equals(Group.SUPERUSER)))
                          && (gp.groupType.Equals(GroupType.REGULAR)))
                        {
                            groupRows += GroupRow(gp.groupID);
                        }
                    }
                }
 
				} // end try
				catch(Exception ex)
				{
				    string msg = "Exception: Cannot list groups. "+ex.Message+". "+ex.GetBaseException()+".";
                    lblResponse.Text = Utilities.FormatErrorMessage(msg);
					lblResponse.Visible = true; 
				}

            groupExpTXTheader = "<p><table border=1><tr><th>Group Name</th><th>Num Users</th><th>Num User Logins</th><th>Total Experiments</th><th>Breakdown by Experiment</th></tr> \n";
            groupExpTXT += groupExpTXTheader + groupRows + "</table></p>";

            return groupExpTXT;

        } // end GroupAllReport
        

        /// ***************************************************

        protected string GroupRow(int groupID)
        {
            string groupClientTXT = "";
            string userExpTXT = "";
            string totExpTXT = "";
            string expListTable = "";
            string expListName = "";
            int numSession = 0;
            int totalSessions = 0;
            int numExp = 0;
            int totalExp = 0;
            int numClients = 0;
            int numClientExp = 0;
            int[] gIDs;
            int[] totalExpByClient;
            string[] totClientName;
            string grName = "";
            string gRowString;
            

            gIDs = new int[1] { groupID };
            Group[] groups = wrapper.GetGroupsWrapper(gIDs);
            foreach (Group g in groups)
            {
                // Get GroupName from GroupID
                grName +=  g.groupName;
                // get the associated lab clients
                int[] lcIDsList = AdministrativeUtilities.GetGroupLabClients(g.groupID);
                LabClient[] lcList = wrapper.GetLabClientsWrapper(lcIDsList);
                numClients = lcList.Length;
            }
            totalExpByClient = new int[numClients];
            totClientName = new string[numClients];

            // Get the number of user logins
            // Get the number of experiment Launches
            // Get the number of experiments submitted 
            int[] userIDs = wrapper.ListUserIDsInGroupWrapper(groupID);
            User[] users = wrapper.GetUsersWrapper(userIDs);
            foreach (User u in users)
            {
                // get the number of logins for this user
                numSession = GetNumLogin(u.userID, groupID);
                totalSessions = totalSessions + numSession;
                //get the number of experiments launches for this user
                numExp = GetNumExp(u.userID, groupID);
                totalExp = totalExp + numExp;

                // for each client get the number of experiment for that client
                foreach (Group gr in groups)
                {
                    userExpTXT = "";  //clear client list
                    expListTable = "";
                    // get the associated lab clients
                    int[] lcIDsList = AdministrativeUtilities.GetGroupLabClients(gr.groupID);
                    LabClient[] lcList = wrapper.GetLabClientsWrapper(lcIDsList);
                    for (int j = 0; j < lcList.Length; j++)
                    {
                        numClientExp = GetNumExpByClient(u.userID, groupID, lcList[j].clientID);
                        userExpTXT += "<br>" + lcList[j].clientName + " - " + numClientExp;
                        expListTable += "<td>" + numClientExp + "</td>";
                        totalExpByClient[j] += numClientExp;
                        totClientName[j] = lcList[j].clientName;
                    }
                }

            }
            for (int k = 0; k < numClients; k++)
            {
                totExpTXT += "<br>" + totClientName[k] + ": " + totalExpByClient[k];
                expListName += "<th>" + totClientName[k] + "</th>";

            }

            gRowString = "<tr><td>"+ grName +"</td><td>"+ users.Length +"</td><td>" + totalSessions + "</td><td>" + totalExp + "</td><td>" + totExpTXT + "</td></tr>";

            return gRowString;

        } // end GroupRow

        /// ***************************************************


        } //end public class sbReport
    } // end Namespace



