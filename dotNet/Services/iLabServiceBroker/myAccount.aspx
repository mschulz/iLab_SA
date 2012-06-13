<%@ Register TagPrefix="uc1" TagName="footer" Src="footer.ascx" %>
<%@ Register TagPrefix="uc1" TagName="userNav" Src="userNav.ascx" %>
<%@ Register TagPrefix="uc1" TagName="banner" Src="banner.ascx" %>
<%@ Page language="c#" Inherits="iLabs.ServiceBroker.iLabSB.myAccount" CodeFile="myAccount.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<!-- 
Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
Please see license.txt in top level directory for full license. 
-->
<!-- $Id$ -->
	<head>
		<title>MIT iLab Service Broker - My Account</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1" />
		<meta name="CODE_LANGUAGE" content="C#" />
		<meta name="vs_defaultClientScript" content="JavaScript" />
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5" />
		<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
		<style type="text/css"> @import url( css/main.css );  </style>
	</head>
	<body>
		<form id="register" method="post" runat="server">
			<a name="top"></a>
			<div id="outerwrapper">
				<uc1:banner id="Banner1" runat="server"></uc1:banner>
				<uc1:userNav id="UserNav1" runat="server"></uc1:userNav>
				<br clear="all" />
				<div id="innerwrapper">
					<div id="pageintro">
						<h1>My Account
						</h1>
						<p>You can edit your account information below.</p>
						<!-- Errormessage should appear here:-->
						<!--div class="errormessage"-->
						<asp:Label Runat="server" id="lblResponse" Visible="False"></asp:Label>
						<!--/div--> <!--End error message -->
					</div> <!-- end pageintro div -->
					<div id="pagecontent">
						<div id="actionbox-right">
							<h3>More Information</h3>
							<p class="message">You are currently a member of the following group(s): <strong>
									<asp:Label ID="lblGroups" Runat="server"></asp:Label></strong></p>
							<p class="message">You have requested membership in the following group(s): <strong>
									<asp:label id="lblRequestGroups" Runat="server"></asp:label></strong></p>
							<p class="message"><a href="requestgroup.aspx"><strong>Request membership in a new group. </strong>
								</a>
							</p>
						</div>
						<h3>Edit Account Information
						</h3>
						<p>Complete all fields below to change your account information. You will be be 
							emailed a confirmation.</p>
						<div class="simpleform">
							<!--form name="register" id="editaccount" method="post" action=""-->
							<table>
								<tr>
									<th>
										<label for="username">Username</label></th>
									<td><asp:TextBox ID="txtUsername" Runat="server"></asp:TextBox>
										<!--input name="username" type="text" class="noneditable" id="username"--></td>
								</tr>
								<tr>
									<th>
										<label for="Authorities">Authority </label></th>
									<td><asp:dropdownlist CssClass="i18n" id="ddlAuthorities" Runat="server" AutoPostBack="True"></asp:dropdownlist></td>
								</tr>
								<tr>
									<th>
										<label for="firstname">First Name </label>
									</th>
									<td><asp:TextBox ID="txtFirstName" Runat="server"></asp:TextBox>
										<!--input type="text" name="firstname" id="firstname"--></td>
								</tr>
								<tr>
									<th>
										<label for="lastname">Last Name </label>
									</th>
									<td><asp:TextBox ID="txtLastName" Runat="server"></asp:TextBox>
										<!--input type="text" name="lastname" id="lastname"--></td>
								</tr>
								<tr>
									<th>
										<label for="email">Email</label></th>
									<td><asp:TextBox ID="txtEmail" Runat="server"></asp:TextBox>
										<!--input type="text" name="email" id="email"--></td>
								</tr>
								<tr>
									<th>
										<label for="password">New Password</label></th>
									<td><asp:TextBox ID="txtNewPassword" Runat="server" TextMode="Password"></asp:TextBox>
										<!--input type="password" name="password" id="password"--></td>
								</tr>
								<tr>
									<th>
										<label for="passwordconfirm">Confirm New Password </label>
									</th>
									<td><asp:TextBox ID="txtConfirmPassword" Runat="server" TextMode="Password"></asp:TextBox>
										<!--input type="password" name="passwordconfirm" id="passwordconfirm"--></td>
								</tr>
								<tr>
									<td colspan="2">
										<asp:Button ID="btnSaveChanges" runat="server" Text="Save Changes" CssClass="buttonright" onclick="btnSaveChanges_Click"></asp:Button><!input 
										type="submit" name="Submit" value="Save Changes" class="buttonright"----></td>
								</tr>
							</table>
							<!--/form-->
						</div>
					</div>
					<br clear="all" /> <!-- end pagecontent div -->
				</div> <!-- end innerwrapper div -->
				<uc1:footer id="Footer1" runat="server"></uc1:footer>
			</div>
		</form>
	</body>
</html>
