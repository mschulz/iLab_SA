<%@ Reference Page="~/admin/grants.aspx" %>
<%@ Reference Page="~/requestGroup.aspx" %>
<%@ Page language="c#" Inherits="iLabs.ServiceBroker.admin.addEditGroupPopup" CodeFile="addEditGroupPopup.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" "http://www.w3.org/TR/html4/loose.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
	<head>
		<title>
			<%= actionCmd %>
			Group</title> 
		<!-- 
Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
Please see license.txt in top level directory for full license. 
-->
		<!-- $Id$ -->
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
		<meta content="C#" name="CODE_LANGUAGE" />
		<meta content="JavaScript" name="vs_defaultClientScript" />
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
		<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
		<link href="../css/main.css" type="text/css" rel="stylesheet" />
		<link href="../css/popup.css" type="text/css" rel="stylesheet" />
	</head>
	<body>
		<form id="Form1" method="post" runat="server">
			<div id="wrapper">
				<div id="pageintro">
					<h1><%= actionCmd %>
						Group</h1>
					<asp:label id="lblResponse" EnableViewState="False" Visible="False" Runat="server"></asp:label></div>
				<div id="pagecontent">
					<div class="simpleform">
						<table cellspacing="0" cellpadding="0" border="0">
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
							<tr>
								<th style="WIDTH: 146px">
									<label>Need Course Staff Group?&nbsp;&nbsp;</label></th>
								<td style="WIDTH: 544px"><asp:checkbox id="cbxTAGroup" Runat="server"></asp:checkbox><asp:textbox id="txtTAGroup" Runat="server" Width="352px"></asp:textbox></td>
							</tr>
						</table>
						<h4>Experiment&nbsp;Grants</h4>
						<%if (showExpGrants) {%>
						<p>(Specify "Read Experiment" (R), "Write Experiment" (W) or "Conduct Experiments" (C) grants for the group 
							and its subgroups)</p>
						<%} else {%>
						<p>Set "Read", "Write" or Create Experiment grants for this group or subgroups . To add these 
							grants, save all changes on this page and return to it by clicking on the Edit 
							Button for the group.</p>
						<%}%>
						<table style="WIDTH: 640px" cellspacing="0" cellpadding="0" border="0">
							<asp:repeater id="repExperiments" Runat="server">
								<ItemTemplate>
									<tr>
										<th style="WIDTH: 146px">
											<asp:Label ID="lblExp" Runat="server" Font-Size="12px"></asp:Label>&nbsp;</th>
										<td><div class="checkset">
												<asp:CheckBox ID="cbxRead" Enabled="false" Runat="server"></asp:CheckBox><label for="cbxRead">R</label>
												<asp:CheckBox ID="cbxWrite" Enabled="False" Runat="server"></asp:CheckBox><label for="cbxWrite">W</label>
												<asp:CheckBox ID="cbxCreate" Enabled="False" Runat="server"></asp:CheckBox><label for="cbxCreate">C</label></div>
										</td>
									</tr>
								</ItemTemplate>
							</asp:repeater></table>
						<h4>Direct Group Membership</h4>
						<table style="WIDTH: 640px" cellspacing="0" cellpadding="0" border="0">
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
										Height="22"></asp:imagebutton><br />
									<asp:imagebutton id="ibtnRemoveParent" Runat="server" Width="74" ImageUrl="../img/remove-btn.gif"
										CssClass="buttonstyle" Height="22"></asp:imagebutton></td>
								<td style="WIDTH: 306px"><asp:listbox cssClass="i18n" id="lbxParentGroups" Runat="server" Width="288px" Rows="6" Columns="30" TextMode="MultiLine"></asp:listbox></td>
								<td style="WIDTH: 1px"></td>
							</tr>
						</table>
						<br />
						
						<table style="WIDTH: 640px" cellspacing="0" cellpadding="0" border="0">
							<% if (showAssocLabClients){ %>
							<tr>
								<td>
									<div id="messagebox">
										<h4>Associated Lab Clients
										</h4>
										<br />
										<ol>
											<asp:repeater id="repLabClients" Runat="server">
												<ItemTemplate>
													<li>
														<strong>Name:</strong>
														<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "ClientName"))%>
														<br>
														<strong>Desc:</strong>
														<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "ClientShortDescription"))%>
													</li>
												</ItemTemplate>
											</asp:repeater></ol>
									</div>
								</td>
							</tr>
							<%}%>
							
							<tr>
								<th style="WIDTH: 635px" colspan="3">
									<asp:button id="btnSaveChanges" Runat="server" Width="133px" CssClass="buttonright" Text="Save Changes"></asp:button></th></tr>
							</table>
					</div>
				</div>
				<!-- End pagecontent div --><br clear="all" />
			</div>
		</form>
	</body>
</html>
