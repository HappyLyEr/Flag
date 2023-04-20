using System;

namespace GASystem.BusinessLayer.FlagReport.Impl
{
	/// <summary>
	/// Summary description for EndDateParam.
	/// </summary>
	public class EndDateParam : IParameter
	{
		System.DateTime _endDate;
		
		public EndDateParam()
		{
			//
			// TODO: Add constructor logic here
			//
			_endDate = System.DateTime.Now;
		}

		public EndDateParam(int ReportInstance)
		{
			if(ReportInstance == 0)
				_endDate = System.DateTime.Now;
			else
				_endDate = BusinessLayer.ReportInstance.GetDateRange(ReportInstance).GetDateTo();
		}

		#region IParameter Members

		public object GetValue()
		{
			// TODO:  Add EndDateParam.GetValue implementation
			return _endDate;
		}

		#endregion
	}
}
