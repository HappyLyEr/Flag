using System;
using System.Collections;
using System.Web.UI.WebControls;
using System.Data;
using GASystem.DataAccess;
// Tor 20140725 - added libraries below to access DataContextInfo
//using FlagGUILibrary.WebControls.ListControls; 
// Tor 20140627 added libraries below to access the web address fields
using System.Web.UI;
using System.Web;


namespace GASystem.AppUtils
{
	/// <summary>
	/// CodeTables. Implements all codetables (lists) used in the application. Use GetList(category)
	/// to get an ArrayList of ListItems containing key,value pair for the given listcatgeory
	/// </summary>
	public class CodeTables
	{
		public CodeTables()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		private static Hashtable map = new Hashtable();

		
		/// <summary>
		/// Returns an ArrayList of ListItem elements. This array may be bound directly to a
		/// DropDownList or any other bindable control. In the current implementation all lists are
		/// stored in a single database table (GALists). Different lists are identifyed with a listcategory 
		/// </summary>
		/// <param name="listCategory">A value identifying the list you want.</param>
		/// <returns>ArrayList of ListItems</returns>
		public static ArrayList GetList(string listCategory, GASystem.DataModel.GADataRecord Context)
		{
//			//todo make this threadsafe
//			if (null==map[listCategory])
//			{
//				map[listCategory] = _createList(listCategory);
//			}
//			return (ArrayList) map[listCategory];


			//changed by jof, needs to support dynamic generation of list based on permissions

			return _createList(listCategory, Context);
		}

        public static ArrayList GetList(string listCategory,string currentClass,string fieldId, GASystem.DataModel.GADataRecord Context)
        {
            // Tor 20140623 Overload to exclude list elements that are to be excluded if currentClass in GALists.TextFree2
            //			see also comments in GetList above


            //HttpContext a = HttpContext.Current;
            //string dataClass = a.Request.QueryString.Get("dataclass");
            //string ownerDataClass = a.Request.QueryString.Get("ownerclass");
            //string ownerDataClassRowId = a.Request.QueryString.Get("ownerrowid");
            //string dataClassRowId = a.Request.QueryString.Get(dataClass.Substring(2)+"RowId");
            //string ifEdit = a.Request.QueryString.Get("edit");
//            string lastCmd = a.Request.QueryString.Get("lastcmd");
            // only dataclass has content when list all
            // edit = true when edit has been clicked , ie get 
            //        http://localhost/flagDev/Default.aspx?tbabId=238&SafetyObservationRowId=9006004&dataclass=GASafetyObservation
            //if (ifEdit != null && ifEdit.ToLower() == "true")
            if (currentClass!=null && Context!=null)
            {
                return _createList(listCategory, currentClass, fieldId, Context);
            }
            else
                if (Context == null)
                {
                    return _createList(listCategory, currentClass, fieldId, getOwner());
//                    GASystem.DataModel.GADataRecord a = getOwner();
                
                }
                //if (currentClass != null && dataClass != string.Empty && ifEdit == "true")
                //{
                //    // Tor 20140725 Get current record OwnerClass and RowId
                //    //GASystem.DataModel.GADataRecord b = GASystem.AppUtils.GUIQueryString.GetOwner(a.Request);
                //    return _createList(listCategory, currentClass, fieldId, GASystem.AppUtils.GUIQueryString.GetOwner(a.Request));
                //}

            // Tor 20140725 Not enough information to select based on ownerclass
            return _createList(listCategory, Context);


            //    if (currentClass != null && Context == null)
            //    {
            //        // get context and get list
            //    }
            //    else
            //    {

                //}

            //if (dataClass != null)
            //{
            //    //                uiRowId = a.Request.QueryString.Get(dataClass.Substring(2)+"RowId");
            //    if (a.Request.QueryString.Get(dataClass.Substring(2) + "RowId") == null)
            //    {
            //        return _createList(listCategory, Context);
            //    }
            //    else
            //    {
            //        return _createList(listCategory, currentClass, fieldId, Context);
            //    }
            //}
            ////string subClass = this.Page.Request["subclass"] == null ? string.Empty : this.Page.Request["subclass"].ToString();

            //if (a.Request.QueryString.Get("subclass") == null)
            //{
            //    return _createList(listCategory, Context);
            //}

            //return _createList(listCategory, currentClass, fieldId, Context);
        }
        
