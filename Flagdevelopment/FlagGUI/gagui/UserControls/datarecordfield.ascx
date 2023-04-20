<%@ Control Language="c#" AutoEventWireup="false" Inherits="GASystem.DatarecordField" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<asp:Label id="DisplayValueLabel" runat="server">Label</asp:Label>
<asp:LinkButton id="DisplayValueLinkButton" runat="server" Visible="False">LinkButton</asp:LinkButton><INPUT type="hidden" runat="server" id="KeyValueHidden">
<a href="javascript:openPickerWindow();"><IMG src="~/gagui/Images/magnify.gif" runat="server" id="MagnifyButton" border="0"></a>
<script language="javascript">
<!--
var Fwin= null;

function openPickerWindow() {
	
	Fwin = window.open('/DotNetNuke/gagui/WebForms/PickDepartmentDialog.aspx?ValueControlId=<%=DisplayValueLabel.ClientID%>&KeyControlId=<%=KeyValueHidden.ClientID%>', 'pick_dialog', 'toolbar=no, directories=no, location=no, status=no, menubar=no, resizable=yes, scrollbars=yes, width=400, height=600');
	
	if (Fwin && !Fwin.closed)
		Fwin.focus();

	
}

function setValueAndKey(value, key)
{
	alert("hello!");
}
// -->
</script>
