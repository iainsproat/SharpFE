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
        public void CalculateModelOfOneSpringWith1DegreesOfFreedomInYAxis()
        {
            FiniteElementModel model = new FiniteElementModel(ModelType.Truss2D); // we will create and analyze a 1D spring in the vertical
            FiniteElementNode node1 = model.NodeFactory.Create(0, 0); // create a node at the origin
            model.ConstrainNode(node1, DegreeOfFreedom.X); // constrain this node from moving in the X axis
            model.ConstrainNode(node1, DegreeOfFreedom.Y); // also constrain it from moving in the Y axis

            FiniteElementNode node2 = model.NodeFactory.Create(0, 1); // create a second node at a distance 1 metre along the Y axis.
            model.ConstrainNode(node2, DegreeOfFreedom.X); // fix this node from moving along the X-axis.  It is still free to move along the Y-axis however.
            
            Spring spring = model.ElementFactory.CreateSpring(node1, node2, 1000); // create a spring between the two nodes of a stiffness of 1000 Newtons per metre
            
            ForceVector force = model.ForceFactory.Create(0, 10); // Create a force of 10 Newtons along the y-axis.
            model.ApplyForceToNode(force, node2); // Apply that force to the second node
            
            IFiniteElementSolver solver = new LinearSolver(model); // Create a new instance of the solver class and pass it the model to solve
            FiniteElementResults results = solver.Solve(); // ask the solver to solve the model and return results
            
            DisplacementVector displacementAtNode2 = results.GetDisplacement(node2);  // get the displacement at the second node
            Assert.AreEqual(0.01, displacementAtNode2.Y); // check that we calculated 0.010 metres (10 millimetres) along the Y axis.
            
            ReactionVector reactionAtNode1 = results.GetReaction(node1); //get the reaction at the first node
            Assert.AreEqual(0, reactionAtNode1.X); // Check that we have calculated a reaction of 0 Newtons in the X axis.
            Assert.AreEqual(-10, reactionAtNode1.Y); // and a reaction of -10 Newtons in the Y axis.
        }
        
        [Test]
        public void CalculateModelOfTwoSpringsIn2DSystem()
        {
            FiniteElementModel model = new FiniteElementModel(ModelType.Truss2D); // we will create and analyze a 2D truss system
            FiniteElementNode node1 = model.NodeFactory.Create(0, 0); // create a node at the origin
            model.ConstrainNode(node1, DegreeOfFreedom.X); // constrain this node from moving in the X axis
            model.ConstrainNode(node1, DegreeOfFreedom.Y); // also constrain it from moving in the Y axis

            FiniteElementNode node2 = model.NodeFactory.Create(1, 1); // create a second node at a distance 1 metre along the X axis and 1 metre along the Y axis.
            
            FiniteElementNode node3 = model.NodeFactory.Create(2, 0); // create a third node at a distance 2 metres along the Y axis.
            model.ConstrainNode(node3, DegreeOfFreedom.X);
            model.ConstrainNode(node3, DegreeOfFreedom.Y);
            
            Spring spring = model.ElementFactory.CreateSpring(node1, node2, 1000); // create a spring between the first two nodes of a stiffness of 1000 Newtons per metre
            Spring spring2 = model.ElementFactory.CreateSpring(node2, node3, 1000); // create a spring between the second two nodes of a stiffness of 1000 Newtons per metre
            
            ForceVector force = model.ForceFactory.Create(0, -10); // Create a force of with components of -10 Newtons along the y-axis.
            model.ApplyForceToNode(force, node2); // Apply that force to the second node
            
            IFiniteElementSolver solver = new LinearSolver(model); // Create a new instance of the solver class and pass it the model to solve
            FiniteElementResults results = solver.Solve(); // ask the solver to solve the model and return results
            
            Console.WriteLine(- Math.Sqrt(2.0) / 2.0 / 1000.0 );
            DisplacementVector displacementAtNode2 = results.GetDisplacement(node2);  // get the displacement at the second node
            Assert.AreEqual(0, displacementAtNode2.X); // Check that we have no displacement in the X axis.
            Assert.AreEqual(-0.014, displacementAtNode2.Y, 0.001); // and 0.020 metres (20 millimetres) along the Y axis to a tolerance of 0.0005.
            
            ReactionVector reactionAtNode1 = results.GetReaction(node1); //get the reaction at the first node
            Assert.AreEqual(5, reactionAtNode1.X, 0.001); // Check that we have calculated a reaction of 1 Newtons in the X axis.
            Assert.AreEqual(5, reactionAtNode1.Y,  0.001); // and a reaction of -1 Newtons in the Y axis.
            
            ReactionVector reactionAtNode3 = results.GetReaction(node3);
            Assert.AreEqual(-5, reactionAtNode3.X);
            Assert.AreEqual( 5, reactionAtNode3.Y);
        }
        
        [Test]
        public void CalculateModelOfTwoSpringsIn2DSystemInTension()
        {
            FiniteElementModel model = new FiniteElementModel(ModelType.Truss2D); // we will create and analyze a 2D truss system
            FiniteElementNode node1 = model.NodeFactory.Create(0, 0); // create a node at the origin
            model.ConstrainNode(node1, DegreeOfFreedom.X); // constrain this node from moving in the X axis
            model.ConstrainNode(node1, DegreeOfFreedom.Y); // also constrain it from moving in the Y axis

            FiniteElementNode node2 = model.NodeFactory.Create(1, 1); // create a second node at a distance 1 metre along the X axis and 1 metre along the Y axis.
            // Note that the model is entirely unitless, but I'm explaining this example in terms of Newtons and metres to help with the understanding of what's happening.
            // You could use pounds and inches if you prefer.  Or elephants and double decker buses.
            
            FiniteElementNode node3 = model.NodeFactory.Create(0, 2); // create a third node at a distance 1 metre along the Y axis.
            model.ConstrainNode(node3, DegreeOfFreedom.X);
            model.ConstrainNode(node3, DegreeOfFreedom.Y);
            
            Spring spring = model.ElementFactory.CreateSpring(node1, node2, 1000); // create a spring between the first two nodes of a stiffness of 1000 Newtons per metre
            Spring spring2 = model.ElementFactory.CreateSpring(node2, node3, 1000); // create a spring between the second two nodes of a stiffness of 1000 Newtons per metre
            
            ForceVector force = model.ForceFactory.Create(0, 10); // Create a force of with components of 10 Newtons along the y-axis.
            model.ApplyForceToNode(force, node2); // Apply that force to the second node
            
            IFiniteElementSolver solver = new LinearSolver(model); // Create a new instance of the solver class and pass it the model to solve
            FiniteElementResults results = solver.Solve(); // ask the solver to solve the model and return results
            
            DisplacementVector displacementAtNode2 = results.GetDisplacement(node2);  // get the displacement at the second node
            Assert.AreEqual(0, displacementAtNode2.X); // Check that there is no displacement in the x-axis
            Assert.AreEqual(0.014, displacementAtNode2.Y, 0.001); // and 0.140 metres (140 millimetres) along the Y axis.
            
            ReactionVector reactionAtNode1 = results.GetReaction(node1); //get the reaction at the first node
            Assert.AreEqual(-5, reactionAtNode1.X, 0.001); // Check that we have calculated a reaction of -5 Newtons in the X axis.
            Assert.AreEqual(-5, reactionAtNode1.Y,  0.001); // and a reaction of -5 Newtons in the Y axis.
            
            ReactionVector reactionAtNode3 = results.GetReaction(node3);
            Assert.AreEqual(5, reactionAtNode3.X);
            Assert.AreEqual(-5, reactionAtNode3.Y);
        }
    }
}
