namespace SharpFE.Core.Tests.Stiffness
{
	using System;
	using SharpFE.Stiffness;
	
	public class StiffnessHelpers
	{
		public static void Assert12x12StiffnessMatrix(IElementStiffnessCalculator SUT, params double[] expectedValues)
        {
            Helpers.AssertMatrix(SUT.StiffnessMatrixInGlobalCoordinates, 12, 12, expectedValues);
        }
		
		public static void Assert18x18StiffnessMatrix(IElementStiffnessCalculator SUT, params double[] expectedValues)
        {
			Helpers.AssertMatrix(SUT.StiffnessMatrixInGlobalCoordinates, 18, 18, expectedValues);
        }
		
		public static void Assert24x24StiffnessMatrix(IElementStiffnessCalculator SUT, params double[] expectedValues)
        {
			Helpers.AssertMatrix(SUT.StiffnessMatrixInGlobalCoordinates, 24, 24, expectedValues);
        }
	}
}
