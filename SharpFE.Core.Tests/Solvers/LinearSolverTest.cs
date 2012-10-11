//-----------------------------------------------------------------------
// <copyright file=LinearSolverTest.cs" company="SharpFE">
//     Copyright Iain Sproat, 2012.
// </copyright>
//-----------------------------------------------------------------------

using System;
using NUnit.Framework;
using SharpFE;

namespace SharpFE.Tests.Solvers
{
    [TestFixture]
    public class LinearSolverTest
    {
        FiniteElementModel model;
        FiniteElementNode node1;
        FiniteElementNode node2;
        FiniteElementNode node3;
        Spring spring1;
        Spring spring2;
        ForceVector force1;
        
        LinearSolver SUT;
        
        [SetUp]
        public void SetUp()
        {
            model = new FiniteElementModel(ModelType.Truss1D);
            node1 = model.NodeFactory.Create(0);
            node2 = model.NodeFactory.Create(1);
            node3 = model.NodeFactory.Create(2);
            
            spring1 = model.ElementFactory.CreateSpring(node1, node2, 3);
            spring2 = model.ElementFactory.CreateSpring(node2, node3, 2);
            
            model.ConstrainNode(node1, DegreeOfFreedom.X);
            model.ConstrainNode(node3, DegreeOfFreedom.X);
            
            force1 = model.ForceFactory.Create(20);
            model.ApplyForceToNode(force1, node2);
            
            SUT = new LinearSolver(model);
        }
        
        [Test]
        public void CanSolveSimple1DOFSpring()
        {
            FiniteElementResults results = SUT.Solve();
            Assert.IsNotNull(results);
            Assert.AreEqual(4, results.GetDisplacement(node2).X);
            Assert.AreEqual(-12, results.GetReaction(node1).X);
            Assert.AreEqual(-8, results.GetReaction(node3).X);
        }
        
        [Test]
        public void CanCalculateCorrectReactionsWithForcesAppliedToConstrainedNodes()
        {
            model.ApplyForceToNode(force1, node1);
            
            FiniteElementResults results = SUT.Solve();
            Assert.IsNotNull(results);
            Assert.AreEqual(4, results.GetDisplacement(node2).X);
            Assert.AreEqual(8, results.GetReaction(node1).X);
            Assert.AreEqual(-8, results.GetReaction(node3).X);
        }
        
        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void WillCheckForInsufficientNumberOfNodes()
        {
            model = new FiniteElementModel(ModelType.Truss1D);
            node1 = model.NodeFactory.Create(0);
            SUT = new LinearSolver(model);
            SUT.Solve();
        }
        
        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void WillCheckForInsufficientNumberOfElements()
        {
            model = new FiniteElementModel(ModelType.Truss1D);
            node1 = model.NodeFactory.Create(0);
            node2 = model.NodeFactory.Create(1);
            SUT = new LinearSolver(model);
            SUT.Solve();
        }
        
        [Test]
        public void WillCheckForOrphanedNodesInModel()
        {
            Assert.Ignore();
        }
        
        [Test]
        public void WillCheckForUnjoinedModel()
        {
            // no continuous path of elements connecting all nodes. i.e. there are actually two or more models
            Assert.Ignore();
        }
        
        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void WillCheckForUnsufficientlyConstrainedModel()
        {
            // remove all constraints
            model.UnconstrainNode(node1, DegreeOfFreedom.X);
            model.UnconstrainNode(node3, DegreeOfFreedom.X);
            
            SUT.Solve();
        }
        
        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void WillCheckForTotallyConstrainedModel()
        {
            model.ConstrainNode(node2, DegreeOfFreedom.X);
            
            SUT.Solve();
        }
    }
}
