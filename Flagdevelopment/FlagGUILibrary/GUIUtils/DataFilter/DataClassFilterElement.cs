using System;

namespace GASystem.GUIUtils.DataFilter
{
	/// <summary>
	/// Summary description for GeneralFilterElement.
	/// </summary>
    public class DataClassFilterElement : GeneralFilterElement
	{

		public DataClassFilterElement(GASystem.AppUtils.FieldDescription fd)
		{
			
			_editWebControl = new GASystem.GUIUtils.DataFilter.EditElements.DataClassEditWebControl(fd);
			_viewWebControl = new GASystem.GUIUtils.DataFilter.ViewElements.ListViewWebcontrol(fd);
		
			
		}


	}
}
