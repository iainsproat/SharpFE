using System;
using NUnit.Framework;
using SharpFE;
using SharpFE.Stiffness;

namespace SharpFE.Core.Tests.Stiffness
{
	[TestFixture]
	public class LinearConstantSpringStiffnessMatrixBuilderGlobalStiffnessTest : LinearConstantSpringStiffnessMatrixBuilderTestBase
	{
		[Test]
		public void CanGetStiffnessAt()
		{
			double result = SUT.GetStiffnessInGlobalCoordinatesAt(start, DegreeOfFreedom.X, start, DegreeOfFreedom.X);
			Assert.AreEqual(2, result);
			result = SUT.GetStiffnessInGlobalCoordinatesAt(start, DegreeOfFreedom.X, end, DegreeOfFreedom.X);
			Assert.AreEqual(-2, result);
		}
		
		[Test]
		public void CanCanCreateGlobalStiffnessMatrixForSpringAlignedToGlobalXAxis()
		{
			this.CreateAndStore2DSpringFromOriginTo(1, 0);
			
			this.Assert6x6StiffnessMatrix(1, 0, 0, -1, 0, 0,
			                              0, 0, 0,  0, 0, 0,
			                              0, 0, 0,  0, 0, 0, 
			                             -1, 0, 0,  1, 0, 0, 
			                              0, 0, 0,  0, 0, 0, 
			                              0, 0, 0,  0, 0, 0);
		}
		
		[Test]
		public void CanCanCreateGlobalStiffnessMatrixForSpringAlignedToNegativeGlobalXAxis()
		{
			this.CreateAndStore2DSpringFromOriginTo(-1, 0);
			
			this.Assert6x6StiffnessMatrix(1, 0, 0, -1, 0, 0, 
			                              0, 0, 0,  0, 0, 0, 
			                              0, 0, 0,  0, 0, 0, 

			                             -1, 0, 0,  1, 0, 0, 
			                              0, 0, 0,  0, 0, 0,
			                              0, 0, 0,  0, 0, 0);
		}
		
		[Test]
		public void CanCanCreateGlobalStiffnessMatrixForSpringAlignedToGlobalYAxis()
		{
			this.CreateAndStore3DSpringFromOriginTo(0, 1, 0);
			
			this.Assert6x6StiffnessMatrix(0,  0, 0,   0,  0, 0, 
			                              0,  1, 0,   0, -1, 0, 
			                              0,  0, 0,   0,  0, 0,
			                              0,  0, 0,   0,  0, 0,
			                              0, -1, 0,   0,  1, 0, 
			                              0,  0, 0,   0,  0, 0);
		}
		
		[Test]
		public void CanCanCreateGlobalStiffnessMatrixForSpringAlignedToNegativeGlobalYAxis()
		{
			this.CreateAndStore3DSpringFromOriginTo(0, -1, 0);
			
			this.Assert6x6StiffnessMatrix(0,  0, 0,   0,  0, 0, 
			                              0,  1, 0,   0, -1, 0, 
			                              0,  0, 0,   0,  0, 0, 
			                               
			                              0,  0, 0,   0,  0, 0, 
			                              0, -1, 0,   0,  1, 0, 
			                              0,  0, 0,   0,  0, 0);
		}
		
		[Test]
		public void CanCanCreateGlobalStiffnessMatrixForSpringAlignedToGlobalZAxis()
		{
			this.CreateAndStore3DSpringFromOriginTo(0, 0, 1);
			
			this.Assert6x6StiffnessMatrix(0, 0,  0,   0, 0,  0, 
			                              0, 0,  0,   0, 0,  0, 
			                              0, 0,  1,   0, 0, -1, 
			                              0, 0,  0,   0, 0,  0, 
			                              0, 0,  0,   0, 0,  0, 
			                              0, 0, -1,   0, 0,  1);
		}
		
		[Test]
		public void CanCanCreateGlobalStiffnessMatrixForSpringAlignedToNegativeGlobalZAxis()
		{
			this.CreateAndStore3DSpringFromOriginTo(0, 0, -1);
			
			this.Assert6x6StiffnessMatrix(0, 0,  0,   0, 0,  0, 
			                              0, 0,  0,   0, 0,  0, 
			                              0, 0,  1,   0, 0, -1, 
			                              
			                              0, 0,  0,   0, 0,  0,
			                              0, 0,  0,   0, 0,  0, 
			                              0, 0, -1,   0, 0,  1);
		}

		
		[Test]
		public void CanCanCreateGlobalStiffnessMatrixForSpringAlignedToGlobalXYPlaneQuadrant1()
		{
			this.CreateAndStore3DSpringFromOriginTo(1, 1, 0);
			
			this.Assert6x6StiffnessMatrix( 0.5,  0.5, 0,   -0.5, -0.5, 0, 
			                               0.5,  0.5, 0,   -0.5, -0.5, 0, 
			                               0,    0,   0,    0,    0,   0, 
			                              -0.5, -0.5, 0,    0.5,  0.5, 0, 
			                              -0.5, -0.5, 0,    0.5,  0.5, 0, 
			                               0,    0,   0,    0,    0,   0);
		}
		
