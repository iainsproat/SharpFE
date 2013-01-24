//-----------------------------------------------------------------------
// <copyright file="?.cs" company="Iain Sproat">
//     Copyright Iain Sproat, 2013.
// </copyright>
//-----------------------------------------------------------------------

namespace SharpFE.MathNetExtensions
{
    using System;
    using System.Collections.Generic;
    
    /// <summary>
    /// Compares two lists for equality in both contained items
    /// and order of items
    /// </summary>
    public class KeyCompatibilityValidator<TLeftKey, TRightKey>
    {
        private IList<string> errorMessages;
        private IList<TLeftKey> lhs;
        private IList<TRightKey> rhs;
        
        public KeyCompatibilityValidator(IList<TLeftKey> leftHandSide, IList<TRightKey> rightHandSide)
        {
            this.lhs = leftHandSide;
            this.rhs = rightHandSide;
        }
        
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
            
            int numItems = this.lhs.Count;
            for (int i = 0; i < numItems; i++)
            {
                this.CompareItemsAtIndex(i);
            }
            
            if (this.errorMessages.Count > 0)
            {
                this.PrintAndThrowErrors();
            }
        }
        
        private void CompareItemsAtIndex(int index)
        {
            TLeftKey lhsItem = this.lhs[index];
            TRightKey rhsItem = this.rhs[index];
            
            if (lhsItem == null)
            {
                this.errorMessages.Add(string.Format(
                    System.Globalization.CultureInfo.InvariantCulture,
                    "Item in lhs at index {0} is null.  The item at the corresponding index in rhs is {1}",
                    index,
                    rhsItem));
                return;
            }
            
            if (rhsItem == null)
            {
                this.errorMessages.Add(string.Format(
                    System.Globalization.CultureInfo.InvariantCulture,
                    "Item in rhs at index {0} is null. The item at the corresponding index in lhs is {1}",
                    index,
                    lhsItem));
                return;
            }
            
            if (!lhsItem.Equals(rhsItem))
            {
                this.errorMessages.Add(string.Format(
                    System.Globalization.CultureInfo.InvariantCulture,
                    "Item in lhs at index {0} does not equal the item in the rhs at the corresponding index.",
                    index));
                return;
            }
        }
        
        private void PrintAndThrowErrors()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine("Error found in keys, refer to the following error messages:");
            foreach (string message in this.errorMessages)
            {
                sb.Append("    ");
                sb.AppendLine(message);
            }
            
            throw new ArgumentException(sb.ToString());
        }
    }
}