        // Tor 20140726 Get Owner data record from url request
        private static GASystem.DataModel.GADataRecord getOwner()
        {
            HttpContext a = HttpContext.Current;
            string dataClass = a.Request.QueryString.Get("dataclass");
            string ownerDataClass = a.Request.QueryString.Get("ownerclass");
            string ownerDataClassRowId = a.Request.QueryString.Get("ownerrowid");
            string dataClassRowId = a.Request.QueryString.Get(dataClass.Substring(2) + "RowId");
            string ifEdit = a.Request.QueryString.Get("edit");

            GASystem.DataModel.GADataClass DataClass;
            GASystem.DataModel.GADataRecord dataRecord;

            if (null == ownerDataClass || null == ownerDataClassRowId || !GAUtils.IsNumeric(ownerDataClassRowId))
            {
                // owner not in query string, find owner with current record record rowid
                if (ifEdit == "true" && null != dataClass && null != dataClassRowId && GAUtils.IsNumeric(dataClassRowId))
                {
//                    return new GASystem.DataModel.GADataRecord(int.Parse(dataClassRowId), new GASystem.DataModel.GADataClass(dataClass));
                    DataClass=GASystem.DataModel.GADataRecord.ParseGADataClass(dataClass.ToString());
                    dataRecord = new GASystem.DataModel.GADataRecord(int.Parse(dataClassRowId), DataClass);
                    return GASystem.BusinessLayer.DataClassRelations.GetOwner(dataRecord);
                }
                else // try owner
                {
                    if (null != ownerDataClass && null != ownerDataClassRowId && GAUtils.IsNumeric(ownerDataClassRowId))
                    {
                        DataClass=GASystem.DataModel.GADataRecord.ParseGADataClass(ownerDataClass.ToString());
                        try
                        {
                            dataRecord = new GASystem.DataModel.GADataRecord(int.Parse(ownerDataClassRowId), DataClass);
                        }
                        catch
                        {
                            //ownerclass does not contain a valid gadatarecord, return null
                            return null;
                        }
                        return dataRecord;

                        //GASystem.DataModel.GADataClass OwnerDataClass;
                        //GASystem.DataModel.GADataRecord OwnerDataClassRecord;
                        //OwnerDataClass = GASystem.DataModel.GADataRecord.ParseGADataClass(ownerDataClass.ToString());
                        //return new GASystem.DataModel.GADataRecord(int.Parse(ownerDataClassRowId), OwnerDataClass);
//                        OwnerDataClassRecord=GASystem.DataModel.GADataRecord(int.Parse(ownerDataClassRowId),OwnerDataClass);

//                        return new GASystem.DataModel.GADataRecord(int.Parse(ownerDataClassRowId), new GASystem.DataModel.GADataClass(ownerDataClass));
                    }

                }
            }
            return null;

        }

		public static void ClearListCache()
		{
			//todo make this threadsafe
			map.Clear();
			return;
		}

