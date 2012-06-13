<%@ Page Language="c#" Inherits="iLabs.Scheduling.UserSide.ReservationManagement"
    CodeFile="ReservationManagement.aspx.cs" %>

<%@ Register TagPrefix="uc1" TagName="banner" Src="banner.ascx" %>
<%@ Register TagPrefix="uc1" TagName="NavBar" Src="NavBar.ascx" %>
<%@ Register TagPrefix="uc1" TagName="footer" Src="footer.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>ReservationManagement</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1" />
    <meta name="CODE_LANGUAGE" content="C#" />
    <meta name="vs_defaultClientScript" content="JavaScript" />
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5" />
    <style type="text/css">@import url( css/main.css ); 
		</style>
</head>
<body>
    <form id="Form1" method="post" runat="server">
        <div id="outerwrapper">
            <uc1:banner ID="Banner1" runat="server"></uc1:banner>
            <uc1:NavBar ID="NavBar1" runat="server"></uc1:NavBar>
            <br clear="all" />
            <div id="innerwrapper">
                <div id="pageintro">
                    <h1>
                        Reservation Management
                    </h1>
                    <asp:Label ID="lblDescription" runat="server" />
                    <asp:Label ID="lblErrorMessage" runat="server" EnableViewState="False" Visible="False"></asp:Label>
                </div>
                <div id="pagecontent">
                    <div class="simpleform">
                        <table>
                            <tr>
                                <th>
                                    <label for="Experiment">
                                        Experiment</label></th>
                                <td colspan="3" style="width: 454px">
                                    <asp:DropDownList CssClass="i18n" ID="ddlExperiment" runat="server" AutoPostBack="True"
                                        Width="100%">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    <label for="Group">
                                        Group</label></th>
                                <td colspan="3" style="width: 454px">
                                    <asp:DropDownList CssClass="i18n" ID="ddlGroup" runat="server" Width="100%">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    <label for="username">
                                        User Name</label></th>
                                <td style="width: 143px">
                                    <asp:TextBox ID="txtUserName" runat="server" Width="100%"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    <label for="time">
                                        Time</label></th>
                                <td colspan="3" style="width: 454px">
                                    <asp:DropDownList CssClass="i18n" ID="ddlTimeIs" runat="server" Width="104px" AutoPostBack="True"
                                        OnSelectedIndexChanged="ddlTimeIs_SelectedIndexChanged">
                                        <asp:ListItem Value="select">Any Time</asp:ListItem>
                                        <asp:ListItem Value="equal">Date</asp:ListItem>
                                        <asp:ListItem Value="before">Before</asp:ListItem>
                                        <asp:ListItem Value="after">After</asp:ListItem>
                                        <asp:ListItem Value="between">Between</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    <asp:Label ID="lblFormat" runat="server" for="format"></asp:Label></th>
                                <td style="width: 143px">
                                    <asp:TextBox ID="txtTime1" runat="server"></asp:TextBox>
                                    <!--input name="textfield" type="text" size="10"-->
                                </td>
                                <td style="width: 78px">
                                    <asp:TextBox ID="txtTime2" runat="server" ReadOnly="true" BackColor="Lavender"></asp:TextBox>
                                    <!--input name="textfield" type="text" class="noneditable" size="10"-->
                                </td>
                                <td style="width: 9px">
                                    <asp:Button ID="btnGo" runat="server" CssClass="button" Text="Go" OnClick="btnGo_Click">
                                    </asp:Button>
                                    <!--input name="Submit" type="submit" class="button" value="Go"-->
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="simpleform">
                        <br>
                        <asp:TextBox ID="txtDisplay" runat="server" Width="720px" Height="288px" TextMode="MultiLine"></asp:TextBox></div>
                </div>
                <br clear="all" />
                <!-- end pagecontent div -->
            </div>
            <!-- end innerwrapper div -->
            <uc1:footer ID="Footer1" runat="server"></uc1:footer>
        </div>
    </form>
</body>
</html>
