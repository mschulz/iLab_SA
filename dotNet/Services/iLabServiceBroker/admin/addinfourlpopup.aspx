<%@ Page language="c#" Inherits="iLabs.ServiceBroker.admin.addInfoUrlPopup" CodeFile="addInfoUrlPopup.aspx.cs"  EnableEventValidation="false"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head>
		<title>addInfoUrlPopup</title> 
		<!-- 
Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
Please see license.txt in top level directory for full license. 
-->
		<!-- $Id$ -->
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
		<meta content="C#" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
		<link href="../css/main.css" rel="stylesheet" type="text/css"/>
		<link href="../css/popup.css" rel="stylesheet" type="text/css"/>
	</head>
	<body>
		<form id="Form1" method="post" runat="server">
			<div id="wrapper">
				<div id="pageintro">
					<h1>Lab Client Resources for <asp:Label ID="lblLabClient" Runat="server"></asp:Label></h1>
					<p>Add, edit, reorder, and remove lab client resource URLs. The save buttons will be enabled once you modify information.</p>
					<p><asp:label id="lblResponse" Runat="server"></asp:label></p>
				</div>
				<div id="pagecontent">
					<div id="sidedisplay">
						<h4>Add/Edit Resource</h4>
						<div class="simpleform">
							<table cellspacing="0" cellpadding="0" border="0">
								<tr>
									<th style="width: 90px"><label for="infoname">Info Name</label></th>
									<td style="WIDTH: 281px"><asp:textbox id="txtInfoname" Runat="server" Width="280px"></asp:textbox></td>
								</tr>
								<tr>
									<th><label for="url">Link URL</label></th>
									<td><asp:textbox id="txtUrl" Runat="server" Width="280px" ></asp:textbox></td>
								</tr>
								<tr>
									<th><label for="desc">Link<br/>Description</label></th>
									<td><asp:textbox id="txtDesc" Runat="server" TextMode="MultiLine" Columns="20" Rows="5" Width="280px" ></asp:textbox></td>
								</tr>
								<tr>
									<th align="center" colspan="2">
										<asp:button id="btnSaveInfoChanges" Runat="server" Text="Save Changes" CssClass="button" onclick="btnSaveInfoChanges_Click"></asp:button>
										<asp:button id="btnNew" Runat="server" Text="New" CssClass="button" onclick="btnNew_Click"></asp:button>
									</th>
								</tr>
							</table>
							<asp:HiddenField id="hdnClientInfoID" runat="server" />
							<asp:HiddenField id="hdnDisplayOrder" runat="server"/>
						</div>
						<!-- End simpleform div -->
						<h4>Reorder List</h4>
						<div class="simpleform">
							<table cellspacing="0" cellpadding="0" border="0">
								<tr>
								    <td style="WIDTH: 90px">&nbsp;</td>
									<th style="WIDTH: 281px" align="Left"><label for="changeorder"><b> Select an item to change its order</b></label></th>
							    </tr>
								<tr>
									<td class="buttonstyle"><p><asp:imagebutton id="ibtnMoveUp" Runat="server" CssClass="buttonstyle" Width="43" ImageUrl="../img/up-btn.gif"
											Height="22"></asp:imagebutton><br/>
										<asp:imagebutton id="ibtnMoveDown" Runat="server" CssClass="buttonstyle" Width="57" ImageUrl="../img/down-btn.gif"
											Height="22"></asp:imagebutton></p>
									</td>
									<td ><asp:listbox  Rows="8" cssClass="i18n" id="lbxChangeOrder" Runat="server" Width="280px"></asp:listbox></td>
								</tr>
								<tr>
									<th  align="center" colspan="2">
										<asp:button id="btnSaveOrderChanges" CssClass="buttonright" Text="Save Changes" Runat="server" onclick="btnSaveOrderChanges_Click"></asp:button></th></tr>
							</table>
						</div>
					</div><!-- End simpleform Div -->
				</div><!-- End sidedisplay Div -->
				<h2>Lab Client Resources</h2>
				<div class="unit">
					<table cellspacing="0" cellpadding="0" border="0" >
						<asp:repeater id="repClientInfo" Runat="server"  >
							<ItemTemplate>
								<tr>
								    <th style="width: 50px; background-color: White">Link</th>
									<td style="width: 340px; background-color: White"><a href='<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "InfoURL"))%>'target="_blank"><%# Convert.ToString(DataBinder.Eval(Container.DataItem, "infoURLName"))%></a></td>
									<td rowspan="2" style="background-color: White">
										<asp:button id="btnEdit" CssClass="button" Text="Edit"  CommandArgument='<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "ClientInfoID"))%>' CommandName="Edit" Runat="server"></asp:button>
										<asp:button id="btnRemove" CssClass="button" Text="Remove" CommandArgument='<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "ClientInfoID"))%>' CommandName="Remove" Runat="server"></asp:button>
									</td>
								</tr>
								<tr>
									<th style="background-color: White">Desc</th>
									<td style="background-color: White"><%# Convert.ToString(DataBinder.Eval(Container.DataItem, "Description"))%></td>
								</tr>
							</ItemTemplate>
							<AlternatingItemTemplate>
							    <tr>
								    <th style="width: 50px">Link</th>
									<td style="width: 340px"><a href='<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "InfoURL"))%>'target="_blank"><%# Convert.ToString(DataBinder.Eval(Container.DataItem, "infoURLName"))%></a></td>
									<td rowspan="2">
										<asp:button id="btnEdit" CssClass="button" Text="Edit"  CommandArgument='<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "ClientInfoID"))%>' CommandName="Edit" Runat="server"></asp:button>
										<asp:button id="btnRemove" CssClass="button" Text="Remove" CommandArgument='<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "ClientInfoID"))%>' CommandName="Remove" Runat="server"></asp:button>
									</td>
								</tr>
								<tr>
									<th>Desc</th>
									<td><%# Convert.ToString(DataBinder.Eval(Container.DataItem, "Description"))%></td>
								</tr>
							</AlternatingItemTemplate>
						</asp:repeater>
					</table>
				</div>
			</div>
			<DIV></DIV>
			<!-- End Page Content Div -->
			<DIV></DIV>
			<!-- End Wrapper Div--></form>
	</body>
</html>
