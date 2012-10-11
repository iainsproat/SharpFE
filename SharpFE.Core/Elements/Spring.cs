//-----------------------------------------------------------------------
// <copyright file="Spring.cs" company="SharpFE">
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
    /// Description of Spring.
    /// </summary>
    public class Spring : FiniteElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Spring" /> class.
        /// </summary>
        /// <param name="node1">The node at the start of the spring.</param>
        /// <param name="node2">The node at the end of the spring.</param>
        /// <param name="springConstant">The value which defines the constant stiffness of the spring.</param>
        internal Spring(FiniteElementNode node1, FiniteElementNode node2, double springConstant)
            : base()
        {
            this.AddNode(node1);
            this.AddNode(node2);
            this.Stiffness = springConstant;
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
        
        /// <summary>
        /// Gets the constant value determining the stiffness of this spring.
        /// </summary>
        public double Stiffness
        {
            get;
            private set;
        }
        
        /// <summary>
        /// Gets or sets the vector representing the local x axis
        /// </summary>
        protected override Vector LocalXAxis
        {
            get
            {
                double initialLengthProjectedInXAxis = this.EndNode.OriginalX - this.StartNode.OriginalX;
                double initialLengthProjectedInYAxis = this.EndNode.OriginalY - this.StartNode.OriginalY;
                double initialLengthProjectedInZAxis = this.EndNode.OriginalZ - this.StartNode.OriginalZ;
                return new DenseVector(new double[]
                                                {
                                                    initialLengthProjectedInXAxis,
                                                    initialLengthProjectedInYAxis,
                                                    initialLengthProjectedInZAxis
                                                });
            }
        }
        
        /// <summary>
        /// Gets or sets the vector representing the direction of the local y axis
        /// </summary>
        protected override Vector LocalYAxis
        {
            get
            {
                return (Vector)this.UpDirection().CrossProduct(this.LocalXAxis);
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
        protected override void ThrowIfNodeCannotBeAdded(FiniteElementNode nodeToAdd)
        {
            if (this.Nodes.Count > 1)
            {
                throw new ArgumentException("Cannot add more than 2 nodes");
            }
            
            if (this.Nodes.Contains(nodeToAdd))
            {
                throw new ArgumentException("Node is already part of this element");
            }
            
            // TODO check for proximity of nodes
        }
        
        /// <summary>
        /// Generates the stiffness matrix for this element
        /// </summary>
        protected override void BuildLocalStiffnessMatrix()
        {
            this.LocalStiffnessMatrix.At(this.StartNode, DegreeOfFreedom.X, this.StartNode, DegreeOfFreedom.X, this.Stiffness);
            this.LocalStiffnessMatrix.At(this.StartNode, DegreeOfFreedom.X, this.EndNode, DegreeOfFreedom.X, -this.Stiffness);
            this.LocalStiffnessMatrix.At(this.EndNode, DegreeOfFreedom.X, this.StartNode, DegreeOfFreedom.X, -this.Stiffness);
            this.LocalStiffnessMatrix.At(this.EndNode, DegreeOfFreedom.X, this.EndNode, DegreeOfFreedom.X, this.Stiffness);
        }
        
        /// <summary>
        /// Lines are unique in that you cannot work out which way is 'up' from the nodes (could be any of an infinte number as the line spins on its own axis).
        /// This method defines an 'up' direction.  It is in the direction of the Global Z, unless the line local X axis is also aligned with global z.
        /// In that case the up direction is aligned with global y axis.
        /// </summary>
        /// <returns>A vector representing the 'up' direction.</returns>
        private Vector UpDirection() // HACK
        {
            if (this.LocalXAxis[0] == 0 && this.LocalXAxis[1] == 0 && (this.LocalXAxis[2] == 1 || this.LocalXAxis[2] == -1))
            {
                // localXAxis is in global Z axis direction which will give poor results so assume the local upright direction is in global -x direction
                return new DenseVector(new double[] { -1, 0, 0 });
            }
            else
            {
                // make the global vertical the default
                return new DenseVector(new double[] { 0, 0, 1 });
            }
        }
    }
}
