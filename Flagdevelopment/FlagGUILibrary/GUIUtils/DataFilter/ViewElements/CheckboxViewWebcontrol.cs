using System;
using GASystem.AppUtils;

namespace GASystem.GUIUtils.DataFilter.ViewElements
{
	/// <summary>
	/// Summary description for CheckboxViewWebcontrol.
	/// </summary>
	public class CheckboxViewWebcontrol : GeneralViewWebControl
	{
        public CheckboxViewWebcontrol(FieldDescription fd) : base(fd)
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public override void GenerateFilterString(string FieldName, FilterOperator Operator, string Condition)
		{
			base.GenerateFilterString(FieldName, Operator, Condition);

			if (Operator == FilterOperator.isCurrent)
				FilterString = "(" + FilterString + " or " + FieldName + " is null)";
		}

	}
}
