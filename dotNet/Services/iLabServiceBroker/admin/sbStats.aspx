<%@ Register TagPrefix="uc1" TagName="footer" Src="../footer.ascx" %>
<%@ Register TagPrefix="uc1" TagName="adminNav" Src="adminNav.ascx" %>
<%@ Register TagPrefix="uc1" TagName="banner" Src="../banner.ascx" %>
<%@ Page language="c#" Inherits="iLabs.ServiceBroker.admin.sbStats" CodeFile="sbStats.aspx.cs" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Service Broker Stats</title>
    <!-- 
Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
Please see license.txt in top level directory for full license. 
-->
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
		<meta content="C#" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
		<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1"/>
		<style type="text/css">@import url( ../css/main.css );
		</style>

</head>
<body>
		<form id="Form1" method="post" runat="server">
			<a name="top"></a>
			<div id="outerwrapper">
				<uc1:banner id="Banner1" runat="server"></uc1:banner>
				<uc1:adminNav id="AdminNav1" runat="server"></uc1:adminNav>
                <br clear="all" />
				<div id="innerwrapper">
					<div id="pageintro">
						<h1>Service Broker Information 	</h1>
						<asp:label id="lblResponse" EnableViewState="False" Visible="False" Runat="server"></asp:label>
					</div>
					<!-- end pageintro div -->
					<div id="pagecontent">
					<table border="0" cellpadding="4">
					     <tr>
                            <td colspan="2"><b><label for="serviceName">Service Name: </label></b><br />
                            <asp:Label ID="txtServiceName" runat="server" Width="496px"></asp:Label></td>
                        </tr>
                        <tr>
                            <td colspan="2"><b><label for="webServiceUrl">Web Service URL: </label></b><br />
                            <asp:Label id="txtWebServiceUrl" runat="server" Width="496px"></asp:Label></td>
                        </tr>
                        <tr>
                            <td colspan="2"><b><label for="agentGuid">Service GUID: </label></b><br />
                            <asp:Label ID="txtServiceGUID" runat="server" Width="360px"></asp:Label></td>
                        </tr>
					    <tr>
                            <td colspan="2"><br /><br /><b>Summary</b><br /><hr /><br /></td>
                        </tr>

                         <tr>
                            <th align="left" valign="top"><label for="ess_list">Experiment Storage Servers (<asp:Label id="txtESSnum" Runat="server"></asp:Label>): </label></th>
                            <td valign="top"><asp:Label id="txtESSlist" Runat="server" ></asp:Label></td>
                        </tr>
                         <tr>
                            <th align="left" valign="top"><label for="uss_list">User-side Scheduling Servers (<asp:Label id="txtUSSnum" Runat="server"></asp:Label>): </label></th>
                            <td><asp:Label id="txtUSSlist" Runat="server" ></asp:Label></td>
                        </tr>
                         <tr>
                            <th align="left" valign="top"><label for="lss_list">Lab-side Scheduling Servers (<asp:Label id="txtLSSnum" Runat="server"></asp:Label>): </label></th>
                            <td valign="top"><asp:Label id="txtLSSlist" Runat="server" Width="344px"></asp:Label></td>
                        </tr>
                         <tr>
                            <th align="left" valign="top"><label for="ls_list">Lab Servers (<asp:Label id="txtLSnum" Runat="server"></asp:Label>): </label></th>
                            <td valign="top"><asp:Label id="txtLSlist" Runat="server" Width="344px"></asp:Label></td>
                        </tr>
                         <tr>
                            <th align="left" valign="top"><label for="ls_list">Batched Lab Servers (<asp:Label id="txtBLSnum" Runat="server"></asp:Label>): </label></th>
                            <td valign="top"><asp:Label id="txtBLSlist" Runat="server" Width="344px"></asp:Label></td>
                        </tr>
                         <tr>
                            <th align="left" valign="top"><label for="ls_list">Remote Service Brokers (<asp:Label id="txtRSBnum" Runat="server"></asp:Label>): </label></th>
                            <td valign="top"><asp:Label id="txtRSBlist" Runat="server" Width="344px"></asp:Label></td>
                        </tr>
						<tr>
							<th align="left" valign="top"><label for="labclient">Lab Clients (<asp:Label id="txtLCnum" Runat="server"></asp:Label>):</label></th>
							<td valign="top"><asp:Label id="txtLClist" Runat="server" Width="496px" ></asp:Label></td>
						</tr>
						<tr>
							<th align="left"><label for="labclient">Users:</label></th>
							<td valign="top">There are (<asp:Label id="txtUsersNum" Runat="server"></asp:Label>) registered users.</td>
						</tr>

						<tr>
                            <td colspan="2"><br /><br /><b>Group Information</b><br /><hr /><br /></td>
                        </tr>
					    <tr>
                            <td colspan="2"><b>Total number of groups:  &nbsp;</b>
                            <asp:Label id="txtGroupNum" Runat="server"></asp:Label> <br />
                            &nbsp;&nbsp; Number of Service Admin groups:  &nbsp;<asp:Label id="txtSGroupNum" Runat="server"></asp:Label> <br />
                            &nbsp;&nbsp; Number of Regular (user) groups:  &nbsp;<asp:Label id="txtRGroupNum" Runat="server"></asp:Label> <br />
                            &nbsp;&nbsp; Number of Cours Staff groups:  &nbsp;<asp:Label id="txtCGroupNum" Runat="server"></asp:Label> <br />
                            <br /> 
                            <asp:repeater id="repUserGroups" Runat="server">
							<ItemTemplate>
								<b><%# Convert.ToString(DataBinder.Eval(Container,"DataItem.gname"))%> &nbsp;</b><br />
								&nbsp;&nbsp;&nbsp; Description:        <%# Convert.ToString(DataBinder.Eval(Container,"DataItem.gdesc"))%> <br />
								&nbsp;&nbsp;&nbsp; Number Users:       <%# Convert.ToString(DataBinder.Eval(Container,"DataItem.usersingroup"))%> <br />
								&nbsp;&nbsp;&nbsp; Associated Clients:  <%# Convert.ToString(DataBinder.Eval(Container,"DataItem.gclients"))%> <br />  
								<br /><br />
							</ItemTemplate>
						    </asp:repeater>
						    </td>
					    </tr>
					    
                    </table>
					</div>
					<!-- end pagecontent div-->
				</div>
				<!-- end innerwrapper div -->
			</div>
		</form>
    

</body>
</html>
