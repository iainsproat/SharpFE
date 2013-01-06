//-----------------------------------------------------------------------
// <copyright file="?.cs" company="Iain Sproat">
//     Copyright Iain Sproat, 2012.
// </copyright>
//-----------------------------------------------------------------------
using System;
using NUnit.Framework;
using MathNet.Numerics.LinearAlgebra.Double;
using SharpFE.Elements;

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
            nodeFactory = new NodeFactory(ModelType.Truss3D);
            start = nodeFactory.Create(1, 2, 4);
            end = nodeFactory.Create(2, 1, 6);
            elementFactory = new ElementFactory();
            SUT = elementFactory.CreateLinearConstantSpring(start, end, 0);
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
        	Vector result = SUT.LocalXAxis;
        	Assert.AreEqual(3, result.Count);
        	Assert.AreEqual(1, result[0]);
        	Assert.AreEqual(-1, result[1]);
        	Assert.AreEqual(2, result[2]);
        }
        
        [Test]
        public void It_can_calculate_the_yAxis()
        {
        	Assert.Ignore();
        }
        
        [Test]
        public void It_can_calculate_the_zAxis()
        {
        	Assert.Ignore();
        }
	}
}