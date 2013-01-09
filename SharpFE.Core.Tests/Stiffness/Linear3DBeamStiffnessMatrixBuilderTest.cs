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
        private SolidRectangle section;
        private Linear3DBeam beam;
        private IStiffnessMatrixBuilder<Linear3DBeam> SUT;
        
        [SetUp]
        public void SetUp()
        {
        	nodeFactory = new NodeFactory(ModelType.Frame2D);
            start = nodeFactory.CreateForTruss(0, 0);
            end = nodeFactory.CreateForTruss(1, 0);
            elementFactory = new ElementFactory();
			material = new GenericElasticMaterial(0, 1, 0, 1);
			section = new SolidRectangle(1, 1);
            beam = elementFactory.CreateLinear3DBeam(start, end, material, section);
            SUT = new Linear3DBeamStiffnessMatrixBuilder(beam);
        }
        
		[Test]
        public void CanCreateGlobalStiffnessMatrixForSpringAlignedToGlobalXAxis()
        {
            double a = 1.0 / 3.0;
			double b = a / 2.0;
			double c = 0.1408333; //torsion
			StiffnessHelpers.Assert12x12StiffnessMatrix(SUT,
			                                             1,  0,    0,    0,  0,    0,      -1,  0,    0,    0,  0,    0,
			                                             0,  1,    0,    0,  0,    0.5,     0, -1,    0,    0,  0,    0.5,
			                                             0,  0,    1,    0, -0.5,  0,       0,  0,   -1,    0, -0.5,  0,
			                                             0,  0,    0,    c,  0,    0,       0,  0,    0,   -c,  0,    0,
			                                             0,  0,   -0.5,  0,  a,    0,       0,  0,    0.5,  0,  b,    0,
			                                             0,  0.5,  0,    0,  0,    a,       0, -0.5,  0,    0,  0,    b,
			                                            
			                                            -1,  0,    0,    0,  0,    0,       1,  0,    0,    0,  0,    0,
			                                             0, -1,    0,    0,  0,   -0.5,     0,  1,    0,    0,  0,   -0.5,
			                                             0,  0,   -1,    0,  0.5,  0,       0,  0,    1,    0,  0.5,  0,
			                                             0,  0,    0,   -c,  0,    0,       0,  0,    0,    c,  0,    0,
			                                             0,  0,   -0.5,  0,  b,    0,       0,  0,    0.5,  0,  a,    0,
			                                             0,  0.5,  0,    0,  0,    b,       0, -0.5,  0,    0,  0,    a);
        }
        
        [Test]
        public void StiffnessMatrixReflectsReleases()
        {
            Assert.Ignore();
        }
	}
}
