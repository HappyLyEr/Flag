using System;
using GASystem.DataModel;
using System.Data.SqlClient;
using System.Data;

namespace GASystem.DataAccess
{
	/// <summary>
	/// Summary description for EmploymentDb.
	/// </summary>
	public class EmploymentDb
	{

		//private static SqlConnection myConnection;
		private static string _selectSql = @"SELECT * FROM GAEmployment";
		
		public EmploymentDb()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static EmploymentDS GetAllEmployments()
		{

			EmploymentDS EmploymentData = new EmploymentDS();	
			SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
			
			SqlDataAdapter da = new SqlDataAdapter(_selectSql, myConnection);
			da.Fill(EmploymentData, "GAEmployment");
		
			return EmploymentData;
		}

		public static EmploymentDS GetEmploymentByEmploymentRowId(int EmploymentRowId)
		{
			String appendSql = " WHERE EmploymentRowId="+EmploymentRowId;
			EmploymentDS EmploymentData = new EmploymentDS();
            SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
		
			SqlDataAdapter da = new SqlDataAdapter(_selectSql+appendSql, myConnection);
			da.Fill(EmploymentData, "GAEmployment");
			return EmploymentData;
		}
		
		public static EmploymentDS GetEmploymentByPersonnelIdOwnerAndStartDate(int PersonnelId, GADataRecord owner, DateTime startDate)    //, DateTime endDate)
		{
			EmploymentDS EmploymentData = new EmploymentDS();	
			string selectSqlOwner = SQLGenerateUtils.GetSelectSqlMembers(GADataClass.GAEmployment, owner.RowId, owner.DataClass);
			selectSqlOwner = selectSqlOwner + " and personnel = " + PersonnelId.ToString()  + " and " + GetDateWhereStatement();
			
			SqlCommand command = new SqlCommand(selectSqlOwner);
			
			SetDateParameters(startDate, command);
			
			SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
			command.Connection = myConnection;
			
			SqlDataAdapter da = new SqlDataAdapter(command);
			da.Fill(EmploymentData, GADataClass.GAEmployment.ToString());
			return EmploymentData;
		}
		

		public static EmploymentDS GetEmploymentsByOwner(int OwnerRowId, GADataClass OwnerDataClass )
		{
			EmploymentDS EmploymentData = new EmploymentDS();	
			string selectSqlOwner = SQLGenerateUtils.GetSelectSqlMembers(GADataClass.GAEmployment, OwnerRowId, OwnerDataClass);
			
			SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
			SqlDataAdapter da = new SqlDataAdapter(selectSqlOwner , myConnection);
			da.Fill(EmploymentData, GADataClass.GAEmployment.ToString());
			return EmploymentData;
		}

		public static EmploymentDS GetEmploymentsByOwnerAndDate(int OwnerRowId, GADataClass OwnerDataClass, System.DateTime EmploymentDate )
		{
			EmploymentDS EmploymentData = new EmploymentDS();	
			string selectSqlOwner = SQLGenerateUtils.GetSelectSqlMembers(GADataClass.GAEmployment, OwnerRowId, OwnerDataClass);
			selectSqlOwner = selectSqlOwner + " and " + GetDateWhereStatement();
			
			SqlCommand command = new SqlCommand(selectSqlOwner);
			
			SetDateParameters(EmploymentDate, command);
			
			SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
			command.Connection = myConnection;
			
			SqlDataAdapter da = new SqlDataAdapter(command);
			da.Fill(EmploymentData, GADataClass.GAEmployment.ToString());
			return EmploymentData;
		}

		private static string GetDateWhereStatement() 
		{
			string where = "(_formDateField_ <= @empdateFrom and (_toDateField_ >= @empdateTo or _toDateField_ is null ))";
			GASystem.AppUtils.ClassDescription cd = GASystem.AppUtils.ClassDefinition.GetClassDescriptionByGADataClass(GADataClass.GAEmployment);
			where = where.Replace("_formDateField_", cd.DateFromField);
			where = where.Replace("_toDateField_", cd.DateToField);
			
			return where;
		}

		private static void SetDateParameters(System.DateTime SingelDate, SqlCommand command) 
		{
            // Tor 20150221 Changed fromdate to time 0 and end date to tim 23,59,59,0
            //System.DateTime fromDate = SingelDate.Date + new TimeSpan(0, 23,59,59,0);
            //System.DateTime toDate = SingelDate.Date + new TimeSpan(0, 0,0,0,0);//include all of the day in the less than test, set check time to the start of the day
            System.DateTime toDate = SingelDate.Date + new TimeSpan(0, 23, 59, 59, 0); //include all of the day in the less than test, set check time to the start of the day
            System.DateTime fromDate = SingelDate.Date + new TimeSpan(0, 0, 0, 0, 0);

            command.Parameters.AddWithValue("@empdateFrom", fromDate);
            command.Parameters.AddWithValue("@empdateTo", toDate);
			
		}

