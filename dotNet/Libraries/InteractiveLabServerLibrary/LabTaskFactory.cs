using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using iLabs.DataTypes.StorageTypes;
using iLabs.DataTypes.TicketingTypes;
using iLabs.DataTypes.SoapHeaderTypes;


namespace iLabs.LabServer.Interactive
{

    /// <summary>
    /// Summary description for LabTaskFactory
    /// </summary>
    public class LabTaskFactory
    {
        public LabTaskFactory()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public virtual LabTask CreateLabTask(LabAppInfo appInfo, Coupon expCoupon, Ticket expTicket)
        {
            return new LabTask();
        }
    }
}