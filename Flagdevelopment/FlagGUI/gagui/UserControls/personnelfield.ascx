<%@ Control Language="c#" AutoEventWireup="false" Inherits="GASystem.PersonnelField" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<asp:TextBox id="DisplayValueTextBox" runat="server" Columns="25"></asp:TextBox><a href="javascript:openPickerWindow<%=DisplayValueTextBox.ClientID%>();"><IMG src="../gagui/Images/checkname.gif" border="0" runat="server" id="CheckNameButton"></a>
<INPUT type="hidden" runat="server" id="KeyValueHidden" NAME="KeyValueHidden">
<script language="javascript">
<!--
var Fwin= null;

function openPickerWindow<%=DisplayValueTextBox.ClientID%>() {
	
	//Fwin = window.open('/DotNetNuke/gagui/WebForms/PickDataRecordDialog.aspx?ValueControlId=<%=DisplayValueTextBox.ClientID%>&KeyControlId=<%=KeyValueHidden.ClientID%>&DataClass=GAPersonnel&DisplayName=GivenName+FamilyName', 'pick_dialog', 'toolbar=no, directories=no, location=no, status=no, menubar=no, resizable=yes, scrollbars=yes, width=400, height=600');
	Fwin = window.open('gagui/WebForms/PickDataRecordDialog.aspx?ValueControlId=<%=DisplayValueTextBox.ClientID%>&KeyControlId=<%=KeyValueHidden.ClientID%>&DataClass=GAPersonnel&DisplayName=GivenName+FamilyName', 'pick_dialog', 'toolbar=no, directories=no, location=no, status=no, menubar=no, resizable=yes, scrollbars=yes, width=400, height=600');
	
	if (Fwin && !Fwin.closed)
		Fwin.focus();
}

function setValueAndKey(value, key)
{
	alert("hello!");
}
// -->
</script>
