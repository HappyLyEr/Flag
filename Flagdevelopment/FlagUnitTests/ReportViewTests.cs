using System;
using GASystem.BusinessLayer;
using GASystem.DataModel;

namespace GATests
{
	using NUnit.Framework;

	[TestFixture]
	public class ReportViewTests
	{
		GADataRecord owner;
		
		public ReportViewTests()
		{
		}
	
		[SetUp]
		public void SetUp() 
		{
			owner = new GADataRecord(13, GADataClass.GACompany);
		}

		[Test]
		public void LoadAllReportViewRecordsByOwner() 
		{
			GASystem.DataModel.View.ReportView emptyDS = new GASystem.DataModel.View.ReportView();
			foreach (GADataClass dataClass in Enum.GetValues(typeof(GADataClass))) 
			{
				//if (emptyDS.Tables.Contains(dataClass.ToString())) 
				//{ //only tests against tables that are in the reportview
                Console.WriteLine(dataClass.ToString() + "hei");
                Console.WriteLine("ConnString:" + System.Configuration.ConfigurationManager.AppSettings.Get("ConnectionString"));
					
					System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetRecordSetAllDetailsForDataClassWithinOwner(dataClass, owner, string.Empty);
					GASystem.DataModel.View.ReportView rvds = new GASystem.DataModel.View.ReportView();
					rvds.Merge(ds);  //load the retrieved data into a reportview. similar to the exercise done in crystal reports
				//}
			}
		}

	}
}
