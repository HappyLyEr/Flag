<%@ Register TagPrefix="uc1" TagName="ListDataRecords" Src="../../UserControls/ListDataRecords.ascx" %>
<%@ Control Language="c#" AutoEventWireup="false" Inherits="GASystem.GAControls.Workflow.WorkitemStatus" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<%@ Register TagPrefix="uc1" TagName="ViewDataRecord" Src="../../UserControls/ViewDataRecord.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AddWorkitemParticipant" Src="AddWorkitemParticipant.ascx" %>
<%@ Register TagPrefix="uc1" TagName="DelegateWorkitem" Src="DelegateWorkitem.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CloseWorkitem" Src="closeworkitem.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CloseNCWorkitem" Src="closencworkitem.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AddWorkitemComment" Src="addworkitemcomment.ascx" %>
<%@ Register TagPrefix="uc1" TagName="WorkitemAcknowledgement" Src="WorkitemAcknowledgement.ascx" %>
<%@ Register TagPrefix="uc1" TagName="RemoveAddedParticipants" Src="RemoveAddedParticipants.ascx" %>

<TABLE id="Table2">
	
		
		<TR>
			<td class="FieldViewContentCell"><asp:linkbutton CssClass="FlagLinkButton" 
			id="lnkCloseWorkitem" runat="server"><asp:Image id="Image1" ImageUrl="~/images/ikon/ok.gif" CssClass="ga_icon" runat="server" Visible="true"/>
			<asp:Label 
			ID="lblCloseWorkitem" runat = "server">Close Workitem</asp:Label></asp:linkbutton></td>
			
			<td class="FieldViewContentCell"><asp:linkbutton CssClass="FlagLinkButton" 
			id="lnkCloseNCWorkitem" runat="server"><asp:Image id="Image8" ImageUrl="~/images/ikon/ok.gif" CssClass="ga_icon" runat="server" Visible="true"/>
			<asp:Label 
			ID="lblCloseNCWorkitem" runat = "server">Close Workitem</asp:Label></asp:linkbutton></td>
				
			<TD class="FieldViewContentCell"><asp:linkbutton CssClass="FlagLinkButton" 
			id="lnkAddComment" runat="server"><asp:Image id="ImageEditTop" ImageUrl="~/images/ikon/notes.gif" CssClass="ga_icon" runat="server" Visible="true"/>
			<asp:Label 
			ID="lblAddNotes" runat = "server">Add Notes</asp:Label></asp:linkbutton></TD>
			
			<TD class="FieldViewContentCell"><asp:linkbutton CssClass="FlagLinkButton" 
			id="lnkDelegate" runat="server"><asp:Image id="Image2" ImageUrl="~/images/ikon/delegate.png" CssClass="ga_icon" runat="server" Visible="true"/>
			<asp:Label 
			ID="txtDelegate" runat = "server">Delegate</asp:Label></asp:linkbutton></TD>
			
			<td class="FieldViewContentCell"><asp:linkbutton CssClass="FlagLinkButton" 
			id="lnkAddParticipant" runat="server"><asp:Image id="Image3" ImageUrl="~/images/ikon/addparticipant.png"  runat="server" Visible="true"/>
			<asp:Literal 
			ID="txtAddParticipant" runat = "server">Add Participant</asp:Literal></asp:linkbutton></td>
			
			<td class="FieldViewContentCell"><asp:linkbutton CssClass="FlagLinkButton" 
			id="lnkRemoveAddedParticipants" runat="server"><asp:Image id="Image4" ImageUrl="~/images/ikon/removeaddedparticipants.png" CssClass="ga_icon" runat="server" Visible="true"/>
            <asp:Label 
            ID="lblRemoveAddedParticipants" runat = "server">RemoveAddedParticipants</asp:Label></asp:linkbutton></td>
            
            <td class="FieldViewContentCell"><asp:linkbutton CssClass="FlagLinkButton" 
            id="lnkAcknowledgeWorkitem" runat="server"><asp:Image id="Image5" ImageUrl="~/images/ikon/handshake.png" CssClass="ga_icon" runat="server" Visible="true"/>
            <asp:Label 
            ID="lblAcknowledge" runat = "server">Acknowledge</asp:Label></asp:linkbutton></td>
			
			<td class="FieldViewContentCell"><asp:linkbutton CssClass="FlagLinkButton" 
			id="lnkRejectAcknowledgeWorkitem" runat="server"><asp:Image id="Image6" ImageUrl="~/images/ikon/handstop.png" CssClass="ga_icon" runat="server" Visible="true"/>
			<asp:Label 
			ID="lblRejectAcknowledge" runat = "server">Reject Acknowledge</asp:Label></asp:linkbutton></td>
			
			<td class="FieldViewContentCell"><asp:linkbutton CssClass="FlagLinkButton" 
			id="lnkRejectAcknowledgeNCWorkitem" runat="server"><asp:Image id="Image9" ImageUrl="~/images/ikon/handstop.png" CssClass="ga_icon" runat="server" Visible="true"/>
			<asp:Label 
			ID="lblRejectAcknowledgeNC" runat = "server">Reject Acknowledge</asp:Label></asp:linkbutton></td>
			
			<td class="FieldViewContentCell"><asp:HyperLink CssClass="FlagLinkButton" 
			id="hyperLinkEdit" runat="server" Visible="False"> <asp:Image id="Image7" ImageUrl="~/images/ikon/edit.gif" CssClass="ga_icon" runat="server" Visible="true"/>
			<asp:Literal 
			ID="txtEditWorkitem" runat = "server">Edit Workitem</asp:Literal></asp:HyperLink></td>
		</TR>
	
	</TABLE>
	
    <table class="EditForm_Table_wizardSeparator" border="0" cellpadding="0" cellspacing="0">
        <tr >
            <td  class="ViewFormField_ContentLastInRow_Alternate">
                <asp:Label id="lblReactionStatus" Visible="false" runat="server" CssClass="workitemStatus"></asp:Label><br />
                <asp:Label id="lblAcknowledgeStatus" Visible="false" runat="server" CssClass="workitemStatus"></asp:Label>
		        <asp:label id="lblMessage" runat="server" Visible="false" EnableViewState="False"></asp:label>
		    </td>
		</tr>
    </table>
	
	
	<uc1:delegateworkitem id="MyDelegateWorkitem" runat="server" Visible="False"></uc1:delegateworkitem>
	<uc1:addworkitemparticipant id="MyAddWorkitemParticipant" runat="server" Visible="False"></uc1:addworkitemparticipant>
    <uc1:CloseWorkitem id="MyCloseWorkitem" runat="server" Visible="False"></uc1:CloseWorkitem>
    <uc1:CloseNCWorkitem id="MyCloseNCWorkitem" runat="server" Visible="False"></uc1:CloseNCWorkitem>
    <uc1:AddWorkitemComment id="MyAddWorkitemComment" runat="server" Visible="False"></uc1:AddWorkitemComment>
    <uc1:RemoveAddedParticipants id="MyRemoveAddedParticipants" runat="server" Visible="False"></uc1:RemoveAddedParticipants>
    
  <table class="EditForm_Table_wizardSeparator" border="0" cellpadding="0" cellspacing="0">
    <tr >
        <td><asp:Label runat="server" ID="separatorLabel"> Workitem</asp:Label></td>
    </tr>
  </table>
    
	
<uc1:viewdatarecord id="MyViewDataRecord" runat="server"></uc1:viewdatarecord>

<br>
	<asp:label id="lblErrorMessage" runat="server" EnableViewState="False" BackColor="Transparent"
		ForeColor="Red"></asp:label>
<P></P>
