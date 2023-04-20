using System;

namespace GASystem.AppUtils.DateRangeGenerator
{
	/// <summary>
	/// Summary description for LastMonthDateRange.
	/// </summary>
	public class Last30DaysBeforeDateDateRange : AbstractDateRange
	{
		public Last30DaysBeforeDateDateRange()
		{
			//
			// TODO: Add constructor logic here
			//
			base.DateToSeed = base._now;
		}

		public override DateTime GetDateFrom()
		{
			DateTime last30days = base._dateToSeed.AddDays(-30);
			
			return last30days;
		}

		public override DateTime GetDateTo()
		{
			return base._dateToSeed;
		}


	}
}
