using System;
using GASystem.DataModel;
using GASystem.DataAccess;

namespace GASystem.BusinessLayer
{
	/// <summary>
	/// Summary description for FieldDefinition.
	/// </summary>
	public class FieldDefinition
	{
		public FieldDefinition()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static FieldDefinitionDS GetActiveColumnsForTable(string TableName) 
		{	
			return FieldDefinitionDb.GetActiveColumnsForTable(TableName);
		}

		/// <summary>
		/// Returns a string holding a default sort order expression for the gadataclass. THis
		/// string is intended to be used as sort setting on a datasetview
		/// </summary>
		public static string GetSortOrderDefinitionForGADataClass(GADataClass DataClass) 
		{
			FieldDefinitionDS fds = FieldDefinitionDb.GetSortColumnsForGADataClass(DataClass);
			//no rows returned, there is no default sort
			if (fds.GAFieldDefinitions.Rows.Count == 0)
				return string.Empty;

			string SortString = GetSingleSortDefition(fds.GAFieldDefinitions[0]);
			for (int t = 1; t < fds.GAFieldDefinitions.Rows.Count; t++) 
				SortString = SortString + ", " + GetSingleSortDefition(fds.GAFieldDefinitions[t]);

			return SortString;
		}

		private static string GetSingleSortDefition(FieldDefinitionDS.GAFieldDefinitionsRow Row) 
		{
			string sortrow = string.Empty;
			if (!Row.IsSortOrderNull()) {
				sortrow = Row.FieldId;
				if (!Row.IsAscDescNull())
					sortrow += " " + Row.AscDesc;
			}
			return sortrow;
		}
	}
}
