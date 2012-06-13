<%@ Control Language="c#" Inherits="iLabs.LabServer.LabView.LVFrame" CodeFile="LVFrame.ascx.cs" %>
<div>
	<iframe src="<% =cgiURL %>?LVFPPVINAME=<% =viName %><% if(path != null){ %>&amp;PATH=<% =path %><% } %>&amp;REQCTRL=<% =hasControl %>&amp;WIDTH=<% =width %>&amp;HEIGHT=<% =height %>"
							WIDTH="<% =fWidth %>" HEIGHT="<% =fHeight %>" scrolling="<% =scroll %>">
							Your Browser does not support iframes. 
	</iframe>
</div>