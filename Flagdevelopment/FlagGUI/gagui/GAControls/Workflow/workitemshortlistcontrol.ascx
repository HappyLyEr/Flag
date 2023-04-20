<%@ Control Language="c#" AutoEventWireup="false" Inherits="GASystem.GAControls.Workflow.WorkitemShortListControl" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<%@ Register Src="../HomeContextNavigationHeader.ascx" TagName="HomeContextNavigationHeader"
    TagPrefix="uc2" %>
<%@ Register TagPrefix="uc1" TagName="DataContextInfo" Src="../../UserControls/DataContextInfo.ascx" %>

<div class="WorkitemShortList" >

<div style="padding: 5 2 2 5;"><uc2:HomeContextNavigationHeader id="HomeContextNavigationHeader1" runat="server">
    </uc2:HomeContextNavigationHeader>
    </div>
	
	<div style="padding:  20 2 5 2;">
<asp:Label CssClass = "FlagContext_SubCaptionFlagClass" runat ="server" ID="workitemLabel"></asp:Label>	
</div><div style="padding:  0 0 2 5">
<asp:HyperLink id="HyperLink1" runat="server"  Font-Size="11px">HyperLink</asp:HyperLink>
</div>

<asp:PlaceHolder id="PlaceHolderMain" runat="server"></asp:PlaceHolder>
<asp:Label id="lblError" runat="server" Font-Size="Smaller" EnableViewState="False" ForeColor="Red"></asp:Label>
    </div>