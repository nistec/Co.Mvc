using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nistec.Caching;
using Nistec.Caching.Remote;
using System.Collections;
using Netcell.Remoting;
using Netcell.Lib.Mobile;
using Netcell.Data;

namespace Netcell.Lib
{
    public class ApiCache
    {
        public const string PublishPrefix = "publish_";
        public const string ActiveDestination = "ActiveDestination";
        public const string AltDestination = "AltDestination";
        public const string WapContent = "WapItems";
        public const string MailContent = "MailContent";
        public const int AltTimeout = 30;
        public const int MaxRetry = 2;

        #region Alt Destination

        private static string GetAltKey(string sessionId, Guid publishKey)
        {
            return string.Format("{0}${1}", sessionId, publishKey);
        }

        //public static bool ParseAltKey(string altKey, ref string sessionId, ref string publishKey)
        //{
        //    if (string.IsNullOrEmpty(altKey))
        //        return false;
        //    return Nistec.Generic.GenericArgs.SplitArgs<string, string>(altKey, '$', ref sessionId, ref publishKey);
        //}

        //public static string ParsePublishKey(string altKey)
        //{
        //    if (string.IsNullOrEmpty(altKey))
        //        return null;
        //    string sessionId = null;
        //    string publishKey = null;
        //    if (Counters.ParseAltKey(altKey, ref sessionId, ref publishKey))
        //        return publishKey;
        //    return null;
        //}

        internal static string CreateAltDestination(string sessionId, Guid publishKey, int campaignId, bool validateExists)
        {

            try
            {
                RemoteSession instance = new RemoteSession(sessionId);

                if (validateExists && !instance.Exists(ApiCache.ActiveDestination))
                {
                    throw new MsgException(AckStatus.CacheException, string.Format("Destination not found for campaign:{0}", campaignId));
                }
                string altKey = GetAltKey(sessionId, publishKey);
                int res = instance.CopyTo(ApiCache.ActiveDestination, altKey, AltTimeout, true);
                if (res <= 0)
                {
                    throw new MsgException(AckStatus.CacheException, string.Format("Destination not found for campaign:{0}", campaignId));

                }

                return altKey;
            }
            catch (MsgException mex)
            {
                throw mex;
            }
            catch (Exception ex)
            {
                throw new MsgException(AckStatus.CacheException, string.Format("Destination not found for campaign:{0}, error::{1}", campaignId, ex.Message));
            }

        }

        public static Hashtable GetAltDestination(string altKey, int campaignId, int retry)
        {

            if (retry > MaxRetry)
            {
                throw new MsgException(AckStatus.CacheException, string.Format("Alt Destination not found for campaign:{0}, finnal retry:{1}", campaignId, retry));
            }
            Hashtable h = null;
            bool hasError = false;
            try
            {
                RemoteCacheClient instance = new RemoteCacheClient();

                object o=instance.Get(altKey);
                if (o == null)
                {
                    throw new MsgException(AckStatus.CacheException, string.Format("Alt Destination not found for campaign:{0}, retry:{1}, altKey:{2}", campaignId, retry, altKey));
                }
                //Log.InfoFormat("AltDestination value:{0}", o.ToString());
                h = (Hashtable)Nistec.Runtime.Serialization.DeserializeFromBase64(o.ToString());

                if (h == null)
                {
                    throw new MsgException(AckStatus.CacheException, string.Format("Alt Destination Deserialize error for campaign:{0}, retry:{1}", campaignId, retry));
                }

            }
            catch (MsgException)
            {
                hasError = true;
            }
            catch (Exception ex)
            {
                MsgException.Trace(AckStatus.CacheException, string.Format("Alt Destination not found for campaign:{0}, error::{1}", campaignId, ex.Message));
                hasError = true;
            }
            if (hasError)
            {
                return GetAltDestination(altKey, campaignId, retry + 1);
            }
            return h;
        }

        public static void RemoveAltDestination(string altKey)
        {
            try
            {
                RemoteCacheClient instance = new RemoteCacheClient();

                instance.RemoveItemAsync(altKey);
            }
            catch (Exception)
            {
            }
        }

