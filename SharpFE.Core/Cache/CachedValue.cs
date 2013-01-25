//-----------------------------------------------------------------------
// <copyright file="CachedValue.cs" company="Iain Sproat">
//     Copyright Iain Sproat, 2013.
// </copyright>
//-----------------------------------------------------------------------

namespace SharpFE.Cache
{
    using System;
    
    /// <summary>
    /// A value which is to be cached requires some way of determining whether it remains valid.
    /// The struct allows the value to be stored alongside a hash.  The hash can be used to determine
    /// the ongoing validitiy of the stored value.
    /// </summary>
    /// <typeparam name="T">The type of value saved in this cache</typeparam>
    public struct CachedValue<T> : IEquatable<CachedValue<T>>
    {
        /// <summary>
        /// A hash which indicates the validity of this store
        /// </summary>
        private int validityHash;
        
        /// <summary>
        /// The value stored in the cache
        /// </summary>
        private T storedValue;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="CachedValue{T}">CachedValue</see> struct.
        /// </summary>
        /// <param name="hashRelatingToValidityOfResult">The hash indicating the validity of the value being stored</param>
        /// <param name="valueToStore">The value which is stored</param>
        public CachedValue(int hashRelatingToValidityOfResult, T valueToStore)
        {
            this.validityHash = hashRelatingToValidityOfResult;
            this.storedValue = valueToStore;
        }
        
        /// <summary>
        /// Gets the hash which can be used to determent as to whether the accompany value is valid.
        /// </summary>
        public int Hash
        {
            get
            {
                return this.validityHash;
            }
        }
        
        /// <summary>
        /// Gets the value to be stored
        /// </summary>
        public T Value
        {
            get
            {
                return this.storedValue;
            }
        }
        
        #region Equals and GetHashCode implementation
        /// <summary>
        /// Equality comparator
        /// </summary>
        /// <param name="leftHandSide">The object to the left of the equality operator</param>
        /// <param name="rightHandSide">The object to the right of the equality operator</param>
        /// <returns></returns>
        public static bool operator ==(CachedValue<T> leftHandSide, CachedValue<T> rightHandSide)
        {
            return leftHandSide.Equals(rightHandSide);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="leftHandSide"></param>
        /// <param name="rightHandSide"></param>
        /// <returns></returns>
        public static bool operator !=(CachedValue<T> leftHandSide, CachedValue<T> rightHandSide)
        {
            return !(leftHandSide == rightHandSide);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return (obj is CachedValue<T>) && this.Equals((CachedValue<T>)obj);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(CachedValue<T> other)
        {
            return this.validityHash == other.validityHash && object.Equals(this.storedValue, other.storedValue);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            int hashCode = 0;
            unchecked
            {
                hashCode += 1000000007 * this.validityHash.GetHashCode();
                if (this.storedValue != null)
                {
                    hashCode += 1000000009 * this.storedValue.GetHashCode();
                }
            }
            
            return hashCode;
        }
        #endregion
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hashToCompareTo"></param>
        /// <returns></returns>
        public bool IsValid(int hashToCompareTo)
        {
            return this.Hash.Equals(hashToCompareTo);
        }
    }
}
