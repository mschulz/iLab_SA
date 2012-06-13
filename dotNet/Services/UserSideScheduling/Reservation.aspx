<%@ Page language="c#" Inherits="iLabs.Scheduling.UserSide.Reservation" CodeFile="Reservation.aspx.cs"  ValidateRequest="false" %>
<%@ Register TagPrefix="uc1" TagName="banner" Src="banner.ascx" %>
<%@ Register TagPrefix="uc1" TagName="footer" Src="footer.ascx" %>
<%@ Register TagPrefix="uc1" TagName="NavBarReg" Src="NavBarReg.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html xmlns="http://www.w3.org/1999/xhtml">
	<head>
		<title>Reservation</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
		<meta content="C#" name="CODE_LANGUAGE" />
		<meta content="JavaScript" name="vs_defaultClientScript" />
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />	
		<style type="text/css">@import url( css/main.css );
		</style>
	<script  type="text/javascript" language="javascript">
    function PopupReserve(startTime, endTime, w,h)
    {
        var PopupWindow=null;    
        //settings='width='+ w + ',height='+ h + ',location=no, left=270, top=10, directories=no,menubar=no,toolbar=no,status=no,scrollbars=yes,resizable=no,dependent=no,modal =yes';
        settings='width='+ w + ',height='+ h + ',location=no, left=270, top=10, directories=no,menubar=no,toolbar=no,status=no,scrollbars=yes,resizable=yes';
        PopupWindow=window.open('SelectTimePeriods.aspx?start=' + startTime + '&end='+ endTime,'SelectTimePeriods', settings);
        PopupWindow.focus();
    }
	</script>
	</head>
	<body>
	<form id="Form1" method="post" runat="server">
	<div id="outerwrapper">
	<uc1:Banner id="Banner1" runat="server"></uc1:Banner>
	<uc1:NavBarReg id="NavBar1" runat="server"></uc1:NavBarReg>
	<br clear="all"/>
	<div id="innerwrapper">
	<div id="pageintro">
						<h1><asp:label id="lblTitleofSchedule" runat="server"></asp:label>
						</h1>
						<asp:Label ID="lblDescription" runat="server" />
						<!-- Administer Groups Error message here: <div class="errormessage"><p>Error message here.</p></div> End error message -->
						<asp:label id="lblErrorMessage" Runat="server" EnableViewState="False" Visible="False"></asp:label>
					</div>
		<div id="pagecontent">
		<!--< div class="simpleform"> -->
		<table>
		<tr><th align="center">&nbsp;&nbsp;Number of Days&nbsp;&nbsp;</th><th align="center">&nbsp;&nbsp;Start Date&nbsp;&nbsp;</th></tr>
		<tr>
		    <td align="center" valign="top"><asp:DropDownList ID="ddlDays" runat="server">
		        <asp:ListItem Value="1"> 1 </asp:ListItem>
                <asp:ListItem Value="2"> 2 </asp:ListItem>
                <asp:ListItem Value="3"> 3 </asp:ListItem>
                <asp:ListItem Value="4"> 4 </asp:ListItem>
                <asp:ListItem Value="5"> 5 </asp:ListItem>
                <asp:ListItem Value="6"> 6 </asp:ListItem>
                <asp:ListItem Value="7"> 7 </asp:ListItem>
		        </asp:DropDownList>
		    </td>
		    <td align="center" valign="top">
		        <asp:Calendar ID="calDate"   OnSelectionChanged="calDayChanged"   SelectionMode="Day" Runat="server" 
		        TodayDayStyle-BackColor="Aquamarine" TodayDayStyle-BorderColor="Black" TodayDayStyle-BorderStyle="Solid"  TodayDayStyle-BorderWidth="1" />
		    </td>
		    
		</tr>
		</table>
		<!-- </div> -->
		<div class="simpleform">
			<br/>
			<table>
				<tr>
					<th class="colspan"  colspan="2">The reservations you have made for this experiment:
					</th>				
				</tr>
				<tr>
				    <th colspan ="2">
					    <asp:listbox cssClass="i18n" id="lbxReservation"   AutoPostBack="true" OnSelectedIndexChanged="ReservationSelected" runat="server" Width="407px" Height="198px" ></asp:listbox>
				    </th>
				</tr>
				<tr>
				<th colspan = "2">	
					<asp:button ID="btnRedeemReservation" runat="server" CssClass="button" Text="Redeem Reservation" OnClick="btnRedeemReservation_Click"  ></asp:button>
                     <asp:button id="btnRemoveReservation" runat="server" CssClass="button" Text="Remove Reservation" onclick="btnRemoveReservation_Click" ></asp:button>
							
				</th>
				</tr>
				<tr>
					<th colspan = "2">
							
						<asp:button ID="btnBackToSB" runat="server" CssClass="button" OnClick="btnBackToSB_Click"
                                Text="Back To Service Broker" ></asp:button>
                                <input id="hiddenPopupOnMakeRev" type="hidden" name="hiddenPopupOnMakeRev"  onclick="OnMakeReservation_Click"
                                value = "0" runat="server" />
                                <asp:button id="btnReserve" style="VISIBILITY: hidden" runat="server" CssClass="button" Text="Make Reservation" ></asp:button>	
					</th>
				</tr>				
			</table>
			</div>	
			</div>		
		</div>
		<uc1:footer id="Footer1" runat="server"></uc1:footer>
		</div>	
	</form>
	</body>               
</html>
