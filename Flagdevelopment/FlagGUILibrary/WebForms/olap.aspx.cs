using System;
using System.Collections.Generic;
using System.Text;
using GASystem.GUIUtils;
using GASystem.DataModel;

namespace GASystem.WebForms
{
    public class Olap : System.Web.UI.Page
    {
        protected GASystem.GAGUI.GAControls.OLAP.CubeView CubeView1;
        protected GASystem.GAGUI.GAControls.OLAP.CubeGraph CubeGraph1;
        protected System.Web.UI.WebControls.Label labelTitle;
        
        public Olap()
        {
            
        }

        private string _displayName;

        public string DisplayName
        {
            get { return _displayName; }
            set { _displayName = value; }
        }
	


        protected override void OnInit(EventArgs e)
        {
            GeneralQueryStringUtils requestUtil = new GeneralQueryStringUtils(this.Page.Request);
            string cubeLayout = requestUtil.getSingleAlphaNumericQueryStringParam("cubelayout");
            string cubeName = requestUtil.getSingleAlphaNumericQueryStringParam("cubename");
            _displayName = requestUtil.getSingleAlphaNumericQueryStringParam("displayname");

            //get datasource
            GASystem.GUIUtils.QuerystringUtils myQueryString = new GASystem.GUIUtils.QuerystringUtils(GASystem.DataModel.GADataClass.GAAnalysis, this.Page.Request);
            int rowId = myQueryString.GetRowId();


            if (rowId != -1)
            {
                BusinessLayer.BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAAnalysis);
                AnalysisDS ds = bc.GetByRowId(rowId) as AnalysisDS;

                if (ds == null || ds.GAAnalysis.Rows.Count != 1)
                    throw new GAExceptions.GADataAccessException("Could not find analysis specification");

                cubeName = ds.GAAnalysis[0].FileName;
                if (!ds.GAAnalysis[0].IsLayoutFileNull())
                    cubeLayout = ds.GAAnalysis[0].LayoutFile;
                _displayName = ds.GAAnalysis[0].Subject;
            }

            if (_displayName == string.Empty)
                _displayName = cubeName;
            
            CubeView1.CubeLayoutFile = cubeLayout;
            CubeView1.cubeName = cubeName;
            CubeView1.DisplayName = _displayName;

            CubeView1.GraphClicked += new EventHandler(CubeView1_GraphClicked);
            CubeGraph1.TableClicked += new EventHandler(CubeGraph1_TableClicked);
            base.OnInit(e);
            
        }

        void CubeGraph1_TableClicked(object sender, EventArgs e)
        {
            CubeGraph1.Visible = false;
            CubeView1.Visible = true;
        }

        void CubeView1_GraphClicked(object sender, EventArgs e)
        {
            CubeGraph1.Visible = true;
            CubeView1.Visible = false;
            CubeGraph1.DataSet = CubeView1.getDataSet();
            CubeGraph1.setChart();
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            labelTitle.Text = DisplayName;
        }
    }
}
