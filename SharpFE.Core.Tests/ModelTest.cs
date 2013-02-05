//-----------------------------------------------------------------------
// <copyright file="ModelTest.cs" company="SharpFE">
//     Copyright Iain Sproat, 2012.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using NUnit.Framework;
using SharpFE;

namespace SharpFE.Core.Tests
{
    [TestFixture]
    public class ModelTest
    {
        FiniteElementModel SUT;
        FiniteElementNode node1;
        FiniteElementNode node2;
        FiniteElementNode node3;
        LinearConstantSpring spring1;
        LinearConstantSpring spring2;
        ForceVector force1;
        
        [SetUp]
        public void Setup()
        {
            SUT = new FiniteElementModel(ModelType.Truss1D);
            
            node1 = SUT.NodeFactory.Create(0);
            node2 = SUT.NodeFactory.Create(1);
            node3 = SUT.NodeFactory.Create(2);
            spring1 = SUT.ElementFactory.CreateLinearConstantSpring(node1, node2, 3);
            spring2 = SUT.ElementFactory.CreateLinearConstantSpring(node2, node3, 2);
            
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
        public void GetConstrainedNodalDegreesOfFreedom()
        {
            IList<NodalDegreeOfFreedom> result = SUT.ConstrainedNodalDegreeOfFreedoms;
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(node1, result[0].Node);
            Assert.AreEqual(DegreeOfFreedom.X, result[0].DegreeOfFreedom);
            Assert.AreEqual(node3, result[1].Node);
            Assert.AreEqual(DegreeOfFreedom.X, result[1].DegreeOfFreedom);
        }
        
        [Test]
        public void GetUnconstrainedNodalDegreesOfFreedom()
        {
            IList<NodalDegreeOfFreedom> result = SUT.UnconstrainedNodalDegreeOfFreedoms;
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(node2, result[0].Node);
            Assert.AreEqual(DegreeOfFreedom.X, result[0].DegreeOfFreedom);
        }
        
        [Test]
        public void GetKnownForceVector()
        {
            KeyedVector<NodalDegreeOfFreedom> result = SUT.KnownForceVector();
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            
            // can be accessed by a key
            NodalDegreeOfFreedom ndof1 = new NodalDegreeOfFreedom(node2, DegreeOfFreedom.X);
            Assert.AreEqual(20, result[ndof1]);
        }
        
        [Test]
        public void GetKnownDisplacementVector()
        {
            KeyedVector<NodalDegreeOfFreedom> result = SUT.KnownDisplacementVector();
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            
            // can be accessed by a key
            NodalDegreeOfFreedom ndof1 = new NodalDegreeOfFreedom(node1, DegreeOfFreedom.X);
            NodalDegreeOfFreedom ndof2 = new NodalDegreeOfFreedom(node3, DegreeOfFreedom.X);
            Assert.AreEqual(0, result[ndof1]);
            Assert.AreEqual(0, result[ndof2]);
        }
    }
}
