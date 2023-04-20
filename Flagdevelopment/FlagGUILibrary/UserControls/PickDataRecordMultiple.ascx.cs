using GASystem.GAGUI.GUIUtils;
using GASystem.WebControls.ListControls;

namespace GASystem
{
	using System;
	using System.Text;
	using System.Collections;
	using System.ComponentModel;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.SessionState;
	using System.Web.UI;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using GASystem.BusinessLayer;
	using GASystem.AppUtils;
	using System.Reflection;
	using GASystem.DataModel;
	using GASystem.WebControls.CustomDataGrid;

	/// <summary>
	///		Summary description for PickDepartment.
	/// </summary>
	public class PickDataRecordMultiple : System.Web.UI.UserControl
	{
		
		protected DataGrid dg;
		protected System.Web.UI.WebControls.PlaceHolder GridPlaceHolder;
		

		protected Button btn;

		//Id's of parent window controls where we want to store the selected department
		protected string	ParentValueControlId;
		protected string ParentKeyControlId;
		
		protected string SelectedValue = "";
		protected string SelectedKeys = "";

		private DataSet _recordDataSet;
		private string _dataClass;
		protected System.Web.UI.WebControls.Button SelectButton;
		private string _displayColumnName;

		private void Page_Load(object sender, System.EventArgs e)
		{
			if (Request["ValueControlId"]!=null)
				ParentValueControlId = Request["ValueControlId"];
			if (Request["KeyControlId"]!=null)
				ParentKeyControlId = Request["KeyControlId"];

			GenerateDataGrid();
			RefreshGrid();
		}

		//Expose this method so that consumers may refresh grid
		/// <summary>
		/// Refresh datagrid. Remember to set RecordsDataSet and DataClass first.
		/// ViewState must be recreated when calling this method! (Do not call this method
		/// from the page_init event)
		/// </summary>
		public void RefreshGrid()
		{
			if (dg!=null)
			{
				DataView view = RecordsDataSet.Tables[DataClass].DefaultView;

				//if page is not a postback, set default sort
				if (!this.Page.IsPostBack) 
				{
					SortColumn = GASystem.BusinessLayer.FieldDefinition.GetSortOrderDefinitionForGADataClass(GADataRecord.ParseGADataClass(DataClass));

				}

				view.Sort = SortColumn;
				
				dg.DataSource = RecordsDataSet.Tables[DataClass].DefaultView;
				dg.DataBind();
			}
		}


		private void GenerateDataGrid()
		{
			if (RecordsDataSet==null) return;

			dg.AutoGenerateColumns = false;
			dg.EnableViewState= true;
			
			dg.AllowSorting = true;
			dg.DataKeyField = RecordsDataSet.Tables[DataClass].PrimaryKey[0].ColumnName;
			
			ArrayList myBoundColumns = new ArrayList();
			foreach(DataColumn c in RecordsDataSet.Tables[DataClass].Columns) 
			{
				MyBoundColumn tmpColumn = CreateMyBoundColumn(c);
				if (tmpColumn!=null && tmpColumn.FieldDesc.ShowInSmallList)
					myBoundColumns.Add(tmpColumn);
				     
			}
			myBoundColumns.Sort();
			
			RowSelectorColumn selectorColumn = new RowSelectorColumn();
			selectorColumn.HeaderText = "Select";
			myBoundColumns.Insert(0, selectorColumn);

			//myBoundColumns.Add(CreateButtonColumn(RecordsDataSet.Tables[DataClass].PrimaryKey[0].ColumnName, "Select"));
		
			foreach (DataGridColumn c in myBoundColumns)
				dg.Columns.Add(c); 


			GUIGenerateUtils.ApplyDatagridSkin(dg);

		}
		

		private MyBoundColumn CreateMyBoundColumn(DataColumn c)
		{
			FieldDescription fieldDesc = FieldDefintion.GetFieldDescription(c.ColumnName, c.Table.TableName);
			
			if (fieldDesc==null) return null;
			
			MyBoundColumn column = new MyBoundColumn();
			
			column.FieldDesc = fieldDesc;
			column.HeaderText = AppUtils.Localization.GetCaptionText(fieldDesc.DataType);

			//When grid is databound, we will replace content of lookupcolumn
//			if (fieldDesc.LookupTable!=null && fieldDesc.LookupTable.Length>0)
//			{
//				column.DataField = fieldDesc.LookupTableDisplayValue;
//				
//			}
//			else
//			{
				column.SortExpression = SortColumn.Equals(c.ColumnName) ? c.ColumnName + " DESC" : c.ColumnName;
				column.DataField = c.ColumnName;
				column.DataFormatString =setFormating(c);
//			}
			
			return column;
		}

	

