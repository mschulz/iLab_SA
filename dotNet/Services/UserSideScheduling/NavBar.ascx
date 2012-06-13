<%@ Control Language="c#" Inherits="iLabs.Scheduling.UserSide.NavBar" CodeFile="NavBar.ascx.cs" %>
<div id="navbar">
    <div id="nav">
        <ul class="navlist" id="ulNavList" runat="server">
            <li id="liAdminister" runat="server"><a id="aAdminister" runat="server" href="~/Administer.aspx">
                Register LSS</a> </li>
             <li id="liSelfRegistration" runat="server"><a id="aSelfRegistration" runat="server" href="~/selfRegistration.aspx">
                Self Registration</a> </li>
            <li id="liRegisterExperiment" runat="server"><a id="aRegisterExperiment" runat="server" href="~/RegisterExperimentInfo.aspx">
                Experiments</a> </li>
            <li id="liManage" runat="server"><a id="aManage" runat="server" href="~/Manage.aspx">
                Policy Management</a> </li>
            <li id="liReservationManagement" runat="server"><a id="aReservationManagement" runat="server"
                href="~/ReservationManagement.aspx">Reservation Management</a> </li> 
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
