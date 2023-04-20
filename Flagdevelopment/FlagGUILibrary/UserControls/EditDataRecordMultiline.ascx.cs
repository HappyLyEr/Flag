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
	using GASystem.DataModel;
	using GASystem.BusinessLayer.Utils;
	using GASystem.DataAccess;
	using log4net;
	using GASystem.WebControls.EditControls;

	/// <summary>
	///		Summary description for EditDataRecordMultiline.
	/// </summary>
	public class EditDataRecordMultiline : System.Web.UI.UserControl
	{
		private static readonly ILog _logger = LogManager.GetLogger(typeof(EditDataRecordMultiline));
		
		//protected DBauer.Web.UI.WebControls.DynamicControlsPlaceholder DCP;
		protected System.Web.UI.WebControls.PlaceHolder ListPHolder;
		protected DataGrid dg;
		protected System.Web.UI.WebControls.Label MessageLabel;

		private bool _refreshGridCalled = false;
		private DataSet _recordDataSet;
		protected System.Web.UI.WebControls.Button ButtonCancel;
		protected System.Web.UI.WebControls.Button ButtonSave;

		protected Button btn;

		public event DataGridCommandEventHandler EditGARecordClicked;
		public event GACommandEventHandler SelectRecordClicked;
		public event GACommandEventHandler EditRecordClicked;
		public event GACommandEventHandler NewRecordClicked;
		public event GACommandEventHandler GACommandExecuted;
		public event GACommandEventHandler SortClicked;

		public event GACommandEventHandler EditRecordCancel;
		public event GACommandEventHandler EditRecordSave;


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
			
		}

		public void ClearGrid()
		{
			if (null!=dg) 
				dg.Controls.Clear();
			
		}


		//Expose this method so that consumers may refresh grid
		public void RefreshGrid() 
		{
			_refreshGridCalled = true;
			FillGrid();
		}

		
		/// <summary>
		/// Refresh datagrid. Remember to set RecordsDataSet and DataClass first.
		/// ViewState must be recreated when calling this method! (Do not call this method
		/// from the page_init event)
		/// </summary>
		/// 
		protected override void OnPreRender(EventArgs e)
		{
			if (_refreshGridCalled) 
				base.OnPreRender (e);
		}



		public void FillGrid()
		{
			//ButtonNewRecord.Visible = DisplayNewButton;

			//			Trace.Warn("RecordsDataSet="+RecordsDataSet+", DataClass="+DataClass+",RecordsDataSet.Tables[DataClass]="+RecordsDataSet.Tables[DataClass]);
			
			
			//if (dg==null) Trace.Warn("Can't refresh, datagrid is null");
			//			if (RecordsDataSet==null) {Trace.Warn("Can't refresh, RecordsDataSet is null");}
			//else if (RecordsDataSet.Tables[DataClass]==null) Trace.Warn("Can't refresh, RecordsDataSet.Tables[DataClass] is null");

			
			if (dg!=null && RecordsDataSet!=null && RecordsDataSet.Tables[DataClass]!=null)
			{
				//if (dg.Columns.Count==0)  // regenerates datagrid each time, workaround for reuse of datagrid bug.
				//TODO, redisign to fix bug properly
				GenerateDataGrid();

				Trace.Warn(this.ID+":Refreshing grid for dataclass "+DataClass);
				
				DataView view = RecordsDataSet.Tables[DataClass].DefaultView;
				view.RowFilter = QueryFilter;
				view.Sort = SortColumn;
				
				if (0 == RecordsDataSet.Tables[DataClass].Rows.Count)
				{
					
					MessageLabel.Text = String.Format(Localization.GetGuiElementText("NoRecords"), Localization.GetGuiElementTextPlural(this.DataClass));
					MessageLabel.CssClass = "MessageNormal";
					dg.Columns.Clear(); //removes column headers
				}
				else
				{	
					MessageLabel.Text = "";
					dg.DataSource = view; //RecordsDataSet.Tables[DataClass].DefaultView;
					dg.DataBind();
					StoreGridControlIdsInViewSate();
				}

				
			}
		}

		//Create a 2d array of datagrid edit-control ids
		private void StoreGridControlIdsInViewSate() 
		{
			//String[,] controlIds = new String[dg.Items.Count,dg.Columns.Count];
			ArrayList controlIds = new ArrayList();

			int i=0;
			foreach (DataGridItem gridRow in dg.Items) 
			{
				DataRow row = RecordsDataSet.Tables[0].Rows[gridRow.DataSetIndex];
				ArrayList rowControlData = new ArrayList();
				foreach (DataColumn column in RecordsDataSet.Tables[0].Columns) 
				{
					System.Web.UI.Control control = gridRow.FindControl(column.ColumnName);
					if (control!=null) 
					{
						GridControlData tmpControl = new GridControlData();
						tmpControl.ControlType = control.GetType();
						tmpControl.ControlId = control.ClientID;
						tmpControl.GridColumnName = control.ID;
						rowControlData.Add(tmpControl);
						
					}
					
				}
				controlIds.Add(rowControlData);
				i++;
			}
			ViewState["GridControlsData"] = controlIds;

		}


		private void GenerateDataGrid()
		{
			if (RecordsDataSet==null || RecordsDataSet.Tables[DataClass]==null) return;

			Trace.Warn(this.ID+":Generate grid definition for dataclass "+DataClass);

			dg.Columns.Clear();
			dg.AutoGenerateColumns = false;
			//dg.EnableViewState= true;
			dg.AllowSorting = false;
			dg.AllowPaging = false;
			//dg.PageSize = 25;
			//dg.PagerStyle.Mode = PagerMode.NumericPages;
			//dg.PagerStyle.CssClass = "gridStyle_PagerStyle";
			//dg.PagerStyle.Position = PagerPosition.TopAndBottom;

			dg.DataKeyField = RecordsDataSet.Tables[DataClass].PrimaryKey[0].ColumnName;

			GUIUtils.GAListTemplateEditColumnFactory factory = new GUIUtils.GAListTemplateEditColumnFactory();
			ArrayList myBoundColumns = new ArrayList();
			foreach(DataColumn c in RecordsDataSet.Tables[DataClass].Columns) 
			{
				GAListColumn tmpColumn = factory.getGAListColumn(c, (SortColumn.Equals(c.ColumnName)), setFormatting(c));
				if (tmpColumn!=null && !tmpColumn.getFieldDescription().HideInSummary)
				{
					myBoundColumns.Add(tmpColumn);
				}
			}
			myBoundColumns.Sort();
			

			//if (DisplayEditButton) myBoundColumns.Add(CreateHyperLinkColumn(RecordsDataSet.Tables[DataClass].PrimaryKey[0].ColumnName, true, "Edit"));
			//if (DisplaySelectButton) myBoundColumns.Add(CreateHyperLinkColumn(RecordsDataSet.Tables[DataClass].PrimaryKey[0].ColumnName, false, "Select"));
			//if (DisplaySelectPostBackButton) myBoundColumns.Add(CreateButtonColumn(RecordsDataSet.Tables[DataClass].PrimaryKey[0].ColumnName, "Select", "Select"));
			//if (Security.IsGAAdministrator()) myBoundColumns.Add(CreateAdminLinkColumn(RecordsDataSet.Tables[DataClass].PrimaryKey[0].ColumnName, "Admin"));
			

			foreach (DataGridColumn c in myBoundColumns)
				dg.Columns.Add(c); 

			GUIGenerateUtils.ApplyDatagridSkin(dg);
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

		private HyperLinkColumn CreateHyperLinkColumn(String primaryKey, bool Edit, String captionId)
		{
			//TemplateColumn tcolumn = new TemplateColumn();
			//ButtonColumn bcolumn = new ButtonColumn();
			HyperLinkColumn hcolumn = new HyperLinkColumn();
			hcolumn.Text = Localization.GetGuiElementText(captionId);

			hcolumn.DataNavigateUrlField = primaryKey;
			//hcolumn.DataNavigateUrlFormatString = "EditRecord.aspx?DataClass=GAPersonnel&Id={0}";
			if (Edit) 
				hcolumn.DataNavigateUrlFormatString = GASystem.GUIUtils.LinkUtils.GenerateURLForSingleRecordDetails(this.DataClass, "{0}");
			else
				hcolumn.DataNavigateUrlFormatString = GASystem.GUIUtils.LinkUtils.GenerateURLForSingleRecordView(this.DataClass, "{0}");
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

		private void initGrid() 
		{
			dg = new DataGrid();
			dg.EnableViewState = false;
			dg.ID = "dg1";
			ListPHolder.Controls.Clear();
			ListPHolder.Controls.Add(dg);
			dg.BorderWidth = Unit.Pixel(0);
			dg.GridLines = GridLines.None;
		}

		
		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
				initGrid();

				dg.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.DataGrid_ItemCommand);
				dg.SortCommand += new System.Web.UI.WebControls.DataGridSortCommandEventHandler(this.DataGrid_SortCommand);
				dg.ItemDataBound += new DataGridItemEventHandler(this.DataGrid_ItemDataBound);	
				dg.PageIndexChanged += new DataGridPageChangedEventHandler(dg_PageIndexChanged);
			
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.ButtonSave.Click += new System.EventHandler(this.ButtonSave_Click);
			this.ButtonCancel.Click += new System.EventHandler(this.ButtonCancel_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion


		private void dg_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
		{
			dg.CurrentPageIndex = e.NewPageIndex;
			FillGrid();
		}

		private void DataGrid_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
		{
			Trace.Warn("Itemcommand on "+e.CommandName);
			
			if (e.CommandName.Equals("Edit"))
			{
				
				String key = dg.DataKeys[e.Item.ItemIndex].ToString();

				if (EditGARecordClicked!=null)
					EditGARecordClicked(source, e);
				
				Trace.Warn("Clicked RowId "+key);
			
				GACommandEventArgs GAEventArgs = new GACommandEventArgs();
				GAEventArgs.CommandName = "EditRecord";
				GAEventArgs.CommandStringArgument = key;
				if (AppUtils.GAUtils.IsNumeric(key)) GAEventArgs.CommandIntArgument = int.Parse(key);

				if (GACommandExecuted!=null) //deprecated
					GACommandExecuted(this, GAEventArgs);

				if (EditRecordClicked!=null)
					EditRecordClicked(this, GAEventArgs);
			}
			else if (e.CommandName.Equals("Select"))
			{
				String key = dg.DataKeys[e.Item.ItemIndex].ToString();
				GACommandEventArgs GAEventArgs = new GACommandEventArgs();
				GAEventArgs.CommandName = "SelectRecord";
				GAEventArgs.CommandStringArgument = key;
				if (AppUtils.GAUtils.IsNumeric(key)) GAEventArgs.CommandIntArgument = int.Parse(key);

				if (SelectRecordClicked!=null)
					SelectRecordClicked(this, GAEventArgs);

			}
			
		}

		private void DataGrid_ItemDataBound(object sender, 
			System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			// Only display if select button is visible
			if (!DisplaySelectButton)
				return;

			ListItemType itemType = e.Item.ItemType;
			if ((itemType == ListItemType.Pager) || 
				(itemType == ListItemType.Header) || 
				(itemType == ListItemType.Footer)) 
			{
				return;
			}

			DataGridItem o = e.Item;
			// Select hyper link button is always in last cell
			//HyperLink button = (HyperLink)o.Cells[o.Cells.Count-1].Controls[0];

	//		e.Item.Attributes["onmouseover"] = "javascript:GAScript_changeBackgroundColor(this, true)";
	//		e.Item.Attributes["onmouseout"] = "javascript:GAScript_changeBackgroundColor(this, false)";
			// the next line of code is horrible
	//		e.Item.Attributes["onclick"] = "javascript:GAScript_goto('" + Request.Url.Scheme + "://" + Request.Url.Host + Request.ApplicationPath + (Request.ApplicationPath.EndsWith("/") ? "" : "/") + button.NavigateUrl.Remove(0,2) + "')";
		}

		private void DataGrid_SortCommand(object source, System.Web.UI.WebControls.DataGridSortCommandEventArgs e)
		{
			//the list control does no longer hold the recordset in viewstate. raising a event so that it is up to the 
			//parent of this control to reload  the recordset
			
			Trace.Warn("Sorting on "+e.SortExpression);
			SortColumn = e.SortExpression;
			GACommandEventArgs GAEventArgs = new GACommandEventArgs();
			GAEventArgs.CommandStringArgument = SortColumn;
			GAEventArgs.CommandName = "Sort";
			if (SortClicked != null)
				SortClicked(this, GAEventArgs);
			RefreshGrid();
		}
		
		private void NewRecord_Click(object sender, System.EventArgs e)
		{
			GACommandEventArgs GAEventArgs = new GACommandEventArgs();
			GAEventArgs.CommandName = "NewRecord";
			if (GACommandExecuted!=null)
				GACommandExecuted(this, GAEventArgs);
			
		}

		private void ButtonNewRecord_Click(object sender, System.EventArgs e)
		{
			GACommandEventArgs GAEventArgs = new GACommandEventArgs();
			GAEventArgs.CommandName = "NewRecord";
			if (null!=NewRecordClicked)
				NewRecordClicked(sender, GAEventArgs);
		}

		private void ButtonSave_Click(object sender, System.EventArgs e)
		{
			
			/*
			 * The dataset is contructed by the RecordsetFactory.GetRecordSetAllDetailsForDataClassByOwner() method
			 * Foreign key columns in the DataSet contains display values (String values). Need
			 * to map the dataset back to something similar to the database schema. Foreign key
			 * columns are suffixed with _keyid. E.g:
			 *  Column Personnel contains a string with the persons name
			 *  Column personnel_keyid contains the personnel rowid 
			 **/
			if (ViewState["GridControlsData"]!=null) 
			{
				ArrayList controlIds = (ArrayList) ViewState["GridControlsData"];
				for (int i=0;i<controlIds.Count;i++) 
				{
					ArrayList rowControlData = (ArrayList) controlIds[i];
					for (int j=0;j<rowControlData.Count;j++)
					{
						GridControlData controlData = (GridControlData) rowControlData[j];
						String controlId = controlData.ControlId.Replace("_", ":").Replace("::", ":_");

						if (controlData.ControlType.Name.Equals("TextBox"))
						{
							String val = Request[controlId];
							if (val!=null) 
							{
								if (RecordsDataSetPersist.Tables[0].Columns.IndexOf(controlData.GridColumnName)>-1)
								{
									if (RecordsDataSet.Tables[0].Rows[i][controlData.GridColumnName].GetType()==RecordsDataSetPersist.Tables[0].Rows[i][controlData.GridColumnName].GetType())
										RecordsDataSetPersist.Tables[0].Rows[i][controlData.GridColumnName] = val;
								}
							}
						}
						else if (controlData.ControlType.Name.Equals("DateTimeControl"))
						{
							//consist of 2 textboxes named date and time
							String date = Request[controlId + ":date"];
							String time = Request[controlId + ":time"];
							try 
							{	
								DateTime selectedDateTime = Convert.ToDateTime(date);
								DateTime selectedTime = Convert.ToDateTime(time);
								selectedDateTime = selectedDateTime.Add(selectedTime.TimeOfDay);
								if (RecordsDataSetPersist.Tables[0].Columns.IndexOf(controlData.GridColumnName)>-1)
								{
									RecordsDataSetPersist.Tables[0].Rows[i][controlData.GridColumnName] = selectedDateTime;
								}
							} 
							catch (Exception ex)
							{
								_logger.Error("Unable to parse date from multiline edit!");
							}
						}
						else if (controlData.ControlType.Name.Equals("DropDownList"))
						{
							String val = Request[controlId];
							if (val!=null) 
							{
								if (RecordsDataSetPersist.Tables[0].Columns.IndexOf(controlData.GridColumnName)>-1)
								{
									RecordsDataSetPersist.Tables[0].Rows[i][controlData.GridColumnName] = val;
								}
							}
						}
						else if (controlData.ControlType.Name.Equals("CheckBox"))
						{
							String val = Request[controlId];
							if (val!=null) 
							{
								bool trueFalse = val.Equals("on") || val.Equals("true");
								if (RecordsDataSetPersist.Tables[0].Columns.IndexOf(controlData.GridColumnName)>-1)
								{
									RecordsDataSetPersist.Tables[0].Rows[i][controlData.GridColumnName] = trueFalse;
								}
							}
						}
					}
				}
			}
		
			ArrayList removeColumns = new ArrayList();
			foreach (DataColumn column in RecordsDataSet.Tables[0].Columns)
			{
				if (column.ColumnName.EndsWith("_keyid"))
				{
					String displayColumnName = column.ColumnName.Substring(0, column.ColumnName.IndexOf("_keyid"));
					DataColumn displayColumn = RecordsDataSet.Tables[0].Columns[displayColumnName];
					
					if (displayColumn!=null) 
					{
						//change name of key column and display column
						displayColumn.ColumnName = displayColumnName + "_display";
						column.ColumnName = displayColumnName;
						removeColumns.Add(displayColumn.ColumnName);	
					}
				}
				//Columns that references GAFile are prefixed with _rowid
				if (column.ColumnName.EndsWith("_rowid")) 
				{
					String displayColumnName = column.ColumnName.Substring(0, column.ColumnName.IndexOf("_rowid"));
					DataColumn displayColumn = RecordsDataSet.Tables[0].Columns[displayColumnName];
					if (displayColumn!=null) 
					{
						//change name of key column and display column
						displayColumn.ColumnName = displayColumnName + "_display";
						column.ColumnName = displayColumnName;
						removeColumns.Add(displayColumn.ColumnName);
					}
				}
				
			}

			foreach (String columnName in removeColumns)
			{
				RecordsDataSet.Tables[0].Columns.Remove(columnName);
			}


			//Must copy data from grid to dataset (reverse binding).
			/*foreach (DataGridItem gridRow in dg.Items) 
			{
				DataRow row = RecordsDataSet.Tables[0].Rows[gridRow.DataSetIndex];
				foreach (DataColumn column in RecordsDataSet.Tables[0].Columns) 
				{
					
					System.Web.UI.Control control = gridRow.FindControl(column.ColumnName);
					if (control!=null) 
					{
						row = ExtractGridCellContent(control, row, column);
						/*if (control is TextBox) 
						{
							row[column.ColumnName] = ((TextBox)control).Text;
						}*/
		//			}
		//		}
		//		RecordsDataSet.Tables[0].Rows[gridRow.DataSetIndex].ItemArray = row.ItemArray;
		//	}
			


			BusinessClass bc = RecordsetFactory.Make(GADataRecord.ParseGADataClass(DataClass));
			/**DataColumn primaryKey = RecordsDataSet.Tables[0].Columns["EmploymentRowId"];
			if (primaryKey!=null)
				RecordsDataSet.Tables[0].PrimaryKey = new DataColumn[] {primaryKey};*/

			bc.CommitDataSet(RecordsDataSetPersist);

			if (null!=EditRecordSave)
			{
				GACommandEventArgs args = new GACommandEventArgs();
				args.CommandDataSetArgument = RecordsDataSet;
				args.CommandName = "Save";
				EditRecordSave(sender, args);
			}


		}

		private void ButtonCancel_Click(object sender, System.EventArgs e)
		{
			GACommandEventArgs args = new GACommandEventArgs();
			EditRecordCancel(sender, args);
		}


		private DataRow ExtractGridCellContent(System.Web.UI.Control control, DataRow row, DataColumn c) 
		{
			TextBox tmpTextBox;
			CheckBox tmpCheckBox;
			DropDownList tmpDropDownList;
			PersonnelField tmpPersonnelField;
				
			RelatedDataRecordField tmpRelatedDataRecordField;
			WebControls.EditControls.DateControl tmpDateControl;
			WebControls.EditControls.DateTimeControl tmpDateTimeControl;
			WebControls.EditControls.Numeric tmpNumeric;
			WebControls.EditControls.FileContent tmpFileContent;
			WebControls.EditControls.FileMimetype tmpFileMimetype;
			WebControls.EditControls.HazardMatrix.HazardMatrixControl tmpHazardMatrix;
			WebControls.EditControls.YearMonthSpan tmpYearMonthSpan;

			if (null != (tmpTextBox = control as TextBox))
			{
						
				if (c.DataType.Equals( typeof(System.Int32))) 
				{
					if (GAUtils.IsNumeric(tmpTextBox.Text))
						row[c]= int.Parse(tmpTextBox.Text);
					else 
						row[c] = DBNull.Value;
				}
				else if (c.DataType.Equals(typeof(System.DateTime))) 
					//support empty/null values for dates
				{
					if (tmpTextBox.Text == null || tmpTextBox.Text == string.Empty)
						row[c] = DBNull.Value;
					else
						row[c]= tmpTextBox.Text;
				}
				else
					row[c]= tmpTextBox.Text;
			}

			if (null != (tmpCheckBox = control as CheckBox))
				row[c]= tmpCheckBox.Checked;
				
			if (null != (tmpDateControl = control as WebControls.EditControls.DateControl)) 
			{
				if (!tmpDateControl.IsNull)
					row[c]= tmpDateControl.Value;
				else
					row[c] = System.DBNull.Value;
			}
			if (null != (tmpDateTimeControl = control as WebControls.EditControls.DateTimeControl)) 
			{
				if (!tmpDateTimeControl.IsNull)
					row[c]= tmpDateTimeControl.Value;
				else
					row[c] = System.DBNull.Value;
			}

			if (null != (tmpFileContent = control as WebControls.EditControls.FileContent)) 
			{
				//only update if actor has selected a new file
				if (!tmpFileContent.IsNull)
					row[c]= tmpFileContent.Value;
			}
			if (null != (tmpFileMimetype = control as WebControls.EditControls.FileMimetype)) 
			{
				//only update if actor has selected a new file
				if (!tmpFileMimetype.IsNull)
					row[c]= tmpFileMimetype.Value;
			}
					
			if (null != (tmpNumeric = control as WebControls.EditControls.Numeric))
				if (!tmpNumeric.IsNull)
					row[c]= tmpNumeric.Value;
				else
					row[c] = System.DBNull.Value;
				
			if (null != (tmpHazardMatrix = control as WebControls.EditControls.HazardMatrix.HazardMatrixControl))
				row[c]= tmpHazardMatrix.Text;
				
			if (null != (tmpYearMonthSpan = control as WebControls.EditControls.YearMonthSpan))
				row[c]= tmpYearMonthSpan.Value;
				
					
			if (null != (tmpDropDownList = control as DropDownList))
				if (null!=tmpDropDownList.SelectedItem )
					row[c] = tmpDropDownList.SelectedItem.Value;
					
			if (null != (tmpPersonnelField = control as PersonnelField))
			{
				if (tmpPersonnelField.IsForeignKeyConstraint) //Is this a foreign key personnel field?
				{
					row[c] = tmpPersonnelField.RowId;
				}
				else  //This is a "optional" foreign key (ex witness on incidentReport)
				{
					//String rowIdColumnName = c.ColumnName + "PersonnelRowId"; 
					//row[rowIdColumnName] = tmpPersonnelField.RowId;
					//TODO: Must make sure that there is a match between displayfield and rowid
					//TODO: Add check for related personnel. If related personnel exist, store personnelrowId	
					row[c] = tmpPersonnelField.DisplayValue;
				}
			}
					
			if (null != (tmpRelatedDataRecordField = control as RelatedDataRecordField))
				row[c] = tmpRelatedDataRecordField.RowId;
			
			return row;
		}


		//Provide a way to store the dataset in viewState
		public DataSet RecordsDataSet
		{
			get
			{
				return null==ViewState["RecordsDataSet"+this.ID] ? null : (DataSet) ViewState["RecordsDataSet"+this.ID];
				//return _recordDataSet;
			}
			set
			{
				ViewState["RecordsDataSet"+this.ID] = value;
				//_recordDataSet = value;
			}
		}

		//Provide a way to store the dataset in viewState
		public DataSet RecordsDataSetPersist
		{
			get
			{
				return null==ViewState["RecordsDataSetPersist"+this.ID] ? null : (DataSet) ViewState["RecordsDataSetPersist"+this.ID];
				//return _recordDataSet;
			}
			set
			{
				ViewState["RecordsDataSetPersist"+this.ID] = value;
				//_recordDataSet = value;
			}
		}

		public string SortColumn
		{
			get
			{
				return null==ViewState["SortColumn"+this.ID] ? "" : (string) ViewState["SortColumn"+this.ID];
			}
			set
			{
				ViewState["SortColumn"+this.ID] = value;
			}
		}

		public bool DisplayNewButton
		{
			get
			{
				return null==ViewState["DisplayNewButton"+this.ID] ? true : (bool) ViewState["DisplayNewButton"+this.ID];
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
				return null==ViewState["DisplaySelectButton"+this.ID] ? true : (bool) ViewState["DisplaySelectButton"+this.ID];
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
				return null==ViewState["DisplayEditButton"+this.ID] ? true : (bool) ViewState["DisplayEditButton"+this.ID];
			}
			set
			{
				ViewState["DisplayEditButton"+this.ID] = value;
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
		/// <summary>
		/// Store filter string in viewstate. Provides a way for setting the filter externally
		/// </summary>
		public String QueryFilter 
		{
			get
			{
				return null==ViewState["QueryFilter"+this.ID] ? string.Empty : (String) ViewState["QueryFilter"+this.ID];
			}
			set
			{
				ViewState["QueryFilter"+this.ID] = value;
			}
		}

	}
	
	[Serializable]
	public class GridControlData 
	{
		public System.Type ControlType;
		public String ControlId;
		public String GridColumnName;
	}

	
}
