using System;

namespace GASystem.WebControls.EditControls.HazardMatrix
{
	/// <summary>
	/// Summary description for AxisInfo.
	/// </summary>
	public class AxisInfo
	{
		private String[] _labels = null;
		private String _title = null;
		private int _selectedIndex = -1;

		public AxisInfo(String title, int size)
		{
			_labels = new String[size];
			_title = title;
		}

		public AxisInfo(String title, String[] labels)
		{
			_labels = labels;
			_title = title;
		}

		public AxisInfo(String[] labels)
		{
			_labels = labels;
			_title = "";
		}

		public string GetTitle()
		{
			return _title;
		}

		public string GetLabel(int index)
		{
			if (index <= _labels.GetLength(0) && index >= 0)
				return _labels[index];

			return "";
		}

		public void SetLabel(int index, String label)
		{
			if (index <= _labels.GetLength(0) && index >= 0)
			{
				_labels[index] = label;
			}
		}

		public void SetSelectedIndex(int index)
		{
			if (index <= _labels.GetLength(0) && index >= 0)
			{
				_selectedIndex = index;
			} 
			else 
			{
				throw new ArgumentOutOfRangeException("index", index, "Cannot set SelectedIndex to " + index + ". Expecting a value between 0 and " + _labels.GetLength(0));
			}
		}

	}
}
