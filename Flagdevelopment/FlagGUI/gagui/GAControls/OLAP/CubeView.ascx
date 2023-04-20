<%@ Control Language="C#" AutoEventWireup="true"  Inherits="GASystem.GAGUI.GAControls.OLAP.CubeView" %>
<%@ Register Assembly="RadarSoft.RadarCube.Web" Namespace="RadarSoft.RadarCube.Web" TagPrefix="radarcube" %>
<%@ Register Assembly="RadarSoft.RadarCube.Web.MSAS" Namespace="RadarSoft.RadarCube.Web" TagPrefix="radarcube" %>
<%@ Register Assembly="RadarSoft.RadarCube.Web.Grid" Namespace="RadarSoft.RadarCube.Web" TagPrefix="radarcube" %>


<table width="100%" cellspacing="0" cellpadding="0" border="0" class="EditFormHeader">
    <tbody>
        <tr>
            <td align="right" colspan="2" style="height: 2px;" class="EditFormHeader_content_aboveflip">&#160;</td>
        </tr>
        <tr>
            <td style="vertical-align: top; width: 8px;"><asp:Image runat="server" ImageAlign="top" ImageUrl="~/images/elementer/flipp.gif"  /></td>
            <td align="left" style="padding-left: 5px;" class="EditFormHeader_content">
            
                                <table border="0" cellpadding="0" width="95%">   
                         <tr><td style="width: 70px;">    
                         
                        
                            <asp:LinkButton Font-Names="Tahoma, Verdana"  ForeColor="#100e37" CssClass="FlagLinkButton" ID="hyperLinkGraphText" runat="server" NavigateUrl="~/gagui/webforms/olapgraph.aspx" Text="Graph" ></asp:LinkButton>
                          </td><td>
                          <radarcube:TOLAPToolbox ExportPDFButton-Visible="true" ID="TOLAPToolbox2" runat="server"  GridID="TOLAPGrid1" ToolboxSettings-BgImage=" " ToolboxSettings-CloserImage="~/images/cube/invisible.png" ToolboxSettings-SeparatorImage="~/images/cube/invisible.png" ToolboxSettings-StarterImage="~/images/cube/invisible.png" SaveLayoutButton-Image="~/images/cube/save.gif" LoadLayoutButton-Image="~/images/cube/load.png" AllAreasButton-Image="~/images/cube/AllAreas.gif" AllAreasButton-PressedImage="~/images/cube/AllAreas.gif" DataAreaButton-Image="~/images/cube/DataArea.gif" DataAreaButton-PressedImage="~/images/cube/DataArea.gif" AllAreasButton-Tooltip="View All Areas" DataAreaButton-Tooltip="View Data Area">
                         
                         <AllAreasButton Visible="true" />
                         <DataAreaButton Visible="true" />
                         <PivotAreaButton Visible="false" />
                        <ExportPDFButton  Visible="true" Tooltip="Export OLAP slice to {0} format" />
                        <ExportJPEGButton  Visible="true" Tooltip="Export OLAP slice to {0} format" />
                        <ConnectButton Visible="False">
                        </ConnectButton>
                        <ExportHTMLButton Visible="true" />
                        <ExportCSVButton Visible="false" Tooltip="Export OLAP slice to {0} format" />
                        <ExportXLSButton Tooltip="Export OLAP slice to {0} format" />
                        <MDXQueryButton  Visible="false" />
                        <SaveLayoutButton Visible="true" />
                            </radarcube:TOLAPToolbox> 
                            
                            
                            
                            
                          </td>
                          <td align="right">
                            <a class="FlagLinkButton" href="javascript:window.close();"><asp:Image  runat="server" ImageUrl="~/images/ikon/orangecross.gif" /> Close</a>
                          </td></tr>

                          </table>         
            </td>
        </tr>
    </tbody>
