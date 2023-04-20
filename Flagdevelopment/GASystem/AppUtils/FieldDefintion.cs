using System;
using System.Data;
using GASystem.DataModel;
using GASystem.DataAccess;
using System.Collections;

namespace GASystem.AppUtils
{
	
	
	/// <summary>
	/// Summary description for Fielddefintion.
	/// </summary>
	public class FieldDefintion
	{
		private static DataSet _fieldDefs;
		private static DataSet _fieldDatatypes;

        // Tor 20070812 : replaced GAFieldDefinitions.IntFree1 as ControlHeigth with GAFieldDefinitions.ControlHeigth and GAFieldDefinitions.switchfree1 as HideInNew with GAFieldDefinitions.HideInNew
        // Tor 20140320 : added field GAFieldDefinitions.TextFree3 
        // Tor 20140629 : added field GAFieldDefinitions.TextFree2 
        // Tor 20141208 : added fields GAFieldDefinitions.SwitchFree3 and IntFree1 for GAHistoryRecordLog reporting 

        private static string _sql = @"SELECT  col.character_maximum_length AS Length,  GAFieldDefinitions.TableId, GAFieldDefinitions.FieldId, GAFieldDatatype.DataType, GAFieldDatatype.InheritFrom, GAFieldDatatype.CaptionWidth, 
                      GAFieldDatatype.ControlType, GAFieldDatatype.ControlWidth, GAFieldDefinitions.ControlHeigth, GAFieldDefinitions.HideInNew, GAFieldDefinitions.SwitchFree1 as HideInExcel, GAFieldDefinitions.SwitchFree2 as DisplayInFilterListOnly, GAFieldDatatype.Dataformat, GAFieldDatatype.ListCategory, GAFieldDatatype.SyntaxValidationRule, GAFieldDatatype.CssClass,
                      GAFieldDefinitions.LookupTable, GAFieldDefinitions.LookupTableKey, GAFieldDefinitions.LookupFilter, GAFieldDefinitions.LookupTableDisplayValue, GAFieldDefinitions.DependsOnField, GAFieldDefinitions.DependantField,
						GAFieldDatatype.Helplink, GAFieldDefinitions.TextFree1 as HideIfFormType, GAFieldDatatype.Comment, GAFieldDefinitions.PSecurityClass, GAFieldDefinitions.ColumnOrder, 
                      GAFieldDefinitions.HideInSummary, GAFieldDefinitions.HideInDetail, GAFieldDefinitions.ShowInSmallList, GAFieldDefinitions.IsActive,
						GAFieldDefinitions.RequiredField, GAFieldDefinitions.CopyFromFieldId, GAFieldDefinitions.CompareOperator, GAFieldDefinitions.CompareTo, GAFieldDefinitions.SortOrder, GAFieldDefinitions.isreadonly, GAFieldDefinitions.AscDesc,
						GAFieldDefinitions.FilterOperator, GAFieldDefinitions.FilterCondition
                        ,GAFieldDefinitions.TextFree2,GAFieldDefinitions.TextFree3
                        ,GAFieldDefinitions.IntFree1,GAFieldDefinitions.SwitchFree3
											
	FROM      GAFieldDatatype INNER JOIN
                      GAFieldDefinitions ON GAFieldDatatype.DataType = GAFieldDefinitions.Datatype
            left join INFORMATION_SCHEMA.COLUMNS col on col.TABLE_NAME = GAFieldDefinitions.TableId and col.COLUMN_NAME = GAFieldDefinitions.FieldId";
				
		private static string _SqlDatatypes = @"SELECT * FROM GAFieldDatatype";

		public FieldDefintion()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static void InitializeFieldDefintion()
		{
			String sql = _sql;//SELECT * FROM GAFieldDefinitions";
			_fieldDefs = DataUtils.convertDataReaderToDataSet(DataUtils.executeSelect(sql));
			_fieldDefs.Tables[0].PrimaryKey = new DataColumn[] {_fieldDefs.Tables[0].Columns["TableId"], _fieldDefs.Tables[0].Columns["FieldId"] };
		
			_fieldDatatypes = DataUtils.convertDataReaderToDataSet(DataUtils.executeSelect(_SqlDatatypes));
			_fieldDatatypes.Tables[0].PrimaryKey = new DataColumn[] {_fieldDatatypes.Tables[0].Columns["Datatype"] };
		}


