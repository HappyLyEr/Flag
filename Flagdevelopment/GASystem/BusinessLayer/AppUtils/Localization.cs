using GASystem.DataModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GASystem.AppUtils
{
    /// <summary>
    /// Summary description for Localization.
    /// </summary>
    public class Localization
    {
        private const string LANGUAGE_RESOURCE_KEY = "LANGUAGE_RESOURCE_KEY";

        private static String _cultureCode = "no";
        private static String _languageEnglishCode = "EN";

        private static ResourceManager _captionTextRm;
        private static ResourceManager _guielementsRm;
        private static ResourceManager _errorTextRm;
        private static ResourceManager _userTextRm;

        private static string locLstPrefix = "LL";
        private static string locHelpClassPrefix = "LC";
        private static string locTablePrefix = "LT";
        //languages list to avoid translate 
        private static string[] avoidList = new string[] { locLstPrefix + _languageEnglishCode, locLstPrefix + "NB", locLstPrefix + "ZH", locLstPrefix + "ES" };

        private static Hashtable _captionList = new Hashtable();

        public static CultureInfo UserCulture
        {
            get { return CultureInfo.CreateSpecificCulture(_cultureCode); }
        }

        public Localization()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public static void ReleaseResources()
        {
            if (_captionTextRm != null)
                _captionTextRm.ReleaseAllResources();
            if (_guielementsRm != null)
                _guielementsRm.ReleaseAllResources();
            if (_errorTextRm != null)
                _errorTextRm.ReleaseAllResources();
            if (_userTextRm != null)
                _userTextRm.ReleaseAllResources();
        }

        public static void InitializeResources()
        {
            _captionTextRm = new ResourceManager("GASystem.Resources.fieldcaption", Assembly.GetExecutingAssembly());
            _captionTextRm.IgnoreCase = true;
            _guielementsRm = new ResourceManager("GASystem.Resources.guielements", Assembly.GetExecutingAssembly());
            _guielementsRm.IgnoreCase = true;
            _errorTextRm = new ResourceManager("GASystem.Resources.errormessages", Assembly.GetExecutingAssembly());
            _errorTextRm.IgnoreCase = true;
            _userTextRm = new ResourceManager("GASystem.Resources.usermessages", Assembly.GetExecutingAssembly());
            _userTextRm.IgnoreCase = true;
        }


        private static string getResourceText(string key, ResourceManager resManager)
        {
            string langKey = resManager.GetString(LANGUAGE_RESOURCE_KEY);
            if (langKey == null)
            {				//try to find value in resource file if no db key defined
                string rvalue = resManager.GetString(key.Trim());
                if (rvalue != null)
                    return rvalue;
                return key;
            }

            string resvalue = getResourceTextDB(key.Trim(), langKey);
            if (resvalue != null)
                return resvalue;

            //try to find value in resource file resource not avalible in db
            resvalue = resManager.GetString(key.Trim());
            if (resvalue != null)
                return resvalue;
            return key;
        }


        public static String GetCaptionText(string key)
        {
            if (key == null) return "NULL";
            if (_captionTextRm == null)
                InitializeResources();

            return getResourceText(key, _captionTextRm);
        }


        private static string getResourceTextDB(string key, string languageKey)
        {

            if (_captionList[languageKey] == null)
                _initilizeLaguageCaption(languageKey);

            return ((FlagResource)_captionList[languageKey]).GetResourceString(key);
        }

        public static string GetCurrentLanguage()
        {
            return CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
        }

        public static string GetCurrentLocalizedListKey()
        {
            string currentLanguage = GetCurrentLanguage();

            return locLstPrefix + currentLanguage.ToUpper();
        }

        public static string GetCurrentLocalizedTableKey()
        {
            string currentLanguage = GetCurrentLanguage();

            return locTablePrefix + currentLanguage.ToUpper();
        }

        public static string GetCurrentLocalizedHelpPrefix()
        {
            return locHelpClassPrefix;
        }

        //GET help html text when creating/editing new record  
        public static string GetCurrentLocalizedClassHelp(string GAClass)
        {
            string currentLang = GetCurrentLanguage();

            if (string.Equals(currentLang, _languageEnglishCode, StringComparison.InvariantCultureIgnoreCase))
                return null;

            int listRowId =
                BusinessLayer.Lists.GetListsRowIdByCategoryAndKey(locHelpClassPrefix + currentLang.ToUpper(), GAClass);

            ListsDS list = BusinessLayer.Lists.GetListsByListsRowId(listRowId);

            if (list.Tables.Count <= 0 || list.GALists.Rows.Count <= 0)
                return null;

            return (string)list.GALists.Rows[0][list.GALists.CommentColumn];
        }

        public static bool AvoidLocalizedListKey(string localizedListKey)
        {
            return _languageEnglishCode == localizedListKey;
        }

        private static bool ContainsLanguageList(string languageList)
        {
            foreach (string language in avoidList)
            {
                if (string.Equals(languageList.Trim(), language.Trim(), StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        private static ListsDS.GAListsDataTable GetLanguageListsTable(string category)
        {
            ListsDS ds = GASystem.BusinessLayer.Lists.GetListsRowIdByCategory(category, false);

            return ds.GALists;
        }

        private static void LocalizeListDataTable(ref ListsDS.GAListsDataTable table, string languageKey)
        {
            ListsDS.GAListsDataTable localizedTable = GetLanguageListsTable(languageKey);

            if (localizedTable.Rows.Count <= 0)
                return;

            foreach (ListsDS.GAListsRow row in table.Rows)
            {
                ListsDS.GAListsRow localizedRows = GetLocalizedListValue(row.ListsRowId, languageKey, localizedTable);

                if (localizedRows == null)
                    continue;

                if (!localizedRows.IsGAListDescriptionNull() && !string.IsNullOrEmpty(localizedRows.GAListDescription))
                {
                    row.GAListDescription = localizedRows.GAListDescription;
                }

                if (!localizedRows.IsGroup1Null() && !string.IsNullOrEmpty(localizedRows.Group1))
                {
                    row.Group1 = localizedRows.Group1;
                }

                if (!localizedRows.IsGroup2Null() && !string.IsNullOrEmpty(localizedRows.Group2))
                {
                    row.Group2 = localizedRows.Group2;
                }

                if (!localizedRows.IsGroup3Null() && !string.IsNullOrEmpty(localizedRows.Group3))
                {
                    row.Group3 = localizedRows.Group3;
                }

                if (!localizedRows.IsGroup4Null() && !string.IsNullOrEmpty(localizedRows.Group4))
                {
                    row.Group4 = localizedRows.Group4;
                }

                /*if (!localizedRows.IsGroup5Null()) //Group5 is used to filter roles
                    row.Group5 = localizedRows.Group5;
                */
            }
        }

        private static ListsDS.GAListsRow GetLocalizedListValue(int ListsRowId, string languageKey, ListsDS.GAListsDataTable localizedTable)
        {
            if (localizedTable == null)
                return null;

            string cat = localizedTable.GAListValueColumn.ColumnName;
            //string val = localizedTable.GAListValue_nb_noColumn.ColumnName;

            DataRow[] row =
                localizedTable.Select(cat + "= '" + ListsRowId + "'");

            if (row.Length > 0)
            {
                return (ListsDS.GAListsRow)row[0];
            }

            return null;
        }

        /*private static bool ListCategoryHasLocalization(string listCategory, string languageKey)
        {
            ListsDS.GAListsDataTable localizedTable = GetLanguageListsTable(languageKey);

            if (localizedTable == null)
                return false;

            string cat = localizedTable.GAListValueColumn.ColumnName;

            DataRow[] row = localizedTable.Select(cat + " LIKE '" + listCategory.Trim() + "|%'");

            if (row.Length > 0)
            {
                return true;
            }

            return false;
        }*/

        /// <summary>
        /// Create new languageResource and add to list
        /// </summary>
        /// <param name="languageKey"></param>
        private static void _initilizeLaguageCaption(string languageKey)
        {
            lock (_captionList.SyncRoot)
            {
                if (!_captionList.Contains(languageKey))
                    _captionList.Add(languageKey, FlagResourceFactory.Make(languageKey));
            }

        }


        /// <summary>
        /// Returns resource text associated with the given key. If key is null, "NULL" is retuned
        /// If text is not found, key value is returned
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static String GetGuiElementText(String key)
        {
            if (key == null) return "NULL";
            if (_guielementsRm == null)
                InitializeResources();

            return getResourceText(key, _guielementsRm);
        }

        /// <summary>
        /// Retrive the plural form of the given word
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static String GetGuiElementTextPlural(String key)
        {
            return GetGuiElementText(key + "Plur");
        }


        public static String GetErrorText(String key)
        {
            if (key == null) return "NULL";
            if (_errorTextRm == null)
                InitializeResources();

            return getResourceText(key, _errorTextRm);
        }

        public static String GetUserMessageText(String key)
        {
            if (key == null) return "NULL";
            if (_userTextRm == null)
                InitializeResources();

            string message = getResourceText(key, _userTextRm);

            //do not return key if a usermessage is not defined
            if (message == key)
                return string.Empty;

            return message;
        }


        public static string GetExceptionMessage(string Key)
        {
            return GetErrorText(Key);
        }


        public static void LocalizeControls(ControlCollection Controls)
        {

            Label label = null;
            Button button = null;
            CheckBox checkBox = null;
            LinkButton linkButton = null;
            HyperLink hyperLink = null;
            //RequiredFieldValidator requiredFieldValidator = null;
            //RegularExpressionValidator regExpValidator = null;
            //CompareValidator compareValidator = null;

            String labelText = null;
            foreach (Control controlItem in Controls)
            {
                if (null != (button = controlItem as Button))
                {
                    if (button.ID.Equals("btnNewUser"))
                        button.Text = button.Text;
                    if (null != (labelText = GetGuiElementText(button.Text)))
                        button.Text = labelText;

                }
                else if (null != (checkBox = controlItem as CheckBox))
                {
                    if (null != (labelText = GetGuiElementText(checkBox.Text)))
                        checkBox.Text = labelText;
                }
                else if (null != (hyperLink = controlItem as HyperLink))
                {
                    if (null != (labelText = GetGuiElementText(hyperLink.Text)))
                        hyperLink.Text = labelText;
                }
                else if (null != (linkButton = controlItem as LinkButton))
                {
                    if (null != (labelText = GetGuiElementText(linkButton.Text)))
                        linkButton.Text = labelText;
                }
                else if (null != (label = controlItem as Label))
                {
                    if (null != (labelText = GetGuiElementText(label.Text)))
                        label.Text = labelText;
                }


            }

        }

        //july23-2020 localize the GAList with the current language
        public static void SetLocalizedListDescription(ListsDS.GAListsDataTable table)
        {
            string languageCategory = GetCurrentLocalizedListKey();

            if (table == null || table.Rows.Count <= 0)
                return;

            string listToTrans = ((ListsDS.GAListsRow)(table.Rows[0])).GAListCategory.Trim();

            if (String.Equals(listToTrans, languageCategory.Trim(),
                StringComparison.CurrentCultureIgnoreCase))
                return;

            if (ContainsLanguageList(listToTrans))
                return;

            LocalizeListDataTable(ref table, languageCategory);
        }



    }
}
