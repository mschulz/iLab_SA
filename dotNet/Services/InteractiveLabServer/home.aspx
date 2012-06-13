<%@ Page language="c#" Inherits="iLabs.LabServer.LabView.home" CodeFile="home.aspx.cs" %>
<%@ Register TagPrefix="uc1" TagName="banner" Src="banner.ascx" %>
<%@ Register TagPrefix="uc1" TagName="userNav" Src="userNav.ascx" %>
<%@ Register TagPrefix="uc1" TagName="footer" Src="footer.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<HTML>
<!-- 
Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
Please see license.txt in top level directory for full license. 
-->
<!-- $Id$ -->
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
		<form method="post" runat="server">
			<a name="top"></a>
			<div id="outerwrapper">
				<uc1:banner id="Banner1" runat="server"></uc1:banner>
				<uc1:userNav id="UserNav1" runat="server"></uc1:userNav>
				<br clear="all">
				<div id="innerwrapper">
					<div id="pageintro">
						<h1>Welcome to iLab</h1> <!-- Graphic goes here <IMG height="150" alt=" " src="img/homepage-img-ph.gif" width="200"> - End Graphic -->
						<p>iLab is dedicated to the proposition that online laboratories - real 
							laboratories accessed through the Internet - can enrich science and engineering 
							education by greatly expanding the range of experiments that the students are 
							exposed to in the course of their education.
						</p>
						<p>Unlike conventional laboratories, iLabs can be shared across a university or 
							across the world. The iLab vision is to share lab experiments as broadly as 
							possible within higher education and beyond. The ultimate goal of the iLab 
							project is to create a rich set of experiment resources that make it easier for 
							faculty members around the world to share their labs over the Internet.
						</p>
						<ul>
							<li><A href="about.aspx"><strong>Read more about iLab</strong></A></li>
						<!--<li><A href="#"><strong>View Labs</strong></A></li>
							<li><A href="#"><strong>View Groups </strong></A></li>-->
						</ul>
					</div> <!-- end pageintro div -->
					
					<br clear="all"> <!-- end pagecontent div --></div> <!-- end innerwrapper div -->
				<uc1:footer id="Footer1" runat="server"></uc1:footer></div>
		</form>
	</body>
</HTML>
