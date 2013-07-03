//-----------------------------------------------------------------------
// <copyright file="Linear1DBeamStiffnessMatrixBuilder.cs" company="SharpFE">
//     Copyright Iain Sproat, 2012 - 2013.
// </copyright>
//-----------------------------------------------------------------------

namespace SharpFE.Stiffness
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    public class Linear1DBernoulliBeamStiffnessMatrixBuilder : ElementStiffnessMatrixBuilder<Linear1DBeam>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="finiteElement"></param>
        public Linear1DBeamStiffnessMatrixBuilder(Linear1DBeam finiteElement)
            : base(finiteElement)
        {
            // empty
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public override KeyedRowColumnMatrix<DegreeOfFreedom, NodalDegreeOfFreedom> ShapeFunctionVector(FiniteElementNode locationInLocalCoordinates)
        {
            double eta = this.ConvertLocalCoordinatesToNaturalCoordinate(locationInLocalCoordinates);
            
            double N1 = 0.5 * (1 - eta);
            double N2 = 0.5 * (1 + eta);
            
            IFiniteElementNode start = this.Element.StartNode;
            IFiniteElementNode end = this.Element.EndNode;
            IList<DegreeOfFreedom> supportedDegreesOfFreedom = new List<DegreeOfFreedom>(1){ DegreeOfFreedom.X };
            IList<NodalDegreeOfFreedom> supportedNodalDegreesOfFreedom = this.Element.SupportedNodalDegreeOfFreedoms;
            KeyedRowColumnMatrix<DegreeOfFreedom, NodalDegreeOfFreedom> shapeFunctions = new KeyedRowColumnMatrix<DegreeOfFreedom, NodalDegreeOfFreedom>(supportedDegreesOfFreedom, supportedNodalDegreesOfFreedom);
            shapeFunctions.At(DegreeOfFreedom.X, new NodalDegreeOfFreedom(start, DegreeOfFreedom.X), N1);
            shapeFunctions.At(DegreeOfFreedom.X, new NodalDegreeOfFreedom(end, DegreeOfFreedom.X), N2);
            return shapeFunctions;
        }
        
        private double ConvertLocalCoordinatesToNaturalCoordinate(FiniteElementNode locationInLocalCoordinates)
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
        public override KeyedRowColumnMatrix<Strain, NodalDegreeOfFreedom> StrainDisplacementMatrix(FiniteElementNode location)
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
            StiffnessMatrix matrix = new StiffnessMatrix(this.Element.SupportedNodalDegreeOfFreedoms);
            
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