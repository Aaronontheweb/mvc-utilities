using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Caching;
using System.Threading;
using Microsoft.ApplicationServer.Caching;

namespace MVC.Utilities.Caching
{
    public class AppFabricCacheService : CacheServiceBase
    {
        private static DataCache _cache;
        private static DataCacheFactory _factory;

        public AppFabricCacheService() : this(new TimeSpan(0, 20, 0)){}

        public AppFabricCacheService(TimeSpan defaultDuration)
            : base(defaultDuration)
        {

        }

        #region Cache helper methods

        private static DataCache GetCache()
        {
            if (_cache != null) return _cache;

            // Disable tracing to avoid informational / verbose messages on the web page
            DataCacheClientLogManager.ChangeLogLevel(TraceLevel.Off);

            _factory = new DataCacheFactory();
            _cache = _factory.GetDefaultCache();
            return _cache;
        }

        /// <summary>
        /// Spins in a loop to try to complete a failed Get request from the cache;
        /// if it still fails after [attemptCount] attempts, it will throw an ApplicationException
        /// </summary>
        /// <param name="key">The key of the item to retrieve from the cache</param>
        /// <param name="attemptCount">The number of get attempts to make</param>
        /// <param name="waitTime">The number of milliseconds to wait between retry attempts</param>
        /// <param name="region">The AppFabric Cache region to use</param>
        /// <returns>The object retrieved from the cache</returns>
        private static object RetryGet(string key, int attemptCount = 10, int waitTime = 100, string region = "")
        {
            var cache = GetCache();
            object itemInCache = null;

