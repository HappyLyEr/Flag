using System;

namespace GASystem.AppUtils.DateRangeGenerator
{
	/// <summary>
	/// Summary description for CurrentWeekDateRange.
	/// </summary>
	public class CurrentWeekDateRange :AbstractDateRange
	{
		public CurrentWeekDateRange()
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
			return DateUtilities.DateUtilities.GetEndOfCurrentWeek(_now);
		}


	}
}
