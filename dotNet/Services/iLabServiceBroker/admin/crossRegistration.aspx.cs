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
using System.Xml;
using System.Xml.XPath;

using iLabs.Core;
using iLabs.DataTypes;
using iLabs.DataTypes.ProcessAgentTypes;
using iLabs.DataTypes.SoapHeaderTypes;
using iLabs.DataTypes.TicketingTypes;
using iLabs.ServiceBroker;
using iLabs.ServiceBroker.Administration;
using iLabs.ServiceBroker.Authorization;
using iLabs.ServiceBroker.Internal;
using iLabs.ServiceBroker.iLabSB;


using iLabs.UtilLib;
using iLabs.Ticketing;
//using iLabs.Services;

namespace iLabs.ServiceBroker.admin 
{

    public partial class CrossRegistration : System.Web.UI.Page
    {
        protected BrokerDB brokerDb = new BrokerDB();


        AuthorizationWrapperClass wrapper = new AuthorizationWrapperClass();

        int labClientID;
        int[] labClientIDs;
        LabClient[] labClients;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Session.Remove("CrossRegLSS");
                initialDropDownList();
            }
        }

        protected void initialDropDownList()
        {
            ddlServiceBroker.Items.Clear();
            IntTag[] tags = brokerDb.GetProcessAgentTagsByType(ProcessAgentType.REMOTE_SERVICE_BROKER);

            ddlServiceBroker.Items.Add(new ListItem(" ------------- select Service Broker ------------ ", "0"));
            if (tags != null)
            {
                foreach (IntTag t in tags)
                {
                    ddlServiceBroker.Items.Add(new ListItem(t.tag, t.id.ToString()));
                }
            }
            ddlClient.Items.Clear();
            labClientIDs = wrapper.ListLabClientIDsWrapper();

            ddlClient.Items.Add(new ListItem(" ------------- select Client ------------ ", "0"));
            if (labClientIDs != null)
            {
                labClients = wrapper.GetLabClientsWrapper(labClientIDs);
                foreach (LabClient lc in labClients)
                {
                    // Check that the client is not a 6.1 Batch client and that it is from this domain
                    if ((lc.clientType.CompareTo(LabClient.BATCH_APPLET) != 0) && (lc.clientType.CompareTo(LabClient.BATCH_APPLET) != 0))
                    {
                        ProcessAgentInfo[] labServers = AdministrativeAPI.GetLabServersForClient(lc.clientID);
                        if (labServers != null && (labServers.Length > 0) && (labServers[0].agentId > 0))
                        {
                            if (labServers[0].domainGuid.Equals(ProcessAgentDB.ServiceGuid))
                            {
                                ddlClient.Items.Add(new ListItem(lc.clientName, lc.clientID.ToString()));
                            }
                        }
                    }
                }
            }
        }

        protected void ddlServiceBroker_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblResponse.Visible = false;
        }
        protected void ddlClient_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblResponse.Visible = false;
            ddlLabServer.Items.Clear();
            ddlLabServer.Items.Add(new ListItem(" ------------- select Lab Server ------------ ", "0"));
            if (Int32.Parse(ddlClient.SelectedValue) > 0)
            {
                int selectedLabClientID = Int32.Parse(ddlClient.SelectedValue);
                ProcessAgentInfo[] pais = AdministrativeAPI.GetLabServersForClient(selectedLabClientID);
                foreach (ProcessAgentInfo pai in pais)
                {
                    if (!pai.retired && pai.domainGuid.Equals(ProcessAgentDB.ServiceGuid))
                    {
                        ddlLabServer.Items.Add(new ListItem(pai.AgentName, pai.AgentId.ToString()));
                    }
                }
            }
        }

        protected void ddlLabServer_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblLSS.Text = null;
            lblResponse.Visible = false;
            Session.Remove("CrossRegLSS");
            if (Int32.Parse(ddlLabServer.SelectedValue) > 0)
            {
                int selectedLabServerID = Int32.Parse(ddlLabServer.SelectedValue);
                //LabServer[] labServers = InternalAdminDB.SelectLabServers(new int[]{selectedLabServerID});
                //Hashtable mappingTable = brokerDb.GetResourceMappingsForKey(selectedLabServerID, ResourceMappingTypes.PROCESS_AGENT);
                //ResourceMappingValue[][] rsrcValues = brokerDb.GetResourceMappingValues(mappingTable);
                int lssId = brokerDb.FindProcessAgentIdForAgent(selectedLabServerID, ProcessAgentType.LAB_SCHEDULING_SERVER);
                if (lssId > 0)
                {
                    ProcessAgent lss = brokerDb.GetProcessAgent(lssId);
                    lblLSS.Text = lss.agentName;
                    Session["crossRegLSS"] = lssId;
                }

            }

        }

        protected void btnRegister_Click(object sender, System.EventArgs e)
        {
            lblResponse.Visible = false;
            bool error = false;
            int lssID = 0;
            StringBuilder message = new StringBuilder();

            try
            {
                // Check for enough information to perform the register

                if (!(ddlServiceBroker.SelectedIndex > 0))
                {
                    message.AppendLine("You must select a Remote ServiceBroker!");
                    error = true;
                }
                if (!(ddlLabServer.SelectedIndex > 0))
                {

                    message.AppendLine("You must select a LabServer!");
                    error = true;
                }
                if (Session["crossRegLSS"] != null)
                {
                    lssID = (int)Session["CrossRegLSS"];
                }
                if (!(ddlClient.SelectedIndex > 0))
                {

                    message.AppendLine("You must select a Lab Client!");
                    error = true;
                }
                if (error)
                {
                    lblResponse.Text = Utilities.FormatErrorMessage(message.ToString());
                    lblResponse.Visible = true;
                }
                else
                {
                    message.AppendLine(RegistrationSupport.RegisterClientServices(Int32.Parse(ddlServiceBroker.SelectedValue),
                          null, lssID, null, Int32.Parse(ddlLabServer.SelectedValue),
                         null, Int32.Parse(ddlClient.SelectedValue)));
                    lblResponse.Text = Utilities.FormatConfirmationMessage(message.ToString());
                    lblResponse.Visible = true;

                }


            }
            catch (Exception ex)
            {
               Logger.WriteLine("Error in cross-Register: " + ex.Message);
                throw;
            }

        }

    }

}
