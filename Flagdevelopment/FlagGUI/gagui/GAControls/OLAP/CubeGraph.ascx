<%@ Control Language="C#" AutoEventWireup="true"  Inherits="GASystem.GAGUI.GAControls.OLAP.CubeGraph" %>
<%@ Register Assembly="RadarSoft.RadarCube.Web" Namespace="RadarSoft.RadarCube.Web"
    TagPrefix="radarcube" %>
<%@ Register Assembly="RadarSoft.RadarCube.Web.MSAS" Namespace="RadarSoft.RadarCube.Web"
    TagPrefix="radarcube" %>
<%@ Register Assembly="RadChart.Net2" Namespace="Telerik.WebControls" TagPrefix="Rad"%>


<table width="100%" cellspacing="0" cellpadding="0" border="0" class="EditFormHeader">
    <tbody>
        <tr>
            <td align="right" colspan="2" style="height: 2px;" class="EditFormHeader_content_aboveflip">&#160;</td>
        </tr>
        <tr>
            <td style="vertical-align: top; width: 8px;"><asp:Image ID="Image1" runat="server" ImageAlign="top" ImageUrl="~/images/elementer/flipp.gif"  /></td>
            <td align="left" style="padding-left: 5px;" class="EditFormHeader_content">
             <asp:LinkButton ID="buttonTable" CssClass="FlagLinkButton" runat="server">Table</asp:LinkButton>
                <asp:LinkButton ID="buttonBar" CssClass="FlagLinkButton" runat="server">Bar Chart</asp:LinkButton>
                <asp:LinkButton ID="buttonStackedBar" CssClass="FlagLinkButton" runat="server">Stacked Bar Chart</asp:LinkButton>
                <asp:LinkButton ID="buttonPie" CssClass="FlagLinkButton" runat="server">Pie Chart</asp:LinkButton>
                <asp:LinkButton ID="buttonLine" CssClass="FlagLinkButton" runat="server">Line Chart</asp:LinkButton>
                 <asp:LinkButton ID="buttonLabelVisibility" CssClass="FlagLinkButton" runat="server">Show Labels</asp:LinkButton>
     </td>
      <td align="right" class="EditFormHeader_content">
        <a class="FlagLinkButton" href="javascript:window.close();"><asp:Image ID="Image2"  runat="server" ImageUrl="~/images/ikon/orangecross.gif" /> Close</a>
      </td></tr>

    </tbody>
</table>     
            
         
<div style="padding: 5px 5px 5px 5px;">

<Rad:RadChart ID="RadChart1" Width="950px" Height="600px" runat="server">

</Rad:RadChart>
</div>



