using System;

namespace GASystem.GUIUtils.DataFilter
{
	/// <summary>
	/// Summary description for GeneralFilterElement.
	/// </summary>
    public class PathLookupFilterElement : GeneralFilterElement
	{

		public PathLookupFilterElement(GASystem.AppUtils.FieldDescription fd)
		{
			
			_editWebControl = new GASystem.GUIUtils.DataFilter.EditElements.PathLookupEditWebControl(fd);
			_viewWebControl = new GASystem.GUIUtils.DataFilter.ViewElements.ListViewWebcontrol(fd);
		
			
		}


	}
}
