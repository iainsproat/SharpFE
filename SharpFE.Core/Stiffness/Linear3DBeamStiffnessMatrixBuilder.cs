

namespace SharpFE.Stiffness
{
	using System;
	
	/// <summary>
	/// Description of Linear3DBeamStiffnessMatrixBuilder.
	/// </summary>
	public class Linear3DBeamStiffnessMatrixBuilder : StiffnessMatrixBuilder
	{
		public Linear3DBeamStiffnessMatrixBuilder(FiniteElement finiteElement)
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
			throw new NotImplementedException("Linear3DBeamStiffnessMatrixBuilder.GetStrainDisplacementMatrix");
		}
		
		public override ElementStiffnessMatrix GetLocalStiffnessMatrix()
		{
			Linear3DBeam beam = this.CastElementTo<Linear3DBeam>();
			
			IMaterial material = this.GetMaterial();
			ICrossSection section = this.GetCrossSection();
			
			double length = beam.OriginalLength;					
			ElementStiffnessMatrix matrix = new ElementStiffnessMatrix(this.Element.SupportedNodalDegreeOfFreedoms);
			
			double axialStiffness = section.Area * material.YoungsModulus / length;
            matrix.At(beam.StartNode, DegreeOfFreedom.X, beam.StartNode, DegreeOfFreedom.X,  axialStiffness);
            matrix.At(beam.StartNode, DegreeOfFreedom.X, beam.EndNode,   DegreeOfFreedom.X, -axialStiffness);
            matrix.At(beam.EndNode,   DegreeOfFreedom.X, beam.StartNode, DegreeOfFreedom.X, -axialStiffness);
            matrix.At(beam.EndNode,   DegreeOfFreedom.X, beam.EndNode,   DegreeOfFreedom.X,  axialStiffness);
            
            
			double shearStiffnessInY = 12.0 * material.YoungsModulus * section.SecondMomentOfAreaAroundZZ / (length * length * length);
			matrix.At(beam.StartNode, DegreeOfFreedom.Y, beam.StartNode, DegreeOfFreedom.Y,  shearStiffnessInY);
            matrix.At(beam.StartNode, DegreeOfFreedom.Y, beam.EndNode,   DegreeOfFreedom.Y, -shearStiffnessInY);
            matrix.At(beam.EndNode,   DegreeOfFreedom.Y, beam.StartNode, DegreeOfFreedom.Y, -shearStiffnessInY);
            matrix.At(beam.EndNode,   DegreeOfFreedom.Y, beam.EndNode,   DegreeOfFreedom.Y,  shearStiffnessInY);
            
            
			double shearStiffnessInZ = 12.0 * material.YoungsModulus * section.SecondMomentOfAreaAroundYY / (length * length * length);
            matrix.At(beam.StartNode, DegreeOfFreedom.Z, beam.StartNode, DegreeOfFreedom.Z,  shearStiffnessInZ);
            matrix.At(beam.StartNode, DegreeOfFreedom.Z, beam.EndNode,   DegreeOfFreedom.Z, -shearStiffnessInZ);
            matrix.At(beam.EndNode,   DegreeOfFreedom.Z, beam.StartNode, DegreeOfFreedom.Z, -shearStiffnessInZ);
            matrix.At(beam.EndNode,   DegreeOfFreedom.Z, beam.EndNode,   DegreeOfFreedom.Z,  shearStiffnessInZ);
            
            
			double torsionalStiffness = material.ShearModulusElasticity * section.MomentOfInertiaInTorsion / length;
			matrix.At(beam.StartNode, DegreeOfFreedom.XX, beam.StartNode, DegreeOfFreedom.XX,  torsionalStiffness);
            matrix.At(beam.StartNode, DegreeOfFreedom.XX, beam.EndNode,   DegreeOfFreedom.XX, -torsionalStiffness);
            matrix.At(beam.EndNode,   DegreeOfFreedom.XX, beam.StartNode, DegreeOfFreedom.XX, -torsionalStiffness);
            matrix.At(beam.EndNode,   DegreeOfFreedom.XX, beam.EndNode,   DegreeOfFreedom.XX,  torsionalStiffness);
            
			double bendingStiffnessAboutYY = 4.0 * material.YoungsModulus * section.SecondMomentOfAreaAroundYY / length;
			double startToEndNodeRelationshipForBendingStiffnessAboutYY = 2.0 * material.YoungsModulus * section.SecondMomentOfAreaAroundYY / length;
			matrix.At(beam.StartNode, DegreeOfFreedom.YY, beam.StartNode, DegreeOfFreedom.YY, bendingStiffnessAboutYY);
            matrix.At(beam.StartNode, DegreeOfFreedom.YY, beam.EndNode,   DegreeOfFreedom.YY, startToEndNodeRelationshipForBendingStiffnessAboutYY);
            matrix.At(beam.EndNode,   DegreeOfFreedom.YY, beam.StartNode, DegreeOfFreedom.YY, startToEndNodeRelationshipForBendingStiffnessAboutYY);
            matrix.At(beam.EndNode,   DegreeOfFreedom.YY, beam.EndNode,   DegreeOfFreedom.YY, bendingStiffnessAboutYY);
            
			double bendingStiffnessAboutZZ = 4.0 * material.YoungsModulus * section.SecondMomentOfAreaAroundZZ / length;
			double startToEndNodeRelationshipForBendingStiffnessAboutZZ = 2.0 * material.YoungsModulus * section.SecondMomentOfAreaAroundZZ / length;
			matrix.At(beam.StartNode, DegreeOfFreedom.ZZ, beam.StartNode, DegreeOfFreedom.ZZ, bendingStiffnessAboutZZ);
            matrix.At(beam.StartNode, DegreeOfFreedom.ZZ, beam.EndNode,   DegreeOfFreedom.ZZ, startToEndNodeRelationshipForBendingStiffnessAboutZZ);
            matrix.At(beam.EndNode,   DegreeOfFreedom.ZZ, beam.StartNode, DegreeOfFreedom.ZZ, startToEndNodeRelationshipForBendingStiffnessAboutZZ);
            matrix.At(beam.EndNode,   DegreeOfFreedom.ZZ, beam.EndNode,   DegreeOfFreedom.ZZ, bendingStiffnessAboutZZ);
            
			double bendingAboutZZshearInY = 6.0 * material.YoungsModulus * section.SecondMomentOfAreaAroundZZ / (length * length);
			matrix.At(beam.StartNode, DegreeOfFreedom.Y,  beam.StartNode, DegreeOfFreedom.ZZ,  bendingAboutZZshearInY);
            matrix.At(beam.StartNode, DegreeOfFreedom.Y,  beam.EndNode,   DegreeOfFreedom.ZZ,  bendingAboutZZshearInY);
            matrix.At(beam.EndNode,   DegreeOfFreedom.Y,  beam.StartNode, DegreeOfFreedom.ZZ, -bendingAboutZZshearInY);
            matrix.At(beam.EndNode,   DegreeOfFreedom.Y,  beam.EndNode,   DegreeOfFreedom.ZZ, -bendingAboutZZshearInY);
			matrix.At(beam.StartNode, DegreeOfFreedom.ZZ, beam.StartNode, DegreeOfFreedom.Y,   bendingAboutZZshearInY);
            matrix.At(beam.StartNode, DegreeOfFreedom.ZZ, beam.EndNode,   DegreeOfFreedom.Y,  -bendingAboutZZshearInY);
            matrix.At(beam.EndNode,   DegreeOfFreedom.ZZ, beam.StartNode, DegreeOfFreedom.Y,   bendingAboutZZshearInY);
            matrix.At(beam.EndNode,   DegreeOfFreedom.ZZ, beam.EndNode,   DegreeOfFreedom.Y,  -bendingAboutZZshearInY);
            
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
