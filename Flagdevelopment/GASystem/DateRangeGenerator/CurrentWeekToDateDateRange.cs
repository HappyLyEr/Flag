using System;

namespace GASystem.AppUtils.DateRangeGenerator
{
	/// <summary>
	/// Summary description for CurrentWeekToDateDateRange.
	/// </summary>
	public class CurrentWeekToDateDateRange : AbstractDateRange
	{
		public CurrentWeekToDateDateRange()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public override DateTime GetDateFrom()
		{
			return DateUtilities.DateUtilities.GetStartOfCurrentWeek(_now);
		}

		public override DateTime GetDateTo()
		{
			return _now;
		}

	}
}
