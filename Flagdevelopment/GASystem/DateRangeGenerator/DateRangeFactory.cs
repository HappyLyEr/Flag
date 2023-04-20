using System;

namespace GASystem.AppUtils.DateRangeGenerator
{
	public enum DateRangeEnum {LastMonth, LastYear, MonthToDate, YearToDate, AdHoc, IncludeAllDates, Last30Days, LastWeek, CurrentWeek, WeekToDate, 
		LastQuarter, QuarterToDate, CurrentQuarter,  Last12Months, ToDay, CurrentMonth, CurrentYear, EarliestToDate, Yesterday }
	/// <summary>
	/// Summary description for DateRangeFactory.
	/// </summary>
	/// 

	 // Tor 2005 Jan Ove asked: EarliestToDate WeekToDate  QuarterToDate    TODO ask Tor, what does these do?????


	public class DateRangeFactory 
	{
		public DateRangeFactory()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		/// <summary>
		/// Parse datetypestring to DateRangeEnum
		/// </summary>
		/// <param name="DateRangeIdentifier"></param>
		/// <returns></returns>
		public static DateRangeEnum ParseDateRangeEnum(string DateRangeIdentifier) 
		{
			return (DateRangeEnum)Enum.Parse(typeof(DateRangeEnum), DateRangeIdentifier, true);
		}


		/// <summary>
		/// Make a new instance of daterange based on DateRangeEnum
		/// </summary>
		/// <param name="DateRangeIdentifier"></param>
		/// <returns></returns>
		public static AbstractDateRange Make(DateRangeEnum DateRangeIdentifier) 
		{
//			public enum DateRangeEnum 
//			{
//				, , , , , , , ,  
//				LastQuarter,  Last12Months, ToDay, CurrentWeek, , CurrentQuarter,  }
//	
			
			
			switch(DateRangeIdentifier) 
			{
				case DateRangeEnum.LastMonth :
					return new LastMonthDateRange();
				case DateRangeEnum.LastYear :
					return new LastYearDateRange();
				case DateRangeEnum.MonthToDate :
					return new ThisMonthToDateDateRange();
				case DateRangeEnum.YearToDate :
					return new ThisYearToDateDateRange();
				case DateRangeEnum.AdHoc : 
					return new DefaultRangeDateRange();
				case DateRangeEnum.IncludeAllDates:
					return new AllDatesDateRange();
				case DateRangeEnum.EarliestToDate:
					return new AllToDateDateRange();
				case DateRangeEnum.Last30Days :
					return new Last30DaysDateRange();
				case DateRangeEnum.CurrentMonth :
					return new CurrentMonth();
				case DateRangeEnum.CurrentYear :
					return new CurrentYear();
				case DateRangeEnum.Last12Months:
					return new Last12MonthsDateRange();
				case DateRangeEnum.CurrentQuarter:
					return new CurrentQuarterDateRange();
				case DateRangeEnum.LastQuarter:
					return new LastQuarterDateRange();
				case DateRangeEnum.QuarterToDate:
					return new QuarterToDateDateRange();
				case DateRangeEnum.CurrentWeek :
					return new CurrentWeekDateRange();
				case DateRangeEnum.LastWeek :
					return new LastWeekDateRange();
				case DateRangeEnum.WeekToDate :
					return new CurrentWeekToDateDateRange();
				case DateRangeEnum.ToDay :
					return new ToDayDateRange();
				case DateRangeEnum.Yesterday :
					return new YesterdayDateRange();




				
			}
			throw new ArgumentException("Invalid date range");
		}
		
		/// <summary>
		/// Make a new instance of daterange based on DateRangeEnum. Set initial to and from date.
		/// </summary>
		/// <param name="DateRangeIdentifier"></param>
		/// <param name="DateFromSeed"></param>
		/// <param name="DateToSeed"></param>
		/// <returns></returns>
		public static AbstractDateRange Make(DateRangeEnum DateRangeIdentifier, DateTime DateFromSeed, DateTime DateToSeed)   
		{
			AbstractDateRange dateRange = Make(DateRangeIdentifier);
			dateRange.DateFromSeed = DateFromSeed;
			dateRange.DateToSeed = DateToSeed;
			return dateRange;
		}

		 
		

	}
}
