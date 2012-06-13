<%@ Page language="c#" Inherits="iLabs.Scheduling.UserSide.SelfRegistration" CodeFile="SelfRegistration.aspx.cs" EnableEventValidation="false" %>
<%@ Register TagPrefix="uc1" TagName="banner" Src="banner.ascx" %>
<%@ Register TagPrefix="uc1" TagName="NavBar" Src="NavBar.ascx" %>
<%@ Register Assembly="iLabControls" Namespace="iLabs.Controls" TagPrefix="iLab" %>
<%@ Register TagPrefix="uc1" TagName="footer" Src="footer.ascx" %>


<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head>
		<title>Self Registration</title> 
		<!-- 
Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
Please see license.txt in top level directory for full license. 
-->
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
		<meta content="C#" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
		<style type="text/css">@import url( css/main.css );
		</style>
	</head>
	<body>
	    <form id="Form1" method="post" runat="server">
	    <a name="top"></a>
		<div id="outerwrapper">
			<uc1:banner id="Banner1" runat="server"></uc1:banner>
			<uc1:NavBar id="NavBar1" runat="server"></uc1:NavBar>
			<div id="innerwrapper">
			    <iLab:RegisterSelf ID="selfReg" runat="server" AgentType="SCHEDULING SERVER" />	
			</div><!-- end innerwrapper div -->
			<uc1:footer id="Footer1" runat="server"></uc1:footer>
		</div>
		<br clear="all">
		</form>			
	</body>
</html>
