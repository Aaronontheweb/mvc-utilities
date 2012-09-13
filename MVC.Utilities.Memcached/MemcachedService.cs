using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
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

        protected override void Save(string key, object value, CacheItemPolicy policy)
        {
            var cache = GetCacheClient();

            var expiration = policy.SlidingExpiration;
            bool cacheOpResult = false;

            if (policy.SlidingExpiration == default(TimeSpan) && policy.AbsoluteExpiration == default(DateTimeOffset))
            {
                cacheOpResult = cache.Store(StoreMode.Set, key, value);
            }
            else if (policy.SlidingExpiration == default(TimeSpan)) //Absolute expiration
            {
                cacheOpResult = cache.Store(StoreMode.Set, key, value, policy.AbsoluteExpiration.DateTime);
            }
            else //Sliding expiration
            {
                cacheOpResult = cache.Store(StoreMode.Set, key, value, policy.SlidingExpiration);
            }

        }

        #endregion

        #region Overrides of CacheServiceBase

        public override object Get(string key)
        {
            throw new NotImplementedException();
        }

        public override bool Remove(string key)
        {
            throw new NotImplementedException();
        }

        public override bool Exists(string key)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
