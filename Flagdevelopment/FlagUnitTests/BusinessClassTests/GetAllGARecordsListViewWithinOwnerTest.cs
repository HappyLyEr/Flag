using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using GASystem.BusinessLayer;
using GASystem.DataModel;

namespace FlagUnitTests
{
    /// <summary>
    /// Test getting all records within a owner. The data is returned in a listview format. Only !HideInSummary fields are returned from database. 
    /// In addtion is relevant galists data added to the dataset.
    /// </summary>
    [TestFixture]
    public class GetAllGARecordsListViewWithinOwnerTest
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
        //    System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GAAction);
        //    System.Data.DataSet ds = bc.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty);
        //}

        // 20140730
        //[Test]
        //public void GetAllGAActionRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GAAction, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGAActionTemplateRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GAActionTemplate, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGAAuditRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GAAudit, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGACertificateRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GACertificate, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGAClassRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GAClass, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGACompanyRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GACompany, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGAControlRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GAControl, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGACostRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GACost, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGACourseRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GACourse, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGACoursePersonListRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GACoursePersonList, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGACrewRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GACrew, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGACrewInProjectRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GACrewInProject, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGADailyEmployeeCountRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GADailyEmployeeCount, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGADamagedEquipmentRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GADamagedEquipment, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGADaysReportRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GADaysReport, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGADepartmentRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GADepartment, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGADocumentRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GADocument, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGAEmploymentRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GAEmployment, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGAEquipmentDamageReportRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GAEquipmentDamageReport, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }


        //[Test]
        //public void GetAllGAFileRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GAFile, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGAFileContentRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GAFileContent, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGAFileFolderRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GAFileFolder, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGAHazardIdentificationRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GAHazardIdentification, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGAHelpRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GAHelp, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGAIncidentReportRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GAIncidentReport, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }

        //[Test]
        //public void GetAllGAInjuredPartyRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GAInjuredParty, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGALicenseRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GALicense, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGALinkRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GALink, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }


        //[Test]
        //public void GetAllGALocationRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GALocation, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGALocationInCrewRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GALocationInCrew, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGAManageChangeRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GAManageChange, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGAMeansOfContactRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GAMeansOfContact, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGAMeetingRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GAMeeting, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGAMeetingPersonListRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GAMeetingPersonList, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGAMeetingTextRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GAMeetingText, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGANextOfKinRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GANextOfKin, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGAOpportunityRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GAOpportunity, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGAPersonnelRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GAPersonnel, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGAProcedureRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GAProcedure, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGAProcedureReferenceRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GAProcedureReference, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGAProjectRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GAProject, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGAReportRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GAReport, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGAReportInstanceRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GAReportInstance, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGAReportInstanceFilterRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GAReportInstanceFilter, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGAReportsRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GAReports, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGAResourceRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GAResource, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGAResourceSpecificationRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GAResourceSpecification, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGARiskRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GARisk, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGARiskControlRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GARiskControl, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }

        //[Test]
        //public void GetAllGArxtReportRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GArxtReport, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGASafetyObservationRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GASafetyObservation, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGAStoreAttributeRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GAStoreAttribute, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGAStoreObjectRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GAStoreObject, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGATaskRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GATask, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGATaskTemplateRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GATaskTemplate, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGATeamRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GATeam, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGATextItemRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GATextItem, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGATimeAndAttendanceRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GATimeAndAttendance, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        // [Test]
        //public void GetAllGAUserRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GAUser, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGAUsersRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GAUsers, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGAWorkflowRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GAWorkflow, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test]
        //public void GetAllGAWorkitemRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GAWorkitem, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test] // added by JOF 7/1/08
        //public void GetAllGALastLoginViewRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GALastLoginView, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        //[Test] // added by JOF 7/1/08
        //public void GetAllGACrewListViewRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GACrewListView, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
// end 20140730

        // code generation
//        select distinct s.MemberClass,'[Test] public void GetAll'+s.MemberClass+'RecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.'+s.MemberClass+', owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }' as Ccode
//from GASuperClassLinks s
//inner join GAClass c on c.Class=s.MemberClass
//where c.nTextFree2 is null or cast(c.nTextFree2 as nvarchar(4000))='' -- len(cast(c.nTextFree2 as nvarchar(4000)))<2
//order by s.MemberClass

        [Test]
        public void GetAllGAActionRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GAAction, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGAActionWorkitemViewRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GAActionWorkitemView, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGAAuditRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GAAudit, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGAClassRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GAClass, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGAClientFeedbackViewRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GAClientFeedbackView, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGACompanyRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GACompany, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGACostRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GACost, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGACourseRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GACourse, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGACoursePersonListViewRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GACoursePersonListView, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGACrewRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GACrew, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGACrewInProjectRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GACrewInProject, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGACrisisRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GACrisis, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGACrisisCheckListRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GACrisisCheckList, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGACrisisCheckListItemRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GACrisisCheckListItem, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGACrisisIssueRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GACrisisIssue, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGACrisisMessageLogRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GACrisisMessageLog, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGACrisisNewsBulletinRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GACrisisNewsBulletin, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGADamagedEquipmentRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GADamagedEquipment, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGADocumentRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GADocument, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGADrugAndAlcoholTestRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GADrugAndAlcoholTest, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGAEmploymentRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GAEmployment, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGAExposedHoursGroupViewRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GAExposedHoursGroupView, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGAFileRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GAFile, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGAFileContentRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GAFileContent, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGAFileFolderRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GAFileFolder, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGAFileViewRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GAFileView, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGAHazardIdentificationRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GAHazardIdentification, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGAHealthCertificateViewRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GAHealthCertificateView, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGAInjuredPartyRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GAInjuredParty, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGAListCategoryRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GAListCategory, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGAListsRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GALists, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGALocationRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GALocation, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGALocationCertificateViewRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GALocationCertificateView, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGALocationInCrewRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GALocationInCrew, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGAManageChangeRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GAManageChange, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGAMeansOfContactRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GAMeansOfContact, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGAMeetingRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GAMeeting, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGAMeetingPersonListRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GAMeetingPersonList, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGAMeetingTextRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GAMeetingText, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGANextOfKinRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GANextOfKin, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGANonConformanceViewRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GANonConformanceView, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGAOpportunityRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GAOpportunity, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGAPassportVisaViewRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GAPassportVisaView, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGAPermitToWorkRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GAPermitToWork, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGAPersonnelRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GAPersonnel, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGAProcedureRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GAProcedure, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGAProcedureTemplateRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GAProcedureTemplate, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGAProjectRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GAProject, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGARemedialActionViewRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GARemedialActionView, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGAReportRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GAReport, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGAReporterRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GAReporter, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGAReportInstanceRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GAReportInstance, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGAResourceRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GAResource, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGAResourceSpecificationRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GAResourceSpecification, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGAResourceUsageRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GAResourceUsage, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGARiskControlRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GARiskControl, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGARxtIssueViewRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GARxtIssueView, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGArxtReportRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GArxtReport, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGASafetyObservationRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GASafetyObservation, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGASubcontractorViewRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GASubcontractorView, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGATaskRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GATask, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGATaskTemplateRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GATaskTemplate, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGATeamRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GATeam, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGATextItemRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GATextItem, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGATrainingInstitutionViewRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GATrainingInstitutionView, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGAUserRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GAUser, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGAVendorRequestRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GAVendorRequest, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }
        [Test]
        public void GetAllGAWorkitemRecordsWithinOwner() { System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass.GAWorkitem, owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }

        
    }
}
