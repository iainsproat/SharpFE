//-----------------------------------------------------------------------
// <copyright file="Spring1DIn2DModel.cs" company="SharpFE">
//     Copyright Iain Sproat, 2012.
// </copyright>
//-----------------------------------------------------------------------
using System;
using SharpFE;
using NUnit.Framework;

namespace SharpFE.Examples.Truss
{
    /// <summary>
    /// Description of Spring1DIn2DModel.
    /// </summary>
    [TestFixture]
    public class TrussIn2DModel
    {
    	[Test]
        public void SpringInXAxis()
        {
            FiniteElementModel model = new FiniteElementModel(ModelType.Truss2D); // we will create and analyze a 1D spring in the vertical
            FiniteElementNode node1 = model.NodeFactory.CreateFor2DTruss(0, 0); // create a node at the origin
            model.ConstrainNode(node1, DegreeOfFreedom.X); // constrain this node from moving in the X axis
            model.ConstrainNode(node1, DegreeOfFreedom.Z); // also constrain it from moving in the Y axis

            FiniteElementNode node2 = model.NodeFactory.CreateFor2DTruss(1, 0); // create a second node at a distance 1 metre along the X axis.
            model.ConstrainNode(node2, DegreeOfFreedom.Z); // fix this node from moving along the Y-axis.  It is still free to move along the X-axis however.
            
            LinearConstantSpring spring = model.ElementFactory.CreateLinearConstantSpring(node1, node2, 1000); // create a spring between the two nodes of a stiffness of 1000 Newtons per metre
            
            ForceVector force = model.ForceFactory.CreateForTruss(10, 0); // Create a force of 10 Newtons along the x-axis.
            model.ApplyForceToNode(force, node2); // Apply that force to the second node
            
            IFiniteElementSolver solver = new MatrixInversionLinearSolver(model); // Create a new instance of the solver class and pass it the model to solve
            FiniteElementResults results = solver.Solve(); // ask the solver to solve the model and return results
            
            DisplacementVector displacementAtNode2 = results.GetDisplacement(node2);  // get the displacement at the second node
            Assert.AreEqual(0.01, displacementAtNode2.X); // check that we calculated 0.010 metres (10 millimetres) along the Y axis.
            
            ReactionVector reactionAtNode1 = results.GetReaction(node1); //get the reaction at the first node
            Assert.AreEqual(-10, reactionAtNode1.X); // Check that we have calculated a reaction of -10 Newtons in the X axis.
            Assert.AreEqual(0, reactionAtNode1.Z); // and a reaction of 0 Newtons in the Y axis.
        }
        
        [Test]
        public void SpringAt60DegreesInXYPlane()
        {
            FiniteElementModel model = new FiniteElementModel(ModelType.Truss2D); // we will create and analyze a 2D truss system
            FiniteElementNode node1 = model.NodeFactory.CreateFor2DTruss(0, 0); // create a node at the origin
            model.ConstrainNode(node1, DegreeOfFreedom.X); // constrain this node from moving in the X axis
            model.ConstrainNode(node1, DegreeOfFreedom.Z); // also constrain it from moving in the Y axis

            FiniteElementNode node2 = model.NodeFactory.CreateFor2DTruss(1, 1.73205); // create a second node at a distance 1 metre along the X axis and 1.73 metres along the Y axis (giving an angle of 60 degrees from x-axis).
            model.ConstrainNode(node2, DegreeOfFreedom.X);
            
            LinearConstantSpring spring = model.ElementFactory.CreateLinearConstantSpring(node1, node2, 1000); // create a spring between the first two nodes of a stiffness of 1000 Newtons per metre
            
            ForceVector force = model.ForceFactory.CreateForTruss(0, 10); // Create a force of with components of 10 Newtons along the y-axis.
            model.ApplyForceToNode(force, node2); // Apply that force to the second node
            
            IFiniteElementSolver solver = new MatrixInversionLinearSolver(model); // Create a new instance of the solver class and pass it the model to solve
            FiniteElementResults results = solver.Solve(); // ask the solver to solve the model and return results
            
            DisplacementVector displacementAtNode2 = results.GetDisplacement(node2);  // get the displacement at the second node
            Assert.AreEqual(0, displacementAtNode2.X); // Check that there is no displacement in the x-axis
            Assert.AreEqual(0.013333, displacementAtNode2.Z, 0.001); // and 0.01333 metres (13 millimetres) along the Y axis.
            
            ReactionVector reactionAtNode1 = results.GetReaction(node1); //get the reaction at the first node
            Assert.AreEqual(-5.774, reactionAtNode1.X, 0.001); // Check that we have calculated a reaction of 10/SQRT(3) Newtons in the X axis.
            Assert.AreEqual(-10, reactionAtNode1.Z,  0.001); // and a reaction of -10 Newtons in the Y axis.
        }
        
