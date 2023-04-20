<%@ Control Language="c#" AutoEventWireup="false" Inherits="GASystem.GAControls.Workflow.AddWorkitemComment" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<asp:panel id="Panel1" runat="server">

<table id="dnn_ctr468_GAModuleWorkitem__ctl3_editform_seperatorfirstpage" class="EditForm_Table_wizardSeparator" border="0" cellpadding="0" cellspacing="0">
<tr id="dnn_ctr468_GAModuleWorkitem__ctl3_editform_seperatorfirstpagefirstpage">
			<td id="dnn_ctr468_GAModuleWorkitem__ctl3_firstpageseperatorc1labels"><span id="dnn_ctr468_GAModuleWorkitem__ctl3_Label_seperatorfirstpage"><asp:Label id=lblMessage runat="server" Width="856px">Add notes to workitem</asp:Label></span></td>
		</tr>
</table>


<TABLE width="750px">
  <TR>
    <TD class="FieldViewLabelCell"><asp:Label id="lblNotes" runat="server">Notes</asp:Label></TD>
    <TD class="FieldLastInRow">
        <asp:TextBox TextMode="MultiLine" Rows="4" Width="" runat="server" ID="commentText"></asp:TextBox>
        <asp:RequiredFieldValidator Text="Notes is required" EnableClientScript="false" ID="commentTextValidator" ControlToValidate="commentText" runat="server" />
  </TD></TR>
</TABLE>
	<TABLE id="Table1" cellSpacing="1" cellPadding="1" width="100%" border="0">
		<TR>
			<TD style="WIDTH: 188px" width="86"></TD>
			<TD style="WIDTH: 166px">
				<asp:Button id="btnSave" CausesValidation="true" runat="server" Text="Add"></asp:Button>
			    <asp:Button id="btnCancel" CausesValidation="false" runat="server" Text="Cancel"></asp:Button>	
			</TD>
			<TD style="WIDTH: 166px">
				</TD>
			<TD></TD>
		</TR>
	</TABLE>

</asp:panel>
