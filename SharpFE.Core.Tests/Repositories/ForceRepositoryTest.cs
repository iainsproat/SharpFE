/*
 * Created by Iain Sproat
 * Date: 25/09/2012
 * Time: 20:36
 * 
 */
using System;
using System.Collections.Generic;
using NUnit.Framework;
using SharpFE;
using MathNet.Numerics.LinearAlgebra.Double;

namespace SharpFE.Tests.Repositories
{
    [TestFixture]
    public class ForceRepositoryTest
    {
        ForceRepository SUT;
        ForceFactory forceFactory;
        ForceVector exampleForce1;
        ForceVector exampleForce2;
        NodeFactory nodeFactory;
        FiniteElementNode node;
        
        [SetUp]
        public void Setup()
        {
            SUT = new ForceRepository();
            forceFactory = new ForceFactory(ModelType.Truss1D, SUT);
            exampleForce1 = forceFactory.Create(1);
            exampleForce2 = forceFactory.Create(2);
            nodeFactory = new NodeFactory(ModelType.Truss1D);
            node = nodeFactory.Create(0);
        }
        
        
        
        [Test]
        public void CanApplyForcesToNodes()
        {
            SUT.ApplyForceToNode(exampleForce1, node);
            Assert.AreEqual(1, SUT.GetAllForcesAppliedTo(node).Count);
        }
        
        [Test]
        public void CanGetAllForcesAppliedToANode()
        {
            SUT.ApplyForceToNode(exampleForce1, node);
            SUT.ApplyForceToNode(exampleForce2, node);
            
            IList<ForceVector> results = SUT.GetAllForcesAppliedTo(node);
            Assert.IsNotNull(results);
            Assert.AreEqual(2, results.Count);
            Assert.IsTrue(results.Contains(exampleForce1));
            Assert.IsTrue(results.Contains(exampleForce2));
        }
        
        [Test]
        public void CanGetCombinedForceOnNode()
        {
            SUT.ApplyForceToNode(exampleForce1, node);
            SUT.ApplyForceToNode(exampleForce2, node);
            
            ForceVector result = SUT.GetCombinedForceOn(node);
            Assert.IsNotNull(result);
            Assert.AreEqual(6, result.Count); // 6 DOF
            Assert.AreEqual(3, result.X);
            Assert.AreEqual(0, result.Y);
        }
        
        [Test]
        public void CanGetZeroCombinedForceOnNodeWithoutForces()
        {
            Assert.AreEqual(0, SUT.GetAllForcesAppliedTo(node).Count);
            
            ForceVector result = SUT.GetCombinedForceOn(node);
            Assert.IsNotNull(result);
            Assert.AreEqual(6, result.Count); // 6 DOF
            Assert.AreEqual(0, result.X);
            Assert.AreEqual(0, result.Y);
            Assert.AreEqual(0, result.Z);
        }
        
        [Test]
        public void CanGetCombinedForcesOnMultipleNodalDegreeOfFreedom()
        {
            NodalDegreeOfFreedom nodeDof = new NodalDegreeOfFreedom(node, DegreeOfFreedom.X);
            IList<NodalDegreeOfFreedom> nodeDofList  = new List<NodalDegreeOfFreedom>(1)
            {
                nodeDof
            };
            
            Vector result = SUT.GetCombinedForcesFor(nodeDofList);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(0, result[0]);
            
            SUT.ApplyForceToNode(exampleForce2, node);
            result = SUT.GetCombinedForcesFor(nodeDofList);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(2, result[0]);
        }
    }
}
