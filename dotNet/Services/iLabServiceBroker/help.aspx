<%@ Register TagPrefix="uc1" TagName="footer" Src="footer.ascx" %>
<%@ Register TagPrefix="uc1" TagName="userNav" Src="userNav.ascx" %>
<%@ Register TagPrefix="uc1" TagName="banner" Src="banner.ascx" %>
<%@ Register TagPrefix="recaptcha" Namespace="Recaptcha" Assembly="Recaptcha" %>
<%@ Page language="c#" CodeFile="help.aspx.cs" AutoEventWireup="false" Inherits="iLabs.ServiceBroker.iLabSB.help" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<HTML>
	<HEAD>
		<title>MIT iLab Service Broker - Help</title> 
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
		<style type="text/css">@import url( css/main.css ); 
		</style>
	</HEAD>
	<body>
	    <asp:label id="lblRevision" Visible="True" Runat="server"></asp:label>
		<form id="helpForm" method="post" runat="server">
			<div id="outerwrapper">
			    <uc1:banner id="Banner1" runat="server"></uc1:banner>
			    <uc1:usernav id="UserNav1" runat="server"></uc1:usernav>
			    <br clear="all">
				<a name="top"></a>
				<div id="innerwrapper">
					<div id="pageintro">
						<h1>Help</h1>
						<p>On this page you can read the <A href="#faqs">FAQ's</A>, <A href="#help">Request 
								Help</A> for a specific problem or lab, or <A href="reportBug.aspx">Report a 
								Bug</A> with the system.</p>
						<!-- Errormessage should appear here:-->
						<asp:label id="lblResponse" Visible="False" Runat="server"></asp:label>
						<!--End error message -->
					</div> <!-- end pageintro div -->
					<div id="pagecontent">
						<p><B>IMPORTANT:</B> To be able to run the Service Broker you must have Pop-Ups 
							enabled.</p>
						<p>To run the Microelectronics Weblab you must have pop-ups enabled and a Java 
							1.4.2 plugin installed. To download the Java plugin please go to the <a href="http://java.sun.com/j2se/1.4.2/download.html">
								Sun Java download site</a>. Download and install the J2SE v 1.4.2 JRE.</p>
						<div id="faq">
							<h2><a id="faqs" name="faqs"></a>FAQ's</h2>
							<ul>
								<li><A href="#q1">Create a Service Broker Account</A></li>
								<li><A href="#q2">Lost Password</A></li>
								<li><A href="#q3">Report a Bug</A></li>
								<li><A href="#q4">Microelectronics Weblab 6.0</A></li>
							</ul>
							<div class="qa"><a id="q1" name="q1"></a>
								<p class="question">Create a Service Broker Account</p>
								<p class="answer">When you enter the Service Broker Web service your browser is 
									directed to the Login page, if you already have created an account you may 
									login. To create an account select the "New User Registration" button, which 
									will take you to the Registration page. Fill out the form and submit your 
									information. User Names's must be unique for each Service Broker, if the user 
									name is already registered, a message will be displayed asking you to choose 
									another user name. An error message is displayed if your password and confirm 
									password entries do not match. Once your information is complete your account 
									will be created and you will automaticly be redirected to the "My Clients" 
									page. You now have access to the public Microelectronics iLab.
								</p>
							</div>
							<div class="qa"><a id="q2" name="q2"></a>
								<p class="question">Lost Password</p>
								<p class="answer">If you have an account, but do not remember your password, select 
									the "Lost Password" button. Fill out the form, you must supply both user name 
									and email address. If a registered user with both user name and email address 
									is found, a new temporary password will be mailed to the email address.</p>
							</div>
							<div class="qa"><a id="q3" name="q3"></a>
								<p class="question">Report a Bug</p>
								<p class="answer">To <a href="reportBug.aspx">report a bug, use this link</a> or 
									the button at the bottom of this page and fill out the form. Please select the 
									general type of problem and enter a detailed description of the problem.</p>
							</div>
							<div class="qa"><a id="q4" name="q4"></a>
								<p class="question">Microelectronics Weblab 6.0</p>
								<p class="answer">If you need help or are experienceing problems with the 
									Microelectrionics Weblab please see the <a href="http://weblab2.mit.edu/docs/weblab/v6.1/manual/">
										Lab specific help</a></p>
							</div>
						</div> <!-- end div faq --><br clear="all"/>
						<div id="requesthelp">
								<h2><a id="help" name="help"></a>Request Help with a Lab</h2>
								<p>Fill out the form below to request help with the iLab system or a particular 
									lab. Someone will respond to you shortly. You will have to enter the the security code before you can send the request.</p>
								<% if (Session["UserID"] == null) { %>
								<p>You are not currently logged in. Please include your name and email address, so 
									that we can respond to you.</p>
								<% } %>
								<div class="simpleform">
									<table>
										<tr>
											<th style="width: 192px"><asp:Label id="lblUserName" runat="server">User Name</asp:Label></th>
											<td style="width: 608px"><asp:TextBox id="txtUserName" runat="server" Width="600px"></asp:TextBox></td>
										</tr>
										<tr>
											<th style="width: 192px"><asp:Label id="lblEmail" runat="server">Email</asp:Label></th>
											<td style="width: 608px"><asp:TextBox id="txtEmail" runat="server" Width="600px"></asp:TextBox></td>
										</tr>
										<tr>
											<th style="width: 192px"><label for="lab">Select the type<br/>of help you need.</label></th>
											<td style="width: 608px"><asp:dropdownlist CssClass="i18n" id="ddlHelpType" Runat="server" Width="603px"></asp:dropdownlist></td>
										</tr>
										<tr>
											<th style="width: 192px"><label for="problem">Describe your problem</label></th>
											<td style="width: 608px"><asp:textbox id="txtProblem" Runat="server" Rows="6" Columns="50" TextMode="MultiLine" Width="600px"></asp:textbox></td>
										</tr>
										<tr>
										    <th style="width: 192px"><label for="enterSecurityCode">Please enter security code</label></th>
										    <!-- This has been patched to support the EmbedJavascript property for IE 6 and 7 -->
										    <td style="width: 608px"><recaptcha:RecaptchaControl  ID="recaptcha" runat="server"  Theme="blackglass"/></td>
										</tr>
										<tr>
										    <th><label for="sendHelp">Send Help Request</label></th>
										    <th style="width: 608px"><asp:button id="btnRequestHelp" Runat="server" CssClass="buttonright" Text="Send Request"></asp:button></th>
										</tr>
										<tr>
											<td colspan="2"><p>&nbsp;</p></td>
										</tr>
										<tr>
											<th>Submit a Bug Report:</th>
											<td style="width: 608px"><asp:button id="btnReportBug" Runat="server" CssClass="buttonright" Text="Report Bug"></asp:button></td>
										</tr>
									</table>
								</div> <!-- end div class simpleform -->
						</div> <!-- end div request help -->
						<p><A href="#top">Top of Page</A></p>
					</div> <!-- end pagecontent div -->
				</div> <!-- end innerwrapper div -->
				<uc1:footer id="Footer1" runat="server"></uc1:footer>
			</div> <!-- end outterwrapper div -->
		</form>
	</body>
</HTML>
