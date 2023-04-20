using System;
using GASystem.DataModel;

namespace GASystem.DataAccess
{
	/// <summary>
	/// Summary description for DataAccessFactory.
	/// </summary>
	public class DataAccessListViewFactory
	{
        public DataAccessListViewFactory()
		{
			//
			// TODO: Add constructor logic here
			//
		}

        public static IDataAccessListView Make(GADataClass DataClass, GADataTransaction Transaction)
		{
            IDataAccessListView listView;
            AppUtils.ClassDescription cd = AppUtils.ClassDefinition.GetClassDescriptionByGADataClass(DataClass);

            if (cd.IsView)
                listView = new DataAccessListViewForViews(DataClass, Transaction);
            else
                listView = new DataAccessListView(DataClass, Transaction);
            return (IDataAccessListView)listView;
		}

        public static IDataAccessListView Make(GADataClass DataClass)
		{
			return Make(DataClass, null);	
		}
	}
}
