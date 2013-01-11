//-----------------------------------------------------------------------
// <copyright file="BeamIn1DModel.cs" company="Iain Sproat">
//     Copyright Iain Sproat, 2012.
// </copyright>
//-----------------------------------------------------------------------
using System;
using NUnit.Framework;

namespace SharpFE.Examples.Beam
{
    [TestFixture]
    public class BeamIn1DModel
    {
        ////TODO most of the below are integration tests rather than examples, and should be split out.
        
        
        /// <summary>
        /// Creates a cantilevered beam between two nodes spaced 1 metre apart.
        /// The beam is fully fixed at the first node and is free at the second node.
        /// We apply a downwards force at the second node
        /// </summary>
        [Test]
        public void Cantilever()
        {
            FiniteElementModel model = new FiniteElementModel(ModelType.Beam1D); // we will create and analyze a 1D beam system
            FiniteElementNode node1 = model.NodeFactory.Create(0); // create a node at the origin
            model.ConstrainNode(node1, DegreeOfFreedom.Z); // constrain the node from moving in the Z-axis
            model.ConstrainNode(node1, DegreeOfFreedom.YY); // constrain this node from rotating around the Y-axis

            FiniteElementNode node2 = model.NodeFactory.Create(1.0); // create a second node at a distance 1 metre along the X axis
            
            IMaterial material = new GenericElasticMaterial(0, 210000000000, 0.3, 80769200000);
            ICrossSection section = new SolidRectangle(0.1, 0.1);
            
            model.ElementFactory.CreateLinear3DBeam(node1, node2, material, section); // create a spring between the two nodes of a stiffness of 2000 Newtons per metre
            
            ForceVector force = model.ForceFactory.CreateFor1DBeam(-10000, 0); // Create a force of 10 Newtons in the z direction
            model.ApplyForceToNode(force, node2); // Apply that force to the second node
            
            IFiniteElementSolver solver = new LinearSolver(model); // Create a new instance of the solver class and pass it the model to solve
            FiniteElementResults results = solver.Solve(); // ask the solver to solve the model and return results
            
            ReactionVector reaction = results.GetReaction(node1); //get the reaction at the first node
            Assert.AreEqual(10000, reaction.Z, 0.001);   // Check that we have calculated a reaction of 10 Newtons in the Z-axis
            Assert.AreEqual(-10000, reaction.YY, 0.001); // Check that we have calculated a reaction of -10 NewtonMetres around the YY axis.
            
            DisplacementVector displacement = results.GetDisplacement(node2);  // get the displacement at the second node
            Assert.AreEqual(-0.00192, displacement.Z, 0.0005);
            Assert.AreEqual(0.00286, displacement.YY, 0.0001);
        }
        
        /// <summary>
        /// Creates a beam between two nodes spaced 1 metre apart.
        /// The beam is pinned at the nodes, so is restricted from translating but not from rotating
        /// We apply a moment at node 1 and check that the same moment is transferred along the beam to node 2
        /// </summary>
        [Test]
        public void MomentThroughSimplySupportedBeam()
        {
            FiniteElementModel model = new FiniteElementModel(ModelType.Beam1D); // we will create and analyze a 1D beam system
            FiniteElementNode node1 = model.NodeFactory.Create(0); // create a node at the origin
            model.ConstrainNode(node1, DegreeOfFreedom.Z); // constrain the node from moving in the Z-axis

            FiniteElementNode node2 = model.NodeFactory.Create(1.0); // create a second node at a distance 1 metre along the X axis
            model.ConstrainNode(node2, DegreeOfFreedom.Z); // constrain the node from moving in the Z-axis
            
            IMaterial material = new GenericElasticMaterial(0, 210000000000, 0.3, 80769200000);
            ICrossSection section = new SolidRectangle(0.1, 0.1);
            
            model.ElementFactory.CreateLinear3DBeam(node1, node2, material, section); // create a spring between the two nodes of a stiffness of 2000 Newtons per metre
            
            ForceVector moment = model.ForceFactory.CreateFor1DBeam(0, 10000); // Create a clockwise(?) moment of 10 KiloNewtonmetres around the yy axis
            model.ApplyForceToNode(moment, node1); // Apply that moment to the first node
            
            IFiniteElementSolver solver = new LinearSolver(model); // Create a new instance of the solver class and pass it the model to solve
            FiniteElementResults results = solver.Solve(); // ask the solver to solve the model and return results
            
            // check the results
            DisplacementVector displacementAtNode1 = results.GetDisplacement(node1);
            Assert.AreEqual(0, displacementAtNode1.Z, 0.001);
            Assert.AreEqual(0.00192, displacementAtNode1.YY, 0.0001);
            
            DisplacementVector displacementAtNode2 = results.GetDisplacement(node2);
            Assert.AreEqual(0, displacementAtNode2.Z, 0.001);
            Assert.AreEqual(-0.000928, displacementAtNode2.YY, 0.0005);
            
            ReactionVector reactionAtNode1 = results.GetReaction(node1);
            Assert.AreEqual(-10000, reactionAtNode1.Z, 0.001);
            Assert.AreEqual(0, reactionAtNode1.YY, 0.001);
            
            ReactionVector reactionAtNode2 = results.GetReaction(node2);
            Assert.AreEqual(10000, reactionAtNode2.Z, 0.001);
            Assert.AreEqual(0, reactionAtNode2.YY, 0.001);
        }
        
