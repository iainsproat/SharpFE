//-----------------------------------------------------------------------
// <copyright file=LinearSolverTest.cs" company="SharpFE">
//     Copyright Iain Sproat, 2012.
// </copyright>
//-----------------------------------------------------------------------

using System;
using NUnit.Framework;
using SharpFE;

namespace SharpFE.Core.Tests.Solvers
{
    [TestFixture]
    public class LinearSolverTest
    {
        FiniteElementModel model;
        FiniteElementNode node1;
        FiniteElementNode node2;
        LinearConstantSpring spring1;
        ForceVector force1;
        
        IFiniteElementSolver SUT;
        
        [SetUp]
        public void SetUp()
        {
            model = new FiniteElementModel(ModelType.Truss1D);
            node1 = model.NodeFactory.Create(0);
            node2 = model.NodeFactory.Create(1);
            
            spring1 = model.ElementFactory.CreateLinearConstantSpring(node1, node2, 4);
            
            model.ConstrainNode(node1, DegreeOfFreedom.X);
            
            force1 = model.ForceFactory.Create(20);
            model.ApplyForceToNode(force1, node2);
            
            SUT = new LinearSolverSVD(model);
        }
        
        [Test]
        public void CanSolveSimple1DOFSpring()
        {
            FiniteElementResults results = SUT.Solve();
            Assert.IsNotNull(results);
            Assert.AreEqual(5, results.GetDisplacement(node2).X);
            Assert.AreEqual(-20, results.GetReaction(node1).X);
        }
        
        [Test]
        public void CanCalculateCorrectReactionsWithForcesAppliedToConstrainedNodes()
        {
            model.ApplyForceToNode(force1, node1);
            
            FiniteElementResults results = SUT.Solve();
            Assert.IsNotNull(results);
            Assert.AreEqual(5, results.GetDisplacement(node2).X);
            Assert.AreEqual(0, results.GetReaction(node1).X);
        }
        
        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void WillCheckForInsufficientNumberOfNodes()
        {
            model = new FiniteElementModel(ModelType.Truss1D);
            node1 = model.NodeFactory.Create(0);
            SUT = new MatrixInversionLinearSolver(model);
            SUT.Solve();
        }
        
        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void WillCheckForInsufficientNumberOfElements()
        {
            model = new FiniteElementModel(ModelType.Truss1D);
            node1 = model.NodeFactory.Create(0);
            node2 = model.NodeFactory.Create(1);
            SUT = new MatrixInversionLinearSolver(model);
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
        	// TODO
            // no continuous path of elements connecting all nodes. i.e. there are actually two or more models
            Assert.Ignore();
        }
        
        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void WillCheckForUnsufficientlyConstrainedModel()
        {
            // remove all constraints
            model.UnconstrainNode(node1, DegreeOfFreedom.X);
            
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
