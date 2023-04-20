using System;

namespace GASystem.AppUtils.DateRangeGenerator
{
	/// <summary>
	/// Summary description for QuarterToDateDateRange.
	/// </summary>
	public class QuarterToDateDateRange : AbstractDateRange
	{
		public QuarterToDateDateRange()
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
			return _now;
		}

	}
}
