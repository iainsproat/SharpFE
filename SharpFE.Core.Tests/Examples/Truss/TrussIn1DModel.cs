//-----------------------------------------------------------------------
// <copyright file=Spring1DOF.cs" company="SharpFE">
//     Copyright Iain Sproat, 2012.
// </copyright>
//-----------------------------------------------------------------------

namespace SharpFE.Examples.Truss
{
	using System;
	using NUnit.Framework; // used for checking the results, not required for example.
	using SharpFE;
	
	/// <summary>
	/// This example uses 1D springs in a 1D system. i.e. springs are lines and can only be aligned with the x-axis.
	/// </summary>
	[TestFixture]
	public class TrussIn1DModel
	{
		/// <summary>
		/// Creates a spring with a stiffness of 2000 Newton per metre
		/// between two nodes spaced 1 metre apart.
		/// The first node is constrained from moving, the second node has a 10 Newton force applied to it.
		/// We then solve the model and will expect a result of 0.005 metre displacement at Node 2
		/// and a -10N reaction at node1
		/// </summary>
		[Test]
		public void CalculateModelOfOneSpringWith2DegreesOfFreedom()
		{
			FiniteElementModel model = new FiniteElementModel(ModelType.Truss1D); // we will create and analyze a 1D truss system
			FiniteElementNode node1 = model.NodeFactory.Create(0); // create a node at the origin
			model.ConstrainNode(node1, DegreeOfFreedom.X); // constrain this node from moving in the X axis

			FiniteElementNode node2 = model.NodeFactory.Create(1.0); // create a second node at a distance 1 metre along the X axis
			
			model.ElementFactory.CreateLinearConstantSpring(node1, node2, 2000.0); // create a spring between the two nodes of a stiffness of 2000 Newtons per metre
			
			ForceVector force = model.ForceFactory.Create(10.0); // Create a force of 10 Newtons in the x direction
			model.ApplyForceToNode(force, node2); // Apply that force to the second node
			
			IFiniteElementSolver solver = new MatrixInversionLinearSolver(model); // Create a new instance of the solver class and pass it the model to solve
			FiniteElementResults results = solver.Solve(); // ask the solver to solve the model and return results
			
			DisplacementVector displacement = results.GetDisplacement(node2);  // get the displacement at the second node
			Assert.AreEqual(0.005, displacement.X); // Check that we have calculated a displacement of 0.005 metres (5 millimetres) along the X axis.
			
			ReactionVector reaction = results.GetReaction(node1); //get the reaction at the first node
			Assert.AreEqual(-10, reaction.X); // Check that we have calculated a reaction of -10 Newtons in the X axis.
		}
		
		[Test]
		public void CalculateModelOfTwoSpringsWith3DegreesOfFreedom()
		{
			FiniteElementModel model = new FiniteElementModel(ModelType.Truss1D); // we will create and analyze a 1D truss system
			FiniteElementNode node1 = model.NodeFactory.Create(0); // create a node at the origin
			model.ConstrainNode(node1, DegreeOfFreedom.X); // constrain this node from moving in the X axis

			FiniteElementNode node2 = model.NodeFactory.Create(1.0); // create a second node at a distance 1 metre along the X axis
			FiniteElementNode node3 = model.NodeFactory.Create(2.0); // create a third node at a distance 2 metres along the X axis
			
			model.ElementFactory.CreateLinearConstantSpring(node1, node2, 10.0); // create a spring between the first two nodes of a stiffness of 10 Newtons per metre
			model.ElementFactory.CreateLinearConstantSpring(node2, node3, 20.0); // create a spring between the second two nodes of a stiffness of 20 Newtons per metre
			
			ForceVector force = model.ForceFactory.Create(0.5); // Create a force of 0.5 Newtons in the x direction
			model.ApplyForceToNode(force, node2); // Apply that force to the second node
			
			ForceVector force2 = model.ForceFactory.Create(3.0); // Create a force of 3 Newtons in the y direction
			model.ApplyForceToNode(force2, node3); // Apply that force to the third node
			
			IFiniteElementSolver solver = new MatrixInversionLinearSolver(model); // Create a new instance of the solver class and pass it the model to solve
			FiniteElementResults results = solver.Solve(); // ask the solver to solve the model and return results
			
			DisplacementVector displacementAtNode2 = results.GetDisplacement(node2);  // get the displacement at the second node
			Assert.AreEqual(0.35, displacementAtNode2.X, 0.0005); // Check that we have calculated a displacement of 0.35 metres along the X axis to a tolerance of 0.5 millimetres.
			
			DisplacementVector displacementAtNode3 = results.GetDisplacement(node3); // get the displacement at the third node
			Assert.AreEqual(0.5, displacementAtNode3.X, 0.0005); // check that we have calculated a displacement of 0.5 metres along the X axis to a tolerance of 0.5 millimetres.
			
			ReactionVector reaction = results.GetReaction(node1); //get the reaction at the first node
			Assert.AreEqual(-3.5, reaction.X, 0.0005); // Check that we have calculated a reaction of -10 Newtons in the X axis to a tolerance of 0.0005 Newtons.
		}
		
