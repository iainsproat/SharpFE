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
    /// <summary>
    /// Description of SpringElementTestBase.
    /// </summary>
    public class LinearConstantSpringStiffnessMatrixBuilderTestBase
    {
        protected NodeFactory nodeFactory;
        protected ElementFactory elementFactory;
        protected FiniteElementNode start;
        protected FiniteElementNode end;
        protected LinearConstantSpring spring;
        protected IStiffnessMatrixBuilder SUT;
        
        [SetUp]
        public void SetUp()
        {
            this.nodeFactory = new NodeFactory(ModelType.Truss1D);
            this.start = nodeFactory.Create(0);
            this.end = nodeFactory.Create(1);
            this.elementFactory = new ElementFactory();
            this.spring = elementFactory.CreateLinearConstantSpring(start, end, 2);
            this.SUT = new LinearTrussStiffnessMatrixBuilder(spring);
        }
        
        protected void CreateAndStore2DSpringFromOriginTo(double x, double z)
        {
            this.nodeFactory = new NodeFactory(ModelType.Truss2D);
            this.start = nodeFactory.CreateForTruss(0, 0);
            this.end = nodeFactory.CreateForTruss(x, z);
            
            this.elementFactory = new ElementFactory();
            this.spring = elementFactory.CreateLinearConstantSpring(this.start, this.end, 1);
            this.SUT = new LinearTrussStiffnessMatrixBuilder(this.spring);
        }
        
        protected void CreateAndStore3DSpringFromOriginTo(double x, double y, double z)
        {
            nodeFactory = new NodeFactory(ModelType.Truss3D);
            start = nodeFactory.Create(0, 0, 0);
            end = nodeFactory.Create(x, y, z);
            
            elementFactory = new ElementFactory();
            this.spring = elementFactory.CreateLinearConstantSpring(start, end, 1);
            this.SUT = new LinearTrussStiffnessMatrixBuilder(this.spring);
        }
        
        protected void Assert12x12StiffnessMatrix(params double[] expectedValues)
        {
        	StiffnessHelpers.Assert12x12StiffnessMatrix(SUT, expectedValues);
        }
        
        protected void Assert3x3RotationMatrix(params double[] expectedValues)
        {
        	// a rotation matrix has a determinant of +1
        	double det = SUT.CalculateElementRotationMatrix().Determinant();
        	Assert.AreEqual(1, 
        	                det,
        	                0.0001,
        	                String.Format("A rotation matrix should have a determinant of + 1. \n\r Determinant of {0} from actual matrix: \n\r {1}", 
        	                              det, 
        	                              SUT.CalculateElementRotationMatrix()));
        	
            Helpers.AssertMatrix(SUT.CalculateElementRotationMatrix(), 3, 3, expectedValues);
        }
    }
}
