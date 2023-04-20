using System;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using DataDynamics.Analysis;
using DataDynamics.Analysis.Schema;
using DataDynamics.Analysis.Layout;
//using DataDynamics.Analysis.Samples.Library;
//using DataDynamics.Analysis.Viewer;
using DataDynamics.Analysis.Web;
using MarkingType = DataDynamics.Analysis.Layout.MarkingType;
using SB = DataDynamics.Analysis.Schema.SchemaBuilder;
using DataDynamics.Analysis.DataSources;
using Hierarchy = DataDynamics.Analysis.Schema.Hierarchy;

using GASystem.GAGUI.GAControls.Analysis;
using DataDynamics.ActiveReports.Chart;
using GASystem.DataModel;

namespace GASystem.GAGUI.GAControls.Analysis

    
{


    public partial class PivotCustomControlTest :  System.Web.UI.UserControl
    {
        private const string UnboundDataSourceName = "Unbound data";

        protected PivotView pivotView;
        protected DropDownList dataSource;
        protected DropDownList listThemes;
        protected CustomValidator dataSourceValidator;
        protected CustomValidator fileNameValidator;
        protected TextBox textBoxFileName;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Helper.AddSizingScript(pivotView, this.Page.ClientScript);
        }

        protected void Page_Load(object sender, EventArgs e)
	    {
	    

            if (IsPostBack)
			    return;

            //get datasource
            GASystem.GUIUtils.QuerystringUtils myQueryString = new GASystem.GUIUtils.QuerystringUtils(GASystem.DataModel.GADataClass.GAAnalysis, this.Page.Request);
            int RowId = myQueryString.GetRowId();



            BusinessLayer.BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAAnalysis);
            AnalysisDS ds = bc.GetByRowId(RowId) as AnalysisDS;

            if (ds == null || ds.GAAnalysis.Rows.Count != 1)
                throw new GAExceptions.GADataAccessException("Could not find analysis specification");
            
        this.Page.Title = "Flag" + typeof(PivotView).Name;

		//PivotView.CardLayout.TitleWidth = Unit.Parse("15em");
		//PivotView.CardLayout[CardType.ColorLegend].Text = "Colors";

		dataSource.Items.Clear();
        //foreach (string name in SharedDataSourceBase.DataSourceCatalog.Names)
        //    dataSource.Items.Add(name);

        //dataSource.Items.Add(UnboundDataSourceName);

        dataSource.Items.Add(ds.GAAnalysis[0].FileName);

		// initialize theme list
		listThemes.Items.Clear();
		listThemes.Items.Add(new ListItem("Default theme", string.Empty));

		Array.ForEach(new string[]
		{
			"Themes/Office2007",
			"Themes/VistaAero",
			"Themes/VistaBlack",
			"Themes/WindowsClassic",
			"Themes/WindowsBlue",
			"Themes/WindowsOliveGreen",
			"Themes/WindowsSilver"
		}, listThemes.Items.Add);

