//-----------------------------------------------------------------------
// <copyright file="SpringElementTest.cs" company="SharpFE">
//     Copyright Iain Sproat, 2012.
// </copyright>
//-----------------------------------------------------------------------
using System;
using SharpFE;
using NUnit.Framework;

namespace SharpFE.Core.Tests.Elements
{
    [TestFixture]
    public class FiniteElementTest_Rotation
    {
        protected NodeFactory nodeFactory;
        protected ElementFactory elementFactory;
        protected FiniteElementNode start;
        protected FiniteElementNode end;
        protected FiniteElement SUT;
        
        [Test]
        public void CanCreateRotationMatrixForSpringAlignedToGlobalXAxis()
        {
            this.CreateAndStore3DSpringFromOriginTo(1, 0, 0);
            
            this.Assert3x3RotationMatrix( 1, 0, 0,
                                          0, 1, 0,
                                          0, 0, 1);
        }
        
        [Test]
        public void CanCreateRotationMatrixForSpringAlignedToNegativeGlobalXAxis()
        {
            this.CreateAndStore3DSpringFromOriginTo(-1, 0, 0);
            
            this.Assert3x3RotationMatrix( -1,  0, 0,
                                           0, -1, 0,
                                           0,  0, 1);
        }
        
        [Test]
        public void CanCreateRotationMatrixForSpringAlignedToGlobalYAxis()
        {
            this.CreateAndStore3DSpringFromOriginTo(0, 1, 0);
            
            this.Assert3x3RotationMatrix(  0, 1, 0,
                                          -1, 0, 0,
                                           0, 0, 1);
        }
        
        [Test]
        public void CanCreateRotationMatrixForSpringAlignedToNegativeGlobalYAxis()
        {
            this.CreateAndStore3DSpringFromOriginTo(0, -1, 0);
            
            this.Assert3x3RotationMatrix( 0, -1, 0,
                                          1,  0, 0,
                                          0,  0, 1);
        }
        
        [Test]
        public void CanCreateRotationMatrixForSpringAlignedToGlobalZAxis()
        {
            this.CreateAndStore3DSpringFromOriginTo(0, 0, 1);
            
            this.Assert3x3RotationMatrix( 0, 0, 1,
                                          0, 1, 0,
                                         -1, 0, 0);
        }
        
        [Test]
        public void CanCreateRotationMatrixForSpringAlignedToNegativeGlobalZAxis()
        {
            this.CreateAndStore3DSpringFromOriginTo(0, 0, -1);
            
            this.Assert3x3RotationMatrix(  0,  0, -1,
                                           0, -1,  0,
                                          -1,  0,  0);
        }
        
        [Test]
        public void CanCreateRotationMatrixForSpringAlignedToGlobalXYPlaneQuadrant1()
        {
            this.CreateAndStore3DSpringFromOriginTo(1, 1, 0);
            
            double a = 1 / Math.Sqrt(2);
            this.Assert3x3RotationMatrix( a, a, 0,
                                         -a, a, 0,
                                          0, 0, 1);
        }
        
        [Test]
        public void CanCreateRotationMatrixForSpringAlignedToGlobalXZPlaneQuadrant1()
        {
            this.CreateAndStore3DSpringFromOriginTo(1, 0, 1);
            
            double a = 1 / Math.Sqrt(2);
            this.Assert3x3RotationMatrix(  a, 0, a,
                                           0, 1, 0,
                                          -a, 0, a);
        }
        
        [Test]
        public void CanCreateRotationMatrixForSpringAlignedToGlobalYZPlaneQuadrant1()
        {
            this.CreateAndStore3DSpringFromOriginTo(0, 1, 1);
            
            double a = 1 / Math.Sqrt(2);
            this.Assert3x3RotationMatrix( 0,  a, a,
                                         -1,  0, 0,
                                          0, -a, a);
        }
        
        [Test]
        public void CanCreateRotationMatrixForSpringInQuadrant1PositiveZ()
        {
            this.CreateAndStore3DSpringFromOriginTo(1, 1, 1);
            
            double a = 1.0 / Math.Sqrt(2.0);
            double b = 1.0 / Math.Sqrt(3.0);
            double c = 1.0 / Math.Sqrt(6.0);
            
            this.Assert3x3RotationMatrix( b,  b,  b,
                                         -a,  a,  0,
                                         -c, -c,  2*c);
        }
        
