using System;
using GASystem.DataModel;
using GASystem.DataAccess;
using System.Collections.Generic;

namespace GASystem.BusinessLayer
{
	/// <summary>
	/// Summary description for Employment.
	/// </summary>
	public class EmploymentPathView : BusinessClass
	{
        public EmploymentPathView()
		{
			//
			// TODO: Add constructor logic here
			//
			DataClass = GADataClass.GAEmployment;
		}

        public static EmploymentPathViewDS GetEmploymentByPersonnelId(int personnelId, GADataTransaction transaction)   //, DateTime endDate)
        {
            return EmploymentPathViewDb.GetEmploymentByPersonnelId(personnelId, transaction);
        }
	}
}
