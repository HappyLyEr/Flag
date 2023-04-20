using System;

namespace GASystem.GUIUtils.DataFilter
{
	/// <summary>
	/// Summary description for GeneralFilterElement.
	/// </summary>
	public class CheckboxFilterElement : GeneralFilterElement
	{
//		EditElements.CheckboxEditWebControl _editWebControl;
//		ViewElements.GeneralViewWebControl _viewWebControl;
		
//		private CheckboxFilterElement()
//		{
//			//
//			// TODO: Add constructor logic here
//			//
////			_editWebControl = new GASystem.GUIUtils.DataFilter.EditElements.CheckboxEditWebControl();
////			_viewWebControl = new GASystem.GUIUtils.DataFilter.ViewElements.GeneralViewWebControl();
//		}

		public CheckboxFilterElement(GASystem.AppUtils.FieldDescription fd)
		{
			
			_editWebControl = new GASystem.GUIUtils.DataFilter.EditElements.CheckboxEditWebControl(fd);
			_viewWebControl = new GASystem.GUIUtils.DataFilter.ViewElements.CheckboxViewWebcontrol(fd);
//			_editWebControl.FieldName = fd.FieldId;
		}
	




	}
}
