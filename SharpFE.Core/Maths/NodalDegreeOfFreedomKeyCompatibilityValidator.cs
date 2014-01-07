//-----------------------------------------------------------------------
// <copyright file="NodalDegreeOfFreedomKeyCompatibilityValidator.cs" company="Iain Sproat">
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
	public class NodalDegreeOfFreedomKeyCompatibilityValidator
	{
		private IList<IFiniteElementNode> lhsNodes;
		private IList<DegreeOfFreedom> lhsDegreeOfFreedoms;
		private IList<IFiniteElementNode> rhsNodes;
		private IList<DegreeOfFreedom> rhsDegreeOfFreedoms;

		public NodalDegreeOfFreedomKeyCompatibilityValidator(IList<IFiniteElementNode> leftHandSideNodes, IList<DegreeOfFreedom> leftHandSideDegreeOfFreedoms, IList<IFiniteElementNode> rightHandSideNodes, IList<DegreeOfFreedom> rightHandSideDegreeOfFreedoms)
		{
			this.lhsNodes = leftHandSideNodes;
			this.lhsDegreeOfFreedoms = leftHandSideDegreeOfFreedoms;
			this.rhsNodes = rightHandSideNodes;
			this.rhsDegreeOfFreedoms = rightHandSideDegreeOfFreedoms;
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

			Guard.AgainstNullOrEmptyListArgument(this.lhsNodes, "lhsNodes");
			Guard.AgainstNullOrEmptyListArgument(this.lhsDegreeOfFreedoms, "lhsDegreeOfFreedoms");
			Guard.AgainstNullOrEmptyListArgument(this.rhsNodes, "rhsNodes");
			Guard.AgainstNullOrEmptyListArgument(this.rhsDegreeOfFreedoms, "rhsDegreeOfFreedoms");

			int numDegreeOfFreedoms = this.lhsDegreeOfFreedoms.Count;
			if (this.rhsDegreeOfFreedoms.Count != numDegreeOfFreedoms)
			{
				throw new ArgumentException(string.Format(
					System.Globalization.CultureInfo.InvariantCulture,
					"There are a different number of supported degree of freedoms in each list.  Argument 'leftHandSideDegreeOfFreedoms' has {0} items but 'rightHandSideDegreeOfFreedoms' has {1} items",
					this.lhsDegreeOfFreedoms.Count,
					this.rhsDegreeOfFreedoms.Count));
			}

			int numNodes = this.lhsNodes.Count;
			if (this.rhsNodes.Count != numNodes)
			{
				throw new ArgumentException(string.Format(
					System.Globalization.CultureInfo.InvariantCulture,
					"There are a different number of nodes in each list.  Argument 'leftHandSideNodes' has {0} items but 'rightHandSideNodes' has {1} items",
					this.lhsNodes.Count,
					this.rhsNodes.Count));
			}

			for(int i = 0; i < numDegreeOfFreedoms; i++)
			{
				if(this.lhsDegreeOfFreedoms[i] != this.rhsDegreeOfFreedoms[i])
				{
					throw new ArgumentException(string.Format(
						System.Globalization.CultureInfo.InvariantCulture,
						"The lists of degrees of freedoms are not similar.  The item at index {0} differs.",
						i));
				}
			}

			for(int j = 0; j < numNodes; j++)
			{
				if(this.lhsNodes[j] != this.rhsNodes[j])
				{
					throw new ArgumentException(string.Format(
						System.Globalization.CultureInfo.InvariantCulture,
						"The lists of nodes are not similar.  The item at index {0} differs.",
						j));
				}
			}
		}

		public bool IsValid()
		{
			try
			{
				this.ThrowIfInvalid();
			}
			catch
			{
				return false;
			}

			return true;
		}

	}
}
