/*
 * Copyright Iain Sproat, 2013
 * User: ispro
 * Date: 05/07/2013
 * 
 */
using System;
using NUnit.Framework;

namespace SharpFE.Examples.Membrane
{
    [TestFixture]
    public class Quad
    {
        /// <summary>
        /// 4-------3
        /// |       |
        /// |       |
        /// |       |
        /// 1-------2
        /// </summary>
        [Test]
		public void OneQuadMembrane()
		{
			FiniteElementModel model = new FiniteElementModel(ModelType.Membrane2D);
			FiniteElementNode node1 = model.NodeFactory.Create(0.0, 0.0);
			model.ConstrainNode(node1, DegreeOfFreedom.X);
			model.ConstrainNode(node1, DegreeOfFreedom.Y);

			FiniteElementNode node2 = model.NodeFactory.Create(1.0, 0.0);
			model.ConstrainNode(node2, DegreeOfFreedom.Y);
			
			FiniteElementNode node3 = model.NodeFactory.Create(1.0, 1.0);
			
			FiniteElementNode node4 = model.NodeFactory.Create(0.0, 1.0);
			
			IMaterial material = new GenericElasticMaterial(0, 210000000000, 0.3, 0);
			
			model.ElementFactory.CreateLinearConstantStressQuadrilateral(node1, node2, node3, node4, material, 0.1);
			
			ForceVector force = model.ForceFactory.Create(0, -10000);
			model.ApplyForceToNode(force, node3);
			model.ApplyForceToNode(force, node4);
			
			IFiniteElementSolver solver = new MatrixInversionLinearSolver(model);
			FiniteElementResults results = solver.Solve();
			
			ReactionVector reaction1 = results.GetReaction(node1);
//			Console.WriteLine("\nReaction1 : \n" + reaction1);
			ReactionVector reaction2 = results.GetReaction(node2);
//			Console.WriteLine("\nReaction2 : \n" + reaction2);
			Assert.AreEqual(20000, reaction1.Y + reaction2.Y, 1);
			DisplacementVector displacement3 = results.GetDisplacement(node3);
			DisplacementVector displacement4 = results.GetDisplacement(node4);
//			Console.WriteLine("\nDisplacement3 : \n" + displacement3);
//			Console.WriteLine("\nDisplacement4 : \n" + displacement4);
			Assert.AreEqual( 0.0000002857, displacement3.X, 0.0000000001);
			Assert.AreEqual(-0.0000009524, displacement3.Y, 0.0000000001);
			
			Assert.AreEqual(0, displacement4.X, 0.0000001);
			Assert.AreEqual(-0.0000009524, displacement4.Y, 0.0000000001);
		}
		
		/// <summary>
        /// 4-------3
        /// |       |
        /// |       |
        /// |       |
        /// 1-------2
        /// </summary>
        [Test]
		public void OneQuadMembraneLateralLoad()
		{
			FiniteElementModel model = new FiniteElementModel(ModelType.Membrane2D);
			FiniteElementNode node1 = model.NodeFactory.Create(0.0, 0.0);
			model.ConstrainNode(node1, DegreeOfFreedom.X);
			model.ConstrainNode(node1, DegreeOfFreedom.Y);

			FiniteElementNode node2 = model.NodeFactory.Create(1.0, 0.0);
			model.ConstrainNode(node2, DegreeOfFreedom.Y);
			
			FiniteElementNode node3 = model.NodeFactory.Create(1.0, 1.0);
			
			FiniteElementNode node4 = model.NodeFactory.Create(0.0, 1.0);
			
			IMaterial material = new GenericElasticMaterial(0, 210000000000, 0.3, 0);
			
			model.ElementFactory.CreateLinearConstantStressQuadrilateral(node1, node2, node3, node4, material, 0.1);
			
			ForceVector force = model.ForceFactory.Create(10000, 0);
			model.ApplyForceToNode(force, node4);
			
			IFiniteElementSolver solver = new MatrixInversionLinearSolver(model);
			FiniteElementResults results = solver.Solve();
			
			ReactionVector reaction1 = results.GetReaction(node1);
//			Console.WriteLine("\nReaction1 : \n" + reaction1);
			ReactionVector reaction2 = results.GetReaction(node2);
//			Console.WriteLine("\nReaction2 : \n" + reaction2);
			Assert.AreEqual(0, reaction1.Y + reaction2.Y, 1);
			Assert.AreEqual(-10000, reaction1.X, 1);
			
			DisplacementVector displacement3 = results.GetDisplacement(node3);
			DisplacementVector displacement4 = results.GetDisplacement(node4);
			Console.WriteLine("\nDisplacement3 : \n" + displacement3);
			Console.WriteLine("\nDisplacement4 : \n" + displacement4);
			
			Assert.AreEqual( 0.000002619047, displacement3.X, 0.0000000001);
			Assert.AreEqual(-0.000001428571, displacement3.Y, 0.0000000001);
			
			Assert.AreEqual( 0.000004047618, displacement4.X, 0.0000000001);
			Assert.AreEqual( 0.000001428571, displacement4.Y, 0.0000000001);
		}
		
