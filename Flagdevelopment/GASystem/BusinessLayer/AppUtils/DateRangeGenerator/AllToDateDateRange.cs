using System;

namespace GASystem.AppUtils.DateRangeGenerator
{
	/// <summary>
	/// Summary description for AllDates.
	/// </summary>
	public class AllToDateDateRange : AbstractDateRange
	{
		public AllToDateDateRange()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public override DateTime GetDateFrom()
		{
			return new DateTime (1753, 1,1);  //setting it to 1753 because this is the lowest possible mssql data
		}

		public override DateTime GetDateTo()
		{
			return DateTime.Now;
		}


	}
}
