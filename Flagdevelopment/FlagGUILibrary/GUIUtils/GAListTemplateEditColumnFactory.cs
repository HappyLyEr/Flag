using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using GASystem.BusinessLayer;
using GASystem.AppUtils;
using System.Data;
using GASystem.GAGUI.GUIUtils;
using GASystem.WebControls.ListControls;

namespace GASystem.GUIUtils
{
	/// <summary>
	/// Creates editable<code>TemplateColumn</code>s, and applies edit templates based on <code>columnType</code>.
	/// Use this factory for creating datagrid columns that accept userinput (textboxes, checkboxes, etc..)
	/// </summary>
	public class GAListTemplateEditColumnFactory : GAListColumnFactory
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
				/*case "CHECKBOX":
					column.setItemTemplate(new GAListItemCheckboxTemplate(c.ColumnName, false));
					break;
				case "HAZARDMATRIX":
					column.setItemTemplate(new GAHazardMatrixListItemTemplate(c.ColumnName));
					break;*/
				default:
					column.setItemTemplate(new GAListItemEditTemplate(c.ColumnName, fieldDesc));
					break;
			}

			return column;
		}
	}
}
