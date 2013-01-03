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
    public class ConstantLinearSpringElementRotationTest : ConstantLinearSpringElementTestBase
    {
        [SetUp]
        public void Setup()
        {
            base.SetUp();
        }
        
        [Test]
        public void CanCreateRotationMatrixForSpringAlignedToGlobalXAxis()
        {
            SUT = this.CreateSpringFromOriginTo(1, 0);
            
            SUT.PrepareAndGenerateLocalStiffnessMatrix();
            this.Assert3x3RotationMatrix( 1, 0, 0,
                                          0, 1, 0,
                                          0, 0, 1);
        }
        
        [Test]
        public void CanCreateRotationMatrixForSpringAlignedToNegativeGlobalXAxis()
        {
            SUT = this.CreateSpringFromOriginTo(-1, 0);
            
            SUT.PrepareAndGenerateLocalStiffnessMatrix();
            this.Assert3x3RotationMatrix( -1,  0, 0,
                                           0, -1, 0,
                                           0,  0, 1);
        }
        
        [Test]
        public void CanCreateRotationMatrixForSpringAlignedToGlobalYAxis()
        {
            SUT = this.CreateSpringFromOriginTo(0, 1);
            
            SUT.PrepareAndGenerateLocalStiffnessMatrix();
            this.Assert3x3RotationMatrix(  0, 1, 0,
                                          -1, 0, 0,
                                           0, 0, 1);
        }
        
        [Test]
        public void CanCreateRotationMatrixForSpringAlignedToNegativeGlobalYAxis()
        {
            SUT = this.CreateSpringFromOriginTo(0, -1);
            SUT.PrepareAndGenerateLocalStiffnessMatrix();
            this.Assert3x3RotationMatrix( 0, -1, 0,
                                          1,  0, 0,
                                          0,  0, 1);
        }
        
        [Test]
        public void CanCreateRotationMatrixForSpringAlignedToGlobalZAxis()
        {
            SUT = this.CreateSpringFromOriginTo(0, 0, 1);
            
            SUT.PrepareAndGenerateLocalStiffnessMatrix();
            this.Assert3x3RotationMatrix( 0, 0, 1,
                                          0, 1, 0,
                                         -1, 0, 0);
        }
        
        [Test]
        public void CanCreateRotationMatrixForSpringAlignedToNegativeGlobalZAxis()
        {
            SUT = this.CreateSpringFromOriginTo(0, 0, -1);
            SUT.PrepareAndGenerateLocalStiffnessMatrix();
            this.Assert3x3RotationMatrix(  0,  0, -1,
                                           0, -1,  0,
                                          -1,  0,  0);
        }
        
        [Test]
        public void CanCreateRotationMatrixForSpringAlignedToGlobalXYPlaneQuadrant1()
        {
            SUT = this.CreateSpringFromOriginTo(1, 1, 0);
            
            SUT.PrepareAndGenerateLocalStiffnessMatrix();
            
            double a = 1 / Math.Sqrt(2);
            this.Assert3x3RotationMatrix( a, a, 0,
                                         -a, a, 0,
                                          0, 0, 1);
        }
        
        [Test]
        public void CanCreateRotationMatrixForSpringAlignedToGlobalXZPlaneQuadrant1()
        {
            SUT = this.CreateSpringFromOriginTo(1, 0, 1);
            
            SUT.PrepareAndGenerateLocalStiffnessMatrix();
            
            double a = 1 / Math.Sqrt(2);
            this.Assert3x3RotationMatrix(  a, 0, a,
                                           0, 1, 0,
                                          -a, 0, a);
        }
        
        [Test]
        public void CanCreateRotationMatrixForSpringAlignedToGlobalYZPlaneQuadrant1()
        {
            SUT = this.CreateSpringFromOriginTo(0, 1, 1);
            
            SUT.PrepareAndGenerateLocalStiffnessMatrix();
            
            double a = 1 / Math.Sqrt(2);
            this.Assert3x3RotationMatrix( 0,  a, a,
                                         -1,  0, 0,
                                          0, -a, a);
        }
        
        [Test]
        public void CanCreateRotationMatrixForSpringInQuadrant1PositiveZ()
        {
            SUT = this.CreateSpringFromOriginTo(1, 1, 1);
            
            double a = 1.0 / Math.Sqrt(2.0);
            double b = 1.0 / Math.Sqrt(3.0);
            double c = 1.0 / Math.Sqrt(6.0);
            SUT.PrepareAndGenerateLocalStiffnessMatrix();
            
            this.Assert3x3RotationMatrix( b,  b,  b,
                                         -a,  a,  0,
                                         -c, -c,  2*c);
        }
        
        [Test]
        public void CanCreateRotationMatrixForSpringInQuadrant2PositiveZ()
        {
            SUT = this.CreateSpringFromOriginTo(-1, 1, 1);
            
            SUT.PrepareAndGenerateLocalStiffnessMatrix();
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
            SUT = this.CreateSpringFromOriginTo(-1, -1, 1);
            
            SUT.PrepareAndGenerateLocalStiffnessMatrix();
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
            SUT = this.CreateSpringFromOriginTo(1, -1, 1);
            
            SUT.PrepareAndGenerateLocalStiffnessMatrix();
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
            SUT = this.CreateSpringFromOriginTo(1, 1, -1);
            
            SUT.PrepareAndGenerateLocalStiffnessMatrix();
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
            SUT = this.CreateSpringFromOriginTo(-1, 1, -1);
            
            SUT.PrepareAndGenerateLocalStiffnessMatrix();
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
            SUT = this.CreateSpringFromOriginTo(-1, -1, -1);
            
            SUT.PrepareAndGenerateLocalStiffnessMatrix();
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
            SUT = this.CreateSpringFromOriginTo(1, -1, -1);
            
            SUT.PrepareAndGenerateLocalStiffnessMatrix();
            double a = 1.0 / Math.Sqrt(2.0);
            double b = 1.0 / Math.Sqrt(3.0);
            double c = 1.0 / Math.Sqrt(6.0);
            
            this.Assert3x3RotationMatrix( b, -b, -b,
                                          a,  a,  0,
                                          c, -c,  2*c);
        }
    }
}
