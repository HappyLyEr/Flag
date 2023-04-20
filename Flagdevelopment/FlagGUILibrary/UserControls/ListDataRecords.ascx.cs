using GASystem.DataModel;
using GASystem.GAGUI.GUIUtils;
using GASystem.WebControls.ListControls;

namespace GASystem
{
	using System;
	using System.Collections;
	using System.Data;
	using System.Web.UI.WebControls;
	using System.Drawing;
	using GASystem.BusinessLayer; 
	using GASystem.AppUtils;
	using GASystem.GAGUIEvents;
	using log4net;
    using Telerik.WebControls;
    using System.Web.UI.HtmlControls;

	// ISSUE: Should be able to display merged and calculated columns. Firstname+lastname
	// is an example of a merged column
	// BUG: For some datalist columns sorting fails. May be related to the datatype of the column
	
	/// <summary>
	///		Summary description for ListDataRecords.
	/// </summary>
	public class ListDataRecords : System.Web.UI.UserControl
	{
		//protected DBauer.Web.UI.WebControls.DynamicControlsPlaceholder DCP;
		protected System.Web.UI.WebControls.PlaceHolder ListPHolder;

        protected System.Web.UI.WebControls.PlaceHolder SumPHolder;
        protected System.Web.UI.WebControls.PlaceHolder SumHeaderPHolder;
		//protected DataGrid dg;
        protected Telerik.WebControls.RadGrid dg;
		protected System.Web.UI.WebControls.Label MessageLabel;

		private bool _refreshGridCalled = false;
		private DataSet _recordDataSet;
        private bool exportToExcel = false;
        private bool _displayExportToExcelLink = true;
        private string _ImportExcelTextLink = null;
        private bool _resetSort = false;
        private bool _displayCommandTemplate = true;
        private GASystem.GAGUI.GAControls.Export.ExportListToExcel dgExport;
        protected ImportExcelCtrl importXlsUC;

		protected Button btn;
		protected System.Web.UI.WebControls.PlaceHolder PlaceHolderFilter;
		//protected System.Web.UI.WebControls.HyperLink ButtonNewRecordTop;
        private System.Web.UI.WebControls.HyperLink buttonNewRecordExternal;
		protected System.Web.UI.WebControls.HyperLink lnkAddGroupTop;
		protected System.Web.UI.WebControls.HyperLink ButtonNewRecord;
		protected System.Web.UI.WebControls.HyperLink lnkAddGroup;
        protected System.Web.UI.WebControls.PlaceHolder ImportExcelPHolder;
        protected System.Web.UI.WebControls.LinkButton ButtonImportExcel;



        public event DataGridCommandEventHandler EditGARecordClicked;
		public event GACommandEventHandler SelectRecordClicked;
		public event GACommandEventHandler EditRecordClicked;
		public event GACommandEventHandler NewRecordClicked;
		public event GACommandEventHandler GACommandExecuted;
		public event GACommandEventHandler SortClicked;

		private GASystem.GUIUtils.DataFilter.FilterBuilder myFilterBuilder;
        private bool _listUpdated = false;

        private bool _radGridInEditMode = false;
        


        private ArrayList _columnsToDisplay = new ArrayList();
        private bool _enableViewState = true;
        private string _viewRecordDataClass = string.Empty;   //dataclass used when genereting link for displaying record. may be differnt that dataclass if listing a view
        private string _baseURL = string.Empty;  //used for redirect when filtering data.

        private bool createNewRecordWhenUpdatingInList = false;

		private void Page_Load(object sender, System.EventArgs e)
		{
			
			Trace.Warn(this.ID+": Page_Load");
			//GenerateDataGrid();
			//RefreshGrid();

			//ButtonNewRecord.Visible = DisplayNewButton;

			if (!Page.IsPostBack)
			{
				//String recordName = null==DataClass ? "" : Localization.GetGuiElementText(DataClass);
				//ButtonNewRecord.Text = String.Format(Localization.GetGuiElementText("New Record"), recordName);
				

			}



            // checks if the user wants to import an excel file
            if (DisplayImportExcelControl)
            {
                ShowImportExcelControl(false);
            }
        }

		public void ClearGrid()
		{
			if (null!=dg) 
				dg.Controls.Clear();
			
		}


		public void RefreshGrid() 
		{
			_refreshGridCalled = true;
			FillGrid();
		}

		//Expose this method so that consumers may refresh grid
		/// <summary>
		/// Refresh datagrid. Remember to set RecordsDataSet and DataClass first.
		/// ViewState must be recreated when calling this method! (Do not call this method
		/// from the page_init event)
		/// </summary>
		/// 
		protected override void OnPreRender(EventArgs e)
		{
            if (exportToExcel)
            {
                
                dgExport.ExportToExcel();
                return;
            }

            _enableViewState = _radGridInEditMode;

            //set newrecord link
            //if (Owner == null) 
            //{
            //    ButtonNewRecord.Visible = false;  //hide if there is no owner
            //    ButtonNewRecordTop.Visible = false;
            //}
            //else 
            //{
				ButtonNewRecord.NavigateUrl = GUIUtils.LinkUtils.GenerateURLForNewRecord(GASystem.DataModel.GADataRecord.ParseGADataClass(this.DataClass), this.Owner);
				//ButtonNewRecordTop.NavigateUrl = GUIUtils.LinkUtils.GenerateURLForNewRecord(GASystem.DataModel.GADataRecord.ParseGADataClass(this.DataClass), this.Owner);
                buttonNewRecordExternal.NavigateUrl = GUIUtils.LinkUtils.GenerateURLForNewRecord(GASystem.DataModel.GADataRecord.ParseGADataClass(this.DataClass), this.Owner);
            //}

			
          
            
            //if (myFilterBuilder != null && !Page.IsPostBack)
            //    myFilterBuilder.SetDefaultFilter();




			//test JOF
			if (myFilterBuilder != null && !Page.IsPostBack) 
			{
				string fName =  Request.QueryString["ffield"] == null ? string.Empty :  Request.QueryString["ffield"].ToString();
				string fCondition =  Request.QueryString["fCondition"] == null ? string.Empty :  Request.QueryString["fCondition"].ToString();
				string fOperator =  Request.QueryString["fOperator"] == null ? string.Empty :  Request.QueryString["fOperator"].ToString();

				myFilterBuilder.SetQueryStringFilter(fName, fOperator, fCondition);
			}



           

			
			if (_refreshGridCalled) 
			base.OnPreRender (e);

			const string SCRIPT_KEY = "GAScript";
			const string SCRIPT = @"<script language=""JavaScript"">
<!--
var lastColorUsed;
function GAScript_changeBackgroundColor(row, highlight)
{{
  if (highlight)
  {{
    lastColorUsed = row.style.backgroundColor;
    color = row.style.color;
    row.style.backgroundColor = '#{0:X2}{1:X2}{2:X2}';
	row.style.color = '#FFFFFF';
    row.style.cursor = 'pointer';
	row.style.cursor = 'hand';
  }}
  else
    row.style.backgroundColor = lastColorUsed;
	row.style.color = color;
}}

function GAScript_goto(url)
{{
  window.location.href = url;
}}
// -->
</script>";

            Color RowHighlightColor = Color.FromArgb(254, 234, 228);
			//Color RowHighlightColor = Color.FromArgb(255, 79, 0);
//			Color RowHighlightColor = Color.FromArgb(219, 216, 41);

            if (RowHighlightColor != Color.Empty && !this.Page.ClientScript.IsClientScriptBlockRegistered(SCRIPT_KEY))
                this.Page.ClientScript.RegisterClientScriptBlock(typeof(ListDataRecords), SCRIPT_KEY, String.Format(SCRIPT, RowHighlightColor.R, RowHighlightColor.G, RowHighlightColor.B));

			SetupGroupLink();

            //if (!_refreshGridCalled)
			    FillGrid();


            //dg.Rebind();

		}

