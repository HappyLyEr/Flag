<%@ Register TagPrefix="uc1" TagName="UserMessage" Src="../UserControls/UserMessage.ascx" %>
<%@ Register TagPrefix="uc1" TagName="EditDataRecord" Src="../UserControls/EditDataRecord.ascx" %>
<%@ Page language="c#"  AutoEventWireup="false" Inherits="GASystem.PickDataRecordDialog" %>
<%@ Register TagPrefix="uc1" TagName="PickDataRecord" Src="../UserControls/PickDataRecord.ascx" %>
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
			<asp:Button id="btnClearFilter" runat="server"></asp:Button><br>
			<uc1:pickdatarecord id="PickDataRecordControl" runat="server"></uc1:pickdatarecord>
			<uc1:EditDataRecord id="MyEditDataRecord" runat="server" Visible="False"></uc1:EditDataRecord>
			<asp:LinkButton id="LinkButton1" runat="server">New Record</asp:LinkButton>
		</form>
	</body>
</HTML>
