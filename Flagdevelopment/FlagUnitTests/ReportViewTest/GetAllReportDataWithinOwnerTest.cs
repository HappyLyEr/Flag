using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using GASystem.BusinessLayer;
using GASystem.DataModel;
using System.Data;
using GASystem.DataAccess;

namespace FlagUnitTests
{
    /// <summary>
    /// Test getting all records within a owner. All details for the record is returned in a untyped dataset. Joins are
    /// created to get listvalues and lookups.
    /// </summary>
    [TestFixture]
    public class GetAllReportDataWithinOwnerTest
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
        [Test]  // added by JOF 7/1/08
        public void GetAllGACrewListViewRecordsWithinOwner() { BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GACrewListView); System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty); }

        [Test]
        public void GetAllGAWorkitemPathViewForReport()
        {
            DataSet lds = new DataSet();
            ReportBuilder rb = new ReportBuilder();
            GADataClass reportDataClass = GADataRecord.ParseGADataClass("GAWorkitemPathView");
            //rb.GenerateDateFilter(ReportInstanceId, reportDataClass);

            //report alias and table names are equal, just add the table as it is defined in ga
            GADataRecord ReportContext = new GADataRecord(14, GADataClass.GALocation);
            lds = rb.FindAllMembers(ReportContext, reportDataClass);


           // lds = GASystem.DataAccess.ReportView.GetRecordSetAllDetails("GAWorkitemPathView", string.Empty);
            System.Console.WriteLine("Number of rows in workitempathview: " + lds.Tables[0].Rows.Count);
		
        }

    }
}
