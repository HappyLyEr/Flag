using System;
using System.Data;
using GASystem.BusinessLayer;
using GASystem.DataModel;
using GASystem.AppUtils;

namespace GASystem.BusinessLayer
{
	/// <summary>
	/// Summary description for Validator.
	/// </summary>
	public class Validator
	{
		public Validator()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static void ValidateDataRow(DataRow Row, GADataClass DataClass)
		{
			foreach (FieldDescription fieldDesc in FieldDefintion.GetFieldDescriptions(DataClass))
			{
				
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="Value1"></param>
		/// <param name="Values"></param>
		/// <returns></returns>
		public bool MutuallyExcludes(String Value1, String[] Values)
		{
			if (Value1==null || Value1.Length==0) return true;
			return false;

		}
		
	}
}
