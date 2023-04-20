using System;
using GASystem.AppUtils;

namespace GASystem.GUIUtils.DataFilter.ViewElements
{
	/// <summary>
	/// Summary description for CheckboxViewWebcontrol.
	/// </summary>
	public class DateViewWebcontrol : GeneralViewWebControl
	{
        public DateViewWebcontrol(FieldDescription fd)
            : base(fd)
		{
			//
			// TODO: Add constructor logic here
			//
		}

        public override void GenerateFilterString()
        {
            FilterString = FieldName + " " + ConditionOperator + " '" + Condition + "'";
        }

		public override void GenerateFilterString(string FieldName, FilterOperator Operator, string Condition)
		{
			if (Operator != FilterOperator.isCurrent && Operator != FilterOperator.Between)
				base.GenerateFilterString(FieldName, Operator, Condition);
            else if (Operator == FilterOperator.Between)
            {
                FilterString = FieldName + " " + getSqlOperator(Operator) + " " + Condition;
            }
            else
			{
//				if (Operator == FilterOperator.isNotChecked)
//					FilterString = "(" + FilterString + " or " + FieldName + " is null)";
//				
				DateTime today = DateTime.Now;
				string todayString = "'" + today.Year + "-" + today.Month + "-" + today.Day +"'";


				String filterBase = " ({0} > {1} or {0} is null ) ";
				FilterString = string.Format(filterBase, FieldName, todayString);
			}
		}

        //public override void GenerateDispalyString(string FieldNameDisplay, FilterOperator Operator, string Condition)
        //{
        //    if (Operator != FilterOperator.isCurrent)
        //        base.GenerateDispalyString(FieldNameDisplay, Operator, Condition);
        //    else
        //        DisplayString = FieldNameDisplay + " " + GASystem.AppUtils.Localization.GetGuiElementText(Operator.ToString());
        //}


        public override void GenerateDispalyString(string FieldNameDisplay, FilterOperator Operator, string Condition)
        {
            string myCondition = string.Empty;

            try
                {


                if (Operator != FilterOperator.isCurrent && Operator != FilterOperator.Between)
                    base.GenerateDispalyString(FieldNameDisplay, Operator, Condition);
                else if (Operator == FilterOperator.Between)
                {
                    string[] conditionDates = Condition.Split('t');

                    DateTime conditionDateFrom = DateTime.Parse(conditionDates[0]);
                    DateTime conditionDateTo = DateTime.Parse(conditionDates[1]);

                    myCondition = conditionDateFrom.ToShortDateString() + " " + AppUtils.Localization.GetGuiElementText("and") + " "
                        + conditionDateTo.ToShortDateString();


                    base.GenerateDispalyString(FieldNameDisplay, Operator, myCondition);
                }
                else
                {
                    DisplayString = FieldNameDisplay + " " + GASystem.AppUtils.Localization.GetGuiElementText(Operator.ToString());
                }


            }
            catch
            {
                //error parsing datetime

                myCondition = Condition.Replace("t", " " + AppUtils.Localization.GetGuiElementText("To") + " ");
                base.GenerateDispalyString(FieldNameDisplay, Operator, myCondition);
            }

            this.Page.Session[this.TableName  + "-" + this.FdFieldName + "-filteroperator"] = Operator;


            this.Page.Session[this.TableName + "-" + this.FdFieldName + "-filtercondition"] = Condition;
           

          


        }


	}
}