		/// <summary>
		/// Get a FieldDescription object for a given field
		/// </summary>
		/// <param name="fieldId">Name of field (usually corresponds to adatabasefield)</param>
		/// <returns>FieldDescription or null if field is not found</returns>
		public static FieldDescription GetFieldDescription(String fieldId, String tableName)
		{
			//TODO make this thread safe
			if (_fieldDefs==null)
				InitializeFieldDefintion();

			FieldDescription fd = null;
			DataRow row = _fieldDefs.Tables[0].Rows.Find(new object[] {tableName, fieldId});
			if (row!=null)
				fd = GetFieldDescription(row);
			return fd;
		}

		public static FieldDescription[] GetFieldDescriptions(GASystem.DataModel.GADataClass DataClass) 
		{
			return GetFieldDescriptions(DataClass.ToString());
		}

        public static FieldDescription[] GetFieldDescriptions(string DataClassName)
        {
            return GetFieldDescriptions(DataClassName, "");
        }

        // Tor 20140320 added overload: include ownerclass, needed to exclued fields if DataClassName is owned by ownerclass
        public static FieldDescription[] GetFieldDescriptions(string DataClassName, string owner) 
		{
			if (_fieldDefs==null)
				InitializeFieldDefintion();

			DataView rows = new DataView(_fieldDefs.Tables[0]);
			rows.RowFilter = "TableId = '" + DataClassName + "'"; 
			//DataRow row = _fieldDefs.Tables[0].Rows.Find(new object[] {tableName, fieldId});
			ArrayList fieldDescs = new ArrayList();

            foreach (DataRowView  row in rows) 
			{ // Tor 20140320 added test to exclude field if record is owned by OwnerClassName
                FieldDescription fd= GetFieldDescription(row.Row);
                if (!fd.HideIfOwnerClass.Contains(";"+owner+";")) // field allowed for this class
                    if (fd.HideIfUserHasRole != string.Empty)     // field closed for roles in HideIfUserHasRole list
                    // exclude field if user has role that matches role in HideIfUserHasRole
                    {
                        if (!hideIfUserHasRole(fd.HideIfUserHasRole)) // user does not have role listed in in HideIfUserHasRole list
                        {
                            fieldDescs.Add(fd);
                        }
                    }
                    else // exclude roles not declared for this field, add to list
                    {
                        fieldDescs.Add(fd);
                    }
			}
            //foreach (DataRowView  row in rows) 
            //{
            //    fieldDescs.Add(GetFieldDescription(row.Row));
            //}
			fieldDescs.Sort();
			
			FieldDescription[] fds = new FieldDescription[fieldDescs.Count];
			fieldDescs.CopyTo(fds);
			return fds;
		}

        // Tor 20140320 Added parameter OwnerClassName
		public static FieldDescription[] GetFieldDescriptionsDetailsForm(string DataClassName, string OwnerClassName) 
		{
			if (_fieldDefs==null)
				InitializeFieldDefintion();

			DataView rows = new DataView(_fieldDefs.Tables[0]);
			rows.RowFilter = "HideInDetail = 0 and  TableId = '" + DataClassName + "'"; 
			//DataRow row = _fieldDefs.Tables[0].Rows.Find(new object[] {tableName, fieldId});
			ArrayList fieldDescs = new ArrayList();
			foreach (DataRowView row in rows) 
			{ // Tor 20140320 added test to exclude field if record is owned by OwnerClassName
                FieldDescription fd= GetFieldDescription(row.Row);
                if (!fd.HideIfOwnerClass.Contains(";"+OwnerClassName+";"))
                    // Tor 20140629 Added test to exclude field if user has role that is listed in HideIfUserHasRole
                {
                    if (fd.HideIfUserHasRole != string.Empty)
                    // exclude field if user has role that matches role in HideIfUserHasRole
                    {
                        if (!hideIfUserHasRole(fd.HideIfUserHasRole))
                        {
                            fieldDescs.Add(fd);
                        }
                    }
                    else
                    {
                        fieldDescs.Add(fd);
                    }
                }
            }

            fieldDescs.Sort();
			
			FieldDescription[] fds = new FieldDescription[fieldDescs.Count];
			fieldDescs.CopyTo(fds);
			return fds;
		}

