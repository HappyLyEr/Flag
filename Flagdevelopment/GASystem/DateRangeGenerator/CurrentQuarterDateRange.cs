using System;

namespace GASystem.AppUtils.DateRangeGenerator
{
	/// <summary>
	/// Summary description for QuarterToDateDateRange.
	/// </summary>
	public class CurrentQuarterDateRange : AbstractDateRange
	{
		public CurrentQuarterDateRange()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public override DateTime GetDateFrom()
		{
			return DateUtilities.DateUtilities.GetStartOfCurrentQuarter(_now);
		}

		public override DateTime GetDateTo()
		{
			return DateUtilities.DateUtilities.GetEndOfCurrentQuarter(_now);
		}

	}
}
