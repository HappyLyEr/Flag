using System;
using System.Data;
using System.Data.SqlClient;
using GASystem.DataAccess.Security;
using GASystem.GAExceptions;
using GASystem.DataModel;

namespace GASystem.DataAccess
{
	/// <summary>
	/// This class contains the basic dataaccess methods needed in order to List data for Flag listviews. Use DataAccessListViewFactory to instanciate this class with correct dataclass
	/// </summary>
    public class DataAccessListViewForViews : IDataAccessListView
	{
		private  string _selectSqlTemplate = @"SELECT * FROM {0} ";
		private  string _selectSql = null;
		private GADataClass _dataClass;
		private GADataTransaction _transaction = null;
		private GASecurityDb_new _security = null;

        public DataAccessListViewForViews(GADataClass DataClass, GADataTransaction Transaction)
		{
			_dataClass = DataClass;
			_transaction = Transaction;
			_selectSql = string.Format(_selectSqlTemplate, DataClass.ToString());
			_security = new GASecurityDb_new(DataClass, Transaction);
		}


        DataSet IDataAccessListView.GetAll()
		{
			DataSet GAData = GetDataSetIntance();	
			SqlConnection myConnection = DataUtils.GetConnection(_transaction);

            //TODO get top owner from configuration file
            _selectSql = "select * from " + _dataClass.ToString();
			SqlDataAdapter da = new SqlDataAdapter(_selectSql, myConnection);
			da.Fill(GAData, _dataClass.ToString());
			return GAData;
		}

        
        DataSet IDataAccessListView.GetByDataRecord(GADataRecord dataRecord)
		{
			DataSet GAData = GetDataSetIntance();	
			SqlConnection myConnection = DataUtils.GetConnection(_transaction);

            //TODO get top owner from configuration file
            _selectSql = "select * from " + _dataClass.ToString();

            _selectSql += " where " + dataRecord.DataClass.ToString().Substring(2) + "rowid = " + dataRecord.RowId.ToString() + " "; 
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
        DataSet IDataAccessListView.GetByOwner(GADataRecord owner, string filter)
		{
			DataSet GAData = GetDataSetIntance();
           
            AppUtils.ClassDescription cd = AppUtils.ClassDefinition.GetClassDescriptionByGADataClass(_dataClass);

            GASystem.DataAccess.Utils.SQLView.ISQLView sqlView = GASystem.DataAccess.Utils.SQLView.SQLViewFactory.Make(cd, owner);
            string selectSqlOwner = sqlView.GetSQLViewQuery();

            if (filter != string.Empty)
                selectSqlOwner = selectSqlOwner + " and " + filter;
			SqlConnection myConnection = DataUtils.GetConnection(_transaction);

            //include all dates
            DateTime   startDate = new DateTime(1754, 1, 1);
            DateTime  endDate = new DateTime(9999, 1, 1);

            SqlDataAdapter da = new SqlDataAdapter(selectSqlOwner, myConnection);
            /*// for debugging only 
            da.SelectCommand.CommandTimeout = 600;*/
            da.SelectCommand.Parameters.AddWithValue("@dateFrom", startDate);
            da.SelectCommand.Parameters.AddWithValue("@DateTo", endDate);
			da.Fill(GAData, _dataClass.ToString());
			return GAData;
		}


        DataSet IDataAccessListView.GetByOwnerAndTimeSpan(GADataRecord owner, System.DateTime startDate, System.DateTime endDate, string filter)
        {
            throw new System.NotImplementedException("Flag views do not support method get all within timespan");
        }


        ///// <summary>
        ///// Get all records of specified dataclass by owner filtered by date. Note that for views are we returning the same data
        ///// as for allwithin. Calling this method will return the same data as calling GetRecordsWithinOwner
        ///// </summary>
        ///// <param name="owner"></param>
        ///// <param name="startDate"></param>
        ///// <param name="endDate"></param>
        ///// <param name="filter"></param>
        ///// <returns></returns>
        //DataSet IDataAccessListView.GetByOwnerAndTimeSpan(GADataRecord owner, System.DateTime startDate, System.DateTime endDate, string filter) 
        //{
        //    DataSet GAData = GetDataSetIntance();

        //    AppUtils.ClassDescription cd = AppUtils.ClassDefinition.GetClassDescriptionByGADataClass(_dataClass);

        //    GASystem.DataAccess.Utils.SQLView.ISQLView sqlView = GASystem.DataAccess.Utils.SQLView.SQLViewFactory.Make(cd, owner);
        //    string selectSqlOwner = sqlView.GetSQLViewQuery();


        //    if (filter != string.Empty)
        //        selectSqlOwner = selectSqlOwner + " and " + filter;

        //    //set date filter            
        //    if (cd.hasDateFromField() && cd.hasDateToField()) 
        //    {
        //        string sqlfilter = DataUtils.GenerateDateSpanFilter(cd.DateFromField, cd.DateToField);
        //        selectSqlOwner += " and " + sqlfilter;
        //    }


        //    String sql = selectSqlOwner;
			
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
        DataSet IDataAccessListView.GetRecordsWithinOwner(GADataRecord owner, string filter, System.DateTime startDate, System.DateTime endDate) 
		{
            DataSet GAData = GetDataSetIntance();

            AppUtils.ClassDescription cd = AppUtils.ClassDefinition.GetClassDescriptionByGADataClass(_dataClass);

            GASystem.DataAccess.Utils.SQLView.ISQLView sqlView = GASystem.DataAccess.Utils.SQLView.SQLViewFactory.Make(cd, owner);
            string selectSqlOwner = sqlView.GetSQLViewQuery();


            if (filter != string.Empty)
                selectSqlOwner = selectSqlOwner + " and " + filter;

            //set date filter            
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


		private DataSet GetDataSetIntance()
		{
            return new DataSet();
		}

    }
}
