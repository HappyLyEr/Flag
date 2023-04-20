using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] referenceIds = 
                {
                    "SOP-OEU-16/07-147",
                    "SOP-U-16/07-14",
                    "S-OEU-16/07-161111",
                };
            string referenceIdConstructor = "SOP-<%ownerprefix%>-<%shortyear%>/<%month2%>-<%autonumberyear%>";
            // find the highest <%autonumberyear%> in referenceIds and add 1
            string autonumberyear="<%autonumberyear%>";
            bool isLastElementAutonumberYear=referenceIdConstructor.EndsWith(autonumberyear, System.StringComparison.CurrentCultureIgnoreCase);
            int LastElementAutonumberYear=referenceIdConstructor.IndexOf(autonumberyear);
            string charactersPreceedingAutonumberYear = referenceIdConstructor.Substring(LastElementAutonumberYear - 2, 2);
            string elementSeparator = referenceIdConstructor.Substring(LastElementAutonumberYear - 1, 1);
            int b;
            bool isElementSeparatorNumeric = int.TryParse(elementSeparator, out b);
            int max=0;
            if (isLastElementAutonumberYear && !isElementSeparatorNumeric || charactersPreceedingAutonumberYear != "%>")
                {
                // last element is autonumber and character before autonumber is not numeric
                foreach (string str in referenceIds)
                {
                    bool test1 = str.StartsWith("sop"); //case sensitive
                    bool test2 = str.StartsWith("sop", System.StringComparison.CurrentCultureIgnoreCase);
                    bool test3 = str.EndsWith("148", System.StringComparison.CurrentCultureIgnoreCase);
                    int first = str.IndexOf(elementSeparator);
                    int last = str.LastIndexOf(elementSeparator);
                    int i=0;
                    string tall = string.Empty;
                    tall = str.Substring(last + 1);

                    if (last > 0)
                    {
                        try
                        {
                            i = Int32.Parse(tall);
                        }
                        catch
                        {
                            // do count
                            // break/return or something
                        }
                        finally
                        {
                            if (i > max) max = i;
                        }

                    }
                }
                max += 1;
            }
            else
            { 
            // use count
            }


            System.Console.ReadKey();

        }
    }
}
