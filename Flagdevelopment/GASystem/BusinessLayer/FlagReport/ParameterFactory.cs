using System;
using GASystem.BusinessLayer;
using GASystem.DataModel;


namespace GASystem.BusinessLayer.FlagReport
{
	/// <summary>
	/// Summary description for ParameterFactory.
	/// </summary>
	public class ParameterFactory
	{
		public ParameterFactory()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static IParameter Make(string ParameterName, GADataRecord DataRecord, int ReportInstance) 
		{
			ParameterEnum paramType;
			
				try 
				{
					paramType = (ParameterEnum)System.Enum.Parse(typeof(ParameterEnum), ParameterName, true);
				} 
				catch (ArgumentException ex) 
				{
					//invalid paramtype, set paramtype to nullparam;
					paramType = ParameterEnum.NullParameter;
				}


			switch (paramType)
			{
				case ParameterEnum.EndDate :
					return new Impl.EndDateParam(ReportInstance);
				case ParameterEnum.StartDate :
					return new Impl.StartDateParam(ReportInstance);
				case ParameterEnum.OwnerName :
					return new Impl.OwnerNameParam(DataRecord);
				case ParameterEnum.TotalExposedHoursLocation :
					return new Impl.NullParam();
				case ParameterEnum.TotalExposedHoursProject :
					return new Impl.NullParam();
			}

			return new Impl.NullParam();

		}
	}
}
