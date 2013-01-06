﻿//-----------------------------------------------------------------------
// <copyright file="?.cs" company="Iain Sproat">
//     Copyright Iain Sproat, 2012.
// </copyright>
//-----------------------------------------------------------------------
using System;
using NUnit.Framework;
using SharpFE;
using SharpFE.Stiffness;

namespace SharpFE.Core.Tests.Stiffness
{
	[TestFixture]
	public class LinearConstantStrainTriangleStiffnessMatrixBuilderTest
	{
		private NodeFactory nodeFactory;
        private ElementFactory elementFactory;
        private FiniteElementNode node0;
        private FiniteElementNode node1;
        private FiniteElementNode node2;
        private GenericElasticMaterial material;
        private SolidRectangle section;
        private LinearConstantStrainTriangle triangle;
        private LinearConstantStrainTriangleStiffnessMatrixBuilder SUT;
        
        [SetUp]
        public void SetUp()
        {
        	nodeFactory = new NodeFactory(ModelType.Slab2D);
            node0 = nodeFactory.Create(0, 0);
            node1 = nodeFactory.Create(2, 0);
            node2 = nodeFactory.Create(1, 1);
            elementFactory = new ElementFactory();
			material = new GenericElasticMaterial(0, 1, 0, 1);
			section = new SolidRectangle(1, 1);
            triangle = elementFactory.CreateLinearConstantStrainTriangle(node0, node1, node2, material, section);
            SUT = new LinearConstantStrainTriangleStiffnessMatrixBuilder(triangle);
        }
        
        [Test]
        public void It_can_be_constructed()
        {
        	Assert.IsNotNull(SUT);
        	Assert.AreEqual(triangle, SUT.Element);
        }
        
        [Test]
        public void CanCanCreateGlobalStiffnessMatrixForSpringAlignedToGlobalXAxis()
        {
            double a = 1.0 / 3.0;
			double b = a / 2.0;
			double c = 0.1408333; //torsion
			
			Assert.Ignore(); //TODO
			StiffnessHelpers.Assert18x18StiffnessMatrix(SUT,
			                                             1,  0,    0,    0,  0,    0,      -1,  0,    0,    0,  0,    0,
			                                             0,  1,    0,    0,  0,    0.5,     0, -1,    0,    0,  0,    0.5,
			                                             0,  0,    1,    0, -0.5,  0,       0,  0,   -1,    0, -0.5,  0,
			                                             0,  0,    0,    c,  0,    0,       0,  0,    0,   -c,  0,    0,
			                                             0,  0,   -0.5,  0,  a,    0,       0,  0,    0.5,  0,  b,    0,
			                                             0,  0.5,  0,    0,  0,    a,       0, -0.5,  0,    0,  0,    b,
			                                            
			                                            -1,  0,    0,    0,  0,    0,       1,  0,    0,    0,  0,    0,
			                                             0, -1,    0,    0,  0,   -0.5,     0,  1,    0,    0,  0,   -0.5,
			                                             0,  0,   -1,    0,  0.5,  0,       0,  0,    1,    0,  0.5,  0,
			                                             0,  0,    0,   -c,  0,    0,       0,  0,    0,    c,  0,    0,
			                                             0,  0,   -0.5,  0,  b,    0,       0,  0,    0.5,  0,  a,    0,
			                                             0,  0.5,  0,    0,  0,    b,       0, -0.5,  0,    0,  0,    a);
        }
        
        [Test]
        public void StiffnessMatrixReflectsReleases()
        {
            Assert.Ignore();
        }
	}
}
