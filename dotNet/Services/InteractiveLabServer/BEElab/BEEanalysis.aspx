<%@ Page language="c#" Inherits="iLabs.LabServer.LabView.BEEanalysis" CodeFile="BEEanalysis.aspx.cs"  EnableSessionState="true"%>
<%@ Register TagPrefix="uc1" TagName="banner" Src="../banner.ascx" %>
<%@ Register TagPrefix="uc1" TagName="lvpanel" Src="../LVRemotePanel.ascx" %>
<%@ Register TagPrefix="uc1" TagName="userNav" Src="../labNav.ascx" %>
<%@ Register TagPrefix="uc1" TagName="footer" Src="../footer.ascx" %>

<!DOCTYPE HTML >
<html lang="en">
	<head>
		<title>Building Energy Efficiency Lab</title> 
		<!-- 
Copyright (c) 2012 The Massachusetts Institute of Technology. All rights reserved.
Please see license.txt in top level directory for full license. 
-->
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
		<meta content="C#" name="CODE_LANGUAGE" />
		<meta content="JavaScript" name="vs_defaultClientScript" />
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
		<style type="text/css">@import url( ../css/main.css );
		</style>
	</head>
	<body>
		<form method="post" runat="server">
			<a name="top"></a>
			<div id="outerwrapper">
			<uc1:banner id="Banner1" runat="server" BannerText="Building Energy Lab"></uc1:banner><br clear="all"/>
			<uc1:userNav ID="UserNav1" runat="server" ></uc1:userNav>
				<div id="innerwrapper">
					<div id="pageintro">
						<h1><asp:label id="lblExperimentTitle" Runat="server"><% =title %></asp:label></h1>
						<asp:label id="lblDescription" Runat="server"></asp:label>
						<p id="pResponse" runat="server"><asp:label id="lblResponse" Runat="server" Visible="False"></asp:label></p>
					</div><!-- end pageintro div -->
					<div id="pagecontent">
					    <table style="width: 960px" >
                            <tr>
                                <th style="width: 430px; height: 20px;">Search Experiments</th>
                                <th style="width: 530px; height: 20px;">Experiment Summary</th>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <table>
                                        <tr>
                                            <td>
                                                <div class="simpleform">
                                                    <table cols="4" width="430px">
                                                        <tbody>
                                                            <tr>
                                                                <th>
                                                                    <label for="timeis">
                                                                        Time Range </label>
                                                                </th>
                                                                <td colspan="3">
                                                                    <asp:DropDownList CssClass="i18n" ID="ddlTimeAttribute" runat="server" Width="128px" AutoPostBack="True"
                                                                        OnSelectedIndexChanged="ddlTimeAttribute_SelectedIndexChanged">
                                                                        <asp:ListItem Value="Any Time">Any Time</asp:ListItem>
                                                                        <asp:ListItem Value="Date">Date</asp:ListItem>
                                                                        <asp:ListItem Value="Before">Before</asp:ListItem>
                                                                        <asp:ListItem Value="After">After</asp:ListItem>
                                                                        <asp:ListItem Value="Between">Between</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <th><label for="timeone">
                                                                Time One </label></th>
                                                                <td colspan="3" style="width: 310px">
                                                                    <asp:TextBox ID="txtTime1" runat="server" Width="160px"></asp:TextBox>
                                                                </td>
                                                                
                                                             </tr>
                                                             <tr>
                                                                 <th><label for="timetwo">
                                                                 Time Two </label></th>
                                                                <!-- the following field uses the class "noneditable" if the user does not select between from the drop-down list -->
                                                                <td colspan="3" style="width: 225px">
                                                                    <asp:TextBox ID="txtTime2" runat="server" Width="160px"></asp:TextBox>
                                                                </td>
                                                               
                                                            </tr>
                                                            <tr>
                                                                <th>
                                                                    &nbsp;</th>
                                                                <td colspan="3">
                                                                    <asp:Button ID="btnGo" runat="server" Text="Search Experiments" CssClass="button"
                                                                        OnClick="btnGo_Click"></asp:Button>
                                                                </td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div class="simpleform">
                                                    <label for="selectexperiment">
                                                        Select Experiment</label><br />
                                                    <asp:listbox cssClass="i18n" ID="lbxSelectExperiment" runat="server" Width="430px" Height="156px"
                                                        AutoPostBack="True" OnSelectedIndexChanged="lbxSelectExperiment_SelectedIndexChanged">
                                                    </asp:listbox>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td valign="top">
                                    <div class="simpleform">
                                            <table style="width: 530px;">
                                                    <tr id="trExperimentID" runat="server" visible="true">
                                                        <th style="width: 160px">
                                                            <label for="experimentid">
                                                                Experiment ID</label></th>
                                                        <td style="width: 370px">&nbsp;<asp:TextBox ID="txtExperimentID" runat="server" ReadOnly="True" Width="200px"></asp:TextBox></td>
                                                    </tr>
                                                    <tr>
                                                        <th>
                                                            <label for="labclientname">
                                                                Lab Client Name</label></th>
                                                        <td>
                                                            &nbsp;<asp:TextBox ID="txtClientName" runat="server" ReadOnly="True" Width="360px"></asp:TextBox></td>
                                                    </tr>
                                                    <tr>
                                                        <th>
                                                            <label for="labservername">
                                                                Lab Server Name</label></th>
                                                        <td>
                                                            <asp:TextBox ID="txtLabServerName" runat="server" ReadOnly="True" Width="360px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <th>
                                                            <label for="username">
                                                                User Name
                                                            </label>
                                                        </th>
                                                        <td>
                                                            <asp:TextBox ID="txtUsername" runat="server" ReadOnly="True" Width="360px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <th style="height: 23px">
                                                            <label for="groupname">
                                                                Effective Group Name
                                                            </label>
                                                        </th>
                                                        <td style="height: 23px;">
                                                            <asp:TextBox ID="txtGroupName" runat="server" ReadOnly="True" Width="360px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <th>
                                                            <label for="status">
                                                                Status</label></th>
                                                        <td>
                                                            <asp:TextBox ID="txtStatus" runat="server" ReadOnly="True" Width="360px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <th>
                                                            <label for="subtime">
                                                                Submission Time
                                                            </label>
                                                        </th>
                                                        <td>
                                                            <asp:TextBox ID="txtSubmissionTime" runat="server" ReadOnly="True"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <th>
                                                            <label for="comtime">
                                                                Completion Time
                                                            </label>
                                                        </th>
                                                        <td>
                                                            <asp:TextBox ID="txtCompletionTime" runat="server" ReadOnly="True"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <th>
                                                            <label for="recordCount">
                                                                Total Records
                                                            </label>
                                                        </th>
                                                        <td>
                                                            <asp:TextBox ID="txtRecordCount" runat="server" ReadOnly="True"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <th>
                                                            <label for="annotation">
                                                                Annotation</label></th>
                                                        <td>
                                                            <asp:TextBox ID="txtAnnotation" runat="server" Columns="20" Rows="5" TextMode="MultiLine" Width="360px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr id="trSaveAnnotation" runat="server" visible="false">
                                                        <th colspan="2">
                                                            <asp:Button ID="btnSaveAnnotation" runat="server" Text="Save Annotation" CssClass="buttonright"
                                                                OnClick="btnSaveAnnotation_Click"></asp:Button>
                                                        </th>
                                                    </tr>
                                                    <tr id="trShowExperiment" runat="server" visible="false">
                                                        <th colspan="2">
                                                            <asp:Button ID="btnShowExperiment" runat="server" Text="Display Experiment Data"
                                                                Enabled="true" CssClass="buttonright" OnClick="btnShowExperiment_Click"></asp:Button>
                                                        </th>
                                                    </tr>
                                            </table>
                                    </div>
                                </td>
                            </tr>
                        </table>
					</div><!-- end pagecontent div -->
					<br clear="all" />
				</div><!-- end innerwrapper div -->
				<div><uc1:footer id="Footer1" runat="server"></uc1:footer></div>
				<asp:HiddenField ID="hdnCoupon" runat="server" />
				<asp:HiddenField ID="hdnPasscode" runat="server" />
				<asp:HiddenField ID="hdnIssuer" runat="server" />
				<asp:HiddenField ID="hdnAppkey" runat="server" />
				<asp:HiddenField ID="hdnSbUrl" runat="server" />
			</div>
		</form>
	</body>
</html>
