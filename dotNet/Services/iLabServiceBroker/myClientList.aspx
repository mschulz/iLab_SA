<%@ Page language="c#" Inherits="iLabs.ServiceBroker.iLabSB.myClientList" CodeFile="myClientList.aspx.cs" %>
<%@ Register TagPrefix="uc1" TagName="banner" Src="banner.ascx" %>
<%@ Register TagPrefix="uc1" TagName="footer" Src="footer.ascx" %>
<%@ Register TagPrefix="uc1" TagName="userNav" Src="userNav.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<!-- 
Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
Please see license.txt in top level directory for full license. 
-->
<!-- $Id$ -->
	<head>
		<title>MIT iLab Service Broker - My Labs</title>
		<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1"/>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
		<meta content="C#" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
		<style type="text/css"> @import url( css/main.css );  </style>
		
	</head>
	<body>
		<form id="manyLabs" method="post" runat="server">
			<a name="top"></a>
			<div id="outerwrapper">
				
				<uc1:banner id="Banner1" runat="server"></uc1:banner>
				<uc1:userNav id="UserNav1" runat="server"></uc1:userNav>	
				<br clear="all"/>
				<div id="innerwrapper">
					<div id="pageintro">
						<h1>My Labs
						</h1>
						<h2>Group:
							<asp:Label ID="lblGroupNameTitle" Runat="server"></asp:Label></h2>
					</div> <!-- end pageintro div -->
					<div id="pagecontent">
						<div id="messagebox-right">
							<h3><asp:Label ID="lblGroupNameSystemMessage" Runat="server"></asp:Label></h3>
							<asp:repeater id="repSystemMessage" runat="server">
								<ItemTemplate>
								    <h4><%# Convert.ToString(DataBinder.Eval(Container.DataItem, "MessageTitle")) %></h4>
									<p class="message"><%# Convert.ToString(DataBinder.Eval(Container.DataItem, "MessageBody")) %></p>
									<p class="date">Date Posted:
										<%# userFormatTime((DateTime)(DataBinder.GetPropertyValue(Container.DataItem, "LastModified"))) %>
									</p>
								</ItemTemplate>
							</asp:repeater>
						</div> <!-- Div id "singlelab" is displayed if only one client is available. Otherwise, div class "group" is displayed, which has a list of available labs. -->
						<div class="group-left">
							<h3>Labs for
								<asp:Label ID="lblGroupNameLabList" Runat="server"></asp:Label></h3>
							<asp:repeater id="repLabs" runat="server">
								<ItemTemplate>
									<p><strong>
											<asp:LinkButton Runat="server" ID="lblLabs" CommandName="SetLabClient">
												<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "ClientName")) %>
											</asp:LinkButton>
											- </strong>
										<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "ClientShortDescription")) %>
									</p>
									<p></p>
								</ItemTemplate>
							</asp:repeater>
							<!--<p class="lab"><strong><a href="samplelab.html">Lab 1</a></strong> - description</p>
							<p class="lab"><strong><a href="samplelab.html">Lab 2</a></strong> - description</p>
							<p class="lab"><strong><a href="samplelab.html">Lab 3 </a></strong>- description</p>
							<p class="lab"><strong><a href="samplelab.html">Lab 4 </a></strong>- description</p>-->
						</div>
					</div>
					<br clear="all"/> <!-- end pagecontent div -->
				</div> <!-- end innerwrapper div -->
				<uc1:footer id="Footer1" runat="server"></uc1:footer>
			</div>
		</form>
	</body>
</html>
