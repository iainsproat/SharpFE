/*
 * Created by Iain Sproat
 * Date: 25/09/2012
 * Time: 20:23
 * 
 */
using System;
using NUnit.Framework;
using SharpFE;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra.Double;

namespace SharpFE.Core.Tests.Elements
{
    [TestFixture]
    public class ConstantLinearSpringElementTest : ConstantLinearSpringElementTestBase
    {
        [SetUp]
        public void Setup()
        {
            base.SetUp();
        }
        
        [Test]
        public void CanCreateSpringElement()
        {
            Assert.IsNotNull(SUT);
        }
        
        [Test]
        public void NodesCanBeFoundAtEachEndOfElement()
        {
            Assert.IsNotNull(SUT.StartNode);
            Assert.IsNotNull(SUT.EndNode);
        }
        
        [Test]
        public void CanGetAllNodesOfElement()
        {
            IList<FiniteElementNode> result = SUT.Nodes;
            
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(start, result[0]);
            Assert.AreEqual(end, result[1]);
        }
        
        [Test]
        public void ElementHasASpringConstant()
        {
            Assert.AreEqual(2, SUT.SpringConstant);
        }
        
        #region Global Stiffness Matrix
        [Test]
        public void CanCanCreateGlobalStiffnessMatrixForSpringAlignedToGlobalXAxis()
        {
            SUT = this.CreateSpringFromOriginTo(1, 0);
            
            SUT.PrepareAndGenerateGlobalStiffnessMatrix();
            
            Helpers.AssertMatrix(SUT.BuildStiffnessRotationMatrixFromLocalToGlobalCoordinates(), 12, 12,
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
            
            SUT.PrepareAndGenerateGlobalStiffnessMatrix();
            
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
            
            SUT.PrepareAndGenerateGlobalStiffnessMatrix();
            
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
            
            SUT.PrepareAndGenerateGlobalStiffnessMatrix();
            
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
            
            SUT.PrepareAndGenerateGlobalStiffnessMatrix();
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
            
            SUT.PrepareAndGenerateGlobalStiffnessMatrix();
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
            SUT.PrepareAndGenerateGlobalStiffnessMatrix();
            this.Assert12x12StiffnessMatrix( 0.5,  0.5, 0, 0, 0, 0,  -0.5, -0.5, 0, 0, 0, 0,
                                             0.5,  0.5, 0, 0, 0, 0,  -0.5, -0.5, 0, 0, 0, 0,
                                             0,    0,   0, 0, 0, 0,   0,    0,   0, 0, 0, 0,
                                             0,    0,   0, 0, 0, 0,   0,    0,   0, 0, 0, 0,
                                             0,    0,   0, 0, 0, 0,   0,    0,   0, 0, 0, 0,
                                             0,    0,   0, 0, 0, 0,   0,    0,   0, 0, 0, 0,
                                            -0.5, -0.5, 0, 0, 0, 0,   0.5,  0.5, 0, 0, 0, 0,
                                            -0.5, -0.5, 0, 0, 0, 0,   0.5,  0.5, 0, 0, 0, 0,
                                             0,    0,   0, 0, 0, 0,   0,    0,   0, 0, 0, 0,
                                             0,    0,   0, 0, 0, 0,   0,    0,   0, 0, 0, 0,
                                             0,    0,   0, 0, 0, 0,   0,    0,   0, 0, 0, 0,
                                             0,    0,   0, 0, 0, 0,   0,    0,   0, 0, 0, 0);
        }
        
        [Test]
        public void CanCanCreateGlobalStiffnessMatrixForSpringAlignedToGlobalXYPlaneQuadrant3()
        {
            SUT = this.CreateSpringFromOriginTo(-1, -1, 0);
            SUT.PrepareAndGenerateGlobalStiffnessMatrix();
            this.Assert12x12StiffnessMatrix( 0.5,  0.5, 0, 0, 0, 0,  -0.5, -0.5, 0, 0, 0, 0,
                                             0.5,  0.5, 0, 0, 0, 0,  -0.5, -0.5, 0, 0, 0, 0,
                                             0,    0,   0, 0, 0, 0,   0,    0,   0, 0, 0, 0,
                                             0,    0,   0, 0, 0, 0,   0,    0,   0, 0, 0, 0,
                                             0,    0,   0, 0, 0, 0,   0,    0,   0, 0, 0, 0,
                                             0,    0,   0, 0, 0, 0,   0,    0,   0, 0, 0, 0,
                                            -0.5, -0.5, 0, 0, 0, 0,   0.5,  0.5, 0, 0, 0, 0,
                                            -0.5, -0.5, 0, 0, 0, 0,   0.5,  0.5, 0, 0, 0, 0,
                                             0,    0,   0, 0, 0, 0,   0,    0,   0, 0, 0, 0,
                                             0,    0,   0, 0, 0, 0,   0,    0,   0, 0, 0, 0,
                                             0,    0,   0, 0, 0, 0,   0,    0,   0, 0, 0, 0,
                                             0,    0,   0, 0, 0, 0,   0,    0,   0, 0, 0, 0);
        }
        
        [Test]
        public void CanCanCreateGlobalStiffnessMatrixForSpringAlignedToGlobalXZPlaneQuadrant1()
        {
            SUT = this.CreateSpringFromOriginTo(1, 0, 1);
            
            SUT.PrepareAndGenerateGlobalStiffnessMatrix();
            this.Assert12x12StiffnessMatrix( 0.5, 0,  0.5, 0, 0, 0, -0.5, 0,   -0.5, 0, 0, 0,
                                             0,   0,  0,   0, 0, 0,  0,   0,    0,   0, 0, 0,
                                             0.5, 0,  0.5, 0, 0, 0, -0.5, 0,   -0.5, 0, 0, 0,
                                             0,   0,  0,   0, 0, 0,  0,   0,    0,   0, 0, 0,
                                             0,   0,  0,   0, 0, 0,  0,   0,    0,   0, 0, 0,
                                             0,   0,  0,   0, 0, 0,  0,   0,    0,   0, 0, 0,
                                            -0.5, 0, -0.5, 0, 0, 0,  0.5, 0,    0.5, 0, 0, 0,
                                             0,   0,  0,   0, 0, 0,  0,   0,    0,   0, 0, 0,
                                            -0.5, 0, -0.5, 0, 0, 0,  0.5, 0,    0.5, 0, 0, 0,
                                             0,   0,  0,   0, 0, 0,  0,   0,    0,   0, 0, 0,
                                             0,   0,  0,   0, 0, 0,  0,   0,    0,   0, 0, 0,
                                             0,   0,  0,   0, 0, 0,  0,   0,    0,   0, 0, 0);
        }
        
        [Test]
        public void CanCanCreateGlobalStiffnessMatrixForSpringAlignedToGlobalXZPlaneQuadrant3()
        {
            SUT = this.CreateSpringFromOriginTo(-1, 0, -1);
            
            SUT.PrepareAndGenerateGlobalStiffnessMatrix();
            this.Assert12x12StiffnessMatrix( 0.5, 0,  0.5, 0, 0, 0, -0.5, 0,   -0.5, 0, 0, 0,
                                             0,   0,  0,   0, 0, 0,  0,   0,    0,   0, 0, 0,
                                             0.5, 0,  0.5, 0, 0, 0, -0.5, 0,   -0.5, 0, 0, 0,
                                             0,   0,  0,   0, 0, 0,  0,   0,    0,   0, 0, 0,
                                             0,   0,  0,   0, 0, 0,  0,   0,    0,   0, 0, 0,
                                             0,   0,  0,   0, 0, 0,  0,   0,    0,   0, 0, 0,
                                            -0.5, 0, -0.5, 0, 0, 0,  0.5, 0,    0.5, 0, 0, 0,
                                             0,   0,  0,   0, 0, 0,  0,   0,    0,   0, 0, 0,
                                            -0.5, 0, -0.5, 0, 0, 0,  0.5, 0,    0.5, 0, 0, 0,
                                             0,   0,  0,   0, 0, 0,  0,   0,    0,   0, 0, 0,
                                             0,   0,  0,   0, 0, 0,  0,   0,    0,   0, 0, 0,
                                             0,   0,  0,   0, 0, 0,  0,   0,    0,   0, 0, 0);
        }
        
        [Test]
        public void CanCanCreateGlobalStiffnessMatrixForSpringAlignedToGlobalYZPlaneQuadrant1()
        {
            SUT = this.CreateSpringFromOriginTo(0, 1, 1);
            
            SUT.PrepareAndGenerateGlobalStiffnessMatrix();
            this.Assert12x12StiffnessMatrix(0,  0,    0,   0, 0, 0, 0,  0,    0,   0, 0, 0,
                                            0,  0.5,  0.5, 0, 0, 0, 0, -0.5, -0.5, 0, 0, 0,
                                            0,  0.5,  0.5, 0, 0, 0, 0, -0.5, -0.5, 0, 0, 0,
                                            0,  0,    0,   0, 0, 0, 0,  0,    0,   0, 0, 0,
                                            0,  0,    0,   0, 0, 0, 0,  0,    0,   0, 0, 0,
                                            0,  0,    0,   0, 0, 0, 0,  0,    0,   0, 0, 0,
                                            0,  0,    0,   0, 0, 0, 0,  0,    0,   0, 0, 0,
                                            0, -0.5, -0.5, 0, 0, 0, 0,  0.5,  0.5, 0, 0, 0,
                                            0, -0.5, -0.5, 0, 0, 0, 0,  0.5,  0.5, 0, 0, 0,
                                            0,  0,    0,   0, 0, 0, 0,  0,    0,   0, 0, 0,
                                            0,  0,    0,   0, 0, 0, 0,  0,    0,   0, 0, 0,
                                            0,  0,    0,   0, 0, 0, 0,  0,    0,   0, 0, 0);
        }
        
                [Test]
        public void CanCanCreateGlobalStiffnessMatrixForSpringAlignedToGlobalYZPlaneQuadrant3()
        {
            SUT = this.CreateSpringFromOriginTo(0, -1, -1);
            
            SUT.PrepareAndGenerateGlobalStiffnessMatrix();
            this.Assert12x12StiffnessMatrix(0,  0,    0,   0, 0, 0, 0,  0,    0,   0, 0, 0,
                                            0,  0.5,  0.5, 0, 0, 0, 0, -0.5, -0.5, 0, 0, 0,
                                            0,  0.5,  0.5, 0, 0, 0, 0, -0.5, -0.5, 0, 0, 0,
                                            0,  0,    0,   0, 0, 0, 0,  0,    0,   0, 0, 0,
                                            0,  0,    0,   0, 0, 0, 0,  0,    0,   0, 0, 0,
                                            0,  0,    0,   0, 0, 0, 0,  0,    0,   0, 0, 0,
                                            0,  0,    0,   0, 0, 0, 0,  0,    0,   0, 0, 0,
                                            0, -0.5, -0.5, 0, 0, 0, 0,  0.5,  0.5, 0, 0, 0,
                                            0, -0.5, -0.5, 0, 0, 0, 0,  0.5,  0.5, 0, 0, 0,
                                            0,  0,    0,   0, 0, 0, 0,  0,    0,   0, 0, 0,
                                            0,  0,    0,   0, 0, 0, 0,  0,    0,   0, 0, 0,
                                            0,  0,    0,   0, 0, 0, 0,  0,    0,   0, 0, 0);
        }
        
        [Test]
        public void CanCanCreateGlobalStiffnessMatrixForSpringInQuadrant1PositiveZ()
        {
            SUT = this.CreateSpringFromOriginTo(1, 1, 1);
            
            SUT.PrepareAndGenerateGlobalStiffnessMatrix();
            
            double a = 1.0 / 3.0;
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
            
            SUT.PrepareAndGenerateGlobalStiffnessMatrix();
            
            double a = 1.0 / 3.0;
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
            
            SUT.PrepareAndGenerateGlobalStiffnessMatrix();
            
            double a = 1.0 / 3.0;
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
            
            SUT.PrepareAndGenerateGlobalStiffnessMatrix();
            
            double a = 1.0 / 3.0;
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
            
            SUT.PrepareAndGenerateGlobalStiffnessMatrix();
            
            double a = 1.0 / 3.0;
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
            
            SUT.PrepareAndGenerateGlobalStiffnessMatrix();
            
            double a = 1.0 / 3.0;
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
            
            SUT.PrepareAndGenerateGlobalStiffnessMatrix();
            
            double a = 1.0 / 3.0;
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
            
            SUT.PrepareAndGenerateGlobalStiffnessMatrix();
            
            double a = 1.0 / 3.0;
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
            Assert.AreEqual(2, result);
            result = SUT.GetStiffnessAt(start, DegreeOfFreedom.X, end, DegreeOfFreedom.X);
            Assert.AreEqual(-2, result);
        }
        

    }
}
