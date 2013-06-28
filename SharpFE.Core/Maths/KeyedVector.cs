//-----------------------------------------------------------------------
// <copyright file="KeyedVector.cs" company="Iain Sproat">
//     Copyright Iain Sproat, 2013.
//
// Based in part on:
//
// Math.NET Numerics, part of the Math.NET Project
// http://numerics.mathdotnet.com
// http://github.com/mathnet/mathnet-numerics
// http://mathnetnumerics.codeplex.com
//
// Copyright (c) 2009-2010 Math.NET
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
//-----------------------------------------------------------------------

namespace SharpFE
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    
    using SharpFE.Maths;
    using MathNet.Numerics.LinearAlgebra.Generic;
    using MathNet.Numerics.LinearAlgebra.Double;
    
    /// <summary>
    /// A KeyedMatrix is a matrix whose elements can be accessed by Keys, rather than just index integers.
    /// This is roughly analagous to what a Dictionary is to a List.
    /// </summary>
    /// <typeparam name="TKey">The type of the instances which form the keys to this KeyedMatrix</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "Vector is more descriptive than Collection")]
    public class KeyedVector<TKey> : IEnumerable<KeyValuePair<TKey, double>>, IEquatable<KeyedVector<TKey>>
    {
        /// <summary>
        /// The keys which identify the items of this keyed vector
        /// </summary>
        private IDictionary<TKey, double> store = new Dictionary<TKey, double>();
        
        #region Constructors
        /// <summary>
        /// 
        /// </summary>
        /// <param name="keysForVector"></param>
        public KeyedVector(IList<TKey> keysForVector)
        {
            this.InitializeKeys(keysForVector);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="keysForVector"></param>
        /// <param name="initialValue"></param>
        public KeyedVector(IList<TKey> keysForVector, double initialValue)
        {
            this.InitializeKeys(keysForVector);
            this.InitializeAllData(initialValue);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="keysForVector"></param>
        public KeyedVector(IList<TKey> keysForVector, params double[] array)
        {
            this.InitializeKeysAndData(keysForVector, array);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="keysForVector"></param>
        public KeyedVector(TKey[] keysForVector, double[] array)
        {
            this.InitializeKeysAndData(keysForVector, array);
        }
        
        public KeyedVector(IList<TKey> keysForVector, Vector<double> vectorToClone)
        {
            this.InitializeKeysAndData(keysForVector, vectorToClone.ToArray());
        }
        
        protected KeyedVector(KeyedVector<TKey> vectorToClone)
            : this(vectorToClone.Keys)
        {
            foreach (KeyValuePair<TKey, double> kvp in vectorToClone.store)
            {
                this[kvp.Key] = kvp.Value;
            }
        }
        
        #endregion
        
        /// <summary>
        /// Clones the keys of this vector
        /// </summary>
        public IList<TKey> Keys
        {
            get
            {
                return new List<TKey>(this.store.Keys);
            }
        }
        
        public int Count
        {
            get
            {
                return this.store.Count;
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
                return store[index];
            }
            
            set
            {
                store[index] = value;
            }
        }
        
        public KeyedVector<TKey> Clone()
        {
            return new KeyedVector<TKey>(this);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public KeyedVector<TKey> Add(KeyedVector<TKey> other)
        {
            var kcv = new KeyCompatibilityValidator<TKey, TKey>(this.Keys, other.Keys);
            kcv.ThrowIfInvalid();
            
            IList<TKey> keyz = this.Keys;
            KeyedVector<TKey> result = Clone();
            foreach (TKey key in keyz)
            {
                result[key] += other[key];
            }
            return result;
        }
        
        public double DotProduct(KeyedVector<TKey> other)
        {
            KeyCompatibilityValidator<TKey, TKey> kcv = new KeyCompatibilityValidator<TKey, TKey>(this.Keys, other.Keys);
            kcv.ThrowIfInvalid();
            
            double sum = 0.0;
            
            sum = this.store.Sum(kvp => kvp.Value * other[kvp.Key]);
            
            return sum;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public KeyedVector<TKey> CrossProduct(KeyedVector<TKey> other)
        {
            Guard.AgainstNullArgument(other, "other");
            Guard.AgainstBadArgument(
                () => { return other.Count != 3; },
                "Cross product can only be carried out with a 3 dimensional vector",
                "other");
            Guard.AgainstInvalidState(
                () => { return this.Count != 3; },
                "Cross product can only be carried out with a 3 dimensional vector");
            
            KeyCompatibilityValidator<TKey, TKey> kcv = new KeyCompatibilityValidator<TKey, TKey>(this.Keys, other.Keys);
            kcv.ThrowIfInvalid();
            ////TODO If the keys match but are in the wrong order, swap the other vector items and keys to match exactly
            
            IList<TKey> keyz = this.Keys;
            TKey key0 = keyz[0];
            TKey key1 = keyz[1];
            TKey key2 = keyz[2];
            
            KeyedVector<TKey> result = new KeyedVector<TKey>(keyz);

            result[key0] = (this[key1] * other[key2]) - (this[key2] * other[key1]);
            result[key1] = (this[key2] * other[key0]) - (this[key0] * other[key2]);
            result[key2] = (this[key0] * other[key1]) - (this[key1] * other[key0]);

            return result;
        }
        
        public KeyedVector<TKey> Multiply(double scalar)
        {
            IList<TKey> keyz = this.Keys;
            KeyedVector<TKey> result = Clone();
            foreach (TKey key in keyz)
            {
                result[key] *= scalar;
            }
            return result;
        }
        
        public KeyedVector<TKey> Negate()
        {
            KeyedVector<TKey> result = new KeyedVector<TKey>(this.Keys);
            foreach (KeyValuePair<TKey, double> kvp in this.store)
            {
                result[kvp.Key] = -1.0 * kvp.Value;
            }
            return result;
        }
        
        public double Norm(double p)
        {
            if (p < 0.0)
            {
                throw new ArgumentOutOfRangeException("p");
            }

            if (Double.IsPositiveInfinity(p))
            {
                throw new NotImplementedException("Have not implemented Norm for when p is Positive infinity");
            }

            var sum = 0.0;

            foreach (KeyValuePair<TKey, double> kvp in this.store)
            {
                sum += Math.Pow(Math.Abs(kvp.Value), p);
            }

            return Math.Pow(sum, 1.0 / p);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "p", Justification = "Following Math.net library convention")]
        public KeyedVector<TKey> Normalize(double p)
        {
            if (p < 0.0)
            {
                throw new ArgumentOutOfRangeException("p");
            }
            
            double norm = Norm(p);
            KeyedVector<TKey> clone = Clone();
            if (norm == 0.0)
            {
                return clone;
            }

            clone = clone.Multiply(1.0 / norm);

            return clone;
        }
        
        public double SumMagnitudes()
        {
            var sum = 0.0;
            
            foreach ( KeyValuePair<TKey, double> kvp in this.store)
            {
                sum += Math.Abs(kvp.Value);
            }
            
            return sum;
        }
        
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        
        public IEnumerator<KeyValuePair<TKey, double>> GetEnumerator()
        {
            return this.store.GetEnumerator();
        }
        
        #region Equals and GetHashCode implementation
        public override bool Equals(object obj)
        {
            KeyedVector<TKey> other = obj as KeyedVector<TKey>;
            return this.Equals(other);
        }
        
        public bool Equals(KeyedVector<TKey> other)
        {
            if (other == null)
            {
                return false;
            }
            
            foreach (KeyValuePair<TKey, double> kvp in this)
            {
                if (other[kvp.Key] != kvp.Value)
                {
                    return false;
                }
            }
            
            return true;
        }
        
        public override int GetHashCode()
        {
            int hashCode = 0;
            unchecked {
                if (store != null)
                    hashCode += 1000000007 * store.GetHashCode();
            }
            return hashCode;
        }
        
        public static bool operator ==(KeyedVector<TKey> lhs, KeyedVector<TKey> rhs)
        {
            if (ReferenceEquals(lhs, rhs))
                return true;
            if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null))
                return false;
            return lhs.Equals(rhs);
        }
        
        public static bool operator !=(KeyedVector<TKey> lhs, KeyedVector<TKey> rhs)
        {
            return !(lhs == rhs);
        }
        #endregion

        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("[");
            int max = this.Keys.Count;
            int i = 0;
            foreach (KeyValuePair<TKey, double> kvp in this.store)
            {
                sb.Append("<");
                sb.Append(kvp.Key.ToString());
                sb.Append(", ");
                sb.Append(kvp.Value);
                sb.Append(">");
                if ((i + 1) != max)
                {
                    sb.AppendLine();
                }
                i++;
            }
            
            sb.Append("]");
            return sb.ToString();
        }
        
        private void InitializeKeys(IList<TKey> keysForVector)
        {
            this.InitializeKeysAndData(keysForVector, new double[keysForVector.Count]);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="keysForVector"></param>
        private void InitializeKeys(TKey[] keysForVector)
        {
            double[] zeroDataArray =  new double[keysForVector.Length];
            this.InitializeKeysAndData(new List<TKey>(keysForVector), zeroDataArray);
        }
        
        /// <summary>
        /// Replaces the keys with the provided lists.
        /// First checks that the lists are reasonably valid (does not check for duplicate keys, however)
        /// </summary>
        /// <param name="keysForVector">The keys to replace the Keys property with</param>
        private void InitializeKeysAndData(IList<TKey> keysForVector, double[] values)
        {
            Guard.AgainstNullArgument(keysForVector, "keysForVector");
            Guard.AgainstNullArgument(values, "values");
            
            Guard.AgainstBadArgument(
                () => { return values.Length != keysForVector.Count; },
                "The number of items in the keys list should match the number of value items",
                "keysForVector");
            
            this.store.Clear();
            
            int numKeys = keysForVector.Count;
            for (int i = 0; i < numKeys; i++)
            {
                this.store.Add(keysForVector[i], values[i]);
            }
        }
        
        /// <summary>
        /// Clears the store and sets all the data to the given value
        /// </summary>
        /// <param name="initialValue">The value to set all data to.</param>
        private void InitializeAllData(double initialValue)
        {
            IList<TKey> keysToInitialize = this.Keys;
            this.InitializeData(keysToInitialize, initialValue);
            
        }
        
        private void InitializeData(IList<TKey> keysToInitialize, double initialValue)
        {
            Guard.AgainstNullArgument(keysToInitialize, "keysToInitialize");
            
            if (keysToInitialize.IsEmpty())
            {
                return;
            }
            
            int max = this.Count;
            foreach (TKey key in keysToInitialize)
            {
                this.AddOrSetData(key, initialValue);
            }
        }
        
        private void AddOrSetData(TKey key, double value)
        {
            if (this.store.ContainsKey(key))
            {
                this.store[key] = value;
            }
            else
            {
                this.store.Add(key, value);
            }
        }
        
        public Vector<double> ToVector()
        {
            ICollection<double> dataToCopy = this.store.Values;
            double[] dataAsArray = new double[dataToCopy.Count];
            dataToCopy.CopyTo(dataAsArray, 0);
            return new DenseVector(dataAsArray);
        }
    }
}
