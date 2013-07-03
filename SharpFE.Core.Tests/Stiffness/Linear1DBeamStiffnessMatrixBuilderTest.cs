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
		private ICrossSection section;
		private Linear1DBeam beam;
		private ElementStiffnessMatrixBuilder<Linear1DBeam> SUT;
		
		[SetUp]
		public void SetUp()
		{
			nodeFactory = new NodeFactory(ModelType.Frame2D);
			start = nodeFactory.CreateFor2DTruss(0, 0);
			end = nodeFactory.CreateFor2DTruss(1, 0);
			elementFactory = new ElementFactory(ModelType.Frame2D);
			material = new GenericElasticMaterial(0, 1, 0, 0);
			section = new GenericCrossSection(1, 1);
			beam = elementFactory.CreateLinear1DBeam(start, end, material, section);
			SUT = new Linear1DBeamStiffnessMatrixBuilder(beam);
		}
		
		[Test]
		public void CanCreateGlobalStiffnessMatrixForBeamAlignedToGlobalXAxis()
		{
			StiffnessHelpers.Assert12x12StiffnessMatrix(SUT,
			                                            1, 0,   0,   0,  0,   0,  -1, 0,   0,   0,  0,   0,
			                                            0, 0,   0,   0,  0,   0,   0, 0,   0,   0,  0,   0,
			                                            0, 0,  12,   0, -6,   0,   0, 0, -12,   0, -6,   0,
			                                            0, 0,   0,   0,  0,   0,   0, 0,   0,   0,  0,   0,
			                                            0, 0,  -6,   0,  4,   0,   0, 0,   6,   0,  2,   0,
			                                            0, 0,   0,   0,  0,   0,   0, 0,   0,   0,  0,   0,
			                                            
			                                           -1, 0,   0,   0,  0,   0,   1, 0,   0,   0,  0,   0,
			                                            0, 0,   0,   0,  0,   0,   0, 0,   0,   0,  0,   0,
			                                            0, 0, -12,   0,  6,   0,   0, 0,  12,   0,  6,   0,
			                                            0, 0,   0,   0,  0,   0,   0, 0,   0,   0,  0,   0,
			                                            0, 0,  -6,   0,  2,   0,   0, 0,   6,   0,    4,   0,
			                                            0, 0,   0,   0,  0,   0,   0, 0,   0,   0,  0,   0);
		}
	
		[Test]
		public void CanCreateGlobalStiffnessMatrixForBeamAlignedToGlobalXAxisInReverseOrientation()
		{
		    elementFactory = new ElementFactory(ModelType.Beam1D);
		    beam = elementFactory.CreateLinear1DBeam(end, start, material, section);
			SUT = new Linear1DBeamStiffnessMatrixBuilder(beam);
			StiffnessHelpers.Assert12x12StiffnessMatrix(SUT,
			                                            1, 0,   0,   0,  0,   0,  -1, 0,   0,   0,  0,   0,
			                                            0, 0,   0,   0,  0,   0,   0, 0,   0,   0,  0,   0,
			                                            0, 0,  12,   0,  6,   0,   0, 0, -12,   0,  6,   0,
			                                            0, 0,   0,   0,  0,   0,   0, 0,   0,   0,  0,   0,
			                                            0, 0,   6,   0,  4,   0,   0, 0,  -6,   0,  2,   0,
			                                            0, 0,   0,   0,  0,   0,   0, 0,   0,   0,  0,   0,
			                                            
			                                           -1, 0,   0,   0,  0,   0,   1, 0,   0,   0,  0,   0,
			                                            0, 0,   0,   0,  0,   0,   0, 0,   0,   0,  0,   0,
			                                            0, 0, -12,   0, -6,   0,   0, 0,  12,   0, -6,   0,
			                                            0, 0,   0,   0,  0,   0,   0, 0,   0,   0,  0,   0,
			                                            0, 0,   6,   0,  2,   0,   0, 0,  -6,   0,  4,   0,
			                                            0, 0,   0,   0,  0,   0,   0, 0,   0,   0,  0,   0);
		}
		
		[Test]
		public void StiffnessMatrixReflectsReleases()
		{
			Assert.Ignore();
		}
	}
}