        [Test]
        public void MomentThroughVerticallyOrientedSimplySupportedBeam()
        {
            FiniteElementModel model = new FiniteElementModel(ModelType.Frame2D); // we will create and analyze a 1D beam system
            FiniteElementNode node1 = model.NodeFactory.CreateForTruss(0, 0); // create a node at the origin
            model.ConstrainNode(node1, DegreeOfFreedom.X);
            model.ConstrainNode(node1, DegreeOfFreedom.Z);

            FiniteElementNode node2 = model.NodeFactory.CreateForTruss(0, 1.0); // create a second node at a distance 1 metre along the X axis
            model.ConstrainNode(node2, DegreeOfFreedom.X);
            
            IMaterial material = new GenericElasticMaterial(0, 210000000000, 0.3, 80769200000);
            ICrossSection section = new SolidRectangle(0.1, 0.1);
            
            model.ElementFactory.CreateLinear3DBeam(node1, node2, material, section); // create a spring between the two nodes of a stiffness of 2000 Newtons per metre
            
            ForceVector moment = model.ForceFactory.CreateFor1DBeam(0, 10000); // Create a clockwise(?) moment of 10 KiloNewtonmetres around the yy axis
            model.ApplyForceToNode(moment, node1); // Apply that moment to the first node
            
            IFiniteElementSolver solver = new LinearSolver(model); // Create a new instance of the solver class and pass it the model to solve
            FiniteElementResults results = solver.Solve(); // ask the solver to solve the model and return results
            
            // check the results
            DisplacementVector displacementAtNode1 = results.GetDisplacement(node1);
            Assert.AreEqual(0, displacementAtNode1.Z, 0.001);
            Assert.AreEqual(0.00192, displacementAtNode1.YY, 0.0001);
            
            DisplacementVector displacementAtNode2 = results.GetDisplacement(node2);
            Assert.AreEqual(0, displacementAtNode2.Z, 0.001);
            Assert.AreEqual(-0.000928, displacementAtNode2.YY, 0.0005);
            
            ReactionVector reactionAtNode1 = results.GetReaction(node1);
            Assert.AreEqual(10000, reactionAtNode1.X, 0.001);
            Assert.AreEqual(0, reactionAtNode1.YY, 0.001);
            
            ReactionVector reactionAtNode2 = results.GetReaction(node2);
            Assert.AreEqual(-10000, reactionAtNode2.X, 0.001);
            Assert.AreEqual(0, reactionAtNode2.YY, 0.001);
        }
        
