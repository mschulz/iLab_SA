<%@ Page EnableEventValidation="false" language="c#" Inherits="iLabs.ServiceBroker.admin.grants" CodeFile="grants.aspx.cs" %>
<%@ Register TagPrefix="uc1" TagName="banner" Src="../banner.ascx" %>
<%@ Register TagPrefix="uc1" TagName="adminNav" Src="adminNav.ascx" %>
<%@ Register TagPrefix="uc1" TagName="footer" Src="../footer.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
	<head>
		<title>MIT iLab Service Broker - Grants</title> 
		<!-- 
Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
Please see license.txt in top level directory for full license. 
-->
		<!-- $Id$ -->
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
		<style type="text/css">@import url( ../css/main.css ); 
		</style>
		<script language="JavaScript" type="text/JavaScript">
	<!--

	// Javascript for Alert for Remove button. You'll need to modify it so that it works properly. 

	function rusure(){
	question = confirm("Are you sure you want to remove this Grant?")
	if (question !="0"){
		top.location = "YOUR LINK GOES HERE"
		}
		}

		//-->
		</script>
	</head>
	<body>
		<form id="Form1" method="post" runat="server">
			<a name="top"></a>
			<div id="outerwrapper"><uc1:banner id="Banner1" runat="server"></uc1:banner><uc1:adminnav id="AdminNav1" runat="server"></uc1:adminnav><br clear="all">
				<div id="innerwrapper">
					<div id="pageintro">
						<h1>Grants</h1>
						<p>Add or remove a grant below. Select a user/group, a function, and qualifiers if 
							applicable.
						</p>
						<!-- Grants Error message here: <div class="errormessage"><p>Error message here.</p></div> End error message --><asp:label id="lblResponse" Visible="False" Runat="server"></asp:label></div>
					<!-- end pageintro div -->
					<div id="pagecontent">
						<div id="messagebox-right">
							<h4>Directions</h4>
							<asp:label id="lblDirections" Runat="server"></asp:label></div>
						<div id="messagebox">
							<h4>Qualifier Types</h4>
							<ul class="inline">
								<li>
									<img height="16" alt="group qualifier" src="../img/GrantImages/Folder.gif" width="16">
								- Group
								</li>
								<li>
									<img height="16" alt="lab client qualifier" src="../img/GrantImages/client.gif" width="16">
								- Lab Client
								</li>
								<li>
									<img height="16" alt="lab server qualifier" src="../img/GrantImages/server.gif" width="16">
								- Lab Server
								</li>
								<li>
									<img src="../img/GrantImages/expt.gif" alt="experiment qualifier" width="16" height="20">
								- Experiment
								</li>
								<li>
									<img src="../img/GrantImages/exptcol.gif" alt="experiment collection qualifier" width="16"
										height="20"> - Experiment Collection
								</li>
								<li>
									<img src="../img/GrantImages/mapping.GIF" alt="resource mapping qualifier" width="16"
										height="20"> - Resource Mapping
								</li>
							</ul>
						</div>
						<div class="simpleform">
							<form name="grants" method="post" action="">
								<table border="0" cellpadding="10" cellspacing="0" class="buttonright">
									<TBODY>
										<tr>
											<th class="top" style="WIDTH: 245px; HEIGHT: 25px">
												<label for="usersgroups">Users and Groups</label></th>
											<th class="top" style="WIDTH: 116px; HEIGHT: 25px">
												<label for="functions">Functions</label></th>
											<th class="top" style="HEIGHT: 25px">
												<label for="qualifiers">Qualifiers</label></th>
										</tr>
										<tr>
											<td style="HEIGHT: 264px">
												<div>
													<!-- Check to see if default style can be set from the css -->
													<asp:TreeView id="userTreeView" PopulateNodesFromClient="true" OnTreeNodePopulate="PopulateAgentNode"
cssClass="treeView" runat="server"  SelectedNodeStyle-ForeColor="White" SelectedNodeStyle-Font-Bold="true"   SelectedNodeStyle-BackColor="BlueViolet" ForeColor="black" style="font-family:verdana,arial,helvetica;font-size:10px"></asp:TreeView>
												</div>
												<!--textarea name="usersgroups" cols="30" rows="15" id="usersgroups"></textarea--></td>
											<td style="HEIGHT: 350px" valign="top">
												<asp:listbox cssClass="i18n" ID="lbxFunctions" Runat="server" Height="270px" Width="190px" Rows="8" Font-Names="Arial"
													Font-Size="12px"></asp:ListBox>
												<!--select name="functions" size="12" id="functions">
												<option value="0" selected>-- Select function type --</option>
												<option value="1">function type 1</option>
												<option value="2">function type 2</option>
												<option value="3">function type 3</option>
											</select--></td>
											<td style="HEIGHT: 264px">
												<div>
													<asp:TreeView id="qualifierTreeView" PopulateNodesFromClient="true" OnTreeNodePopulate="PopulateQualifierNode" runat="server"  SelectedNodeStyle-ForeColor="White" SelectedNodeStyle-Font-Bold="true"   SelectedNodeStyle-BackColor="BlueViolet" ForeColor="black" cssClass="treeView"  style="font-family:verdana,arial,helvetica;font-size:10px"></asp:TreeView>
												</div>
												<!--textarea name="qualifiers" cols="30" rows="15" id="qualifiers"></textarea--></td>
										</tr>
										<tr>
											<td style="WIDTH: 245px">&nbsp;</td>
											<th colspan="2">
												<asp:Button ID="btnViewGrants" Runat="server" Text="View Grants" CssClass="buttonright" onclick="btnViewGrant_Click"></asp:Button>
												<!--input name="Submit" type="submit" class="buttonright" value="View Grant"-->
												<asp:Button ID="btnRemoveGrant" Runat="server" Text="Remove Grant" CssClass="buttonright" onclick="btnRemoveGrant_Click"></asp:Button>
												<!--input name="Submit" type="submit" class="buttonright" value="Remove Grant" onClick="rusure(); return false;"-->
												<asp:Button ID="btnAddGrant" Runat="server" Text="Add Grant" CssClass="buttonright" onclick="btnAddGrant_Click"></asp:Button>
												<!--input name="Submit" type="submit" class="buttonright" value="Add Grant"-->
											</th>
										</tr>
									</TBODY>
								</table>
							</form>
						</div>
					</div>
					<br clear="all">
					<!-- end pagecontent div --></div> <!-- end innerwrapper div -->
				<uc1:footer id="Footer1" runat="server"></uc1:footer></div>
		</form>
	</body>
</html>
