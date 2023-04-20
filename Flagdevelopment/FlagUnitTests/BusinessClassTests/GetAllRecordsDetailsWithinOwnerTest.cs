using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using GASystem.BusinessLayer;
using GASystem.DataModel;

namespace FlagUnitTests
{
    /// <summary>
    /// Test getting all records within a owner. All details for the record is returned in a untyped dataset. Joins are
    /// created to get listvalues and lookups.
    /// </summary>
    [TestFixture]
    public class GetAllRecordsDetailsWithinOwnerTest
    {
        GADataRecord owner;
		
		[SetUp]
		public void SetUp() 
		{
            owner = new GADataRecord(1, GADataClass.GAFlag);
		}


        //[Test]
        //public void GetAllActionRecordsWithinOwner()
        //{
        //    BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAAction);
        //    System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty);
        //}
        //[Test]
        //public void GetAllGAActivityRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAActivity); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGAActionRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAAction); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGAActionTemplateRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAActionTemplate); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGAAuditRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAAudit); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGACertificateRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GACertificate); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGAClassRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAClass); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGACompanyRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GACompany); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGAControlRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAControl); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGACostRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GACost); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGACourseRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GACourse); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGACoursePersonListRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GACoursePersonList); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGACrewRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GACrew); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGACrewInProjectRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GACrewInProject); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGADailyEmployeeCountRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GADailyEmployeeCount); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGADamagedEquipmentRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GADamagedEquipment); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGADaysReportRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GADaysReport); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGADepartmentRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GADepartment); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGADocumentRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GADocument); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGAEmploymentRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAEmployment); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGAEquipmentDamageReportRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAEquipmentDamageReport); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }


        //[Test]
        //public void GetAllGAFileRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAFile); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGAFileContentRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAFileContent); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGAFileFolderRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAFileFolder); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGAHazardIdentificationRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAHazardIdentification); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGAHelpRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAHelp); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGAIncidentReportRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAIncidentReport); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }

        //[Test]
        //public void GetAllGAInjuredPartyRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAInjuredParty); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGALicenseRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GALicense); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGALinkRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GALink); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }


        //[Test]
        //public void GetAllGALocationRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GALocation); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGALocationInCrewRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GALocationInCrew); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGAManageChangeRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAManageChange); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGAMeansOfContactRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAMeansOfContact); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGAMeetingRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAMeeting); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGAMeetingPersonListRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAMeetingPersonList); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGAMeetingTextRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAMeetingText); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGANextOfKinRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GANextOfKin); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGAOpportunityRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAOpportunity); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGAPersonnelRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAPersonnel); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGAProcedureRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAProcedure); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGAProcedureReferenceRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAProcedureReference); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGAProjectRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAProject); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGAReportRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAReport); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        ////[Test]
        ////public void GetAllGAReporterRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAReporter); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGAReportInstanceRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAReportInstance); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGAReportInstanceFilterRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAReportInstanceFilter); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGAReportsRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAReports); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGAResourceRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAResource); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGAResourceSpecificationRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAResourceSpecification); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGARiskRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GARisk); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGARiskControlRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GARiskControl); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }

        //[Test]
        //public void GetAllGArxtReportRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GArxtReport); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGASafetyObservationRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GASafetyObservation); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        ////[Test]
        ////public void GetAllGASafetyStatisticsRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GASafetyStatistics); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGAStoreAttributeRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAStoreAttribute); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGAStoreObjectRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAStoreObject); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGATaskRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GATask); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGATaskTemplateRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GATaskTemplate); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGATeamRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GATeam); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGATextItemRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GATextItem); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGATimeAndAttendanceRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GATimeAndAttendance); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        ////[Test]
        ////public void GetAllGASuggestionRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GASuggestion); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGAUserRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAUser); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGAUsersRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAUsers); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGAWorkflowRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAWorkflow); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGAWorkitemRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAWorkitem); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test] // Tor 20070915 : added 
        //public void GetAllGAExposedHoursGroupViewRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAExposedHoursGroupView); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test] // jof 20080107 : added 
        //public void GetAllGALastLoginViewRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GALastLoginView); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]  // jof 20080107 : added 
        //public void GetAllGACrewListViewRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GACrewListView); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }

