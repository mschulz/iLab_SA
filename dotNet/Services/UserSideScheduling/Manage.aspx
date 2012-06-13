<%@ Page language="c#" Inherits="iLabs.Scheduling.UserSide.Manage" CodeFile="Manage.aspx.cs" %>

<%@ Register TagPrefix="uc1" TagName="banner" Src="banner.ascx" %>
<%@ Register TagPrefix="uc1" TagName="NavBar" Src="NavBar.ascx" %>
<%@ Register TagPrefix="uc1" TagName="footer" Src="footer.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html xmlns="http://www.w3.org/1999/xhtml">
	<head>
		<title>Policy Management</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
		<meta content="C#" name="CODE_LANGUAGE" />
		<meta content="JavaScript" name="vs_defaultClientScript" />
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
		
		<style type="text/css">@import url( css/main.css ); 
		</style>
	</head>
	<body>
	<div id="outerwrapper">
		<uc1:Banner id="Banner1" runat="server"></uc1:Banner>
		<uc1:NavBar id="NavBar1" runat="server"></uc1:NavBar>
		<br clear="all" />
		<div id="innerwrapper">
		<div id="pageintro">
						<h1>Policy Management
						</h1>
						<p>Add, remove, or edit a policy below.
						</p>
						<!-- Administer Groups Error message here: <div class="errormessage"><p>Error message here.</p></div> End error message -->
						<p><asp:label id="lblErrorMessage" Runat="server" EnableViewState="False" Visible="False"></asp:label></p>
					</div>
		<div id="pagecontent">	
		<div class="simpleform"><form id="Form1" method="post" runat="server">
		<table>
		<tr>
										<th style="width: 151px">
											<label for="policyfor">Policy For</label></th>
										<td style="width: 454px"><asp:dropdownlist cssClass="i18n" id="ddlPolicy"  Runat="server" AutoPostBack="True" onselectedindexchanged="ddlPolicy_SelectedIndexChanged" Width="100%"></asp:dropdownlist>
											</td>
									</tr>
		<tr>
										<th style="width: 151px">
											<label for="experiment">Experiment</label></th>
										<td style="width: 454px"><asp:dropdownlist cssClass="i18n" id="ddlExperiment"  Runat="server" Width="100%"></asp:dropdownlist>
											</td>
									</tr>
									<tr>
										<th style="width: 151px">
											<label for="group">Group</label></th>
										<td style="width: 454px"><asp:dropdownlist cssClass="i18n" id="ddlGroup" Runat="server" Width="100%" ></asp:dropdownlist>
											</td>
									</tr>
									<tr>
										<th style="width: 151px">
										<asp:label  ID="lblfield3" runat="server" Font-Size="Smaller">Minimum time required</asp:label></th>
											
										<td style="width: 454px"><asp:textbox id="txtMinReservableTimeSlots" Runat="server" Width="100%"></asp:textbox>
											</td>
									</tr>
									<tr>
										<th style="width: 151px">
											<asp:label  ID="lblfield2" runat="server" Font-Size="Smaller">Maximum reservable time</asp:label>
											</th>
										<td style="width: 454px"><asp:textbox id="txtMaxReservableTimeSlots" Runat="server" Width="100%"></asp:textbox>
											</td>
									</tr>
									
									<tr>
										<th colspan="2">
											<asp:button id="btnSaveChanges" Runat="server" Text="Save Changes" CssClass="button" onclick="btnSaveChanges_Click"></asp:button>
											<asp:button id="btnRemove" Runat="server" Text="Remove" CssClass="button" onclick="btnRemove_Click"></asp:button>
											<asp:button id="btnNew" Runat="server" Text="New" CssClass="button" onclick="btnNew_Click"></asp:button>
											</th>
									</tr>	
									</table>
			
		
			</form>
			</div>
			</div>
					<br clear="all"/>
					<!-- end pagecontent div -->
				</div> <!-- end innerwrapper div -->
			
				<uc1:footer id="Footer1" runat="server"></uc1:footer>
			</div>
	</body>
</html>
		



	
