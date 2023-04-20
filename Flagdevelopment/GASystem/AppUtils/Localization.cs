using System;
using System.Collections;
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
		
		private static ResourceManager _captionTextRm;
		private static ResourceManager _guielementsRm;
		private static ResourceManager _errorTextRm;
        private static ResourceManager _userTextRm;	

		private static Hashtable _captionList = new Hashtable();
		
		
		public static CultureInfo UserCulture 
		{
			get {return CultureInfo.CreateSpecificCulture(_cultureCode);}
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
			_captionTextRm = new ResourceManager("GASystem.Resources.fieldcaption",Assembly.GetExecutingAssembly());
			_captionTextRm.IgnoreCase = true;
			_guielementsRm = new ResourceManager("GASystem.Resources.guielements",Assembly.GetExecutingAssembly());
			_guielementsRm.IgnoreCase = true;
			_errorTextRm = new ResourceManager("GASystem.Resources.errormessages",Assembly.GetExecutingAssembly());
			_errorTextRm.IgnoreCase = true;
            _userTextRm = new ResourceManager("GASystem.Resources.usermessages", Assembly.GetExecutingAssembly());
            _userTextRm.IgnoreCase = true;
		}

		
		private static string getResourceText(string key, ResourceManager resManager)
		{
			string langKey = resManager.GetString(LANGUAGE_RESOURCE_KEY);
			if (langKey == null) 
			{				//try to find value in resource file if no db key defined
				string rvalue =  resManager.GetString(key.Trim());
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
			if (key==null) return "NULL";
			if (_captionTextRm==null)
				InitializeResources();
			
			return getResourceText(key, _captionTextRm);
		}

		
		private static string getResourceTextDB(string key, string languageKey)
		{
			
			if (_captionList[languageKey] == null)			
				_initilizeLaguageCaption(languageKey);
			
			return ((FlagResource) _captionList[languageKey]).GetResourceString(key);
		}
		
		
		/// <summary>
		/// Create new languageResource and add to list
		/// </summary>
		/// <param name="languageKey"></param>
		private static void _initilizeLaguageCaption(string languageKey)
		{
			lock(_captionList.SyncRoot)
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
			if (key==null) return "NULL";
			if (_guielementsRm==null)
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
			return GetGuiElementText(key+"Plur");
		}


		public static String GetErrorText(String key)
		{
			if (key==null) return "NULL";
			if (_errorTextRm==null)
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
				else if (null!= (label = controlItem as Label))
				{
					if (null != (labelText = GetGuiElementText(label.Text)))
						label.Text = labelText;
				}
				
																				   
			}

		}


		
		
	
	}
}
