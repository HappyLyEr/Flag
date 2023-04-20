using System;
using GASystem.DataModel;

namespace GASystem.GUIUtils
{
	/// <summary>
	/// Summary description for LinkUtils.
	/// </summary>
	public class LinkUtils
	{
		public LinkUtils()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static string GenerateHelpLink(string DataClass) 
		{
			return "~/gagui/webforms/helpdialog.aspx?helpclass=" + DataClass;
		}

		public static string GenerateURLForSingleRecordDetails(string DataClass, string RowId) 
		{
            string tabId = GetTabId(DataClass).ToString();
			string rowIdName = DataClass.ToString() + "RowId";
			rowIdName = rowIdName.Substring(2);

            return "~/Default.aspx?tabId=" + tabId + "&" + rowIdName + "=" + RowId + "&edit=true" + "&dataclass=" + DataClass;	
						
		}

        //public static string GenerateSimpleURLForSingleRecordView(string DataClass, string RowId) 
        //{
        //    string tabId = GetTabId(DataClass).ToString();
        //    string rowIdName = DataClass.ToString() + "RowId";
        //    rowIdName = rowIdName.Substring(2);


        //    return "Default.aspx?tabId=" + tabId + "&" + rowIdName + "=" + RowId + "&dataclass=" + DataClass;		
						
        //}

        public static string GenerateSimpleURLForSingleRecordView(string DataClass, string RowId)
        {
            string tabId = GetTabId(DataClass).ToString();
            string rowIdName = DataClass.ToString() + "RowId";
            rowIdName = rowIdName.Substring(2);


            return "~/Default.aspx?tabId=" + tabId + "&" + rowIdName + "=" + RowId + "&dataclass=" + DataClass;
        }



        public static string GenerateSimpleJscriptURLForSingleRecordView(string DataClass, string RowId)
        {
            string tabId = GetTabId(DataClass).ToString();
            string rowIdName = DataClass.ToString() + "RowId";
            rowIdName = rowIdName.Substring(2);


            return "Default.aspx?tabId=" + tabId + "&" + rowIdName + "=" + RowId + "&dataclass=" + DataClass;
        }




		public static string GenerateURLForSingleRecordView(string DataClass, string RowId) 
		{
            string tabId = GetTabId(DataClass).ToString();
			string rowIdName = DataClass.ToString() + "RowId";
			rowIdName = rowIdName.Substring(2);


            return "~/Default.aspx?tabId=" + tabId + "&" + rowIdName + "=" + RowId + "&dataclass=" + DataClass;	
						
		}



		public static string GenerateURLForSingleRecordView(string DataClass, string RowId, string TabSubClass) 
		{
            string tabId = GetTabId(DataClass).ToString();
			string rowIdName = DataClass.ToString() + "RowId";
			rowIdName = rowIdName.Substring(2);
			return "~/Default.aspx?tabId="+ tabId  + "&" + rowIdName + "=" + RowId + "&dataclass=" + DataClass + "&subclass=" + TabSubClass.ToString();
						
		}

		
        /// <summary>
        /// list all datarecords of type dataClass
        /// </summary>
        /// <param name="DataClass"></param>
        /// <returns></returns>
        public static string GenerateURLForListAll(string dataClass) 
		{
            string tabId = GetTabId(dataClass).ToString();
            return "~/Default.aspx?tabId=" + tabId + "&dataclass=" + dataClass;
		}

        

        /// <summary>
        /// List all records of type dataclass within owner
        /// </summary>
        /// <param name="DataClass"></param>
        /// <returns></returns>
        public static string GenerateURLForListAll(string dataClass, GADataRecord owner) 
		{
            string tabId = GetTabId(dataClass).ToString();
            return "~/Default.aspx?tabId=" + tabId + "&dataclass=" + dataClass + "&ownerclass=" + owner.DataClass.ToString() + "&ownerrowid=" + owner.RowId;
		}

