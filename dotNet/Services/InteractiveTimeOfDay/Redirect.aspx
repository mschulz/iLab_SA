<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Redirect.aspx.cs" Inherits="iLabs.LabServer.TimeOfDay.Redirect" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="text-align: left">
        <asp:Button ID="btnGetTimeOfDay" runat="server" OnClick="btnGetTimeOfDay_Click" Style="z-index: 100;
            left: 39px; position: relative; top: 119px" Text="Get Time Of Day" />
        <br />
        <strong><span style="font-size: 24pt">Interactive Time Of Day Client</span></strong><br />
        <br />
        <asp:HyperLink ID="HyperLinkSB" runat="server" Font-Bold="True"><< Back To SB</asp:HyperLink><br />
        <asp:Label ID="lblGetTimeOfDay" runat="server" Style="z-index: 101; left: 50px;
            position: absolute; top: 169px" Width="358px"></asp:Label>
        <asp:Label ID="Label1" runat="server" Font-Size="Smaller" Style="z-index: 105; left: 190px;
            position: absolute; top: 452px"></asp:Label>
        <asp:Image ID="Image1" runat="server" Style="z-index: 103; left: 53px; position: absolute;
            top: 259px" Visible="False" />
        <asp:Button ID="btnRequestBlobAccess" runat="server" OnClick="btnRequestBlobAccess_Click"
            Style="z-index: 104; left: 48px; position: absolute; top: 209px" Text="Request Blob Access"
            Width="144px" />
        &nbsp;
    
    </div>
    </form>
</body>
</html>
