<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ServiceBrokerInfo.aspx.cs" Inherits="admin_ServiceBrokerInfo"  EnableEventValidation="false" %>

<%@ Reference Page="~/admin/messages.aspx" %>
<%@ Register TagPrefix="uc1" TagName="banner" Src="../banner.ascx" %>
<%@ Register TagPrefix="uc1" TagName="adminNav" Src="adminNav.ascx" %>
<%@ Register TagPrefix="uc1" TagName="footer" Src="../footer.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>MIT iLab Service Broker - Service Broker Info</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
		<style type="text/css">@import url( ../css/main.css ); 
		</style>
</head>
<body>
    <form id="form1" runat="server">
   <a name="top"></a>
			<div id="outerwrapper">
				<uc1:banner id="Banner1" runat="server"></uc1:banner>
				<uc1:adminNav id="AdminNav1" runat="server"></uc1:adminNav>
				<br clear="all">
				<div id="innerwrapper">
					<div id="pageintro">
						<h1>Service Broker Information
						</h1>
						<p>Associate the Service Broker below.</p>
						<p><asp:label id="lblErrorMessage" Runat="server" Visible="False"></asp:label></p>
						<div class="simpleform">
						<table style="WIDTH: 428px; HEIGHT: 200px" cellSpacing="0" cellPadding="5" border="0">
										<TBODY>
											
											<tr>
												<th style="width: 240px; height: 22px" rowspan="">
													<label for="notes">
                                                        GUID</label></th>
												<td><asp:textbox id="txtAgentGUID" Runat="server" Width="360px" ReadOnly="True"></asp:textbox></td>
											</tr>
											<tr>
												<th style="width: 240px;height: 22px" rowspan="">
													<label for="notes">
                                                        Name</label></th>
												<td><asp:textbox id="txtName" Runat="server" Width="360px" ReadOnly="True"></asp:textbox></td>
											</tr>
											<tr>
												<th style="width: 240px;height: 22px" rowspan="">
													<label for="notes">
                                                        URL</label></th>
												<td><asp:textbox id="txtURL" Runat="server" Width="360px" ReadOnly="True"></asp:textbox></td>
											</tr>
											<tr>
												<th colspan="2" rowspan="">
                                                    <asp:Button ID="btnAssociated" runat="server" CssClass="button" Text="Associate" OnClick="btnAssociated_Click" />
                                                    &nbsp;
													</th></tr>
											</TBODY>
											</table>
											</div>
    </DIV></DIV></DIV>
    </form>
						<uc1:footer id="Footer1" runat="server"></uc1:footer>
  
</body>
</html>
