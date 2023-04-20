<%@ Control Language="c#" AutoEventWireup="false" Inherits="GASystem.ListDataRecords" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<asp:PlaceHolder id="PlaceHolderFilter" runat="server"></asp:PlaceHolder>

	<table border="0" width="100%" cellpadding="0" cellspacing="0" class="Listdatarecords_master">
		<tr>
			<td width="5px">
				
			</td>
			<td>
				<asp:HyperLink id="ButtonNewRecordTop" runat="server" CssClass="FlagLinkButton" Visible="False">NewRecord</asp:HyperLink>
				<asp:HyperLink id="lnkAddGroupTop" runat="server" CssClass="FlagLinkButton" Visible="False">Add Group</asp:HyperLink>
	
			</td>
		</tr>
	</table>

<asp:placeholder id="ListPHolder" runat="server" EnableViewState="True"></asp:placeholder>

    <table border="0" width="100%" cellpadding="0" cellspacing="0" class="ListForm_Message">
    <tr><td><asp:Label id="MessageLabel" runat="server" CssClass="Normal"></asp:Label></td></tr>
    </table>
	<table border="0" width="100%" cellpadding="0" cellspacing="0" class="ListForm_NewRecord">
		<tr>
			<td width="5px">
				
			</td>
			<td>
			    <asp:HyperLink id="ButtonNewRecord" runat="server" CssClass="FlagLinkButton" Visible="False">NewRecord</asp:HyperLink>
			    <asp:linkbutton cssclass="FlagLinkButton" id="ButtonImportExcel" runat="server" Visible="False">
                    <img height="10" src="gagui/Images/up.gif" /><%= this.ImportExcelTextLink %>
                </asp:linkbutton>
				<asp:HyperLink id="lnkAddGroup" runat="server" CssClass="FlagLinkButton" Visible="False">Add Group</asp:HyperLink>
			</td>
		</tr>
	</table>
	<asp:placeholder id="SumHeaderPHolder" runat="server" EnableViewState="True">
	<table border="0" cellpadding="0" width="100%" cellspacing="0" class="EditFormHeader">
       
        
        <tr>
            <td align="right" class="EditFormHeader_content_aboveflip" colspan="2" >
               
            </td>
        </tr>
        <tr>
            <td style="width : 8px; vertical-align : top; "><img align="top" src="images/elementer/flipp.gif" /></td>
            <td class="EditFormHeader_content" align="left" >
            &nbsp
            </td>
				
        </tr>
       
        
        </table></asp:placeholder>
<asp:placeholder id="SumPHolder" runat="server" EnableViewState="True"></asp:placeholder>
<asp:placeholder id="ImportExcelPHolder" runat="server" EnableViewState="True" Visible="False"></asp:placeholder>