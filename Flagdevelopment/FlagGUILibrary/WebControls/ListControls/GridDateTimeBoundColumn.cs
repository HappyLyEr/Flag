using System;
using System.Collections.Generic;
using System.Text;
using Telerik.WebControls;
using System.Web.UI.WebControls;
using System.Data;
namespace GASystem.WebControls.ListControls
{
    public class GridDateTimeBoundColumn : GridTemplateColumn
    {

        private GridDateTimeBoundColumn(){}

        public GridDateTimeBoundColumn(string columnName)
        {
            this.ItemTemplate = new GridDateItemTemplate("{0:g}", columnName);
            this.EditItemTemplate = new GridDateTimeItemEditorTemplate("{0}", columnName);
        }
    }



}
