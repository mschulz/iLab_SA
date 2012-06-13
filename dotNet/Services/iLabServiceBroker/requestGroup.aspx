<%@ Register TagPrefix="uc1" TagName="footer" Src="footer.ascx" %>
<%@ Register TagPrefix="uc1" TagName="userNav" Src="userNav.ascx" %>
<%@ Register TagPrefix="uc1" TagName="banner" Src="banner.ascx" %>
<%@ Page language="c#" Inherits="iLabs.ServiceBroker.iLabSB.requestGroup" CodeFile="requestGroup.aspx.cs" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
	<head>
		<title>MIT iLab Service Broker - Request Membership in a New Group</title> 
		<!-- 
Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
Please see license.txt in top level directory for full license. 
-->
		<!-- $Id$ -->
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1"/>
		<meta name="CODE_LANGUAGE" content="C#"/>
		<meta name="vs_defaultClientScript" content="JavaScript"/>
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5"/>
		<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1"/>
		<style type="text/css"> @import url( css/main.css );  </style>
	</head>
	<body>
		<form id="Form1" method="post" runat="server">
			<a name="top"></a>
			<div id="outerwrapper">
				<uc1:banner id="Banner1" runat="server"></uc1:banner>
				<uc1:userNav id="UserNav1" runat="server"></uc1:userNav>
				<br clear="all"/>
				<div id="innerwrapper">
					<div id="pageintro">
						<h1>Request Membership in a New Group</h1>
						<p>Select the group(s) you'd like to join below.</p>
						<!-- Errormessage-->
						<asp:Label Runat="server" id="lblResponse" Visible="False"></asp:Label>
						<!--End error message -->
					</div>
					<!-- end pageintro div -->
					<div id="pagecontent">
						<div id="actionbox-right">
							<h3>More Information</h3>
							<p class="message">You are currently a member of the following group(s): <strong>
									<asp:Label ID="lblGroups" Runat="server"></asp:Label></strong></p>
							<p class="message">You have requested membership in the following group(s): <strong>
									<asp:label id="lblRequestGroups" Runat="server"></asp:label></strong></p>
						</div>
						<div class="group">
						    <asp:label id="lblNoGroups" runat="server"></asp:label>
						    <asp:CheckBoxList ID="cblGroups" runat="server" Width="455px" />
						    
						    <asp:repeater id="repAvailableGroups" runat="server">
								<ItemTemplate>
									<p><asp:CheckBox ID="cbxGroup" Runat="server" CssClass="checkbox"></asp:CheckBox>
									<label for="ReqGroup" runat="server" visible="false">
									<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "GroupID")) %>
									</label>
									<label for="group1" >
										<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "groupName")) %>
									</label>
								</ItemTemplate>
								<SeparatorTemplate>
									<p></p>
								</SeparatorTemplate>
							</asp:repeater>
							
						</div>
						<asp:Button ID="btnRequestMembership" Runat="server" Text="Request Membership"  CssClass="button"  OnClick="btnRequestMembership_Click"></asp:Button>
					</div>
					<br clear="all"/>
					<!-- end pagecontent div -->
				</div>
				<!-- end innerwrapper div -->
				<uc1:footer id="Footer1" runat="server"></uc1:footer>
			</div>
		</form>
	</body>
</html>
