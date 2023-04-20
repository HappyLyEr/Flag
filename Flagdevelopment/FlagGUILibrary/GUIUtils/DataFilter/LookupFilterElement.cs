using System;
using GASystem.AppUtils;

namespace GASystem.GUIUtils.DataFilter
{
	/// <summary>
	/// Summary description for GeneralFilterElement.
	/// </summary>
	public class LookupFilterElement : GeneralFilterElement
	{
        FieldDescription _fd;

        public LookupFilterElement(GASystem.AppUtils.FieldDescription fd)
		{
			
			_editWebControl = new GASystem.GUIUtils.DataFilter.EditElements.LookupEditWebControl(fd);
			_viewWebControl = new GASystem.GUIUtils.DataFilter.ViewElements.GeneralViewWebControl(fd);
            _fd = fd;    
          
			
		}


        public override void GenerateFilterString()
        {
            if (_editWebControl.enabledElement == true)
            {

                _viewWebControl.enabledElement = true;
                _viewWebControl.GenerateFilterString(GenerateCombinedColumns(_fd), _editWebControl.GetFilterOperator(), _editWebControl.Condition);
                _viewWebControl.GenerateDispalyString(_editWebControl.FieldNameDisplay, _editWebControl.GetFilterOperator(), _editWebControl.Condition);

                //_viewWebControl.FilterString = _editWebControl.FieldName + " = '" + _editWebControl.Condition + "'";
            }
            else
            {
                _viewWebControl.Reset();  //reset i case of deselection
            }
            _viewWebControl.SetSession(_editWebControl.enabledElement, _editWebControl.GetFilterOperator(), _editWebControl.ConditionText);

        }

        /// <param name="FieldDescriptionInfo"></param>
        /// <param name="CurrentTableAlias"></param>
        /// <returns></returns>
        private string GenerateCombinedColumns(GASystem.AppUtils.FieldDescription FieldDescriptionInfo)
        {

            string columnSelect = "''";
            foreach (String columnName in FieldDescriptionInfo.GetLookupTableDisplayColumns())
            {
                GASystem.AppUtils.FieldDescription fd = GASystem.AppUtils.FieldDefintion.GetFieldDescription(columnName, FieldDescriptionInfo.LookupTable);
                if (fd.LookupTable == string.Empty)
                    columnSelect += " + " + FieldDescriptionInfo.LookupTable + FieldDescriptionInfo.ColumnOrder + '.' + columnName + " + ' ' ";
                else
                {
                    columnSelect += " + " + GenerateCombinedColumns(fd);
                }
            }

            return columnSelect;
        }



	}
}
