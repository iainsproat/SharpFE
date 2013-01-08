//-----------------------------------------------------------------------
// <copyright file="?.cs" company="Iain Sproat">
//     Copyright Iain Sproat, 2013.
// </copyright>
//-----------------------------------------------------------------------
using System;
using NUnit.Framework;
using SharpFE.Stiffness;

namespace SharpFE.Core.Tests.Stiffness
{
	[TestFixture]
	public class LinearConstantStressQuadrilateralStiffnessMatrixBuilderTest
	{
		private NodeFactory nodeFactory;
        private ElementFactory elementFactory;
        private FiniteElementNode node0;
        private FiniteElementNode node1;
        private FiniteElementNode node2;
        private FiniteElementNode node3;
        private GenericElasticMaterial material;
        private LinearConstantStressQuadrilateral quad;
        private LinearConstantStressQuadrilateralStiffnessMatrixBuilder SUT;
        
        [SetUp]
        public void SetUp()
        {
        	nodeFactory = new NodeFactory(ModelType.Slab2D);
            node0 = nodeFactory.Create(0, 0);
            node1 = nodeFactory.Create(1, 0);
            node2 = nodeFactory.Create(1, 1);
            node3 = nodeFactory.Create(0, 1);
            elementFactory = new ElementFactory();
			material = new GenericElasticMaterial(0, 1, 0.1, 0);
            quad = elementFactory.CreateLinearConstantStressQuadrilateral(node0, node1, node2, node3, material, 0.1);
            SUT = new LinearConstantStressQuadrilateralStiffnessMatrixBuilder(quad);
        }
        
        [Test]
        public void It_can_be_constructed()
        {
        	Assert.IsNotNull(SUT);
        	Assert.AreEqual(quad, SUT.Element);
        }
        
        [Test]
        public void CanCreateGlobalStiffnessMatrixForSpringAlignedToGlobalXAxis()
        {
            double a = 1.0 / 3.0;
			double b = a / 2.0;
			double c = 0.1408333; //torsion
			
//			Assert.Ignore();
			// FIXME calculate and verify actual values.  The below values are just placeholders for now!
			StiffnessHelpers.Assert24x24StiffnessMatrix(SUT,
			                                             1,  0,    0,    0,  0,    0,      -1,  0,    0,    0,  0,    0,      0, 0, 0, 0, 0, 0,    0, 0, 0, 0, 0, 0,
			                                             0,  1,    0,    0,  0,    0.5,     0, -1,    0,    0,  0,    0.5,    0, 0, 0, 0, 0, 0,    0, 0, 0, 0, 0, 0,
			                                             0,  0,    1,    0, -0.5,  0,       0,  0,   -1,    0, -0.5,  0,      0, 0, 0, 0, 0, 0,    0, 0, 0, 0, 0, 0,
			                                             0,  0,    0,    c,  0,    0,       0,  0,    0,   -c,  0,    0,      0, 0, 0, 0, 0, 0,    0, 0, 0, 0, 0, 0,
			                                             0,  0,   -0.5,  0,  a,    0,       0,  0,    0.5,  0,  b,    0,      0, 0, 0, 0, 0, 0,    0, 0, 0, 0, 0, 0,
			                                             0,  0.5,  0,    0,  0,    a,       0, -0.5,  0,    0,  0,    b,      0, 0, 0, 0, 0, 0,    0, 0, 0, 0, 0, 0,
			                                            
			                                            -1,  0,    0,    0,  0,    0,       1,  0,    0,    0,  0,    0,      0, 0, 0, 0, 0, 0,    0, 0, 0, 0, 0, 0,
			                                             0, -1,    0,    0,  0,   -0.5,     0,  1,    0,    0,  0,   -0.5,    0, 0, 0, 0, 0, 0,    0, 0, 0, 0, 0, 0,
			                                             0,  0,   -1,    0,  0.5,  0,       0,  0,    1,    0,  0.5,  0,      0, 0, 0, 0, 0, 0,    0, 0, 0, 0, 0, 0,
			                                             0,  0,    0,   -c,  0,    0,       0,  0,    0,    c,  0,    0,      0, 0, 0, 0, 0, 0,    0, 0, 0, 0, 0, 0,
			                                             0,  0,   -0.5,  0,  b,    0,       0,  0,    0.5,  0,  a,    0,      0, 0, 0, 0, 0, 0,    0, 0, 0, 0, 0, 0,
			                                             0,  0.5,  0,    0,  0,    b,       0, -0.5,  0,    0,  0,    a,      0, 0, 0, 0, 0, 0,    0, 0, 0, 0, 0, 0,
			                                            
			                                            -1,  0,    0,    0,  0,    0,       1,  0,    0,    0,  0,    0,      0, 0, 0, 0, 0, 0,    0, 0, 0, 0, 0, 0,
			                                             0, -1,    0,    0,  0,   -0.5,     0,  1,    0,    0,  0,   -0.5,    0, 0, 0, 0, 0, 0,    0, 0, 0, 0, 0, 0,
			                                             0,  0,   -1,    0,  0.5,  0,       0,  0,    1,    0,  0.5,  0,      0, 0, 0, 0, 0, 0,    0, 0, 0, 0, 0, 0,
			                                             0,  0,    0,   -c,  0,    0,       0,  0,    0,    c,  0,    0,      0, 0, 0, 0, 0, 0,    0, 0, 0, 0, 0, 0,
			                                             0,  0,   -0.5,  0,  b,    0,       0,  0,    0.5,  0,  a,    0,      0, 0, 0, 0, 0, 0,    0, 0, 0, 0, 0, 0,
			                                             0,  0.5,  0,    0,  0,    b,       0, -0.5,  0,    0,  0,    a,      0, 0, 0, 0, 0, 0,    0, 0, 0, 0, 0, 0,
			                                            
			                                             0,  0,    0,    0,  0,    0,       0,  0,    0,    0,  0,    0,      0, 0, 0, 0, 0, 0,    0, 0, 0, 0, 0, 0,
			                                             0,  0,    0,    0,  0,    0,       0,  0,    0,    0,  0,    0,      0, 0, 0, 0, 0, 0,    0, 0, 0, 0, 0, 0,
			                                             0,  0,    0,    0,  0,    0,       0,  0,    0,    0,  0,    0,      0, 0, 0, 0, 0, 0,    0, 0, 0, 0, 0, 0,
			                                             0,  0,    0,    0,  0,    0,       0,  0,    0,    0,  0,    0,      0, 0, 0, 0, 0, 0,    0, 0, 0, 0, 0, 0,
			                                             0,  0,    0,    0,  0,    0,       0,  0,    0,    0,  0,    0,      0, 0, 0, 0, 0, 0,    0, 0, 0, 0, 0, 0,
			                                             0,  0,    0,    0,  0,    0,       0,  0,    0,    0,  0,    0,      0, 0, 0, 0, 0, 0,    0, 0, 0, 0, 0, 0);
        }
        
        [Test]
        public void ShapeFunctions()
        {
        	Assert.Ignore();
        }
        
        [Test]
        public void StrainDisplacement()
        {
        	Assert.Ignore();
        }
        
        [Test]
        public void StiffnessMatrixReflectsReleases()
        {
            Assert.Ignore();
        }
	}
}