		private void SetupGroupLink() 
		{
			if (!DisplayNewButton)    //do not display group button if we are not displaying the new button
			{
				lnkAddGroup.Visible = false;
				lnkAddGroupTop.Visible = false;

				return;
			}

			GASystem.DataModel.GADataClass dataClass;
			try 
			{
			
				dataClass = GASystem.DataModel.GADataRecord.ParseGADataClass(this.DataClass);
			} 
			catch (Exception ex) 
			{
				//error parsing dataclass, set visibility to false and return
				lnkAddGroup.Visible = false;
				lnkAddGroupTop.Visible = false;
				return;					   
			}

            ClassDescription cd = GASystem.AppUtils.ClassDefinition.GetClassDescriptionByGADataClass(dataClass);
            if (dataClass == GASystem.DataModel.GADataClass.GAMeetingPersonList ||
// Tor 20150730				dataClass == GASystem.DataModel.GADataClass.GATimeAndAttendance ||
// Tor 20150730				dataClass == GASystem.DataModel.GADataClass.GACoursePersonList ||
                dataClass == GASystem.DataModel.GADataClass.GACoursePersonListView||
                dataClass == GASystem.DataModel.GADataClass.GAHazardIdentification ||
                dataClass == GASystem.DataModel.GADataClass.GAInfoToCommunity
                ||  dataClass == GASystem.DataModel.GADataClass.GAGroups
                // ToDo Tor 20150730 exchange dataClass == test with if (GAClass.method(dataClass)) free attribute=IntFree2 
                || cd.isLookupfieldMultipleClass
                )            
            {

                lnkAddGroup.Visible = true;
                lnkAddGroup.Text = String.Format(Localization.GetGuiElementText("NewRecords"), Localization.GetGuiElementText(DataClass));
				//lnkAddGroup.Text = GASystem.AppUtils.Localization.GetCaptionText("AddGroupOfPeople");  
				lnkAddGroup.NavigateUrl = GASystem.GUIUtils.LinkUtils.GenerateURLForAddingGroupOfPeople(dataClass.ToString(), Owner);
                

				lnkAddGroupTop.Visible = true;
                lnkAddGroupTop.Text = String.Format(Localization.GetGuiElementText("NewRecords"), Localization.GetGuiElementText(DataClass));
				lnkAddGroupTop.NavigateUrl = GASystem.GUIUtils.LinkUtils.GenerateURLForAddingGroupOfPeople(dataClass.ToString(), Owner);

                ButtonNewRecord.Visible = false;
               // ButtonNewRecordTop.Visible = false;
                buttonNewRecordExternal.Visible = false;
                DisplayNewButton = false;
            } 
            else 
			{
				lnkAddGroup.Visible = false;
				lnkAddGroupTop.Visible = false;
			}
        }



		public void FillGrid()
		{
			ButtonNewRecord.Visible = DisplayNewButton;
			//ButtonNewRecordTop.Visible = DisplayNewButton;
            buttonNewRecordExternal.Visible = DisplayNewButton;

			if (dg!=null && RecordsDataSet!=null && RecordsDataSet.Tables[DataClass]!=null)
			{
				//if (dg.Columns.Count==0)  // regenerates datagrid each time, workaround for reuse of datagrid bug.
											//TODO, redisign to fix bug properly
                GenerateDataGrid();

				Trace.Warn(this.ID+":Refreshing grid for dataclass "+DataClass);
				
				//DataView view = RecordsDataSet.Tables[DataClass].DefaultView;

                //if (myFilterBuilder != null)
                //    RecordsDataSet.Tables[DataClass].DefaultView.RowFilter = myFilterBuilder.GetFilterString();
                //    //view.RowFilter = myFilterBuilder.GetFilterString();

                string myFilter = string.Empty;

                if (myFilterBuilder != null)
                    myFilter = myFilterBuilder.GetFilterString();

				
                if (0 == RecordsDataSet.Tables[DataClass].Rows.Count)
                {
                    if (myFilter == string.Empty) 
                        MessageLabel.Text = String.Format(Localization.GetGuiElementText("NoRecords"), Localization.GetGuiElementTextPlural(this.DataClass));
                    else
                        MessageLabel.Text = String.Format(Localization.GetGuiElementText("NoRecordsFound"), Localization.GetGuiElementTextPlural(this.DataClass));


					MessageLabel.CssClass = "MessageNormal";

                    dg.Visible = false;
                    //ButtonNewRecordTop.Visible = false;
                    lnkAddGroupTop.Visible = false;
                    buttonNewRecordExternal.Visible = false;
                    ButtonImportExcel.Visible = HasAccessToImportExcel();

                }
                else
				{
                    dg.Visible = true;
                  //  ButtonNewRecordTop.Visible = true;
                    MessageLabel.Text = "";
					
                    
             
             
     //test:  set datasource
                    ////       dg.DataSource = view;        //RecordsDataSet.Tables[DataClass].DefaultView;
                    dg.DataSource = RecordsDataSet;
                    dg.DataMember = this.DataClass.ToString();

                    dg.DataBind();
				}
				ButtonNewRecord.Text = String.Format(Localization.GetGuiElementText("NewRecord"), Localization.GetGuiElementText(DataClass));
				//ButtonNewRecordTop.Text = String.Format(Localization.GetGuiElementText("NewRecord"), Localization.GetGuiElementText(DataClass));
                buttonNewRecordExternal.Text = String.Format(Localization.GetGuiElementText("NewRecord"), Localization.GetGuiElementText(DataClass));
			}
		}

		/// <summary>
		/// Method for telling control to add filters. This method is normally not needed to be called because the control 
		/// is normally added during onint. This method is added in order to support the 
		/// member tabs control where the dataclass is not known prior to adding the listdatarecords control. 
		/// </summary>
		public void AddFilterControl(bool SetDefaultFilter) 
		{
			if (myFilterBuilder != null) 
				PlaceHolderFilter.Controls.Clear();

			//{
			myFilterBuilder = new GASystem.GUIUtils.DataFilter.FilterBuilder(GASystem.DataModel.GADataRecord.ParseGADataClass(this.DataClass));
			PlaceHolderFilter.Controls.Add(myFilterBuilder.FilterControl);
			myFilterBuilder.FilterControl.SetFilterClicked += new EventHandler(FilterControl_SetFilterClicked);
            myFilterBuilder.FilterChanged += new EventHandler(FilterControl_SetFilterClicked);

			if (SetDefaultFilter)
				myFilterBuilder.SetDefaultFilter();
			//}
		}

      

		public string GetFilterString() 
		{
			if (myFilterBuilder != null)
				return myFilterBuilder.GetFilterString();
			return string.Empty;
		}

        /// <summary>
        /// Add one or more specific column to display in the Flag datagrid. Adding column here will override the default settings in 
        /// fielddefinition. If one or more columns are added will the grid only display these columns.
        /// </summary>
        /// <param name="ColumnName"></param>
        public void AddColumnToDisplay(string ColumnName)
        {
            _columnsToDisplay.Add(ColumnName.ToLower());
        }

        protected bool DisplayColumn(GAListColumnContainer Col)
        {
            if (_columnsToDisplay.Count == 0)
                return !Col.getFieldDescription().HideInSummary;
            return _columnsToDisplay.Contains(Col.getFieldDescription().FieldId.ToLower());
        }

        protected bool IsColumnReadOnly(string FieldName)
        {
            if (this.DataClass == GASystem.DataModel.GADataClass.GAExposedHoursGroupView.ToString() && _radGridInEditCountMode)
            {
                string exposedHoursEditFields = string.Empty;  //default is no fields 

                try
                {
// Tor 20160127                    exposedHoursEditFields = System.Configuration.ConfigurationManager.AppSettings.Get("ExposedHoursCountEditFields");
                    exposedHoursEditFields = new GASystem.AppUtils.FlagSysResource().GetResourceString("ExposedHoursCountEditFields");
                }
                catch { }

                if (exposedHoursEditFields.Contains(";" + FieldName.ToUpper() + ";"))
                    return false;
    
                return true;
            }
            return false;
        }


