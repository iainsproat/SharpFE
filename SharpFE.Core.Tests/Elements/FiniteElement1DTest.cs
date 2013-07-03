//-----------------------------------------------------------------------
// <copyright file="?.cs" company="Iain Sproat">
//     Copyright Iain Sproat, 2012.
// </copyright>
//-----------------------------------------------------------------------
using System;
using NUnit.Framework;
using SharpFE.Elements;
using SharpFE.Geometry;

namespace SharpFE.Core.Tests.Elements
{
	/// <summary>
	/// Tests methods on the FiniteElement1D abstract class
	/// </summary>
	[TestFixture]
	public class FiniteElement1DTest
	{
		private NodeFactory nodeFactory;
        private ElementFactory elementFactory;
        private FiniteElementNode start;
        private FiniteElementNode end;
        private FiniteElement1D SUT;
        
        [SetUp]
        public void SetUp()
        {
            this.CreateFiniteElement1D(1, 2, 4, 2, 1, 6);
        }
        
        [Test]
        public void Nodes_can_be_found_at_the_ends_of_each_element()
        {
            Assert.IsNotNull(SUT.StartNode);
            Assert.IsNotNull(SUT.EndNode);
        }
        
        [Test]
        public void It_can_calculate_the_xAxis()
        {
        	GeometricVector result = SUT.LocalXAxis;
        	Assert.AreEqual(3, result.Count);
        	Assert.AreEqual(1, result[DegreeOfFreedom.X]);
        	Assert.AreEqual(-1, result[DegreeOfFreedom.Y]);
        	Assert.AreEqual(2, result[DegreeOfFreedom.Z]);
        }
        
        [Test]
        public void It_can_calculate_the_yAxis()
        {
        	Assert.Ignore();
        }
        
        [Test]
        public void It_can_calculate_the_yAxis_for_vertical_beam()
        {
            this.CreateFiniteElement1D(-10, 0, 0, -10, 0, 10);
            
            GeometricVector axisY = SUT.LocalYAxis;
            
            Assert.AreNotEqual(0, axisY.SumMagnitudes());
        }
        
        [Test]
        public void It_can_calculate_the_zAxis()
        {
        	Assert.Ignore();
        }
        
        private void CreateFiniteElement1D(double startX, double startY, double startZ, double endX, double endY, double endZ)
        {
            nodeFactory = new NodeFactory(ModelType.Truss3D);
            start = nodeFactory.Create(startX, startY, startZ);
            end = nodeFactory.Create(endX, endY, endZ);
            elementFactory = new ElementFactory(ModelType.Truss3D);
            SUT = elementFactory.CreateLinearConstantSpring(start, end, 0);
        }
	}
}