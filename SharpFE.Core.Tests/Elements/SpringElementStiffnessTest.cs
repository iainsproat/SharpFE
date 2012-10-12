//-----------------------------------------------------------------------
// <copyright file="SpringElementRotationTest.cs" company="Iain Sproat">
//     Copyright Iain Sproat, 2012.
// </copyright>
//-----------------------------------------------------------------------
using System;
using NUnit.Framework;

namespace SharpFE.Tests.Elements
{
    [TestFixture]
    public class SpringElementStiffnessTest : SpringElementTestBase
    {
                
        #region Global Stiffness Matrix
        [Test]
        public void CanCanCreateGlobalStiffnessMatrixForSpringAlignedToGlobalXAxis()
        {
            SUT = this.CreateSpringFromOriginTo(1, 0);
            
            SUT.PrepareAndGenerateLocalStiffnessMatrix();
            
            Helpers.AssertMatrix(SUT.ElementStiffnessRotationMatrixFromLocalToGlobalCoordinates, 12, 12,
                                 1, 0, 0, 0, 0, 0,  0, 0, 0, 0, 0, 0,
                                 0, 1, 0, 0, 0, 0,  0, 0, 0, 0, 0, 0,
                                 0, 0, 1, 0, 0, 0,  0, 0, 0, 0, 0, 0,
                                 0, 0, 0, 0, 0, 0,  0, 0, 0, 0, 0, 0,
                                 0, 0, 0, 0, 0, 0,  0, 0, 0, 0, 0, 0,
                                 0, 0, 0, 0, 0, 0,  0, 0, 0, 0, 0, 0,
                                 0, 0, 0, 0, 0, 0,  1, 0, 0, 0, 0, 0,
                                 0, 0, 0, 0, 0, 0,  0, 1, 0, 0, 0, 0,
                                 0, 0, 0, 0, 0, 0,  0, 0, 1, 0, 0, 0,
                                 0, 0, 0, 0, 0, 0,  0, 0, 0, 0, 0, 0,
                                 0, 0, 0, 0, 0, 0,  0, 0, 0, 0, 0, 0,
                                 0, 0, 0, 0, 0, 0,  0, 0, 0, 0, 0, 0);
            
            this.Assert12x12StiffnessMatrix(1, 0, 0, 0, 0, 0, -1, 0, 0, 0, 0, 0,
                                            0, 0, 0, 0, 0, 0,  0, 0, 0, 0, 0, 0,
                                            0, 0, 0, 0, 0, 0,  0, 0, 0, 0, 0, 0,
                                            0, 0, 0, 0, 0, 0,  0, 0, 0, 0, 0, 0,
                                            0, 0, 0, 0, 0, 0,  0, 0, 0, 0, 0, 0,
                                            0, 0, 0, 0, 0, 0,  0, 0, 0, 0, 0, 0,
                                            -1, 0, 0, 0, 0, 0,  1, 0, 0, 0, 0, 0,
                                            0, 0, 0, 0, 0, 0,  0, 0, 0, 0, 0, 0,
                                            0, 0, 0, 0, 0, 0,  0, 0, 0, 0, 0, 0,
                                            0, 0, 0, 0, 0, 0,  0, 0, 0, 0, 0, 0,
                                            0, 0, 0, 0, 0, 0,  0, 0, 0, 0, 0, 0,
                                            0, 0, 0, 0, 0, 0,  0, 0, 0, 0, 0, 0);
        }
        
        [Test]
        public void CanCanCreateGlobalStiffnessMatrixForSpringAlignedToNegativeGlobalXAxis()
        {
            SUT = this.CreateSpringFromOriginTo(-1, 0);
            
            SUT.PrepareAndGenerateLocalStiffnessMatrix();
            
            this.Assert12x12StiffnessMatrix(1, 0, 0, 0, 0, 0, -1, 0, 0, 0, 0, 0,
                                            0, 0, 0, 0, 0, 0,  0, 0, 0, 0, 0, 0,
                                            0, 0, 0, 0, 0, 0,  0, 0, 0, 0, 0, 0,
                                            0, 0, 0, 0, 0, 0,  0, 0, 0, 0, 0, 0,
                                            0, 0, 0, 0, 0, 0,  0, 0, 0, 0, 0, 0,
                                            0, 0, 0, 0, 0, 0,  0, 0, 0, 0, 0, 0,
                                            -1, 0, 0, 0, 0, 0,  1, 0, 0, 0, 0, 0,
                                            0, 0, 0, 0, 0, 0,  0, 0, 0, 0, 0, 0,
                                            0, 0, 0, 0, 0, 0,  0, 0, 0, 0, 0, 0,
                                            0, 0, 0, 0, 0, 0,  0, 0, 0, 0, 0, 0,
                                            0, 0, 0, 0, 0, 0,  0, 0, 0, 0, 0, 0,
                                            0, 0, 0, 0, 0, 0,  0, 0, 0, 0, 0, 0);
        }
        
