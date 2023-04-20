using System;
using GASystem.DataModel;
using System.Collections;
using GASystem.BusinessLayer;
using GASystem.AppUtils;


namespace GASystem.GUIUtils.DataFilter
{
	/// <summary>
	/// Summary description for FilterUtils.
	/// </summary>
	public class FilterUtils
	{
		public FilterUtils()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static ArrayList GetFilterShortCutsForDataClass(GADataClass DataClass, bool IsViewPage) 
		{
			ClassDescription cd = ClassDefinition.GetClassDescriptionByGADataClass(DataClass);
			if (cd.FilterShortCutField == string.Empty)  
				return new ArrayList();  //no shortcut defined
			
			FieldDescription fd = FieldDefintion.GetFieldDescription(cd.FilterShortCutField, cd.DataClassName);

			if (fd.ListCategory == string.Empty)
				return new ArrayList();  //for the time being we are only supporting drowdownlist types. TODO expand this

			//get dropdownlist values
			GASystem.DataModel.ListsDS ds = GASystem.BusinessLayer.Lists.GetListsRowIdByCategory(fd.ListCategory);

			GADataRecord owner = SessionManagement.GetCurrentDataContext().SubContextRecord;
			//bool isViewPage = 


			ArrayList tabs = new ArrayList();

			if (IsViewPage) 
			{  //we are currently viewing a singel record
			
				foreach (ListsDS.GAListsRow row in ds.GALists.Rows)
					tabs.Add(new GASystem.UserControls.SubTabDetails(LinkUtils.GenerateURLWithDataClassFilterTab(owner.DataClass, owner.RowId.ToString(), DataClass, GUIUtils.DataFilter.FilterOperator.Equal.ToString(), fd.FieldId, row.GAListDescription), row.GAListDescription));
				//add all tab
				tabs.Add(new GASystem.UserControls.SubTabDetails(LinkUtils.GenerateURLForSingleRecordView(owner.DataClass.ToString(), owner.RowId.ToString(), DataClass.ToString()), GASystem.AppUtils.Localization.GetGuiElementText("All")));
			} 
			else
			{ //we are currently view a list all
				foreach (ListsDS.GAListsRow row in ds.GALists.Rows)
					tabs.Add(new GASystem.UserControls.SubTabDetails(LinkUtils.GenerateURLWithDataClassFilter(DataClass, GUIUtils.DataFilter.FilterOperator.Equal.ToString(), fd.FieldId, row.GAListDescription), row.GAListDescription));
				//add all tab
				tabs.Add(new GASystem.UserControls.SubTabDetails(LinkUtils.GenerateURLForListAll(DataClass.ToString()), GASystem.AppUtils.Localization.GetGuiElementText("All")));

			}

			return tabs;
		}
		
//		public static string getDataTypeForFieldName(string TableName, string FieldName)
//		{
//			FieldDescription fd = FieldDefintion.GetFieldDescription(FieldName, TableName);
//			if (fd != null)
//				return fd.DataType;
//			return FieldName;
//		}
	}
}
