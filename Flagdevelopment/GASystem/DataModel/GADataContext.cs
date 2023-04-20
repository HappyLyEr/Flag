using System;

namespace GASystem.DataModel
{
	/// <summary>
	/// Summary description for GADataContext.
	/// </summary>
	public class GADataContext
	{
		public GADataContext(GADataRecord InitialContextDataRecord, GADataRecord SubContextDataRecord)
		{
			_initialContextRecord = InitialContextDataRecord;
			_subContextRecord = SubContextDataRecord;
			
		}

		private GADataRecord _initialContextRecord;
		private GADataRecord _subContextRecord;
		

		public GADataRecord InitialContextRecord
		{
			get
			{
				return _initialContextRecord;
			}
			set
			{
				_initialContextRecord = value;
			}
		}

		public GADataRecord SubContextRecord
		{
			get
			{
				return _subContextRecord;
			}
			set
			{
				_subContextRecord = value;
			}
		}

		

	

	}
}