        [Test]
        public void CanCreateRotationMatrixForSpringInQuadrant2PositiveZ()
        {
            this.CreateAndStore3DSpringFromOriginTo(-1, 1, 1);
            
            double a = 1.0 / Math.Sqrt(2.0);
            double b = 1.0 / Math.Sqrt(3.0);
            double c = 1.0 / Math.Sqrt(6.0);
            
            this.Assert3x3RotationMatrix( -b,  b,  b,
                                          -a, -a,  0,
                                           c, -c,  2*c);
        }
        
        [Test]
        public void CanCreateRotationMatrixForSpringInQuadrant3PositiveZ()
        {
            this.CreateAndStore3DSpringFromOriginTo(-1, -1, 1);
            
            double a = 1.0 / Math.Sqrt(2.0);
            double b = 1.0 / Math.Sqrt(3.0);
            double c = 1.0 / Math.Sqrt(6.0);
            
            this.Assert3x3RotationMatrix( -b, -b, b,
                                           a, -a, 0,
                                           c,  c, 2*c);
        }
        
        [Test]
        public void CanCreateRotationMatrixForSpringInQuadrant4PositiveZ()
        {
            this.CreateAndStore3DSpringFromOriginTo(1, -1, 1);
            
            double a = 1.0 / Math.Sqrt(2.0);
            double b = 1.0 / Math.Sqrt(3.0);
            double c = 1.0 / Math.Sqrt(6.0);
            
            this.Assert3x3RotationMatrix( b, -b, b,
                                          a,  a, 0,
                                         -c,  c, 2*c);
        }
        
        [Test]
        public void CanCreateRotationMatrixForSpringInQuadrant1NegativeZ()
        {
            this.CreateAndStore3DSpringFromOriginTo(1, 1, -1);
            
            double a = 1.0 / Math.Sqrt(2.0);
            double b = 1.0 / Math.Sqrt(3.0);
            double c = 1.0 / Math.Sqrt(6.0);
            
            this.Assert3x3RotationMatrix( b, b, -b,
                                         -a, a,  0,
                                          c, c,  2*c);
        }
        
        [Test]
        public void CanCreateRotationMatrixForSpringInQuadrant2NegativeZ()
        {
            this.CreateAndStore3DSpringFromOriginTo(-1, 1, -1);
            
            double a = 1.0 / Math.Sqrt(2.0);
            double b = 1.0 / Math.Sqrt(3.0);
            double c = 1.0 / Math.Sqrt(6.0);
            
            this.Assert3x3RotationMatrix( -b,  b, -b,
                                          -a, -a,  0,
                                          -c,  c,  2*c);
        }
        
        [Test]
        public void CanCreateRotationMatrixForSpringInQuadrant3NegativeZ()
        {
            this.CreateAndStore3DSpringFromOriginTo(-1, -1, -1);
            
            double a = 1.0 / Math.Sqrt(2.0);
            double b = 1.0 / Math.Sqrt(3.0);
            double c = 1.0 / Math.Sqrt(6.0);
            
            this.Assert3x3RotationMatrix(-b, -b, -b,
                                          a, -a,  0,
                                         -c, -c,  2*c);
        }
        
        [Test]
        public void CanCreateRotationMatrixForSpringInQuadrant4NegativeZ()
        {
            this.CreateAndStore3DSpringFromOriginTo(1, -1, -1);
            
            double a = 1.0 / Math.Sqrt(2.0);
            double b = 1.0 / Math.Sqrt(3.0);
            double c = 1.0 / Math.Sqrt(6.0);
            
            this.Assert3x3RotationMatrix( b, -b, -b,
                                          a,  a,  0,
                                          c, -c,  2*c);
        }
        
        protected void CreateAndStore2DSpringFromOriginTo(double x, double z)
        {
            this.nodeFactory = new NodeFactory(ModelType.Truss2D);
            this.start = nodeFactory.CreateFor2DTruss(0, 0);
            this.end = nodeFactory.CreateFor2DTruss(x, z);
            
            this.elementFactory = new ElementFactory(ModelType.Truss2D);
            this.SUT = elementFactory.CreateLinearConstantSpring(this.start, this.end, 1);
        }
        
        protected void CreateAndStore3DSpringFromOriginTo(double x, double y, double z)
        {
            nodeFactory = new NodeFactory(ModelType.Truss3D);
            start = nodeFactory.Create(0, 0, 0);
            end = nodeFactory.Create(x, y, z);
            
            elementFactory = new ElementFactory(ModelType.Truss3D);
            this.SUT = elementFactory.CreateLinearConstantSpring(start, end, 1);
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
