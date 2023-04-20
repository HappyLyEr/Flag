using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using RadarSoft.RadarCube.Web;
using RadarSoft.RadarCube.Common;
using Telerik.WebControls;

namespace GASystem.GAGUI.GAControls.OLAP
{

    public class CubeView : System.Web.UI.UserControl
    {
        private const string FLAGCUBECONNECTIONSTRING = "FlagCubeConnectionString";
        private const string FLAGCUBENAME = "FlagCubeName";

        protected TOLAPGrid TOLAPGrid1;
        protected CubeGraph CubeGraph1;
        protected TOLAPToolbox TOLAPToolbox2;
        //protected LinkButton HyperLink1;

        protected LinkButton hyperLinkGraphText;
        protected Label lblErrorMessage;
        private TMDCube tmdCube2;


        private TOLAPExport myTOLAPExport;
        private DataGrid dg = new DataGrid();
      
        private RadAjaxPanel myRadAjaxPanel = new RadAjaxPanel();

        public event EventHandler GraphClicked;

       
        protected void Page_Load(object sender, EventArgs e)
        {
            lblErrorMessage.Visible = false;

            tmdCube2.ConnectionString = this.connectionString;
            tmdCube2.CubeName = this.cubeName;

            //TOLAPGrid1.OnRenderCell += new RenderCellEventHandler(TOLAPGrid1_OnRenderCell);

            //using a ajax callback in order to refill a datatable session of the cube data. this datatable is used for 
            //generating graphs.
            TOLAPGrid1.ClientCallbackFunction = myRadAjaxPanel.ClientID + ".AjaxRequest()";
            TOLAPGrid1.AllowPaging = false;
            if (!IsPostBack)
            {
                TOLAPGrid1.CubeID = tmdCube2.ID;
                tmdCube2.Active = true;

                //tmdCube2.ClearTempDirectory();


                

               // tmdCube2.DataBind();
               // TOLAPGrid1.DataBind();

          
            }

            TOLAPToolbox2.ExportHTMLButton.Visible = true;
            TOLAPToolbox2.PivotAreaButton.Visible = false;
            TOLAPToolbox2.ConnectButton.Visible = false;
           

           
            

        }

        //void  TOLAPGrid1_OnRenderCell(TOLAPGrid sender, RenderCellEventArgs e)
        //{
        //    try
        //    {
        //        IDataCell dc = e.Cell as IDataCell;
        //        if ((dc != null) && (dc.Data != null))
        //        {
        //            if ((dc.Address.Measure != null) && (dc.Data.GetType() == typeof(System.Double)))
        //            {
        //                e.Text = String.Format("{0:0.00}", dc.Data);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //ignore
        //    }
        //}

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
           
            this.Controls.Add(myRadAjaxPanel);
            myTOLAPExport = new TOLAPExport();
            myTOLAPExport.ID = "flagOLAPExport";
            
            this.Controls.Add(myTOLAPExport);
            myTOLAPExport.GridID = TOLAPGrid1.ID;
            TOLAPToolbox2.ExportID = myTOLAPExport.ID;

            tmdCube2 = new TMDCube();
            tmdCube2.ID = new GASystem.GUIUtils.GeneralQueryStringUtils(this.Request).getSingleAlphaNumericQueryStringParam(GASystem.DataModel.GADataClass.GAAnalysis.ToString().Substring(2) + "rowid").ToString() + "cube";
            this.Controls.Add(tmdCube2);

            hyperLinkGraphText.Click += new EventHandler(hyperLinkGraphText_Click);



        }

        void hyperLinkGraphText_Click(object sender, EventArgs e)
        {
             if (GraphClicked != null)
                GraphClicked(this, new EventArgs());
        }

        public DataTable getDataSet()
        {
             DataTable dtt = new DataTable("qqq");
            TOLAPGrid1.GetOLAPData(dtt, System.Drawing.Rectangle.Empty, TCellsetTableMode.ctmWholeCellsetExceptTotals);
            return dtt;
        }
        
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (!this.Page.IsPostBack)
            {
                if (CubeLayoutFile != string.Empty)
                    TOLAPGrid1.ShowAreasMode = TrsShowAreasOLAPGrid.rsDataOnly;
                else
                    TOLAPGrid1.ShowAreasMode = TrsShowAreasOLAPGrid.rsAll;

                try
                {

                    if (CubeLayoutFile != string.Empty)
                        TOLAPGrid1.Serializer.ReadXML(getLayoutFileWithPath());
                }
                catch (Exception ex)
                {
                    lblErrorMessage.Text = GASystem.AppUtils.Localization.GetErrorText("ErrorAddingCubeLayoutFile");
                    lblErrorMessage.Visible = true;
                }

            }

            

            //DataTable dtt = new DataTable("qqq");
            //TOLAPGrid1.GetOLAPData(dtt, System.Drawing.Rectangle.Empty, TCellsetTableMode.ctmWholeCellsetExceptTotals);
            //Session["OLAPDataTable"] = dtt;

        
            //set hyperlink chartname 
            string chartName  = string.Empty;
            foreach (TMeasure measure in TOLAPGrid1.Measures)
                if (measure.Visible)
                    chartName += measure.DisplayName + "  ";
          //  HyperLink1.NavigateUrl = "~/gagui/webforms/olapgraph.aspx?chartName=" + chartName;
            hyperLinkGraphText.Text = GASystem.AppUtils.Localization.GetGuiElementText("graph");
        //    hyperLinkGraphText.NavigateUrl = "~/gagui/webforms/olapgraph.aspx?chartName=" + chartName;

        }

        /// <summary>
        /// MS Analysis services connection string. Get this setting from web.config.
        /// </summary>
        private string connectionString
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings.Get(FLAGCUBECONNECTIONSTRING);
            }
        }

        private string _cubeName = string.Empty;
        /// <summary>
        /// OLAP cube name. 
        /// Will use default cube name from web.config if not specified.
        /// </summary>
        public string cubeName
        {
            get
            {
                if (_cubeName == string.Empty)
// Tor 20160127                    _cubeName = System.Configuration.ConfigurationManager.AppSettings.Get(FLAGCUBENAME);
                    _cubeName= new GASystem.AppUtils.FlagSysResource().GetResourceString(FLAGCUBENAME);
                return _cubeName;
            }

            set
            {
                _cubeName = value;
            }
        }


        private string _displayName = string.Empty;
        /// <summary>
        /// OLAP cube name. 
        /// Will use default cube name from web.config if not specified.
        /// </summary>
        public string DisplayName
        {
            get
            {
                return _displayName;
            }

            set
            {
                _displayName = value;
            }
        }



        private string _layoutfile = string.Empty; 
         /// <summary>
         /// Name of a xml cube layout file used for displaying a predefined cube layout to the user. 
         /// Name must be specified without the .xml ending. 
         /// This property is not mandatory. When no layout is specified, will the user be presented with a full
         /// workarea for 
         /// </summary>
        public string CubeLayoutFile
        {
            get
            {
                return _layoutfile;
            }

            set {
                _layoutfile = value;
            }
        }

        /// <summary>
        /// generate full path for layoutfile
        /// </summary>
        /// <returns></returns>
        private string getLayoutFileWithPath() 
        {
            return BusinessLayer.File.URLPath + CubeLayoutFile;
        }


    }
}