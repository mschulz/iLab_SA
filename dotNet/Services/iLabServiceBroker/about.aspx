<%@ Register TagPrefix="uc1" TagName="banner" Src="banner.ascx" %>
<%@ Register TagPrefix="uc1" TagName="userNav" Src="userNav.ascx" %>
<%@ Register TagPrefix="uc1" TagName="footer" Src="footer.ascx" %>
<%@ Page language="c#" Inherits="iLabs.ServiceBroker.iLabSB.about" CodeFile="about.aspx.cs" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<HTML>
	<HEAD>
		<title>MIT iLab Service Broker - Home</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
		<style type="text/css">@import url( css/main.css ); 
		</style>
	</HEAD>
	<body>
		<form id="home" method="post" runat="server">
			<a name="top"></a>
			<div id="outerwrapper">
				<uc1:banner id="Banner1" runat="server"></uc1:banner>
				<uc1:userNav id="UserNav1" runat="server"></uc1:userNav>
				<br clear="all">
				<div id="innerwrapper">
					<div id="pageintro"><!-- Introductory text goes here -->
					<h2>About iLabs</h2>
					</div><!-- end pageintro div -->
					<div id="pagecontent"><!-- main content goes here -->
					<P><B><I>iLabs</I></B> is dedicated to the proposition that online laboratories 
- real laboratories accessed through the Internet - can enrich science and 
engineering education by greatly expanding the range of experiments that 
students are exposed to in the course of their education. Unlike conventional 
laboratories, iLabs can be shared across a university or across the world.  The 
iLabs vision is to share expensive equipment and educational materials 
associated with lab experiments as broadly as possible within higher education 
and beyond.</P>
<P>iLab teams have created remote laboratories at MIT in microelectronics, 
chemical engineering, polymer crystallization, structural engineering, and 
signal processing as case studies for understanding the complex requirements of 
operating remote lab experiments and scaling their use to large groups of 
students at MIT and around the world.</P>
<P>Based on the experiences of the different iLab development teams, <i>The iLabs 
Project</i> is developing a suite of software tools that makes it efficient to bring 
online complex laboratory experiments, and provides the infrastructure for user management. The <A 
href="http://icampus.mit.edu/ilabs/architecture">iLabs Shared Architecture</A> has the following 
design goals:</P>
<ul style="MARGIN-TOP: 0in; MARGIN-BOTTOM: 0in" type=disc>
<li>Minimize development and management effort for users and providers of remote 
labs.</li> 
<li>Provide a common set of services and development tools.</li> 
<li>Scale to large numbers of users worldwide.</li>
<li>Allow multiple universities with diverse network infrastructures to share remote labs.</li> 
</ul>	
						<br clear="all">
					</div><!-- end pagecontent div -->
				</div><!-- end innerwrapper div -->
				<uc1:footer id="Footer1" runat="server"></uc1:footer>
			</div><!-- End outterwrapper -->
		</form>
	</body>
</HTML>
