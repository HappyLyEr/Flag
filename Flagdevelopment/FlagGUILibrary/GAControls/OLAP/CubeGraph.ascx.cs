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
using System.IO;
using Telerik.Charting.Styles;

namespace GASystem.GAGUI.GAControls.OLAP
{

    public class CubeGraph : System.Web.UI.UserControl
    {
        private const string FLAGCUBECONNECTIONSTRING = "FlagCubeConnectionString";
        private const string FLAGCUBENAME = "FlagCubeName";

        protected RadioButtonList graphTypeList;
        protected CheckBox CheckBoxItemLabel;

        protected LinkButton buttonTable;
        protected LinkButton buttonBar;
        protected LinkButton buttonStackedBar;
        protected LinkButton buttonPie;
        protected LinkButton buttonLine;
        protected LinkButton buttonLabelVisibility;


        protected Telerik.WebControls.RadChart RadChart1;  // = new Telerik.WebControls.RadChart();
        public event EventHandler TableClicked;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            //TMDCube1.ConnectionString = this.connectionString;
            //TMDCube1.CubeName = this.cubeName;

            //this.Controls.Add(dg);
            //this.Controls.Add(myRadAjaxPanel);
            //myRadAjaxPanel.Controls.Add(RadChart1);
            this.Controls.Add(RadChart1);
            RadChart1.PreRender += new EventHandler(RadChart1_PreRender);

            buttonBar.Text = GASystem.AppUtils.Localization.GetGuiElementText("BarChart");
            buttonBar.CommandArgument = "Bar";
            buttonBar.Click += new EventHandler(buttonChartType_Click);

            buttonStackedBar.Text = GASystem.AppUtils.Localization.GetGuiElementText("StackedBarChart");
            buttonStackedBar.CommandArgument = "StackedBar";
            buttonStackedBar.Click += new EventHandler(buttonChartType_Click);

            buttonPie.Text = GASystem.AppUtils.Localization.GetGuiElementText("PieChart");
            buttonPie.CommandArgument = "Pie";
            buttonPie.Click += new EventHandler(buttonChartType_Click);

            buttonLine.Text = GASystem.AppUtils.Localization.GetGuiElementText("LineChart");
            buttonLine.CommandArgument = "Line";
            buttonLine.Click += new EventHandler(buttonChartType_Click);

            buttonLabelVisibility.Click += new EventHandler(buttonLabelVisibility_Click);

            buttonTable.Text = GASystem.AppUtils.Localization.GetGuiElementText("Table");
            buttonTable.Click += new EventHandler(buttonTable_Click);
           
            ////init graph type list
            //graphTypeList.Items.Add(new ListItem("Bar Chart", "Bar" ));
            //graphTypeList.Items.Add(new ListItem("Stacked Bar Chart", "StackedBar" ));
            //graphTypeList.Items.Add(new ListItem("Pie Chart", "Pie"));
            //graphTypeList.Items.Add(new ListItem("Line Chart", "Line"));
            //graphTypeList.SelectedIndexChanged += new EventHandler(graphTypeList_SelectedIndexChanged);
            //graphTypeList.AutoPostBack = true;
            //graphTypeList.RepeatDirection = RepeatDirection.Horizontal;

            //CheckBoxItemLabel.AutoPostBack = true;
            //CheckBoxItemLabel.CheckedChanged += new EventHandler(CheckBoxItemLabel_CheckedChanged);
        }

        void buttonTable_Click(object sender, EventArgs e)
        {
            if (TableClicked != null)
                TableClicked(this, new EventArgs());
        }

        void buttonLabelVisibility_Click(object sender, EventArgs e)
        {
            displayItemLabels = !displayItemLabels;
        }

        void buttonChartType_Click(object sender, EventArgs e)
        {
            LinkButton myLinkButton = (LinkButton)sender;

            try
            {
                Telerik.Charting.ChartSeriesType chartType = (Telerik.Charting.ChartSeriesType)Enum.Parse(typeof(Telerik.Charting.ChartSeriesType), myLinkButton.CommandArgument);
                RadChart1.DefaultType = chartType;

                if (chartType == Telerik.Charting.ChartSeriesType.Pie)
                {
                    displayItemLabels = true;
                }
            }
            catch (System.ArgumentException ex)
            {
                RadChart1.DefaultType = Telerik.Charting.ChartSeriesType.Bar;
            }
        }

        //void CheckBoxItemLabel_CheckedChanged(object sender, EventArgs e)
        //{
            
