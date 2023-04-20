<%@ Control Language="c#" AutoEventWireup="false" Inherits="GASystem.UserControls.EditUserRoles" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<asp:PlaceHolder id="UserMessagePlaceHolder" runat="server"></asp:PlaceHolder>
<asp:datagrid id="grdDataRecordRolePermissions" ShowFooter="True" AutoGenerateColumns="False"
	runat="server" CssClass="gridStyle">
	<FooterStyle CssClass="gridStyle_FooterStyle"></FooterStyle>
	<SelectedItemStyle CssClass="gridStyle_SelectedItemStyle"></SelectedItemStyle>
	<AlternatingItemStyle CssClass="gridStyle_AlternatingItemStyle"></AlternatingItemStyle>
	<ItemStyle CssClass="gridStyle_ItemStyle"></ItemStyle>
	<HeaderStyle CssClass="gridStyle_HeaderStyle"></HeaderStyle>
	<Columns>
		<asp:TemplateColumn HeaderText="GroupName">
			<ItemTemplate>
				<asp:Label id=Label4 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.RoleName") %>'>
				</asp:Label>
			</ItemTemplate>
			<FooterTemplate>
				<asp:DropDownList id="ddlRoles" runat="server"></asp:DropDownList>
			</FooterTemplate>
			<EditItemTemplate>
				<asp:Label id=Label1 runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.RoleName") %>'>
				</asp:Label>
			</EditItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="Read">
			<ItemTemplate>
				&nbsp;
				<asp:CheckBox id=CheckRead runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.HasRead") %>' Enabled="False">
				</asp:CheckBox>
			</ItemTemplate>
			<FooterTemplate>
				<asp:CheckBox id="CheckReadInsert" runat="server"></asp:CheckBox>
			</FooterTemplate>
			<EditItemTemplate>
				<asp:CheckBox id=CheckReadEdit runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.HasRead") %>'>
				</asp:CheckBox>
			</EditItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="Update">
			<ItemTemplate>
				<asp:CheckBox id=CheckUpdate runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.HasUpdate") %>' Enabled="False">
				</asp:CheckBox>
			</ItemTemplate>
			<FooterTemplate>
				<asp:CheckBox id="CheckUpdateInsert" runat="server"></asp:CheckBox>
			</FooterTemplate>
			<EditItemTemplate>
				<asp:CheckBox id=CheckUpdateEdit runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.HasUpdate") %>'>
				</asp:CheckBox>
			</EditItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="Create">
			<ItemTemplate>
				<asp:CheckBox id=CheckCreate runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.HasCreate") %>' Enabled="False">
				</asp:CheckBox>
			</ItemTemplate>
			<FooterTemplate>
				<asp:CheckBox id="CheckCreateInsert" runat="server"></asp:CheckBox>
			</FooterTemplate>
			<EditItemTemplate>
				<asp:CheckBox id=CheckCreateEdit runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.HasCreate") %>'>
				</asp:CheckBox>
			</EditItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="Delete">
			<ItemTemplate>
				<asp:CheckBox id=CheckDelete runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.HasDelete") %>' Enabled="False">
				</asp:CheckBox>
			</ItemTemplate>
			<FooterTemplate>
				<asp:CheckBox id="CheckDeleteInsert" runat="server"></asp:CheckBox>
			</FooterTemplate>
			<EditItemTemplate>
				<asp:CheckBox id=CheckDeleteEdit runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.HasDelete") %>'>
				</asp:CheckBox>
			</EditItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn>
			<ItemTemplate>
				<asp:LinkButton id="LinkButton2" CssClass="smallText" runat="server" CommandName="Edit" CausesValidation="False">EDIT</asp:LinkButton>&nbsp;
				<asp:LinkButton id="LinkButton1" CssClass="smallText" runat="server" CommandName="Delete" CausesValidation="False">DELETE</asp:LinkButton>
			</ItemTemplate>
			<FooterTemplate>
				<asp:Button id="btnAdd" CssClass="CommandButton" CommandName="Insert" CausesValidation="False"
					Runat="server" EnableViewState="True" Text="ADD"></asp:Button>
			</FooterTemplate>
			<EditItemTemplate>
				<asp:LinkButton id="LinkButton3" CssClass="smallText" runat="server" CommandName="Update" CausesValidation="False">SAVE</asp:LinkButton>
				<asp:LinkButton id="LinkButton4" CssClass="smallText" runat="server" CommandName="Cancel" CausesValidation="False">CANCEL</asp:LinkButton>
			</EditItemTemplate>
		</asp:TemplateColumn>
	</Columns>
	<PagerStyle HorizontalAlign="Center" ForeColor="Black" BackColor="#999999" Mode="NumericPages"></PagerStyle>
</asp:datagrid>
<asp:PlaceHolder id="TestOutput" runat="server"></asp:PlaceHolder>
