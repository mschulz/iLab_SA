using System;
using System.Data;
using System.Configuration;
using System.Collections;
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

using iLabs.Core;
using iLabs.DataTypes;
using iLabs.DataTypes.ProcessAgentTypes;
using iLabs.DataTypes.SoapHeaderTypes;
using iLabs.DataTypes.TicketingTypes;
using iLabs.Proxies.PAgent;
using iLabs.ServiceBroker;
using iLabs.Ticketing;

using iLabs.ServiceBroker.Internal;
using iLabs.ServiceBroker;
using iLabs.ServiceBroker.Administration;
using iLabs.ServiceBroker.Authentication;
using iLabs.ServiceBroker.Authorization;
using iLabs.UtilLib;
namespace iLabs.ServiceBroker.admin
{
    /// <summary>
    /// Summary description for service Broker Stats.
    /// </summary>
    /// 

    public class GroupReportList
    {
        private string g_name;
        private string g_desc;
        private string g_type;
        private string g_clients;
        private int users_ingroup;

        public string gname
        {
            get
            {
                return g_name;
            }
            set
            {
                g_name = value;
            }
        }
        public int usersingroup
        {
            get
            {
                return users_ingroup;
            }
            set
            {
                users_ingroup = value;
            }
        }
        public string gdesc
        {
            get
            {
                return g_desc;
            }
            set
            {
                g_desc = value;
            }
        }
        public string gtype
        {
            get
            {
                return g_type;
            }
            set
            {
                g_type = value;
            }
        }
        public string gclients
        {
            get
            {
                return g_clients;
            }
            set
            {
                g_clients = value;
            }
        }
    }


    public partial class sbStats : System.Web.UI.Page
    {
        private Color disabled = Color.FromArgb(243, 239, 229);
        private bool secure = false;
        protected ArrayList groupList;
        protected ArrayList groupUList;
        protected int associatedGroup = 0;

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
                return;
            }
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

