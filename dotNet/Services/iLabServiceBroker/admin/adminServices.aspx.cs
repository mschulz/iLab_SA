using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Web;
using System.Text;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Security;
using System.Web.Mail;

using iLabs.Core;
using iLabs.ServiceBroker;
using iLabs.ServiceBroker.Internal;
using iLabs.ServiceBroker.Administration;
using iLabs.ServiceBroker.Authorization;
using iLabs.ServiceBroker.Mapping;
using iLabs.DataTypes;
using iLabs.DataTypes.ProcessAgentTypes;
using iLabs.DataTypes.TicketingTypes;
using iLabs.Ticketing;
using iLabs.UtilLib;

namespace iLabs.ServiceBroker.admin
{
    public partial class adminServices : System.Web.UI.Page
    {
       
        BrokerDB ticketIssuer = new BrokerDB();
        AuthorizationWrapperClass wrapper = new AuthorizationWrapperClass();
        Dictionary<int, List<Grant>> servAdminGrants;

        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (Session["UserID"] == null)
                Response.Redirect("../login.aspx");

            if (!IsPostBack)
            {
                // build table of service admin grants
                servAdminGrants = getServiceAdminGrants();
                if (servAdminGrants != null)
                {
                    // load the PA's that this user is an administer of         
                    Dictionary<int, List<Grant>>.Enumerator paEnum = servAdminGrants.GetEnumerator();
                    paDropDownList.Items.Add(new ListItem("--- Select Process Agent ---"));


                    while (paEnum.MoveNext())
                    {
                        KeyValuePair<int, List<Grant>> entry = paEnum.Current;
                        IntTag tag = ticketIssuer.GetProcessAgentTagWithType((int)entry.Key);
                        if (tag != null)
                        {
                            string agentName = tag.tag;
                            List<Grant> grants = entry.Value;
                            StringBuilder buf = new StringBuilder();
                            int count = 0;
                            foreach (Grant g in grants)
                            {
                                if (count > 0)
                                {
                                    buf.Append(",");
                                }
                                buf.Append(g.grantID);
                                count++;
                            }
                            paDropDownList.Items.Add(new ListItem(agentName, buf.ToString()));
                        }
                    }
                }
               
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
            //this.btnRefresh.ServerClick += new System.EventHandler(this.btnRefresh_ServerClick);
            //
            // The following is added to support window popups from within the repeater.
            //
            this.repAdminGrants.ItemCreated += new RepeaterItemEventHandler(this.repAdminGrants_ItemCreated);
            this.repAdminGrants.ItemDataBound += new RepeaterItemEventHandler(this.repAdminGrants_ItemBound);
            this.repAdminGrants.ItemCommand += new RepeaterCommandEventHandler(this.repAdminGrants_ItemCommand);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {

        }
        #endregion

        /// <summary>
        /// Returns a hashmap of service admin grants. 
        /// Keys are process agent IDs where the process agent is the qualifier of the grant
        /// Values are lists of service admin grants that have that process agent as a qualifier
        /// 
        /// A grant is a service admin grant if
        ///     1. The agent is a service admin group
        ///     2. The function should be a "service admin" or "service management" ticket type 
        ///     3. qualifier should be a process agent
        /// </summary>
        /// <returns></returns>
        protected Dictionary<int, List<Grant>> getServiceAdminGrants()
        {
            // initializations
            Dictionary<int, List<Grant>> servAdminGrants = new Dictionary<int, List<Grant>>();
            AuthorizationWrapperClass wrapper = new AuthorizationWrapperClass();

            // get all grants
            int[] grantIDs = wrapper.ListGrantIDsWrapper();
            Grant[] grants = wrapper.GetGrantsWrapper(grantIDs);

            int effGroupID = Convert.ToInt32(Session["GroupID"]);

            foreach (Grant grant in grants)
            {
                if (grant.agentID.Equals(effGroupID))
                {
                    // check if the grant is a service admin or service manage grant
                    // 1. agent should be a "service admin" group
                    // 2. function should be a "service admin" or "service management" ticket type 
                    // 3. qualifier should be a process agent
                    int paID = 0;
                    Qualifier qualifier = new Qualifier();
                    string function = grant.function;
                    if (TicketTypes.IsAdministerPAType(function))
                    {

                        // get process agent that corresponds to qualifier
                        qualifier = AuthorizationAPI.GetQualifier(grant.qualifierID);
                        paID = qualifier.qualifierReferenceID;
                    }
                    else if (TicketTypes.IsManagePAType(function))
                    {
                        bool isManage = true;
                        bool isProcessAgent = false;
                        int targetId = -1;
                        qualifier = AuthorizationAPI.GetQualifier(grant.qualifierID);
                        //Qualifier is a Resource Mapping
                        if (qualifier.qualifierType.Equals(Qualifier.resourceMappingQualifierTypeID))
                        {
                            //int resourceMappingID = ;
                            ResourceMapping mapping = ticketIssuer.GetResourceMapping(qualifier.qualifierReferenceID);
                            if (mapping != null)
                            {
                                ResourceMappingKey mappingKey = mapping.Key;
                                if (mapping.Key.Type.Equals(ResourceMappingTypes.GROUP))
                                {
                                    if (mapping.values[0].Type == ResourceMappingTypes.CLIENT)
                                        paID = ticketIssuer.FindProcessAgentIdForClient((int)mapping.values[0].Entry, ProcessAgentType.SCHEDULING_SERVER);
                                }
                                else if (mapping.Key.Type.Equals(ResourceMappingTypes.PROCESS_AGENT))
                                {
                                    if (mapping.values[2].Type.Equals(ResourceMappingTypes.TICKET_TYPE))// && mapping.values[2].Entry.Equals(TicketTypes.GetTicketType(TicketTypes.MANAGE_USS_GROUP)))
                                    {
                                        paID = (int)mapping.values[1].Entry;
                                        ResourceMappingValue[] mappingValues = mapping.values;

                                        //TO BE FIXED: I am assuming that the Mapping has 3 values, the 3rd one being the Ticket Type,
                                        //the 2nd one being the Process Agent, and the 1st one the Resource Type.

                                        //if (mappingValues[2].Type.Equals(ResourceMappingTypes.TICKET_TYPE))
                                        //{
                                        //    if (TicketTypes.IsManagePAType((string)mappingValues[2].Entry))
                                        //        isManage = true;
                                        //}

                                        if (mappingValues[1].Type.Equals(ResourceMappingTypes.PROCESS_AGENT))
                                        {
                                            isProcessAgent = true;
                                            targetId = (int)mappingValues[1].Entry;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (paID > 0)
                    {
                        //get the list of grants that correspond to the process agent qualifier
                        List<Grant> list = new List<Grant>();
                        if (servAdminGrants.TryGetValue(paID, out list))
                        {
                            // process agent already exists
                            // add grant

                            list.Add(grant);
                        }
                        else
                        // process agent does not exist
                        // add list
                        {
                            list = new List<Grant>();

                            list.Add(grant);
                            servAdminGrants.Add(paID, list);

                        }
                    }

                }
            }
            return servAdminGrants;
        }

        /// <summary>
        /// Returns a hashmap of service admin grants. 
        /// Keys are process agent IDs where the process agent is the qualifier of the grant
        /// Values are lists of service admin grants that have that process agent as a qualifier
        /// 
        /// A grant is a service admin grant if
        ///     1. The agent is a service admin group
        ///     2. The function should be a "service admin" or "service management" ticket type 
        ///     3. qualifier should be a process agent
        /// </summary>
        /// <returns></returns>
        protected Dictionary<int, List<Grant>> getServiceAdminGrants_ORIG()
        {
            // initializations
            Dictionary<int, List<Grant>> servAdminGrants = new Dictionary<int, List<Grant>>();
            AuthorizationWrapperClass wrapper = new AuthorizationWrapperClass();

            // get all grants
            int[] grantIDs = wrapper.ListGrantIDsWrapper();
            Grant[] grants = wrapper.GetGrantsWrapper(grantIDs);

            int effGroupID = Convert.ToInt32(Session["GroupID"]);

            foreach (Grant grant in grants)
            {
                // check if the grant is a service admin or service manage grant
                // 1. agent should be a "service admin" group
                // 2. function should be a "service admin" or "service management" ticket type 
                // 3. qualifier should be a process agent
                string function = grant.function;


                // get process agent that corresponds to qualifier
                Qualifier qualifier = AuthorizationAPI.GetQualifier(grant.qualifierID);

                bool isManage = true;
                bool isProcessAgent = false;
                int targetId = -1;

                //Qualifier is a Resource Mapping
                if (qualifier.qualifierType.Equals(Qualifier.resourceMappingQualifierTypeID))
                {
                    int resourceMappingID = qualifier.qualifierReferenceID;
                    ResourceMapping mapping = ticketIssuer.GetResourceMapping(resourceMappingID);

                    ResourceMappingKey mappingKey = mapping.Key;
                    if (mappingKey.Type.Equals(ResourceMappingTypes.GROUP) ||
                        mappingKey.Type.Equals(ResourceMappingTypes.PROCESS_AGENT))
                    {
                        ResourceMappingValue[] mappingValues = mapping.values;

                        //TO BE FIXED: I am assuming that the Mapping has 3 values, the 3rd one being the Ticket Type,
                        //the 2nd one being the Process Agent, and the 1st one the Resource Type.

                        //if (mappingValues[2].Type.Equals(ResourceMappingTypes.TICKET_TYPE))
                        //{
                        //    if (TicketTypes.IsManagePAType((string)mappingValues[2].Entry))
                        //        isManage = true;
                        //}

                        if (mappingValues[1].Type.Equals(ResourceMappingTypes.PROCESS_AGENT))
                        {
                            isProcessAgent = true;
                            targetId = (int)mappingValues[1].Entry;
                        }
                    }
                }

                int paID = 0;

                if (isManage && isProcessAgent && targetId > 0)
                {
                    paID = targetId;
                }

                else
                {
                    paID = qualifier.qualifierReferenceID;
                }

                if (TicketTypes.IsAdministerPAType(function) || TicketTypes.IsManagePAType(function) &&
                    paID > 0)
                {
                    //get the list of grants that correspond to the process agent qualifier
                    List<Grant> list = new List<Grant>();
                    if (servAdminGrants.TryGetValue(paID, out list))
                    {
                        // process agent already exists
                        // add grant
                        if (grant.agentID.Equals(effGroupID))
                            list.Add(grant);
                    }
                    else
                    // process agent does not exist
                    // add list
                    {
                        list = new List<Grant>();
                        if (grant.agentID.Equals(effGroupID))
                        {
                            list.Add(grant);
                            servAdminGrants.Add(paID, list);
                        }
                    }
                }
            }
            return servAdminGrants;
        }


        private string CreatePayloadForAdminTicket(string ticketType)
        {
            string payload = "";

            TicketLoadFactory factory = TicketLoadFactory.Instance();
            int userTZ = Convert.ToInt32(Session["UserTZ"]);

            if (ticketType.Equals(TicketTypes.ADMINISTER_USS))
                payload = factory.createAdministerUSSPayload(userTZ);
            else if (ticketType.Equals(TicketTypes.ADMINISTER_LSS))
                payload = factory.createAdministerLSSPayload(userTZ);

            return payload;
        }

        protected void repAdminGrants_ItemCreated(object source, RepeaterItemEventArgs e)
        {
        }

        protected void repAdminGrants_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName.Equals("Redirect"))
            {
                Object o = e.CommandArgument;
                // get current grant
                int curGrantId = Convert.ToInt32(e.CommandArgument);
                Grant curGrant = AuthorizationAPI.GetGrants(new int[] { curGrantId })[0];

                string ticketType = curGrant.Function;
                Qualifier qualifier = AuthorizationAPI.GetQualifier(curGrant.qualifierID);

                int processAgentID = 0;
                ProcessAgent processAgent = null;
                ProcessAgent labServer = null;
                string groupName = null;
                string clientGuid = null;
                
                if (qualifier.qualifierType.Equals(Qualifier.resourceMappingQualifierTypeID))
                {
                    int resourceMappingID = qualifier.qualifierReferenceID;
                    ResourceMapping mapping = ticketIssuer.GetResourceMapping(resourceMappingID);

                    ResourceMappingKey mappingKey = mapping.Key;

                    if (mappingKey.Type.Equals(ResourceMappingTypes.GROUP))
                    {
                        int groupID = (int)mappingKey.Entry;
                        groupName = AdministrativeAPI.GetGroups(new int[] { groupID })[0].groupName;
                        if (mapping.values.Length >= 1 && mapping.values[0].Type == ResourceMappingTypes.CLIENT)
                        {
                            clientGuid = AdministrativeAPI.GetLabClientGUID((int)mapping.values[0].Entry);
                            processAgentID = ticketIssuer.FindProcessAgentIdForClient((int)mapping.values[0].Entry, ProcessAgentType.SCHEDULING_SERVER);
                        }
                    }

                    else if (mappingKey.Type.Equals(ResourceMappingTypes.PROCESS_AGENT))
                    {
                        int lsId = (int)mappingKey.Entry;
                        labServer = ticketIssuer.GetProcessAgent(lsId);
                        ResourceMappingValue[] mappingValues = mapping.values;
                        processAgentID = (int)mappingValues[1].Entry;
                    }

                    
                    
                }

                else
                {
                    processAgentID = qualifier.qualifierReferenceID;
                }

                // get redirect url that corresponds to this process agent and ticket type
                AdminUrl adminUrl = ticketIssuer.RetrieveAdminURL(processAgentID, curGrant.Function);
                if (adminUrl == null)
                    return;
                
                // get the default ticket duration from web.config
                long duration = Convert.ToInt64(ConfigurationManager.AppSettings["serviceAdminTicketDuration"]);

                string payload = "";

                if (TicketTypes.IsAdministerPAType(ticketType))
                    payload = CreatePayloadForAdminTicket(ticketType);

                else if (TicketTypes.IsManagePAType(ticketType))
                {
                    TicketLoadFactory factory = TicketLoadFactory.Instance();

                    //TO BE FIXED: this is a special case. I am assuming that a mapping of key type "GROUP" only
                    //corresponds to a USS
                    if (groupName != null)
                        payload = factory.createManageUSSGroupPayload(groupName, ticketIssuer.GetIssuerGuid(), clientGuid, Convert.ToInt32(Session["UserTZ"]));

                    //TO BE FIXED: this is a special case. I am assuming that a mapping of key type "PROCESS_AGENT" only
                    //corresponds to an LSS
                   if (labServer != null)
                       payload = factory.createManageLabPayload(labServer.agentGuid, labServer.agentName,
                           ProcessAgentDB.ServiceGuid, Session["GroupName"].ToString(), Convert.ToInt32(Session["UserTZ"]));
                    
                }
                processAgent = ticketIssuer.GetProcessAgent(processAgentID);
                // create a ticket that allows the user to administer the process agent
                Coupon coupon = ticketIssuer.CreateTicket(curGrant.Function, processAgent.agentGuid, ticketIssuer.GetIssuerGuid(), duration, payload);

                // construct the redirect coupon
                StringBuilder url = new StringBuilder(adminUrl.Url.Trim());
                if (url.ToString().IndexOf("?") == -1)
                    url.Append('?');
                else
                    url.Append('&');

                string sbUrl = Utilities.ExportUrlPath(Request.Url);

                url.Append("coupon_id=" + coupon.couponId + "&passkey=" + coupon.passkey
                    + "&issuer_guid=" + ticketIssuer.GetIssuerGuid() + "&sb_url=" + sbUrl);
 
                // redirect the browser to the admin page
                Response.Redirect(url.ToString());
            }
        }

        protected void repAdminGrants_ItemBound(object source, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                // get the current grant
                Grant curGrant = (Grant)e.Item.DataItem;

                // get the ticket type that corresponds to the function name
                TicketType ticketType = TicketTypes.GetTicketType(curGrant.Function);
                if (ticketType == null)
                    return;

                Label lbl = (Label)e.Item.FindControl("lblAdmin");
                Button btn = (Button)e.Item.FindControl("btnAdmin");
                if (TicketTypes.IsAdministerPAType(ticketType.name))
                {
                    // set label text
                    lbl.Text = ticketType.shortDescription;
                    // set button label
                    btn.Text = "Administer";
                }
                else if (TicketTypes.IsManagePAType(ticketType.name))
                {
                    Qualifier qual = AuthorizationAPI.GetQualifier(curGrant.qualifierID);
                    if (qual.qualifierType == Qualifier.resourceMappingQualifierTypeID)
                    {
                        ResourceMapping rMap = ticketIssuer.GetResourceMapping(qual.qualifierReferenceID);
                        if (rMap.key.Type == ResourceMappingTypes.PROCESS_AGENT)
                            lbl.Text = ticketIssuer.GetProcessAgent((int)rMap.key.Entry).agentName;
                        else if (rMap.key.Type == ResourceMappingTypes.GROUP)
                        {
                            
                            lbl.Text = AdministrativeAPI.GetGroups(new int[] { (int)rMap.key.Entry })[0].groupName;
                            if ((rMap.values.Length >= 1) && (rMap.values[0].Type == ResourceMappingTypes.CLIENT))
                            {
                                lbl.Text += ": " + AdministrativeAPI.GetLabClientName((int)rMap.values[0].Entry);
                            }
                        }
                        else
                            lbl.Text = qual.qualifierName;
                    }
                    else
                    {
                        lbl.Text = qual.qualifierName;
                    }
                    btn.Text = ticketType.shortDescription;
                }
                btn.CommandArgument = Convert.ToString(curGrant.grantID);
            }
        }

        /// <summary>
        /// Change the admin grant repeater content to show the admin grants that correspond to the selected process agent
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void paDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            // if the first item is not selected
            if (paDropDownList.SelectedIndex > 0)
            {
                // get process agent that corresponds to selected item
                string processAgentName = paDropDownList.SelectedItem.Text;
                string [] grantStr = paDropDownList.SelectedValue.Split(',');
                int [] grantIds = new int[grantStr.Length];
                for(int i = 0;i< grantStr.Length;i++){
                    grantIds[i] = Int32.Parse(grantStr[i]);
                }

                // get the list of Admin Grants that correspond to the selected process agent
                List<Grant> adminGrantsList = new List<Grant>();
                Grant[] grants = AuthorizationAPI.GetGrants(grantIds);
                //if (!servAdminGrants.TryGetValue(processAgentInfo.AgentId, out adminGrantsList))
                //    return;

                repAdminGrants.DataSource = grants;
                repAdminGrants.DataBind();
            }

            else {
                repAdminGrants.DataSource = "";
                repAdminGrants.DataBind();
            }
        }
    }
}
