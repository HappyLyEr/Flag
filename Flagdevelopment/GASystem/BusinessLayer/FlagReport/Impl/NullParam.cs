using System;
using GASystem.BusinessLayer.FlagReport;

namespace GASystem.BusinessLayer.FlagReport.Impl
{
	/// <summary>
	/// Nullparam. Default parameter class returning a null value;
	/// </summary>
	public class NullParam : IParameter
	{
		public NullParam()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		#region IParameter Members

		public object GetValue()
		{
			// TODO:  Add NullParam.GetValue implementation
			return null;
		}

		#endregion
	}
}
