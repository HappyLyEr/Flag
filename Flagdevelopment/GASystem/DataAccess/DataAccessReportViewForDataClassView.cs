using System;
using System.Data;
using System.Data.SqlClient;
using GASystem.DataAccess.Security;
using GASystem.GAExceptions;
using GASystem.DataModel;
using System.Collections;

namespace GASystem.DataAccess
{
	/// <summary>
	/// This class contains the basic dataaccess methods needed in order to List data for Flag listviews. Use DataAccessListViewFactory to instanciate this class with correct dataclass
	/// </summary>
    public class DataAccessReportViewForDataClassView : IDataAccessReportView
	{
		private  string _selectSqlTemplate = @"SELECT * FROM {0} ";
		private  string _selectSql = null;
		private GADataClass _dataClass;
		private GADataTransaction _transaction = null;
		private GASecurityDb_new _security = null;

        public DataAccessReportViewForDataClassView(GADataClass DataClass, GADataTransaction Transaction)
		{
			_dataClass = DataClass;
			_transaction = Transaction;
			_selectSql = string.Format(_selectSqlTemplate, DataClass.ToString());
			_security = new GASecurityDb_new(DataClass, Transaction);
		}


        DataSet IDataAccessReportView.GetAll()
		{
			DataSet GAData = GetDataSetIntance();	
			SqlConnection myConnection = DataUtils.GetConnection(_transaction);

            //TODO get top owner from configuration file
            _selectSql = SQLGenerateUtils.GenerateSelectAllWithinFromFieldDefinition(_dataClass, 1, GADataClass.GAFlag);

			_selectSql = _security.AppendSecurityFilterQueryForRead(_selectSql);

			SqlDataAdapter da = new SqlDataAdapter(_selectSql, myConnection);
			da.Fill(GAData, _dataClass.ToString());
			return GAData;
		}


        /// <summary>
        /// Get all records of specified dataclass by owner
        /// </summary>
        /// <param name="OwnerRowId"></param>
        /// <param name="OwnerDataClass"></param>
        /// <returns></returns>
        DataSet IDataAccessReportView.GetByOwner(GADataRecord owner, string filter)
		{
			DataSet GAData = GetDataSetIntance();
            string selectSqlOwner = SQLGenerateUtils.GenerateSelectFromFieldDefinition(_dataClass, owner.RowId, owner.DataClass);
            if (filter != string.Empty)
                selectSqlOwner = selectSqlOwner + " and " + filter;
			SqlConnection myConnection = DataUtils.GetConnection(_transaction);

		    String sql = _security.AppendSecurityFilterQueryForRead(selectSqlOwner);
			
			
			SqlDataAdapter da = new SqlDataAdapter(sql , myConnection);
			da.Fill(GAData, _dataClass.ToString());
			return GAData;
		}





		/// <summary>
		/// Get all records within the owner record specified
		/// </summary>
		/// <param name="Owner"></param>
		/// <param name="Filter"></param>
		/// <param name="DateFrom"></param>
		/// <param name="DateTo"></param>
		/// <returns></returns>
        DataSet IDataAccessReportView.GetRecordsWithinOwner(GADataRecord owner, string filter, System.DateTime startDate, System.DateTime endDate) 
		{
            DataSet GAData = GetDataSetIntance();
            string selectSqlOwner = SQLGenerateUtils.GenerateSelectAllWithinFromFieldDefinition(_dataClass, owner.RowId, owner.DataClass);
            if (filter != string.Empty)
                selectSqlOwner = selectSqlOwner + " and " + filter;

            //set date filter            
            GASystem.AppUtils.ClassDescription cd = GASystem.AppUtils.ClassDefinition.GetClassDescriptionByGADataClass(_dataClass);
            if (cd.hasDateFromField() && cd.hasDateToField())
            {
                string sqlfilter = DataUtils.GenerateDateSpanFilter(cd.DataClassName + "." + cd.DateFromField, cd.DataClassName + "." + cd.DateToField);
                selectSqlOwner += " and " + sqlfilter;
            }


            if (cd.hasDateField())
            {
                string sqlfilter = DataUtils.GenerateDateFilter(cd);
                selectSqlOwner += " and " + sqlfilter;
            }




            String sql = _security.AppendSecurityFilterQueryForRead(selectSqlOwner);

            //adjust from and to date. SqlServer dates must be between 1753 and 9999
            if (startDate.Year < 1754)
                startDate = new DateTime(1754, 1, 1);

            if (endDate.Year > 9999)
                endDate = new DateTime(9999, 1, 1);


            SqlConnection myConnection = DataUtils.GetConnection(_transaction);
            SqlDataAdapter da = new SqlDataAdapter(sql, myConnection);
            da.SelectCommand.Parameters.AddWithValue("@dateFrom", startDate);
            da.SelectCommand.Parameters.AddWithValue("@DateTo", endDate);

            if (_transaction != null)
                da.SelectCommand.Transaction = (SqlTransaction)_transaction.Transaction;
            da.Fill(GAData, _dataClass.ToString());
            return GAData;
		}



