using System;
using System.Collections.Generic;
using System.Collections;
using System.Runtime.InteropServices;
using System.Text;

namespace GASystem.DataAccess.Utils
{
    public class DataCache
    {
        public static Hashtable Cache = new Hashtable();
        public static Hashtable TimeStamps = new Hashtable();

        public enum DataCacheType 
        {
            SuperClassByOwner, SuperClassByMember, SuperClassLinksByOwner, SuperClassLinksByMember,
            ListsByListsRowId, ListsRowIdByCategory, ListsByClass, ListCategoryRowIdByName,
            UserByLogonId, UserGroups, AllActiveWorkItemsByPersonnelId, ClassByGADataClass,
            CompanyByCompanyRowId, CompanyNoContentByCompanyRowId, ByRowId, PersonnelByPersonnelRowId,
            EmploymentByPersonnelId,
            ShortcutLinks, HasStaticReadOrCreateAccessToMemberUnderOwner, HasStaticAccessToDataClass

        };

        private static string GetLanguageCacheKey(string hashKey, string languageKey)
        {
            return hashKey + "-" + languageKey;
        }

        public static object GetCachedObject(DataCacheType cacheType, string hashKey)
        {
            if (Cache.Contains(cacheType) && ((Hashtable)Cache[cacheType]).Contains(hashKey))
                return ((Hashtable)Cache[cacheType])[hashKey];

            return null;
        }

        //july 20-2020 get cache object using language key
        public static object GetCachedObject(DataCacheType cacheType, string hashKey, [Optional, DefaultParameterValue(null)]string languageKey)
        {
            return GetCachedObject(cacheType, GetLanguageCacheKey(hashKey, languageKey));
        }

        // Tor 20170703 enable access from FlagGUILibrary internal static void ValidateCache(DataCacheType cacheType)
        public static void ValidateCache(DataCacheType cacheType)
        {
            if (!TimeStamps.Contains(cacheType) || DateTime.Now.Subtract((DateTime)TimeStamps[cacheType]).TotalSeconds > DataUtils.getCacheTimeout())
            {
                Cache[cacheType] =  new Hashtable();
                TimeStamps[cacheType] = DateTime.Now;
            }
        }

        // Tor 20170703 Added time parameter for infrequent cache updates
        // Tor 20170703 enable access from FlagGUILibrary internal static void ValidateCacheLong(DataCacheType cacheType)
        public static void ValidateCacheLong(DataCacheType cacheType)
        {
            if (!TimeStamps.Contains(cacheType) || DateTime.Now.Subtract((DateTime)TimeStamps[cacheType]).TotalSeconds > DataUtils.getCacheTimeoutLong())
            {
                Cache[cacheType] = new Hashtable();
                TimeStamps[cacheType] = DateTime.Now;
            }
        }

        // Tor 20170703 enable access from FlagGUILibrary internal static void AddCachedObject(DataCacheType cacheType, string cacheKey, object objectToCache)
        public static void AddCachedObject(DataCacheType cacheType, string cacheKey, object objectToCache)
        {
            if (!Cache.Contains(cacheType))
            {
                Cache[cacheType] = new Hashtable();
                TimeStamps[cacheType] = DateTime.Now;
            }
            ((Hashtable)Cache[cacheType])[cacheKey] = objectToCache;
        }

        // July-23-2020 For language lists cache 
        public static void AddCachedObject(DataCacheType cacheType, string cacheKey, object objectToCache, [Optional, DefaultParameterValue(null)] string languageKey)
        {
            AddCachedObject(cacheType, GetLanguageCacheKey(cacheKey, languageKey), objectToCache);
        }
    }
}
