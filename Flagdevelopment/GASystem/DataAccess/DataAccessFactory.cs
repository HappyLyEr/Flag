using System;
using GASystem.DataModel;

namespace GASystem.DataAccess
{
	/// <summary>
	/// Summary description for DataAccessFactory.
	/// </summary>
	public class DataAccessFactory
	{
		public DataAccessFactory()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static DataAccess Make(GADataClass DataClass, GADataTransaction Transaction)
		{
			return new DataAccess(DataClass, Transaction);	
		}

		public static DataAccess Make(GADataClass DataClass)
		{
			return Make(DataClass, null);	
		}
	}
}
