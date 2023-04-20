<%@ Control Language="c#" AutoEventWireup="false" Inherits="GASystem.GAControls.Workflow.CloseWorkitem" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<asp:panel id="Panel1" runat="server">

<table id="dnn_ctr468_GAModuleWorkitem__ctl3_editform_seperatorfirstpage" class="EditForm_Table_wizardSeparator" border="0" cellpadding="0" cellspacing="0">
<tr id="dnn_ctr468_GAModuleWorkitem__ctl3_editform_seperatorfirstpagefirstpage">
			<td id="dnn_ctr468_GAModuleWorkitem__ctl3_firstpageseperatorc1labels"><span id="dnn_ctr468_GAModuleWorkitem__ctl3_Label_seperatorfirstpage"><asp:Label id="lblMessage" runat="server" Width="856px">Close workitem</asp:Label></span></td>
		</tr>
</table>


<asp:Table runat="server" width="750px">
  <asp:TableRow runat="server" ID="tableRowNotes">
    <asp:TableCell CssClass="FieldViewLabelCell"><asp:Label id="lblNotes" runat="server">Notes</asp:Label></asp:TableCell>
    <asp:TableCell CssClass="FieldLastInRow">
        <asp:TextBox TextMode="MultiLine" Rows="4" Width="" runat="server" ID="closeComment"></asp:TextBox>
        <asp:RequiredFieldValidator Text="Notes is required" EnableClientScript="false" ID="commentTextValidator" ControlToValidate="closeComment" runat="server" />
  </asp:TableCell>
  </asp:TableRow>
  
  <asp:TableRow>
    <asp:TableCell CssClass="FieldViewLabelCell"><asp:label id="lblWorkitemReaction" runat="server"></asp:label></asp:TableCell>
    <asp:TableCell CssClass="FieldLastInRow">
        <asp:button id="btnCompleted" CausesValidation="true" runat="server" Visible="False"></asp:button>
        <asp:button id="btnReject" CausesValidation="true" runat="server" Visible="False"></asp:button>
        <asp:button id="btnDissatisfy" CausesValidation="true" runat="server" Visible="False"></asp:button>
        <asp:button id="btnOk" CausesValidation="true" runat="server" Visible="False"></asp:button>
        <asp:button id="btnYes" CausesValidation="true" runat="server" Visible="False"></asp:button>
        <asp:button id="btnNo" CausesValidation="true" runat="server" Visible="False"></asp:button>
        <!-- Tor 20130508 new buttons -->
        <asp:button id="btnApprove" CausesValidation="true" runat="server" Visible="False"></asp:button>
        <asp:button id="btnProceed" CausesValidation="true" runat="server" Visible="False"></asp:button>
        <asp:button id="btnbtnFree1" CausesValidation="true" runat="server" Visible="False"></asp:button>
        <asp:button id="btnbtnFree2" CausesValidation="true" runat="server" Visible="False"></asp:button>
        <asp:button id="btnbtnFree3" CausesValidation="true" runat="server" Visible="False"></asp:button>
        <asp:button id="btnClose" CausesValidation="true" runat="server" Visible="False"></asp:button>
        
<asp:PlaceHolder id="placeHolderCloseWorkitem" runat="server"></asp:PlaceHolder></asp:TableCell></asp:TableRow></asp:Table>

<asp:label id="Label1" runat="server" EnableViewState="False"></asp:label><br/>
	<asp:label id="lblErrorMessage" runat="server" EnableViewState="False" BackColor="Transparent"
		ForeColor="Red"></asp:label>
</asp:panel>
