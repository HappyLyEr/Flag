using System;

namespace GASystem.AppUtils.DateRangeGenerator
{
	/// <summary>
	/// Summary description for LastYearDateRange.
	/// </summary>
	public class LastYearDateRange : AbstractDateRange
	{
		public LastYearDateRange()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public override DateTime GetDateFrom()
		{
			DateTime lastyear = DateTime.Now.AddYears(-1);
			return new DateTime (lastyear.Year, 1, 1);
		}

		public override DateTime GetDateTo()
		{
			DateTime lastyear = DateTime.Now.AddYears(-1);
			return new DateTime (lastyear.Year, 12, 31, 23,59,59,999);
		}

	}
}
