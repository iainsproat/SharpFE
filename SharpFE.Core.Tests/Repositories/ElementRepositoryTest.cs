/*
 * Created by Iain Sproat
 * Date: 25/09/2012
 * Time: 19:41
 * 
 */
using System;
using System.Collections.Generic;
using NUnit.Framework;
using SharpFE;
using MathNet.Numerics.LinearAlgebra.Double;

namespace SharpFE.Core.Tests.Repositories
{
    [TestFixture]
    public class ElementRepositoryTest
    {
        NodeFactory nodeFactory;
        FiniteElementNode node1;
        FiniteElementNode node2;
        FiniteElementNode node3;
        LinearConstantSpring spring1;
        LinearConstantSpring spring2;
        ElementRepository SUT;
        ElementFactory elementFactory;
        
        [SetUp]
        public void Setup()
        {
            nodeFactory = new NodeFactory(ModelType.Truss2D);
            node1 = nodeFactory.CreateForTruss(0, 0);
            node2 = nodeFactory.CreateForTruss(0, 1);
            node3 = nodeFactory.CreateForTruss(0, 2);
            SUT = new ElementRepository();
            elementFactory = new ElementFactory(SUT);
            spring1 = elementFactory.CreateLinearConstantSpring(node1, node2, 1);
            spring2 = elementFactory.CreateLinearConstantSpring(node2, node3, 2);
        }
        
        [Test]
        public void AllElementsConnectedToANodeCanBeFound()
        {
            IList<IFiniteElement> results = SUT.GetAllElementsConnectedTo(node1);
            Assert.IsNotNull(results);
            Assert.AreEqual(1, results.Count);
            Assert.IsTrue(results.Contains(spring1));
            
            results = SUT.GetAllElementsConnectedTo(node2);
            Assert.IsNotNull(results);
            Assert.AreEqual(2, results.Count);
            Assert.IsTrue(results.Contains(spring1));
            Assert.IsTrue(results.Contains(spring2));
            
            results = SUT.GetAllElementsConnectedTo(node3);
            Assert.IsNotNull(results);
            Assert.AreEqual(1, results.Count);
            Assert.IsTrue(results.Contains(spring2));
            
            FiniteElementNode unconnectedNode = nodeFactory.CreateForTruss(0,3);
            results = SUT.GetAllElementsConnectedTo(unconnectedNode);
            Assert.IsNotNull(results);
            Assert.AreEqual(0, results.Count);
        }
        
        [Test]
        public void Can_get_all_elements_directly_connecting_two_nodes()
        {
            IList<IFiniteElement> results = SUT.GetAllElementsDirectlyConnecting(node1, node2);
            Assert.IsNotNull(results);
            Assert.AreEqual(1, results.Count);
            Assert.IsTrue(results.Contains(spring1));
            
            results = SUT.GetAllElementsDirectlyConnecting(node2, node3);
            Assert.IsNotNull(results);
            Assert.AreEqual(1, results.Count);
            Assert.IsTrue(results.Contains(spring2));
        }
        
        [Test]
        public void Can_get_all_elements_directly_connecting_the_same_node()
        {
            IList<IFiniteElement> results = SUT.GetAllElementsDirectlyConnecting(node1, node1);
            Assert.IsNotNull(results);
            Assert.AreEqual(1, results.Count);
            Assert.IsTrue(results.Contains(spring1));
            
            results = SUT.GetAllElementsDirectlyConnecting(node2, node2);
            Assert.IsNotNull(results);
            Assert.AreEqual(2, results.Count);
            Assert.IsTrue(results.Contains(spring1));
            Assert.IsTrue(results.Contains(spring2));
        }
        
        [Test]
        public void Will_return_empty_list_if_no_elements_directly_connect_nodes()
        {
            IList<IFiniteElement> results = SUT.GetAllElementsDirectlyConnecting(node1, node3);
            Assert.IsNotNull(results);
            Assert.AreEqual(0, results.Count);
        }
        
        [Test]
        public void AllNodesConnectedViaElementsToANodeCanBeFound()
        {
            IList<IFiniteElementNode> results = SUT.GetAllNodesConnectedViaElementsTo(node1);
            Assert.IsNotNull(results);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(node2, results[0]);
            
            results = SUT.GetAllNodesConnectedViaElementsTo(node2);
            Assert.IsNotNull(results);
            Assert.AreEqual(2, results.Count);
            Assert.IsTrue(results.Contains(node1));
            Assert.IsTrue(results.Contains(node3));
            
            results = SUT.GetAllNodesConnectedViaElementsTo(node3);
            Assert.IsNotNull(results);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(node2, results[0]);
        }
    }
}