        /*
        internal static int SetAltDestination(Hashtable h, string sessionId, int campaignId)
        {

            if (h == null)
            {
                throw new MsgException(AckStatus.CacheException, string.Format("Destination is null or empty for campaign:{0}", campaignId));
            }

            try
            {
                RemoteCacheClient instance = new RemoteCacheClient();

                return instance.AddItem(GetAltKey(sessionId), h, AltTimeout);
            }
            catch (MsgException mex)
            {
                throw mex;
            }
            catch (Exception ex)
            {
                throw new MsgException(AckStatus.CacheException, string.Format("Set Alt Destination Error for campaign:{0}, error::{1}", campaignId, ex.Message));
            }
        }

        
                public static Hashtable GetAltDestination(string sessionId, int campaignId, int retry)
                {

                    if (retry > MaxRetry)
                    {
                        throw new MsgException(AckStatus.CacheException, string.Format("Alt Destination not found for campaign:{0}, finnal retry:{1}", campaignId, retry));
                    }
                    Hashtable h = null;
                    bool hasError = false;
                    try
                    {
                        RemoteCacheClient instance = new RemoteCacheClient();

                        h = (Hashtable)instance.Get(GetAltKey(sessionId));

                        if (h == null)
                        {
                            throw new MsgException(AckStatus.CacheException, string.Format("Alt Destination not found for campaign:{0}, retry:{1}", campaignId, retry));
                        }

                    }
                    catch (MsgException)
                    {
                        hasError = true;
                    }
                    catch (Exception ex)
                    {
                        MsgException.Trace(AckStatus.CacheException, string.Format("Alt Destination not found for campaign:{0}, error::{1}", campaignId, ex.Message));
                        hasError = true;
                    }
                    if (hasError)
                    {
                        return GetAltDestination(sessionId, campaignId, retry + 1);
                    }
                    return h;
                }
        */
        #endregion

        
        internal static Hashtable GetActiveDestination(string sessionId, int campaignId)
        {

            Hashtable h = null;
            try
            {
                RemoteSession instance = new RemoteSession(sessionId);

                h = (Hashtable)instance.Get(ApiCache.ActiveDestination);

                if (h == null)
                {
                    throw new MsgException(AckStatus.CacheException, string.Format("Destination not found for campaign:{0}", campaignId));
                }
                return h;
            }
            catch (MsgException mex)
            {
                throw mex;
            }
            catch (Exception ex)
            {
                throw new MsgException(AckStatus.CacheException, string.Format("Destination not found for campaign:{0}, error::{1}", campaignId, ex.Message));
            }
        }

/*
        internal static int SetAltDestination(Hashtable h, string sessionId, int campaignId)
        {

            if (h == null)
            {
                throw new MsgException(AckStatus.CacheException, string.Format("Destination is null or empty for campaign:{0}", campaignId));
            }

            try
            {
                RemoteSession instance = new RemoteSession(sessionId);

                return instance.AddItem(ApiCache.AltDestination, h, true);
            }
            catch (MsgException mex)
            {
                throw mex;
            }
            catch (Exception ex)
            {
                throw new MsgException(AckStatus.CacheException, string.Format("Set Alt Destination Error for campaign:{0}, error::{1}", campaignId, ex.Message));
            }
        }

        public static Hashtable GetAltDestination(string sessionId, int campaignId, int retry)
        {

            if (retry > MaxRetry)
            {
                throw new MsgException(AckStatus.CacheException, string.Format("Alt Destination not found for campaign:{0}, finnal retry:{1}", campaignId, retry));

            }
            Hashtable h = null;
            bool hasError = false;
            try
            {
                RemoteSession instance = new RemoteSession(sessionId);

                h = (Hashtable)instance.Get(ApiCache.ActiveDestination);

                if (h == null)
                {
                    throw new MsgException(AckStatus.CacheException, string.Format("Alt Destination not found for campaign:{0}, retry:{1}", campaignId, retry));
                }

            }
            catch (MsgException)
            {
                hasError = true;
            }
            catch (Exception ex)
            {
                MsgException.Trace(AckStatus.CacheException, string.Format("Alt Destination not found for campaign:{0}, error::{1}", campaignId, ex.Message));
                hasError = true;
            }
            if (hasError)
            {
                return GetAltDestination(sessionId, campaignId, retry + 1);
            }
            return h;
        }
*/

 

