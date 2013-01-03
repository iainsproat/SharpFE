namespace SharpFE
{
	using System;
	using System.Collections.Generic;
	
	using MathNet.Numerics.LinearAlgebra.Double;
	using MathNet.Numerics.LinearAlgebra.Generic;
	
	/// <summary>
	/// A KeyedMatrix is a matrix whose elements can be accessed by Keys, rather than just index integers.
    /// This is roughly analagous to what a Dictionary is to a List.
	/// </summary>
    /// <typeparam name="TKey">The type of the instances which form the keys to this KeyedMatrix</typeparam>
	public class KeyedVector<TKey> : DenseVector
	{
		/// <summary>
        /// The keys which identify the items of this keyed vector
        /// </summary>
        private IList<TKey> _keys;
        	
		public KeyedVector(IList<TKey> keysForVector)
        	: base(keysForVector.Count)
		{
        	this.CheckAndAddKeys(keysForVector);
		}
        
        public KeyedVector(Vector vector, IList<TKey> keysForVector)
        	:base(vector)
        {
        	this.CheckAndAddKeys(keysForVector);
        }
        
        public double this[TKey index]
        {
        	get
        	{
        		return this[this.KeyIndex(index)];
        	}
        	set
        	{
        		this[this.KeyIndex(index)] = value;
        	}
        }
        
        public IList<TKey> Keys
        {
            get
            {
            	return ((List<TKey>)this._keys).AsReadOnly();
            }
            
            private set
            {
                this._keys = new List<TKey>(value);
            }
        }
        
        /// <summary>
        /// Determines the row index in the matrix
        /// </summary>
        /// <param name="rowKey">The key for which to find the index</param>
        /// <returns>An integer representing the row index in the matrix</returns>
        private int KeyIndex(TKey key)
        {
            return this._keys.IndexOf(key);
        }
        
        private TKey KeyFromIndex(int index)
        {
        	return this._keys[index];
        }
        
        /// <summary>
        /// Replaces the keys with the provided lists.
        /// First checks that the lists are reasonably valid (does not check for duplicate keys, however)
        /// </summary>
        /// <param name="keysForRows">The keys to replace the RowKeys property with</param>
        /// <param name="keysForColumns">The keys to replace the ColumnKeys property with</param>
        private void CheckAndAddKeys(IList<TKey> keysForVector)
        {
            Guard.AgainstNullArgument(keysForVector, "keysForVector");
            Guard.AgainstBadArgument(
                () => { return this.Count != keysForVector.Count; },
                "The number of items in the keys list should match the number of items of the underlying vector",
                "keysForVector");
            
            // TODO check for duplicate keys
           this.Keys = keysForVector;
        }
	}
}