		private static ArrayList _createList(string listCategory, GASystem.DataModel.GADataRecord Context)
		{

			//TODO make this threadsafe
			GASystem.DataModel.ListsDS lds;
//			if (null==map[listCategory])
//			{
				lds = BusinessLayer.Lists.GetListsRowIdByCategory(listCategory);
//				map[listCategory] = lds;
//			} 
//			else 
//			{
//				lds = (GASystem.DataModel.ListsDS)map[listCategory];
//			}

			ArrayList list = new ArrayList();
			
			//TODO this should be moved to the business layer
			foreach( GASystem.DataModel.ListsDS.GAListsRow row in lds.GALists.Rows )
			{
				
				//add security check
				if (!row.IsGroup5Null() && row.Group5.ToString() != string.Empty)
				{
					// will not add the list item if context is null
					if (Context != null)   //TODO, context is null when listing toplevel record, should never get here with context = null unless we are editing multiple top level rows. How to handle this in the future?
					{
						bool userHasRole = false;
						string validRoles = row.Group5.ToString();
						GASystem.DataModel.RolesDS rds = GASystem.DataAccess.Security.GASecurityDb_new.GetUserRolesForContext(Context);
						foreach (GASystem.DataModel.RolesDS.RolesRow roleRow in rds.Roles.Rows) 
						{
							if (validRoles.IndexOf(";" + roleRow.RoleID.ToString() + ";") > -1)
								userHasRole = true;
						}
						if (userHasRole)
							list.Add(new ListItem(row.GAListDescription, row.ListsRowId.ToString()));

					}
				} 
				else 
				{
					list.Add(new ListItem(row.GAListDescription, row.ListsRowId.ToString()));
				}
			}

			return list;
		}

        private static ArrayList _createList(string listCategory, string currentClass,string fieldId, GASystem.DataModel.GADataRecord Context)
        {
            // Tor 20140623 overload method to check if current list element should be excluded because:
            // currentClass in GALists.TextFree1
            // or ownerclass in GALists.TextFree2
            // or unique owner member combination in GALists.TextFree3
            // see comments in _createList above
            // Tor 20140928 Get current RowId if edit data record
            //if (null = HttpContext.Current.Request.Url.
            //if (null != HttpContext.Current.Session[_currentDataContextKey])
            //    currentContext = (GADataContext)HttpContext.Current.Session[_currentDataContextKey];
            //else
            //{
            //    currentContext = GetDefaultDataContext();
            //}


            string omitIfClass = string.Empty;
            string omitIfOwnerClass = string.Empty;
            string uniqueInOwnerMemberCombination = string.Empty;
            GASystem.DataModel.ListsDS lds;
            lds = BusinessLayer.Lists.GetListsRowIdByCategory(listCategory);
            ArrayList list = new ArrayList();
            foreach (GASystem.DataModel.ListsDS.GAListsRow row in lds.GALists.Rows)
            {
//                omitIfClass = row.TextFree1 == DBNull.Value ? string.Empty : row.TextFree1;
  //              omitIfOwnerClass = row.TextFree2 == DBNull.Value ? string.Empty : row.TextFree2;
                omitIfClass = row.IsTextFree1Null() || row.TextFree1.ToString() == string.Empty ? string.Empty : row.TextFree1;
                omitIfOwnerClass = row.IsTextFree2Null() || row.TextFree2.ToString() == string.Empty ? string.Empty : row.TextFree2;
                uniqueInOwnerMemberCombination = row.IsTextFree3Null() || row.TextFree3.ToString() == string.Empty ? string.Empty : row.TextFree3;
                
                //add security check
                if (!row.IsGroup5Null() && row.Group5.ToString() != string.Empty)
                {
                    // will not add the list item if context is null
                    if (Context != null)   //TODO, context is null when listing toplevel record, should never get here with context = null unless we are editing multiple top level rows. How to handle this in the future?
                    {
                        bool userHasRole = false;
                        string validRoles = row.Group5.ToString();
                        GASystem.DataModel.RolesDS rds = GASystem.DataAccess.Security.GASecurityDb_new.GetUserRolesForContext(Context);
                        foreach (GASystem.DataModel.RolesDS.RolesRow roleRow in rds.Roles.Rows)
                        {
                            if (validRoles.IndexOf(";" + roleRow.RoleID.ToString() + ";") > -1)
                                userHasRole = true;
                        }
                        if (userHasRole)
                            // Tor 20140623 Do not add to list if currentClass in GALists.TextFree1 or ownerclass in GALists.TextFree2
                            //if(!checkIfOmitListElement(row.TextFree1,row.TextFree2,currentClass,Context.DataClass.ToString()))
                            if (!checkIfOmitListElement(row.ListsRowId,omitIfClass, omitIfOwnerClass, uniqueInOwnerMemberCombination,currentClass, fieldId,Context))
                                list.Add(new ListItem(row.GAListDescription, row.ListsRowId.ToString()));
                    }
                }
                else
                {
                    // Tor 20140623 Omit entry if current class in GALists.TextFree2 or record Owner class in GALists.TextFree3 
                    //if(!checkIfOmitListElement(row.TextFree1,row.TextFree2,currentClass,Context.DataClass.ToString()) )
                    if (!checkIfOmitListElement(row.ListsRowId,omitIfClass,omitIfOwnerClass,uniqueInOwnerMemberCombination,currentClass,fieldId,Context))
                        list.Add(new ListItem(row.GAListDescription, row.ListsRowId.ToString()));
                }
            }

            return list;
        }

