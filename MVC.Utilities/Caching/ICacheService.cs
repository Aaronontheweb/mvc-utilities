using System;


namespace MVC.Utilities.Caching
{
    /// <summary>
    /// Contract for defining all of the caching services we want to work with
    /// </summary>
    public interface ICacheService
    {
        /// <summary>
        /// Validates if an object with this key exists in the cache
        /// </summary>
        /// <param name="key">The key of the object to check</param>
        /// <returns>True if it exists, false if it doesn't</returns>
        bool Exists(string key);

        /// <summary>
        /// Insert an item into the cache with no specifics as to how it will be used
        /// </summary>
        /// <param name="key">The key used to map this object</param>
        /// <param name="value">The value to be saved to the cache</param>
        bool Save(string key, object value);

        /// <summary>
        /// Insert an item in the cache with the expiration that it will expire if not used past its window
        /// </summary>
        /// <param name="key">The key used to map this object</param>
        /// <param name="value">The object to insert</param>
        /// <param name="slidingExpiration">The expiration window</param>
        bool Save(string key, object value, TimeSpan slidingExpiration);

        /// <summary>
        /// Insert an item in the cache with the expiration that will expire at a specific point in time
        /// </summary>
        /// <param name="key">The key used to map this object</param>
        /// <param name="value">The object to insert</param>
        /// <param name="absoluteExpiration">The DateTime in which this object will expire</param>
        bool Save(string key, object value, DateTime absoluteExpiration);

        /// <summary>
        /// Retrieves a cached object from the cache
        /// </summary>
        /// <param name="key">The key used to identify this object</param>
        /// <returns>The object from the database, or an exception if the object doesn't exist</returns>
        object Get(string key);

        /// <summary>
        /// Retrieves a cached object using an indexers
        /// </summary>
        /// <param name="key">The key used to identify this object</param>
        /// <returns>The cached object</returns>
        object this[string key] { get; set; }

        /// <summary>
        /// Removes an object from the cache with the specified key
        /// </summary>
        /// <param name="key">The key used to identify this object</param>
        /// <returns>True if the object was removed, false if it didn't exist or was unable to be removed</returns>
        bool Remove(string key);
    }
}
