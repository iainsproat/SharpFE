//-----------------------------------------------------------------------
// <copyright file="Index.cs" company="SharpFE">
//     Copyright Iain Sproat, 2012.
// </copyright>
//-----------------------------------------------------------------------

namespace SharpFE
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// An Index is a special kind of dictionary.  The key is stored with a list of values.
    /// </summary>
    /// <typeparam name="Key">The type of the instances which will form the keys stored in this index.</typeparam>
    /// <typeparam name="Value">The type of the instances which will form the values stored in this index.</typeparam>
    internal class Index<Key, Value> : IDictionary<Key, IList<Value>>
    {
        /// <summary>
        /// Flag indicating whether duplicate values are allowed in the lists for each key.
        /// </summary>
        private bool duplicateValuesAreAllowedAgainstEachKey;
        
        /// <summary>
        /// The wrapped dictionary which stores the data for this index.
        /// </summary>
        private IDictionary<Key, IList<Value>> internalStore = new Dictionary<Key, IList<Value>>();
        
        /// <summary>
        /// Initializes a new instance of the <see cref="Index{Key,Value}" /> class.
        /// </summary>
        public Index()
            : this(false)
        {
            // empty
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="Index{Key,Value}" /> class.
        /// </summary>
        /// <param name="canHaveDuplicateValuesRegisteredWithAKey">Indicates whether the list of values stored for a key can have duplicates.</param>
        public Index(bool canHaveDuplicateValuesRegisteredWithAKey)
        {
            this.duplicateValuesAreAllowedAgainstEachKey = canHaveDuplicateValuesRegisteredWithAKey;
        }
        
        /// <summary>
        /// Gets the number of keys stored in this index.
        /// </summary>
        public int Count
        {
            get
            {
                return this.internalStore.Count;
            }
        }
        
        /// <summary>
        /// Gets an ICollection containing the keys of the index
        /// </summary>
        public ICollection<Key> Keys
        {
            get
            {
                return this.internalStore.Keys;
            }
        }
        
        /// <summary>
        /// Gets a collection containing the values in the Index
        /// </summary>
        public ICollection<IList<Value>> Values
        {
            get
            {
                ICollection<IList<Value>> values = this.internalStore.Values;
                int numberOfValues = values.Count;
                
                IList<IList<Value>> response = new List<IList<Value>>(numberOfValues);
                foreach (IList<Value> value in values)
                {
                    response.Add(new List<Value>(value)); // clone each list to prevent modification of it, and save it back to the response
                }
                
                return response;
            }
        }
        
        /// <summary>
        /// Gets a value indicating whether the Index is readonly.
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return this.internalStore.IsReadOnly;
            }
        }
        
        /// <summary>
        /// Gets or sets the element with the specified key.
        /// </summary>
        /// <param name="key">The key of the element to get or set</param>
        /// <returns>The element with the specified key</returns>
        public IList<Value> this[Key key]
        {
            get { return this.Get(key); }  // by using the Get method we ensure the value is cloned to prevent modification of it.
            set { this.Add(key, value); }
        }
        
        /// <summary>
        /// Gets all the values stored for a key.
        /// This method clones the values to prevent corruption of the data.
        /// </summary>
        /// <param name="key">The key to search for.</param>
        /// <returns>An empty list if the key does not exist in the index.</returns>
        public IList<Value> Get(Key key)
        {
            if (!this.ContainsKey(key))
            {
                return new List<Value>(0);
            }
            
            return new List<Value>(this.internalStore[key]);
        }
        
        /// <summary>
        /// Gets the enumerator over the stored data in this index
        /// </summary>
        /// <returns>The enumerator for this index</returns>
        public IEnumerator<KeyValuePair<Key, IList<Value>>> GetEnumerator()
        {
            return new IndexEnumerator<Key, Value>(this.internalStore.GetEnumerator());
        }
        
        /// <summary>
        /// Gets the enumerator over the stored data in this index
        /// </summary>
        /// <returns>The enumerator for this index</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        
        /// <summary>
        /// Allows an item to be added to a key.
        /// The key is added if it does not already exist.
        /// </summary>
        /// <param name="key">The key to add</param>
        /// <param name="item">The item to add to the list of items for the given key</param>
        public virtual void Add(Key key, Value item)
        {
            IList<Value> values;
            if (!this.ContainsKey(key))
            {
                values = new List<Value>();
                this.internalStore.Add(key, values);
            }
            else
            {
                values = this.internalStore[key];
            }
            
            if (!this.duplicateValuesAreAllowedAgainstEachKey && values.Contains(item))
            {
                return; // do nothing for duplicates
            }
            
            values.Add(item);
        }
        
        /// <summary>
        /// Removes an item from all keys containing it.
        /// </summary>
        /// <param name="itemToRemove">The item to remove from all keys.</param>
        /// <returns>true if at least one item was removed.</returns>
        public bool RemoveValue(Value itemToRemove)
        {
            bool success = false;
            ICollection<IList<Value>> keyValues = this.internalStore.Values;
            foreach (IList<Value> values in keyValues)
            {
                if (values.Remove(itemToRemove))
                {
                    success = true;
                }
            }
            
            return success;
        }
        
        /// <summary>
        /// Removes the element with the specified key from the Index.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <returns>true if the element is successfully removed, otherwise false.  This method also returns false if the key was not found in the Index</returns>
        public virtual bool Remove(Key key)
        {
            return this.internalStore.Remove(key);
        }
        
        /// <summary>
        /// Removes the value from the list at the specified key from the Index
        /// </summary>
        /// <param name="key">The key of the list from which the value should be removed</param>
        /// <param name="value">The element to remove from the list at the given key</param>
        /// <returns>
        /// true if the element is successfully removed; otherwise, false.
        /// This method also returns false if the key was not found in the Index, or the value was not found in the list at the specified key
        /// </returns>
        public virtual bool Remove(Key key, Value value)
        {
            if (!this.ContainsKey(key))
            {
                return false;
            }
            
            return this.internalStore[key].Remove(value);
        }
        
        /// <summary>
        /// Removes the first occurence of the specified object from the index
        /// </summary>
        /// <param name="item">The object to remove from the index</param>
        /// <returns>true if the item was successfully removed, false otherwise.  This method also returns false if the item was not found in the Index</returns>
        public virtual bool Remove(KeyValuePair<Key, IList<Value>> item)
        {
            return this.internalStore.Remove(item);
        }
        
        /// <summary>
        /// Copies the elements of the index to a System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array">the one-dimensional System.Array that is the destination of the elements copied from the index.  The System.Array must have zero-based indexing.</param>
        /// <param name="arrayIndex">the zero-based index in the array at which copying begins</param>
        public void CopyTo(KeyValuePair<Key, IList<Value>>[] array, int arrayIndex)
        {
            this.internalStore.CopyTo(array, arrayIndex);
        }
        
        /// <summary>
        /// Determines whether the Index contains a specific value
        /// </summary>
        /// <param name="item">the object to locate in the index</param>
        /// <returns>true if the item is found in the index; otherwise, false</returns>
        public bool Contains(KeyValuePair<Key, IList<Value>> item)
        {
            return this.internalStore.Contains(item);
        }
        
        /// <summary>
        /// Removes all items from the index
        /// </summary>
        public void Clear()
        {
            this.internalStore.Clear();
        }
        
        /// <summary>
        /// Adds an element with the provided key and value to the index.
        /// </summary>
        /// <param name="key">The object to use as the key of the element to add</param>
        /// <param name="value">The object to use as the value of the element to add</param>
        public virtual void Add(Key key, IList<Value> value)
        {
            Guard.AgainstInvalidState(() => { return this.ContainsKey(key); },
                                      "Cannot Add if the key is already contained in the index. Replacing the entire list is not supported.");
            
            foreach (Value item in value)
            {
                this.Add(key, item);
            }
        }
        
        /// <summary>
        /// Adds an item to the index
        /// </summary>
        /// <param name="item">The object to add to the index</param>
        public void Add(KeyValuePair<Key, IList<Value>> item)
        {
            Guard.AgainstInvalidState(() => { return this.ContainsKey(item.Key); },
                                      "Cannot Add if the key is already contained in the index");
            
            this.internalStore.Add(item.Key, item.Value);
        }
        
        /// <summary>
        /// Gets the value associated with the specified key
        /// </summary>
        /// <param name="key">The key whose value to get</param>
        /// <param name="value">when this method returns, the value associated with the specified key, if the key is found; otherwise, an empty list.  the parameter is assumed passed uninitialized.</param>
        /// <returns>true if Index contains an element with the specified key; otherwise, false.</returns>
        public bool TryGetValue(Key key, out IList<Value> value)
        {
            IList<Value> tempValue;
            bool success = this.internalStore.TryGetValue(key, out tempValue);
            value = new List<Value>(tempValue); // ensure the value is cloned to prevent modification of it.
            return success;
        }
        
        /// <summary>
        /// Determines whether the index contains an element with the specified key
        /// </summary>
        /// <param name="key">The key to locate in the index</param>
        /// <returns>true if the index contains an element with the specified key; otherwise, false.</returns>
        public bool ContainsKey(Key key)
        {
            return this.internalStore.ContainsKey(key);
        }
    }
}
