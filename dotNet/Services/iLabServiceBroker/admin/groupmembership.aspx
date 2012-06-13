<%@ Register TagPrefix="uc1" TagName="footer" Src="../footer.ascx" %>
<%@ Register TagPrefix="uc1" TagName="adminNav" Src="adminNav.ascx" %>
<%@ Register TagPrefix="uc1" TagName="banner" Src="../banner.ascx" %>
<%@ Page language="c#" Inherits="iLabs.ServiceBroker.admin.groupMembership" CodeFile="groupMembership.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
	<head>
		<title>MIT iLab Service Broker - Group Membership</title> 
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
			<div id="outerwrapper"><uc1:banner id="Banner1" runat="server"></uc1:banner><uc1:adminnav id="AdminNav1" runat="server"></uc1:adminnav><br clear="all"/>
				<div id="innerwrapper">
					<div id="pageintro">
						<h1>Group Membership</h1>
						<p>To copy or move users from one group to another, open the group in the left window and select users. In the right window select the 
						target group or groups. If multiple target groups are selected the selected users will be copied to all groups.</p>
						<p>To remove users from a group or groups  expand the groups tree in the left window and select the users, users will only be removed from
						the groups where they are selected. If a user is removed from all groups the user will be added to the orphan group.</p>
						<p>When the page is first displayed the tree on the left groups with sub-groups are expanded, groups that only have users will need to be expanded to display the users.</p>
						<!-- Group Membership Error message here: -->
						<asp:label id="lblResponse" Visible="False" Runat="server"></asp:label>
					</div><!-- end pageintro div -->
					<div id="pagecontent">
						<!--
						<div class="simpleform"><label for="searchby">Search by:&nbsp;&nbsp;</label><asp:dropdownlist CssClass="i18n" id="ddlSearchBy" Runat="server" Width="243px">
								<asp:ListItem Value="-- select one --">-- select one --</asp:ListItem>
								<asp:ListItem Value="Username">Username</asp:ListItem>
								<asp:ListItem Value="Last Name">Last Name</asp:ListItem>
								<asp:ListItem Value="First Name">First Name</asp:ListItem>
								<asp:ListItem Value="Group">Group</asp:ListItem>
							</asp:dropdownlist>
							<br/><br/>
							<asp:textbox id="txtSearchBy" Runat="server" Width="303px"></asp:textbox>&nbsp;&nbsp;<asp:button id="btnSearch" Runat="server" Text="Search" CssClass="button" onclick="btnSearch_Click"></asp:button>
						</div>
						-->
						<div class="simpleform">
							<table>
								<tr>
									<th class="top">
										<label for="usersandgroups">Users and Groups </label>
									</th>
									<th>
										&nbsp;
									</th>
									<th class="top">
										<label for="targetgroups">Target Groups </label>
									</th>
								</tr>
								<tr>
									<td style="WIDTH: 300px">
										<!--Check to see if default style can be set from the css -->
										<asp:TreeView   id="agentsTreeView" runat="server" cssClass="treeView" 
										ForeColor="black" style="font-family:verdana,arial,helvetica;font-size:10px"
										SelectedNodeStyle-ForeColor="White" SelectedNodeStyle-Font-Bold="true"   SelectedNodeStyle-BackColor="BlueViolet"></asp:TreeView>
									</td>
									<td class="buttonstyle"><asp:ImageButton ID="ibtnCopyTo" Runat="server" Width="74" Height="22" CssClass="buttonstyle" ImageUrl="../img/copy-btn.gif"
											AlternateText="Copy To"></asp:ImageButton><br/>
										<asp:ImageButton ID="ibtnMoveTo" Runat="server" Width="74" Height="22" CssClass="buttonstyle" ImageUrl="../img/move-btn.gif"
											AlternateText="Move to"></asp:ImageButton><br/>
										<asp:ImageButton ID="ibtnRemove" Runat="server" Width="74" Height="22" CssClass="buttonstyle" ImageUrl="../img/remove-btn.gif"
											AlternateText="Remove"></asp:ImageButton>
									</td>
									<td style="WIDTH: 310px">
										<!-- Check to see if default style can be set from the css -->
										<asp:TreeView id="groupsTreeView" runat="server"  cssClass="treeView"
										ForeColor="black" style="font-family:verdana,arial,helvetica;font-size:10px"
										SelectedNodeStyle-ForeColor="White" SelectedNodeStyle-Font-Bold="true"   SelectedNodeStyle-BackColor="BlueViolet"></asp:TreeView>
										<div></div>
									</td>
								</tr>
							</table>
						</div>
					</div>
					<br clear="all"/>
					<!-- end pagecontent div -->
				</div> <!-- end innerwrapper div -->
				<uc1:footer id="Footer1" runat="server"></uc1:footer></div>
		</form>
	</body>
</html>
