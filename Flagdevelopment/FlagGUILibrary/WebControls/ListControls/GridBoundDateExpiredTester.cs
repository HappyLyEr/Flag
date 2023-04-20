using System;
using System.Collections.Generic;
using System.Text;
using Telerik.WebControls;
using System.Data;
using GASystem.DataModel;
using GASystem.BusinessLayer;

namespace FlagGUILibrary.WebControls.ListControls
{
    class GridBoundDateExpiredTester : GridBoundColumn
    {
        private string _separator = "<br/>";
        public const string EXPIREDMESSAGE = "RemedialExpiredMessage";

        public string Separator
        {
            get
            {
                return _separator;
            }
            set
            {
                _separator = value;
            }
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

        private string dataClass;

        public string DataClass
        {
            get { return dataClass; }
            set { dataClass = value; }
        }

        private GASystem.AppUtils.ClassDescription cd;

        public GASystem.AppUtils.ClassDescription CD
        {
            get { return cd; }
            set { cd = value; }
        }


        private GADataRecord _owner;

        public GADataRecord Owner
        {
            get { return _owner; }
            set { _owner = value; }
        }


        protected override string FormatDataValue(object dataValue, GridItem item)
        {
            if (dataValue == null)
                return string.Empty;


            DateTime dateNow = System.DateTime.Now;
            dateNow = new DateTime(dateNow.Year, dateNow.Month, dateNow.Day, 0, 0, 0);
            DateTime dateValue = dateNow;


            try
            {
                dateValue = (DateTime)dataValue;
            }
            catch
            {
                return string.Empty;
            }

           
            if (dateNow > dateValue)
                return "<img src=\"images/warning.png\" title=\"" + GASystem.AppUtils.Localization.GetGuiElementText(EXPIREDMESSAGE)  + "\" >";
        
               
            return string.Empty;

            
            
        }

       
    }
}
