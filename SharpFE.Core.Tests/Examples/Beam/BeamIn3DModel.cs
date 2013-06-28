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
	public class BeamInFull3DModel
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
			
			IFiniteElementSolver solver = new LinearSolver(model);
			
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

		[Test]
		public void Frame()
		{
			FiniteElementModel model = new FiniteElementModel(ModelType.Full3D); // we will create and analyze a 3D frame system
			FiniteElementNode node1 = model.NodeFactory.Create(0,0,0); // create a node at the origin
			model.ConstrainNode(node1, DegreeOfFreedom.X);
			model.ConstrainNode(node1, DegreeOfFreedom.Y);
			model.ConstrainNode(node1, DegreeOfFreedom.Z);
			model.ConstrainNode(node1, DegreeOfFreedom.XX);
			model.ConstrainNode(node1, DegreeOfFreedom.ZZ);
			
			FiniteElementNode node2 = model.NodeFactory.Create(0,0,1);
			
			FiniteElementNode node3 = model.NodeFactory.Create(1,0,1);
			FiniteElementNode node4 = model.NodeFactory.Create(1,0,0);
			model.ConstrainNode(node4, DegreeOfFreedom.X);
			model.ConstrainNode(node4, DegreeOfFreedom.Y);
			model.ConstrainNode(node4, DegreeOfFreedom.Z);
			model.ConstrainNode(node4, DegreeOfFreedom.XX);
			model.ConstrainNode(node4, DegreeOfFreedom.ZZ);

			IMaterial material = new GenericElasticMaterial(0, 1, 0, 1);
			ICrossSection section = new SolidRectangle(1, 1);
			
			model.ElementFactory.CreateLinear3DBeam(node1, node2, material, section);
			model.ElementFactory.CreateLinear3DBeam(node2,node3,material,section);
			model.ElementFactory.CreateLinear3DBeam(node3,node4,material,section);
			
			ForceVector force = model.ForceFactory.Create(10, 0, 0,0,0,0);
			model.ApplyForceToNode(force, node2);
			
			IFiniteElementSolver solver = new LinearSolver(model);
			FiniteElementResults results = solver.Solve();
			
			DisplacementVector displacement = results.GetDisplacement(node2);
			Assert.AreNotEqual(0.0, displacement.X);
			Assert.AreEqual(0.0, displacement.Y, 0.001);
		}
		
		/// <summary>
		/// 10---->    (3)
		///            / \
		///           /   \
		///          /     \
		///         /       \
		///       (2)       (4)
		///        |         |
		///        |         |
		///        |         |
		///        |         |
		///       (1)       (5)
		/// </summary>
		[Test]
		public void PitchedRoofPortalFrame()
		{
			FiniteElementModel model = new FiniteElementModel(ModelType.Full3D); // we will create and analyze a 3D frame system
			FiniteElementNode node1 = model.NodeFactory.Create(0,0,0);
			model.ConstrainNode(node1, DegreeOfFreedom.X);
			model.ConstrainNode(node1, DegreeOfFreedom.Y);
			model.ConstrainNode(node1, DegreeOfFreedom.Z);
			model.ConstrainNode(node1, DegreeOfFreedom.XX);
			model.ConstrainNode(node1, DegreeOfFreedom.YY);
			model.ConstrainNode(node1, DegreeOfFreedom.ZZ);
			
			FiniteElementNode node2 = model.NodeFactory.Create(0,0,10);
			FiniteElementNode node3 = model.NodeFactory.Create(10,0,14);
			FiniteElementNode node4 = model.NodeFactory.Create(20,0,10);
			
			FiniteElementNode node5 = model.NodeFactory.Create(20,0,0);
			model.ConstrainNode(node5, DegreeOfFreedom.X);
			model.ConstrainNode(node5, DegreeOfFreedom.Y);
			model.ConstrainNode(node5, DegreeOfFreedom.Z);
			model.ConstrainNode(node5, DegreeOfFreedom.XX);
			model.ConstrainNode(node5, DegreeOfFreedom.YY);
			model.ConstrainNode(node5, DegreeOfFreedom.ZZ);
			
			IMaterial material = new GenericElasticMaterial(0, 1, 0, 1);
			ICrossSection section = new SolidRectangle(1, 1);
			
			model.ElementFactory.CreateLinear3DBeam(node1, node2, material, section);
			model.ElementFactory.CreateLinear3DBeam(node2, node3, material, section);
			model.ElementFactory.CreateLinear3DBeam(node3, node4, material, section);
			model.ElementFactory.CreateLinear3DBeam(node4, node5, material, section);
			
			ForceVector force = model.ForceFactory.Create(10, 0, 0,0,0,0);
			model.ApplyForceToNode(force, node3);
			
			IFiniteElementSolver solver = new LinearSolver(model);
			FiniteElementResults results = solver.Solve();
			
			DisplacementVector displacement = results.GetDisplacement(node4);
			Assert.AreNotEqual(0.0, displacement.X);
			Assert.AreEqual(0.0, displacement.Y, 0.001);
		}
	}
}

