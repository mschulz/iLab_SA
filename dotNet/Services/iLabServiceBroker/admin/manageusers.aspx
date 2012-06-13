<%@ Register TagPrefix="uc1" TagName="footer" Src="../footer.ascx" %>
<%@ Register TagPrefix="uc1" TagName="adminNav" Src="adminNav.ascx" %>
<%@ Register TagPrefix="uc1" TagName="banner" Src="../banner.ascx" %>
<%@ Page language="c#" Inherits="iLabs.ServiceBroker.admin.manageUser" CodeFile="manageUsers.aspx.cs"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
	<head>
		<title>MIT iLab Service Broker - Manage Users</title> 
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
			<div id="outerwrapper" ><uc1:banner id="Banner1" runat="server"></uc1:banner><uc1:adminnav id="AdminNav1" runat="server"></uc1:adminnav><br clear="all" />
				<div id="innerwrapper">
					<div id="pageintro">
						<h1>Manage Users</h1>
						<p>Add or modify a User below.</p>
						<asp:label id="lblResponse" EnableViewState="False" Visible="False" Runat="server"></asp:label>
					</div><!-- end pageintro div -->
					<div id="pagecontent">
					    <asp:HiddenField ID="hdnUserId" Runat="server" />
						<div id="itemdisplay">
							<h4>Selected User</h4>
							<div class="message">
								<p><asp:label id="lblGroups" Runat="server"></asp:label></p>
								<p><asp:label id="lblRequestGroups" Runat="server"></asp:label></p>
							</div>
							<div class="simpleform">
								<table>
									<tr>
										<th>
											<label for="username">Username</label></th>
										<td><asp:textbox id="txtUsername" Runat="server" Width="310px"></asp:textbox></td>
									</tr>
									<tr>
										<th>
											<label for="firstname">First Name </label>
										</th>
										<td><asp:textbox id="txtFirstName" Runat="server" Width="310px"></asp:textbox></td>
									</tr>
									<tr>
										<th>
											<label for="lastname">Last Name</label></th>
										<td><asp:textbox id="txtLastName" Runat="server" Width="310px"></asp:textbox></td>
									</tr>
									<tr>
										<th>
											<label for="email">Email</label></th>
										<td><asp:textbox id="txtEmail" Runat="server" Width="310px"></asp:textbox></td>
									</tr>
									<tr>
										<th>
											<label for="authority">Authority</label></th>
										<td>
                                            &nbsp;<asp:dropdownlist CssClass="i18n" id="ddlAuthority" Runat="server" Width="315px"></asp:dropdownlist></td>
									</tr><tr>
										<th>
											<label for="affiliation">Affiliation</label></th>
										<td>
											<% if(ConfigurationManager.AppSettings["useAffiliationDDL"].Equals("true")){ %>
											<asp:dropdownlist CssClass="i18n" id="ddlAffiliation" Runat="server" Width="171px"></asp:dropdownlist>
											<% }else{ %>
											<asp:textbox id="txtAffiliation" Runat="server"></asp:textbox>
											<% } %>
										</td>
									</tr>
									<tr>
										<th>
											<label for="pword">Password</label></th>
										<td><asp:textbox id="txtPassword" Runat="server" Width="152px" TextMode="Password"></asp:textbox></td>
									</tr>
									<tr>
										<th>
											<label for="confirmpword">Confirm Password </label>
										</th>
										<td><asp:textbox id="txtConfirmPassword" Runat="server" Width="152px" TextMode="Password"></asp:textbox></td>
									</tr>
									<tr>
										<th>
											&nbsp;</th>
										<td><asp:checkbox id="cbxLockAccount" Runat="server"></asp:checkbox><label for="lock">Lock Account</label></td>
									</tr>
									<tr>
										<th colspan="2">
											<asp:button id="btnSaveChanges" Runat="server" OnClick="btnSaveChanges_Click" Text="Save Changes"></asp:button><asp:button id="btnRemove" Runat="server" OnClick="btnRemove_Click" Text="Remove" CssClass="button"></asp:button><asp:button id="btnNew" Runat="server" OnClick="btnNew_Click" Text="New" CssClass="button"></asp:button></th></tr>
								</table>
							</div>
						</div>
						<div class="simpleform"><label for="searchby">Search by:&nbsp;&nbsp;</label><asp:dropdownlist CssClass="i18n" id="ddlSearchBy" Runat="server" Width="243px">
								<asp:ListItem Value="Select All">Select All</asp:ListItem>
								<asp:ListItem Value="Username">Username</asp:ListItem>
								<asp:ListItem Value="Last Name">Last Name</asp:ListItem>
								<asp:ListItem Value="First Name">First Name</asp:ListItem>
								<asp:ListItem Value="Group">Group</asp:ListItem>
							</asp:dropdownlist>
							<br/><br/>
							<asp:textbox id="txtSearchBy" Runat="server" Width="303px" OnTextChanged="txtSearchBy_TextChanged"></asp:textbox>&nbsp;&nbsp;<asp:button id="btnSearch" Runat="server"  OnClick="btnSearch_Click" Text="Search" CssClass="button"></asp:button>
						</div>
						<div>&nbsp;</div>
						<div class="simpleform"><label for="selectauser">Select a User (Username --  Last Name, First Name)</label><br/>
							<asp:listbox cssClass="i18n" id="lbxSelectUser" Runat="server" AutoPostBack="true"  OnSelectedIndexChanged="lbxSelectUser_SelectedIndexChanged" Width="380px" Rows="15"  Height="310px"></asp:listbox>
						</div>
					</div>
					<br clear="all" />
					<!-- end pagecontent div --></div>
				<!-- end innerwrapper div --><uc1:footer id="Footer1" runat="server"></uc1:footer></div>
		</form>
		<div></div>
	</body>
</html>
