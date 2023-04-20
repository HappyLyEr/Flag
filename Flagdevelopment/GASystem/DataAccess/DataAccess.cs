using System;
using System.Data;
using System.Data.SqlClient;
using GASystem.DataAccess.Security;
using GASystem.DataAccess.Utils;
using GASystem.GAExceptions;
using GASystem.DataModel;
using System.Collections;

namespace GASystem.DataAccess
{
	/// <summary>
	/// This class contains the basic dataaccess methods. Use DataAccessFactory to instanciate this class with correct dataclass
	/// </summary>
	public class DataAccess
	{
		private  string _selectSqlTemplate = @"SELECT * FROM {0} ";
        private string _selectSqlTemplateNoColumns = @"SELECT {0} FROM {1} ";
        private string _selectdistinct = @"SELECT distinct {0} FROM {1} ";
		private  string _selectSql = null;
		private GADataClass _dataClass;
        // Tor 201611 Security 20161215 added
        // Tor 20170323 remove added
        //private int _rowId = -1;
        //private GADataRecord _owner;
        //private GADataRecord _record;
        // Tor 201611 Security 20161215 added end
        private GADataTransaction _transaction = null;
		private GASecurityDb_new _security = null;

		public DataAccess(GADataClass DataClass, GADataTransaction Transaction)
		{
			_dataClass = DataClass;
			_transaction = Transaction;
			_selectSql = string.Format(_selectSqlTemplate, DataClass.ToString());
            _security = new GASecurityDb_new(DataClass, Transaction);
		}

        // Tor 20170323 overload method removed
        //public DataAccess(GADataClass DataClass, int rowId, GADataTransaction Transaction)
        //{
        //    _dataClass = DataClass;
        //    //_rowId = rowId;
        //    //_record=new GADataRecord(_rowId,_dataClass);
        //    _transaction = Transaction;
        //    _selectSql = string.Format(_selectSqlTemplate, DataClass.ToString());
        //    // Tor 201611 Security 20161215 
        //    _security = new GASecurityDb_new(DataClass, Transaction);
        //    // Tor Rollback 201611 Security 20161215 _security = new GASecurityDb_new(_record, _dataClass, _transaction);
        //}

		public  DataSet GetAll()
		{
			DataSet GAData = GetDataSetIntance();	
			SqlConnection myConnection = DataUtils.GetConnection(_transaction);
			
			_selectSql = _security.AppendSecurityFilterQueryForRead(_selectSql);

			SqlDataAdapter da = new SqlDataAdapter(_selectSql, myConnection);
			da.Fill(GAData, _dataClass.ToString());
			return GAData;
		}

		public DataSet GetByRowId(int RowId)
		{
            string rowIdColumnName = _dataClass.ToString().Substring(2) + "RowId";
			String appendSql = string.Format(" WHERE {0}={1}", rowIdColumnName, RowId);
            DataSet GAData = GetDataSetIntance();
            DataSet cachedObject = GetDataSetIntance();

            DataCache.ValidateCache(DataCache.DataCacheType.ByRowId);

            cachedObject = (DataSet)DataCache.GetCachedObject(DataCache.DataCacheType.ByRowId, appendSql);
            if (cachedObject != null)
                return cachedObject;
		
			SqlConnection myConnection = DataUtils.GetConnection(_transaction);

			String sql = _security.AppendSecurityFilterQueryForRead(_selectSql+appendSql);

            AppUtils.ClassDescription cd = AppUtils.ClassDefinition.GetClassDescriptionByGADataClass(_dataClass);

            if (cd.hasViewSQL())
                sql = GASystem.DataAccess.Utils.SQLView.SQLViewFactory.Make(cd, null).getByRowId(RowId);



			SqlDataAdapter da = new SqlDataAdapter(sql, myConnection);
			if (_transaction != null)
				da.SelectCommand.Transaction = (SqlTransaction) _transaction.Transaction;
			da.Fill(GAData, _dataClass.ToString());
            DataCache.AddCachedObject(DataCache.DataCacheType.ByRowId, appendSql, GAData);
            return GAData;
		}

        /// <summary>
        /// Get a number of records of type dataclass based on an array of rowids
        /// </summary>
        /// <param name="RowId"></param>
        /// <returns></returns>
        public DataSet GetByRowIds(int[] rowIds)
        {
            if (rowIds.Length == 0)  //no rowids passed. return an empty dataset
            {
                return GetDataSetIntance();      
            }

            object[] objectRowIds = new object[rowIds.Length];
            Array.Copy(rowIds, objectRowIds, rowIds.Length);    //copy in order to cast an array of int to an array of objects

            string rowIdString = GASystem.AppUtils.GAUtils.ConvertArrayToString(objectRowIds, ", ");

            string rowIdColumnName = _dataClass.ToString().Substring(2) + "RowId";
            String appendSql = string.Format(" WHERE {0} in ({1})  ", rowIdColumnName, rowIdString);
            DataSet GAData = GetDataSetIntance();
            SqlConnection myConnection = DataUtils.GetConnection(_transaction);

            String sql = _security.AppendSecurityFilterQueryForRead(_selectSql + appendSql);

            SqlDataAdapter da = new SqlDataAdapter(sql, myConnection);
            if (_transaction != null)
                da.SelectCommand.Transaction = (SqlTransaction)_transaction.Transaction;
            da.Fill(GAData, _dataClass.ToString());
            return GAData;
        }


		public DataSet GetEmptyDataSet() 
		{
			return GetDataSetIntance();
		}

