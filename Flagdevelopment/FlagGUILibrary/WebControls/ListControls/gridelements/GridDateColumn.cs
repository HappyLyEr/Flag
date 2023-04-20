using System;
using System.Collections.Generic;
using System.Text;
using Telerik.WebControls;
using System.Web.UI.WebControls;
using System.Data;

namespace GASystem.GAGUI.WebControls.ListControls.gridelements
{
    class GridDateColumn : GridEditableColumn 
    {
        protected override IGridColumnEditor CreateDefaultColumnEditor()
        {
            return new GridDateColumnEditor();
        }

        public override void FillValues(System.Collections.IDictionary newValues, GridEditableItem editableItem)
        {
            throw new Exception("The FillValues method or operation is not implemented.");
        }

        public override GridColumn Clone()
        {
            throw new Exception("The Clone  method or operation is not implemented.");
        }
    }
}
