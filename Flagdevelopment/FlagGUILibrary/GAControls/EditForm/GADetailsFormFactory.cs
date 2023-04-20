using System;
using GASystem.DataModel;

namespace GASystem.GAControls.EditForm
{
	/// <summary>
	/// Factory for creating instances of ga form for editing a record.
	/// </summary>
	public class GADetailsFormFactory
	{
		public GADetailsFormFactory()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		/// <summary>
		/// Create an instance of a ga details form for editing based on a GADataRecord
		/// </summary>
		/// <param name="DataRecord">GADataRecord</param>
		/// <returns>Instance of a ga edit form</returns>
		public static AbstractDetailsForm Make(GADataRecord DataRecord) 
		{
			//have only got one class to choose from for the time being
			if (DataRecord.DataClass == GADataClass.GAAction)
				return new ActionDetailsForm(DataRecord);
			if (DataRecord.DataClass == GADataClass.GAReport)
				return new ReportDetailsForm(DataRecord);
			if (DataRecord.DataClass == GADataClass.GADailyEmployeeCount)
				return new DailyEmpCountDetailsForm(DataRecord);
            if (DataRecord.DataClass == GADataClass.GAExposedHoursGroupView)
                return new ExposedHoursGroupViewDetailsForm(DataRecord);
			//if (DataRecord.DataClass == GADataClass.GAReportInstance)
			//	return new ReportInstanceDetailsFrom(DataRecord);
			return new GeneralDetailsForm(DataRecord);

		}

		public static AbstractDetailsForm Make(GADataRecord DataRecord, bool CreateMultipleRecords)  
		{
			//only create a multiple form if we are not editing an existing record.
			if (!CreateMultipleRecords || DataRecord.RowId > 0)
				return Make(DataRecord);

			return new CreateMultipleRecordsForm(DataRecord);


		}


	}
}
