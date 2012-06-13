<%@ Page language="c#" Inherits="iLabs.ServiceBroker.admin.messagePreviewPopup" CodeFile="messagePreviewPopup.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN"
"http://www.w3.org/TR/html4/loose.dtd">
<HTML>
	<HEAD>
		<title>Message Preview</title> 
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
		<link href="../css/main.css" rel="stylesheet" type="text/css">
		<link href="../css/popup.css" rel="stylesheet" type="text/css">
		<script language="JavaScript" type="text/JavaScript">
	<!--

	// Javascript for Alert for Remove button. You'll need to modify it so that it works properly. 

	function rusure(){
		question = confirm("Are you sure you want to remove this Additional Lab Client Info?")
		if (question !="0"){
			top.location = "YOUR LINK GOES HERE"
		}	
	}

	//-->
		</script>
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<div id="wrapper">
				<div id="pageintro">
					<h1>Message Preview</h1>
				</div>
				<div id="pagecontent">
					<div id="messagebox">
						<h3><asp:Label ID="lblTitle" Runat="server"></asp:Label></h3>
						<p><asp:Label ID="lblMessage" Runat="server"></asp:Label></p>
						<p><asp:Label ID="lblDatePosted" Runat="server"></asp:Label></p>
					</div>
					<p><a href="#" onclick="window.close()">Close Window</a></p>
				</div>
				<!-- End pagecontent div -->
				<br clear="all">
			</div>
		</form>
	</body>
</HTML>
