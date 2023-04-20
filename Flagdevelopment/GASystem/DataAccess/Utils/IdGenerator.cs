using System;
using GASystem.DataModel;
using GASystem.AppUtils;
using System.Data.SqlClient;

namespace GASystem.DataAccess.Utils
{
	/// <summary>
	/// Summary description for IdGenerator.
	/// </summary>
    public class IdGenerator
    {


        public IdGenerator()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public static int GetNextAutoNumber(string referenceIdConstructor, string Pattern, GADataClass DataClass, string RefIdField, GADataTransaction Transaction)
        {
            // Tor 20160707 changed method to get next autonumber from existing max number + 1
            // example referenceIdConstructor = "SOP-<%ownerprefix%>-<%shortyear%>/<%month2%>-<%autonumberyear%>";
            // find the max <%autonumberyear%> in referenceIds and add 1
            string autonumberyear = "<%autonumberyear%>";
            bool isLastElementAutonumberYear = referenceIdConstructor.EndsWith(autonumberyear, System.StringComparison.CurrentCultureIgnoreCase);
            int LastElementAutonumberYear = referenceIdConstructor.IndexOf(autonumberyear);
            string charactersPreceedingAutonumberYear = referenceIdConstructor.Substring(LastElementAutonumberYear - 2, 2);
            string elementSeparator = referenceIdConstructor.Substring(LastElementAutonumberYear - 1, 1);
            int number;
            bool isElementSeparatorNumeric = int.TryParse(elementSeparator, out number);
            if (isLastElementAutonumberYear && !isElementSeparatorNumeric || charactersPreceedingAutonumberYear != "%>")
            {
                // last element is autonumber and character before autonumber is not numeric
                // get max referenceids from database using pattern
                string sql = "select MAX({0}) from {1} where {0} like '{2}%'";
                sql = string.Format(sql, new object[] { RefIdField, DataClass.ToString(), Pattern });
                SqlConnection myConnection = DataUtils.GetConnection(Transaction); //new SqlConnection(DataUtils.getConnectionString());
                SqlCommand command = new SqlCommand(sql, myConnection, (SqlTransaction)Transaction.Transaction);
                string referenceId = command.ExecuteScalar().ToString();
                if (referenceId == string.Empty)
                {
                    return GetNextAutoNumber(Pattern, DataClass, RefIdField, Transaction);
                }
                else
                {

                    int first = referenceId.IndexOf(elementSeparator);
                    int last = referenceId.LastIndexOf(elementSeparator);
                    int i = 0;
                    int autonumber = 0;
                    string tall = string.Empty;
                    tall = referenceId.Substring(last + 1);

                    if (last > 0)
                    {
                        try
                        {
                            autonumber = Int32.Parse(tall);
                        }
                        catch
                        {
                            // not numeric, do count
                            return GetNextAutoNumber(Pattern, DataClass, RefIdField, Transaction);
                        }
                        finally
                        {
                            autonumber += 1;
                        }
                    }
                    return autonumber;

                }
            }
            else
            {
                // use count
                return GetNextAutoNumber(Pattern, DataClass, RefIdField, Transaction);
            }
            
        }




        public static int GetNextAutoNumber(string Pattern, GADataClass DataClass, string RefIdField, GADataTransaction Transaction)
        {
            string sql = "select count(*) from {0} where {1} like '{2}%'";
            //string RefIdField = "";
            //			switch (DataClass) 
            //			{
            //				case GADataClass.GAIncidentReport:
            //					RefIdField = "IncidentId";
            //					break;
            //				case GADataClass.GAHazardIdentification:
            //					RefIdField = "HazardReferenceId";
            //					break;
            //				case GADataClass.GASafetyObservation:
            //					RefIdField = "SafetyObservationReferenceId";
            //					break;
            //				case GADataClass.GAAction:
            //					RefIdField = "ActionReferenceId";
            //					break;
            //				case GADataClass.GAAudit:
            //					RefIdField = "AuditReferenceId";
            //					break;
            //				case GADataClass.GACourse:
            //					RefIdField = "CourseReferenceId";
            //					break;
            //				case GADataClass.GADailyEmployeeCount:
            //					RefIdField = "DailyEmployeeCountReferenceId";
            //					break;
            //				case GADataClass.GAEquipmentDamageReport:
            //					RefIdField = "ReferenceId";
            //					break;
            //				case GADataClass.GAMeeting:
            //					RefIdField = "MeetingReferenceId";
            //					break;
            //				case GADataClass.GAProject:
            //					RefIdField = "ProjectReferenceId";
            //					break;
            //			
            //			}

            sql = string.Format(sql, new object[] { DataClass.ToString(), RefIdField, Pattern });
            SqlConnection myConnection = DataUtils.GetConnection(Transaction); //new SqlConnection(DataUtils.getConnectionString());
            SqlCommand command = new SqlCommand(sql, myConnection, (SqlTransaction)Transaction.Transaction);

            int autoNumber = (int)command.ExecuteScalar() + 1;

            return autoNumber;
        }
    }
}
