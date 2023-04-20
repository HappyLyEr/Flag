using System;

namespace GASystem.AppUtils.DateRangeGenerator
{
	/// <summary>
	/// Summary description for LastMonthDateRange.
	/// </summary>
	public class Last12MonthsDateRange : AbstractDateRange
	{
		public Last12MonthsDateRange()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public override DateTime GetDateFrom()
		{
			DateTime last12monts = base._now.AddMonths(-12);
			
			return last12monts;
		}

		public override DateTime GetDateTo()
		{
			return base._now;
		}


	}
}
