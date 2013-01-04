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
	/// The finite element class defines the topology between nodes.
	/// The finite element class defines the local coordinate frame for the finite element in relation to the global frame.
	/// The StiffnessBuilder property connects to a separate object implementing IStiffnessBuilder - that class calculates the stiffness matrices, strain-displacement matrices, rotation matrices and shape functions for this element.
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
		/// The nodal degrees of freedom supported by this element.
		/// </summary>
		/// <param name="stiffness"></param>
		private IList<NodalDegreeOfFreedom> _supportedNodalDegreeOfFreedoms;
		
		/// <summary>
		/// Initializes a new instance of the <see cref="FiniteElement" /> class.
		/// </summary>
		protected FiniteElement(IStiffnessMatrixBuilder stiffness)
		{
			if (stiffness == null)
			{
				throw new ArgumentNullException("stiffness");
			}
			this.StiffnessBuilder = stiffness;
			this.StiffnessBuilder.Initialize(this);
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
					this.GlobalStiffnessMatrix = this.StiffnessBuilder.BuildGlobalStiffnessMatrix();
					this.elementIsDirty = false;
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
		internal IStiffnessMatrixBuilder StiffnessBuilder
		{
			get;
			private set;
		}
		
		/// <summary>
		/// Gets the local x-axis of this finite element
		/// </summary>
		public abstract Vector LocalXAxis
		{
			get;
		}
		
		/// <summary>
		/// Gets the local y-axis of this finite element.
		/// </summary>
		public abstract Vector LocalYAxis
		{
			get;
		}
		
		/// <summary>
		/// Gets the local z-axis of this finite element.
		/// </summary>
		/// <remarks>
		/// Calculates the normalised cross-product of the x and y axes.
		/// </remarks>
		public Vector LocalZAxis
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
			get
			{
				if(this.elementIsDirty)
				{
					this.BuildSupportedGlobalNodalDegreeOfFreedoms();
				}
				return this._supportedNodalDegreeOfFreedoms;
			}
			private set
			{
				this._supportedNodalDegreeOfFreedoms = value;
			}
		}
		
		#region Equals and GetHashCode implementation
		public override bool Equals(object obj)
		{
			FiniteElement other = obj as FiniteElement;
			if (other == null)
				return false;
			return object.Equals(this.nodeStore, other.nodeStore) && this.elementIsDirty == other.elementIsDirty && object.Equals(this.stiffnessMatrix, other.stiffnessMatrix) && object.Equals(this.StiffnessBuilder, other.StiffnessBuilder);
			//TODO compare supported nodal degrees of freedom (will need to compare the contents, as the list object will change).
		}
		
		public override int GetHashCode()
		{
			int hashCode = 0;
			unchecked {
				if (nodeStore != null)
					hashCode += 1000000007 * nodeStore.GetHashCode();
				if (stiffnessMatrix != null)
					hashCode += 1000000021 * stiffnessMatrix.GetHashCode();
				if (StiffnessBuilder != null)
					hashCode += 1000000033 * StiffnessBuilder.GetHashCode();
				// TODO implement the below, using the items of the list rather than the list objec
//				if (SupportedNodalDegreeOfFreedoms != null)
//					hashCode += 1000000087 * SupportedNodalDegreeOfFreedoms.GetHashCode();
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
		public abstract bool IsASupportedLocalStiffnessDegreeOfFreedom(DegreeOfFreedom degreeOfFreedom);
		
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
				this.GlobalStiffnessMatrix = this.StiffnessBuilder.BuildGlobalStiffnessMatrix();
				this.elementIsDirty = false;
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
		/// Builds the list of possible nodal degree of freedoms for this element which are expected by the model
		/// </summary>
		/// <returns>A list of all the possible nodal degree of freedoms for this element</returns>
		protected void BuildSupportedGlobalNodalDegreeOfFreedoms() //FIXME make abstract and move to derived classes.
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
			
			this.SupportedNodalDegreeOfFreedoms = nodalDegreeOfFreedoms;
		}
		
		
	}
}