        [Test]
        public void CanCanCreateGlobalStiffnessMatrixForSpringAlignedToGlobalYAxis()
        {
            SUT = this.CreateSpringFromOriginTo(0, 1);
            
            SUT.PrepareAndGenerateLocalStiffnessMatrix();
            
            this.Assert12x12StiffnessMatrix(0,  0, 0, 0, 0, 0, 0,  0, 0, 0, 0, 0,
                                            0,  1, 0, 0, 0, 0, 0, -1, 0, 0, 0, 0,
                                            0,  0, 0, 0, 0, 0, 0,  0, 0, 0, 0, 0,
                                            0,  0, 0, 0, 0, 0, 0,  0, 0, 0, 0, 0,
                                            0,  0, 0, 0, 0, 0, 0,  0, 0, 0, 0, 0,
                                            0,  0, 0, 0, 0, 0, 0,  0, 0, 0, 0, 0,
                                            0,  0, 0, 0, 0, 0, 0,  0, 0, 0, 0, 0,
                                            0, -1, 0, 0, 0, 0, 0,  1, 0, 0, 0, 0,
                                            0,  0, 0, 0, 0, 0, 0,  0, 0, 0, 0, 0,
                                            0,  0, 0, 0, 0, 0, 0,  0, 0, 0, 0, 0,
                                            0,  0, 0, 0, 0, 0, 0,  0, 0, 0, 0, 0,
                                            0,  0, 0, 0, 0, 0, 0,  0, 0, 0, 0, 0);
        }
        
                [Test]
        public void CanCanCreateGlobalStiffnessMatrixForSpringAlignedToNegativeGlobalYAxis()
        {
            SUT = this.CreateSpringFromOriginTo(0, -1);
            
            SUT.PrepareAndGenerateLocalStiffnessMatrix();
            
            this.Assert12x12StiffnessMatrix(0,  0, 0, 0, 0, 0, 0,  0, 0, 0, 0, 0,
                                            0,  1, 0, 0, 0, 0, 0, -1, 0, 0, 0, 0,
                                            0,  0, 0, 0, 0, 0, 0,  0, 0, 0, 0, 0,
                                            0,  0, 0, 0, 0, 0, 0,  0, 0, 0, 0, 0,
                                            0,  0, 0, 0, 0, 0, 0,  0, 0, 0, 0, 0,
                                            0,  0, 0, 0, 0, 0, 0,  0, 0, 0, 0, 0,
                                            0,  0, 0, 0, 0, 0, 0,  0, 0, 0, 0, 0,
                                            0, -1, 0, 0, 0, 0, 0,  1, 0, 0, 0, 0,
                                            0,  0, 0, 0, 0, 0, 0,  0, 0, 0, 0, 0,
                                            0,  0, 0, 0, 0, 0, 0,  0, 0, 0, 0, 0,
                                            0,  0, 0, 0, 0, 0, 0,  0, 0, 0, 0, 0,
                                            0,  0, 0, 0, 0, 0, 0,  0, 0, 0, 0, 0);
        }
        