        /// <summary>
		/// Example problem and results are derived from:
		/// MATLAB Codes for Finite Element Analysis, Solid Mechanics and its applications Volume 157, A.J.M. Ferreira, Springer 2010
		/// Section 4.5, page 53
		/// </summary>
		[Test]
		public void Calculate2DTrussOf3BarsAnd8Dof()
		{
			FiniteElementModel model = new FiniteElementModel(ModelType.Truss2D);
			
			FiniteElementNode node1 = model.NodeFactory.CreateFor2DTruss(0, 0);
						
			FiniteElementNode node2 = model.NodeFactory.CreateFor2DTruss(0, 10);
			model.ConstrainNode(node2, DegreeOfFreedom.X);
			model.ConstrainNode(node2, DegreeOfFreedom.Z);
			
			FiniteElementNode node3 = model.NodeFactory.CreateFor2DTruss(10, 10);
			model.ConstrainNode(node3, DegreeOfFreedom.X);
			model.ConstrainNode(node3, DegreeOfFreedom.Z);
			
			FiniteElementNode node4 = model.NodeFactory.CreateFor2DTruss(10, 0);
			model.ConstrainNode(node4, DegreeOfFreedom.X);
			model.ConstrainNode(node4, DegreeOfFreedom.Z);
			
			IMaterial material = new GenericElasticMaterial(0, 30000000, 0, 0);
            ICrossSection section = new SolidRectangle(2, 1);
			
			model.ElementFactory.CreateLinearTruss(node1, node2, material, section);
			model.ElementFactory.CreateLinearTruss(node1, node3, material, section);
			model.ElementFactory.CreateLinearTruss(node1, node4, material, section);
						
			ForceVector externalForce = model.ForceFactory.CreateForTruss(0, -10000);
			model.ApplyForceToNode(externalForce, node1);
			
			IFiniteElementSolver solver = new MatrixInversionLinearSolver(model);
			FiniteElementResults results = solver.Solve();
			
			ReactionVector reactionAtNode2 = results.GetReaction(node2);
			Assert.AreEqual(0, reactionAtNode2.X, 1);
			Assert.AreEqual(7929, reactionAtNode2.Z, 1);
			
			ReactionVector reactionAtNode3 = results.GetReaction(node3);
			Assert.AreEqual(2071, reactionAtNode3.X, 1);
			Assert.AreEqual(2071, reactionAtNode3.Z, 1);
			
			ReactionVector reactionAtNode4 = results.GetReaction(node4);
			Assert.AreEqual(-2071, reactionAtNode4.X, 1);
			Assert.AreEqual(0, reactionAtNode4.Z, 1);
			
			DisplacementVector displacementAtNode1 = results.GetDisplacement(node1);
			Assert.AreEqual( 0.00035, displacementAtNode1.X, 0.00001);  ///NOTE this does not match the example in the book, but was instead verified by commercial FE software.  It appears as it may be an errata in the book.
			Assert.AreEqual(-0.00132, displacementAtNode1.Z, 0.00001);  ///NOTE this does not match the example in the book, but was instead verified by commercial FE software.  It appears as it may be an errata in the book.
		}
		
