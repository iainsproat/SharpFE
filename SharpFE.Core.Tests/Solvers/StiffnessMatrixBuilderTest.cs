//-----------------------------------------------------------------------
// <copyright file=StiffnessMatrixBuilderTest.cs" company="SharpFE">
//     Copyright Iain Sproat, 2012.
// </copyright>
//-----------------------------------------------------------------------

namespace SharpFE.Core.Tests.Solvers
{
    using System;
    using NUnit.Framework;
    using MathNet.Numerics.LinearAlgebra.Double;
    using SharpFE;
    
    [TestFixture]
    public class StiffnessMatrixBuilderTest
    {
        FiniteElementModel model;
        FiniteElementNode node1;
        FiniteElementNode node2;
        FiniteElementNode node3;
        LinearConstantSpring spring1;
        LinearConstantSpring spring2;
        StiffnessMatrixBuilder SUT;
        
        [SetUp]
        public void Setup()
        {
            model = new FiniteElementModel(ModelType.Truss1D);
            
            node1 = model.NodeFactory.Create(0);
            node2 = model.NodeFactory.Create(1);
            node3 = model.NodeFactory.Create(2);
            spring1 = model.ElementFactory.CreateLinearConstantSpring(node1, node2, 3);
            spring2 = model.ElementFactory.CreateLinearConstantSpring(node2, node3, 2);
            
            model.ConstrainNode(node1, DegreeOfFreedom.X);
            model.ConstrainNode(node3, DegreeOfFreedom.X);
            
            SUT = new StiffnessMatrixBuilder(model);
        }
        
        [Test]
        public void KnownForcesKnownDisplacementsMatrixCanBeGenerated()
        {
            Matrix result = SUT.BuildKnownForcesKnownDisplacementStiffnessMatrix();
            Helpers.AssertMatrix(result, 1, 2, -3, -2);
        }
        
        [Test]
        public void KnownForcesUnknownDisplacementsMatrixCanBeGenerated()
        {
            Matrix result = SUT.BuildKnownForcesUnknownDisplacementStiffnessMatrix();
            Helpers.AssertMatrix(result, 1, 1, 5);
        }
        
        [Test]
        public void UnknownForcesKnownDisplacementsMatrixCanBeGenerated()
        {
            Matrix result = SUT.BuildUnknownForcesKnownDisplacementStiffnessMatrix();
            Helpers.AssertMatrix(result, 2, 2, 3, 0, 0, 2);
        }
        
        [Test]
        public void UnknownForcesUnknownDisplacementsMatrixCanBeGenerated()
        {
            Matrix result = SUT.BuildUnknownForcesUnknownDisplacementStiffnessMatrix();
            Helpers.AssertMatrix(result, 2, 1, -3, -2);
        }
    }
}
