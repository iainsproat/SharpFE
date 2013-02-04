//-----------------------------------------------------------------------
// <copyright file="LinearConstantStressQuadrilateral.cs" company="Iain Sproat">
//     Copyright Iain Sproat, 2012.
// </copyright>
//-----------------------------------------------------------------------

namespace SharpFE
{
    using System;
    using MathNet.Numerics.LinearAlgebra.Double;
    
    /// <summary>
    /// An implementation of Quad4
    /// </summary>
    public class LinearConstantStressQuadrilateral : FiniteElement, IHasMaterial
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="node0"></param>
        /// <param name="node1"></param>
        /// <param name="node2"></param>
        /// <param name="node3"></param>
        /// <param name="mat"></param>
        /// <param name="elementThickness"></param>
        public LinearConstantStressQuadrilateral(FiniteElementNode node0, FiniteElementNode node1, FiniteElementNode node2, FiniteElementNode node3, IMaterial mat, double elementThickness)
        {
            this.AddNode(node0);
            this.AddNode(node1);
            this.AddNode(node2);
            this.AddNode(node3);
            
            Guard.AgainstNullArgument(mat, "mat");
            Guard.AgainstBadArgument(
                () => { return elementThickness <= 0; },
                "thickness has to be greater than zero",
                "t");
            this.Material = mat;
            this.Thickness = elementThickness;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public IMaterial Material
        {
            get;
            private set;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public double Thickness
        {
            get;
            private set;
        }
        
        /// <summary>
        /// Gets or sets the vector representing the local x axis
        /// </summary>
        public override KeyedVector<DegreeOfFreedom> LocalXAxis
        {
            get
            {
                double initialLengthOfSide1ProjectedInXAxis = this.Nodes[1].X - this.Nodes[0].X;
                double initialLengthOfSide1ProjectedInYAxis = this.Nodes[1].Y - this.Nodes[0].Y;
                double initialLengthOfSide1ProjectedInZAxis = this.Nodes[1].Z - this.Nodes[0].Z;
                return new KeyedVector<DegreeOfFreedom>(
                    new double[]
                    {
                        initialLengthOfSide1ProjectedInXAxis,
                        initialLengthOfSide1ProjectedInYAxis,
                        initialLengthOfSide1ProjectedInZAxis
                    },
                    DegreeOfFreedom.X,
                    DegreeOfFreedom.Y,
                    DegreeOfFreedom.Z);
            }
        }
        
        /// <summary>
        /// Gets the vector representing the direction of the local y axis
        /// </summary>
        /// <remarks>
        /// Uses the right-angled vector from side1 to the third point as the Y-axis.
        /// </remarks>
        public override KeyedVector<DegreeOfFreedom> LocalYAxis
        {
            get
            {
                Vector result = GeometricHelpers.VectorBetweenPointAndLine(this.Nodes[3], this.Nodes[0], this.LocalXAxis);
                return new KeyedVector<DegreeOfFreedom>(result.Negate(), DegreeOfFreedom.X, DegreeOfFreedom.Y, DegreeOfFreedom.Z);
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public double Area
        {
            get
            {
                return GeometricHelpers.AreaQuadrilateral(this.Nodes[0], this.Nodes[1], this.Nodes[2], this.Nodes[3]);
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="degreeOfFreedom"></param>
        /// <returns></returns>
        public override bool IsASupportedBoundaryConditionDegreeOfFreedom(DegreeOfFreedom degreeOfFreedom)
        {
            switch (degreeOfFreedom)
            {
                case DegreeOfFreedom.X:
                case DegreeOfFreedom.Y:
                    return true;
                case DegreeOfFreedom.Z:
                case DegreeOfFreedom.XX:
                case DegreeOfFreedom.YY:
                case DegreeOfFreedom.ZZ:
                default:
                    return false;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodeToAdd"></param>
        protected override void ThrowIfNodeCannotBeAdded(FiniteElementNode nodeToAdd)
        {
            if (this.Nodes.Count > 3)
            {
                throw new ArgumentException("Cannot add more than 3 nodes");
            }
        }
    }
}
