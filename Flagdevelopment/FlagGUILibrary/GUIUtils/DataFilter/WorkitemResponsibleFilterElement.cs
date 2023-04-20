using System;

namespace GASystem.GUIUtils.DataFilter
{
	/// <summary>
	/// Summary description for GeneralFilterElement.
	/// </summary>
	public class WorkitemResponsibleFilterElement : GeneralFilterElement
	{

        public WorkitemResponsibleFilterElement(GASystem.AppUtils.FieldDescription fd)
		{
			
			_editWebControl = new GASystem.GUIUtils.DataFilter.EditElements.WorkitemResponsibleEditWebControl(fd);
			_viewWebControl = new GASystem.GUIUtils.DataFilter.ViewElements.GeneralViewWebControl(fd);
		
			
		}



        public override void GenerateFilterString()
        {
            if (_editWebControl.enabledElement == true)
            {

                _viewWebControl.enabledElement = true;
                _viewWebControl.GenerateFilterString(_editWebControl.FieldName, _editWebControl.GetFilterOperator(), _editWebControl.Condition);
                _viewWebControl.GenerateDispalyString(_editWebControl.FieldNameDisplay, _editWebControl.GetFilterOperator(), _editWebControl.ConditionDisplay);

                //_viewWebControl.FilterString = _editWebControl.FieldName + " = '" + _editWebControl.Condition + "'";
            }
            else
            {
                _viewWebControl.Reset();  //reset i case of deselection
            }
            _viewWebControl.SetSession(_editWebControl.enabledElement, _editWebControl.GetFilterOperator(), _editWebControl.ConditionDisplay);
        }

	}
}
