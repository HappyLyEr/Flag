using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using GASystem.BusinessLayer;
using GASystem.DataModel;
using GASystem.AppUtils.DateRangeGenerator;

namespace FlagUnitTests
{
    /// <summary>
    /// Test getting all records within a owner. Data is returned in a typed dataset
    /// </summary>
    [TestFixture]
    public class TestDateGenerator
    {
        // Tor 201611 Security 20161122 GADataRecord owner;



        [Test]
        public void DateGeneratorCurrentWeek()
        {
            
            AbstractDateRange testRange = new GASystem.AppUtils.DateRangeGenerator.CurrentWeekDateRange();
            System.Console.WriteLine("curret start of week is: " + testRange.GetDateFrom().DayOfWeek.ToString());
            System.Console.WriteLine("curret end of week is: " + testRange.GetDateTo().DayOfWeek.ToString());
           
            if (testRange.GetDateFrom().DayOfWeek.ToString() != "Monday")
                throw new Exception("wrong start of week:" + testRange.GetDateFrom().DayOfWeek.ToString());
            if (testRange.GetDateTo().DayOfWeek.ToString() != "Sunday")
                throw new Exception("wrong end of week:" + testRange.GetDateFrom().DayOfWeek.ToString());
        }

        [Test]
        public void DateGeneratorLastWeek()
        {
            AbstractDateRange testRange = new GASystem.AppUtils.DateRangeGenerator.LastWeekDateRange();
            System.Console.WriteLine("curret start of last week is: " + testRange.GetDateFrom().DayOfWeek.ToString());
            System.Console.WriteLine("curret end of week is: " + testRange.GetDateTo().DayOfWeek.ToString());
           
            if (testRange.GetDateFrom().DayOfWeek.ToString() != "Monday")
                throw new Exception("wrong start of last week:" + testRange.GetDateFrom().DayOfWeek.ToString());
            if (testRange.GetDateTo().DayOfWeek.ToString() != "Sunday")
                throw new Exception("wrong end of week:" + testRange.GetDateFrom().DayOfWeek.ToString());
        }
    }
}