        [Test]
        public void CanCanCreateGlobalStiffnessMatrixForSpringAlignedToGlobalZAxis()
        {
            SUT = this.CreateSpringFromOriginTo(0, 0, 1);
            
            SUT.PrepareAndGenerateLocalStiffnessMatrix();
            this.Assert12x12StiffnessMatrix(0, 0,  0, 0, 0, 0, 0, 0,  0, 0, 0, 0,
                                            0, 0,  0, 0, 0, 0, 0, 0,  0, 0, 0, 0,
                                            0, 0,  1, 0, 0, 0, 0, 0, -1, 0, 0, 0,
                                            0, 0,  0, 0, 0, 0, 0, 0,  0, 0, 0, 0,
                                            0, 0,  0, 0, 0, 0, 0, 0,  0, 0, 0, 0,
                                            0, 0,  0, 0, 0, 0, 0, 0,  0, 0, 0, 0,
                                            0, 0,  0, 0, 0, 0, 0, 0,  0, 0, 0, 0,
                                            0, 0,  0, 0, 0, 0, 0, 0,  0, 0, 0, 0,
                                            0, 0, -1, 0, 0, 0, 0, 0,  1, 0, 0, 0,
                                            0, 0,  0, 0, 0, 0, 0, 0,  0, 0, 0, 0,
                                            0, 0,  0, 0, 0, 0, 0, 0,  0, 0, 0, 0,
                                            0, 0,  0, 0, 0, 0, 0, 0,  0, 0, 0, 0);
        }
        
        [Test]
        public void CanCanCreateGlobalStiffnessMatrixForSpringAlignedToNegativeGlobalZAxis()
        {
            SUT = this.CreateSpringFromOriginTo(0, 0, -1);
            
            SUT.PrepareAndGenerateLocalStiffnessMatrix();
            this.Assert12x12StiffnessMatrix(0, 0,  0, 0, 0, 0, 0, 0,  0, 0, 0, 0,
                                            0, 0,  0, 0, 0, 0, 0, 0,  0, 0, 0, 0,
                                            0, 0,  1, 0, 0, 0, 0, 0, -1, 0, 0, 0,
                                            0, 0,  0, 0, 0, 0, 0, 0,  0, 0, 0, 0,
                                            0, 0,  0, 0, 0, 0, 0, 0,  0, 0, 0, 0,
                                            0, 0,  0, 0, 0, 0, 0, 0,  0, 0, 0, 0,
                                            0, 0,  0, 0, 0, 0, 0, 0,  0, 0, 0, 0,
                                            0, 0,  0, 0, 0, 0, 0, 0,  0, 0, 0, 0,
                                            0, 0, -1, 0, 0, 0, 0, 0,  1, 0, 0, 0,
                                            0, 0,  0, 0, 0, 0, 0, 0,  0, 0, 0, 0,
                                            0, 0,  0, 0, 0, 0, 0, 0,  0, 0, 0, 0,
                                            0, 0,  0, 0, 0, 0, 0, 0,  0, 0, 0, 0);
        }
        
        [Test]
        public void CanCanCreateGlobalStiffnessMatrixForSpringAlignedToGlobalXYPlaneQuadrant1()
        {
            SUT = this.CreateSpringFromOriginTo(1, 1, 0);
            SUT.PrepareAndGenerateLocalStiffnessMatrix();
            this.Assert12x12StiffnessMatrix( 0.25,  0.25, 0, 0, 0, 0,  -0.25, -0.25, 0, 0, 0, 0,
                                             0.25,  0.25, 0, 0, 0, 0,  -0.25, -0.25, 0, 0, 0, 0,
                                             0,     0,    0, 0, 0, 0,   0,     0,    0, 0, 0, 0,
                                             0,     0,    0, 0, 0, 0,   0,     0,    0, 0, 0, 0,
                                             0,     0,    0, 0, 0, 0,   0,     0,    0, 0, 0, 0,
                                             0,     0,    0, 0, 0, 0,   0,     0,    0, 0, 0, 0,
                                            -0.25, -0.25, 0, 0, 0, 0,   0.25,  0.25, 0, 0, 0, 0,
                                            -0.25, -0.25, 0, 0, 0, 0,   0.25,  0.25, 0, 0, 0, 0,
                                             0,     0,    0, 0, 0, 0,   0,     0,    0, 0, 0, 0,
                                             0,     0,    0, 0, 0, 0,   0,     0,    0, 0, 0, 0,
                                             0,     0,    0, 0, 0, 0,   0,     0,    0, 0, 0, 0,
                                             0,     0,    0, 0, 0, 0,   0,     0,    0, 0, 0, 0); // FIXME this is incorrect, it should involve sqrt(2) somewhere.
        }
        
