<%@ Page Language="C#" AutoEventWireup="true" Inherits="GASystem.WebForms.Olap" %>

<%@ Register Src="~/gagui/GAControls/OLAP/CubeView.ascx" TagName="CubeView" TagPrefix="uc1" %>
<%@ Register Src="~/gagui/GAControls/OLAP/CubeGraph.ascx" TagName="CubeGraph" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Data Drilldown</title>
    
    
    <link id="_flag_Portals__default_" rel="stylesheet" type="text/css" href="../../Portals/_default/default.css" />
<link id="_flag_Portals__default_Skins_norblue_" rel="stylesheet" type="text/css" href="../../Portals/_default/Skins/norblue/skin.css" />
<link id="_flag_Portals__default_Containers_FlagContainer_" rel="stylesheet" type="text/css" href="../../Portals/_default/Containers/FlagContainer/container.css" />
<link id="_flag_Portals_0_" rel="stylesheet" type="text/css" href="../../Portals/0/portal.css" />

</head>
<body>
    <form id="form1" runat="server">
    
          
<TABLE class="pagemaster" width="100%" border="0" cellspacing="0" cellpadding="0">
<tr>
<td valign="top">

<table width="100%" class="brandmaster" border="0" align="center" cellspacing="0" cellpadding="0">
<tr>
<td class="BrandMasterContent">




<TABLE class="skinmaster"  border="0" align="center" cellspacing="0" cellpadding="0">
<tr>
<td id="dnn_ControlPanel" class="ControlPanelContainer" valign="top" align="center"></td>

</tr>


<tr><td valign="top">


    

</td></tr>





<tr><td valign="top" height="100%">

  
  

   


<table border="0" cellspacing="0" cellpadding="0" width="100%" summary="Module Design Table">
  <tr>
    <td id="dnn_ctr2017_ContentPane" class="DNNAlignleft">
   
<div id="dnn_ctr2017_ModuleContent">
	<div id="dnn_ctr2017_HtmlModule_HtmlModule_lblContent" class="Normal">
	
	                    <table width="100%" cellspacing="0" cellpadding="0" border="0" class="FlagWelcomeBox">
                        <tbody>
                            <tr>
                                <td style="border-bottom: 1px solid rgb(221, 224, 229); padding-top: 10px;">
                                <div class="FlagContext_SubCaptionFlagClass"><asp:Label runat="server" ID="labelTitle"></asp:Label></div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                        
                        
                        
                        
                        <div>
                            <uc1:CubeView id="CubeView1" runat="server">
                            </uc1:CubeView></div>
                            
                            <uc1:CubeGraph id="CubeGraph1" visible="false" runat="server">
                            </uc1:CubeGraph></div>
                            
                            
                            
                                   </td>
                            </tr>
                        </tbody>
                    </table>

</div>

</div></td>

  </tr>
</table>



</td></tr>




<tr>
<td valign="top" align="center"></td>
</tr>

<tr><td valign="top" align="center">
</td></tr>

</TABLE>










</td>
<td class="BrandLabel">
 <img src="../../images/Norsolution_Merke_bakgrunn.gif" />
</td>
</tr>
</table>


</td>

</tr>
</TABLE>
        
        
        
        
        
    </form>
</body>
</html>
