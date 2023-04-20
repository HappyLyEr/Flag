<%@ Register TagPrefix="uc1" TagName="ListDataRecords" Src="ListDataRecords.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Tabsnavigation" Src="Tabsnavigation.ascx" %>
<%@ Control Language="c#" AutoEventWireup="false" Inherits="GASystem.MemberDataTabs" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<br>
<div class="MemberDataTabs">
	<div class="MemberDataTabsHead">
		<uc1:Tabsnavigation id="TabsnavigationControl" runat="server"></uc1:Tabsnavigation>
	</div>
	<div class="MemberDataTabsBody">
		<asp:Panel id="DataListPanel" runat="server">
<asp:PlaceHolder id=RecordListPlaceHolder runat="server"></asp:PlaceHolder>
		</asp:Panel>
	
	</div>
</div>
