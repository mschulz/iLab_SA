<%@ Control Language="c#" Inherits="iLabs.ServiceBroker.iLabSB.userNav" CodeFile="userNav.ascx.cs" %>
<!-- This the main navigation. The buttons that are visible depend on which page the user is on. -->
<!-- *******NOTE - The active page in the nav bar should have <a href="abc" class="topactive"> -->
<!-- The first item in the nav bar should always have <a href="abc" class="first">. -->
<!-- The last item in the nav bar should always have <a href="abc" class="last"> ********* -->
<div id="navbar"><div id="nav">
		<ul class="navlist" id="ulNavList" runat="server">
			<li>
				<a id="aHome" href="home.aspx" runat="server">Home</a>
			</li>
			<li id="liNavlistAdmin" runat="server">
				<a id="aAdministration" href="admin/manageUsers.aspx" runat="server">Administration</a>
			</li>
			<li id="liNavlistServiceAdmin" runat="server">
				<a id="aServiceAdministration" href="admin/adminServices.aspx" runat="server">Service Administration</a>
			</li>
			<li id="liNavlistMyGroups" runat="server">
				<a id="aMyGroups" href="myGroups.aspx" runat="server">My Groups</a>
			</li>
			<li id="liNavlistMyLabs" runat="server">
				<a id="aMyLabs" href="myLabs.aspx" runat="server">My Labs</a>
			</li>
			<li id="liNavlistExperiments" runat="server">
				<a id="aMyExperiments" href="myExperiments.aspx" runat="server">My Experiments</a>
			</li>
			<li id="liNavlistMyAccount" runat="server">
				<a id="aMyAccount" href="myAccount.aspx" runat="server">My Account</a>
			</li>
		</ul>
	</div>
	<!-- end nav div -->
	<div id="nav2">
		<!-- This is where the help and logout buttons go. Log out only appears if the user is logged in. -->
		<ul class="navlist2">
			<li>
				<a id="aHelp" runat="server">Help</a></li>
			<li>
				<asp:linkbutton id="lbtnLogout" Runat="server" onclick="lbtnLogout_Click">Log out</asp:linkbutton></li></ul>
	</div> <!-- end nav2 div -->
</div> <!-- end navbar -->
