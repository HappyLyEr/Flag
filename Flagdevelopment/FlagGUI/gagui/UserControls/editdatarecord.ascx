<%@ Control Language="c#" AutoEventWireup="false" Inherits="GASystem.EditDataRecord" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<TABLE id="Table1" class="EditFormContainer" cellSpacing="0" cellPadding="0" width="100%" border="0">
    <tr><td class="EditFormHeaderContainer">
        <table border="0" cellpadding="0" width="100%" cellspacing="0" class="EditFormHeader">
        <tr>
            <td align="right" class="EditFormHeader_content_aboveflip" colspan="2" >
                <asp:hyperlink CssClass="FlagLinkButton" Visible="false" id="HelpLink" runat="server" Target="_blank">-help link-</asp:hyperlink>
            </td>
        </tr>
        <tr>
            <td style="width : 8px; vertical-align : top; "><img align="top" src="images/elementer/flipp.gif" /></td>
            <td class="EditFormHeader_content" style="text-align : right;" >&nbsp;</td>
        </tr>
        
        </table>
        <asp:Table ID="tblHelpText" runat="server" BorderStyle="none" CellPadding="0" Width="100%" CellSpacing="0" CssClass="EditFormHeader">
        <asp:TableRow runat="server">
            <asp:TableCell runat="server" CssClass="EditFormHeader_helptext" ><asp:Label CssClass="classHelpText" ID="classHelpText" runat="server"></asp:Label>
           
            </asp:TableCell>
        </asp:TableRow>
    
        
        </asp:Table>
    
    
    </td></tr>


	<tr><td ><asp:PlaceHolder id="DCP" runat="server"></asp:PlaceHolder></td></tr>
	<tr><td ><asp:PlaceHolder id="PlaceHolderSubClasses" runat="server"></asp:PlaceHolder></td></tr>
	

	
	<TR  >
		
		<td class="EditFormFooter">
		<asp:ImageButton id="ImgButtonSave" runat="server" ImageUrl="~/images/ikon/save.gif" /><asp:LinkButton CssClass="FlagLinkButton" id="ButtonSave" runat="server" ></asp:LinkButton>
		<asp:ImageButton id="ImgButtonCancel" runat="server" ImageUrl="~/images/ikon/orangeKryss.gif" CausesValidation="False"></asp:ImageButton><asp:LinkButton CssClass="FlagLinkButton" id="ButtonCancel" runat="server"  CausesValidation="False"></asp:LinkButton>
		<asp:ImageButton id="ImgButtonDelete" runat="server" ImageUrl="~/images/ikon/blaaKryss.gif" Visible="False" CausesValidation="False"></asp:ImageButton><asp:LinkButton CssClass="FlagLinkButton" id="ButtonDelete" runat="server"  CausesValidation="False"></asp:LinkButton>
		
		</TD>
		
	</TR>
</TABLE>
