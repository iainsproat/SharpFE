//-----------------------------------------------------------------------
// <copyright file="Linear1DBeamStiffnessMatrixBuilder.cs" company="SharpFE">
//     Copyright Iain Sproat, 2012 - 2013.
// </copyright>
//-----------------------------------------------------------------------

namespace SharpFE.Stiffness
{
    using System;
    using System.Collections.Generic;
    
    using MathNet.Numerics.LinearAlgebra.Double;

    /// <summary>
    /// 
    /// </summary>
    public class Linear1DBernoulliBeamStiffnessMatrixBuilder : ElementStiffnessMatrixBuilder<Linear1DBeam>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="finiteElement"></param>
        public Linear1DBernoulliBeamStiffnessMatrixBuilder(Linear1DBeam finiteElement)
            : base(finiteElement)
        {
            // empty
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public override DenseVector ShapeFunctions(XYZ locationInLocalCoordinates)
        {
            Guard.AgainstNullArgument(locationInLocalCoordinates, "locationInLocalCoordinates");
            double xi = locationInLocalCoordinates.X;
            Guard.AgainstBadArgument("locationInLocalCoordinates",
                                     () => { return xi < -1 || xi > 1; },
                                     "location in local coordinates must be within the boundary -1 to +1. You provided {0}", locationInLocalCoordinates);
            
            double N1 = 0.5 * (1 - xi);
            double N2 = 0.5 * (1 + xi);
            
            DenseVector shapeFunctions = new DenseVector(2);
            shapeFunctions[0] = N1;
            shapeFunctions[1] = N2;
            return shapeFunctions;
        }
        
        public override DenseMatrix ShapeFunctionFirstDerivatives(XYZ locationInLocalCoordinates)
        {
            throw new NotImplementedException("Linear1DBernoulliBeamStiffnessMatrixBuilder.ShapeFunctionDerivatives");
        }
        
        private double ConvertLocalCoordinatesToNaturalCoordinate(XYZ locationInLocalCoordinates)
        {
            ////TODO check that the location lies on the beam
            
            double beamLength = this.Element.OriginalLength;
            double x = locationInLocalCoordinates.X;
            double eta = ((2 * x) / beamLength) - 1; // in natural coordinates
            return eta;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override KeyedRowColumnMatrix<Strain, NodalDegreeOfFreedom> StrainDisplacementMatrix(XYZ locationInLocalCoordinates)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override StiffnessMatrix LocalStiffnessMatrix()
        {
            double length = this.Element.OriginalLength;
            StiffnessMatrix matrix = new StiffnessMatrix(this.Element.SupportedLocalNodalDegreeOfFreedoms);
            
            double axialStiffness = this.Element.StiffnessEA / length;
            matrix.At(this.Element.StartNode, DegreeOfFreedom.X, this.Element.StartNode, DegreeOfFreedom.X, axialStiffness);
            matrix.At(this.Element.StartNode, DegreeOfFreedom.X, this.Element.EndNode, DegreeOfFreedom.X, -axialStiffness);
            matrix.At(this.Element.EndNode, DegreeOfFreedom.X, this.Element.StartNode, DegreeOfFreedom.X, -axialStiffness);
            matrix.At(this.Element.EndNode, DegreeOfFreedom.X, this.Element.EndNode, DegreeOfFreedom.X, axialStiffness);
            
            double shearStiffnessInZ = 12.0 * this.Element.BendingStiffnessEIy / (length * length * length);
            matrix.At(this.Element.StartNode, DegreeOfFreedom.Z, this.Element.StartNode, DegreeOfFreedom.Z,  shearStiffnessInZ);
            matrix.At(this.Element.StartNode, DegreeOfFreedom.Z, this.Element.EndNode,   DegreeOfFreedom.Z, -shearStiffnessInZ);
            matrix.At(this.Element.EndNode,   DegreeOfFreedom.Z, this.Element.StartNode, DegreeOfFreedom.Z, -shearStiffnessInZ);
            matrix.At(this.Element.EndNode,   DegreeOfFreedom.Z, this.Element.EndNode,   DegreeOfFreedom.Z,  shearStiffnessInZ);
            
            double bendingStiffnessAboutYY = 2.0 * this.Element.BendingStiffnessEIy / length;
            matrix.At(this.Element.StartNode, DegreeOfFreedom.YY, this.Element.StartNode, DegreeOfFreedom.YY, 2.0 * bendingStiffnessAboutYY);
            matrix.At(this.Element.StartNode, DegreeOfFreedom.YY, this.Element.EndNode,   DegreeOfFreedom.YY, bendingStiffnessAboutYY);
            matrix.At(this.Element.EndNode,   DegreeOfFreedom.YY, this.Element.StartNode, DegreeOfFreedom.YY, bendingStiffnessAboutYY);
            matrix.At(this.Element.EndNode,   DegreeOfFreedom.YY, this.Element.EndNode,   DegreeOfFreedom.YY, 2.0 * bendingStiffnessAboutYY);
            
            double bendingAboutYYShearInZ = 6.0 * this.Element.BendingStiffnessEIy / (length * length);
            matrix.At(this.Element.StartNode, DegreeOfFreedom.Z,  this.Element.StartNode, DegreeOfFreedom.YY, -bendingAboutYYShearInZ);
            matrix.At(this.Element.StartNode, DegreeOfFreedom.Z,  this.Element.EndNode,   DegreeOfFreedom.YY, -bendingAboutYYShearInZ);
            matrix.At(this.Element.EndNode,   DegreeOfFreedom.Z,  this.Element.StartNode, DegreeOfFreedom.YY,  bendingAboutYYShearInZ);
            matrix.At(this.Element.EndNode,   DegreeOfFreedom.Z,  this.Element.EndNode,   DegreeOfFreedom.YY,  bendingAboutYYShearInZ);
            matrix.At(this.Element.StartNode, DegreeOfFreedom.YY, this.Element.StartNode, DegreeOfFreedom.Z,  -bendingAboutYYShearInZ);
            matrix.At(this.Element.StartNode, DegreeOfFreedom.YY, this.Element.EndNode,   DegreeOfFreedom.Z,   bendingAboutYYShearInZ);
            matrix.At(this.Element.EndNode,   DegreeOfFreedom.YY, this.Element.StartNode, DegreeOfFreedom.Z,  -bendingAboutYYShearInZ);
            matrix.At(this.Element.EndNode,   DegreeOfFreedom.YY, this.Element.EndNode,   DegreeOfFreedom.Z,   bendingAboutYYShearInZ);
            
            return matrix;
        }
    }
}