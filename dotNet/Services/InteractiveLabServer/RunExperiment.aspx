<%@ Page language="c#" Inherits="iLabs.LabServer.LabView.RunExperiment" CodeFile="RunExperiment.aspx.cs"  EnableSessionState="true"%>
<%@ Register TagPrefix="uc1" TagName="banner" Src="banner.ascx" %>
<%@ Register TagPrefix="uc1" TagName="lvpanel" Src="LVRemotePanel.ascx" %>
<%@ Register TagPrefix="uc1" TagName="footer" Src="footer.ascx" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head>
		<title><% =title %></title> 
		<!-- 
Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
Please see license.txt in top level directory for full license. 
-->
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
		<meta content="C#" name="CODE_LANGUAGE" />
		<meta content="JavaScript" name="vs_defaultClientScript" />
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
		<style type="text/css">@import url( css/main.css );
		</style>
	</head>
	<body>
		<form method="post" runat="server">
			<a name="top"></a>
			<div id="outerwrapper"><uc1:banner id="Banner1" runat="server"></uc1:banner><br clear="all">
				<div id="innerwrapper">
					<div id="pageintro">
						<h1><asp:label id="lblExperimentTitle" Runat="server"><% =title %></asp:label></h1>
						<asp:label id="lblDescription" Runat="server"></asp:label>
					</div><!-- end pageintro div -->
					<div id="pagecontent">
					    <p><asp:HyperLink id="lnkBackSB" Text="Back to InteractiveSB" runat="server" ></asp:HyperLink></p>
						<uc1:lvpanel id="thePanel" Runat="Server"> </uc1:lvpanel>
					    <div id="divTimeRemaining" runat="server" visible="false" ><p><asp:Label runat="server" ID="lblTime">Time Remaining:&nbsp;&nbsp;&nbsp;&nbsp;</asp:Label><asp:TextBox ID="txtTimeRemaining" runat="server"  ReadOnly="true" Width="200px"/> </p></div>
					</div><!-- end pagecontent div -->
					<br clear="all" />
				</div><!-- end innerwrapper div -->
				<div><uc1:footer id="Footer1" runat="server"></uc1:footer></div>
			</div>
		</form>
	</body>
</html>
