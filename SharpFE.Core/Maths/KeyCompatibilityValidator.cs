//-----------------------------------------------------------------------
// <copyright file="KeyCompatibilityValidator.cs" company="Iain Sproat">
//     Copyright Iain Sproat, 2013.
// </copyright>
//-----------------------------------------------------------------------

namespace SharpFE.Maths
{
    using System;
    using System.Collections.Generic;
    
    /// <summary>
    /// Compares two lists for equality in both contained items
    /// and order of items
    /// </summary>
    /// <typeparam name="TLeftKey"></typeparam>
    /// <typeparam name="TRightKey"></typeparam>
    public class KeyCompatibilityValidator<TLeftKey, TRightKey>
    {
        /// <summary>
        /// 
        /// </summary>
        private IList<string> errorMessages;
        
        /// <summary>
        /// 
        /// </summary>
        private IList<TLeftKey> lhs;
        
        /// <summary>
        /// 
        /// </summary>
        private IList<TRightKey> rhs;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="leftHandSide"></param>
        /// <param name="rightHandSide"></param>
        public KeyCompatibilityValidator(IList<TLeftKey> leftHandSide, IList<TRightKey> rightHandSide)
        {
            this.lhs = leftHandSide;
            this.rhs = rightHandSide;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public void ThrowIfInvalid()
        {
            Guard.AgainstNullArgument(this.lhs, "lhs");
            Guard.AgainstNullArgument(this.rhs, "rhs");
            
            if (this.lhs.Count != this.rhs.Count)
            {
                throw new ArgumentException(string.Format(
                    System.Globalization.CultureInfo.InvariantCulture,
                    "There are a different number of keys in each list.  Argument 'lhs' has {0} items but 'rhs' has {1} items",
                    this.lhs.Count,
                    this.rhs.Count));
            }
            
            this.errorMessages = new List<string>();
            
            int max = rhs.Count;
            IList<int> rhsHashCodes = new List<int>(max);
            for( int i = 0; i < max; i++)
            {
                TRightKey rhsItem = rhs[i];
                if (rhsItem == null)
                {
                    throw new ArgumentException(string.Format("rhs contains a null value at index {0}", i));
                }
                
                rhsHashCodes.Add(rhsItem.GetHashCode());
            }
            
            foreach ( TLeftKey lhsItem in this.lhs)
            {
                if (lhsItem == null)
                {
                    throw new ArgumentException("lhs contains a null value");
                }
                
                if (!rhsHashCodes.Contains(lhsItem.GetHashCode()))
                {
                    throw new ArgumentException(string.Format("Item {0} in the left hand list is not present in the right hand list", lhsItem));
                }
            }
        }
    }
}
