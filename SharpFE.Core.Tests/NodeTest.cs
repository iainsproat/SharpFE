/*
 * Created by Iain Sproat
 * Date: 25/09/2012
 * Time: 17:38
 * 
 */
using System;
using NUnit.Framework;
using SharpFE;

namespace SharpFE.Core.Tests
{
    [TestFixture]
    public class NodeTest
    {
        NodeFactory factory;
        
        [SetUp]
        public void Setup()
        {
            factory = new NodeFactory(ModelType.Truss1D);
        }
        
        [Test]
        public void NodesHaveAnOriginalGlobalLocation()
        {
            FiniteElementNode SUT = factory.Create(0);
            Assert.AreEqual(0, SUT.X);
            Assert.AreEqual(0, SUT.Y);
            
            SUT = factory.Create(100);
            Assert.AreEqual(100, SUT.X);
        }
        
        [Test]
        public void NodesAreEqualIfTheirCoordinatesAreEqual()
        {
            factory = new NodeFactory(ModelType.Truss3D);
            FiniteElementNode SUT = factory.Create(0, 0, 0);
            
            FiniteElementNode equalToSUT = factory.Create(0, 0, 0);
            Assert.IsTrue(SUT.Equals(equalToSUT));
            Assert.IsTrue(SUT == equalToSUT);
            Assert.IsFalse(SUT != equalToSUT);
            
            FiniteElementNode notEqualToSUTByX = factory.Create(1, 0, 0);
            Assert.IsFalse(SUT.Equals(notEqualToSUTByX));
            Assert.IsFalse(SUT == notEqualToSUTByX);
            Assert.IsTrue(SUT != notEqualToSUTByX);
            
            FiniteElementNode notEqualToSUTByY = factory.Create(0, 1, 0);
            Assert.IsFalse(SUT.Equals(notEqualToSUTByY));
            Assert.IsFalse(SUT == notEqualToSUTByY);
            Assert.IsTrue(SUT != notEqualToSUTByY);
            
            FiniteElementNode notEqualToSUTByZ = factory.Create(0, 0, 1);
            Assert.IsFalse(SUT.Equals(notEqualToSUTByZ));
            Assert.IsFalse(SUT == notEqualToSUTByZ);
            Assert.IsTrue(SUT != notEqualToSUTByZ);
        }
        
        [Test]
        public void NodesCanBeDeactivated()
        {
            Assert.Ignore();
        }
    }
}
