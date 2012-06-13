<%@ Control Language="c#" Inherits="iLabs.Scheduling.UserSide.NavBarReg" CodeFile="NavBarReg.ascx.cs" %>
<div id="navbar">
    <div id="nav">
        <ul class="navlist" id="ulNavList" runat="server">    
            <li id="liReservation" runat="server"><a id="aReservation" runat="server"
                href="~/Reservation.aspx?refresh=t">Reservation</a> </li>
        </ul>
    </div>
    <!-- end nav div -->
    <div id="nav2">
        <!-- This is where the help and logout buttons go. Log out only appears if the user is logged in. -->
        <ul class="navlist2">
         <li id="liBackToSB" runat="server"><a id="aBackToSB" runat="server">To ServiceBroker</a> </li>
         <li><a id="aHelp" runat="server">Help</a></li> 
        </ul>
    </div>
    <!-- end nav2 div -->
</div>
<!-- end navbar -->
