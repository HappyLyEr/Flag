using System;
using System.Collections.Generic;
using System.Text;

namespace FlagUnitTests
{
    class Program
    {
        public static void Main(String[] Args)
        {
            Console.WriteLine("hei");
            Console.WriteLine("ConnString:" + System.Configuration.ConfigurationManager.AppSettings.Get("ConnectionString"));
            GATests.ReportViewTests rt = new GATests.ReportViewTests();
            rt.LoadAllReportViewRecordsByOwner();
        }
    }
}
