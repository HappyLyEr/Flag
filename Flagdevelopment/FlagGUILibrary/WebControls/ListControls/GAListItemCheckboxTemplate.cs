using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GASystem.WebControls.ListControls
{
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
            Telerik.WebControls.GridDataItem dgi = (Telerik.WebControls.GridDataItem)l.NamingContainer;
			string val = ((DataRowView)dgi.DataItem)[_columnName].ToString();

			l.Checked = val.ToUpper().Equals("TRUE");
			l.Enabled = _editable;
		}

	}
}