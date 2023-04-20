<%@ Control Language="c#" AutoEventWireup="false" Inherits="GASystem.GAControls.ShortCutLinks" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<asp:datagrid id="DataGrid1" GridLines="None" CellPadding="0" AutoGenerateColumns="False" CssClass="ContextNavigationTable"
	runat="server">
	<Columns>
		<asp:TemplateColumn>
			<ItemTemplate>
				<asp:HyperLink runat="server" Text='<%# ((GASystem.GUIUtils.ShortCutLinkElement)Container.DataItem).DataClassName %>' NavigateUrl='<%# String.Format("~/default.aspx?tabid={0}&dataclass={1}"  ,((GASystem.GUIUtils.ShortCutLinkElement)Container.DataItem).DataClassTabId,  ((GASystem.GUIUtils.ShortCutLinkElement)Container.DataItem).DataClass) %>'>
				</asp:HyperLink>
			</ItemTemplate>
		</asp:TemplateColumn>
	</Columns>
</asp:datagrid>