		public static EmploymentDS GetEmploymentsByPersonnelIdAndDate(int PersonnelId, System.DateTime EmploymentDate) 
		{
			EmploymentDS EmploymentData = new EmploymentDS();	
			//string selectSql = SQLGenerateUtils.GenerateSelectFromFieldDefinition(GADataClass.GAEmployment);
			string selectSql = _selectSql;
			//todo check agains field def to get field linked to GAPersonnel
			selectSql = selectSql + " where " + GetDateWhereStatement() +  " and personnel = " + PersonnelId;
			
			SqlCommand command = new SqlCommand(selectSql);
			
			SetDateParameters(EmploymentDate, command);

			SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
			command.Connection = myConnection;
			
			SqlDataAdapter da = new SqlDataAdapter(command);
			da.Fill(EmploymentData, GADataClass.GAEmployment.ToString());
			return EmploymentData;
		}

        /// <summary>
        /// Get all employments by roleid for the given date
        /// </summary>
        /// <param name="PersonnelId"></param>
        /// <param name="EmploymentDate"></param>
        /// <returns></returns>
        public static EmploymentDS GetEmploymentsByRoleIdAndDate(int RoleId, System.DateTime EmploymentDate)
        {
            return GetEmploymentsByRoleIdOrJobTitleAndDate(RoleId,EmploymentDate,"RoleListsRowId");
        }

        // Tor 20170325 Added common method for getting Job Title and Access Role
        public static EmploymentDS GetEmploymentsByRoleIdOrJobTitleAndDate(int JobOrRoleId, System.DateTime EmploymentDate, string FieldName)
        {
            EmploymentDS EmploymentData = new EmploymentDS();
            //string selectSql = SQLGenerateUtils.GenerateSelectFromFieldDefinition(GADataClass.GAEmployment);
            string selectSql = _selectSql;
            //todo check agains field def to get field linked to GAPersonnel
            // Tor 20170325 Job Role moved from RoleListsRowId to JobDescription
            //selectSql = selectSql + " where " + GetDateWhereStatement() + " and RoleListsRowId = " + RoleId;
            selectSql = selectSql + " where " + GetDateWhereStatement() + " and " + FieldName + " = " + JobOrRoleId.ToString();

            SqlCommand command = new SqlCommand(selectSql);

            SetDateParameters(EmploymentDate, command);

            SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
            command.Connection = myConnection;

            SqlDataAdapter da = new SqlDataAdapter(command);
            da.Fill(EmploymentData, GADataClass.GAEmployment.ToString());
            return EmploymentData;
        }



		public static EmploymentDS GetEmploymentsByOwnerDateAndRoleId(int OwnerRowId, GADataClass OwnerDataClass, System.DateTime EmploymentDate, int RoleId, GADataTransaction Transaction ) 
		{
            return GetEmploymentsByOwnerDateAndRoleIdOrJobTitle(OwnerRowId, OwnerDataClass, EmploymentDate, RoleId, "RoleListsRowId", Transaction);
		}

        public static EmploymentDS GetEmploymentsByOwnerDateAndJobTitle(int OwnerRowId, GADataClass OwnerDataClass, System.DateTime EmploymentDate, int RoleId, GADataTransaction Transaction)
        {
            return GetEmploymentsByOwnerDateAndRoleIdOrJobTitle(OwnerRowId, OwnerDataClass, EmploymentDate, RoleId, "JobDescription", Transaction);
        }

