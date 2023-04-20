using System;
using System.Data;
using GASystem.DataModel;

namespace GASystem.BusinessLayer.Validate
{
	/// <summary>
	/// Validate a ga dataset. Throws GAValidationException if invalid
	/// </summary>
	public class DataSetValidator
	{
		private DataSet _recordSet;
		private GADataClass _dataClass;
		
		public DataSetValidator()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public GADataClass DataClass 
		{
			get {return _dataClass;}
			set {_dataClass = value;}
		}

		public DataSet Recordset
		{
			get{return _recordSet;}
			set{_recordSet = value;}
		}

		public void CheckLength() 
		{
			DataTable table = Recordset.Tables[DataClass.ToString()];	
			foreach(DataColumn c in table.Columns) 
			{
		//		FieldDescription fieldDesc = FieldDefintion.GetFieldDescription(c.ColumnName, c.Table.TableName);
			
			}
		}
	}
}
