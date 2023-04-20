using System;

namespace GASystem.AppUtils.DateRangeGenerator
{
	/// <summary>
	/// Summary description for ThisMonthToDateDateRange.
	/// </summary>
	public class ThisMonthToDateDateRange : AbstractDateRange
	{
		public ThisMonthToDateDateRange()
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
			return _now;
		}


	}
}
