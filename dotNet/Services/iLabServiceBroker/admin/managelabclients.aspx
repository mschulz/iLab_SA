<%@ Register TagPrefix="uc1" TagName="footer" Src="../footer.ascx" %>
<%@ Register TagPrefix="uc1" TagName="adminNav" Src="adminNav.ascx" %>
<%@ Register TagPrefix="uc1" TagName="banner" Src="../banner.ascx" %>
<%@ Page language="c#" Inherits="iLabs.ServiceBroker.admin.manageLabClients" validateRequest="false" CodeFile="manageLabClients.aspx.cs" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
	<head>
		<title>MIT iLab Service Broker - Manage Lab Clients</title> 
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
		<style type="text/css">@import url( ../css/main.css ); 
		</style>
		<script language="JavaScript" type="text/JavaScript">
	<!--
	function MM_openBrWindow(theURL,winName,features) { //v2.0
	 window.open(theURL,winName,features);
	}
	//-->
		</script>
	</head>
	<body>
		<form id="Form1" method="post" runat="server">
		<asp:HiddenField id="hdnLabServerID" runat="server" /><asp:HiddenField id="hdnEssID" runat="server" /><asp:HiddenField id="hdnUssID" runat="server" />
		<asp:HiddenField id="hdnNeedsEss" runat="server" /><asp:HiddenField id="hdnNeedsUss" runat="server" />
			<a name="top"></a><input id="hiddenPopupOnSave" type="hidden" name="hiddenPopupOnSave" runat="server" />
			<div id="outerwrapper">
				<uc1:banner id="Banner1" runat="server"></uc1:banner>
				<uc1:adminNav id="AdminNav1" runat="server"></uc1:adminNav>
				<br clear="all" />
				<div id="innerwrapper">
					<div id="pageintro">
						<h1>Manage Lab Clients</h1>
						<p>Add, remove or modify a lab client below.</p>
						<p id="pResponse" runat="server"><asp:label id="lblResponse" Runat="server" Visible="False"></asp:label></p>
						<div id="Div1"  runat="server">
					</div>
					</div><!-- end pageintro div -->
					<div id="pagecontent">
					<!--    <button id="btnRefresh" style="VISIBILITY: hidden" type="button" runat="server" ></button> -->
						<div class="simpleform">
							<table cellspacing="5" cellpadding="0" border="0">
							    <tr style="height: 0px">
							     <td style="width: 100px"></td>
							     <td style="width: 250px"></td>
							     <td style="width: 100px"></td>
							     <td style="width: 250px"></td>
							    </tr>
								<tr>
									<th><label for="labclient">Lab Client</label></th>
									<td colspan="3"><asp:dropdownlist CssClass="i18n" id="ddlLabClient" Runat="server" Width="615px" AutoPostBack="True" onselectedindexchanged="ddlLabClient_SelectedIndexChanged"></asp:dropdownlist></td>
								</tr>
								<tr>
									<th><label for="clientName">Client Name</label></th>
									<td  colspan="3"><asp:textbox id="txtLabClientName" Runat="server" Width="615px"></asp:textbox></td>
								</tr>
								<tr>
									<th><label for="ClientGuid">Client GUID</label></th>
									<td  colspan="3"><asp:textbox id="txtClientGuid" Runat="server"  Width="503px"></asp:textbox>&nbsp;&nbsp;&nbsp;<asp:Button ID="btnGuid" runat="server" CssClass="button" Text="Make Guid" OnClick="btnGuid_Click" /></td>
								</tr>
								<tr>
									<th><label for="version">Version</label></th>
									<td  colspan="3"><asp:textbox id="txtVersion" Runat="server" Width="615px"></asp:textbox></td>
								</tr>
								<tr>
									<th><label for="shordesc">Short Description </label></th>
									<td colspan="3"><asp:textbox id="txtShortDesc" Runat="server" Width="615px"></asp:textbox></td>
								</tr>
								<tr>
									<th><label for="longdesc">Long Description</label></th>
									<td colspan="3"><asp:textbox id="txtLongDesc" Runat="server" Width="615px" TextMode="MultiLine" Rows="3" Columns="20"></asp:textbox></td>
								</tr>
								<tr>
									<th><label for="contactfirstname">Contact:&nbsp;&nbsp;&nbsp;First Name</label></th>
									<td><asp:textbox id="txtContactFirstName" Runat="server" Width="249px"></asp:textbox></td>
									<th><label for="contactlastname">Last Name</label></th>
									<td><asp:textbox id="txtContactLastName" Runat="server" Width="249px"></asp:textbox></td>
								</tr>
								<tr>
									<th><label for="contactemail">Contact Email</label></th>
									<td colspan="3"><asp:textbox id="txtContactEmail" Runat="server" Width="615px"></asp:textbox></td>
								</tr>
								<tr>
									<th><label for="docurl">Documentation URL</label></th>
									<td colspan="3"><asp:textbox id="txtDocURL" Runat="server" Width="615px"></asp:textbox></td>
								</tr>
								<tr>
									<th><label for="notes">Notes</label></th>
									<td colspan="3"><asp:textbox id="txtNotes" Runat="server" Width="615px" TextMode="MultiLine" Rows="2"></asp:textbox></td>
								</tr>
								<tr>
									<th><label for="clientType">Client Type</label></th>
									<td style="width: 185px"><asp:DropDownList CssClass="i18n" id="ddlClientTypes" Runat="server" Width="249px"></asp:DropDownList></td>
									<th style="height: 26px"><label for="isReentrant">Is Reentrant</label></th>
									<td><asp:checkbox id="cbxIsReentrant" Runat="server" Width="24px"></asp:checkbox></td>
								</tr>
								<tr>
									<th><label for="loaderscript">Loader Script</label></th>
									<td colspan="3"><asp:textbox id="txtLoaderScript" Runat="server" Width="615px" TextMode="MultiLine" Rows="5"></asp:textbox></td>
								</tr>
								<tr>
								    <th>&nbsp;</th>
								    <th align="left"> Associated Services</th>
								</tr>
								<tr id="trLabServer">
									<th><label for="labServer">Lab&nbsp;Server</label></th>
									<td colspan="3" style="height: 26px;"><asp:DropDownList CssClass="i18n" id="ddlLabServer" Runat="server" Width="516px"></asp:DropDownList>
									<asp:textbox id="txtLabServer" Runat="server" Width="516px" CssClass="i18n" ReadOnly="true"></asp:textbox>&nbsp;&nbsp;&nbsp;<asp:Button ID="btnRegisterLS" runat="server" CssClass="button" Width="80px" Text="Register" OnClick="btnRegisterLS_Click"/></td>
								</tr>
								<tr>
									<th><label for="labServerURL">Lab Server URL</label></th>
									<td colspan="3"><asp:textbox id="txtLsUrl" Runat="server" Width="615px" ReadOnly="true"></asp:textbox></td>
								</tr>
								<tr id="trNeedsESS">
									<th><label for="needsExperimentStorage">Needs&nbsp;ESS&nbsp;&nbsp;</label><asp:checkBox id="cbxESS" Runat="server" Width="24px"></asp:checkBox>&nbsp;</th>
									<td colspan="3" style="height: 26px;"><asp:DropDownList CssClass="i18n" id="ddlAssociatedESS" Runat="server" Width="516px"></asp:DropDownList>
									<asp:textbox id="txtAssociatedESS" Runat="server" CssClass="i18n" Width="516px" ReadOnly="true"></asp:textbox>&nbsp;&nbsp;&nbsp;<asp:Button ID="btnRegisterESS" CssClass="button" runat="server"  Width="80px"  Text="Register" OnClick="btnRegisterESS_Click"/></td>
								</tr>
								<tr id="trNeedsUSS">
									<th><label for="needsSchduling">Needs&nbsp;USS&nbsp;&nbsp;</label><asp:checkbox id="cbxScheduling" Runat="server" Width="24px"></asp:checkbox>&nbsp;</th>
									<td colspan="3" style="height: 26px;"><asp:DropDownList CssClass="i18n" id="ddlAssociatedUSS" Runat="server" Width="516px"></asp:DropDownList>
									<asp:textbox id="txtAssociatedUSS" Runat="server" CssClass="i18n" Width="516px" ReadOnly="true"></asp:textbox>&nbsp;&nbsp;&nbsp;<asp:Button ID="btnRegisterUSS" runat="server" CssClass="button" Width="80px" Text="Register" OnClick="btnRegisterUSS_Click"/></td>
								</tr>
								<tr>
								    <td>&nbsp;</td>
									<th colspan="3" align="center">
										<asp:button id="btnSaveChanges" Runat="server" Text="Save Changes" CssClass="button" onclick="btnSaveChanges_Click"></asp:button>
										<asp:button id="btnRemove" Runat="server" CssClass="button" Text="Remove" onclick="btnRemove_Click"></asp:button>
										<asp:button id="btnNew" Runat="server" Text="New" CssClass="button" onclick="btnNew_Click"></asp:button>
									</th>
								</tr>
								<tr id="trOptions" runat="server">
								    <td>&nbsp;</td>
									<th colspan="3" align="center">
                                        <asp:Button ID="btnAssociateGroups" runat="server" CssClass="button" Text="Associate Groups" />
										<asp:button id="btnAddEditResources" Runat="server" Text="Add/Edit Resources" CssClass="button"
											Width="173px" onclick="btnAddEditResources_Click"/>
										<asp:button id="btnMetadata" Runat="server" Text="Edit Metadata" CssClass="button"
											Width="173px" visible="false" onclick="btnMetadata_Click"/>
									</th>
								</tr>
							</table>
						</div>
						<br clear="all" />
						<!-- end pagecontent div -->
					</div>
					<!-- end innerwrapper div -->
				</div>
				<uc1:footer id="Footer1" runat="server"></uc1:footer>
			</div>
		</form>
	</body>
</html>
