<%@ Page language="c#" Inherits="iLabs.ServiceBroker.iLabSB.myGroups" CodeFile="myGroups.aspx.cs" %>
<%@ Register TagPrefix="uc1" TagName="banner" Src="banner.ascx" %>
<%@ Register TagPrefix="uc1" TagName="userNav" Src="userNav.ascx"%>
<%@ Register TagPrefix="uc1" TagName="footer" Src="footer.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<HTML>
<!-- 
Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
Please see license.txt in top level directory for full license. 
-->
<!-- $Id$ -->
	<HEAD>
		<title>MIT iLab Service Broker - My Groups</title>
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
						<h1>My Groups
						</h1>
						<p>Select the group you would like to use for this session.
						</p>
					</div>
					<!-- end pageintro div -->
					<div id="pagecontent">
						<div id="actionbox-right">
							<h3>More Information</h3>
							<p class="message">You have requested membership in the following group(s): <strong>
									<asp:label id="lblRequestGroups" Runat="server"></asp:label></strong></p>
							<p class="message"><A href="requestgroup.aspx"><strong>Request membership in a new group. </strong>
								</A>
							</p>
						</div>
						<h2>Available Groups and Labs
						</h2>
						<div class="group"><asp:label id="lblNoGroups" runat="server"></asp:label><asp:repeater id="repGroups" runat="server">
								<ItemTemplate>
									<p><strong>
											<asp:LinkButton Runat="server" ID="lblGroups" CommandName="SetEffectiveGroup">
												<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "groupName")) %>
											</asp:LinkButton>
											- </strong>
										<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "description")) %>
									</p>
									<p></p>
									
								</ItemTemplate>
							</asp:repeater>
							<p></p>
						</div>
					</div>
					<br clear="all">
					<!-- end pagecontent div --></div>
				<!-- end innerwrapper div -->
				<uc1:footer id="Footer1" runat="server"></uc1:footer>
			</div>
		</form>
	</body>
</HTML>