        /// <summary>
        /// (1)----(2)----(3)
        /// </summary>
        [Test]
        public void ThreeNodeBeam()
        {
            FiniteElementModel model = new FiniteElementModel(ModelType.Frame2D);
            FiniteElementNode node1 = model.NodeFactory.CreateForTruss(0,0);
            model.ConstrainNode(node1, DegreeOfFreedom.X);
            model.ConstrainNode(node1, DegreeOfFreedom.Z);

            FiniteElementNode node2 = model.NodeFactory.CreateForTruss(1,0);
            
            FiniteElementNode node3 = model.NodeFactory.CreateForTruss(2,0);
            model.ConstrainNode(node3, DegreeOfFreedom.Z);
            
            IMaterial material = new GenericElasticMaterial(0, 10000000000, 0.3, 1000000000);
            ICrossSection section = new SolidRectangle(1, 1);
            
            model.ElementFactory.CreateLinear1DBeam(node1, node2, material, section);
            model.ElementFactory.CreateLinear1DBeam(node2,node3,material,section);
            
            ForceVector force = model.ForceFactory.Create(0, 0, -10000, 0, 0, 0);
            model.ApplyForceToNode(force, node2);
            
            IFiniteElementSolver solver = new LinearSolver(model);
            FiniteElementResults results = solver.Solve();
            
            DisplacementVector node1Displacement = results.GetDisplacement(node1);
            Console.WriteLine("node1Displacement : " + node1Displacement);
            
            DisplacementVector node2Displacement = results.GetDisplacement(node2);
            Console.WriteLine("node2Displacement : " + node2Displacement);
            
            DisplacementVector node3Displacement = results.GetDisplacement(node3);
            Console.WriteLine("node3Displacement : " + node3Displacement);
            
            ReactionVector node1Reaction = results.GetReaction(node1);
            Console.WriteLine("node1Reaction : " + node1Reaction);
            
            ReactionVector node3Reaction = results.GetReaction(node3);
            Console.WriteLine("node5Reaction : " + node3Reaction);
            
            // FIXME the angles are incorrect!
            //            Assert.AreEqual(0.00143, node1Displacement.YY, 0.00001);
            Assert.AreEqual(0, node2Displacement.XX, 0.001);
            Assert.AreEqual(-0.00096, node2Displacement.Z, 0.001);
            //            Assert.AreEqual(-0.00143, node3Displacement.YY, 0.00001);
            Assert.AreEqual(5000, node1Reaction.Z, 0.001);
            Assert.AreEqual(5000, node3Reaction.Z, 0.001);
        }
        
        /// <summary>
        ///            (2)
        ///            / \
        ///           /   \
        ///          /     \
        ///         /       \
        ///       (1)       (3)
        /// </summary>
        [Test]
        public void AFrame()
        {
            FiniteElementModel model = new FiniteElementModel(ModelType.Frame2D);
            FiniteElementNode node1 = model.NodeFactory.CreateForTruss(-5,0);
            model.ConstrainNode(node1, DegreeOfFreedom.X);
            model.ConstrainNode(node1, DegreeOfFreedom.Z);

            FiniteElementNode node2 = model.NodeFactory.CreateForTruss(0,5);
            
            FiniteElementNode node3 = model.NodeFactory.CreateForTruss(5,0);
            model.ConstrainNode(node3, DegreeOfFreedom.Z);
            
            IMaterial material = new GenericElasticMaterial(0, 210000000000, 0.3, 80769200000);
            ICrossSection section = new SolidRectangle(0.1, 0.1);
            
            model.ElementFactory.CreateLinear1DBeam(node1, node2, material, section);
            model.ElementFactory.CreateLinear1DBeam(node2,node3,material,section);
            
            ForceVector force = model.ForceFactory.Create(0, 0, -10000, 0, 0, 0);
            model.ApplyForceToNode(force, node2);
            
            IFiniteElementSolver solver = new LinearSolver(model);
            FiniteElementResults results = solver.Solve();
            
            DisplacementVector node1Displacement = results.GetDisplacement(node1);
            Console.WriteLine("node1Displacement : " + node1Displacement);
            
            DisplacementVector node2Displacement = results.GetDisplacement(node2);
            Console.WriteLine("node2Displacement : " + node2Displacement);
            
            DisplacementVector node3Displacement = results.GetDisplacement(node3);
            Console.WriteLine("node3Displacement : " + node3Displacement);
            
            ReactionVector node1Reaction = results.GetReaction(node1);
            Console.WriteLine("node1Reaction : " + node1Reaction);
            
            ReactionVector node3Reaction = results.GetReaction(node3);
            Console.WriteLine("node5Reaction : " + node3Reaction);
            
            Assert.AreEqual(5000, node1Reaction.Z, 1);
            Assert.AreEqual(5000, node3Reaction.Z, 1);
            
            // FIXME the tolerance values are very high!
            Assert.AreEqual(0.0457, node1Displacement.YY, 0.005);
            Assert.AreEqual(0.122, node2Displacement.X, 0.05);
            Assert.AreEqual(-0.1525, node2Displacement.Z, 0.02);
            Assert.AreEqual(0, node2Displacement.YY, 0.001);
            Assert.AreEqual(0.244, node3Displacement.X, 0.1);
            Assert.AreEqual(-0.0457, node3Displacement.YY, 0.005);
        }
        
