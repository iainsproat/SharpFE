

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
        private IStiffnessMatrixBuilder SUT;
        
        [SetUp]
        public void SetUp()
        {
        	nodeFactory = new NodeFactory(ModelType.Truss2D);
            start = nodeFactory.Create(0, 0);
            end = nodeFactory.Create(1, 0);
            elementFactory = new ElementFactory();
			material = new GenericElasticMaterial(0, 2, 0, 3);
			section = new SolidRectangle(5, 1);
            beam = elementFactory.CreateLinear3DBeam(start, end, material, section);
            SUT = this.beam.StiffnessBuilder;
        }
        
		[Test]
        public void CanCanCreateGlobalStiffnessMatrixForSpringAlignedToGlobalXAxis()
        {
            StiffnessHelpers.Assert12x12StiffnessMatrix(SUT, 1, 0, 0, 0, 0, 0, -1, 0, 0, 0, 0, 0,
                                            0, 0, 0, 0, 0, 0,  0, 0, 0, 0, 0, 0,
                                            0, 0, 0, 0, 0, 0,  0, 0, 0, 0, 0, 0,
                                            0, 0, 0, 0, 0, 0,  0, 0, 0, 0, 0, 0,
                                            0, 0, 0, 0, 0, 0,  0, 0, 0, 0, 0, 0,
                                            0, 0, 0, 0, 0, 0,  0, 0, 0, 0, 0, 0,
                                            -1, 0, 0, 0, 0, 0,  1, 0, 0, 0, 0, 0,
                                            0, 0, 0, 0, 0, 0,  0, 0, 0, 0, 0, 0,
                                            0, 0, 0, 0, 0, 0,  0, 0, 0, 0, 0, 0,
                                            0, 0, 0, 0, 0, 0,  0, 0, 0, 0, 0, 0,
                                            0, 0, 0, 0, 0, 0,  0, 0, 0, 0, 0, 0,
                                            0, 0, 0, 0, 0, 0,  0, 0, 0, 0, 0, 0);
        }
        
		[Test]
        public void ElementHasAStiffnessAgainstBending()
        {
            Assert.Ignore();
        }
        
        [Test]
        public void ElementHasAStiffnessAgainstShear()
        {
            Assert.Ignore();
        }
        
        [Test]
        public void StiffnessMatrixReflectsReleases()
        {
            Assert.Ignore();
        }
	}
}
