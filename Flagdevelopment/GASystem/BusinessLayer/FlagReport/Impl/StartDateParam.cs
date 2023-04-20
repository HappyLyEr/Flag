using System;

namespace GASystem.BusinessLayer.FlagReport.Impl
{
	/// <summary>
	/// Summary description for StartDateParam.
	/// </summary>
	public class StartDateParam : IParameter
	{
		System.DateTime _startDate;

		public StartDateParam()
		{
			_startDate = System.DateTime.Now;
		}

		public StartDateParam(int ReportInstance)
		{
			if(ReportInstance == 0)
				_startDate = System.DateTime.Now;
			else
				_startDate = BusinessLayer.ReportInstance.GetDateRange(ReportInstance).GetDateFrom();
		}

		#region IParameter Members

		public object GetValue()
		{
			// TODO:  Add StartDateParam.GetValue implementation
			return _startDate;
		}

		#endregion
	}
}