		private ButtonColumn CreateButtonColumn(String primaryKey, String command)
		{
			//TemplateColumn tcolumn = new TemplateColumn();
			ButtonColumn bcolumn = new ButtonColumn();
			bcolumn.CommandName = command;
			
			
			bcolumn.Text = Localization.GetGuiElementText(command);
			bcolumn.ButtonType = ButtonColumnType.LinkButton;
			bcolumn.HeaderText = "";
			return bcolumn;
		}

//		private HyperLinkColumn CreateHyperLinkColumn(String primaryKey)
//		{
//			//TemplateColumn tcolumn = new TemplateColumn();
//			//ButtonColumn bcolumn = new ButtonColumn();
//			HyperLinkColumn hcolumn = new HyperLinkColumn();
//			hcolumn.Text = Localization.GetGuiElementText("Edit");
//			hcolumn.DataNavigateUrlField = primaryKey;
//			hcolumn.DataNavigateUrlFormatString = "EditRecord.aspx?DataClass=GAPersonnel&Id={0}";
//			return hcolumn;
//		}


//		private BoundColumn CreateBoundColumn(DataColumn c)
//		{
//			BoundColumn column = new BoundColumn();
//			
//			column.DataField = c.ColumnName;
//			column.HeaderText = AppUtils.Localization.GetCaptionText(c.ColumnName);
//			column.DataFormatString =setFormating(c);
//			return column;
//		}


