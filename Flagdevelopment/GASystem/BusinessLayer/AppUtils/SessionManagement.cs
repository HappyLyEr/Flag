using System;
using GASystem.DataModel;
using System.Web;

namespace GASystem.AppUtils
{
	/// <summary>
	/// Summary description for SessionManagement.
	/// </summary>
    public class SessionManagement
    {
        public SessionManagement()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private static String _currentDataContextKey = "CurrentDataContext";
        //	private static string _currentSkeltaDesignerKey ="CurrentSkeltaDesignerContext";

        public static GADataContext GetDefaultDataContext()
        {
            try
            {
                string logonId = GAUsers.GetUserId();
                UserDS ds = GASystem.BusinessLayer.User.GetUserByLogonId(logonId);
                if (ds.GAUser.Rows.Count == 1)
                {
                    GADataClass dataClass = GADataRecord.ParseGADataClass(ds.GAUser[0].ContextClass);
                    int rowId = ds.GAUser[0].ContextRowId;
                    return new GADataContext(new GADataRecord(rowId, dataClass), new GADataRecord(rowId, dataClass));
                }
                else
                    return GetGuestContext();
            }
            catch
            {
                return GetGuestContext();
            }
        }

        private static GADataContext GetGuestContext()
        {
            //get guest context from config files
            GADataClass guestDataClass = DefaultContextClass;
            int classRowId = DefaultContextRowId;
            return new GADataContext(new GADataRecord(classRowId, guestDataClass), new GADataRecord(classRowId, guestDataClass));
        }


        public static GADataContext GetCurrentDataContext()
        {
            GADataContext currentContext;
            if (null != HttpContext.Current.Session[_currentDataContextKey])
                currentContext = (GADataContext)HttpContext.Current.Session[_currentDataContextKey];
            else
            {
                currentContext = GetDefaultDataContext();
            }
            return currentContext;
        }

        public static void SetCurrentDataContext(GADataContext DataContext)
        {
            HttpContext.Current.Session[_currentDataContextKey] = DataContext;
        }

        public static GADataContext SetCurrentSubContext(int RowId, GADataClass DataClass)
        {
            return SetCurrentSubContext(new GADataRecord(RowId, DataClass));
        }

        public static GADataContext SetCurrentSubContext(GADataRecord DataRecord)
        {
            GADataContext currentContext = GetCurrentDataContext();
            currentContext.SubContextRecord = DataRecord;
            SetCurrentDataContext(currentContext);
            return currentContext;
        }

        public static GADataContext SetCurrentInitialContext(GADataRecord DataRecord)
        {
            GADataContext currentContext = GetCurrentDataContext();
            currentContext.InitialContextRecord = DataRecord;
            SetCurrentDataContext(currentContext);

            //save it back to the database

            string logonId = GAUsers.GetUserId();
            UserDS ds = GASystem.BusinessLayer.User.GetUserByLogonId(logonId);
            if (ds.GAUser.Rows.Count == 1)
            {
                ds.GAUser[0].ContextClass = DataRecord.DataClass.ToString();
                ds.GAUser[0].ContextRowId = DataRecord.RowId;
                GASystem.BusinessLayer.User.UpdateUser(ds);
            }
            return currentContext;
        }

        public static GADataClass DefaultContextClass
        {
            get
            {
                // Tor 20160127				return  GADataRecord.ParseGADataClass(System.Configuration.ConfigurationManager.AppSettings.Get("DefaultContextClass"));
                return GADataRecord.ParseGADataClass(new GASystem.AppUtils.FlagSysResource().GetResourceString("DefaultContextClass"));
            }
        }

        public static int DefaultContextRowId
        {
            get
            {
                // Tor 20160127                return int.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("DefaultContextRowId"));
                return int.Parse(new GASystem.AppUtils.FlagSysResource().GetResourceString("DefaultContextRowId"));
            }
        }
    }		
}
