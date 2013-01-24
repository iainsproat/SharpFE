//-----------------------------------------------------------------------
// <copyright file="?.cs" company="Iain Sproat">
//     Copyright Iain Sproat, 2013.
// </copyright>
//-----------------------------------------------------------------------

namespace SharpFE.Cache
{
    using System;
    
    /// <summary>
    /// Description of CachedResult.
    /// </summary>
    public struct CachedValue<T> : IEquatable<CachedValue<T>>
    {
        private int validityHash;
        private T storedValue;
        
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
        
        #region Equals and GetHashCode implementation
        public static bool operator ==(CachedValue<T> leftHandSide, CachedValue<T> rightHandSide)
        {
            return leftHandSide.Equals(rightHandSide);
        }
        
        public static bool operator !=(CachedValue<T> leftHandSide, CachedValue<T> rightHandSide)
        {
            return !(leftHandSide == rightHandSide);
        }
        
        public override bool Equals(object obj)
        {
            return (obj is CachedValue<T>) && this.Equals((CachedValue<T>)obj);
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
        #endregion
        
        public bool IsValid(int hashToCompareTo)
        {
            return this.Hash.Equals(hashToCompareTo);
        }
    }
}
