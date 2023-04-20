using System;

namespace GASystem.AppUtils.DateRangeGenerator
{
	/// <summary>
	/// Summary description for LastMonthDateRange.
	/// </summary>
	public class LastMonthDateRange : AbstractDateRange
	{
		public LastMonthDateRange()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public override DateTime GetDateFrom()
		{
			DateTime lastMonth = DateTime.Now.AddMonths(-1);		
			return new DateTime(lastMonth.Year, lastMonth.Month, 1);
		}

		public override DateTime GetDateTo()
		{
			DateTime lastMonth = DateTime.Now.AddMonths(-1);
			return new DateTime (lastMonth.Year, lastMonth.Month, DateTime.DaysInMonth(lastMonth.Year, lastMonth.Month), 23,59,59);
		}

	}
}
