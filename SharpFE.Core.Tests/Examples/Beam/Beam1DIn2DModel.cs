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
    public class Beam1DIn2DModel
    {
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

            FiniteElementNode node2 = model.NodeFactory.Create(3.0); // create a second node at a distance 1 metre along the X axis
            
            IMaterial material = new GenericElasticMaterial(0, 210000000000, 0, 0);
            ICrossSection section = new GenericCrossSection(0.0001, 0.0002);
            
            model.ElementFactory.CreateLinear1DBeam(node1, node2, material, section);
            
            ForceVector force = model.ForceFactory.CreateFor1DBeam(-10000, 0); // Create a force of 10 KiloNewtons in the z direction
            model.ApplyForceToNode(force, node2); // Apply that force to the second node
            
            IFiniteElementSolver solver = new MatrixInversionLinearSolver(model); // Create a new instance of the solver class and pass it the model to solve
            FiniteElementResults results = solver.Solve(); // ask the solver to solve the model and return results
            
            ReactionVector reaction = results.GetReaction(node1); //get the reaction at the first node
            Assert.AreEqual(10000, reaction.Z, 0.001);   // Check that we have calculated a reaction of 10 KiloNewtons in the Z-axis
            Assert.AreEqual(-30000, reaction.YY, 0.001); // Check that we have calculated a reaction of -30 KiloNewtonMetres around the YY axis.
            
            DisplacementVector displacement = results.GetDisplacement(node2);  // get the displacement at the second node
            Assert.AreEqual(-0.00214, displacement.Z, 0.0005);
            Assert.AreEqual(0.00107, displacement.YY, 0.0001);
        }
        
        [Test]
        public void ReversedCantilever()
        {
            FiniteElementModel model = new FiniteElementModel(ModelType.Beam1D);
            FiniteElementNode node1 = model.NodeFactory.Create(0);
            model.ConstrainNode(node1, DegreeOfFreedom.Z);
            model.ConstrainNode(node1, DegreeOfFreedom.YY);

            FiniteElementNode node2 = model.NodeFactory.Create(3.0);
            
            IMaterial material = new GenericElasticMaterial(0, 210000000000, 0, 0);
            ICrossSection section = new GenericCrossSection(0.0001, 0.0002);
            
            model.ElementFactory.CreateLinear1DBeam(node2, node1, material, section);  //connecting the nodes in reverse order to the Cantilever() example
            
            ForceVector force = model.ForceFactory.CreateFor1DBeam(-10000, 0);
            model.ApplyForceToNode(force, node2);
            
            IFiniteElementSolver solver = new MatrixInversionLinearSolver(model);
            FiniteElementResults results = solver.Solve();
            
            ReactionVector reaction = results.GetReaction(node1);
            Assert.AreEqual(10000, reaction.Z, 0.001);
            Assert.AreEqual(-30000, reaction.YY, 0.001);
            
            DisplacementVector displacement = results.GetDisplacement(node2);
            Assert.AreEqual(-0.00214, displacement.Z, 0.0005);
            Assert.AreEqual(0.00107, displacement.YY, 0.0001);
        }
        
        [Test]
        public void ThreeNodeCantilever()
        {
            FiniteElementModel model = new FiniteElementModel(ModelType.Beam1D);
            FiniteElementNode node1 = model.NodeFactory.Create(0);
            model.ConstrainNode(node1, DegreeOfFreedom.Z);
            model.ConstrainNode(node1, DegreeOfFreedom.YY);

            FiniteElementNode node2 = model.NodeFactory.Create(1.5);
            FiniteElementNode node3 = model.NodeFactory.Create(3.0);
            
            IMaterial material = new GenericElasticMaterial(0, 210000000000, 0, 0);
            ICrossSection section = new GenericCrossSection(0.0001, 0.0002);
            
            model.ElementFactory.CreateLinear1DBeam(node1, node2, material, section);
            model.ElementFactory.CreateLinear1DBeam(node2, node3, material, section);
            
            ForceVector force = model.ForceFactory.CreateFor1DBeam(-10000, 0);
            model.ApplyForceToNode(force, node3);
            
            IFiniteElementSolver solver = new MatrixInversionLinearSolver(model);
            FiniteElementResults results = solver.Solve();
            
            ReactionVector reaction = results.GetReaction(node1);
            Assert.AreEqual(10000, reaction.Z, 0.001);
            Assert.AreEqual(-30000, reaction.YY, 0.001);
            
            DisplacementVector displacement2 = results.GetDisplacement(node2);
            Assert.AreEqual(-0.000670, displacement2.Z, 0.0005);
            Assert.AreEqual(0.000804, displacement2.YY, 0.0001);
            
            DisplacementVector displacement3 = results.GetDisplacement(node3);
            Assert.AreEqual(-0.00214, displacement3.Z, 0.0005);
            Assert.AreEqual(0.00107, displacement3.YY, 0.0001);
        }
        
        /// <summary>
        /// Creates a beam between two nodes spaced 1 metre apart.
        /// The beam is pinned at the nodes, so is restricted from translating but not from rotating
        /// We apply a moment at node 1 and check that the same moment is transferred along the beam to node 2
        /// </summary>
        [Test]
        public void SimplySupportedBeam() //TODO verify using independent check
        {
            FiniteElementModel model = new FiniteElementModel(ModelType.Beam1D); // we will create and analyze a 1D beam system
            FiniteElementNode node1 = model.NodeFactory.Create(0); // create a node at the origin
            model.ConstrainNode(node1, DegreeOfFreedom.Z); // constrain the node from moving in the Z-axis

            FiniteElementNode node2 = model.NodeFactory.Create(1.0); // create a second node at a distance 1 metre along the X axis
            model.ConstrainNode(node2, DegreeOfFreedom.Z); // constrain the node from moving in the Z-axis
            
            IMaterial material = new GenericElasticMaterial(0, 210000000000, 0, 0);
            ICrossSection section = new GenericCrossSection(0.0001, 0.0002);
            
            model.ElementFactory.CreateLinear3DBeam(node1, node2, material, section); // create a spring between the two nodes of a stiffness of 2000 Newtons per metre
            
            ForceVector moment = model.ForceFactory.CreateFor1DBeam(0, 10000); // Create a clockwise(?) moment of 10 KiloNewtonmetres around the yy axis
            model.ApplyForceToNode(moment, node1); // Apply that moment to the first node
            
            IFiniteElementSolver solver = new MatrixInversionLinearSolver(model); // Create a new instance of the solver class and pass it the model to solve
            FiniteElementResults results = solver.Solve(); // ask the solver to solve the model and return results
            
            // check the results
            DisplacementVector displacementAtNode1 = results.GetDisplacement(node1);
            Assert.AreEqual(0, displacementAtNode1.Z, 0.001);
            Assert.AreEqual(0.00007936, displacementAtNode1.YY, 0.00000001);
            
            DisplacementVector displacementAtNode2 = results.GetDisplacement(node2);
            Assert.AreEqual(0, displacementAtNode2.Z, 0.001);
            Assert.AreEqual(-0.00003968, displacementAtNode2.YY, 0.00000001);
            
            ReactionVector reactionAtNode1 = results.GetReaction(node1);
            Assert.AreEqual(-10000, reactionAtNode1.Z, 0.001);
            Assert.AreEqual(0, reactionAtNode1.YY, 0.001);
            
            ReactionVector reactionAtNode2 = results.GetReaction(node2);
            Assert.AreEqual(10000, reactionAtNode2.Z, 0.001);
            Assert.AreEqual(0, reactionAtNode2.YY, 0.001);
        }
        
        /// <summary>
        /// (1)----(2)----(3)
        /// </summary>
        [Test]
        public void ThreeNodeSimplySupportedBeam() //TODO verify results using independent check
        {
            FiniteElementModel model = new FiniteElementModel(ModelType.Frame2D);
            FiniteElementNode node1 = model.NodeFactory.CreateFor2DTruss(0, 0);
            model.ConstrainNode(node1, DegreeOfFreedom.X);
            model.ConstrainNode(node1, DegreeOfFreedom.Z);

            FiniteElementNode node2 = model.NodeFactory.CreateFor2DTruss(1, 0);
            
            FiniteElementNode node3 = model.NodeFactory.CreateFor2DTruss(2, 0);
            model.ConstrainNode(node3, DegreeOfFreedom.Z);
            
            IMaterial material = new GenericElasticMaterial(0, 10000000000, 0, 0);
            ICrossSection section = new GenericCrossSection(0.0001, 0.0002);
            
            model.ElementFactory.CreateLinear1DBeam(node1, node2, material, section);
            model.ElementFactory.CreateLinear1DBeam(node2,node3,material,section);
            
            ForceVector force = model.ForceFactory.Create(0, 0, -10000, 0, 0, 0);
            model.ApplyForceToNode(force, node2);
            
            IFiniteElementSolver solver = new MatrixInversionLinearSolver(model);
            
            Stiffness.GlobalModelStiffnessMatrixBuilder gmsmb = new SharpFE.Stiffness.GlobalModelStiffnessMatrixBuilder(model);
            Console.WriteLine(gmsmb.BuildKnownForcesUnknownDisplacementStiffnessMatrix());
            
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
            
            Assert.AreEqual(-0.000833333, node2Displacement.Z, 0.0000001);
            Assert.AreEqual(5000, node1Reaction.Z, 0.001);
            Assert.AreEqual(5000, node3Reaction.Z, 0.001);
            Assert.AreEqual(0, node2Displacement.YY, 0.0001);
            Assert.AreEqual( 0.00125, node1Displacement.YY, 0.0000001);
            Assert.AreEqual(-0.00125, node3Displacement.YY, 0.0000001);
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
            FiniteElementModel model = new FiniteElementModel(ModelType.Frame2D);
            FiniteElementNode node1 = model.NodeFactory.CreateFor2DTruss(-10, 0);
            model.ConstrainNode(node1, DegreeOfFreedom.X);
            model.ConstrainNode(node1, DegreeOfFreedom.Z);

            FiniteElementNode node2 = model.NodeFactory.CreateFor2DTruss(-10, 10);
            FiniteElementNode node3 = model.NodeFactory.CreateFor2DTruss(0, 14);
            FiniteElementNode node4 = model.NodeFactory.CreateFor2DTruss(10, 10);
            
            FiniteElementNode node5 = model.NodeFactory.CreateFor2DTruss(10, 0);
            model.ConstrainNode(node5, DegreeOfFreedom.X);
            model.ConstrainNode(node5, DegreeOfFreedom.Z);
            
            IMaterial material = new GenericElasticMaterial(0, 210000000000, 0, 0);
            ICrossSection section = new GenericCrossSection(0.0001, 0.0002);
            
            model.ElementFactory.CreateLinear1DBeam(node1, node2, material, section);
            model.ElementFactory.CreateLinear1DBeam(node2, node3, material, section);
            model.ElementFactory.CreateLinear1DBeam(node3, node4, material, section);
            model.ElementFactory.CreateLinear1DBeam(node4, node5, material, section);
            
            ForceVector force = model.ForceFactory.Create(0, 0, 10000, 0, 0, 0);
            model.ApplyForceToNode(force, node3);
            
            IFiniteElementSolver solver = new LinearSolverSVD(model);
            FiniteElementResults results = solver.Solve();
            
            DisplacementVector node1Displacement = results.GetDisplacement(node1);
            DisplacementVector node2Displacement = results.GetDisplacement(node2);
            DisplacementVector node3Displacement = results.GetDisplacement(node3);
            DisplacementVector node4Displacement = results.GetDisplacement(node4);
            DisplacementVector node5Displacement = results.GetDisplacement(node5);
            ReactionVector node1Reaction = results.GetReaction(node1);
            ReactionVector node5Reaction = results.GetReaction(node5);
            
            Console.WriteLine("node1 displacements : " + node1Displacement);
            Console.WriteLine("node2 displacements : " + node2Displacement);
            Console.WriteLine("node3 displacements : " + node3Displacement);
            Console.WriteLine("node4 displacements : " + node4Displacement);
            Console.WriteLine("node5 displacements : " + node5Displacement);
            Console.WriteLine("node1 reactions : " + node1Reaction);
            Console.WriteLine("node5 reactions : " + node5Reaction);
            
            Assert.AreEqual(0, node1Displacement.X, 0.0001);
            Assert.AreEqual(0, node1Displacement.Z, 0.0001);
            Assert.AreEqual(0.0010985, node1Displacement.YY, 0.0001);
            
            Assert.AreEqual(0.004027, node2Displacement.X, 0.0001);
            Assert.AreEqual(0.002381, node2Displacement.Z, 0.0001);
            Assert.AreEqual(-0.000996, node2Displacement.YY, 0.0001);
            
            Assert.AreEqual(0, node3Displacement.X, 0.0001);
            Assert.AreEqual(0.01723, node3Displacement.Z, 0.0001);
            Assert.AreEqual(0, node3Displacement.YY, 0.0001);
            
            Assert.AreEqual(-0.004027, node4Displacement.X, 0.0001);
            Assert.AreEqual(0.002381, node4Displacement.Z, 0.0001);
            Assert.AreEqual(0.000996, node4Displacement.YY, 0.0001);
            
            Assert.AreEqual(0, node5Displacement.X, 0.0001);
            Assert.AreEqual(0, node5Displacement.Z, 0.0001);
            Assert.AreEqual(-0.0010985, node5Displacement.YY, 0.0001);
            
            Console.WriteLine(node1Reaction);
            Assert.AreEqual(-5000, node1Reaction.Z, 1);
            Assert.AreEqual(-1759, node1Reaction.X, 1);
            Assert.AreEqual(0, node1Reaction.YY, 1);
            
            Console.WriteLine(node5Reaction);
            Assert.AreEqual(-5000, node5Reaction.Z, 1);
            Assert.AreEqual(1759, node5Reaction.X, 1);
            Assert.AreEqual(0, node5Reaction.YY, 1);
        }
        
        ///<summary>
        /// Example problem and results are derived from:
        /// MATLAB Codes for Finite Element Analysis, Solid Mechanics and its applications Volume 157, A.J.M. Ferreira, Springer 2010
        /// Section 7.2, page 91
        /// </summary>
        [Test]
        public void Calculate2DFrameOf3BeamsAnd12Dof()
        {
            FiniteElementModel model = new FiniteElementModel(ModelType.Frame2D);
            FiniteElementNode node1 = model.NodeFactory.CreateFor2DTruss(0,3);
            model.ConstrainNode(node1, DegreeOfFreedom.X);
            model.ConstrainNode(node1, DegreeOfFreedom.Z);
            model.ConstrainNode(node1, DegreeOfFreedom.YY);

            FiniteElementNode node2 = model.NodeFactory.CreateFor2DTruss(3,3);
            
            FiniteElementNode node3 = model.NodeFactory.CreateFor2DTruss(6,0);
            
            FiniteElementNode node4 = model.NodeFactory.CreateFor2DTruss(9,0);
            model.ConstrainNode(node4, DegreeOfFreedom.X);
            model.ConstrainNode(node4, DegreeOfFreedom.Z);
            model.ConstrainNode(node4, DegreeOfFreedom.YY);
            
            IMaterial material = new GenericElasticMaterial(0, 210000000000, 0, 0);
            ICrossSection section = new GenericCrossSection(0.0001, 0.0002);
            
            model.ElementFactory.CreateLinear1DBeam(node1, node2, material, section);
            model.ElementFactory.CreateLinear1DBeam(node2, node3, material, section);
            model.ElementFactory.CreateLinear1DBeam(node3, node4, material, section);
            
            ForceVector force2 = model.ForceFactory.Create(0, 0, -10000, 0, 5000, 0);
            model.ApplyForceToNode(force2, node2);
            
            
            ForceVector force3 = model.ForceFactory.Create(0, 0, -10000, 0, -5000, 0);
            model.ApplyForceToNode(force3, node3);
            
            IFiniteElementSolver solver = new LinearSolverSVD(model);
            FiniteElementResults results = solver.Solve();
            
            DisplacementVector node2Displacement = results.GetDisplacement(node2);
            DisplacementVector node3Displacement = results.GetDisplacement(node3);
            ReactionVector node1Reaction = results.GetReaction(node1);
            ReactionVector node4Reaction = results.GetReaction(node4);
            
            
            Assert.AreEqual(0, node2Displacement.X, 0.0001);
            Assert.AreEqual(-0.0013496, node2Displacement.Z, 0.0001); ///NOTE this value of -0.0013496 matches the example, but -0.0314233 was calculated using a commercial finite element software
            Assert.AreEqual(0.00059173, node2Displacement.YY, 0.0001); ///NOTE this value of 0.00059173 does not match the example, but was verified using a commercial finite element software
            
            Assert.AreEqual(0, node3Displacement.X, 0.0001);
            Assert.AreEqual(-0.0013496, node3Displacement.Z, 0.0001); ///NOTE this value matches the example, but was verified using a commercial finite element software
            Assert.AreEqual(-0.00059173, node3Displacement.YY, 0.0001); ///NOTE this value of -0.00059173 does not match the example, but was verified using a commercial finite element software
            
            Assert.AreEqual(0, node1Reaction.X, 1);
            Assert.AreEqual(10000, node1Reaction.Z, 1);
            Assert.AreEqual(-23284, node1Reaction.YY, 1); ///NOTE this value of -23284 does not match the example, but was verified using a commercial finite element software
            
            Console.WriteLine(node4Reaction);
            Assert.AreEqual(0, node4Reaction.X, 1);
            Assert.AreEqual(10000, node4Reaction.Z, 1);
            Assert.AreEqual(23284, node4Reaction.YY, 1); ///NOTE this value of 23284 does not match the example, but was verified using a commercial finite element software
            
        }
        
        ///<summary>
        /// Example problem and results are derived from:
        /// MATLAB Codes for Finite Element Analysis, Solid Mechanics and its applications Volume 157, A.J.M. Ferreira, Springer 2010
        /// Section 7.3, page 95
        /// </summary>
        [Test]
        public void Calculate2DPortalFrameOf3BeamsAnd12Dof()
        {
            var model = new FiniteElementModel(ModelType.Frame2D);
            var node1 = model.NodeFactory.CreateFor2DTruss(-3, 0);
            model.ConstrainNode(node1, DegreeOfFreedom.X);
            model.ConstrainNode(node1, DegreeOfFreedom.Z);
            model.ConstrainNode(node1, DegreeOfFreedom.YY);

            FiniteElementNode node2 = model.NodeFactory.CreateFor2DTruss(-3, 6);
            
            FiniteElementNode node3 = model.NodeFactory.CreateFor2DTruss(3, 6);
            
            FiniteElementNode node4 = model.NodeFactory.CreateFor2DTruss(3, 0);
            model.ConstrainNode(node4, DegreeOfFreedom.X);
            model.ConstrainNode(node4, DegreeOfFreedom.Z);
            model.ConstrainNode(node4, DegreeOfFreedom.YY);
            
            IMaterial material = new GenericElasticMaterial(0, 210000000000, 0, 0);
            ICrossSection section = new GenericCrossSection(0.0002, 0.0002);
            
            model.ElementFactory.CreateLinear1DBeam(node1, node2, material, section);
            model.ElementFactory.CreateLinear1DBeam(node2, node3, material, section);
            model.ElementFactory.CreateLinear1DBeam(node3, node4, material, section);
            
            ForceVector force2 = model.ForceFactory.Create(15000, 0, 0, 0, -10000, 0);
            model.ApplyForceToNode(force2, node2);
            
            IFiniteElementSolver solver = new LinearSolverSVD(model);
            FiniteElementResults results = solver.Solve();
            
            DisplacementVector node2Displacement = results.GetDisplacement(node2);
            DisplacementVector node3Displacement = results.GetDisplacement(node3);
            ReactionVector node1Reaction = results.GetReaction(node1);
            ReactionVector node4Reaction = results.GetReaction(node4);
            
            Assert.AreEqual(0.0052843, node2Displacement.X, 0.0001);
            Assert.AreEqual(0.0006522, node2Displacement.Z, 0.0001);
            Assert.AreEqual(0.0005, node2Displacement.YY, 0.0001);
            
            Assert.AreEqual(0.0044052, node3Displacement.X, 0.0001);
            Assert.AreEqual(-0.0006522, node3Displacement.Z, 0.0001);
            Assert.AreEqual(0.0006, node3Displacement.YY, 0.0001);
            
            Assert.AreEqual(-9000, node1Reaction.X, 500);
            Assert.AreEqual(-5000, node1Reaction.Z, 500);
            Assert.AreEqual(-30022, node1Reaction.YY, 500);
            
            Console.WriteLine(node4Reaction);
            Assert.AreEqual(-6000, node4Reaction.X, 500);
            Assert.AreEqual(5000, node4Reaction.Z, 500);
            Assert.AreEqual(-22586, node4Reaction.YY, 500); 
        }
        
        [Test]
        public void Example4_2_FirstCourseInTheFiniteElementMethod_Logan_4thEd()
        {
            var model = new FiniteElementModel(ModelType.Beam1D);
            var node1 = model.NodeFactory.Create(-240); //-20 feet in inches
            model.ConstrainNode(node1, DegreeOfFreedom.Z);
            model.ConstrainNode(node1, DegreeOfFreedom.YY);
            var node2 = model.NodeFactory.Create(-120); //-10 feet in inches
            var node3 = model.NodeFactory.Create(  0);
            model.ConstrainNode(node3, DegreeOfFreedom.Z);
            var node4 = model.NodeFactory.Create( 120); //10 feet in inches
            var node5 = model.NodeFactory.Create( 240); //20 feet in inches
            model.ConstrainNode(node5, DegreeOfFreedom.Z);
            model.ConstrainNode(node5, DegreeOfFreedom.YY);
                        
            IMaterial material = new GenericElasticMaterial(0, 30000000, 0, 0); //E=30E6 psi
            ICrossSection section = new GenericCrossSection(1, 500); //I = 500 in^4. (A is ignored)
            
            var beam1 = model.ElementFactory.CreateLinear1DBeam(node1, node2, material, section);
            var beam2 = model.ElementFactory.CreateLinear1DBeam(node2, node3, material, section);
            var beam3 = model.ElementFactory.CreateLinear1DBeam(node3, node4, material, section);
            var beam4 = model.ElementFactory.CreateLinear1DBeam(node4, node5, material, section);
            
            var force = model.ForceFactory.CreateFor1DBeam(-10000, 0); //10,000 lb
            model.ApplyForceToNode(force, node2);
            model.ApplyForceToNode(force, node4);
            
            var solver = new MatrixInversionLinearSolver(model);
            var results = solver.Solve();
            
            var displacementNode2 = results.GetDisplacement(node2);
            var displacementNode4 = results.GetDisplacement(node4);
            
            Assert.AreEqual(-0.048, displacementNode2.Z, 0.0005);
            Assert.AreEqual(-0.048, displacementNode2.Z, 0.0005);
            
            var reactionNode1 = results.GetReaction(node1);
            var reactionNode3 = results.GetReaction(node3);
            var reactionNode5 = results.GetReaction(node5);
            
            Assert.AreEqual(   5000,  reactionNode1.Z,  1);
            Assert.AreEqual(-300000,  reactionNode1.YY, 1); //NOTE the value in the book is -25000 lb-ft.  This is -300000 lb-in.
            Assert.AreEqual(  10000,  reactionNode3.Z,  1);
            Assert.AreEqual(      0,  reactionNode3.YY, 1);
            Assert.AreEqual(   5000,  reactionNode5.Z,  1);
            Assert.AreEqual( 300000,  reactionNode5.YY, 1); //NOTE the value in the book is  25000 lb-ft.  This is  300000 lb-in.
        }
        
        [Test]
        public void Example4_3_FirstCourseInTheFiniteElementMethod_Logan_4thEd()
        {
            var model = new FiniteElementModel(ModelType.Frame2D);
            var node1 = model.NodeFactory.CreateFor2DFrame(-3,  0);
            var node2 = model.NodeFactory.CreateFor2DFrame( 0,  0);
            var node3 = model.NodeFactory.CreateFor2DFrame( 3,  0);
            var node4 = model.NodeFactory.CreateFor2DFrame( 3, -1);
            
            model.ConstrainNode(node1, DegreeOfFreedom.X);
            model.ConstrainNode(node1, DegreeOfFreedom.Z);
            model.ConstrainNode(node1, DegreeOfFreedom.YY);
            
            model.ConstrainNode(node2, DegreeOfFreedom.X);
            model.ConstrainNode(node2, DegreeOfFreedom.Z);
            
            model.ConstrainNode(node3, DegreeOfFreedom.X);
            
            model.ConstrainNode(node4, DegreeOfFreedom.X);
            model.ConstrainNode(node4, DegreeOfFreedom.Z);
            model.ConstrainNode(node4, DegreeOfFreedom.YY);
            
            var material = new GenericElasticMaterial(0, 210000000000, 0, 0); //E = 210 GPa
            var section = new GenericCrossSection(1, 0.0002); // I = 2E-4 m^4
            
            var beam1 = model.ElementFactory.CreateLinear1DBeam(node1, node2, material, section);
            var beam2 = model.ElementFactory.CreateLinear1DBeam(node2, node3, material, section);
            var spring3 = model.ElementFactory.CreateLinearConstantSpring(node3, node4, 200000); //200 kN/m
            
            var force = model.ForceFactory.CreateFor1DBeam(-50000, 0); //50kN
            model.ApplyForceToNode(force, node3);
            
            var solver = new MatrixInversionLinearSolver(model);
            var results = solver.Solve();
            
            var displacementNode2 = results.GetDisplacement(node2);
            var displacementNode3 = results.GetDisplacement(node3);
            
            Assert.AreEqual( 0.00249, displacementNode2.YY, 0.000005); //FIXME wrong sign?!
            Assert.AreEqual(-0.0174,  displacementNode3.Z,  0.00005);
            Assert.AreEqual( 0.00747, displacementNode3.YY, 0.00001); //FIXME wrong sign?! //FIXME would prefer a tolerance of 0.000005
            
            var reactionNode1 = results.GetReaction(node1);
            var reactionNode2 = results.GetReaction(node2);
            var reactionNode4 = results.GetReaction(node4);
            
            Assert.AreEqual(-69600, reactionNode1.Z,  200); //FIXME this tolerance seems too high, should aim for 50.  Not sure if this is due to rounding in the book?
            Assert.AreEqual( 69700, reactionNode1.YY, 100); //FIXME this tolerance seems too high, should aim for 50.  Not sure if this is due to rounding in the book?
            Assert.AreEqual(116400, reactionNode2.Z,  200); //FIXME this tolerance seems too high, should aim for 50.  Not sure if this is due to rounding in the book?
            Assert.AreEqual(  3500, reactionNode4.Z,   50);
        }
        
        [Test]
        public void Example4_4_FirstCourseInTheFiniteElementMethod_Logan_4thEd()
        {
            var model = new FiniteElementModel(ModelType.Beam1D);
            var node1 = model.NodeFactory.Create(0);
            var node2 = model.NodeFactory.Create(3);
            var node3 = model.NodeFactory.Create(6);
            
            model.ConstrainNode(node1, DegreeOfFreedom.Z);
            model.ConstrainNode(node1, DegreeOfFreedom.YY);
            model.ConstrainNode(node3, DegreeOfFreedom.Z);
            model.ConstrainNode(node3, DegreeOfFreedom.YY);
            
            var material = new GenericElasticMaterial(0, 210000000000, 0, 0); //E = 210 GPa
            var section = new GenericCrossSection(1, 0.0004); //I = 4E-4 (A is ignored)
            
            var beam1 = model.ElementFactory.CreateLinear1DBeam(node1, node2, material, section);
            var beam2 = model.ElementFactory.CreateLinear1DBeam(node2, node3, material, section);
            
            var force = model.ForceFactory.CreateFor1DBeam(-10000, -20000); //10kN Z, 20kNm YY
            model.ApplyForceToNode(force, node2);
            
            var solver = new MatrixInversionLinearSolver(model);
            var results = solver.Solve();
            
            var displacementNode2 = results.GetDisplacement(node2);
            Assert.AreEqual(-0.0001339, displacementNode2.Z, 0.00000005);
            Assert.AreEqual(-0.00008928, displacementNode2.YY, 0.00000001); //FIXME rotation was inverse //FIXME would prefer tollerance of 0.000000005
            
            var reactionNode1 = results.GetReaction(node1);
            var reactionNode3 = results.GetReaction(node3);
            
            Assert.AreEqual( 10000, reactionNode1.Z,  1);
            Assert.AreEqual(-12500, reactionNode1.YY, 1); //FIXME rotation was inverse
            Assert.AreEqual(     0, reactionNode3.Z,  0.5);
            Assert.AreEqual(  2500, reactionNode3.YY, 1); //FIXME rotation was inverse
        }
    }
}
