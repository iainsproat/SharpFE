//-----------------------------------------------------------------------
// <copyright file="?.cs" company="Iain Sproat">
//     Copyright Iain Sproat, 2012.
// </copyright>
//-----------------------------------------------------------------------
using System;

namespace SharpFE.Stiffness
{
	/// <summary>
	/// Description of LinearConstantStrainTriangleStiffnessMatrixBuilder.
	/// </summary>
	public class LinearConstantStrainTriangleStiffnessMatrixBuilder : StiffnessMatrixBuilder
	{
		public LinearConstantStrainTriangleStiffnessMatrixBuilder(FiniteElement element)
			:base(element)
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
			throw new NotImplementedException();
		}
	}
}
