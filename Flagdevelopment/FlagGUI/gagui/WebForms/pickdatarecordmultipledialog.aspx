
<%@ Register TagPrefix="uc1" TagName="PickDataRecord" Src="../UserControls/PickDataRecord.ascx" %>
<%@ Register TagPrefix="uc1" TagName="PickDataRecordMultiple" Src="../UserControls/PickDataRecordMultiple.ascx" %>
<%@ Page language="c#" AutoEventWireup="false" Inherits="GASystem.PickDataRecordMultipleDialog" %>
<%@ Register TagPrefix="uc1" TagName="EditDataRecord" Src="../UserControls/EditDataRecord.ascx" %>
<%@ Register TagPrefix="uc1" TagName="UserMessage" Src="../UserControls/UserMessage.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Select record</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link rel="stylesheet" href="../../Portals/_default/portal.css">
		<link rel="stylesheet" href="../../Portals/_default/default.css">
		<link rel="stylesheet" href="../../Portals/_default/Skins/norsolutions/skin.css">
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			&nbsp;
			<uc1:UserMessage id="UserMessageControl" runat="server"></uc1:UserMessage>
			<asp:Label id="lblFilter" runat="server"></asp:Label><br>
			<uc1:PickDataRecordMultiple id="PickDataRecordControl" runat="server"></uc1:PickDataRecordMultiple><br>
		</form>
	</body>
</HTML>
