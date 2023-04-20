using System;
using System.Collections.Generic;
using System.Text;
using GASystem.GUIUtils;

namespace GASystem.WebForms
{
    public class OLAPGraph : System.Web.UI.Page
    {
        protected GASystem.GAGUI.GAControls.OLAP.CubeGraph CubeGraph1;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            GeneralQueryStringUtils requestUtil = new GeneralQueryStringUtils(this.Page.Request);
            string chartName = requestUtil.getSingleAlphaNumericQueryStringParam("chartname");
            CubeGraph1.ChartName = chartName;
        }
    }
}
