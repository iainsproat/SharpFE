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
        protected FiniteElement()
        {
            // empty
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
                    this.PrepareAndGenerateLocalStiffnessMatrix();
                }
                
                return this.stiffnessMatrix;
            }
            
            private set
            {
                this.stiffnessMatrix = value;
            }
        }
        
        /// <summary>
        /// Gets the stiffness matrix for this element.
        /// Amending values in the matrix will alter the behaviour of this element.
        /// </summary>
        internal ElementStiffnessMatrix LocalStiffnessMatrix // HACK exposed as internal for unit testing only, should be private
        {
            get;
            private set;
        }
        
        /// <summary>
        /// Gets the rotational matrix of this element, which will transform local coordinates to global coordinates
        /// </summary>
        internal ElementStiffnessMatrix ElementStiffnessRotationMatrixFromLocalToGlobalCoordinates // HACK exposed as internal for unit testing only, should be private
        {
            get;
            private set;
        }
        
        /// <summary>
        /// Gets the rotation matrix for rotating this element from local to global coordinates
        /// </summary>
        internal Matrix RotationMatrixFromLocalToGlobal // HACK exposed as internal for unit testing only, should be private
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
        private IList<NodalDegreeOfFreedom> SupportedNodalDegreeOfFreedoms
        {
            get;
            set;
        }
        
        #region Equals and GetHashCode implementation
        /// <summary>
        /// Determines whether two finite elements equal each other
        /// </summary>
        /// <param name="leftHandSide">The finite element on the left hand side of the equality comparison statement</param>
        /// <param name="rightHandSide">The finite element on the right hand side of the equality comparison statement</param>
        /// <returns>true if the nodes equal each other; otherwise, false</returns>
        public static bool operator ==(FiniteElement leftHandSide, FiniteElement rightHandSide)
        {
            if (ReferenceEquals(leftHandSide, rightHandSide))
            {
                return true;
            }
            
            if (ReferenceEquals(leftHandSide, null) || ReferenceEquals(rightHandSide, null))
            {
                return false;
            }
            
            return leftHandSide.Equals(rightHandSide);
        }
        
        /// <summary>
        /// Determines whether two finite elements do not equal each other
        /// </summary>
        /// <param name="leftHandSide">The finite element on the left hand side of the inequality comparison statement</param>
        /// <param name="rightHandSide">The finite element on the right hand side of the inequality comparison statement</param>
        /// <returns>false if the nodes equal each other; otherwise, true</returns>
        public static bool operator !=(FiniteElement leftHandSide, FiniteElement rightHandSide)
        {
            return !(leftHandSide == rightHandSide);
        }
        
        /// <summary>
        /// Determines whether this element equals the object
        /// </summary>
        /// <param name="obj">The object being compared to this element</param>
        /// <returns>true if the object equals this element; otherwise, false</returns>
        public override bool Equals(object obj)
        {
            FiniteElement other = obj as FiniteElement;
            return this.Equals(other);
        }
        
        /// <summary>
        /// Determines whether this element equals the other element
        /// </summary>
        /// <param name="other">The other element being compared to this element</param>
        /// <returns>true if the other element equals this element; otherwise, false</returns>
        public bool Equals(FiniteElement other)
        {
            if (other == null)
            {
                return false;
            }
            
            int numberNodes = this.nodeStore.Count;
            if (other.nodeStore.Count != numberNodes)
            {
                return false;
            }
            
            for (int i = 0; i < numberNodes; i++)
            {
                if (!object.Equals(this.nodeStore[i], other.nodeStore[i]))
                {
                    return false;
                }
            }
            
            // FIXME what about elements which validly connect exactly the same nodes? e.g. two springs between the same nodes?
            return object.Equals(this.LocalStiffnessMatrix, other.LocalStiffnessMatrix);
        }
        
        /// <summary>
        /// Serves as a hash function for a particular type
        /// </summary>
        /// <returns>A hashcode for this element</returns>
        public override int GetHashCode()
        {
            int hashCode = 0;
            unchecked
            {
                if (this.nodeStore != null)
                {
                    hashCode += 1000000007 * this.nodeStore.GetHashCode();
                }
                
                if (this.LocalStiffnessMatrix != null)
                {
                    hashCode += 1000000009 * this.LocalStiffnessMatrix.GetHashCode();
                }
            }
            
            return hashCode;
        }
        #endregion
        
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
                this.PrepareAndGenerateLocalStiffnessMatrix();
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
        internal void PrepareAndGenerateLocalStiffnessMatrix() // HACK exposed as internal for unit testing, should be private
        {
            this.SupportedNodalDegreeOfFreedoms = this.BuildSupportedNodalDegreeOfFreedoms();
            
            this.LocalStiffnessMatrix = new ElementStiffnessMatrix(this.SupportedNodalDegreeOfFreedoms);
            this.BuildLocalStiffnessMatrix();
            
            this.BuildRotationalMatrixFromGlobalToLocalCoordinates();
            
            this.GlobalStiffnessMatrix = this.CalculateGlobalStiffnessMatrix();
            
            this.elementIsDirty = false;
        }
        
        /// <summary>
        /// The values of the stiffness matrix are to be generated by implementing classes.
        /// The should call the StiffnessMatrix property and set values using the At method.
        /// </summary>
        protected abstract void BuildLocalStiffnessMatrix();
        
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
        private ElementStiffnessMatrix CalculateGlobalStiffnessMatrix()
        {
            Matrix t = (Matrix)this.ElementStiffnessRotationMatrixFromLocalToGlobalCoordinates;
            Matrix k = this.LocalStiffnessMatrix;
            Matrix kt = (Matrix)k.Multiply(t); // K*T
            Matrix ttransposed = (Matrix)t.Transpose(); // T^T
            Matrix ttransposedkt = (Matrix)ttransposed.Multiply(kt); // (T^T)*K*T
            return new ElementStiffnessMatrix(ttransposedkt, this.LocalStiffnessMatrix.RowKeys, this.LocalStiffnessMatrix.ColumnKeys); 
        }
        
        /// <summary>
        /// Builds the rotational matrix from local coordinates to global coordinates.
        /// Assumes the coordinates of nodes of this element are in the global coordinate system.
        /// </summary>
        private void BuildRotationalMatrixFromGlobalToLocalCoordinates()
        {
            this.RotationMatrixFromLocalToGlobal = (Matrix)DenseMatrix.CreateFromRows(new List<Vector<double>>(3) { this.LocalXAxis, this.LocalYAxis, this.LocalZAxis });
            this.RotationMatrixFromLocalToGlobal = (Matrix)this.RotationMatrixFromLocalToGlobal.NormalizeRows(1);
            
            this.ElementStiffnessRotationMatrixFromLocalToGlobalCoordinates = new ElementStiffnessMatrix(this.SupportedNodalDegreeOfFreedoms);

            int numberOfRowsInRotationMatrix = this.RotationMatrixFromLocalToGlobal.RowCount;
            int numberOfColumnsInRotationMatrix = this.RotationMatrixFromLocalToGlobal.ColumnCount;
            this.ElementStiffnessRotationMatrixFromLocalToGlobalCoordinates.SetSubMatrix(0, numberOfRowsInRotationMatrix, 0, numberOfColumnsInRotationMatrix, this.RotationMatrixFromLocalToGlobal);
            this.ElementStiffnessRotationMatrixFromLocalToGlobalCoordinates.SetSubMatrix(6, numberOfRowsInRotationMatrix, 6, numberOfColumnsInRotationMatrix, this.RotationMatrixFromLocalToGlobal);
        }
    }
}
