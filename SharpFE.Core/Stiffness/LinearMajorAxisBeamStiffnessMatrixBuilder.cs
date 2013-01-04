namespace SharpFE.Stiffness
{
	using System;

	public class LinearMajorAxisBeamStiffnessMatrixBuilder : StiffnessMatrixBuilder
	{
		public override KeyedVector<NodalDegreeOfFreedom> GetStrainDisplacementMatrix()
		{
			throw new NotImplementedException();
		}
		
		public override ElementStiffnessMatrix GetStiffnessMatrix()
		{
			throw new NotImplementedException();
		}
	}
}
