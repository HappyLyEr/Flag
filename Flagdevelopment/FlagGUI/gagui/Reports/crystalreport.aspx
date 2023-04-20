<%@ Register TagPrefix="cr" Namespace="CrystalDecisions.Web" Assembly="CrystalDecisions.Web, Version=10.2.3600.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" %>
<%@ Page language="c#" Inherits="GASystem.Reports.crystalreport" CodeFile="crystalreport.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>crystalreport</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<P>Export report to:
				<asp:HyperLink id="excelLink" runat="server">Excel</asp:HyperLink>,
				<asp:HyperLink id="pdfLink" runat="server">PDF</asp:HyperLink></P>
			<P>
				<cr:CrystalReportViewer id="CrystalReportViewer1" runat="server" Width="350px" Height="50px" DisplayGroupTree="False"
					DisplayToolbar="False" SeparatePages="False"></cr:CrystalReportViewer></P>
		</form>
	</body>
</HTML>