		/// <summary>
		/// Example problem and results are derived from:
		/// MATLAB Codes for Finite Element Analysis, Solid Mechanics and its applications Volume 157, A.J.M. Ferreira, Springer 2010
		/// Section 2.5, page 21
		/// </summary>
		[Test]
		public void CalculateModelOfThreeSpringsWithFourDegreesOfFreedom()
		{
			FiniteElementModel model = new FiniteElementModel(ModelType.Truss1D);
			FiniteElementNode node1 = model.NodeFactory.Create(0);
			model.ConstrainNode(node1, DegreeOfFreedom.X);
			
			FiniteElementNode node2 = model.NodeFactory.Create(1.0);
			
			FiniteElementNode node3 = model.NodeFactory.Create(2.0);
			model.ConstrainNode(node3, DegreeOfFreedom.X);
			
			FiniteElementNode node4 = model.NodeFactory.Create(2.00001); ///TODO allow multiple nodes to be added in the same location
			model.ConstrainNode(node4, DegreeOfFreedom.X);
			
			model.ElementFactory.CreateLinearConstantSpring(node1, node2, 1);
			model.ElementFactory.CreateLinearConstantSpring(node2, node3, 1);
			model.ElementFactory.CreateLinearConstantSpring(node2, node4, 1);
			
			ForceVector externalForce = model.ForceFactory.Create(10);
			model.ApplyForceToNode(externalForce, node2);
			
			IFiniteElementSolver solver = new MatrixInversionLinearSolver(model);
			FiniteElementResults results = solver.Solve();
			
			DisplacementVector displacementAtNode2 = results.GetDisplacement(node2);
			Assert.AreEqual(3.333, displacementAtNode2.X, 0.001);
			
			ReactionVector reactionAtNode1 = results.GetReaction(node1);
			Assert.AreEqual(-3.333, reactionAtNode1.X, 0.001);
			
			ReactionVector reactionAtNode3 = results.GetReaction(node3);
			Assert.AreEqual(-3.333, reactionAtNode3.X, 0.001);
			
			ReactionVector reactionAtNode4 = results.GetReaction(node4);
			Assert.AreEqual(-3.333, reactionAtNode4.X, 0.001);
		}
		
