//-----------------------------------------------------------------------
// <copyright file="FiniteElement.cs" company="SharpFE">
//     Copyright Iain Sproat, 2012.
// </copyright>
//-----------------------------------------------------------------------

namespace SharpFE
{
	using System;
	using System.Collections.Generic;
	using MathNet.Numerics.LinearAlgebra.Double;
	using MathNet.Numerics.LinearAlgebra.Generic;
	using SharpFE.Stiffness;

	/// <summary>
	/// Finite elements connect nodes and define the relationship between these nodes.
	/// Finite elements do this by providing stiffness values between nodes.
	/// </summary>
	public abstract class FiniteElement
	{
		/// <summary>
		/// The nodes of this element.
		/// </summary>
		private IList<FiniteElementNode> nodeStore = new List<FiniteElementNode>();
		
		/// <summary>
		/// A flag to indicate whether the element has been modified since the stiffness matrix was last generated.
		/// This should only be set to false by the PrepareAndGenerateStiffnessMatrix method.
		/// It can be set to true by any other method or property. e.g. when another node is added.
		/// </summary>
		private bool elementIsDirty = true;
		
		/// <summary>
		/// The global stiffness matrix of this element
		/// </summary>
		private ElementStiffnessMatrix stiffnessMatrix;
		
		
		
		/// <summary>
		/// Initializes a new instance of the <see cref="FiniteElement" /> class.
		/// </summary>
		protected FiniteElement(IStiffnessMatrixBuilder stiffness)
		{
			if (stiffness == null)
			{
				throw new ArgumentNullException("stiffness");
			}
			this.StiffnessProvider = stiffness;
		}
		
		/// <summary>
		/// Gets the nodes which comprise this element.
		/// </summary>
		/// <returns>Returns a shallow copy of the list of nodes which comprise this element.</returns>
		public IList<FiniteElementNode> Nodes
		{
			get
			{
				return new List<FiniteElementNode>(this.nodeStore);
			}
		}
		
		/// <summary>
		/// Gets the stiffness matrix of this element.
		/// </summary>
		internal ElementStiffnessMatrix GlobalStiffnessMatrix
		{
			get
			{
				if (this.elementIsDirty)
				{
					this.PrepareAndGenerateGlobalStiffnessMatrix();
				}
				
				return this.stiffnessMatrix;
			}
			
			private set
			{
				this.stiffnessMatrix = value;
			}
		}
		
		/// <summary>
		/// Determine the stiffness of the element,
		/// and is used when building the local stiffness matrix
		/// </summary>
		protected IStiffnessMatrixBuilder StiffnessProvider
		{
			get;
			private set;
		}
		
		/// <summary>
		/// Gets the local x-axis of this finite element
		/// </summary>
		protected abstract Vector LocalXAxis
		{
			get;
		}
		
		/// <summary>
		/// Gets the local y-axis of this finite element.
		/// </summary>
		protected abstract Vector LocalYAxis
		{
			get;
		}
		
		/// <summary>
		/// Gets the local z-axis of this finite element.
		/// </summary>
		/// <remarks>
		/// Calculates the normalised cross-product of the x and y axes.
		/// </remarks>
		protected Vector LocalZAxis
		{
			get
			{
				return (Vector)this.LocalXAxis.CrossProduct(this.LocalYAxis);
			}
		}
		
		/// <summary>
		/// Gets or sets the nodal degrees of freedom supported by this finite element
		/// </summary>
		internal IList<NodalDegreeOfFreedom> SupportedNodalDegreeOfFreedoms
		{
			get;
			private set;
		}
		
		#region Equals and GetHashCode implementation
		public override bool Equals(object obj)
		{
			FiniteElement other = obj as FiniteElement;
			if (other == null)
				return false;
			return object.Equals(this.nodeStore, other.nodeStore) && this.elementIsDirty == other.elementIsDirty && object.Equals(this.stiffnessMatrix, other.stiffnessMatrix) && object.Equals(this.StiffnessProvider, other.StiffnessProvider) && object.Equals(this.SupportedNodalDegreeOfFreedoms, other.SupportedNodalDegreeOfFreedoms);
		}
		
