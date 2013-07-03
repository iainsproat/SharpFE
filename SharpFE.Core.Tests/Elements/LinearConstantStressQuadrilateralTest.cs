//-----------------------------------------------------------------------
// <copyright file="BeamIn1DModel.cs" company="Iain Sproat">
//     Copyright Iain Sproat, 2012.
// </copyright>
//-----------------------------------------------------------------------


namespace SharpFE.Core.Tests.Elements
{
	using System;
	using NUnit.Framework;
	using SharpFE.Geometry;

	[TestFixture]
	public class LinearConstantStressQuadrilateralTest
	{
		private NodeFactory nodeFactory;
        private ElementFactory elementFactory;
        private FiniteElementNode node0;
        private FiniteElementNode node1;
        private FiniteElementNode node2;
        private FiniteElementNode node3;
		private GenericElasticMaterial material;
        private LinearConstantStressQuadrilateral SUT;
        
        [SetUp]
        public void SetUp()
        {
            nodeFactory = new NodeFactory(ModelType.Slab2D);
            node0 = nodeFactory.Create(1, 1);
            node1 = nodeFactory.Create(3, 1);
            node2 = nodeFactory.Create(5, 4);
            node3 = nodeFactory.Create(2, 3);
			material = new GenericElasticMaterial(0, 0.1, 0, 0);
            elementFactory = new ElementFactory(ModelType.Slab2D);
            SUT = elementFactory.CreateLinearConstantStressQuadrilateral(node0, node1, node2, node3, material, 0.1);
        }
        
		[Test]
		public void It_can_be_constructed()
		{
			Assert.IsNotNull(SUT);
			Assert.AreEqual(4, SUT.Nodes.Count);
			Assert.AreEqual(material, SUT.Material);
			Assert.AreEqual(0.1, SUT.Thickness);
		}
		
		[Test]
		public void It_cannot_be_constructed_from_four_aligned_nodes()
		{
			Assert.Ignore();
		}
		
		[Test]
		public void It_will_warn_if_nodes_are_too_close()
		{
			Assert.Ignore();
		}
		
		[Test]
		public void It_can_calculate_X_axis()
		{
			GeometricVector result = SUT.LocalXAxis;
			Assert.IsNotNull(result);
			Assert.AreEqual(2, result[DegreeOfFreedom.X]);
			Assert.AreEqual(0, result[DegreeOfFreedom.Y]);
			Assert.AreEqual(0, result[DegreeOfFreedom.Z]);
		}
		
		[Test]
		public void It_can_calculate_Y_axis()
		{
			GeometricVector result = SUT.LocalYAxis;
			Assert.IsNotNull(result);
			Assert.AreEqual(0, result[DegreeOfFreedom.X]);
			Assert.AreEqual(2, result[DegreeOfFreedom.Y]);
			Assert.AreEqual(0, result[DegreeOfFreedom.Z]);
		}
		
		[Test]
		public void It_can_calculate_Area()
		{
			Assert.AreEqual(5.5, SUT.Area);
		}
	}
}
