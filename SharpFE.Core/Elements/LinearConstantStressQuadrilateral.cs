//-----------------------------------------------------------------------
// <copyright file="LinearConstantStressQuadrilateral.cs" company="Iain Sproat">
//     Copyright Iain Sproat, 2012.
// </copyright>
//-----------------------------------------------------------------------

namespace SharpFE
{
    using System;
    using System.Collections.Generic;
    using SharpFE.Geometry;
    
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
        public LinearConstantStressQuadrilateral(IFiniteElementNode node0, IFiniteElementNode node1, IFiniteElementNode node2, IFiniteElementNode node3, IMaterial mat, double elementThickness)
        {
            this.AddNode(node0);
            this.AddNode(node1);
            this.AddNode(node2);
            this.AddNode(node3);
            
            Guard.AgainstNullArgument(mat, "mat");
            Guard.AgainstBadArgument(
                "t",
                () => { return elementThickness <= 0; },
                "thickness has to be greater than zero");
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
        public override GeometricVector LocalXAxis
        {
            get
            {
                double initialLengthOfSide1ProjectedInXAxis = this.Nodes[1].X - this.LocalOrigin.X;
                double initialLengthOfSide1ProjectedInYAxis = this.Nodes[1].Y - this.LocalOrigin.Y;
                double initialLengthOfSide1ProjectedInZAxis = this.Nodes[1].Z - this.LocalOrigin.Z;
                return new GeometricVector(initialLengthOfSide1ProjectedInXAxis, initialLengthOfSide1ProjectedInYAxis, initialLengthOfSide1ProjectedInZAxis);
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
                return this.LocalXAxis.PerpendicularLineTo(this.LocalOrigin, this.Nodes[3].Location);
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
        public override IList<DegreeOfFreedom> SupportedBoundaryConditionDegreeOfFreedom
        {
            get
            {
                return new List<DegreeOfFreedom>
                {
                    DegreeOfFreedom.X,
                    DegreeOfFreedom.Y
                };
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodeToAdd"></param>
        protected override void ThrowIfNodeCannotBeAdded(IFiniteElementNode nodeToAdd)
        {
            if (this.Nodes.Count > 3)
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
                case ModelType.Membrane2D:
                    return true;
                case ModelType.Truss3D:
                    return false;
                case ModelType.Membrane3D:
                    return true;
                case ModelType.MultiStorey2DSlab:
                    return true;
                case ModelType.Full3D:
                    return true;
                default:
                    throw new NotImplementedException(string.Format(
                        System.Globalization.CultureInfo.InvariantCulture,
                        "LinearConstantStressQuadrilateral.IsSupportedModelType(ModelType) has not been defined for a model type of {0}",
                        modelType));
            }
        }
    }
}
