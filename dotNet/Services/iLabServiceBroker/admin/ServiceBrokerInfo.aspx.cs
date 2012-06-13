using System;
using System.Collections;
using System.Configuration;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Security;

using iLabs.Core;
using iLabs.DataTypes;
using iLabs.DataTypes.ProcessAgentTypes;
using iLabs.DataTypes.SoapHeaderTypes;
using iLabs.DataTypes.TicketingTypes;
using iLabs.ServiceBroker;
using iLabs.ServiceBroker.Internal;
using iLabs.ServiceBroker.Administration;
using iLabs.ServiceBroker.Authorization;
using iLabs.ServiceBroker;
using iLabs.Ticketing;

//using iLabs.Services;


using iLabs.UtilLib;
public partial class admin_ServiceBrokerInfo : System.Web.UI.Page
{
   
    protected void Page_Load(object sender, EventArgs e)
    {
       
        txtAgentGUID.Text = System.Configuration.ConfigurationManager.AppSettings["serviceGUID"].ToString();
        txtURL.Text = System.Configuration.ConfigurationManager.AppSettings["codebaseURL"].ToString();
        txtName.Text = System.Configuration.ConfigurationManager.AppSettings["serviceName"].ToString();
        BrokerDB ticketing = new BrokerDB();
        if (ticketing.GetProcessAgent(txtAgentGUID.Text) != null)
        {
            btnAssociated.Enabled = false;
        }
        else
        {
            btnAssociated.Enabled = true;
        }
        

     

    }
    protected void btnAssociated_Click(object sender, EventArgs e)
    {
        BrokerDB ticketing = new BrokerDB();
        Coupon coupon = null;
        string webserviceURL = System.Configuration.ConfigurationManager.AppSettings["serviceURL"].ToString();
        //string infoURL = System.Configuration.ConfigurationManager.AppSettings["infoURL"].ToString();
        string contactEmail = System.Configuration.ConfigurationManager.AppSettings["registrationMailAddress"].ToString();

        ticketing.SelfRegisterProcessAgent(txtAgentGUID.Text, txtName.Text, ProcessAgentType.SERVICE_BROKER,
            txtAgentGUID.Text, txtURL.Text, webserviceURL);
        ProcessAgentDB.RefreshServiceAgent();
        //ticketing.InsertProcessAgent(txtAgentGUID.Text, txtName.Text,
        //    ProcessAgentType.SERVICE_BROKER, null,  webserviceURL, txtURL.Text, infoURL,  contactEmail, -5,
        //    coupon, coupon);
    }
}
