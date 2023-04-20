using System;
using GASystem.DataModel;


namespace GASystem.AppUtils
{
	/// <summary>
	/// GA Class Description. Holds Class level info for GADataClasses. 
	/// This info is used both in business rules and in displaying information.
	/// </summary>
	public class ClassDescription
	{
		//default values;
		//public GADataClass DataClass;
		public string DataClassName = string.Empty;
		public string Comment = string.Empty;
		public bool HasReport = false;
		public string RefIdConstructor = string.Empty;
		public bool IsActive = true;
		public bool IsTop = false;
		public bool ApplyAdditionalAccessControl = false;
		public string NameOfReferenceId = string.Empty;
        public string NameOfReferenceIdPrefix = string.Empty;
		public string DateField = string.Empty;
		public string DateFromField = string.Empty;
		public string DateToField = string.Empty;
		public string ObjectName = string.Empty;
		public string ReadRoles = string.Empty;
		public string UpdateRoles = string.Empty;
		public string CreateRoles = string.Empty;
		public string DeleteRoles = string.Empty;
		public string ManyToManyField = string.Empty;
		public bool IsView = false;
		public string FilterShortCutField = string.Empty;
        public string FormTypeField = string.Empty;
		public GADataRecord ClassTemplateRootNode = null;
        public string VirtualClassAttributeName = string.Empty;
        public string HelpText = string.Empty;
        public int RowId;
        //public string ProcedureURL = string.Empty;
        //public string ProcedureURLText = string.Empty;
        public string ViewSQL = string.Empty;
        // Tor 20150108 Added fields for GARecordHistoryLog reporting
        public string ReportRecordChangesToRole = string.Empty;  // stored in GAClass.Caption_nb_No
        public bool ReportUpdatesToRecordHistoryLog = false;

		public ClassDescription()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public bool isClassManyToManyLink() 
		{
			return ManyToManyField != string.Empty;
		}

		public bool hasDateToField() 
		{
			return DateToField != string.Empty;
		}

		public bool hasDateFromField() 
		{
			return DateFromField != string.Empty;
		}

        public bool hasDateField()
        {
            return DateField != string.Empty;
        }

        public bool hasViewSQL()
        {
            return ViewSQL != string.Empty;
        }

        public bool hasVirtualClass()
        {
            return VirtualClassAttributeName != string.Empty;
        }

	}

}
