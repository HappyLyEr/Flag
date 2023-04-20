using System;

namespace GASystem.AppUtils.DateRangeGenerator
{
	/// <summary>
	/// Summary description for ThisMonthToDateDateRange.
	/// </summary>
	public class CurrentMonth : AbstractDateRange
	{
		public CurrentMonth()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public override DateTime GetDateFrom()
		{
			return new DateTime (_now.Year, _now.Month, 1);
		}

		public override DateTime GetDateTo()
		{
			return new DateTime (_now.Year, _now.Month, DateTime.DaysInMonth(_now.Year, _now.Month), 23, 59, 59, 999); 
		}


	}
}
