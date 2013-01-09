namespace SharpFE.Core.Tests.Stiffness
{
	using System;
	using SharpFE.Stiffness;
	
	public class StiffnessHelpers
	{
		public static void Assert12x12StiffnessMatrix(IStiffnessProvider SUT, params double[] expectedValues)
        {
            Helpers.AssertMatrix(SUT.GlobalStiffnessMatrix, 12, 12, expectedValues);
        }
		
		public static void Assert18x18StiffnessMatrix(IStiffnessProvider SUT, params double[] expectedValues)
        {
			Helpers.AssertMatrix(SUT.GlobalStiffnessMatrix, 18, 18, expectedValues);
        }
		
		public static void Assert24x24StiffnessMatrix(IStiffnessProvider SUT, params double[] expectedValues)
        {
			Helpers.AssertMatrix(SUT.GlobalStiffnessMatrix, 24, 24, expectedValues);
        }
	}
}
