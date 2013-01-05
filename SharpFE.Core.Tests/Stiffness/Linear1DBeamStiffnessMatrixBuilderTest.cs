//-----------------------------------------------------------------------
// <copyright file="?.cs" company="Iain Sproat">
//     Copyright Iain Sproat, 2012.
// </copyright>
//-----------------------------------------------------------------------
namespace SharpFE.Core.Tests.Stiffness
{
	using System;
	using NUnit.Framework;
	using SharpFE.Stiffness;
	
	[TestFixture]
	public class Linear1DBeamStiffnessMatrixBuilderTest
	{
		private NodeFactory nodeFactory;
		private ElementFactory elementFactory;
		private FiniteElementNode start;
		private FiniteElementNode end;
		private GenericElasticMaterial material;
		private SolidRectangle section;
		private Linear1DBeam beam;
		private IStiffnessMatrixBuilder SUT;
		
		[SetUp]
		public void SetUp()
		{
			nodeFactory = new NodeFactory(ModelType.Frame2D);
			start = nodeFactory.CreateForTruss(0, 0);
			end = nodeFactory.CreateForTruss(1, 0);
			elementFactory = new ElementFactory();
			material = new GenericElasticMaterial(0, 1, 0, 0);
			section = new SolidRectangle(1, 1);
			beam = elementFactory.CreateLinear1DBeam(start, end, material, section);
			SUT = this.beam.StiffnessBuilder;
		}
		
		[Test]
		public void CanCanCreateGlobalStiffnessMatrixForSpringAlignedToGlobalXAxis()
		{
			double a = 1.0 / 3.0;
			double b = a / 2.0;
			StiffnessHelpers.Assert12x12StiffnessMatrix(SUT,
			                                            0, 0,  0,   0,  0,   0,   0, 0,  0,   0,  0,   0,
			                                            0, 0,  0,   0,  0,   0,   0, 0,  0,   0,  0,   0,
			                                            0, 0,  1,   0, -0.5, 0,   0, 0, -1,   0, -0.5, 0,
			                                            0, 0,  0,   0,  0,   0,   0, 0,  0,   0,  0,   0,
			                                            0, 0, -0.5, 0,  a,   0,   0, 0,  0.5, 0,  b,   0,
			                                            0, 0,  0,   0,  0,   0,   0, 0,  0,   0,  0,   0,
			                                            
			                                            0, 0,  0,   0,  0,   0,   0, 0,  0,   0,  0,   0,
			                                            0, 0,  0,   0,  0,   0,   0, 0,  0,   0,  0,   0,
			                                            0, 0, -1,   0,  0.5, 0,   0, 0,  1,   0,  0.5, 0,
			                                            0, 0,  0,   0,  0,   0,   0, 0,  0,   0,  0,   0,
			                                            0, 0, -0.5, 0,  b,   0,   0, 0,  0.5, 0,  a,   0,
			                                            0, 0,  0,   0,  0,   0,   0, 0,  0,   0,  0,   0);
		}
		
		[Test]
		public void StiffnessMatrixReflectsReleases()
		{
			Assert.Ignore();
		}
	}
}
