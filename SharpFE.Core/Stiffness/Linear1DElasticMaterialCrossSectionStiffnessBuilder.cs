

namespace SharpFE.Stiffness
{
	using System;
	using SharpFE.Elements;
	
	/// <summary>
	/// Description of LinearElasticMaterialCrossSectionStiffnessBuilder.
	/// </summary>
	public class Linear1DElasticMaterialCrossSectionStiffnessBuilder : LinearStiffnessMatrixBuilder
	{
		
		public override ElementStiffnessMatrix GetStiffnessMatrix(FiniteElement element)
		{
			IMaterial material = this.GetMaterialFromElement(element);
			ICrossSection crossSection = this.GetCrossSectionFromElement(element);
			FiniteElement1D element1D = this.CastToFiniteElement1D(element);
			
			double stiffness = material.YoungsModulus * crossSection.Area / element1D.OriginalLength;
			
			ElementStiffnessMatrix matrix = new ElementStiffnessMatrix(element1D.SupportedNodalDegreeOfFreedoms);
            matrix.At(element1D.StartNode, DegreeOfFreedom.X, element1D.StartNode, DegreeOfFreedom.X, stiffness);
            matrix.At(element1D.StartNode, DegreeOfFreedom.X, element1D.EndNode, DegreeOfFreedom.X, -stiffness);
            matrix.At(element1D.EndNode, DegreeOfFreedom.X, element1D.StartNode, DegreeOfFreedom.X, -stiffness);
            matrix.At(element1D.EndNode, DegreeOfFreedom.X, element1D.EndNode, DegreeOfFreedom.X, stiffness);
            
            return matrix;
		}
		
		private IMaterial GetMaterialFromElement(FiniteElement element)
        {
        	if (element == null)
        	{
        		throw new ArgumentNullException("element");
        	}
        	
        	IHasMaterial hasMaterial = element as IHasMaterial;
			if (hasMaterial == null)
			{
				throw new NotImplementedException("LinearElasticMaterialCrossSectionStiffnessBuilder expects finite elements to implement IHasMaterial");
			}
			
			return hasMaterial.Material;
        }
		
		private ICrossSection GetCrossSectionFromElement(FiniteElement element)
        {
        	if (element == null)
        	{
        		throw new ArgumentNullException("element");
        	}
        	
        	IHasConstantCrossSection hasCrossSection = element as IHasConstantCrossSection;
			if (hasCrossSection == null)
			{
				throw new NotImplementedException("LinearElasticMaterialCrossSectionStiffnessBuilder expects finite elements to implement IHasConstantCrossSection");
			}
			
			return hasCrossSection.CrossSection;
        }
	}
}
