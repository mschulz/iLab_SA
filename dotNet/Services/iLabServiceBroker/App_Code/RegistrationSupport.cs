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

using iLabs.ServiceBroker.Administration;
using iLabs.ServiceBroker.Internal;
using iLabs.ServiceBroker;
using iLabs.ServiceBroker;
using iLabs.Core;
using iLabs.DataTypes;
using iLabs.DataTypes.ProcessAgentTypes;
using iLabs.DataTypes.SoapHeaderTypes;
using iLabs.DataTypes.TicketingTypes;
using iLabs.Proxies.PAgent;
using iLabs.UtilLib;
using iLabs.Ticketing;

namespace iLabs.ServiceBroker.iLabSB
{
/// <summary>
/// Summary description for RegistrationSupport
/// </summary>
    public class RegistrationSupport
    {
        // Provider Registration methods - issued by the provider

        public static string RegisterClientServices(int sbId, Coupon lssCoupon, int lssId,
            Coupon lsCoupon, int lsId, Coupon clientCoupon, int clientId)
        {
         
            bool error = false;
            StringBuilder message = new StringBuilder();
            string lsDescriptor = null;
            string lssDescriptor = null;
            string clientDescriptor = null;
            ProcessAgentInfo remoteSB;
            ProcessAgentDB db = new ProcessAgentDB();
            ResourceDescriptorFactory factory = ResourceDescriptorFactory.Instance();
            try
            {
                // Check for enough information to perform the register

               
                    lsDescriptor = factory.CreateProcessAgentDescriptor(lsId);
                    if(lssId > 0)
                        lssDescriptor = factory.CreateProcessAgentDescriptor(lssId);
                
                
                    clientDescriptor = factory.CreateClientDescriptor(clientId);
               
                // What tickets do we need, create them

                string guid = Utilities.MakeGuid();
                ServiceDescription[] values = null;
                if (lssDescriptor != null)
                {
                    // Order is important
                    values = new ServiceDescription[3];
                    values[0] = new ServiceDescription(lssDescriptor, lssCoupon, null);
                    values[1] = new ServiceDescription(lsDescriptor, lsCoupon, null);
                    values[2] = new ServiceDescription(clientDescriptor, clientCoupon, null);
                }
                else
                {
                    values = new ServiceDescription[2];
                    values[0] = new ServiceDescription(lsDescriptor, lsCoupon, null);
                    values[1] = new ServiceDescription(clientDescriptor, clientCoupon, null);
                }


                // get the remote sb Proxy
                remoteSB = db.GetProcessAgentInfo(sbId);
                if (remoteSB == null || remoteSB.retired)
                {
                    throw new Exception("The remote service broker is not registered or is retired");
                }
                ProcessAgentProxy sbProxy = new ProcessAgentProxy();
                sbProxy.Url = remoteSB.webServiceUrl;
                sbProxy.AgentAuthHeaderValue = new AgentAuthHeader();
                sbProxy.AgentAuthHeaderValue.coupon = remoteSB.identOut;
                sbProxy.AgentAuthHeaderValue.agentGuid = ProcessAgentDB.ServiceGuid;
                sbProxy.Register(guid, values);
                message.AppendLine("RegisterClientServices: " + remoteSB.webServiceUrl + " \t" + guid + " \t" + "success");
            }
            catch (Exception ex)
            {
               Logger.WriteLine("Error in cross-Register: " + ex.Message);
                message.AppendLine("Error in cross-Register: " + ex.Message);
                //throw;
            }
            return message.ToString();

        }

        // Consumer Registration Methods -- Called by the consumer

