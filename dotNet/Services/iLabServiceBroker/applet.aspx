<%@ Page EnableSessionState="true" language="c#" Inherits="iLabs.ServiceBroker.iLabSB.applet" CodeFile="applet.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
<!-- 
Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
Please see license.txt in top level directory for full license. 
-->
<!-- $Id$ -->
	<HEAD>
		<title>applet</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio 7.0"/>
		<meta name="CODE_LANGUAGE" Content="C#"/>
		<meta name="vs_defaultClientScript" content="JavaScript"/>
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5"/>
	</HEAD>
	<body>
		<form method="post" runat="server">
			<img src="img/applet.gif" WIDTH="940" HEIGHT="32"/>
			<%if (Session["LoaderScript"] != null ) {%>
				<%=Session["LoaderScript"]%>
				<script language="javascript">focus();</script>
			<% } %>
		</form>
	</body>
</HTML>
