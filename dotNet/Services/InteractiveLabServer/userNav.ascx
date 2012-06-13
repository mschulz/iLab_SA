<%@ Control Language="c#" Inherits="iLabs.LabServer.userNav" CodeFile="userNav.ascx.cs" %>
<!-- This the main navigation. The buttons that are visible depend on which page the user is on. -->
<!-- *******NOTE - The active page in the nav bar should have <a href="abc" class="topactive">.. 
The first item in the nav bar should always have <a href="abc" class="first">.
The last item in the nav bar should always have <a href="abc" class="last"> ********* -->
<div id="navbar"><div id="nav">
		<ul class="navlist" id="ulNavList" runat="server">
			<li>
				<a id="aHome" href="home.aspx" runat="server">Home</a>
			</li>
			<li id="liAdmin" runat="server">
				<a id="aAdminister" href="administer.aspx" runat="server">Administer</a>
			</li>
			<li id="liSelfRegistration" runat="server">
				<a id="aSelfRegistration" href="selfRegistration.aspx" runat="server">Self Registration</a>
			</li>
			<li id="liGroups" runat="server">
				<a id="aGroups" href="localGroups.aspx" runat="server">Groups</a>
			</li>
			<li id="liGroupPermissions" runat="server">
				<a id="aGroupPermissions" href="groupPermissions.aspx" runat="server">Permissions</a>
			</li>
			<li id="liExperiments" runat="server">
				<a id="aExperiments" href="labExperiments.aspx" runat="server">Lab Experiments</a>
			</li>
			<li id="liTasks" runat="server">
				<a id="aTasks" href="manageTasks.aspx" runat="server">Tasks</a>
			</li>
		</ul>
	</div>
	<!-- end nav div -->
	<div id="nav2">
		<!-- This is where the help and logout buttons go. Log out only appears if the user is logged in. -->
		<ul class="navlist2">
		    <li id="liBackToSB" runat="server"><a id="aBackToSB" runat="server">To ServiceBroker</a> </li>
			<li><a id="aHelp" runat="server">Help</a></li>	
		</ul>
	</div> <!-- end nav2 div -->
</div> <!-- end navbar -->
