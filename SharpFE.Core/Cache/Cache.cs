//-----------------------------------------------------------------------
// <copyright file="Cache.cs" company="Iain Sproat">
//     Copyright Iain Sproat, 2013.
// </copyright>
//-----------------------------------------------------------------------
namespace SharpFE.Cache
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Very simple cache implementation
    /// </summary>
    /// <typeparam name="TKey">Type of the keys in this cache</typeparam>
    /// <typeparam name="TValue">Type of the values stored in this cache</typeparam>
    public class Cache<TKey, TValue>
    {
        /// <summary>
        /// Internal data store used by the cache
        /// </summary>
        private IDictionary<TKey, CachedValue<TValue>> internalStore = new Dictionary<TKey, CachedValue<TValue>>();
        
        /// <summary>
        /// Initializes a new instance of the <see cref="Cache{TKey,TValue}">Cache</see> class.
        /// </summary>
        public Cache()
        {
            // empty
        }
        
        /// <summary>
        /// Determines as to whether this cache has stored an item at the provided key.
        /// </summary>
        /// <param name="key">The key to check as to whether it is stored in the cache</param>
        /// <returns>True if the cache contains the key, false otherwise</returns>
        public bool ContainsKey(TKey key)
        {
            return this.internalStore.ContainsKey(key);
        }
        
        /// <summary>
        /// Determines whether the cache contains a key
        /// </summary>
        /// <param name="key">The key to search for within this cache</param>
        /// <param name="cachedValue">The value which is stored in this cache at the given key</param>
        /// <returns>True if the key is contained in this cache, false otherwise</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "1#", Justification = "Out parameter is required in order to return information on both the key containment and the item stored by the key, which may be null")]
        public bool ContainsKey(TKey key, out TValue cachedValue)
        {
            return this.ContainsKey(key, false, 0, out cachedValue);
        }
        
        /// <summary>
        /// Determines whether the cache contains a key, and that the item stored is valid
        /// </summary>
        /// <param name="key">The key to search for in this cache</param>
        /// <param name="validHash">A hash which determines whether the item stored at the key is valid</param>
        /// <param name="cachedValue">If the cache contains the key, the item stored at the key will be output</param>
        /// <returns>true if the cache contains the key, and the item stored is valid for the given hash</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "2#", Justification = "Out parameter is required in order to return information on both the key containment and the item stored by the key, which may be null")]
        public bool ContainsKey(TKey key, int validHash, out TValue cachedValue)
        {
            return this.ContainsKey(key, true, validHash, out cachedValue);
        }
        
        /// <summary>
        /// Saves a key, value pair to this cache
        /// </summary>
        /// <param name="key">The key to save</param>
        /// <param name="value">The value to save</param>
        /// <param name="hashForValidityOfValue">The hash which is used to determine the saved value's ongoing validity</param>
        /// <returns>True if the key value pair were successfully saved to the cache, false otherwise</returns>
        public bool Save(TKey key, TValue value, int hashForValidityOfValue)
        {
            CachedValue<TValue> valueToCache = new CachedValue<TValue>(hashForValidityOfValue, value);
            if (this.ContainsKey(key))
            {
                // update cache with new value
                this.internalStore[key] = valueToCache;
            }
            else
            {
                // store new value in cache
                this.internalStore.Add(key, valueToCache);
            }
            
            return true;
        }
        
        /// <summary>
        /// Determines whether a key is already contained within this cache
        /// </summary>
        /// <param name="key">The key to search for within this cache</param>
        /// <param name="hashWasProvided">Flag to indicate whether a hash was provided alongside the key, and whether the validity of the hash should be accounted for within the results of this method</param>
        /// <param name="validHash">An optional hash to use to ensure the key contains valid results.</param>
        /// <param name="cachedValue">The value saved alongside the key, if it is found</param>
        /// <returns>True if the key was found.  Also checks that the value is valid, if the hash check was requested.</returns>
        private bool ContainsKey(TKey key, bool hashWasProvided, int validHash, out TValue cachedValue)
        {
            cachedValue = default(TValue);
            
            bool cachedResultExistsForThisKey = this.internalStore.ContainsKey(key);
            
            if (cachedResultExistsForThisKey)
            {
                CachedValue<TValue> cacheResult = this.internalStore[key];
                cachedValue = cacheResult.Value;
                if (hashWasProvided && cacheResult.IsValid(validHash))
                {
                    return true;
                }
            }
            
            return false;
        }
    }
}
