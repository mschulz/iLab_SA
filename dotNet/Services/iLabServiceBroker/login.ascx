<%@ Control Language="c#" Inherits="iLabs.ServiceBroker.iLabSB.login1" CodeFile="login.ascx.cs" %>
<div class="simpleform">
	<script language="javascript" >
        var visitortime = new Date();
        document.write('<input type="hidden" name="userTZ" id="userTZ"');
        if(visitortime) {
            document.write('value="' + -visitortime.getTimezoneOffset() + '">');
        }
        else {
            document.write('value="JavaScript not Date() enabled">');
        }
    </script>
	<table>
		<tr>
			<th colspan="2">
				<!-- Error message for login-->
				<asp:label id="lblLoginErrorMessage" Runat="server" Visible="False"></asp:label>
			</th>
		</tr>
		<tr>
			<th>
				<label for="username">Username</label></th>
			<td><asp:textbox id="txtUsername" Runat="server"></asp:textbox>
				<asp:RegularExpressionValidator ControlToValidate="txtUsername" ValidationExpression="{1,40}" ErrorMessage="Username cannot be longer than 40 characters"></asp:RegularExpressionValidator>
			</td>
		</tr>
		<tr>
			<th>
				<label for="password">Password</label></th>
			<td><asp:textbox id="txtPassword" Runat="server" TextMode="Password"></asp:textbox></td>
		</tr>
		<tr>
			<th colSpan="2">
				<asp:button id="btnLogIn" runat="server" Text="Log in" cssclass="buttonright" onclick="btnLogIn_Click"></asp:button>
			</th>
		</tr>
	</table>
</div>
