using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Web.UI.WebControls;
using GASystem.AppUtils;
using GASystem.GAGUI.GUIUtils;
using GASystem.GAGUIEvents;

namespace GASystem.WebControls.ListControls 
{
	/// <summary>
	/// Extension of the datagrid to support Flag columns from fielddefinitions
	/// </summary>
	public class FlagDataGrid : WebControl
	{
		public event GACommandEventHandler SortClicked;
		private DataSet _recordDataSet;
		private DataGrid _dg ;
		
		private ArrayList _columnsToDisplay = new ArrayList();
		
		public FlagDataGrid() : base()
		{
			
			
		}
		
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);
			DataBind();
			
		}

		
		
		protected override void OnPreRender(EventArgs e)
		{	
			base.OnPreRender (e);

			//rebind data in case sort has been changed
			PreRenderDataBind();
			
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

			//Color RowHighlightColor = Color.FromArgb(255, 79, 0);
            Color RowHighlightColor = Color.FromArgb(254, 234, 228);
			//			Color RowHighlightColor = Color.FromArgb(219, 216, 41);

			if (RowHighlightColor != Color.Empty && !Page.IsClientScriptBlockRegistered(SCRIPT_KEY))
				Page.RegisterClientScriptBlock(SCRIPT_KEY, String.Format(SCRIPT, RowHighlightColor.R, RowHighlightColor.G, RowHighlightColor.B));
		
		}

		
		public override void DataBind()
		{
			DataView view = RecordsDataSet.Tables[DataClass].DefaultView;
			view.Sort = SortColumn;
			_dg.DataSource = view;
			GenerateDataGrid();
			_dg.DataBind ();
		}

		public void PreRenderDataBind()
		{
			DataView view = RecordsDataSet.Tables[DataClass].DefaultView;
			view.Sort = SortColumn;
			_dg.DataSource = view;
			//GenerateDataGrid();
			_dg.DataBind ();
		}

		private void GenerateDataGrid()
		{
			if (RecordsDataSet==null || RecordsDataSet.Tables[DataClass]==null) return;

		
			_dg.Columns.Clear();
			_dg.AutoGenerateColumns = false;


			_dg.DataKeyField = RecordsDataSet.Tables[DataClass].PrimaryKey[0].ColumnName;

			GAListTemplateColumnFactory factory = new GAListTemplateColumnFactory();
			ArrayList myBoundColumns = new ArrayList();
			foreach(DataColumn c in RecordsDataSet.Tables[DataClass].Columns) 
			{
				GAListColumn tmpColumn = factory.getGAListColumn(c, false, setFormatting(c));
				if (tmpColumn!=null && DisplayColumn(tmpColumn) )
				{
					myBoundColumns.Add(tmpColumn);
				}
			}
			myBoundColumns.Sort();
			

			if (DisplayEditButton) myBoundColumns.Add(CreateHyperLinkColumn(RecordsDataSet.Tables[DataClass].PrimaryKey[0].ColumnName, true, "Edit"));
			if (DisplaySelectButton) myBoundColumns.Add(CreateHyperLinkColumn(RecordsDataSet.Tables[DataClass].PrimaryKey[0].ColumnName, false, "Select"));
			

  //commented out for testing          
//			foreach (DataGridColumn c in myBoundColumns)
//				_dg.Columns.Add(c); 

			GUIGenerateUtils.ApplyDatagridSkin(_dg);
		}

		protected bool DisplayColumn(GAListColumn Col) 
		{
			if (_columnsToDisplay.Count == 0)
				return !Col.getFieldDescription().HideInSummary;
			return _columnsToDisplay.Contains(Col.getFieldDescription().FieldId.ToLower());
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

		private HyperLinkColumn CreateHyperLinkColumn(String primaryKey, bool Edit, String captionId)
		{
			HyperLinkColumn hcolumn = new HyperLinkColumn();
			hcolumn.Text = Localization.GetGuiElementText(captionId);

			hcolumn.DataNavigateUrlField = primaryKey;
			if (Edit) 
				hcolumn.DataNavigateUrlFormatString = GASystem.GUIUtils.LinkUtils.GenerateURLForSingleRecordDetails(this.DataClass, "{0}");
			else
				hcolumn.DataNavigateUrlFormatString = GASystem.GUIUtils.LinkUtils.GenerateURLForSingleRecordView(this.DataClass, "{0}");
			return hcolumn;
		}


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


		private void DisableViewState(DataGrid dg)
		{
			foreach (DataGridItem dgi in dg.Items)
			{
				dgi.EnableViewState = false;
			}
		}
		
		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			base.OnInit(e);
			
			_dg = new DataGrid();
			_dg.EnableViewState = false;
			_dg.BorderWidth = Unit.Pixel(0);
			_dg.GridLines = GridLines.None;
			_dg.AllowSorting = true;
			_dg.AllowPaging = true;
			_dg.PageSize = 25;
			_dg.PagerStyle.Mode = PagerMode.NumericPages;
			_dg.PagerStyle.CssClass = "gridStyle_PagerStyle";
			_dg.PagerStyle.Position = PagerPosition.Bottom;
			
			
			this.EnableViewState = true;
			

			_dg.PageIndexChanged += new DataGridPageChangedEventHandler(dg_PageIndexChanged);
			_dg.SortCommand += new System.Web.UI.WebControls.DataGridSortCommandEventHandler(this.DataGrid_SortCommand);
			_dg.ItemDataBound += new DataGridItemEventHandler(this.DataGrid_ItemDataBound);	
			
			

			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
		
			this.Controls.Add(_dg);
		}
		
		/// <summary>
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
		
		}
		#endregion
	
		private void dg_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
		{
			_dg.CurrentPageIndex = e.NewPageIndex;
			CurrentPage = e.NewPageIndex;
		}

		
		private void DataGrid_ItemDataBound(object sender, 
			System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
//			// Only display if select button is visible
//			if (!DisplaySelectButton)
//				return;

			ListItemType itemType = e.Item.ItemType;
			if ((itemType == ListItemType.Pager) || 
				(itemType == ListItemType.Header) || 
				(itemType == ListItemType.Footer)) 
			{
				return;
			}

			if (e.Item.DataItem.GetType() != typeof(System.Data.DataRowView)) 
				return;

			System.Data.DataRowView rowView = (System.Data.DataRowView)e.Item.DataItem;
			
			DataGridItem o = e.Item;
			// Select hyper link button is always in last cell
			//HyperLink button = (HyperLink)o.Cells[o.Cells.Count-1].Controls[0];

			e.Item.Attributes["onmouseover"] = "javascript:GAScript_changeBackgroundColor(this, true)";
			e.Item.Attributes["onmouseout"] = "javascript:GAScript_changeBackgroundColor(this, false)";
			// the next line of code is horrible
			//e.Item.Attributes["onclick"] = "javascript:GAScript_goto('" + this.Page.Request.Url.Scheme + "://" + this.Page.Request.Url.Host + this.Page.Request.ApplicationPath + (this.Page.Request.ApplicationPath.EndsWith("/") ? "" : "/") + button.NavigateUrl.Remove(0,2) + "')";
			e.Item.Attributes["onclick"] = "javascript:GAScript_goto('" + this.Page.Request.Url.Scheme + "://" + this.Page.Request.Url.Host + this.Page.Request.ApplicationPath + (this.Page.Request.ApplicationPath.EndsWith("/") ? "" : "/") + GUIUtils.LinkUtils.GenerateSimpleURLForSingleRecordView(this.DataClass.ToString(), rowView.Row[this.DataClass.ToString().Substring(2) + "rowid"].ToString()) + "')";
		}

		private void DataGrid_SortCommand(object source, System.Web.UI.WebControls.DataGridSortCommandEventArgs e)
		{
			//the list control does no longer hold the recordset in viewstate. raising a event so that it is up to the 
			//parent of this control to reload  the recordset
			
			SortColumn = SortColumn.Equals(e.SortExpression) ? e.SortExpression + " DESC" : e.SortExpression;
			GACommandEventArgs GAEventArgs = new GACommandEventArgs();
			GAEventArgs.CommandStringArgument = SortColumn;
			GAEventArgs.CommandName = "Sort";
			if (SortClicked != null)
				SortClicked(this, GAEventArgs);
		}
		
		/// <summary>
		/// Get default sort from fielddefinition
		/// </summary>
		/// <returns></returns>
		private string getDefaultSort() 
		{
			DataModel.GADataClass sortDataClass;
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
		
		public DataSet RecordsDataSet
		{
			get
			{
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
				return null==ViewState["SortColumn"+ this.DataClass] ? getDefaultSort() : (string) ViewState["SortColumn"+ this.DataClass];
			}
			set
			{
				ViewState["SortColumn"+  this.DataClass] = value;
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
	
		//Provide a way to store the dataclass in viewState
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
				ViewState["CurrentPage"+this.ID] = value;
			}
		}

		
		
	}
}
