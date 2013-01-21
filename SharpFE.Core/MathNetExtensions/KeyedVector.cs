namespace SharpFE
{
    using System;
    using System.Collections.Generic;
    
    using MathNet.Numerics.LinearAlgebra.Double;
    using MathNet.Numerics.LinearAlgebra.Generic;
    
    using SharpFE.MathNetExtensions;
    
    /// <summary>
    /// A KeyedMatrix is a matrix whose elements can be accessed by Keys, rather than just index integers.
    /// This is roughly analagous to what a Dictionary is to a List.
    /// </summary>
    /// <typeparam name="TKey">The type of the instances which form the keys to this KeyedMatrix</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
    public class KeyedVector<TKey> : DenseVector
    {
        /// <summary>
        /// The keys which identify the items of this keyed vector
        /// </summary>
        private IDictionary<TKey, int> _keys = new Dictionary<TKey, int>();
            
        public KeyedVector(IList<TKey> keysForVector)
            : base(keysForVector.Count)
        {
            this.CheckAndAddKeys(keysForVector);
        }
        
        public KeyedVector(IList<TKey> keysForVector, double initialValue)
            : base(keysForVector.Count, initialValue)
        {
            this.CheckAndAddKeys(keysForVector);
        }
        
        public KeyedVector(Vector vector, IList<TKey> keysForVector)
            : base(vector)
        {
            this.CheckAndAddKeys(keysForVector);
        }
        
        public KeyedVector(double[] array, IList<TKey> keysForVector)
            : base(array)
        {
            this.CheckAndAddKeys(keysForVector);
        }
        
        public KeyedVector(Vector<double> vector, IList<TKey> keysForVector)
            : base(vector)
        {
            this.CheckAndAddKeys(keysForVector);
        }
        
        public KeyedVector(double[] array, params TKey[] keysForVector)
            : base(array)
        {
            this.CheckAndAddKeys(keysForVector);
        }
        
        public KeyedVector(Vector<double> vector, params TKey[] keysForVector)
            : base(vector)
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
                return new List<TKey>(this._keys.Keys);
            }
        }
        
        public KeyedVector<TKey> Add(KeyedVector<TKey> other)
        {
            KeyCompatibilityValidator<TKey, TKey> kcv = new KeyCompatibilityValidator<TKey, TKey>(this.Keys, other.Keys);
            kcv.ThrowIfInvalid();
            
            ////TODO If the keys match but are in the wrong order, swap the other vector items and keys to match exactly
            
            Vector<double> result = ((Vector<double>)this).Add((Vector<double>)other);
            return new KeyedVector<TKey>(result, this.Keys);
        }
        
        public KeyedVector<TKey> CrossProduct(KeyedVector<TKey> other)
        {
            KeyCompatibilityValidator<TKey, TKey> kcv = new KeyCompatibilityValidator<TKey, TKey>(this.Keys, other.Keys);
            kcv.ThrowIfInvalid();
            
            ////TODO If the keys match but are in the wrong order, swap the other vector items and keys to match exactly
             
            Vector result = ((Vector)this).CrossProduct((Vector)other);
            return new KeyedVector<TKey>(result, this.Keys);
        }
        
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "p")]
        public new KeyedVector<TKey> Normalize(double p)
        {
            Vector<double> result = base.Normalize(p);
            return new KeyedVector<TKey>(result, this.Keys);
        }
        
        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("[");
            int max = this.Keys.Count;
            for (int i = 0; i < max; i++)
            {
                sb.Append("<");
                sb.Append(this.Keys[i].ToString());
                sb.Append(", ");
                sb.Append(this[i]);
                sb.Append(">");
                if ((i + 1) != max)
                {
                    sb.AppendLine();
                }
            }
            
            sb.Append("]");
            return sb.ToString();
        }

        
        /// <summary>
        /// Determines the row index in the matrix
        /// </summary>
        /// <param name="rowKey">The key for which to find the index</param>
        /// <returns>An integer representing the row index in the matrix</returns>
        private int KeyIndex(TKey key)
        {
            return this._keys[key];
        }
        
        private void CheckAndAddKeys(TKey[] keysForVector)
        {
            this.CheckAndAddKeys(new List<TKey>(keysForVector));
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
            
            this._keys.Clear();
            
            int numKeys = keysForVector.Count;
            for (int i = 0; i < numKeys; i++)
            {
                this._keys.Add(keysForVector[i], i);
            }
        }
    }
}
