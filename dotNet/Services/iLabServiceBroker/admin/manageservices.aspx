<%@ Reference Page="~/admin/messages.aspx" %>
<%@ Page language="c#" Inherits="iLabs.ServiceBroker.admin.manageServices" CodeFile="manageServices.aspx.cs" EnableEventValidation="false" %>
<%@ Register TagPrefix="uc1" TagName="banner" Src="../banner.ascx" %>
<%@ Register TagPrefix="uc1" TagName="adminNav" Src="adminNav.ascx" %>
<%@ Register TagPrefix="uc1" TagName="footer" Src="../footer.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
	<head>
		<title>MIT iLab Service Broker - Manage Services</title> 
		<!-- 
Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
Please see license.txt in top level directory for full license. 
-->
		<!-- $Id$ -->
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
						<h1>Manage Process Agents</h1>
						<p>Add, remove or modify a Service below.</p>
						<p><asp:label id="lblErrorMessage" Runat="server" Visible="False"></asp:label></p>
					</div><!-- end pageintro div -->
					<div id="pagecontent">
							<div class="simpleform">
							    <form id="modifylab" action="" method="post" name="modifylab">
									<table style="WIDTH: 680px; HEIGHT: 460px" cols="3" cellspacing="0" cellpadding="5" border="0">
										<TBODY>
										    <tr style="height: 1px">
										        <th style="width: 120px"></th><td style="width: 440px"></td><td style="width: 120px"></td>
										    </tr>
										    <tr><th style="width: 120px"><label for="registerBatch">Register BatchLS</label><asp:checkbox ID="cbxDoBatch" runat="server" AutoPostBack="True" OnCheckedChanged="cbxDoBatch_Changed" /></th>
											<td colspan="2" style="width: 560px"><asp:textbox ID="txtBatchHelp" runat="server" Columns="70" Rows="4" TextMode="MultiLine" Visible="false" Width="560px">Note: Batch LabServers require that the LabServer GUID be less than 36 characters,  the incoming passcode be specified by the ServiceBroker provider, and the outgoing passcode is specified by the LabServer provider.</asp:textbox></td>
										   </tr>
											<tr>
												<th style="width: 120px">
													<label for="Service">Service</label></th>
												<td colspan="2" style="width: 560px;"><asp:dropdownlist CssClass="i18n" id="ddlService" Runat="server" AutoPostBack="True" Width="560px" onselectedindexchanged="ddlService_SelectedIndexChanged"></asp:dropdownlist></td>
											</tr>
											
											<tr>
												<th style="width: 120px">
													<label for="webserviceurl">Web Service URL</label></th>
												<td colspan="2" style="width: 560px"><asp:textbox id="txtWebServiceURL" Runat="server" Width="560px"></asp:textbox></td>
											</tr>
											<tr id="trPasskey" runat="server">
												<th style="width: 120px">
													<label for="outpasskey">Initial Passkey </label>
												</th>
												<td colspan="2" style="width: 560px"><asp:textbox id="txtOutPassKey" Runat="server" Width="560px"></asp:textbox></td>
											</tr>
											<tr>
												<th style="width: 120px">
													<label for="codebaseurl">Codebase URL </label>
												</th>
												<td colspan="2" style="width: 560px"><asp:textbox id="txtApplicationURL" Runat="server" Width="560px"></asp:textbox></td>
											</tr>
											<tr>
												<th style="width: 120px">
													<label for="Servicename">Service Name </label>
												</th>
												<td colspan="2" style="width: 560px"><asp:textbox id="txtServiceName" Runat="server" Width="560px"></asp:textbox></td>
											</tr>
											<tr>
												<th style="width: 120px">
													<label for="AgentType">Agent Type </label>
												</th>
												<td colspan="2" style="width: 560px"><asp:textbox id="txtServiceType" Runat="server" Width="560px"></asp:textbox></td>
											</tr>
											<tr>
												<th style="width: 120px">
													<label for="Agentguid">Agent GUID </label>
												</th>
												<td colspan="2" style="width: 560px"><asp:textbox id="txtServiceGUID" Runat="server" Width="560px"></asp:textbox></td>
											</tr>
											<tr id="trBatchIn" runat="server" visible="false">
											<th style="width: 120px">
													<label for="Passcode In">Incoming Passcode From LabServer</label>
												</th>
												<td colspan="2" style="width: 560px"><asp:textbox id="txtBatchPassIn" Runat="server" Width="560px"
												Tooltip="This is the code generated on the ServiceBroker."></asp:textbox></td>
											</tr>
											<tr id="trBatchOut" runat="server" visible="false">
											<th style="width: 120px">
													<label for="Passcode Out">Outgoing Passcode To LabServer</label>
												</th>
												<td colspan="2" style="width: 560px"><asp:textbox id="txtBatchPassOut" Runat="server" Width="560px"
												Tooltip="This is the code generated by the Lab Server."></asp:textbox></td>
											</tr>
											<tr>
												<th style="width: 120px">
													<label for="domainServer">Domain Server </label>
												</th>
												<td colspan="2" style="width: 560px"><asp:textbox id="txtDomainServer" Runat="server" Width="560px"></asp:textbox></td>
											</tr>
											<tr>
												<th style="width: 120px">
													<label for="description">Description</label>
												</th>
												<td colspan="2" style="width: 560px"><asp:textbox id="txtServiceDescription" Runat="server" Columns="20" Rows="5" TextMode="MultiLine" Width="560px"></asp:textbox></td>
											</tr>
											<tr>
												<th style="width: 120px">
													<label for="infourl">Info URL</label>
												</th>
												<td colspan="2" style="width: 560px"><asp:textbox id="txtInfoURL" Runat="server" Width="560px"></asp:textbox></td>
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
											</tr>
											<tr id="trAdminGroup" runat="server">
												<th style="width: 120px">
													<label id="lblAdminGroup" runat="server" for="adminGroup">Admin Group</label>
												</th>
												<td colspan="2" style="width: 560px">
                                                    <asp:DropDownList CssClass="i18n" ID="ddlAdminGroup" runat="server" Width="560px"></asp:DropDownList>
                                                </td>
											</tr>
											<tr id="trAssociate" runat="server">
												<th style="width: 120px">
													<label id="lblAssociate" runat="server"  for="associatedLSS">Associated LSS</label>
                                                </th>
												<td style="width: 440px; height: 1px;">
                                                    <asp:DropDownList CssClass="i18n" ID="ddlLSS" runat="server" Width="440px"></asp:DropDownList>
                                                    <asp:TextBox CssClass="i18n" ID="txtLSS" runat="server" Width="440px"/>
                                                </td>
                                                <td rowspan="2" style="width: 120px" id="tdBtnAssociateLSS" runat="server">
                                                    <asp:Button ID="btnAssociateLSS" CssClass="button"  runat="server" Text="Associate" OnClick="btnAssociateLSS_Click"></asp:Button>
                                                </td>
											</tr>
											<tr id="trManage" runat="server">
												<th style="width: 120px">
													<label id="lblManage" runat="server"  for="ManageLSS">Manage on LSS</label>
                                                </th>
												<td style="width: 440px; height: 1px;">
                                                    <asp:DropDownList CssClass="i18n" ID="ddlManageLSS" runat="server" Width="440px"></asp:DropDownList>
                                                    <asp:TextBox CssClass="i18n" ID="txtManageLSS" runat="server" Width="440px"/>
                                                </td>
											</tr>                                            
											<tr align="right">
												<th colspan="3" style="width: 680px; height: 34px">
												    <asp:button id="btnRegister" Runat="server" CssClass="button" Text="Install Domain Credentials" onclick="btnRegister_Click"></asp:button>
													<asp:button id="btnSaveChanges" Runat="server" CssClass="button" Text="Save Changes" onclick="btnSaveChanges_Click"></asp:button>
													<asp:button ID="btnAdminURLs" runat="server" CssClass="button"  Text="Domain URLs" Width="140px"></asp:button>
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
