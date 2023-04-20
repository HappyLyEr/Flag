using System;
using GASystem.DataAccess;
using GASystem.DataModel;

namespace GASystem.BusinessLayer
{
	/// <summary>
	/// Summary description for ReportInstanceFilter.
	/// </summary>
	public class ReportInstanceFilter
	{
		public ReportInstanceFilter()
		{
			//
			// TODO: Add constructor logic here
			//
		}
	
		public static ReportInstanceFilterDS GetReportInstanceFilterByOwner(GADataRecord Owner) 
		{
			return ReportInstanceFilterDb.GetReportInstanceFilterByOwner(Owner);
		}

		public static ReportInstanceFilterDS GetReportInstanceFilterByRowId(int RowId) 
		{
			return ReportInstanceFilterDb.GetReportInstanceFilterByRowId(RowId);
		}

		public static ReportInstanceFilterDS NewReportInstanceFilter() 
		{
			ReportInstanceFilterDS ds = new ReportInstanceFilterDS();
			ds.GAReportInstanceFilter.Rows.Add(ds.GAReportInstanceFilter.NewGAReportInstanceFilterRow());
			return ds;
		}

		public static ReportInstanceFilterDS UpdateReportInstanceFilter(ReportInstanceFilterDS ReportInstanceFilterSet) 
		{
			return ReportInstanceFilterDb.UpdateReportInstanceFilter(ReportInstanceFilterSet);
		}
	}

	
}
