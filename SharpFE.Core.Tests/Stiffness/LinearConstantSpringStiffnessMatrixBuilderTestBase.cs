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
        
        public void SetUp()
        {
            nodeFactory = new NodeFactory(ModelType.Truss1D);
            start = nodeFactory.Create(0);
            end = nodeFactory.Create(1);
            elementFactory = new ElementFactory();
            spring = elementFactory.CreateLinearConstantSpring(start, end, 2);
            SUT = spring.StiffnessBuilder;
        }
        
        protected void CreateAndStore2DSpringFromOriginTo(double x, double z)
        {
            nodeFactory = new NodeFactory(ModelType.Truss2D);
            start = nodeFactory.CreateForTruss(0, 0);
            end = nodeFactory.CreateForTruss(x, z);
            
            elementFactory = new ElementFactory();
            this.spring = elementFactory.CreateLinearConstantSpring(start, end, 1);
            this.SUT = this.spring.StiffnessBuilder;
        }
        
        protected void CreateAndStore3DSpringFromOriginTo(double x, double y, double z)
        {
            nodeFactory = new NodeFactory(ModelType.Truss3D);
            start = nodeFactory.Create(0, 0, 0);
            end = nodeFactory.Create(x, y, z);
            
            elementFactory = new ElementFactory();
            this.spring = elementFactory.CreateLinearConstantSpring(start, end, 1);
            this.SUT = this.spring.StiffnessBuilder;
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
