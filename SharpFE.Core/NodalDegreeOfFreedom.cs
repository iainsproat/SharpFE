//-----------------------------------------------------------------------
// <copyright file="NodalDegreeOfFreedom.cs" company="SharpFE">
//     Copyright Iain Sproat, 2012.
// </copyright>
//-----------------------------------------------------------------------

namespace SharpFE
{
    using System;
    
    /// <summary>
    /// Each node has up to six degrees of freedom.  This data type identifies the node and a one particular degree of freedom on that node.
    /// </summary>
    public struct NodalDegreeOfFreedom : IEquatable<NodalDegreeOfFreedom>
    {
        /// <summary>
        /// The node to which the degree of freedom relates.
        /// </summary>
        private FiniteElementNode targetNode;
        
        /// <summary>
        /// The degree of freedom of the node
        /// </summary>
        private DegreeOfFreedom dof;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="NodalDegreeOfFreedom" /> struct.
        /// </summary>
        /// <param name="node">The node to which the degree of freedom relates</param>
        /// <param name="degreeOfFreedom">The degree of freedom of the node</param>
        /// <exception cref="ArgumentNullException">node parameter cannot be null</exception>
        public NodalDegreeOfFreedom(FiniteElementNode node, DegreeOfFreedom degreeOfFreedom)
        {
            Guard.AgainstNullArgument(node, "node");
            
            this.targetNode = node;
            this.dof = degreeOfFreedom;
        }
        
        /// <summary>
        /// Gets the node which this relates to.
        /// </summary>
        /// <remarks>Node will never be null.</remarks>
        public FiniteElementNode Node
        {
            get { return this.targetNode; }
        }
        
        /// <summary>
        /// Gets the Degree of freedom of the node.
        /// </summary>
        public DegreeOfFreedom DegreeOfFreedom
        {
            get { return this.dof; }
        }
        
        #region Equals and GetHashCode implementation
        
        /// <summary>
        /// Determines whether the left hand side equals the right hand side of the statement.
        /// </summary>
        /// <param name="leftHandSide">The first instance in this comparison</param>
        /// <param name="rightHandSide">The second instance in this comparison</param>
        /// <returns>true if the objects are equal, false otherwise</returns>
        public static bool operator ==(NodalDegreeOfFreedom leftHandSide, NodalDegreeOfFreedom rightHandSide)
        {
            return leftHandSide.Equals(rightHandSide);
        }
        
        /// <summary>
        /// Determines whether the left hand side is unequal to the right hand side of the statement.
        /// </summary>
        /// <param name="leftHandSide">The first instance in this comparison</param>
        /// <param name="rightHandSide">The second instance in this comparison</param>
        /// <returns>true if the instances are not equal, false otherwise.</returns>
        public static bool operator !=(NodalDegreeOfFreedom leftHandSide, NodalDegreeOfFreedom rightHandSide)
        {
            return !(leftHandSide == rightHandSide);
        }
        
        /// <summary>
        /// Determines whether this instance of <see cref="NodalDegreeOfFreedom" /> equals the other object.
        /// </summary>
        /// <param name="obj">The other object to compare this instance against.</param>
        /// <returns>True if the objects equal each other, false otherwise.</returns>
        public override bool Equals(object obj)
        {
            return (obj is NodalDegreeOfFreedom) && this.Equals((NodalDegreeOfFreedom)obj);
        }
        
        /// <summary>
        /// Determines whether this instance of <see cref="NodalDegreeOfFreedom" /> equals the other instance.
        /// </summary>
        /// <param name="other">The instance to compare this instance against.</param>
        /// <returns>true if the instances equal each other, false otherwise</returns>
        public bool Equals(NodalDegreeOfFreedom other)
        {
            return object.Equals(this.targetNode, other.targetNode) && this.dof == other.dof;
        }
        
        /// <summary>
        /// Calculates the hashcode for this instance.
        /// </summary>
        /// <returns>An integer representing the hashcode of this object</returns>
        public override int GetHashCode()
        {
            int hashCode = 0;
            unchecked
            {
                if (this.targetNode != null)
                {
                    hashCode += 1000000007 * this.targetNode.GetHashCode();
                }
                
                hashCode += 1000000009 * this.dof.GetHashCode();
            }
            
            return hashCode;
        }
        
        /// <summary>
        /// Overrides the ToString function to
        /// provide custom string representation of this object
        /// </summary>
        /// <returns>A string representation of the data in this object</returns>
        public override string ToString()
        {
            return string.Format(
                System.Globalization.CultureInfo.InvariantCulture,
                "[{0}, {1}]",
                this.targetNode,
                this.dof);
        }
        #endregion
    }
}