		// register shared data source in the factory
		SharedDataSourceBase.RegisterSharedInFactory();
	}

        #region Event handlers

        protected void buttonConnect_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataSource.SelectedValue == UnboundDataSourceName)
                    ConnectUnbound();
                else
                    ConnectDataSource(dataSource.SelectedValue);
            }
            catch (Exception ex)
            {
                dataSourceValidator.IsValid = false;
                dataSourceValidator.ErrorMessage = ex.Message;
            }
        }

        protected void buttonSave_Click(object sender, EventArgs e)
        {
            try
            {
                string fileName = GetFileName();
                XmlWriterSettings writerSettings = new XmlWriterSettings();
                writerSettings.Encoding = System.Text.Encoding.UTF8;
                writerSettings.Indent = true;

                using (XmlWriter writer = XmlWriter.Create(fileName, writerSettings))
                {
                    pivotView.Write(writer, PersistSettings.DataSource | PersistSettings.Layout | PersistSettings.CardLayout);
                }
            }
            catch (Exception ex)
            {
                fileNameValidator.IsValid = false;
                fileNameValidator.ErrorMessage = ex.Message;
            }
        }
        protected void buttonLoad_Click(object sender, EventArgs e)
        {
            fileNameValidator.IsValid = true;
            try
            {
                string fileName = GetFileName();
                XmlDocument doc = new XmlDocument();
                doc.Load(fileName);
                XmlNode node = doc.SelectSingleNode("//PivotView");
                SamplesImpl.LoadLayout(node, pivotView);
            }
            catch (Exception ex)
            {
                fileNameValidator.IsValid = false;
                fileNameValidator.ErrorMessage = ex.Message;
            }
        }

        #endregion

        #region Implementation

        private static DataSet PrepareDataSet()
        {
            DataSet dataset = new DataSet("TestingPubsTitles");
            DataTable table = dataset.Tables.Add("TestingPubsTitles");
            table.Columns.Add("title_id", typeof(string));
            table.Columns.Add("title", typeof(string));
            table.Columns.Add("type", typeof(string));
            table.Columns.Add("pub_id", typeof(string));
            table.Columns.Add("price", typeof(decimal));
            table.Columns.Add("advance", typeof(decimal));
            table.Columns.Add("royalty", typeof(int));
            table.Columns.Add("ytd_sales", typeof(int));
            table.Columns.Add("notes", typeof(string));
            table.Columns.Add("pubdate_str", typeof(string));
            table.Columns.Add("au_id", typeof(string));
            table.Columns.Add("au_lname", typeof(string));
            table.Columns.Add("au_fname", typeof(string));

            table.Rows.Add("BU1032", "The Busy Executive's Database Guide", "business    ", "1389", 19.99, 5000.00, 10, 4095, "An overview of available database systems with emphasis on common business applications. Illustrated.", new DateTime(1991, 06, 12), "409-56-7008", "Green", "Marjorie");
            table.Rows.Add("BU1032", "The Busy Executive's Database Guide", "business    ", "1389", 19.99, 5000.00, 10, 4095, "An overview of available database systems with emphasis on common business applications. Illustrated.", new DateTime(1991, 06, 12), "409-56-7008", "Bennet", "Abraham");
            table.Rows.Add("BU1111", "Cooking with Computers: Surreptitious Balance Sheets", "business    ", "1389", 11.95, 5000.00, 10, 3876, "Helpful hints on how to use your electronic resources to the best advantage.", new DateTime(1991, 06, 09), "267-41-2394", "O'Leary", "Michael");
            table.Rows.Add("BU1111", "Cooking with Computers: Surreptitious Balance Sheets", "business    ", "1389", 11.95, 5000.00, 10, 3876, "Helpful hints on how to use your electronic resources to the best advantage.", new DateTime(1991, 06, 09), "724-80-9391", "MacFeather", "Stearns");
            table.Rows.Add("BU2075", "You Can Combat Computer Stress!", "business    ", "0736", 2.99, 10125.00, 24, 18722, "The latest medical and psychological techniques for living with the electronic office. Easy-to-understand explanations.", new DateTime(1991, 06, 30), "213-46-8915", "Green", "Marjorie");
            table.Rows.Add("BU7832", "Straight Talk About Computers", "business    ", "1389", 19.99, 5000.00, 10, 4095, "Annotated analysis of what computers can do for you: a no-hype guide for the critical user.", new DateTime(1991, 06, 22), "274-80-9391", "Straight", "Dean");
            table.Rows.Add("MC2222", "Silicon Valley Gastronomic Treats", "mod_cook    ", "0877", 19.99, 0.00, 12, 2032, "\"Favorite recipes for quick, easy and elegant meals.\"", new DateTime(1991, 06, 09), "712-45-1867", "del Castillo", "Innes");
            table.Rows.Add("MC3021", "The Gourmet Microwave", "mod_cook    ", "0877", 2.99, 15000.00, 24, 22246, "Traditional French gourmet recipes adapted for modern microwave cooking.", new DateTime(1991, 06, 18), "722-51-5454", "DeFrance", "Michel");
            table.Rows.Add("MC3021", "The Gourmet Microwave", "mod_cook    ", "0877", 2.99, 15000.00, 24, 22246, "Traditional French gourmet recipes adapted for modern microwave cooking.", new DateTime(1991, 06, 18), "899-46-2035", "Ringer", "Anne");
            table.Rows.Add("PC1035", "But Is It User Friendly?", "popular_comp", "1389", 22.95, 7000.00, 16, 8780, "\"A survey of software for the naive user focusing on the 'friendliness' of each.\"", new DateTime(1991, 06, 30), "238-95-7766", "Carson", "Cheryl");
            table.Rows.Add("PC8888", "Secrets of Silicon Valley", "popular_comp", "1389", 20.00, 8000.00, 10, 4095, "Muckraking reporting on the world's largest computer hardware and software manufacturers.", new DateTime(1994, 06, 12), "427-17-2319", "Dull", "Ann");
            table.Rows.Add("PC8888", "Secrets of Silicon Valley", "popular_comp", "1389", 20.00, 8000.00, 10, 4095, "Muckraking reporting on the world's largest computer hardware and software manufacturers.", new DateTime(1994, 06, 12), "846-92-7186", "Hunter", "Sheryl");
            table.Rows.Add("PC9999", "Net Etiquette", "popular_comp", "1389", null, null, null, null, "A must-read for computer conferencing.", new DateTime(2000, 08, 06), "486-29-1786", "Locksley", "Charlene");
            table.Rows.Add("PS1372", "Computer Phobic AND Non-Phobic Individuals: Behavior Variations", "psychology  ", "0877", 21.59, 7000.00, 10, 375, "\"A must for the specialist, this book examines the difference between those who hate and fear computers and those who don't.", new DateTime(1991, 10, 21), "724-80-9391", "MacFeather", "Stearns");
            table.Rows.Add("PS1372", "Computer Phobic AND Non-Phobic Individuals: Behavior Variations", "psychology  ", "0877", 21.59, 7000.00, 10, 375, "\"A must for the specialist, this book examines the difference between those who hate and fear computers and those who don't.", new DateTime(1991, 10, 21), "756-30-7391", "Karsen", "Livia");
            table.Rows.Add("PS2091", "Is Anger the Enemy?", "psychology  ", "0736", 10.95, 2275.00, 12, 2045, "Carefully researched study of the effects of strong emotions on the body. Metabolic charts included.", new DateTime(1991, 06, 15), "899-46-2035", "Ringer", "Anne");
            table.Rows.Add("PS2091", "Is Anger the Enemy?", "psychology  ", "0736", 10.95, 2275.00, 12, 2045, "Carefully researched study of the effects of strong emotions on the body. Metabolic charts included.", new DateTime(1991, 06, 15), "998-72-3567", "Ringer", "Albert");
            table.Rows.Add("PS2106", "Life Without Fear", "psychology  ", "0736", 7.00, 6000.00, 10, 111, "\"New exercise, meditation, and nutritional techniques that can reduce the shock of daily interactions. Popular audience. Sample menus included exercise video available separately.\"", new DateTime(1991, 10, 05), "998-72-3567", "Ringer", "Albert");
            table.Rows.Add("PS3333", "Prolonged Data Deprivation: Four Case Studies", "psychology  ", "0736", 19.99, 2000.00, 10, 4072, "What happens when the data runs dry?  Searching evaluations of information-shortage effects.", new DateTime(1991, 06, 12), "172-32-1176", "White", "Johnson");
            table.Rows.Add("PS7777", "Emotional Security: A New Algorithm", "psychology  ", "0736", 7.99, 4000.00, 10, 3336, "Protecting yourself and your loved ones from undue emotional stress in the modern world. Use of computer and nutritional aids emphasized.", new DateTime(1991, 06, 12), "486-29-1786", "Locksley", "Charlene");
            table.Rows.Add("TC3218", "\"Onions, Leeks, and Garlic: Cooking Secrets of the Mediterranean\"", "trad_cook   ", "0877", 20.95, 7000.00, 10, 375, "\"Profusely illustrated in color, this makes a wonderful gift book for a cuisine-oriented friend.", new DateTime(1991, 10, 21), "807-91-6654", "Panteley,Sylvia");
            table.Rows.Add("TC4203", "Fifty Years in Buckingham Palace Kitchens", "trad_cook   ", "0877", 11.95, 4000.00, 14, 15096, "\"More anecdotes from the Queen's favorite cook describing life among English royalty. Recipes techniques, tender vignettes.\"", new DateTime(1991, 06, 12), "648-92-1872", "Blotchet-Halls", "Reginald");
            table.Rows.Add("TC7777", "\"Sushi, Anyone?\"", "trad_cook   ", "0877", 14.99, 8000.00, 10, 4095, "Detailed instructions on how to make authentic Japanese sushi in your spare time.", new DateTime(1991, 06, 12), "267-41-2394", "O'Leary", "Michael");
            table.Rows.Add("TC7777", "\"Sushi, Anyone?\"", "trad_cook   ", "0877", 14.99, 8000.00, 10, 4095, "Detailed instructions on how to make authentic Japanese sushi in your spare time.", new DateTime(1991, 06, 12), "472-27-2349", "Gringlesby", "Burt");
            table.Rows.Add("TC7777", "\"Sushi, Anyone?\"", "trad_cook   ", "0877", 14.99, 8000.00, 10, 4095, "Detailed instructions on how to make authentic Japanese sushi in your spare time.", new DateTime(1991, 06, 12), "672-71-3249", "Yokomoto", "Akiko");

            return dataset;
        }

        private static SchemaDefinition PrepareSchema()
        {
            SB sb = new SB("TestingPubsTitles");

            sb.AddQueryFields(
                "title_id", "title", "type", "pub_id", "price", "advance",
                "royalty", "ytd_sales", "notes", "pubdate_str", "au_id", "au_lname", "au_fname",
                new QueryFieldBuilder("PubDate")
                    .SetExpression<DateTime>("=CDate(Fields!pubdate_str.Value)"),
                new QueryFieldBuilder("PubDate_Year")
                    .SetExpression<int>("=Year(Fields!PubDate.Value)"),
                new QueryFieldBuilder("PubDate_Quarter")
                    .SetExpression<int>("=DatePart(DateInterval.Quarter, Fields!PubDate.Value)"),
                new QueryFieldBuilder("PubDate_Month")
                    .SetExpression<int>("=Month(Fields!PubDate.Value)"),
                new QueryFieldBuilder("PubDate_Day")
                    .SetExpression<int>("=Day(Fields!PubDate.Value)")
                );

            AttributeBuilder defaultField;

            sb.AddDimensions(
                new MeasuresDimBuilder("Measures")
                    .AddField(
                    new MeasureBuilder("Order count")
                        .SetAggregateFunction(AggregateFunction.Count)
                        .SetExpression<int>("=Fields!title_id.Value")
                        .SetDefaultFormat("#0")
                        .SetForeColor(Color.DarkBlue)
                    )
                    .AddField(
                    new MeasureBuilder("Unit Price")
                        .SetExpression<float>("=Fields!price.Value")
                        .SetAggregateFunction(AggregateFunction.Avg)
                        .SetFontFlags(FontFlags.Italic)
                    )
                    .AddField(
                    new MeasureBuilder("Total Cost")
                        .SetExpression<float>("=Fields!price.Value")
                        .SetLocale("en-US")
                        .SetDefaultFormat("c")
                    )
                    .AddFields(
                    new MeasureBuilder("Advance")
                        .SetExpression<float>("=Fields!advance.Value"),
                    new MeasureBuilder("Royalty")
                        .SetExpression<float>("=Fields!royalty.Value")
                    ),
                new AttributesDimBuilder("Dimension")
                    .SetCaption("Other fields")
                    .SetDefaultField(
                    defaultField = new AttributeBuilder("title")
                        .SetCaption("Book Title")
                        .SetExpression<string>("=Fields!title.Value"))
                    .AddField(defaultField)
                    .AddField(
                    new AttributeBuilder("type")
                        .SetCaption("Type")
                        .SetExpression<string>("=Fields!type.Value")
                    )
                    .AddField(
                    new AttributeBuilder("author")
                        .SetCaption("Author name")
                        .SetExpression<string>("=Fields!au_id.Value")
                        .SetCaptionExpression("=Fields!au_lname.Value & \" \" & Fields!au_fname.Value")
                    ),
                new AttributesDimBuilder("Pub Date")
                    .SetCaption("Date")
                    .SetDefaultField(defaultField = new HierarchyBuilder("Pub Date")
                        .AddLevel(
                        new LevelBuilder("Year")
                            .SetExpression("=Fields!PubDate_Year.Value")
                            .SetCaptionExpression("=Fields!PubDate_Year.Value")
                        )
                        .AddLevel(
                        new LevelBuilder("Quarter")
                            .SetExpression<int>("=Fields!PubDate_Quarter.Value")
                            .SetCaptionExpression("=\"Q\" & Fields!PubDate_Quarter.Value & \" \" & Fields!PubDate_Year.Value")
                        )
                        .AddLevel(
                        new LevelBuilder("Month")
                            .SetExpression<int>("=Fields!PubDate_Month.Value")
                            .SetCaptionExpression("=MonthName(Fields!PubDate_Month.Value) & \", \" & Fields!PubDate_Year.Value")
                        )
                        .AddLevel(
                        new LevelBuilder("Day")
                            .SetExpression<int>("=Fields!PubDate_Day.Value")
                            .SetCaptionExpression("=MonthName(Fields!PubDate_Month.Value) & \" \" & Fields!PubDate_Day.Value & \", \" & Fields!PubDate_Year.Value")
                        )
                    )
                    .AddFields(
                    defaultField,
                    new AttributeBuilder("Date").SetExpression<DateTime>("=Fields!PubDate.Value"),
                    new AttributeBuilder("Year").SetExpression<int>("=Fields!PubDate_Year.Value"),
                    new AttributeBuilder("Quarter").SetExpression<int>("=Fields!PubDate_Quarter.Value"),
                    new AttributeBuilder("Month").SetExpression<int>("=Fields!PubDate_Month.Value"),
                    new AttributeBuilder("Day").SetExpression<int>("=Fields!PubDate_Day.Value")
                    )
                );

            return sb.BuildSchema();
        }

        private void ConnectDataSource(string name)
        {
            if (pivotView.DataSource != null)
                pivotView.DataSource.Disconnect();
            pivotView.DataSource = SharedDataSourceBase.Create(name);
            pivotView.DataSource.Connect();
        }

        private void ConnectUnbound()
        {
            SchemaDefinition schema = PrepareSchema();
            UnboundDataSource ds = new UnboundDataSource();
            ds.CustomSchema = schema;
            ds.DataSource = PrepareDataSet();
            ds.Connect();
            pivotView.DataSource = ds;

            // initialize new undoable unit with name 'Initialization'
            using (ILayoutActions la = pivotView.BeginLayoutUpdate("Initialization"))
            {
                // get from schema the hierarchy with the unique name '[Pub Date].[Pub Date]'
                Hierarchy pubDate = schema.Get<Hierarchy>("[Pub Date].[Pub Date]");

                // set marking type to the Circle
                la.SetMarkingType(MarkingType.Circle);

                // append the hierarchy 'pubDate' to the Column
                la.AppendField(pubDate.UniqueName, ShelfKind.ColumnShelf);

                // append the measure '[Measures].[Total Cost]' right after the hierarchy
                la.AppendField("[Measures].[Total Cost]", ShelfKind.ColumnShelf);

                // appent the attribute '[Dimension].[author]' to the Row
                la.AppendField("[Dimension].[author]", ShelfKind.RowShelf);

                // appent the measure '[Measures].[Order count]' to the Color
                la.AppendField("[Measures].[Order count]", ShelfKind.ColorShelf);

                // drill down to the 1st level of the hierarchy (note: hierarchy levels are zero based)
                la.DrillHierarchy(pubDate.UniqueName, 1, ShelfKind.ColumnShelf);

                // drill down member 'Q2 1991' of the hierarchy
                la.DrillMembers(pubDate.UniqueName, 1, DrillDirection.Down,
                    string.Format("[{0}].&[{1}]", pubDate.Levels[0].UniqueName, "1991.2"));

                // exclude from grid '2000' year
                la.ExcludeMembers(pubDate.Levels[0].UniqueName,
                    string.Format("[{0}].&[{1}]", pubDate.Levels[0].UniqueName, "2000"));

                // commit undoable unit with name 'Initialization'
                la.Commit();
            } // NOTE: la.Dispose() must reject our changes if la.Commit() or la.Reject() isn't called yet.
        }

        private string GetFileName()
        {
            // TODO not error proof method
            string fileName = textBoxFileName.Text;
            if (string.IsNullOrEmpty(fileName))
                return string.Empty;
            fileName = Path.Combine(ConfigurationManager.AppSettings["LayoutsLocation"], fileName);

            if (!fileName.EndsWith(".analysis", StringComparison.CurrentCultureIgnoreCase))
            {
                fileName += ".analysis";
            }

            return fileName;
        }

        #endregion

        protected void listThemes_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                PivotView.ApplyTheme(Session, listThemes.SelectedValue);
            }
            catch
            { }
        }
    }

}