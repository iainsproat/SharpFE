//-----------------------------------------------------------------------
// <copyright file=StiffnessMatrixBuilderTest.cs" company="SharpFE">
//     Copyright Iain Sproat, 2012.
// </copyright>
//-----------------------------------------------------------------------

namespace SharpFE.Core.Tests.Solvers
{
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;
    using MathNet.Numerics.LinearAlgebra.Double;
    using Rhino.Mocks;
    using SharpFE;
    using SharpFE.Stiffness;
    
    [TestFixture]
    public class GlobalStiffnessMatrixBuilderTest
    {
        MockRepository mocks;
        IModelConstraintProvider constraintProvider;
        ITopologyQueryable topologyQueryable;
        IElementStiffnessMatrixBuilderFactory elementStiffnessMatrixBuilderFactory;
        NodeFactory nodeFactory;
        FiniteElementNode node1;
        FiniteElementNode node2;
        FiniteElementNode node3;
        IFiniteElement spring1;
        IFiniteElement spring2;
        IElementStiffnessCalculator spring1Calculator;
        IElementStiffnessCalculator spring2Calculator;
        GlobalModelStiffnessMatrixBuilder SUT;
        
        [SetUp]
        public void Setup()
        {
            mocks = new MockRepository();
            nodeFactory = new NodeFactory(ModelType.Truss1D);
            
            node1 = nodeFactory.Create(0);
            node2 = nodeFactory.Create(1);
            node3 = nodeFactory.Create(2);
            
            spring1 = mocks.StrictMock<IFiniteElement>();
            spring2 = mocks.StrictMock<IFiniteElement>();
            
            spring1Calculator = mocks.StrictMock<IElementStiffnessCalculator>();
            spring2Calculator = mocks.StrictMock<IElementStiffnessCalculator>();
            
            constraintProvider = mocks.StrictMock<IModelConstraintProvider>();
            
            topologyQueryable = mocks.StrictMock<ITopologyQueryable>();
            
            elementStiffnessMatrixBuilderFactory = mocks.StrictMock<IElementStiffnessMatrixBuilderFactory>();
            
            Expect.Call(elementStiffnessMatrixBuilderFactory.Create(spring1))
                .Return(spring1Calculator);
            Expect.Call(elementStiffnessMatrixBuilderFactory.Create(spring2))
                .Return(spring2Calculator);
            
            SUT = new GlobalModelStiffnessMatrixBuilder(topologyQueryable, constraintProvider, elementStiffnessMatrixBuilderFactory);
        }
        
        [Test]
        public void KnownForcesKnownDisplacementsMatrixCanBeGenerated()
        {
            ExpectCallForConstrainedNodalDegreeOfFreedoms();
            ExpectCallForUnconstrainedNodalDegreeOfFreedoms();
            
            ExpectCallForElementsDirectlyConnectingNode2Node1();
            ExpectCallForElementsDirectlyConnectingNode2Node3();
            
            Expect.Call(spring1Calculator.GetStiffnessInGlobalCoordinatesAt(node2, DegreeOfFreedom.X, node1, DegreeOfFreedom.X))
                .Return(7);
            Expect.Call(spring2Calculator.GetStiffnessInGlobalCoordinatesAt(node2, DegreeOfFreedom.X, node3, DegreeOfFreedom.X))
                .Return(11);

            mocks.ReplayAll();
            
            StiffnessMatrix result = SUT.BuildKnownForcesKnownDisplacementStiffnessMatrix();
            
            mocks.VerifyAll();
            Helpers.AssertMatrix(result, 1, 2, 7, 11);
        }
        
        [Test]
        public void KnownForcesUnknownDisplacementsMatrixCanBeGenerated()
        {
            ExpectCallForUnconstrainedNodalDegreeOfFreedoms().Repeat.Twice();
            ExpectCallForElementsDirectlyConnectingNode2();
            
            Expect.Call(spring1Calculator.GetStiffnessInGlobalCoordinatesAt(node2, DegreeOfFreedom.X, node2, DegreeOfFreedom.X))
                .Return(2);
            Expect.Call(spring2Calculator.GetStiffnessInGlobalCoordinatesAt(node2, DegreeOfFreedom.X, node2, DegreeOfFreedom.X))
                .Return(3);
            
            mocks.ReplayAll();
            
            StiffnessMatrix result = SUT.BuildKnownForcesUnknownDisplacementStiffnessMatrix();
            
            mocks.VerifyAll();
            Helpers.AssertMatrix(result, 1, 1, 5);
        }
        
        [Test]
        public void UnknownForcesKnownDisplacementsMatrixCanBeGenerated()
        {
            ExpectCallForConstrainedNodalDegreeOfFreedoms().Repeat.Twice();
            ExpectCallForElementsDirectlyConnectingNode1();
            ExpectCallForElementsDirectlyConnectingNode3();
            
            mocks.ReplayAll();
            
            StiffnessMatrix result = SUT.BuildUnknownForcesKnownDisplacementStiffnessMatrix();
            
            mocks.VerifyAll();
            Helpers.AssertMatrix(result, 2, 2, 3, 0, 0, 2);
        }
        
