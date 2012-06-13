<%@ Page language="c#" Inherits="iLabs.Scheduling.LabSide.TimeBlockManagement" CodeFile="TimeBlockManagement.aspx.cs" EnableEventValidation="false"%>
<%@ Register TagPrefix="uc1" TagName="banner" Src="banner.ascx" %>
<%@ Register TagPrefix="uc1" TagName="NavBar" Src="NavBar.ascx" %>
<%@ Register TagPrefix="uc1" TagName="footer" Src="footer.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html xmlns="http://www.w3.org/1999/xhtml">
	<head>
		<title>TimeBlockManagement</title>
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
		<br clear="all" />
		<div id="innerwrapper">
		<div id="pageintro">
						<h1>Manage Time Blocks
						</h1>
						<asp:Label ID="lblDescription" runat="server" />
						
						<!-- Administer Groups Error message here: <div class="errormessage"><p>Error message here.</p></div> End error message -->
						<p><asp:label id="lblErrorMessage" Runat="server" EnableViewState="False" Visible="False"></asp:label></p>
					    </div>
					
					
					<div id="pagecontent">	
	
	<div class="simpleform"><label for="selectTimeBlock">Select an time block</label><br>
	<table>
	<tr>
	<th></th>
	<td style="width: 793px">
		 <asp:listbox cssClass="i18n" id="lbxSelectTimeBlock" runat="server" Width="790px" Height="221px" AutoPostBack="True" onselectedindexchanged="lbxSelectTimeBlock_SelectedIndexChanged"></asp:listbox>
		</td>
		</tr>
		<tr>
			<th colspan="2">
				<asp:button id="btnSaveChanges" Runat="server" Text="Save Changes" CssClass="button" onclick="btnSaveChanges_Click" Visible= "false"></asp:button>
				<asp:button id="btnEdit" Runat="server" Text="Edit" CssClass="button" onclick="btnEdit_Click" Visible= "false"></asp:button>
				<asp:button id="btnRemove" Runat="server" Text="Remove" CssClass="button" onclick="btnRemove_Click"></asp:button>
		        <!-- <a href="javascript:;" onclick="window.open('NewTimeBlockPopUp.aspx','NewTimeBlockPopUp','width=900,height=1000,left=270,top=180,resizable=yes,modal=yes').focus()"> -->
		        <asp:button id="btnNew" Runat="server" Text="New" CssClass="button" onclick="btnNew_Click"></asp:button>
			</th>
		</tr>			
	</table>
	</div>
	<div class="simpleform">
	<table>
	<tr>
		<th class="top" style="height: 32px; width: 285px;">
			<label for="permittedExperiments">Select Experiment</label>
		</th>
		<th style="width: 59px; height: 32px">
										&nbsp;
		</th>
		<th class="top" style="height: 32px">
			<label for="selectExperiments">Permitted Experiment(s)</label>
		</th>
		</tr>
		<tr>
			<td style="WIDTH: 285px">
				<asp:listbox cssClass="i18n" id="lbxSelectExperiment" runat="server" Height="132px" Width="280px" ></asp:listbox>
			</td>
			<td class="buttonstyle" style="width: 59px">
				<asp:button CssClass="button" id="btnPermit" runat="server" Width="72px" Text= "Permit" onclick="btnPermit_Click" ></asp:button>
				<asp:button CssClass="button" id="btnUnPermit" runat="server" Width="72px" Text="UnPermit" onclick="btnUnPermit_Click" ></asp:button>
			</td>
			<td style="WIDTH: 300px">
				<asp:listbox cssClass="i18n" id="lbxPermittedExperiments" runat="server" Height="132px" Width="280px" ></asp:listbox>
			</td>
		</tr>
	    <tr>
			<th class="top" style="height: 32px; width: 285px;">
				<label for="permittedGroupss">Select Group</label>
			</th>
			<th style="width: 59px; height: 32px">
			    &nbsp;
			</th>
			<th class="top" style="height: 32px">
				<label for="selectGroups">Permitted Groups(s)</label>
			</th>
		</tr>
		<tr>
		    <td style="WIDTH: 285px">
				<asp:listbox cssClass="i18n" id="lbxSelectGroup" runat="server" Height="132px" Width="280px" ></asp:listbox>
			</td>
			<td class="buttonstyle" style="width: 59px">
				<asp:button CssClass="button" id="btnPermitGroup" runat="server" Width="72px" Text= "Permit" onclick="btnPermitGroup_Click" ></asp:button>
				<asp:button CssClass="button" id="btnUnPermitGroup" runat="server" Width="72px" Text="UnPermit" onclick="btnUnPermitGroup_Click" ></asp:button>
			</td>
			<td style="WIDTH: 300px">
				<asp:listbox cssClass="i18n" id="lbxPermittedGroups" runat="server" Height="132px" Width="280px" ></asp:listbox>					
			</td>
		</tr>        	
	</table>
	<input id="hiddenPopupOnNewTB" type="hidden" name="hiddenPopupOnNewTB" runat="server" /><br />
	</div>
	</div>
	</div>
	</div>
	</form>
		
		<uc1:footer id="Footer1" runat="server"></uc1:footer>
		
					
	</body>
</html>
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
