//-----------------------------------------------------------------------
// <copyright file=StiffnessMatrixBuilderTest.cs" company="SharpFE">
//     Copyright Iain Sproat, 2012.
// </copyright>
//-----------------------------------------------------------------------

namespace SharpFE.Core.Tests.Solvers
{
    using System;
    using NUnit.Framework;
    using MathNet.Numerics.LinearAlgebra.Double;
    using SharpFE;
    using SharpFE.Stiffness;
    
    [TestFixture]
    public class GlobalStiffnessMatrixBuilderTest
    {
        FiniteElementModel model;
        FiniteElementNode node1;
        FiniteElementNode node2;
        FiniteElementNode node3;
        LinearConstantSpring spring1;
        LinearConstantSpring spring2;
        GlobalModelStiffnessMatrixBuilder SUT;
        
        [SetUp]
        public void Setup()
        {
            model = new FiniteElementModel(ModelType.Truss1D);
            
            node1 = model.NodeFactory.Create(0);
            node2 = model.NodeFactory.Create(1);
            node3 = model.NodeFactory.Create(2);
            spring1 = model.ElementFactory.CreateLinearConstantSpring(node1, node2, 3);
            spring2 = model.ElementFactory.CreateLinearConstantSpring(node2, node3, 2);
            
            model.ConstrainNode(node1, DegreeOfFreedom.X);
            model.ConstrainNode(node3, DegreeOfFreedom.X);
            
            SUT = new GlobalModelStiffnessMatrixBuilder(model);
        }
        
        [Test]
        public void KnownForcesKnownDisplacementsMatrixCanBeGenerated()
        {
            StiffnessMatrix result = SUT.BuildKnownForcesKnownDisplacementStiffnessMatrix();
            Helpers.AssertMatrix(result, 1, 2, -3, -2);
        }
        
        [Test]
        public void KnownForcesUnknownDisplacementsMatrixCanBeGenerated()
        {
            StiffnessMatrix result = SUT.BuildKnownForcesUnknownDisplacementStiffnessMatrix();
            Helpers.AssertMatrix(result, 1, 1, 5);
        }
        
        [Test]
        public void UnknownForcesKnownDisplacementsMatrixCanBeGenerated()
        {
            StiffnessMatrix result = SUT.BuildUnknownForcesKnownDisplacementStiffnessMatrix();
            Helpers.AssertMatrix(result, 2, 2, 3, 0, 0, 2);
        }
        
        [Test]
        public void UnknownForcesUnknownDisplacementsMatrixCanBeGenerated()
        {
            StiffnessMatrix result = SUT.BuildUnknownForcesUnknownDisplacementStiffnessMatrix();
            Helpers.AssertMatrix(result, 2, 1, -3, -2);
        }
        
        [Test]
        public void Can_build_matrices_for_beam_frame()
        {
            model = new FiniteElementModel(ModelType.Frame2D);
            
            node1 = model.NodeFactory.CreateForTruss(0, 0);
            node2 = model.NodeFactory.CreateForTruss(1, 0);
            node3 = model.NodeFactory.CreateForTruss(2, 0);
            
            IMaterial material = new GenericElasticMaterial(0, 1, 0.3, 1);
            ICrossSection section = new SolidRectangle(1, 1);
            
            Linear1DBeam beam1 = model.ElementFactory.CreateLinear1DBeam(node1, node2, material, section);
            Linear1DBeam beam2 = model.ElementFactory.CreateLinear1DBeam(node2, node3, material, section);
            
            model.ConstrainNode(node1, DegreeOfFreedom.X);
            model.ConstrainNode(node1, DegreeOfFreedom.Z);
            model.ConstrainNode(node3, DegreeOfFreedom.X);
            
            SUT = new GlobalModelStiffnessMatrixBuilder(model);
            
            StiffnessMatrix globalModelStiffnessMatrix = SUT.BuildGlobalStiffnessMatrix();
            StiffnessMatrix knownForcesKnownDisplacements = SUT.BuildKnownForcesKnownDisplacementStiffnessMatrix();
            StiffnessMatrix knownForcesUnknownDisplacements = SUT.BuildKnownForcesUnknownDisplacementStiffnessMatrix();
            StiffnessMatrix unknownForcesKnownDisplacements = SUT.BuildUnknownForcesKnownDisplacementStiffnessMatrix();
            StiffnessMatrix unknownForcesUnknownDisplacements = SUT.BuildUnknownForcesUnknownDisplacementStiffnessMatrix();
            
            
//            Console.WriteLine(Helpers.PrettyPrintKeyedRowColumnMatrix(globalModelStiffnessMatrix));
//            Console.WriteLine("Determinant = " + globalModelStiffnessMatrix.Determinant());
//            Console.WriteLine("KnownForcesKnownDisplacements");
//            Console.WriteLine(Helpers.PrettyPrintKeyedRowColumnMatrix(knownForcesKnownDisplacements));
//            Console.WriteLine();
//            Console.WriteLine("KnownForcesUnknownDisplacements");
//            Console.WriteLine(Helpers.PrettyPrintKeyedRowColumnMatrix(knownForcesUnknownDisplacements));
//            Console.WriteLine();
//            Console.WriteLine("UnknownForcesKnownDisplacements");
//            Console.WriteLine(Helpers.PrettyPrintKeyedRowColumnMatrix(unknownForcesKnownDisplacements));
//            Console.WriteLine();
//            Console.WriteLine("UnknownForcesUnknownDisplacements");
//            Console.WriteLine(Helpers.PrettyPrintKeyedRowColumnMatrix(unknownForcesUnknownDisplacements));
            
            
            Helpers.AssertMatrix(knownForcesKnownDisplacements, 6, 3,
                                  0, -0.5,  0,
                                 -1,  0,   -1,
                                  0, -1,    0,
                                  0, -0.5,  0,
                                  0,  0,    0,
                                  0,  0,    0);
        }
    }
}
