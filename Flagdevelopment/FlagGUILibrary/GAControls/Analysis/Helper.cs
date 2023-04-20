using System.Web.UI;
using DataDynamics.Analysis.Web;



namespace GASystem.GAGUI.GAControls.Analysis
{
    /// <summary>
    /// Summary description for Helper
    /// </summary>
    public static class Helper
    {
        public static void AddSizingScript(
            PivotView pivotView, ClientScriptManager clientScript)
        {
            string autosizeScript = string.Format(
                "	function adjustSize(){{\n" +
                "		var winSize = DD.Dom.getBrowserWindowSize();\n" +
                "		var container = document.getElementById(\"{0}\");\n" +
                "		var setSize = DD.PivotView.SetSize;\n" +
                "		if (!setSize('{0}', winSize.width, (winSize.height-container.offsetTop - 16))) {{ \n" +
                "			setTimeout(adjustSize, 100); \n" +
                "			return; \n" +
                "		}}\n" +
                "	}};\n" +
                "	DD.attachEvent(window, 'resize', adjustSize);\n" +
                "	adjustSize();\n", pivotView.ID);

            clientScript.RegisterStartupScript(
                typeof(PivotView), "autosizeScript", autosizeScript, true);
        }
    }
}