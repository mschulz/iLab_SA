<%@ Reference Page="~/admin/grants.aspx" %>
<%@ Reference Page="~/requestGroup.aspx" %>
<%@ Page language="c#" Inherits="iLabs.ServiceBroker.admin.addEditServAdminGroupPopup" CodeFile="addEditServAdminGroupPopup.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" "http://www.w3.org/TR/html4/loose.dtd">
<HTML>
	<HEAD>
		<title>
			<%= actionCmd %>
			Service Admin Group</title> 
		<!-- 
Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
Please see license.txt in top level directory for full license. 
-->
		<!-- $Id$ -->
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
		<LINK href="../css/main.css" type="text/css" rel="stylesheet">
		<LINK href="../css/popup.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<div id="wrapper">
				<div id="pageintro">
					<h1><%= actionCmd %>
                        Service Administration Group</h1>
					<asp:label id="lblResponse" EnableViewState="False" Visible="False" Runat="server"></asp:label></div>
				<div id="pagecontent">
					<div class="simpleform">
						<table cellSpacing="0" cellPadding="0" border="0">
							<tr>
								<th style="WIDTH: 146px">
									<label for="name">Group Name&nbsp;&nbsp;</label></th>
								<td style="WIDTH: 544px"><asp:textbox id="txtName" Runat="server" Width="408px"></asp:textbox></td>
							</tr>
							<tr>
								<th style="WIDTH: 146px">
									<label for="description">Description&nbsp;&nbsp;</label></th>
								<td style="WIDTH: 544px"><asp:textbox id="txtDescription" Runat="server" Width="408px" Font-Size="12px" Font-Names="arial; helvetica"
										Rows="4" Columns="30" TextMode="MultiLine"></asp:textbox></td>
							</tr>
							<tr>
								<th style="WIDTH: 146px">
									<label for="email">Admin Contact Email&nbsp;&nbsp;</label></th>
								<td style="WIDTH: 544px"><asp:textbox id="txtEmail" Runat="server" Width="408px" Columns="50"></asp:textbox></td>
							</tr>
							<tr>
								<th style="WIDTH: 146px">
									<label>Need Request Group?&nbsp;&nbsp;</label></th>
								<td style="WIDTH: 544px"><asp:checkbox id="cbxRequestgroup" Runat="server"></asp:checkbox><asp:textbox id="txtRequestgroup" Runat="server" Width="352px"></asp:textbox></td>
							</tr>
						</table>
						<h4>Direct Group Membership</h4>
						<table style="WIDTH: 640px" cellSpacing="0" cellPadding="0" border="0">
							<tr>
								<th class="top" style="WIDTH: 307px">
									<label for="allParents">Available Groups</label></th>
								<th>
									&nbsp;</th>
								<th class="top" style="WIDTH: 306px">
									<label for="parentGroups">Parent Groups </label>
								</th>
							</tr>
							<tr>
								<td style="WIDTH: 307px"><asp:listbox cssClass="i18n" id="lbxAllParents" Runat="server" Width="280px" Rows="6" Columns="30" TextMode="MultiLine"></asp:listbox></td>
								<td class="buttonstyle"><asp:imagebutton id="ibtnAddParent" Runat="server" Width="50" ImageUrl="../img/add-btn.gif" CssClass="buttonstyle"
										Height="22"></asp:imagebutton><br>
									<asp:imagebutton id="ibtnRemoveParent" Runat="server" Width="74" ImageUrl="../img/remove-btn.gif"
										CssClass="buttonstyle" Height="22"></asp:imagebutton></td>
								<td style="WIDTH: 306px"><asp:listbox cssClass="i18n" id="lbxParentGroups" Runat="server" Width="288px" Rows="6" Columns="30" TextMode="MultiLine"></asp:listbox>
								<td style="WIDTH: 1px"></td>
							</tr>
						</table>
						<br>
						<table style="WIDTH: 640px" cellSpacing="0" cellPadding="0" border="0">
							<tr>
								<th style="WIDTH: 635px" colSpan="3">
									<asp:button id="btnSaveChanges" Runat="server" Width="133px" CssClass="buttonright" Text="Save Changes"></asp:button></th></tr>
							</table>
					</div>
				</div>
				<!-- End pagecontent div --><br clear="all">
			</div>
		</form>
	</body>
</HTML>
