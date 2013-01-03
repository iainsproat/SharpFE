/*
 * Created by Iain Sproat
 * Date: 25/09/2012
 * Time: 19:02
 * 
 */
using System;
using System.Collections.Generic;
using NUnit.Framework;
using SharpFE;

namespace SharpFE.Core.Tests.Repositories
{
    [TestFixture]
    public class NodeRepositoryTest
    {
        NodeRepository SUT;
        NodeFactory nodeFactory;

        FiniteElementNode node1;
        FiniteElementNode node2;
        FiniteElementNode node3;
        
        [SetUp]
        public void Setup()
        {
            SUT = new NodeRepository(ModelType.Truss1D);
            nodeFactory = new NodeFactory(ModelType.Truss1D, SUT);
            node1 = nodeFactory.Create(0);
            node2 = nodeFactory.Create(1);
            node3 = nodeFactory.Create(2);
        }
        
        [Test]
        public void NodesCanBeMadeDeactivatedButNotDeleted()
        {
            Assert.Ignore();
        }
        
        [Test]
        public void NodesCanBeSearchedForByLocation()
        {
            Assert.Ignore();
        }
        
        [Test]
        public void NodesCanHaveAConstraint()
        {
            SUT = new NodeRepository(ModelType.Full3D);
            nodeFactory = new NodeFactory(ModelType.Full3D, SUT);
            node1 = nodeFactory.Create(0, 0, 0);
            
            Assert.IsFalse(SUT.IsConstrained(node1, DegreeOfFreedom.X));
            Assert.IsFalse(SUT.IsConstrained(node1, DegreeOfFreedom.Y));
            Assert.IsFalse(SUT.IsConstrained(node1, DegreeOfFreedom.Z));
            Assert.IsFalse(SUT.IsConstrained(node1, DegreeOfFreedom.XX));
            Assert.IsFalse(SUT.IsConstrained(node1, DegreeOfFreedom.YY));
            Assert.IsFalse(SUT.IsConstrained(node1, DegreeOfFreedom.ZZ));
            
            SUT.ConstrainNode(node1, DegreeOfFreedom.X);
            Assert.IsTrue(SUT.IsConstrained(node1, DegreeOfFreedom.X));
            Assert.IsFalse(SUT.IsConstrained(node1, DegreeOfFreedom.Y));
            
            SUT.ConstrainNode(node1, DegreeOfFreedom.Y);
            Assert.IsTrue(SUT.IsConstrained(node1, DegreeOfFreedom.X));
            Assert.IsTrue(SUT.IsConstrained(node1, DegreeOfFreedom.Y));
            
            SUT.ConstrainNode(node1, DegreeOfFreedom.Z);
            Assert.IsTrue(SUT.IsConstrained(node1, DegreeOfFreedom.Z));
            
            SUT.ConstrainNode(node1, DegreeOfFreedom.XX);
            Assert.IsTrue(SUT.IsConstrained(node1, DegreeOfFreedom.XX));
            
            SUT.ConstrainNode(node1, DegreeOfFreedom.YY);
            Assert.IsTrue(SUT.IsConstrained(node1, DegreeOfFreedom.YY));
            
            SUT.ConstrainNode(node1, DegreeOfFreedom.ZZ);
            Assert.IsTrue(SUT.IsConstrained(node1, DegreeOfFreedom.ZZ));
        }
        
        [Test]
        public void ConstrainedNodesCanBeFreed()
        {
            Assert.IsFalse(SUT.IsConstrained(node1, DegreeOfFreedom.X));
            Assert.AreEqual(3, SUT.UnconstrainedNodalDegreeOfFreedoms.Count);
            
            SUT.ConstrainNode(node1, DegreeOfFreedom.X);
            Assert.IsTrue(SUT.IsConstrained(node1, DegreeOfFreedom.X));
            Assert.AreEqual(1, SUT.ConstrainedNodalDegreeOfFreedoms.Count);
            Assert.AreEqual(2, SUT.UnconstrainedNodalDegreeOfFreedoms.Count);
            
            SUT.UnconstrainNode(node1, DegreeOfFreedom.X);
            Assert.IsFalse(SUT.IsConstrained(node1, DegreeOfFreedom.X));
            Assert.AreEqual(0, SUT.ConstrainedNodalDegreeOfFreedoms.Count);
            Assert.AreEqual(3, SUT.UnconstrainedNodalDegreeOfFreedoms.Count);
        }
        
        [Test]
        public void CanGetAllKnownDisplacements()
        {
            SUT.ConstrainNode(node1, DegreeOfFreedom.X);
            SUT.ConstrainNode(node3, DegreeOfFreedom.X);
            
            IList<NodalDegreeOfFreedom> result = SUT.ConstrainedNodalDegreeOfFreedoms;
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(node1, result[0].Node);
            Assert.AreEqual(DegreeOfFreedom.X, result[0].DegreeOfFreedom);
            
            Assert.AreEqual(node3, result[1].Node);
            Assert.AreEqual(DegreeOfFreedom.X, result[1].DegreeOfFreedom);
        }
        
        [Test]
        public void CanGetAllUnknownDisplacements()
        {
            SUT.ConstrainNode(node1, DegreeOfFreedom.X);
            SUT.ConstrainNode(node3, DegreeOfFreedom.X);
            
            IList<NodalDegreeOfFreedom> result = SUT.UnconstrainedNodalDegreeOfFreedoms;
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(node2, result[0].Node);
            Assert.AreEqual(DegreeOfFreedom.X, result[0].DegreeOfFreedom);
        }
        
        [Test]
        public void CanGetAllKnownForces()
        {
            SUT.ConstrainNode(node1, DegreeOfFreedom.X);
            SUT.ConstrainNode(node3, DegreeOfFreedom.X);
            
            IList<NodalDegreeOfFreedom> result = SUT.UnconstrainedNodalDegreeOfFreedoms;
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(node2, result[0].Node);
            Assert.AreEqual(DegreeOfFreedom.X, result[0].DegreeOfFreedom);
        }
        
        [Test]
        public void CanGetAllUnknownForces()
        {
            SUT.ConstrainNode(node1, DegreeOfFreedom.X);
            SUT.ConstrainNode(node3, DegreeOfFreedom.X);
            
            IList<NodalDegreeOfFreedom> result = SUT.ConstrainedNodalDegreeOfFreedoms;
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(node1, result[0].Node);
            Assert.AreEqual(DegreeOfFreedom.X, result[0].DegreeOfFreedom);
            Assert.AreEqual(node3, result[1].Node);
            Assert.AreEqual(DegreeOfFreedom.X, result[1].DegreeOfFreedom);
        }
    }
}
