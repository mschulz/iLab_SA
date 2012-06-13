<%@ Page language="c#" Inherits="iLabs.ServiceBroker.admin.assocLabServersPopup" CodeFile="assocLabServersPopup.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN"
"http://www.w3.org/TR/html4/loose.dtd">
<HTML>
	<HEAD>
		<title>Associated Lab Servers</title> 
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
		<link href="../css/main.css" rel="stylesheet" type="text/css">
		<link href="../css/popup.css" rel="stylesheet" type="text/css">
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<div id="wrapper">
				<div id="pageintro">
					<h1>Associated Lab Servers for Lab Client
						<br>
						<asp:Label ID="lblLabClient" Runat="server"></asp:Label>
					</h1>
					<p><b>This page is currently not being used as a client may only be associated with one LabServer in the current iLab Achitecture.</b></p>
					<p>Add, remove, or reorder associated lab servers below.</p>
					<div id="divErrorMessage" runat="server" class="errormessage"><p><asp:label id="lblResponse" Runat="server"></asp:label></p>
					</div>
				</div>
				<div id="pagecontent">
					<div class="simpleform">
						<p>Note: Lab servers associated on this page will automatically give "useLabServer" grants for any user or group that has a "useLabClient" grant for this lab client.</p>
						<table cellspacing="0" cellpadding="0" border="0">
							<tr>
								<th class="top" style="WIDTH: 98px">
									<label for="available">Available Lab Servers</label></th>
								<th style="WIDTH: 91px">
									&nbsp;</th>
								<th class="top">
									<label for="associated">Associated Lab Servers</label></th>
								<th class="top" style="WIDTH: 71px">
									Reorder</th></tr>
							<tr>
								<td><asp:listbox cssClass="i18n" id="lbxAvailable" Height="90px" Width="300px" Runat="server"></asp:listbox></td>
								<td class="buttonstyle" style="WIDTH: 91px"><asp:imagebutton id="ibtnAdd" Height="22" Width="50" Runat="server" ImageUrl="../img/add-btn.gif"
										CssClass="buttonstyle" AlternateText="Add"></asp:imagebutton><br>
									<asp:imagebutton id="ibtnRemove" Height="22" Width="74" Runat="server" ImageUrl="../img/remove-btn.gif"
										CssClass="buttonstyle" AlternateText="Remove"></asp:imagebutton></td>
								<td><asp:listbox cssClass="i18n" id="lbxAssociated" Height="90px" Width="300px" Runat="server"></asp:listbox></td>
								<td class="buttonstyle" style="WIDTH: 71px"><asp:imagebutton id="ibtnMoveUp" Height="22" Width="43" Runat="server" ImageUrl="../img/up-btn.gif"
										CssClass="buttonstyle" AlternateText="Move Up"></asp:imagebutton><br>
									<asp:imagebutton id="ibtnMoveDown" Height="22" Width="57" Runat="server" ImageUrl="../img/down-btn.gif"
										CssClass="buttonstyle" AlternateText="Move Down"></asp:imagebutton></td>
							</tr>
							<tr>
								<th style="WIDTH: 539px" colSpan="4">
									<asp:button id="btnSaveChanges" Runat="server" CssClass="buttonright" Text="Save Order Changes" onclick="btnSaveChanges_Click"></asp:button>
								</th>
							</tr>
						</table>
					</div>
				</div>
				<!-- End pagecontent div --><br clear="all"/>
			</div>
		</form>
	</body>
</HTML>
