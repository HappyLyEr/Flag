<%@ Control Language="c#" AutoEventWireup="false"  Inherits="GASystem.UserControls.SelectTemplate" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<TABLE id="Table1" cellSpacing="1" cellPadding="1" width="100%" border="0" class="EditForm_Table">
	<TR>
		<TD class="FieldViewLabelCell">
			<asp:Label id="SelectTemplateLabel" runat="server"></asp:Label></TD>
		<TD></TD>
		<TD class="FieldLastInRow">
			<asp:DropDownList id="TemplateDropDown" runat="server"></asp:DropDownList></TD>
	</TR>
	<TR>
		<TD style="HEIGHT: 1px"></TD>
		<TD style="HEIGHT: 1px"></TD>
		<TD style="HEIGHT: 1px"></TD>
	</TR>
	<TR>
		<TD></TD>
		<TD></TD>
		<TD>
			<asp:Button id="SelectTemplateButton" runat="server" CausesValidation="False"></asp:Button></TD>
	</TR>
</TABLE>
