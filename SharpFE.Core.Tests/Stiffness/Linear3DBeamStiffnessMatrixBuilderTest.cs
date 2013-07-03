namespace SharpFE.Core.Tests.Stiffness
{
	using System;
	using NUnit.Framework;
	using SharpFE.Stiffness;
	
	[TestFixture]
	public class Linear3DBeamStiffnessMatrixBuilderTest
	{
		private NodeFactory nodeFactory;
        private ElementFactory elementFactory;
        private FiniteElementNode start;
        private FiniteElementNode end;
        private GenericElasticMaterial material;
        private ICrossSection section;
        private Linear3DBeam beam;
        private ElementStiffnessMatrixBuilder<Linear3DBeam> SUT;
        
        [SetUp]
        public void SetUp()
        {
        	nodeFactory = new NodeFactory(ModelType.Frame2D);
            start = nodeFactory.CreateFor2DTruss(0, 0);
            end = nodeFactory.CreateFor2DTruss(1, 0);
            elementFactory = new ElementFactory(ModelType.Frame2D);
			material = new GenericElasticMaterial(0, 1, 0, 1);
			section = new GenericCrossSection(1, 1, 1, 1);
            beam = elementFactory.CreateLinear3DBeam(start, end, material, section);
            SUT = new Linear3DBeamStiffnessMatrixBuilder(beam);
        }
        
		[Test]
        public void CanCreateGlobalStiffnessMatrixForSpringAlignedToGlobalXAxis()
        {
			StiffnessHelpers.Assert12x12StiffnessMatrix(SUT,
			                                             1,   0,    0,    0,  0,    0,      -1,   0,    0,    0,  0,    0,
			                                             0,  12,    0,    0,  0,    6,       0, -12,    0,    0,  0,    6,
			                                             0,   0,   12,    0, -6,    0,       0,   0,  -12,    0, -6,    0,
			                                             0,   0,    0,    1,  0,    0,       0,   0,    0,   -1,  0,    0,
			                                             0,   0,   -6,    0,  4,    0,       0,   0,    6,    0,  2,    0,
			                                             0,   6,    0,    0,  0,    4,       0,  -6,    0,    0,  0,    2,
			                                            
			                                            -1,   0,    0,    0,  0,    0,       1,   0,    0,    0,  0,    0,
			                                             0, -12,    0,    0,  0,   -6,       0,  12,    0,    0,  0,   -6,
			                                             0,   0,  -12,    0,  6,    0,       0,   0,   12,    0,  6,    0,
			                                             0,   0,    0,   -1,  0,    0,       0,   0,    0,    1,  0,    0,
			                                             0,   0,   -6,    0,  2,    0,       0,   0,    6,    0,  4,    0,
			                                             0,   6,    0,    0,  0,    2,       0,  -6,    0,    0,  0,    4);
        }
        
        [Test]
        public void StiffnessMatrixReflectsReleases()
        {
            Assert.Ignore();
        }
	}
}
