using System;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using GASystem.DataModel;

namespace GASystem.DataAccess
{
	/// <summary>
	/// Summary description for DataUtils.
	/// </summary>
	public class DataUtils
	{
		public DataUtils()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		//
		public static String getDnnConnectionString()
		{
            // Tor 20160303 has to reside in .config file - varies between servers
            return System.Configuration.ConfigurationManager.AppSettings.Get("SiteSqlServer");
		}

        public static int getCacheTimeout()
        {
            // Tor 20160303 - This parameter has to reside in the .config file
            return Int32.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("DataCacheTimeoutSeconds"));
        }

        // Tor 20170703 Add paramater for infrequent datacacheTimeouts
        public static int getCacheTimeoutLong()
        {
            // Tor 20160303 - This parameter has to reside in the .config file
            return Int32.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("DataCacheTimeoutSecondsLong"));
        }

        public static String getConnectionString()
		{
            // Tor 20160303 has to reside in .config file - varies between servers
            String str = System.Configuration.ConfigurationManager.AppSettings.Get("connectionString");
			
			//If appsettings is not available (when unittesting)
			if (str==null || str.Length==0)
				str = "Server=(local);Database=flagdata;uid=sa;pwd=;";
			
			return str;
		}

		public static SqlConnection GetConnection(GADataTransaction Transaction)
		{
			//SqlConnection myConnection;
			if (null!=Transaction)
				return  (SqlConnection) Transaction.Connection;
			
            return  new SqlConnection(DataUtils.getConnectionString());
			
			//return myConnection;
		}

		public static int executeNoneQuery(String sql, GADataTransaction Transaction)
		{
			SqlConnection myConnection = DataUtils.GetConnection(Transaction);
			SqlCommand myCommand;
			int result;
			try
			{
				myCommand = new SqlCommand(sql,myConnection);
				if (null!=Transaction)
					myCommand.Transaction = (SqlTransaction) Transaction.Transaction;
				if (myConnection.State == ConnectionState.Closed)
					myConnection.Open();
				result = myCommand.ExecuteNonQuery();
			}
			catch (Exception e)
			{
				throw e;
			}
			finally
			{
				if (null!=myConnection && null==Transaction)
					myConnection.Close();
			}
			return result;
		}

        public static object ExecuteScalar(String sql, GADataTransaction Transaction)
        {
            SqlConnection myConnection = DataUtils.GetConnection(Transaction);
            SqlCommand myCommand;
            object result;
            try
            {
                myCommand = new SqlCommand(sql, myConnection);
                if (null != Transaction)
                    myCommand.Transaction = (SqlTransaction)Transaction.Transaction;
                if (myConnection.State == ConnectionState.Closed)
                    myConnection.Open();
                result = myCommand.ExecuteScalar();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (null != myConnection && null == Transaction)
                    myConnection.Close();
            }
            return result;
        }

        //// Tor 20160515 replace by method public static System.Collections.ArrayList executeSelectSpecial(String sql, String connectionString, string resultFormat)
        //public static SqlDataReader executeSelectSpecial(String sql, String  connectionString)
        //{
        //    SqlConnection myConnection;
        //    SqlCommand myCommand;
        //    SqlDataReader result;
        //    myConnection = new SqlConnection(connectionString);
        //    try
        //    {
        //        myCommand = new SqlCommand(sql, myConnection);
        //        myConnection.Open();
        //        result = myCommand.ExecuteReader(CommandBehavior.CloseConnection);
        //    }
        //    catch (Exception e)
        //    {
        //        // Tor 20151111 moved to finally myConnection.Close();
        //        throw e;
        //    }
        //    // Tor 20151111 added finally to close connection that was opened at the beginning of the method
        //    finally 
        //    {
        //        myConnection.Close();
        //    }
        //    return result;
        //}

        // Tor 20160515 Added method to return result in array
        public static System.Collections.ArrayList executeSelectSpecial(String sql, String connectionString, string resultFormat)
        {
            System.Collections.ArrayList resultList = new System.Collections.ArrayList();
            string STRINGFORMAT="STRING";
            string INTFORMAT="INT";
            string DATETIMEFORMAT="DATETIME";
            string myFormat = string.Empty;
            if (resultFormat.ToUpper() == STRINGFORMAT) myFormat = STRINGFORMAT;
            if (resultFormat.ToUpper() == INTFORMAT) myFormat = INTFORMAT;
            if (resultFormat.ToUpper() == DATETIMEFORMAT) myFormat = DATETIMEFORMAT;
            if (myFormat == string.Empty) return resultList;

            SqlConnection myConnection;
            SqlCommand myCommand;
            SqlDataReader sqlResult;
            string myConnectionString = string.Empty;
            if (connectionString == null)
            {
                myConnectionString = getConnectionString();
            }
            else
            {
                myConnectionString = connectionString;
            }
            myConnection = new SqlConnection(myConnectionString);
            //myConnection = new SqlConnection(connectionString);
            try
            {
                myCommand = new SqlCommand(sql, myConnection);
                myConnection.Open();
                sqlResult=myCommand.ExecuteReader(CommandBehavior.CloseConnection);
                while (sqlResult.Read())
                {
                    if (myFormat == STRINGFORMAT) resultList.Add(sqlResult.GetString(0));
                    if (myFormat == INTFORMAT) resultList.Add(sqlResult.GetInt32(0));
                    if (myFormat == DATETIMEFORMAT) resultList.Add(sqlResult.GetDateTime(0));
                }
            }
            catch (Exception e)
            {
                // Tor 20151111 moved to finally myConnection.Close();
                throw e;
            }
            // Tor 20151111 added finally to close connection that was opened at the beginning of the method
            finally
            {
                myConnection.Close();
            }

            return resultList;
        }

        // Tor 20160417 method for retrieving VerticalListsRowId from record or record above
        public static int getVerticalListsRowIdFromOwner(string fromClass, int fromClassRowId, GADataTransaction Transaction)
        {
            int returnValue=0;
            GADataClass oclass = GADataRecord.ParseGADataClass(fromClass);
            if (oclass.ToString()==string.Empty) return returnValue;
            string verticalFieldId = GASystem.BusinessLayer.Class.GetClassVerticalFieldName(fromClass);
            GADataRecord oclassRecord = new GADataRecord(fromClassRowId, oclass);
            if (verticalFieldId == string.Empty)
            {
                GADataRecord owner = GASystem.BusinessLayer.DataClassRelations.GetOwner(oclassRecord,Transaction);
//                return GASystem.DataAccess.DataAccess.getVerticalFromActionOwner(owner.DataClass.ToString(),owner.RowId);
                return GASystem.DataAccess.DataAccess.getVerticalListsRowIdFromRecord(owner.DataClass.ToString(), owner.RowId);
            }
            else
            {
                return GASystem.DataAccess.DataAccess.getVerticalFromActionOwner(fromClass, fromClassRowId);
            }
        }


        // Tor 20151113 method for retrieving VerticalListsRowId from record and above
        // removed 20160417
