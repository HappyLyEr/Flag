using System;
using System.Web.UI.WebControls;
using GASystem.GUIUtils;
using System.Web.UI;
using System.Collections;
using GASystem.AppUtils;

namespace GASystem.GUIUtils.DataFilter.ViewElements
{
	/// <summary>
	/// Summary description for CheckboxViewWebcontrol.
	/// </summary>
    public class DatePeriodeViewWebcontrol : GeneralViewWebControl
	{
        FieldDescription _fd;



        public DatePeriodeViewWebcontrol(FieldDescription fd)
            : base(fd)
        {
            _fd = fd;
        }

        public override void GenerateFilterString()
        {
            FilterString = Condition;
        }

		public override void GenerateFilterString(string FieldName, FilterOperator Operator, string Condition)
		{

            FilterString = Condition;
			
		}

		public override void GenerateDispalyString(string FieldNameDisplay, FilterOperator Operator, string Condition)
		{
            string myCondition = string.Empty;
            
            try
            {
                string[] conditionDates = Condition.Split('t');

                DateTime conditionDateFrom = DateTime.Parse(conditionDates[0]);
                DateTime conditionDateTo = DateTime.Parse(conditionDates[1]);

                myCondition = conditionDateFrom.ToShortDateString() + " " + AppUtils.Localization.GetGuiElementText("To") + " " 
                    +conditionDateTo.ToShortDateString();

           
            }
            catch
            {
                //error parsing datetime

                myCondition = Condition.Replace("t", " " + AppUtils.Localization.GetGuiElementText("To") + " ");
            }

            base.GenerateDispalyString(FieldNameDisplay, Operator, myCondition);
			
		   
		}


	}
}
