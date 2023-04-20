<%@ Control Language="c#" AutoEventWireup="false" Inherits="GASystem.GAControls.ActionOwnerContext" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<asp:panel id="Panel1" CssClass="ContextInfoPanel" runat="server">
	<DIV class="DataContextInfo">
		<TABLE width="95%" border="0">
			<TR>
				<TD>
					<asp:Panel id="PlaceHolderShow" runat="server">
						<asp:Image id="ImageShow" runat="server" ImageUrl="~/gagui/images/plus.gif"></asp:Image>
					</asp:Panel>
					<asp:Panel id="PlaceHolderHide" runat="server">
						<asp:Image id="ImageHide" runat="server" ImageUrl="~/gagui/images/minus.gif"></asp:Image>
					</asp:Panel></TD>
			</TR>
		</TABLE>
	</DIV>
</asp:panel>
