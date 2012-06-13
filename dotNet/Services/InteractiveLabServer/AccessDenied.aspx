<%@ Page language="c#" Inherits="iLabs.LabServer.LabView.AccessDenied" CodeFile="AccessDenied.aspx.cs" %>

<%@ Register TagPrefix="uc1" TagName="banner" Src="banner.ascx" %>
<%@ Register TagPrefix="uc1" TagName="userNav" Src="userNav.ascx" %>
<%@ Register TagPrefix="uc1" TagName="footer" Src="footer.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" > 

<html>
  <!-- 
Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
Please see license.txt in top level directory for full license. 
-->
<!-- $Id$ -->
	<HEAD>
		<title>AccessDenied</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
		<style type="text/css">@import url( css/main.css ); 
		</style>
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<a name="top"></a>
			<div id="outerwrapper">
				<uc1:banner id="Banner1" runat="server"></uc1:banner>
				<uc1:userNav id="UserNav1" runat="server"></uc1:userNav>
				<br clear="all">
				<div id="innerwrapper">
					<div id="pageintro">
						<h1>Access Denied</h1>
					</div>
					<!-- end pageintro div -->
					<div id="pagecontent">
					<p>Too Bad</p>
					<asp:Label id="lblText" runat="server"></asp:Label>
					</div>
					<br clear="all">
					<!-- end pagecontent div --></div>
				<!-- end innerwrapper div --><uc1:footer id="Footer1" runat="server"></uc1:footer></div>
		</form>
	</body>
</HTML>

  
  
  
  
