<%@ Control Language="c#" AutoEventWireup="false" Inherits="GASystem.UserControls.SubTabsNavigation" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<asp:datalist id="TabList" RepeatDirection="Horizontal" runat="server" CssClass="SubTabsTableStyle"
	ItemStyle-CssClass="SubTabsNavigationStyle" CellPadding="3">
	<ItemStyle CssClass="SubTabsNavigationStyle"></ItemStyle>
	<ItemTemplate>
		<asp:HyperLink id="HyperLink1" runat="server" NavigateUrl="<%# ((GASystem.UserControls.SubTabDetails)Container.DataItem).URL %>">
			<%# ((GASystem.UserControls.SubTabDetails)Container.DataItem).URLText %>
		</asp:HyperLink>
	</ItemTemplate>
</asp:datalist>
