//-----------------------------------------------------------------------
// <copyright file="NodeFactoryTest.cs" company="SharpFE">
//     Copyright Iain Sproat, 2012.
// </copyright>
//-----------------------------------------------------------------------
using System;
using NUnit.Framework;

namespace SharpFE.Tests.Factory
{
    [TestFixture]
    public class ElementFactoryTest
    {
        NodeFactory nodeFactory;
        FiniteElementNode node1;
        FiniteElementNode node2;
        ElementFactory SUT;
        
        [SetUp]
        public void SetUp()
        {
            nodeFactory = new NodeFactory(ModelType.Truss2D);
            node1 = nodeFactory.Create(0, 0);
            node2 = nodeFactory.Create(0, 1);
            SUT = new ElementFactory();
        }
        
        [Test]
        public void ElementsCanBeCreated()
        {
            Spring result = SUT.CreateSpring(node1, node2, 1);
            Assert.IsNotNull(result);
            Assert.AreEqual(node1, result.StartNode);
            Assert.AreEqual(node2, result.EndNode);
            Assert.AreEqual(1, result.Stiffness);
        }
        
        [Test]
        public void ElementsCanBeCreatedAndAddedToTheRepository()
        {
            ElementRepository repository = new ElementRepository();
            SUT = new ElementFactory(repository);
            Assert.AreEqual(0, repository.Count);
            
            SUT.CreateSpring(node1, node2, 2);
            Assert.AreEqual(1, repository.Count);
        }
    }
}