		        /// <summary>
        /// 4-------3
        /// |       |
        /// |       |
        /// |       |
        /// 1-------2
        /// </summary>
        [Test]
		public void OneVerticalQuadFixed3PointsMembrane()
		{
			FiniteElementModel model = new FiniteElementModel(ModelType.Membrane3D);
			FiniteElementNode node1 = model.NodeFactory.Create(0.0, 0.0, 0.0);
			model.ConstrainNode(node1, DegreeOfFreedom.X);
			model.ConstrainNode(node1, DegreeOfFreedom.Y);
			model.ConstrainNode(node1, DegreeOfFreedom.Z);

			FiniteElementNode node2 = model.NodeFactory.Create(1.0, 0.0, 0.0);
			model.ConstrainNode(node2, DegreeOfFreedom.X);
			model.ConstrainNode(node2, DegreeOfFreedom.Y);
			model.ConstrainNode(node2, DegreeOfFreedom.Z);
			
			FiniteElementNode node3 = model.NodeFactory.Create(1.0, 0.0, 1.0);
			model.ConstrainNode(node3, DegreeOfFreedom.X);
			model.ConstrainNode(node3, DegreeOfFreedom.Y);
			model.ConstrainNode(node3, DegreeOfFreedom.Z);
			
			FiniteElementNode node4 = model.NodeFactory.Create(0.0, 0.0, 1.0);
			model.ConstrainNode(node4, DegreeOfFreedom.Y);
			
			IMaterial material = new GenericElasticMaterial(0, 210000000000, 0.3, 0);
			
			model.ElementFactory.CreateLinearConstantStressQuadrilateral(node1, node2, node3, node4, material, 0.1);
			
			ForceVector force = model.ForceFactory.Create(10000, 0, 0, 0, 0, 0);
			model.ApplyForceToNode(force, node4);
			
			IFiniteElementSolver solver = new MatrixInversionLinearSolver(model);
			FiniteElementResults results = solver.Solve();
			
			ReactionVector reaction1 = results.GetReaction(node1);
			Console.WriteLine("\nReaction1 : \n" + reaction1);
			ReactionVector reaction2 = results.GetReaction(node2);
			Console.WriteLine("\nReaction2 : \n" + reaction2);
			ReactionVector reaction3 = results.GetReaction(node3);
			Console.WriteLine("\nReaction3 : \n" + reaction3);
			DisplacementVector displacement4 = results.GetDisplacement(node4);
			Console.WriteLine("\nDisplacement4 : \n" + displacement4);
			
			Assert.AreEqual( 2621, reaction1.X, 1);
			Assert.AreEqual(-3039, reaction1.Z, 1);
			Assert.AreEqual(-5660, reaction2.X, 1);
			Assert.AreEqual( 1706, reaction2.Z, 1);
			Assert.AreEqual(-6961, reaction3.X, 1);
			Assert.AreEqual( 1333, reaction3.Z, 1);
			
			Assert.AreEqual( 0.0000012400, displacement4.X, 0.0000000001);
			Assert.AreEqual( 0.0000004875, displacement4.Z, 0.0000000001);
		}
		
