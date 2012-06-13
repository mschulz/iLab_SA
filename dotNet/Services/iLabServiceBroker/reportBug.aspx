<%@ Register TagPrefix="uc1" TagName="footer" Src="footer.ascx" %>
<%@ Register TagPrefix="uc1" TagName="userNav" Src="userNav.ascx" %>
<%@ Register TagPrefix="uc1" TagName="banner" Src="banner.ascx" %>
<%@ Register TagPrefix="recaptcha" Namespace="Recaptcha" Assembly="Recaptcha" %>
<%@ Page language="c#" CodeFile="reportBug.aspx.cs" AutoEventWireup="false" Inherits="iLabs.ServiceBroker.iLabSB.reportBug" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<HTML>
	<HEAD>
		<title>MIT iLab Service Broker - Report Bug</title> 
		<!-- 
Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
Please see license.txt in top level directory for full license. 
-->
		<!-- $Id$ -->
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
		<style type="text/css"> @import url( css/main.css );  </style>
	</HEAD>
	<body>
		<form id="reportBugForm" method="post" runat="server">
			<a name="top"></a>
			<div id="outerwrapper">
				<uc1:banner id="Banner1" runat="server"></uc1:banner>
				<uc1:userNav id="UserNav1" runat="server"></uc1:userNav>
				<br clear="all">
				<div id="innerwrapper">
					<div id="pageintro">
						<h1>Bug Report</h1>
						<p>On this page you can submit a bug report.</p>
						<!-- Errormessage should appear here:-->
						<asp:label id="lblResponse" Runat="server" Visible="False"></asp:label>
					</div>
					<!-- end pageintro div -->
					<div id="pagecontent">
						<div id="reportbug">
							<h2><a name="bug" id="bug"></a>Report a Bug</h2>
							<p>Fill out the form below to report a bug with the iLab system or a particular 
								lab.</p>
							<% if( Session["UserID"] == null) { %>
							<p>You are not currently logged in. Please include your name and email address, so 
								that we can respond to you.</p>
							<% } %>
							<div class="simpleform">
								<table>
									<tr>
										<th>
											<asp:Label ID="lblUserName" Visible="false" Runat="server">User Name</asp:Label>
										</th>
										<td>
											<asp:TextBox id="txtUserName" Visible="False" Runat="server" Width="415px"></asp:TextBox>
										</td>
									</tr>
									<tr>
										<th>
											<asp:Label ID="lblEmail" Visible="false" Runat="server">Email</asp:Label>
										</th>
										<td>
											<asp:TextBox id="txtEmail" Visible="False" Runat="server" Width="415px"></asp:TextBox>
										</td>
									</tr>
									<tr>
										<th>
											<label for="whichlab">Select the type of<br/>problem or lab server</label></th>
										<td>
											<asp:DropDownList CssClass="i18n" ID="ddlArea" Runat="server" Width="415px"></asp:DropDownList>
										</td>
									</tr>
									<tr>
										<th>
											<label for="problemtext">Describe your problem</label></th>
										<td>
											<asp:TextBox ID="txtBugReport" Runat="server" TextMode="MultiLine" Columns="50" Rows="6"></asp:TextBox>
										</td>
									</tr>
									<tr>
									    <th><label for="captcha">Please enter the<br/>security code</label></th>
									    <td><!-- This has been patched to support the EmbedJavascript property for IE 6 and 7 -->
										    <recaptcha:RecaptchaControl  ID="recaptcha" runat="server"  Theme="blackglass"/>
										</td>
									</tr>
									<tr>
										<th colspan="2">
											<asp:Button ID="btnReportBug" Runat="server" Text="Report Bug" CssClass="buttonright"></asp:Button>
										</th>
									</tr>
								</table>
							</div> <!-- end div class=simpleform -->
							<p><a href="#top">Top of Page</a></p>
						</div> <!-- end div reportbug -->
						<br clear="all">
					</div> <!-- end pagecontent div -->
				</div> <!-- end innerwrapper div -->
				<uc1:footer id="Footer1" runat="server"></uc1:footer>
			</div> <!-- end outterwrapper div -->
		</form>
	</body>
</HTML>