        public static string RegisterClientUSS(int sbId, Coupon lssCoupon, int lssId,
            Coupon lsCoupon, int lsId, Coupon ussCoupon, int ussId,
            Coupon clientCoupon, int clientId)
        {

            bool error = false;
            StringBuilder message = new StringBuilder();
            string lsDescriptor = null;
            string lssDescriptor = null;
            string ussDescriptor = null;
            string clientDescriptor = null;
            ProcessAgentInfo remoteSB;
            ProcessAgentDB db = new ProcessAgentDB();
            ResourceDescriptorFactory factory = ResourceDescriptorFactory.Instance();
            try
            {
                // Check for enough information to perform the register


                lsDescriptor = factory.CreateProcessAgentDescriptor(lsId);
                if (lssId > 0)
                    lssDescriptor = factory.CreateProcessAgentDescriptor(lssId);

                ussDescriptor = factory.CreateProcessAgentDescriptor(ussId);
                clientDescriptor = factory.CreateClientDescriptor(clientId);

                // What tickets do we need, create them

                string guid = Utilities.MakeGuid();
                ServiceDescription[] values = null;
                if (lssDescriptor != null)
                {
                    // Order is important
                    values = new ServiceDescription[4];
                    values[0] = new ServiceDescription(null, lssCoupon, lssDescriptor);
                    values[1] = new ServiceDescription(null, lsCoupon, lsDescriptor);
                    values[2] = new ServiceDescription(null, ussCoupon, ussDescriptor);
                    values[3] = new ServiceDescription(null, clientCoupon, clientDescriptor);
                }
                else
                {
                    values = new ServiceDescription[3];
                    values[0] = new ServiceDescription(null, lsCoupon, lsDescriptor);
                    values[1] = new ServiceDescription(null, ussCoupon, ussDescriptor);
                    values[2] = new ServiceDescription(null, clientCoupon, clientDescriptor);
                }


                // get the remote sb Proxy
                remoteSB = db.GetProcessAgentInfo(sbId);
                if (remoteSB == null || remoteSB.retired)
                {
                    throw new Exception("The remote service broker is not registered or is retired");
                }
                ProcessAgentProxy sbProxy = new ProcessAgentProxy();
                sbProxy.Url = remoteSB.webServiceUrl;
                sbProxy.AgentAuthHeaderValue = new AgentAuthHeader();
                sbProxy.AgentAuthHeaderValue.coupon = remoteSB.identOut;
                sbProxy.AgentAuthHeaderValue.agentGuid = ProcessAgentDB.ServiceGuid;
                sbProxy.Register(guid, values);
                message.AppendLine("RegisterClientUSS: " + remoteSB.webServiceUrl + " \t" + guid + " \t" + "success");
            }
            catch (Exception ex)
            {
               Logger.WriteLine("Error in cross-Register: " + ex.Message);
                message.AppendLine("Error in cross-Register: " + ex.Message);
                throw;
            }
            return message.ToString();

        }


        public static string RegisterGroupCredentials(int sbId)
        {
            StringBuilder message = new StringBuilder();
            ProcessAgentInfo remoteSB = null;

            ProcessAgentDB db = new ProcessAgentDB();
            ResourceDescriptorFactory factory = ResourceDescriptorFactory.Instance();
            string guid = Utilities.MakeGuid();
            ServiceDescription[] values = null;
            // get the remote sb Proxy
            remoteSB = db.GetProcessAgentInfo(sbId);
            if (remoteSB == null || remoteSB.retired)
            {
                throw new Exception("The remote service broker is not registered or is retired");
            }
            ProcessAgentProxy sbProxy = new ProcessAgentProxy();
            sbProxy.Url = remoteSB.webServiceUrl;
            sbProxy.AgentAuthHeaderValue = new AgentAuthHeader();
            sbProxy.AgentAuthHeaderValue.coupon = remoteSB.identOut;
            sbProxy.AgentAuthHeaderValue.agentGuid = ProcessAgentDB.ServiceGuid;
            sbProxy.Register(guid, values);
            message.AppendLine("RegisterGroupCredentials: " + remoteSB.webServiceUrl + " \t" + guid + " \t" + "success");
            return message.ToString();
        }
    }
}
