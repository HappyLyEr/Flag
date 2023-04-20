<%@ Register TagPrefix="uc1" TagName="EditArcLinksAccessRoles" Src="../UserControls/EditArcLinksAccessRoles.ascx" %>
<%@ Page language="c#"  AutoEventWireup="false" Inherits="GASystem.WebForms.EditArcLinkRolePermission" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title><%=ArcLinkLabel.Text%></title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link rel="stylesheet" href="../../Portals/_default/portal.css">
		<link rel="stylesheet" href="../../Portals/_default/default.css">
		<link rel="stylesheet" href="../../Portals/_default/Skins/norsolutions/skin.css">
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<P>
				<asp:Label id="ArcLinkLabel" runat="server" CssClass="SubCaption">Label</asp:Label></P>
			<P>
				<uc1:EditArcLinksAccessRoles id="EditArcLinksAccessRoles1" runat="server"></uc1:EditArcLinksAccessRoles></P>
		</form>
	</body>
</HTML>
