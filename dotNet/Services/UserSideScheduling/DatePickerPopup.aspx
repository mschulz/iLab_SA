<%@ Page language="c#" Inherits="JavaScriptPopups.datePickerPopup" CodeFile="datePickerPopup.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html xmlns="http://www.w3.org/1999/xhtml">
<!-- 
Author...: Chris Felknor / cf@chrisfelknor.com
Date.....: 3/12/2005
--> 
	<head>
		<title>datePickerPopup</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1" />
		<meta name="CODE_LANGUAGE" content="C#" />
		<meta name="vs_defaultClientScript" content="JavaScript" />
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5" />
	</head>
	<body>
		<form id="Form1" method="post" runat="server">
			<asp:Calendar ID="calDate" OnSelectionChanged="popupDate_changed"  SelectionMode="DayWeek"  Runat="server" />
		</form>
	</body>
</html>