        [Test]
        public void CanCanCreateGlobalStiffnessMatrixForSpringAlignedToGlobalXYPlaneQuadrant3()
        {
            SUT = this.CreateSpringFromOriginTo(-1, -1, 0);
            SUT.PrepareAndGenerateLocalStiffnessMatrix();
            this.Assert12x12StiffnessMatrix( 0.25,  0.25, 0, 0, 0, 0,  -0.25, -0.25, 0, 0, 0, 0,
                                             0.25,  0.25, 0, 0, 0, 0,  -0.25, -0.25, 0, 0, 0, 0,
                                             0,     0,    0, 0, 0, 0,   0,     0,    0, 0, 0, 0,
                                             0,     0,    0, 0, 0, 0,   0,     0,    0, 0, 0, 0,
                                             0,     0,    0, 0, 0, 0,   0,     0,    0, 0, 0, 0,
                                             0,     0,    0, 0, 0, 0,   0,     0,    0, 0, 0, 0,
                                            -0.25, -0.25, 0, 0, 0, 0,   0.25,  0.25, 0, 0, 0, 0,
                                            -0.25, -0.25, 0, 0, 0, 0,   0.25,  0.25, 0, 0, 0, 0,
                                             0,     0,    0, 0, 0, 0,   0,     0,    0, 0, 0, 0,
                                             0,     0,    0, 0, 0, 0,   0,     0,    0, 0, 0, 0,
                                             0,     0,    0, 0, 0, 0,   0,     0,    0, 0, 0, 0,
                                             0,     0,    0, 0, 0, 0,   0,     0,    0, 0, 0, 0);
        }
        
        [Test]
        public void CanCanCreateGlobalStiffnessMatrixForSpringAlignedToGlobalXZPlaneQuadrant1()
        {
            SUT = this.CreateSpringFromOriginTo(1, 0, 1);
            
            SUT.PrepareAndGenerateLocalStiffnessMatrix();
            this.Assert12x12StiffnessMatrix( 0.25, 0,  0.25, 0, 0, 0, -0.25, 0,   -0.25, 0, 0, 0,
                                             0,    0,  0,    0, 0, 0,  0,    0,    0,    0, 0, 0,
                                             0.25, 0,  0.25, 0, 0, 0, -0.25, 0,   -0.25, 0, 0, 0,
                                             0,    0,  0,    0, 0, 0,  0,    0,    0,    0, 0, 0,
                                             0,    0,  0,    0, 0, 0,  0,    0,    0,    0, 0, 0,
                                             0,    0,  0,    0, 0, 0,  0,    0,    0,    0, 0, 0,
                                            -0.25, 0, -0.25, 0, 0, 0,  0.25, 0,    0.25, 0, 0, 0,
                                             0,    0,  0,    0, 0, 0,  0,    0,    0,    0, 0, 0,
                                            -0.25, 0, -0.25, 0, 0, 0,  0.25, 0,    0.25, 0, 0, 0,
                                             0,    0,  0,    0, 0, 0,  0,    0,    0,    0, 0, 0,
                                             0,    0,  0,    0, 0, 0,  0,    0,    0,    0, 0, 0,
                                             0,    0,  0,    0, 0, 0,  0,    0,    0,    0, 0, 0);
        }
        
        [Test]
        public void CanCanCreateGlobalStiffnessMatrixForSpringAlignedToGlobalXZPlaneQuadrant3()
        {
            SUT = this.CreateSpringFromOriginTo(-1, 0, -1);
            
            SUT.PrepareAndGenerateLocalStiffnessMatrix();
            this.Assert12x12StiffnessMatrix( 0.25, 0,  0.25, 0, 0, 0, -0.25, 0,   -0.25, 0, 0, 0,
                                             0,    0,  0,    0, 0, 0,  0,    0,    0,    0, 0, 0,
                                             0.25, 0,  0.25, 0, 0, 0, -0.25, 0,   -0.25, 0, 0, 0,
                                             0,    0,  0,    0, 0, 0,  0,    0,    0,    0, 0, 0,
                                             0,    0,  0,    0, 0, 0,  0,    0,    0,    0, 0, 0,
                                             0,    0,  0,    0, 0, 0,  0,    0,    0,    0, 0, 0,
                                            -0.25, 0, -0.25, 0, 0, 0,  0.25, 0,    0.25, 0, 0, 0,
                                             0,    0,  0,    0, 0, 0,  0,    0,    0,    0, 0, 0,
                                            -0.25, 0, -0.25, 0, 0, 0,  0.25, 0,    0.25, 0, 0, 0,
                                             0,    0,  0,    0, 0, 0,  0,    0,    0,    0, 0, 0,
                                             0,    0,  0,    0, 0, 0,  0,    0,    0,    0, 0, 0,
                                             0,    0,  0,    0, 0, 0,  0,    0,    0,    0, 0, 0);
        }
        
