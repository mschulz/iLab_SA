<%@ Page language="c#" Inherits="iLabs.ServiceBroker.iLabSB.login" CodeFile="login.aspx.cs" %>
<%@ Register TagPrefix="uc1" TagName="banner" Src="banner.ascx" %>
<%@ Register TagPrefix="uc1" TagName="login" Src="login.ascx" %>
<%@ Register TagPrefix="uc1" TagName="footer" Src="footer.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<HTML>
<!-- 
Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
Please see license.txt in top level directory for full license. 
-->
<!-- $Id$ -->
	<HEAD>
		<title>MIT iLab Service Broker - Log In</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
		<style type="text/css"> @import url( css/main.css );  </style>
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<a name="top"></a>
			<div id="outerwrapper">
				<uc1:banner id="Banner1" runat="server"></uc1:banner>
				<div id="navbar">
					<!-- This is where the main navigation goes. The buttons that are visible depend on which page the user is on. -->
					<div id="nav">
						<ul class="navlist">
							<li><a href="home.aspx" class="only">Home</a></li>
						</ul>
					</div> <!-- end nav div -->
					<div id="nav2"> <!-- This is where the help and logout buttons go. Log out only appears if the user is logged in. -->
						<ul class="navlist2">
							<li><a href="help.aspx">Help</a></li>
						</ul>
					</div> <!-- end nav2 div -->
				</div> <!-- end navbar-->
				<br clear="all">
				<div id="innerwrapper">
					<div id="pageintro">
						<h1>Log In</h1>
						<!-- Errormessage for login should appear here:-->
						<!--div class="errormessage"-->
						<!--/div--> <!--End login error message -->
						<p>If you don't have an account, <a href="register.aspx">register here.</a> Let us 
							know if you <a href="lostPassword.aspx">lost your password.</a></p>
					</div> <!-- end pageintro div -->
					<div id="pagecontent">
						<div id="messagebox-right">
							<asp:Label ID="lblSystemMessage" Runat="server">
								<h3>System News and Messages
								</h3>
							</asp:Label>
							<asp:Repeater id="repSystemMessage" runat="server">
								<ItemTemplate>
								    <h4><%# Convert.ToString(DataBinder.Eval(Container.DataItem, "MessageTitle")) %></h4>
									<p class="message"><%# Convert.ToString(DataBinder.Eval(Container.DataItem, "MessageBody")) %></p>
									<p class="date">Date Posted:<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "LastModified")) %></p>
								</ItemTemplate>
							</asp:Repeater>
						</div>
						<!-- Login Box -->
						<uc1:login id="Login1" runat="server"></uc1:login>
					</div>
					<br clear="all"> <!-- end pagecontent div -->
				</div> <!-- end innerwrapper div -->
				<uc1:footer id="Footer1" runat="server"></uc1:footer>
			</div>
		</form>
	</body>
</HTML>
