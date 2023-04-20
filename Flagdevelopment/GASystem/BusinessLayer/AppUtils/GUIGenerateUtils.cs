using System;
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

namespace GASystem.WebControls.ListControls
{
	/// <summary>
	/// Summary description for GUIGenerateUtils.
	/// </summary>
	public class GUIGenerateUtils
	{
		public GUIGenerateUtils()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static void ApplyDatagridSkin(DataGrid dg)
		{
			dg.CellPadding = 3;
			dg.CssClass = "gridStyle";
			dg.HeaderStyle.CssClass = "gridStyle_HeaderStyle";
			dg.AlternatingItemStyle.CssClass = "gridStyle_AlernatingItem";
			dg.ItemStyle.CssClass = "gridStyle_ItemStyle";
			dg.FooterStyle.CssClass = "gridStyle_FooterStyle";
			dg.PagerStyle.CssClass = "gridStyle_PagerStyle";
		}
	}

	public class MyBoundColumn : BoundColumn, IComparable
	{
		private FieldDescription _fieldDefinition;


		public System.Int32 CompareTo ( System.Object obj )
		{
			if ( obj == null) return 1;
			if ( !(obj is MyBoundColumn) )
				throw new ArgumentException(); 
			
			MyBoundColumn boundColumn = (MyBoundColumn) obj;
			return this.FieldDesc.ColumnOrder.CompareTo(boundColumn.FieldDesc.ColumnOrder);
		}


		public FieldDescription FieldDesc
		{
			get
			{
				return _fieldDefinition;
			}
			set
			{
				_fieldDefinition = value;
			}
		}
	}

	/// <summary>
	/// Default template for displaying a value in a DataGrid.
	/// </summary>
	/// <remarks>
	/// TODO: Apply formatting string.
	/// </remarks>
	public class GAListItemTemplate : ITemplate
	{
		private Literal lbl;
		private string _columnName;
        //private string _formattingString = "{0}";
		private static int MAX_COLUMN_LENGTH = 50;
		private static int MAX_COLUMN_LENGTH_NO_SPACES = 15;

		public GAListItemTemplate(string columnName)
		{
			_columnName = columnName;
		}

		#region ITemplate Members

		public void InstantiateIn(Control container)
		{
			lbl = new Literal();
			lbl.DataBinding += new EventHandler(lbl_DataBinding);
			container.Controls.Add(lbl);
		}

		#endregion

		protected void lbl_DataBinding(object sender, EventArgs e)
		{
			Literal l = (Literal)sender;
			DataGridItem dgi = (DataGridItem)l.NamingContainer;
			string val = ((DataRowView)dgi.DataItem)[_columnName].ToString();
			if (val.Length > MAX_COLUMN_LENGTH)
			{
				if (val.IndexOf(" ") == -1)
				{
					lbl.Text = val.Substring(0, MAX_COLUMN_LENGTH_NO_SPACES-1) + "...";
				} 
				else 
				{
					lbl.Text = val.Substring(0, MAX_COLUMN_LENGTH-1) + "...";
				}
			} 
			else 
			{
				lbl.Text = val;
			}
		}

	}

	public class GAListDateTemplate : ITemplate
	{
		private Literal lbl;
		private string _columnName;
		private string _formattingString = "{0:d}";


		public GAListDateTemplate(string columnName)
		{
			_columnName = columnName;
		}

		#region ITemplate Members

		public void InstantiateIn(Control container)
		{
			lbl = new Literal();
			lbl.DataBinding += new EventHandler(lbl_DataBinding);
			container.Controls.Add(lbl);
		}

		#endregion

		protected void lbl_DataBinding(object sender, EventArgs e)
		{
			Literal l = (Literal)sender;
			DataGridItem dgi = (DataGridItem)l.NamingContainer;
			
			try 
			{
				if (((DataRowView)dgi.DataItem)[_columnName] != DBNull.Value) 
				{
					DateTime val = (DateTime)((DataRowView)dgi.DataItem)[_columnName];
					lbl.Text = string.Format(_formattingString, val);
				} 
				else 
				{
					lbl.Text = string.Empty;
				}
				
			} 
			catch 
			{
				lbl.Text = string.Empty;
			}
			
		}

	}

	/// <summary>
	/// Displays a value as a checkbox
	/// </summary>
	public class GAListItemCheckboxTemplate : ITemplate
	{
		private CheckBox lbl;
		private string _columnName;
		private bool _editable;

		public GAListItemCheckboxTemplate(string columnName, bool editable)
		{
			_columnName = columnName;
			_editable = editable;
		}

		#region ITemplate Members

		public void InstantiateIn(Control container)
		{
			lbl = new CheckBox();
			lbl.DataBinding += new EventHandler(lbl_DataBinding);
			container.Controls.Add(lbl);
		}

		#endregion

		protected void lbl_DataBinding(object sender, EventArgs e)
		{
			CheckBox l = (CheckBox)sender;
			DataGridItem dgi = (DataGridItem)l.NamingContainer;
			string val = ((DataRowView)dgi.DataItem)[_columnName].ToString();

			l.Checked = val.ToUpper().Equals("TRUE");
			l.Enabled = _editable;
		}

	}

