//-----------------------------------------------------------------------
// <copyright file="KeyedVector.cs" company="Iain Sproat">
//     Copyright Iain Sproat, 2013.
// </copyright>
//-----------------------------------------------------------------------

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
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "Vector is more descriptive than Collection")]
    public class KeyedVector<TKey> : DenseVector
    {
        /// <summary>
        /// The keys which identify the items of this keyed vector
        /// </summary>
        private IDictionary<TKey, int> keyz = new Dictionary<TKey, int>();
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="keysForVector"></param>
        public KeyedVector(IList<TKey> keysForVector)
            : base(keysForVector.Count)
        {
            this.CheckAndAddKeys(keysForVector);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="keysForVector"></param>
        /// <param name="initialValue"></param>
        public KeyedVector(IList<TKey> keysForVector, double initialValue)
            : base(keysForVector.Count, initialValue)
        {
            this.CheckAndAddKeys(keysForVector);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="keysForVector"></param>
        public KeyedVector(Vector vector, IList<TKey> keysForVector)
            : base(vector)
        {
            this.CheckAndAddKeys(keysForVector);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="keysForVector"></param>
        public KeyedVector(double[] array, IList<TKey> keysForVector)
            : base(array)
        {
            this.CheckAndAddKeys(keysForVector);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="keysForVector"></param>
        public KeyedVector(Vector<double> vector, IList<TKey> keysForVector)
            : base(vector)
        {
            this.CheckAndAddKeys(keysForVector);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="keysForVector"></param>
        public KeyedVector(double[] array, params TKey[] keysForVector)
            : base(array)
        {
            this.CheckAndAddKeys(keysForVector);
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="KeyedVector">KeyedVector</see> class
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="keysForVector"></param>
        public KeyedVector(Vector<double> vector, params TKey[] keysForVector)
            : base(vector)
        {
            this.CheckAndAddKeys(keysForVector);
        }
        
        /// <summary>
        /// Gets the keys of this vector
        /// </summary>
        public IList<TKey> Keys
        {
            get
            {
                return new List<TKey>(this.keyz.Keys);
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public KeyedVector<TKey> Add(KeyedVector<TKey> other)
        {
            KeyCompatibilityValidator<TKey, TKey> kcv = new KeyCompatibilityValidator<TKey, TKey>(this.Keys, other.Keys);
            kcv.ThrowIfInvalid();
            
            ////TODO If the keys match but are in the wrong order, swap the other vector items and keys to match exactly
            
            Vector<double> result = ((Vector<double>)this).Add((Vector<double>)other);
            return new KeyedVector<TKey>(result, this.Keys);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public KeyedVector<TKey> CrossProduct(KeyedVector<TKey> other)
        {
            KeyCompatibilityValidator<TKey, TKey> kcv = new KeyCompatibilityValidator<TKey, TKey>(this.Keys, other.Keys);
            kcv.ThrowIfInvalid();
            
            ////TODO If the keys match but are in the wrong order, swap the other vector items and keys to match exactly
            
            Vector result = ((Vector)this).CrossProduct((Vector)other);
            return new KeyedVector<TKey>(result, this.Keys);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "p", Justification = "Following Math.net library convention")]
        public new KeyedVector<TKey> Normalize(double p)
        {
            Vector<double> result = base.Normalize(p);
            return new KeyedVector<TKey>(result, this.Keys);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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
        /// <param name="key">The key for which to find the index</param>
        /// <returns>An integer representing the row index in the matrix</returns>
        private int KeyIndex(TKey key)
        {
            return this.keyz[key];
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="keysForVector"></param>
        private void CheckAndAddKeys(TKey[] keysForVector)
        {
            this.CheckAndAddKeys(new List<TKey>(keysForVector));
        }
        
        /// <summary>
        /// Replaces the keys with the provided lists.
        /// First checks that the lists are reasonably valid (does not check for duplicate keys, however)
        /// </summary>
        /// <param name="keysForVector">The keys to replace the Keys property with</param>
        private void CheckAndAddKeys(IList<TKey> keysForVector)
        {
            Guard.AgainstNullArgument(keysForVector, "keysForVector");
            Guard.AgainstBadArgument(
                () => { return this.Count != keysForVector.Count; },
                "The number of items in the keys list should match the number of items of the underlying vector",
                "keysForVector");
            
            this.keyz.Clear();
            
            int numKeys = keysForVector.Count;
            for (int i = 0; i < numKeys; i++)
            {
                this.keyz.Add(keysForVector[i], i);
            }
        }
    }
}
