<%@ Page language="c#"  AutoEventWireup="false" Inherits="GASystem.WebForms.HelpDialog" %>
<%@ Register TagPrefix="uc1" TagName="ListDataRecords" Src="../UserControls/ListDataRecords.ascx" %>
<%@ Register TagPrefix="uc1" TagName="MemberDataTabs" Src="../UserControls/MemberDataTabs.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ViewDataRecord" Src="../UserControls/ViewDataRecord.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>HelpDialog</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link rel="stylesheet" href="../../Portals/_default/portal.css"/>
		<link rel="stylesheet" href="../../Portals/_default/default.css"/>
		<link rel="stylesheet" href="../../Portals/_default/Skins/norblue/skin.css"/>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<h1>Help</h1>
			<uc1:ViewDataRecord id="MyViewDataRecord" runat="server"></uc1:ViewDataRecord>
			<uc1:ListDataRecords id="ListFileDataRecords" runat="server"></uc1:ListDataRecords>
		</form>
	</body>
</HTML>
