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

namespace SharpFE.Tests.Elements
{
    [TestFixture]
    public class SpringElementTest : SpringElementTestBase
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
            Assert.AreEqual(2, SUT.Stiffness);
        }
        
        [Test]
        public void HasALocalStiffnessMatrix()
        {
            SUT.PrepareAndGenerateLocalStiffnessMatrix();
            Matrix result = SUT.LocalStiffnessMatrix;
            Helpers.AssertMatrix(result, 12, 12,
                                 2, 0, 0, 0, 0, 0, -2, 0, 0, 0, 0, 0,
                                 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                 -2, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0,
                                 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        }
    }
}
