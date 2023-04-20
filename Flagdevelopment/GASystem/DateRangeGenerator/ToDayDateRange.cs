using System;

namespace GASystem.AppUtils.DateRangeGenerator
{
	/// <summary>
	/// Summary description for ToDayDateRange.
	/// </summary>
	public class ToDayDateRange : AbstractDateRange
	{
		public ToDayDateRange()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public override DateTime GetDateFrom()
		{
			return DateUtilities.DateUtilities.GetStartOfDay(_now);
		}

		public override DateTime GetDateTo()
		{
			return DateUtilities.DateUtilities.GetEndOfDay(_now);
		}


	}
}
