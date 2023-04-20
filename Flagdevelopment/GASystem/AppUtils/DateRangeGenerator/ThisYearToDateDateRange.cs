using System;

namespace GASystem.AppUtils.DateRangeGenerator
{
	/// <summary>
	/// Summary description for ThisYearToNowDateRange.
	/// </summary>
	public class ThisYearToDateDateRange : AbstractDateRange
	{
		public ThisYearToDateDateRange()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public override DateTime GetDateFrom()
		{
			return new System.DateTime(System.DateTime.Now.Year, 1, 1);
		}

		public override DateTime GetDateTo()
		{
			return System.DateTime.Now;
		}


	}
}
