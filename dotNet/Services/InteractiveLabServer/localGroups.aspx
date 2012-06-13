<%@ Page language="c#" Inherits="iLabs.LabServer.LabView.localGroups" CodeFile="localGroups.aspx.cs" %>

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
						<h1><asp:label id="lblTitle" Runat="server"></asp:label></h1>
						<asp:label id="lblDescription" Runat="server"></asp:Label>
					</div>
					<!-- end pageintro div -->
					<div id="pagecontent">
						<p><asp:HyperLink id="lnkBackSB" Text="Back to InteractiveSB" runat="server" ></asp:HyperLink></p>
						<!-- Content goes here -->
						<div class="simpleform">
						    <form id="localGroupInfo" action="" method="post" name="localGroupInfo">
									<table style="WIDTH: 564px; HEIGHT: 460px" cellSpacing="0" cellPadding="5" border="0">
										<TBODY>
											
											<tr>
												<th style="width: 480px">
													<label for="groups">Local Groups</label></th>
												<td style="width: 484px"><asp:dropdownlist cssClass="i18n" id="ddlGroup" Runat="server" AutoPostBack="True" Width="360px" onselectedindexchanged="ddlGroups_SelectedIndexChanged"></asp:dropdownlist></td>
											</tr>
											<tr>
												<th style="width: 480px">
													<label for="description">Description</label></th>
												<td style="width: 484px"><asp:textbox id="txtServiceDescription" Runat="server" Columns="20" Rows="5" TextMode="MultiLine"
														Width="360px"></asp:textbox></td>
											</tr>
											<tr>
												<th style="width: 480px">
													<label for="Comment">Comment</label></th>
												<td style="width: 484px"><asp:textbox id="txtComment" Runat="server" Width="360px"></asp:textbox><br>
												</td>
											</tr>
											<tr>
                                                <th colspan="2">
                                                    <asp:Button ID="btnRsrcMappings" runat="server" CssClass="button"  Text="Resource Mappings" Width="140"/></th>
                                            </tr>
                                            <tr>
                                                <th colspan="2">
                                                    <asp:Button ID="btnAdminURLs" runat="server" CssClass="button"  Text="Admin URLs" Width="140px" /></th>
                                            </tr>
											<tr>
												<th colSpan="2">
													<label for="register">Install Service &nbsp;
                                                    </label>
                                                    &nbsp;<asp:button id="btnRegister" Runat="server" CssClass="button" Text="Register" onclick="btnRegister_Click"></asp:button></th></tr>
											<tr>
												<th colSpan="2">
													<asp:button id="btnSaveChanges" Runat="server" CssClass="button" Text="Save Changes" onclick="btnSaveChanges_Click"></asp:button><asp:button id="btnRemove" Runat="server" CssClass="button" Text="Remove" onclick="btnRemove_Click"></asp:button><asp:button id="btnNew" Runat="server" CssClass="button" Text="Clear" onclick="btnNew_Click"></asp:button></th></tr>
										</TBODY>
									</table>
									
								</form>
								</div>
						<div id="messagebox-right">
							<h3><asp:label id="lblGroupNameSystemMessage" Runat="server"></asp:label></h3>
							<asp:repeater id="repSystemMessage" runat="server">
								<ItemTemplate>
									<p class="message">
										<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "MessageBody")) %>
									</p>
									<p class="date">Date Posted:
										<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "LastModified")) %>
									</p>
								</ItemTemplate>
							</asp:repeater>
						</div> <!-- End messagebox-right -->
					</div>
					<br clear="all">
					<!-- end pagecontent div --></div>
				<!-- end innerwrapper div --><uc1:footer id="Footer1" runat="server"></uc1:footer></div>
		</form>
	</body>
</HTML>
