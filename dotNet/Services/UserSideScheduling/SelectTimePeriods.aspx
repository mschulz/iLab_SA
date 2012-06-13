<%@ Page language="c#" Inherits="iLabs.Scheduling.UserSide.SelectTimePeriods" CodeFile="SelectTimePeriods.aspx.cs" %>
<%@ Register Assembly="iLabControls" Namespace="iLabs.Controls.Scheduling" TagPrefix="iLab" %>
<%@ Register TagPrefix="uc1" TagName="banner" Src="banner.ascx" %>
<%@ Register TagPrefix="uc1" TagName="footer" Src="footer.ascx" %>
<%  
    Response.Write(cntrScheduling.getDocType());
%>

<html xmlns="http://www.w3.org/1999/xhtml">
	<head>
		<title>Select Time Periods</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
		<meta content="C#" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta http-equiv="X-UA-Compatible" content="IE=Edge"/> 
		<meta name="vs_targetSchema"  content="http://schemas.microsoft.com/intellisense/html-401" />
		<!-- Default styles -->
		<style type="text/css">@import url( css/main.css ); @import url( css/scheduling.css );
		</style>
	
		<!-- Use these styles to override colors for the different scheduling TimeBlock classes 
		<style type="text/css">
		table .day td[class=available] {	background-color:#88ff88;}
        table .day td[class=reserved] {	background-color:Blue; }
        table .day td[class=void] {	background-color:#aaaaaa; }
        </style>
        -->
		<script type="text/javascript">
		<!--
		function ReloadParent() 
        {          
            if (window.opener){            
                window.opener.location.href = 'Reservation.aspx?refresh=t';
                window.opener.focus();
            } 
            window.close();  
        }
        -->
        </script>
	</head>
	<body>
		<form id="Form1" method="post" runat="server">
	<div id="outerwrapper">
	<uc1:Banner id="Banner1" runat="server"></uc1:Banner>
	<br />
	<div id="innerwrapper">
			<div id="pageintro">
                    <h3><asp:label id="lblTitleofSchedule"  Runat="server" 
						text="Available Reservation Times"></asp:label></h3>
					<p>Please select the general starting time of the reservation.</p>
					 <p><asp:Label ID="lblTimezone" runat="server" ></asp:Label></p>
                     <p><asp:Label ID="lblUssPolicy" runat="server" ></asp:Label></p>
					 <p><asp:label id="lblErrorMessage" EnableViewState="False" Visible="False" Runat="server"></asp:label></p>
			</div>
			<div>		
			<table>
			<tr><th>Available Times</th><th style="width:50px;">&nbsp;</th><th style="width:400px;">Reservation Details</th></tr>
			<tr>
			    <td><iLab:SchedulingControl id="cntrScheduling" runat="server" Hours24="false"  ColumnWidth="120" BorderColor="black"  
			      ScheduledColor="red" VoidColor="gray" OnAvailableClick="TimePeriod_Click"></iLab:SchedulingControl></td>
			    <td>&nbsp;</td>
			    <td style="vertical-align:top">
			        <table style="vertical-align:top">
			            <tr style="vertical-align:top">
			            <td colspan="2">
			            <p>Select the general start time period from the availible times on the left.<br />
			            Select the exact starting time from the dropdown list below.<br />
			            Select the duration of the request.<br />
			            Make the reservation by clicking the button.</p>
			            </td>
			            </tr>
			            <tr style="vertical-align:top">
			                <th style="width: 120px">Start Time</th>
					        <td style="width: 200px"><asp:DropDownList ID="ddlSelectTime" runat="server" Width="200px"></asp:DropDownList></td>
				        </tr>
				        <tr style="vertical-align:top">
					        <th>Duration</th>
					        <td><asp:DropDownList ID="ddlDuration" runat="server" Width="120px"></asp:DropDownList>&nbsp;&nbsp;(hh:mm)</td>
				        </tr>
				        <tr style="vertical-align:top">
					        <td style="height: 15px"></td>
					        <td ><asp:button id="btnMakeReservation" runat="server" Text="Make Reservation" CssClass="button" onclick="btnMakeReservation_Click"></asp:button>
                                <asp:Button ID="btnReturnToReservation" runat="server" Text="Close" CssClass="button"  OnClick="btnReturn_Click"></asp:Button>                
                            </td>
				        </tr >
			       </table>	
			    </td>
			</tr>
			</table>
			</div>
			</div>
		<uc1:footer id="Footer1" runat="server"></uc1:footer>
		</div>	
		</form>
	</body>
</html>



