using System;

namespace GASystem.GAExceptions
{
	/// <summary>
	/// Summary description for GAValidationException.
	/// </summary>
	public class GAValidationException : GAException
	{
		public GAValidationException() : base()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public GAValidationException(string Message, Exception InnerException) : base(Message, InnerException)
		{

		}

	}
}
