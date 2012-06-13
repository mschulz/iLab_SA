/*
 * Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
 * Please see license.txt in top level directory for full license.
 * 
 * $Id$
 */
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
using System.Web.Security;

using iLabs.DataTypes;
using iLabs.ServiceBroker.Internal;
using iLabs.ServiceBroker;
using iLabs.ServiceBroker.Administration;
using iLabs.ServiceBroker.Authentication;
using iLabs.ServiceBroker.Authorization;
using iLabs.UtilLib;

namespace iLabs.ServiceBroker.admin
{
	/// <summary>
	/// Summary description for loginRecords.
	/// </summary>
    public partial class sessionHistory : System.Web.UI.Page
    {
        AuthorizationWrapperClass wrapper = new AuthorizationWrapperClass();
        CultureInfo culture;
        int userTZ;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            
            if (Session["UserID"] == null)
                Response.Redirect("../login.aspx");
            culture = DateUtil.ParseCulture(Request.Headers["Accept-Language"]);
            if (Session["userTZ"] != null)
            {
                userTZ = Convert.ToInt32(Session["userTZ"]);
            }
            if (!IsPostBack)
            {
                lblTimezone.Text = "Times are GMT " + userTZ / 60.0;
                LoadAuthorityList();
            }
            if (ddlTimeIs.SelectedIndex != 4)
            {
                txtTime2.ReadOnly = true;
                txtTime2.BackColor = Color.Lavender;
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
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {

        }
        #endregion

        private void LoadAuthorityList()
        {
            ddlAuthority.Items.Clear();
            BrokerDB brokerDB = new BrokerDB();
            IntTag[] authTags = brokerDB.GetAuthorityTags();
            if (authTags != null && authTags.Length > 0)
            {
                foreach (IntTag t in authTags)
                {
                    ListItem li = new ListItem(t.tag, t.id.ToString());
                    ddlAuthority.Items.Add(li);
                }
                ddlAuthority.SelectedValue = "0";
            }
        }

        protected void ddlTimeIs_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            txtTime1.Text = null;
            txtTime2.Text = null;
            if (ddlTimeIs.SelectedIndex == 4)
            {
                txtTime2.ReadOnly = false;
                txtTime2.BackColor = Color.White;
            }
        }
        //list the log-in sessions according to the selected criterion
        private void BuildListBox(int userID, int groupID, DateTime time1, DateTime time2, bool cvs)
        {
            string pad = "  ";
            try
            {
                DataSet sessionData = InternalAdminDB.SelectSessionHistory(userID, groupID, time1, time2);
                DataTable sessionHistory = sessionData.Tables[0];

                if (sessionHistory.Rows.Count == 0)
                {
                    lblResponse.Text = Utilities.FormatWarningMessage(" No sessions found for your criteria.");
                    lblResponse.Visible = true;
                }
                else
                {
                    StringBuilder buf = new StringBuilder();
                    buf.AppendLine(String.Format("{0,-14}{1,-18}{2,-10}{3,-10}{4,-26}{5,-24}",
                        "User Name", "Create Time", "End Time","Modify","Group","Client"));
                    
                    DataTableReader dtr = new DataTableReader(sessionHistory);
                    while(dtr.Read())
                    {
                        //buf.Append(dtr.GetInt64(0) + pad);
                        buf.Append(String.Format("{0,-12:12}", dtr.GetString(1)) + pad);
                        if(!dtr.IsDBNull(2))
                            buf.Append(DateUtil.ToUserTime(dtr.GetDateTime(2),
                                CultureInfo.InvariantCulture,userTZ,"g") +pad);
                        else
                            buf.Append(String.Format("{0,-9:9}", "Not Set") + pad);
                        if (!dtr.IsDBNull(3))
                            buf.Append(DateUtil.ToUserTime(dtr.GetDateTime(3), CultureInfo.InvariantCulture, userTZ, "T") + pad);
                        else
                            buf.Append(String.Format("{0,-8}", "Not Set") + pad);
                        if (!dtr.IsDBNull(4))
                            buf.Append(DateUtil.ToUserTime(dtr.GetDateTime(4), CultureInfo.InvariantCulture, userTZ, "T") + pad);
                        else
                            buf.Append(String.Format("{0,-8}", "Not Set") + pad);
                        string tmp = dtr.GetString(5);
                        if (tmp != null && tmp.Length > 24)
                            tmp = tmp.Substring(0, 24);
                        buf.Append(String.Format("{0,-24}",tmp) + pad);
                        tmp = dtr.GetString(6);
                        if (tmp != null && tmp.Length > 24)
                            tmp = tmp.Substring(0, 24);
                        buf.Append(String.Format("{0,-24}",tmp));
                        
                        buf.AppendLine();
                    }
                    txtLoginDisplay.Text = buf.ToString();
                }
            }
            catch (Exception ex)
            {
                lblResponse.Text = Utilities.FormatErrorMessage(" Cannot retrieve UserSessions. " + ex.GetBaseException());
                lblResponse.Visible = true;
            }
        }
        protected void btnGo_Click(object sender, System.EventArgs e)
        {
            int userID = -1;
            int groupID = -1;
            int authID = 0;
            DateTime startTime = DateTime.MinValue;
            DateTime endTime = DateTime.MaxValue;
            txtLoginDisplay.Text = null;
            if (!(txtGroupName.Text == "" && txtUserName.Text == "" && txtTime1.Text == "" && txtTime2.Text == ""))
            {
                if (txtUserName.Text != "")
                {
                    if(ddlAuthority.SelectedValue != null)
                        authID = Convert.ToInt32(ddlAuthority.SelectedValue);
                    userID = wrapper.GetUserIDWrapper(txtUserName.Text,authID);
                    if (userID == -1)
                    {
                        lblResponse.Text = Utilities.FormatWarningMessage(" No user with the username '" + txtUserName.Text + "' found.");
                        lblResponse.Visible = true;
                        return;
                    }
                }
                if (txtGroupName.Text != "")
                {
                    groupID = wrapper.GetGroupIDWrapper(txtGroupName.Text);
                    if (groupID == -1)
                    {
                        lblResponse.Text = Utilities.FormatWarningMessage(" Group '" + txtGroupName.Text + "' not found.");
                        lblResponse.Visible = true;
                        return;
                    }
                }
                if (ddlTimeIs.SelectedIndex > 0)
                {
                    DateTime time1;
                    try
                    {
                        
                        time1 = DateUtil.ParseUserToUtc(txtTime1.Text.Trim(), culture, userTZ);
                    }
                    catch
                    {
                        lblResponse.Text = Utilities.FormatWarningMessage(" Please enter a valid time.");
                        lblResponse.Visible = true;
                        return;
                    }
                    if (ddlTimeIs.SelectedIndex == 1)
                    {
                        startTime = time1;
                        endTime = time1;

                    }
                    else if (ddlTimeIs.SelectedIndex == 2)
                    {
                        endTime = time1;

                    }
                    else if (ddlTimeIs.SelectedIndex == 3)
                    {
                        startTime = time1;

                    }
                    else if (ddlTimeIs.SelectedIndex == 4)
                    {
                        DateTime time2;
                        try
                        {
                            time2 = DateUtil.ParseUserToUtc(txtTime2.Text.Trim(), culture, userTZ);
                        }
                        catch
                        {
                            lblResponse.Text = Utilities.FormatWarningMessage(" Please enter a valid time.");
                            lblResponse.Visible = true;
                            return;
                        }
                        if (time2 < time1)
                        {
                            lblResponse.Text = Utilities.FormatWarningMessage(" The second time must be greater than or equal to the start time.");
                            lblResponse.Visible = true;
                            return;
                        }
                        startTime = time1;
                        endTime = time2;

                    }
                }
            }
            BuildListBox(userID, groupID, startTime, endTime,false);
        }

    }
}

