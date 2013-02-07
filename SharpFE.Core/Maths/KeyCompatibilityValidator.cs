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
    /// Compares two lists to ensure they contain the same items but in any order.
    /// </summary>
    /// <typeparam name="TLeftKey"></typeparam>
    /// <typeparam name="TRightKey"></typeparam>
    public class KeyCompatibilityValidator<TLeftKey, TRightKey>
    {
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
        /// The two lists of keys are expected to contain exactly the same items, but in any order.
        /// The lists are therefore to be the same length
        /// </summary>
        /// <remarks>
        /// Checks keys by comparing their hashcodes only
        /// </remarks>
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
            
            IList<int> rhsHashCodes = GenerateListOfHashCodes(this.rhs);
            
            foreach ( TLeftKey lhsItem in this.lhs)
            {
                if (lhsItem == null)
                {
                    throw new ArgumentException("lhs contains a null value. Null values are not acceptable as keys.");
                }
                
                if (!rhsHashCodes.Contains(lhsItem.GetHashCode()))
                {
                    throw new ArgumentException(string.Format("Item {0} in the left hand list is not present in the right hand list", lhsItem));
                }
            }
        }
        
        private IList<int> GenerateListOfHashCodes(IList<TRightKey> itemsFromWhichToGetHashCodes)
        {
            int max = itemsFromWhichToGetHashCodes.Count;
            IList<int> rhsHashCodes = new List<int>(max);
            for( int i = 0; i < max; i++)
            {
                TRightKey item = itemsFromWhichToGetHashCodes[i];
                if (item == null)
                {
                    throw new ArgumentException(string.Format("A null value was found at index {0}.  Null values are not acceptable as keys.", i), "itemsFromWhichToGetHashCodes");
                }
                
                rhsHashCodes.Add(item.GetHashCode());
            }
            return rhsHashCodes;
        }
    }
}
