using System;
using System.Collections.Generic;
using System.Text;
using Telerik.WebControls;

namespace FlagGUILibrary.WebControls.ListControls
{
    class GABoundMinutesColumn : GridBoundColumn
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

            int Hours = 0;
            Hours = intValue / 60;
            int minutes = intValue - (Hours * 60);

            return Hours.ToString() + ":" + (minutes < 10 ? "0" : "") + minutes.ToString();
            

        }



    }
}