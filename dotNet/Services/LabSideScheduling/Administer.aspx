<%@ Register TagPrefix="uc1" TagName="NavBar" Src="NavBar.ascx" %>
<%@ Register TagPrefix="uc1" TagName="banner" Src="banner.ascx" %>
<%@ Register TagPrefix="uc1" TagName="footer" Src="footer.ascx" %>
<%@ Page language="c#" Inherits="iLabs.Scheduling.LabSide.Administer" CodeFile="Administer.aspx.cs" EnableEventValidation="true"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html xmlns="http://www.w3.org/1999/xhtml">
	<head>
		<title>RegisterUSS</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1" />
		<meta name="CODE_LANGUAGE" content="C#" />
		<meta name="vs_defaultClientScript" content="JavaScript" />
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5" />
	
		<style type="text/css">@import url( css/main.css ); 
		</style>
	</head>
	<body>
	<div id="outerwrapper">
		<uc1:Banner id="Banner1" runat="server"></uc1:Banner>
		<uc1:NavBar id="NavBar1" runat="server"></uc1:NavBar>
		<br clear="all">
		<div id="innerwrapper">
		<div id="pageintro">
						<h1>Register User Side Scheduling Server
						</h1>
						<p>Add, remove, or edit a User Side Scheduling Server below.
						</p>
						<!-- Administer Groups Error message here: <div class="errormessage"><p>Error message here.</p></div> End error message -->
						<p><asp:label id="lblErrorMessage" Runat="server" EnableViewState="False" Visible="False"></asp:label></p>
					</div>
					
					
					<div id="pagecontent">	
		<div class="simpleform"><form id="Form1" method="post" runat="server">
		<table>
		
		<tr>
										<th>
											<label for="uss">User Side Scheduling Server</label></th>
										<td style="width: 454px"><asp:DropDownList cssClass="i18n" id="ddlUSS" runat="server" AutoPostBack="True" onselectedindexchanged="ddlUSS_SelectedIndexChanged" Width="100%"></asp:DropDownList>
											</td>
									</tr>
		<tr>
										<th>
											<label for="ussID">User Side Scheduling Server ID</label></th>
										<td style="width: 454px"><asp:textbox id="txtUSSID" Runat="server" Width="100%"></asp:textbox>
											</td>
									</tr>
									<tr>
										<th>
											<label for="ussName">User Side Scheduling Server Name</label></th>
										<td style="width: 454px"><asp:textbox id="txtUSSName" Runat="server" Width="100%" ></asp:textbox>
											</td>
									</tr>
									<tr>
										<th>
											<label for="ussURL">User Side Scheduling Server URL</label></th>
										<td style="width: 454px"><asp:textbox id="txtUSSURL" Runat="server" Width="100%"></asp:textbox>
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
		</div>
		<uc1:footer id="Footer1" runat="server"></uc1:footer>
		</div>
					
	</body>
</html>
