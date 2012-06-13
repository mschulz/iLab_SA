<%@ Page language="c#" Inherits="iLabs.ServiceBroker.admin.messages" validateRequest="false" CodeFile="messages.aspx.cs" %>
<%@ Register TagPrefix="uc1" TagName="banner" Src="../banner.ascx" %>
<%@ Register TagPrefix="uc1" TagName="adminNav" Src="adminNav.ascx" %>
<%@ Register TagPrefix="uc1" TagName="footer" Src="../footer.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
	<head>
		<title>MIT iLab Service Broker - Messages</title> 
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
		<script language="JavaScript" type="text/JavaScript">
<!--
// Javascript for Alert for Remove button. You'll need to modify it so that it works properly. 

function rusure(){
	question = confirm("Are you sure you want to remove this Message?")
	if (question !="0"){
		top.location = "YOUR LINK GOES HERE"
	}
}

function MM_openBrWindow(theURL,winName,features) { //v2.0
  window.open(theURL,winName,features);
}

//-->
		</script>
	</head>
	<body>
		<form id="Form1" method="post" runat="server">
			<a name="top"></a>
			<div id="outerwrapper">
				<uc1:banner id="Banner1" runat="server"></uc1:banner>
				<uc1:adminNav id="AdminNav1" runat="server"></uc1:adminNav>
				<br clear="all">
				<div id="innerwrapper">
					<div id="pageintro">
						<h1>Messages</h1>
						<p>Add, remove, or modify a system message below.</p>
						<asp:label id="lblResponse" Runat="server" Visible="False" EnableViewState="False"></asp:label>
					</div><!-- end pageintro div -->
					<div id="pagecontent">
						<div id="itemdisplay" style="WIDTH: 50.13%; HEIGHT: 469px">
							<h4>Selected Message
							</h4>
							<div class="simpleform">
								<table class="button" cellSpacing="0" cellPadding="0" border="0">
									<tr>
										<th style="HEIGHT: 77px">
											<label for="targetlabs">Target Labs </label>
										</th>
										<td style="HEIGHT: 77px"><asp:textbox id="txtTargetLabs" Runat="server" ReadOnly="true" BackColor="Lavender" TextMode="MultiLine"
												Columns="40" Rows="3"></asp:textbox>
										</td>
									</tr>
									<tr>
										<th style="HEIGHT: 65px">
											<label for="targetgroups">Target Groups </label>
										</th>
										<td style="HEIGHT: 65px"><asp:textbox id="txtTargetGroups" Runat="server" ReadOnly="true" BackColor="Lavender" TextMode="MultiLine"
												Columns="40" Rows="3"></asp:textbox>
										</td>
									</tr>
									<tr>
										<th style="HEIGHT: 31px">
											<label for="id">Message ID </label>
										</th>
										<td style="HEIGHT: 31px"><asp:textbox id="txtMessageID" Runat="server" ReadOnly="true" BackColor="Lavender"></asp:textbox>
										</td>
									</tr>
									<tr>
										<th style="HEIGHT: 39px">
											<label for="title">Message Title </label>
										</th>
										<td style="HEIGHT: 39px"><asp:textbox id="txtMessageTitle" Runat="server" Columns="50"></asp:textbox>
										</td>
									</tr>
									<tr>
										<th style="HEIGHT: 37px">
											<label for="lasmodified">Last Modified </label>
										</th>
										<td style="HEIGHT: 37px"><asp:textbox id="txtLastModified" Runat="server" ReadOnly="true" BackColor="Lavender"></asp:textbox>&nbsp;&nbsp;<asp:label id="lblDateFormat" for="dateformate" runat="server"></asp:label>&nbsp;&nbsp;<asp:label id="lblTzOff" for="timezoneOffset" runat="server"></asp:label>
										</td>
									</tr>
									<tr>
										<th style="HEIGHT: 90px">
											<label for="message">Message</label></th>
										<td style="HEIGHT: 90px"><asp:textbox id="txtMessageBody" Runat="server" TextMode="MultiLine" Columns="40" Rows="5"></asp:textbox>
										</td>
									</tr>
									<tr>
										<th>
											&nbsp;</th>
										<td><asp:checkbox id="cbxDisplayMessage" Runat="server"></asp:checkbox>&nbsp;Display message
										</td>
									</tr>
									<tr>
										<th colSpan="2">
											<asp:button id="previewMessage" Runat="server" Text="Preview Message" CssClass="buttonright" onclick="previewMessage_Click"></asp:button>
										</th>
									</tr>
									<tr>
										<th colSpan="2">
											<asp:button id="btnNew" Runat="server" Text="New" CssClass="buttonright" onclick="btnNew_Click"></asp:button>
											<asp:button id="btnRemove" Runat="server" Text="Remove" CssClass="buttonright" onclick="btnRemove_Click"></asp:button>
											<asp:button id="btnSaveChanges" Runat="server" Text="Save Changes" CssClass="buttonright" onclick="btnSaveChanges_Click"></asp:button>
										</th>
									</tr>
								</table>
							</div>
						</div>
						<div class="simpleform">
						<%if (Session["GroupName"].ToString().Equals(iLabs.ServiceBroker.Administration.Group.SUPERUSER)) {%>
							<fieldset><legend>Select the type of message you want to view</legend><asp:radiobuttonlist id="rbtnSelectType" Runat="server" RepeatColumns="3" RepeatDirection="Horizontal"
									TextAlign="Right" AutoPostBack="True" onselectedindexchanged="rbtnSelectType_SelectedIndexChanged">
									<asp:ListItem Value="group">group</asp:ListItem>
									<asp:ListItem Value="lab">lab server</asp:ListItem>
									<asp:ListItem Value="system">system</asp:ListItem>
								</asp:radiobuttonlist></fieldset> <%}%>
							<label for="messagetype">Select the groupname
									<%if (Session["GroupName"].ToString().Equals(iLabs.ServiceBroker.Administration.Group.SUPERUSER)) {%>, lab, or system<%}%></label>&nbsp;<br>
							<asp:dropdownlist CssClass="i18n" id="ddlMessageTarget" Runat="server" Width="300px"></asp:dropdownlist>&nbsp;&nbsp;<asp:button id="btnGo" Runat="server" Text="Go" CssClass="button" onclick="btnGo_Click"></asp:button></div>
						<div>&nbsp;</div>
						<div class="simpleform"><label for="selectmessage">Select message</label>
							<br>
							<asp:listbox cssClass="i18n" id="lbxSelectMessage" Runat="server" AutoPostBack="True" Width="400px" Height="346px" onselectedindexchanged="lbxSelectMessage_SelectedIndexChanged"></asp:listbox>
						</div>
						<p>&nbsp;</p>
					</div> <!-- end pagecontent div -->
					<br clear="all">
				</div> <!-- end innerwrapper div -->
				<uc1:footer id="Footer1" runat="server"></uc1:footer>
			</div> <!-- end outerwrapper div -->
		</form>
	</body>
</html>
