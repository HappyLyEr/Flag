<%@ Control Language="c#" AutoEventWireup="false" Inherits="GASystem.PickDataRecord" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<BR>
<asp:placeholder id="GridPlaceHolder" runat="server" EnableViewState="False"></asp:placeholder>
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
	keyElement.value = "<%=SelectedKey%>"
	
}

function clearParentWindowValue(parentFieldName) {
	//alert("<%=ParentValueControlId%>");
	var valueElement = window.opener.document.getElementById(parentFieldName);
	//var valueElement = window.opener.document.getElementsByName["<%=ParentValueControlId%>"];
	
	if (valueElement)
		valueElement.value = "";

}





// -->
</script>
