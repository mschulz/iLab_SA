<%@ Register TagPrefix="uc1" TagName="footer" Src="footer.ascx" %>
<%@ Register TagPrefix="uc1" TagName="userNav" Src="NavBarReg.ascx" %>
<%@ Register TagPrefix="uc1" TagName="banner" Src="banner.ascx" %>
<%@ Register TagPrefix="recaptcha" Namespace="Recaptcha" Assembly="Recaptcha" %>
<%@ Page language="c#" Inherits="iLabs.Scheduling.UserSide.helpReg" CodeFile="helpReg.aspx.cs" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<!-- 
Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
Please see license.txt in top level directory for full license. 
-->
<!-- $Id$ -->
	<head>
		<title>MIT iLab Lab Server - Help</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
		<meta content="C#" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="/vs_targetSchema"/>
		<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1"/>
		<style type="text/css">@import url( css/main.css ); 
		</style>
	</head>
	<body>
		<form id="helpForm" method="post" runat="server">
			<a name="top"></a>
			<div id="outerwrapper"><uc1:banner id="Banner1" runat="server"></uc1:banner><uc1:usernav id="UserNav1" runat="server"></uc1:usernav><br clear="all"/>
				<div id="innerwrapper">
					<div id="pageintro">
						<h1>Help</h1>
						<p>On this page you can read the <A href="#faqs">FAQ's</A>, or <A href="#help">Request 
								Help</A> for a specific problem or lab. <!-- , or <A href="reportBug.aspx">Report a 
								Bug</A> with the system. --> </p>
						<!-- Errormessage should appear here:-->
						<!--div class="errormessage"--><asp:label id="lblErrorMessage" Visible="False" Runat="server"></asp:label>
						<!--/div--> <!--End error message -->
					</div> <!-- end pageintro div -->
					<div id="pagecontent">
						<p><b>IMPORTANT:</b> To be able to run the Labs you must have requested access from your Service Broker</p>
						<p></p>
						<div id="faq">
							<h2><a id="faqs" name="faqs"></a>FAQ's</h2>
							<ul>
								<li>
									<a href="#q1">Create a Service Broker Account</a></li>
								
								
								
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
									page. You now have access to any Labs available to your group, depending on 
									the lab you may need to schedual a time to run the Lab.
								</p>
							</div>
							<!-- <div class="qa"><a id="q2" name="q3"></a>
								<p class="question">Report a Bug</p>
								<p class="answer">To <a href="reportBug.aspx">report a bug, use this link</a> or 
									the button at the bottom of this page and fill out the form. Please select the 
									general type of problem and enter a detailed description of the problem.</p>
							</div> -->
							<div id="requesthelp">
								<h2><a id="help" name="help"></a>Request Help with a Lab</h2>
								<p>Fill out the form below to request help with a lab. Someone will respond to you 
									shortly.</p>
								<% if (Session !=null) 
							if (Session["UserID"] !=null) { %>
								<p>The Lab Server does not track individual users. Please enter your email address, so that support may respond to your question.</p>
								<% } %>
								<div class="simpleform">
									<table>
									    <tr>
											<th>
												<label for="email">Email Addres:</label></th>
											<td><asp:textbox id="txtEmail" Runat="server" Rows="1" Columns="50" TextMode="SingleLine" Width="416px"></asp:textbox>
												<!--textarea name="problem" cols="50" rows="6" id="problem"></textarea--></td>
										</tr>
										<tr>
											<th>
												<label for="lab">Select the type of help you need.</label></th>
											<td><asp:dropdownlist cssClass="i18n" id="ddlWhichLab" Runat="server"></asp:dropdownlist></td>
										</tr>
										<tr>
											<th>
												<label for="problem">Describe your problem</label></th>
											<td><asp:textbox id="txtProblem" Runat="server" Rows="6" Columns="50" TextMode="MultiLine"></asp:textbox>
												<!--textarea name="problem" cols="50" rows="6" id="problem"></textarea--></td>
										</tr>
										<tr>
										    <th style="width: 192px"><label for="enterSecurityCode">Please enter security code</label></th>
										    <!-- This has been patched to support the EmbedJavascript property for IE 6 and 7 -->
										    <td style="width: 608px"><recaptcha:RecaptchaControl  ID="recaptcha" runat="server"  Theme="blackglass"/></td>
										</tr>
										<tr>
											<th colspan="2">
												<asp:button id="btnRequestHelp" Runat="server" CssClass="buttonright" Text="Request Help" onclick="btnRequestHelp_Click"></asp:button>
											</th>
										</tr>
									</table>
								</div> <!-- end div class simpleform -->
								<p><a href="#top">Top of Page</a></p>
							</div> <!-- end div request help -->
						</div> <!-- end div faq --><br clear="all"/>
					</div> <!-- end pagecontent div -->
				</div> <!-- end innerwrapper div -->
				<uc1:footer id="Footer1" runat="server"></uc1:footer>
			</div> <!-- end outterwrapper div -->
		</form>
	</body>
</html>
