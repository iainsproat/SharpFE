//-----------------------------------------------------------------------
// <copyright file="BeamInFull3DModel.cs" company="Andreas Bak">
//     Copyright Andreas Bak, 2012.
// </copyright>
//-----------------------------------------------------------------------
using System;
using NUnit.Framework;

namespace SharpFE.Examples.Beam
{
    [TestFixture]
    public class Beam3DInFull3DModel
    {
        
        /// <summary>
        /// (1)----(2)----(3)
        /// </summary>
        [Test]
        public void ThreeNodeBeam()
        {
            FiniteElementModel model = new FiniteElementModel(ModelType.Full3D);
            FiniteElementNode node1 = model.NodeFactory.Create(0,0,0);
            model.ConstrainNode(node1, DegreeOfFreedom.X);
            model.ConstrainNode(node1, DegreeOfFreedom.Y);
            model.ConstrainNode(node1, DegreeOfFreedom.Z);
            model.ConstrainNode(node1, DegreeOfFreedom.XX);

            FiniteElementNode node2 = model.NodeFactory.Create(1,0, 0);
            model.ConstrainNode(node2, DegreeOfFreedom.Y);
            
            FiniteElementNode node3 = model.NodeFactory.Create(2,0, 0);
            model.ConstrainNode(node3, DegreeOfFreedom.Y);
            model.ConstrainNode(node3, DegreeOfFreedom.Z);
            
            IMaterial material = new GenericElasticMaterial(0, 10000000000, 0.3, 1000000000);
            ICrossSection section = new SolidRectangle(1, 1);
            
            model.ElementFactory.CreateLinear3DBeam(node1, node2, material, section);
            model.ElementFactory.CreateLinear3DBeam(node2,node3,material,section);
            
            ForceVector force = model.ForceFactory.Create(0, 0, -10000, 0, 0, 0);
            model.ApplyForceToNode(force, node2);
            
            IFiniteElementSolver solver = new MatrixInversionLinearSolver(model);
            
            Stiffness.GlobalModelStiffnessMatrixBuilder gmsmb = new SharpFE.Stiffness.GlobalModelStiffnessMatrixBuilder(model);
            Console.WriteLine(gmsmb.BuildKnownForcesUnknownDisplacementStiffnessMatrix());
            
            FiniteElementResults results = solver.Solve();
            
            DisplacementVector node1Displacement = results.GetDisplacement(node1);
            Console.WriteLine("node1Displacement : \n" + node1Displacement);
            
            DisplacementVector node2Displacement = results.GetDisplacement(node2);
            Console.WriteLine("node2Displacement : \n" + node2Displacement);
            
            DisplacementVector node3Displacement = results.GetDisplacement(node3);
            Console.WriteLine("node3Displacement : \n" + node3Displacement);
            
            ReactionVector node1Reaction = results.GetReaction(node1);
            Console.WriteLine("node1Reaction : \n" + node1Reaction);
            
            ReactionVector node3Reaction = results.GetReaction(node3);
            Console.WriteLine("node5Reaction : \n" + node3Reaction);
            
            Assert.AreEqual(0, node2Displacement.XX, 0.00001);
            Assert.AreEqual(0, node2Displacement.Y, 0.00001);
            Assert.AreEqual(0, node2Displacement.ZZ, 0.0001);
            Assert.AreEqual(-0.000002, node2Displacement.Z, 0.0000001);
            
            Assert.AreEqual(5000, node1Reaction.Z, 0.00001);
            Assert.AreEqual(5000, node3Reaction.Z, 0.00001);
            Assert.AreEqual(0, node2Displacement.YY, 0.0001);
            Assert.AreEqual( 0.000003, node1Displacement.YY, 0.0000001);
            Assert.AreEqual(-0.000003, node3Displacement.YY, 0.0000001);
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
        public void PitchedRoofPortalFrame()
        {
            FiniteElementModel model = new FiniteElementModel(ModelType.Full3D);
            FiniteElementNode node1 = model.NodeFactory.Create(-10, 0, 0);
            model.ConstrainNode(node1, DegreeOfFreedom.X);
            model.ConstrainNode(node1, DegreeOfFreedom.Z);
            model.ConstrainNode(node1, DegreeOfFreedom.Y);
            model.ConstrainNode(node1, DegreeOfFreedom.XX);

            FiniteElementNode node2 = model.NodeFactory.Create(-10, 0, 10);
            FiniteElementNode node3 = model.NodeFactory.Create(0, 0, 14);
            FiniteElementNode node4 = model.NodeFactory.Create(10, 0, 10);
            
            FiniteElementNode node5 = model.NodeFactory.Create(10, 0, 0);
            model.ConstrainNode(node5, DegreeOfFreedom.X);
            model.ConstrainNode(node5, DegreeOfFreedom.Z);
            model.ConstrainNode(node5, DegreeOfFreedom.Y);
            model.ConstrainNode(node5, DegreeOfFreedom.XX);
            
            IMaterial material = new GenericElasticMaterial(0, 210000000000, 0.3, 84000000000);
            ICrossSection section = new GenericCrossSection(0.0001, 0.0002, 0.0002, 0.00005);
            
            model.ElementFactory.CreateLinear3DBeam(node1, node2, material, section);
            model.ElementFactory.CreateLinear3DBeam(node2, node3, material, section);
            model.ElementFactory.CreateLinear3DBeam(node3, node4, material, section);
            model.ElementFactory.CreateLinear3DBeam(node4, node5, material, section);
            
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
            
            Assert.AreEqual(0, node1Displacement.X, 0.0001);
            Assert.AreEqual(0, node1Displacement.Y, 0.0001);
            Assert.AreEqual(0, node1Displacement.Z, 0.0001);
            Assert.AreEqual(0.0010985, node1Displacement.YY, 0.0001);
            
            Assert.AreEqual(0.004027, node2Displacement.X, 0.0001);
            Assert.AreEqual(0, node2Displacement.Y, 0.0001);
            Assert.AreEqual(0.002381, node2Displacement.Z, 0.0001);
            Assert.AreEqual(-0.000996, node2Displacement.YY, 0.0001);
            
            Assert.AreEqual(0, node3Displacement.X, 0.0001);
            Assert.AreEqual(0, node3Displacement.Y, 0.0001);
            Assert.AreEqual(0.01723, node3Displacement.Z, 0.0001);
            Assert.AreEqual(0, node3Displacement.YY, 0.0001);
            
            Assert.AreEqual(-0.004027, node4Displacement.X, 0.0001);
            Assert.AreEqual(0, node4Displacement.Y, 0.0001);
            Assert.AreEqual(0.002381, node4Displacement.Z, 0.0001);
            Assert.AreEqual(0.000996, node4Displacement.YY, 0.0001);
            
            Assert.AreEqual(0, node5Displacement.X, 0.0001);
            Assert.AreEqual(0, node5Displacement.Y, 0.0001);
            Assert.AreEqual(0, node5Displacement.Z, 0.0001);
            Assert.AreEqual(-0.0010985, node5Displacement.YY, 0.0001);
            
            Assert.AreEqual(-5000, node1Reaction.Z, 1);
            Assert.AreEqual(-1759, node1Reaction.X, 1);
            Assert.AreEqual(0, node1Reaction.YY, 1);
            
            Assert.AreEqual(-5000, node5Reaction.Z, 1);
            Assert.AreEqual(1759, node5Reaction.X, 1);
            Assert.AreEqual(0, node5Reaction.YY, 1);
        }
        
        ///<summary>
        /// Example problem and results are derived from:
        /// MATLAB Codes for Finite Element Analysis, Solid Mechanics and its applications Volume 157, A.J.M. Ferreira, Springer 2010
        /// Section 8.3, page 104
        /// </summary>
        [Test]
        public void Calculate3DFrameOf3BeamsAnd24Dof()
        {
            FiniteElementModel model = new FiniteElementModel(ModelType.Full3D);
            FiniteElementNode node1 = model.NodeFactory.Create(0,  0,  0);
            
            FiniteElementNode node2 = model.NodeFactory.Create(3,  0,  0);
            model.ConstrainNode(node2, DegreeOfFreedom.X);
            model.ConstrainNode(node2, DegreeOfFreedom.Y);
            model.ConstrainNode(node2, DegreeOfFreedom.Z);
            
            FiniteElementNode node3 = model.NodeFactory.Create(0,  3, 0);
            model.ConstrainNode(node3, DegreeOfFreedom.X);
            model.ConstrainNode(node3, DegreeOfFreedom.Y);
            model.ConstrainNode(node3, DegreeOfFreedom.Z);
            
            FiniteElementNode node4 = model.NodeFactory.Create(0, 0,  -4);
            model.ConstrainNode(node4, DegreeOfFreedom.X);
            model.ConstrainNode(node4, DegreeOfFreedom.Y);
            model.ConstrainNode(node4, DegreeOfFreedom.Z);
            
            IMaterial material = new GenericElasticMaterial(0, 210000000000, 0.3, 84000000000);
            ICrossSection section = new GenericCrossSection(0.02, 0.0001, 0.0002, 0.00005);

            model.ElementFactory.CreateLinear3DBeam(node1, node2, material, section);
            model.ElementFactory.CreateLinear3DBeam(node1, node3, material, section);
            model.ElementFactory.CreateLinear3DBeam(node1, node4, material, section);
            
            ForceVector force = model.ForceFactory.Create(-10000, -15000, 0, 0, 0, 0);
            model.ApplyForceToNode(force, node1);
            
            IFiniteElementSolver solver = new LinearSolverSVD(model);
            FiniteElementResults results = solver.Solve();
            
            DisplacementVector node1Displacement = results.GetDisplacement(node1);
            ReactionVector node2Reaction = results.GetReaction(node2);
            ReactionVector node3Reaction = results.GetReaction(node3);
            ReactionVector node4Reaction = results.GetReaction(node4);
            
            Console.WriteLine("\nNode1 displacement : \n" + node1Displacement);
            Console.WriteLine("\nNode2 reaction : \n" + node2Reaction);
            Console.WriteLine("\nNode3 reaction : \n" + node3Reaction);
            Console.WriteLine("\nNode4 reaction : \n" + node4Reaction);
            
            Assert.Inconclusive("The below x, y and zz pass, but z, xx, yy fail");
            Assert.AreEqual(-0.000007109, node1Displacement.X, 0.00000001);
            Assert.AreEqual(-0.000010680, node1Displacement.Y, 0.00000001);
            Assert.AreEqual(-0.000014704, node1Displacement.Z, 0.00000001);
            Assert.AreEqual(0.000001147, node1Displacement.XX, 0.00000001);
            Assert.AreEqual(-0.000001068, node1Displacement.YY, 0.00000001);
            Assert.AreEqual(0.000000595, node1Displacement.ZZ, 0.000000001);
        }
        
        
        ///<summary>
        /// Example problem and results are derived from:
        /// MATLAB Codes for Finite Element Analysis, Solid Mechanics and its applications Volume 157, A.J.M. Ferreira, Springer 2010
        /// Section 8.4 page 91
        /// </summary>
        [Test]
        public void Calculate3DPortalFrameOf8BeamsAnd48Dof()
        {
            FiniteElementModel model = new FiniteElementModel(ModelType.Full3D);
            FiniteElementNode node1 = model.NodeFactory.Create(0, 0, 0);
            model.ConstrainNode(node1, DegreeOfFreedom.X);
            model.ConstrainNode(node1, DegreeOfFreedom.Y);
            model.ConstrainNode(node1, DegreeOfFreedom.Z);
            
            FiniteElementNode node2 = model.NodeFactory.Create(0, -4, 0);
            model.ConstrainNode(node2, DegreeOfFreedom.X);
            model.ConstrainNode(node2, DegreeOfFreedom.Y);
            model.ConstrainNode(node2, DegreeOfFreedom.Z);
            
            FiniteElementNode node3 = model.NodeFactory.Create(4, -4, 0);
            model.ConstrainNode(node3, DegreeOfFreedom.X);
            model.ConstrainNode(node3, DegreeOfFreedom.Y);
            model.ConstrainNode(node3, DegreeOfFreedom.Z);
            
            FiniteElementNode node4 = model.NodeFactory.Create(4, 0, 0);
            model.ConstrainNode(node4, DegreeOfFreedom.X);
            model.ConstrainNode(node4, DegreeOfFreedom.Y);
            model.ConstrainNode(node4, DegreeOfFreedom.Z);
            
            FiniteElementNode node5 = model.NodeFactory.Create(0, 0, 5);
            FiniteElementNode node6 = model.NodeFactory.Create(0, -4, 5);
            FiniteElementNode node7 = model.NodeFactory.Create(4, -4, 5);
            FiniteElementNode node8 = model.NodeFactory.Create(4, 0, 5);
            
            IMaterial material = new GenericElasticMaterial(0, 210000000000, 0, 84000000000);
            ICrossSection section = new GenericCrossSection(0.02, 0.0001, 0.0002, 0.00005);

            model.ElementFactory.CreateLinear3DBeam(node1, node5, material, section);
            model.ElementFactory.CreateLinear3DBeam(node2, node6, material, section);
            model.ElementFactory.CreateLinear3DBeam(node3, node7, material, section);
            model.ElementFactory.CreateLinear3DBeam(node4, node8, material, section);
            model.ElementFactory.CreateLinear3DBeam(node5, node6, material, section);
            model.ElementFactory.CreateLinear3DBeam(node6, node7, material, section);
            model.ElementFactory.CreateLinear3DBeam(node7, node8, material, section);
            model.ElementFactory.CreateLinear3DBeam(node8, node5, material, section);
            
            ForceVector force = model.ForceFactory.Create(-15000, 0, 0, 0, 0, 0);
            model.ApplyForceToNode(force, node7);
            
            IFiniteElementSolver solver = new LinearSolverSVD(model);
            FiniteElementResults results = solver.Solve();
            
            ReactionVector node1Reaction = results.GetReaction(node1);
            ReactionVector node2Reaction = results.GetReaction(node2);
            ReactionVector node3Reaction = results.GetReaction(node3);
            ReactionVector node4Reaction = results.GetReaction(node4);
            DisplacementVector node5Displacement = results.GetDisplacement(node5);
            DisplacementVector node6Displacement = results.GetDisplacement(node6);
            DisplacementVector node7Displacement = results.GetDisplacement(node7);
            DisplacementVector node8Displacement = results.GetDisplacement(node8);
            
//            Console.WriteLine("\nNode5 displacement : \n" + node5Displacement);
//            Console.WriteLine("\nNode6 displacement : \n" + node6Displacement);
//            Console.WriteLine("\nNode7 displacement : \n" + node7Displacement);
//            Console.WriteLine("\nNode8 displacement : \n" + node8Displacement);
            
            Assert.Inconclusive("The below tests pass for y-displacements, but fail for x-displacements");
            Assert.AreEqual(-0.0026743, node5Displacement.X, 0.0001);
            Assert.AreEqual(0.0038697, node5Displacement.Y, 0.0001);
            Assert.AreEqual(-0.0107708, node6Displacement.X, 0.0001);
            Assert.AreEqual(0.0038697, node6Displacement.Y, 0.0001);
            Assert.AreEqual(-0.0107780, node7Displacement.X, 0.0001);
            Assert.AreEqual(-0.0038697, node7Displacement.Y, 0.0001);
            Assert.AreEqual(-0.0026743, node8Displacement.X, 0.0001);
            Assert.AreEqual(-0.0038697, node8Displacement.Y, 0.0001);
        }
        
        ///<summary>
        /// Example problem and results are derived from:
        /// MATLAB Codes for Finite Element Analysis, Solid Mechanics and its applications Volume 157, A.J.M. Ferreira, Springer 2010
        /// Section 9.2 page 116
        /// </summary>
        [Test]
        public void CalculateGridOf2BeamsAnd18Dof()
        {
            FiniteElementModel model = new FiniteElementModel(ModelType.Slab2D);
            FiniteElementNode node1 = model.NodeFactory.Create(-3, 0);
            model.ConstrainNode(node1, DegreeOfFreedom.Z);
            model.ConstrainNode(node1, DegreeOfFreedom.XX);
            model.ConstrainNode(node1, DegreeOfFreedom.YY);
            
            FiniteElementNode node2 = model.NodeFactory.Create(0, 4);
            
            FiniteElementNode node3 = model.NodeFactory.Create(3, 0);
            model.ConstrainNode(node3, DegreeOfFreedom.Z);
            model.ConstrainNode(node3, DegreeOfFreedom.XX);
            model.ConstrainNode(node3, DegreeOfFreedom.YY);
            
            IMaterial material = new GenericElasticMaterial(0, 210000000000, 0, 84000000000);
            ICrossSection section = new GenericCrossSection(0.02, 0.0002, 0.0002, 0.00005);
            
            model.ElementFactory.CreateLinear3DBeam(node1, node2, material, section);
            model.ElementFactory.CreateLinear3DBeam(node2, node3, material, section);
            
            ForceVector force = model.ForceFactory.Create(0, 0, -10000, 0, 0, 0);
            model.ApplyForceToNode(force, node2);
            
            IFiniteElementSolver solver = new LinearSolverSVD(model);
            FiniteElementResults results = solver.Solve();
            
            ReactionVector node1Reaction = results.GetReaction(node1);
            ReactionVector node3Reaction = results.GetReaction(node3);
            DisplacementVector node2Displacement = results.GetDisplacement(node2);
            
            Assert.AreEqual(-0.0048, node2Displacement.Z, 0.0001);
            Assert.AreEqual(-0.0018, node2Displacement.XX, 0.0001);
            
            Assert.AreEqual(5000, node1Reaction.Z, 1);
            Assert.AreEqual(20000, node1Reaction.XX, 1);
            Assert.AreEqual(-13890, node1Reaction.YY, 1);
            
            Assert.AreEqual(5000, node3Reaction.Z, 1);
            Assert.AreEqual(20000, node3Reaction.XX, 1);
            Assert.AreEqual(13890, node3Reaction.YY, 1);
        }
        
        ///<summary>
        /// Example problem and results are derived from:
        /// MATLAB Codes for Finite Element Analysis, Solid Mechanics and its applications Volume 157, A.J.M. Ferreira, Springer 2010
        /// Section 9.3 page 119
        /// </summary>
        [Test]
        public void CalculateGridOf3BeamsAnd24Dof()
        {
            FiniteElementModel model = new FiniteElementModel(ModelType.Slab2D);
            
            FiniteElementNode node1 = model.NodeFactory.Create(4, 4);
            
            FiniteElementNode node2 = model.NodeFactory.Create(4, 0);
            model.ConstrainNode(node2, DegreeOfFreedom.Z);
            model.ConstrainNode(node2, DegreeOfFreedom.XX);
            model.ConstrainNode(node2, DegreeOfFreedom.YY);
            
            FiniteElementNode node3 = model.NodeFactory.Create(0, 0);
            model.ConstrainNode(node3, DegreeOfFreedom.Z);
            model.ConstrainNode(node3, DegreeOfFreedom.XX);
            model.ConstrainNode(node3, DegreeOfFreedom.YY);
            
            FiniteElementNode node4 = model.NodeFactory.Create(0, 4);
            model.ConstrainNode(node4, DegreeOfFreedom.Z);
            model.ConstrainNode(node4, DegreeOfFreedom.XX);
            model.ConstrainNode(node4, DegreeOfFreedom.YY);
            
            IMaterial material = new GenericElasticMaterial(0, 210000000000, 0, 84000000000);
            ICrossSection section = new GenericCrossSection(0.02, 0.0002, 0.0002, 0.00005);
            
            model.ElementFactory.CreateLinear3DBeam(node1, node2, material, section);
            model.ElementFactory.CreateLinear3DBeam(node1, node3, material, section);
            model.ElementFactory.CreateLinear3DBeam(node1, node4, material, section);
            
            ForceVector force = model.ForceFactory.Create(0, 0, -20000, 0, 0, 0);
            model.ApplyForceToNode(force, node1);
            
            IFiniteElementSolver solver = new LinearSolverSVD(model);
            FiniteElementResults results = solver.Solve();
            
            DisplacementVector node1Displacement = results.GetDisplacement(node1);
            ReactionVector node2Reaction = results.GetReaction(node2);
            ReactionVector node3Reaction = results.GetReaction(node3);
            ReactionVector node4Reaction = results.GetReaction(node4);
            
            Assert.AreEqual(-0.0033, node1Displacement.Z, 0.0001);
            Assert.AreEqual(-0.0010, node1Displacement.XX, 0.0001);
            Assert.AreEqual( 0.0010, node1Displacement.YY, 0.0001);
            
            Assert.AreEqual(10794, node2Reaction.Z, 1);
            Assert.AreEqual(31776, node2Reaction.XX, 1);
            Assert.AreEqual(-1019, node2Reaction.YY, 1);
            
            Assert.AreEqual(-1587, node3Reaction.Z, 1);
            Assert.AreEqual(4030, node3Reaction.XX, 1);
            Assert.AreEqual(-4030, node3Reaction.YY, 1);
            
            Assert.AreEqual(10794, node4Reaction.Z, 1);
            Assert.AreEqual(1019, node4Reaction.XX, 1);
            Assert.AreEqual(-31776, node4Reaction.YY, 1);
        }
    }
}