		public override int GetHashCode()
		{
			int hashCode = 0;
			unchecked {
				if (nodeStore != null)
					hashCode += 1000000007 * nodeStore.GetHashCode();
				hashCode += 1000000009 * elementIsDirty.GetHashCode();
				if (stiffnessMatrix != null)
					hashCode += 1000000021 * stiffnessMatrix.GetHashCode();
				if (StiffnessProvider != null)
					hashCode += 1000000033 * StiffnessProvider.GetHashCode();
				if (SupportedNodalDegreeOfFreedoms != null)
					hashCode += 1000000087 * SupportedNodalDegreeOfFreedoms.GetHashCode();
			}
			return hashCode;
		}
		
		public static bool operator ==(FiniteElement lhs, FiniteElement rhs)
		{
			if (ReferenceEquals(lhs, rhs))
				return true;
			if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null))
				return false;
			return lhs.Equals(rhs);
		}
		
		public static bool operator !=(FiniteElement lhs, FiniteElement rhs)
		{
			return !(lhs == rhs);
		}
		#endregion

		
		/// <summary>
		/// Determines whether a degree of freedom is supported by this element
		/// </summary>
		/// <param name="degreeOfFreedom"></param>
		/// <returns></returns>
		public abstract bool IsASupportedStiffnessDegreeOfFreedom(DegreeOfFreedom degreeOfFreedom);
		
		/// <summary>
		/// Gets the exact stiffness value for a given node and degree of freedom combinations.
		/// </summary>
		/// <param name="rowNode">The node defining the row (force equations)</param>
		/// <param name="rowDegreeOfFreedom">The degree of freedom defining the row (force equations)</param>
		/// <param name="columnNode">The node defining the column (displacement equations)</param>
		/// <param name="columnDegreeOfFreedom">the degree of freedom defining the column (displacement equations)</param>
		/// <returns>A value representing the stiffness at the given locations</returns>
		/// <exception cref="ArgumentException">Thrown if either of the nodes is not part of this element, or either of the degrees of freedom are not supported by this element.</exception>
		public double GetStiffnessAt(FiniteElementNode rowNode, DegreeOfFreedom rowDegreeOfFreedom, FiniteElementNode columnNode, DegreeOfFreedom columnDegreeOfFreedom)
		{
			if (rowNode == null)
			{
				throw new ArgumentNullException("rowNode");
			}
			
			if (columnNode == null)
			{
				throw new ArgumentNullException("columnNode");
			}
			
			if (this.elementIsDirty)
			{
				this.PrepareAndGenerateGlobalStiffnessMatrix();
			}
			
			return this.GlobalStiffnessMatrix.At(rowNode, rowDegreeOfFreedom, columnNode, columnDegreeOfFreedom);
		}
		
		/// <summary>
		/// Adds a new node to the element.
		/// </summary>
		/// <param name="nodeToAdd">The node to add to the element</param>
		internal void AddNode(FiniteElementNode nodeToAdd)
		{
			this.ThrowIfNodeCannotBeAdded(nodeToAdd);
			this.nodeStore.Add(nodeToAdd);
			this.elementIsDirty = true;
		}
		
		/// <summary>
		/// Removes a node from the element.
		/// </summary>
		/// <param name="nodeToRemove">The node to remove from the element</param>
		internal void RemoveNode(FiniteElementNode nodeToRemove)
		{
			bool success = this.nodeStore.Remove(nodeToRemove);
			if (success)
			{
				this.elementIsDirty = true;
			}
		}
		
		/// <summary>
		/// Prepares and generates the stiffness matrix.
		/// It creates an entirely new matrix from the current set of nodes and the supported degrees of freedom of this element.
		/// It calls the GenerateStiffnessMatrix method which inheriting classes are expected to implement.
		/// It sets the stiffnessMatrixHasBeenGenerated flag to true.
		/// </summary>
		internal void PrepareAndGenerateGlobalStiffnessMatrix() // HACK exposed as internal for unit testing only, should be private
		{
			this.SupportedNodalDegreeOfFreedoms = this.BuildSupportedNodalDegreeOfFreedoms();
			
			Matrix localStiffnessMatrix = this.StiffnessProvider.GetStiffnessMatrix(this);
			Matrix rotationalMatrix = this.BuildStiffnessRotationMatrixFromLocalToGlobalCoordinates();
			this.GlobalStiffnessMatrix = this.CalculateGlobalStiffnessMatrix(localStiffnessMatrix, rotationalMatrix);
			
			this.elementIsDirty = false;
		}
		
		/// <summary>
		/// Builds the rotational matrix from local coordinates to global coordinates.
		/// </summary>
		internal Matrix BuildStiffnessRotationMatrixFromLocalToGlobalCoordinates()
		{
			Matrix rotationMatrix = CalculateElementRotationMatrix();
			
			Matrix elementRotationMatrixFromLocalToGlobalCoordinates = new ElementStiffnessMatrix(this.SupportedNodalDegreeOfFreedoms);

			int numberOfRowsInRotationMatrix = rotationMatrix.RowCount;
			int numberOfColumnsInRotationMatrix = rotationMatrix.ColumnCount;
			elementRotationMatrixFromLocalToGlobalCoordinates.SetSubMatrix(0, numberOfRowsInRotationMatrix, 0, numberOfColumnsInRotationMatrix, rotationMatrix);
			elementRotationMatrixFromLocalToGlobalCoordinates.SetSubMatrix(6, numberOfRowsInRotationMatrix, 6, numberOfColumnsInRotationMatrix, rotationMatrix);
			return elementRotationMatrixFromLocalToGlobalCoordinates;
		}
		
		internal Matrix CalculateElementRotationMatrix()
		{
			Matrix rotationMatrix = (Matrix)DenseMatrix.CreateFromRows(new List<Vector<double>>(3) { this.LocalXAxis, this.LocalYAxis, this.LocalZAxis });
			rotationMatrix = (Matrix)rotationMatrix.NormalizeRows(2);
			return rotationMatrix;
		}
		
		/// <summary>
		/// Checks as to whether a new node can actually be added
		/// </summary>
		/// <param name="nodeToAdd">The candidate node to add to this element</param>
		/// <exception cref="ArgumentException">
		/// Thrown if the node cannot be added.
		/// This might be because there would be too many nodes for the type of element (e.g. greater than 2 nodes for a spring),
		/// the node is a duplicate of an existing node, is too close to an existing node,
		/// would be out of the plane of the other nodes if this element is planar,
		/// or would be out of an acceptable order (e.g. create a 'twist' in a quadrilateral)
		/// </exception>
		protected abstract void ThrowIfNodeCannotBeAdded(FiniteElementNode nodeToAdd);
		
		/// <summary>
		/// Builds the list of possible nodal degree of freedoms for this element
		/// </summary>
		/// <returns>A list of all the possible nodal degree of freedoms for this element</returns>
		private IList<NodalDegreeOfFreedom> BuildSupportedNodalDegreeOfFreedoms()
		{
			IList<NodalDegreeOfFreedom> nodalDegreeOfFreedoms = new List<NodalDegreeOfFreedom>();
			foreach (FiniteElementNode node in this.nodeStore)
			{
				nodalDegreeOfFreedoms.Add(new NodalDegreeOfFreedom(node, DegreeOfFreedom.X));
				nodalDegreeOfFreedoms.Add(new NodalDegreeOfFreedom(node, DegreeOfFreedom.Y));
				nodalDegreeOfFreedoms.Add(new NodalDegreeOfFreedom(node, DegreeOfFreedom.Z));
				nodalDegreeOfFreedoms.Add(new NodalDegreeOfFreedom(node, DegreeOfFreedom.XX));
				nodalDegreeOfFreedoms.Add(new NodalDegreeOfFreedom(node, DegreeOfFreedom.YY));
				nodalDegreeOfFreedoms.Add(new NodalDegreeOfFreedom(node, DegreeOfFreedom.ZZ));
			}
			
			return nodalDegreeOfFreedoms;
		}
		
		/// <summary>
		/// Calculates the global stiffness matrix using the local stiffness matrix and the rotational matrix
		/// </summary>
		/// <returns>A matrix representing the stiffness matrix of this element in the global coordinate system</returns>
		private ElementStiffnessMatrix CalculateGlobalStiffnessMatrix(Matrix k, Matrix t)
		{
			Matrix kt = (Matrix)k.Multiply(t); // K*T
			Matrix ttransposed = (Matrix)t.Transpose(); // T^
			Matrix ttransposedkt = (Matrix)ttransposed.Multiply(kt); // (T^)*K*T
			ElementStiffnessMatrix result = new ElementStiffnessMatrix(ttransposedkt, this.SupportedNodalDegreeOfFreedoms, this.SupportedNodalDegreeOfFreedoms);
			return result;
		}
	}
}