        [Test]
        public void CanCanCreateGlobalStiffnessMatrixForSpringAlignedToGlobalYZPlaneQuadrant1()
        {
            SUT = this.CreateSpringFromOriginTo(0, 1, 1);
            
            SUT.PrepareAndGenerateLocalStiffnessMatrix();
            this.Assert12x12StiffnessMatrix(0,  0,     0,    0, 0, 0, 0,  0,     0,    0, 0, 0,
                                            0,  0.25,  0.25, 0, 0, 0, 0, -0.25, -0.25, 0, 0, 0,
                                            0,  0.25,  0.25, 0, 0, 0, 0, -0.25, -0.25, 0, 0, 0,
                                            0,  0,     0,    0, 0, 0, 0,  0,     0,    0, 0, 0,
                                            0,  0,     0,    0, 0, 0, 0,  0,     0,    0, 0, 0,
                                            0,  0,     0,    0, 0, 0, 0,  0,     0,    0, 0, 0,
                                            0,  0,     0,    0, 0, 0, 0,  0,     0,    0, 0, 0,
                                            0, -0.25, -0.25, 0, 0, 0, 0,  0.25,  0.25, 0, 0, 0,
                                            0, -0.25, -0.25, 0, 0, 0, 0,  0.25,  0.25, 0, 0, 0,
                                            0,  0,     0,    0, 0, 0, 0,  0,     0,    0, 0, 0,
                                            0,  0,     0,    0, 0, 0, 0,  0,     0,    0, 0, 0,
                                            0,  0,     0,    0, 0, 0, 0,  0,     0,    0, 0, 0);
        }
        
                [Test]
        public void CanCanCreateGlobalStiffnessMatrixForSpringAlignedToGlobalYZPlaneQuadrant3()
        {
            SUT = this.CreateSpringFromOriginTo(0, -1, -1);
            
            SUT.PrepareAndGenerateLocalStiffnessMatrix();
            this.Assert12x12StiffnessMatrix(0,  0,     0,    0, 0, 0, 0,  0,     0,    0, 0, 0,
                                            0,  0.25,  0.25, 0, 0, 0, 0, -0.25, -0.25, 0, 0, 0,
                                            0,  0.25,  0.25, 0, 0, 0, 0, -0.25, -0.25, 0, 0, 0,
                                            0,  0,     0,    0, 0, 0, 0,  0,     0,    0, 0, 0,
                                            0,  0,     0,    0, 0, 0, 0,  0,     0,    0, 0, 0,
                                            0,  0,     0,    0, 0, 0, 0,  0,     0,    0, 0, 0,
                                            0,  0,     0,    0, 0, 0, 0,  0,     0,    0, 0, 0,
                                            0, -0.25, -0.25, 0, 0, 0, 0,  0.25,  0.25, 0, 0, 0,
                                            0, -0.25, -0.25, 0, 0, 0, 0,  0.25,  0.25, 0, 0, 0,
                                            0,  0,     0,    0, 0, 0, 0,  0,     0,    0, 0, 0,
                                            0,  0,     0,    0, 0, 0, 0,  0,     0,    0, 0, 0,
                                            0,  0,     0,    0, 0, 0, 0,  0,     0,    0, 0, 0);
        }
        