        //internal static Hashtable GetActiveDestination(string sessionId, int campaignId, int retry)
        //{

        //    if (retry > MaxRetry)
        //    {
        //        throw new MsgException(AckStatus.CacheException, string.Format("Destination not found for campaign:{0}, finnal retry:{1}", campaignId, retry));

        //    }
        //    Hashtable h = null;
        //    bool hasError = false;
        //    try
        //    {
        //        RemoteSession instance = new RemoteSession(sessionId);

        //        h = (Hashtable)instance.Get(ApiCache.ActiveDestination);

        //        if (h == null)
        //        {
        //            throw new MsgException(AckStatus.CacheException, string.Format("Destination not found for campaign:{0}, retry:{1}", campaignId, retry));
        //        }

        //    }
        //    catch (MsgException)
        //    {
        //        hasError = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        MsgException.Trace(AckStatus.CacheException, string.Format("Destination not found for campaign:{0}, error::{1}", campaignId, ex.Message));
        //        hasError = true;
        //    }
        //    if (hasError)
        //    {
        //        return GetActiveDestination(sessionId, campaignId, retry + 1);
        //    }
        //    return h;
        //}


        public static string GetMailContent(string sessionId, int campaignId)
        {

            try
            {
                RemoteSession instance = new RemoteSession(sessionId);

                object o = instance.Get(ApiCache.MailContent);

                if (o == null)
                {
                    throw new MsgException(AckStatus.InvalidContent, string.Format("MailContent not found for campaign:{0}", campaignId));
                }
                return o.ToString();
            }
            catch (MsgException mex)
            {
                throw mex;
            }
            catch (Exception ex)
            {
                throw new MsgException(AckStatus.InvalidContent, string.Format("MailContent not found for campaign:{0}, error::{1}", campaignId, ex.Message));
            }
        }

        public static WapItemsList GetWapContent(string sessionId, int campaignId)
        {

            try
            {
                RemoteSession instance = new RemoteSession(sessionId);

                object o = instance.Get(ApiCache.WapContent);

                if (o == null)
                {
                    throw new MsgException(AckStatus.InvalidContent, string.Format("MobileContent not found for campaign:{0}", campaignId));
                }
                return (WapItemsList)o;
            }
            catch (MsgException mex)
            {
                throw mex;
            }
            catch (Exception ex)
            {
                throw new MsgException(AckStatus.InvalidContent, string.Format("MobileContent not found for campaign:{0}, error::{1}", campaignId, ex.Message));
            }
        }


        #region Cache Session

        //public static void AddSession(string sessionId, string loginId, string args, UserCookie uk)
        //{
        //    Nistec.Caching.Remote.RemoteSession.Session.AddSession(sessionId, loginId, 30, args);
        //    Add(sessionId, CacheHelper.UCOOKIE, uk);
        //}

        public static void RemoveSession(string sessionId)
        {
            Nistec.Caching.Remote.RemoteSession.Instance(sessionId).RemoveSession();//.RemoveSessionAsync();
        }

        public static SessionItem GetSession(string sessionId)
        {
            SessionItem item = Nistec.Caching.Remote.RemoteSession.Instance(sessionId).GetSession();
            return item;
        }

        #endregion

        #region  mc remote cach

        //public static void ClearAll(string sessionId, bool contacts, bool accounts)
        //{
        //    if (contacts)
        //    {
        //        Remove(sessionId, AddressBook);
        //        Remove(sessionId, AddressBook_Filtred);
        //        Remove(sessionId, AddressBook_Group);
        //    }
        //    if (accounts)
        //    {
        //        Remove(sessionId, AccountList);
        //    }
        //}

        //public static void ClearAll(string sessionId, string[] items)
        //{
        //    foreach (string s in items)
        //    {
        //        Remove(sessionId, s);
        //    }
        //}

        //public static int Add(Page p, string sessionId, string itemKey, object value)
        //{
        //    return Add(p, sessionId, itemKey, value, 30);
        //}

