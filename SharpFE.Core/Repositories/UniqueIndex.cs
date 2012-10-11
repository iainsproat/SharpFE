//-----------------------------------------------------------------------
// <copyright file="UniqueIndex.cs" company="SharpFE">
//     Copyright Iain Sproat, 2012.
// </copyright>
//-----------------------------------------------------------------------

namespace SharpFE
{
    using System;
    using System.Collections.Generic;
    
    /// <summary>
    /// An index in which each value can only appear against zero or one of the keys
    /// </summary>
    /// <typeparam name="Key">The type of the instances which will form the keys stored in this index.</typeparam>
    /// <typeparam name="Value">The type of the instances which will form the values stored in this index.</typeparam>
    internal class UniqueIndex<Key, Value> : Index<Key, Value>
    {
        /// <summary>
        /// A reverse index to keep track of which item is stored against which key
        /// </summary>
        private IDictionary<Value, Key> reverseIndex = new Dictionary<Value, Key>();
        
        /// <summary>
        /// Initializes a new instance of the <see cref="UniqueIndex{Key,Value}" /> class.
        /// </summary>
        public UniqueIndex()
            : base(false)
        {
            // empty
        }
        
        /// <summary>
        /// Allows an item to be added to a key.
        /// The key is added if it does not already exist.
        /// </summary>
        /// <param name="key">The key to add</param>
        /// <param name="item">The item to add to the list of items for the given key</param>
        public override void Add(Key key, Value item)
        {
            this.RemoveItemFromOtherKeyLists(item);
            this.reverseIndex.Add(item, key);
            base.Add(key, item);
        }
        
        /// <summary>
        /// Adds an element with the provided key and value to the index.
        /// </summary>
        /// <param name="key">The object to use as the key of the element to add</param>
        /// <param name="value">The object to use as the value of the element to add</param>
        public override void Add(Key key, IList<Value> value)
        {
            if (this.ContainsKey(key))
            {
                throw new InvalidOperationException("Cannot Add if the key is already contained in the index. Replacement of lists is not supported.");
            }
            
            foreach (Value item in value)
            {
                this.RemoveItemFromOtherKeyLists(item);
                this.reverseIndex.Add(item, key);
                base.Add(key, item);
            }
        }
        
        /// <summary>
        /// Gets the key of an item
        /// </summary>
        /// <param name="item">The item for which we wish to search for the key</param>
        /// <returns>the key for the item</returns>
        public Key KeyOfValue(Value item)
        {
            Guard.AgainstBadArgument(
                () => { return !this.reverseIndex.ContainsKey(item); },
                "The item is not contained by any key in this UniqueIndex",
                "item");
            
            return this.reverseIndex[item];
        }
        
        /// <summary>
        /// Removes the element with the specified key from the Index.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <returns>true if the element is successfully removed, otherwise false.  This method also returns false if the key was not found in the Index</returns>
        public override bool Remove(Key key)
        {
            if (!this.ContainsKey(key))
            {
                return false;
            }
            
            // clear the reverse index of values held by this key
            IList<Value> values = this[key];
            foreach (Value value in values)
            {
                this.reverseIndex.Remove(value);
            }
            
            return base.Remove(key);
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
        public override bool Remove(Key key, Value value)
        {
            if (!this.ContainsKey(key))
            {
                return false;
            }
            
            this.reverseIndex.Remove(value);
            
            return base.Remove(key, value);
        }
        
        /// <summary>
        /// Removes the first occurence of the specified object from the index
        /// </summary>
        /// <param name="item">The object to remove from the index</param>
        /// <returns>true if the item was successfully removed, false otherwise.  This method also returns false if the item was not found in the Index</returns>
        public override bool Remove(KeyValuePair<Key, IList<Value>> item)
        {
            return this.Remove(item.Key);
        }
        
        /// <summary>
        /// Removes the item from any other key it is currently registered with
        /// </summary>
        /// <param name="item">the item to remove from any existing key</param>
        private void RemoveItemFromOtherKeyLists(Value item)
        {
            if (this.reverseIndex.ContainsKey(item))
            {
                Key previousKey = this.reverseIndex[item];
                this.Remove(previousKey, item);
                this.reverseIndex.Remove(item);
            }
        }
    }
}
