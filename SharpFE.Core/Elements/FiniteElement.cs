//-----------------------------------------------------------------------
// <copyright file="FiniteElement.cs" company="SharpFE">
//     Copyright Iain Sproat, 2012.
// </copyright>
//-----------------------------------------------------------------------

namespace SharpFE
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using SharpFE.Stiffness;
    using SharpFE.Geometry;

    /// <summary>
    /// Finite elements connect nodes and define the relationship between these nodes.
    /// The finite element class defines the topology between nodes.
    /// The finite element class defines the local coordinate frame for the finite element in relation to the global frame.
    /// The StiffnessBuilder property connects to a separate object implementing IStiffnessBuilder - that class calculates the stiffness matrices, strain-displacement matrices, rotation matrices and shape functions for this element.
    /// </summary>
    public abstract class FiniteElement : IFiniteElement, IEquatable<FiniteElement>
    {
        /// <summary>
        /// The nodes of this element.
        /// </summary>
        private IList<IFiniteElementNode> nodeStore = new List<IFiniteElementNode>();
        
        /// <summary>
        /// The nodal degrees of freedom supported by this element.
        /// </summary>
        /// <param name="stiffness"></param>
        private IList<NodalDegreeOfFreedom> supportedNodalDof;
        
        /// <summary>
        /// 
        /// </summary>
        private int hashAtWhichNodalDegreesOfFreedomWereLastBuilt;
        
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
        public IList<IFiniteElementNode> Nodes
        {
            get
            {
                return new List<IFiniteElementNode>(this.nodeStore);
            }
        }
        
        /// <summary>
        /// The point in the global coordinate frame which represents the origin in the local coordinate frame.
        /// </summary>
        public Point LocalOrigin
        {
            get
            {
                return this.Nodes[0].Location;
            }
        }
        
        /// <summary>
        /// Gets the local x-axis of this finite element
        /// </summary>
        public abstract GeometricVector LocalXAxis
        {
            get;
        }
        
        /// <summary>
        /// Gets the local y-axis of this finite element.
        /// </summary>
        public abstract GeometricVector LocalYAxis
        {
            get;
        }
        
        /// <summary>
        /// Gets the local z-axis of this finite element.
        /// </summary>
        /// <remarks>
        /// Calculates the normalised cross-product of the x and y axes.
        /// </remarks>
        public GeometricVector LocalZAxis
        {
            get
            {
                return this.LocalXAxis.CrossProduct(this.LocalYAxis).Normalize(2);
            }
        }
        
        /// <summary>
        /// Gets the nodal degrees of freedom supported by this finite element
        /// </summary>
        internal IList<NodalDegreeOfFreedom> SupportedNodalDegreeOfFreedoms
        {
            get
            {
                if (this.IsDirty(this.hashAtWhichNodalDegreesOfFreedomWereLastBuilt))
                {
                    this.SupportedNodalDegreeOfFreedoms = this.BuildSupportedGlobalNodalDegreeOfFreedoms();
                    this.hashAtWhichNodalDegreesOfFreedomWereLastBuilt = this.GetHashCode();
                }
                
                return this.supportedNodalDof;
            }
            
            private set
            {
                this.supportedNodalDof = value;
            }
        }
        
        #region Equals and GetHashCode implementation
        /// <summary>
        /// 
        /// </summary>
        /// <param name="leftHandSide"></param>
        /// <param name="rightHandSide"></param>
        /// <returns></returns>
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
        /// 
        /// </summary>
        /// <param name="leftHandSide"></param>
        /// <param name="rightHandSide"></param>
        /// <returns></returns>
        public static bool operator !=(FiniteElement leftHandSide, FiniteElement rightHandSide)
        {
            return !(leftHandSide == rightHandSide);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return this.Equals(obj as FiniteElement);
            
        }
        
        public bool Equals(FiniteElement other)
        {
            if (other == null)
            {
                return false;
            }
            
            int numberNodes = this.nodeStore.Count;
            if (numberNodes != other.nodeStore.Count)
            {
                return false;
            }
            
            for (int i = 0; i < numberNodes; i++)
            {
                if (!this.nodeStore[i].Equals(other.nodeStore[i]))
                {
                    return false;
                }
            }
            
            return true;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            int hashCode = 0;
            int i = 0;
            unchecked
            {
                foreach (FiniteElementNode node in this.nodeStore)
                {
                    hashCode += (1000000000 + i++) * node.GetHashCode();
                    
                }
                
                hashCode += 1000000007 * this.LocalOrigin.GetHashCode();
            }
            
            return hashCode;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            sb.Append(this.GetType().FullName);
            sb.Append(", ");
            
            int numNodes = this.nodeStore.Count;
            if (numNodes == 0)
            {
                sb.Append("<no nodes>");
                sb.Append("]");
                return sb.ToString();
            }
            
            for (int i = 0; i < numNodes; i++)
            {
                sb.Append("<");
                sb.Append(this.nodeStore[i].ToString());
                sb.Append(">");
                if (i < numNodes - 1)
                {
                    sb.Append(", ");
                }
            }
            
            sb.Append("]");
            
            return sb.ToString();
        }

        #endregion

        public Point ConvertGlobalCoordinatesToLocalCoordinates(Point globalPoint)
        {
            GeometricVector localCoordRelativeToLocalOrigin = globalPoint.Subtract(this.LocalOrigin);
            
            KeyedSquareMatrix<DegreeOfFreedom> rotationMatrix = CalculateElementRotationMatrix();
            Point localCoord = new Point(rotationMatrix.Multiply(localCoordRelativeToLocalOrigin));
            
            return new Point(localCoord);
        }
        
        public Point ConvertLocalCoordinatesToGlobalCoordinates(Point localPoint)
        {
            KeyedSquareMatrix<DegreeOfFreedom> rotationMatrix = CalculateElementRotationMatrix().Transpose();
            Point globalCoordRelativeToLocalOrigin = new Point(rotationMatrix.Multiply(localPoint));
            
            GeometricVector globalCoord = globalCoordRelativeToLocalOrigin.Add(this.LocalOrigin);
            return new Point(globalCoord);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public KeyedSquareMatrix<DegreeOfFreedom> CalculateElementRotationMatrix()
        {
            KeyedSquareMatrix<DegreeOfFreedom> rotationMatrix = FiniteElement.CreateFromRows(this.LocalXAxis, this.LocalYAxis, this.LocalZAxis);
            rotationMatrix = rotationMatrix.NormalizeRows(2);
            return rotationMatrix;
        }
        
        /// <summary>
        /// Determines whether a degree of freedom is supported by this element
        /// </summary>
        /// <param name="degreeOfFreedom"></param>
        /// <returns></returns>
        public abstract bool IsASupportedBoundaryConditionDegreeOfFreedom(DegreeOfFreedom degreeOfFreedom);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="previousHash"></param>
        /// <returns></returns>
        public bool IsDirty(int previousHash)
        {
            return this.GetHashCode() != previousHash;
        }
        
        /// <summary>
        /// Removes a node from the element.
        /// </summary>
        /// <param name="nodeToRemove">The node to remove from the element</param>
        internal void RemoveNode(FiniteElementNode nodeToRemove)
        {
            this.nodeStore.Remove(nodeToRemove);
        }
        
        /// <summary>
        /// Adds a new node to the element.
        /// </summary>
        /// <param name="nodeToAdd">The node to add to the element</param>
        /// <exception cref="ArgumentNullException">Thrown if the node to add is null</exception>
        /// <exception cref="ArgumentException">Thrown if the node is already part of the finite element</exception>
        protected void AddNode(FiniteElementNode nodeToAdd)
        {
            Guard.AgainstNullArgument(nodeToAdd, "nodeToAdd");
            
            if (this.Nodes.Contains(nodeToAdd))
            {
                throw new ArgumentException("Node is already part of this element");
            }
            
            this.ThrowIfNodeCannotBeAdded(nodeToAdd);
            this.nodeStore.Add(nodeToAdd);
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
        protected IList<NodalDegreeOfFreedom> BuildSupportedGlobalNodalDegreeOfFreedoms() ////TODO make abstract and require derived classes to implement for their specific requirements
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
        /// Creates a new keyed matrix from keyed vectors representing the rows of the new matrix
        /// </summary>
        /// <param name="axis1">The vector representing the first row</param>
        /// <param name="axis2">The vector representing the second row</param>
        /// <param name="axis3">The vector representing the third row</param>
        /// <returns>A matrix built from the vectors</returns>
        protected static KeyedSquareMatrix<DegreeOfFreedom> CreateFromRows(KeyedVector<DegreeOfFreedom> axis1, KeyedVector<DegreeOfFreedom> axis2, KeyedVector<DegreeOfFreedom> axis3)
        {
            ////TODO this should be devolved to the KeyedMatrix class
            Guard.AgainstBadArgument(
                () => { return axis1.Count != 3; },
                "All axes should be 3D, i.e. have 3 items",
                "axis1");
            Guard.AgainstBadArgument(
                () => { return axis2.Count != 3; },
                "All axes should be 3D, i.e. have 3 items",
                "axis2");
            Guard.AgainstBadArgument(
                () => { return axis3.Count != 3; },
                "All axes should be 3D, i.e. have 3 items",
                "axis3");
            Guard.AgainstBadArgument(
                () => { return axis1.SumMagnitudes() == 0; },
                string.Format(
                    System.Globalization.CultureInfo.InvariantCulture,
                    "Axis should not be zero: {0}",
                    axis1),
                "axis1");
            Guard.AgainstBadArgument(
                () => { return axis2.SumMagnitudes() == 0; },
                string.Format(
                    System.Globalization.CultureInfo.InvariantCulture,
                    "Axis should not be zero: {0}",
                    axis2),
                "axis2");
            Guard.AgainstBadArgument(
                () => { return axis3.SumMagnitudes() == 0; },
                string.Format(
                    System.Globalization.CultureInfo.InvariantCulture,
                    "Axis should not be zero: {0}",
                    axis3),
                "axis3");
            
            KeyedVector<DegreeOfFreedom> axis1Norm = axis1.Normalize(2);
            KeyedVector<DegreeOfFreedom> axis2Norm = axis2.Normalize(2);
            KeyedVector<DegreeOfFreedom> axis3Norm = axis3.Normalize(2);
            
            IList<DegreeOfFreedom> dof = new List<DegreeOfFreedom>(3) { DegreeOfFreedom.X, DegreeOfFreedom.Y, DegreeOfFreedom.Z };
            
            KeyedSquareMatrix<DegreeOfFreedom> result = new KeyedSquareMatrix<DegreeOfFreedom>(dof);
            result.At(DegreeOfFreedom.X, DegreeOfFreedom.X, axis1Norm[DegreeOfFreedom.X]);
            result.At(DegreeOfFreedom.X, DegreeOfFreedom.Y, axis1Norm[DegreeOfFreedom.Y]);
            result.At(DegreeOfFreedom.X, DegreeOfFreedom.Z, axis1Norm[DegreeOfFreedom.Z]);
            result.At(DegreeOfFreedom.Y, DegreeOfFreedom.X, axis2Norm[DegreeOfFreedom.X]);
            result.At(DegreeOfFreedom.Y, DegreeOfFreedom.Y, axis2Norm[DegreeOfFreedom.Y]);
            result.At(DegreeOfFreedom.Y, DegreeOfFreedom.Z, axis2Norm[DegreeOfFreedom.Z]);
            result.At(DegreeOfFreedom.Z, DegreeOfFreedom.X, axis3Norm[DegreeOfFreedom.X]);
            result.At(DegreeOfFreedom.Z, DegreeOfFreedom.Y, axis3Norm[DegreeOfFreedom.Y]);
            result.At(DegreeOfFreedom.Z, DegreeOfFreedom.Z, axis3Norm[DegreeOfFreedom.Z]);
            
            return result;
        }
    }
}