        private void GenerateDataGrid()
        {
            if (RecordsDataSet == null || RecordsDataSet.Tables[DataClass] == null) return;

            Trace.Warn(this.ID + ":Generate grid definition for dataclass " + DataClass);

            dg.Columns.Clear();
            dg.AutoGenerateColumns = false;
            dg.EnableViewState = true;      //viewstate is needed for lineedit to function correctly
            dg.AllowSorting = true;
            dg.AllowPaging = true;
            dg.CurrentPageIndex = CurrentPage;
            dg.PageSize = 25;
            dg.PagerStyle.Mode = GridPagerMode.NumericPages;

            dg.PagerStyle.CssClass = "gridStyle_PagerStyle";
            dg.PagerStyle.Position = GridPagerPosition.TopAndBottom; // PagerPosition.TopAndBottom;
            dg.AllowMultiRowSelection = true;
            dg.AllowMultiRowEdit = true;
            dg.AllowMultiRowSelection = true;
            dg.ClientSettings.Selecting.AllowRowSelect = true;
            dg.Skin = "FlagGrid";

            //check for rowid column
            string rowidColumn = DataClass.ToString().Substring(2) + "rowid";
            if (!RecordsDataSet.Tables[DataClass].Columns.Contains(rowidColumn))
                throw new GAExceptions.GADataAccessException("Recorddataset for list " + DataClass.ToString() + " does not contain a rowid column");

            //get class definition
            AppUtils.ClassDescription cd = ClassDefinition.GetClassDescriptionByGADataClass(this.DataClass);
            if (cd.IsView && cd.VirtualClassAttributeName != string.Empty)
                _viewRecordDataClass = cd.VirtualClassAttributeName;
            else
                _viewRecordDataClass = DataClass;

            // dg.MasterTableView.AllowFilteringByColumn = true;
            //dg.DataKeyField = RecordsDataSet.Tables[DataClass].PrimaryKey[0].ColumnName;
            GAListColumnContainerFactory factory = new GAListColumnContainerFactory();
            ArrayList myBoundColumns = new ArrayList();
            string baseURL = Page.Request.Url.Scheme + "://" + Page.Request.Url.Authority + Page.Request.ApplicationPath.TrimEnd('/') + "/";
            foreach (DataColumn c in RecordsDataSet.Tables[DataClass].Columns)
            {
                //GAListColumn tmpColumn = factory.getGAListColumn(c, (SortColumn.Equals(c.ColumnName)), setFormatting(c));
                //GAListColumn tmpColumn = factory.getGAListColumn(c, false, setFormatting(c));
                GAListColumnContainer tmpColumn = factory.getGAListColumnContainer(c, IsColumnReadOnly(c.ColumnName), false, setFormatting(c), baseURL); //    = new GAListColumnContainer(c);

                if (tmpColumn != null && DisplayColumn(tmpColumn))  // !tmpColumn.getFieldDescription().HideInSummary)
                {
                    //make column hidden in list if it is to be available in filter settings only
                    if (tmpColumn.getFieldDescription().DisplayInFilterListOnly)
                        tmpColumn.GridColumn.Display = false;

                    myBoundColumns.Add(tmpColumn);
                }
            }
            myBoundColumns.Sort();

            foreach (GAListColumnContainer c in myBoundColumns)
            {
                dg.Columns.Add(c.GridColumn);
            }

            //display warnings or error messages. 
            //adds special waring and message columns dataclass i gaexposedhoursgroupview og garemedialactionview
            if (this.DataClass.ToUpper() == GASystem.DataModel.GADataClass.GAExposedHoursGroupView.ToString().ToUpper())
            {
                FlagGUILibrary.WebControls.ListControls.GridBoundDateOverlapTester tmpColumn = new FlagGUILibrary.WebControls.ListControls.GridBoundDateOverlapTester();
                tmpColumn.HeaderText = string.Empty;
                tmpColumn.DataField = this.DataClass.Substring(2) + "rowid";
                tmpColumn.DataClass = this.DataClass;

                tmpColumn.CD = GASystem.AppUtils.ClassDefinition.GetClassDescriptionByGADataClass(this.DataClass);
                tmpColumn.UniqueName = "gridcolumn_" + this.DataClass + "_" + "overlaptest";
                //tmpColumn.SortExpression = fd.FieldId;
                tmpColumn.ReadOnly = true;
                dg.Columns.AddAt(0, (Telerik.WebControls.GridBoundColumn)tmpColumn);
            }

            if (this.DataClass.ToUpper() == GASystem.DataModel.GADataClass.GARemedialActionView.ToString().ToUpper())
            {
                ClassDescription cdRemedial = ClassDefinition.GetClassDescriptionByGADataClass(this.DataClass);

                FlagGUILibrary.WebControls.ListControls.GridBoundDateExpiredTester tmpColumn = new FlagGUILibrary.WebControls.ListControls.GridBoundDateExpiredTester();
                tmpColumn.HeaderText = string.Empty;
                tmpColumn.DataField = cdRemedial.DateField;
                tmpColumn.DataClass = this.DataClass;

                tmpColumn.CD = ClassDefinition.GetClassDescriptionByGADataClass(this.DataClass);
                tmpColumn.UniqueName = "gridcolumn_" + this.DataClass + "_" + "expiredtester";
                //tmpColumn.SortExpression = fd.FieldId;
                tmpColumn.ReadOnly = true;
                dg.Columns.AddAt(0, (Telerik.WebControls.GridBoundColumn)tmpColumn);
            }

            if (DisplayEditButton)
            {
                // Tor 20140919 Edit not allowed on virtual classes. Line below replaces: if (!_radGridInEditMode &&  !(this.DataClass == GASystem.DataModel.GADataClass.GARemedialActionView.ToString()))
                if (!_radGridInEditMode && cd.VirtualClassAttributeName == string.Empty)
                    dg.Columns.Add(CreateHyperLinkColumn(rowidColumn, true, "Edit"));
                else
                    dg.Columns.Add(CreateHyperLinkColumn(rowidColumn, true, ""));

                GridClientSelectColumn gridSelect = new GridClientSelectColumn();
                gridSelect.HeaderStyle.HorizontalAlign = HorizontalAlign.Left;
                dg.Columns.AddAt(0, gridSelect);
            }

            if (DisplaySelectButton) dg.Columns.Add(CreateHyperLinkColumn(rowidColumn, false, "Select"));
            if (DisplaySelectPostBackButton) dg.Columns.Add(CreateButtonColumn(rowidColumn, "Select", "Select"));
            //  dg.Columns.Add(new GridEditCommandColumn());
            //create command template and only show updateandcreatenewlink if dataclass is exposed hours
            if (DisplayCommandTemplate)
                dg.MasterTableView.CommandItemDisplay = GridCommandItemDisplay.Top;
            else
                dg.MasterTableView.CommandItemDisplay = GridCommandItemDisplay.None;

            bool displayUpdateAndCreateNew =
                this.DataClass == GASystem.DataModel.GADataClass.GAExposedHoursGroupView.ToString();

            FlagListCommandItemTemplate template = new WebControls.ListControls.FlagListCommandItemTemplate(_radGridInEditMode,
                DisplayEditButton,
                DisplayExportToExcelLink,
                displayUpdateAndCreateNew,
                _radGridInEditCountMode,
                DisplayCancelSortButton,
                HasAccessToImportExcel()
            );

            dg.MasterTableView.CommandItemTemplate = template;
            //set sort
            dg.MasterTableView.SortExpressions.AllowMultiColumnSorting = true;
            if ((ResetSort || !this.Page.IsPostBack))
            {
                dg.MasterTableView.SortExpressions.Clear();
                // dg.MasterTableView.SortExpressions.
                isCurrentSortDefaultSort = true;

                //if custom sort is set, use this value
                if (CustomSort != string.Empty)
                {
                    dg.MasterTableView.SortExpressions.AddSortExpression(CustomSort);
                }
                else
                {
                    foreach (GAListColumnContainer c in myBoundColumns)
                    {
                        if (c.getFieldDescription().sortOrder != 0)
                        {
                            GridSortExpression sortExpression = new GridSortExpression();
                            sortExpression.FieldName = c.getFieldDescription().FieldId;
                            if (c.getFieldDescription().sortAscDesc.ToUpper() == "ASC")
                                sortExpression.SortOrder = GridSortOrder.Ascending;
                            else
                                sortExpression.SortOrder = GridSortOrder.Descending;
                            dg.MasterTableView.SortExpressions.Add(sortExpression);
                        }
                    }
                }
            }

            ListPHolder.Controls.Clear();
            ListPHolder.Controls.Add(dg);

            //GUIGenerateUtils.ApplyDatagridSkin(dg);
            FieldDescription[] fds = FieldDefintion.GetFieldDescriptions(this.DataClass);

            SumPHolder.Visible = false;
            SumHeaderPHolder.Visible = false;
            SumPHolder.Controls.Clear();
            HtmlTable tbl = new HtmlTable();
            tbl.EnableViewState = true;
            tbl.ID = "Tablex";
            SumPHolder.Controls.Add(tbl);
            //tbl.Width = "100%";
            tbl.Border = 0;
            tbl.Attributes.Add("class", "ViewForm_Table");
            tbl.CellPadding = 0;
            tbl.CellSpacing = 0;

            foreach (FieldDescription fd in fds)
            {
                if (fd.ListAggregat != string.Empty)
                {
                    DataTable table;
                    table = this.RecordsDataSet.Tables[0];
                    if (fd.ListAggregat.Equals("SUM"))
                    {
                        string operation = "Sum(" + fd.FieldId + ")";
                        object sumObject = table.Compute(operation, "");
                        Label sumLabel = new Label();
                        sumLabel.Text = fd.ListAggregat + ":" + sumObject.ToString();

                        HtmlTableRow tblRow;
                        tblRow = new HtmlTableRow();
                        tblRow.ID = tbl.ID + fd.FieldId;
                        tbl.Rows.Add(tblRow);
                        double d = 0;
                        // Tor 20141124 set d=0 if no records in dataset
                        // d = double.Parse(sumObject.ToString());
                        if (this.RecordsDataSet.Tables[0].Rows.Count > 0)
                        {
                            d = double.Parse(sumObject.ToString());
                            if (fd.Dataformat.ToUpper().Equals("DECIMAL2"))
                                d = d / 100;
                        }

                        AddLabelAndContentToRow(tblRow, fd, d.ToString());
                        SumPHolder.Visible = true;
                        SumHeaderPHolder.Visible = true;
                        //SumPHolder.Controls.Add(sumLabel);
                    }

                    if (fd.ListAggregat.Equals("COUNT"))
                    {
                        string operation = "Count(" + fd.FieldId + ")";
                        object sumObject = table.Compute(operation, "");
                        Label sumLabel = new Label();
                        sumLabel.Text = fd.ListAggregat + ":" + sumObject.ToString();

                        HtmlTableRow tblRow;
                        tblRow = new HtmlTableRow();
                        tblRow.ID = tbl.ID + fd.FieldId;
                        tbl.Rows.Add(tblRow);
                        double d = double.Parse(sumObject.ToString());
                        if (fd.Dataformat.ToUpper().Equals("DECIMAL2"))
                            d = d / 100;

                        AddLabelAndContentToRow(tblRow, fd, d.ToString());
                        SumPHolder.Visible = true;
                        SumHeaderPHolder.Visible = true;
                        //SumPHolder.Controls.Add(sumLabel);
                    }

                    if (fd.ListAggregat.Equals("RSS"))
                    {
                        double total = 0;

                        foreach (DataRow row in table.Rows)
                        {
                            double d = 0;
                            try
                            {
                                d = double.Parse(row[fd.FieldId].ToString());
                            }
                            catch (Exception ex) { }
                            d = d / 100;
                            total = total + d * d;
                        }
                        total = Math.Sqrt(total);

                        HtmlTableRow tblRow;
                        tblRow = new HtmlTableRow();
                        tblRow.ID = tbl.ID + fd.FieldId;
                        tbl.Rows.Add(tblRow);
                        //AddLabelAndContentToRow(tblRow, fd, total.ToString());
                        AddLabelAndContentToRow(tblRow, fd, total.ToString("#,#.0#)"));
                        SumPHolder.Visible = true;
                        SumHeaderPHolder.Visible = true;
                    }
                }
            }
        }