                string returnUrl = Request.QueryString["returnURL"];
                if (returnUrl != null && returnUrl.Length > 0)
                    Session["returnURL"] = returnUrl;
            }
            DisplayForm();
            //DataBind();
        }
        protected void DisplayForm()
        {
            lblResponse.Visible = false;
            ProcessAgent pai = ProcessAgentDB.ServiceAgent;
            if (pai != null)
            {
                txtServiceName.Text = pai.agentName;
                txtServiceGUID.Text = pai.agentGuid;
                txtWebServiceUrl.Text = pai.webServiceUrl;

                //Put in availabe USS
                IntTag[] usses = dbTicketing.GetProcessAgentTagsByType(ProcessAgentType.SCHEDULING_SERVER);
                StringBuilder ussList = new StringBuilder();
                int numUSS = 0;
                foreach (IntTag uss in usses)
                {
                    ussList.Append(uss.tag);
                    ussList.Append(" <br> ");
                    numUSS = numUSS + 1;
                }
                txtUSSlist.Text = ussList.ToString();
                txtUSSnum.Text = numUSS.ToString();

                //Put in availabe ESS
                IntTag[] esses = dbTicketing.GetProcessAgentTagsByType(ProcessAgentType.EXPERIMENT_STORAGE_SERVER);
                StringBuilder essList = new StringBuilder();
                int numESS = 0;
                foreach (IntTag ess in esses)
                {
                    essList.Append(ess.tag); 
                    essList.Append(" <br> ");
                    numESS = numESS + 1;
                }
                txtESSlist.Text = essList.ToString();
                txtESSnum.Text = numESS.ToString();

                //Put in availabe LSS
                IntTag[] lsses = dbTicketing.GetProcessAgentTagsByType(ProcessAgentType.LAB_SCHEDULING_SERVER);
                StringBuilder lssList = new StringBuilder();
                int numLSS = 0;
                foreach (IntTag lss in lsses)
                {
                    lssList.Append(lss.tag);
                    lssList.Append(" <br> ");
                    numLSS = numLSS + 1;
                }
                txtLSSlist.Text = lssList.ToString();
                txtLSSnum.Text = numLSS.ToString();

                //Put in availabe LS
                IntTag[] lses = dbTicketing.GetProcessAgentTagsByType(ProcessAgentType.LAB_SERVER);
                StringBuilder lsList = new StringBuilder();
                int numLS = 0;
                foreach (IntTag ls in lses)
                {
                    lsList.Append(ls.tag);
                    lsList.Append(" <br> ");
                    numLS = numLS + 1;
                }
                txtLSlist.Text = lsList.ToString();
                txtLSnum.Text = numLS.ToString();

                //Put in availabe BLS
                IntTag[] blses = dbTicketing.GetProcessAgentTagsByType(ProcessAgentType.BATCH_LAB_SERVER);
                StringBuilder blsList = new StringBuilder();
                int numBLS = 0;
                foreach (IntTag bls in blses)
                {
                    blsList.Append(bls.tag);
                    blsList.Append(" <br> ");
                    numBLS = numBLS + 1;
                }
                txtBLSlist.Text = blsList.ToString();
                txtBLSnum.Text = numBLS.ToString();

                //Put in availabe RSB
                IntTag[] rsbes = dbTicketing.GetProcessAgentTagsByType(ProcessAgentType.REMOTE_SERVICE_BROKER);
                StringBuilder rsbList = new StringBuilder();
                int numRSB = 0;
                foreach (IntTag rsb in rsbes)
                {
                    rsbList.Append(rsb.tag);
                    rsbList.Append(" <br> ");
                    numRSB = numRSB + 1;
                }
                txtRSBlist.Text = rsbList.ToString();
                txtRSBnum.Text = numRSB.ToString();

                // Put in clients
                int labClientID;
                int[] labClientIDs;
                LabClient[] labClients;
                labClientIDs = wrapper.ListLabClientIDsWrapper();
                labClients = wrapper.GetLabClientsWrapper(labClientIDs);
                StringBuilder lcList = new StringBuilder();
                int numLC = 0;
                foreach (LabClient lc in labClients)
                {
                    lcList.Append(lc.clientName);
                    lcList.Append(" (");
                    lcList.Append(lc.clientType);
                    lcList.Append(") ");
                    lcList.Append(" <br> ");
                    numLC = numLC + 1;
                }
                txtLClist.Text = lcList.ToString();
                txtLCnum.Text = numLC.ToString();
                

                // Group information
               //int groupNum = GroupListRepeater();
               // txtGroupNum.Text = groupNum.ToString();

                // Number of Users
                int numUsers=0;
   				int[] userIDs = wrapper.ListUserIDsWrapper ();
                User[] users = wrapper.GetUsersWrapper(userIDs);
                foreach (int uID in userIDs)
                {
                    numUsers = numUsers + 1;
                } 
                txtUsersNum.Text = numUsers.ToString();

                // List Users Per group
                int groupNum = UserPerGroupRepeater();
                txtGroupNum.Text = groupNum.ToString();
            } //end else 

            // Client Information
        }


        private int UserPerGroupRepeater()
        {
            int gNum = 0;   // number of groups
            int rNum = 0;   // number of user regular groups
            int cNum = 0;   // number of course staff groups
            int sNum = 0;   // number of service admin groups

            try
            {
                groupUList = new ArrayList();
                int[] groupIDs = wrapper.ListGroupIDsWrapper();
                Group[] groups = wrapper.GetGroupsWrapper(groupIDs);
                foreach (Group g in groups)
                {
                    if (g.groupID > 0)
                    {
                        if (
                            (!g.groupName.Equals(Group.ROOT))
                          && (!g.groupName.Equals(Group.ORPHANEDGROUP)) && (!g.groupName.Equals(Group.NEWUSERGROUP))
                          && (g.groupType.Equals(GroupType.REGULAR) || g.groupType.Equals(GroupType.COURSE_STAFF) || g.groupType.Equals(GroupType.SERVICE_ADMIN)))
                        {
                            gNum = gNum + 1;
                            // group info
                            GroupReportList groupRL = new GroupReportList();
                            groupRL.gname = g.groupName;
                            groupRL.gdesc = g.description;
                            groupRL.gtype = g.groupType;
                            // # of users in group
                            int[] childUserIDs = wrapper.ListUserIDsInGroupWrapper(g.groupID);
                            groupRL.usersingroup = childUserIDs.Length;
                            //Response.Write(groupRL.gname);
                            //Response.Write(groupRL.usersingroup);

                            // if this is a reqular group, get the associated lab clients
                            if ((g.groupType.Equals(GroupType.REGULAR)) && (!g.groupName.Equals(Group.SUPERUSER)))
                            {
                                rNum = rNum + 1;
                                int[] lcIDsList = AdministrativeUtilities.GetGroupLabClients(g.groupID);
                                LabClient[] lcList = wrapper.GetLabClientsWrapper(lcIDsList);

                                Label lblGroupLabs = new Label();
                                lblGroupLabs.Visible = true;
                                lblGroupLabs.Text = "<ul>";

                                for (int i = 0; i < lcList.Length; i++)
                                {
                                    lblGroupLabs.Text += "<li><strong class=lab>" +
                                    lcList[i].clientName + "</strong> - " +
                                    lcList[i].clientShortDescription + "</li>";
                                }
                                lblGroupLabs.Text += "</ul>";
                                groupRL.gclients = lblGroupLabs.Text.ToString();
                            }
                            else
                            {
                                Label lblGroupLabs = new Label();
                                lblGroupLabs.Visible = true;
                                lblGroupLabs.Text = "Not Applicable";
                                groupRL.gclients = lblGroupLabs.Text.ToString();
                            }

                            if (g.groupType.Equals(GroupType.COURSE_STAFF))
                            {
                                cNum = cNum + 1;
                            };
                            if (g.groupType.Equals(GroupType.SERVICE_ADMIN))
                            {
                                sNum = sNum + 1;
                            };

                            txtCGroupNum.Text = cNum.ToString();
                            txtSGroupNum.Text = sNum.ToString();
                            txtRGroupNum.Text = rNum.ToString();

                            groupUList.Add(groupRL);

                        }
                    }
                }
                repUserGroups.DataSource = groupUList;
                repUserGroups.DataBind();
            }
            catch (Exception ex)
            {
                lblResponse.Text = Utilities.FormatErrorMessage("Cannot list users in groups. " + ex.GetBaseException());
            }
            return gNum;
        }


        /*
     private int GroupListRepeater()
       {
           int gNum = 0;
           try
           {
               int[] groupIDs = wrapper.ListGroupIDsWrapper();
               groupList = new ArrayList();
               Group[] groups = wrapper.GetGroupsWrapper(groupIDs);
               foreach (Group g in groups)
               {
                   if (g.groupID > 0)
                       if (
                           (!g.groupName.Equals(Group.ROOT))
                         && (!g.groupName.Equals(Group.SUPERUSER))
                         && (!g.groupName.Equals(Group.ORPHANEDGROUP)) && (!g.groupName.Equals(Group.NEWUSERGROUP))
                         && (g.groupType.Equals(GroupType.REGULAR) || g.groupType.Equals(GroupType.SERVICE_ADMIN)))
                       {
                           groupList.Add(g);
                           gNum = gNum + 1;
                       }
               }
               repGroups.DataSource = groupList;
               repGroups.DataBind();
           }
           catch (Exception ex)
           {
               lblResponse.Text = Utilities.FormatErrorMessage("Cannot list groups. " + ex.GetBaseException());
           }
           return gNum;
       }
*/


    }
}

