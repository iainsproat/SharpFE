/*
 * Copyright Iain Sproat, 2013
 * User: ispro
 * Date: 28/06/2013
 * 
 */
using System;
using NUnit.Framework;

namespace SharpFE.Examples.Truss
{
	[TestFixture]
	public class TrussIn3DModel
	{
		///<summary>
        /// Example problem and results are derived from:
		/// MATLAB Codes for Finite Element Analysis, Solid Mechanics and its applications Volume 157, A.J.M. Ferreira, Springer 2010
		/// Section 5.2, page 69
		/// </summary>
		[Test]
		public void Calculate3DTrussOf3BarsAnd12Dof()
		{
			FiniteElementModel model = new FiniteElementModel(ModelType.Truss3D);
			
			FiniteElementNode node1 = model.NodeFactory.Create(72,0,0);
			model.ConstrainNode(node1, DegreeOfFreedom.Y);
			
			FiniteElementNode node2 = model.NodeFactory.Create(0, 36, 0);
			model.ConstrainNode(node2, DegreeOfFreedom.X);
			model.ConstrainNode(node2, DegreeOfFreedom.Y);
			model.ConstrainNode(node2, DegreeOfFreedom.Z);
			
			FiniteElementNode node3 = model.NodeFactory.Create(0, 36, 72);
			model.ConstrainNode(node3, DegreeOfFreedom.X);
			model.ConstrainNode(node3, DegreeOfFreedom.Y);
			model.ConstrainNode(node3, DegreeOfFreedom.Z);
			
			FiniteElementNode node4 = model.NodeFactory.Create(0, 0, -48);
			model.ConstrainNode(node4, DegreeOfFreedom.X);
			model.ConstrainNode(node4, DegreeOfFreedom.Y);
			model.ConstrainNode(node4, DegreeOfFreedom.Z);
			
			IMaterial material = new GenericElasticMaterial(0, 1200000, 0, 0);
			ICrossSection section1 = new SolidRectangle(1, 0.302);
			ICrossSection section2 = new SolidRectangle(1, 0.729); ///NOTE example also refers to this as A1.  Assume errata in book
			ICrossSection section3 = new SolidRectangle(1, 0.187); ///NOTE example also refers to this as A1.  Assume errata in book
			
			model.ElementFactory.CreateLinearTruss(node1, node2, material, section1);
			model.ElementFactory.CreateLinearTruss(node1, node3, material, section2);
			model.ElementFactory.CreateLinearTruss(node1, node4, material, section3);
			
			ForceVector externalForce = model.ForceFactory.Create(0, 0, -1000, 0, 0, 0);
			model.ApplyForceToNode(externalForce, node1);
			
			IFiniteElementSolver solver = new MatrixInversionLinearSolver(model);
			FiniteElementResults results = solver.Solve();
			
			ReactionVector reactionAtNode1 = results.GetReaction(node1);
			Assert.AreEqual(0, reactionAtNode1.X, 1);
			Assert.AreEqual(-223.1632, reactionAtNode1.Y, 1);
			Assert.AreEqual(0, reactionAtNode1.Z, 1);
			
			ReactionVector reactionAtNode2 = results.GetReaction(node2);
			Assert.AreEqual(256.1226, reactionAtNode2.X, 1);
			Assert.AreEqual(-128.0613, reactionAtNode2.Y, 1);
			Assert.AreEqual(0, reactionAtNode2.Z, 1);
			
			ReactionVector reactionAtNode3 = results.GetReaction(node3);
			Assert.AreEqual(-702.4491, reactionAtNode3.X, 1);
			Assert.AreEqual(351.2245, reactionAtNode3.Y, 1);
			Assert.AreEqual(702.4491, reactionAtNode3.Z, 1);
			
			ReactionVector reactionAtNode4 = results.GetReaction(node4);
			Assert.AreEqual(446.3264, reactionAtNode4.X, 1);
			Assert.AreEqual(0, reactionAtNode4.Y, 1);
			Assert.AreEqual(297.5509, reactionAtNode4.Z, 1);
			
			DisplacementVector displacementAtNode1 = results.GetDisplacement(node1);
			Assert.AreEqual(-0.0711, displacementAtNode1.X, 0.0001);
			Assert.AreEqual(0, displacementAtNode1.Y, 0.0001);
			Assert.AreEqual(-0.2662, displacementAtNode1.Z, 0.0001);
		}
		
