<%@ Register TagPrefix="uc1" TagName="footer" Src="../footer.ascx" %>
<%@ Register TagPrefix="uc1" TagName="userNav" Src="../userNav.ascx" %>
<%@ Register TagPrefix="uc1" TagName="banner" Src="../banner.ascx" %>
<%@ Page language="c#" Inherits="iLabs.ServiceBroker.admin.adminServices" CodeFile="adminServices.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Administer Services</title>
    <style type="text/css"> @import url( ../css/main.css );  </style>
</head>
<body>
    <form id="form1" runat="server">
        <a name="top"></a>
			<div id="outerwrapper">
				<uc1:banner id="Banner1" runat="server"></uc1:banner>
				<uc1:userNav id="UserNav1" runat="server"></uc1:userNav>
				<br clear="all" />
				<div id="innerwrapper">
					<div id="pageintro">
						<h1>Administer Services</h1>
						<!-- <p>Select the group(s) you'd like to join below.</p> -->
						<!-- Errormessage-->
						<asp:Label Runat="server" id="lblResponse" Visible="False"></asp:Label>
                        <br />
                        <!--End error message --></div>
					<!-- end pageintro div -->
					<div id="pagecontent">
					</div>
                     <asp:DropDownList CssClass="i18n" ID="paDropDownList" Runat="server" AutoPostBack="True" OnSelectedIndexChanged="paDropDownList_SelectedIndexChanged">
                    </asp:DropDownList><br/>
                    <br />
                    <asp:Repeater ID="repAdminGrants" runat="server" OnItemCommand="repAdminGrants_ItemCommand">
                        <ItemTemplate>
                            <div class="unit">
                                <table border="0" cellpadding="0" cellspacing="0" cols="2">
                                    <tr>
                                        <td width="400">
                                            <asp:Label ID="lblAdmin" Runat="server"></asp:Label>
                                        </td>
                                        <td rowspan="2" width="180">
                                            <asp:Button ID="btnAdmin" runat="server" CommandName="Redirect" CssClass="button" Text="" width="150"/>
                                        </td>
                                    </tr>                                                                     
                                </table>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                    <br />
					<!-- end pagecontent div -->
				</div>
				<!-- end innerwrapper div -->
				<uc1:footer id="Footer1" runat="server"></uc1:footer>
			</div>
    </form>
</body>
</html>
