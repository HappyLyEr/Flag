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

	/// <summary>
	///		Summary description for PickDepartment.
	/// </summary>
	public class PickDepartment : System.Web.UI.UserControl
	{
		
		protected DataGrid dg;
		protected System.Web.UI.WebControls.PlaceHolder GridPlaceHolder;
		

		protected Button btn;

		//Id's of parent window controls where we want to store the selected department
		protected string	ParentValueControlId;
		protected string ParentKeyControlId;
		
		protected string SelectedValue = "";
		protected string SelectedKey = "";

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
				if (tmpColumn!=null && !tmpColumn.FieldDesc.HideInSummary)
					myBoundColumns.Add(tmpColumn);
				     
			}
			myBoundColumns.Sort();
			
			
			myBoundColumns.Add(CreateButtonColumn(RecordsDataSet.Tables[DataClass].PrimaryKey[0].ColumnName, "Select"));
		
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
			if (fieldDesc.LookupTable!=null && fieldDesc.LookupTable.Length>0)
			{
				column.DataField = fieldDesc.LookupTableDisplayValue;
				
			}
			else
			{
				column.SortExpression = SortColumn.Equals(c.ColumnName) ? c.ColumnName + " DESC" : c.ColumnName;
				column.DataField = c.ColumnName;
				column.DataFormatString =setFormating(c);
			}
			
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

		private HyperLinkColumn CreateHyperLinkColumn(String primaryKey)
		{
			//TemplateColumn tcolumn = new TemplateColumn();
			//ButtonColumn bcolumn = new ButtonColumn();
			HyperLinkColumn hcolumn = new HyperLinkColumn();
			hcolumn.Text = Localization.GetGuiElementText("Edit");
			hcolumn.DataNavigateUrlField = primaryKey;
			hcolumn.DataNavigateUrlFormatString = "EditRecord.aspx?DataClass=GAPersonnel&Id={0}";
			return hcolumn;
		}


//		private BoundColumn CreateBoundColumn(DataColumn c)
//		{
//			BoundColumn column = new BoundColumn();
//			column.DataField = c.ColumnName;
//			column.HeaderText = AppUtils.Localization.GetCaptionText(c.ColumnName);
//			column.DataFormatString =setFormating(c);
//			return column;
//		}
//

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
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		
	


		

		private void DataGrid_SortCommand(object source, System.Web.UI.WebControls.DataGridSortCommandEventArgs e)
		{
			SortColumn = e.SortExpression;
		}

		private void DataGrid_SelectedItemChanged(object sender, System.EventArgs e)
		{
			
			SelectedKey = dg.DataKeys[dg.SelectedIndex].ToString();
			DataRow row = RecordsDataSet.Tables[DataClass].Rows.Find(new object[] {SelectedKey});
			SelectedValue = row[DisplayColumnName].ToString();

			StringBuilder sb = new StringBuilder();
			sb.Append("<script language=javascript>");

			sb.Append("setParentWindowValues();");
			sb.Append("window.close();");
			sb.Append("</script>");
			Page.RegisterStartupScript("Setvalues", sb.ToString());
			
		}
		
		

		//Provide a way to store the dataset in viewState
		public DataSet RecordsDataSet
		{
			get
			{
				return null==ViewState["RecordsDataSet"] ? null : (DataSet) ViewState["RecordsDataSet"];
			}
			set
			{
				ViewState["RecordsDataSet"] = value;
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
		public String DataClass
		{
			get
			{
				return null==ViewState["DataClass"] ? null : (String) ViewState["DataClass"];
			}
			set
			{
				ViewState["DataClass"] = value;
			}
		}

		//Provide a way to store the dataset in viewState
		public String DisplayColumnName
		{
			get
			{
				return null==ViewState["DisplayColumnName"] ? null : (String) ViewState["DisplayColumnName"];
			}
			set
			{
				ViewState["DisplayColumnName"] = value;
			}
		}


	}
}
