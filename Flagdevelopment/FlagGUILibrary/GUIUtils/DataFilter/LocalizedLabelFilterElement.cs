using System;

namespace GASystem.GUIUtils.DataFilter
{
	/// <summary>
	/// Summary description for GeneralFilterElement.
	/// </summary>
    public class LocalizedLabelFilterElement : GeneralFilterElement
	{

		public LocalizedLabelFilterElement(GASystem.AppUtils.FieldDescription fd)
		{
			
			_editWebControl = new GASystem.GUIUtils.DataFilter.EditElements.LocalizedLabelEditWebControl(fd);
			_viewWebControl = new GASystem.GUIUtils.DataFilter.ViewElements.ListViewWebcontrol(fd);
		
			
		}


	}
}
