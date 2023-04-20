<%@ Control Language="c#" AutoEventWireup="false" Inherits="GASystem.GAControls.WorkflowStatus" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>



<p><asp:label id=LabelWorkFlowStatus runat="server"></asp:label><asp:panel 
id=PanelStartWorkFlow runat="server">Workflow has not been 
started. 
<asp:LinkButton id=LinkButtonStartWorkFlow runat="server">Start workflow now</asp:LinkButton></asp:panel><asp:panel 
id=PanelWorkFlowStarted runat="server">Workflow was started 
on 
<asp:Label id=LabelWorkflowStartDate runat="server"></asp:Label>
<asp:HyperLink id=HrefReportDetails runat="server">View Details</asp:HyperLink></asp:panel></P>
<script language=javascript>
var Fwin= null;

function openReportWindow(url) {
	
	Fwin = window.open(url, 'pick_dialog', 'toolbar=no, directories=no, location=no, status=no, menubar=no, resizable=yes, scrollbars=yes, width=400, height=600');
	
	if (Fwin && !Fwin.closed)
		Fwin.focus();

	}
</script>