		[Test]
		public void CanCanCreateGlobalStiffnessMatrixForSpringAlignedToGlobalXYPlaneQuadrant3()
		{
			this.CreateAndStore3DSpringFromOriginTo(-1, -1, 0);
			
			this.Assert6x6StiffnessMatrix( 0.5,  0.5, 0,   -0.5, -0.5, 0, 
			                               0.5,  0.5, 0,   -0.5, -0.5, 0, 
			                               0,    0,   0,    0,    0,   0, 
			                              -0.5, -0.5, 0,    0.5,  0.5, 0, 
			                              -0.5, -0.5, 0,    0.5,  0.5, 0, 
			                               0,    0,   0,    0,    0,   0);
		}
		
		[Test]
		public void CanCanCreateGlobalStiffnessMatrixForSpringAlignedToGlobalXZPlaneQuadrant1()
		{
			this.CreateAndStore3DSpringFromOriginTo(1, 0, 1);
			
			this.Assert6x6StiffnessMatrix( 0.5, 0,  0.5,  -0.5, 0,   -0.5, 
			                               0,   0,  0,     0,   0,    0,   
			                               0.5, 0,  0.5,  -0.5, 0,   -0.5, 
			                              -0.5, 0, -0.5,   0.5, 0,    0.5,
			                               0,   0,  0,     0,   0,    0,   
			                              -0.5, 0, -0.5,   0.5, 0,    0.5);
		}
		
		[Test]
		public void CanCanCreateGlobalStiffnessMatrixForSpringAlignedToGlobalXZPlaneQuadrant3()
		{
			this.CreateAndStore3DSpringFromOriginTo(-1, 0, -1);
			
			this.Assert6x6StiffnessMatrix( 0.5, 0,  0.5,  -0.5, 0,   -0.5, 
			                               0,   0,  0,     0,   0,    0,   
			                               0.5, 0,  0.5,  -0.5, 0,   -0.5, 
			                              -0.5, 0, -0.5,   0.5, 0,    0.5, 
			                               0,   0,  0,     0,   0,    0,   
			                              -0.5, 0, -0.5,   0.5, 0,    0.5);
		}
		
		[Test]
		public void CanCanCreateGlobalStiffnessMatrixForSpringAlignedToGlobalYZPlaneQuadrant1()
		{
			this.CreateAndStore3DSpringFromOriginTo(0, 1, 1);
			
			this.Assert6x6StiffnessMatrix(0,  0,    0,    0,  0,    0,   
			                              0,  0.5,  0.5,  0, -0.5, -0.5, 
			                              0,  0.5,  0.5,  0, -0.5, -0.5, 
			                              0,  0,    0,    0,  0,    0,   
			                              0, -0.5, -0.5,  0,  0.5,  0.5, 
			                              0, -0.5, -0.5,  0,  0.5,  0.5);
		}
		
		[Test]
		public void CanCanCreateGlobalStiffnessMatrixForSpringAlignedToGlobalYZPlaneQuadrant3()
		{
			this.CreateAndStore3DSpringFromOriginTo(0, -1, -1);
			
			this.Assert6x6StiffnessMatrix(0,  0,    0,    0,  0,    0,   
			                              0,  0.5,  0.5,  0, -0.5, -0.5, 
			                              0,  0.5,  0.5,  0, -0.5, -0.5, 
			                              0,  0,    0,    0,  0,    0,   
			                              0, -0.5, -0.5,  0,  0.5,  0.5,
			                              0, -0.5, -0.5,  0,  0.5,  0.5);
		}
		
		[Test]
		public void CanCanCreateGlobalStiffnessMatrixForSpringInQuadrant1PositiveZ()
		{
			this.CreateAndStore3DSpringFromOriginTo(1, 1, 1);
			
			double a = 1.0 / 3.0;
			this.Assert6x6StiffnessMatrix( a,  a,  a,  -a, -a, -a, 
			                               a,  a,  a,  -a, -a, -a, 
			                               a,  a,  a,  -a, -a, -a, 
			                              -a, -a, -a,   a,  a,  a, 
			                              -a, -a, -a,   a,  a,  a, 
			                              -a, -a, -a,   a,  a,  a);
		}
		