        private void AddLabelAndContentToRow(HtmlTableRow currentTableRow, FieldDescription fd, string content)
        {
            //label cell
            HtmlTableCell cell = new HtmlTableCell();
            cell.ID = fd.FieldId + "c1labels";

            Label lbl = new Label();
            lbl.ID = "Label_" + fd.FieldId;
            cell.Controls.Add(lbl);
            if (fd.ListAggregat.Equals("COUNT"))
                lbl.Text =  fd.ListAggregat;
            else
                lbl.Text = fd.ListAggregat + ": " + AppUtils.Localization.GetCaptionText(fd.DataType);
            //lbl.EnableViewState = false;
            currentTableRow.Cells.Add(cell);
            cell.Attributes.Add("class", "FieldViewLabelCell");

            //content cell
            HtmlTableCell contentCell = new HtmlTableCell();
            contentCell = new HtmlTableCell();
            contentCell.ID = fd.FieldId + "con" + 3;

            Label lblc = new Label();
            lblc.ID = "cLabel_" + fd.FieldId;
            contentCell.Controls.Add(lblc);
            lblc.Text = content;


            currentTableRow.Cells.Add(contentCell);
            //if (nextLastInRowIsAlternate)
            //{
           // contentCell.Attributes.Add("class", contentCSS);
           
        }

		private ButtonColumn CreateButtonColumn(String primaryKey, String command, String captionId)
		{
			//TemplateColumn tcolumn = new TemplateColumn();
			ButtonColumn bcolumn = new ButtonColumn();
			bcolumn.CommandName = command;
			bcolumn.Text = Localization.GetGuiElementText(captionId);
			bcolumn.ButtonType = ButtonColumnType.LinkButton;
			bcolumn.HeaderText = "";
			return bcolumn;
		}

        private GridHyperLinkColumn CreateHyperLinkColumn(String primaryKey, bool Edit, String captionId)
		{
			//TemplateColumn tcolumn = new TemplateColumn();
			//ButtonColumn bcolumn = new ButtonColumn();
            Telerik.WebControls.GridHyperLinkColumn hcolumn = new GridHyperLinkColumn();
			hcolumn.Text = Localization.GetGuiElementText(captionId);

			hcolumn.DataNavigateUrlField = primaryKey;
			//hcolumn.DataNavigateUrlFormatString = "EditRecord.aspx?DataClass=GAPersonnel&Id={0}";
			if (Edit) 
				hcolumn.DataNavigateUrlFormatString = GASystem.GUIUtils.LinkUtils.GenerateURLForSingleRecordDetails(this.DataClass, "{0}");
			else
				hcolumn.DataNavigateUrlFormatString = GASystem.GUIUtils.LinkUtils.GenerateURLForSingleRecordView(this.DataClass, "{0}");
			return hcolumn;
		}

        private HyperLinkColumn CreateViewReportHyperLinkColumn(String primaryKey, String captionId)
        {
            //TemplateColumn tcolumn = new TemplateColumn();
            //ButtonColumn bcolumn = new ButtonColumn();
            HyperLinkColumn hcolumn = new HyperLinkColumn();
            hcolumn.Text = Localization.GetGuiElementText(captionId);

            hcolumn.DataNavigateUrlField = primaryKey;
            hcolumn.Target = "_blank";
            hcolumn.DataNavigateUrlFormatString = GASystem.Reports.Utils.URLGenerator.GenerateURLForSingleRecordDetailsFormatString(this.DataClass, GASystem.Reports.Utils.ReportExportType.PDF);           
            return hcolumn;
        }

