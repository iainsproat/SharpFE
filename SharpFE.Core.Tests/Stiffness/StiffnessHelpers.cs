namespace SharpFE.Core.Tests.Stiffness
{
	using System;
	using SharpFE.Stiffness;
	
	public class StiffnessHelpers
	{
		public static void Assert12x12StiffnessMatrix(IStiffnessMatrixBuilder SUT, params double[] expectedValues)
        {
            Helpers.AssertMatrix(SUT.BuildGlobalStiffnessMatrix(), 12, 12, expectedValues);
        }
	}
}