//        public static int getVerticalListsRowIdFromOwnerAllLevels(string fromClass, int fromClassRowId, GADataTransaction Transaction)
//        {
//            int returnValue;

//            GADataClass oclass = GADataRecord.ParseGADataClass(fromClass);
//            if (oclass == null) return returnValue;
////            String sql="exec [dbo].[GAGetVerticalFromOwnerAllLevels] '@myClass',@myRowId, @myVerticalRowId = @x OUTPUT";
//            String sql = "GAGetVerticalFromOwnerAllLevels";
////            sql=sql.Replace("@myClass",fromClass);
////            sql=sql.Replace("@myRowId",fromClassRowId.ToString());

//        //    returnValue=(int)executeStoredProcedure(sql,Transaction);
//        //    return returnValue;
//        //}

//        //public static Object executeStoredProcedure(String sql, GADataTransaction Transaction)
//        //{
//            Object returnObject;
//            SqlConnection myConnection = DataUtils.GetConnection(Transaction);
//            SqlCommand myCommand;
//            int returnInt=0;
//            try
//            {
//                myCommand = new SqlCommand(sql, myConnection);
//                myCommand.CommandText = sql;
//                myCommand.CommandType = CommandType.StoredProcedure;

//                // Add the input parameters and set the properties.
//                SqlParameter myClass = new SqlParameter();
//                myClass.ParameterName = "@myClass";
//                myClass.SqlDbType = SqlDbType.NVarChar;
//                myClass.Direction = ParameterDirection.Input;
//                myClass.Value = fromClass;

//                SqlParameter myRowId = new SqlParameter();
//                myRowId.ParameterName = "@myRowId";
//                myRowId.SqlDbType = SqlDbType.Int;
//                myRowId.Direction = ParameterDirection.Input;
//                myRowId.Value = fromClassRowId;

