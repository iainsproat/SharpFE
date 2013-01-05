

namespace SharpFE.Stiffness
{
	using System;
	using System.Collections.Generic;
	using SharpFE.Elements;
	
	public abstract class Linear1DStiffnessMatrixBuilder : StiffnessMatrixBuilder
	{
		public Linear1DStiffnessMatrixBuilder()
		{
			// empty
		}
		
		/// <summary>
		/// Generates the transposed strain-displacement matrix for the given element
		/// </summary>
		/// <param name="element"></param>
		/// <returns></returns>
		public override KeyedVector<NodalDegreeOfFreedom> GetStrainDisplacementMatrix()
		{
			FiniteElement1D fe1d = this.CastElementTo<FiniteElement1D>();
			
			IList<NodalDegreeOfFreedom> supportedNodalDegreeOfFreedoms = this.Element.SupportedNodalDegreeOfFreedoms;
			KeyedVector<NodalDegreeOfFreedom> B = new KeyedVector<NodalDegreeOfFreedom>(supportedNodalDegreeOfFreedoms);
			
			double originalLength = fe1d.OriginalLength;
			
			B[new NodalDegreeOfFreedom(fe1d.StartNode, DegreeOfFreedom.X)] = -1.0 / originalLength;
			B[new NodalDegreeOfFreedom(fe1d.EndNode, DegreeOfFreedom.X)]   =  1.0 / originalLength;
			
			return B;
		}
	}
}
