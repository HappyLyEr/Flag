<%@ Control Language="c#" AutoEventWireup="false" Inherits="GASystem.GAControls.ActionStatus" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<P>
	<asp:Label id="Label1" runat="server">Actions initilized but not started:</asp:Label>
	<asp:DataGrid id="ActionDataGrid" runat="server" CssClass="MinorTable" GridLines="None" AutoGenerateColumns="False">
		<HeaderStyle CssClass="MinorTableHeader" />
		<ItemStyle CssClass="MinorTableItem" />
		<AlternatingItemStyle CssClass="MinorTableAlternatingItem" />
		<Columns>
			<asp:BoundColumn Visible="False" DataField="ActionRowId"></asp:BoundColumn>
			<asp:BoundColumn DataField="ActionName" HeaderText="Name"></asp:BoundColumn>
			<asp:BoundColumn DataField="Description" HeaderText="Description"></asp:BoundColumn>
			<asp:ButtonColumn Text="Start now" CommandName="Select"></asp:ButtonColumn>
		</Columns>
	</asp:DataGrid></P>