		private HyperLinkColumn CreateAdminLinkColumn(String primaryKey, String captionId)
		{
			//TemplateColumn tcolumn = new TemplateColumn();
			//ButtonColumn bcolumn = new ButtonColumn();
			HyperLinkColumn hcolumn = new HyperLinkColumn();
			hcolumn.Text = Localization.GetGuiElementText(captionId);
			hcolumn.DataNavigateUrlField = primaryKey;
			hcolumn.DataNavigateUrlFormatString = "/ga/gagui/WebForms/EditDataRecordRolePermissions.aspx?DataClass="+this.DataClass+"&RowId={0}";
			hcolumn.Target = "_blank";
			return hcolumn;
		}

//		private BoundColumn CreateBoundColumn(DataColumn c)
//		{
//			BoundColumn column = new BoundColumn();
//			column.DataField = c.ColumnName;
//			column.HeaderText = AppUtils.Localization.GetCaptionText(c.ColumnName);
//			column.DataFormatString =setFormatting(c);
//			return column;
//		}

		private	string setFormatting(DataColumn bc)	   
		{	  
			string dataType =	null;	   
			switch(bc.DataType.ToString())			 
			{				 
				case "System.Int32":			  
					dataType = "{0:#,###}";		  
					break;						 
				case "System.Decimal":						 
					dataType =	 "{0:c}";		   
					break;		 
				case "System.DateTime":		 
					dataType= "{0:d}";//+System.Threading.Thread.CurrentThread.CurrentUICulture.DateTimeFormat.ShortDatePattern;//"{0:dd.MM.yyyy}";			
					//dataType=System.Threading.Thread.CurrentThread.CurrentUICulture.DateTimeFormat.sShortDatePattern;
					break;		  
				case "System.String":			 
					dataType="{0,10}";		   
					break;		 
				default:			   
					dataType= "";		  
					break;			 
			}		
			return dataType;	
		}


		
		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
            buttonNewRecordExternal = new HyperLink();
            buttonNewRecordExternal.CssClass = "FlagLinkButton";
            
            if (this.DataClass != null && this.DataClass != string.Empty && DisplayFilter == true) 
			{
				myFilterBuilder = new GASystem.GUIUtils.DataFilter.FilterBuilder(GASystem.DataModel.GADataRecord.ParseGADataClass(this.DataClass));
				PlaceHolderFilter.Controls.Add(myFilterBuilder.FilterControl);
				myFilterBuilder.FilterControl.SetFilterClicked += new EventHandler(FilterControl_SetFilterClicked);
                myFilterBuilder.FilterChanged += new EventHandler(FilterControl_SetFilterClicked);
                myFilterBuilder.FilterControl.AddControlToCommandLine(buttonNewRecordExternal);

			}

           // buttonExportExcel.Click += new EventHandler(buttonExportExcel_Click);

			dg = new Telerik.WebControls.RadGrid();
            dg.EnableViewState = true;
            
			dg.ID = "dg1";
            dg.MasterTableView.EditMode = GridEditMode.InPlace;
			
			dg.BorderWidth = Unit.Pixel(0);
			dg.GridLines = GridLines.None;

			//dg.ItemCommand += new GridCommandEventHandler(this.DataGrid_ItemCommand);
//			dg.SortCommand += new GridSortCommandEventHandler(this.DataGrid_SortCommand);
    		
            dg.ItemDataBound += new GridItemEventHandler(this.DataGrid_ItemDataBound);	
			dg.PageIndexChanged += new GridPageChangedEventHandler(dg_PageIndexChanged);
            dg.ColumnCreating += new GridColumnCreatingEventHandler(dg_ColumnCreating);
            dg.UpdateCommand += new GridCommandEventHandler(dg_UpdateCommand);
        

            dg.ItemCommand += new GridCommandEventHandler(dg_ItemCommand);
            dg.SortCommand += new GridSortCommandEventHandler(dg_SortCommand);

            ButtonImportExcel.Click += ButtonImportExcel_Click;

            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
			base.OnInit(e);


		}
        

        void dg_SortCommand(object source, GridSortCommandEventArgs e)
        {
            if (isCurrentSortDefaultSort)
                dg.MasterTableView.SortExpressions.Clear();
            isCurrentSortDefaultSort = false;
            e.Canceled = true;
            //dg.MasterTableView.SortExpressions.AddSortExpression(e.SortExpression);

            if (dg.MasterTableView.SortExpressions.ContainsExpression(e.SortExpression))
                dg.MasterTableView.SortExpressions.ChangeSortOrder(e.SortExpression);
            else
            {

                GridSortExpression sortExpression = GridSortExpression.Parse(e.SortExpression);
                dg.MasterTableView.SortExpressions.AddAt(0, sortExpression);
                dg.MasterTableView.Rebind();
            }
        }


        public void ExportToExcel()
        {
            //dg = new Telerik.WebControls.RadGrid();
            //dg.EnableViewState = true;
            //dg.ID = "dg1";
            //dg.MasterTableView.EditMode = GridEditMode.InPlace;
			
            
            
            //GenerateExcelDataGrid();
            ////FillGrid();
           
            
            //dg.ExportSettings.ExportOnlyData = true;
            //dg.ExportSettings.IgnorePaging = true;
            //dg.ExportSettings.OpenInNewWindow = true;
            //dg.ExportSettings.FileName = DataClass.ToString() + System.DateTime.Now.Ticks.ToString();

            //dg.MasterTableView.ExportToExcel();

            //dg.DataSource = RecordsDataSet;
            //dg.DataMember = this.DataClass.ToString();


            //dg.DataBind();

            exportToExcel = true;


            dgExport = new GASystem.GAGUI.GAControls.Export.ExportListToExcel();
            
           
            dgExport.ID = this.ID + "excel";
            this.Controls.Add(dgExport);
            dgExport.RecordsDataSet = this.RecordsDataSet;
            dgExport.DataClass = this.DataClass;



            //check if there are aggregate fields
            Boolean isAggregate = false;
            FieldDescription[] fds = FieldDefintion.GetFieldDescriptions(this.DataClass);
            foreach (FieldDescription fd in fds)
            {
                if (!fd.ListAggregat.Equals(string.Empty))
                    isAggregate = true;
            }




            if (isAggregate)
            {
                

                //add empty row
                this.RecordsDataSet.Tables[0].Rows.Add(this.RecordsDataSet.Tables[0].NewRow());


                /// 
                this.RecordsDataSet.Tables[0].Rows.Add(this.RecordsDataSet.Tables[0].NewRow());
                this.RecordsDataSet.Tables[0].Rows[RecordsDataSet.Tables[0].Rows.Count - 1][1] = "Aggregate";
                            

                foreach (FieldDescription fd in fds)
                {

                    if (fd.ListAggregat != string.Empty)
                    {
                        DataTable table;
                        table = this.RecordsDataSet.Tables[0];
                        if (fd.ListAggregat.Equals("SUM"))
                        {
                            string operation = "Sum(" + fd.FieldId + ")";
                            object sumObject = table.Compute(operation, "");
                            
                            double d = 0;
                            // Tor 20141124 set d=0 if no records in dataset
                            // d = double.Parse(sumObject.ToString());
                            if (this.RecordsDataSet.Tables[0].Rows.Count > 0)
                            {
                                try
                                {
                                    d = double.Parse(sumObject.ToString());
                                }
                                catch (Exception ex) { }
                            }
                            this.RecordsDataSet.Tables[0].Rows[RecordsDataSet.Tables[0].Rows.Count - 1][fd.FieldId] = d;
                        }

                        if (fd.ListAggregat.Equals("COUNT"))
                        {
                            string operation = "Count(" + fd.FieldId + ")";
                            object sumObject = table.Compute(operation, "");

                            double d = 0;
                            try
                            {
                                d = double.Parse(sumObject.ToString());
                            }
                            catch (Exception ex) { }

                            this.RecordsDataSet.Tables[0].Rows[RecordsDataSet.Tables[0].Rows.Count - 1][fd.FieldId] = d;

                        }


                        if (fd.ListAggregat.Equals("RSS"))
                        {
                            double total = 0;

                            foreach (DataRow row in table.Rows)
                            {
                                
                                double d = 0;
                                try
                                {
                                    
                                    d = double.Parse(row[fd.FieldId].ToString());
                                }
                                catch (Exception ex) { }


                                d = d / 100;
                                total = total + d * d;
                            }
                            total = Math.Sqrt(total);

                            this.RecordsDataSet.Tables[0].Rows[RecordsDataSet.Tables[0].Rows.Count - 1][fd.FieldId] = total * 100;

                        }

                    }
                }

            }




            

            //set sort
            foreach (GridSortExpression sortExpression in dg.MasterTableView.SortExpressions)
            {
                //need to make a copy of the sort expression. references to the exsiting sortexpression in dg will fail
                GridSortExpression excelSortExpression = new GridSortExpression();
                excelSortExpression.SortOrder = sortExpression.SortOrder;
                excelSortExpression.FieldName = sortExpression.FieldName;
                dgExport.AddSortExpression(excelSortExpression);
            }
            dgExport.FillGrid();
            

        }

