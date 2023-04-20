using System;
using System.Collections.Generic;
using System.Text;
using Telerik.WebControls;

namespace FlagGUILibrary.WebControls.ListControls
{
    class GridBoundLimitColumn : GridBoundColumn
    {
        private int characterLimit = 75;

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
            return Truncate(base.FormatDataValue(dataValue, item));
        }

        string Truncate(string input)
        {
            string output = input;

            // Check if the string is longer than the allowed amount
            // otherwise do nothing
            if (output.Length > characterLimit && characterLimit > 0)
            {

                // cut the string down to the maximum number of characters
                output = output.Substring(0, characterLimit);

                // Check if the space right after the truncate point 
                // was a space. if not, we are in the middle of a word and 
                // need to cut out the rest of it
                if (input.Substring(output.Length, 1) != " ")
                {
                    int LastSpace = output.LastIndexOf(" ");

                    // if we found a space then, cut back to that space
                    if (LastSpace != -1)
                    {
                        output = output.Substring(0, LastSpace);
                    }
                }
                // Finally, add the "..."
                output += "...";
            }
            return output;
        }

    }
}