		private	string setFormating(DataColumn bc)	   
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
					dataType="{0:dd.MM.yyyy}";			
					break;		  
				case "System.String":			 
					dataType="";		   
					break;		 
				default:			   
					dataType	= "";		  
					break;			 
			}		
			return dataType;	
		}


		

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			
			dg = new DataGrid();
			dg.EnableViewState = false;
			dg.ID = "dg1";
			GridPlaceHolder.Controls.Clear();
			GridPlaceHolder.Controls.Add(dg);
			dg.SelectedIndexChanged += new EventHandler(this.DataGrid_SelectedItemChanged);
			dg.SortCommand += new System.Web.UI.WebControls.DataGridSortCommandEventHandler(this.DataGrid_SortCommand);
			
			

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
			this.SelectButton.Click += new System.EventHandler(this.SelectButton_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		
	


		

		private void DataGrid_SortCommand(object source, System.Web.UI.WebControls.DataGridSortCommandEventArgs e)
		{
			SortColumn = e.SortExpression;
			RefreshGrid();
		}

		private void DataGrid_SelectedItemChanged(object sender, System.EventArgs e)
		{
			
			//SelectedKeys = dg.DataKeys[dg.SelectedIndex].ToString();
			//SelectRecord(SelectedKeys);
			
		}

		public void SelectRecord(string SelectedRowId) 
		{
			/*SelectedKey = SelectedRowId;
			DataRow row = RecordsDataSet.Tables[DataClass].Rows.Find(new object[] {SelectedKey});
			SelectedValue = "";
			ArrayList DisplayColumns = ParseDisplayColumns(DisplayColumnName);
			foreach (String columnName in DisplayColumns)
			{
				SelectedValue = SelectedValue + row[columnName].ToString() + " ";
			}
			SelectedValue.TrimEnd();
			//SelectedValue = row[DisplayColumnName].ToString();

			StringBuilder sb = new StringBuilder();
			sb.Append("<script language=javascript>");

			sb.Append("setParentWindowValues();");
			sb.Append("window.close();");
			sb.Append("</script>");
			Page.RegisterStartupScript("Setvalues", sb.ToString());
			dg.Visible = false;*/
		}

		private ArrayList ParseDisplayColumns(String ColumnName)
		{
			ArrayList columnList = new ArrayList();
			if (ColumnName.IndexOf(" ")>-1)
			{
				foreach (String columnName in ColumnName.Split(new char[] {' '}))
				{
					columnList.Add(columnName);
				}
			}
			else
			{
				columnList.Add(ColumnName);
			}
			return columnList;
		}

		public int[] GetSelectedIndexes() {
			if (dg==null)
				return new int[0];

			RowSelectorColumn rsc = dg.Columns[0] as RowSelectorColumn;
			return rsc.SelectedIndexes;
		}

		private void SelectButton_Click(object sender, System.EventArgs e)
		{
			foreach( Int32 selectedIndex in GetSelectedIndexes() ) 
			{
				//FW21052006:If grid is sorted, this is wrong:SelectedValue = SelectedValue + GetValueOfRowIndex(selectedIndex) + ", ";	
				SelectedKeys = SelectedKeys + dg.DataKeys[selectedIndex].ToString() +","; //no space for datakeys
				SelectedValue = SelectedValue + GetValueOfRowId((int)dg.DataKeys[selectedIndex]) + ", ";	
			}

			//remove last comma and space character
			SelectedValue = SelectedValue.TrimEnd(new char[] {' ', ','});
			SelectedKeys = SelectedKeys.TrimEnd(new char[] {' ', ','});

			StringBuilder sb = new StringBuilder();
			sb.Append("<script language=javascript>");

			sb.Append("setParentWindowValues();");
			sb.Append("window.close();");
			sb.Append("</script>");
			Page.RegisterStartupScript("Setvalues", sb.ToString());
		
		}

		//may not be used on sorted lists
		private String GetValueOfRowIndex(int RowIndex) 
		{
			String displayValue= "";
			DataRow row = RecordsDataSet.Tables[DataClass].Rows[RowIndex];
			
			ArrayList DisplayColumns = ParseDisplayColumns(DisplayColumnName);
			foreach (String columnName in DisplayColumns)
			{
				displayValue = displayValue + row[columnName].ToString() + " ";
			}
			displayValue.TrimEnd();
			return displayValue;
		}

		private String GetValueOfRowId(int RowId) 
		{
			String displayValue= "";
			DataRow row = null;//RecordsDataSet.Tables[DataClass].Rows[RowIndex];
			
			foreach (DataRow tmpRow in RecordsDataSet.Tables[DataClass].Rows) 
			{
				int tmpRowId = (int) tmpRow[DataClass.Substring(2) + "RowId"];
				if (tmpRowId==RowId) 
				{
					row=tmpRow;
					break;
				}
			}

			if (row==null) 
			{
				return "";
			}

			ArrayList DisplayColumns = ParseDisplayColumns(DisplayColumnName);
			foreach (String columnName in DisplayColumns)
			{
				displayValue = displayValue + row[columnName].ToString() + " ";
			}
			displayValue.TrimEnd();
			return displayValue;
		}
		
		

		//Provide a way to store the dataset in viewState
//		public DataSet RecordsDataSet
//		{
//			get
//			{
//				return null==ViewState["RecordsDataSet"] ? null : (DataSet) ViewState["RecordsDataSet"];
//			}
//			set
//			{
//				ViewState["RecordsDataSet"] = value;
//			}
//		}

		public DataSet RecordsDataSet
		{
			get
			{
				//return null==ViewState["RecordsDataSet"] ? null : (DataSet) ViewState["RecordsDataSet"];
				return _recordDataSet;
			}
			set
			{
				_recordDataSet = value;
			}
		}

		public string SortColumn
		{
			get
			{
				return null==ViewState["SortColumn"] ? "" : (string) ViewState["SortColumn"];
			}
			set
			{
				ViewState["SortColumn"] = value;
			}
		}



		//Provide a way to store the dataset in viewState
//		public String DataClass
//		{
//			get
//			{
//				return null==ViewState["DataClass"] ? null : (String) ViewState["DataClass"];
//			}
//			set
//			{
//				ViewState["DataClass"] = value;
//			}
//		}

		public String DataClass
		{
			get
			{
				return _dataClass;
			}
			set
			{
				_dataClass = value;
			}
		}

		//Provide a way to store the dataset in viewState
//		public String DisplayColumnName
//		{
//			get
//			{
//				return null==ViewState["DisplayColumnName"] ? null : (String) ViewState["DisplayColumnName"];
//			}
//			set
//			{
//				ViewState["DisplayColumnName"] = value;
//			}
//		}

		public String DisplayColumnName
		{
			get
			{
				return _displayColumnName;
			}
			set
			{
				_displayColumnName = value;
			}
		}

	}
}
