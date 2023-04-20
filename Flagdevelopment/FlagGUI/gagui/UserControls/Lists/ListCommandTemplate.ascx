<%@ control language="C#" autoeventwireup="False" inherits="GASystem.UserControls.Lists.ListCommandTemplate" %>


<table border="0" cellpadding="0" width="100%" cellspacing="0" class="ListFormHeader">
    <tr>
        <td align="right" class="ListFormHeader_content_aboveflip" colspan="2">&nbsp;
        </td>
    </tr>
    <tr>
        <td style="width: 8px; vertical-align: top;">
            <img align="top" src="images/elementer/flipp.gif" /></td>
        <td class="ListFormHeader_content" align="left">

            <asp:linkbutton cssclass="FlagLinkButtonCombined" id="btnEditSelected" runat="server" commandname="EditSelected" causesvalidation="false" visible='<%# this.DisplayEditLink %>'><img  alt="" src="images/Ikon/edit.gif" /><%# this.EditSelectedText %></asp:linkbutton>
            <asp:linkbutton cssclass="FlagLinkButtonCombined" id="btnEditCount" runat="server" commandname="EditSelected" commandargument="EditCount" causesvalidation="false" visible='<%# this.DisplayEditCountLink %>'><img  alt="" src="images/Ikon/edit.gif" /><%# this.EditCountText %></asp:linkbutton>
            <asp:linkbutton cssclass="FlagLinkButtonCombined" id="btnUpdateEdited" runat="server" commandname="UpdateEdited" visible='<%# this.DisplayUpdateLink %>'><img  alt="" src="gagui/Images/save.gif" /><%# this.UpdateSelectedText %></asp:linkbutton>
            <asp:linkbutton cssclass="FlagLinkButtonCombined" id="btnUpdateEditedAndCreateNew" runat="server" commandname="UpdateEdited" commandargument="CreateNewAfterUpdate" visible='<%# this.DisplayUpdateAndCreateNewLink %>'><img  alt="" src="gagui/Images/save.gif" /><%# this.UpdateAndCreateNewSelectedText%></asp:linkbutton>


            <asp:linkbutton cssclass="FlagLinkButtonCombined" id="btnCancel" runat="server" commandname="CancelAll" causesvalidation="false" visible='<%# this.IsEditMode %>'><img  alt="" src="gagui/Images/delete.gif" /><%# this.CancelEditText %></asp:linkbutton>
            <asp:linkbutton cssclass="FlagLinkButtonCombined" id="btnImportExcel" runat="server" commandname="ImportFromExcel" causesvalidation="false" visible='<%# this.DisplayImportFromExcelLink %>'><img  alt="" src="gagui/Images/up.gif" /><%# this.ImportFromExcelText %></asp:linkbutton>
            <asp:linkbutton cssclass="FlagLinkButtonCombined" id="btnExportToExcel" runat="server" commandname="ExportToExcel" causesvalidation="false" visible='<%# this.DisplayExportToExcelLink %>'><%# this.ExportToExcelText %></asp:linkbutton>
            <asp:linkbutton cssclass="FlagLinkButtonCombined" id="btnCancelSort" text="Cancel Sort" runat="server" commandname="CancelSort" causesvalidation="false" visible='<%# this.DisplayCancelSortLink %>'></asp:linkbutton>



            &nbsp;
        </td>
    </tr>

</table>


