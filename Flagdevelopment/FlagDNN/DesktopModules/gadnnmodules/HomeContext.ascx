<%@ Control Language="c#" AutoEventWireup="false"  Inherits="gadnnmodules.HomeContext" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<%@ Register TagPrefix="uc1" TagName="ShortCutLinks" Src="../../gagui/GAControls/ShortCutLinks.ascx" %>
<div class="ContextNavigation">
	<div class="ContextNavigationHeader"><asp:HyperLink id="HyperLink1" CssClass="ContextNavigationLink" runat="server">HyperLink</asp:HyperLink>
		<asp:Label id="lblError" runat="server" Visible="False" ForeColor="Red">lblError</asp:Label></div>
	<div class="ContextNavigationBody">
		<uc1:ShortCutLinks id="MyShortCutLinks" runat="server"></uc1:ShortCutLinks>
	</div>
</div>