        // Tor 20140630 added method to check if current logged on user has role in list (list format ;role;role;...)
        public static bool hideIfUserHasRole(string HideIfUserHasRole)
        {
            UserDS currentUser = GASystem.BusinessLayer.User.GetUserByLogonId(GASystem.AppUtils.GAUsers.GetUserId().ToString());
            EmploymentDS eds = GASystem.BusinessLayer.Employment.GetEmploymentsByPersonnelIdAndDate(currentUser.GAUser[0].PersonnelRowId, System.DateTime.Now);
            foreach (EmploymentDS.GAEmploymentRow erow in eds.GAEmployment)
            {
                if (HideIfUserHasRole.Contains(";"+erow.RoleListsRowId.ToString()+";"))
                    return true;
            }
            return false;
        }

		public static string GetSortExpression(GASystem.DataModel.GADataClass DataClass) 
		{
			if (_fieldDefs==null)
				InitializeFieldDefintion();

			DataView rows = new DataView(_fieldDefs.Tables[0]);
			rows.RowFilter = "TableId = '" + DataClass.ToString() + "' and SortOrder is not null"; 
			rows.Sort ="SortOrder";
			//DataRow row = _fieldDefs.Tables[0].Rows.Find(new object[] {tableName, fieldId});
			string sortString = "";
			string seperator = "";
			foreach (DataRowView  row in rows) 
			{
				sortString += seperator + row["fieldid"] + " " + row["AscDesc"];
				seperator = ", ";
			}
			return sortString;
		}

