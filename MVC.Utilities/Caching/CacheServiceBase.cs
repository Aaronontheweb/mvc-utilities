using System;
using System.Collections.Generic;
using System.Runtime.Caching;

namespace MVC.Utilities.Caching
{
    /// <summary>
    /// Abstract base class for working with cache service implementations
    /// </summary>
    public abstract class CacheServiceBase : ICacheService
    {

        protected readonly TimeSpan _default_duration;

        protected CacheServiceBase(TimeSpan defaultDuration)
        {
            _default_duration = defaultDuration;
        }

        #region abstract ICacheService members

        /// <summary>
        /// Private method which contains business logic for saving / creating items in cache
        /// </summary>
        /// <param name="key">The key of the item to be saved</param>
        /// <param name="value">The value of the item to be saved</param>
        /// <param name="policy">The expiration policy of the item to be saved</param>
        protected abstract bool Save(string key, object value, CacheItemPolicy policy);

        public abstract object Get(string key);
        public abstract IDictionary<string, object> Get(string[] keys);

        public abstract bool Remove(string key);

        #endregion

        #region Implementation of ICacheService

        public abstract bool Exists(string key);

        public bool Save(string key, object value)
        {
            return Save(key, value, GetDefaultPolicy());
        }

        public bool Save(string key, object value, TimeSpan slidingExpiration)
        {
            return Save(key, value, GetSlidingPolicy(slidingExpiration));
        }

        public bool Save(string key, object value, DateTime absoluteExpiration)
        {
            return Save(key, value, GetAbsolutePolicy(absoluteExpiration));
        }

        public object this[string key]
        {
            get { return Get(key); }
            set { Save(key, value, GetDefaultPolicy()); }
        }

        #endregion


        #region Expiration Policy Helpers

        protected CacheItemPolicy GetDefaultPolicy()
        {
            return new CacheItemPolicy() { SlidingExpiration = _default_duration };
        }

        protected CacheItemPolicy GetAbsolutePolicy(DateTime absoluteExpiration)
        {
            return new CacheItemPolicy { AbsoluteExpiration = absoluteExpiration };
        }

        protected CacheItemPolicy GetSlidingPolicy(TimeSpan slidingExpiration)
        {
            return new CacheItemPolicy { SlidingExpiration = slidingExpiration };
        }

        #endregion
    }
}
