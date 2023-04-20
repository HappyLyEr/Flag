using System;

namespace GASystem.AppUtils.DateRangeGenerator
{
	/// <summary>
	/// Abstract class for calculating dateranges.
	/// </summary>
	public abstract class AbstractDateRange
	{
		protected DateTime _dateFromSeed;
		protected DateTime _dateToSeed;
		protected DateTime _now = DateTime.Now;
		
		public AbstractDateRange(DateTime DateFromSeed, DateTime DateToSeed)
		{
			//
			// TODO: Add constructor logic here
			//
			_dateFromSeed = DateFromSeed;
			_dateToSeed = DateToSeed;
		}

		public AbstractDateRange()
		{
			//
			// TODO: Add constructor logic here
			//
//			_dateFromSeed = System.DateTime.MinValue;// null;
//			_dateToSeed = System.DateTime.MinValue;//null;
		}

		public DateTime DateFromSeed 
		{
			get {return _dateFromSeed;}
			set {_dateFromSeed = value;}
		}

		public DateTime DateToSeed 
		{
			set {_dateToSeed = value;}
			get {return _dateToSeed;}
		}

		/// <summary>
		/// get start date for this daterange
		/// </summary>
		/// <returns></returns>
		public abstract DateTime GetDateFrom();
		
		/// <summary>
		/// Get end date for this date range
		/// </summary>
		/// <returns></returns>
		public abstract DateTime GetDateTo();
		

	}
}
