using System;

namespace GASystem.GUIUtils.DataFilter
{
	/// <summary>
	/// Summary description for GeneralFilterElement.
	/// </summary>
	public class ListFilterElement : GeneralFilterElement
	{

		public ListFilterElement(GASystem.AppUtils.FieldDescription fd)
		{
			
			_editWebControl = new GASystem.GUIUtils.DataFilter.EditElements.ListEditWebControl(fd);
			_viewWebControl = new GASystem.GUIUtils.DataFilter.ViewElements.ListViewWebcontrol(fd);
		
			
		}


	}
}
