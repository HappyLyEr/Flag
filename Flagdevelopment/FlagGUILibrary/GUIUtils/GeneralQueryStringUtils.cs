using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace GASystem.GUIUtils
{
    public class GeneralQueryStringUtils
    {
        private HttpRequest myRequest;
		public const string WORKITEMCURRENTUSERONLY = "wcurrentuser";

        public GeneralQueryStringUtils(HttpRequest Request)
		{

			myRequest = Request;
		}

        /// <summary>
        /// get a querystring paramerter in alphanumeric format. All charathers except [a..z][A..Z][0..9] 
        /// is removed
        /// </summary>
        /// <param name="paramName">param name</param>
        /// <returns>Alphanumeric value. Empty string in parameter does not exist</returns>
        public string getSingleAlphaNumericQueryStringParam(string paramName)
        {
            if (myRequest.QueryString[paramName] == null)
                return string.Empty;

            string rawformat = myRequest.QueryString[paramName].ToString();
            string cleanedText = System.Text.RegularExpressions.Regex.Replace(rawformat, @"[^a-zA-Z_0-9 \t].", "");
            return cleanedText;
        }

    }
}
