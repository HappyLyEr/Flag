using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GASystem.WebControls.ListControls
{
	/// <summary>
	/// Summary description for GAWorkitemResponsibleTemplate.
	/// </summary>
	public class GAWorkitemResponsibleTemplate : ITemplate
	{
		private Literal lbl;
		
		private string _columnName;
		
		public GAWorkitemResponsibleTemplate(string columnName)
		{
			_columnName = columnName;
		}
		
		#region ITemplate Members

		public void InstantiateIn(Control container)
		{
			// TODO:  Add GAWorkitemResponsibleTemplate.InstantiateIn implementation
			lbl = new Literal();
			lbl.DataBinding += new EventHandler(lbl_DataBinding);
			container.Controls.Add(lbl);
		}

		#endregion

		private void lbl_DataBinding(object sender, EventArgs e)
		{
			Literal l = (Literal)sender;
            Telerik.WebControls.GridDataItem dgi = (Telerik.WebControls.GridDataItem)l.NamingContainer;
			string val = ((DataRowView)dgi.DataItem)[_columnName].ToString();
		

			string participantIds = string.Empty;
			if (val.Length > 2) 
			{
				participantIds = val.Substring(1, val.Length - 2);
				participantIds = participantIds.Replace(";", ",");
			}
			DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetRecordSetAllDetailsForRowIds(GASystem.DataModel.GADataClass.GAPersonnel, participantIds);
			
			string resp = string.Empty;
			foreach (DataRow row in ds.Tables[0].Rows) 
			{
                resp += row["FamilyName"] + " " + row["GivenName"] + "</br>";
			}
			lbl.Text = resp;

		}
	}
}
