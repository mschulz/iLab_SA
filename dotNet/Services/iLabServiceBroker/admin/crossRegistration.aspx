<%@ Page Language="C#" AutoEventWireup="true" CodeFile="crossRegistration.aspx.cs" Inherits="iLabs.ServiceBroker.admin.CrossRegistration" %>
<%@ Register TagPrefix="uc1" TagName="banner" Src="../banner.ascx" %>
<%@ Register TagPrefix="uc1" TagName="adminNav" Src="adminNav.ascx" %>
<%@ Register TagPrefix="uc1" TagName="footer" Src="../footer.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<HTML>
<!-- 
Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
Please see license.txt in top level directory for full license. 
-->
<!-- $Id$ -->
	<HEAD>
		<title>MIT iLab Service Broker - Cross-domain Registration</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
		<style type="text/css">@import url( ../css/main.css ); 
		</style>
		</HEAD>
		<body>
		<form id="Form1" method="post" runat="server">
		<div id="outerwrapper">
				<uc1:banner id="Banner1" runat="server"></uc1:banner>
				<uc1:adminNav id="AdminNav1" runat="server"></uc1:adminNav>
				<br clear="all">
				<div id="innerwrapper">
					<div id="pageintro">
						<h1>Cross-domain Registration
						</h1>
						<p>Exchange Lab client information with a remote ServiceBroker.
						</p>
						<p><asp:label id="lblResponse" Runat="server" Visible="False"></asp:label></p>
					</div>
				        <div id="pagecontent">	
				        <div class="simpleform">
							<table cellSpacing="5" cellPadding="0" border="0">
				        <tr>
				            <th><label for="servicebroker">Service Broker</label></th>
							<td><asp:dropdownlist CssClass="i18n" id="ddlServiceBroker" Runat="server" Width="260px" AutoPostBack="True" onselectedindexchanged="ddlServiceBroker_SelectedIndexChanged"></asp:dropdownlist></td>
				        </tr>
				        <tr>
				            <th><label for="client">Client</label></th>
							<td><asp:dropdownlist CssClass="i18n" id="ddlClient" Runat="server" Width="260px" AutoPostBack="True" onselectedindexchanged="ddlClient_SelectedIndexChanged"></asp:dropdownlist></td>
				       </tr>
				       <tr>
				            <th><label for="lab server">Lab Server</label></th>
							<td><asp:dropdownlist CssClass="i18n" id="ddlLabServer" Runat="server" Width="260px" AutoPostBack="True" onselectedindexchanged="ddlLabServer_SelectedIndexChanged"></asp:dropdownlist></td>
									
				        </tr>
				        <tr>
				            <th><label for="lab side scheduling server">Lab Side Scheduling Server</label></th>
							<td><asp:label id="lblLSS" Runat="server" Width="260px"></asp:label></td>
				        </tr>
				        <tr>
				            <th><label for="email">email</label></th>
				            <td><asp:TextBox ID="txtEmail" runat="server"></asp:TextBox></td>
				        </tr>
				        <tr><td>&nbsp;</td>
                            <td><asp:Button ID="btnRegister" runat="server" Text="Register" CssClass="button" onclick="btnRegister_Click" /></td>
                        </tr>
                       
				        </table>
				        </div>
						</div>
						</div>
		</div>			
		</form>
		</body>
		</HTML>
