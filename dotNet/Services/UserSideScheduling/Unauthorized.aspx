<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Unauthorized.aspx.cs" Inherits="Unauthorized" %>
<%@ Register TagPrefix="uc1" TagName="banner" Src="banner.ascx" %>
<%@ Register TagPrefix="uc1" TagName="NavBar" Src="NavBar.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Authorization Failed</title>
    <style type="text/css">@import url( css/main.css ); </style>
	<style type="text/css">@import url( css/charu.css ); </style>
</head>
<body>
    <form id="form1" runat="server">
    <uc1:Banner id="Banner1" runat="server"></uc1:Banner>
    <uc1:NavBar id="NavBar1" runat="server"></uc1:NavBar>
    <div style="text-align: left">
        <br />
        <strong><span style="font-size: 24pt"><span style="color: darkred">
            <br />
            Access Forbidden!</span><br />
            <span style="color: #ff0000">You are not authorized to use this resource<br />
                <br />
            </span></span></strong>
    </div>
    </form>
</body>
</html>