        public void ShowImportExcelControl(bool isOpen)
        {
            importXlsUC = (ImportExcelCtrl) this.Page.LoadControl("~/gagui/usercontrols/ImportExcelCtrl.ascx");

            importXlsUC.ID = "importExcel";
            importXlsUC.DataClass = this.DataClass;
            importXlsUC.Owner = this.Owner;
            importXlsUC.OnClosePanel += ImportXlsUC_OnClosePanel;
            importXlsUC.OnSuccessfullyImported += ImportXlsUC_OnSuccessfullyImported;

            this.ImportExcelPHolder.Controls.Clear();
            this.ImportExcelPHolder.Visible = true;
            this.ImportExcelPHolder.Controls.Add(importXlsUC);
            importXlsUC.IsOpenModalView = isOpen;
        }

        private void ImportXlsUC_OnSuccessfullyImported(object sender, EventArgs e)
        {
            this._listUpdated = true;
        }

        private void ImportXlsUC_OnClosePanel(object sender, EventArgs e)
        {
            this.DisplayImportExcelControl = false;
            this.ImportExcelPHolder.Controls.Clear();
        }

        private void ButtonImportExcel_Click(object sender, EventArgs e)
        {
            ShowImportExcelControl(true);
            DisplayImportExcelControl = true;
        }

        void dg_ItemCommand(object source, GridCommandEventArgs e)
        {
            if (e.CommandName == "EditSelected" || e.CommandName == "Edit")
                _radGridInEditMode = true;
            else
                _radGridInEditMode = false;

            if (e.CommandName == "ExportToExcel")
                ExportToExcel();

            if (e.CommandName == "ImportFromExcel")
            {
                DisplayImportExcelControl = true;
                ShowImportExcelControl(true);
            }
            else
            {
                DisplayImportExcelControl = false;
            }

            if (e.CommandName == "UpdateEdited" && e.CommandArgument.ToString() == "CreateNewAfterUpdate")
            {
                createNewRecordWhenUpdatingInList = true;
            }

            if (e.CommandName == "EditSelected" && e.CommandArgument.ToString() == "EditCount")
                _radGridInEditCountMode = true;
            else
                _radGridInEditCountMode = false;

            if (e.CommandName == "CancelSort" )
            {
                dg.MasterTableView.SortExpressions.Clear();
            }
            
            
        }

        void dg_UpdateCommand(object source, GridCommandEventArgs e)
        {
            string test = e.Item.DataItem.ToString();

            System.Data.DataRowView rv = (System.Data.DataRowView)e.Item.DataItem;
            int rowid = (int)rv[this.DataClass.Substring(2) + "rowid"];

            GridEditableItem editedItem = (GridEditableItem)e.Item;
            Hashtable newValues = new Hashtable();
            e.Item.OwnerTableView.ExtractValuesFromItem(newValues, editedItem);

          //  e.Item.Edit = false;
            newValues.Add(this.DataClass.Substring(2) + "rowid", rowid);

            if (this.DataClass.ToUpper() == GASystem.DataModel.GADataClass.GAActionWorkitemView.ToString().ToUpper())
            {
                try
                {
                    newValues.Add("ActionRowId", (int)rv["ActionRowId"]);
                }
                catch { }

            }

            BusinessClass bc = BusinessLayer.Utils.RecordsetFactory.Make(GASystem.DataModel.GADataRecord.ParseGADataClass(this.DataClass));

            if (createNewRecordWhenUpdatingInList)
                bc.UpdateFromListAndCreateNew(newValues, this.Owner);
            else
                bc.UpdateFromList(newValues); // Tor 20161020



            //foreach (DictionaryEntry entry in newValues) 
            //{
            //    Response.Write(String.Format("{0}: {1} <br />", entry.Key, entry.Value)); 
            //}

            this._listUpdated = true;

          //  throw new Exception("The updatecommand method or operation is not implemented.");
        }

        void dg_ColumnCreating(object sender, GridColumnCreatingEventArgs e)
        {
            if ((e.ColumnType == typeof(FlagGUILibrary.WebControls.ListControls.GridBoundLimitColumn).Name))
            {
                e.Column = new FlagGUILibrary.WebControls.ListControls.GridBoundLimitColumn();
            }
        }
		
		/// <summary>
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion


		private void dg_PageIndexChanged(object source, GridPageChangedEventArgs e)
		{
			dg.CurrentPageIndex = e.NewPageIndex;
			CurrentPage = e.NewPageIndex;
			dg.DataBind();
			//FillGrid();
		}