        [Test]
        public void CanCanCreateGlobalStiffnessMatrixForSpringInQuadrant1PositiveZ()
        {
            SUT = this.CreateSpringFromOriginTo(1, 1, 1);
            
            SUT.PrepareAndGenerateLocalStiffnessMatrix();
            
            double a = 1.0 / 9.0;
            this.Assert12x12StiffnessMatrix( a,  a,  a, 0, 0, 0, -a, -a, -a, 0, 0, 0,
                                             a,  a,  a, 0, 0, 0, -a, -a, -a, 0, 0, 0,
                                             a,  a,  a, 0, 0, 0, -a, -a, -a, 0, 0, 0,
                                             0,  0,  0, 0, 0, 0,  0,  0,  0, 0, 0, 0,
                                             0,  0,  0, 0, 0, 0,  0,  0,  0, 0, 0, 0,
                                             0,  0,  0, 0, 0, 0,  0,  0,  0, 0, 0, 0,
                                            -a, -a, -a, 0, 0, 0,  a,  a,  a, 0, 0, 0,
                                            -a, -a, -a, 0, 0, 0,  a,  a,  a, 0, 0, 0,
                                            -a, -a, -a, 0, 0, 0,  a,  a,  a, 0, 0, 0,
                                             0,  0,  0, 0, 0, 0,  0,  0,  0, 0, 0, 0,
                                             0,  0,  0, 0, 0, 0,  0,  0,  0, 0, 0, 0,
                                             0,  0,  0, 0, 0, 0,  0,  0,  0, 0, 0, 0);
        }
        
        [Test]
        public void CanCanCreateGlobalStiffnessMatrixForSpringInQuadrant2PositiveZ()
        {
            SUT = this.CreateSpringFromOriginTo(-1, 1, 1);
            
            SUT.PrepareAndGenerateLocalStiffnessMatrix();
            
            double a = 1.0 / 9.0;
            this.Assert12x12StiffnessMatrix( a, -a, -a, 0, 0, 0, -a,  a,  a, 0, 0, 0,
                                            -a,  a,  a, 0, 0, 0,  a, -a, -a, 0, 0, 0,
                                            -a,  a,  a, 0, 0, 0,  a, -a, -a, 0, 0, 0,
                                             0,  0,  0, 0, 0, 0,  0,  0,  0, 0, 0, 0,
                                             0,  0,  0, 0, 0, 0,  0,  0,  0, 0, 0, 0,
                                             0,  0,  0, 0, 0, 0,  0,  0,  0, 0, 0, 0,
                                            -a,  a,  a, 0, 0, 0,  a, -a, -a, 0, 0, 0,
                                             a, -a, -a, 0, 0, 0, -a,  a,  a, 0, 0, 0,
                                             a, -a, -a, 0, 0, 0, -a,  a,  a, 0, 0, 0,
                                             0,  0,  0, 0, 0, 0,  0,  0,  0, 0, 0, 0,
                                             0,  0,  0, 0, 0, 0,  0,  0,  0, 0, 0, 0,
                                             0,  0,  0, 0, 0, 0,  0,  0,  0, 0, 0, 0);
        }
        
        [Test]
        public void CanCanCreateGlobalStiffnessMatrixForSpringInQuadrant3PositiveZ()
        {
            SUT = this.CreateSpringFromOriginTo(-1, -1, 1);
            
            SUT.PrepareAndGenerateLocalStiffnessMatrix();
            
            double a = 1.0 / 9.0;
            this.Assert12x12StiffnessMatrix( a,  a, -a, 0, 0, 0, -a, -a,  a, 0, 0, 0,
                                             a,  a, -a, 0, 0, 0, -a, -a,  a, 0, 0, 0,
                                            -a, -a,  a, 0, 0, 0,  a,  a, -a, 0, 0, 0,
                                             0,  0,  0, 0, 0, 0,  0,  0,  0, 0, 0, 0,
                                             0,  0,  0, 0, 0, 0,  0,  0,  0, 0, 0, 0,
                                             0,  0,  0, 0, 0, 0,  0,  0,  0, 0, 0, 0,
                                            -a, -a,  a, 0, 0, 0,  a,  a, -a, 0, 0, 0,
                                            -a, -a,  a, 0, 0, 0,  a,  a, -a, 0, 0, 0,
                                             a,  a, -a, 0, 0, 0, -a, -a,  a, 0, 0, 0,
                                             0,  0,  0, 0, 0, 0,  0,  0,  0, 0, 0, 0,
                                             0,  0,  0, 0, 0, 0,  0,  0,  0, 0, 0, 0,
                                             0,  0,  0, 0, 0, 0,  0,  0,  0, 0, 0, 0);
        }
        
