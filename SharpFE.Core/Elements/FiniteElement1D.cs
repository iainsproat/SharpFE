

namespace SharpFE
{
    using System;
    using MathNet.Numerics.LinearAlgebra.Double;
    using SharpFE.Stiffness;
    
    /// <summary>
    /// One dimensional finite element. i.e. a line
    /// </summary>
    public abstract class FiniteElement1D : FiniteElement
    {
        protected FiniteElement1D(FiniteElementNode node1, FiniteElementNode node2)
        {
            this.AddNode(node1);
            this.AddNode(node2);
        }
        
        /// <summary>
        /// Gets the node at the start of this Spring
        /// </summary>
        public FiniteElementNode StartNode
        {
            get
            {
                return this.Nodes[0];
            }
        }
        
        /// <summary>
        /// Gets the node at the end of this Spring
        /// </summary>
        public FiniteElementNode EndNode
        {
            get
            {
                return this.Nodes[1];
            }
        }
        
        public double OriginalLength
        {
            get
            {
                double lengthX = this.EndNode.OriginalX - this.StartNode.OriginalX;
                double lengthY = this.EndNode.OriginalY - this.StartNode.OriginalY;
                double lengthZ = this.EndNode.OriginalZ - this.StartNode.OriginalZ;
                return Math.Sqrt(lengthX * lengthX + lengthY * lengthY + lengthZ * lengthZ);
            }
        }
        
        /// <summary>
        /// Gets or sets the vector representing the local x axis
        /// </summary>
        public override KeyedVector<DegreeOfFreedom> LocalXAxis
        {
            get
            {
                double initialLengthProjectedInXAxis = this.EndNode.OriginalX - this.StartNode.OriginalX;
                double initialLengthProjectedInYAxis = this.EndNode.OriginalY - this.StartNode.OriginalY;
                double initialLengthProjectedInZAxis = this.EndNode.OriginalZ - this.StartNode.OriginalZ;

                return new KeyedVector<DegreeOfFreedom>(new double[]
                                                     {
                                                         initialLengthProjectedInXAxis,
                                                         initialLengthProjectedInYAxis,
                                                         initialLengthProjectedInZAxis
                                                     }, DegreeOfFreedom.X, DegreeOfFreedom.Y, DegreeOfFreedom.Z);
            }
        }
        
        /// <summary>
        /// Gets or sets the vector representing the direction of the local y axis
        /// </summary>
        public override KeyedVector<DegreeOfFreedom> LocalYAxis
        {
            get
            {
                return this.UpDirection().CrossProduct(this.LocalXAxis);
            }
        }
        
        #region Equals and GetHashCode implementation
        public override bool Equals(object obj)
        {
            FiniteElement1D other = obj as FiniteElement1D;
            if (other == null)
                return false;
            return base.Equals(other);
        }
        
        public override int GetHashCode()
        {
            int hashCode = 0;
            unchecked
            {
                hashCode += base.GetHashCode();
            }
            return hashCode;
        }
        
        public static bool operator ==(FiniteElement1D lhs, FiniteElement1D rhs)
        {
            if (object.ReferenceEquals(lhs, rhs))
                return true;
            if (object.ReferenceEquals(lhs, null) || object.ReferenceEquals(rhs, null))
                return false;
            return lhs.Equals(rhs);
        }
        
        public static bool operator !=(FiniteElement1D lhs, FiniteElement1D rhs)
        {
            return !(lhs == rhs);
        }
        #endregion

        
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
        protected override void ThrowIfNodeCannotBeAdded(FiniteElementNode nodeToAdd)
        {
            if (this.Nodes.Count > 1)
            {
                throw new ArgumentException("Cannot add more than 2 nodes");
            }
            
            // TODO check for proximity of nodes
        }
        
        /// <summary>
        /// Lines are unique in that you cannot work out which way is 'up' from the nodes (could be any of an infinte number as the line spins on its own axis).
        /// This method defines an 'up' direction.  It is in the direction of the Global Z, unless the line local X axis is also aligned with global z.
        /// In that case the up direction is aligned with global y axis.
        /// </summary>
        /// <returns>A vector representing the 'up' direction.</returns>
        private KeyedVector<DegreeOfFreedom> UpDirection() // HACK
        {
            if (this.LocalXAxis[DegreeOfFreedom.X] == 0 && this.LocalXAxis[DegreeOfFreedom.Y] == 0 && this.LocalXAxis[DegreeOfFreedom.Z] != 0)
            {
                // localXAxis is in global Z axis direction which will give poor results so assume the local upright direction is in global -x direction
                return new KeyedVector<DegreeOfFreedom>(new double[] { -1, 0, 0 }, DegreeOfFreedom.X, DegreeOfFreedom.Y, DegreeOfFreedom.Z);
            }
            else
            {
                // make the global vertical the default
                return new KeyedVector<DegreeOfFreedom>(new double[] { 0, 0, 1 }, DegreeOfFreedom.X, DegreeOfFreedom.Y, DegreeOfFreedom.Z);
            }
        }
    }
}