        /// <summary>
        /// Create the hover effect for selecting records
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGrid_ItemDataBound(object sender,
            GridItemEventArgs e)
        {
            // Only display if select button is visible
            //f (!DisplaySelectButton)
            //return;

          


            GridItemType itemType = e.Item.ItemType;

            if ((itemType == GridItemType.Pager) ||
                (itemType == GridItemType.Header) ||
                (itemType == GridItemType.Footer) ||
                (itemType == GridItemType.EditFormItem) ||
                (itemType == GridItemType.FilteringItem) ||
                (itemType == GridItemType.EditItem)

                )
            {
                return;
            }

            GridItem o = e.Item;

            if (e.Item.DataItem == null)        //telerik has a number of differnt item types. Ignore if it is not a dataitem
                return;

            if (e.Item.DataItem.GetType() != typeof(System.Data.DataRowView))
                return;



            System.Data.DataRowView rowView = (System.Data.DataRowView)e.Item.DataItem;
            // Select hyper link button is always in last cell
            //HyperLink button = (HyperLink)o.Cells[o.Cells.Count-1].Controls[0];


            ClassDescription cd = ClassDefinition.GetClassDescriptionByGADataClass(this.DataClass);


            if (!cd.hasViewSQL())
            {
                foreach (TableCell cell in e.Item.Cells)
                {

                    //cell.Attributes["onmouseover"] = "javascript:GAScript_changeBackgroundColor(this, true)";
                    //cell.Attributes["onmouseout"] = "javascript:GAScript_changeBackgroundColor(this, false)";
                    if (cell.Controls.Count > 0)
                    {
                        if (cell.Controls[0].GetType() != typeof(System.Web.UI.WebControls.CheckBox) && cell.Controls[0].GetType() != typeof(System.Web.UI.WebControls.HyperLink) )
                        {
                            cell.Attributes["onclick"] = "javascript:GAScript_goto('" + Request.Url.Scheme + "://" + Request.Url.Host + Request.ApplicationPath + (Request.ApplicationPath.EndsWith("/") ? "" : "/") + GUIUtils.LinkUtils.GenerateSimpleJscriptURLForSingleRecordView(_viewRecordDataClass, rowView.Row[this.DataClass.ToString().Substring(2) + "rowid"].ToString()) + "')";
                        }
                    }
                    else
                        cell.Attributes["onclick"] = "javascript:GAScript_goto('" + Request.Url.Scheme + "://" + Request.Url.Host + Request.ApplicationPath + (Request.ApplicationPath.EndsWith("/") ? "" : "/") + GUIUtils.LinkUtils.GenerateSimpleJscriptURLForSingleRecordView(_viewRecordDataClass, rowView.Row[this.DataClass.ToString().Substring(2) + "rowid"].ToString()) + "')";

                }
            }

            //foreach (TableCell cell in e.Item.Cells)
            //{
                
            //    //cell.Attributes["onmouseover"] = "javascript:GAScript_changeBackgroundColor(this, true)";
            //    //cell.Attributes["onmouseout"] = "javascript:GAScript_changeBackgroundColor(this, false)";
            //    if (cell.Controls.Count > 0)
            //    {
            //        // Tor 20121115 changed: not to View record if the cell that is clicked on is chackbox or hyperlink: if (cell.Controls[0].GetType() != typeof(System.Web.UI.WebControls.CheckBox))
            //        if (cell.Controls[0].GetType() != typeof(System.Web.UI.WebControls.CheckBox) && cell.Controls[0].GetType() != typeof(System.Web.UI.WebControls.HyperLink))
            //            {
            //            cell.Attributes["onclick"] = "javascript:GAScript_goto('" + Request.Url.Scheme + "://" + Request.Url.Host + Request.ApplicationPath + (Request.ApplicationPath.EndsWith("/") ? "" : "/") + GUIUtils.LinkUtils.GenerateSimpleURLForSingleRecordView(_viewRecordDataClass, rowView.Row[this.DataClass.ToString().Substring(2) + "rowid"].ToString()) + "')";
            //        }
            //    } else
            //        cell.Attributes["onclick"] = "javascript:GAScript_goto('" + Request.Url.Scheme + "://" + Request.Url.Host + Request.ApplicationPath + (Request.ApplicationPath.EndsWith("/") ? "" : "/") + GUIUtils.LinkUtils.GenerateSimpleURLForSingleRecordView(_viewRecordDataClass, rowView.Row[this.DataClass.ToString().Substring(2) + "rowid"].ToString()) + "')";
                    
            //}

            e.Item.Attributes["onmouseover"] = "javascript:GAScript_changeBackgroundColor(this, true)";
            e.Item.Attributes["onmouseout"] = "javascript:GAScript_changeBackgroundColor(this, false)";
            // the next line of code is horrible
            //e.Item.Attributes["onclick"] = "javascript:GAScript_goto('" + Request.Url.Scheme + "://" + Request.Url.Host + Request.ApplicationPath + (Request.ApplicationPath.EndsWith("/") ? "" : "/") + button.NavigateUrl.Remove(0,2) + "')";
            //e.Item.Attributes["onclick"] = "javascript:GAScript_goto('" + Request.Url.Scheme + "://" + Request.Url.Host + Request.ApplicationPath + (Request.ApplicationPath.EndsWith("/") ? "" : "/") + GUIUtils.LinkUtils.GenerateSimpleURLForSingleRecordView(this.DataClass.ToString(), rowView.Row[this.DataClass.ToString().Substring(2) + "rowid"].ToString()) + "')";
        }

		private void DataGrid_SortCommand(object source, GridSortCommandEventArgs e)
		{
			//the list control does no longer hold the recordset in viewstate. raising a event so that it is up to the 
			//parent of this control to reload  the recordset
			
			Trace.Warn("Sorting on "+e.SortExpression);
			//SortColumn = e.SortExpression;
			SortColumn = SortColumn.Equals(e.SortExpression) ? e.SortExpression + " DESC" : e.SortExpression;
			GACommandEventArgs GAEventArgs = new GACommandEventArgs();
			GAEventArgs.CommandStringArgument = SortColumn;
			GAEventArgs.CommandName = "Sort";
			if (SortClicked != null)
				SortClicked(this, GAEventArgs);
			RefreshGrid();
		}
		
//		private void NewRecord_Click(object sender, System.EventArgs e)
//		{
//			GACommandEventArgs GAEventArgs = new GACommandEventArgs();
//			GAEventArgs.CommandName = "NewRecord";
//			if (GACommandExecuted!=null)
//				GACommandExecuted(this, GAEventArgs);
//			
//		}
//
//		private void ButtonNewRecord_Click(object sender, System.EventArgs e)
//		{
//			GACommandEventArgs GAEventArgs = new GACommandEventArgs();
//			GAEventArgs.CommandName = "NewRecord";
//			if (null!=NewRecordClicked)
//				NewRecordClicked(sender, GAEventArgs);
//		}


		/// <summary>
		/// Get default sort from fielddefinition
		/// </summary>
		/// <returns></returns>
		private string getDefaultSort() 
		{
			GASystem.DataModel.GADataClass sortDataClass;
			try 
			{
				sortDataClass = GASystem.DataModel.GADataRecord.ParseGADataClass(this.DataClass);
			} 
			catch (Exception ex) 
			{
				//could not parse dataclass, it might be a view that is not defined in GADataClass
				return string.Empty;
			}
			return GASystem.BusinessLayer.FieldDefinition.GetSortOrderDefinitionForGADataClass(sortDataClass);
		}

		//Provide a way to store the dataset in viewState
        //stored as session in order ot avoid large viewstate. used dataclassname and listidentfier to generate sessionname
		public DataSet RecordsDataSet
		{
            get
            {
                //return null==ViewState["RecordsDataSet"+this.ID] ? null : (DataSet) ViewState["RecordsDataSet"+this.ID];
                return null == Session["RecordsDataSet" + this.DataClass + ListIdentifier] ? null : (DataSet)Session["RecordsDataSet" + this.DataClass + ListIdentifier];

                //return _recordDataSet;
            }
            set
            {
                //ViewState["RecordsDataSet"+this.ID] = value;
                Session["RecordsDataSet" + this.DataClass + ListIdentifier] = value;
                //_recordDataSet = value;
            }
		}



        /// <summary>
        /// Custom string used to identify the listcontrol. Used by some controls
        /// </summary>
        private string _listIdentifier = string.Empty;
        public string ListIdentifier
        {
            get { return _listIdentifier; }
            set { _listIdentifier = value; }
        }
	





		public string SortColumn
		{
			get
			{
				return null==ViewState["SortColumn"+this.ID + this.DataClass] ? getDefaultSort() : (string) ViewState["SortColumn"+this.ID + this.DataClass];
			}
			set
			{
				ViewState["SortColumn"+this.ID +  this.DataClass] = value;
			}
		}

		public bool DisplayNewButton
		{
			get
			{
				return null==ViewState["DisplayNewButton"+this.ID] ? false : (bool) ViewState["DisplayNewButton"+this.ID];
			}
			set
			{
				ViewState["DisplayNewButton"+this.ID] = value;
			}
		}
		public bool DisplaySelectButton
		{
			get
			{
				return null==ViewState["DisplaySelectButton"+this.ID] ? false : (bool) ViewState["DisplaySelectButton"+this.ID];
			}
			set
			{
				ViewState["DisplaySelectButton"+this.ID] = value;
			}
		}
		public bool DisplaySelectPostBackButton
		{
			get
			{
				return null==ViewState["DisplaySelectPostBackButton"+this.ID] ? false : (bool) ViewState["DisplaySelectPostBackButton"+this.ID];
			}
			set
			{
				ViewState["DisplaySelectPostBackButton"+this.ID] = value;
			}
		}
		public bool DisplayEditButton
		{
			get
			{
				return null==ViewState["DisplayEditButton"+this.ID] ? false : (bool) ViewState["DisplayEditButton"+this.ID];
			}
			set
			{
				ViewState["DisplayEditButton"+this.ID] = value;
			}
        }

