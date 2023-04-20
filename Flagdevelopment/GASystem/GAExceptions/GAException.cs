using System;
using System.Reflection;
using System.Text;

namespace GASystem.GAExceptions
{
	/// <summary>
	/// Summary description for GAException.
	/// </summary>
	public class GAException : Exception
	{
		
		private string _debugMessage = "";
	
		public GAException() : base()
		{
			
		}

		public GAException(string Message) : base(Message)
		{

		}

		public GAException(string Message, Exception InnerException) : base(Message, InnerException)
		{

		}

		public void SetDebugMessage(MethodInfo Method, object[] MethodParameterValues)
		{
			try
			{
				StringBuilder debugMessage = new StringBuilder();
				debugMessage.Append("\nMethod parameters for "+Method.Name+"() : \n");

				int parameterIndex = 0;
				foreach ( ParameterInfo paramInfo in Method.GetParameters())
				{
					debugMessage.Append(paramInfo.Name).Append(" = ");
					debugMessage.Append(MethodParameterValues[parameterIndex++].ToString()).Append("\n");
				}
				_debugMessage = debugMessage.ToString();
			}
			catch (Exception e)
			{
				_debugMessage = "Error extracting method parameters : " + e.Message;
			}
		}
		

		public string DebugMessage
		{
			get { return _debugMessage; }
			set { _debugMessage = value; }
		}

		
		
		
	}
}