		public static FieldDescription GetFieldDescription(DataRow FieldDefinitionRow) 
		{
			FieldDescription fd = new FieldDescription();
			String inheritFrom = null;
			fd.TableId = FieldDefinitionRow["TableId"].ToString();
			fd.FieldId = FieldDefinitionRow["FieldId"].ToString();
			fd.DataType = FieldDefinitionRow["DataType"].ToString();
			fd.CaptionWidth =  FieldDefinitionRow["CaptionWidth"].ToString();
			fd.ControlType =  FieldDefinitionRow["ControlType"].ToString();
			
			if (FieldDefinitionRow["Length"] != null && !(FieldDefinitionRow["Length"] is System.DBNull))
				fd.DataLength = int.Parse(FieldDefinitionRow["Length"].ToString());
           
            else
                fd.DataLength = 1;
            if (fd.DataLength > 100000) fd.DataLength = -1;


			fd.ControlWidth =  FieldDefinitionRow["ControlWidth"].ToString();
			
			if (FieldDefinitionRow["ControlHeigth"] != null && !(FieldDefinitionRow["ControlHeigth"] is System.DBNull))
				fd.ControlHeight = (int) FieldDefinitionRow["ControlHeigth"];
			else
				fd.ControlHeight = 0;
			
			if (FieldDefinitionRow["HideInNew"] != null && !(FieldDefinitionRow["HideInNew"] is System.DBNull))
				fd.HideInNew = (bool) FieldDefinitionRow["HideInNew"];
			else
				fd.HideInNew = false;



			fd.CodeTableName =  "";//FieldDefinitionRow["CodeTableName"].ToString();
			fd.LookupTable =  FieldDefinitionRow["LookupTable"].ToString();
			fd.LookupTableKey = FieldDefinitionRow["LookupTableKey"].ToString();
			if (FieldDefinitionRow["LookupFilter"] != DBNull.Value)
				fd.LookupFilter = FieldDefinitionRow["LookupFilter"].ToString();
			else
				fd.LookupFilter = string.Empty;
			fd.LookupTableDisplayValue =  FieldDefinitionRow["LookupTableDisplayValue"].ToString();
			if (FieldDefinitionRow["CssClass"] != null)
				fd.CssClass = FieldDefinitionRow["CssClass"].ToString();
				
			fd.ListCategory =  FieldDefinitionRow["ListCategory"].ToString();
			fd.Dataformat =  FieldDefinitionRow["DataFormat"].ToString();
			fd.ColumnOrder = int.Parse(FieldDefinitionRow["ColumnOrder"].ToString());
			fd.HideInSummary = (bool) FieldDefinitionRow["HideInSummary"];
			fd.HideInDetail = (bool) FieldDefinitionRow["HideInDetail"];
            if (FieldDefinitionRow["TextFree2"] != DBNull.Value) // Tor 20140629 added to be able to exclude field if current user has RoleId in list (format ;role;role;... Example: ;3714;3715;) 
                fd.HideIfUserHasRole = FieldDefinitionRow["TextFree2"].ToString();
            if (FieldDefinitionRow["TextFree3"] != DBNull.Value) // Tor 20140320 added to be able to exclude field if record owned by class in list (format ;classname;classname;... Example: ;GACompany;GAProject;)
                fd.HideIfOwnerClass = FieldDefinitionRow["TextFree3"].ToString();
            if (FieldDefinitionRow["HideIfFormType"] != DBNull.Value)
                fd.HideIfFormType = FieldDefinitionRow["HideIfFormType"].ToString();
            if (FieldDefinitionRow["HideInExcel"] != DBNull.Value)
                fd.HideInExcel = (bool)FieldDefinitionRow["HideInExcel"];

            if (FieldDefinitionRow["DisplayInFilterListOnly"] != DBNull.Value)
                fd.DisplayInFilterListOnly = (bool)FieldDefinitionRow["DisplayInFilterListOnly"];

			fd.ShowInSmallList = (bool) FieldDefinitionRow["ShowInSmallList"];
			fd.RequiredField = (bool) FieldDefinitionRow["RequiredField"];
			if (FieldDefinitionRow["IsReadOnly"] != DBNull.Value)
				fd.IsReadOnly = (bool) FieldDefinitionRow["IsReadOnly"];
			fd.CopyFromFieldId = FieldDefinitionRow["CopyFromFieldId"].ToString();
			fd.CompareOperator = FieldDefinitionRow["CompareOperator"].ToString();
			fd.CompareToField = FieldDefinitionRow["CompareTo"].ToString();
			fd.DependsOnField = FieldDefinitionRow["DependsOnField"].ToString();
			fd.DependantField = FieldDefinitionRow["DependantField"].ToString();
			fd.FilterOperator = FieldDefinitionRow["FilterOperator"].ToString();
			fd.FilterCondition = FieldDefinitionRow["FilterCondition"].ToString();
            if (FieldDefinitionRow["Sortorder"] != DBNull.Value)
                fd.sortOrder = (int)FieldDefinitionRow["Sortorder"];
            if (FieldDefinitionRow["AscDesc"] != DBNull.Value)
                fd.sortAscDesc = FieldDefinitionRow["AscDesc"].ToString().Trim();
            if (FieldDefinitionRow["PSecurityClass"] != DBNull.Value)
                fd.ListAggregat = FieldDefinitionRow["PSecurityClass"].ToString().Trim();

            // Tor 20141208 added fields ReportOnFieldUpdate and ReportTo. If ReportOnFieldUpdate=true, attribute change to be stored in GARecordHistoryLog, if ReportTo=True, report to role or person
            if (FieldDefinitionRow["SwitchFree3"] != DBNull.Value)
                fd.ReportOnFieldUpdate = (bool)FieldDefinitionRow["SwitchFree3"];
            if (FieldDefinitionRow["IntFree1"] != DBNull.Value && (fd.LookupTable=="GAPersonnel" || fd.ListCategory=="ER" ))
                fd.ReportUpdateTo = true;
            
            inheritFrom = FieldDefinitionRow["InheritFrom"].ToString();
			if (null!=inheritFrom && inheritFrom.Length>0)
			{
				DataRow row = _fieldDatatypes.Tables[0].Rows.Find(inheritFrom);
				if (null!=row)
				{
					if (0==fd.CaptionWidth.Length)
						fd.CaptionWidth = row["CaptionWidth"].ToString();
					if (0==fd.ControlType.Length)
						fd.ControlType = row["ControlType"].ToString();
					if (0==fd.ControlWidth.Length)
						fd.ControlWidth = row["ControlWidth"].ToString();
					if (0==fd.Dataformat.Length)
						fd.Dataformat = row["DataFormat"].ToString();
				}
			}

            ////hack correct datalength is the control is a text area with an ntext
            //asuming this if control type = textarea and length is 16/2=8

            if (fd.ControlType.ToUpper().Trim() == "TEXTAREA" && fd.DataLength == 8) fd.DataLength = -1;

			return fd;
		}
	}

