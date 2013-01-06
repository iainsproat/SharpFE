//-----------------------------------------------------------------------
// <copyright file="FiniteElementNode.cs" company="SharpFE">
//     Copyright Iain Sproat, 2012.
// </copyright>
//-----------------------------------------------------------------------

namespace SharpFE
{
    using System;
    using MathNet.Numerics.LinearAlgebra.Double;
    
    /// <summary>
    /// A finite element node is a fundamental part of a finite element mesh.
    /// Nodes have a position in space.
    /// Constraints and forces can be applied to nodes.
    /// </summary>
    public class FiniteElementNode
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="FiniteElementNode" /> class.
        /// </summary>
        /// <param name="locationAlongXAxis">The location of this node along the x axis</param>
        internal FiniteElementNode(double locationAlongXAxis)
            : this(locationAlongXAxis, 0, 0)
        {
            // empty
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="FiniteElementNode" /> class.
        /// </summary>
        /// <param name="locationAlongXAxis">The location of this node along the x axis</param>
        /// <param name="locationAlongYAxis">The location of this node along the y axis</param>
        internal FiniteElementNode(double locationAlongXAxis, double locationAlongYAxis)
            : this(locationAlongXAxis, locationAlongYAxis, 0)
        {
            // empty
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="FiniteElementNode" /> class.
        /// </summary>
        /// <param name="locationAlongXAxis">The location of this node along the x axis</param>
        /// <param name="locationAlongYAxis">The location of this node along the y axis</param>
        /// <param name="locationAlongZAxis">The location of this node along the z axis</param>
        internal FiniteElementNode(double locationAlongXAxis, double locationAlongYAxis, double locationAlongZAxis)
        {
            this.OriginalX = locationAlongXAxis;
            this.OriginalY = locationAlongYAxis;
            this.OriginalZ = locationAlongZAxis;
        }
        
        /// <summary>
        /// Prevents a default instance of the <see cref="FiniteElementNode" /> class from being created.
        /// </summary>
        private FiniteElementNode()
        {
            // empty
        }
        #endregion
        
        // TODO include an identifier, which is anything hashable and may be user defined.  The noderepository will have to check that the Ids do not conflict as they are added or edited.
        
        /// <summary>
        /// Gets the original location of this node along the global x-axis.
        /// </summary>
        public double OriginalX
        {
            get;
            private set;
        }
        
        /// <summary>
        /// Gets the original location of this node along the global y-axis.
        /// </summary>
        public double OriginalY
        {
            get;
            private set;
        }
        
        /// <summary>
        /// Gets the original location of this node along the global z-axis.
        /// </summary>
        public double OriginalZ
        {
            get;
            private set;
        }
        
        public Vector AsVector()
        {
        	Vector result = new DenseVector(3);
        	result[0] = this.OriginalX;
        	result[1] = this.OriginalY;
        	result[2] = this.OriginalZ;
        	return result;
        }
        
        #region Equals and GetHashCode implementation
        /// <summary>
        /// Determines whether two nodes are equal.
        /// </summary>
        /// <param name="leftHandSide">the node at the left hand side of the equality comparison statement</param>
        /// <param name="rightHandSide">The node at the right hand side of the equality comparison statement</param>
        /// <returns>true if the two nodes are equal; otherwise, false</returns>
        public static bool operator ==(FiniteElementNode leftHandSide, FiniteElementNode rightHandSide)
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
        /// Determines whether two nodes are unequal.
        /// </summary>
        /// <param name="leftHandSide">the node at the left hand side of the inequality comparison statement</param>
        /// <param name="rightHandSide">The node at the right hand side of the inequality comparison statement</param>
        /// <returns>false if the two nodes are equal; otherwise, true</returns>
        public static bool operator !=(FiniteElementNode leftHandSide, FiniteElementNode rightHandSide)
        {
            return !(leftHandSide == rightHandSide);
        }
        
        /// <summary>
        /// Determines whether this node is equal to an object
        /// </summary>
        /// <param name="obj">the object to compare with this node for equality</param>
        /// <returns>true if the object is equal to this node; otherwise, false</returns>
        public override bool Equals(object obj)
        {
            FiniteElementNode other = obj as FiniteElementNode;
            return this.Equals(other);
        }
        
        /// <summary>
        /// Determines whether this node matches another node
        /// </summary>
        /// <param name="other">the other node to compare for equality</param>
        /// <returns>true if the nodes are equal; otherwise, false</returns>
        public bool Equals(FiniteElementNode other)
        {
            if (other == null)
            {
                return false;
            }
            
            return object.Equals(this.OriginalX, other.OriginalX) && object.Equals(this.OriginalY, other.OriginalY) && object.Equals(this.OriginalZ, other.OriginalZ);
        }
        
        /// <summary>
        /// Returns the hashcode for this instance
        /// </summary>
        /// <returns>A 32-bit signed integer hashcode</returns>
        public override int GetHashCode()
        {
            int hashCode = 0;
            unchecked
            {
                hashCode += 1000000007 * this.OriginalX.GetHashCode();
                hashCode += 1000000009 * this.OriginalY.GetHashCode();
                hashCode += 1000000021 * this.OriginalZ.GetHashCode();
            }
            
            return hashCode;
        }
        
        /// <summary>
        /// Overrides the ToString method
        /// </summary>
        /// <returns>A string representation of the contents of this object</returns>
        public override string ToString()
        {
            return string.Format(
                System.Globalization.CultureInfo.InvariantCulture,
                "[{0}, {1}, {2}]",
                this.OriginalX,
                this.OriginalY,
                this.OriginalZ);
        }

        #endregion
    }
}