		/// <summary>
		///     4
		///    / \
		///   /   \
		///  /     \
        /// 1       3
        ///  \     / \  
        ///   \   /   \
        ///    \ /     \
        ///     2       5
        /// </summary>
        [Test]
		public void QuadMembraneAFrame()
		{
			FiniteElementModel model = new FiniteElementModel(ModelType.Membrane3D);
			FiniteElementNode node1 = model.NodeFactory.Create(0.0, 0.0, 0.0);
			model.ConstrainNode(node1, DegreeOfFreedom.X);
			model.ConstrainNode(node1, DegreeOfFreedom.Y);
			model.ConstrainNode(node1, DegreeOfFreedom.Z);

			FiniteElementNode node2 = model.NodeFactory.Create(1.0, 0.0, 0.0);
			model.ConstrainNode(node2, DegreeOfFreedom.Y);
			model.ConstrainNode(node2, DegreeOfFreedom.Z);
			
			FiniteElementNode node3 = model.NodeFactory.Create(1.0, 1.0, 1.0);
			FiniteElementNode node4 = model.NodeFactory.Create(0.0, 1.0, 1.0);
			
			FiniteElementNode node5 = model.NodeFactory.Create(1.0, 2.0, 0.0);
			model.ConstrainNode(node5, DegreeOfFreedom.Y);
			model.ConstrainNode(node5, DegreeOfFreedom.Z);

			FiniteElementNode node6 = model.NodeFactory.Create(0.0, 2.0, 0.0);
			model.ConstrainNode(node6, DegreeOfFreedom.X);
			model.ConstrainNode(node6, DegreeOfFreedom.Y);
			model.ConstrainNode(node6, DegreeOfFreedom.Z);
			
			IMaterial material = new GenericElasticMaterial(0, 210000000000, 0.3, 0);
			
			model.ElementFactory.CreateLinearConstantStressQuadrilateral(node1, node2, node3, node4, material, 0.1);
			model.ElementFactory.CreateLinearConstantStressQuadrilateral(node5, node6, node4, node3, material, 0.1);
			
			ForceVector force = model.ForceFactory.Create(0, 0, -10000, 0, 0, 0);
			model.ApplyForceToNode(force, node3);
			model.ApplyForceToNode(force, node4);
			
			IFiniteElementSolver solver = new MatrixInversionLinearSolver(model);
			FiniteElementResults results = solver.Solve();
			
			ReactionVector reaction1 = results.GetReaction(node1);
			Console.WriteLine("\nReaction1 : \n" + reaction1);
			ReactionVector reaction2 = results.GetReaction(node2);
			Console.WriteLine("\nReaction2 : \n" + reaction2);
			ReactionVector reaction5 = results.GetReaction(node5);
			Console.WriteLine("\nReaction2 : \n" + reaction2);
			ReactionVector reaction6 = results.GetReaction(node6);
			Console.WriteLine("\nReaction2 : \n" + reaction2);
			Assert.AreEqual(20000, reaction1.Z + reaction2.Z + reaction5.Z + reaction6.Z, 1);
			
			DisplacementVector displacement3 = results.GetDisplacement(node3);
			DisplacementVector displacement4 = results.GetDisplacement(node4);
			Console.WriteLine("\nDisplacement3 : \n" + displacement3);
			Console.WriteLine("\nDisplacement4 : \n" + displacement4);
			Assert.AreEqual( 0.0000002020, displacement3.X, 0.0000000001);
			Assert.AreEqual( 0, displacement3.Y, 0.0000000001);
			Assert.AreEqual(-0.0000013469, displacement3.Z, 0.0000000001);
			
			Assert.AreEqual(0, displacement4.X, 0.0000000001);
			Assert.AreEqual(0, displacement4.Y, 0.0000000001);
			Assert.AreEqual(-0.0000013469, displacement4.Z, 0.0000000001);
		}
		