//                SqlParameter myVerticalRowId = new SqlParameter();
//                myVerticalRowId.ParameterName = "@myVerticalRowId";
//                myVerticalRowId.SqlDbType = SqlDbType.Int;
//                myVerticalRowId.Direction = ParameterDirection.Output;
//                //myVerticalRowId.Value = returnInt;

//                // Add the parameter to the Parameters collection. 
//                myCommand.Parameters.Add(myClass);
//                myCommand.Parameters.Add(myRowId);
//                myCommand.Parameters.Add(myVerticalRowId);


//                //myCommand.Parameters.Add("@myClass", SqlDbType.NVarChar);
//                //myCommand.Parameters["@myClass"].Value = fromClass;
//                //myCommand.Parameters.Add("@myRowId", SqlDbType.Int);
//                //myCommand.Parameters["@myRowId"].Value = fromClassRowId;
//                //myCommand.Parameters.Add("@myVerticalRowId", SqlDbType.Int);
//                //myCommand.Parameters["@myVerticalRowId"].Value = "returnInt";

//                if (null != Transaction)
//                    myCommand.Transaction = (SqlTransaction)Transaction.Transaction;
//                if (myConnection.State == ConnectionState.Closed)
//                    myConnection.Open();
//                returnObject = myCommand.ExecuteScalar();
////                returnInt = (int)returnObject;

//                returnInt = (int)myVerticalRowId.Value;

