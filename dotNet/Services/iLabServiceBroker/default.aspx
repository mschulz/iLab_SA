<%@ Page EnableSessionState="true"  AutoEventWireup="false" Language="c#" Inherits="iLabs.ServiceBroker.iLabSB._default" CodeFile="default.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head>
		<title>MIT iLab Service Broker</title> 
		<!-- 
Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
Please see license.txt in top level directory for full license. 
-->
		<!-- $Id$ -->
		<meta HTTP-EQUIV="Content-Type" CONTENT="text/html; charset=iso-8859-1" />
		<style type="text/css">@import url( css/main.css ); 
		</style>
	</head>
	<frameset rows="*,1" cols="*">
		<frame id="frmUser" runat="server"   src="home.aspx" name="theuser" />
		<frame id="frmApplet" src="no_applet.html" name="theapplet" scrolling="no" noresize="true" />
		<noframes>
		<p>This application requires frames, please upgrade your browser.</p>
		</noframes>
	</frameset>
</html>
