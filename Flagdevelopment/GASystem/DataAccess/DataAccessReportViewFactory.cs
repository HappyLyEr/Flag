using System;
using GASystem.DataModel;

namespace GASystem.DataAccess
{
	/// <summary>
	/// Summary description for DataAccessFactory.
	/// </summary>
	public class DataAccessReportViewFactory
	{
        public DataAccessReportViewFactory()
		{
			//
			// TODO: Add constructor logic here
			//
		}

        public static IDataAccessReportView Make(GADataClass dataClass, GADataTransaction transaction)
		{
            IDataAccessReportView reportView;
            AppUtils.ClassDescription cd = AppUtils.ClassDefinition.GetClassDescriptionByGADataClass(dataClass);

            if (cd.hasViewSQL())
                reportView = new DataAccessReportViewForDataClassView(dataClass, transaction);
            else 
            if (cd.IsView)
                reportView = new DataAccessReportViewForViews(dataClass, transaction);
            else
                reportView = new DataAccessReportView(dataClass, transaction);

            return (IDataAccessReportView)reportView;
		}

        public static IDataAccessReportView Make(GADataClass dataClass)
		{
			return Make(dataClass, null);	
		}
	}
}
