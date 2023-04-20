using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GASystem.WebControls.ListControls
{
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
            Telerik.WebControls.GridDataItem dgi = (Telerik.WebControls.GridDataItem)l.NamingContainer;
			string val = ((DataRowView)dgi.DataItem)[_columnName].ToString();
			
			lbl.Text = val;
			
			img.AlternateText = val;
			img.ImageAlign = ImageAlign.Middle;

			switch (val)
			{
				case "LOW":
                    img.ImageUrl = "~/gagui/gagui/images/greenlight.gif";
					break;
				case "MEDIUM":
                    img.ImageUrl = "~/gagui/gagui/images/yellowlight.gif";
					break;
				case "HIGH":
                    img.ImageUrl = "~/gagui/gagui/images/redlight.gif";
					break;
			}

		}

	}
}