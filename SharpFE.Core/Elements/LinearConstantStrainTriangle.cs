//-----------------------------------------------------------------------
// <copyright file="LinearConstantStrainTriangle.cs" company="Iain Sproat">
//     Copyright Iain Sproat, 2012.
// </copyright>
//-----------------------------------------------------------------------

namespace SharpFE
{
    using System;
    using MathNet.Numerics.LinearAlgebra.Double;

    /// <summary>
    /// Triangular shaped element which calculates membrane forces only
    /// </summary>
    public class LinearConstantStrainTriangle : FiniteElement, IHasMaterial
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="node0"></param>
        /// <param name="node1"></param>
        /// <param name="node2"></param>
        /// <param name="elementMaterial"></param>
        /// <param name="elementThickness"></param>
        public LinearConstantStrainTriangle(FiniteElementNode node0, FiniteElementNode node1, FiniteElementNode node2, IMaterial elementMaterial, double elementThickness)
        {
            this.AddNode(node0);
            this.AddNode(node1);
            this.AddNode(node2);
            
            Guard.AgainstNullArgument(elementMaterial, "elementMaterial");
            Guard.AgainstBadArgument(
                () => { return elementThickness <= 0; },
                "thickness has to be greater than zero",
                "elementThickness");
            this.Material = elementMaterial;
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
                double initialLengthOfSide1ProjectedInXAxis = this.Nodes[1].OriginalX - this.Nodes[0].OriginalX;
                double initialLengthOfSide1ProjectedInYAxis = this.Nodes[1].OriginalY - this.Nodes[0].OriginalY;
                double initialLengthOfSide1ProjectedInZAxis = this.Nodes[1].OriginalZ - this.Nodes[0].OriginalZ;
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
                Vector result = GeometricHelpers.VectorBetweenPointAndLine(this.Nodes[2], this.Nodes[0], this.LocalXAxis);
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
                return GeometricHelpers.AreaTriangle(this.Nodes[0], this.Nodes[1], this.Nodes[2]);
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
            if (this.Nodes.Count > 2)
            {
                throw new ArgumentException("Cannot add more than 3 nodes");
            }
        }
    }
}
