<%@ control language="c#" autoeventwireup="false" inherits="GASystem.ImportExcelCtrl" targetschema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ register assembly="RadUpload.Net2" namespace="Telerik.WebControls" tagprefix="telerik" %>
<%@ register assembly="RadAjax.Net2" namespace="Telerik.WebControls" tagprefix="telerik" %>
 
<asp:linkbutton cssclass="FlagLinkButtonCombined" id="btnImportFromExcel" visible="False"
    runat="server" href="javascript: return;"
    causesvalidation="false" onclientclick="showHideModalImportExcel(false); return false;">
     <%=this.Text %>
</asp:linkbutton>

<div id="DivBackDrop" class="modal-backdrop noDisplay" runat="server"></div>
<div id="DivContainer" class="noDisplay" runat="server">
 
    <asp:panel runat="server" class="closeBtn">
        <telerik:RadAjaxPanel runat="server" EnableAJAX="True">
            <asp:LinkButton ID="btnClosePanel" runat="server" 
                            OnClientClick="showHideModalImportExcel(true)">X</asp:LinkButton>
        </telerik:RadAjaxPanel>
    </asp:panel>

    <telerik:radupload id="RadUploadXLS" runat="server"
        maxfileinputscount="1"
        maxfilesize="10485760"
        allowedfileextensions=".xlsx"
        overwriteexistingfiles="true"
        controlobjectsvisibility="ClearButtons" />

    <asp:customvalidator runat="server" id="CustomValidatorXLS" display="Dynamic" CssClass="importXlsLabels"
                         clientvalidationfunction="validateRadUploadXLS">        
        <%= this.ValidationImportExcelText %>
    </asp:customvalidator>

    <asp:Label ID="LblSuccessMessage" runat="server" Visible="False" CssClass="successImportExcel importXlsLabels"></asp:Label>

    <asp:linkbutton cssclass="FlagLinkButtonCombined" id="BtnProcessImportXLS" runat="server" 
                    onclick="btnProcessImportXLS_OnClick">
        <img  alt="" src="gagui/Images/up.gif" /><%= this.Text %>
    </asp:linkbutton>
</div>
<script type="text/javascript">
    window.addEventListener('load', onLoadXLSImportUC);

    var isXlsImportOpen = false;
    var xlsImportDvContainerID = "<%= DivContainer.ClientID %>";
    var xlsImportDvBackDropID = "<%= DivBackDrop.ClientID %>";
    var xlsImportRadUpload;

    function onLoadXLSImportUC() {
        setTimeout(function () {
                <%-- prints directly the boolean --%>
            if (<%= this.IsOpenModalView.ToString().ToLower() %>) {
                showHideModalImportExcel(false);
            }
        }, 0);
    }

    function validateRadUploadXLS(source, arguments) {
        arguments.IsValid = false;
        xlsImportRadUpload = <%= RadUploadXLS.ClientID  %>;

        if (isXlsImportOpen === false) {
            arguments.IsValid = true;
            return;
        }

        if (!xlsImportRadUpload.ValidateExtensions()) {
            setInvalidExtensionMessage();
            hideSucceedLabel();
            return;
        }

        var inputs = xlsImportRadUpload.GetFileInputs();

        for (var i = 0; i < inputs.length; i++) {
            if (inputs[i].value !== "") {
                arguments.IsValid = true;
                processExcelImport();
                break;
            }
        }

        setInvalidExtensionMessage();
        hideSucceedLabel();
    }

    function setInvalidExtensionMessage() {
        $("#<%= CustomValidatorXLS.ClientID%>").text("<%= this.InvalidFileTypeText%>").append("<br />");
    }

    function hideSucceedLabel() {
        $("#<%= LblSuccessMessage.ClientID %>").hide();
    }

    function showHideModalImportExcel(isHide) {
        $("#" + xlsImportDvContainerID).toggleClass("noDisplay", isHide);
        $("#" + xlsImportDvBackDropID).toggleClass("noDisplay", isHide);

        isXlsImportOpen = !isHide;
    }

    function processExcelImport() {
        $("body").css("cursor", "progress");
    }

</script>

<style type="text/css">
    .closeBtn {
        position: absolute;
        right: -3px;
        top: -5px;
        background: white;
        border-radius: 50%;
        width: 20px;
        text-align: center;
        cursor: pointer;
        border: 1px red solid;
    }

    .noDisplay {
        display: none !important;
    }

    #<%= DivContainer.ClientID %> {
        position: fixed;
        top: 50%;
        left: 50%;
        transform: translate(-50%, -50%);
        width: auto;
        height: auto;
        background: #f5f8fa;
        padding: 10px;
        z-index: 1041;
        display: block;
        min-width: 380px;
    }

    .modal-backdrop {
        position: fixed;
        top: 0;
        right: 0;
        bottom: 0;
        left: 0;
        z-index: 1040;
        background-color: #000000;
        opacity: 0.6;
    }

    .successImportExcel {
        color: green;
    }

    .importXlsLabels {
        margin-bottom: 12px;
        display: block;
        padding: 5px;
    }

    .importXlsLabels:after {
        content: '\a';
        white-space: pre;
    }

</style>