//            }
//            catch (Exception e)
//            {
//                throw e;
//            }
//            finally
//            {
//                if (null != myConnection && null == Transaction)
//                    myConnection.Close();
//            }
//            //return (int)returnObject;
//            return returnInt;
//        }


        public static SqlDataReader executeSelect(String sql)
		{
			return executeSelect(sql, null);
		}

		public static SqlDataReader executeSelect(String sql, GADataTransaction Transaction)
		{
			SqlConnection myConnection;
			SqlCommand myCommand;
			SqlDataReader result = null;
			myConnection = DataUtils.GetConnection(Transaction); //new SqlConnection(connectionString);
			try
			{
				myCommand = new SqlCommand(sql,myConnection);
				if (null!=Transaction)
					myCommand.Transaction = (SqlTransaction) Transaction.Transaction;
				if (myConnection.State == ConnectionState.Closed)
				{
					myConnection.Open();
					result = myCommand.ExecuteReader(CommandBehavior.CloseConnection);
				}
				else
					result = myCommand.ExecuteReader(CommandBehavior.Default);
			}
			catch (Exception e)
			{
                myConnection.Close();
				throw e;
			}
            // Tor 20151111 close connection has been executed if the connection was opened in the method. 
            // ref: result = myCommand.ExecuteReader(CommandBehavior.CloseConnection); above
            //finally
            //{
            //    myConnection.Close();
            //}

			return result;
			//return executeSelect(sql, );
		}
		
		///    <summary>
		///    Converts a SqlDataReader to a DataTable
		///    <param name='reader'>
		/// SqlDataReader to convert.</param>
		///    <returns>
		/// DataTable filled with the contents of the reader.</returns>
		///    </summary>
		public static DataTable convertDataReaderToDataTable(IDataReader reader, String tableName)
		{
			DataSet dataSet = new DataSet();
			DataTable dataTable;
			do
			{
				// Create new data table
				DataTable schemaTable = reader.GetSchemaTable();
				dataTable = new DataTable(tableName);

				if ( schemaTable != null )
				{
					// A query returning records was executed
					for ( int i = 0; i < schemaTable.Rows.Count; i++ )
					{
						DataRow dataRow = schemaTable.Rows[ i ];
						// Create a column name that is unique in the data table
						string columnName = ( string )dataRow[ "ColumnName" ]; //+ "<C" + i + "/>";
						// Add the column definition to the data table
						DataColumn column = new DataColumn( columnName, ( Type )dataRow[ "DataType" ] );
						dataTable.Columns.Add( column );
					}

					//dataSet.Tables.Add( dataTable );
					// Fill the data table we just created
					while ( reader.Read() )
					{
						DataRow dataRow = dataTable.NewRow();
						for ( int i = 0; i < reader.FieldCount; i++ )
							dataRow[ i ] = reader.GetValue( i );
						dataTable.Rows.Add( dataRow );
					}
				}
				else
				{
					// No records were returned
					DataColumn column = new DataColumn("RowsAffected");
					dataTable.Columns.Add(column);
					dataSet.Tables.Add( dataTable );
					DataRow dataRow = dataTable.NewRow();
					dataRow[0] = reader.RecordsAffected;
					dataTable.Rows.Add( dataRow );
				}
			}
			while ( reader.NextResult() );
			reader.Close();
			return dataTable;
		}
		
		///    <summary>
		///    Converts a SqlDataReader to a DataSet
		///    <param name='reader'>
		/// SqlDataReader to convert.</param>
		///    <returns>
		/// DataSet filled with the contents of the reader.</returns>
		///    </summary>
		public static DataSet convertDataReaderToDataSet(IDataReader reader)
		{
			DataSet dataSet = new DataSet();
			dataSet.Tables.Add( convertDataReaderToDataTable(reader, "Table1") );
			return dataSet;
		}

		//FJC = First Join Column
		//SJC = Second Join Column
		public static DataTable Join (DataTable First, DataTable Second, DataColumn[] FJC, DataColumn[] SJC)
		{
			//Create Empty Table
			DataTable table = new DataTable("Join");

			// Use a DataSet to leverage DataRelation
			using(DataSet ds = new DataSet())
			{
				//Add Copy of Tables
				ds.Tables.AddRange(new DataTable[]{First.Copy(),Second.Copy()});
				//Identify Joining Columns from First
				DataColumn[] parentcolumns = new DataColumn[FJC.Length];
				for(int i = 0; i < parentcolumns.Length; i++)
				{
					parentcolumns[i] = ds.Tables[0].Columns[FJC[i].ColumnName];
				}
				//Identify Joining Columns from Second
				DataColumn[] childcolumns = new DataColumn[SJC.Length];
				for(int i = 0; i < childcolumns.Length; i++)
				{
					childcolumns[i] = ds.Tables[1].Columns[SJC[i].ColumnName];
				}

				//Create DataRelation
				DataRelation r = new DataRelation(string.Empty,parentcolumns,childcolumns,false);
				ds.Relations.Add(r);

                //Create Columns for JOIN table
				for(int i = 0; i < First.Columns.Count; i++)
				{
					table.Columns.Add(First.Columns[i].ColumnName, First.Columns[i].DataType);
				}
				for(int i = 0; i < Second.Columns.Count; i++)
				{
					//Beware Duplicates
					if(!table.Columns.Contains(Second.Columns[i].ColumnName))
						table.Columns.Add(Second.Columns[i].ColumnName, Second.Columns[i].DataType);
					else
						table.Columns.Add(Second.Columns[i].ColumnName + "_Second", Second.Columns[i].DataType);
				}

				//Loop through First table
				table.BeginLoadData();
				foreach(DataRow firstrow in ds.Tables[0].Rows)
				{
					//Get "joined" rows
					DataRow[] childrows = firstrow.GetChildRows(r);
					if(childrows != null && childrows.Length > 0)
					{
						object[] parentarray = firstrow.ItemArray; 
						foreach(DataRow secondrow in childrows)
						{
							object[] secondarray = secondrow.ItemArray;
							object[] joinarray = new object[parentarray.Length+secondarray.Length];
							Array.Copy(parentarray,0,joinarray,0,parentarray.Length);
							Array.Copy(secondarray,0,joinarray,parentarray.Length,secondarray.Length);
							table.LoadDataRow(joinarray,true);
						}
					}
				}
				table.EndLoadData();
			}
			return table;
		}

		public static DataTable Join (DataTable First, DataTable Second, DataColumn FJC, DataColumn SJC)
		{
			return DataUtils.Join(First, Second, new DataColumn[]{FJC}, new DataColumn[]{SJC});
		}

		public static DataTable Join (DataTable First, DataTable Second, string FJC, string SJC)
		{
			return DataUtils.Join(First, Second, new DataColumn[]{First.Columns[FJC]}, new DataColumn[]{First.Columns[SJC]});
		}

		
		//Method for updating the identity value in the dataset during inserts.
		//assign to the DataAdapter RowUpdated event
		public static void DataAdapter_RowUpdated(object sender, SqlRowUpdatedEventArgs e)
		{
			//get identity uniqie rowid column name
			//TODO change to look up this column using primary key definitions
			string RowIdColumn = "";
			RowIdColumn = e.Row.Table.TableName.Substring(2) + "RowId";
			
			if (e.StatementType == StatementType.Insert && e.Status == UpdateStatus.Continue)
			{
                SqlCommand oCmd = new SqlCommand("SELECT SCOPE_IDENTITY()", e.Command.Connection);
				if (null != e.Command.Transaction) 
					oCmd.Transaction = e.Command.Transaction;
                int scopeValue = (int)oCmd.ExecuteScalar();
				e.Row.Table.Columns[RowIdColumn].ReadOnly = false;
                e.Row[RowIdColumn] = scopeValue;
				e.Row.Table.Columns[RowIdColumn].ReadOnly = true;
				e.Row.AcceptChanges();
			}
		}