        /// <summary>
        /// Get all records of specified dataclass by owner
        /// </summary>
        /// <param name="OwnerRowId"></param>
        /// <param name="OwnerDataClass"></param>
        /// <returns></returns>
		public DataSet GetByOwner(int OwnerRowId, GADataClass OwnerDataClass)
		{
			DataSet GAData = GetDataSetIntance();	
			string selectSqlOwner = SQLGenerateUtils.GetSelectSqlMembers(_dataClass, OwnerRowId, OwnerDataClass);
			SqlConnection myConnection = DataUtils.GetConnection(_transaction);

			//String sql = _security.AppendSecurityFilterQueryForRead(selectSqlOwner, new GADataRecord(OwnerRowId, OwnerDataClass));
			String sql = _security.AppendSecurityFilterQueryForRead(selectSqlOwner);
			
			
			SqlDataAdapter da = new SqlDataAdapter(sql , myConnection);
			da.Fill(GAData, _dataClass.ToString());
			return GAData;
		}

        ///// <summary>
        ///// Get record by GADataRecord
        ///// </summary>
        ///// <param name="OwnerRowId"></param>
        ///// <param name="OwnerDataClass"></param>
        ///// <returns></returns>
        //public DataSet GetByOwner(GADataRecord dataRecord)
        //{
        //    DataSet GAData = GetDataSetIntance();
        //    string selectSqlOwner = SQLGenerateUtils.GetSelectSqlMembers(_dataClass, OwnerRowId, OwnerDataClass);
        //    SqlConnection myConnection = DataUtils.GetConnection(_transaction);

        //    //String sql = _security.AppendSecurityFilterQueryForRead(selectSqlOwner, new GADataRecord(OwnerRowId, OwnerDataClass));
        //    String sql = _security.AppendSecurityFilterQueryForRead(selectSqlOwner);


        //    SqlDataAdapter da = new SqlDataAdapter(sql, myConnection);
        //    da.Fill(GAData, _dataClass.ToString());
        //    return GAData;
        //}
        public DataSet GetByOwnerAndTimeSpan(GADataRecord Owner, System.DateTime StartDate, System.DateTime EndDate)
        {
            return GetByOwnerAndTimeSpan(Owner, StartDate, EndDate, string.Empty);
        }

		public  DataSet GetByOwnerAndTimeSpan(GADataRecord Owner, System.DateTime StartDate, System.DateTime EndDate, string filter) 
		{
			DataSet GAData = GetDataSetIntance();	
			string selectSqlOwner = SQLGenerateUtils.GetSelectSqlMembers(_dataClass, Owner.RowId, Owner.DataClass);
			GASystem.AppUtils.ClassDescription cd = GASystem.AppUtils.ClassDefinition.GetClassDescriptionByGADataClass(_dataClass);
			if (cd.hasDateFromField() && cd.hasDateToField()) 
			{
				string sqlfilter = generateDateSpanFilter(cd.DateFromField, cd.DateToField, cd.DataClassName);
				selectSqlOwner += " and " + sqlfilter;
			} else if (cd.hasDateField())
            {
                string sqlfilter = DataUtils.GenerateDateFilter(cd);
                selectSqlOwner += " and " + sqlfilter;
            } else if (filter != string.Empty)
                selectSqlOwner += " and " + filter;

			String sql = _security.AppendSecurityFilterQueryForRead(selectSqlOwner);
			
			//adjust from and to date. SqlServer dates must be between 1753 and 9999
			if (StartDate.Year < 1754)
				StartDate = new DateTime(1754, 1, 1);

			if (EndDate.Year > 9999) 
				EndDate = new DateTime(9999,1,1);
			

			SqlConnection myConnection = DataUtils.GetConnection(_transaction);
			SqlDataAdapter da = new SqlDataAdapter(sql, myConnection);
			da.SelectCommand.Parameters.AddWithValue("@dateFrom", StartDate);
            da.SelectCommand.Parameters.AddWithValue("@DateTo", EndDate);

			if (_transaction != null)
				da.SelectCommand.Transaction = (SqlTransaction) _transaction.Transaction;
			da.Fill(GAData, _dataClass.ToString());
			return GAData;


		}



        public int GetNumberOfRowsByOwnerAndTimeSpan(GADataRecord Owner, System.DateTime StartDate, System.DateTime EndDate, string filter)
        {
            //TODO complete this method
            string selectSqlMembers = @"SELECT     count(*)
										FROM       {0} INNER JOIN
										GASuperClass ON {0}.{1}RowId = GASuperClass.MemberClassRowId
										WHERE     (dbo.GASuperClass.OwnerClassRowId = {2}) AND (dbo.GASuperClass.OwnerClass = '{3}' ) AND (dbo.GASuperClass.MemberClass = '{0}' )";



            
            
            //DataSet GAData = GetDataSetIntance();
            string selectSqlOwner = String.Format(selectSqlMembers, new Object[] { _dataClass, _dataClass.ToString().Substring(2), Owner.RowId.ToString(), Owner.DataClass.ToString() });
            GASystem.AppUtils.ClassDescription cd = GASystem.AppUtils.ClassDefinition.GetClassDescriptionByGADataClass(_dataClass);
            if (cd.hasDateFromField() && cd.hasDateToField())
            {
                string sqlfilter = generateDateSpanFilter(cd.DateFromField, cd.DateToField, cd.DataClassName);
                selectSqlOwner += " and " + sqlfilter;
            } else if (cd.hasDateField())
            {
                string sqlfilter = DataUtils.GenerateDateFilter(cd);
                selectSqlOwner += " and " + sqlfilter;
            }


            if (filter != string.Empty)
                selectSqlOwner += " and " + filter;

           // String sql = _security.AppendSecurityFilterQueryForRead(selectSqlOwner);

            //adjust from and to date. SqlServer dates must be between 1753 and 9999
            if (StartDate.Year < 1754)
                StartDate = new DateTime(1754, 1, 1);

            if (EndDate.Year > 9999)
                EndDate = new DateTime(9999, 1, 1);


            SqlConnection myConnection = DataUtils.GetConnection(_transaction);
            bool closeConnectionAfterExecute = false;
           
            SqlCommand cmd = new SqlCommand(selectSqlOwner, myConnection);
            cmd.Parameters.AddWithValue("@dateFrom", StartDate);
            cmd.Parameters.AddWithValue("@DateTo", EndDate);

            if (!(myConnection.State == ConnectionState.Open))
            {      //if not using a current open connection, open the connection and close after execution
                myConnection.Open();
                closeConnectionAfterExecute = true;
            }

            int returnValue = (int)cmd.ExecuteScalar();

            if (closeConnectionAfterExecute)
                myConnection.Close();
            return returnValue;

       }



