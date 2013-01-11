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
	public class LinearSolverTestsIn2D
	{
		FiniteElementModel model;
		FiniteElementNode node1;
		FiniteElementNode node2;
		LinearConstantSpring spring1;
		ForceVector force1;
		
		LinearSolver SUT;
		
		// Along X Axis will result in an indeterminate matrix - see LinearSolverTest instead
		
		[SetUp]
		public void SetUp()
		{
			model = null;
			node1 = null;
			node2 = null;
			spring1 = null;
			force1 = null;
			SUT = null;
		}
		
		[Test]
		public void AlongZAxis()
		{
			Create2DSingleSpringModelAroundOrigin(0, 1);
			FiniteElementResults results = SUT.Solve();
			Assert.IsNotNull(results);
            Assert.AreEqual(0, results.GetReaction(node1).X, 0.001);
            Assert.AreEqual(10, results.GetReaction(node1).Z, 0.001);
            Assert.AreEqual(0, results.GetReaction(node2).X, 0.001);
            Assert.AreEqual(-0.01, results.GetDisplacement(node2).Z, 0.001);
            Assert.AreEqual(0, results.GetDisplacement(node2).X, 0.001);
		}
		
		[Test]
		public void At45Degrees()
		{
			Create2DSingleSpringModelAroundOrigin(1, 1);
			FiniteElementResults results = SUT.Solve();
            
			Assert.IsNotNull(results);
            Assert.AreEqual(10, results.GetReaction(node1).X, 0.001);
            Assert.AreEqual(10, results.GetReaction(node1).Z, 0.001);
            Assert.AreEqual(-10, results.GetReaction(node2).X, 0.001);
            Assert.AreEqual(-0.02, results.GetDisplacement(node2).Z, 0.001);
            Assert.AreEqual(0, results.GetDisplacement(node2).X, 0.001);
		}
		
		[Test]
		public void At60Degrees()
		{
			Create2DSingleSpringModelAroundOrigin(1, Math.Sqrt(3));
			FiniteElementResults results = SUT.Solve();
            
			Assert.IsNotNull(results);
            Assert.AreEqual(5.774, results.GetReaction(node1).X, 0.001);
            Assert.AreEqual(10, results.GetReaction(node1).Z, 0.001);
            Assert.AreEqual(-5.774, results.GetReaction(node2).X, 0.001);
            Assert.AreEqual((-4.0 / 3.0 / 100.0), results.GetDisplacement(node2).Z, 0.001);
            Assert.AreEqual(0, results.GetDisplacement(node2).X);
		}
		
		[Test]
		public void At120Degrees()
		{
			Create2DSingleSpringModelAroundOrigin(-1, Math.Sqrt(3));
			FiniteElementResults results = SUT.Solve();
            
			Assert.IsNotNull(results);
            Assert.AreEqual(-5.774, results.GetReaction(node1).X, 0.001);
            Assert.AreEqual(10, results.GetReaction(node1).Z, 0.001);
            Assert.AreEqual(5.774, results.GetReaction(node2).X, 0.001);
            Assert.AreEqual((-4.0 / 3.0 / 100.0), results.GetDisplacement(node2).Z, 0.001);
            Assert.AreEqual(0, results.GetDisplacement(node2).X);
		}
		
		[Test]
		public void At135Degrees()
		{
			Create2DSingleSpringModelAroundOrigin(-1, 1);
			FiniteElementResults results = SUT.Solve();
            
			Assert.IsNotNull(results);
            Assert.AreEqual(-10, results.GetReaction(node1).X, 0.001);
            Assert.AreEqual(10, results.GetReaction(node1).Z, 0.001);
            Assert.AreEqual(10, results.GetReaction(node2).X, 0.001);
            Assert.AreEqual(-0.02, results.GetDisplacement(node2).Z, 0.001);
            Assert.AreEqual(0, results.GetDisplacement(node2).X, 0.001);
		}
		
		[Test]
		public void At225Degrees()
		{
			Create2DSingleSpringModelAroundOrigin(-1, -1);
			FiniteElementResults results = SUT.Solve();
            
			Assert.IsNotNull(results);
            Assert.AreEqual(10, results.GetReaction(node1).X, 0.001);
            Assert.AreEqual(10, results.GetReaction(node1).Z, 0.001);
            Assert.AreEqual(-10, results.GetReaction(node2).X, 0.001);
            Assert.AreEqual(-0.02, results.GetDisplacement(node2).Z, 0.001);
            Assert.AreEqual(0, results.GetDisplacement(node2).X, 0.001);
		}
		
		[Test]
		public void At315Degrees()
		{
			Create2DSingleSpringModelAroundOrigin(1, -1);
			FiniteElementResults results = SUT.Solve();
            
			Assert.IsNotNull(results);
            Assert.AreEqual(-10, results.GetReaction(node1).X, 0.001);
            Assert.AreEqual(10, results.GetReaction(node1).Z, 0.001);
            Assert.AreEqual(10, results.GetReaction(node2).X, 0.001);
            Assert.AreEqual(-0.02, results.GetDisplacement(node2).Z, 0.001);
            Assert.AreEqual(0, results.GetDisplacement(node2).X, 0.001);
		}
		
		private void Create2DSingleSpringModelAroundOrigin(double x, double z)
		{
			model = new FiniteElementModel(ModelType.Truss2D);
			node1 = model.NodeFactory.CreateForTruss(0, 0);
			node2 = model.NodeFactory.CreateForTruss(x, z);

			spring1 = model.ElementFactory.CreateLinearConstantSpring(node1, node2, 1000);
			
			model.ConstrainNode(node1, DegreeOfFreedom.X);
			model.ConstrainNode(node1, DegreeOfFreedom.Z);
			model.ConstrainNode(node2, DegreeOfFreedom.X);
			
			force1 = model.ForceFactory.CreateForTruss(0, -10);
			model.ApplyForceToNode(force1, node2);
			
			SUT = new LinearSolverSVD(model);
		}
	}
}
