using System;
using GASystem;
using GASystem.DataModel;


namespace GASystem.DataAccess.Utils.SQLView
{
	/// <summary>
	/// Base class for generating select statements for a view. 
	/// The select statement should take into account the owner context passed in by the parameter Owner
	/// From and To dates can be added to the query by using the sql parmameters @dateFrom and @dateTo
	/// </summary>
	public abstract class ISQLView
	{
		public AppUtils.ClassDescription _cd;
		private GADataRecord _owner;

		public ISQLView(AppUtils.ClassDescription cd, GADataRecord Owner)
		{
			//
			// TODO: Add constructor logic here
			//
			_cd = cd;
			_owner = Owner;
		}

		
		public ISQLView(AppUtils.ClassDescription cd)
		{
			//
			// TODO: Add constructor logic here
			//
			_cd = cd;
			_owner = null;
		}

		protected AppUtils.ClassDescription ClassDesc 
		{	
			get {return _cd;}
		}

		protected GADataRecord Owner 
		{	
			get {return _owner;}
		}

		/// <summary>
		/// Get the full sql statement for this view
		/// </summary>
		/// <returns></returns>
		public abstract string GetSQLViewQuery();


		protected string GenerateDateFilter() 
		{
			//if classdescription has a datafield definition use this
			string filter = string.Empty;
			if (ClassDesc != null && ClassDesc.DateField != string.Empty) 
			{
                filter = " ( (@dateFrom <= _formDateField_  and _toDateField_ <= @DateTo) or (_formDateField_ is null)     )";
                filter = filter.Replace("_formDateField_", ClassDesc.DataClassName + "." + ClassDesc.DateField);
                filter = filter.Replace("_toDateField_", ClassDesc.DataClassName + "." + ClassDesc.DateField);
			}
			return filter;
		}

		protected string generateDateSpanFilter() 
		{
			string filter = string.Empty;
			if (ClassDesc.hasDateFromField() && ClassDesc.hasDateToField()) 
			{
				filter = generateDateSpanFilter(ClassDesc.DateFromField, ClassDesc.DateToField);
				
			}

			return filter;
		}

		protected string generateDateSpanFilter(string fieldFrom, string FieldTo) 
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

			filter = filter.Replace("_formDateField_", fieldFrom);
			filter = filter.Replace("_toDateField_", FieldTo);

			return filter;
		}

        public virtual string getByRowId(int rowid)
        {
            return GetSQLViewQuery() + " and " + _cd.DataClassName.Substring(2) + "RowId = " + rowid.ToString();
        }

	}
}
