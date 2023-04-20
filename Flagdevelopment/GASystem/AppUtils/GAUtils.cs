using System;
using GASystem.DataModel;

namespace GASystem.AppUtils
{
	/// <summary>
	/// Summary description for GAUtils.
	/// </summary>
	public class GAUtils
	{
		public GAUtils()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		/// <summary>
		/// Use this function to check if a given string may be convertet to a int (using int.Parse())
		/// </summary>
		/// <param name="num"></param>
		/// <returns></returns>
		public static bool IsNumeric(string num)
		{
			bool isNum = false;
			try
			{
				int i = int.Parse(num);
				isNum = true; //if this statement is reached, num is numeric
			}
			catch (Exception ex) {}
			return isNum;

		}

		public static bool IsDate(string sdate)
		{
			DateTime dt;
			bool isDate = true;
			try
			{
				dt = DateTime.Parse(sdate);
			}
			catch
			{
				isDate = false;
			}
			return isDate;
		}


		public static bool IsGADataClass(string className)
		{
			bool isDataClass = false;
			try
			{
				GADataRecord.ParseGADataClass(className);
				isDataClass = true;
			}
			catch
			{
				
			}
			return isDataClass;
		}


		/// <summary>
		/// Convert an array of object to a string. Each item is convert to a string and appended together 
		/// </summary>
		/// <param name="items">Array</param>
		/// <returns>String of all elements appended together</returns>
		
		public static string ConvertArrayToString(object[] items) 
		{
			return ConvertArrayToString(items, string.Empty);
		}


		/// <summary>
		/// Convert an array of object to a string. Each item is convert to a string and appended together using
		/// the specified seperator
		/// </summary>
		/// <param name="items">Array</param>
		/// <param name="Seperator">string seperator e.g. ", "</param>
		/// <returns>String of all elements appended together</returns>
		public static string ConvertArrayToString(object[] items, string Seperator) 
		{
			if (items.Length == 0 )
				return string.Empty;

			//build a comma seperated string of listrow ids.  TODO move this to a general utility function.
			System.Text.StringBuilder sb = new System.Text.StringBuilder(items[0].ToString());
			for (int t=1; t<items.Length; t++) 
			{
				sb.Append(Seperator);
				sb.Append(items[t]);
			}

			return sb.ToString();
		}


	}
}
