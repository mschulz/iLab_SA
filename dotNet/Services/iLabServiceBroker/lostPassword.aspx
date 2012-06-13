<%@ Page language="c#" Inherits="iLabs.ServiceBroker.iLabSB.lostPassword" CodeFile="lostPassword.aspx.cs" EnableEventValidation="false" %>
<%@ Register TagPrefix="uc1" TagName="banner" Src="banner.ascx" %>
<%@ Register TagPrefix="uc1" TagName="userNav" Src="userNav.ascx" %>
<%@ Register TagPrefix="uc1" TagName="footer" Src="footer.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<HTML>
<!-- 
Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
Please see license.txt in top level directory for full license. 
-->
<!-- $Id$ -->
	<HEAD>
		<title>MIT iLab Service Broker - Lost Password</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
		<style type="text/css">@import url( css/main.css ); 
		</style>
	</HEAD>
	<body>
		<form method="post" action="" runat="server">
			<a name="top"></a>
			<div id="outerwrapper">
				<uc1:banner id="Banner1" runat="server"></uc1:banner>
				<uc1:userNav id="UserNav1" runat="server"></uc1:userNav>
				<br clear="all">
				<div id="innerwrapper">
					<div id="pageintro">
						<h1>Lost Password</h1> <!-- Errormessage for login should appear here:-->
						<!--div class="errormessage"-->
						<asp:label id="lblResponse" Runat="server" Visible="False"></asp:label>
						<!--/div--> <!--End login error message -->
						<p>Submit your username and email information. A random password will be generated and emailed to you.
						</p>
					</div> <!-- end pageintro div -->
					<div id="pagecontent">
						<div class="simpleform"><form id="login" action="" method="post" name="login">
								<table>
									<tr>
										<th>
											<label for="username">Username</label></th>
										<td><asp:textbox id="txtUsername" Runat="server"></asp:textbox> <!--input name="username" type="text" id="username" /--></td>
									</tr>
									<tr>
										<th>
											<label for="email">Email</label></th>
										<td><asp:textbox id="txtEmail" Runat="server"></asp:textbox> <!--input type="password" name="password" id="password" /--></td>
									</tr>
									<tr>
										<td colSpan="2"><asp:button id="btnSubmit" runat="server" cssclass="buttonright" Text="Submit" onclick="btnSubmit_Click"></asp:button></td>
									</tr>
								</table>
							</form>
						</div>
						<p>&nbsp;</p>
					</div>
					<br clear="all"> <!-- end pagecontent div --></div> <!-- end innerwrapper div -->
				<uc1:footer id="Footer1" runat="server"></uc1:footer>
			</div>
		</form>
	</body>
</HTML>
