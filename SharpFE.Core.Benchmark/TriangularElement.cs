using System;
using System.Diagnostics;

using NUnit.Framework;
using SharpFE;

namespace SharpFE.Core.Benchmark
{
	[TestFixture()]
	public class TriangularElement
	{
		[Test()]
		public void TestCase()
		{
			int iterations = 10000;
            var sw = Stopwatch.StartNew();
			for(int i = 0; i < iterations; i++) {
				FourTriangleAFrame();
			}

			Console.WriteLine("Four Triangle A-Frame problem was run {0} times in {1}", iterations, sw.Elapsed);
		}

	
		public void FourTriangleAFrame()
		{
			var model = new FiniteElementModel(ModelType.Membrane3D);
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

