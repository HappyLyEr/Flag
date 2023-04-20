using GASystem.GAGUI.GUIUtils;
using GASystem.WebControls.ListControls;
using System;
using System.Collections;
using System.Data;
using System.Web.UI.WebControls;
using System.Drawing;
using GASystem.BusinessLayer;
using GASystem.AppUtils;
using GASystem.GAGUIEvents;
using GASystem.DataModel;
using log4net;
using Telerik.WebControls;

namespace GASystem.GAGUI.GAControls.Export
{
    class ExportListToExcel : System.Web.UI.WebControls.WebControl
    {
        private Telerik.WebControls.RadGrid dg;
        private string _dataClass;
        private DataSet _recordDataSet;


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            dg = new RadGrid();
            dg.EnableViewState = false;
            dg.AutoGenerateColumns = false;

            dg.ID = "dg1export";
            dg.MasterTableView.EditMode = GridEditMode.InPlace;

            dg.BorderWidth = Unit.Pixel(0);
            dg.GridLines = GridLines.None;
            this.Controls.Add(dg);
        }


        public void FillGrid()
        {
            if (dg != null && RecordsDataSet != null && RecordsDataSet.Tables[DataClass] != null)
            {
                GenerateExcelDataGrid();

                dg.DataSource = RecordsDataSet;
                dg.DataMember = this.DataClass.ToString();


                dg.DataBind();
            }
        }

        private void GenerateExcelDataGrid()
        {
            dg.Columns.Clear();
            
            if (RecordsDataSet == null || RecordsDataSet.Tables[DataClass] == null) return;

            dg.Skin = "FlagGrid";

            //check for rowid column
            string rowidColumn = DataClass.ToString().Substring(2) + "rowid";
            if (!RecordsDataSet.Tables[DataClass].Columns.Contains(rowidColumn))
                throw new GAExceptions.GADataAccessException("Recorddataset for list " + DataClass.ToString() + " does not contain a rowid column");


            GAListExcelColumnContainerFactory factory = new GAListExcelColumnContainerFactory();
            ArrayList myBoundColumns = new ArrayList();
            foreach (DataColumn c in RecordsDataSet.Tables[DataClass].Columns)
            {
                GAListExcelColumnContainer tmpColumn = factory.getGAListColumnContainer(c, false, setFormatting(c)); //    = new GAListColumnContainer(c);

                if (tmpColumn != null && DisplayExcelColumn(tmpColumn))  // !tmpColumn.getFieldDescription().HideInSummary)
                {
                    myBoundColumns.Add(tmpColumn);
                }
            }

            myBoundColumns.Sort();

            foreach (GAListExcelColumnContainer c in myBoundColumns)
                dg.Columns.Add(c.GridColumn);

        }

        protected bool DisplayExcelColumn(GAListExcelColumnContainer Col)
        {
            return !Col.getFieldDescription().HideInExcel;
        }

        private string setFormatting(DataColumn bc)
        {
            string dataType = null;
            switch (bc.DataType.ToString())
            {
                case "System.Int32":
                    dataType = "{0:#,###}";
                    break;
                case "System.Decimal":
                    dataType = "{0:c}";
                    break;
                case "System.DateTime":
                    dataType = "{0:d}";//+System.Threading.Thread.CurrentThread.CurrentUICulture.DateTimeFormat.ShortDatePattern;//"{0:dd.MM.yyyy}";			
                    //dataType=System.Threading.Thread.CurrentThread.CurrentUICulture.DateTimeFormat.sShortDatePattern;
                    break;
                case "System.String":
                    dataType = "{0,10}";
                    break;
                default:
                    dataType = "";
                    break;
            }
            return dataType;
        }


        //public void prepareExcelExport()
        //{
        //    GenerateExcelDataGrid();
        //    dg.DataSource = RecordsDataSet;
        //    dg.DataMember = this.DataClass.ToString();


        //    dg.DataBind();

        //    dg.ExportSettings.ExportOnlyData = true;
        //    dg.ExportSettings.IgnorePaging = true;
        //    dg.ExportSettings.OpenInNewWindow = true;
        //    dg.ExportSettings.FileName = DataClass.ToString() + System.DateTime.Now.Ticks.ToString();

            
        //}

        public void ExportToExcel()
        {
            dg.Skin = "FlagGrid";
            dg.GridLines = GridLines.Both;
            //dg.Width = new Unit("600px");
            dg.ClientSettings.ApplyStylesOnClient = false;

           
    
            dg.ItemStyle.VerticalAlign = VerticalAlign.Top;
            dg.AlternatingItemStyle.VerticalAlign = VerticalAlign.Top;
            dg.ItemStyle.Width = new Unit("100px");
            
            
            //.CssClass = "excelExport";
            dg.ExportSettings.ExportOnlyData = false;
            dg.ExportSettings.IgnorePaging = true;
            dg.ExportSettings.OpenInNewWindow = true;
            dg.ExportSettings.FileName = DataClass.ToString() + System.DateTime.Now.Ticks.ToString();

            dg.MasterTableView.ExportToExcel();
        }

        public DataSet RecordsDataSet
        {
            set { _recordDataSet = value; }
            get { return _recordDataSet; }
        }

        public string DataClass
        {
            set { _dataClass = value;}
            get { return _dataClass; }
        }

        public void AddSortExpression(GridSortExpression SortExpression)
        {
            this.dg.MasterTableView.SortExpressions.Add(SortExpression);
        }
    }
}
