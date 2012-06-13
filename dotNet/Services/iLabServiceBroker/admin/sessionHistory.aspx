<%@ Register TagPrefix="uc1" TagName="footer" Src="../footer.ascx" %>
<%@ Register TagPrefix="uc1" TagName="adminNav" Src="adminNav.ascx" %>
<%@ Register TagPrefix="uc1" TagName="banner" Src="../banner.ascx" %>
<%@ Page language="c#" Inherits="iLabs.ServiceBroker.admin.sessionHistory" CodeFile="sessionHistory.aspx.cs" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
	<head>
		<title>MIT iLab Service Broker - Session History Records</title> 
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
			<div id="outerwrapper"><uc1:banner id="Banner1" runat="server"></uc1:banner><uc1:adminnav id="AdminNav1" runat="server"></uc1:adminnav><br clear="all" />
				<div id="innerwrapper">
					<div id="pageintro">
						<h1>Session History Records
						</h1>
						<p>View Session History records below.
						</p>
						<p><asp:Label ID="lblTimezone" runat="server"></asp:Label></p>
						<asp:label id="lblResponse" EnableViewState="False" Visible="False" Runat="server"></asp:label></div>
					<!-- end pageintro div -->
					<div id="pagecontent">
						<div class="simpleform">
							<table style="WIDTH: 488px; HEIGHT: 111px">
								<TBODY>
									<tr>
										<th style="HEIGHT: 35px">
											<label for="useris">Username</label></th>
										<td style="HEIGHT: 35px" colSpan="4"><asp:textbox id="txtUserName" Runat="server"></asp:textbox>
											<!--input name="textfield" type="text" size="20"--></td>
										<!-- the following field uses the class "noneditable" if the user does not select between from the drop-down list --></tr>
									<tr>
										<th style="HEIGHT: 31px">
											<label for="authorityis">Authority</label></th>
										<td style="HEIGHT: 31px" colSpan="4"><asp:dropdownlist id="ddlAuthority" Runat="server" Width="412px"></asp:dropdownlist>
											<!-- the following field uses the class "noneditable" if the user does not select between from the drop-down list --></td>
									</tr>
									<tr>
										<th style="HEIGHT: 31px">
											<label for="groupis">Groupname</label></th>
										<td style="HEIGHT: 31px" colSpan="4"><asp:textbox id="txtGroupName" Runat="server"></asp:textbox>
											<!-- the following field uses the class "noneditable" if the user does not select between from the drop-down list --></td>
									</tr>
									<tr>
										<th style="HEIGHT: 31px">
											<label for="timeis">Time </label>
										</th>
										<td style="WIDTH: 118px; HEIGHT: 31px"><asp:dropdownlist CssClass="i18n" id="ddlTimeIs" Runat="server" Width="112px" AutoPostBack="True" onselectedindexchanged="ddlTimeIs_SelectedIndexChanged">
												<asp:ListItem Value="select">--Select one--</asp:ListItem>
												<asp:ListItem Value="equal">equal to</asp:ListItem>
												<asp:ListItem Value="before">before</asp:ListItem>
												<asp:ListItem Value="after">after</asp:ListItem>
												<asp:ListItem Value="between">between</asp:ListItem>
											</asp:dropdownlist></td>
										<td style="WIDTH: 125px; HEIGHT: 31px"><asp:textbox id="txtTime1" Runat="server" Width="120px"></asp:textbox>
											<!--input name="textfield" type="text" size="10"--></td>
										<!-- the following field uses the class "noneditable" if the user does not select between from the drop-down list -->
										<td style="WIDTH: 130px; HEIGHT: 31px"><asp:textbox id="txtTime2" Runat="server" Width="127px" ReadOnly="true"></asp:textbox>
											<!--input name="textfield" type="text" class="noneditable" size="10"--></td>
										<td style="HEIGHT: 31px"><asp:button id="btnGo" Runat="server" Width="50px" CssClass="button" Text=" Go " onclick="btnGo_Click"></asp:button>
											<!--input name="Submit" type="submit" class="button" value="Go"--></td>
									</tr>
								</TBODY>
							</table>
						</div>
						<asp:textbox id="txtLoginDisplay" Runat="server"  Height="300px" TextMode="MultiLine"
							Rows="20" Width="900px"></asp:textbox>
						<!--textarea name="logindisplay" cols="50" rows="20" id="logindisplay"--> 
						</div>
					<br clear="all"/>
					<!-- end pagecontent div --></div> <!-- end innerwrapper div -->
					<uc1:footer id="Footer1" runat="server"></uc1:footer></div>
		</form>
	</body>
</html>