</table>
            
            
            
            
            
            
<table width="100%" cellspacing="0" cellpadding="0" border="0" >
    <tbody>
        <tr>
            <td style="padding: 5px 5px 5px 5px">
                <asp:Label ID="lblErrorMessage" CssClass="MessageError" runat="server" />
                
                <radarcube:TOLAPGrid CssClass="cubeTolapGrid" Width="1000px" Font-Names="Tahoma,Verdana,Arial" Font-Size="12px"  ID="TOLAPGrid1" runat="server" CellPadding="0" AllowEditing="True" BackColor="#c3c9d5" CellSpacing="1"  CubeID="TMDCube1" ForeColor="Black" >
                    <InternalGridStyle CssClass="cubeInternalGridStyle" CellPadding="2" HorizontalAlign="Left" CellSpacing="0" GridLines="Both" BackColor="#c3c9d5" />
                    
                    
                    <PivotPanelStyle CssClass="cubePivotPanelStyle" Font-Size="9px"   ForeColor="black" BorderColor="#9eadc2" BorderStyle="Solid" BorderWidth="1px" CellPadding="0" BackColor="#ebedf4" />
                    
                    
                    
                    <PivotAreaStyle BackColor="white" BorderColor="#dde0e5" BorderStyle="Inset" BorderWidth="1px" />
                  
                  
                  
                    <CubeStructureStyle>
                        <LeafNodeStyle Font-Bold="True" ForeColor="Black" />
                        <ControlStyle Font-Bold="False" ForeColor="Black" />
                    </CubeStructureStyle>
                    
                    <LevelCellStyle CssClass="cubeLevelCellStyle"  Font-Names="Tahoma,Verdana,Arial" Font-Size="12px"  Font-Bold="True" ForeColor="#003366" BorderColor="#dde0e5" BorderStyle="Solid" BorderWidth="1px" />    
                    <MemberCellStyle BackColor="#e7e9ef" Font-Names="Tahoma,Verdana,Arial;" Font-Size="12px"  ForeColor="black" BorderColor="#dde0e5" BorderStyle="Solid" BorderWidth="1px" />
                    <MemberCellStyleAlternate BackColor="#e7e9ef" Font-Names="Tahoma,Verdana,Arial;" Font-Size="12px"  BorderColor="#dde0e5" BorderStyle="Solid" BorderWidth="1px" ForeColor="black" />
                    <MemberGroupCellStyle Font-Names="Tahoma,Verdana,Arial" Font-Size="11px" Font-Bold="False" Font-Italic="True" BackColor="#6A8CBC" BorderColor="#dde0e5" BorderStyle="Solid" BorderWidth="1px" ForeColor="black" />
                    <MemberTotalCellStyle Font-Names="Tahoma,Verdana,Arial" Font-Size="12px" Font-Italic="False" BackColor="#e7e9ef" Font-Bold="True" BorderColor="#dde0e5" BorderStyle="Solid" BorderWidth="1px" ForeColor="#003366" />
                    <PopupMenuStyle BackColor="#f5f3f3" BorderColor="#f5f3f3" BorderStyle="Outset" BorderWidth="2px" />
                    <DataTotalCellStyle BackColor="#e7e9ef" Font-Names="Tahoma,Verdana,Arial" Font-Size="11px"  Font-Bold="True" ForeColor="#003366" BorderColor="#dde0e5" BorderWidth="1px" HorizontalAlign="Right" BorderStyle="Solid" />
                    <DataCellStyleAlternate  CssClass="cubeDataCellStyleAlternate" BackColor="#e7e9ef" BorderColor="#dde0e5" BorderWidth="1px" ForeColor="Black" HorizontalAlign="Right" BorderStyle="Solid" />
                    <DataCellStyle CssClass="cubeDataCellStyleAlternate" HorizontalAlign="Right" Font-Names="Tahoma,Verdana,Arial" Font-Size="11px" ForeColor="Black" BackColor="#F5F8FA" BorderColor="#dde0e5" BorderStyle="Solid" BorderWidth="1px" />
                    <DataVerticalTotalCellStyleAlternate BackColor="#e7e9ef" BorderColor="#dde0e5" Font-Bold="True" BorderWidth="1px" ForeColor="#003366" HorizontalAlign="Right" BorderStyle="Solid" />
                    <DataVerticalTotalCellStyle BackColor="#F5F8FA" BorderColor="#dde0e5" Font-Bold="True" BorderWidth="1px" ForeColor="#003366" HorizontalAlign="Right" BorderStyle="Solid" />
                    <DataGrandTotalCellStyle BackColor="#c3c9d5" Font-Bold="True" ForeColor="#003366" BorderColor="#dde0e5" BorderWidth="1px" HorizontalAlign="Right" BorderStyle="Solid" />
                    <MemberVerticalTotalCellStyleAlternate BackColor="#c3c9d5" BorderColor="#dde0e5" BorderStyle="Solid" BorderWidth="1px" Font-Bold="True" Font-Italic="False" ForeColor="#003366" />
                    <MemberVerticalTotalCellStyle BackColor="#c3c9d5" BorderColor="#dde0e5" BorderStyle="Solid" BorderWidth="1px" Font-Bold="True" Font-Italic="False" ForeColor="#003366" />
                    <DraggedItemStyle Font-Bold="false" />
                    <HierarchyEditorStyle />
                    <PopupMenuHoverStyle />
                    <PopupMenuStyle />
                    
                        
            </radarcube:TOLAPGrid>
            
            
            
            </td>
        </tr>
    </tbody>
</table>





 <div>

    
        
    </div>
    <div>
       
    </div>