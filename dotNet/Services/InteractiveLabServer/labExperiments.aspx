<%@ Page language="c#" Inherits="iLabs.LabServer.LabView.labExperiments" CodeFile="labExperiments.aspx.cs" EnableEventValidation="false" %>
<%@ Register TagPrefix="uc1" TagName="banner" Src="banner.ascx" %>
<%@ Register TagPrefix="uc1" TagName="userNav" Src="userNav.ascx" %>
<%@ Register TagPrefix="uc1" TagName="footer" Src="footer.ascx" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>RunLab</title> 
		<!-- 
Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
Please see license.txt in top level directory for full license. 
-->
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( css/main.css );
		</style>
	</HEAD>
	<body>
		<form method="post" runat="server">
			<a name="top"></a>
			<div id="outerwrapper"><uc1:banner id="Banner1" runat="server"></uc1:banner>
			<uc1:userNav id="UserNav1" runat="server"></uc1:userNav><br clear="all">
				<div id="innerwrapper">
					<div id="pageintro">
						<h1><asp:label id="lblTitle" Runat="server">Configure Lab Experiments</asp:label></h1>
						<asp:label id="lblDescription" Runat="server"></asp:label>
						<p><asp:label id="lblErrorMessage" Runat="server" Visible="False"></asp:label></p>
					    <div id="Div1"  runat="server">
					        <asp:CustomValidator ID="valGuid" ControlToValidate="txtClientGuid" OnServerValidate="checkGuid" 
                            Text="A Guid must be unique and no longer than 50 characters" runat="server"/>
					    </div>
					</div>
					<!-- end pageintro div -->
					<div id="pagecontent">						
						<!-- Content goes here -->
						<p><asp:HyperLink id="lnkBackSB" Text="Back to InteractiveSB" runat="server" ></asp:HyperLink></p>	
						<div class="simpleform">
						    <form id="appInfo" action="" method="post" name="appInfo">
									<table  cellSpacing="0" cellPadding="5" border="0">
										<TBODY>
											<tr>
												<th style="width: 140px">
													<label for="appName">Lab Application</label></th>
												<td style="width: 484px"><asp:dropdownlist cssClass="i18n" id="ddlApplications" Runat="server" Width="480px" onselectedindexchanged="ddlApplications_SelectedIndexChanged" AutoPostBack="True"></asp:dropdownlist></td>
											</tr>
											<tr>
												<th >
													<label for="title">Title</label></th>
												<td><asp:textbox id="txtTitle" Runat="server" Width="480px"></asp:textbox></td>
											</tr>
											<tr>
									            <th>
										            <label for="ClientGuid">Client GUID</label></th>
									            <td><asp:textbox id="txtClientGuid" Runat="server"  Width="380px"
									            ToolTip="You must enter a GUID and it must match the client GUID on the ServiceBroker"></asp:textbox>&nbsp;&nbsp;&nbsp;<asp:Button ID="btnGuid" runat="server" Text="Make Guid" OnClick="btnGuid_Click" /></td>
								            </tr>
								            <tr runat="server">
												<th>
													<label for="version">Version</label></th>
												<td><asp:textbox id="txtVersion" Runat="server" Width="480px"
												ToolTip="You must enter a version string" ></asp:textbox></td>
											</tr>
											<tr id="Tr1" runat="server">
												<th>
													<label for="version">Revision</label></th>
												<td><asp:textbox id="txtRev" Runat="server" Width="480px"
												 ToolTip="You may specify the LabVIEW release in this field. Legal values are 8.2, 8.6, 2009 or 2010"></asp:textbox></td>
											</tr><tr runat="server">
												<th>
													<label for="applicationKey">Application Key</label></th>
												<td><asp:textbox id="txtAppKey" Runat="server" Width="480px"
												ToolTip="The app value used in the loader script to select this application"></asp:textbox></td>
											</tr>
											<tr>
												<th>
													<label for="applicationPath">Path</label></th>
												<td><asp:textbox id="txtApplicationPath" Runat="server" Width="480px" 
												ToolTip="The absolute path to the application directory, without a trailing seperator"></asp:textbox></td>
											</tr>
											<tr>
												<th>
													<label for="application">Application</label></th>
												<td><asp:textbox id="txtApplication" Runat="server" Width="480px"
												ToolTip="The application Application file name including file type extension"></asp:textbox></td>
											</tr>
											<tr>
												<th>
													<label for="webPageUrl">Web Page URL</label></th>
												<td><asp:textbox id="txtPageUrl" Runat="server" Width="480px"
												ToolTip="The URL used to display the client"></asp:textbox></td>
											</tr>
											<tr>
												<th>
													<label for="width">Application URL</label></th>
												<td><asp:textbox id="txtURL" Runat="server" Width="480px"
												ToolTip="The URL used execute the client, for LabVIEW it is the LabVIEW Web Server"></asp:textbox></td>
											</tr>
											<tr>
												<th>
													<label for="width">Width</label></th>
												<td><asp:textbox id="txtWidth" Runat="server" Width="480px"></asp:textbox></td>
											</tr>
											<tr>
												<th>
													<label for="height">Height</label></th>
												<td><asp:textbox id="txtHeigth" Runat="server" Width="480px"></asp:textbox></td>
											</tr>
											<tr>
												<th>
													<label for="dataSources">Data Sources </label></th>
												<td><asp:textbox id="txtDataSources" Runat="server" Width="480px"
												ToolTip="A comma delimited list of dataSource URLs with optional record type"></asp:textbox></td>
											</tr>		
											<tr>
												<th>
													<label for="server">Server</label></th>
												<td><asp:textbox id="txtServer" Runat="server" Width="480px"></asp:textbox></td>
											</tr>
											<tr>
												<th>
													<label for="port">Port</label></th>
												<td><asp:textbox id="txtPort" Runat="server" Width="48px"></asp:textbox></td>
											</tr>
											<tr>
												<th>
													<label for="contactemail">Contact Email </label></th>
												<td><asp:textbox id="txtContactEmail" Runat="server" Width="480px"></asp:textbox></td>
											</tr>
											<tr>
												<th>
													<label for="description">Description</label></th>
												<td><asp:textbox id="txtDescription" Runat="server" Width="480px" Rows="5" TextMode="MultiLine"></asp:textbox></td>
											</tr>
											<tr>
												<th>
													<label for="infoUrl">Info URL </label></th>
												<td><asp:textbox id="txtInfoUrl" Runat="server" Width="480px"
												ToolTip="An optional URL to information about the lab"></asp:textbox></td>
											</tr>
											<tr>
												<th>
													<label for="comment">Comment</label></th>
												<td><asp:textbox id="txtComment" Runat="server" Width="480px"></asp:textbox></td>
											</tr> 
											<tr>
												<th>
													<label for="port">Extra Data</label></th>
												<td><asp:textbox id="txtExtra" Runat="server" Width="480px"></asp:textbox></td>
											</tr>
											<tr>
												<th colSpan="2">
												    <asp:button id="btnSaveChanges" Runat="server" CssClass="button" Text="Save Changes" onclick="btnSaveChanges_Click"></asp:button>
                                                    &nbsp;&nbsp;<asp:button id="btnNew" Runat="server" CssClass="button" Text="Clear" onclick="btnNew_Click"></asp:button>
                                                     &nbsp;&nbsp;<asp:button id="btnDelete" Runat="server" CssClass="button" Text="Delete" onclick="btnDelete_Click"></asp:button>
                                                 </th>
                                            </tr>
											<tr>
												<th colSpan="2">
													</th></tr>
										</TBODY>
									</table>
								</form>
							</div>
					</div>
					<br clear="all">
					<!-- end pagecontent div --></div>
				<!-- end innerwrapper div --><uc1:footer id="Footer1" runat="server"></uc1:footer></div>
		</form>
	</body>
</HTML>
