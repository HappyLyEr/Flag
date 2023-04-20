using System;
using System.Collections.Generic;
using System.Text;
using Telerik.WebControls;
using System.Web.UI.WebControls;
using System.Data;
namespace GASystem.WebControls.ListControls
{
    public class GridDateItemTemplate : System.Web.UI.ITemplate
    {
        Label datoDisplay;
        string _formatString;
        string _columnName;

        public GridDateItemTemplate(string formatString, string columnName) 
        {
            _formatString = formatString;
            _columnName = columnName;
        }

        #region ITemplate Members
        public void InstantiateIn(System.Web.UI.Control container)
        {
            datoDisplay = new Label();
            
            //a.Text = "dato";
            datoDisplay.DataBinding += new EventHandler(a_DataBinding);
            container.Controls.Add(datoDisplay);
        }
        #endregion

        void a_DataBinding(object sender, EventArgs e)
        {
            Label l = (Label)sender;
			Telerik.WebControls.GridDataItem dgi = (Telerik.WebControls.GridDataItem)l.NamingContainer;
            string val = "&nbsp;"; // string.Empty;
           
            if (((DataRowView)dgi.DataItem)[_columnName] != DBNull.Value)
                val = string.Format(_formatString, ((DataRowView)dgi.DataItem)[_columnName]);
			datoDisplay.Text = val;
        }
    }
}
