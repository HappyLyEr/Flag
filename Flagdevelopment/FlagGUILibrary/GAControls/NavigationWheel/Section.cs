using System;

namespace GASystem.GAControls.NavigationWheel
{
	/// <summary>
	/// Summary description for Section.
	/// </summary>
	public class Section
	{
		private string _name;
		private string _src;
		private int _tabid;
		private int _index;

		public Section(int index, string name, string src)
		{
			this.index = index;
			this.name = name;
			this.src = src;
		}

		public Section(int index, string name, int tabid)
		{
			this.index = index;
			this.name = name;
			this.tabid = tabid;
		}

		public int index
		{
			get
			{
				return _index;
			}
			set
			{
				_index = value;
			}
		}

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

		public string src
		{
			get 
			{
				return _src;
			}
			set
			{
				_src = value;
			}
		}

		public int tabid
		{
			get 
			{
				return _tabid;
			}
			set
			{
				_tabid = value;
			}
		}


	}
}
