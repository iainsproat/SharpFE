//-----------------------------------------------------------------------
// <copyright file="Cache.cs" company="Iain Sproat">
//     Copyright Iain Sproat, 2013.
// </copyright>
//-----------------------------------------------------------------------
namespace SharpFE.Cache
{
    using System;
    using System.Collections.Generic;

    public delegate TValue CreateValue<TValue>();
    
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
        protected IDictionary<TKey, CachedValue<TValue>> internalStore = new Dictionary<TKey, CachedValue<TValue>>();
        
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
        /// Determines whether the cache contains a key, and that the item stored is valid
        /// </summary>
        /// <param name="key">The key to search for in this cache</param>
        /// <param name="validHash">A hash which determines whether the item stored at the key is valid</param>
        /// <param name="cachedValue">If the cache contains the key, the item stored at the key will be output</param>
        /// <returns>true if the cache contains the key, and the item stored is valid for the given hash</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "2#", Justification = "Out parameter is required in order to return information on both the key containment and the item stored by the key, which may be null")]
        public bool ContainsKey(TKey key, int validHash)
        {
            bool cachedResultExistsForThisKey = this.ContainsKey(key);
            if (!cachedResultExistsForThisKey)
            {
                return false;
            }
            
            CachedValue<TValue> cacheResult = this.internalStore[key];
            return cacheResult.IsValid(validHash);
        }
        
        /// <summary>
        /// Saves a key, value pair to this cache
        /// </summary>
        /// <param name="key">The key to save</param>
        /// <param name="value">The value to save</param>
        /// <returns>True if the key value pair were successfully saved to the cache, false otherwise</returns>
        public bool Save(TKey key, TValue value)
        {
            return this.Save(key, value, 0);
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
        
        public TValue GetOrCreateAndSave(TKey key, CreateValue<TValue> createValue)
        {
            return this.GetOrCreateAndSave(key, 0, createValue);
        }
        
        public TValue GetOrCreateAndSave(TKey key, int hashForValidityOfValue, CreateValue<TValue> createValue)
        {
            if(this.ContainsKey(key, hashForValidityOfValue))
            {
                CachedValue<TValue> cacheResult = this.internalStore[key];
                return cacheResult.Value;
            }
            
            TValue value = createValue();
            this.Save(key, value, hashForValidityOfValue);
            return value;
        }
    }
}
