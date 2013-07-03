//-----------------------------------------------------------------------
// <copyright file="SpringElementTest.cs" company="SharpFE">
//     Copyright Iain Sproat, 2012.
// </copyright>
//-----------------------------------------------------------------------
using System;
using NUnit.Framework;
using SharpFE.Stiffness;

namespace SharpFE.Core.Tests.Stiffness
{
    public class LinearConstantSpringStiffnessMatrixBuilderTestBase
    {
        protected NodeFactory nodeFactory;
        protected ElementFactory elementFactory;
        protected FiniteElementNode start;
        protected FiniteElementNode end;
        protected LinearConstantSpring spring;
        protected ElementStiffnessMatrixBuilder<FiniteElement1D> SUT;
        
        [SetUp]
        public void SetUp()
        {
            this.nodeFactory = new NodeFactory(ModelType.Truss1D);
            this.start = nodeFactory.Create(0);
            this.end = nodeFactory.Create(1);
            this.elementFactory = new ElementFactory(ModelType.Truss1D);
            this.spring = elementFactory.CreateLinearConstantSpring(start, end, 2);
            this.SUT = new LinearTrussStiffnessMatrixBuilder(spring);
        }
        
        protected void CreateAndStore2DSpringFromOriginTo(double x, double z)
        {
            this.nodeFactory = new NodeFactory(ModelType.Truss2D);
            this.start = nodeFactory.CreateFor2DTruss(0, 0);
            this.end = nodeFactory.CreateFor2DTruss(x, z);
            
            this.elementFactory = new ElementFactory(ModelType.Truss2D);
            this.spring = elementFactory.CreateLinearConstantSpring(this.start, this.end, 1);
            this.SUT = new LinearTrussStiffnessMatrixBuilder(this.spring);
        }
        
        protected void CreateAndStore3DSpringFromOriginTo(double x, double y, double z)
        {
            nodeFactory = new NodeFactory(ModelType.Truss3D);
            start = nodeFactory.Create(0, 0, 0);
            end = nodeFactory.Create(x, y, z);
            
            elementFactory = new ElementFactory(ModelType.Truss3D);
            this.spring = elementFactory.CreateLinearConstantSpring(start, end, 1);
            this.SUT = new LinearTrussStiffnessMatrixBuilder(this.spring);
        }
        
        protected void Assert12x12StiffnessMatrix(params double[] expectedValues)
        {
        	StiffnessHelpers.Assert12x12StiffnessMatrix(SUT, expectedValues);
        }
    }
}
