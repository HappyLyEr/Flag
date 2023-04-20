using System;
using System.Collections;
using GASystem.DataModel;
using GASystem.AppUtils;
using GASystem.DataAccess.Utils;

namespace GASystem.GUIUtils
{
	/// <summary>
	/// Summary description for ShortcutLinks.
	/// </summary>
	public class ShortcutLinks
	{
        // Tor 20170428 add hashtable to remember shortcutlinks for ownerclass
        // Tor 20170703 private static Hashtable shortcutArrayHash = new Hashtable();

		public ShortcutLinks()
		{
			//
			// TODO: Add constructor logic here
			//
		}

        // Tor TODO get list from GASuperClassLinks
// Tor 20170703 revert to old model
//        public static ArrayList getAllLinks()
//        {
//            GADataRecord currentContextRecord = GASystem.AppUtils.SessionManagement.GetDefaultDataContext().InitialContextRecord;
//            string ownerClass = currentContextRecord.DataClass.ToString();

//            object param = shortcutArrayHash[ownerClass];

//            if (param != null)
//                return (ArrayList)param;

//// add new entry to hastable

//            ArrayList linkList = new ArrayList();
//            string[] dataClasses = getLinkDefinition().Split(',');
//            foreach (string linkClass in dataClasses)
//            {
//                try
//                {
//                    linkList.Add(new ShortCutLinkElement(GADataRecord.ParseGADataClass(linkClass.Trim())));
//                }
//                catch (GAExceptions.GAException ex)
//                {
//                    //ignore this class
//                    //TODO log!!!
//                }
//            }

//            // get all unique memberclasses below ownerClass
//            ArrayList memberClasses = new ArrayList();
//            memberClasses = GASystem.DataAccess.SuperClassDb.GetAllMemberClassesBelowOwnerClass(ownerClass);
//            bool found = false;
//            foreach (string newClass in memberClasses)
//            { // if not already in linkClass
//                found = false;
//                foreach (string linkClass in dataClasses)
//                {
//                    if (linkClass == newClass) found = true;
//                }
//                if (!found) linkList.Add(new ShortCutLinkElement(GADataRecord.ParseGADataClass(newClass)));

//            }

//            // add to hashtable
//            shortcutArrayHash.Add(ownerClass, linkList);

//            return linkList;
//        }

        // Tor 20170703 use DataCache for caching List
        // Tor 20170428
        public static ArrayList getAllLinks()
        {
            // Tor 20151019 reads classes from GASuperClassLinks from the user's current Home class
            // add start
            // Get Home Class
            //            GADataRecord currentContextRecord=GASystem.AppUtils.SessionManagement.GetCurrentDataContext().SubContextRecord;
            //          GADataRecord currentContextRecord = GASystem.AppUtils.SessionManagement.GetCurrentDataContext().InitialContextRecord;
            GADataRecord currentContextRecord = GASystem.AppUtils.SessionManagement.GetDefaultDataContext().InitialContextRecord;
            
            // 优先从缓存中提取链接
            string ownerClass = currentContextRecord.DataClass.ToString();
            string currentLanguage = Localization.GetCurrentLanguage();// 当前语言环境

            DataCache.ValidateCacheLong(DataCache.DataCacheType.ShortcutLinks);
            ArrayList cachedObject = (ArrayList)DataCache.GetCachedObject(DataCache.DataCacheType.ShortcutLinks, ownerClass, currentLanguage);// 增加language标识，支持多语言切换
            if (cachedObject != null)
            {
                return cachedObject;
            }

            // 缓存中未找到链接，生成新的链接列表，并进行缓存处理
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

            // get all unique memberclasses below ownerClass
            ArrayList memberClasses = new ArrayList();
            memberClasses = GASystem.DataAccess.SuperClassDb.GetAllMemberClassesBelowOwnerClass(ownerClass);
            foreach (string newClass in memberClasses)
            { // add if not already in linkClass
                bool found = false;
                foreach (string linkClass in dataClasses)
                {
                    if (linkClass == newClass)
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    linkList.Add(new ShortCutLinkElement(GADataRecord.ParseGADataClass(newClass)));
                }

            }
            // add end

            DataCache.AddCachedObject(DataCache.DataCacheType.ShortcutLinks, ownerClass, linkList, currentLanguage);

            return linkList;
        }

        //// Tor 20170703 revert to old model
        //// Tor 20170428
        //public static ArrayList getAllLinks()
        //{
        //    ArrayList linkList = new ArrayList();
        //    string[] dataClasses = getLinkDefinition().Split(',');
        //    foreach (string linkClass in dataClasses)
        //    {
        //        try
        //        {
        //            linkList.Add(new ShortCutLinkElement(GADataRecord.ParseGADataClass(linkClass.Trim())));
        //        }
        //        catch (GAExceptions.GAException ex)
        //        {
        //            //ignore this class
        //            //TODO log!!!
        //        }
        //    }

        //    // Tor 20151019 reads classes from GASuperClassLinks from the user's current Home class
        //    // add start
        //    // Get Home Class
        //    //            GADataRecord currentContextRecord=GASystem.AppUtils.SessionManagement.GetCurrentDataContext().SubContextRecord;
        //    //          GADataRecord currentContextRecord = GASystem.AppUtils.SessionManagement.GetCurrentDataContext().InitialContextRecord;
        //    GADataRecord currentContextRecord = GASystem.AppUtils.SessionManagement.GetDefaultDataContext().InitialContextRecord;

        //    string ownerClass = currentContextRecord.DataClass.ToString();
        //    // get all unique memberclasses below ownerClass
        //    ArrayList memberClasses = new ArrayList();
        //    memberClasses = GASystem.DataAccess.SuperClassDb.GetAllMemberClassesBelowOwnerClass(ownerClass);
        //    bool found = false;
        //    foreach (string newClass in memberClasses)
        //    { // add if not already in linkClass
        //        found = false;
        //        foreach (string linkClass in dataClasses)
        //        {
        //            if (linkClass == newClass) found = true;
        //        }
        //        if (!found) linkList.Add(new ShortCutLinkElement(GADataRecord.ParseGADataClass(newClass)));

        //    }
        //    // add end

        //    return linkList;
        //}
        
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
// Tor 20160127				return System.Configuration.ConfigurationManager.AppSettings.Get("HomeContextLinkClasses");
                return new GASystem.AppUtils.FlagSysResource().GetResourceString("HomeContextLinkClasses");
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
                _dataClassTabId = LinkUtils.GetTabId(this._dataClass);
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
