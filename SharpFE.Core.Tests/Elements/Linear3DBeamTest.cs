namespace SharpFE.Core.Tests.Elements
{
	using System;
	using NUnit.Framework;
	using SharpFE;

	public class Linear3DBeamTest
	{
		private NodeFactory nodeFactory;
        private ElementFactory elementFactory;
        private FiniteElementNode start;
        private FiniteElementNode end;
        private GenericElasticMaterial material;
        private SolidRectangle section;
        private Linear3DBeam SUT;
        
        [SetUp]
        public void SetUp()
        {
        	nodeFactory = new NodeFactory(ModelType.Truss1D);
            start = nodeFactory.Create(0);
            end = nodeFactory.Create(1);
            elementFactory = new ElementFactory(ModelType.Truss1D);
			material = new GenericElasticMaterial(0, 0.1, 0, 0);
			section = new SolidRectangle(0.1, 1);
            SUT = elementFactory.CreateLinear3DBeam(start, end, material, section);
        }
        
		[Test]
        public void It_can_be_constructed()
        {
        	Assert.IsNotNull(SUT);
        	Assert.AreEqual(material, SUT.Material);
        	Assert.AreEqual(section, SUT.CrossSection);
        }
        
        [Test]
        public void HasCorrectSupportedDOF()
        {
        	Assert.IsTrue(SUT.IsASupportedBoundaryConditionDegreeOfFreedom(DegreeOfFreedom.X));
        	Assert.IsTrue(SUT.IsASupportedBoundaryConditionDegreeOfFreedom(DegreeOfFreedom.Y));
        	Assert.IsTrue(SUT.IsASupportedBoundaryConditionDegreeOfFreedom(DegreeOfFreedom.Z));
        	Assert.IsTrue(SUT.IsASupportedBoundaryConditionDegreeOfFreedom(DegreeOfFreedom.XX));
        	Assert.IsTrue(SUT.IsASupportedBoundaryConditionDegreeOfFreedom(DegreeOfFreedom.YY));
        	Assert.IsTrue(SUT.IsASupportedBoundaryConditionDegreeOfFreedom(DegreeOfFreedom.ZZ));
        }
        
        [Test]
        public void ElementEndsCanBeReleased()
        {
            Assert.Ignore();
        }
	}
}
