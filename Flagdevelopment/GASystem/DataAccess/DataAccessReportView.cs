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
    public class DataAccessReportView : IDataAccessReportView
	{
		private  string _selectSqlTemplate = @"SELECT * FROM {0} ";
		private  string _selectSql = null;
		private GADataClass _dataClass;
		private GADataTransaction _transaction = null;
		private GASecurityDb_new _security = null;

        public DataAccessReportView(GADataClass DataClass, GADataTransaction Transaction)
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



        //DataSet IDataAccessReportView.GetByOwnerAndTimeSpan(GADataRecord owner, System.DateTime startDate, System.DateTime endDate, string filter) 
        //{
        //    DataSet GAData = GetDataSetIntance();
        //    string selectSqlOwner = SQLGenerateUtils.GenerateSelectFromFieldDefinition(_dataClass, owner.RowId, owner.DataClass);
        //    if (filter != string.Empty)
        //        selectSqlOwner = selectSqlOwner + " and " + filter;
            
        //    //set date filter            
        //    GASystem.AppUtils.ClassDescription cd = GASystem.AppUtils.ClassDefinition.GetClassDescriptionByGADataClass(_dataClass);
        //    if (cd.hasDateFromField() && cd.hasDateToField()) 
        //    {
        //        string sqlfilter = DataUtils.GenerateDateSpanFilter(cd.DateFromField, cd.DateToField);
        //        selectSqlOwner += " and " + sqlfilter;
        //    }


        //    String sql = _security.AppendSecurityFilterQueryForRead(selectSqlOwner);
			
        //    //adjust from and to date. SqlServer dates must be between 1753 and 9999
        //    if (startDate.Year < 1754)
        //        startDate = new DateTime(1754, 1, 1);

        //    if (endDate.Year > 9999) 
        //        endDate = new DateTime(9999,1,1);
			

        //    SqlConnection myConnection = DataUtils.GetConnection(_transaction);
        //    SqlDataAdapter da = new SqlDataAdapter(sql, myConnection);
        //    da.SelectCommand.Parameters.AddWithValue("@dateFrom", startDate);
        //    da.SelectCommand.Parameters.AddWithValue("@DateTo", endDate);

        //    if (_transaction != null)
        //        da.SelectCommand.Transaction = (SqlTransaction) _transaction.Transaction;
        //    da.Fill(GAData, _dataClass.ToString());
        //    return GAData;
        //}


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


            // Tor 20140819 Check with ownerclass and memberclass (use GASuperClassLinks instead of GAClass permissions)
            String sql = _security.AppendSecurityFilterQueryForRead(selectSqlOwner);
            //String sql = _security.AppendSecurityFilterQueryForRead(selectSqlOwner, _dataClass, owner.DataClass);
            
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
            DataSet GAData = GetDataSetIntance();
            string selectSql = GASystem.DataAccess.SQLGenerateUtils.GenerateSelectFromFieldDefinition(new GADataRecord(rowId, this._dataClass));
            String sql = _security.AppendSecurityFilterQueryForRead(selectSql);

            SqlConnection myConnection = DataUtils.GetConnection(_transaction);
            SqlDataAdapter da = new SqlDataAdapter(sql, myConnection);
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
