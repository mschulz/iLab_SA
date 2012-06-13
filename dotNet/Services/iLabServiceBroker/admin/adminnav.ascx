<%@ Control Language="c#" Inherits="iLabs.ServiceBroker.admin.adminNav" CodeFile="adminNav.ascx.cs" %>
<div id="navbar"><div id="nav">
		<ul class="navlist" id="ulAdminNavList" runat="server">
			<li><a class="first" href="../home.aspx">Home</a></li>
			<li><a id="aMyGroups" runat="server" href="../myGroups.aspx">My Groups</a></li>
			<li><a id="aMyLabs" runat="server" href="../myLabs.aspx">My Labs</a></li>
			<li><a id="aManageServices" runat="server" href="manageServices.aspx">Services &amp; Clients</a></li>
			<li><a id="aManageUsers" href="manageUsers.aspx" runat="server">Users &amp; Groups</a></li>
			<li><a id="aGrants" runat="server" href="grants.aspx">Grants</a></li>
			<li><a id="aMappings" runat="server" href="adminResourceMappings.aspx">Resource Mappings</a></li>
			<li><a id="aExperimentRecords" runat="server" href="experimentRecords.aspx">Records</a></li>
			<li><a id="aMessages" class="last" runat="server" href="messages.aspx">Messages</a></li>
		</ul>
	</div>
	<!-- end nav div -->
	<div id="nav2">
		<ul class="navlist2">
			<li><a id="aHelp" runat="server">Help</a></li>
			<li><asp:LinkButton ID="lbtnLogout" Runat="server" onclick="lbtnLogout_Click">Log out</asp:LinkButton></li>
		</ul>
	</div>
	<!-- end nav2 div -->
	<div id="nav3">
		<ul id="ulNav3Labs" runat="server" class="navlist3">
			<!-- Lab Servers, Lab Clients, and Labs navigation -->
			<li><a id="aNav3ServiceBrokerInfo" runat="server" href="SelfRegistration.aspx">Self Registration</a></li>
			<li><a id="aNav3ManageServices" runat="server" href="manageServices.aspx">Manage Process Agents</a></li>			
			<li><a id="aNav3ManageLabClients" runat="server" href="manageLabClients.aspx">Manage Lab Clients</a></li>
			<li><a id="aNav3ManageLabs" runat="server" href="crossRegistration.aspx">Cross-domain Registration</a></li>
			<li><a id="aNav3Authorities" runat="server" href="authorities.aspx">Manage Authorities</a></li>
		</ul>
		<ul id="ulNav3UsersGroups" runat="server" class="navlist3">
			<!-- Users and Groups navigation -->
			<li><a id="aNav3ManageUsers" runat="server" href="manageUsers.aspx">Manage Users</a></li>
			<li><a id="aNav3AdministerGroups" runat="server" href="administerGroups.aspx">Administer Groups</a></li>
			<li><a id="aNav3GroupMembership" runat="server" href="groupMembership.aspx">Group Membership</a></li>
		</ul>
		<ul id="ulNav3Records" runat="server" class="navlist3">
			<li><a id="aNav3SBinfo" runat="server" href="sbStats.aspx">Service Broker Information</a></li>
			<li><a id="aNav3Reports" runat="server" href="sbReport.aspx">Reports</a></li>
			<li><a id="aNav3ExperimentRecords" runat="server" href="experimentRecords.aspx">Experiments</a></li>
			<li><a id="aNav3LoginRecords" runat="server" href="loginRecords.aspx">Log-ins</a></li>
			<li><a id="aNav3SessionHistory" runat="server" href="sessionHistory.aspx">Session History</a></li>
		</ul>
	</div> <!-- end nav3 div -->
</div> <!-- end navbar -->
