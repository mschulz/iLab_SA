<%@ Control Language="c#" Inherits="iLabs.Scheduling.LabSide.NavBar" CodeFile="NavBar.ascx.cs" %>
<div id="navbar">
    <div id="nav">
        <ul class="navlist" id="ulNavList" runat="server">
            <li id="liAdminister" runat="server"><a id="aAdminister" runat="server" href="~/Administer.aspx">
                Register USS</a> </li>
            <li id="liSelfRegistration" runat="server"><a id="aSelfRegistration" runat="server" href="~/selfRegistration.aspx">
                Self Registration</a> </li>
            <li id="liRegisterGroup" runat="server"><a id="aRegisterGroup" runat="server" href="~/RegisterGroup.aspx">
                Groups</a> </li>
            <li id="liManage" runat="server"><a id="aManage" runat="server" href="~/Manage.aspx">
                Experiments</a> </li>
            <li id="liReservationInfo" runat="server"><a id="aReservationInfo" runat="server"
                href="~/ReservationInfo.aspx">Reservations</a> </li>
            <li id="liRevokeReservation" runat="server"><a id="aRevokeReservation" runat="server"
                href="~/RevokeReservation.aspx">Revoke Reservation</a> </li>
            <li id="liTimeBlockManage" runat="server"><a id="aTimeBlockManage" runat="server"
                href="~/TimeBlockManagement.aspx">TimeBlocks</a> </li>
           
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
