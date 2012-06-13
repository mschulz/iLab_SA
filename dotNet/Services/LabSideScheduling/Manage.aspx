<%@ Register TagPrefix="uc1" TagName="NavBar" Src="NavBar.ascx" %>
<%@ Register TagPrefix="uc1" TagName="banner" Src="banner.ascx" %>
<%@ Register TagPrefix="uc1" TagName="footer" Src="footer.ascx" %>

<%@ Page language="c#" Inherits="iLabs.Scheduling.LabSide.Manage" CodeFile="Manage.aspx.cs" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html xmlns="http://www.w3.org/1999/xhtml">
	<head>
		<title>Manage ExperimentInfo</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
		<meta content="C#" name="CODE_LANGUAGE" />
		<meta content="JavaScript" name="vs_defaultClientScript" />
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
		
		<style type="text/css">@import url( css/main.css ); 
		</style>
	</head>
	<body>
	<form id="Form1" method="post" runat="server">
	<div id="outerwrapper">
		<uc1:Banner id="Banner1" runat="server"></uc1:Banner>
		<uc1:NavBar id="NavBar1" runat="server"></uc1:NavBar>
		<br clear="all">
		<div id="innerwrapper">
		<div id="pageintro">
		    <!-- Administer Groups Error message here: <div class="errormessage"><p>Error message here.</p></div> End error message -->
			<p><asp:label id="lblErrorMessage" Runat="server" EnableViewState="False" Visible="False"></asp:label></p>
			<h1>Manage Experiment Information</h1>
			<p>Add, remove, or edit a Experiment below.</p>		
		</div>
		<div id="pagecontent">
						<div id="itemdisplay">
							<h4>Selected Experiment</h4>
							<div class="simpleform">
								<table>
									<tr>
										<th>
											<label for="labClientName">Lab Client Name</label></th>
										<td style="width: 232px"><asp:textbox id="txtLabClientName" Width="100%" Runat="server"></asp:textbox></td>
									</tr>
									<tr>
										<th>
											<label for="labClientVersion">Lab Client Version </label>
										</th>
										<td style="width: 232px"><asp:textbox id="txtLabClientVersion"  Width="100%" Runat="server"></asp:textbox></td>
									</tr>
									<tr>
										<th>
											<label for="labClientGuid">Lab Client GUID </label>
										</th>
										<td style="width: 232px"><asp:textbox id="txtClientGuid"  Width="100%" Runat="server"></asp:textbox></td>
									</tr>
									<tr>
										<th>
											<label for="providerName">Provider Name</label></th>
										<td style="width: 232px"><asp:textbox id="txtProviderName" Width="100%" Runat="server"></asp:textbox></td>
									</tr>
									<tr>
										<th>
											<label for="contactEmail">Contact Email</label></th>
										<td style="width: 232px"><asp:textbox id="txtContactEmail" Width="100%" Runat="server"></asp:textbox></td>
									</tr>
									<tr>
										<th>
											<label for="minimumTime">Minimum Time(min)</label>
										</th>
										
										<td style="width: 232px"><asp:textbox id="txtMinimumTime" Width="100%" Runat="server"></asp:textbox></td>
									</tr>
									<tr>
										<th>
											<label for="prepareTime">Prepare Time(min)</label></th>
										<td style="width: 232px">
											<asp:textbox id="txtPrepareTime" Width="100%" Runat="server"></asp:textbox>
										</td>
									</tr>
									<tr>
										<th>
											<label for="recoverTime">Recover Time(min)</label></th>
										<td style="width: 232px"><asp:textbox id="txtRecoverTime" Width="100%" Runat="server" ></asp:textbox></td>
									</tr>
									<tr>
										<th>
											<label for="earlyArriveTime">Early Arrive Time(min)</label>
										</th>
										
										<td style="width: 232px"><asp:textbox id="txtEarlyArriveTime"  Width="100%" Runat="server" ></asp:textbox></td>
									</tr>
									<tr>
										<th colSpan="2">
											<asp:button id="btnSave"  CssClass="button" Runat="server" Text="Save Changes" onclick="btnSave_Click"></asp:button><asp:button id="btnRemove" Runat="server" Text="Remove" CssClass="button" onclick="btnRemove_Click"></asp:button><asp:button id="btnNew" Runat="server" Text="New" CssClass="button" onclick="btnNew_Click"></asp:button></th></tr>
								</table>
							</div>
						</div>
						<div class="simpleform"><label for="selectexperiment">Select an experiment</label><br>
							<asp:listbox cssClass="i18n" id="lbxSelectExperiment" Runat="server" Width="360px" AutoPostBack="True" Rows="15" onselectedindexchanged="lbxSelectExperiment_SelectedIndexChanged"></asp:listbox></div>
					</div>
					</div>
	<uc1:footer id="Footer1" runat="server"></uc1:footer>
	</div>
	</form>
	</body>
</html>
