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

public partial class Banner : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        lblTitleofLSS.Text = System.Configuration.ConfigurationManager.AppSettings["LSSName"];
        // Get Lab Server Name
        if (Session["labServerName"] != null)
        {
            lblUserNameBanner.Visible = true;
            lblUserNameBanner.Text = "Lab Server: " + Session["labServerName"].ToString();
        }
        else
        {
            lblUserNameBanner.Visible = false;
        }
        if (Session["adminGroup"] != null)
        {
            lblGroup.Visible = true;
            lblGroup.Text = "Group: " + Session["adminGroup"].ToString();
        }
        else
        {
            lblGroup.Visible = false;
        }

      
    }
}