		///<summary>
        /// Example problem and results are derived from:
		/// MATLAB Codes for Finite Element Analysis, Solid Mechanics and its applications Volume 157, A.J.M. Ferreira, Springer 2010
		/// Section 5.3, page 73
		/// </summary>
		[Test]
		public void Calculate3DTrussOf4BarsAnd15Dof()
		{
			FiniteElementModel model = new FiniteElementModel(ModelType.Truss3D);
			
			FiniteElementNode node1 = model.NodeFactory.Create(4, 4, 3);
			
			FiniteElementNode node2 = model.NodeFactory.Create(0, 4, 0);
			model.ConstrainNode(node2, DegreeOfFreedom.X);
			model.ConstrainNode(node2, DegreeOfFreedom.Y);
			model.ConstrainNode(node2, DegreeOfFreedom.Z);
			
			FiniteElementNode node3 = model.NodeFactory.Create(0, 4, 6);
			model.ConstrainNode(node3, DegreeOfFreedom.X);
			model.ConstrainNode(node3, DegreeOfFreedom.Y);
			model.ConstrainNode(node3, DegreeOfFreedom.Z);
			
			FiniteElementNode node4 = model.NodeFactory.Create(4, 0, 3);
			model.ConstrainNode(node4, DegreeOfFreedom.X);
			model.ConstrainNode(node4, DegreeOfFreedom.Y);
			model.ConstrainNode(node4, DegreeOfFreedom.Z);
			
			FiniteElementNode node5 = model.NodeFactory.Create(8, -1, 1);
			model.ConstrainNode(node5, DegreeOfFreedom.X);
			model.ConstrainNode(node5, DegreeOfFreedom.Y);
			model.ConstrainNode(node5, DegreeOfFreedom.Z);
			
			IMaterial material = new GenericElasticMaterial(0, 210000000000, 0, 0);
			ICrossSection section = new SolidRectangle(0.01, 0.01);
			
			model.ElementFactory.CreateLinearTruss(node1, node2, material, section);
			model.ElementFactory.CreateLinearTruss(node1, node3, material, section);
			model.ElementFactory.CreateLinearTruss(node1, node4, material, section);
			model.ElementFactory.CreateLinearTruss(node1, node5, material, section);
			
			ForceVector externalForce = model.ForceFactory.Create(0, -10000, 0, 0, 0, 0);
			model.ApplyForceToNode(externalForce, node1);
			
			IFiniteElementSolver solver = new MatrixInversionLinearSolver(model);
			FiniteElementResults results = solver.Solve();
			
			ReactionVector reactionAtNode2 = results.GetReaction(node2);
			Assert.AreEqual(270.9, reactionAtNode2.X, 1);
			Assert.AreEqual(0, reactionAtNode2.Y, 1);
			Assert.AreEqual(203.2, reactionAtNode2.Z, 1);
			
			ReactionVector reactionAtNode3 = results.GetReaction(node3);
			Assert.AreEqual(1354.6, reactionAtNode3.X, 1);
			Assert.AreEqual(0, reactionAtNode3.Y, 1);
			Assert.AreEqual(-1016, reactionAtNode3.Z, 1);
			
			ReactionVector reactionAtNode4 = results.GetReaction(node4);
			Assert.AreEqual(0, reactionAtNode4.X, 1);
			Assert.AreEqual(7968.1, reactionAtNode4.Y, 1);
			Assert.AreEqual(0, reactionAtNode4.Z, 1);
			
			ReactionVector reactionAtNode5 = results.GetReaction(node5);
			Assert.AreEqual(-1625.5, reactionAtNode5.X, 1);
			Assert.AreEqual(2031.9, reactionAtNode5.Y, 1);
			Assert.AreEqual(812.8, reactionAtNode5.Z, 1);
			
			DisplacementVector displacementAtNode1 = results.GetDisplacement(node1);
			Assert.AreEqual(-0.0003024, displacementAtNode1.X, 0.0001); //NOTE the results given in the book are 1E03 
			Assert.AreEqual(-0.0015177, displacementAtNode1.Y, 0.0001);
			Assert.AreEqual(0.0002688, displacementAtNode1.Z, 0.0001);
		}
		