        [Test]
        public void CanCanCreateGlobalStiffnessMatrixForSpringInQuadrant4PositiveZ()
        {
            SUT = this.CreateSpringFromOriginTo(1, -1, 1);
            
            SUT.PrepareAndGenerateLocalStiffnessMatrix();
            
            double a = 1.0 / 9.0;
            this.Assert12x12StiffnessMatrix( a, -a,  a, 0, 0, 0, -a,  a, -a, 0, 0, 0,
                                            -a,  a, -a, 0, 0, 0,  a, -a,  a, 0, 0, 0,
                                             a, -a,  a, 0, 0, 0, -a,  a, -a, 0, 0, 0,
                                             0,  0,  0, 0, 0, 0,  0,  0,  0, 0, 0, 0,
                                             0,  0,  0, 0, 0, 0,  0,  0,  0, 0, 0, 0,
                                             0,  0,  0, 0, 0, 0,  0,  0,  0, 0, 0, 0,
                                            -a,  a, -a, 0, 0, 0,  a, -a,  a, 0, 0, 0,
                                             a, -a,  a, 0, 0, 0, -a,  a, -a, 0, 0, 0,
                                            -a,  a, -a, 0, 0, 0,  a, -a,  a, 0, 0, 0,
                                             0,  0,  0, 0, 0, 0,  0,  0,  0, 0, 0, 0,
                                             0,  0,  0, 0, 0, 0,  0,  0,  0, 0, 0, 0,
                                             0,  0,  0, 0, 0, 0,  0,  0,  0, 0, 0, 0);
        }
        
        [Test]
        public void CanCanCreateGlobalStiffnessMatrixForSpringInQuadrant1NegativeZ()
        {
            SUT = this.CreateSpringFromOriginTo(1, 1, -1);
            
            SUT.PrepareAndGenerateLocalStiffnessMatrix();
            
            double a = 1.0 / 9.0;
            this.Assert12x12StiffnessMatrix( a,  a, -a, 0, 0, 0, -a, -a,  a, 0, 0, 0,
                                             a,  a, -a, 0, 0, 0, -a, -a,  a, 0, 0, 0,
                                            -a, -a,  a, 0, 0, 0,  a,  a, -a, 0, 0, 0,
                                             0,  0,  0, 0, 0, 0,  0,  0,  0, 0, 0, 0,
                                             0,  0,  0, 0, 0, 0,  0,  0,  0, 0, 0, 0,
                                             0,  0,  0, 0, 0, 0,  0,  0,  0, 0, 0, 0,
                                            -a, -a,  a, 0, 0, 0,  a,  a, -a, 0, 0, 0,
                                            -a, -a,  a, 0, 0, 0,  a,  a, -a, 0, 0, 0,
                                             a,  a, -a, 0, 0, 0, -a, -a,  a, 0, 0, 0,
                                             0,  0,  0, 0, 0, 0,  0,  0,  0, 0, 0, 0,
                                             0,  0,  0, 0, 0, 0,  0,  0,  0, 0, 0, 0,
                                             0,  0,  0, 0, 0, 0,  0,  0,  0, 0, 0, 0);
        }
        
        [Test]
        public void CanCanCreateGlobalStiffnessMatrixForSpringInQuadrant2NegativeZ()
        {
            SUT = this.CreateSpringFromOriginTo(-1, 1, -1);
            
            SUT.PrepareAndGenerateLocalStiffnessMatrix();
            
            double a = 1.0 / 9.0;
            this.Assert12x12StiffnessMatrix( a, -a,  a, 0, 0, 0, -a,  a, -a, 0, 0, 0,
                                            -a,  a, -a, 0, 0, 0,  a, -a,  a, 0, 0, 0,
                                             a, -a,  a, 0, 0, 0, -a,  a, -a, 0, 0, 0,
                                             0,  0,  0, 0, 0, 0,  0,  0,  0, 0, 0, 0,
                                             0,  0,  0, 0, 0, 0,  0,  0,  0, 0, 0, 0,
                                             0,  0,  0, 0, 0, 0,  0,  0,  0, 0, 0, 0,
                                            -a,  a, -a, 0, 0, 0,  a, -a,  a, 0, 0, 0,
                                             a, -a,  a, 0, 0, 0, -a,  a, -a, 0, 0, 0,
                                            -a,  a, -a, 0, 0, 0,  a, -a,  a, 0, 0, 0,
                                             0,  0,  0, 0, 0, 0,  0,  0,  0, 0, 0, 0,
                                             0,  0,  0, 0, 0, 0,  0,  0,  0, 0, 0, 0,
                                             0,  0,  0, 0, 0, 0,  0,  0,  0, 0, 0, 0);
        }
        
