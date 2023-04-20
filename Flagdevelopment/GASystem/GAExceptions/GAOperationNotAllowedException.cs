using System;


namespace GASystem.GAExceptions
{
	/// <summary>
	/// Summary description for GAOperationNotAllowedException.
	/// </summary>
	public class GAOperationNotAllowedException : GAException
	{
		public GAOperationNotAllowedException() : base()
		{
			
		}

		public GAOperationNotAllowedException(string Message) : base(Message)
		{

		}

		public GAOperationNotAllowedException(string Message, Exception InnerException) : base(Message, InnerException)
		{

		}
	}
}
