//-----------------------------------------------------------------------
// <copyright file="LinearConstantStrainTriangle.cs" company="Iain Sproat">
//     Copyright Iain Sproat, 2012.
// </copyright>
//-----------------------------------------------------------------------

namespace SharpFE
{
    using System;
    using SharpFE.Geometry;

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
                "elementThickness",
                () => { return elementThickness <= 0; },
                "thickness has to be greater than zero");
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
        public override GeometricVector LocalXAxis
        {
            get
            {
                double initialLengthOfSide1ProjectedInXAxis = this.Nodes[1].X - this.LocalOrigin.X;
                double initialLengthOfSide1ProjectedInYAxis = this.Nodes[1].Y - this.LocalOrigin.Y;
                double initialLengthOfSide1ProjectedInZAxis = this.Nodes[1].Z - this.LocalOrigin.Z;
                return new GeometricVector(initialLengthOfSide1ProjectedInXAxis,
                                           initialLengthOfSide1ProjectedInYAxis,
                                           initialLengthOfSide1ProjectedInZAxis);
            }
        }
        
        /// <summary>
        /// Gets the vector representing the direction of the local y axis
        /// </summary>
        /// <remarks>
        /// Uses the right-angled vector from side1 to the third point as the Y-axis.
        /// </remarks>
        public override GeometricVector LocalYAxis
        {
            get
            {
                GeometricVector result = this.LocalXAxis.PerpendicularLineTo(this.LocalOrigin, this.Nodes[2].Location);
                return result;
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
        
        public static bool IsASupportedModelType(ModelType modelType)
        {
            switch(modelType)
            {
               case ModelType.Truss1D:
                    return false;
                case ModelType.Beam1D:
                    return false;
                case ModelType.Truss2D:
                    return false;
                case ModelType.Frame2D:
                    return false;
                case ModelType.Slab2D:
                    return true;
                case ModelType.Truss3D:
                    return false;
                case ModelType.MultiStorey2DSlab:
                    return true;
                case ModelType.Full3D:
                    return true;
                default:
                    throw new NotImplementedException(string.Format(
                        System.Globalization.CultureInfo.InvariantCulture,
                        "LinearConstantStrainTriangle.IsSupportedModelType(ModelType) has not been defined for a model type of {0}",
                        modelType));
            }
        }
    }
}