        /// <summary>
        ///            (3)
        ///            / \
        ///           /   \
        ///          /  ^  \
        ///         /   |   \
        ///       (2)   |   (4)
        ///        |         |
        ///        |    1    |
        ///        |    0    |
        ///        |    k    |
        ///       (1)   N   (5)
        /// </summary>
        [Test]
        public void PortalFrame()
        {
            FiniteElementModel model = new FiniteElementModel(ModelType.Frame2D); // we will create and analyze a 3D frame system
            FiniteElementNode node1 = model.NodeFactory.CreateForTruss(-10,0);
            model.ConstrainNode(node1, DegreeOfFreedom.X);
            model.ConstrainNode(node1, DegreeOfFreedom.Z);
            model.ConstrainNode(node1, DegreeOfFreedom.YY);

            FiniteElementNode node2 = model.NodeFactory.CreateForTruss(-10,10);
            FiniteElementNode node3 = model.NodeFactory.CreateForTruss(0,14);
            FiniteElementNode node4 = model.NodeFactory.CreateForTruss(10,10);
            
            FiniteElementNode node5 = model.NodeFactory.CreateForTruss(10,0);
            model.ConstrainNode(node5, DegreeOfFreedom.X);
            model.ConstrainNode(node5, DegreeOfFreedom.Z);
            model.ConstrainNode(node5, DegreeOfFreedom.YY);
            
            IMaterial material = new GenericElasticMaterial(0, 210000000000, 0.3, 80769200000);
            ICrossSection section = new SolidRectangle(0.1, 0.1);
            
            model.ElementFactory.CreateLinear1DBeam(node1, node2, material, section);
            model.ElementFactory.CreateLinear1DBeam(node2,node3,material,section);
            model.ElementFactory.CreateLinear1DBeam(node3,node4,material,section);
            model.ElementFactory.CreateLinear1DBeam(node4,node5,material,section);
            
            ForceVector force = model.ForceFactory.Create(0, 0, 10000, 0, 0, 0);
            model.ApplyForceToNode(force, node3);
            
            IFiniteElementSolver solver = new LinearSolverSVD(model);
            FiniteElementResults results = solver.Solve();
            
            DisplacementVector node2Displacement = results.GetDisplacement(node2);
            DisplacementVector node3Displacement = results.GetDisplacement(node3);
            DisplacementVector node4Displacement = results.GetDisplacement(node4);
            ReactionVector node1Reaction = results.GetReaction(node1);
            ReactionVector node5Reaction = results.GetReaction(node5);
            
            Console.WriteLine("node2 displacements : " + node2Displacement);
            Console.WriteLine("node3 displacements : " + node3Displacement);
            Console.WriteLine("node4 displacements : " + node4Displacement);
            Console.WriteLine("node1 reactions : " + node1Reaction);
            Console.WriteLine("node5 reactions : " + node5Reaction);
            
            Assert.AreEqual(-0.0972, node2Displacement.X, 0.001);
            Assert.AreEqual(0, node2Displacement.Z, 0.001);
            Assert.AreEqual(-0.00999, node2Displacement.YY, 0.0001);
            
            Assert.AreEqual(0, node3Displacement.X, 0.001);
            Assert.AreEqual(0.2431, node3Displacement.Z, 0.001);
            Assert.AreEqual(0, node3Displacement.YY, 0.0001);
            
            Assert.AreEqual(-0.0972, node4Displacement.X);
            Assert.AreEqual(0, node4Displacement.Z, 0.001);
            Assert.AreEqual(0.00999, node4Displacement.YY, 0.0001);
            
            Console.WriteLine(node1Reaction);
            Assert.AreEqual(-5000, node1Reaction.Z, 1);
            Assert.AreEqual(-3090, node1Reaction.X, 1);
            Assert.AreEqual(-13700, node1Reaction.YY, 0);
            
            Console.WriteLine(node5Reaction);
            Assert.AreEqual(-5000, node5Reaction.Z, 1);
            Assert.AreEqual(3090, node5Reaction.X, 1);
            Assert.AreEqual(13700, node5Reaction.YY, 0);
        }
    }
}
