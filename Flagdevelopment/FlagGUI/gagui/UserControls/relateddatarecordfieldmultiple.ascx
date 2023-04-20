<%@ Control Language="c#" AutoEventWireup="false" Inherits="GASystem.RelatedDataRecordFieldMultiple" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<asp:textbox id="DisplayValueTextBox" Columns="25" runat="server" ReadOnly="True"></asp:textbox><A href="javascript:openPickerWindow<%=DisplayValueTextBox.ClientID%>();"><IMG id="CheckNameButton" src="~/gagui/Images/magnify.gif" border="0" runat="server"></A>
<INPUT id="KeyValueHidden" type="hidden" name="KeyValueHidden" runat="server">
<script language="javascript">
<!--
var Fwin= null;

function openPickerWindow<%=DisplayValueTextBox.ClientID%>() {
	
	Fwin = window.open('gagui/WebForms/PickDataRecordMultipleDialog.aspx?ValueControlId=<%=DisplayValueTextBox.ClientID%>&KeyControlId=<%=KeyValueHidden.ClientID%>&DataClass=<%=DataClass%>&DisplayName=<%=DisplayName%>&ownerclass=<%=OwnerClass%>&ownerfield=<%=OwnerField%>', 'pick_dialog', 'toolbar=no, directories=no, location=no, status=no, menubar=no, resizable=yes, scrollbars=yes, width=400, height=600');
	
	if (Fwin && !Fwin.closed)
		Fwin.focus();
}

function setValueAndKey(value, key)
{
	alert("hello!");
}
// -->
</script>
<asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" ErrorMessage="RequiredFieldValidator"
	ControlToValidate="DisplayValueTextBox"></asp:RequiredFieldValidator>
