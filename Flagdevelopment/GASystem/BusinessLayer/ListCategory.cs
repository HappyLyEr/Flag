using System;
using GASystem.DataModel;
using GASystem.DataAccess;

namespace GASystem.BusinessLayer
{
	/// <summary>
	/// Summary description for ListCategory.
	/// </summary>
	public class ListCategory : BusinessClass
	{
		public ListCategory()
		{
			//
			// TODO: Add constructor logic here
			//
			DataClass = GADataClass.GAListCategory;
		}

        // Tor 20161028 use standard update method in : BusinessClass
        //public override System.Data.DataSet Update(System.Data.DataSet ds, GADataTransaction transaction)
        //{
        //    return UpdateListCategory((ListCategoryDS)ds, transaction);
        //}

		public static ListCategoryDS GetListCategoryByListCategoryRowId(int RowId) 
		{
			return ListCategoryDb.GetListCategoryByListCategoryRowId(RowId);
		}
		
		public override System.Data.DataSet GetByRowId(int RowId)
		{
			return GetListCategoryByListCategoryRowId(RowId);
		}

		public static ListCategoryDS GetAllListCategories()
		{
			return ListCategoryDb.GetAllListCategories();
		}
	

		public static ListCategoryDS GetListCategorysByOwner(GADataRecord ListCategoryOwner)
		{
			return  ListCategoryDb.GetListCategorysByOwner(ListCategoryOwner.RowId, ListCategoryOwner.DataClass);
		}

		public static int GetListCategoryRowIdByName(string ListCategoryName) 
		{
			return ListCategoryDb.GetListCategoryRowIdByName(ListCategoryName);
		}

		public static ListCategoryDS GetNewListCategory()
		{
			ListCategoryDS iDS = new ListCategoryDS();
			GASystem.DataModel.ListCategoryDS.GAListCategoryRow row = iDS.GAListCategory.NewGAListCategoryRow();
		//	set default values for non-null attributes
		//	row.EmploymentRowId = 0;
		//	row.DateTimeFrom = DateTime.Today;
			iDS.GAListCategory.Rows.Add(row);
			return iDS;
			
		}

		public static ListCategoryDS SaveNewListCategory(ListCategoryDS ListCategorySet, GADataRecord ListCategoryOwner)
		{
			ListCategorySet = UpdateListCategory(ListCategorySet);
			//DataClassRelations.CreateDataClassRelation(ListCategoryOwner.RowId, ListCategoryOwner.DataClass, ListCategorySet.GAListCategory[0].ListCategoryRowId, GADataClass.GAListCategory);
			return ListCategorySet;
		}

		public static ListCategoryDS UpdateListCategory(ListCategoryDS ListCategorySet)
		{
			return UpdateListCategory(ListCategorySet, null);
		}
		public static ListCategoryDS UpdateListCategory(ListCategoryDS ListCategorySet, GADataTransaction transaction )
		{
			return ListCategoryDb.UpdateListCategory(ListCategorySet, transaction);
		}

	}
}
