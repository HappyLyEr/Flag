using System;
using System.IO;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using System.Web.UI.Design;

namespace GASystem.WebControls.EditControls.HazardMatrix
{
	/// <summary>
	/// Summary description for HazardMatrixControlDesigner.
	/// </summary>
	public class HazardMatrixControlDesigner : ControlDesigner
	{
		public HazardMatrixControlDesigner()
		{
		}

		public override string GetDesignTimeHtml()
		{
			HazardMatrixControl ctrl = new HazardMatrixControl();

			StringWriter sw = new StringWriter();
			HtmlTextWriter tw = new HtmlTextWriter(sw);

			sw.Write(ctrl.getDesignerHTML());

			return sw.ToString();
		}


		
	}
}
