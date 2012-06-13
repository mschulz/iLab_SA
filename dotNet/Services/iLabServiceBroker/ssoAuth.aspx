<%@ Register TagPrefix="uc1" TagName="footer" Src="footer.ascx" %>
<%@ Register TagPrefix="uc1" TagName="userNav" Src="userNav.ascx" %>
<%@ Register TagPrefix="uc1" TagName="banner" Src="banner.ascx" %>
<%@ Register TagPrefix="uc1" TagName="login" Src="login.ascx" %>
<%@ Page language="c#" Inherits="iLabs.ServiceBroker.iLabSB.ssoAuth" CodeFile="ssoAuth.aspx.cs" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
	<head>
		<title>iLab Service Broker - Authorization</title> 
		<!-- 
Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
Please see license.txt in top level directory for full license. 
-->
		<!-- $Id$ -->
		<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1"/>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
		<meta content="C#" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
		<style type="text/css">@import url( css/main.css ); 
		</style>
	</head>
	<body>
		<form method="post" action="ssoAuth.aspx" runat="server">
		<script language="javascript" >
        var visitortime = new Date();
        document.write('<input type="hidden" name="userTZ" id="userTZ"');
        if(visitortime) {
            document.write('value="' + -visitortime.getTimezoneOffset() + '">');
        }
        else {
            document.write('value="JavaScript not Date() enabled">');
        }
    </script>
			<a name="top"></a>
			<div id="outerwrapper">
				<uc1:banner id="Banner1" runat="server"></uc1:banner>
				<uc1:userNav id="UserNav1" runat="server"></uc1:userNav>
				<br clear="all"/>
				<div id="innerwrapper">
					<div id="pageintro">
						<h1>Client Authorization</h1>
						<p><asp:label id="lblMessages" Runat="server" Visible="False"></asp:label></p>
						<h1>Welcome to iLab</h1> <!-- Graphic goes here <IMG height="150" alt=" " src="img/homepage-img-ph.gif" width="200"> - End Graphic -->
						<p>iLab is dedicated to the proposition that online laboratories - real 
							laboratories accessed through the Internet - can enrich science and engineering 
							education by greatly expanding the range of experiments that students are 
							exposed to in the course of their education.
						</p>
					</div>
					<!-- end pageintro div -->
					<div id="pagecontent">
					<asp:HiddenField ID="hdnAuthority" runat="server" />
					<asp:HiddenField ID="hdnKey" runat="server" />
					<asp:HiddenField ID="hdnUser" runat="server" />
					<asp:HiddenField ID="hdnGroup" runat="server" />
					<asp:HiddenField ID="hdnClient" runat="server" />
						<!-- Login Box -->
						<div id="divLogin" runat="server">
						<table>
		<tr>
			<th colspan="2">
				<!-- Error message for login-->
				<asp:label id="lblLoginErrorMessage" Runat="server" Visible="False"></asp:label>
			</th>
		</tr>
		<tr>
			<th>
				<label for="username">Username</label></th>
			<td><asp:textbox id="txtUsername" Runat="server"></asp:textbox>
		
			</td>
		</tr>
		<tr>
			<th>
				<label for="password">Password</label></th>
			<td><asp:textbox id="txtPassword" Runat="server" TextMode="Password"></asp:textbox></td>
		</tr>
		<tr>
			<th colspan="2">
				<asp:button id="btnLogIn" runat="server" Text="Log in" cssclass="buttonright" onclick="btnLogIn_Click"></asp:button>
			</th>
		</tr>
	</table>
						<p>If you don't have an account, <a href="register.aspx">register here</a>.
							<br/>
							Let us know if you <a href="lostPassword.aspx">lost your password</a>.</p>
						</div>
						<!-- Div id "singlelab" is displayed if only one client is available. Otherwise, div class "group" is displayed, which has a list of available labs. -->
					</div>
					<br clear="all" />
					<!-- end pagecontent div --></div>
				<!-- end innerwrapper div --><uc1:footer id="Footer1" runat="server"></uc1:footer></div>
			
		</form>
	</body>
</html>
