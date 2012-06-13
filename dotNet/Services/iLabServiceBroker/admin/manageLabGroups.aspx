<%@ Page language="c#" Inherits="iLabs.ServiceBroker.admin.manageLabGroups" CodeFile="manageLabGroups.aspx.cs" EnableEventValidation="false"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
	<head>
		<title>MIT iLab Service Broker - Manage Lab Groups</title> 
		<!-- 
Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
Please see license.txt in top level directory for full license. 
-->
		<!-- $Id$ -->
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
		<meta content="C#" name="CODE_LANGUAGE" />
		<meta content="JavaScript" name="vs_defaultClientScript" />
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
		<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
		<link href="../css/main.css" type="text/css" rel="stylesheet"/>
		<link href="../css/popup.css" type="text/css" rel="stylesheet"/>
		<style type="text/css">@import url( ../css/popup.css );
		</style>
		<script type="text/javascript">
		<!--
		function ReloadParent() 
        {      str = "manageLabClients.aspx?refresh="  + id;
            if (window.opener){            
                window.opener.location.href = str;
                window.opener.focus();
            } 
            window.close();  
        }
        
        function ConfirmRemove()
        {
            return confirm("Are you sure you want to remove the group?\nDoing so will revoke all remaining reservations for the client made by the group!");
        }
        -->
        </script>
	</head>
	<body>
		<form id="Form1" method="post" runat="server">
			<a name="top"></a>
				<div id="innerwrapper">
					<div id="pageintro">
						<h1>Manage Lab Groups
						</h1>
						<p>Select a Lab Client and then give permissions to groups below.
						</p>
						<div runat="server"><p><asp:label id="lblResponse" Runat="server"></asp:label></p></div>
					</div>
					<!-- end pageintro div -->
					<div id="pagecontent">
						<div class="simpleform"><label for="labclient">Select Lab Client</label>
							<br />
							<asp:dropdownlist CssClass="i18n" id="ddlLabClient" Runat="server" AutoPostBack="True" Width="260px" onselectedindexchanged="ddlLabClient_SelectedIndexChanged"></asp:dropdownlist></div>
						<DIV class="simpleform">&nbsp;</DIV>
						<div id="messagebox">
							<h3>Associated Lab Servers</h3>
							<ul><asp:repeater id="repLabServers" Runat="server"><ItemTemplate><li><strong>Name:</strong><%# Convert.ToString(DataBinder.Eval(Container.DataItem, "AgentName"))%><br />
								<strong>URL:</strong><%# Convert.ToString(DataBinder.Eval(Container.DataItem, "ServiceURL"))%>
							    </li>
							</ItemTemplate></asp:repeater></ul>
							<h3>Associated USS</h3>
							<ul><asp:repeater id="repUSS" Runat="server"><ItemTemplate><li><strong>USS Name:</strong><%# Convert.ToString(DataBinder.Eval(Container.DataItem, "AgentName"))%><br />
								<strong>URL:</strong><%# Convert.ToString(DataBinder.Eval(Container.DataItem, "ServiceURL"))%>
								</li>
							</ItemTemplate></asp:repeater></ul>
							<h3>Associated ESS</h3>
							<ul><asp:repeater id="repESS" Runat="server"><ItemTemplate><li><strong>ESS Name:</strong><%# Convert.ToString(DataBinder.Eval(Container.DataItem, "AgentName"))%><br />
								<strong>URL:</strong><%# Convert.ToString(DataBinder.Eval(Container.DataItem, "ServiceURL"))%>
								</li>
							</ItemTemplate></asp:repeater></ul>
						</div>
						<br clear="all" />
						<div class="simpleform">
							<table cellspacing="0" cellpadding="0" border="0">
							<tr>
								<th class="top" style="width: 201px; height: 22px">
									<label for="userGroup">User Group</label>
                                    <asp:DropDownList CssClass="i18n" ID="ddlUserGroup" runat="server" Width="240px"></asp:DropDownList>
                                </th>         
								<th id="thManagementGroup" runat="server" class="top" style="width: 201px; height: 22px">
									<label for="managementGroup">Management Group</label>
                                    <asp:DropDownList CssClass="i18n" ID="ddlAdminGroup" runat="server" Width="288px"></asp:DropDownList>
                                </th>         
							</tr>
							<tr>
							    <th><asp:Button ID="btnClose" runat="server" Text="Close" CssClass="button"  OnClick="btnClose_Click" Visible="false"></asp:Button></th>
								<th><asp:button id="btnSaveChange" Runat="server" CssClass="buttonright" Text="Save Changes" onclick="btnSaveChange_Click"></asp:button></th>
							</tr>
							</table>
							</div>
						
							<asp:Repeater ID="repAdminUserGroups"   runat="server">
                            <ItemTemplate>
							<div class="unit">
								<table border="0" cellspacing="0" cellpadding="0" cols="3">
										<th width="40">User Group</th>
										<td width="400">
											<asp:Label ID="lblUserGroup" Runat="server"></asp:Label>
										</td>
										<td id="tdRemove" rowspan="2" width="110" runat="server">
											<asp:Button ID="btnRemove" Runat="server" Text="Remove" CommandName="Remove" CssClass="button"></asp:Button>
										</td>											
									</tr>	
									<tr id="trManagement" runat="server">
										<th>Management Group</th>
										<td>
											<asp:Label ID="lblManageGroup" Runat="server">
											</asp:Label>
										</td>
									</tr>								
								<table>
							</div>
							</ItemTemplate>
                        </asp:Repeater>				
					<br clear="all" />
					<!-- end pagecontent div --></div>
				<!-- end innerwrapper div --></div>
		
		</form>
	</body>
</html>