		/// <summary>
		/// Example problem and results are derived from:
		/// MATLAB Codes for Finite Element Analysis, Solid Mechanics and its applications Volume 157, A.J.M. Ferreira, Springer 2010
		/// Section 4.5, page 53
		/// </summary>
		[Test]
		public void Calculate2DTrussOf11BarsAnd12Dof()
		{
			
			FiniteElementModel model = new FiniteElementModel(ModelType.Truss2D);
			
			//build geometric model and constraints
			
			FiniteElementNode node1 = model.NodeFactory.CreateFor2DTruss(0, 0);
			model.ConstrainNode(node1, DegreeOfFreedom.X);
			model.ConstrainNode(node1, DegreeOfFreedom.Z);
			
			FiniteElementNode node2 = model.NodeFactory.CreateFor2DTruss(0, 3);
			
			FiniteElementNode node3 = model.NodeFactory.CreateFor2DTruss(3, 0);
			
			FiniteElementNode node4 = model.NodeFactory.CreateFor2DTruss(3, 3);
			
			FiniteElementNode node5 = model.NodeFactory.CreateFor2DTruss(6, 0);
			model.ConstrainNode(node5, DegreeOfFreedom.Z);
			
			FiniteElementNode node6 = model.NodeFactory.CreateFor2DTruss(6, 3);
			
			IMaterial material = new GenericElasticMaterial(0, 70000000, 0, 0);
            ICrossSection section = new SolidRectangle(0.03, 0.01);
			
            LinearTruss truss1 = model.ElementFactory.CreateLinearTruss(node1, node2, material, section);
            LinearTruss truss2 = model.ElementFactory.CreateLinearTruss(node1, node3, material, section);
            LinearTruss truss3 = model.ElementFactory.CreateLinearTruss(node2, node3, material, section);
            LinearTruss truss4 = model.ElementFactory.CreateLinearTruss(node2, node4, material, section);
            LinearTruss truss5 = model.ElementFactory.CreateLinearTruss(node1, node4, material, section);
            LinearTruss truss6 = model.ElementFactory.CreateLinearTruss(node3, node4, material, section);
            LinearTruss truss7 = model.ElementFactory.CreateLinearTruss(node3, node6, material, section);
            LinearTruss truss8 = model.ElementFactory.CreateLinearTruss(node4, node5, material, section);
            LinearTruss truss9 = model.ElementFactory.CreateLinearTruss(node4, node6, material, section);
            LinearTruss truss10 = model.ElementFactory.CreateLinearTruss(node3, node5, material, section);
            LinearTruss truss11 = model.ElementFactory.CreateLinearTruss(node5, node6, material, section);
            
            //apply forces
            
            ForceVector force50Z = model.ForceFactory.CreateForTruss(0, -50000);
            model.ApplyForceToNode(force50Z, node2);
            model.ApplyForceToNode(force50Z, node6);
            
            ForceVector force100Z = model.ForceFactory.CreateForTruss(0, -100000);
            model.ApplyForceToNode(force100Z, node4);
            
            //solve model
            IFiniteElementSolver solver = new MatrixInversionLinearSolver(model);
			FiniteElementResults results = solver.Solve();
			
			//assert results
			
			ReactionVector reactionAtNode1 = results.GetReaction(node1);
			Assert.AreEqual(0, reactionAtNode1.X, 1);
			Assert.AreEqual(100000, reactionAtNode1.Z, 1);
			
			ReactionVector reactionAtNode5 = results.GetReaction(node1);
			Assert.AreEqual(0, reactionAtNode5.X, 1);
			Assert.AreEqual(100000, reactionAtNode5.Z, 1);
			
			DisplacementVector displacementAtNode2 = results.GetDisplacement(node2);
			Assert.AreEqual(7.1429, displacementAtNode2.X, 0.001);
			Assert.AreEqual(-9.0386, displacementAtNode2.Z, 0.001);
			
			DisplacementVector displacementAtNode3 = results.GetDisplacement(node3);
			Assert.AreEqual(5.2471, displacementAtNode3.X, 0.001);
			Assert.AreEqual(-16.2965, displacementAtNode3.Z, 0.001);
			
			DisplacementVector displacementAtNode4 = results.GetDisplacement(node4);
			Assert.AreEqual(5.2471, displacementAtNode4.X, 0.001);
			Assert.AreEqual(-20.0881, displacementAtNode4.Z, 0.001);
			
			DisplacementVector displacementAtNode5 = results.GetDisplacement(node5);
			Assert.AreEqual(10.4942, displacementAtNode5.X, 0.001);
			Assert.AreEqual(0, displacementAtNode5.Z, 0.001);
			
			DisplacementVector displacementAtNode6 = results.GetDisplacement(node6);
			Assert.AreEqual(3.3513, displacementAtNode6.X, 0.001);
			Assert.AreEqual(-9.0386, displacementAtNode6.Z, 0.001);
		}
		
