using System;
using System.Data;
using System.Collections;
using GASystem.DataModel;

namespace GASystem.GAGUIEvents
{
	/// <summary>
	/// Summary description for GACommandEventArgs.
	/// </summary>
	public class GACommandEventArgs
	{
		private string _commandName;
		private string _commandStringArgument;
		private int _commandIntArgument = 0;
		private DataSet _commandDataSetArgument;
		private Hashtable _commandHashTableArgument;
		private GADataRecord _commandDataRecordArgument;

		public string CommandName
		{
			get
			{
				return null==_commandName ? "" : _commandName;
			}
			set
			{
				_commandName = value;
			}
		}

		public string CommandStringArgument
		{
			get
			{
				return null==_commandStringArgument ? "" : _commandStringArgument;
			}
			set
			{
				_commandStringArgument = value;
			}
		}

		public DataSet CommandDataSetArgument
		{
			get
			{
				return _commandDataSetArgument;
			}
			set
			{
				_commandDataSetArgument = value;
			}
		}
		public Hashtable CommandHashTableArgument
		{
			get
			{
				return _commandHashTableArgument;
			}
			set
			{
				_commandHashTableArgument = value;
			}
		}


		public int CommandIntArgument
		{
			get
			{
				return _commandIntArgument;
			}
			set
			{
				_commandIntArgument = value;
			}
		}
		public GADataRecord CommandDataRecordArgument
		{
			get
			{
				return _commandDataRecordArgument;
			}
			set
			{
				_commandDataRecordArgument = value;
			}
		}
		
	}

	public delegate void GACommandEventHandler(object sender, GACommandEventArgs e);
}