        public bool DisplayImportExcelControl
        {
            get
            {
                return null == ViewState["DisplayImportExcelControl" + this.ID] ? false : (bool)ViewState["DisplayImportExcelControl" + this.ID];
            }
            set
            {
                ViewState["DisplayImportExcelControl" + this.ID] = value;
            }
        }

        /// <summary>
        /// Allows users of this class to set whether the cancelsort link shold be displayed on in
        /// the commandtemplate. Default value is true
        /// </summary>
        bool _displayCancelSortButton = true;
        public bool DisplayCancelSortButton
        {
            get
            {
                return _displayCancelSortButton;
            }
            set
            {
                _displayCancelSortButton = value;
            }
        }

        public bool DisplayExportToExcelLink
        {
            get
            {
                return _displayExportToExcelLink;
            }
            set
            {
                _displayExportToExcelLink = value;
            }
        }

        public bool DisplayCommandTemplate 
        {
            set {_displayCommandTemplate = value;}
            get {return _displayCommandTemplate;}
        }

        //August2020- import excel 
        public string ImportExcelTextLink
        {
            get
            {
                return GASystem.AppUtils.Localization.GetGuiElementText("importFromExcel");
            }
        }


        //Provide a way to store the dataset in viewState
        public String DataClass
		{
			get
			{
				return null==ViewState["DataClass"+this.ID] ? null : (String) ViewState["DataClass"+this.ID];
			}
			set
			{
				ViewState["DataClass"+this.ID] = value;
			}
		}

		//Provide a way to store the owner in viewState
		public GASystem.DataModel.GADataRecord Owner
		{
			get
			{
				return null==ViewState["Owner"+this.ID] ? null : (GASystem.DataModel.GADataRecord) ViewState["Owner"+this.ID];
			}
			set
			{
				ViewState["Owner"+this.ID] = value;
			}
		}

		/// <summary>
		/// Provide a way to the DisplayFilter setting in viewstate
		/// returns false if the value is not exsplisitly set
		/// </summary>
		public bool DisplayFilter
		{
			get
			{
				return null==ViewState["DisplayFilter"+this.ID] ? false : (bool) ViewState["DisplayFilter"+this.ID];
			}
			set
			{
				ViewState["DisplayFilter"+this.ID] = value;
			}
		}

		/// <summary>
		/// Provide a way to store the current datagrid page. Viewstate is not enabled for datagrid, so previous page number is lost.
		/// We store it here on changes and reset it in generate datagrid
		/// </summary>
		public int CurrentPage 
		{
			get
			{
				return null==ViewState["CurrentPage"+this.ID] ? 0 : (int) ViewState["CurrentPage"+this.ID];
			}
			set
			{
                ViewState["CurrentPage" + this.ID] = value;
			}
		}


        /// <summary>
        /// Flag indicating whether we are in edit count mode
        /// Neede to be moved to viewstate due to postbacks
        /// </summary>
        private bool _radGridInEditCountMode
        {
			get
			{
				return null==ViewState["radGridInEditCountMode"+this.ID] ? false : (bool) ViewState["radGridInEditCountMode"+this.ID];
			}
			set
			{
                ViewState["radGridInEditCountMode" + this.ID] = value;
			}
		}


        /// <summary>
        /// Used to control whether the current sorting is the default system sort
        /// </summary>
        private bool isCurrentSortDefaultSort
        {
            get
            {
                return null == ViewState["isCurrentSortDefaultSort" + this.ID] ? true : (bool)ViewState["isCurrentSortDefaultSort" + this.ID];
            }
            set
            {
                ViewState["isCurrentSortDefaultSort" + this.ID] = value;
            }
        }



        public string BaseURL
        {
            get { return _baseURL; }
            set { _baseURL = value; }
        }


        private void FilterControl_SetFilterClicked(object sender, EventArgs e)
		{
			this.CurrentPage = 0;
            _listUpdated = true;

            ////TEST JOF 11/10/2007
            //if (BaseURL != string.Empty)
            //{
            //    //string url = GASystem.GUIUtils.LinkUtils.GenerateURLForListAll(this.DataClass);
            //    this.Page.Response.Redirect(BaseURL + myFilterBuilder.getQueryStringFilter());
            //}
		}

        public bool ListUpdated
        {
            get { return _listUpdated; }
        }


        /// <summary>
        /// Allows user of this class to override the default sort settings defined in fielddefinition.
        /// </summary>
        private string _customSort = string.Empty;
        public string CustomSort
        {
            get { return _customSort; }
            set { _customSort = value; }
        }
	

        
        /// <summary>
        /// Reset sortorder to default sortorder on next fillgrid.
        /// </summary>
        public bool ResetSort
        {
            get { return _resetSort; }
            set { _resetSort = value; }
        }

        public void clearViewStates()
        {
            ViewState["CurrentPage" + this.ID] = null;
            
        }

        public string getFilter()
        {
            if (myFilterBuilder == null)
                return string.Empty;
            return myFilterBuilder.GetFilterString();
        }

        public void setDefaultFilter()
        {
            if (Request.QueryString["nofilter"] != null && Request.QueryString["nofilter"].ToString() == "true")
                return;  //do not filter data

            if (myFilterBuilder != null)
            {
                if (myFilterBuilder != null && !Page.IsPostBack && Request.QueryString["ffield"] != null)
                {
                    string fName = Request.QueryString["ffield"] == null ? string.Empty : Request.QueryString["ffield"].ToString();
                    string fCondition = Request.QueryString["fCondition"] == null ? string.Empty : Request.QueryString["fCondition"].ToString();
                    string fOperator = Request.QueryString["fOperator"] == null ? string.Empty : Request.QueryString["fOperator"].ToString();

                    myFilterBuilder.SetQueryStringFilter(fName, fOperator, fCondition);
                }
                else if (myFilterBuilder != null && !Page.IsPostBack && Request.QueryString["ffield0"] != null) {

                    myFilterBuilder.FilterControl.EditVisible = true;
                        //check for other filter settings
                        int maxFilter = 20;
                        if (myFilterBuilder != null && !Page.IsPostBack)
                        {
                            for (int t = 0; t < maxFilter; t++)
                            {
                                if (Request.QueryString["ffield" + t.ToString()] != null)
                                {
                                    string fName = Request.QueryString["ffield" + t.ToString()] == null ? string.Empty : Request.QueryString["ffield" + t.ToString()].ToString();
                                    string fCondition = Request.QueryString["fCondition" + t.ToString()] == null ? string.Empty : Request.QueryString["fCondition" + t.ToString()].ToString();
                                    string fOperator = Request.QueryString["fOperator" + t.ToString()] == null ? string.Empty : Request.QueryString["fOperator" + t.ToString()].ToString();

                                    myFilterBuilder.SetQueryStringFilter(fName, fOperator, fCondition);
                                }
                            }
                        }
                } 
                else
                {
                    myFilterBuilder.SetDefaultFilter();
                }

            }
        }

        public bool HasAccessToImportExcel()
        {
            GASystem.DataModel.GADataClass dataClass = GASystem.DataModel.GADataRecord.ParseGADataClass(this.DataClass);

            if (dataClass != GASystem.DataModel.GADataClass.GASafetyObservation
                || Owner.DataClass != GADataClass.GALocation)
                return false;

            if (Security.IsGAAdministrator())
                return true;

            GASystem.DataModel.DataRecordRolePermissionsDS roles =
                Security.GetSecurityRolesAccessForArcLink(Owner.DataClass, dataClass);

            if (roles == null || roles.DataRecordRolePermissions.Rows.Count == 0)
                return false;

            //todo  select * from GALists where GAListCategory='SYS'   7505
            bool hasCreatePermission = Security.HasCreatePermissionByRoleId(dataClass, Owner, 7505) ||
                Security.HasCreatePermissionByRoleId(dataClass, Owner, 6098);

            if (hasCreatePermission)
            {
                return true;
            }

            return false;
        }
    }
}