        [Test]
        public void UnknownForcesUnknownDisplacementsMatrixCanBeGenerated()
        {
            mocks.ReplayAll();
            StiffnessMatrix result = SUT.BuildUnknownForcesUnknownDisplacementStiffnessMatrix();
            mocks.VerifyAll();
            Helpers.AssertMatrix(result, 2, 1, -3, -2);
        }
        
        [Test]
        public void Can_build_matrices_for_beam_frame()
        {
            nodeFactory = new NodeFactory(ModelType.Frame2D);
            
            node1 = nodeFactory.CreateForTruss(0, 0);
            node2 = nodeFactory.CreateForTruss(1, 0);
            node3 = nodeFactory.CreateForTruss(2, 0);
            
            IMaterial material = new GenericElasticMaterial(0, 1, 0.3, 1);
            ICrossSection section = new SolidRectangle(1, 1);
            
            ElementFactory elementFactory = new ElementFactory();
            Linear1DBeam beam1 = elementFactory.CreateLinear1DBeam(node1, node2, material, section);
            Linear1DBeam beam2 = elementFactory.CreateLinear1DBeam(node2, node3, material, section);
            
            topologyQueryable = mocks.StrictMock<ITopologyQueryable>();
            
            constraintProvider = mocks.StrictMock<IModelConstraintProvider>();
            Expect.Call(constraintProvider.ConstrainedNodalDegreeOfFreedoms).Return(
                new List<NodalDegreeOfFreedom>(2){
                    new NodalDegreeOfFreedom(node1, DegreeOfFreedom.X),
                    new NodalDegreeOfFreedom(node1, DegreeOfFreedom.Z),
                    new NodalDegreeOfFreedom(node3, DegreeOfFreedom.X)
                });
            
            SUT = new GlobalModelStiffnessMatrixBuilder(topologyQueryable, constraintProvider);
            
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
        
        private Rhino.Mocks.Interfaces.IMethodOptions<IList<NodalDegreeOfFreedom>> ExpectCallForConstrainedNodalDegreeOfFreedoms()
        {
            return Expect.Call(constraintProvider.ConstrainedNodalDegreeOfFreedoms).Return(
                new List<NodalDegreeOfFreedom>(2){
                    new NodalDegreeOfFreedom(node1, DegreeOfFreedom.X),
                    new NodalDegreeOfFreedom(node3, DegreeOfFreedom.X)
                });
        }
        
        private Rhino.Mocks.Interfaces.IMethodOptions<IList<NodalDegreeOfFreedom>> ExpectCallForUnconstrainedNodalDegreeOfFreedoms()
        {
            return Expect.Call(constraintProvider.UnconstrainedNodalDegreeOfFreedoms).Return(
                new List<NodalDegreeOfFreedom>(1){
                    new NodalDegreeOfFreedom(node2, DegreeOfFreedom.X)
                });
        }
        
        private Rhino.Mocks.Interfaces.IMethodOptions<IList<IFiniteElement>> ExpectCallForElementsDirectlyConnectingNode1()
        {
            return Expect.Call(topologyQueryable.AllElementsDirectlyConnecting(node1, node1)).Return(
                new List<IFiniteElement>(1)
                {
                    spring1
                });
        }
        
        private Rhino.Mocks.Interfaces.IMethodOptions<IList<IFiniteElement>> ExpectCallForElementsDirectlyConnectingNode2()
        {
            return Expect.Call(topologyQueryable.AllElementsDirectlyConnecting(node2, node2)).Return(
                new List<IFiniteElement>(2)
                {
                    spring1,
                    spring2
                });
        }
        
        private Rhino.Mocks.Interfaces.IMethodOptions<IList<IFiniteElement>> ExpectCallForElementsDirectlyConnectingNode3()
        {
            return Expect.Call(topologyQueryable.AllElementsDirectlyConnecting(node3, node3)).Return(
                new List<IFiniteElement>(1)
                {
                    spring2
                });
        }
        
        private Rhino.Mocks.Interfaces.IMethodOptions<IList<IFiniteElement>> ExpectCallForElementsDirectlyConnectingNode2Node1()
        {
            return Expect.Call(topologyQueryable.AllElementsDirectlyConnecting(node2, node1)).Return(
                new List<IFiniteElement>(1)
                {
                    spring1
                });
        }
        
        private Rhino.Mocks.Interfaces.IMethodOptions<IList<IFiniteElement>> ExpectCallForElementsDirectlyConnectingNode2Node3()
        {
            return Expect.Call(topologyQueryable.AllElementsDirectlyConnecting(node2, node3)).Return(
                new List<IFiniteElement>(1)
                {
                    spring2
                });
        }
    }
}
