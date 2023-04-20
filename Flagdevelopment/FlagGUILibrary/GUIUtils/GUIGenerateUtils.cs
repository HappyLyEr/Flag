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
using GASystem.WebControls.ListControls;

namespace GASystem.GAGUI.GUIUtils
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
    /// Creates an galistcolumncontainer. Handles all initilazation and fielddescription checking. Returns null if 
    /// the column is not defined in fieldefinition.
    /// </summary>
    public class GAListColumnContainerFactory
    {

        public GAListColumnContainer getGAListColumnContainer(DataColumn c, Boolean readOnly, Boolean sortColumn, string formattingString, string baseURL)
        {
            GAListColumnContainer column = createGAListColumnContainer(c, readOnly);

            if (column != null)
            {
                column.init();
                column.setSortColumn(sortColumn);
                column.setFormattingString(formattingString);
                column.setBaseURL(baseURL);
            }
            return column;
        }

        protected virtual GAListColumnContainer createGAListColumnContainer(DataColumn c, bool readOnly)
        {
            FieldDescription fieldDesc = FieldDefintion.GetFieldDescription(c.ColumnName, c.Table.TableName);
            if (fieldDesc == null)
                return null;	//field not defined in fielddefinition. return null

            string controlType = fieldDesc.ControlType;

            GAListColumnContainer column = new GAListColumnContainer(c, readOnly);

            return column;
        }
    }


    /// <summary>
    /// Creates an galistcolumncontainer for use when exporting to excel. Handles all initilazation and fielddescription checking. Returns null if 
    /// the column is not defined in fieldefinition.
    /// </summary>
    public class GAListExcelColumnContainerFactory
    {

        public GAListExcelColumnContainer getGAListColumnContainer(DataColumn c, Boolean sortColumn, string formattingString)
        {
            GAListExcelColumnContainer column = createGAListColumnContainer(c);

            if (column != null)
            {
                column.init();
                column.setSortColumn(sortColumn);
                column.setFormattingString(formattingString);
            }
            return column;
        }

        protected virtual GAListExcelColumnContainer createGAListColumnContainer(DataColumn c)
        {
            FieldDescription fieldDesc = FieldDefintion.GetFieldDescription(c.ColumnName, c.Table.TableName);
            if (fieldDesc == null)
                return null;	//field not defined in fielddefinition. return null

            string controlType = fieldDesc.ControlType;

            GAListExcelColumnContainer column = new GAListExcelColumnContainer(c);

            return column;
        }
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
				case "DATARECORDIDENTIFIER":
					column.setItemTemplate(new GADataRecordIdentifierTemplate(c.ColumnName));
					break;
				case "WORKITEMRESPONSIBLE":
					column.setItemTemplate(new GAWorkitemResponsibleTemplate(c.ColumnName));
					break;
				default:
					column.setItemTemplate(new GAListItemTemplate(c.ColumnName));
					break;
			}

			return column;
		}
	}
}