	/// <summary>
	/// Displays a hazard risk value as a "traffic light"
	/// </summary>
	public class GAHazardMatrixListItemTemplate : ITemplate
	{
		Label lbl;
		System.Web.UI.WebControls.Image img;

		private string _columnName;

		public GAHazardMatrixListItemTemplate(string columnName)
		{
			_columnName = columnName;
		}

		#region ITemplate Members

		public void InstantiateIn(Control container)
		{
			lbl = new Label();
			img = new System.Web.UI.WebControls.Image();
			lbl.DataBinding += new EventHandler(lbl_DataBinding);
			
			container.Controls.Add(img);
			container.Controls.Add(lbl);
		}

		#endregion

		protected void lbl_DataBinding(object sender, EventArgs e)
		{
			Label l = (Label)sender;
			DataGridItem dgi = (DataGridItem)l.NamingContainer;
			string val = ((DataRowView)dgi.DataItem)[_columnName].ToString();
			
			lbl.Text = val;
			
			img.AlternateText = val;
			img.ImageAlign = ImageAlign.Middle;

			switch (val)
			{
				case "LOW":
					img.ImageUrl = "~/images/greenlight.gif";
					break;
				case "MEDIUM":
					img.ImageUrl = "~/images/yellowlight.gif";
					break;
				case "HIGH":
					img.ImageUrl = "~/images/redlight.gif";
					break;
			}

		}

	}

	/// <summary>
	/// Base class for Columns used in GA
	/// </summary>
	public class GAListColumn : TemplateColumn, IComparable
	{
		protected FieldDescription _fieldDescription;
		protected DataColumn _c;
		protected Boolean _sortColumn;
		protected string _formattingString;

		public GAListColumn(DataColumn c)
		{
			_c = c;
		}

		public void setSortColumn(Boolean sortColumn)
		{
			_sortColumn = sortColumn;
		}

		public void setFormattingString(string formattingString)
		{
			_formattingString = formattingString;
				
		}

		public FieldDescription getFieldDescription()
		{
			return _fieldDescription;
		}



		/// <summary>
		/// Init
		/// Setting sort expression for column
		/// </summary>
		public void init()
		{
			_fieldDescription = FieldDefintion.GetFieldDescription(_c.ColumnName, _c.Table.TableName);
			this.HeaderText = AppUtils.Localization.GetCaptionText(_c.ColumnName);

			if (_fieldDescription.LookupTable!=null && _fieldDescription.LookupTable.Length>0)
			{	
				//				this.DataField = _fieldDescription.LookupTableDisplayValue;
				this.SortExpression = (_sortColumn) ? _c.ColumnName + " DESC" : _c.ColumnName;
			}
			else
			{
				this.SortExpression = (_sortColumn) ? _c.ColumnName + " DESC" : _c.ColumnName;
			}
		}

		public System.Int32 CompareTo ( System.Object obj )
		{
			if (obj == null) return 1;
			if (!(obj is GAListColumn))
				throw new ArgumentException(); 
				
			GAListColumn column = (GAListColumn) obj;
			return this.getFieldDescription().ColumnOrder.CompareTo(column.getFieldDescription().ColumnOrder);
		}

		public void setItemTemplate(ITemplate template)
		{
			this.ItemTemplate = template;
		}
	}

	/// <summary>
	/// Uses ControlType to determine what type of column to create for ListDataRecords
	/// </summary>
	public abstract class GAListColumnFactory
	{
		public GAListColumn getGAListColumn(DataColumn c, Boolean sortColumn, string formattingString)
		{
			GAListColumn column = createGAListColumn(c);

			if (column != null) 
			{
				column.init();
				column.setSortColumn(sortColumn);
				column.setFormattingString(formattingString);
			}
			return column;
		}

		protected abstract GAListColumn createGAListColumn(DataColumn c);
	}

	/// <summary>
	/// Creates <code>TemplateColumn</code>s, and applies templates based on <code>columnType</code>.
	/// </summary>
	public class GAListTemplateColumnFactory : GAListColumnFactory
	{
		protected override GAListColumn createGAListColumn(DataColumn c)
		{
			FieldDescription fieldDesc = FieldDefintion.GetFieldDescription(c.ColumnName, c.Table.TableName);
			if (fieldDesc == null)
				return null;	//field not defined in fielddefinition. return null

			string controlType = fieldDesc.ControlType;

			GAListColumn column = new GAListColumn(c);

			switch (controlType.ToUpper())
			{
				case "CHECKBOX":
					column.setItemTemplate(new GAListItemCheckboxTemplate(c.ColumnName, false));
					break;
				case "HAZARDMATRIX":
					column.setItemTemplate(new GAHazardMatrixListItemTemplate(c.ColumnName));
					break;
				case "DATE":
					column.setItemTemplate(new GAListDateTemplate(c.ColumnName));
					break;
				default:
					column.setItemTemplate(new GAListItemTemplate(c.ColumnName));
					break;
			}

			return column;
		}
	}
}

