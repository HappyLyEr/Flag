using System;
using GASystem.DataModel;
using System.Web;
using GASystem.AppUtils;


namespace GASystem.GUIUtils
{
	/// <summary>
	/// Helper class for getting values from the querystring. 
	/// </summary>
	public class QuerystringUtils
	{
		private GADataClass myDataClass;
		private HttpRequest myRequest;
		public const string WORKITEMCURRENTUSERONLY = "wcurrentuser";
		
		public QuerystringUtils(GADataClass DataClass, HttpRequest Request)
		{
			myDataClass = DataClass;
			myRequest = Request;
		}

		/// <summary>
		/// Check querystring for whether workitem list should be filtered by current user.
		/// Defaults to false. 
		/// </summary>
		public bool FilterWorkitemByCurrentUser 
		{
			get 
			{
				if (null!=myRequest[WORKITEMCURRENTUSERONLY] &&  myRequest[WORKITEMCURRENTUSERONLY].ToUpper() == "TRUE")
					return true;
				return false;
			}
		}

		/// <summary>
		/// Get RowId from querystring
		/// </summary>
		/// <returns>RowId. Returns -1 if no rowid is present</returns>
		public int GetRowId() 
		{
			if (null!=myRequest[RowIdName] &&  GAUtils.IsNumeric(myRequest[RowIdName].ToString()))
				return  int.Parse(myRequest[RowIdName]);
			return -1;
		}


		public string GetRowIdAsString()
		{
			if (null!=myRequest[RowIdName])
				return myRequest[RowIdName].ToString();
			return string.Empty;
		}
		/// <summary>
		/// Get edit flag from querystring. True if "edit=true" is present
		/// </summary>
		public bool EditRecord 
		{
			get 
			{
				if (null!=myRequest["edit"] && myRequest["edit"].ToString() == "true")
					return true;
				return false;
			}
		}

		/// <summary>
		/// Get createMultipleRecords flag from querystirng. True if multiple=true is present
		/// </summary>
		public bool createMultipleRecords 
		{
			get 
			{
				if (null!=myRequest["multiple"] && myRequest["multiple"].ToString().ToUpper() == "TRUE")
					return true;
				return false;
			}
		}
		
		/// <summary>
		/// Gets the name of the dataclass without the ga prefix
		/// </summary>
		/// <returns></returns>
		private string RowIdName
		{
			get {return myDataClass.ToString().Substring(2) + "RowId";}
		}


	}
}
