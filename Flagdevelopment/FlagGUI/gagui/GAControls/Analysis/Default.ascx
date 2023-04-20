<%@ Control Language="C#" AutoEventWireup="true"  Inherits="GASystem.GAGUI.GAControls.Analysis.PivotCustomControlTest" %>

<%@ Register Assembly="datadynamics.analysis.web" Namespace="DataDynamics.Analysis.Web" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

    <div>
		<asp:Panel ID="Panel1" runat="server" Width="406px" BackColor="Control" BorderColor="WindowFrame" BorderStyle="Solid" BorderWidth="1px" Font-Names="Tahoma" Font-Size="Small">
			<table style="width: 100%" cellpadding="0" cellspacing="2">
				<tr>
					<td>
						Data source</td>
					<td>
						<asp:DropDownList ID="dataSource" runat="server">
						</asp:DropDownList>
						<asp:CustomValidator ID="dataSourceValidator" runat="server" ControlToValidate="dataSource"
							EnableClientScript="False" EnableTheming="False" EnableViewState="False" ErrorMessage="error"
							SetFocusOnError="True" Visible="False"></asp:CustomValidator></td>
					<td>
						<asp:Button ID="buttonConnect" runat="server" Text="Connect" OnClick="buttonConnect_Click" /></td>
				</tr>
				<tr>
					<td>
						File name</td>
					<td valign="top">
						<asp:TextBox ID="textBoxFileName" runat="server"></asp:TextBox>
						<asp:CustomValidator ID="fileNameValidator" runat="server" ControlToValidate="textBoxFileName"
							EnableClientScript="False" EnableTheming="False" EnableViewState="False" ErrorMessage="error"
							SetFocusOnError="True" Visible="False" Width="65px"></asp:CustomValidator></td>
					<td>
						<asp:Button ID="buttonSave" runat="server" Text="Save" OnClick="buttonSave_Click" />
						<asp:Button ID="buttonLoad" runat="server" Text="Load" OnClick="buttonLoad_Click" /></td>
				</tr>
				<tr>
					<td>
						Theme</td>
					<td>
						<asp:DropDownList ID="listThemes" runat="server" AutoPostBack="True" OnSelectedIndexChanged="listThemes_SelectedIndexChanged">
						</asp:DropDownList></td>
					<td>
					</td>
				</tr>
			</table>
		</asp:Panel>
		<asp:ValidationSummary ID="validationSummary" runat="server" BorderStyle="Double"
			EnableClientScript="False" EnableTheming="False" EnableViewState="False" />
		&nbsp;&nbsp;
			<cc1:PivotView BackColor="Beige" ID="pivotView" runat="server"/>
		end of page
    </div>
   