		[Test]
		public void Example3_8_FirstCourseInTheFiniteElementMethod_Logan_4thEd()
		{
		    var model = new FiniteElementModel(ModelType.Truss3D);
		    
		    var node1 = model.NodeFactory.Create(72,  0,  0);
		    model.ConstrainNode(node1, DegreeOfFreedom.Y);
		    
		    var node2 = model.NodeFactory.Create( 0, 36,  0);
		    model.ConstrainNode(node2, DegreeOfFreedom.X);
		    model.ConstrainNode(node2, DegreeOfFreedom.Y);
		    model.ConstrainNode(node2, DegreeOfFreedom.Z);
		    
		    var node3 = model.NodeFactory.Create( 0, 36, 72);
		    model.ConstrainNode(node3, DegreeOfFreedom.X);
		    model.ConstrainNode(node3, DegreeOfFreedom.Y);
		    model.ConstrainNode(node3, DegreeOfFreedom.Z);
		    
		    var node4 = model.NodeFactory.Create( 0,  0,-48);
		    model.ConstrainNode(node4, DegreeOfFreedom.X);
		    model.ConstrainNode(node4, DegreeOfFreedom.Y);
		    model.ConstrainNode(node4, DegreeOfFreedom.Z);
		    
		    var material = new GenericElasticMaterial(0, 1200000, 0, 0); //E = 1.2E6 psi
			var section1 = new GenericCrossSection(0.302); //square inches
			var section2 = new GenericCrossSection(0.729); //square inches
			var section3 = new GenericCrossSection(0.187); //square inches
			
			model.ElementFactory.CreateLinearTruss(node1, node2, material, section1);
			model.ElementFactory.CreateLinearTruss(node1, node3, material, section2);
			model.ElementFactory.CreateLinearTruss(node1, node4, material, section3);
			
			var externalForce = model.ForceFactory.Create(0, 0, -1000, 0, 0, 0); //1000 lbs vertically downwards in z axis
			model.ApplyForceToNode(externalForce, node1);
			
			var solver = new MatrixInversionLinearSolver(model);
			var results = solver.Solve();
			
			var displacementNode1 = results.GetDisplacement(node1);
			Assert.AreEqual(-0.072, displacementNode1.X, 0.001); //FIXME is this tolerance too great? Would typically try to achieve 0.0005
			Assert.AreEqual(-0.264, displacementNode1.Z, 0.003); //FIXME is this tolerance too great? Would typically try to achieve 0.0005
		}
		
		[Test]
		public void Example3_9_FirstCourseInTheFiniteElementMethod_Logan_4thEd()
		{
		    var model = new FiniteElementModel(ModelType.Truss3D);
		    
		    var node1 = model.NodeFactory.Create(12, -3, -4);  //metres
		    
		    var node2 = model.NodeFactory.Create( 0,  0,  0);
		    model.ConstrainNode(node2, DegreeOfFreedom.X);
		    model.ConstrainNode(node2, DegreeOfFreedom.Y);
		    model.ConstrainNode(node2, DegreeOfFreedom.Z);
		    
		    var node3 = model.NodeFactory.Create(12, -3, -7);
		    model.ConstrainNode(node3, DegreeOfFreedom.X);
		    model.ConstrainNode(node3, DegreeOfFreedom.Y);
		    model.ConstrainNode(node3, DegreeOfFreedom.Z);
		    
		    var node4 = model.NodeFactory.Create(14,  6,  0);
		    model.ConstrainNode(node4, DegreeOfFreedom.X);
		    model.ConstrainNode(node4, DegreeOfFreedom.Y);
		    model.ConstrainNode(node4, DegreeOfFreedom.Z);
		    
		    var material = new GenericElasticMaterial(0, 210000000000, 0, 0); //E = 210 GPa
			var section = new GenericCrossSection(0.001); //A = 10E-4 square metres
			
			model.ElementFactory.CreateLinearTruss(node1, node2, material, section);
			model.ElementFactory.CreateLinearTruss(node1, node3, material, section);
			model.ElementFactory.CreateLinearTruss(node1, node4, material, section);
			
			var externalForce = model.ForceFactory.Create(20000, 0, 0, 0, 0, 0); //20kN in x-direction
			model.ApplyForceToNode(externalForce, node1);
			
			var solver = new MatrixInversionLinearSolver(model);
			var results = solver.Solve();
			
			var displacementNode1 = results.GetDisplacement(node1);
			Assert.AreEqual( 0.001383,   displacementNode1.X, 0.000001); //FIXME is this tolerance too great? Would typically try to achieve 0.5 of given least significant digit
			Assert.AreEqual(-0.00005119, displacementNode1.Y, 0.000001); //FIXME is this tolerance too great? Would typically try to achieve 0.5 of given least significant digit
			Assert.AreEqual( 0.00006015, displacementNode1.Z, 0.000000005);
		}
	}
}
