/*
 * Created by Iain Sproat
 * Date: 25/09/2012
 * Time: 20:23
 * 
 */
using System;
using NUnit.Framework;
using SharpFE;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra.Double;

namespace SharpFE.Core.Tests.Elements
{
    [TestFixture]
    public class LinearConstantSpringTest
    {
    	private NodeFactory nodeFactory;
        private ElementFactory elementFactory;
        private FiniteElementNode start;
        private FiniteElementNode end;
        private LinearConstantSpring SUT;
        
        [SetUp]
        public void SetUp()
        {
            nodeFactory = new NodeFactory(ModelType.Truss1D);
            start = nodeFactory.Create(0);
            end = nodeFactory.Create(1);
            elementFactory = new ElementFactory();
            SUT = elementFactory.CreateLinearConstantSpring(start, end, 2);
        }
        
        [Test]
        public void CanCreateSpringElement()
        {
            Assert.IsNotNull(SUT);
        }
        
        [Test]
        public void NodesCanBeFoundAtEachEndOfElement()
        {
            Assert.IsNotNull(SUT.StartNode);
            Assert.IsNotNull(SUT.EndNode);
        }
        
        [Test]
        public void CanGetAllNodesOfElement()
        {
            IList<FiniteElementNode> result = SUT.Nodes;
            
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(start, result[0]);
            Assert.AreEqual(end, result[1]);
        }
        
        [Test]
        public void Has_initialized_all_supported_degrees_of_freedom()
        {
        	IList<NodalDegreeOfFreedom> result = SUT.SupportedNodalDegreeOfFreedoms;
        	Assert.IsNotNull(result);
        	Assert.AreEqual(12, result.Count);
        }
        
        [Test]
        public void ElementHasASpringConstant()
        {
            Assert.AreEqual(2, SUT.SpringConstant);
        }
  

    }
}
