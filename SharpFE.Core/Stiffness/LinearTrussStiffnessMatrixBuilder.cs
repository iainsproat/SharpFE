

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
			IMaterial material = this.GetMaterial();
			ICrossSection crossSection = this.GetCrossSection();
			FiniteElement1D element1D = this.CastElementTo<FiniteElement1D>();
			
			double stiffness = material.YoungsModulus * crossSection.Area / element1D.OriginalLength;
			
			ElementStiffnessMatrix matrix = new ElementStiffnessMatrix(this.Element.SupportedNodalDegreeOfFreedoms);
            matrix.At(element1D.StartNode, DegreeOfFreedom.X, element1D.StartNode, DegreeOfFreedom.X, stiffness);
            matrix.At(element1D.StartNode, DegreeOfFreedom.X, element1D.EndNode, DegreeOfFreedom.X, -stiffness);
            matrix.At(element1D.EndNode, DegreeOfFreedom.X, element1D.StartNode, DegreeOfFreedom.X, -stiffness);
            matrix.At(element1D.EndNode, DegreeOfFreedom.X, element1D.EndNode, DegreeOfFreedom.X, stiffness);
            
            return matrix;
		}
	}
}
