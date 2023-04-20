using System;
using GASystem.BusinessLayer;
using GASystem.DataModel;


namespace GASystem.AppUtils.LookupFilterGenerator
{
	/// <summary>
	/// Specific filter for filtering personnel records based on whether the personnel has a login
	/// </summary>
    public class PersonnelLoginFilter : GeneralLookupFilter
	{
		public PersonnelLoginFilter()
		{
			Filter = " personnelrowid in (select personnelrowid from gauser) ";
	

		}


	}
}
