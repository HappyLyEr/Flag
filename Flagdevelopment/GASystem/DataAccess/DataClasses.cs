using System;
using System.Data;
using System.Collections.Generic;

namespace GASystem.DataModel
{

	public enum GADataClass {
        //GAActivity                // Tor 20070813 : removed 
		GAAction, 
		GAActionTemplate,
        GAActionWorkitemView,       // added by jof 22/8/2008, used for displaying workitems for a given class
        GAAnalysis,                 // added by jof 14/01/2008, used for analysis services
        GAAudit, 
		GACertificate,
        GACertificateExpirationView,            // Tor 20111013 : added view for reporting assigned expiring personnel certificates from project,crew,location, not in GASystem.DataModel
        GAClass,
        GAClassAndAttributeSelectionWithSort,   // Tor 20090529 : added view for personal filters, column selection and sort
        GAClassAttributeSelection,              // Tor 20090529 : added view for personal filters, column selection and sort
        GAClassFilter,                          // Tor 20090529 : added view Tor 20090529 : added view for personal filters, column selection and sort
        GACompany, 
		GAControl, 
		GACost, 
		GACourse, 
		GACoursePersonList,
        GACoursePersonListView,                 // Tor 20110824 : added view for GACoursePersonlist including attachment/file reference
        GACrew,
        GACrewListView,	            // used for crewlist report to US Coast Guard
        GACrewInProject, 
        
        GACrisis,
        GACrisisCheckList,
        GACrisisCheckListItem,
        GACrisisIssue,
        GACrisisMessageLog, 
        GACrisisNewsBulletin,

        GAClientFeedbackView,       // Tor 20120609 : added on request from Phil Bigg/Caroline Hall
		GADailyEmployeeCount, 
		GADamagedEquipment, 
		GADaysReport, 
		GADepartment, 
		GADocument,
        GADrugAndAlcoholTest,       // Tor 20140205 : added on request from Michael Caddick/Phil Bigg
        GAEmployeeSiteLogView,      // view holding employee site login information
        GAEmployment,
        GAEmploymentHierarchyView,  // Tor 20071122 : added 
        GAEmploymentPathView,       // jof 20090730 : added view for listing current useremployment with owner and path info
        GAEquipmentDamageReport,
        GAExposedTimeView,          // Tor 20071122 : added 
        GAExposedHoursGroupView,    // Tor 20070915 : added 
        GAFieldDefinitions, 
		GAFile,
        GAFileView,                 // Tor 20120228 : added for ApplyAdditionalAccessControl=1 classes (currently for GAPersonnel)
        GAFileContent, 
		GAFileFolder,
        GAFlag,
        GAFlagTask,
        GAHazardIdentification,
        GAHealthCertificateView,    // Tor 20110726 : added view for personell health certificates
        GAHelp, 
		GAIncidentReport,
        GAIncidentReportCountView,
        GAIncidentReportDailyEmployeeCountView, 
		GAInjuredParty, 
        GAIssue,
        GALastLoginDateTimeView,    // Tor 20080405 : added view for displaying last login date (based on date in aspnet_Users)
        GALastLoginView,            // Tor 20080405 : added view for displaying last login for employments at a location  
        GALicense, 
		GALink, 
		GAListCategory, 
		GALists, 
        GAListsSelected,            // JOF 20090417 : added , used for multipleselect
		GALocation,
        GALocationCertificateView,  // Tor 20120125 : added view for adding Location Certificate by shipowner representative (eg. master)
        GALocationInCrew,
		GAManageChange,
		GAMeansOfContact,
		GAMeeting, 
		GAMeetingPersonList, 
		GAMeetingText,
        GAMilestone,                // Tor 20081002 : added 
        GANextOfKin,
        GANonConformanceView,       // Tor 20120224 : added on request from Phil Bigg
		GAOpportunity,
        GAPassportVisaView,         // Tor 20110726 : added view for personell passports and visas
        GAPermitToWork,             // Tor 20140209 : added view for adding Permits to Work, replaces GAPermitToWorkView shich did not contain fields for embedded documents
        GAPermitToWorkView,         // Tor 20120125 : added view for adding Permits to Work
        GAPersonnel,
        GAPersonCertificateListView,// Tor 20110828 : added view for reporting assigned personnel's certificates, not in GASystem.DataModel
        GAPersonnelCertificateView, // Tor 20120130 : added view for viewing Personnel Certificate dates from GAPersonnel by path employment (isView)
        GAPersonnelCrewingListView, // Tor 20130118 : added view for subcontractor managementGAPersonnelRecordsView,                  // added by Tor 26/7/2011 views for personell records to be used in Personnel report - dataset defined in GAClass, not in GASystem.DataModel
        GAPersonnelHRDocumentView,  // Tor 20131015 : added on request from Andreia Almeida and Phil Bigg
        GAPersonTrainingRecordView, // Tor 20110726 : added view for personell training certificates - dataset defined in GAClass, not in GASystem.DataModel
        GAProcedure, 
		GAProcedureReference,
        GAProcedureTemplate,        // added by jof 16/12/2008, used for storing smtp templates releated to procedures
        GAProject, 
        GARemedialActionView,
        GAPurchaseOrder,            // Tor 20080812 : added 
        GAPurchaseOrderLine,        // Tor 20080812 : added added by Tor 080812
        GARepairIssueView,          // Tor 20071017 : added added by Tor 17/10/2007
        GAReport,
        GAReporter,                 // Tor 20070813 : removed, then  // Tor 20080708 : added 
		GAReportInstance, 
		GAReportInstanceFilter, 
		GAReports, 
		GAResource, 
		GAResourceSpecification,
        GAResourceUsage,            // Tor 20080814 : added 
        GARisk, 
		GARiskControl, 
		GARootClass,
        GARxtIssueView,             // Tor 20080903 : added 
        GArxtReport, 
		GASafetyObservation,
        // GASafetyStatistics,      // Tor 20070813 : removed 
        GAServiceDeskView,          // Tor 20080812 : added 
        GAStoreAttribute, 
		GAStoreObject,
        GASubcontractorView,        // Tor 20121119 : added view for subcontractor management
        // GASuggestion,            // Tor 20070813 : removed 
        GATask, 
		GATaskTemplate, 
		GATeam, 
        GATemplate,
		GATextItem, 
		GATimeAndAttendance,
        //GATrainingCertificateView,   // Tor 20110726 : added view for personell training certificates
        GATrainingInstitutionView,	// Tor 20110825 : added view for GATrainingInstitutionView
        GAUser, 
		GAUsers,
        GAVendorRequest,            // Tor 20130306 : added on request from Caroline Hall
        GAWorkflow, 
		GAWorkitem, 
		NullClass,
        GAWorkitemPathView
        
    }


