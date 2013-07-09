using System;
using NUnit.Framework;

namespace SharpFE.Examples.Membrane
{
	[TestFixture]
	public class Triangle
	{
		[Test]
		public void TwoTriangleWall()
		{
			FiniteElementModel model = new FiniteElementModel(ModelType.Membrane2D); // we will create and analyze a 2D slab system
			FiniteElementNode node1 = model.NodeFactory.Create(0.0, 0.0); // create a node at the origin
			model.ConstrainNode(node1, DegreeOfFreedom.X); // constrain the node from moving in the x-axis
			model.ConstrainNode(node1, DegreeOfFreedom.Y); // constrain the node from moving in the y-axis

			FiniteElementNode node2 = model.NodeFactory.Create(1.0, 0.0); // create a second node at a distance 1 metre along the X axis
			model.ConstrainNode(node2, DegreeOfFreedom.X); // constrain the node from moving in the x-axis
			model.ConstrainNode(node2, DegreeOfFreedom.Y); // constrain the node from moving in the y-axis
			
			FiniteElementNode node3 = model.NodeFactory.Create(0.0, 1.0);
			
			FiniteElementNode node4 = model.NodeFactory.Create(1.0, 1.0);
			
			IMaterial material = new GenericElasticMaterial(0, 200000, 0.2, 0.1);
			
			model.ElementFactory.CreateLinearConstantStrainTriangle(node1, node2, node3, material, 0.1); // create a triangle of thickness of 0.1 metres
			model.ElementFactory.CreateLinearConstantStrainTriangle(node2, node4, node3, material, 0.1);
			
			ForceVector force = model.ForceFactory.Create(0, -10); // Create a force of 10 Newtons in the y direction
			model.ApplyForceToNode(force, node3); // Apply that force to the third node
			model.ApplyForceToNode(force, node4);
			
			IFiniteElementSolver solver = new MatrixInversionLinearSolver(model);
			FiniteElementResults results = solver.Solve();
			
			ReactionVector reaction1 = results.GetReaction(node1); //get the reaction at the first node
			Console.WriteLine("\nReaction1 : \n" + reaction1);
			ReactionVector reaction2 = results.GetReaction(node2);
			Console.WriteLine("\nReaction2 : \n" + reaction2);
			Assert.AreEqual(20, reaction1.Y + reaction2.Y, 0.001);
			
			DisplacementVector displacement3 = results.GetDisplacement(node3);  // get the displacement at the second node
			Console.WriteLine("\nDisplacement3 : \n" + displacement3);
			Assert.AreNotEqual(0.0, displacement3.X); // TODO calculate the actual value, rather than just checking we have any value
			Assert.AreNotEqual(0.0, displacement3.Y); // TODO calculate the actual value, rather than just checking we have any value
			
			DisplacementVector displacement4 = results.GetDisplacement(node4);  // get the displacement at the second node
			Console.WriteLine("\nDisplacement4 : \n" + displacement4);
			Assert.AreNotEqual(0.0, displacement4.X); // TODO calculate the actual value, rather than just checking we have any value
			Assert.AreNotEqual(0.0, displacement4.Y); // TODO calculate the actual value, rather than just checking we have any value
		}
		
		[Test]
		public void FourTriangleAFrame()
		{
			FiniteElementModel model = new FiniteElementModel(ModelType.Truss3D);
			FiniteElementNode node1 = model.NodeFactory.Create(0.0, 0.0, 0.0);
			model.ConstrainNode(node1, DegreeOfFreedom.X);
			model.ConstrainNode(node1, DegreeOfFreedom.Y);
			model.ConstrainNode(node1, DegreeOfFreedom.Z);

			FiniteElementNode node2 = model.NodeFactory.Create(1.0, 0.0, 0.0);
			model.ConstrainNode(node2, DegreeOfFreedom.X);
			model.ConstrainNode(node2, DegreeOfFreedom.Y);
			model.ConstrainNode(node2, DegreeOfFreedom.Z);
			
			FiniteElementNode node3 = model.NodeFactory.Create(0.0, 1.0, 0.5);
			
			FiniteElementNode node4 = model.NodeFactory.Create(1.0, 1.0, 0.5);
			
			FiniteElementNode node5 = model.NodeFactory.Create(0.0, 2.0, 0.0);
			model.ConstrainNode(node5, DegreeOfFreedom.X);
			model.ConstrainNode(node5, DegreeOfFreedom.Y);
			model.ConstrainNode(node5, DegreeOfFreedom.Z);

			FiniteElementNode node6 = model.NodeFactory.Create(1.0, 2.0, 0.0);
			model.ConstrainNode(node6, DegreeOfFreedom.X);
			model.ConstrainNode(node6, DegreeOfFreedom.Y);
			model.ConstrainNode(node6, DegreeOfFreedom.Z);
			
			IMaterial material = new GenericElasticMaterial(0, 200000, 0.2, 84000);
			
			model.ElementFactory.CreateLinearConstantStrainTriangle(node1, node2, node3, material, 0.1);
			model.ElementFactory.CreateLinearConstantStrainTriangle(node2, node4, node3, material, 0.1);
			model.ElementFactory.CreateLinearConstantStrainTriangle(node3, node4, node5, material, 0.1);
			model.ElementFactory.CreateLinearConstantStrainTriangle(node4, node6, node5, material, 0.1);
			
			ForceVector force = model.ForceFactory.Create(0, 0, -10, 0, 0, 0);
			model.ApplyForceToNode(force, node3);
			model.ApplyForceToNode(force, node4);
			
			IFiniteElementSolver solver = new LinearSolverSVD(model);
			FiniteElementResults results = solver.Solve();
			
			ReactionVector reaction1 = results.GetReaction(node1); //get the reaction at the first node
			Console.WriteLine("\nReaction1 : \n" + reaction1);
			ReactionVector reaction2 = results.GetReaction(node2);
			Console.WriteLine("\nReaction2 : \n" + reaction2);
			Assert.AreEqual(20, reaction1.Y + reaction2.Y, 0.001);
			
			DisplacementVector displacement3 = results.GetDisplacement(node3);  // get the displacement at the second node
			Console.WriteLine("\nDisplacement3 : \n" + displacement3);
			Assert.AreNotEqual(0.0, displacement3.X); // TODO calculate the actual value, rather than just checking we have any value
			Assert.AreNotEqual(0.0, displacement3.Y); // TODO calculate the actual value, rather than just checking we have any value
			Assert.AreNotEqual(0.0, displacement3.Z); // TODO calculate the actual value, rather than just checking we have any value
			
			DisplacementVector displacement4 = results.GetDisplacement(node4);  // get the displacement at the second node
			Console.WriteLine("\nDisplacement4 : \n" + displacement4);
			Assert.AreNotEqual(0.0, displacement4.X); // TODO calculate the actual value, rather than just checking we have any value
			Assert.AreNotEqual(0.0, displacement4.Y); // TODO calculate the actual value, rather than just checking we have any value
			Assert.AreNotEqual(0.0, displacement4.Z); // TODO calculate the actual value, rather than just checking we have any value
		}
	}
}
