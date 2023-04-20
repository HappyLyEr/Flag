using System;

namespace GASystem.GUIUtils.DataFilter
{
	/// <summary>
	/// Summary description for GeneralFilterElement.
	/// </summary>
	public class NumericFilterElement : GeneralFilterElement
	{
//		EditElements.NumericEditWebControl _editWebControl;
//		ViewElements.GeneralViewWebControl _viewWebControl;
		
//		public NumericFilterElement()
//		{
//			//
//			// TODO: Add constructor logic here
//			//
//			_editWebControl = new GASystem.GUIUtils.DataFilter.EditElements.NumericEditWebControl();
//			_viewWebControl = new GASystem.GUIUtils.DataFilter.ViewElements.GeneralViewWebControl();
//		}

		public NumericFilterElement(GASystem.AppUtils.FieldDescription fd)
		{
			
			_editWebControl = new GASystem.GUIUtils.DataFilter.EditElements.NumericEditWebControl(fd);
			_viewWebControl = new GASystem.GUIUtils.DataFilter.ViewElements.GeneralViewWebControl(fd);
			_editWebControl.FieldName = fd.FieldId;
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




	}
}
