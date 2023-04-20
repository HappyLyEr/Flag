using System;
using System.IO;
using System.Configuration;
using System.Web;

namespace GASystem.AppUtils
{
	/// <summary>
	/// Summary description for Logger.
	/// </summary>
	public class Logger
	{
		
		
		public Logger()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static void LogInfo(String origin, String message)
		{
			try 
			{
				
				StreamWriter sw = new StreamWriter(HttpContext.Current.Server.MapPath("log.txt"), true);
				sw.WriteLine(DateTime.Now.ToString()+"::INFO::"+origin+"::"+message);
				sw.Close();
			}
			catch (Exception e)
			{
			}
		}


		public static void LogError(String origin, String message, Exception ex)
		{
			try 
			{
				if (ex!=null) message += ex.StackTrace;
				StreamWriter sw = new StreamWriter(HttpContext.Current.Server.MapPath("log.txt"), true);
				sw.WriteLine(DateTime.Now.ToString()+"::ERROR::"+origin+"::"+message);
				sw.Close();
			}
			catch (Exception e)
			{
			}
		}

		
	}
}