    // Tor 20060309 : removed by tor 9.3.06 - added class GArxtReport public enum GADataClass {GAActivity, GAAction, GAActionTemplate, GAAudit, GACertificate, GAClass, GACompany, GAControl, GACost, GACourse, GACoursePersonList, GACrew, GACrewInProject, GADailyEmployeeCount, GADamagedEquipment, GADaysReport, GADepartment, GADocument, GAEmployment, GAEquipmentDamageReport, GAFieldDefinitions, GAFile, GAFileContent, GAFileFolder, GAHazardIdentification, GAHelp, GAIncidentReport, GAIncidentReportDailyEmployeeCountView, GAInjuredParty, GALicense, GALink, GAListCategory, GALists, GALocation, GALocationInCrew, GAMeansOfContact, GAMeeting, GAMeetingPersonList, GAMeetingText, GANextOfKin, GAOpportunity, GAPersonnel, GAProcedure, GAProcedureReference, GAProject, GAReport, GAReporter, GAReportInstance, GAReportInstanceFilter, GAReports, GAResource, GAResourceSpecification, GARisk, GARiskControl, GARootClass, GASafetyObservation, GASafetyStatistics, GAStoreAttribute, GAStoreObject, GATask, GATaskTemplate, GATeam, GATextItem, GATimeAndAttendance, GASuggestion, GAUser, GAUsers, GAWorkflow, GAWorkitem, NullClass}
	//public enum GADataClass {GAActivity, GAAction, GAAudit, GACertificate, GACompany, GACost, GACourse, GACoursePersonList, GADailyEmployeeCount, GADamagedEquipment, GADepartment, GADocument, GAEmployment, GAEquipmentDamageReport, GAFieldDefinitions, GAFile, GAFileFolder, GAHazardIdentification, GAHelp, GAIncidentReport, GAInjuredParty, GALink, GALocation, GAMeansOfContact, GAMeeting, GAMeetingPersonList, GAMeetingText, GANextOfKin, GAPersonnel, GAProcedure, GAProject, GAReport, GAReporter, GAReportInstance, GAReportInstanceFilter, GAReports, GAResource, GAResourceSpecification, GARootClass, GASafetyObservation, GASafetyStatistics, GAStoreAttribute, GAStoreObject, GATask, GATaskTemplate, GATeam, GATextItem, GATimeAndAttendance, GAUsers, GAWorkflow, GAUser, GALists, GAClass, GAWorkitem, GAListCategory, GAFileContent, NullClass}
			// dataclasses not included :
				//GAActionTemplate, 
				//
				//
				//GAProjectPartyChief,
				//GASuperClass, GASuperClassLinks,
				//GAUserContext,


