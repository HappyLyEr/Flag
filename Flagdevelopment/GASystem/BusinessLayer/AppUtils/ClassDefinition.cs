using System;
using GASystem.DataModel;
using GASystem.DataAccess;
using System.Collections;

namespace GASystem.AppUtils
{
	/// <summary>
	/// Class for loading and chaching GA Class Description.
	/// </summary>
	public class ClassDefinition
	{
		private static Hashtable map = new Hashtable();
		
		public ClassDefinition()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static ClassDescription GetClassDescriptionByGADataClass(GADataClass DataClass) 
		{
			//todo make this threadsafe
			if (null==map[DataClass.ToString()])
			{
				ClassDescription cd = GetClassDescriptionByGADataClass(DataClass.ToString());
				lock(map) 
				{
					map[DataClass.ToString()] = cd;
				}
			}
			return (ClassDescription) map[DataClass.ToString()];
			//return GetClassDescriptionByGADataClass(DataClass.ToString());
		}

		/// <summary>
		/// Get a Class Description by GADataClass. Checks if the definition exists in the 
		/// database. if not returns a default description.
		/// </summary>
		/// <param name="DataClass"></param>
		/// <returns></returns>
		public static ClassDescription GetClassDescriptionByGADataClass(string DataClassName)
		{
            // Tor 20140515 replaced with below to conform DataAccess class names with standard <class>DS: ClassDS ds = Class.GetClassByGADataClass(DataClassName);
            ClassDS ds = ClassDb.GetClassByGADataClass(DataClassName);

			ClassDescription cd = new ClassDescription();
			//cd.DataClass = DataClass;
			cd.DataClassName = DataClassName;
			if (ds.GAClass.Rows.Count > 0)
			{
				// fill in values from database;
                if (!ds.GAClass[0].IsCommentNull()) cd.Comment = ds.GAClass[0].Comment;
                //else cd.Comment = string.Empty;
                cd.HasReport = ds.GAClass[0].IsHasReportNull() ? false : ds.GAClass[0].HasReport;
                cd.IsActive = ds.GAClass[0].IsIsActiveNull() ? false : ds.GAClass[0].IsActive;
                cd.IsTop = ds.GAClass[0].IsIsTopNull() ? false : ds.GAClass[0].IsTop;
                if (!ds.GAClass[0].IsApplyAdditionalAccessControlNull()) cd.ApplyAdditionalAccessControl = ds.GAClass[0].ApplyAdditionalAccessControl;
                //else cd.ApplyAdditionalAccessControl = false;

				if (!ds.GAClass[0].IsNameOfReferenceIdNull()) cd.NameOfReferenceId = ds.GAClass[0].NameOfReferenceId;
				//else cd.NameOfReferenceId = string.Empty;
				if (!ds.GAClass[0].IsRefIdConstructorNull()) cd.RefIdConstructor = ds.GAClass[0].RefIdConstructor;
                if (!ds.GAClass[0].IsDateFieldNull()) cd.DateField = ds.GAClass[0].DateField;
				if (!ds.GAClass[0].IsDateFromFieldNull()) cd.DateFromField = ds.GAClass[0].DateFromField;
				if (!ds.GAClass[0].IsDateToFieldNull()) cd.DateToField = ds.GAClass[0].DateToField;
				if (!ds.GAClass[0].IsObjectNameNull()) cd.ObjectName = ds.GAClass[0].ObjectName;

				if (!ds.GAClass[0].IsReadRolesNull()) cd.ReadRoles = ds.GAClass[0].ReadRoles;
				if (!ds.GAClass[0].IsUpdateRolesNull()) cd.UpdateRoles = ds.GAClass[0].UpdateRoles;
				if (!ds.GAClass[0].IsCreateRolesNull()) cd.CreateRoles = ds.GAClass[0].CreateRoles;
				if (!ds.GAClass[0].IsDeleteRolesNull()) cd.DeleteRoles = ds.GAClass[0].DeleteRoles;

				if (!ds.GAClass[0].IsManyToManyFieldNull()) cd.ManyToManyField = ds.GAClass[0].ManyToManyField;
				if (!ds.GAClass[0].IsIsViewNull()) cd.IsView = ds.GAClass[0].IsView;
				if (!ds.GAClass[0].IsTextFree1Null()) cd.FilterShortCutField = ds.GAClass[0].TextFree1;
                if (!ds.GAClass[0].IsTextFree3Null()) cd.FormTypeField = ds.GAClass[0].TextFree3;
                if (!ds.GAClass[0].IsVirtualClassAttributeNameNull()) cd.VirtualClassAttributeName = ds.GAClass[0].VirtualClassAttributeName;
                if (!ds.GAClass[0].IsnTextFree1Null()) cd.HelpText = ds.GAClass[0].nTextFree1;
                if (!ds.GAClass[0].IsnTextFree2Null()) cd.ViewSQL = ds.GAClass[0].nTextFree2;
                // Tor 20150108 TextFree2 will be used for other purpose
                //if (!ds.GAClass[0].IsTextFree2Null())
                //    cd.NameOfReferenceIdPrefix = ds.GAClass[0].NameOfReferenceIdPrefix;
                if (!ds.GAClass[0].IsNameOfReferenceIdPrefixNull()) cd.NameOfReferenceIdPrefix = ds.GAClass[0].NameOfReferenceIdPrefix;
                // Tor 20150108 for GARecordHistoryLog reporting
                if (!ds.GAClass[0].IsCaption_nb_NoNull()) cd.ReportRecordChangesToRole = ds.GAClass[0].Caption_nb_No;
                if (!ds.GAClass[0].IsSwitchFree3Null()) cd.ReportUpdatesToRecordHistoryLog = ds.GAClass[0].SwitchFree3;
                // Tor 20150730 for LookupfieldMultiple - when true, possible to add several records
                if (!ds.GAClass[0].IsIntFree2Null() && ds.GAClass[0].IntFree2 == 1) cd.isLookupfieldMultipleClass = true;
                // JOF 20151114 add Vertical field name to class description
                if (!ds.GAClass[0].IsnTextFree3Null()) cd.VerticalFieldName = ds.GAClass[0].nTextFree3.ToString();
                // Tor 20170106 set value to IsUseAsHomeClass
                cd.IsUseAsHomeClass = ds.GAClass[0].IsSwitchFree2Null() ? false : ds.GAClass[0].SwitchFree2;

                cd.RowId = ds.GAClass[0].ClassRowId;
                //if (!ds.GAClass[0].IsTextFree3Null())
                //    cd.ProcedureURLText = ds.GAClass[0].TextFree3;
                cd.RemoveFromDateRestrict = ds.GAClass[0].IsIntFree3Null() == false && ds.GAClass[0].IntFree3 == 1;
			}
			return cd;
		}
	}


}

