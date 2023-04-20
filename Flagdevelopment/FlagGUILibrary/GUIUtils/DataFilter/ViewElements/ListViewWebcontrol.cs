using System;
using GASystem.AppUtils;

namespace GASystem.GUIUtils.DataFilter.ViewElements
{
	/// <summary>
	/// Summary description for CheckboxViewWebcontrol.
	/// </summary>
	public class ListViewWebcontrol : GeneralViewWebControl
	{
        //private string _tablename;
        //private string _fieldname;

        public ListViewWebcontrol(FieldDescription fd)
            : base(fd)
		{
            //_tablename = fd.TableId;
            //_fieldname = fd.FieldId;
            //
			// TODO: Add constructor logic here
			//
		}


        /// <summary>
        /// Generate a human friendly display string based on conditon and operator
        /// </summary>
        public override void GenerateDispalyString(string FieldNameDisplay, FilterOperator Operator, string Condition)
        {
            string myCondition = Condition;
            try
            {
                int listrowid = Int32.Parse(myCondition);
                myCondition = GASystem.BusinessLayer.Lists.GetListDescriptionByRowId(listrowid);

            }
            catch (System.Exception ex)
            {

            }
            
            DisplayString = FieldNameDisplay + " " + GASystem.AppUtils.Localization.GetGuiElementText(Operator.ToString()) + " " + myCondition;


            

        }

        /// <summary>
        /// Geneerate sql filter string for this element
        /// </summary>
        public override void GenerateFilterString()
        {
            



            FilterString = FieldName + " " + ConditionOperator + " " + Condition;
        }

	}
}
