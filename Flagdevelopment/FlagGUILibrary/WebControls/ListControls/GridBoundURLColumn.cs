using System;
using System.Collections.Generic;
using System.Text;
using Telerik.WebControls;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace FlagGUILibrary.WebControls.ListControls
{
    class GridBoundURLColumn : GridBoundColumn
    {
        string baseURL;

        public string BaseURL
        {
            get { return baseURL; }
            set { baseURL = value; }
        }

        public override bool ReadOnly
        {
            get
            {
                return true;
            }
            set
            {
                base.ReadOnly = value;
            }
        }

        protected override string FormatDataValue(object dataValue, GridItem item)
        {
            
            
            return checkURL(base.FormatDataValue(dataValue, item));
        }

        string checkURL(string input)
        {
            string output = input;


           //string baseUrl = Page.Request.Url.Scheme + "://" + Page.Request.Url.Authority + Page.Request.ApplicationPath.TrimEnd('/') + "/";

            //txtBoxURL.Text = txtBoxURL.Text.Replace(baseUrl, "");
            //string flagURL = System.Configuration.ConfigurationManager.AppSettings.Get("flagbaseurl");
            string flagURL = (new GASystem.AppUtils.FlagSysResource()).GetResourceString("flagbaseurl");
// Tor 20160302            output = output.Replace(flagURL, baseURL);
            output = output.Replace(flagURL, BaseURL);
               

            return output;
        }

    }
}
