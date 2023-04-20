using GASystem.AppUtils;
using GASystem.GUIUtils.DataFilter.EditElements;
using GASystem.GUIUtils.DataFilter.ViewElements;

namespace GASystem.GUIUtils.DataFilter
{
	/// <summary>
	/// Summary description for GeneralFilterElement.
	/// </summary>
	public class DateFilterElement : GeneralFilterElement
	{
//		EditElements.DateEditWebControl _editWebControl;
//		ViewElements.GeneralViewWebControl _viewWebControl;
		
//		public DateFilterElement()
//		{
//			//
//			// TODO: Add constructor logic here
//			//
//		}

		public DateFilterElement(FieldDescription fd)
		{
			
			_editWebControl = new DateEditWebControl(fd);
			_viewWebControl = new DateViewWebcontrol(fd);
	
			
		}
//		public override GASystem.GUIUtils.DataFilter.EditElements.IEditWebControl EditWebControl
//		{
//			get
//			{
//				return _editWebControl;
//			}
//			
//		}
//
//		public override GASystem.GUIUtils.DataFilter.ViewElements.IViewWebControl ViewWebControl
//		{
//			get
//			{
//				return _viewWebControl;
//			}
//		}
//
//		public override string GetFilterString()
//		{
//			if (_viewWebControl.enabledElement)
//				return _viewWebControl.FilterString;
//			return null;
//		}
//
//		public override void GenerateFilterString()
//		{
//			if (_editWebControl.enabledElement == true) 
//			{
//			
//				_viewWebControl.enabledElement = true;
//				_viewWebControl.GenerateFilterString(_editWebControl.FieldName, _editWebControl.ConditionOperator, _editWebControl.Condition);
//				
//				//_viewWebControl.FilterString = _editWebControl.FieldName + " = '" + _editWebControl.Condition + "'";
//			} 
//			else 
//			{
//				_viewWebControl.Reset();  //reset i case of deselection
//			}
//		}
//
        public override void GenerateFilterString()
        {
            if (_editWebControl.enabledElement == true)
            {

                _viewWebControl.enabledElement = true;
                _viewWebControl.GenerateFilterString(_editWebControl.FieldName, _editWebControl.GetFilterOperator(), _editWebControl.Condition);
                _viewWebControl.GenerateDispalyString(_editWebControl.FieldNameDisplay, _editWebControl.GetFilterOperator(), _editWebControl.ConditionText);

            }
            else
            {
                _viewWebControl.Reset();  //reset i case of deselection
            }
            _viewWebControl.SetSession(_editWebControl.enabledElement, _editWebControl.GetFilterOperator(), _editWebControl.ConditionText);

        }


	}
}