				/// <summary>
		///     4
		///    / \
		///   /   \
		///  /     \
        /// 1       3
        ///  \     / \  
        ///   \   /   \
        ///    \ /     \
        ///     2       5
        /// </summary>
        [Test]
		public void QuadMembraneAFrameLateralLoad()
		{
			FiniteElementModel model = new FiniteElementModel(ModelType.Membrane3D);
			FiniteElementNode node1 = model.NodeFactory.Create(0.0, 0.0, 0.0);
			model.ConstrainNode(node1, DegreeOfFreedom.X);
			model.ConstrainNode(node1, DegreeOfFreedom.Y);
			model.ConstrainNode(node1, DegreeOfFreedom.Z);

			FiniteElementNode node2 = model.NodeFactory.Create(1.0, 0.0, 0.0);
			model.ConstrainNode(node2, DegreeOfFreedom.Y);
			model.ConstrainNode(node2, DegreeOfFreedom.Z);
			
			FiniteElementNode node3 = model.NodeFactory.Create(1.0, 1.0, 1.0);
			FiniteElementNode node4 = model.NodeFactory.Create(0.0, 1.0, 1.0);
			
			FiniteElementNode node5 = model.NodeFactory.Create(1.0, 2.0, 0.0);
			model.ConstrainNode(node5, DegreeOfFreedom.Y);
			model.ConstrainNode(node5, DegreeOfFreedom.Z);

			FiniteElementNode node6 = model.NodeFactory.Create(0.0, 2.0, 0.0);
			model.ConstrainNode(node6, DegreeOfFreedom.X);
			model.ConstrainNode(node6, DegreeOfFreedom.Y);
			model.ConstrainNode(node6, DegreeOfFreedom.Z);
			
			IMaterial material = new GenericElasticMaterial(0, 210000000000, 0.3, 0);
			
			model.ElementFactory.CreateLinearConstantStressQuadrilateral(node1, node2, node3, node4, material, 0.01);
			model.ElementFactory.CreateLinearConstantStressQuadrilateral(node5, node6, node4, node3, material, 0.01);
			
			ForceVector force = model.ForceFactory.Create(10000, 0, 0, 0, 0, 0);
			model.ApplyForceToNode(force, node3);
			model.ApplyForceToNode(force, node4);
			
			IFiniteElementSolver solver = new MatrixInversionLinearSolver(model);
			FiniteElementResults results = solver.Solve();
			
			ReactionVector reaction1 = results.GetReaction(node1);
			Console.WriteLine("\nReaction1 : \n" + reaction1);
			ReactionVector reaction2 = results.GetReaction(node2);
			Console.WriteLine("\nReaction2 : \n" + reaction2);
			ReactionVector reaction5 = results.GetReaction(node5);
			Console.WriteLine("\nReaction2 : \n" + reaction2);
			ReactionVector reaction6 = results.GetReaction(node6);
			Console.WriteLine("\nReaction2 : \n" + reaction2);
			
			DisplacementVector displacement3 = results.GetDisplacement(node3);
			DisplacementVector displacement4 = results.GetDisplacement(node4);
			Console.WriteLine("\nDisplacement3 : \n" + displacement3);
			Console.WriteLine("\nDisplacement4 : \n" + displacement4);
			
			Assert.AreEqual( 0.00003367, displacement3.X, 0.0000001);
			Assert.AreEqual( 0, displacement3.Y, 0.0000000001);
			Assert.AreEqual( 0.00002020147, displacement3.Z, 0.0000000001);
			
			Assert.AreEqual(0.00002861940, displacement4.X, 0.0000001);
			Assert.AreEqual(0, displacement4.Y, 0.0000000001);
			Assert.AreEqual(-0.00002020147, displacement4.Z, 0.0000000001);
		}
    }
}
