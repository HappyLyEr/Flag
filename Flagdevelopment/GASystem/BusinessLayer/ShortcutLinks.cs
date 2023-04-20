using System;
using System.Collections;
using GASystem.DataModel;
using GASystem.AppUtils;

namespace GASystem.BusinessLayer
{
	/// <summary>
	/// Summary description for ShortcutLinks.
	/// </summary>
	public class ShortcutLinks
	{
		public ShortcutLinks()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		
		public static ArrayList getAllLinks() 
		{
			ArrayList linkList = new ArrayList();
			string[] dataClasses = getLinkDefinition().Split(',');
			foreach (string linkClass in dataClasses) 
			{
				try 
				{
					linkList.Add(new ShortCutLinkElement(GADataRecord.ParseGADataClass(linkClass.Trim())));
				} 
				catch (GAExceptions.GAException ex) 
				{
					//ignore this class
					//TODO log!!!
				}
			}
			return linkList;

		}

		private static string getLinkDefinition() 
		{
			if (HomeContextLinkClasses == null)
                return "GARemedialActionView, GAAction, GAEmployment, GAFile, GAAudit, GAManageChange, GAOpportunity, GArxtReport, GALocation, GAMeeting, GADaysReport, GAProject, GASafetyObservation, GACourse";
			return HomeContextLinkClasses;
		}

		private static string HomeContextLinkClasses 
		{
			get 
			{
				return System.Configuration.ConfigurationManager.AppSettings.Get("HomeContextLinkClasses");
			}
		}

	}

	public class ShortCutLinkElement 
	{
		private string _dataClass = string.Empty;
		private string _dataClassName = string.Empty;
		private int _dataClassTabId = -1;

		public ShortCutLinkElement(GADataClass DataClass) 
		{
			try 
			{
				_dataClass = DataClass.ToString();
				_dataClassName = String.Format(Localization.GetGuiElementText("AllRecords"), Localization.GetGuiElementTextPlural(DataClass.ToString()));
				_dataClassTabId = int.Parse(System.Configuration.ConfigurationManager.AppSettings.Get(DataClass.ToString() + "Details" + "TabId"));
			} 
			catch (FormatException ex) 
			{
				throw new GAExceptions.GAException("There is no tab for this dataclass");
			}
		}

		public string DataClass 
		{
			get {return _dataClass;}
		}

		public string DataClassName 
		{
			get {return _dataClassName;}
		}

		public int DataClassTabId 
		{
			get {return _dataClassTabId;}
		}
	}
	
}
