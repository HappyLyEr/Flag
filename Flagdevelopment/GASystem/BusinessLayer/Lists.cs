using System;
using System.Runtime.InteropServices;
using GASystem.DataModel;
using GASystem.DataAccess;
using GASystem.AppUtils;

namespace GASystem.BusinessLayer
{
	/// <summary>
	/// Summary description for Lists.
	/// </summary>
	public class Lists : BusinessClass
	{
		public Lists()
		{
			//
			// TODO: Add constructor logic here
			//
			DataClass = GADataClass.GALists;
		}

		public override System.Data.DataSet Update(System.Data.DataSet ds, GADataTransaction transaction)
		{
            // Tor 20161028 added: use UpdateGASuperClassChangedBy to update attributes in GASuperClass record 
            GASystem.AppUtils.SuperClassAttributes.UpdateGASuperClassChangedBy(ds, transaction);
			System.Data.DataSet dsResult = UpdateLists((ListsDS)ds, transaction);
			//refresh galist cache
			AppUtils.CodeTables.ClearListCache();

			return dsResult; 
		}
		
		public override System.Data.DataSet GetByRowId(int RowId)
		{
			return GetListsByListsRowId(RowId);
		}

		

		public static ListsDS GetAllListss()
		{
			return ListsDb.GetAllListss();
		}
	

		public static ListsDS GetListssByOwner(GADataRecord ListsOwner)
		{
			return  ListsDb.GetListssByOwner(ListsOwner.RowId, ListsOwner.DataClass);
		}

		public static ListsDS GetListsByListsRowId(int ListsRowId)
		{
			return ListsDb.GetListsByListsRowId(ListsRowId);
		}

        public static string GetListValueByRowId(int RowId)
        {
            return GetListValueByRowId(RowId, null);
//            return ListsDb.GetListValueByRowId(RowId, transaction);

            //ListsDS ds = GetListsByListsRowId(RowId);
            //if (ds.GALists.Rows.Count > 0)
            //    return ds.GALists[0].GAListValue;
            //return null;
        }

        public static string GetListValueByRowId(int RowId, GASystem.DataAccess.GADataTransaction transaction) 
		{
            return ListsDb.GetListValueByRowId(RowId, transaction);

            //ListsDS ds = GetListsByListsRowId(RowId);
            //if (ds.GALists.Rows.Count > 0)
            //    return ds.GALists[0].GAListValue;
            //return null;
		}

        public static string GetListDescriptionByRowId(int RowId)
        {
            ListsDS ds = GetListsByListsRowId(RowId);
            if (ds.GALists.Rows.Count > 0)
                return ds.GALists[0].GAListDescription;
            return null;
        }

		public static ListsDS GetListsByListsRowIds(int[] ListsRowIds) 
		{
			return ListsDb.GetListsByListsRowIds(ListsRowIds);
		}

		public static ListsDS GetListsByClass(GADataClass DataClass) 
		{
			return ListsDb.GetListsByClass(DataClass);
		}

		public static int GetListsRowIdByCategoryAndKey(string CategoryName, string Key) 
		{
			int categoryId = ListCategory.GetListCategoryRowIdByName(CategoryName);
			return ListsDb.GetListsRowIdByCategoryAndKey(categoryId, Key);
			//get categoryid
			//BusinessLayer.ListCategory.
		}

		public static ListsDS GetListsRowIdByCategory(string CategoryName)
		{
			int categoryId = ListCategory.GetListCategoryRowIdByName(CategoryName);
			return (new ListsDb()).GetListsRowIdByCategory(categoryId);
			//get categoryid
			//BusinessLayer.ListCategory.
		}

        //july23-2020 localize the GAList with the current language
        public static ListsDS GetListsRowIdByCategory(string CategoryName, [Optional, DefaultParameterValue(true)]
            bool localizeIt)
        {
            int categoryId = ListCategory.GetListCategoryRowIdByName(CategoryName);

            return (new ListsDb()).GetListsRowIdByCategory(categoryId, localizeIt);
        }

		public static ListsDS GetNewLists()
		{
			ListsDS iDS = new ListsDS();
			GASystem.DataModel.ListsDS.GAListsRow row = iDS.GALists.NewGAListsRow();
		//	set default values for non-null attributes
		//	row.EmploymentRowId = 0;
		//	row.DateTimeFrom = DateTime.Today;
			iDS.GALists.Rows.Add(row);
			return iDS;
			
		}

		public static ListsDS SaveNewLists(ListsDS ListsSet, GADataRecord ListsOwner)
		{
			ListsSet = UpdateLists(ListsSet);
			//DataClassRelations.CreateDataClassRelation(ListsOwner.RowId, ListsOwner.DataClass, ListsSet.GALists[0].ListsRowId, GADataClass.GALists);
			return ListsSet;
		}

		public static ListsDS UpdateLists(ListsDS ListsSet)
		{
			return UpdateLists(ListsSet, null);
		}
		public static ListsDS UpdateLists(ListsDS ListsSet, GADataTransaction transaction )
		{
			return ListsDb.UpdateLists(ListsSet, transaction);
		}

	}
}
