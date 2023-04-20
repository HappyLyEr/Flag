using System;
using GASystem.DataModel;
using GASystem.DataAccess;
using System.Data;

namespace GASystem.BusinessLayer
{
    public class Class : BusinessClass
    {
        public Class()
		{
			//
			// TODO: Add constructor logic here
			//
			DataClass = GADataClass.GAClass;
		}

		public static ClassDS GetClassByClassRowId(int ClassRowId)
		{
			return ClassDb.GetClassByClassRowId(ClassRowId);
		}
        // Tor 20150325 get class by class name
        public static ClassDS GetClassByClass(string Class)
        {
            return ClassDb.GetClassByGADataClass(Class);
        }
        // Tor 20140708 get if class is use as home class
        public static bool GetClassIsUseAsHomeClass(GADataClass Class)
        {
            ClassDS ds = ClassDb.GetClassByGADataClass(Class);
            if (ds.GAClass.Rows.Count == 1)
            {
                bool b = ds.GAClass[0].SwitchFree2;
                return b;
            }
            return false;
        }
        // Tor 20140709 get if class is Top Class
        public static bool GetClassIsTop(GADataClass Class)
        {
            ClassDS ds = ClassDb.GetClassByGADataClass(Class);
            if (ds.GAClass.Rows.Count == 1)
            {
                bool b = ds.GAClass[0].IsTop;
                return b;
            }
            return false;
        }

        // Tor 20151104 get if class is View Dataclass, ie class is defined in sql statement in GAClass
        public static bool GetClassIsView(GADataClass Class)
        {
            return GetClassIsView(Class.ToString());
        }

        // Tor 20151104 get if class is View Dataclass, ie class is defined in sql statement in GAClass
        public static bool GetClassIsView(string Class)
        {
            ClassDS ds = ClassDb.GetClassByGADataClass(Class);
            if (ds.GAClass.Rows.Count == 1)
            {
//                  bool b = ds.GAClass[0].IsView;
//                  return b;
                if (ds.GAClass[0].nTextFree2 != null && ds.GAClass[0].nTextFree2.Length > 5)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
        // Tor 20151105 get class Vertical Field Name

        public static string GetClassVerticalFieldName(string Class)
        {
            ClassDS ds = ClassDb.GetClassByGADataClass(Class);
            if (ds.GAClass.Rows.Count == 1)
            {
                if (ds.Tables[0].Rows[0]["nTextFree3"] != DBNull.Value)
                    return ds.GAClass[0].nTextFree3.ToString();
            }
            return string.Empty;
        }

        // Tor 20160624 get class Reference Id field name
        public static string GetClassReferenceIdFieldName(string Class)
        {
            ClassDS ds = ClassDb.GetClassByGADataClass(Class);
            if (ds.GAClass.Rows.Count == 1)
            {
                if (ds.Tables[0].Rows[0]["NameOfReferenceId"] != DBNull.Value)
                    return ds.GAClass[0].NameOfReferenceId.ToString();
            }
            return string.Empty;
        }

        // Tor 20160624 get class Reference Id field name
        public static string GetClassObjectName(string Class)
        {
            ClassDS ds = ClassDb.GetClassByGADataClass(Class);
            if (ds.GAClass.Rows.Count == 1)
            {
                if (ds.Tables[0].Rows[0]["ObjectName"] != DBNull.Value)
                    return ds.GAClass[0].ObjectName.ToString();
            }
            return string.Empty;
        }

        // Tor 20150625 get class rowId by class name
        public static int GetClassRowIdByClassName(string DataClassName)
        {
            ClassDS ds = ClassDb.GetClassByGADataClass(DataClassName);
            if (ds.GAClass.Rows.Count == 1)
            {
                return (int)ds.GAClass[0].ClassRowId;
            }
            return -1;
        }

    }
}
