<%@ Control language="vb" CodeBehind="~/admin/Skins/skin.vb" AutoEventWireup="false" Explicit="True" Inherits="DotNetNuke.Skin" %>
<%@ Register TagPrefix="dnn" TagName="USER" Src="~/Admin/Skins/User.ascx" %>
<%@ Register TagPrefix="dnn" TagName="LOGIN" Src="~/Admin/Skins/Login.ascx" %>
<%@ Register TagPrefix="dnn" TagName="SOLPARTMENU" Src="~/Admin/Skins/SolPartMenu.ascx" %>
<%@ Register TagPrefix="dnn" TagName="BREADCRUMB" Src="~/Admin/Skins/BreadCrumb.ascx" %>
<%@ Register TagPrefix="dnn" TagName="LINKS" Src="~/Admin/Skins/Links.ascx" %>
<%@ Register TagPrefix="dnn" TagName="DOTNETNUKE" Src="~/Admin/Skins/DotNetNuke.ascx" %>

<div class="ga_header">
	<img src="Portals/_default/Skins/norsolutions/norsolutions2.jpg"/>
	<span class="va_top">
		<dnn:USER runat="server" id="dnnUser" CssClass="Login" Text="[Not logged in]" /> |
		<dnn:LOGIN runat="server" id="dnnLOGIN" CssClass="Login" />
	</span>
</div>
					
<div class="ga_main_menu">
	<dnn:SOLPARTMENU runat="server" id="dnnSOLPARTMENU" />
</div>

<!--div class="ga_breadcrumb">
	<dnn:BREADCRUMB runat="server" id="dnnBREADCRUMB" CssClass="Breadcrumb" />
</div-->
<div class="ga_container">
	<div id="LeftPane" class="ga_left_pane" runat="server" visible="false"></div>
	<div id="ContentPane" class="ga_content_pane" runat="server" visible="false"></div>
	<div id="RightPane" class="ga_right_pane" runat="server" visible="false"></div>	
<div>