		/// <summary>
		/// Example problem and results are derived from:
		/// MATLAB Codes for Finite Element Analysis, Solid Mechanics and its applications Volume 157, A.J.M. Ferreira, Springer 2010
		/// Section 3.3, page 37
		/// </summary>
		[Test]
		public void CalculateModelOfThreeBarsWithFourDegreesOfFreedom()
		{
			FiniteElementModel model = new FiniteElementModel(ModelType.Truss1D);
			FiniteElementNode node1 = model.NodeFactory.Create(0);
			model.ConstrainNode(node1, DegreeOfFreedom.X);
			
			FiniteElementNode node2 = model.NodeFactory.Create(30.0);
			
			FiniteElementNode node3 = model.NodeFactory.Create(60.0);
			
			FiniteElementNode node4 = model.NodeFactory.Create(90.0);
			model.ConstrainNode(node4, DegreeOfFreedom.X);
			
			IMaterial material = new GenericElasticMaterial(0, 30000000, 0, 0);
            ICrossSection section = new SolidRectangle(1, 1);
			
			model.ElementFactory.CreateLinearTruss(node1, node2, material, section);
			model.ElementFactory.CreateLinearTruss(node2, node3, material, section);
			model.ElementFactory.CreateLinearTruss(node3, node4, material, section);
			
			
			ForceVector externalForce = model.ForceFactory.Create(3000);
			model.ApplyForceToNode(externalForce, node2);
			
			IFiniteElementSolver solver = new MatrixInversionLinearSolver(model);
			FiniteElementResults results = solver.Solve();
			
			DisplacementVector displacementAtNode2 = results.GetDisplacement(node2);
			Assert.AreEqual(0.002, displacementAtNode2.X, 0.001);
			
			DisplacementVector displacementAtNode3 = results.GetDisplacement(node3);
			Assert.AreEqual(0.001, displacementAtNode3.X, 0.001);
			
			ReactionVector reactionAtNode1 = results.GetReaction(node1);
			Assert.AreEqual(-2000, reactionAtNode1.X, 0.001);
			
			ReactionVector reactionAtNode4 = results.GetReaction(node4);
			Assert.AreEqual(-1000, reactionAtNode4.X, 0.001);
		}
		
		/// <summary>
		/// Example problem and results are derived from:
		/// MATLAB Codes for Finite Element Analysis, Solid Mechanics and its applications Volume 157, A.J.M. Ferreira, Springer 2010
		/// Section 3.5, page 44
		/// </summary>
		[Test]
		public void CalculateModelOfTwoBarsAndOneSpringWithFourDegreesOfFreedom()
		{
			FiniteElementModel model = new FiniteElementModel(ModelType.Truss1D);
			FiniteElementNode node1 = model.NodeFactory.Create(0);
			model.ConstrainNode(node1, DegreeOfFreedom.X);
			
			FiniteElementNode node2 = model.NodeFactory.Create(2.0);
			
			FiniteElementNode node3 = model.NodeFactory.Create(4.0);
			
			FiniteElementNode node4 = model.NodeFactory.Create(6.0);
			model.ConstrainNode(node4, DegreeOfFreedom.X);
			
			IMaterial material = new GenericElasticMaterial(0, 70000000000, 0, 0);
            ICrossSection section = new SolidRectangle(0.02, 0.01);
			
			model.ElementFactory.CreateLinearTruss(node1, node2, material, section);
			model.ElementFactory.CreateLinearTruss(node2, node3, material, section);
			model.ElementFactory.CreateLinearConstantSpring(node3, node4, 2000000);
			
			
			ForceVector externalForce = model.ForceFactory.Create(8000);
			model.ApplyForceToNode(externalForce, node2);
			
			IFiniteElementSolver solver = new MatrixInversionLinearSolver(model);
			FiniteElementResults results = solver.Solve();
			
			DisplacementVector displacementAtNode2 = results.GetDisplacement(node2);
			Assert.AreEqual(0.000935, displacementAtNode2.X, 0.001);
			
			DisplacementVector displacementAtNode3 = results.GetDisplacement(node3);
			Assert.AreEqual(0.000727, displacementAtNode3.X, 0.001);
			
			ReactionVector reactionAtNode1 = results.GetReaction(node1);
			Assert.AreEqual(-6546, reactionAtNode1.X, 1);
			
			ReactionVector reactionAtNode4 = results.GetReaction(node4);
			Assert.AreEqual(-1455, reactionAtNode4.X, 1);
		}
	}
}
