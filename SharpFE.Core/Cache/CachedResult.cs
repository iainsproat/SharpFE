//-----------------------------------------------------------------------
// <copyright file="?.cs" company="Iain Sproat">
//     Copyright Iain Sproat, 2013.
// </copyright>
//-----------------------------------------------------------------------
using System;

namespace SharpFE.Cache
{
    /// <summary>
    /// Description of CachedResult.
    /// </summary>
    public struct CachedValue<T> : IEquatable<CachedValue<T>>
    {
        int validityHash;
        T storedValue;
        
        public CachedValue(int hashRelatingToValidityOfResult, T resultToStore)
        {
            this.validityHash = hashRelatingToValidityOfResult;
            this.storedValue = resultToStore;
        }
        
        public int Hash
        {
            get
            {
                return this.validityHash;
            }
        }
        
        public T Value
        {
            get
            {
                return this.storedValue;
            }
        }
        
        public bool IsValid(int hashToCompareTo)
        {
            return this.Hash.Equals(hashToCompareTo);
        }
        
        #region Equals and GetHashCode implementation
        public override bool Equals(object obj)
        {
            return (obj is CachedValue<T>) && Equals((CachedValue<T>)obj);
        }
        
        public bool Equals(CachedValue<T> other)
        {
            return this.validityHash == other.validityHash && object.Equals(this.storedValue, other.storedValue);
        }
        
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
        
        public static bool operator ==(CachedValue<T> leftHandSide, CachedValue<T> rightHandSide)
        {
            return leftHandSide.Equals(rightHandSide);
        }
        
        public static bool operator !=(CachedValue<T> leftHandSide, CachedValue<T> rightHandSide)
        {
            return !(leftHandSide == rightHandSide);
        }
        #endregion
    }
}