        DataSet IDataAccessReportView.GetByRowId(int rowId)
        {
            GASystem.AppUtils.ClassDescription cd = GASystem.AppUtils.ClassDefinition.GetClassDescriptionByGADataClass(_dataClass);
           
            string selectSqlOwner = SQLGenerateUtils.GenerateSelectListViewFromFieldDefinition(GADataRecord.ParseGADataClass(cd.DataClassName));
           
            string subQuery = " (" + cd.ViewSQL + ") as " + cd.DataClassName + " ";

            selectSqlOwner = selectSqlOwner.Replace("from  ", "from ");
            selectSqlOwner = selectSqlOwner.Replace("from " + cd.DataClassName, " from " + subQuery);

            
            //string sqlview = "select * from {0} where path like '%{1}-{2}/%'";

            //string sqlview = selectSqlOwner + " AND (gasuperclass.ReadRoles = '{0}-{1}'"
            //                                + " OR gasuperclass.UpdateRoles = '{0}-{1}'"
            //                                + " OR gasuperclass.CreateRoles = '{0}-{1}'"
            //                                + " OR gasuperclass.DeleteRoles = '{0}-{1}'"
            //                                + " OR gasuperclass.TextFree1 = '{0}-{1}'"
            //                                + " OR gasuperclass.TextFree2 = '{0}-{1}')";
            //and replaced again with the statement below 

            //string sqlview = selectSqlOwner + " and path like '%{0}-{1}/%'"; //has been changed with statement above

            
             selectSqlOwner += " and " + cd.DataClassName.Substring(2) + "RowId = " + rowId.ToString();
            
            
            
            
            
            
            
            
            DataSet GAData = GetDataSetIntance();
            //string selectSql = GASystem.DataAccess.SQLGenerateUtils.GenerateSelectFromFieldDefinition(new GADataRecord(rowId, this._dataClass));
            //String sql = _security.AppendSecurityFilterQueryForRead(selectSql);

            SqlConnection myConnection = DataUtils.GetConnection(_transaction);
            SqlDataAdapter da = new SqlDataAdapter(selectSqlOwner, myConnection);
            da.Fill(GAData, _dataClass.ToString());
            return GAData;





        }





        DataSet IDataAccessReportView.GetByRowIds(ArrayList rowIds)
        {
            DataSet GAData = GetDataSetIntance();

            string selectSql = GASystem.DataAccess.SQLGenerateUtils.GenerateSelectFromFieldDefinition(_dataClass);
            selectSql = selectSql + " and " + _dataClass.ToString().Substring(2) + "rowid in (" + GASystem.AppUtils.GAUtils.ConvertArrayToString(rowIds.ToArray(), ",") + ")";
            String sql = _security.AppendSecurityFilterQueryForRead(selectSql);

            SqlConnection myConnection = DataUtils.GetConnection(_transaction);
            SqlDataAdapter da = new SqlDataAdapter(sql, myConnection);
            da.Fill(GAData, _dataClass.ToString());
            return GAData;
        }


		private DataSet GetDataSetIntance()
		{
            return new DataSet();
		}


        
    }
}
