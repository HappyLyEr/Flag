using System;
using System.Collections;
using GASystem.DataModel;

namespace GASystem.AppUtils
{
	/// <summary>
	/// Summary description for FlagResource.
	/// </summary>
	public class FlagSysResource
	{
		private Hashtable sysHash = new Hashtable();
		private string _sysKey = "SYS";
		
		public FlagSysResource()
		{

            //System.Configuration.ConfigurationManager
			loadHashTable();
			
			
		}
		
		public string GetResourceString(string Key)
		{
            object param = sysHash[Key];
			if (param != null)
				return param.ToString();

            return System.Configuration.ConfigurationManager.AppSettings.Get(Key);
			
		}
		
		/// <summary>
		/// Loads hash table. Locks access and clears before updateing in case this 
		/// operation is called outside of constructor
        /// Add Key(galistvalue) - value(GAListDescription if nTextFree2 is Null/default or nTextFree2) 
		/// </summary>
		private void loadHashTable()
		{	
			ListsDS ds = GASystem.BusinessLayer.Lists.GetListsRowIdByCategory(_sysKey);
			//lock hashtable for update

            lock (sysHash.SyncRoot)
			{
                sysHash.Clear();
				foreach (ListsDS.GAListsRow row in ds.GALists.Rows)
				{
                    // Tor 20160226 descriptions have been moved to nTextFree2 because GAListDescrion is max 400 characters
                    // sysHash.Add(row.GAListValue, row.GAListDescription);
                    if (row.nTextFree2 != null) // fails when nTextFree2 = DEFAULT (column database default)
                    {
                        sysHash.Add(row.GAListValue, row.nTextFree2);
                    }
                    else
                    {
                        sysHash.Add(row.GAListValue, row.GAListDescription);
                    }
                    //if (string.IsNullOrEmpty(row.nTextFree2))
                    //{
                    //    sysHash.Add(row.GAListValue, row.GAListDescription);
                    //}
                    //else
                    //{
                    //    sysHash.Add(row.GAListValue, row.nTextFree2);
                    //}
                }
			}
		}
	}
}
