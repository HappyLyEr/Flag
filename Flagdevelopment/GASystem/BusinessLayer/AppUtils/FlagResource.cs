using System;
using System.Collections;
using GASystem.DataModel;

namespace GASystem.AppUtils
{
	/// <summary>
	/// Summary description for FlagResource.
	/// </summary>
	public class FlagResource
	{
		private Hashtable resourceHash = new Hashtable();
		private string _languageKey = "NoLanguageSet";
		
		public FlagResource(string LanguageKey)
		{
			_languageKey = LanguageKey;
			loadHashTable();
			
			
		}
		
		public string GetResourceString(string Key)
		{
			object param = resourceHash[Key];
			if (param != null)
				return param.ToString();
			return null;
		}
		
		/// <summary>
		/// Loads hash table. Locks access and clears before updateing in case this 
		/// operation is called outside of constructor
		/// </summary>
		private void loadHashTable()
		{	
			ListsDS ds = GASystem.BusinessLayer.Lists.GetListsRowIdByCategory(_languageKey);
			//lock hashtable for update
			
			lock(resourceHash.SyncRoot)
			{
				resourceHash.Clear();
				foreach (ListsDS.GAListsRow row in ds.GALists.Rows)
				{
					resourceHash.Add(row.GAListValue, row.GAListDescription);
				}
			}
		}
	}
}