		[Test]
		public void CanCanCreateGlobalStiffnessMatrixForSpringInQuadrant2PositiveZ()
		{
			this.CreateAndStore3DSpringFromOriginTo(-1, 1, 1);
			
			double a = 1.0 / 3.0;
			this.Assert6x6StiffnessMatrix( a, -a, -a,  -a,  a,  a, 
			                              -a,  a,  a,   a, -a, -a, 
			                              -a,  a,  a,   a, -a, -a, 
			                              -a,  a,  a,   a, -a, -a,
			                               a, -a, -a,  -a,  a,  a, 
			                               a, -a, -a,  -a,  a,  a);
		}
		
		[Test]
		public void CanCanCreateGlobalStiffnessMatrixForSpringInQuadrant3PositiveZ()
		{
			this.CreateAndStore3DSpringFromOriginTo(-1, -1, 1);
			
			double a = 1.0 / 3.0;
			this.Assert6x6StiffnessMatrix( a,  a, -a,  -a, -a,  a,
			                               a,  a, -a,  -a, -a,  a, 
			                              -a, -a,  a,   a,  a, -a, 
			                              -a, -a,  a,   a,  a, -a, 
			                              -a, -a,  a,   a,  a, -a, 
			                               a,  a, -a,  -a, -a,  a);
		}
		
		[Test]
		public void CanCanCreateGlobalStiffnessMatrixForSpringInQuadrant4PositiveZ()
		{
			this.CreateAndStore3DSpringFromOriginTo(1, -1, 1);
			
			double a = 1.0 / 3.0;
			this.Assert6x6StiffnessMatrix( a, -a,  a,  -a,  a, -a, 
			                              -a,  a, -a,   a, -a,  a, 
			                               a, -a,  a,  -a,  a, -a, 
			                              -a,  a, -a,   a, -a,  a, 
			                               a, -a,  a,  -a,  a, -a, 
			                              -a,  a, -a,   a, -a,  a);
		}
		
		[Test]
		public void CanCanCreateGlobalStiffnessMatrixForSpringInQuadrant1NegativeZ()
		{
			this.CreateAndStore3DSpringFromOriginTo(1, 1, -1);
			
			double a = 1.0 / 3.0;
			this.Assert6x6StiffnessMatrix( a,  a, -a,  -a, -a,  a, 
			                               a,  a, -a,  -a, -a,  a, 
			                              -a, -a,  a,   a,  a, -a, 
			                              -a, -a,  a,   a,  a, -a, 
			                              -a, -a,  a,   a,  a, -a, 
			                               a,  a, -a,  -a, -a,  a);
		}
		
		[Test]
		public void CanCanCreateGlobalStiffnessMatrixForSpringInQuadrant2NegativeZ()
		{
			this.CreateAndStore3DSpringFromOriginTo(-1, 1, -1);
			
			double a = 1.0 / 3.0;
			this.Assert6x6StiffnessMatrix( a, -a,  a,  -a,  a, -a, 
			                              -a,  a, -a,   a, -a,  a, 
			                               a, -a,  a,  -a,  a, -a, 
			                              -a,  a, -a,   a, -a,  a, 
			                               a, -a,  a,  -a,  a, -a, 
			                              -a,  a, -a,   a, -a,  a);
		}
		
		[Test]
		public void CanCanCreateGlobalStiffnessMatrixForSpringInQuadrant3NegativeZ()
		{
			this.CreateAndStore3DSpringFromOriginTo(-1, -1, -1);

			
			double a = 1.0 / 3.0;
			this.Assert6x6StiffnessMatrix( a,  a,  a,  -a, -a, -a, 
			                               a,  a,  a,  -a, -a, -a, 
			                               a,  a,  a,  -a, -a, -a, 
			                              -a, -a, -a,   a,  a,  a, 
			                              -a, -a, -a,   a,  a,  a, 
			                              -a, -a, -a,   a,  a,  a);
		}
		
		[Test]
		public void CanCanCreateGlobalStiffnessMatrixForSpringInQuadrant4NegativeZ()
		{
			this.CreateAndStore3DSpringFromOriginTo(1, -1, -1);
			
			double a = 1.0 / 3.0;
			this.Assert6x6StiffnessMatrix( a, -a, -a,  -a,  a,  a, 
			                              -a,  a,  a,   a, -a, -a, 
			                              -a,  a,  a,   a, -a, -a, 
			                              -a,  a,  a,   a, -a, -a, 
			                               a, -a, -a,  -a,  a,  a, 
			                               a, -a, -a,  -a,  a,  a);
		}
	}
}
