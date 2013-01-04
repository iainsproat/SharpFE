//-----------------------------------------------------------------------
// <copyright file="Spring1DIn2DModel.cs" company="SharpFE">
//     Copyright Iain Sproat, 2012.
// </copyright>
//-----------------------------------------------------------------------
using System;
using SharpFE;
using NUnit.Framework;
using MathNet.Numerics.LinearAlgebra.Double;

namespace SharpFE.Examples
{
    /// <summary>
    /// Description of Spring1DIn2DModel.
    /// </summary>
    [TestFixture]
    public class Spring1DIn2DModel
    {
    	[Test]
        public void SpringInXAxis()
        {
            FiniteElementModel model = new FiniteElementModel(ModelType.Truss2D); // we will create and analyze a 1D spring in the vertical
            FiniteElementNode node1 = model.NodeFactory.Create(0, 0); // create a node at the origin
            model.ConstrainNode(node1, DegreeOfFreedom.X); // constrain this node from moving in the X axis
            model.ConstrainNode(node1, DegreeOfFreedom.Y); // also constrain it from moving in the Y axis

            FiniteElementNode node2 = model.NodeFactory.Create(1, 0); // create a second node at a distance 1 metre along the X axis.
            model.ConstrainNode(node2, DegreeOfFreedom.Y); // fix this node from moving along the Y-axis.  It is still free to move along the X-axis however.
            
            LinearConstantSpring spring = model.ElementFactory.CreateLinearConstantSpring(node1, node2, 1000); // create a spring between the two nodes of a stiffness of 1000 Newtons per metre
            
            ForceVector force = model.ForceFactory.Create(10, 0); // Create a force of 10 Newtons along the x-axis.
            model.ApplyForceToNode(force, node2); // Apply that force to the second node
            
            IFiniteElementSolver solver = new LinearSolver(model); // Create a new instance of the solver class and pass it the model to solve
            FiniteElementResults results = solver.Solve(); // ask the solver to solve the model and return results
            
            DisplacementVector displacementAtNode2 = results.GetDisplacement(node2);  // get the displacement at the second node
            Assert.AreEqual(0.01, displacementAtNode2.X); // check that we calculated 0.010 metres (10 millimetres) along the Y axis.
            
            ReactionVector reactionAtNode1 = results.GetReaction(node1); //get the reaction at the first node
            Assert.AreEqual(-10, reactionAtNode1.X); // Check that we have calculated a reaction of -10 Newtons in the X axis.
            Assert.AreEqual(0, reactionAtNode1.Y); // and a reaction of 0 Newtons in the Y axis.
        }
        
        [Test]
        public void SpringAt60DegreesInXYPlane()
        {
            FiniteElementModel model = new FiniteElementModel(ModelType.Truss2D); // we will create and analyze a 2D truss system
            FiniteElementNode node1 = model.NodeFactory.Create(0, 0); // create a node at the origin
            model.ConstrainNode(node1, DegreeOfFreedom.X); // constrain this node from moving in the X axis
            model.ConstrainNode(node1, DegreeOfFreedom.Y); // also constrain it from moving in the Y axis

            FiniteElementNode node2 = model.NodeFactory.Create(1, 1.73205); // create a second node at a distance 1 metre along the X axis and 1.73 metres along the Y axis (giving an angle of 60 degrees from x-axis).
            model.ConstrainNode(node2, DegreeOfFreedom.X);
            
            LinearConstantSpring spring = model.ElementFactory.CreateLinearConstantSpring(node1, node2, 1000); // create a spring between the first two nodes of a stiffness of 1000 Newtons per metre
            
            ForceVector force = model.ForceFactory.Create(0, 10); // Create a force of with components of 10 Newtons along the y-axis.
            model.ApplyForceToNode(force, node2); // Apply that force to the second node
            
            IFiniteElementSolver solver = new LinearSolver(model); // Create a new instance of the solver class and pass it the model to solve
            FiniteElementResults results = solver.Solve(); // ask the solver to solve the model and return results
            
            DisplacementVector displacementAtNode2 = results.GetDisplacement(node2);  // get the displacement at the second node
            Assert.AreEqual(0, displacementAtNode2.X); // Check that there is no displacement in the x-axis
            Assert.AreEqual(0.013333, displacementAtNode2.Y, 0.001); // and 0.01333 metres (13 millimetres) along the Y axis.
            
            ReactionVector reactionAtNode1 = results.GetReaction(node1); //get the reaction at the first node
            Assert.AreEqual(-5.774, reactionAtNode1.X, 0.001); // Check that we have calculated a reaction of 10/SQRT(3) Newtons in the X axis.
            Assert.AreEqual(-10, reactionAtNode1.Y,  0.001); // and a reaction of -10 Newtons in the Y axis.
        }
    }
}
