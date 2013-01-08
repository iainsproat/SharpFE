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

			IMaterial material = new GenericElasticMaterial(0, 2000, 0.2, 1000);
			ICrossSection section = new SolidRectangle(0.5, 0.1);
			
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
	}
	
}

