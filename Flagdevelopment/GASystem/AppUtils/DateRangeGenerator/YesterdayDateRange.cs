using System;

namespace GASystem.AppUtils.DateRangeGenerator
{
	/// <summary>
	/// Summary description for YesterdayDateRange.
	/// </summary>
	public class YesterdayDateRange : AbstractDateRange
	{
		public YesterdayDateRange()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		
		public override DateTime GetDateFrom()
		{
			return DateUtilities.DateUtilities.GetStartOfDay(_now.Subtract(new TimeSpan(1,0,0,0)));
		}

		public override DateTime GetDateTo()
		{
			return DateUtilities.DateUtilities.GetEndOfDay(_now.Subtract(new TimeSpan(1,0,0,0)));
		}

	}
}
