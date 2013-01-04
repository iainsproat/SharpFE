

namespace SharpFE.Stiffness
{
	using System;
	
	/// <summary>
	/// Description of Linear3DBeamStiffnessMatrixBuilder.
	/// </summary>
	public class Linear3DBeamStiffnessMatrixBuilder : StiffnessMatrixBuilder
	{
		public override KeyedVector<NodalDegreeOfFreedom> GetStrainDisplacementMatrix()
		{
			throw new NotImplementedException("Linear3DBeamStiffnessMatrixBuilder.GetStrainDisplacementMatrix");
		}
		
		public override ElementStiffnessMatrix GetStiffnessMatrix()
		{
			Linear3DBeam beam = this.CastElementToBeam();
			
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
            
			double bendingStiffnessAboutY = 4.0 * material.YoungsModulus * section.SecondMomentOfAreaAroundYY / length;
			double startToEndNodeRelationshipForBendingStiffnessAboutY = 2.0 * material.YoungsModulus * section.SecondMomentOfAreaAroundYY / length;
			matrix.At(beam.StartNode, DegreeOfFreedom.YY, beam.StartNode, DegreeOfFreedom.YY,  bendingStiffnessAboutY);
            matrix.At(beam.StartNode, DegreeOfFreedom.YY, beam.EndNode,   DegreeOfFreedom.YY, -bendingStiffnessAboutY);
            matrix.At(beam.EndNode,   DegreeOfFreedom.YY, beam.StartNode, DegreeOfFreedom.YY, -bendingStiffnessAboutY);
            matrix.At(beam.EndNode,   DegreeOfFreedom.YY, beam.EndNode,   DegreeOfFreedom.YY,  bendingStiffnessAboutY);
            
			double bendingStiffnessAboutZ = 4.0 * material.YoungsModulus * section.SecondMomentOfAreaAroundZZ / length;
			double startToEndNodeRelationshipForBendingStiffnessAboutZ = 2.0 * material.YoungsModulus * section.SecondMomentOfAreaAroundZZ / length;
			matrix.At(beam.StartNode, DegreeOfFreedom.ZZ, beam.StartNode, DegreeOfFreedom.ZZ, bendingStiffnessAboutZ);
            matrix.At(beam.StartNode, DegreeOfFreedom.ZZ, beam.EndNode,   DegreeOfFreedom.ZZ, startToEndNodeRelationshipForBendingStiffnessAboutZ);
            matrix.At(beam.EndNode,   DegreeOfFreedom.ZZ, beam.StartNode, DegreeOfFreedom.ZZ, startToEndNodeRelationshipForBendingStiffnessAboutZ);
            matrix.At(beam.EndNode,   DegreeOfFreedom.ZZ, beam.EndNode,   DegreeOfFreedom.ZZ, bendingStiffnessAboutZ);
            
			double bendingAboutZshearInY = 6.0 * material.YoungsModulus * section.SecondMomentOfAreaAroundZZ / (length * length);
			matrix.At(beam.StartNode, DegreeOfFreedom.Y,  beam.StartNode, DegreeOfFreedom.ZZ,  bendingAboutZshearInY);
            matrix.At(beam.StartNode, DegreeOfFreedom.Y,  beam.EndNode,   DegreeOfFreedom.ZZ, -bendingAboutZshearInY);
            matrix.At(beam.EndNode,   DegreeOfFreedom.Y,  beam.StartNode, DegreeOfFreedom.ZZ, -bendingAboutZshearInY);
            matrix.At(beam.EndNode,   DegreeOfFreedom.Y,  beam.EndNode,   DegreeOfFreedom.ZZ,  bendingAboutZshearInY);
			matrix.At(beam.StartNode, DegreeOfFreedom.ZZ, beam.StartNode, DegreeOfFreedom.Y,   bendingAboutZshearInY);
            matrix.At(beam.StartNode, DegreeOfFreedom.ZZ, beam.EndNode,   DegreeOfFreedom.Y,  -bendingAboutZshearInY);
            matrix.At(beam.EndNode,   DegreeOfFreedom.ZZ, beam.StartNode, DegreeOfFreedom.Y,  -bendingAboutZshearInY);
            matrix.At(beam.EndNode,   DegreeOfFreedom.ZZ, beam.EndNode,   DegreeOfFreedom.Y,   bendingAboutZshearInY);
            
			double bendingAboutYShearInZ = 6.0 * material.YoungsModulus * section.SecondMomentOfAreaAroundYY / (length * length);
			matrix.At(beam.StartNode, DegreeOfFreedom.Z,  beam.StartNode, DegreeOfFreedom.YY, -bendingAboutYShearInZ);
            matrix.At(beam.StartNode, DegreeOfFreedom.Z,  beam.EndNode,   DegreeOfFreedom.YY,  bendingAboutYShearInZ);
            matrix.At(beam.EndNode,   DegreeOfFreedom.Z,  beam.StartNode, DegreeOfFreedom.YY,  bendingAboutYShearInZ);
            matrix.At(beam.EndNode,   DegreeOfFreedom.Z,  beam.EndNode,   DegreeOfFreedom.YY, -bendingAboutYShearInZ);
			matrix.At(beam.StartNode, DegreeOfFreedom.YY, beam.StartNode, DegreeOfFreedom.Z,  -bendingAboutYShearInZ);
            matrix.At(beam.StartNode, DegreeOfFreedom.YY, beam.EndNode,   DegreeOfFreedom.Z,   bendingAboutYShearInZ);
            matrix.At(beam.EndNode,   DegreeOfFreedom.YY, beam.StartNode, DegreeOfFreedom.Z,   bendingAboutYShearInZ);
            matrix.At(beam.EndNode,   DegreeOfFreedom.YY, beam.EndNode,   DegreeOfFreedom.Z,  -bendingAboutYShearInZ);
            
            return matrix;
		}
		
		private IMaterial GetMaterial()
        {        	
        	IHasMaterial hasMaterial = this.Element as IHasMaterial;
			if (hasMaterial == null)
			{
				throw new NotImplementedException("Linear3DBeamStiffnessMatrixBuilder expects finite elements to implement IHasMaterial");
			}
			
			return hasMaterial.Material;
        }
		
		private ICrossSection GetCrossSection()
        {
        	IHasConstantCrossSection hasCrossSection = this.Element as IHasConstantCrossSection;
			if (hasCrossSection == null)
			{
				throw new NotImplementedException("Linear3DBeamStiffnessMatrixBuilder expects finite elements to implement IHasConstantCrossSection");
			}
			
			return hasCrossSection.CrossSection;
        }
		
		private Linear3DBeam CastElementToBeam()
        {
        	Linear3DBeam beam = this.Element as Linear3DBeam;
			if (beam == null)
			{
				throw new NotImplementedException("Linear3DBeamStiffnessMatrixBuilder has only been implemented for Linear3DBeam finite elements");
			}
			
			return beam;
        }
	}
}
