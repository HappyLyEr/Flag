using System;

namespace GASystem.AppUtils.DateRangeGenerator
{
	/// <summary>
	/// Summary description for ThisYearToNowDateRange.
	/// </summary>
	public class CurrentYear : AbstractDateRange
	{
		public CurrentYear()
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
			return new System.DateTime(System.DateTime.Now.Year, 12, 31, 23, 59, 59, 999);
		}


	}
}