        public static EmploymentDS GetEmploymentsByOwnerDateAndRoleIdOrJobTitle(int OwnerRowId, GADataClass OwnerDataClass, System.DateTime EmploymentDate, int RoleId, string FieldId, GADataTransaction Transaction)
        {
            EmploymentDS EmploymentData = new EmploymentDS();
            string selectSqlOwner = SQLGenerateUtils.GetSelectSqlMembers(GADataClass.GAEmployment, OwnerRowId, OwnerDataClass);
//            selectSqlOwner = selectSqlOwner + " and " + GetDateWhereStatement() + "and RoleListsRowId = " + RoleId;
            selectSqlOwner = selectSqlOwner + " and " + GetDateWhereStatement() + "and "+FieldId+" = " + RoleId;

            SqlCommand command = new SqlCommand(selectSqlOwner);

            SetDateParameters(EmploymentDate, command);

            SqlConnection myConnection = DataUtils.GetConnection(Transaction); // new SqlConnection(DataUtils.getConnectionString());
            command.Connection = myConnection;
            if (Transaction != null)
                command.Transaction = (SqlTransaction)Transaction.Transaction;

            SqlDataAdapter da = new SqlDataAdapter(command);
            da.Fill(EmploymentData, GADataClass.GAEmployment.ToString());
            return EmploymentData;
        }

//        // Tor 20190227 Get distinct all tableid and fieldid above memberrecord where fieldname = inputvalue
//        public static System.Collections.ArrayList GetDistinctJobTitleOrRoleAboveMember(GADataRecord fromMember, int PersonnelRowId, string RoleOrTitle)
//        {
//            // check if valid FindInClass, FindInField, GADateRecord.DataClass, searchFieldName
//            System.Collections.ArrayList result = new System.Collections.ArrayList();
//            GADataClass dc = GADataRecord.ParseGADataClass(fromMember.DataClass.ToString());
//            if (dc == null) return result;
//            if (fromMember.RowId == null || fromMember.RowId < 1) return result;

//            System.Collections.Generic.List<GASystem.DataModel.GADataRecord> foundOwnerRecords = GASystem.BusinessLayer.DataClassRelations.GetCurrentParentLevelDataRecords(fromMember);
//            if (foundOwnerRecords == null) return result;

//            String whereStatement = string.Empty;
//            foreach (GASystem.DataModel.GADataRecord foundOwner in foundOwnerRecords)
//            {
//                whereStatement = whereStatement + " (s.OwnerClass='" + foundOwner.DataClass.ToString()
//                    + "' and s.OwnerClassRowId=" + foundOwner.RowId.ToString() + ") or ";
//            }

//            if (whereStatement != string.Empty)
//                whereStatement = "( " + whereStatement.Substring(0, whereStatement.Length - 3) + " )"; // remove laste OR

//            string searchField = string.Empty;
//            if (RoleOrTitle.ToLower() == "role") searchField = "RoleListsRowId";
//            else
//                if (RoleOrTitle.ToLower() == "title") searchField = "JobDescription";
//                else return result;

//            string selectDistinct = @"
//select distinct e.{0} 
//from GAEmployment e 
//inner join GASuperClass s on s.MemberClass='GAEmployment' and s.MemberClassRowId=e.EmploymentRowId and ( {1} ) 
//where (e.Personnel = {2} and (e.FromDate<=GETUTCDATE() and (e.ToDate>=GETUTCDATE() or e.ToDate is null))) ";

//            string sql = string.Format(selectDistinct, searchField, whereStatement, PersonnelRowId);

//            SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
//            //            System.Collections.ArrayList DNNUsersList = new System.Collections.ArrayList();
//            try
//            {
//                myConnection.Open();
//                SqlCommand myCommand = new SqlCommand(sql, myConnection);
//                SqlDataReader reader = myCommand.ExecuteReader();
//                while (reader.Read())
//                {
//                    if (reader != null) result.Add(reader);
//                }
//            }
//            catch
//            { }
//            finally
//            {
//                myConnection.Close();
//            }
//            return result;
//        }


        // Tor 20190227 Get distinct all tableid and fieldid above memberrecord where fieldname = inputvalue
        public static string GetDistinctJobTitleOrRoleAboveMember(GADataRecord fromMember, int PersonnelRowId, string RoleOrTitle)
        {
            // check if valid FindInClass, FindInField, GADateRecord.DataClass, searchFieldName
            string result = string.Empty;
            GADataClass dc = GADataRecord.ParseGADataClass(fromMember.DataClass.ToString());
            if (dc == null) return result;
            if (fromMember.RowId == null || fromMember.RowId < 1) return result;

            System.Collections.Generic.List<GASystem.DataModel.GADataRecord> foundOwnerRecords = GASystem.BusinessLayer.DataClassRelations.GetCurrentParentLevelDataRecords(fromMember);
            if (foundOwnerRecords == null) return result;

            String whereStatement = string.Empty;
            foreach (GASystem.DataModel.GADataRecord foundOwner in foundOwnerRecords)
            {
                whereStatement = whereStatement + " (s.OwnerClass='" + foundOwner.DataClass.ToString()
                    + "' and s.OwnerClassRowId=" + foundOwner.RowId.ToString() + ") or ";
            }

            if (whereStatement != string.Empty)
                whereStatement = "( " + whereStatement.Substring(0, whereStatement.Length - 3) + " )"; // remove laste OR

            string searchField = string.Empty;
            if (RoleOrTitle.ToLower() == "role") searchField = "RoleListsRowId";
            else
                if (RoleOrTitle.ToLower() == "title") searchField = "JobDescription";
                else return result;

            string selectDistinct = @"
select distinct e.{0} 
from GAEmployment e 
inner join GASuperClass s on s.MemberClass='GAEmployment' and s.MemberClassRowId=e.EmploymentRowId and ( {1} ) 
where (e.Personnel = {2} and (e.FromDate<=GETUTCDATE() and (e.ToDate>=GETUTCDATE() or e.ToDate is null))) ";

            string sql = string.Format(selectDistinct, searchField, whereStatement, PersonnelRowId);

            SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
            //            System.Collections.ArrayList DNNUsersList = new System.Collections.ArrayList();
            try
            {
                myConnection.Open();
                SqlCommand myCommand = new SqlCommand(sql, myConnection);
                SqlDataReader reader = myCommand.ExecuteReader();
                while (reader.Read())
                {
                    if (reader != null) result = result + reader.GetInt32(0).ToString() + ";";
                    //if (reader != null) result = result + reader.ToString() + ";";
                }
            }
            catch
            { }
            finally
            {
                myConnection.Close();
            }
            if (result != string.Empty) result = ";" + result;
            return result;
        }

