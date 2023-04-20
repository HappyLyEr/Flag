using System;
using GASystem.AppUtils;

namespace GASystem.GUIUtils.DataFilter
{
	/// <summary>
	/// Summary description for FilterFactory.
	/// </summary>
	public class FilterFactory
	{
		public FilterFactory()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static IFilterElement MakeFilterElement(FieldDescription fd) 
		{
            //check for class def date fields first

            //ClassDescription cd = ClassDefinition.GetClassDescriptionByGADataClass(fd.TableId);

            //if (fd.FieldId.Trim().ToUpper() == cd.DateFromField.Trim().ToUpper() ||
            //    fd.FieldId.Trim().ToUpper() == cd.DateToField.Trim().ToUpper())
            //{
            //    return new DatePeriodeFilterElement(fd);
            //}

            if (fd.ControlType.ToUpper() == "DROPDOWNLIST" || fd.ControlType.ToUpper() == "POSTBACKDROPDOWNLIST"
                // Tor 20171215 added DROPDOWNLISTMULTIPLE
                //|| fd.ControlType.ToUpper() == "DROPDOWNLISTMULTIPLE"
                )
				return new ListFilterElement(fd);
            if (fd.ControlType.ToUpper() == "SELECTLIST")
                return new SelectListFilterElement(fd);

			if (fd.ControlType.ToUpper() == "DATE")
				return new DateFilterElement(fd);
			
			if (fd.ControlType.ToUpper() == "DATETIME")
				return new DateFilterElement(fd);
			
			if (fd.ControlType.ToUpper() == "NUMERIC")
				return new NumericFilterElement(fd);

			if (fd.ControlType.ToUpper() == "CHECKBOX")
				return new CheckboxFilterElement(fd);

            if (fd.ControlType.ToUpper() == "LOOKUPFIELD" || fd.ControlType.ToUpper() == "LOOKUPFIELDEDIT" || fd.ControlType.ToUpper() == "LOOKUPFIELDVIEW" || fd.ControlType.ToUpper() == "RESPONSIBLE" || fd.ControlType.ToUpper() == "LOOKUPFIELDMULTIPLE")
                return new LookupFilterElement(fd);

            if (fd.ControlType.ToUpper() == "WORKITEMRESPONSIBLE")
                return new WorkitemResponsibleFilterElement(fd);

            if (fd.ControlType.ToUpper() == "GADATACLASS" )
                return new DataClassFilterElement(fd);

            if (fd.ControlType.ToUpper() == "LOCALIZEDLABEL")
                return new LocalizedLabelFilterElement(fd);

            if (fd.ControlType.ToUpper() == "PATHLOOKUP")
                return new PathLookupFilterElement(fd);

			IFilterElement filterElement = new GeneralFilterElement(fd);
			return filterElement;
		}
	}
}
