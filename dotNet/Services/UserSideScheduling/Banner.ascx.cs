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
        lblTitleofUSS.Text = System.Configuration.ConfigurationManager.AppSettings["ussName"];
        // Get User Name
        if (Session["UserName"] != null)
        {
            lblUserNameBanner.Visible = true;
            lblUserNameBanner.Text = "User: " + Session["userName"].ToString();
        }
        else
        {
            lblUserNameBanner.Visible = false;
        }

        // Get Group Name
        if (Session["GroupName"] != null)
        {
            lblGroupNameBanner.Text = "Group: " + Session["groupName"].ToString();
            lblGroupNameBanner.Visible = true;
        }
        else
        {
            lblGroupNameBanner.Visible = false;
        }
    }
}
