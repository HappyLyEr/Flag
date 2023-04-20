using System;
using System.Xml;
using System.Collections;

namespace GASystem.GAControls.NavigationWheel
{
	/// <summary>
	/// NavigasjonshjulController is not a real controller, but keeps track of 
	/// data which the navigation wheel uses
	/// </summary>
	public class NavigasjonshjulController
	{
		private static int INITIAL_SECTION_COUNT = 4;
		private ArrayList _sections = new ArrayList(INITIAL_SECTION_COUNT);
		private Queue _lastOrderedSections = null;
		private Hashtable _properties = new Hashtable();
		private ArrayList _labelPlaceholders = new ArrayList(INITIAL_SECTION_COUNT);
		private int _currentSectionTabId = -1;

		public NavigasjonshjulController(string configPath)
		{
			init(configPath);
		}

		public void init(string configPath)
		{
			if (configPath == null) return;

			XmlDocument xmldoc = new XmlDocument();
			xmldoc.Load(configPath);

			// Load all sections from config file
			XmlNodeList l_sections = xmldoc.SelectNodes("/config/sections/section");
			int sectionIndex = 0;
			foreach (XmlNode s in l_sections)
			{
				XmlNode l_name = s.Attributes.GetNamedItem("name");
				XmlNode l_src = s.Attributes.GetNamedItem("src");
				XmlNode l_tabid = s.Attributes.GetNamedItem("tabid");

				if (l_name != null)
				{
					if (l_src == null && l_tabid != null)
					{
						addSection(sectionIndex, l_name.Value.ToString(), Convert.ToInt32(l_tabid.Value.ToString()));
					} 
					else if (l_src != null && l_tabid == null)
					{
						addSection(sectionIndex, l_name.Value.ToString(), l_src.Value.ToString());
					}
				}
				sectionIndex++;
			}

			// Set currentSectionTabId to first element in array
			if (_sections[0] != null)
				_currentSectionTabId = ((Section)_sections[0]).tabid;

			// Load other properties
			XmlNodeList l_properties = xmldoc.SelectNodes("/config/properties/property");
			foreach (XmlNode s in l_properties)
			{
				XmlNode l_name = s.Attributes.GetNamedItem("name");
				XmlNode l_value = s.Attributes.GetNamedItem("value");

				if (l_name != null && l_value != null)
				{
					_properties.Add(l_name.Value.ToString(), l_value.Value.ToString());
				}
			}

			// Load label properties
			XmlNodeList l_labelProperties = xmldoc.SelectNodes("/config/labels/label");
			foreach (XmlNode b in l_labelProperties)
			{
				try
				{
					//XmlNode l_order = b.Attributes.GetNamedItem("order");
					XmlNode l_name = b.Attributes.GetNamedItem("name");
					XmlNode l_x = b.Attributes.GetNamedItem("x");
					XmlNode l_y = b.Attributes.GetNamedItem("y");

					if (l_name != null && l_x != null && l_y != null)
					{
						LabelPlaceholder nLp = new LabelPlaceholder(l_name.Value.ToString(), Convert.ToInt32(l_x.Value.ToString()), Convert.ToInt32(l_y.Value.ToString()));
						_labelPlaceholders.Add(nLp);
					}
				}
				catch (Exception ex)
				{
					
				}
			}

		}

		public void addSection(int index, string name, string src)
		{
			Section s = new Section(index, name, src);
			_sections.Add(s);
		}

		public void addSection(int index, string name, int tabid)
		{
			Section s = new Section(index, name, tabid);
			_sections.Add(s);
		}

		public ICollection getSections()
		{
			return (ICollection)_sections;
		}

		public object getProperty(String name)
		{
			if (_properties.ContainsKey(name))
				return _properties[name];

			return null;
		}

		public ICollection getLabelPlaceholders()
		{
			return (ICollection)_labelPlaceholders;
		}

		public int getCurrentSectionTabId()
		{
			return _currentSectionTabId;
		}

		public Queue getSectionsOrdered()
		{
			Queue abc = null;
			try
			{
				abc = new Queue(getSections());
				
				int currentSection = getCurrentSectionTabId();
				if (((Section)abc.Peek()).tabid != currentSection)
				{
					Object o = abc.Dequeue();
					abc.Enqueue(o);
					while (((Section)abc.Peek()).tabid != currentSection && !abc.Peek().Equals(o))
					{
						Section s = (Section)abc.Dequeue();
						abc.Enqueue(s);
					}
				}
			}
			catch (Exception ex)
			{
				//Trace.Warn("");
			}

			_lastOrderedSections = abc;

			return _lastOrderedSections;
		}

		public void updateCurrentSection(int x, int y)
		{
			for (int i = 0; i<_labelPlaceholders.Count;i++)
			{
				LabelPlaceholder lbl = (LabelPlaceholder)_labelPlaceholders[i];

				int x1, x2, y1, y2 = -1;
				x1 = lbl.x - 100;
				x2 = lbl.x + 100;
				y1 = lbl.y - 15;
				y2 = lbl.y + 15;

				if (x < x2 && x > x1)
				{
					if (y < y2 && y > y1)
					{
						_currentSectionTabId = ((Section)_lastOrderedSections.ToArray()[i]).tabid;
					}
				}
			}
			
		}

	}
}
