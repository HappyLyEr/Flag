using System;

namespace GASystem.GAExceptions
{
	/// <summary>
	/// Summary description for GADataClassNotSupportedException.
	/// </summary>
	public class GADataClassNotSupportedException : GAException
	{
		public GADataClassNotSupportedException() : base()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public GADataClassNotSupportedException(string Message) : base(Message)
		{

		}

		public GADataClassNotSupportedException(string Message, Exception InnerException) : base(Message, InnerException)
		{

		}
	}
}