		[Test]
		public void Example3_6SolutionOfPlaneTruss_FirstCourseInTheFiniteElementMethod_Logan_4thEd()
		{
		    var model = new FiniteElementModel(ModelType.Truss2D);
		    
		    var node1 = model.NodeFactory.CreateFor2DTruss( 0,  0);
		    
		    var node2 = model.NodeFactory.CreateFor2DTruss( 0, 120); //10 feet in inches
		    model.ConstrainNode(node2, DegreeOfFreedom.X);
		    model.ConstrainNode(node2, DegreeOfFreedom.Z);
		    
		    var node3 = model.NodeFactory.CreateFor2DTruss(120, 120); //10 feet in inches
		    model.ConstrainNode(node3, DegreeOfFreedom.X);
		    model.ConstrainNode(node3, DegreeOfFreedom.Z);
		    
		    var node4 = model.NodeFactory.CreateFor2DTruss(120,  0); //10 feet in inches
		    model.ConstrainNode(node4, DegreeOfFreedom.X);
		    model.ConstrainNode(node4, DegreeOfFreedom.Z);
		    
		    IMaterial material = new GenericElasticMaterial(0, 30000000, 0, 0); //E = 30E6 psi
            ICrossSection section = new GenericCrossSection(2, 2); //A = 2 in^2
            
            var truss1 = model.ElementFactory.CreateLinearTruss(node1, node2, material, section);
            var truss2 = model.ElementFactory.CreateLinearTruss(node1, node3, material, section);
            var truss3 = model.ElementFactory.CreateLinearTruss(node1, node4, material, section);
            
            var force10Z = model.ForceFactory.CreateForTruss(0, -10000); //lbs
            model.ApplyForceToNode(force10Z, node1);
            
            //solve model
            var solver = new MatrixInversionLinearSolver(model);
			var results = solver.Solve();
			
			var displacementAtNode1 = results.GetDisplacement(node1);
			Assert.AreEqual(0.00414, displacementAtNode1.X, 0.000005);
			Assert.AreEqual(-0.0159, displacementAtNode1.Z, 0.00005);
		}
		
		[Test]
		public void Example3_7_FirstCourseInTheFiniteElementMethod_Logan_4thEd()
		{
		    var model = new FiniteElementModel(ModelType.Truss2D);
		    var node3 = model.NodeFactory.CreateFor2DTruss(  0,  0);
		    model.ConstrainNode(node3, DegreeOfFreedom.X);
		    model.ConstrainNode(node3, DegreeOfFreedom.Z);
		    
		    var node1 = model.NodeFactory.CreateFor2DTruss( 10,  0);
		    		    
		    var node2 = model.NodeFactory.CreateFor2DTruss( 10.0 - 5.0 / Math.Sqrt(2), 5.0 / Math.Sqrt(2)); //5m long at 45 degree angle
		    model.ConstrainNode(node2, DegreeOfFreedom.X);
		    model.ConstrainNode(node2, DegreeOfFreedom.Z);
		    
		    var node4 = model.NodeFactory.CreateFor2DTruss( 10, -1);
		    model.ConstrainNode(node4, DegreeOfFreedom.X);
		    model.ConstrainNode(node4, DegreeOfFreedom.Z);
		    
		    IMaterial material = new GenericElasticMaterial(0, 210000000000, 0, 0); //E = 210 GPa expressed as Pa == N/mm2
            ICrossSection section = new GenericCrossSection(0.0005, 1); //A = 5E-4 m^2
            
            var truss1 = model.ElementFactory.CreateLinearTruss(node1, node2, material, section);
            var truss2 = model.ElementFactory.CreateLinearTruss(node1, node3, material, section);
            var spring3 = model.ElementFactory.CreateLinearConstantSpring(node1, node4, 2000000); //2000 kN/m expressed as N/mm
            
            var force = model.ForceFactory.CreateForTruss(0, -25000);
            model.ApplyForceToNode(force, node1);
            
            var solver = new MatrixInversionLinearSolver(model);
            var results = solver.Solve();
            
            var displacementAtNode1 = results.GetDisplacement(node1);
            Console.WriteLine(displacementAtNode1);
            Assert.AreEqual(-0.001724, displacementAtNode1.X, 0.0000005);
            Assert.AreEqual(-0.003448, displacementAtNode1.Z, 0.0000005);
		}
    }
}
