<%@ Control Language="c#" AutoEventWireup="false" Inherits="GASystem.PickDataRecordMultiple" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<asp:placeholder id="GridPlaceHolder" runat="server" EnableViewState="False"></asp:placeholder>
<br/>
<asp:Button id="SelectButton" runat="server" Text="Select"></asp:Button>
<script language="javascript">
<!--

function setParentWindowValues() {
	//alert("<%=ParentValueControlId%>");
	var valueElement = window.opener.document.getElementById("<%=ParentValueControlId%>");
	//var valueElement = window.opener.document.getElementsByName["<%=ParentValueControlId%>"];
	

	if (valueElement)
		valueElement.value = "<%=SelectedValue%>";
		
	
	var keyElement = window.opener.document.getElementById("<%=ParentKeyControlId%>");
	//var keyElement = window.opener.document.getElementsByName["<%=ParentKeyControlId%>"];
	//alert(keyElement.length);
	keyElement.value = "<%=SelectedKeys%>"
	
}


// -->
</script>
