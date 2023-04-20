using System;
using GASystem.GAExceptions;
using log4net;

namespace GASystem.AppUtils
{
	/// <summary>
	/// Summary description for ErrorUtil.
	/// </summary>
	public class ErrorUtil
	{
		public ErrorUtil()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static GAException AnalyzeAndLogError (Exception Ex, Type ErrorSourceClass)
		{
			try
			{
				ILog _logger = LogManager.GetLogger(ErrorSourceClass);
				//Set up default excpetion with default message
				String message = Localization.GetExceptionMessage("StandardGAErrorMessage");
				GAException gaEx = new GAException(message, Ex);
				
				//analyze error and derive useful message
				if (Ex.GetType() == typeof(GAException))
				{
					
				}

				_logger.Error(message, Ex);
				return gaEx;
			}
			catch (Exception ex)
			{
				return new GAException("An exception occured while trying to analyse an exception!", Ex);
			}
		
		}
	}
}
