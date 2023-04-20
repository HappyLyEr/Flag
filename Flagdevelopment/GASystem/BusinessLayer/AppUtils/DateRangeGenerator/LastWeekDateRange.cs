using System;

namespace GASystem.AppUtils.DateRangeGenerator
{
	/// <summary>
	/// Summary description for LastMonthDateRange.
	/// </summary>
	public class LastWeekDateRange : AbstractDateRange
	{
		public LastWeekDateRange()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public override DateTime GetDateFrom()
		{
			return DateUtilities.DateUtilities.GetStartOfLastWeek(_now);
		}

		public override DateTime GetDateTo()
		{
			return DateUtilities.DateUtilities.GetEndOfLastWeek(_now);
		}


	}
}
