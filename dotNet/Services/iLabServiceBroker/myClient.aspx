<%@ Register TagPrefix="uc1" TagName="footer" Src="footer.ascx" %>
<%@ Register TagPrefix="uc1" TagName="userNav" Src="userNav.ascx" %>
<%@ Register TagPrefix="uc1" TagName="banner" Src="banner.ascx" %>
<%@ Page language="c#" Inherits="iLabs.ServiceBroker.iLabSB.myClient" CodeFile="myClient.aspx.cs" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
	<head>
		<title>MIT iLab Service Broker - My Client</title> 
		<!-- 
Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
Please see license.txt in top level directory for full license. 
-->
		<!-- $Id$ -->
		<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1"/>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
		<meta content="C#" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
		<style type="text/css">@import url( css/main.css ); 
		</style>
	</head>
	<body>
		<form method="post" runat="server">
			<a name="top"></a>
			<div id="outerwrapper">
				<uc1:banner id="Banner1" runat="server"></uc1:banner>
				<uc1:userNav id="UserNav1" runat="server"></uc1:userNav>
				<br clear="all"/>
				<div id="innerwrapper">
					<div id="pageintro">
						<h1>My Client</h1>
						<h2>Group: <asp:label id="lblGroupNameTitle" Runat="server"></asp:label></h2>
						<asp:Label ID="lblResponse" runat="server" Visible="False"></asp:Label>
					</div>
					<!-- end pageintro div -->
					<div id="pagecontent">
						<div id="messagebox-right">
						    <h3><asp:label id="lblServerSystemMessage" Runat="server"></asp:label></h3>
							<asp:repeater id="repServerMessage" runat="server">
								<ItemTemplate>
								    <h4><%# Convert.ToString(DataBinder.Eval(Container.DataItem, "MessageTitle")) %></h4>
									<p class="message"><%# Convert.ToString(DataBinder.Eval(Container.DataItem, "MessageBody")) %></p>
									<p class="date">Date Posted:<%# userFormatTime((DateTime) DataBinder.GetPropertyValue(Container.DataItem, "LastModified")) %></p>
								</ItemTemplate>
							</asp:repeater>
							<h3><asp:label id="lblGroupNameSystemMessage" Runat="server"></asp:label></h3>
							<asp:repeater id="repGroupMessage" runat="server">
								<ItemTemplate>
								    <h4><%# Convert.ToString(DataBinder.Eval(Container.DataItem, "MessageTitle")) %></h4>
									<p class="message"><%# Convert.ToString(DataBinder.Eval(Container.DataItem, "MessageBody")) %></p>
									<p class="date">Date Posted:<%# userFormatTime((DateTime) DataBinder.GetPropertyValue(Container.DataItem, "LastModified")) %></p>
								</ItemTemplate>
							</asp:repeater>
						</div>
						<!-- Div id "singlelab" is displayed if only one client is available. Otherwise, div class "group" is displayed, which has a list of available labs. -->
						<div class="singlelab-left">
							<h3>Lab Client:
								<asp:label id="lblClientName" Runat="server"></asp:label>
							</h3>
							<p><strong>Version: </strong><asp:label id="lblVersion" Runat="server"></asp:label></p>
							<p><strong>Description: </strong><asp:label id="lblLongDescription" Runat="server"></asp:label></p>
							<p id="pNotes" runat="server"><strong>Notes: </strong><asp:label id="lblNotes" Runat="server"></asp:label></p>
							<p id="pDocURL" runat="server"><strong>Documentation: </strong><asp:Label ID="lblDocURL" Runat="server"></asp:Label></p>
							<p id="pEmail" runat="server"><strong>Contact Email: </strong><asp:label id="lblEmail" Runat="server"></asp:label></p>
							<p id="pLaunch" runat="server"><asp:button id="btnLaunchLab" Runat="server" CssClass="button"
							 Text="Launch Lab" onClick="btnLaunchLab_Click" Visible="false" Width="171px" ></asp:button></p>
							<p id="pReenter" runat="server"><asp:button id="btnReenter" Runat="server" CssClass="button" 
							Text="Re-enter Experiment" onClick="btnReenter_Click" Visible="false" Width="171px" ></asp:button></p>
                            <p id="pSchedule" runat="server"><asp:button id="btnSchedule" Runat="server" CssClass="button" 
                            Text="Schedule/Redeem Session" onclick="btnSchedule_Click" Visible="false" Width="170px"></asp:button>&nbsp;</p>
                            <p/>
							<asp:repeater id="repClientInfos" Runat="server" ></asp:repeater>
						</div>
					</div>
					<br clear="all" />
					<!-- end pagecontent div --></div>
				<!-- end innerwrapper div --><uc1:footer id="Footer1" runat="server"></uc1:footer></div>
		</form>
	</body>
</html>
