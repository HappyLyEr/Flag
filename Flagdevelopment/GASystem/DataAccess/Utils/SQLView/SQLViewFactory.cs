using System;
using GASystem;

namespace GASystem.DataAccess.Utils.SQLView
{
	/// <summary>
	/// Summary description for SQLViewFactory.
	/// </summary>
	public class   SQLViewFactory
	{
		public SQLViewFactory()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static ISQLView Make(AppUtils.ClassDescription cd, DataModel.GADataRecord Owner) 
		{
			if (cd.DataClassName.ToUpper() == DataModel.GADataClass.GACrewListView.ToString().ToUpper())
				return new CrewListSQLView(cd, Owner);
			if (cd.DataClassName.ToUpper() == DataModel.GADataClass.GALastLoginView.ToString().ToUpper())
				return new LastUserLoginSQLView(cd, Owner);
            if (cd.hasViewSQL())
                return new FlagClassDefinedSQLView(cd, Owner);
			return new GeneralSQLView(cd, Owner);
		}

	}
}