        public static bool IsCurrentEmploymentByPersonnelAndDate(int PersonnelRowId, System.DateTime EmploymentDate, GADataTransaction Transaction)
        {
            EmploymentDS EmploymentData = new EmploymentDS();
            string selectSQL = _selectSql + " and " + GetDateWhereStatement();

            SqlCommand command = new SqlCommand(selectSQL);

            SetDateParameters(EmploymentDate, command);

            SqlConnection myConnection = DataUtils.GetConnection(Transaction); // new SqlConnection(DataUtils.getConnectionString());
            command.Connection = myConnection;
            if (Transaction != null)
                command.Transaction = (SqlTransaction)Transaction.Transaction;

            SqlDataAdapter da = new SqlDataAdapter(command);
            da.Fill(EmploymentData, GADataClass.GAEmployment.ToString());
            if (EmploymentData != null) return true;
            return false;
        }

        public static EmploymentDS SearchEmploymentsByOwnerRecordsDateAndRoleId
            (System.Collections.Generic.List<GADataRecord> owner, System.DateTime EmploymentDate, int roleId, GADataTransaction Transaction)
        {
            return SearchEmploymentsByOwnerRecordsDateAndRoleIdOrJobTitle(owner, EmploymentDate, roleId, "RoleListsRowId", Transaction);
        }

        public static EmploymentDS SearchEmploymentsByOwnerRecordsDateAndJobTitle
            (System.Collections.Generic.List<GADataRecord> owner, System.DateTime EmploymentDate, int roleId, GADataTransaction Transaction)
        {
            return SearchEmploymentsByOwnerRecordsDateAndRoleIdOrJobTitle(owner, EmploymentDate, roleId, "JobDescription", Transaction);
        }

        public static EmploymentDS SearchEmploymentsByOwnerRecordsDateAndRoleIdOrJobTitle
            (System.Collections.Generic.List<GADataRecord> owner, System.DateTime EmploymentDate, int roleId, string FieldName,GADataTransaction Transaction)
        {
            EmploymentDS EmploymentData = new EmploymentDS();
            string sql=string.Empty;
            if (FieldName == "RoleListsRowId")
                sql = SQLGenerateUtils.SearchEmploymentsByOwnerRecordsDateAndRoleId(owner, "GAEmployment", System.DateTime.Now, roleId);
            else
                if (FieldName == "JobDescription")
                    sql = SQLGenerateUtils.SearchEmploymentsByOwnerRecordsDateAndJobTitle(owner, "GAEmployment", System.DateTime.Now, roleId);
                else
                    return EmploymentData;

//            string sql = SQLGenerateUtils.SearchEmploymentsByOwnerRecordsDateAndRoleId(owner, "GAEmployment", System.DateTime.UtcNow, roleId);

            SqlCommand command = new SqlCommand(sql);

            //            SetDateParameters(EmploymentDate, command);

            SqlConnection myConnection = DataUtils.GetConnection(Transaction); // new SqlConnection(DataUtils.getConnectionString());
            command.Connection = myConnection;
            if (Transaction != null)
                command.Transaction = (SqlTransaction)Transaction.Transaction;

            SqlDataAdapter da = new SqlDataAdapter(command);
            da.Fill(EmploymentData, GADataClass.GAEmployment.ToString());
            return EmploymentData;
        }

		
		public static EmploymentDS UpdateEmployment(EmploymentDS EmploymentSet, GADataTransaction Transaction)
		{
			SqlConnection myConnection = DataUtils.GetConnection(Transaction);
			SqlDataAdapter da = new SqlDataAdapter(_selectSql, myConnection);
			da.SelectCommand.Transaction = (SqlTransaction) Transaction.Transaction;
			da.SelectCommand.Connection = myConnection;
			SqlCommandBuilder  cb = new SqlCommandBuilder(da);
			da.RowUpdated += new SqlRowUpdatedEventHandler(DataUtils.DataAdapter_RowUpdated);
			

			da.Update(EmploymentSet, GADataClass.GAEmployment.ToString());
			return EmploymentSet;
		}
	}
}
