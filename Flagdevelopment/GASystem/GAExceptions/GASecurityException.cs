using System;

namespace GASystem.GAExceptions
{
	/// <summary>
	/// Summary description for GASecurityException.
	/// </summary>
	public class GASecurityException : GAException
	{
		public GASecurityException() : base()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public GASecurityException(string Message) : base(Message)
		{

		}

		public GASecurityException(string Message, Exception InnerException) : base(Message, InnerException)
		{

		}
	}
}