        // herfra

        // code generation script
//        select distinct s.MemberClass,'[Test] public void GetAll'+s.MemberClass+'RecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.'+s.MemberClass+'); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }' as Ccode
//from GASuperClassLinks s
//inner join GAClass c on c.Class=s.MemberClass
//where c.nTextFree2 is null or cast(c.nTextFree2 as nvarchar(4000))='' -- len(cast(c.nTextFree2 as nvarchar(4000)))<2
//order by s.MemberClass

        [Test]
        public void GetAllGAActionRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAAction); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGAActionWorkitemViewRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAActionWorkitemView); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGAAuditRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAAudit); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGAClassRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAClass); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGAClientFeedbackViewRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAClientFeedbackView); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGACompanyRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GACompany); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGACostRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GACost); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGACourseRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GACourse); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGACoursePersonListViewRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GACoursePersonListView); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGACrewRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GACrew); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGACrewInProjectRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GACrewInProject); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGACrisisRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GACrisis); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGACrisisCheckListRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GACrisisCheckList); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGACrisisCheckListItemRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GACrisisCheckListItem); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGACrisisIssueRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GACrisisIssue); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGACrisisMessageLogRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GACrisisMessageLog); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGACrisisNewsBulletinRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GACrisisNewsBulletin); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGADamagedEquipmentRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GADamagedEquipment); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGADocumentRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GADocument); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGADrugAndAlcoholTestRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GADrugAndAlcoholTest); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGAEmploymentRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAEmployment); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGAExposedHoursGroupViewRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAExposedHoursGroupView); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGAFileRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAFile); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGAFileContentRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAFileContent); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGAFileFolderRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAFileFolder); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGAFileViewRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAFileView); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGAHazardIdentificationRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAHazardIdentification); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGAHealthCertificateViewRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAHealthCertificateView); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGAInjuredPartyRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAInjuredParty); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGAListCategoryRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAListCategory); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGAListsRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GALists); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGALocationRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GALocation); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGALocationCertificateViewRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GALocationCertificateView); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGALocationInCrewRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GALocationInCrew); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGAManageChangeRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAManageChange); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGAMeansOfContactRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAMeansOfContact); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGAMeetingRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAMeeting); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGAMeetingPersonListRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAMeetingPersonList); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGAMeetingTextRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAMeetingText); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGANextOfKinRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GANextOfKin); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGANonConformanceViewRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GANonConformanceView); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGAOpportunityRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAOpportunity); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGAPassportVisaViewRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAPassportVisaView); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGAPermitToWorkRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAPermitToWork); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGAPersonnelRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAPersonnel); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGAProcedureRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAProcedure); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGAProcedureTemplateRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAProcedureTemplate); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGAProjectRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAProject); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGARemedialActionViewRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GARemedialActionView); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGAReportRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAReport); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGAReporterRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAReporter); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGAReportInstanceRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAReportInstance); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGAResourceRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAResource); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGAResourceSpecificationRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAResourceSpecification); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGAResourceUsageRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAResourceUsage); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGARiskControlRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GARiskControl); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGARxtIssueViewRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GARxtIssueView); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGArxtReportRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GArxtReport); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGASafetyObservationRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GASafetyObservation); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGASubcontractorViewRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GASubcontractorView); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGATaskRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GATask); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGATaskTemplateRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GATaskTemplate); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGATeamRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GATeam); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGATextItemRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GATextItem); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGATrainingInstitutionViewRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GATrainingInstitutionView); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGAUserRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAUser); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGAVendorRequestRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAVendorRequest); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGAWorkitemRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAWorkitem); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }


    }
}