            for (var i = 0; i < attemptCount; i++)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(region) || string.IsNullOrEmpty(region))
                    {
                        //Attempt to get the item from cache again...
                        itemInCache = cache.Get(key);
                    }
                    else
                    {
                        itemInCache = cache.Get(key, region);
                    }
                    
                }
                catch (DataCacheException cacheException)
                {
                    //If we received another "retry later" error from the cache
                    if (cacheException.ErrorCode == DataCacheErrorCode.RetryLater)
                        Thread.Sleep(waitTime); //Put the thread to sleep for the duration and try again
                }
            }

            //If we were never able to retrieve the cache...
            if (itemInCache == null)
                throw new ApplicationException(string.Format("Unable to retrieve item {0} from cache after {1} retry attempts", key, attemptCount));

            return itemInCache;
        }

        private static bool RetryPut(string key, object value, TimeSpan timeout, int attemptCount = 10, int waitTime = 100, string region = "")
        {
            var cache = GetCache();
            object cacheObject = null;

            for (var i = 0; i < attemptCount; i++)
            {
                try
                {
                    if(string.IsNullOrWhiteSpace(region) || string.IsNullOrEmpty(region))
                    {
                        //Attempt to get the item from cache again...
                        cacheObject = cache.Put(key, value, timeout);
                    }
                    else
                    {
                        cacheObject = cache.Put(key, value, timeout,region);
                    }
                    
                }
                catch (DataCacheException cacheException)
                {
                    //If we received another "retry later" error from the cache
                    if (cacheException.ErrorCode == DataCacheErrorCode.RetryLater)
                        Thread.Sleep(waitTime); //Put the thread to sleep for the duration and try again
                }
            }

            if(cacheObject == null)
                throw new ApplicationException(string.Format("Unable to save item {0} in cache after {1} retry attempts", key, attemptCount));

            return true;
        }

        private static bool RetryDelete(string key, int attemptCount = 10, int waitTime = 100, string region = "")
        {
            var cache = GetCache();
            var removeResult = false;

            for (var i = 0; i < attemptCount; i++)
            {
                try
                {

                    if (string.IsNullOrWhiteSpace(region) || string.IsNullOrEmpty(region))
                    {
                        //Attempt to remove the item from cache again...
                        removeResult = cache.Remove(key);
                    }
                    else
                    {
                        removeResult = cache.Remove(key, region);
                    }
                }
                catch (DataCacheException cacheException)
                {
                    //If we received another "retry later" error from the cache
                    if (cacheException.ErrorCode == DataCacheErrorCode.RetryLater)
                        Thread.Sleep(waitTime); //Put the thread to sleep for the duration and try again
                }
            }

            return removeResult;
        }

        #endregion

        #region Overrides of CacheServiceBase

        public void Save(string key, object value, CacheItemPolicy policy, string region)
        {
            try
            {
                var expiration = policy.SlidingExpiration;

                if (policy.SlidingExpiration == TimeSpan.MinValue)
                {
                    expiration = _default_duration;
                }

                var cache = GetCache();

                cache.Put(key, value, expiration,region);
            }
            catch (DataCacheException cacheException)
            {
                switch (cacheException.ErrorCode)
                {
                    case DataCacheErrorCode.RetryLater:
                        RetryPut(key, value, _default_duration);
                        break;
                    case DataCacheErrorCode.Timeout:
                    default:
                        throw; //If the cache isn't available or it's an error we can't handle
                }
            }
        }

        public IEnumerable<KeyValuePair<string, object>> GetObjectsInRegion(string region)
        {
            var cache = GetCache();
            return cache.GetObjectsInRegion(region);
        }

        protected override bool Save(string key, object value, CacheItemPolicy policy)
        {
            try
            {
                var expiration = policy.SlidingExpiration;

                if (policy.SlidingExpiration == TimeSpan.MinValue)
                {
                    expiration = _default_duration;
                }

                var cache = GetCache();

                var item = cache.Put(key, value, expiration);
                return true;
            }
            catch (DataCacheException cacheException)
            {
                switch (cacheException.ErrorCode)
                {
                    case DataCacheErrorCode.RetryLater:
                        return RetryPut(key, value, _default_duration);
                        break;
                    case DataCacheErrorCode.Timeout:
                    default:
                        throw; //If the cache isn't available or it's an error we can't handle
                }
            }
        }

        public object Get(string key, string region)
        {
            try
            {
                var cache = GetCache();

                return cache.Get(key, region);
            }
            catch (DataCacheException cacheException)
            {
                //So why isn't this switch statement it's own method?
                //Answer: the naked throw; from the catch block allows
                //us to preserve the stacktrace from the original error.
                //A new method wouldn't allow it, although there is probably a way
                //we can refactor it.
                switch (cacheException.ErrorCode)
                {
                    case DataCacheErrorCode.RetryLater:
                        return RetryGet(key,10,100,region);
                    case DataCacheErrorCode.Timeout:
                    default:
                        throw; //If the cache isn't available or it's an error we can't handle
                }
            }
        }

        public override object Get(string key)
        {
            try
            {
                var cache = GetCache();

                return cache.Get(key);
            }
            catch (DataCacheException cacheException)
            {
                //So why isn't this switch statement it's own method?
                //Answer: the naked throw; from the catch block allows
                //us to preserve the stacktrace from the original error.
                //A new method wouldn't allow it, although there is probably a way
                //we can refactor it.
                switch (cacheException.ErrorCode)
                {
                    case DataCacheErrorCode.RetryLater:
                        return RetryGet(key);
                    case DataCacheErrorCode.Timeout:
                    default:
                        throw; //If the cache isn't available or it's an error we can't handle
                }
            }
        }

        public override IDictionary<string, object> Get(string[] keys)
        {
            throw new NotImplementedException();
        }

        public bool Remove(string key, string region)
        {
            try
            {
                if (!Exists(key, region))
                {
                    return false;
                }

                var cache = GetCache();

                return cache.Remove(key, region);
            }
            catch (DataCacheException cacheException)
            {
                switch (cacheException.ErrorCode)
                {
                    case DataCacheErrorCode.KeyDoesNotExist:
                    case DataCacheErrorCode.RegionDoesNotExist:
                        return false; //We weren't able to find the 
                    case DataCacheErrorCode.RetryLater:
                        return RetryDelete(key,10,100,region);
                    case DataCacheErrorCode.Timeout:
                    default:
                        throw; //If the cache isn't available or it's an error we can't handle
                }
            }
        }

        public override bool Remove(string key)
        {
            try
            {
                if (!Exists(key))
                {
                    return false;
                }

                var cache = GetCache();

                return cache.Remove(key);
            }
            catch (DataCacheException cacheException)
            {
                switch (cacheException.ErrorCode)
                {
                    case DataCacheErrorCode.KeyDoesNotExist:
                    case DataCacheErrorCode.RegionDoesNotExist:
                        return false; //We weren't able to find the 
                    case DataCacheErrorCode.RetryLater:
                        return RetryDelete(key);
                    case DataCacheErrorCode.Timeout:
                    default:
                        throw; //If the cache isn't available or it's an error we can't handle
                }
            }
        }

        public bool Exists(string key, string region)
        {
            try
            {
                var cache = GetCache();

                return cache.Get(key, region) != null;
            }
            catch (DataCacheException cacheException)
            {
                /* 
                 * Instead of just tracing the exception, we're going to check out the status
                 * and see if we can retry the request in a second
                 */

                switch (cacheException.ErrorCode)
                {
                    case DataCacheErrorCode.KeyDoesNotExist:
                    case DataCacheErrorCode.RegionDoesNotExist:
                        return false; //We weren't able to find the 
                    case DataCacheErrorCode.RetryLater:
                        return RetryGet(key,10,100, region) != null;
                    case DataCacheErrorCode.Timeout:
                    default:
                        throw; //If the cache isn't available or it's an error we can't handle
                }
            }
        }

        public override bool Exists(string key)
        {
            try
            {
                var cache = GetCache();

                return cache.Get(key) != null;
            }
            catch (DataCacheException cacheException)
            {
                /* 
                 * Instead of just tracing the exception, we're going to check out the status
                 * and see if we can retry the request in a second
                 */

                switch (cacheException.ErrorCode)
                {
                    case DataCacheErrorCode.KeyDoesNotExist:
                    case DataCacheErrorCode.RegionDoesNotExist:
                        return false; //We weren't able to find the 
                    case DataCacheErrorCode.RetryLater:
                        return RetryGet(key) != null;
                    case DataCacheErrorCode.Timeout:
                    default:
                        throw; //If the cache isn't available or it's an error we can't handle
                }
            }
        }

        #endregion
    }
}
