using System;

namespace GASystem.AppUtils.DateRangeGenerator
{
	/// <summary>
	/// Summary description for LastMonthDateRange.
	/// </summary>
	public class Last30DaysDateRange : AbstractDateRange
	{
		public Last30DaysDateRange()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public override DateTime GetDateFrom()
		{
			DateTime last30days = base._now.AddDays(-30);
			
			return last30days;
		}

		public override DateTime GetDateTo()
		{
			return base._now;
		}


	}
}