	//enum of controls in gadatadefinition
	public enum GADataControl {
	
		Checkbox,
		Date,
		DateTime,
		DropdownList,
		FileContent,
		FileLookupField,
		FileMimeType,
		FileURL,
		GADataClass,
		HazardMatrix,
		LookupField,
		LookupFieldMultiple,
		Numeric,
		PersonnelPicker,
		ReportURL,
		TextArea,
		Textbox,
		URL,
		WorkflowStarted,
		YearMonthSpan
	}

	[Serializable]
    public class GADataRecord : IEquatable<GADataRecord>
	{

		public GADataRecord(int RowId, GADataClass DataClass)
		{
			
			_dataClass = DataClass;
			_rowId = RowId;
			
		}

		private GADataClass _dataClass;
		private int _rowId;


		public static GADataClass ParseGADataClass(String DataClassString)
		{
			GADataClass dataClassEnum = (GADataClass) Enum.Parse(typeof(GADataClass), DataClassString,true);
			return dataClassEnum;
		}
		
		/// <summary>
		/// parse a string of format {dataclass}-{rowid} into a gadatarecord
		/// </summary>
		/// <param name="DataRecord"></param>
		/// <returns></returns>
		public static GADataRecord ParseGADataRecord(string DataRecord) 
		{
			int indexOfSlash = DataRecord.IndexOf("-");
			
			GADataRecord dataRecord = null;
			if (indexOfSlash != -1) 
			{
				string dataClass;
				string rowid;
				dataClass = DataRecord.Substring(0, indexOfSlash);
				rowid = DataRecord.Substring(indexOfSlash + 1);
				dataRecord = new GADataRecord(int.Parse(rowid), ParseGADataClass(dataClass));
			}
			return dataRecord;
		}

		public static GADataRecord GetGADataRecordFromFirstRow(DataSet GADataSet, GADataClass DataSetDataClass)
		{
			if (GADataSet == null || GADataSet.Tables[0] == null || GADataSet.Tables[0].Rows.Count == 0)
				return null;
			GADataRecord returnedRecord = null;
			try
			{
				string rowIdColumnName = DataSetDataClass.ToString().Substring(2) + "RowId";
				int rowId = (int)GADataSet.Tables[0].Rows[0][rowIdColumnName];
				returnedRecord = new GADataRecord(rowId, DataSetDataClass);
			}
			catch (Exception ex)
			{
				throw new GAExceptions.GAException("GetGADataRecordFromFirstRow: Was not able to get DataRecord of type " + DataSetDataClass + " from first row in DataSet");
			}

			return returnedRecord;
		}
		public GADataClass DataClass
		{
			get 
			{
				return _dataClass;
			}
			set
			{
				_dataClass = value;
			}
		}

		public int RowId
		{
			get 
			{
				return _rowId;
			}
			set
			{
				_rowId = value;
			}
		}

 
        #region IEquatable<GADataRecord> Members

        public bool Equals(GADataRecord other)
        {
            return other.DataClass == this.DataClass && this.RowId == other.RowId;
        }

        #endregion
    }

	public class GADataRecordDate : GADataRecord 
	{
		private System.DateTime _startDate = new DateTime(1800,1,1); //startdate accepted by sql server;
		private System.DateTime _endDate = new DateTime(3000,1,1); //end date accepted by sql server;

		public GADataRecordDate(int RowId, GADataClass DataClass) : base(RowId, DataClass) {}
		

		public System.DateTime StartDate 
		{
			get {return _startDate;}
			set {_startDate = value;}
		}

		public System.DateTime EndDate 
		{
			get {return _endDate;}
			set {_endDate = value;}
		}
	}



}
