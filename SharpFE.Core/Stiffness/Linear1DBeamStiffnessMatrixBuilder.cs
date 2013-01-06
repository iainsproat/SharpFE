namespace SharpFE.Stiffness
{
	using System;

	public class Linear1DBeamStiffnessMatrixBuilder : StiffnessMatrixBuilder
	{
		public Linear1DBeamStiffnessMatrixBuilder(FiniteElement finiteElement)
			:base(finiteElement)
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
		
		public override ElementStiffnessMatrix GetLocalStiffnessMatrix()
		{
			Linear1DBeam beam = this.CastElementTo<Linear1DBeam>();
			
			IMaterial material = this.GetMaterial();
			ICrossSection section = this.GetCrossSection();
			
			double length = beam.OriginalLength;					
			ElementStiffnessMatrix matrix = new ElementStiffnessMatrix(this.Element.SupportedNodalDegreeOfFreedoms);
			
			double shearStiffnessInZ = 12.0 * material.YoungsModulus * section.SecondMomentOfAreaAroundYY / (length * length * length);
            matrix.At(beam.StartNode, DegreeOfFreedom.Z, beam.StartNode, DegreeOfFreedom.Z,  shearStiffnessInZ);
            matrix.At(beam.StartNode, DegreeOfFreedom.Z, beam.EndNode,   DegreeOfFreedom.Z, -shearStiffnessInZ);
            matrix.At(beam.EndNode,   DegreeOfFreedom.Z, beam.StartNode, DegreeOfFreedom.Z, -shearStiffnessInZ);
            matrix.At(beam.EndNode,   DegreeOfFreedom.Z, beam.EndNode,   DegreeOfFreedom.Z,  shearStiffnessInZ);
            
			double bendingStiffnessAboutYY = 4.0 * material.YoungsModulus * section.SecondMomentOfAreaAroundYY / length;
			double startToEndNodeRelationshipForBendingStiffnessAboutYY = 2.0 * material.YoungsModulus * section.SecondMomentOfAreaAroundYY / length;
			matrix.At(beam.StartNode, DegreeOfFreedom.YY, beam.StartNode, DegreeOfFreedom.YY, bendingStiffnessAboutYY);
            matrix.At(beam.StartNode, DegreeOfFreedom.YY, beam.EndNode,   DegreeOfFreedom.YY, startToEndNodeRelationshipForBendingStiffnessAboutYY);
            matrix.At(beam.EndNode,   DegreeOfFreedom.YY, beam.StartNode, DegreeOfFreedom.YY, startToEndNodeRelationshipForBendingStiffnessAboutYY);
            matrix.At(beam.EndNode,   DegreeOfFreedom.YY, beam.EndNode,   DegreeOfFreedom.YY, bendingStiffnessAboutYY);
            
			double bendingAboutYYShearInZ = 6.0 * material.YoungsModulus * section.SecondMomentOfAreaAroundYY / (length * length);
			matrix.At(beam.StartNode, DegreeOfFreedom.Z,  beam.StartNode, DegreeOfFreedom.YY, -bendingAboutYYShearInZ);
            matrix.At(beam.StartNode, DegreeOfFreedom.Z,  beam.EndNode,   DegreeOfFreedom.YY, -bendingAboutYYShearInZ);
            matrix.At(beam.EndNode,   DegreeOfFreedom.Z,  beam.StartNode, DegreeOfFreedom.YY,  bendingAboutYYShearInZ);
            matrix.At(beam.EndNode,   DegreeOfFreedom.Z,  beam.EndNode,   DegreeOfFreedom.YY,  bendingAboutYYShearInZ);
			matrix.At(beam.StartNode, DegreeOfFreedom.YY, beam.StartNode, DegreeOfFreedom.Z,  -bendingAboutYYShearInZ);
            matrix.At(beam.StartNode, DegreeOfFreedom.YY, beam.EndNode,   DegreeOfFreedom.Z,   bendingAboutYYShearInZ);
            matrix.At(beam.EndNode,   DegreeOfFreedom.YY, beam.StartNode, DegreeOfFreedom.Z,  -bendingAboutYYShearInZ);
            matrix.At(beam.EndNode,   DegreeOfFreedom.YY, beam.EndNode,   DegreeOfFreedom.Z,   bendingAboutYYShearInZ);
            
			return matrix;
		}
	}
}