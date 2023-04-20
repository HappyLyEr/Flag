using System;

namespace GASystem.AppUtils.DateRangeGenerator
{
	/// <summary>
	/// Summary description for LastQuarterDateRange.
	/// </summary>
	public class LastQuarterDateRange : AbstractDateRange
	{
		public LastQuarterDateRange()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		

		public override DateTime GetDateFrom()
		{
			return DateUtilities.DateUtilities.GetStartOfLastQuarter(_now);
		}

		public override DateTime GetDateTo()
		{
			return DateUtilities.DateUtilities.GetEndOfLastQuarter(_now);
		}

	}
}
