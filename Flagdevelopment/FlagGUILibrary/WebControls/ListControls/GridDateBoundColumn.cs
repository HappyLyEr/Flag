using System;
using System.Collections.Generic;
using System.Text;
using Telerik.WebControls;
using System.Web.UI.WebControls;
using System.Data;
namespace GASystem.WebControls.ListControls
{
    public class GridDateBoundColumn : GridTemplateColumn
    {

        private GridDateBoundColumn(){}

        public GridDateBoundColumn(string columnName)
        {
            this.ItemTemplate = new GridDateItemTemplate("{0:d}", columnName);
            this.EditItemTemplate = new GridDateItemEditorTemplate("{0}", columnName);
           
        }

      
	
     
    }

}
