//-----------------------------------------------------------------------
// <copyright file="ModelTest.cs" company="SharpFE">
//     Copyright Iain Sproat, 2012.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using NUnit.Framework;
using SharpFE;
using MathNet.Numerics.LinearAlgebra.Double;

namespace SharpFE.Tests
{
    [TestFixture]
    public class ModelTest
    {
        FiniteElementModel SUT;
        FiniteElementNode node1;
        FiniteElementNode node2;
        FiniteElementNode node3;
        Spring spring1;
        Spring spring2;
        ForceVector force1;
        
        [SetUp]
        public void Setup()
        {
            SUT = new FiniteElementModel(ModelType.Truss1D);
            
            node1 = SUT.NodeFactory.Create(0);
            node2 = SUT.NodeFactory.Create(1);
            node3 = SUT.NodeFactory.Create(2);
            spring1 = SUT.ElementFactory.CreateSpring(node1, node2, 3);
            spring2 = SUT.ElementFactory.CreateSpring(node2, node3, 2);
            
            SUT.ConstrainNode(node1, DegreeOfFreedom.X);
            SUT.ConstrainNode(node3, DegreeOfFreedom.X);
            
            force1 = SUT.ForceFactory.Create(20);
            SUT.ApplyForceToNode(force1, node2);
        }
        
        [Test]
        public void CanSetModelType()
        {
            Assert.AreEqual(ModelType.Truss1D, SUT.ModelType);
        }
        
        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CannotCreateNodeOfWrongDOF()
        {
            // try to create a 2D node in a 1D model
            SUT.NodeFactory.Create(0, 0);
        }
        
        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CannotCreateForceOfWrongDOF()
        {
            // try to create a 2D force in a 1D model
            SUT.ForceFactory.Create(0, 0);
        }
        
        
        
        [Test]
        public void GetKnownForceVector()
        {
            Vector result = SUT.KnownForceVector();
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(20, result[0]);
        }
        
        [Test]
        public void GetKnownDisplacementVector()
        {
            Vector result = SUT.KnownDisplacementVector();
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(0, result[0]);
            Assert.AreEqual(0, result[1]);
        }
        
        [Test]
        public void GetUnknownForceVector()
        {
            IList<NodalDegreeOfFreedom> result = SUT.DegreesOfFreedomWithUnknownForce;
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(node1, result[0].Node);
            Assert.AreEqual(DegreeOfFreedom.X, result[0].DegreeOfFreedom);
            Assert.AreEqual(node3, result[1].Node);
            Assert.AreEqual(DegreeOfFreedom.X, result[1].DegreeOfFreedom);
        }
        
        [Test]
        public void GetUnknownDisplacementVector()
        {
            IList<NodalDegreeOfFreedom> result = SUT.DegreesOfFreedomWithUnknownDisplacement;
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(node2, result[0].Node);
            Assert.AreEqual(DegreeOfFreedom.X, result[0].DegreeOfFreedom);
        }
    }
}
