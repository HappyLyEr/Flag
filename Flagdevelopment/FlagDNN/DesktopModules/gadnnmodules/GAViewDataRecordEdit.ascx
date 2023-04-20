<%@ Control Language="c#" AutoEventWireup="false" Codebehind="GAViewDataRecordEdit.ascx.cs" Inherits="gadnnmodules.GAViewDataRecordEdit" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<table>
	<tr>
		<td><asp:Label id="Label2" runat="server">Class</asp:Label></td>
		<td><asp:TextBox id="txtClass" runat="server"></asp:TextBox></td>
	</tr>
	<tr>
		<td><asp:Label id="Label3" runat="server">RowId</asp:Label></td>
		<td><asp:TextBox id="txtRowId" runat="server"></asp:TextBox></td>
	</tr>
	<tr>
		<td></td>
		<td><asp:Button id="Button1" runat="server" Text="Save"></asp:Button><asp:Label id="Label1" runat="server"></asp:Label></td>
	</tr>
</table>
