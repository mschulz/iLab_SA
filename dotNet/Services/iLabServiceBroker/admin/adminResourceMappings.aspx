<%@ Page language="c#" Inherits="iLabs.ServiceBroker.admin.adminResourceMappings" CodeFile="adminResourceMappings.aspx.cs" %>
<%@ Register TagPrefix="uc1" TagName="banner" Src="../banner.ascx" %>
<%@ Register TagPrefix="uc1" TagName="adminNav" Src="adminNav.ascx" %>
<%@ Register TagPrefix="uc1" TagName="footer" Src="../footer.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<HTML>
<!-- 
Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
Please see license.txt in top level directory for full license. 
-->
<!-- $Id$ -->
	<HEAD>
		<title>MIT iLab Service Broker - Administer Resource Mappings</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
		<style type="text/css">@import url( ../css/main.css ); 
		</style>
		<script language="JavaScript" type="text/JavaScript">
	<!--
	// Javascript for Alert for Remove button. You'll need to modify it so that it works properly. 

	function confirmDelete()
	{
		if(confirm("Are you sure you want to delete the group?")== true)
			return true;
		else
			return false;
	}
	
	function openPopupWindow(url){
		window.open(url,'addeditgroup','scrollbars=yes,resizable=yes,width=760,height=820');
	}

	//-->
		</script>
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<a name="top"></a><input id="hiddenPopupOnSave" type="hidden" runat="server" NAME="hiddenPopupOnSave">
			<button id="btnRefresh" runat="server" style="VISIBILITY: hidden" type="button"></button>
			<div id="outerwrapper">
				<uc1:banner id="Banner1" runat="server"></uc1:banner>
				<uc1:adminNav id="AdminNav1" runat="server"></uc1:adminNav>
				<br clear="all">
				<div id="innerwrapper">
					<div id="pageintro">
						<h1>Administer Resource Mappings
						</h1>
						<p>Add, remove, or edit a group below.
						</p>
						<!-- Administer Groups Error message here: <div class="errormessage"><p>Error message here.</p></div> End error message -->
						<asp:label id="lblResponse" Runat="server" EnableViewState="False" Visible="False"></asp:label>
					</div>
					<!-- end pageintro div -->
					<div id="pagecontent">											
						<!-- key and values input table -->
						<div class="unit">
						    <table border="0" cellspacing="0" cellpadding="0" cols="2" >
						        <tr>
						            <th width="400" style="height: 41px"><h2>Key</h2></th>
						            <th width="400" style="height: 41px"><h2>Values</h2></th>
						        </tr>
						        <tr>
						            <td width="400">
						                <!-- mapping key entry table -->
						                 <table border="0" cellspacing="0" cellpadding="0" cols="2" >
						                    <tr>
						                        <th width="50" style="height: 71px" valign="top"><h3 style="position: static">New Key Type</h3></th>
						                        <td width="300" style="height: 71px" valign="top">
						                            <asp:dropdownlist  CssClass="i18n" id="keyTypeDropdown" Runat="server" AutoPostBack="True" Width="300px" onselectedindexchanged="keyType_SelectedIndexChanged"></asp:dropdownlist>
						                        </td>
            						        </tr>
			            			        <tr>
			            			            <th width="50" valign="top"><h3 style="position: static">New Key</h3></th>
			            			            <td width="300" valign="top">
						                            <asp:dropdownlist CssClass="i18n" id="key_ProcessAgentDropdown" Runat="server" AutoPostBack="True" Width="300px" onselectedindexchanged="key_ProcessAgent_SelectedIndexChanged"></asp:dropdownlist>
						                            <asp:dropdownlist CssClass="i18n" id="key_ClientDropdown" Runat="server" AutoPostBack="True" Width="300px" onselectedindexchanged="key_Client_SelectedIndexChanged"></asp:dropdownlist>						                        
						                            <asp:dropdownlist CssClass="i18n" id="key_TicketTypeDropdown" Runat="server" AutoPostBack="True" Width="300px" onselectedindexchanged="key_TicketType_SelectedIndexChanged"></asp:dropdownlist>
                                                    <asp:dropdownlist CssClass="i18n" id="key_GroupDropdown" Runat="server" AutoPostBack="True" Width="300px" onselectedindexchanged="key_TicketType_SelectedIndexChanged"></asp:dropdownlist>
						                        </td>
						                    </tr>
						                    <tr></tr>
							             </table>    
									</td>
									<td width="400">
									    <!-- mapping value entry table -->
						                 <table border="0" cellspacing="0" cellpadding="0" cols="2" >
						                    <tr>
						                        <th width="50"><h3 style="position: static">New Value Type</h3></th>
						                        <td width="300">
						                            <asp:dropdownlist  CssClass="i18n" id="valueTypeDropdown" Runat="server" AutoPostBack="True" Width="300px" onselectedindexchanged="valueType_SelectedIndexChanged"></asp:dropdownlist>
						                        </td>
            						        </tr>
			            			        <tr>
			            			            <th width="50"><h3 style="position: static">New Value</h3></th>
			            			            <td width="300">
						                            <asp:dropdownlist CssClass="i18n" id="value_ProcessAgentDropdown" Runat="server" AutoPostBack="True" Width="300px" onselectedindexchanged="value_ProcessAgent_SelectedIndexChanged"></asp:dropdownlist>
						                            <asp:dropdownlist CssClass="i18n" id="value_ClientDropdown" Runat="server" AutoPostBack="True" Width="300px" onselectedindexchanged="value_Client_SelectedIndexChanged"></asp:dropdownlist>
                                                    <asp:dropdownlist CssClass="i18n" id="value_MappingDropdown" Runat="server" AutoPostBack="True" Width="300px" onselectedindexchanged="value_TicketType_SelectedIndexChanged"></asp:dropdownlist>
                                                    <br />
						                            <asp:textbox id="value_StringText" Runat="server" Width="293px"></asp:textbox>
						                            <asp:dropdownlist CssClass="i18n" id="value_TicketTypeDropdown" Runat="server" AutoPostBack="True" Width="300px" onselectedindexchanged="value_TicketType_SelectedIndexChanged"></asp:dropdownlist><asp:dropdownlist id="value_GroupDropdown" Runat="server" AutoPostBack="True" Width="300px" onselectedindexchanged="value_TicketType_SelectedIndexChanged">
                                                    </asp:dropdownlist><br />
                                                    <asp:TextBox ID="value_ResourceTypeText" runat="server" Width="293px"></asp:TextBox></td>						                          
						                    </tr>						                    
						                    <tr>
						                        <td>
						                        </td>
						                        <td>
						                            <asp:button id="btnAddValue" Runat="server" Text="Add Value" CssClass="buttonright"  Width="100px" onclick="btnAddValue_Click"></asp:button>
						                        </td>  
						                    </tr>
						                    <tr>
						                        <td></td>
						                        <td>
                                                    <asp:repeater id="repValues" Runat="server">
							                            <ItemTemplate>
							                                <div class="unit">
								                                <table border="0" cellspacing="0" cellpadding="0" cols="3" >
										                            <tr>
											                            <th width="40">
												                           Type
											                            </th>
											                            <td width="300">
											                                    <asp:Label ID="valueTypeLabel" Runat="server">
											                                        <%# Convert.ToString(DataBinder.Eval(Container,"DataItem.TypeName"))%>
											                                    </asp:Label>    
											                            </td>
											                            <td rowspan="2" width="110">
												                            <asp:Button ID="btnRemoveValue" Runat="server" Text="Remove" CommandName="Remove" CssClass="button"></asp:Button>
											                            </td>
										                            </tr>
										                           <tr>
											                            <th>
												                            Value
											                            </th>
											                            <td width = "300">
											                                    <asp:Label ID="valueLabel" Runat="server"></asp:Label> 
											                            </td>
										                            </tr>
								                                </table>
								                           </div>
							                            </ItemTemplate>
	                            					</asp:repeater>						                        
						                        </td>
						                    </tr>
							             </table>    
                                    </td>
						        </tr>
						        <tr><td colspan="2" align="left"><asp:Button ID="btnAddMapping" Runat="server" Text="Add New Mapping" OnClick="btnAddResource_Click" CssClass="button"></asp:Button></td></tr>
						        <tr>
                                       <td colspan="2"><h2>Resource Mappings</h2></td>
                                </tr>
                                <tr>
                                     <!-- Resource Mapping Repeater -->
                                     <td colspan="2">
                                        <asp:repeater id="repRsrcMappings" Runat="server">
							                 <ItemTemplate>
							                        <div class="unit">
								                           <table border="0" cellspacing="0" cellpadding="0" cols="3" >
								                                <tr>
											                         <th width="40">
												                       Mapping ID
											                          </th>
											                            <td width="300">
											                                    <asp:Label ID="mappingIDLabel" Runat="server">
											                                        <%# Convert.ToString(DataBinder.Eval(Container,"DataItem.MappingID"))%>
											                                    </asp:Label>    
											                            </td>
											                            <td rowspan="2" width="110">
												                            <asp:Button ID="btnRemove" Runat="server" Text="Remove" CommandName="Remove" CssClass="button" OnClick="btnRemove_Click"></asp:Button>
											                            </td>
										                            </tr>
										                            <tr>
											                         <th width="40">
												                       Key Type
											                          </th>
											                            <td width="300">
											                                    <asp:Label ID="keyTypeLabel" Runat="server">
											                                        <%# Convert.ToString(DataBinder.Eval(Container,"DataItem.Key.TypeName"))%>
											                                    </asp:Label>    
											                            </td>											                            
										                            </tr>
										                           <tr>
											                            <th>
												                            Key
											                            </th>
											                            <td width = "300">
											                                    <asp:Label ID="keyLabel" Runat="server"></asp:Label> 											                                    
											                            </td>
										                            </tr>
										                            <tr>
										                            <th>Values</th>
										                            <!-- repeater for values within mapping repeater -->
										                           
										                            <td>
										                            	<div class="unit">
								                                            <table border="0" cellspacing="0" cellpadding="0" cols="3" >
								                                            <asp:repeater id="repValues2" Runat="server">
							                                                    <ItemTemplate>
            										                                 <tr><th width="40">Value Type</th>
			             								                               <td width="300">
											                                             <asp:Label ID="valueTypeLabel2" Runat="server">
											                                                <%# Convert.ToString(DataBinder.Eval(Container,"DataItem.TypeName"))%>
											                                             </asp:Label>    
											                                           </td>											                           
										                                             </tr>
										                                             <tr><th valign="top">Value</th>
											                                           <td width = "300">
											                                             <asp:Label ID="valueLabel2" Runat="server"></asp:Label> 
											                                           </td>
										                                             </tr>
										                                        </ItemTemplate>
	                            					                          </asp:repeater>	
										                            	      </table>
										                            	  </div>										                            										                            	
	                            					                </td>	                            					                
	                            					                </tr>
								                                </table>
								                           </div>
							                  </ItemTemplate>
	                            		</asp:repeater>
                                     </td>
                                </tr>
							</table>
						</div>
					</div>
					<br clear="all"/>
					<!-- end pagecontent div -->
				</div> <!-- end innerwrapper div -->
				<uc1:footer id="Footer1" runat="server"></uc1:footer>
			</div> <!-- end outerwrapper -->
		</form>
	</body>
</HTML>
