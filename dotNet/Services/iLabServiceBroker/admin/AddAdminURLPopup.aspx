<%@ Reference Page="~/admin/grants.aspx" %>
<%@ Reference Page="~/requestGroup.aspx" %>
<%@ Page language="c#" Inherits="iLabs.ServiceBroker.admin.addAdminURLPopup" CodeFile="addAdminURLPopup.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" "http://www.w3.org/TR/html4/loose.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
	<head>
		<title>
		Add/Delete Administration URLs
	    </title> 
		<!-- 
Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
Please see license.txt in top level directory for full license. 
-->
		<!-- $Id$ -->
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
		<meta content="C#" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
		<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1"/>
		<link href="../css/main.css" type="text/css" rel="stylesheet"/>
		<link href="../css/popup.css" type="text/css" rel="stylesheet"/>
	</head>
	<body>
		<form id="Form1" method="post" runat="server">
			<div id="wrapper" style="padding-bottom: 15px; padding-top: 15px">
                <br />
				<div id="pageintro">
                    <h1>
                        Add/Delete Domain URLs
                    </h1>
   					<h2>
                        &nbsp;<% = paTag.tag %>&nbsp;
                    </h2>

					<asp:label id="lblResponse" EnableViewState="False" Visible="False" Runat="server"></asp:label></div>
				<div id="pagecontent">					
				</div>
				<!-- End pagecontent div -->
                <strong>Add New URL<br />
                </strong>
                <br clear="all"/>
                <table border="0" cellpadding="0" cellspacing="0" style="padding-right: 10px; padding-bottom: 15px; padding-top: 15px">
                    <tr>
                        <th style="width: 146px; height: 22px; text-align: right;">
                            <label for="name">
                                <span style="font-size: 9pt">Ticket Type &nbsp;</span></label></th>
                        <td style="font-size: 9pt; width: 536px; height: 22px;">
                            <asp:DropDownList  CssClass="i18n" ID="ttDropDownList" runat="server" AutoPostBack="True">
                            </asp:DropDownList></td>
                    </tr>
                    <tr style="font-size: 9pt">
                        <th style="width: 146px; height: 15px; text-align: right;">
                            <label for="description" style="padding-right: 10px">
                                URL</label></th>
                        <td style="width: 536px; height: 15px">
                            <asp:TextBox ID="txtURL" runat="server" Columns="50" Width="470px"></asp:TextBox></td>
                    </tr>
                    <tr style="font-size: 9pt">
                        <th colspan="2" style="height: 15px">
                            <asp:Button ID="btnSaveChanges" runat="server" CssClass="buttonright" OnClick="btnSaveChanges_Click"
                                Text="Save Changes" Width="133px" /></th>
                    </tr>
                </table>
                <br />
                <br />
                <br />
                <strong>&nbsp;&nbsp; &nbsp;Existing URL</strong><br />
                <br />
                <asp:Repeater ID="repAdminURLs" runat="server" OnItemCommand="repAdminURLs_ItemCommand">
                    <ItemTemplate>
							<div class="unit">
								<table border="0" cellspacing="0" cellpadding="0" cols="3" >
										<tr>
											<th width="40">
												Ticket Type
											</th>
											<td width="400">
											<asp:Label ID="Label1" Runat="server"><%# Convert.ToString(DataBinder.Eval(Container,"DataItem.TicketType.Name"))%>
											</asp:Label>
											</td>											
										</tr>
										<tr>
											<th>
												URL</th>
											<td>
												<asp:Label ID="lblUrl" Runat="server"><%# Convert.ToString(DataBinder.Eval(Container,"DataItem.Url"))%>
												</asp:Label>
											</td>
											<td rowspan="2" width="110">
												<asp:Button ID="btnRemove" Runat="server" Text="Remove" CommandName="Remove" CssClass="button"></asp:Button>
											</td>
										</tr>										
								</table>
								</div>
							</ItemTemplate>
                </asp:Repeater>
			</div>
            <br/>
            <strong>
                <br/>
            </strong>
            <br />
		</form>
	</body>
</html>
