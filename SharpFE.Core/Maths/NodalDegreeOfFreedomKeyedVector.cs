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
	public class NodalDegreeOfFreedomKeyedVector : IEnumerable<double>, IEquatable<NodalDegreeOfFreedomKeyedVector>
	{
		private IList<IFiniteElementNode> supportedNodes = new List<IFiniteElementNode>();
		private IList<DegreeOfFreedom> supportedDof = new List<DegreeOfFreedom>();

		private Vector<double> internalVector;

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		/// <param name="supportedNodesForVector"></param>
		public NodalDegreeOfFreedomKeyedVector(IList<IFiniteElementNode> supportedNodesForVector, IList<DegreeOfFreedom> supportedDegreesOfFreedom)
		{
			this.InitializeKeys(supportedNodesForVector, supportedDegreesOfFreedom);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="keysFosupportedNodesForVectorrVector"></param>
		/// <param name="initialValue"></param>
		public NodalDegreeOfFreedomKeyedVector(IList<IFiniteElementNode> supportedNodesForVector, IList<DegreeOfFreedom> supportedDegreesOfFreedom, double initialValue)
		{
			this.InitializeKeys(supportedNodesForVector, supportedDegreesOfFreedom);
			this.InitializeAllData(initialValue);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="array"></param>
		/// <param name="supportedNodesForVector"></param>
		public NodalDegreeOfFreedomKeyedVector(IList<IFiniteElementNode> supportedNodesForVector, IList<DegreeOfFreedom> supportedDegreesOfFreedom, params double[] array)
		{
			this.InitializeKeysAndData(supportedNodesForVector, supportedDegreesOfFreedom, array);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="array"></param>
		/// <param name="supportedNodesForVector"></param>
		public NodalDegreeOfFreedomKeyedVector(IFiniteElementNode[] supportedNodesForVector, IList<DegreeOfFreedom> supportedDegreesOfFreedom, double[] array)
		{
			this.InitializeKeysAndData(supportedNodesForVector, supportedDegreesOfFreedom, array);
		}

		public NodalDegreeOfFreedomKeyedVector(IList<IFiniteElementNode> supportedNodesForVector, IList<DegreeOfFreedom> supportedDegreesOfFreedom, Vector<double> vectorToClone)
		{
			this.InitializeKeysAndData(supportedNodesForVector, supportedDegreesOfFreedom, vectorToClone.ToArray());
		}

		protected NodalDegreeOfFreedomKeyedVector(NodalDegreeOfFreedomKeyedVector vectorToClone)
			: this(vectorToClone.supportedNodes, vectorToClone.supportedDof)
		{
			this.internalVector = vectorToClone.internalVector.Clone();
		}

		#endregion

		/// <summary>
		/// Clones the keys of this vector
		/// </summary>
		public IList<IFiniteElementNode> Nodes
		{
			get
			{
				return new List<IFiniteElementNode>(this.supportedNodes);
			}
		}

		public IList<DegreeOfFreedom> SupportedDegreesOfFreedom
		{
			get
			{
				return new List<DegreeOfFreedom>(this.supportedDof);
			}
		}

		public int Count
		{
			get
			{
				return this.supportedNodes.Count * this.supportedDof.Count;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public double this[int index]
		{
			get
			{
				return this.internalVector[index];
			}

			set
			{
				this.internalVector[index] = value;
			}
		}

		public double this[IFiniteElementNode node, DegreeOfFreedom dof]
		{
			get
			{
				int position = this.CalculateDataPosition(node, dof);
				return this.internalVector[position];
			}
			set
			{
				int position = this.CalculateDataPosition(node, dof);
				this.internalVector[position] = value;
			}
		}

		public double this[NodalDegreeOfFreedom nodalDegreeOfFreedom]
		{
			get
			{
				return this[nodalDegreeOfFreedom.Node, nodalDegreeOfFreedom.DegreeOfFreedom];
			}
			set
			{
				this[nodalDegreeOfFreedom.Node, nodalDegreeOfFreedom.DegreeOfFreedom] = value;
			}
		}

		private int CalculateDataPosition(IFiniteElementNode node, DegreeOfFreedom degreeOfFreedom)
		{
			int position = this.supportedNodes.IndexOf(node) * this.supportedDof.Count;
			position += this.supportedDof.IndexOf(degreeOfFreedom);
			return position;
		}

		private int CalculateDataPosition(NodalDegreeOfFreedom nodalDegreeOfFreedom)
		{
			return this.CalculateDataPosition(nodalDegreeOfFreedom.Node, nodalDegreeOfFreedom.DegreeOfFreedom);
		}

		public NodalDegreeOfFreedomKeyedVector Clone()
		{
			return new NodalDegreeOfFreedomKeyedVector(this);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		public NodalDegreeOfFreedomKeyedVector Add(NodalDegreeOfFreedomKeyedVector other)
		{
			NodalDegreeOfFreedomKeyCompatibilityValidator kcv = new NodalDegreeOfFreedomKeyCompatibilityValidator(this.supportedNodes, this.supportedDof, other.supportedNodes, other.supportedDof);
			kcv.ThrowIfInvalid();

			NodalDegreeOfFreedomKeyedVector result = this.Clone();
			result.internalVector = result.internalVector.Add(other.internalVector);

			return result;
		}

		public double DotProduct(NodalDegreeOfFreedomKeyedVector other)
		{
			NodalDegreeOfFreedomKeyCompatibilityValidator kcv = new NodalDegreeOfFreedomKeyCompatibilityValidator(this.supportedNodes, this.supportedDof, other.supportedNodes, other.supportedDof);
			kcv.ThrowIfInvalid();

			return this.internalVector.DotProduct(other.internalVector);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		public NodalDegreeOfFreedomKeyedVector CrossProduct(NodalDegreeOfFreedomKeyedVector other)
		{
			Guard.AgainstNullArgument(other, "other");
			Guard.AgainstBadArgument(
				"other",
				() => { return other.Count != 3; },
				"Cross product can only be carried out with a 3 dimensional vector");
			Guard.AgainstInvalidState(
				() => { return this.Count != 3; },
				"Cross product can only be carried out with a 3 dimensional vector. Number of dimensions of this vectors are {0}",
				this.Count);

			NodalDegreeOfFreedomKeyCompatibilityValidator kcv = new NodalDegreeOfFreedomKeyCompatibilityValidator(this.supportedNodes, this.supportedDof, other.supportedNodes, other.supportedDof);
			kcv.ThrowIfInvalid();

			NodalDegreeOfFreedomKeyedVector result = this.Clone();

			result.internalVector[0] = (result.internalVector[1] * other.internalVector[2]) - (result.internalVector[2] * other.internalVector[1]);
			result.internalVector[1] = (result.internalVector[2] * other.internalVector[0]) - (result.internalVector[0] * other.internalVector[2]);
			result.internalVector[2] = (result.internalVector[0] * other.internalVector[1]) - (result.internalVector[1] * other.internalVector[0]);

			return result;
		}

		public NodalDegreeOfFreedomKeyedVector Multiply(double scalar)
		{
			NodalDegreeOfFreedomKeyedVector result = this.Clone();
			result.internalVector = result.internalVector.Multiply(scalar);
			return result;
		}

		public NodalDegreeOfFreedomKeyedVector Negate()
		{
			NodalDegreeOfFreedomKeyedVector result = this.Clone();
			result.internalVector = result.internalVector.Negate();
			return result;
		}

		public double Norm(double p)
		{
			return this.internalVector.Norm(p);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="p"></param>
		/// <returns></returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "p", Justification = "Following Math.net library convention")]
		public NodalDegreeOfFreedomKeyedVector Normalize(double p)
		{
			NodalDegreeOfFreedomKeyedVector result = this.Clone();
			result.internalVector = result.internalVector.Normalize(p);
			return result;
		}

		public double SumMagnitudes()
		{
			return this.internalVector.SumMagnitudes();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		public IEnumerator<double> GetEnumerator()
		{
			return this.internalVector.GetEnumerator();
		}

		#region Equals and GetHashCode implementation
		public override bool Equals(object obj)
		{
			NodalDegreeOfFreedomKeyedVector other = obj as NodalDegreeOfFreedomKeyedVector;
			return this.Equals(other);
		}

		public bool Equals(NodalDegreeOfFreedomKeyedVector other)
		{
			if (other == null)
			{
				return false;
			}

			NodalDegreeOfFreedomKeyCompatibilityValidator kcv = new NodalDegreeOfFreedomKeyCompatibilityValidator(this.supportedNodes, this.supportedDof, other.supportedNodes, other.supportedDof);
			if(!kcv.IsValid()) {
				return false;
			}

			int numValues = this.Count;
			for (int i = 0; i < numValues; i++)
			{
				if (!other[i].IsApproximatelyEqualTo(this[i]))
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
				if (this.supportedNodes != null) {
					hashCode += 1000000007 * this.supportedNodes.GetHashCode();
				}

				if (this.supportedDof != null) {
					hashCode += 1000100008 * this.supportedDof.GetHashCode();
				}

				if(this.internalVector != null) {
					hashCode += 1000000087 * this.internalVector.GetHashCode();
				}
			}

			return hashCode;
		}

		public static bool operator ==(NodalDegreeOfFreedomKeyedVector lhs, NodalDegreeOfFreedomKeyedVector rhs)
		{
			if (ReferenceEquals(lhs, rhs))
				return true;
			if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null))
				return false;
			return lhs.Equals(rhs);
		}

		public static bool operator !=(NodalDegreeOfFreedomKeyedVector lhs, NodalDegreeOfFreedomKeyedVector rhs)
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
			sb.AppendLine("[");
			int maxNodes = this.supportedNodes.Count;
			int maxDofs = this.supportedDof.Count;
			foreach(var node in this.supportedNodes)
			{
				foreach(var dof in this.supportedDof)
				{
					sb.Append("<");
					sb.Append(node.ToString());
					sb.Append(", ");
					sb.Append(dof.ToString());
					sb.Append(">");
					sb.Append(",");
				}

				sb.AppendLine();
			}

			sb.Length -= 2; //removes last end-of-line and comma
			sb.AppendLine(); //re-append a new end-of-line
			sb.AppendLine("]");
			return sb.ToString();
		}

		private void InitializeKeys(IList<IFiniteElementNode> supportedNodesForVector, IList<DegreeOfFreedom> supportedDegreesOfFreedomsForVector)
		{
			this.InitializeKeysAndData(supportedNodesForVector, supportedDegreesOfFreedomsForVector, new double[supportedNodesForVector.Count]);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="supportedNodesForVector"></param>
		private void InitializeKeys(IFiniteElementNode[] supportedNodesForVector, DegreeOfFreedom[] supportedDegreesOfFreedomsForVector)
		{
			double[] zeroDataArray =  new double[supportedNodesForVector.Length];
			this.InitializeKeysAndData(new List<IFiniteElementNode>(supportedNodesForVector), new List<DegreeOfFreedom>(supportedDegreesOfFreedomsForVector), zeroDataArray);
		}

		/// <summary>
		/// Replaces the keys with the provided lists.
		/// First checks that the lists are reasonably valid (does not check for duplicate keys, however)
		/// </summary>
		/// <param name="supportedNodesForVector">The keys to replace the Keys property with</param>
		private void InitializeKeysAndData(IList<IFiniteElementNode> supportedNodesForVector, IList<DegreeOfFreedom> supportedDegreesOfFreedomsForVector, double[] values)
		{
			Guard.AgainstNullArgument(supportedNodesForVector, "supportedNodesForVector");
			Guard.AgainstNullArgument(supportedDegreesOfFreedomsForVector, "supportedDegreesOfFreedomsForVector");
			Guard.AgainstNullArgument(values, "values");

			Guard.AgainstBadArgument(
				"supportedNodesForVector",
				() => { return values.Length != supportedNodesForVector.Count * supportedDegreesOfFreedomsForVector.Count; },
				"The number of values should match the product of the nodes and degrees of freedoms");

			this.supportedNodes = new List<IFiniteElementNode>(supportedNodesForVector);
			this.supportedDof = new List<DegreeOfFreedom>(supportedDegreesOfFreedomsForVector);
			this.internalVector = new DenseVector(values);
		}

		/// <summary>
		/// Clears the store and sets all the data to the given value
		/// </summary>
		/// <param name="initialValue">The value to set all data to.</param>
		private void InitializeAllData(double initialValue)
		{
			this.InitializeData(this.supportedNodes, this.supportedDof, initialValue);
		}

		private void InitializeData(IList<IFiniteElementNode> nodesToInitialize, IList<DegreeOfFreedom> degreesOfFreedomsToInitialize, double initialValue)
		{
			Guard.AgainstNullOrEmptyListArgument(nodesToInitialize, "nodesToInitialize");
			Guard.AgainstNullOrEmptyListArgument(degreesOfFreedomsToInitialize, "degreesOfFreedomsToInitialize");

			foreach (IFiniteElementNode key in nodesToInitialize)
			{
				foreach(DegreeOfFreedom dof in degreesOfFreedomsToInitialize) {
					this[key, dof] = initialValue;
				}
			}
		}

		public Vector<double> ToVector()
		{
			return this.internalVector.Clone();
		}
	}
}