		public static string GenerateURLForListMyWorkitems() 
		{
            string tabId = GetTabId(GADataClass.GAWorkitem).ToString();
			return "~/Default.aspx?tabId="+ tabId +  "&" + GUIUtils.QuerystringUtils.WORKITEMCURRENTUSERONLY + "=true" + "&dataclass=" + GADataClass.GAWorkitem.ToString();
		}


		public static string GenerateURLForAddingGroupOfPeople(string DataClass, GADataRecord Owner) 
		{
            string tabId = GetTabId(DataClass).ToString();
			string rowIdName = DataClass.ToString() + "RowId";
			rowIdName = rowIdName.Substring(2);
            string owner = string.Empty;
            if (null != Owner)
                owner = "&ownerclass=" + Owner.DataClass.ToString() + "&ownerrowid=" + Owner.RowId.ToString();


            return "~/Default.aspx?tabId=" + tabId + "&" + rowIdName + "=0&edit=true&multiple=true" + owner + "&dataclass=" + DataClass.ToString(); ;
		}

		public static string GenerateURLWithDataClassFilterTab(GADataClass OwnerDataClass, string RowId, GADataClass TabDataClass, string Operator, string Field, string Condition) 
		{
            string tabId = GetTabId(OwnerDataClass).ToString();
			string returnURL = "~/Default.aspx?tabId="+ tabId;
			string rowIdName = OwnerDataClass.ToString() + "RowId";
			rowIdName = rowIdName.Substring(2);
			
			returnURL += "&" + rowIdName + "=" + RowId;
			returnURL += "&subclass=" + TabDataClass.ToString();
			returnURL += "&ffield=" + TabDataClass.ToString() + "." + Field;
			returnURL += "&fCondition=" + Condition;
			returnURL += "&fOperator=" + Operator;
            returnURL += "&dataclass=" + OwnerDataClass.ToString(); 
			return returnURL;
		}

	

		public static string GenerateURLWithDataClassFilter(GADataClass DataClass, string Operator, string Field, string Condition) 
		{
            string tabId = GetTabId(DataClass).ToString();
			string returnURL = "~/Default.aspx?tabId="+ tabId;
            returnURL += "&dataclass=" + DataClass.ToString(); 
			returnURL += "&ffield=" + DataClass.ToString() + "." + Field;
			returnURL += "&fCondition=" + Condition;
			returnURL += "&fOperator=" + Operator;

			return returnURL;
		}
		
		public static string GenerateURLForNewRecord(GADataClass DataClass, GADataRecord Owner)
		{
			string tabId = GetTabId(DataClass).ToString();
			string owner = string.Empty;
			if (null != Owner) 
				owner = "&ownerclass=" + Owner.DataClass.ToString() + "&ownerrowid=" + Owner.RowId.ToString();
			
			string returnURL = "~/Default.aspx?tabId="+ tabId + "&" + DataClass.ToString().Substring(2) + "rowid=0&edit=true&dataclass=" + DataClass.ToString() + owner;
			return returnURL;
		}

        public static int GetTabId(string DataClass) 
        {
            string tabId = System.Configuration.ConfigurationManager.AppSettings.Get(DataClass.ToString() + "Details" + "TabId");
			if (tabId == null)
                tabId = System.Configuration.ConfigurationManager.AppSettings.Get("DefaultTabId");
            int tabIdNumber = -1;
            try
            {
                tabIdNumber = int.Parse(tabId);
            } catch (System.ArgumentException ex) 
            {
                throw new GAExceptions.GAException("Could not get find tabid or page for class:" + DataClass);
            }
          
            if (tabIdNumber == -1)
                 throw new GAExceptions.GAException("Could not get find tabid or page for class:" + DataClass);

             return tabIdNumber;
        }


        public static int GetTabId(GADataClass DataClass) 
        {
            return GetTabId(DataClass.ToString());
        }



	}
}
