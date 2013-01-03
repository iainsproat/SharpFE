//-----------------------------------------------------------------------
// <copyright file="SpringElementTest.cs" company="SharpFE">
//     Copyright Iain Sproat, 2012.
// </copyright>
//-----------------------------------------------------------------------
using System;
using NUnit.Framework;

namespace SharpFE.Core.Tests.Elements
{
    /// <summary>
    /// Description of SpringElementTestBase.
    /// </summary>
    public class ConstantLinearSpringElementTestBase
    {
        protected NodeFactory nodeFactory;
        protected ElementFactory elementFactory;
        protected FiniteElementNode start;
        protected FiniteElementNode end;
        protected ConstantLinearSpring SUT;
        
        public void SetUp()
        {
            nodeFactory = new NodeFactory(ModelType.Truss1D);
            start = nodeFactory.Create(0);
            end = nodeFactory.Create(1);
            elementFactory = new ElementFactory();
            SUT = elementFactory.CreateConstantLinearSpring(start, end, 2);
        }
        
        protected ConstantLinearSpring CreateSpringFromOriginTo(double x, double y)
        {
            nodeFactory = new NodeFactory(ModelType.Truss2D);
            start = nodeFactory.Create(0, 0);
            end = nodeFactory.Create(x, y);
            
            elementFactory = new ElementFactory();
            return elementFactory.CreateConstantLinearSpring(start, end, 1);
        }
        
        protected ConstantLinearSpring CreateSpringFromOriginTo(double x, double y, double z)
        {
            nodeFactory = new NodeFactory(ModelType.Truss3D);
            start = nodeFactory.Create(0, 0, 0);
            end = nodeFactory.Create(x, y, z);
            
            elementFactory = new ElementFactory();
            return elementFactory.CreateConstantLinearSpring(start, end, 1);
        }
        
        protected void Assert12x12StiffnessMatrix(params double[] expectedValues)
        {
            Helpers.AssertMatrix(SUT.GlobalStiffnessMatrix, 12, 12, expectedValues);
        }
        
        protected void Assert3x3RotationMatrix(params double[] expectedValues)
        {
        	// a rotation matrix has a determinant of +1
        	double det = SUT.RotationMatrixFromGlobalToLocal.Determinant();
        	Assert.AreEqual(1, 
        	                det,
        	                0.0001,
        	                String.Format("A rotation matrix should have a determinant of + 1. \n\r Determinant of {0} from actual matrix: \n\r {1}", 
        	                              det, 
        	                              SUT.RotationMatrixFromGlobalToLocal));
        	
            Helpers.AssertMatrix(SUT.RotationMatrixFromGlobalToLocal, 3, 3, expectedValues);
        }
    }
}
