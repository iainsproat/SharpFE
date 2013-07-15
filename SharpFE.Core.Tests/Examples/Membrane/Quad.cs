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
			
			IMaterial material = new GenericElasticMaterial(0, 210000000000, 0.3, 84000000000);
			
			model.ElementFactory.CreateLinearConstantStressQuadrilateral(node1, node2, node3, node4, material, 0.1);
			
			ForceVector force = model.ForceFactory.Create(0, -10000);
			model.ApplyForceToNode(force, node3);
			model.ApplyForceToNode(force, node4);
			
			IFiniteElementSolver solver = new MatrixInversionLinearSolver(model);
			FiniteElementResults results = solver.Solve();
			
			ReactionVector reaction1 = results.GetReaction(node1);
			Console.WriteLine("\nReaction1 : \n" + reaction1);
			ReactionVector reaction2 = results.GetReaction(node2);
			Console.WriteLine("\nReaction2 : \n" + reaction2);
			Assert.AreEqual(20000, reaction1.Y + reaction2.Y, 1);
			
			DisplacementVector displacement3 = results.GetDisplacement(node3);
			DisplacementVector displacement4 = results.GetDisplacement(node4);
			Console.WriteLine("\nDisplacement3 : \n" + displacement3);
			Console.WriteLine("\nDisplacement4 : \n" + displacement4);
			Assert.AreEqual( 0.0000002857, displacement3.X, 0.0000000001);
			Assert.AreEqual(-0.0000009524, displacement3.Y, 0.0000000001);
			
			Assert.AreEqual(0, displacement4.X, 0.0000001);
			Assert.AreEqual(-0.0000009524, displacement4.Y, 0.0000000001);
		}
    }
}
