<%@ Reference Page="~/admin/messages.aspx" %>
<%@ Page language="c#" Inherits="iLabs.ServiceBroker.admin.authorities" CodeFile="authorities.aspx.cs" EnableEventValidation="false" %>
<%@ Register TagPrefix="uc1" TagName="banner" Src="../banner.ascx" %>
<%@ Register TagPrefix="uc1" TagName="adminNav" Src="adminNav.ascx" %>
<%@ Register TagPrefix="uc1" TagName="footer" Src="../footer.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
	<head>
		<title>MIT iLab Service Broker - Manage Authentication Authorities</title> 
		<!-- 
Copyright (c) 2011 The Massachusetts Institute of Technology. All rights reserved.
Please see license.txt in top level directory for full license. 
-->
		<!-- $Id:$ -->
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
		<meta content="C#" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
		<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1"/>
		<style type="text/css">@import url( ../css/main.css ); 
		</style>
	</head>
	<body>
		<form id="Form1" method="post" runat="server">
			<a name="top"></a>
			<div id="outerwrapper">
				<uc1:banner id="Banner1" runat="server"></uc1:banner>
				<uc1:adminNav id="AdminNav1" runat="server"></uc1:adminNav>
				<br clear="all"/>
				<div id="innerwrapper">
					<div id="pageintro">
						<h1>Manage Authentication Authorities</h1>
						<p>Add, remove or modify an authentication authority.</p>
						<p><asp:label id="lblErrorMessage" Runat="server" Visible="False"></asp:label></p>
					</div><!-- end pageintro div -->
					<div id="pagecontent">
							<div class="simpleform">
							    <form id="authorities" action="" method="post" name="authorities">
									<table style="WIDTH: 680px; HEIGHT: 460px" cols="3" cellspacing="0" cellpadding="5" border="0">
										<TBODY>
										    <tr style="height: 1px">
										        <th style="width: 120px"></th><td style="width: 440px"></td><td style="width: 120px"></td>
										    </tr>
											<tr>
												<th style="width: 120px">
													<label for="Authorities">Authority </label></th>
												<td colspan="2" style="width: 560px;"><asp:dropdownlist CssClass="i18n" id="ddlAuthorities" Runat="server" AutoPostBack="True" Width="560px" onselectedindexchanged="ddlAuthorities_SelectedIndexChanged"></asp:dropdownlist></td>
											</tr>
											<tr>
												<th style="width: 120px">
													<label for="Servicename">Service Name </label>
												</th>
												<td colspan="2" style="width: 560px"><asp:textbox id="txtServiceName" Runat="server" Width="560px"></asp:textbox></td>
											</tr>
											<tr>
												<th style="width: 120px">
													<label for="serviceurl">Service URL</label></th>
												<td colspan="2" style="width: 560px"><asp:textbox id="txtServiceURL" Runat="server" Width="560px"></asp:textbox></td>
											</tr>
											<tr>
												<th style="width: 120px">
													<label for="serviceuguid">Service GUID</label></th>
												<td colspan="2" style="width: 560px"><asp:textbox id="txtServiceGuid" Runat="server" Width="560px"></asp:textbox></td>
											</tr><tr>
												<th style="width: 120px">
													<label for="description">Description</label>
												</th>
												<td colspan="2" style="width: 560px"><asp:textbox id="txtServiceDescription" Runat="server" Columns="20" Rows="5" TextMode="MultiLine" Width="560px"></asp:textbox></td>
											</tr>
											<tr>
												<th style="width: 120px">
													<label for="group">Initial Group</label>
												</th>
												<td colspan="2" style="width: 560px"><asp:dropdownlist id="ddlGroup" Runat="server" Width="560px"></asp:dropdownlist></td>
											</tr>
											<tr>
												<th style="width: 120px">
													<label for="emailproxy">Email Proxy</label>
												</th>
												<td colspan="2" style="width: 560px"><asp:textbox id="txtEmailProxy" Runat="server" Width="560px"></asp:textbox></td>
											</tr>
											<tr>
												<th style="width: 120px">
													<label for="contactemail">Contact Email</label>
												</th>
												<td colspan="2" style="width: 560px"><asp:textbox id="txtContactEmail" Runat="server" Width="560px"></asp:textbox></td>
											</tr>
											<tr>
												<th style="width: 120px">
													<label for="bugemail">Bug Report Email</label>
												</th>
												<td colspan="2" style="width: 560px"><asp:textbox id="txtBugEmail" Runat="server" Width="560px"></asp:textbox></td>
											</tr>
											<tr>
												<th style="width: 120px">
													<label for="location">Location</label>
												</th>
												<td colspan="2" style="width: 560px"><asp:textbox id="txtLocation" Runat="server" Width="560px"></asp:textbox></td>
											</tr><tr>
												<th style="width: 120px">
													<label for=">AuthenticationProtocol">Authentication Protocol </label>
												</th>
												<td colspan="2" style="width: 560px"><asp:dropdownlist id="ddlAuthProtocol" Runat="server" Width="560px"></asp:dropdownlist></td>
											</tr>
											<tr id="trPassphrase" runat="server">
												<th style="width: 120px">
													<label for="passphrase">Pass Phrase</label>
												</th>
												<td colspan="2" style="width: 560px"><asp:textbox id="txtPassPhrase" Runat="server" Width="560px"></asp:textbox></td>
											</tr>                               
											<tr align="right">
												<th colspan="3" style="width: 680px; height: 34px">
												    <asp:button id="btnSaveChanges" Runat="server" CssClass="button" Text="Save Changes" onclick="btnSave_Click"></asp:button>
												    <asp:button id="btnRegister" Runat="server" CssClass="button" Text="Register" onclick="btnRemove_Click"></asp:button>
													<asp:button id="btnNew" runat="server" CssClass="button" Text="New" onclick="btnNew_Click"></asp:button>
												</th>
											</tr>
       									</TBODY>
									</table>
								</form>
							</div>
						</div><!-- end pagecontent div -->
		            </div><!-- end innerwrapper div -->
		            <br clear="all" />
		            <uc1:footer id="Footer1" runat="server"></uc1:footer>
				</div>
		</form>		
	</body>
</html>