        //}



        //void graphTypeList_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    RadioButtonList myRadioButtonList = (RadioButtonList)sender;
            
        //    try 
        //    {	        
        //        Telerik.Charting.ChartSeriesType chartType = (Telerik.Charting.ChartSeriesType)Enum.Parse(typeof(Telerik.Charting.ChartSeriesType), myRadioButtonList.SelectedValue);
        //        RadChart1.DefaultType = chartType;

        //        if (chartType == Telerik.Charting.ChartSeriesType.Pie)
        //            displayItemLabels = true;
        //    }
        //    catch (System.ArgumentException ex)
        //    {
        //         RadChart1.DefaultType = Telerik.Charting.ChartSeriesType.Bar;
        //    }
        //}

        void RadChart1_PreRender(object sender, EventArgs e)
        {
            setChart();
        }

        public void setChart()
        {
            DataTable dt = myDataSet;

            if (dt == null)
                return;

            RadChart1.Series.Clear();
            
            RadChart1.DataSource = dt;
            RadChart1.PlotArea.XAxis.DataLabelsColumn = dt.Columns[0].ColumnName;
            RadChart1.PlotArea.XAxis.Appearance.LabelAppearance.RotationAngle = 300;
            RadChart1.PlotArea.XAxis.Appearance.LabelAppearance.Position.AlignedPosition = Telerik.Charting.Styles.AlignedPositions.TopLeft;

            RadChart1.PlotArea.Appearance.Dimensions.Margins.Bottom =
                      Telerik.Charting.Styles.Unit.Percentage(20);
        //    RadChart1.PlotArea.XAxis.Appearance.TextAppearance.Overflow = Telerik.Charting.Styles.Overflow.Auto;
        //    RadChart1.PlotArea.XAxis.Appearance.LabelAppearance.Dimensions.AutoSize = true;
        //    RadChart1.PlotArea.XAxis.Appearance.LabelAppearance.Dimensions.Margins = new Telerik.Charting.Styles.ChartMargins(4, 4, 4, 4);
     
        //    RadChart1.Width = new Unit(900);
        //    RadChart1.Height = new Unit(500);
         
            RadChart1.ChartTitle.TextBlock.Text = ChartName;
            RadChart1.DataBind();

            //set item labels visibility
            foreach (Telerik.Charting.ChartSeries series in RadChart1.Series)
            {
                series.Appearance.LabelAppearance.Visible = displayItemLabels;
                series.Appearance.EmptyValue.Mode = EmtyValuesMode.Zero;
            }

            //remove column1 from series if present. Is present if all columns from the datatable is added as series
            if (RadChart1.Series.Count == dt.Columns.Count)
                RadChart1.Series.RemoveAt(0);

            // set names on items in series
            if (RadChart1.DefaultType == Telerik.Charting.ChartSeriesType.Pie)
                foreach (Telerik.Charting.ChartSeries serie in RadChart1.Series)
                {
                    serie.DataLabelsColumn = dt.Columns[0].ColumnName;
                   // serie.Appearance.LegendDisplayMode = Telerik.Charting.ChartSeriesLegendDisplayMode.ItemLabels;
                    serie.Appearance.ShowLabelConnectors = false;

                    serie.DefaultLabelValue = "#ITEM";
                    
                    int x = 0;
                    foreach (Telerik.Charting.ChartSeriesItem item in serie.Items)
                    {
                        item.Name = dt.Rows[x][0].ToString();
                        x++;
                    }
                }
        }

        private DataTable myDataSet
        {
            set { Session["OLAPDataTable"] = value; }
            get { return (DataTable)Session["OLAPDataTable"]; }
        }

        public DataTable DataSet
        {
            get { return myDataSet; }
            set { myDataSet = value; }
        }
	

        private bool displayItemLabels
        {
            get { return ViewState["displayItemLabels"] == null ? false : (bool)ViewState["displayItemLabels"]; }
            set { ViewState["displayItemLabels"] = value; }
        }
	

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (!this.Page.IsPostBack)
            {
                setChart();
            }

            if (displayItemLabels)
                buttonLabelVisibility.Text = GASystem.AppUtils.Localization.GetGuiElementText("HideLabels");
            else
                buttonLabelVisibility.Text = GASystem.AppUtils.Localization.GetGuiElementText("ShowLabels");
        }

        

        private string _chartName = string.Empty;

        public string ChartName
        {
            get { return _chartName; }
            set { _chartName = value; }
        }
	
    }
}