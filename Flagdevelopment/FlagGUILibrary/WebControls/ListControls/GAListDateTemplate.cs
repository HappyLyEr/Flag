using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GASystem.WebControls.ListControls
{
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
            Telerik.WebControls.GridDataItem dgi = (Telerik.WebControls.GridDataItem)l.NamingContainer;
			
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
}