		/// <summary>
		/// Gets a dataset with records for classes defined as many to many. For user Role building these records must be 
		/// availble without security checking.
		/// Will throw an exception if this method is called on a dataclass that is not many to many
		/// </summary>
		/// <param name="Owner"></param>
		/// <param name="StartDate"></param>
		/// <param name="EndDate"></param>
		/// <returns></returns>
		public  DataSet GetManyToManyRecordsByOwnerAndTimeSpan(GADataRecord Owner, System.DateTime StartDate, System.DateTime EndDate) 
		{
			DataSet GAData = GetDataSetIntance();	
			string selectSqlOwner = SQLGenerateUtils.GetSelectSqlMembers(_dataClass, Owner.RowId, Owner.DataClass);
			GASystem.AppUtils.ClassDescription cd = GASystem.AppUtils.ClassDefinition.GetClassDescriptionByGADataClass(_dataClass);
			
			
			if (!cd.isClassManyToManyLink())
				throw new GAExceptions.GASecurityException("Error getting records for many to many class. Class "  + _dataClass.ToString() +  " is not defined as many to many");

			if (cd.hasDateFromField() && cd.hasDateToField()) 
			{
				string sqlfilter = generateDateSpanFilter(cd.DateFromField, cd.DateToField, cd.DataClassName);
				selectSqlOwner += " and " + sqlfilter;
			}



			//adjust from and to date. SqlServer dates must be between 1753 and 9999
			if (StartDate.Year < 1754)
				StartDate = new DateTime(1754, 1, 1);

			if (EndDate.Year > 9999) 
				EndDate = new DateTime(9999,1,1);
			

			SqlConnection myConnection = DataUtils.GetConnection(_transaction);
			SqlDataAdapter da = new SqlDataAdapter(selectSqlOwner, myConnection);
            da.SelectCommand.Parameters.AddWithValue("@dateFrom", StartDate);
            da.SelectCommand.Parameters.AddWithValue("@DateTo", EndDate);

			if (_transaction != null)
				da.SelectCommand.Transaction = (SqlTransaction) _transaction.Transaction;
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
		public DataSet GetRecordsWithinOwner(GADataRecord Owner, string Filter, System.DateTime DateFrom, System.DateTime DateTo) 
		{
			DataSet ds = GetDataSetIntance();

			string sql;
			AppUtils.ClassDescription cd = AppUtils.ClassDefinition.GetClassDescriptionByGADataClass(_dataClass);

		
			if (!cd.IsTop)
				sql = GASystem.DataAccess.SQLGenerateUtils.GenerateSelectForGATableAllWithinOwner(_dataClass, Owner.RowId, Owner.DataClass);
			else
				sql = "select * from " + _dataClass.ToString() + " where 1=1 ";
		
			if (Filter != string.Empty)
				sql += " and " + Filter;


			// check wether dataclass has a datefrom - dateto definition, or a specific data definition. Add filter accordingly
			if (cd.hasDateFromField() && cd.hasDateToField()) 
			{
				string sqlfilter = generateDateSpanFilter(cd.DateFromField, cd.DateToField, cd.DataClassName);
				sql += " and " + sqlfilter;
			} 
			else if (cd.DateField != string.Empty) 
			{
				string dateFilter = generateDateFilter(cd);
				sql += " and " + dateFilter;
			}
			
			
			//adjust from and to date. SqlServer dates must be between 1753 and 9999
			if (DateFrom.Year < 1754)
				DateFrom = new DateTime(1754, 1, 1);

			if (DateTo.Year > 9999) 
				DateTo = new DateTime(9999,1,1);


            // Tor 201611 Security 20161122 
            GASystem.DataAccess.Security.GASecurityDb_new securityDb = new GASystem.DataAccess.Security.GASecurityDb_new(_dataClass, null);
            // Tor Rollback 201611 Security 20161122 GASystem.DataAccess.Security.GASecurityDb_new securityDb = new GASystem.DataAccess.Security.GASecurityDb_new(Owner,_dataClass, null);

			string sqlSecure = securityDb.AppendSecurityFilterQueryForRead(sql);
			



			//System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand(sqlSecure);
			System.DateTime fromDate = DateFrom.Date + new TimeSpan(0, 0,0,0,0);//include all of the day in the less than test, set check time to the start of the day
			System.DateTime toDate = DateTo.Date + new TimeSpan(0, 23,59,59,0);

			SqlConnection myConnection = DataUtils.GetConnection(_transaction);
			SqlDataAdapter da = new SqlDataAdapter(sqlSecure, myConnection);
            da.SelectCommand.Parameters.AddWithValue("@dateFrom", fromDate);
            da.SelectCommand.Parameters.AddWithValue("@dateTo", toDate);

			


		//	command.Parameters.Add("@dateFrom",fromDate) ;
		//	command.Parameters.Add("@dateTo",toDate) ;


			if (_transaction != null)
				da.SelectCommand.Transaction = (SqlTransaction) _transaction.Transaction;
			da.Fill(ds, _dataClass.ToString());
			return ds;
		}

        public DataSet GetDistinctColumn(string columnname, string filter) 
        {
            string sql =  string.Format(_selectdistinct, columnname, _dataClass.ToString());
            if (filter != string.Empty)
                sql += " where " + filter;
            
            DataSet GAData = GetDataSetIntance();
            SqlConnection myConnection = DataUtils.GetConnection(_transaction);

            SqlDataAdapter da = new SqlDataAdapter(sql, myConnection);
            if (_transaction != null)
                da.SelectCommand.Transaction = (SqlTransaction)_transaction.Transaction;
            da.Fill(GAData, _dataClass.ToString());
            return GAData;
        }

        // Tor 20150626 added to get rowid by column value
        public DataSet GetRowIdFromClassWithFilter(string ClassName, string columnname, string filter)
        {
//                    private string _selectSqlTemplateNoColumns = @"SELECT {0} FROM {1} ";

            string sql = string.Format(_selectSqlTemplateNoColumns, ClassName.Substring(3,99)+"RowId",ClassName);
            if (filter != string.Empty) sql += " where " + filter;

            DataSet GAData = GetDataSetIntance();
            SqlConnection myConnection = DataUtils.GetConnection(_transaction);

            SqlDataAdapter da = new SqlDataAdapter(sql, myConnection);
            if (_transaction != null)
                da.SelectCommand.Transaction = (SqlTransaction)_transaction.Transaction;
            da.Fill(GAData, _dataClass.ToString());
            return GAData;
        }

        // Tor 20160321 Get vertical from owner class 
        public static int getVerticalFromActionOwner(string Class,int rowId)
        {
            GADataRecord member = new GADataRecord(rowId, GADataRecord.ParseGADataClass(Class));
            GADataRecord owner = GASystem.BusinessLayer.DataClassRelations.GetOwner(member, null);
            // Tor 20160417 
            return getVerticalListsRowIdFromRecord(owner.DataClass.ToString(), owner.RowId);

            //string ownerVerticalFieldName = GASystem.BusinessLayer.Class.GetClassVerticalFieldName(owner.DataClass.ToString());
            //if (ownerVerticalFieldName != string.Empty)
            //{
            //    SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
            //    try
            //    {
            //        myConnection.Open();
            //        string sql = "select {0} from {1} where {2}RowId={3}";
            //        sql = string.Format(sql, ownerVerticalFieldName, owner.DataClass.ToString(), owner.DataClass.ToString().Substring(2), owner.RowId.ToString());
            //        SqlCommand myCommand = new SqlCommand(sql, myConnection);
            //        verticalListsRowId = (int)myCommand.ExecuteScalar();
            //    }
            //    catch
            //    { }
            //    finally
            //    {
            //        myConnection.Close();
            //    }
            //}
            //return verticalListsRowId;
        }

        // Tor 20160417 Get vertical from record 
        public static int getVerticalListsRowIdFromRecord(string Class, int rowId)
        {
            int verticalListsRowId = 0;
            string verticalFieldName = GASystem.BusinessLayer.Class.GetClassVerticalFieldName(Class);
            if (verticalFieldName != string.Empty)
            {
                SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
                try
                {
                    myConnection.Open();
                    string sql = "select {0} from {1} where {2}RowId={3}";
                    sql = string.Format(sql, verticalFieldName, Class, Class.Substring(2), rowId.ToString());
                    SqlCommand myCommand = new SqlCommand(sql, myConnection);
                    verticalListsRowId = (int)myCommand.ExecuteScalar();
                    if (verticalListsRowId.ToString() == string.Empty) verticalListsRowId = 0;
                }
                catch
                { }
                finally
                {
                    myConnection.Close();
                }
            }
            return verticalListsRowId;
        }

        // Tor 20160624 Get reference id from record 
        public static string getReferenceIdFromRecord(string Class, int rowId)
        {
            string referenceIdValue = string.Empty;
            string referenceIdFieldName = GASystem.BusinessLayer.Class.GetClassReferenceIdFieldName(Class);
            if (referenceIdFieldName != string.Empty)
            {
                SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
                try
                {
                    myConnection.Open();
                    string sql = "select {0} from {1} where {2}RowId={3}";
                    sql = string.Format(sql, referenceIdFieldName, Class, Class.Substring(2), rowId.ToString());
                    SqlCommand myCommand = new SqlCommand(sql, myConnection);
                    referenceIdValue=myCommand.ExecuteScalar().ToString();
                }
                catch
                { }
                finally
                {
                    myConnection.Close();
                }
            }
            return referenceIdValue;
        }

        // Tor 20160624 Get Object Name from record 
        public static string getObjectNameFromRecord(string Class, int rowId)
        {
            string objectNameValue = string.Empty;
            string objectNameFieldName = GASystem.BusinessLayer.Class.GetClassObjectName(Class);
            if (objectNameFieldName != string.Empty)
            {
                SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
                try
                {
                    myConnection.Open();
                    string sql = "select {0} from {1} where {2}RowId={3}";
                    sql = string.Format(sql, objectNameFieldName, Class, Class.Substring(2), rowId.ToString());
                    SqlCommand myCommand = new SqlCommand(sql, myConnection);
                    objectNameValue = myCommand.ExecuteScalar().ToString();
                }
                catch
                { }
                finally
                {
                    myConnection.Close();
                }
            }
            return objectNameValue;
        }
        private string generateDateSpanFilter(string DateFromField, string DateToField, string DataClass) 
		{
			//todate in timespan
			string filterto = " ( @dateFrom <= _formDateField_  and _formDateField_ <= @DateTo     ) ";

			//from date in timespan
			string filterfrom = " ( @dateFrom <= _toDateField_  and _toDateField_ <= @DateTo     ) ";

			//timespan between start and end
			string filterin =  " ( @dateFrom >= _formDateField_  and _toDateField_ >= @DateTo     ) ";
			
			//from is null
			string filterfromnull = " ( _formDateField_ is null and _toDateField_ >= @dateFrom     ) ";
			//to is null
			string filtertonull = " ( _toDateField_ is null  and _formDateField_ <= @DateTo     ) ";
			//both is null
			string filternull = " ( _toDateField_ is null  and _formDateField_ is null     ) ";

			//combined 
			string filter = " (" + filterto + " or " + filterfrom + " or "+ filterin  + " or "+ filterfromnull + " or " + filtertonull + " or " + filternull + ") ";

            filter = filter.Replace("_formDateField_", DataClass + "." + DateFromField);
            filter = filter.Replace("_toDateField_", DataClass + "." + DateToField);
			
			return filter;
		}
		

		private string generateDateFilter(AppUtils.ClassDescription cd)
		{
			string filter = string.Empty;
            filter = " ( (@dateFrom <= _formDateField_  and _toDateField_ <= @DateTo) or (_formDateField_ is null)     )";
            filter = filter.Replace("_formDateField_", cd.DataClassName + "." + cd.DateField);
            filter = filter.Replace("_toDateField_", cd.DataClassName + "." + cd.DateField);
			return filter;

		}


		public  DataSet Update(DataSet GADataSet)
		{
            //generate select for sqlcommandbuilder
            // Tor 20141229 below moved up
            System.Collections.ArrayList columns = new System.Collections.ArrayList();
            foreach (DataColumn column in GADataSet.Tables[_dataClass.ToString()].Columns)
            {
                if (column.ColumnName.ToLower() != "rowguid")
                    if (column.ColumnName.ToLower() != "[rowguid]")
                        columns.Add(column.ColumnName);
            }
            string selectSql = string.Format(_selectSqlTemplateNoColumns,
                GASystem.AppUtils.GAUtils.ConvertArrayToString(columns.ToArray(), ", "),
                _dataClass.ToString());


       		SqlConnection myConnection = DataUtils.GetConnection(_transaction);
            SqlDataAdapter da = new SqlDataAdapter(selectSql, myConnection);
			da.SelectCommand.Transaction = (SqlTransaction) _transaction.Transaction;
			da.SelectCommand.Connection = myConnection;
			
            //using commandbuilder for adding update and delete statements to dataadapter
            SqlCommandBuilder  cb = new SqlCommandBuilder(da);
            
            string sqlInsert = cb.GetInsertCommand(true).CommandText;

            ////hack, remove rowguid from insert
            //sqlInsert = sqlInsert.Replace("[rowguid],", string.Empty);
            //sqlInsert = sqlInsert.Replace("@rowguid,", string.Empty);
            //// and of rowguid is last in list
            //sqlInsert = sqlInsert.Replace(", [rowguid]", string.Empty);
            //sqlInsert = sqlInsert.Replace(", @rowguid", string.Empty);





            //overriding commandbuilders insert command, generating our own insert command
            da.InsertCommand = new SqlCommand(sqlInsert, myConnection, (SqlTransaction)_transaction.Transaction);
           
            //add parameters to insertcommand
            foreach (System.Data.SqlClient.SqlParameter param in cb.GetInsertCommand(true).Parameters) 
            {
                System.Data.SqlClient.SqlParameter newParam = new System.Data.SqlClient.SqlParameter();
                
                newParam.ParameterName = param.ParameterName;
                newParam.SqlDbType = param.SqlDbType;
                newParam.DbType = param.DbType;
                newParam.Size =param.Size;
                newParam.SourceColumn = param.SourceColumn;
                newParam.SourceColumnNullMapping = param.SourceColumnNullMapping;
                newParam.SqlValue = param.SqlValue;
                newParam.Direction = param.Direction;
                
                newParam.SourceVersion = param.SourceVersion;

                da.InsertCommand.Parameters.Add(newParam);
               
            }
            

            //add select command with scope_identity, to insert statement in order to get the new rowid and other database created values
            string RowIdColumn = this._dataClass.ToString().Substring(2) + "RowId";
            da.InsertCommand.CommandText += "; " + da.SelectCommand.CommandText + " where " + RowIdColumn + " = scope_identity();";

			da.Update(GADataSet, _dataClass.ToString());
			return GADataSet;
		}

		public System.Data.DataSet GetNewRecord()
		{
			System.Data.DataSet ds = GetDataSetIntance();
			ds.Tables[_dataClass.ToString()].Rows.Add(ds.Tables[_dataClass.ToString()].NewRow());
			return ds;
		}


		private DataSet GetDataSetIntance()
		{
			switch (_dataClass)
			{
				// Generate script: Select N'case GADataClass.'+Class+N' : return new ' +substring(class,3,99)+N'DS(); break;' from GAClass where not(GAClass.class like N'%view%') order by class
				case GADataClass.GAAction : return new ActionDS(); 
				case GADataClass.GAActionTemplate : return new ActionTemplateDS();
                case GADataClass.GAAspectImpact: return new AspectImpactDS();  // Tor 20151028 : REQUESTOR  Daniella Bordon
				case GADataClass.GAAudit : return new AuditDS();
                case GADataClass.GABusinessRisk: return new BusinessRiskDS();   // Tor 20160730 : Requestor Michael Dudley
				case GADataClass.GACertificate : return new CertificateDS(); 
				case GADataClass.GAClass : return new ClassDS();
                case GADataClass.GAClient : return new ClientDS();   // GAO 20201224
                case GADataClass.GAClientVisit : return new ClientVisitDS();   // GAO 20220316
                case GADataClass.GAClientFeedbackView: return new ClientFeedbackViewDS();  // Tor 20120609 added - class has URL
                case GADataClass.GAClientFeedbackViewEvaluationForm: return new ClientFeedbackViewEvaluationFormDS(); //Lmz 20200914
                case GADataClass.GAClientFeedbackViewProject: return new ClientFeedbackViewProjectDS(); //Lmz 20200915
				case GADataClass.GACompany : return new CompanyDS();
				case GADataClass.GAControl : return new ControlDS();
                case GADataClass.GAControlMeasureAdd : return new ControlMeasureAddDS();     // Tor 20180420 added on request from Phil Bigg
                case GADataClass.GAControlMeasureCurrent: return new ControlMeasureCurrentDS();  // Tor 20180420 added on request from Phil Bigg
				case GADataClass.GACost : return new CostDS();
				case GADataClass.GACourse : return new CourseDS(); 
				case GADataClass.GACoursePersonList : return new CoursePersonListDS();
                case GADataClass.GACoursePersonListView: return new CoursePersonListViewDS(); // Tor 20110824 : added - same as GACoursePersonList but Url and Mimetype exest in view to enable attachments reference in object 
                case GADataClass.GACreateRecordFromClass: return new CreateRecordFromClassDS(); // Tor 20150622 : class to create new record from other record (diffent classes)
                case GADataClass.GACrew: return new CrewDS();
                case GADataClass.GACrewInProject: return new CrewInProjectDS();
                case GADataClass.GACrisis: return new CrisisDS();
                case GADataClass.GACrisisCheckList: return new CrisisCheckListDS();
                case GADataClass.GACrisisCheckListItem: return new CrisisCheckListItemDS();
                case GADataClass.GACrisisIssue: return new CrisisIssueDS();
                case GADataClass.GACrisisMessageLog: return new CrisisMessageLogDS();
                case GADataClass.GACrisisNewsBulletin: return new CrisisNewsBulletinDS();
                
                case GADataClass.GADailyEmployeeCount: return new DailyEmployeeCountDS(); 
				case GADataClass.GADamagedEquipment : return new DamagedEquipmentDS();
                case GADataClass.GADayOperation: return new DayOperationDS(); // Tor 20190204 on request from Gao Peng
				case GADataClass.GADaysReport : return new DaysReportDS(); 
				case GADataClass.GADepartment : return new DepartmentDS(); 
				case GADataClass.GADocument : return new DocumentDS();
                case GADataClass.GADocumentControl: return new DocumentControlDS(); // Tor 20140501 request from Phil Bigg: class to create workflows to control document revisions
                case GADataClass.GAEmployment: return new EmploymentDS(); 
				case GADataClass.GAEquipmentDamageReport : return new EquipmentDamageReportDS(); 
			//	case GADataClass.GAFieldDatatype : return new FieldDatatypeDS(); break;
			//	case GADataClass.GAFieldDefinitions : return new FieldDefinitionsDS(); break;
				case GADataClass.GAFile : return new FileDS();
                case GADataClass.GAFileContent: return new FileContentDS(); // Tor 20110415 : added 
				case GADataClass.GAFileFolder : return new FileFolderDS();
                case GADataClass.GAFileView: return new FileViewDS(); // Tor 20120228 for ApplyAdditionalAccessControl=1 classes (currently for GAPersonnel)
                case GADataClass.GAFlag: return new FlagDS(); // Tor 20140418 required after start using GASuperClassLinks instead of GAClass to decide permission
                case GADataClass.GAFlagTask: return new FlagTaskDS();
                case GADataClass.GAGoalObjective: return new GoalObjectiveDS(); // Tor 20150923 added on request from Phil Bigg
                case GADataClass.GAGroups: return new GroupsDS();
                case GADataClass.GAionGoalObjective: return new ionGoalObjectiveDS(); // Tor 20160728 added on request from Phil Bigg
                
				case GADataClass.GAHazardIdentification : return new HazardIdentificationDS();
                case GADataClass.GAHealthCertificateView: return new HealthCertificateViewDS(); // Tor 20110726 : added 
                case GADataClass.GAHelp: return new HelpDS(); 
				case GADataClass.GAIncidentReport : return new IncidentReportDS();
                case GADataClass.GAInfoToCommunity: return new InfoToCommunityDS();
                case GADataClass.GAInjuredParty: return new InjuredPartyDS();
                case GADataClass.GAInvestigationTeam: return new InvestigationTeamDS(); // Tor 20180409 : added view on request from Phil Bigg
                case GADataClass.GAIssue: return new IssueDS();
                case GADataClass.GAKPIIndicator: return new KPIIndicatorDS(); // Tor 20170303 : added on request from Tim Granlie/Phil Bigg

                case GADataClass.GALessonLearnt: return new LessonLearntDS(); // Tor 20160728 : added on request from Phil Bigg
                case GADataClass.GALegalCompliance: return new LegalComplianceDS(); // Gao 20210304 :  added for new tab Legal Compliance
                    
                case GADataClass.GALicense: return new LicenseDS();
                case GADataClass.GALink: return new LinkDS(); 
				case GADataClass.GAListCategory : return new ListCategoryDS(); 
				case GADataClass.GALists : return new ListsDS();
                case GADataClass.GAListsSelected: return new ListsSelectedDS();
				case GADataClass.GALocation : return new LocationDS();
                case GADataClass.GALocationCertificateView: return new LocationCertificateViewDS();
                case GADataClass.GALocationInCrew: return new LocationInCrewDS();
                case GADataClass.GAManageChange: return new ManageChangeDS(); 
				case GADataClass.GAMeansOfContact : return new MeansOfContactDS();
				case GADataClass.GAMeeting : return new MeetingDS();
                case GADataClass.GAMeetingToolboxView: return new MeetingToolboxViewDS(); //Lmz 20200914
				case GADataClass.GAMeetingPersonList : return new MeetingPersonListDS();
                case GADataClass.GAMeetingReport: return new MeetingReportDS(); // Tor 20171110 : added on request from Emerson Verissimo - moved all GAMeeting and member records to GAMeetingReport
				case GADataClass.GAMeetingText : return new MeetingTextDS();
                case GADataClass.GAMilestone: return new MilestoneDS(); // Tor 20081002 : added 
                case GADataClass.GAMMOReport: return new MMOReportDS(); // Tor 20190105 on request from Gao Peng
                case GADataClass.GAMonitoringNote: return new MonitoringNoteDS(); // Tor 20150925 : added on request from Phil Bigg - notes under GAGoalObjective  
                case GADataClass.GANextOfKin: return new NextOfKinDS(); 
				case GADataClass.GAOpportunity : return new OpportunityDS();
                case GADataClass.GAOpportunityDetail: return new OpportunityDetailDS(); // Tor 20170406 : added view to store GAOpportunity details during I&O Process on request from Phil Bigg / Daniella Bordon 
                case GADataClass.GAPassportVisaView: return new PassportVisaViewDS(); // Tor 20110726 : added 
                case GADataClass.GAPermitToWork: return new PermitToWorkDS(); // Tor 20140209 : added to replace PermitToWorkView which missed attributes for embedded documents 
                case GADataClass.GAPermitToWorkView: return new PermitToWorkViewDS();
                case GADataClass.GAionPermitToWork: return new ionPermitToWorkDS(); // Tor 20160728 : Phil Bigg: added to replace PermitToWork 
                case GADataClass.GAPersonnel: return new PersonnelDS();
                case GADataClass.GAPersonnelMedicalRecord: return new PersonnelMedicalRecordDS(); // Tor 20150205 Added on request from Michael Caddick
                case GADataClass.GAPersonnelTrainingMatrix: return new PersonnelTrainingMatrixDS(); // Tor 20140716
                case GADataClass.GAProcedure: return new ProcedureDS(); 
				case GADataClass.GAProcedureReference : return new ProcedureReferenceDS();
                //case GADataClass.GAProductService: return new ProductServiceDS(); // Tor 20150708 on request from Fabio Sarment: lookupfield instead of galists
                case GADataClass.GAProject : return new ProjectDS(); 
				//case GADataClass.GAProjectPartyChief : return new ProjectPartyChiefDS(); break;
                case GADataClass.GAProjectDocument: return new ProjectDocumentDS(); // Tor 20170415 on request from Tim Granli/Phil Bigg
                case GADataClass.GAProjectRisk: return new ProjectRiskDS(); // Tor 20190318 on request from Phil Bigg
                case GADataClass.GAPurchaseOrder: return new PurchaseOrderDS(); // Tor 20080812 : added 
                case GADataClass.GAPurchaseOrderLine: return new PurchaseOrderLineDS(); // Tor 20080812 : added 
                case GADataClass.GARegionalOfficeView: return new RegionalOfficeDS();  // GAO 20201224
                case GADataClass.GAReport : return new ReportDS();
                case GADataClass.GAReporter: return new ReporterDS(); // Tor 20070813 : removed , // Tor 20080714 : added 
                case GADataClass.GAReportInstance: return new ReportInstanceDS(); 
				case GADataClass.GAReportInstanceFilter : return new ReportInstanceFilterDS(); 
				case GADataClass.GAReports : return new ReportsDS();
                case GADataClass.GARequiredTraining: return new RequiredTrainingDS(); // Tor 20171211 : added on request from OceanGeo Emerson Verissimo Ricardo Silva
				case GADataClass.GAResource : return new ResourceDS(); 
				case GADataClass.GAResourceSpecification : return new ResourceSpecificationDS();
                case GADataClass.GAResourceUsage: return new ResourceUsageDS(); // Tor 20080814 : added 
                case GADataClass.GARisk: return new RiskDS();
                case GADataClass.GARiskCompTitle: return new RiskCompTitleDS(); // Tor 20171114 : added on request from Phil Bigg
				case GADataClass.GARiskControl : return new RiskControlDS(); 
				case GADataClass.GArxtReport : return new rxtReportDS();
				case GADataClass.GASafetyObservation : return new SafetyObservationDS();
                case GADataClass.GASeafarerView: return new SeafarerViewDS();  // Gao 20201123 : added
                case GADataClass.GAServiceDeskView: return new ServiceDeskViewDS(); // Tor 20080812 : added 
                case GADataClass.GAStakeHolder: return new StakeHolderDS();         // Tor 20171027 : added on request from Phil Bigg
				case GADataClass.GAStoreAttribute : return new StoreAttributeDS(); 
				case GADataClass.GAStoreObject : return new StoreObjectDS();
                // Tor 20070813 : removed case GADataClass.GASuggestion: return new SuggestionDS();
                //case GADataClass.GASuperClass : return new SuperClassDS(); 
				//case GADataClass.GASuperClassLinks : return new SuperClassLinksDS(); 
                case GADataClass.GASubcontractorView: return new SubcontractorViewDS(); // Tor 20121119 : added 
                case GADataClass.GASupplier: return new SupplierDS(); // Tor 20150418 : on request from Phil Bigg/Fabio Sarmento 
                case GADataClass.GATask: return new TaskDS(); 
				case GADataClass.GATaskTemplate : return new TaskTemplateDS(); 
				case GADataClass.GATeam : return new TeamDS();
                case GADataClass.GATemplate: return new TemplateDS();
                case GADataClass.GATitleToTrain: return new TitleToTrainDS();   // Tor 20171211 : added on request from OceanGeo Emerson Verissimo Ricardo Silva                
                case GADataClass.GATextItem: return new TextItemDS(); 
				case GADataClass.GATimeAndAttendance : return new TimeAndAttendanceDS();
                    
                //case GADataClass.GATrainingCertificateView: return new TrainingCertificateViewDS(); // Tor 20110726 : added 
                case GADataClass.GATrainingInstitutionView: return new TrainingInstitutionViewDS(); // Tor 20110825 : added 
                case GADataClass.GATrainingMatrix: return new TrainingMatrixDS(); // Tor 20140716
                case GADataClass.GATravelRiskAssessment: return new TravelRiskAssessmentDS(); // Tor 20170810
                case GADataClass.GAUser: return new UserDS();
                case GADataClass.GAUserCommunity: return new UserCommunityDS(); // Tor 20150707
                //case GADataClass.GAUserContext : return new UserContextDS(); 
				case GADataClass.GAUsers : return new UsersDS();
                case GADataClass.GAWaste: return new WasteDS();
                case GADataClass.GAWasteOnshore: return new WasteOnshoreDS();
                case GADataClass.GAWasteOffshore: return new WasteOffshoreDS();
                case GADataClass.GAWorkflow: return new WorkflowDS();
                case GADataClass.GAWorkflowStarter: return new WorkflowStarterDS(); // Tor 20140501 class to start workflows/send e-mail notifications
                case GADataClass.GAWorkitem: return new WorkitemDS();
                
                case GADataClass.GAExposedHoursGroupView: return new ExposedHoursGroupViewDS(); // Tor 20070915 : view added 
                case GADataClass.GARepairIssueView: return new RepairIssueViewDS(); // Tor 20071017 : view added 
                case GADataClass.GARxtIssueView: return new RxtIssueViewDS(); // Tor 20080903 : view added 
                case GADataClass.GAEmployeeSiteLogView: return new EmployeeSiteLogViewDS(); // Tor 20071207 : view added 
                case GADataClass.GAProcedureTemplate: return new ProcedureTemplateDS(); // jof 20081216 : view added 
                case GADataClass.GAAnalysis: return new AnalysisDS(); // jof 20090114 : view added 

                case GADataClass.GAClassFilter: return new ClassFilterDS();
                case GADataClass.GANonConformanceView: return new NonConformanceViewDS(); // Tor 20120224 : added on request from Phil Bigg
                //case GADataClass.GAVendorRequest: return new VendorRequestDS(); // Tor 20130306 : added on request from Phil Bigg
                //case GADataClass.GAPersonTrainingRecordView: return new PersonTrainingRecordViewDS(); // Tor 20130213 : added for report
                case GADataClass.GAPersonnelHRDocumentView: return new PersonnelHRDocumentViewDS(); // Tor 20131015 : added on request from Andreia Almeida and Phil Bigg
                case GADataClass.GADrugAndAlcoholTest: return new DrugAndAlcoholTestDS(); // Tor 20140205 : added on request from Michael Caddick and Phil Bigg
                case GADataClass.GAMedicalTreatmentLog: return new MedicalTreatmentLogDS(); // Tor 20140412 : added on request from Michael Caddick and Phil Bigg
                case GADataClass.GAToolProcess: return new ToolProcessDS(); //JOF 20141018 - part of HAVS
                case GADataClass.GAHandArmVibrationLog: return new HandArmVibrationLogDS(); //JOF 20141019 - part of HAVS
                case GADataClass.GARecordHistoryLog: return new RecordHistoryLogDS(); // Tor 20150105 - stores record changes 
                case GADataClass.GAVendor: return new VendorDS();   // Gao 20210913
                case GADataClass.GAVendorContact: return new VendorContactDS();     // Gao 20210913
                case GADataClass.GAVendorEvaluation: return new VendorEvaluationDS();   // Gao 20210916
                case GADataClass.GAVendorAudit: return new VendorAuditDS();     // Gao 20210916
                case GADataClass.GAVendorAuditFinding: return new VendorAuditFindingDS();     // Gao 20210916
                case GADataClass.GAWorkPlan: return new WorkPlanDS();   // Gao 20220818
                case GADataClass.GAWBOperationList: return new WBOperationListDS();     // Gao 20220823
                case GADataClass.GAProjectEmission: return new ProjectEmissionDS();     // Gao 20220921

				//case GADataClass.GAAction :
				//	return new ActionDS();
				//	break;
				
				default :
                    return new DataSet(_dataClass.ToString());

                    //throw new GADataClassNotSupportedException("Dataclass is not supported yet in DataAccess");
			}
		}
	}
}
