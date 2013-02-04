//-----------------------------------------------------------------------
// <copyright file="Linear3DBeamStiffnessMatrixBuilder.cs" company="SharpFE">
//     Copyright Iain Sproat, 2012 - 2013.
// </copyright>
//-----------------------------------------------------------------------

namespace SharpFE.Stiffness
{
    using System;
    using System.Collections.Generic;
    
    /// <summary>
    /// </summary>
    public class Linear3DBeamStiffnessMatrixBuilder : ElementStiffnessMatrixBuilder<Linear3DBeam>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="finiteElement"></param>
        public Linear3DBeamStiffnessMatrixBuilder(Linear3DBeam finiteElement)
            : base(finiteElement)
        {
            // empty
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public override KeyedRowColumnMatrix<DegreeOfFreedom, NodalDegreeOfFreedom> ShapeFunctionVector(FiniteElementNode location)
        {
            IFiniteElementNode start = this.Element.StartNode;
            IFiniteElementNode end = this.Element.EndNode;
            double locationAlongBeamAsProjectedInGlobalXAxis = location.X - start.X;
            double locationAlongBeamAsProjectedInGlobalYAxis = location.Y - start.Y;
            double x = Math.Sqrt((locationAlongBeamAsProjectedInGlobalXAxis * locationAlongBeamAsProjectedInGlobalXAxis) + (locationAlongBeamAsProjectedInGlobalYAxis * locationAlongBeamAsProjectedInGlobalYAxis));
            
            double beamLengthProjectedInGlobalXAxis = end.X - start.X;
            double beamLengthProjectedInGlobalYAxis = end.Y - start.Y;
            double beamLength = Math.Sqrt((beamLengthProjectedInGlobalXAxis * beamLengthProjectedInGlobalXAxis) + (beamLengthProjectedInGlobalYAxis * beamLengthProjectedInGlobalYAxis));
            
            ////TODO check that the location is on the line
            
            double N1 = 1 - ((3 * x * x) / (beamLength * beamLength)) + ((2 * x * x * x) / (beamLength * beamLength * beamLength));
            double N2 = x - ((2 * x * x) / beamLength) + ((x * x * x) / (beamLength * beamLength)); ////FIXME the angle might be reversed (i.e. this is positive for anti-clockwise rotation, rather than for clockwise)
            double N3 = ((3 * x * x) / (beamLength * beamLength)) - ((2 * x * x) / (beamLength * beamLength * beamLength));
            double N4 = ((x * x * x) / (beamLength * beamLength)) - ((x * x) / beamLength); ////FIXME the angle might be reversed (i.e. this is positive for anti-clockwise rotation, rather than for clockwise)
            
            IList<DegreeOfFreedom> supportedDegreesOfFreedom = new List<DegreeOfFreedom>(1){ DegreeOfFreedom.X };
            IList<NodalDegreeOfFreedom> supportedNodalDegreesOfFreedom = this.Element.SupportedNodalDegreeOfFreedoms;
            KeyedRowColumnMatrix<DegreeOfFreedom, NodalDegreeOfFreedom> shapeFunctions = new KeyedRowColumnMatrix<DegreeOfFreedom, NodalDegreeOfFreedom>(supportedDegreesOfFreedom, supportedNodalDegreesOfFreedom);
            
            shapeFunctions.At( DegreeOfFreedom.X, new NodalDegreeOfFreedom(start, DegreeOfFreedom.Z), N1);
            shapeFunctions.At(DegreeOfFreedom.X, new NodalDegreeOfFreedom(start, DegreeOfFreedom.YY), N2);
            shapeFunctions.At(DegreeOfFreedom.X, new NodalDegreeOfFreedom(end, DegreeOfFreedom.Z), N3);
            shapeFunctions.At(DegreeOfFreedom.X, new NodalDegreeOfFreedom(end, DegreeOfFreedom.YY), N4);
            
            return shapeFunctions;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override KeyedRowColumnMatrix<Strain, NodalDegreeOfFreedom> StrainDisplacementMatrix(FiniteElementNode location)
        {
            throw new NotImplementedException("Linear3DBeamStiffnessMatrixBuilder.GetStrainDisplacementMatrix");
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override StiffnessMatrix LocalStiffnessMatrix()
        {
            double length = this.Element.OriginalLength;
            StiffnessMatrix matrix = new StiffnessMatrix(this.Element.SupportedNodalDegreeOfFreedoms);
            
            double axialStiffness = this.Element.CrossSection.Area * this.Element.Material.YoungsModulus / length;
            matrix.At(this.Element.StartNode, DegreeOfFreedom.X, this.Element.StartNode, DegreeOfFreedom.X,  axialStiffness);
            matrix.At(this.Element.StartNode, DegreeOfFreedom.X, this.Element.EndNode,   DegreeOfFreedom.X, -axialStiffness);
            matrix.At(this.Element.EndNode,   DegreeOfFreedom.X, this.Element.StartNode, DegreeOfFreedom.X, -axialStiffness);
            matrix.At(this.Element.EndNode,   DegreeOfFreedom.X, this.Element.EndNode,   DegreeOfFreedom.X,  axialStiffness);
            
            double shearStiffnessInY = 12.0 * this.Element.Material.YoungsModulus * this.Element.CrossSection.SecondMomentOfAreaAroundZZ / (length * length * length);
            matrix.At(this.Element.StartNode, DegreeOfFreedom.Y, this.Element.StartNode, DegreeOfFreedom.Y,  shearStiffnessInY);
            matrix.At(this.Element.StartNode, DegreeOfFreedom.Y, this.Element.EndNode,   DegreeOfFreedom.Y, -shearStiffnessInY);
            matrix.At(this.Element.EndNode,   DegreeOfFreedom.Y, this.Element.StartNode, DegreeOfFreedom.Y, -shearStiffnessInY);
            matrix.At(this.Element.EndNode,   DegreeOfFreedom.Y, this.Element.EndNode,   DegreeOfFreedom.Y,  shearStiffnessInY);
            
            double shearStiffnessInZ = 12.0 * this.Element.Material.YoungsModulus * this.Element.CrossSection.SecondMomentOfAreaAroundYY / (length * length * length);
            matrix.At(this.Element.StartNode, DegreeOfFreedom.Z, this.Element.StartNode, DegreeOfFreedom.Z,  shearStiffnessInZ);
            matrix.At(this.Element.StartNode, DegreeOfFreedom.Z, this.Element.EndNode,   DegreeOfFreedom.Z, -shearStiffnessInZ);
            matrix.At(this.Element.EndNode,   DegreeOfFreedom.Z, this.Element.StartNode, DegreeOfFreedom.Z, -shearStiffnessInZ);
            matrix.At(this.Element.EndNode,   DegreeOfFreedom.Z, this.Element.EndNode,   DegreeOfFreedom.Z,  shearStiffnessInZ);
            
            double torsionalStiffness = this.Element.Material.ShearModulusElasticity * this.Element.CrossSection.MomentOfInertiaInTorsion / length;
            matrix.At(this.Element.StartNode, DegreeOfFreedom.XX, this.Element.StartNode, DegreeOfFreedom.XX,  torsionalStiffness);
            matrix.At(this.Element.StartNode, DegreeOfFreedom.XX, this.Element.EndNode,   DegreeOfFreedom.XX, -torsionalStiffness);
            matrix.At(this.Element.EndNode,   DegreeOfFreedom.XX, this.Element.StartNode, DegreeOfFreedom.XX, -torsionalStiffness);
            matrix.At(this.Element.EndNode,   DegreeOfFreedom.XX, this.Element.EndNode,   DegreeOfFreedom.XX,  torsionalStiffness);
            
            double bendingStiffnessAboutYY = 4.0 * this.Element.Material.YoungsModulus * this.Element.CrossSection.SecondMomentOfAreaAroundYY / length;
            matrix.At(this.Element.StartNode, DegreeOfFreedom.YY, this.Element.StartNode, DegreeOfFreedom.YY, bendingStiffnessAboutYY);
            matrix.At(this.Element.StartNode, DegreeOfFreedom.YY, this.Element.EndNode,   DegreeOfFreedom.YY, bendingStiffnessAboutYY / 2.0);
            matrix.At(this.Element.EndNode,   DegreeOfFreedom.YY, this.Element.StartNode, DegreeOfFreedom.YY, bendingStiffnessAboutYY / 2.0);
            matrix.At(this.Element.EndNode,   DegreeOfFreedom.YY, this.Element.EndNode,   DegreeOfFreedom.YY, bendingStiffnessAboutYY);
            
            double bendingStiffnessAboutZZ = 4.0 * this.Element.Material.YoungsModulus * this.Element.CrossSection.SecondMomentOfAreaAroundZZ / length;
            matrix.At(this.Element.StartNode, DegreeOfFreedom.ZZ, this.Element.StartNode, DegreeOfFreedom.ZZ, bendingStiffnessAboutZZ);
            matrix.At(this.Element.StartNode, DegreeOfFreedom.ZZ, this.Element.EndNode,   DegreeOfFreedom.ZZ, bendingStiffnessAboutZZ / 2.0);
            matrix.At(this.Element.EndNode,   DegreeOfFreedom.ZZ, this.Element.StartNode, DegreeOfFreedom.ZZ, bendingStiffnessAboutZZ / 2.0);
            matrix.At(this.Element.EndNode,   DegreeOfFreedom.ZZ, this.Element.EndNode,   DegreeOfFreedom.ZZ, bendingStiffnessAboutZZ);
            
            double bendingAboutZZshearInY = 6.0 * this.Element.Material.YoungsModulus * this.Element.CrossSection.SecondMomentOfAreaAroundZZ / (length * length);
            matrix.At(this.Element.StartNode, DegreeOfFreedom.Y,  this.Element.StartNode, DegreeOfFreedom.ZZ,  bendingAboutZZshearInY);
            matrix.At(this.Element.StartNode, DegreeOfFreedom.Y,  this.Element.EndNode,   DegreeOfFreedom.ZZ,  bendingAboutZZshearInY);
            matrix.At(this.Element.EndNode,   DegreeOfFreedom.Y,  this.Element.StartNode, DegreeOfFreedom.ZZ, -bendingAboutZZshearInY);
            matrix.At(this.Element.EndNode,   DegreeOfFreedom.Y,  this.Element.EndNode,   DegreeOfFreedom.ZZ, -bendingAboutZZshearInY);
            matrix.At(this.Element.StartNode, DegreeOfFreedom.ZZ, this.Element.StartNode, DegreeOfFreedom.Y,   bendingAboutZZshearInY);
            matrix.At(this.Element.StartNode, DegreeOfFreedom.ZZ, this.Element.EndNode,   DegreeOfFreedom.Y,  -bendingAboutZZshearInY);
            matrix.At(this.Element.EndNode,   DegreeOfFreedom.ZZ, this.Element.StartNode, DegreeOfFreedom.Y,   bendingAboutZZshearInY);
            matrix.At(this.Element.EndNode,   DegreeOfFreedom.ZZ, this.Element.EndNode,   DegreeOfFreedom.Y,  -bendingAboutZZshearInY);
            
            double bendingAboutYYShearInZ = 6.0 * this.Element.Material.YoungsModulus * this.Element.CrossSection.SecondMomentOfAreaAroundYY / (length * length);
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
