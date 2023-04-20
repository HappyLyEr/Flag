using System;

namespace GASystem.AppUtils.DateRangeGenerator
{
	/// <summary>
	/// Summary description for DeafultRangeDateRange.
	/// </summary>
	public class DefaultRangeDateRange : AbstractDateRange
	{
		public DefaultRangeDateRange()
		{
			//
			// TODO: Add constructor logic here
			//
			_dateFromSeed = DateTime.Now;
			_dateToSeed = _dateFromSeed;
		}
		
		public override DateTime GetDateFrom()
		{
			return _dateFromSeed;
		}

		public override DateTime GetDateTo()
		{
            //return _dateToSeed;
            return new DateTime( _dateToSeed.Year,_dateToSeed.Month, _dateToSeed.Day, 23, 59, 59);
		}
	}
}
