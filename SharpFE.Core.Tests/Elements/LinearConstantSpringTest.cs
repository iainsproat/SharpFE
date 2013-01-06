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
    public class LinearConstantSpringTest
    {
    	private NodeFactory nodeFactory;
        private ElementFactory elementFactory;
        private FiniteElementNode start;
        private FiniteElementNode end;
        private LinearConstantSpring SUT;
        
        [SetUp]
        public void SetUp()
        {
            nodeFactory = new NodeFactory(ModelType.Truss1D);
            start = nodeFactory.Create(0);
            end = nodeFactory.Create(1);
            elementFactory = new ElementFactory();
            SUT = elementFactory.CreateLinearConstantSpring(start, end, 2);
        }
        
        [Test]
        public void CanCreateSpringElement()
        {
            Assert.IsNotNull(SUT);
        }
        
        [Test]
        public void Has_initialized_all_supported_degrees_of_freedom()
        {
        	IList<NodalDegreeOfFreedom> result = SUT.SupportedNodalDegreeOfFreedoms;
        	Assert.IsNotNull(result);
        	Assert.AreEqual(12, result.Count);
        }
        
        [Test]
        public void ElementHasASpringConstant()
        {
            Assert.AreEqual(2, SUT.SpringConstant);
        }
        
        [Test]
        public void Can_determine_if_equal()
        {
        	Assert.IsTrue(SUT.Equals(SUT));
        	
        	LinearConstantSpring equal = elementFactory.CreateLinearConstantSpring(start, end, 2);
        	Assert.IsTrue(SUT.Equals(equal));
        	
        	LinearConstantSpring notEqual = elementFactory.CreateLinearConstantSpring(start, end, 4);
        	Assert.IsFalse(SUT.Equals(notEqual));
        }
        
        [Test]
        public void HashCode_changes_with_stiffness_value()
        {
        	int SUTOriginalHash = SUT.GetHashCode();
        	
        	LinearConstantSpring equal = elementFactory.CreateLinearConstantSpring(start, end, 2);
        	Assert.AreEqual(SUTOriginalHash, equal.GetHashCode());
        	
        	LinearConstantSpring unequal = elementFactory.CreateLinearConstantSpring(start, end, 4);
        	Assert.AreNotEqual(SUTOriginalHash, unequal.GetHashCode());
        }
    }
}
