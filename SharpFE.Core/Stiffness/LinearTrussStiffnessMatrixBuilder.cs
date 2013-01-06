

namespace SharpFE.Stiffness
{
	using System;
	using SharpFE.Elements;
	
	/// <summary>
	/// Description of LinearElasticMaterialCrossSectionStiffnessBuilder.
	/// </summary>
	public class LinearTrussStiffnessMatrixBuilder : Linear1DStiffnessMatrixBuilder
	{
		public LinearTrussStiffnessMatrixBuilder(FiniteElement finiteElement)
			:base(finiteElement)
		{
			// empty
		}
		
		public override ElementStiffnessMatrix GetLocalStiffnessMatrix()
		{
			FiniteElement1D element1D = this.CastElementTo<FiniteElement1D>();
			double stiffness = this.CalculateStiffnessConstant(element1D);
			
			ElementStiffnessMatrix matrix = new ElementStiffnessMatrix(this.Element.SupportedNodalDegreeOfFreedoms);
			matrix.At(element1D.StartNode, DegreeOfFreedom.X, element1D.StartNode, DegreeOfFreedom.X, stiffness);
			matrix.At(element1D.StartNode, DegreeOfFreedom.X, element1D.EndNode, DegreeOfFreedom.X, -stiffness);
			matrix.At(element1D.EndNode, DegreeOfFreedom.X, element1D.StartNode, DegreeOfFreedom.X, -stiffness);
			matrix.At(element1D.EndNode, DegreeOfFreedom.X, element1D.EndNode, DegreeOfFreedom.X, stiffness);
			
			return matrix;
		}
		
		private double CalculateStiffnessConstant(FiniteElement1D element1D)
		{
			try
			{
				LinearConstantSpring spring = this.CastElementTo<LinearConstantSpring>();
				if (spring != null)
				{
					return spring.SpringConstant;
				}
			}
			catch
			{
				// do nothing
			}
			
			IMaterial material = this.GetMaterial();
			ICrossSection crossSection = this.GetCrossSection();
			
			return material.YoungsModulus * crossSection.Area / element1D.OriginalLength;
		}
	}
}
