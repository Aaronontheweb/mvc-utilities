using System;
using System.Collections.Generic;
using System.Runtime.Caching;

namespace MVC.Utilities.Caching
{
    public class RuntimeCacheService : CacheServiceBase
    {
        private readonly ObjectCache _cache;
        
        public RuntimeCacheService() : this(MemoryCache.Default, new TimeSpan(0, 20, 0))
        {
        }

        public RuntimeCacheService(ObjectCache cache, TimeSpan defaultSlidingDuration)
            : base(defaultSlidingDuration)
        {
            //Point the ObjectCache to a derived class which implements the appropriate policy
            _cache = cache;
        }

        #region Implementation of ICacheService

        public override bool Exists(string key)
        {
            return _cache.Contains(key);
        }

        /// <summary>
        /// Private method which contains business logic for saving / creating items in cache
        /// </summary>
        /// <param name="key">The key of the item to be saved</param>
        /// <param name="value">The value of the item to be saved</param>
        /// <param name="policy">The expiration policy of the item to be saved</param>
        protected override bool Save(string key, object value, CacheItemPolicy policy)
        {
            if (Exists(key))
            {
                _cache.Set(key, value, policy);
                return true;
            }
            else
            {
                return _cache.Add(key, value, policy);
            }
        }

        public override object Get(string key)
        {
            return _cache.Get(key);
        }

        public override IDictionary<string, object> Get(string[] keys)
        {
            return _cache.GetValues(keys);
        }

        public override bool Remove(string key)
        {
            //Remove the item from the cache if it exists...
            if (Exists(key))
            {
                _cache.Remove(key);
                return !Exists(key); //Return true or false depending upon if the item still exists..
            }

            return false; //Return false if the item never existed
        }

        #endregion
    }
}