        //public static int Add(Page p, string sessionId, string itemKey, object value, int expiration)
        //{
        //    try
        //    {
        //        string cachKey = GetCacheKey(itemKey, sessionId);
        //        int res = RemoteCacheClient.Instance.AddItem(cachKey, value, sessionId, expiration);
        //        return res;
        //    }
        //    catch (Exception ex)
        //    {
        //        JS.Alert(p, ex.Message);
        //        return -1;
        //    }
        //}

        //public static int Add(string sessionId, string itemKey, object value, int expiration)
        //{
        //    try
        //    {
        //        string cachKey = GetCacheKey(itemKey, sessionId);
        //        int res = RemoteCacheClient.Instance.AddItem(cachKey, value, sessionId, expiration);
        //        return res;
        //    }
        //    catch (Exception)
        //    {
        //        return -1;
        //    }
        //}
        //public static int Add(string sessionId, string itemKey, object value)
        //{
        //    return Add(sessionId, itemKey, value, 30);
        //}

        public static string GetCacheKey(string itemKey, string sessionId)
        {
            return CacheItem.SessionKey(itemKey, sessionId);
        }

       
        public static int Copy(string sessionId, string itemKey, string destKey, int expiration)
        {
            try
            {
                string sourceKey = GetCacheKey(itemKey, sessionId);
                int res = RemoteCacheClient.Instance.CopyItem(sourceKey, destKey, expiration);
                return res;
            }
            catch (Exception)
            {
                return -1;
            }
        }
        public static object Get(string sessionId, string itemKey)
        {
            string cachKey = GetCacheKey(itemKey, sessionId);
            return RemoteCacheClient.Instance.GetItem(cachKey, null);
        }

        public static string GetAsString( string sessionId, string itemKey)
        {
            object o = Get(sessionId, itemKey);
            if (o != null)
                return o.ToString();

            return "";
        }

        public static void Remove(string sessionId, string itemKey)
        {
            string cachKey = GetCacheKey(itemKey, sessionId);
            RemoteCacheClient.Instance.RemoveItem(cachKey);
        }

#if(CKSESSION)

        public static string GetCacheKey(string siteName, string itemKey, string sessionId)
        {
            return siteName + "_" + CacheItem.SessionKey(itemKey, sessionId);
        }

        public static int Copy(string siteName, string sessionId, string itemKey, string destKey, int expiration)
        {
            try
            {
                string sourceKey = GetCacheKey(siteName,itemKey, sessionId);
                int res = RemoteCacheClient.Instance.CopyItem(sourceKey, destKey, expiration);
                return res;
            }
            catch (Exception)
            {
                return -1;
            }
        }
        public static object Get(string siteName, string sessionId, string itemKey)
        {
            string cachKey = GetCacheKey(siteName,itemKey, sessionId);
            return RemoteCacheClient.Instance.GetItem(cachKey, null);
        }

        public static string GetAsString(string siteName, string sessionId, string itemKey)
        {
            object o=Get(siteName, sessionId, itemKey);
            if (o != null)
                return o.ToString();

            return "";
        }

        //public static object Get(string itemKey)
        //{
        //    return Get(LoginKey, itemKey);
        //}

        public static void Remove(string siteName, string sessionId, string itemKey)
        {
            string cachKey = GetCacheKey(siteName,itemKey, sessionId);
            RemoteCacheClient.Instance.RemoveItem(cachKey);
        }
#endif
        #endregion

        #region Cache by LoginKey

        //public static void ClearByLoginKey(string[] items)
        //{
        //    string loginKey = LoginKey;
        //    foreach (string s in items)
        //    {
        //        Remove(loginKey, s);
        //    }
        //}

        //public static int AddByLoginKey(string itemKey, object value)
        //{
        //    return Add(LoginKey, itemKey, value, 30);
        //}
        //public static object GetByLoginKey(string itemKey)
        //{
        //    return Get(LoginKey, itemKey);
        //}

        //public static void RemoveByLoginKey(string itemKey)
        //{
        //    Remove(LoginKey, itemKey);
        //}

        #endregion

    }
}
