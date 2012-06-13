<%@ Page Language="C#" AutoEventWireup="true" CodeFile="NewTimeBlockPopUp.aspx.cs" Inherits="iLabs.Scheduling.LabSide.NewTimeBlockPopUp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Create Recurring Time Blocks</title>
<script language="javascript" type="text/javascript">
<!--

function DIV1_onclick() {

}

// -->
</script>
<style type="text/css">@import url( css/main.css ); 
		</style>
</head>
<body> 
    <form id="form1" runat="server">
    <div id="outerwrapper">
    <div id="innerwrapper">
    <div id="pageintro">
    <asp:label id="lblErrorMessage" EnableViewState="False" Visible="False" Runat="server"></asp:label>
    <h1>Create Recurring Permission Periods</h1>
    <p>Create recurring reservation blocks for the specified date span. 
    Start dates begin at 12:00AM using LSS local time [ UTC  <%= LocalTZ %> ], end dates include the the entire day until midnight.</p>
    <p>Single Block creates a continuous block for the entire period using the dates and specified start and stop times.<br /> 
    Daily creates a block for each day in the date span starting at the specified time, the end time may be up to 24 hours after the start time.<br /> 
    Weekly creates time blocks for the specified days of the week in the date span using the time specified. Weekly periods may have a duration that extends beyond a 24 hour period as long as the specified time does not conflict with other weekly days.</p>
    <p>A new recurrence is checked for any conflicts with existing recurrences. Reservations that already exist should not be affected</p>
    </div>
    <div id="pagecontent">
    <div style="height: 287px; width: 906px;" id="DIV1" language="javascript" onclick="return DIV1_onclick()">
    <table border="0" style="width: 900px" >
    <tr>
      <td style="width: 158px; height: 50px;"><asp:Label CssClass = "label" ID="Label6" runat="server" Text="Lab Server Resource" TabIndex="15"></asp:Label></td>
      <td colspan="4" style="height: 50px" align="left" valign="middle"><asp:DropDownList cssClass="i18n" ID="ddlLabServers" runat="server" Width="544px">
        </asp:DropDownList><br />
      </td>
    </tr>
     <tr>
      <td style="width: 158px; height: 50px;"><asp:Label ID="Label1" runat="server" Text="Recurrence Type" CssClass = "label" TabIndex="15"></asp:Label></td>
      <td colspan="4" style="height: 50px">
        <asp:DropDownList cssClass="i18n" ID="ddlRecurrence" runat="server" AutoPostBack="True" TabIndex="3" Width="235px" >
            <asp:ListItem Value="0">-- Select Recurrence Type--</asp:ListItem>
            <asp:ListItem Value="1">Single Block</asp:ListItem>
            <asp:ListItem Value="2">Daily</asp:ListItem>
             <asp:ListItem Value="3">Weekly</asp:ListItem>
        </asp:DropDownList><br />
      </td>
    </tr>
    <tr>
     <td style="width: 158px; height: 50px;"><asp:Label ID="Label7" runat="server" Text="Date Span" CssClass = "label" TabIndex="15"></asp:Label></td> 
      <td style="width: 30px; height: 50px;"><asp:Label ID="Label4" runat="server" Text="Begin&nbsp;Date" CssClass = "label" TabIndex="15"></asp:Label></td>     
      <td style="width: 150px; height: 50px;"><asp:TextBox ID="txtStartDate" runat="server" Width="74px" TabIndex="1"></asp:TextBox>
        <a href="javascript:;" onclick="window.open('datePickerPopup.aspx?date=start','cal','width=280,height=220,left=550,top=250')">
	    <img src="calendar.gif" border="0" class="normal" /></a></td>
      <td style="width: 30px; height: 50px;"> <asp:Label ID="Label5" runat="server" Text="End&nbsp;Date" CssClass = "label" TabIndex="15"></asp:Label></td>
      <td style="height: 50px; width: 150px;" valign="middle"><asp:TextBox ID="txtEndDate" runat="server" Width="74px" TabIndex="2"></asp:TextBox>
        <a href="javascript:;" onclick="window.open('datePickerPopup.aspx?date=end','cal','width=280,height=220,left=550,top=300')">
		<img src="calendar.gif" border="0" class="normal" /></a><br />
      </td>
    </tr>
   <tr>
   <td style="width: 158px; height: 50px;"><asp:Label ID="Label9" runat="server" Text="Quantum" CssClass = "label" TabIndex="15"></asp:Label>
   </td>
   <td colspan="4" ><asp:TextBox ID="txtQuantum" runat="server" Width="120px"></asp:TextBox></td>
   </tr>
    <tr>
    <td style="width: 158px; height: 50px;"><asp:Label ID="Label8" runat="server" Text="Time Each Day" CssClass = "label" TabIndex="15"></asp:Label></td> 
      <td style="width: 30px; height: 50px;"> <asp:Label ID="Label2" runat="server" Text="Start&nbsp;Time" CssClass = "label" TabIndex="15"></asp:Label></td>
      <td style="width: 150px; height: 50px;"><asp:DropDownList cssClass="i18n" ID="ddlStartHour" runat="server"
            Width="55px" TabIndex="4">
            <asp:ListItem>12</asp:ListItem>
            <asp:ListItem>1</asp:ListItem>
            <asp:ListItem>2</asp:ListItem>
            <asp:ListItem>3</asp:ListItem>
            <asp:ListItem>4</asp:ListItem>
            <asp:ListItem>5</asp:ListItem>
            <asp:ListItem>6</asp:ListItem>
            <asp:ListItem>7</asp:ListItem>
            <asp:ListItem>8</asp:ListItem>
            <asp:ListItem>9</asp:ListItem>
            <asp:ListItem>10</asp:ListItem>
            <asp:ListItem>11</asp:ListItem>
        </asp:DropDownList>&nbsp;:&nbsp;<asp:TextBox ID="txtStartMin" runat="server" 
        Width="25px" TabIndex="5">00</asp:TextBox>&nbsp;<asp:DropDownList cssClass="i18n" ID="ddlStartAM" runat="server" TabIndex="6" Width="50px">
            <asp:ListItem>AM</asp:ListItem>
            <asp:ListItem>PM</asp:ListItem>
        </asp:DropDownList>
      </td>
      <td style="width: 30px; height: 50px;"><asp:Label ID="Label3" runat="server" Text="End&nbsp;Time" CssClass = "label" TabIndex="15"></asp:Label></td>
      <td style="width: 150px; height: 50px;"><asp:DropDownList cssClass="i18n" ID="ddlEndHour" runat="server"
            Width="55px" TabIndex="7">
            <asp:ListItem>12</asp:ListItem>
            <asp:ListItem>1</asp:ListItem>
            <asp:ListItem>2</asp:ListItem>
            <asp:ListItem>3</asp:ListItem>
            <asp:ListItem>4</asp:ListItem>
            <asp:ListItem>5</asp:ListItem>
            <asp:ListItem>6</asp:ListItem>
            <asp:ListItem>7</asp:ListItem>
            <asp:ListItem>8</asp:ListItem>
            <asp:ListItem>9</asp:ListItem>
            <asp:ListItem>10</asp:ListItem>
            <asp:ListItem>11</asp:ListItem>
        </asp:DropDownList>&nbsp;:&nbsp;<asp:TextBox ID="txtEndMin" runat="server" 
        Width="25px" TabIndex="8">00</asp:TextBox>&nbsp;<asp:DropDownList cssClass="i18n" ID="ddlEndAM" runat="server" TabIndex="9" Width="50px">
            <asp:ListItem>AM</asp:ListItem>
            <asp:ListItem>PM</asp:ListItem>
        </asp:DropDownList>
    </td>
    </tr>
    <tr>
      <td style="width: 158px; height: 50px;"><asp:Label ID="lblRecur" runat="server" Text="Recur every week on" CssClass = "label" TabIndex="15"></asp:Label></td>
      <td colspan="4" style="height: 50px"><asp:CheckBoxList ID="cbxRecurWeekly" runat="server" RepeatDirection="Horizontal" Font-Size="Smaller" TabIndex="10">
            <asp:ListItem>Sunday</asp:ListItem>
            <asp:ListItem>Monday</asp:ListItem>
            <asp:ListItem>Tuesday</asp:ListItem>
            <asp:ListItem>Wednesday</asp:ListItem>
            <asp:ListItem>Thursday</asp:ListItem>
            <asp:ListItem>Friday</asp:ListItem>
            <asp:ListItem>Saturday</asp:ListItem>
        </asp:CheckBoxList>
      </td>
    </tr>
    <tr>
      <td style="width: 158px; height: 50px;">&nbsp;</td>
      <td colspan="4" align="center" style="height: 50px"><asp:Button ID="btnSave" runat="server" Text="Save" CssClass="button" Width="88px" OnClick="btnSave_Click" TabIndex="11" /><br /></td>
    </tr>
  </table>        
    </div>
    </div>
    </div>
    </div>
    </form>
</body>
</html>
