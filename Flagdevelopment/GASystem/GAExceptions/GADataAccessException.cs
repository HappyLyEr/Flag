using System;

namespace GASystem.GAExceptions
{
	/// <summary>
	/// Summary description for GAValidationException.
	/// </summary>
	public class GADataAccessException : GAException
	{
		public GADataAccessException() : base()
		{
			//
			// TODO: Add constructor logic here
			//
		}

        public GADataAccessException(string Message)
            : base(Message)
		{

		}


        public GADataAccessException(string Message, Exception InnerException) : base(Message, InnerException)
		{

		}

	}
}