//		public static DataSet FillNewDataSet(string sql, GADataClass DataClass) 
//		{
//			SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
//			SqlDataAdapter da = new SqlDataAdapter(sql, myConnection);
//			DataSet ds = DatasetFactory.NewDataSet(DataClass);
//			da.Fill(ds, DataClass.ToString());
//			myConnection.Close();
//			return ds;
//		}

        /// <summary>
        /// Generate a generic date filter for matching records to a defined date range
        /// </summary>
        /// <param name="DateFromField">Date from field in table</param>
        /// <param name="DateToField">Date to field in table</param>
        /// <returns>A string containg the specified filter. The string contains @dateFrom and @DateTo elements for using sql parameters </returns>
        public static string GenerateDateSpanFilter(string DateFromField, string DateToField)
        {
            //todate in timespan
            string filterto = " ( @dateFrom <= _formDateField_  and _formDateField_ <= @DateTo     ) ";

            //from date in timespan
            string filterfrom = " ( @dateFrom <= _toDateField_  and _toDateField_ <= @DateTo     ) ";

            //timespan between start and end
            string filterin = " ( @dateFrom >= _formDateField_  and _toDateField_ >= @DateTo     ) ";

            //from is null
            string filterfromnull = " ( _formDateField_ is null and _toDateField_ >= @dateFrom     ) ";
            //to is null
            string filtertonull = " ( _toDateField_ is null  and _formDateField_ <= @DateTo     ) ";
            //both is null
            string filternull = " ( _toDateField_ is null  and _formDateField_ is null     ) ";

            //combined 
            string filter = " (" + filterto + " or " + filterfrom + " or " + filterin + " or " + filterfromnull + " or " + filtertonull + " or " + filternull + ") ";

            filter = filter.Replace("_formDateField_", DateFromField);
            filter = filter.Replace("_toDateField_", DateToField);

            return filter;
        }

        public static string GenerateDateSpanFilter(string DateFromField, string DateToField, string DataClass)
        {
            //todate in timespan
            string filterto = " ( @dateFrom <= _formDateField_  and _formDateField_ <= @DateTo     ) ";

            //from date in timespan
            string filterfrom = " ( @dateFrom <= _toDateField_  and _toDateField_ <= @DateTo     ) ";

            //timespan between start and end
            string filterin = " ( @dateFrom >= _formDateField_  and _toDateField_ >= @DateTo     ) ";

            //from is null
            string filterfromnull = " ( _formDateField_ is null and _toDateField_ >= @dateFrom     ) ";
            //to is null
            string filtertonull = " ( _toDateField_ is null  and _formDateField_ <= @DateTo     ) ";
            //both is null
            string filternull = " ( _toDateField_ is null  and _formDateField_ is null     ) ";

            //combined 
            string filter = " (" + filterto + " or " + filterfrom + " or " + filterin + " or " + filterfromnull + " or " + filtertonull + " or " + filternull + ") ";

            filter = filter.Replace("_formDateField_", DataClass + "." + DateFromField);
            filter = filter.Replace("_toDateField_", DataClass + "." + DateToField);

            return filter;
        }

        /// <summary>
        /// Generate a date filter for matching record agains a given date.
        /// </summary>
        /// <param name="cd">Class description for the requested table</param>
        /// <returns>A string containing the requested date filter. The string contains @dateFrom and @DateTo elements for using sql parameters </returns>
        public static string GenerateDateFilter(AppUtils.ClassDescription cd)
        {
            string filter = string.Empty;
            filter = " ((@dateFrom <= _dateField_  and _dateField_ <= @DateTo) or (_dateField_ is null)    )";
            filter = filter.Replace("_dateField_", cd.DataClassName + "." + cd.DateField);
            return filter;
        }
	}
}
