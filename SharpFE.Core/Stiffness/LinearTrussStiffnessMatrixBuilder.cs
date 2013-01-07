

namespace SharpFE.Stiffness
{
	using System;
	using System.Collections.Generic;
	using SharpFE.Elements;
	
	/// <summary>
	/// Description of LinearElasticMaterialCrossSectionStiffnessBuilder.
	/// </summary>
	public class LinearTrussStiffnessMatrixBuilder : StiffnessMatrixBuilder
	{
		public LinearTrussStiffnessMatrixBuilder(FiniteElement finiteElement)
			:base(finiteElement)
		{
			// empty
		}

		public override KeyedRowColumnMatrix<DegreeOfFreedom, NodalDegreeOfFreedom> GetShapeFunctionVector(FiniteElementNode location)
		{
			throw new NotImplementedException();
		}
		
		/// <summary>
		/// Generates the transposed strain-displacement matrix for the given element
		/// </summary>
		/// <param name="element"></param>
		/// <returns></returns>
		public override KeyedRowColumnMatrix<Strain, NodalDegreeOfFreedom> GetStrainDisplacementMatrix()
		{
			FiniteElement1D fe1d = this.CastElementTo<FiniteElement1D>();
			
			IList<Strain> supportedDegreesOfFreedom = new List<Strain>(1);
			supportedDegreesOfFreedom.Add(Strain.LinearStrainX);
			IList<NodalDegreeOfFreedom> supportedNodalDegreeOfFreedoms = this.Element.SupportedNodalDegreeOfFreedoms;
			KeyedRowColumnMatrix<Strain, NodalDegreeOfFreedom> B = new KeyedRowColumnMatrix<Strain, NodalDegreeOfFreedom>(supportedDegreesOfFreedom, supportedNodalDegreeOfFreedoms);
			
			double originalLength = fe1d.OriginalLength;
			
			B.At(Strain.LinearStrainX, new NodalDegreeOfFreedom(fe1d.StartNode, DegreeOfFreedom.X), -1.0 / originalLength);
			B.At(Strain.LinearStrainX, new NodalDegreeOfFreedom(fe1d.EndNode, DegreeOfFreedom.X), 1.0 / originalLength);
			
			return B;
		}
		
		public override StiffnessMatrix GetLocalStiffnessMatrix()
		{
			FiniteElement1D element1D = this.CastElementTo<FiniteElement1D>();
			double stiffness = this.CalculateStiffnessConstant(element1D);
			
			StiffnessMatrix matrix = new StiffnessMatrix(this.Element.SupportedNodalDegreeOfFreedoms);
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
