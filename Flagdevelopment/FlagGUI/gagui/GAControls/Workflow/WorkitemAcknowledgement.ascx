<%@ Control Language="C#" AutoEventWireup="false"  Inherits="GASystem.GAControls.Workflow.WorkitemAcknowledgement" %>
<asp:Panel ID="Panel1" runat="server" >
  <table class="EditForm_Table_wizardSeparator" border="0" cellpadding="0" cellspacing="0" width="100%"> 
  <tr><td><asp:Label ID="lblAcknowledgeMsg" runat="server" Text="Label"></asp:Label></td></tr>
   <tr><td>   <asp:Button ID="acknowledge" runat="server" Text="Acknowledge" />
    <asp:Button ID="acknowledgeReject" runat="server" Text="Reject Acknowledgment" /></td></tr>
   
 
    </table>
    
    </asp:Panel>
