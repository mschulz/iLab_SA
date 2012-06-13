<%@ Page language="c#" Inherits="iLabs.LabServer.LabView.RunLab" CodeFile="RunLab.aspx.cs" %>
<%@ Register TagPrefix="uc1" TagName="banner" Src="banner.ascx" %>
<%@ Register TagPrefix="uc1" TagName="lvframe" Src="LVFrame.ascx" %>
<%@ Register TagPrefix="uc1" TagName="footer" Src="footer.ascx" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>RunLab</title> 
		<!-- 
Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
Please see license.txt in top level directory for full license. 
-->
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( css/main.css );
		</style>
	</HEAD>
	<body>
		<form method="post" runat="server">
			<a name="top"></a>
			<div id="outerwrapper"><uc1:banner id="Banner1" runat="server"></uc1:banner><br clear="all">
				<div id="innerwrapper">
					<div id="pageintro">
						<h1><asp:label id="lblExperimentTitle" Runat="server"></asp:label></h1>
						<asp:label id="lblDescription" Runat="server"></asp:Label>
					</div>
					<!-- end pageintro div -->
					<div id="pagecontent">
					<p><asp:HyperLink id="lnkBackSB" Text="Back to InteractiveSB" runat="server" ></asp:HyperLink></p>
						<uc1:lvframe id="theFrame" Runat="Server"> </uc1:lvframe>
						<div id="messagebox-right">
							<h3><asp:label id="lblGroupNameSystemMessage" Runat="server"></asp:label></h3>
							<asp:repeater id="repSystemMessage" runat="server">
								<ItemTemplate>
									<p class="message">
										<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "MessageBody")) %>
									</p>
									<p class="date">Date Posted:
										<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "LastModified")) %>
									</p>
								</ItemTemplate>
							</asp:repeater>
						</div> <!-- End messagebox-right -->
						<!-- Div id "singlelab" is displayed if only one client is available. Otherwise, div class "group" is displayed, which has a list of available labs. -->
					</div>
					<br clear="all">
					<!-- end pagecontent div --></div>
				<!-- end innerwrapper div --><uc1:footer id="Footer1" runat="server"></uc1:footer></div>
		</form>
	</body>
</HTML>
