<%@ Control Language="c#" AutoEventWireup="false" Inherits="GASystem.RelatedDataRecordField" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<asp:textbox id="DisplayValueTextBox" EnableViewState="false" Columns="25" runat="server" ReadOnly="true" CssClass="LookupDisplay"></asp:textbox><A href="javascript:openPickerWindow<%=DisplayValueTextBox.ClientID%>();"><IMG id="CheckNameButton" src="~/gagui/Images/magnify.gif" border="0" runat="server"></A>
<asp:ImageButton id="bntClear" runat="server" ImageUrl="~/gagui/Images/delete.gif"></asp:ImageButton>
<INPUT id="KeyValueHidden" type="hidden" name="KeyValueHidden" runat="server">

<asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" ErrorMessage="RequiredFieldValidator"
	ControlToValidate="DisplayValueTextBox" Display="Dynamic"></asp:RequiredFieldValidator>

<script language="javascript">
<!--
var Fwin= null;

function openPickerWindow<%=DisplayValueTextBox.ClientID%>() {
	
	Fwin = window.open('gagui/WebForms/PickDataRecordDialog.aspx?ValueControlId=<%=DisplayValueTextBox.ClientID%>&KeyControlId=<%=KeyValueHidden.ClientID%>&DataClass=<%=DataClass%>&DisplayName=<%=DisplayName%>&KeyName=<%=KeyName%>&DependentValueField=<%=DependentValueField%>&DependantKeyField=<%=DependantKeyField%>&ownerclass=<%=OwnerClass%>&ownerfield=<%=OwnerField%>', 'pick_dialog', 'toolbar=no, directories=no, location=no, status=no, menubar=no, resizable=yes, scrollbars=yes, width=400, height=600');
	
	if (Fwin && !Fwin.closed)
		Fwin.focus();
}

function setValueAndKey(value, key)
{
	alert("hello!");
}
// -->
</script>


