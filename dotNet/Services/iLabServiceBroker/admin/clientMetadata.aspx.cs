using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
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

namespace iLabs.ServiceBroker.admin 
{

    public partial class ClientMetadata : System.Web.UI.Page
    {
        protected BrokerDB brokerDB = new BrokerDB();
        AuthorizationWrapperClass wrapper = new AuthorizationWrapperClass();

        int labClientID;
        string clientGuid;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                hdnMetaId.Value = "";
                initialDropDownList();
                ddlGroups.Items.Clear();
            }
        }
        protected void loadGroupsDDL(int [] groups)
        {
            ddlGroups.Items.Clear();
            foreach(int i in groups){
                string name = AdministrativeAPI.GetGroupName(i);
                ListItem item = new ListItem(name,i.ToString());
                 ddlGroups.Items.Add(item);
            }
        }
        protected void initialDropDownList()
        {
           
            ddlClient.Items.Clear();
            IntTag [] clientTags = brokerDB.GetIntTags("Client_RetrieveTags", null);

            ddlClient.Items.Add(new ListItem(" ------------- select Client ------------ ", "0"));
            if (clientTags != null)
            {
                foreach (IntTag tag in clientTags)
                {
                   ddlClient.Items.Add(new ListItem(tag.tag, tag.id.ToString()));
                }
            }
        }

      
        protected void ddlClient_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblResponse.Visible = false;
            labClientID = Convert.ToInt32(ddlClient.SelectedValue);
            if (labClientID > 0)
            {
                clientGuid = AdministrativeAPI.GetLabClientGUID(labClientID);
                int [] groups = AdministrativeUtilities.GetLabClientGroups(labClientID,true);
                loadGroupsDDL(groups);
                DbConnection connection = FactoryDB.GetConnection();
                DbCommand cmd = FactoryDB.CreateCommand("ClientMetadata_Retrieve", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                // populate parameters
                cmd.Parameters.Add(FactoryDB.CreateParameter("@clientID", labClientID, DbType.Int32));

                try
                {
                    connection.Open();
                    DbDataReader reader = null;
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        if (!reader.IsDBNull(0))
                            hdnMetaId.Value = reader.GetInt32(0).ToString();
                        else
                            hdnMetaId.Value = "";
                        if (!reader.IsDBNull(2))
                            txtCouponID.Text = reader.GetInt64(2).ToString();
                        else
                            txtCouponID.Text = "";
                        if (!reader.IsDBNull(4))
                            txtPasscode.Text = reader.GetString(4);
                        else
                            txtPasscode.Text = "";
                        if (!reader.IsDBNull(5))
                            txtMetadata.Text = reader.GetString(5);
                        else
                            txtMetadata.Text = "";
                        if (!reader.IsDBNull(6))
                            txtScorm.Text = reader.GetString(6);
                        else
                            txtScorm.Text = "";
                        if (!reader.IsDBNull(7))
                            txtFormat.Text = reader.GetString(7);
                        else
                            txtFormat.Text = "";
                    }
                }
                catch (DbException ex)
                {
                    throw ex;
                }
                finally
                {
                    connection.Close();
                }
            }
        }


        protected void btnRegister_Click(object sender, System.EventArgs e)
        {
            lblResponse.Visible = false;
            bool error = false;
            StringBuilder message = new StringBuilder();
            labClientID = Convert.ToInt32(ddlClient.SelectedValue);
            if(labClientID > 0){
                clientGuid = AdministrativeAPI.GetLabClientGUID(labClientID);
            if(txtCouponID.Text == null || txtCouponID.Text.Length == 0)
            {
                Coupon authCoupon = null;
                if(txtPasscode.Text == null || txtPasscode.Text.Length == 0){
                    authCoupon = brokerDB.CreateCoupon();
                }
                else{
                    authCoupon = brokerDB.CreateCoupon(txtPasscode.Text);
                }

                TicketLoadFactory tlf = TicketLoadFactory.Instance();
                if (ddlGroups.SelectedItem != null)
                {
                    string payload = tlf.createAuthorizeClientPayload(clientGuid, ddlGroups.SelectedItem.Text);
                    brokerDB.AddTicket(authCoupon, TicketTypes.AUTHORIZE_CLIENT, ProcessAgentDB.ServiceGuid,
                        ProcessAgentDB.ServiceGuid, -1L, payload);
                    txtCouponID.Text = authCoupon.couponId.ToString();
                    txtIssuer.Text = authCoupon.issuerGuid;
                    txtPasscode.Text = authCoupon.passkey;
                }

            
            }
            DbConnection connection = FactoryDB.GetConnection();
            DbCommand cmd = null;
            if (hdnMetaId.Value.Length == 0)
            {
                cmd = FactoryDB.CreateCommand("ClientMetadata_Insert", connection);
                cmd.CommandType = CommandType.StoredProcedure;
            }
            else
            {
                cmd = FactoryDB.CreateCommand("ClientMetadata_Update", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(FactoryDB.CreateParameter("@clientMetaID", Convert.ToInt32(hdnMetaId.Value), DbType.Int32));
                 
            }
            
            // populate parameters
            cmd.Parameters.Add(FactoryDB.CreateParameter("@clientID", labClientID, DbType.Int32));
            cmd.Parameters.Add(FactoryDB.CreateParameter("@authCouponID", Convert.ToInt64(txtCouponID.Text), DbType.Int64));
            cmd.Parameters.Add(FactoryDB.CreateParameter("@scoGuid", txtPasscode.Text, DbType.AnsiString,50));
            cmd.Parameters.Add(FactoryDB.CreateParameter("@metadata", txtMetadata.Text, DbType.String));
            cmd.Parameters.Add(FactoryDB.CreateParameter("@sco",txtScorm.Text, DbType.String));
            cmd.Parameters.Add(FactoryDB.CreateParameter("@metadataFormat", txtFormat.Text, DbType.String,256));
            try
            {
                connection.Open();
                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    int value = Convert.ToInt32(result);
                    if (hdnMetaId.Value.Length > 0)
                    { //was update
                        message.Append("Update: " + value + " records");
                    }
                    else
                    {
                        message.Append("Inserted new metadata: ID = " + value);
                        hdnMetaId.Value = value.ToString();
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.WriteLine("Error in metadata register: " + ex.Message);
                throw;
            }
            finally
            {
                connection.Close();
            }
            if (error)
            {
                lblResponse.Text=Utilities.FormatErrorMessage(message.ToString());
            }
            else
            {
                lblResponse.Text = Utilities.FormatConfirmationMessage(message.ToString());
            }
            }
            lblResponse.Visible = true;


        }

        protected void btnGuid_Click(object sender, System.EventArgs e)
        {
            Guid guid = System.Guid.NewGuid();
            txtPasscode.Text = Utilities.MakeGuid();
        }

        protected void checkGuid(object sender, ServerValidateEventArgs args)
        {
            if (args.Value.Length > 0 && args.Value.Length <= 50)
                args.IsValid = true;
            else
                args.IsValid = false;
        }

    }
    

}
