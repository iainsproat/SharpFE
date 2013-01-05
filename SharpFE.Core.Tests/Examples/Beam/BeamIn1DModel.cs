//-----------------------------------------------------------------------
// <copyright file="?.cs" company="Iain Sproat">
//     Copyright Iain Sproat, 2012.
// </copyright>
//-----------------------------------------------------------------------
using System;
using NUnit.Framework;

namespace SharpFE.Core.Tests.Examples.Beam
{
	[TestFixture]
	public class Beam3D
	{
		/// <summary>
        /// Creates a beam between two nodes spaced 1 metre apart.
        /// The beam is pinned at the nodes, so is restricted from translating but not from rotating
        /// We apply a moment at node 1 and check that the same moment is transferred along the beam to node 2
        /// </summary>
        [Test]
        public void CalculateModelOfOneSpringWith2DegreesOfFreedom()
        {
            FiniteElementModel model = new FiniteElementModel(ModelType.Beam1D); // we will create and analyze a 1D beam system
            FiniteElementNode node1 = model.NodeFactory.Create(0); // create a node at the origin
            model.ConstrainNode(node1, DegreeOfFreedom.Z); // constrain this node from moving in the Z axis
            // it is still free to rotate around the Y-axis however

            FiniteElementNode node2 = model.NodeFactory.Create(1.0); // create a second node at a distance 1 metre along the X axis
            model.ConstrainNode(node1, DegreeOfFreedom.Z); // constrain this node from moving in the Z axis
            // it is still free to rotate around the Y-axis however
            
            //TODO model.ElementFactory.CreateLinear3DBeam(; // create a spring between the two nodes of a stiffness of 2000 Newtons per metre
            
            ForceVector moment = model.ForceFactory.Create(10.0); // Create a force of 10 Newtons in the x direction
            model.ApplyForceToNode(moment, node2); // Apply that force to the second node
            
            IFiniteElementSolver solver = new LinearSolver(model); // Create a new instance of the solver class and pass it the model to solve
            FiniteElementResults results = solver.Solve(); // ask the solver to solve the model and return results
            
            DisplacementVector displacement = results.GetDisplacement(node2);  // get the displacement at the second node
            Assert.AreEqual(0.005, displacement.X); // Check that we have calculated a displacement of 0.005 metres (5 millimetres) along the X axis.
            
            ReactionVector reaction = results.GetReaction(node1); //get the reaction at the first node
            Assert.AreEqual(-10, reaction.X); // Check that we have calculated a reaction of -10 Newtons in the X axis.
        }
	}
}
