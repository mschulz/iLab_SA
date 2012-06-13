<%@ Control Language="c#" Inherits="iLabs.LabServer.LabView.LVRemotePanel" CodeFile="LVRemotePanel.ascx.cs"  %>
<div>
<!-- LVRemotePanel -->
<object id="LabVIEWControl" classid="<% =classId %>"
    codebase="<% =codebase %>"
    border="<% =border %>" width="<% =width %>" height="<% =height %>" >
    <param name="server" value="<% =serverURL %>" />
	<param name="LVFPPVINAME" value="<% =viName %>" />
	<param name="REQCTRL" value="<% =hasControl %>" /> 
	<embed src="<% =serverURL  %>/<% =fpProtocol %>" lvfppviname="<% =viName %>" 
	reqctrl="<% =hasControl %>"  width="<% =width %>" height="<% =height %>" type="<% =appMimeType %>" 
	pluginspace="<% =pluginspace %>">
	 </embed> 
</object> 
</div>