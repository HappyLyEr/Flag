using System;
using GASystem.AppUtils;

namespace GASystem.GUIUtils.DataFilter.ViewElements
{
	/// <summary>
	/// Summary description for CheckboxViewWebcontrol.
	/// </summary>
	public class SelectListViewWebcontrol : GeneralViewWebControl
	{
        //private string _tablename;
        //private string _fieldname;

        private string filterTemplate = @" {0} in (select s.ownerclassrowid  from gasuperclass s 
inner join galistsselected ls on s.memberclassrowid =  ls.listsselectedrowid and s.memberclass='galistsselected'
where ls.listsrowid = {1} and s.ownerclass = '{2}')";

        private FieldDescription fieldDesc;

        public SelectListViewWebcontrol(FieldDescription fd)
            : base(fd)
		{
            fieldDesc = fd;
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
           FilterString = string.Format(filterTemplate, fieldDesc.TableId.Substring(2) + "rowid", Condition, fieldDesc.TableId);
        }

	}
}
