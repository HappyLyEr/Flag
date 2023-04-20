using System;
using GASystem.DataModel;
using System.Data.SqlClient;
using System.Data;

namespace GASystem.DataAccess
{
	/// <summary>
	/// Summary description for TimeAndAttendanceDb.
	/// </summary>
	public class TimeAndAttendanceDb
	{

		//private static SqlConnection myConnection;
		private static string _selectSql = @"SELECT * FROM GATimeAndAttendance";
		
		public TimeAndAttendanceDb()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static TimeAndAttendanceDS GetAllTimeAndAttendances()
		{

			TimeAndAttendanceDS TimeAndAttendanceData = new TimeAndAttendanceDS();	
			SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
			
			SqlDataAdapter da = new SqlDataAdapter(_selectSql, myConnection);
			da.Fill(TimeAndAttendanceData, "GATimeAndAttendance");
		
			return TimeAndAttendanceData;
		}

		public static TimeAndAttendanceDS GetTimeAndAttendanceByTimeAndAttendanceRowId(int TimeAndAttendanceRowId)
		{
			String appendSql = " WHERE TimeAndAttendanceRowId="+TimeAndAttendanceRowId;
			TimeAndAttendanceDS TimeAndAttendanceData = new TimeAndAttendanceDS();		
			SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
		
			SqlDataAdapter da = new SqlDataAdapter(_selectSql+appendSql, myConnection);
			da.Fill(TimeAndAttendanceData, "GATimeAndAttendance");
			return TimeAndAttendanceData;
		}

		public static TimeAndAttendanceDS GetTimeAndAttendancesByOwner(int OwnerRowId, GADataClass OwnerDataClass )
		{
			TimeAndAttendanceDS TimeAndAttendanceData = new TimeAndAttendanceDS();	
			string selectSqlOwner = SQLGenerateUtils.GetSelectSqlMembers(GADataClass.GATimeAndAttendance, OwnerRowId, OwnerDataClass);
			
			SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
			SqlDataAdapter da = new SqlDataAdapter(selectSqlOwner , myConnection);
			da.Fill(TimeAndAttendanceData, GADataClass.GATimeAndAttendance.ToString());
			return TimeAndAttendanceData;
		}

		public static TimeAndAttendanceDS UpdateTimeAndAttendance(TimeAndAttendanceDS TimeAndAttendanceSet, GADataTransaction Transaction)
		{
			SqlConnection myConnection = DataUtils.GetConnection(Transaction);
			SqlDataAdapter da = new SqlDataAdapter(_selectSql, myConnection);
			da.SelectCommand.Transaction = (SqlTransaction) Transaction.Transaction;
			da.SelectCommand.Connection = myConnection;
			SqlCommandBuilder  cb = new SqlCommandBuilder(da);
			da.RowUpdated += new SqlRowUpdatedEventHandler(DataUtils.DataAdapter_RowUpdated);
			
			da.Update(TimeAndAttendanceSet, GADataClass.GATimeAndAttendance.ToString());
			return TimeAndAttendanceSet;
		}





		public static TimeAndAttendanceDS GetTimeAndAttendancesByOwnerAndDate(int OwnerRowId, GADataClass OwnerDataClass, System.DateTime EmploymentDate )
		{
			
			TimeAndAttendanceDS TimeAndAttendanceData = new TimeAndAttendanceDS();	
			string selectSqlOwner = SQLGenerateUtils.GetSelectSqlMembers(GADataClass.GATimeAndAttendance, OwnerRowId, OwnerDataClass);
			selectSqlOwner = selectSqlOwner + " and " + GetDateWhereStatement();

			SqlCommand command = new SqlCommand(selectSqlOwner);
			SetDateParameters(EmploymentDate, command);

			SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
			command.Connection = myConnection;

			SqlDataAdapter da = new SqlDataAdapter(command);
			da.Fill(TimeAndAttendanceData, GADataClass.GATimeAndAttendance.ToString());
			return TimeAndAttendanceData;
		}

		public static TimeAndAttendanceDS GetTimeAndAttendancesByOwnerAndDate(int OwnerRowId, GADataClass OwnerDataClass, System.DateTime StartDate, System.DateTime EndDate  )
		{
			
			TimeAndAttendanceDS TimeAndAttendanceData = new TimeAndAttendanceDS();	
			string selectSqlOwner = SQLGenerateUtils.GetSelectSqlMembers(GADataClass.GATimeAndAttendance, OwnerRowId, OwnerDataClass);
			selectSqlOwner = selectSqlOwner + " and " + GetDateWhereStatementPeriode();

			SqlCommand command = new SqlCommand(selectSqlOwner);
			SetDateParameters(StartDate, EndDate, command);

			SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
			command.Connection = myConnection;

			SqlDataAdapter da = new SqlDataAdapter(command);
			da.Fill(TimeAndAttendanceData, GADataClass.GATimeAndAttendance.ToString());
			return TimeAndAttendanceData;
		}



		private static string GetDateWhereStatement() 
		{
			string where = "(_formDateField_ <= @empdateFrom and (_toDateField_ >= @empdateTo or _toDateField_ is null ))";
			GASystem.AppUtils.ClassDescription cd = GASystem.AppUtils.ClassDefinition.GetClassDescriptionByGADataClass(GADataClass.GATimeAndAttendance);
			where = where.Replace("_formDateField_", cd.DateFromField);
			where = where.Replace("_toDateField_", cd.DateToField);
			
			return where;
		}

		private static string GetDateWhereStatementPeriode() 
		{
			string where = "(_formDateField_ <= @empdateTo and (_toDateField_ >= @empdateFrom or _toDateField_ is null ))";
			GASystem.AppUtils.ClassDescription cd = GASystem.AppUtils.ClassDefinition.GetClassDescriptionByGADataClass(GADataClass.GATimeAndAttendance);
			where = where.Replace("_formDateField_", cd.DateFromField);
			where = where.Replace("_toDateField_", cd.DateToField);
			
			return where;
		}



		private static void SetDateParameters(System.DateTime SingelDate, SqlCommand command) 
		{
			System.DateTime fromDate = SingelDate.Date + new TimeSpan(0, 23,59,59,0);
			System.DateTime toDate = SingelDate.Date + new TimeSpan(0, 23,59,59,0);  //must be employed at end of day to be included in the count
			
			//System.DateTime toDate = SingelDate.Date + new TimeSpan(0, 0,0,0,0);//include all of the day in the less than test, set check time to the start of the day

			command.Parameters.AddWithValue("@empdateFrom",fromDate) ;
            command.Parameters.AddWithValue("@empdateTo", toDate);
			
		}

		private static void SetDateParameters(System.DateTime StartDate, System.DateTime EndDate, SqlCommand command) 
		{
			System.DateTime fromDate = StartDate.Date + new TimeSpan(0, 0,0,0,0);
			System.DateTime toDate = EndDate.Date + new TimeSpan(0, 23,59,59,0);  //must be employed at end of day to be included in the count
			
			//System.DateTime toDate = SingelDate.Date + new TimeSpan(0, 0,0,0,0);//include all of the day in the less than test, set check time to the start of the day

            command.Parameters.AddWithValue("@empdateFrom", fromDate);
            command.Parameters.AddWithValue("@empdateTo", toDate);
			
		}


	}
}
