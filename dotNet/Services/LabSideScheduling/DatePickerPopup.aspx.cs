using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using iLabs.UtilLib;

namespace JavaScriptPopups
{
    /// <summary>
    /// Summary description for datePickerPopup.
    /// </summary>
    public partial class datePickerPopup : System.Web.UI.Page
    {
        string dateParam;
        CultureInfo culture;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            culture = DateUtil.ParseCulture(Request.Headers["Accept-Language"]);
            dateParam = Request.QueryString["date"];
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

        }
        #endregion
        // This routine will create a javascript block to be executed on the client.
        protected void popupDate_changed(Object Src, EventArgs E)
        {
            StringBuilder buf = new StringBuilder();
            ClientScriptManager cs = Page.ClientScript;
            buf.Append("setDate('");
            buf.Append(dateParam + "','");
            buf.AppendLine(calDate.SelectedDate.ToString(culture.DateTimeFormat.ShortDatePattern) + "');");
           
            // close the popup calendar
            buf.AppendLine("self.close()");
            
            cs.RegisterStartupScript(this.GetType(), "dayClick", buf.ToString(), true);
        }

    }
}
