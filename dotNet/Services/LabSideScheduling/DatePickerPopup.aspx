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
		<script type="text/javascript">
		<!--
		 function setDate(type,targetDate) {
            if(type== 'start')
                window.opener.document.forms[0].txtStartDate.value = targetDate;
            if(type== 'end')
                window.opener.document.forms[0].txtEndDate.value = targetDate;
          }
          -->
          </script>
	</head>
	<body>
		<form id="Form1" method="post" runat="server">
			<asp:Calendar ID="calDate" OnSelectionChanged="popupDate_changed" Runat="server" />
		</form>
	</body>
</html>
