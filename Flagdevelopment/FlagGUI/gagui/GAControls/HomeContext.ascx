<%@ Control Language="C#" AutoEventWireup="true" Inherits="GASystem.GAGUI.GAControls.HomeContext" %>
<%@ Register Src="HomeContextNavigationHeader.ascx" TagName="HomeContextNavigationHeader"
    TagPrefix="uc2" %>
<%@ Register TagPrefix="uc1" TagName="ShortCutLinks" Src="~/gagui/GAControls/ShortCutLinks.ascx" %>
<div class="ContextNavigation">
	
	<div class="ContextNavigationBody">
		<uc2:HomeContextNavigationHeader id="MyHomeContextNavigationHeader" runat="server">
        </uc2:HomeContextNavigationHeader>
		<uc1:ShortCutLinks id="MyShortCutLinks" runat="server"></uc1:ShortCutLinks>
        
	</div>
</div>