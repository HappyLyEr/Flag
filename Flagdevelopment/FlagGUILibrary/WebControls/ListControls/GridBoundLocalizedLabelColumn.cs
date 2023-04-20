using System;
using System.Collections.Generic;
using System.Text;
using Telerik.WebControls;

namespace FlagGUILibrary.WebControls.ListControls
{
    class GridBoundLocalizedLabelColumn : GridBoundColumn
    {
        private int characterLimit = 50;

        public int CharacterLimit
        {
            get { return characterLimit; }
            set { characterLimit = value; }
        }

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
            
            
            return GASystem.AppUtils.Localization.GetGuiElementText(base.FormatDataValue(dataValue, item));
        }

        

    }
}
