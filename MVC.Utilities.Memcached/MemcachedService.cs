using System;
using System.Runtime.Caching;
using Enyim.Caching;
using Enyim.Caching.Configuration;
using Enyim.Caching.Memcached;
using MVC.Utilities.Caching;

namespace MVC.Utilities.Memcached
{
    public class MemcachedService : CacheServiceBase
    {
        private static MemcachedClient _memcachedClient;

        public MemcachedService(TimeSpan defaultDuration)
            : base(defaultDuration)
        {
        }

        /// <summary>
        /// Initializes the Memcached Client.
        /// 
        /// Method should only be called ONCE.
        /// </summary>
        /// <param name="clientConfiguration">An instantiated MemcachedClientConfiguration</param>
        public static void Initialize(MemcachedClientConfiguration clientConfiguration)
        {
            _memcachedClient = new MemcachedClient(clientConfiguration);
        }

        /// <summary>
        /// Initializes the Memcached Client.
        /// 
        /// Method should only be called ONCE.
        /// </summary>
        /// <param name="configSectionName">The name of the configuration section to use for Memcached</param>
        public static void Initialize(string configSectionName)
        {
            _memcachedClient = new MemcachedClient(configSectionName);
        }

        /// <summary>
        /// Initializes the Memcached Client using default configuration section of web/app.config
        /// 
        /// Method should only be called ONCE.
        /// </summary>
        public static void Initialize()
        {
            _memcachedClient = new MemcachedClient();
        }

        #region Cache Helpers

        private static MemcachedClient GetCacheClient()
        {
            if (_memcachedClient == null)
            {
                throw new InvalidOperationException("Memcache has not been initialized. You need to call the Initialize method before using it.");
            }

            return _memcachedClient;
        }

        #endregion

        #region Overrides of CacheServiceBase

        protected override bool Save(string key, object value, CacheItemPolicy policy)
        {
            var cache = GetCacheClient();

            var expiration = policy.SlidingExpiration;

            //No set expiration policy
            if (policy.SlidingExpiration == default(TimeSpan) && policy.AbsoluteExpiration == default(DateTimeOffset))
            {
                return cache.Store(StoreMode.Set, key, value);
            }
            
            if (policy.SlidingExpiration == default(TimeSpan)) //Absolute expiration
            {
                return cache.Store(StoreMode.Set, key, value, policy.AbsoluteExpiration.DateTime);
            }
            
            //Sliding expiration
            return cache.Store(StoreMode.Set, key, value, policy.SlidingExpiration);
        }

        #endregion

        #region Overrides of CacheServiceBase

        public override object Get(string key)
        {
            var cache = GetCacheClient();
            return cache.Get(key);
        }

        public override bool Remove(string key)
        {
            var cache = GetCacheClient();
            return cache.Remove(key);
        }

        public override bool Exists(string key)
        {
            object objInDb;
            var cache = GetCacheClient();
            return cache.TryGet(key, out objInDb);
        }

        #endregion
    }
}
