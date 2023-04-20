<%@ Control Language="c#" AutoEventWireup="false"  Inherits="GASystem.DataContextInfo" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>

		<TABLE width="100%" border="0" cellpadding="0" cellspacing="0" class="ContextInfoPanel">
			<TR>
				<TD>
					<asp:HyperLink id="TopLevelLink" CssClass="FlagContext_TopLevelLink" runat="server">TopLevelLink</asp:HyperLink>
					
					<asp:Label id="LabelCaption" runat="server">ContextMemberInfo</asp:Label>&nbsp;
				</TD>
				
				<TD align="right" rowspan="2">
					<p><asp:LinkButton CssClass="FlagLinkButton" id="ListAllLinkButton" runat="server" CausesValidation="False">ListAll</asp:LinkButton></p>
					<p><asp:LinkButton CssClass="FlagLinkButton" id="ListAllWithinLinkButton" runat="server" CausesValidation="False">ListAllWithin</asp:LinkButton></p>
					<p><asp:HyperLink CssClass="FlagLinkButton" Visible="false" id="SecurityAdminLink" runat="server" NavigateUrl="/ga/gagui/WebForms/EditArcLinkRolePermission.aspx"
						Target="_blank">Security Settings</asp:HyperLink></p>
					<p><asp:LinkButton CssClass="FlagLinkButton" id="LinkSetContext" runat="server" CausesValidation="False">Use as home</asp:LinkButton></p>
				</TD>
			</TR>
			<tr>
			    <td>
			          <asp:LinkButton CssClass="FlagContext_ContextLinkButton" id="ContextLinkButton" runat="server" CausesValidation="False">LinkButton</asp:LinkButton>
					
			    </td>
			</tr>
			<TR>
				<TD>
				    <asp:Label id="LabelSubCaptionAllWithin" CssClass="FlagContext_SubCaptionAllWithin" runat="server">Label</asp:Label>
				    <asp:Label ID="LabelSubCaptionFlagClass"  CssClass="FlagContext_SubCaptionFlagClass" runat="server">Label</asp:Label>
					<asp:Label id="LabelSubCaption" CssClass="FlagContext_SubCaption" runat="server">Label</asp:Label>
				
				  
				</TD>
			</TR>
		</TABLE>