	[Serializable]
	public class FieldDescription : object, IComparable
	{
		
		public FieldDescription ParentFieldDescription;
		
		public string TableId;
		public string FieldId;
		public string DataType;
		public string CaptionWidth;
		public string ControlType;
		public string ControlWidth;
		public int ControlHeight = 0;   
		public int DataLength; //COL_LENGTH() of database column
		public string LookupTable;
		public string LookupTableKey;
		public string LookupTableDisplayValue;
		public string LookupFilter;
		public string CodeTableName;
		public string ListCategory;
		
		public string Dataformat;
		public int ColumnOrder;
		public bool HideInSummary;
		public bool HideInDetail;
		public bool HideInNew;
        public bool HideInExcel = true;
        public bool DisplayInFilterListOnly = false;
		public bool ShowInSmallList;
		public bool RequiredField;
		public bool IsReadOnly = false;
        public int sortOrder = 0;
        public string sortAscDesc = string.Empty;
		public string CompareOperator;
		public string CompareToField;
		public string DependsOnField;
		public string DependantField;

		public string FilterOperator;
		public string FilterCondition;
        public string HideIfFormType = string.Empty;

		public string CopyFromFieldId;
        public string HideIfOwnerClass = string.Empty; // Tor 20140320 added to be able to exclude field if record owned by class in list (format ;classname;classname;... Example: ;GACompany;GAProject;)
        public string HideIfUserHasRole = string.Empty; // Tor 20140629 added to be able to exclude field if current user has RoleId in list (format ;role;role;... Example: ;3714;3715;) 
        public string ListAggregat = string.Empty; // Tor 20141208 implemented by Jan Ove. Aggregate function on field. 
        public bool ReportOnFieldUpdate = false; // Tor 20141208 if true, field attribute changes are to be reported.
        public bool ReportUpdateTo = false; // Tor 20141208 if true, field attribute contains id of person/role to be reported to.

		public string CssClass = String.Empty;

		public ArrayList GetLookupTableDisplayColumns()
		{
			ArrayList columnList = new ArrayList();
			if (LookupTableDisplayValue == string.Empty || LookupTableDisplayValue == null)
				return columnList;   //no columns in value return empty array;
			
			if (LookupTableDisplayValue.Trim().IndexOf(" ")>-1)
			{
				foreach (String columnName in LookupTableDisplayValue.Trim().Split(new char[] {' '}))
				{
					columnList.Add(columnName);
				}
			}
			else
			{
				columnList.Add(LookupTableDisplayValue);
			}
			return columnList;
		}

		public bool hasLookupFilter() 
		{
			return LookupFilter != string.Empty;
		}

		#region IComparable Members

		public System.Int32 CompareTo(object obj)
		{
			if ( obj == null) return 1;
			if ( !(obj is FieldDescription) )
				throw new ArgumentException();
			// TODO:  Add FieldDescription.System.IComparable.CompareTo implementation
			return this.ColumnOrder.CompareTo(((FieldDescription)obj).ColumnOrder);
		}

		#endregion
	}
}
