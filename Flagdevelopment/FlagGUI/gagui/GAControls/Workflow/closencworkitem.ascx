<%@ Control Language="c#" AutoEventWireup="false" Inherits="GASystem.GAControls.Workflow.CloseNCWorkitem"
    TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<asp:Panel ID="Panel1" runat="server">
    <table id="dnn_ctr468_GAModuleWorkitem__ctl3_editform_seperatorfirstpage" class="EditForm_Table_wizardSeparator"
        border="0" cellpadding="0" cellspacing="0">
        <tr id="dnn_ctr468_GAModuleWorkitem__ctl3_editform_seperatorfirstpagefirstpage">
            <td id="dnn_ctr468_GAModuleWorkitem__ctl3_firstpageseperatorc1labels">
                <span id="dnn_ctr468_GAModuleWorkitem__ctl3_Label_seperatorfirstpage">
                    <asp:Label ID="lblMessage" runat="server" Width="856px">Close workitem</asp:Label></span></td>
        </tr>
    </table>
    <asp:Table runat="server" Width="750px">
        <asp:TableRow runat="server" ID="tableRowFinding">
            <asp:TableCell CssClass="FieldViewLabelCell">
                <asp:Label ID="lblFinding" runat="server"></asp:Label></asp:TableCell>
            <asp:TableCell CssClass="FieldLastInRow">
                <asp:TextBox TextMode="MultiLine" Rows="4" Width="" runat="server" ID="tbFinding" ReadOnly="true"></asp:TextBox>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow ID="tableRowInfo">
            <asp:TableCell ColumnSpan="2">
                <asp:PlaceHolder id="InfoPlaceHolder" runat="server"></asp:PlaceHolder>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow runat="server" ID="tableRowCorrection">
            <asp:TableCell CssClass="FieldViewLabelCell">
                <asp:Label ID="lblCorrection" runat="server"></asp:Label></asp:TableCell>
            <asp:TableCell CssClass="FieldLastInRow">
                <asp:TextBox TextMode="MultiLine" Rows="4" Width="" runat="server" ID="tbCorrection"></asp:TextBox>
                <asp:RequiredFieldValidator Text="Correction is required" EnableClientScript="false" ID="tbCorrectionValidator"
                    ControlToValidate="tbCorrection" runat="server" />
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow runat="server" ID="tableRowCorrectiveAction">
            <asp:TableCell CssClass="FieldViewLabelCell">
                <asp:Label ID="lblCorrectiveAction" runat="server"></asp:Label></asp:TableCell>
            <asp:TableCell CssClass="FieldLastInRow">
                <asp:TextBox TextMode="MultiLine" Rows="4" Width="" runat="server" ID="tbCorrectiveAction"></asp:TextBox>
                <asp:RequiredFieldValidator Text="Corrective Action is required" EnableClientScript="false" ID="tbCorrectiveActionValidator"
                    ControlToValidate="tbCorrectiveAction" runat="server" />
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow runat="server" ID="tableRowReviewofCorrectiveActions">
            <asp:TableCell CssClass="FieldViewLabelCell">
                <asp:Label ID="lblReviewofCorrectiveActions" runat="server"></asp:Label></asp:TableCell>
            <asp:TableCell CssClass="FieldLastInRow">
                <asp:TextBox TextMode="MultiLine" Rows="4" Width="" runat="server" ID="tbReviewofCorrectiveActions"></asp:TextBox>
                <asp:RequiredFieldValidator Text="Review of Corrective Actions is required" EnableClientScript="false" ID="tbReviewofCorrectiveActionsValidator"
                    ControlToValidate="tbReviewofCorrectiveActions" runat="server" />
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow runat="server" ID="tableRowReasons" Visible="false">
            <asp:TableCell CssClass="FieldViewLabelCell">
                <asp:Label ID="lblReasons" runat="server"></asp:Label></asp:TableCell>
            <asp:TableCell CssClass="FieldLastInRow">
                <asp:TextBox TextMode="MultiLine" Rows="4" Width="" runat="server" ID="tbReasons"></asp:TextBox>
                <asp:RequiredFieldValidator Text="Reason is required" EnableClientScript="false" ID="tbReasonsValidator"
                    ControlToValidate="tbReasons" runat="server" />
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow runat="server">
            <asp:TableCell CssClass="FieldViewLabelCell">
                <asp:Label ID="lblWorkitemReaction" runat="server"></asp:Label>
            </asp:TableCell>
            <asp:TableCell CssClass="FieldLastInRow">
                <asp:Button ID="btnCompleted" CausesValidation="true" runat="server">
                </asp:Button>
                <asp:Button ID="btnReject" CausesValidation="true" runat="server" Visible="False"></asp:Button>
                <asp:Button ID="btnDissatisfy" CausesValidation="true" runat="server" Visible="False">
                </asp:Button>
                <asp:PlaceHolder ID="placeHolderCloseWorkitem" runat="server"></asp:PlaceHolder>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    <asp:Label ID="Label1" runat="server" EnableViewState="False"></asp:Label><br />
    <asp:Label ID="lblErrorMessage" runat="server" EnableViewState="False" BackColor="Transparent"
        ForeColor="Red"></asp:Label>
</asp:Panel>
