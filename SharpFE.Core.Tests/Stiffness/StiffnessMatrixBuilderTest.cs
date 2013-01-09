//-----------------------------------------------------------------------
// <copyright file="SpringElementTest.cs" company="SharpFE">
//     Copyright Iain Sproat, 2012.
// </copyright>
//-----------------------------------------------------------------------
using System;
using SharpFE;
using NUnit.Framework;

namespace SharpFE.Core.Tests.Stiffness
{
    [TestFixture]
    public class StiffnessMatrixBuilderTest : LinearConstantSpringStiffnessMatrixBuilderTestBase
    {
        
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
    }
}
