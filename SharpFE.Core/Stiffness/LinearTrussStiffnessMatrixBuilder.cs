

namespace SharpFE.Stiffness
{
	using System;
	using SharpFE.Elements;
	
	/// <summary>
	/// Description of LinearElasticMaterialCrossSectionStiffnessBuilder.
	/// </summary>
	public class LinearTrussStiffnessMatrixBuilder : Linear1DStiffnessMatrixBuilder
	{
		
		public override ElementStiffnessMatrix GetStiffnessMatrix()
		{
			IMaterial material = this.GetMaterialFromElement();
			ICrossSection crossSection = this.GetCrossSectionFromElement();
			FiniteElement1D element1D = this.CastElementToFiniteElement1D();
			
			double stiffness = material.YoungsModulus * crossSection.Area / element1D.OriginalLength;
			
			ElementStiffnessMatrix matrix = new ElementStiffnessMatrix(this.Element.SupportedNodalDegreeOfFreedoms);
            matrix.At(element1D.StartNode, DegreeOfFreedom.X, element1D.StartNode, DegreeOfFreedom.X, stiffness);
            matrix.At(element1D.StartNode, DegreeOfFreedom.X, element1D.EndNode, DegreeOfFreedom.X, -stiffness);
            matrix.At(element1D.EndNode, DegreeOfFreedom.X, element1D.StartNode, DegreeOfFreedom.X, -stiffness);
            matrix.At(element1D.EndNode, DegreeOfFreedom.X, element1D.EndNode, DegreeOfFreedom.X, stiffness);
            
            return matrix;
		}
		
		private IMaterial GetMaterialFromElement()
        {        	
        	IHasMaterial hasMaterial = this.Element as IHasMaterial;
			if (hasMaterial == null)
			{
				throw new NotImplementedException("LinearElasticMaterialCrossSectionStiffnessBuilder expects finite elements to implement IHasMaterial");
			}
			
			return hasMaterial.Material;
        }
		
		private ICrossSection GetCrossSectionFromElement()
        {
        	IHasConstantCrossSection hasCrossSection = this.Element as IHasConstantCrossSection;
			if (hasCrossSection == null)
			{
				throw new NotImplementedException("LinearElasticMaterialCrossSectionStiffnessBuilder expects finite elements to implement IHasConstantCrossSection");
			}
			
			return hasCrossSection.CrossSection;
        }
	}
}
