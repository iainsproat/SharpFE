//-----------------------------------------------------------------------
// <copyright file="IndexEnumerator.cs" company="SharpFE">
//     Copyright Iain Sproat, 2012.
// </copyright>
//-----------------------------------------------------------------------

namespace SharpFE
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Supports a simple iteration over a collection of KeyValuePairs.  The Value property of the KeyValuePair must implement IList.
    /// IndexEnumerator is an enumerator which ensures the value of the object returned by Current is a shallow clone.
    /// </summary>
    /// <typeparam name="Key">The type of the Key property of the KeyValuePair</typeparam>
    /// <typeparam name="Value">The type of the elements stored in the list, which is the Value of the KeyValuePair.</typeparam>
    internal class IndexEnumerator<Key, Value> : IEnumerator<KeyValuePair<Key, IList<Value>>>
    {
        /// <summary>
        /// The wrapped enumerator
        /// </summary>
        private IEnumerator<KeyValuePair<Key, IList<Value>>> underlyingEnumerator;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="IndexEnumerator{Key,Value}" /> class.
        /// </summary>
        /// <param name="enumeratorToWrap">The underlying enumerator to wrap</param>
        public IndexEnumerator(IEnumerator<KeyValuePair<Key, IList<Value>>> enumeratorToWrap)
        {
            this.underlyingEnumerator = enumeratorToWrap;
        }
        
        /// <summary>
        /// Gets the element in the collection at the current position of the enumerator
        /// </summary>
        /// <returns>
        /// The element in the collection at the current position of the enumerator
        /// </returns>
        public KeyValuePair<Key, IList<Value>> Current
        {
            get
            {
                KeyValuePair<Key, IList<Value>> underlyingCurrent = this.underlyingEnumerator.Current;
                
                IList<Value> shallowCopyOfValues = new List<Value>(underlyingCurrent.Value);
                
                return new KeyValuePair<Key, IList<Value>>(underlyingCurrent.Key, shallowCopyOfValues);
            }
        }
        
        /// <summary>
        /// Gets the element in the collection at the current position of the enumerator
        /// </summary>
        /// <returns>
        /// The element in the collection at the current position of the enumerator
        /// </returns>
        object System.Collections.IEnumerator.Current
        {
            get
            {
                return this.Current;
            }
        }
        
        /// <summary>
        /// Sets the enumerator to its initial position, which is before the first element in the collection
        /// </summary>
        public void Reset()
        {
            this.underlyingEnumerator.Reset();
        }
        
        /// <summary>
        /// Advances the enumerator to the next element of the collection.
        /// </summary>
        /// <returns>true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection</returns>
        public bool MoveNext()
        {
            return this.underlyingEnumerator.MoveNext();
        }
        
        /// <summary>
        /// Performs application-defined tasks associated with the freeing, releasing or resetting of unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.underlyingEnumerator.Dispose();
        }
    }
}
