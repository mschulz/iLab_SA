<%@ Page language="c#" Inherits="iLabs.Scheduling.LabSide.Register" CodeFile="RegisterGroup.aspx.cs" %>
<%@ Register TagPrefix="uc1" TagName="banner" Src="banner.ascx" %>
<%@ Register TagPrefix="uc1" TagName="NavBar" Src="NavBar.ascx" %>
<%@ Register TagPrefix="uc1" TagName="footer" Src="footer.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Register</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
    <meta content="C#" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
    <style type="text/css">@import url( css/main.css );
		</style>
</head>
<body>
    <div id="outerwrapper">
        <uc1:banner ID="Banner1" runat="server"></uc1:banner>
        <uc1:NavBar ID="NavBar1" runat="server"></uc1:NavBar>
        <br clear="all">
        <div id="innerwrapper">
            <div id="pageintro">
                <h1>
                    Register Group
                </h1>
                <p>
                    Add, remove, or edit a Group below.
                </p>
                <!-- Administer Groups Error message here: <div class="errormessage"><p>Error message here.</p></div> End error message -->
                <p>
                    <asp:Label ID="lblErrorMessage" runat="server" EnableViewState="False" Visible="False"></asp:Label></p>
            </div>
            <div id="pagecontent">
                <div class="simpleform">
                    <form id="Form1" method="post" runat="server">
                        <table>
                            <tr>
                                <th>
                                    <label for="group">
                                        Group</label></th>
                                <td style="width: 454px">
                                    <asp:DropDownList CssClass="i18n" ID="ddlGroup" runat="server" AutoPostBack="True"
                                        OnSelectedIndexChanged="ddlGroup_SelectedIndexChanged" Width="100%">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    <label for="groupName">
                                        Group Name</label></th>
                                <td style="width: 454px">
                                    <asp:TextBox ID="txtGroupName" runat="server" Width="100%"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    <label for="serviceBrokerID">
                                        Service Broker ID</label></th>
                                <td style="width: 454px">
                                    <asp:TextBox ID="txtServiceBrokerID" runat="server" Width="100%"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    <label for="serviceBrokerName">
                                        Service Broker Name</label></th>
                                <td style="width: 454px">
                                    <asp:TextBox ID="txtServiceBrokerName" runat="server" Width="100%"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th colspan="2">
                                    <asp:Button ID="btnSaveChanges" runat="server" Text="Save Changes" CssClass="button"
                                        OnClick="btnSaveChanges_Click"></asp:Button>
                                    <asp:Button ID="btnRemove" runat="server" Text="Remove" CssClass="button" OnClick="btnRemove_Click">
                                    </asp:Button>
                                    <asp:Button ID="btnNew" runat="server" Text="New" CssClass="button" OnClick="btnNew_Click">
                                    </asp:Button>
                                </th>
                            </tr>
                        </table>
                    </form>
                </div>
            </div>
        </div>
        <uc1:footer ID="Footer1" runat="server"></uc1:footer>
    </div>
</body>
</html>
	

	
	
	