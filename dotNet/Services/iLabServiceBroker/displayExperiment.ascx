<%@ Control Language="C#" CodeFile="displayExperiment.ascx.cs" Inherits="iLabs.ServiceBroker.iLabSB.displayExperiment" %>
<div>
    <div id="pageintro">
        <h1>
            Show Experiment</h1>
        <p>
            View your experiment records.
        </p>
        <asp:Label ID="lblResponse" runat="server" Visible="False"></asp:Label>
    </div>
    <!-- end pageintro div -->
    <div id="pagecontent">
        <h4>Selected Experiment</h4>
        <div class="simpleform">
            <table class="button">
                <tbody>
                    <tr>
                        <th style="width: 120px">
                            <label for="experimentid">
                                Experiment ID
                            </label>
                        </th>
                        <td colspan="3">
                            <asp:TextBox ID="txtExperimentID" runat="server" ReadOnly="True" Width="100px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            <label for="username">
                                User Name
                            </label>
                        </th>
                        <td style="width: 335px">
                            <asp:TextBox ID="txtUsername" runat="server" ReadOnly="True" Width="330px"></asp:TextBox>
                        </td>
                        <th style="height: 23px">
                            <label for="groupname">
                                Effective Group Name
                            </label>
                        </th>
                        <td style="height: 23px; width: 272px">
                            <asp:TextBox ID="txtGroupName" runat="server" ReadOnly="True" Width="270px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            <label for="labservername">
                                Lab Server Name</label></th>
                        <td style="width: 335px">
                            <asp:TextBox ID="txtLabServerName" runat="server" ReadOnly="True" Width="330px"></asp:TextBox>
                        </td>
                        <th>
                            <label for="labclientname">
                                Lab Client Name</label></th>
                        <td style="width: 272px">
                            <asp:TextBox ID="txtClientName" runat="server" ReadOnly="True" Width="270px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            <label for="subtime">
                                Submission Time
                            </label>
                        </th>
                        <td style="width: 335px">
                            <asp:TextBox ID="txtSubmissionTime" runat="server" ReadOnly="True" Width="330px"></asp:TextBox>
                        </td>
                        <th>
                            <label for="comtime">
                                Completion Time
                            </label>
                        </th>
                        <td style="width: 272px">
                            <asp:TextBox ID="txtCompletionTime" runat="server" ReadOnly="True" Width="270px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            <label for="recordCount">
                                Total Records
                            </label>
                        </th>
                        <td style="width: 335px">
                            <asp:TextBox ID="txtRecordCount" runat="server" ReadOnly="True" Width="330px"></asp:TextBox>
                        </td>
                        <th>
                            <label for="status">
                                Status</label>
                        </th>
                        <td style="width: 272px">
                            <asp:TextBox ID="txtStatus" runat="server" ReadOnly="True" Width="270px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            <label for="annotation">
                                Annotation</label></th>
                        <td colspan="3">
                            <asp:TextBox ID="txtAnnotation" runat="server" Rows="5" TextMode="MultiLine" Width="532px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            &nbsp;
                        </th>
                        <th style="text-align:left; width: 335px;">
                            <asp:Button ID="btnSaveAnnotation" runat="server" Text="Save Annotation" CssClass="button"
                                OnClick="btnSaveAnnotation_Click" ></asp:Button>
                            <asp:Button ID="btnBack" runat="server" Text="Back to Experiments" CssClass="button"
                                OnClick="btnBack_Click" ></asp:Button>
                         </th>
                         <th colspan="2" style="text-align:right">
                            <asp:Button ID="btnDeleteExperiment" runat="server" Text="Delete Experiment" CssClass="button"
                                OnClick="btnDeleteExperiment_Click"></asp:Button>
                        </th>
                    </tr>
                    <tr>
                        <th colspan="4">
                            &nbsp;
                        </th>
                    </tr>
                    <tr>
                        <th>
                            &nbsp;
                        </th>
                        <th style="text-align:left"  colspan="3">
                            <asp:CheckBox ID="cbxContents" runat="server"  TextAlign="Right" Text="Data Only"></asp:CheckBox>
                            &nbsp;&nbsp;
                            <asp:Button ID="btnDisplayRecords" runat="server" Text="Get Records" CssClass="button"
                                OnClick="btnDisplayRecords_Click"></asp:Button>
                        </th>
                    </tr>
                </tbody>
            </table>
        </div>
       
        <div id="divRecords" runat="server" >
            <p>
                &nbsp;</p>
            <h4>
                Experiment Records</h4>
            <asp:TextBox ID="txtExperimentRecords" runat="server" Width="700px" Height="156px"
                TextMode="MultiLine"></asp:TextBox>
            <asp:GridView ID="grvExperimentRecords" runat="server" Width="700px" CellPadding="5"
                AutoGenerateColumns="False" HeaderStyle-Font-Bold="true" BorderColor="black">
                <Columns>
                    <asp:BoundField DataField="Seq_Num" HeaderText="Seq_Num" ReadOnly="True">
                        <HeaderStyle Font-Bold="True" HorizontalAlign="center" Width="80px" Wrap="False" />
                        <ItemStyle HorizontalAlign="Right" Wrap="False" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Record Type" HeaderText="Record Type" ReadOnly="True">
                        <HeaderStyle Font-Bold="True" HorizontalAlign="center" Width="200px" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Contents" HeaderText="Data" ReadOnly="True">
                        <HeaderStyle Font-Bold="True" HorizontalAlign="left" Width="420px" Wrap="True" />
                        <ItemStyle HorizontalAlign="Left" Width="420px" Wrap="True" />
                    </asp:BoundField>
                </Columns>
            </asp:GridView>
        </div>
        <div id="divBlobs" runat="server" visible="false">
            <p>
                &nbsp;</p>
            <h4>
                Experiment BLOBS</h4>
            <asp:GridView ID="grvBlobs" runat="server" CellPadding="5" Width="700px" AutoGenerateColumns="False"
                HeaderStyle-Font-Bold="True" OnRowDataBound="On_BindBlobRow" OnRowCommand="On_BlobSelected"
                HeaderStyle-HorizontalAlign="Center" BorderColor="black">
                <Columns>
                    <asp:TemplateField HeaderText="Select" HeaderStyle-HorizontalAlign="Center">
                        <HeaderStyle Font-Bold="True" HorizontalAlign="Center" Width="80px" Wrap="False" />
                        <ItemStyle HorizontalAlign="Center" />
                        <ItemTemplate>
                            <asp:Button ID="Button1" runat="server" CausesValidation="false" CommandName="" CommandArgument='<%# Eval("Blob_ID") %>'
                                Text='<%# Eval("Blob_ID") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Seq_Num" HeaderText="Seq_Num" ReadOnly="True">
                        <HeaderStyle Font-Bold="True" HorizontalAlign="center" Width="80px" Wrap="False" />
                        <ItemStyle HorizontalAlign="Right" Wrap="False" />
                    </asp:BoundField>
                    <asp:BoundField DataField="MimeType" HeaderText="MimeType" ReadOnly="True">
                        <HeaderStyle Font-Bold="True" HorizontalAlign="center" Width="200px" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Description" HeaderText="Description" ReadOnly="True">
                        <HeaderStyle Font-Bold="True" HorizontalAlign="left" Width="440px" Wrap="True" />
                        <ItemStyle HorizontalAlign="Left" Wrap="true" />
                    </asp:BoundField>
                </Columns>
                <HeaderStyle Font-Bold="True" />
            </asp:GridView>
        </div>
        <p>
            &nbsp;</p>
        <br clear="all" />
    </div>
    <!-- end pagecontent div -->
</div>
