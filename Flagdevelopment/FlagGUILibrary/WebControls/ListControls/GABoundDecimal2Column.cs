using System;
using System.Collections.Generic;
using System.Text;
using Telerik.WebControls;

namespace FlagGUILibrary.WebControls.ListControls
{
    class GABoundDecimal2Column : GridBoundColumn
    {
        

        public override bool ReadOnly
        {
            get
            {
                return true;
            }
            set
            {
                base.ReadOnly = value;
            }
        }

        protected override string FormatDataValue(object dataValue, GridItem item)
        {
            int intValue = 0;
            try
            {
                intValue = int.Parse(dataValue.ToString());
            }
            catch (Exception ex)
            {
                return string.Empty;
            }

            return base.FormatDataValue(((double)intValue) / 100, item);
            
        }

        

    }
}