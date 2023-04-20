using System;

namespace GASystem.GUIUtils.DataFilter
{
	/// <summary>
	/// Summary description for GeneralFilterElement.
	/// </summary>
	public class GeneralFilterElement : IFilterElement
	{
		protected EditElements.IEditWebControl _editWebControl;
		protected ViewElements.IViewWebControl _viewWebControl;
//		EditElements.GeneralEditWebControl _editWebControl;
//		ViewElements.GeneralViewWebControl _viewWebControl;
////		
		public GeneralFilterElement() 
		{
			//
			// TODO: Add constructor logic here
			//
//			_editWebControl = new GASystem.GUIUtils.DataFilter.EditElements.GeneralEditWebControl();
//			_viewWebControl = new GASystem.GUIUtils.DataFilter.ViewElements.GeneralViewWebControl();
		}

		public GeneralFilterElement(GASystem.AppUtils.FieldDescription fd) : base(fd)
		{
			
			_editWebControl = new GASystem.GUIUtils.DataFilter.EditElements.GeneralEditWebControl(fd);
			_viewWebControl = new GASystem.GUIUtils.DataFilter.ViewElements.GeneralViewWebControl(fd);
			_editWebControl.FieldName = fd.TableId + "." + fd.FieldId;
		}
		
	
		
		
		public override GASystem.GUIUtils.DataFilter.EditElements.IEditWebControl EditWebControl
		{
			get
			{
				return _editWebControl;
			}			
		}

		public override GASystem.GUIUtils.DataFilter.ViewElements.IViewWebControl ViewWebControl
		{
			get
			{
				return _viewWebControl;
			}
		}		
        
        public override string GetFilterString()
		{
			if (_viewWebControl.enabledElement)
				return _viewWebControl.FilterString;
			return null;
		}




		public override void GenerateFilterString()
		{
			if (_editWebControl.enabledElement == true) 
			{
			
				_viewWebControl.enabledElement = true;
				_viewWebControl.GenerateFilterString(_editWebControl.FieldName, _editWebControl.GetFilterOperator(), _editWebControl.Condition);
				_viewWebControl.GenerateDispalyString(_editWebControl.FieldNameDisplay, _editWebControl.GetFilterOperator(), _editWebControl.ConditionText);
				
				//_viewWebControl.FilterString = _editWebControl.FieldName + " = '" + _editWebControl.Condition + "'";
			} 
			else 
			{
				_viewWebControl.Reset();  //reset i case of deselection
			}
            _viewWebControl.SetSession(_editWebControl.enabledElement, _editWebControl.GetFilterOperator(), _editWebControl.ConditionText);

		}




	}
}
