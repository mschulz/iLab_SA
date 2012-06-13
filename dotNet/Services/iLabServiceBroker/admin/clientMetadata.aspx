<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ClientMetadata.aspx.cs" Inherits="iLabs.ServiceBroker.admin.ClientMetadata" %>

<%@ Register TagPrefix="uc1" TagName="banner" Src="../banner.ascx" %>
<%@ Register TagPrefix="uc1" TagName="adminNav" Src="adminNav.ascx" %>
<%@ Register TagPrefix="uc1" TagName="footer" Src="../footer.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<!-- 
Copyright (c) 2011 The Massachusetts Institute of Technology. All rights reserved.
Please see license.txt in top level directory for full license. 
-->
<!-- $Id$ -->
<head>
    <title>MIT iLab Service Broker - Client Metadata</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
    <meta content="C#" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
    <meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
    <style type="text/css">@import url( ../css/main.css ); 
		</style>
</head>
<body>
    <form id="Form1" method="post" runat="server">
        <asp:HiddenField ID="hdnMetaId" runat="server" />
        <div id="outerwrapper">
            <uc1:banner ID="Banner1" runat="server"></uc1:banner>
            <br clear="all">
            <div id="innerwrapper">
                <div id="pageintro">
                    <h1>
                        Client Metadata
                    </h1>
                    <p>
                        Create, modify and publish client metadata. Note: This page is not integrated into the ServiceBroker and currently may only be used to create a ClientAuthorization Ticket.
                    </p>
                    <p>
                        <asp:Label ID="lblResponse" runat="server" Visible="False"></asp:Label></p>
                </div>
                <div id="pagecontent">
                    <div class="simpleform">
                        <table cellspacing="5" cellpadding="0" border="0" style="width: 793px">
                            <tr>
                                <th><label for="client">Client </label></th>
                                <td colspan="2"><asp:DropDownList CssClass="i18n" ID="ddlClient" runat="server"  AutoPostBack="True"
                                        OnSelectedIndexChanged="ddlClient_SelectedIndexChanged" Width="672px"></asp:DropDownList></td>
                            </tr>
                            <tr>
                                <th><label for="groups">Client Groups</label></th>
                                <td colspan="2"><asp:DropDownList CssClass="i18n" ID="ddlGroups" runat="server"  AutoPostBack="True"
                                        Width="672px"></asp:DropDownList></td>
                            </tr>
                            <tr>
                                <th rowspan="2"><label for="client">Client Authorization </label></th>
                                <td style="width: 336px"><asp:Textbox CssClass="i18n" ID="txtCouponID" runat="server" Width="200px"></asp:Textbox></td>
                                <td style="width: 180px"><asp:Textbox CssClass="i18n" ID="txtIssuer" runat="server" Columns="50"></asp:Textbox></td>
                            </tr>
                            <tr>
                                <td style="width: 336px"><asp:Textbox CssClass="i18n" ID="txtPasscode" runat="server" Columns="50"></asp:Textbox>&nbsp;&nbsp;&nbsp;</td><td><asp:Button ID="btnGuid" runat="server" CssClass="button" Text="Make Guid" OnClick="btnGuid_Click" /></td>
                            </tr>
                            <tr>
                                <th><label for="client">Metadata </label></th>
                                <td colspan="2"><asp:Textbox CssClass="i18n" ID="txtMetadata" runat="server" Width="667px" Rows="8" TextMode="MultiLine"></asp:Textbox></td>
                            </tr>
                            <tr>
                                <th><label for="client">Client SCORM </label></th>
                                <td colspan="2"><asp:Textbox CssClass="i18n" ID="txtScorm" runat="server" Width="667px" Rows="8" TextMode="MultiLine"></asp:Textbox></td>
                            </tr>
                            <tr>
                                <th><label for="client">Metadata Format </label></th>
                                <td colspan="2"><asp:Textbox CssClass="i18n" ID="txtFormat" runat="server" Width="667px" Rows="3" TextMode="MultiLine"></asp:Textbox></td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td colspan="2"><asp:Button ID="btnRegister" runat="server" Text="Register" CssClass="button" OnClick="btnRegister_Click" /></td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
            <uc1:footer ID="Footer1" runat="server"></uc1:footer>
        </div>
    </form>
</body>
</html>
