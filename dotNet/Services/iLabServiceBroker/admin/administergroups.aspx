<%@ Page language="c#" Inherits="iLabs.ServiceBroker.admin.administerGroups" CodeFile="administerGroups.aspx.cs" %>
<%@ Register TagPrefix="uc1" TagName="banner" Src="../banner.ascx" %>
<%@ Register TagPrefix="uc1" TagName="adminNav" Src="adminNav.ascx" %>
<%@ Register TagPrefix="uc1" TagName="footer" Src="../footer.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<!-- 
Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
Please see license.txt in top level directory for full license. 
-->
<!-- $Id$ -->
	<head>
		<title>MIT iLab Service Broker - Administer Groups</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
		<meta content="C#" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
		<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1"/>
		<style type="text/css">@import url( ../css/main.css ); 
		</style>
		<script language="JavaScript" type="text/JavaScript">
	<!--
	// Javascript for Alert for Remove button. You'll need to modify it so that it works properly. 

	function confirmDelete()
	{
		if(confirm("Are you sure you want to delete the group?")== true)
			return true;
		else
			return false;
	}
	
	function openPopupWindow(url){
		window.open(url,'addeditgroup','scrollbars=yes,resizable=yes,width=760,height=1000');
	}

	//-->
		</script>
	</head>
	<body>
		<form id="Form1" method="post" runat="server">
			<a name="top"></a><input id="hiddenPopupOnSave" type="hidden" runat="server" NAME="hiddenPopupOnSave">
			<button id="btnRefresh" runat="server" style="VISIBILITY: hidden" type="button"></button>
			<div id="outerwrapper">
				<uc1:banner id="Banner1" runat="server"></uc1:banner>
				<uc1:adminNav id="AdminNav1" runat="server"></uc1:adminNav>
				<br clear="all"/>
				<div id="innerwrapper">
					<div id="pageintro">
						<h1>Administer Groups
						</h1>
						<p>Add, remove, or edit a group below.
						</p>
						<!-- Administer Groups Error message here: <div class="errormessage"><p>Error message here.</p></div> End error message -->
						<asp:label id="lblResponse" Runat="server" EnableViewState="False" Visible="False"></asp:label>
					</div>
					<!-- end pageintro div -->
					<div id="pagecontent">
						<p><asp:button id="btnAddGroup" CssClass="button" Text="Add Group" Runat="server"></asp:button>
						   <asp:button id="btnAddServAdminGroup" CssClass="button" Text="Add Service Admin Group" Runat="server"></asp:button>
						</p>
						<p></p>
						
						
						<asp:repeater id="repGroups" Runat="server">
							<ItemTemplate>
							<div class="unit">
								<table border="0" cellspacing="0" cellpadding="0" cols="3" >
										<tr>
											<th width="40">
												Group
											</th>
											<td width="400"><%# Convert.ToString(DataBinder.Eval(Container,"DataItem.groupName"))%>
											</td>
											<td rowspan="2" width="110">
												<asp:Button ID="btnEdit" Runat="server" Text="Edit" CommandName="Edit" CssClass="button"></asp:Button>
												<asp:Button ID="btnRemove" Runat="server" Text="Remove" CommandName="Remove" CssClass="button"></asp:Button>
											</td>
										</tr>
										<tr>
											<th>
												Description</th>
											<td>
												<asp:Label ID="lblDescription" Runat="server"></asp:Label>
											</td>
										</tr>
										<tr>
											<th>
												Type
											</th>
											<td>
												<%# Convert.ToString(DataBinder.Eval(Container,"DataItem.GroupType"))%>
											</td>
										</tr>
								</table>
								</div>
							</ItemTemplate>
						</asp:repeater>
					</div>
					<br clear="all">
					<!-- end pagecontent div -->
				</div> <!-- end innerwrapper div -->
				<uc1:footer id="Footer1" runat="server"></uc1:footer>
			</div> <!-- end outerwrapper -->
		</form>
	</body>
</html>
