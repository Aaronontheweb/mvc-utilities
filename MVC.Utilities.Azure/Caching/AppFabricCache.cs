using System;
using System.Diagnostics;
using System.Runtime.Caching;
using Microsoft.ApplicationServer.Caching;

namespace MVC.Utilities.Caching
{
    public class AppFabricCacheService : CacheServiceBase
    {
        private static DataCache _cache;
        private static DataCacheFactory _factory;

        public AppFabricCacheService(TimeSpan defaultDuration)
            : base(defaultDuration)
        {

        }

        private static DataCache GetCache()
        {
            if (_cache != null) return _cache;

            // Disable tracing to avoid informational / verbose messages on the web page
            DataCacheClientLogManager.ChangeLogLevel(TraceLevel.Off);

            _factory = new DataCacheFactory();
            _cache = _factory.GetDefaultCache();

            return _cache;
        }

        #region Overrides of CacheServiceBase

        protected override void Save(string key, object value, CacheItemPolicy policy)
        {
            try
            {
                var expiration = policy.SlidingExpiration;

                if (policy.SlidingExpiration == TimeSpan.MinValue)
                {
                    expiration = _default_duration;
                }

                var cache = GetCache();

                if (Exists(key))
                {
                    cache.Put(key, value, expiration);
                }
                else
                {
                    cache.Add(key, value, expiration);
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(string.Format("Caching error; Message {0}  Trace {1}", ex.Message, ex.StackTrace));
            }
        }

        public override object Get(string key)
        {
            try
            {
                var cache = GetCache();

                return cache.Get(key);
            }
            catch (Exception ex)
            {
                Trace.TraceError(string.Format("Caching error; Message {0}  Trace {1}", ex.Message, ex.StackTrace));
                return null;
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
            catch (Exception ex)
            {
                Trace.TraceError(string.Format("Caching error; Message {0}  Trace {1}", ex.Message, ex.StackTrace));
                return false;
            }
        }

        public override bool Exists(string key)
        {
            try
            {
                var cache = GetCache();

                return cache.Get(key) != null;
            }
            catch (Exception ex)
            {
                Trace.TraceError(string.Format("Caching error; Message {0}  Trace {1}", ex.Message, ex.StackTrace));
                return false;
            }
        }

        #endregion
    }
}
