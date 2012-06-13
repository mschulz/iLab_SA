<%@ Page language="c#" Inherits="iLabs.LabServer.LabView.TestCGI" CodeFile="TestCGI.aspx.cs" %>
<%@ Register TagPrefix="uc1" TagName="lvframe" Src="LVFrame.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" > 

<html>
  <head>
    <title>TestCGI</title>
    <meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" Content="C#">
    <meta name=vs_defaultClientScript content="JavaScript">
    <meta name=vs_targetSchema content="http://schemas.microsoft.com/intellisense/ie5">
  </head>
  <body>
	
    <form id="Form1" method="post" runat="server">
    <p>A simple test page for the LVFrame</p>
    <uc1:lvframe id="theFrame" runat="Server" width="500" fWidth="520" height="450" fHeight="470"></uc1:lvframe>
	<p>after the frame</p>
     </form>
	
  </body>
</html>