        [Test]
        public void CanCanCreateGlobalStiffnessMatrixForSpringInQuadrant3NegativeZ()
        {
            SUT = this.CreateSpringFromOriginTo(-1, -1, -1);
            
            SUT.PrepareAndGenerateLocalStiffnessMatrix();
            
            double a = 1.0 / 9.0;
            this.Assert12x12StiffnessMatrix( a,  a,  a, 0, 0, 0, -a, -a, -a, 0, 0, 0,
                                             a,  a,  a, 0, 0, 0, -a, -a, -a, 0, 0, 0,
                                             a,  a,  a, 0, 0, 0, -a, -a, -a, 0, 0, 0,
                                             0,  0,  0, 0, 0, 0,  0,  0,  0, 0, 0, 0,
                                             0,  0,  0, 0, 0, 0,  0,  0,  0, 0, 0, 0,
                                             0,  0,  0, 0, 0, 0,  0,  0,  0, 0, 0, 0,
                                            -a, -a, -a, 0, 0, 0,  a,  a,  a, 0, 0, 0,
                                            -a, -a, -a, 0, 0, 0,  a,  a,  a, 0, 0, 0,
                                            -a, -a, -a, 0, 0, 0,  a,  a,  a, 0, 0, 0,
                                             0,  0,  0, 0, 0, 0,  0,  0,  0, 0, 0, 0,
                                             0,  0,  0, 0, 0, 0,  0,  0,  0, 0, 0, 0,
                                             0,  0,  0, 0, 0, 0,  0,  0,  0, 0, 0, 0);
        }
        
        [Test]
        public void CanCanCreateGlobalStiffnessMatrixForSpringInQuadrant4NegativeZ()
        {
            SUT = this.CreateSpringFromOriginTo(1, -1, -1);
            
            SUT.PrepareAndGenerateLocalStiffnessMatrix();
            
            double a = 1.0 / 9.0;
            this.Assert12x12StiffnessMatrix( a, -a, -a, 0, 0, 0, -a,  a,  a, 0, 0, 0,
                                            -a,  a,  a, 0, 0, 0,  a, -a, -a, 0, 0, 0,
                                            -a,  a,  a, 0, 0, 0,  a, -a, -a, 0, 0, 0,
                                             0,  0,  0, 0, 0, 0,  0,  0,  0, 0, 0, 0,
                                             0,  0,  0, 0, 0, 0,  0,  0,  0, 0, 0, 0,
                                             0,  0,  0, 0, 0, 0,  0,  0,  0, 0, 0, 0,
                                            -a,  a,  a, 0, 0, 0,  a, -a, -a, 0, 0, 0,
                                             a, -a, -a, 0, 0, 0, -a,  a,  a, 0, 0, 0,
                                             a, -a, -a, 0, 0, 0, -a,  a,  a, 0, 0, 0,
                                             0,  0,  0, 0, 0, 0,  0,  0,  0, 0, 0, 0,
                                             0,  0,  0, 0, 0, 0,  0,  0,  0, 0, 0, 0,
                                             0,  0,  0, 0, 0, 0,  0,  0,  0, 0, 0, 0);
        }
        #endregion
        
        [Test]
        public void CanGetStiffnessAt()
        {
            double result = SUT.GetStiffnessAt(start, DegreeOfFreedom.X, start, DegreeOfFreedom.X);
            Assert.AreEqual(1.0 / 9.0, result);
            result = SUT.GetStiffnessAt(start, DegreeOfFreedom.X, end, DegreeOfFreedom.X);
            Assert.AreEqual(-1.0 / 9.0, result);
        }
    }
}
