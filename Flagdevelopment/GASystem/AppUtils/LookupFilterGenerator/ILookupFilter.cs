using System;
using GASystem.DataModel;

namespace GASystem.AppUtils.LookupFilterGenerator
{
	/// <summary>
	/// Summary description for ILookupFilter.
	/// </summary>
	public interface ILookupFilter
	{
		string Filter 
		{
			get;
			set;
		}

		string FilterDescription 
		{
			get;
		}

		bool CanDisableFilter 
		{
			get;
		}


		GADataRecord Owner 
		{
			get;
			set;
		}
	}
}
