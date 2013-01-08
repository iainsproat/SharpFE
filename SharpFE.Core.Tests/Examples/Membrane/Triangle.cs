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
			FiniteElementModel model = new FiniteElementModel(ModelType.Full3D); // we will create and analyze a 2D slab system
			FiniteElementNode node1 = model.NodeFactory.Create(0.0, 0.0, 0.0); // create a node at the origin
			model.ConstrainNode(node1, DegreeOfFreedom.X); // constrain the node from moving in the x-axis
			model.ConstrainNode(node1, DegreeOfFreedom.Y); // constrain the node from moving in the y-axis
			model.ConstrainNode(node1, DegreeOfFreedom.Z);
			model.ConstrainNode(node1, DegreeOfFreedom.XX);
			model.ConstrainNode(node1, DegreeOfFreedom.YY);
			model.ConstrainNode(node1, DegreeOfFreedom.ZZ);

			FiniteElementNode node2 = model.NodeFactory.Create(1.0, 0.0, 0.0); // create a second node at a distance 1 metre along the X axis
			model.ConstrainNode(node2, DegreeOfFreedom.X); // constrain the node from moving in the x-axis
			model.ConstrainNode(node2, DegreeOfFreedom.Y); // constrain the node from moving in the y-axis
			model.ConstrainNode(node2, DegreeOfFreedom.Z);
			model.ConstrainNode(node2, DegreeOfFreedom.XX);
			model.ConstrainNode(node2, DegreeOfFreedom.YY);
			model.ConstrainNode(node2, DegreeOfFreedom.ZZ);
			
			FiniteElementNode node3 = model.NodeFactory.Create(0.0, 1.0, 0.0);
			model.ConstrainNode(node3, DegreeOfFreedom.Z);
			model.ConstrainNode(node3, DegreeOfFreedom.XX);
			model.ConstrainNode(node3, DegreeOfFreedom.YY);
			model.ConstrainNode(node3, DegreeOfFreedom.ZZ);
			
			FiniteElementNode node4 = model.NodeFactory.Create(1.0, 1.0, 0.0);
			model.ConstrainNode(node4, DegreeOfFreedom.Z);
			model.ConstrainNode(node4, DegreeOfFreedom.XX);
			model.ConstrainNode(node4, DegreeOfFreedom.YY);
			model.ConstrainNode(node4, DegreeOfFreedom.ZZ);
			
			IMaterial material = new GenericElasticMaterial(0, 200000, 0.2, 0.1);
			
			model.ElementFactory.CreateLinearConstantStrainTriangle(node1, node2, node3, material, 0.1); // create a triangle of thickness of 0.1 metres
			model.ElementFactory.CreateLinearConstantStrainTriangle(node2, node4, node3, material, 0.1);
			
			ForceVector force = model.ForceFactory.Create(0, -10); // Create a force of 10 Newtons in the y direction
			model.ApplyForceToNode(force, node3); // Apply that force to the third node
			model.ApplyForceToNode(force, node4);
			
			IFiniteElementSolver solver = new LinearSolver(model);
			FiniteElementResults results = solver.Solve();
			
			ReactionVector reaction = results.GetReaction(node1); //get the reaction at the first node
			ReactionVector reaction2 = results.GetReaction(node2);
			Assert.AreEqual(20, reaction.Y + reaction2.Y, 0.001);
			
			DisplacementVector displacement = results.GetDisplacement(node3);  // get the displacement at the second node
			Assert.AreNotEqual(0.0, displacement.X); // TODO calculate the actual value, rather than just checking we have any value
			Assert.AreNotEqual(0.0, displacement.Y); // TODO calculate the actual value, rather than just checking we have any value
		}
	}
}
