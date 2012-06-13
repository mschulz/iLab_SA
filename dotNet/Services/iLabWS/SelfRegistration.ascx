<%@Control Debug="true" AutoEventWireup="true" Language="c#" Inherits="iLabs.WebService.SelfRegistration" CodeFile="SelfRegistration.ascx.cs"  EnableViewState="true" %>

        <div id="pageintro">
            <h1><asp:Label ID="lblTitle" runat="server">SelfRegistration for </asp:Label><asp:Label ID="lblServiceType" visible="true" runat="server" /></h1>
            <p><asp:Label ID="lblDescription" runat="server">Configure this service's specification.</asp:Label></p>
            <p><asp:Label ID="lblResponse" runat="server" Visible="true"></asp:Label></p>
            <div>
               <asp:CustomValidator ID="valGuid" ControlToValidate="txtServiceGuid" OnServerValidate="checkGuid" 
                    Text="A Guid must be unique and no longer than 50 characters" runat="server"/>
            </div>
             <p><asp:HyperLink id="lnkBackSB" Text="Back to InteractiveSB" runat="server" ></asp:HyperLink></p>
         <!-- end pageintro -->
         <div id="pagecontent">
             <div class="simpleform">
             <form id="frmRegister" action="" method="post"   onload="saveValues">
                    <table  style="WIDTH: 620px" cols="3" cellspacing="0" cellpadding="5" border="0">
                    <tbody>
                        <tr>
                            <% // Set column widths %>
                            <th style="width: 150px; height: 1px" ></th>
                            <td style="width: 380px"></td>
                            <td style="width: 60px"></td>
                        </tr>
                        <tr><th style="text-align:center " colspan="3">Required Credential Information</th></tr>
                        <tr>
                            <th ><label for="serviceName">Service Name</label></th>
                            <td colspan="2"><asp:TextBox ID="txtServiceName" runat="server" Width="496px"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <th><label for="codebaseUrl">Codebase URL</label></th>
                            <td colspan="2"><asp:TextBox ID="txtCodebaseUrl" runat="server" Width="496px"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <th ><label for="webServiceUrl">Web Service URL</label></th>
                            <td colspan="2"><asp:TextBox id="txtWebServiceUrl" runat="server" Width="496px"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <th><label for="agentGuid">Service GUID</label></th>
                            <td><asp:TextBox ID="txtServiceGUID" runat="server" Width="360px"></asp:TextBox></td>
                            <th><asp:Button ID="btnGuid" runat="server" CssClass="button" Text="Create GUID" OnClick="btnGuid_Click"></asp:Button></th>
                        </tr>
                        <tr id="trDomainSB" runat="server">
                            <th ><label for="DomainServer">Domain ServiceBroker</label></th>
                            <td colspan="2"><asp:TextBox ID="txtDomainServer" runat="server" Width="496px" ReadOnly="true"></asp:TextBox></td>
                        </tr>
                        <tr id="trPasskey" runat="server">
                            <th><label for="outpasskey">Install Credential Passkey</label></th>
                            <td colspan="2"><asp:TextBox ID="txtOutPassKey" runat="server" Width="496px" ToolTip="May only be changed by modifying the Web.config file"></asp:TextBox></td>
                        </tr><tr>
                            <th colspan="2">
                                &nbsp;&nbsp;<asp:Button ID="btnSaveChanges" runat="server" CssClass="button" Text="Save Changes"
                                    OnClick="saveChanges"></asp:Button>
                                    <asp:Button ID="btnModifyService" runat="server" CssClass="button" Text="Modify Service"
                                    OnClick="modifyService"></asp:Button>
                                &nbsp;&nbsp;<asp:Button ID="btnRefresh" runat="server" CssClass="button" Text="Refresh"
                                    OnClick="btnRefresh_Click"></asp:Button>
                                &nbsp;&nbsp;<asp:Button ID="btnNew" runat="server" CssClass="button" Text="Clear"
                                    OnClick="btnNew_Click"></asp:Button>
                                     &nbsp;&nbsp;<asp:Button ID="btnRetire" runat="server" CssClass="button" Text="Retire"
                                     OnClientClick="return confirm('Are you sure you want to retire this WebService?\nIf you proceed all references to this site will be retired');" 
                                     OnClick="btnRetire_Click" ></asp:Button>
                            </th>
                            <td></td>
                        </tr>
                        <tr>
                            <th colspan="2">
                            </th>
                        </tr>
                        </tbody>
                </table>
                <asp:HiddenField ID="bakServiceUrl" runat="server" />
                <asp:HiddenField ID="bakServiceName" runat="server" />
                <asp:HiddenField ID="bakCodebase" runat="server" />
                </form>
            </div>
        </div>
    </div>

