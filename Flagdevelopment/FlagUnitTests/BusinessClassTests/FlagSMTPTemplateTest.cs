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
    public class FlagSMTPTemplateTest
    {
        ProcedureSMTPTemplate smtpTemplate;
		
		[SetUp]
		public void SetUp() 
		{
            smtpTemplate = new ProcedureSMTPTemplate(3947, "SimpleTemplate");
		}


        [Test]
        public void GetTemplateText()
        {
            System.Console.WriteLine(smtpTemplate.getTemplateText());
        }

        [Test]
        public void GetExpandedText()
        {
            System.Console.WriteLine(smtpTemplate.getExpandedText());
        }
    }
}
