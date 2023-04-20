<%@ Control Language="c#" AutoEventWireup="false" Inherits="GASystem.GAControls.ViewDataClass" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="uc1" TagName="MemberDataTabs" Src="../UserControls/MemberDataTabs.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ViewDataRecord" Src="../UserControls/ViewDataRecord.ascx" %>
<table border="0" class="ViewDataClass" cellpadding="0" cellspacing="0" width="100%">
<tr><td>

        
        
<table border="0" cellpadding="0" width="100%" cellspacing="0" class="EditFormHeader">
       
        
        <tr>
            <td align="right" class="EditFormHeader_content_aboveflip" colspan="2" >
                <asp:hyperlink CssClass="FlagLinkButton" id="HelpLink" runat="server" Visible="false" Target="_blank">-help link-</asp:hyperlink>
            </td>
        </tr>
        <tr>
            <td style="width : 8px; vertical-align : top; "><img align="top" src="images/elementer/flipp.gif" /></td>
            <td class="EditFormHeader_content" align="left" >
            
            <asp:Image id="ImagePrintTop" ImageUrl="~/images/ikon/print.gif" CssClass="ga_icon" runat="server"
				Visible="False"></asp:Image><!--img src="images/ga_printer.png" class="ga_icon"--><asp:HyperLink id="ReportLinkTop" CssClass="FlagLinkButton" runat="server" target="_blank" Visible="False">Print Report</asp:HyperLink>
            
            <asp:Image id="ImageEditTop" ImageUrl="~/images/ikon/edit.gif" CssClass="ga_icon" runat="server"
				Visible="true"></asp:Image><asp:HyperLink CssClass="FlagLinkButton" id="EditLinkTop" runat="server">Edit</asp:HyperLink>
				<asp:Image id="ImageAddRemedialAction" ImageUrl="~/images/ikon/edit.gif" CssClass="ga_icon" runat="server"
				Visible="true"></asp:Image><asp:LinkButton CssClass="FlagLinkButton" id="AddRemedialAction" runat="server">Add Remedial Action</asp:LinkButton></td>
				
        </tr>
       
        
        </table>

</td></tr>

<tr><td class="ViewFormField_ContentLastInRow_Alternate">
        <asp:PlaceHolder id="workItemPlaceHolder" runat="server"></asp:PlaceHolder>
        </td>
        </tr>
        
        <tr><td>
<uc1:ViewDataRecord id="MyViewDataRecord" runat="server"></uc1:ViewDataRecord>


</td></tr>
<tr><td>


    <asp:PlaceHolder id="InfoPlaceHolder" runat="server"></asp:PlaceHolder>
    <a name="tabsnavigation">
	    <uc1:MemberDataTabs id="MyMemberDataTabs" runat="server"></uc1:MemberDataTabs></a>



</td></tr>


</table>
