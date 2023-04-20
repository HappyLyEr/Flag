using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GASystem.WebControls.ListControls
{
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
        // Tor 201611 Security 20161122 (never referenced) private string _formattingString = "{0}";
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
			Telerik.WebControls.GridDataItem dgi = (Telerik.WebControls.GridDataItem)l.NamingContainer;
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
}