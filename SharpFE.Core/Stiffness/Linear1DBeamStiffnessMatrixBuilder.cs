namespace SharpFE.Stiffness
{
    using System;

    public class Linear1DBeamStiffnessMatrixBuilder : ElementStiffnessMatrixBuilder<Linear1DBeam>
    {
        public Linear1DBeamStiffnessMatrixBuilder(Linear1DBeam finiteElement)
            : base(finiteElement)
        {
            // empty
        }
        
        public override KeyedRowColumnMatrix<DegreeOfFreedom, NodalDegreeOfFreedom> GetShapeFunctionVector(FiniteElementNode location)
        {
            throw new NotImplementedException();
        }
        
        public override KeyedRowColumnMatrix<Strain, NodalDegreeOfFreedom> GetStrainDisplacementMatrix()
        {
            throw new NotImplementedException();
        }
        
        public override StiffnessMatrix GetLocalStiffnessMatrix()
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
            
            double bendingStiffnessAboutYY = 4.0 * this.Element.BendingStiffnessEIy / length;
            matrix.At(this.Element.StartNode, DegreeOfFreedom.YY, this.Element.StartNode, DegreeOfFreedom.YY, bendingStiffnessAboutYY);
            matrix.At(this.Element.StartNode, DegreeOfFreedom.YY, this.Element.EndNode,   DegreeOfFreedom.YY, bendingStiffnessAboutYY / 2.0);
            matrix.At(this.Element.EndNode,   DegreeOfFreedom.YY, this.Element.StartNode, DegreeOfFreedom.YY, bendingStiffnessAboutYY / 2.0);
            matrix.At(this.Element.EndNode,   DegreeOfFreedom.YY, this.Element.EndNode,   DegreeOfFreedom.YY, bendingStiffnessAboutYY);
            
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