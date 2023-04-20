using System;

namespace GASystem.GAControls.NavigationWheel
{
	/// <summary>
	/// Summary description for Label.
	/// </summary>
	public class LabelPlaceholder
	{
		private string _name = null;
		private int _x = -1;
		private int _y = -1;

		public string name
		{
			get
			{
				return _name;
			}
			set
			{
				_name = value;
			}
		}

		public int x 
		{
			get
			{
				return _x;
			}
			set
			{
				_x = value;
			}
		}

		public int y
		{
			get
			{
				return _y;
			}
			set
			{
				_y = value;
			}
		}

		public LabelPlaceholder(string name, int x, int y)
		{
			this.name = name;
			this.x = x;
			this.y = y;
		}
	}
}
