using System;
using System.Collections.Generic;
using System.Text;
using Telerik.WebControls;

namespace FlagGUILibrary.WebControls.ListControls
{
    class GridBoundIntToBoolColumn : GridBoundColumn
    {
        protected override string FormatDataValue(object dataValue, GridItem item)
        {
            string baseVlaue = base.FormatDataValue(dataValue, item);
            if (baseVlaue == null)
            {
                return "";
            }

            return baseVlaue == "1" ? "true" : "false";
        }
    }
}