        private static bool checkIfOmitListElement(int ListsRowId,string omitCurrentClass, string omitOwnerClass, string uniqueInOwnerMemberCombination, string currentClass, string fieldId, GASystem.DataModel.GADataRecord Context)
        {
            // Tor 20140623 Omit entry if 
            // - current class in GALists.TextFree1 or 
            // - record Owner class in GALists.TextFree2 or
            // - ownerClass;currentclass in GALists.TextFree3 and entry in currentclass already exists with this rowId.
            
            string filter="";
            if (omitCurrentClass!="" && omitCurrentClass != null)
            {
                if (omitCurrentClass.IndexOf(";" + currentClass + ";") > -1 )
                    return true; 
            }
            if (Context == null) return false; // Tor 20140823 cannot check on ownerclass if ownerclass is null

            int ownerClassRowId = Context.RowId;
            if (omitOwnerClass != "" && omitOwnerClass != null)
            {
                if (omitOwnerClass.IndexOf(";" + Context.DataClass.ToString() + ";") > -1)
                    return true;
            }
            string sql;
            if (uniqueInOwnerMemberCombination!="" && uniqueInOwnerMemberCombination != null)
            {
                if (uniqueInOwnerMemberCombination.IndexOf("<" + Context.DataClass.ToString() + ";" + currentClass + ">") > -1)
                {
                    filter = @"select m.{4} from GASuperClass s 
                        inner join {0} m on m.{1}RowId=s.MemberClassRowId
                        where s.OwnerClass='{2}' and s.OwnerClassRowId={3}
                        and s.MemberClass='{0}' and m.{4}={5}";
                    // 0 current class name
                    // 1 current class name rowid name
                    // 2 owner class name
                    // 3 owner class rowid number
                    // 4 field id in current class
                    // 5 current listentry rowid

                    //select m.* from GASuperClass s 
                    //inner join GAMeansOfContact m on m.MeansOfContactRowId=s.MemberClassRowId
                    //where s.OwnerClass='GAPersonnel' and s.OwnerClassRowId=38 and s.MemberClass='GAMeansOfContact' and m.ContactDeviceTypeListsRowId=66

                    sql = String.Format(filter, new Object[] { currentClass, currentClass.Substring(2), Context.DataClass.ToString(), Context.RowId, fieldId, ListsRowId });
                    System.Data.SqlClient.SqlDataReader records = GASystem.DataAccess.DataUtils.executeSelect(sql);

                    if (records.HasRows) return true;
                    // uniqueInOwnerMemberCombination format: <GAPersonnel;GAMeansOfContact>
                }
            }
            return false;
        }


		public static void BindCodeTable(System.Web.UI.WebControls.ListControl listControl, ArrayList ht)
		{
			listControl.DataSource = ht;
			listControl.DataTextField = "Text";
			listControl.DataValueField = "Value" ;
			listControl.DataBind();
		}


	}
}
