//-----------------------------------------------------------------------
// <copyright file="PointTest.cs" company="Iain Sproat">
//     Copyright Iain Sproat, 2013.
// </copyright>
//-----------------------------------------------------------------------
using System;
using NUnit.Framework;
using SharpFE.Geometry;

namespace SharpFE.Core.Tests.Geometry
{
    [TestFixture]
    public class PointTest
    {
        [Test]
        public void Can_be_constructed()
        {
            CartesianPoint SUT = new CartesianPoint(3, 4, 5);
            Assert.AreEqual(3, SUT.X);
            Assert.AreEqual(4, SUT.Y);
            Assert.AreEqual(5, SUT.Z);
            
            Assert.AreEqual(3, SUT[DegreeOfFreedom.X]);
            Assert.AreEqual(4, SUT[DegreeOfFreedom.Y]);
            Assert.AreEqual(5, SUT[DegreeOfFreedom.Z]);
        }
        
        [Test]
        public void Can_determine_if_equal()
        {
            CartesianPoint SUT = new CartesianPoint(3, 4, 5);
            CartesianPoint equal = new CartesianPoint(3, 4, 5);
            CartesianPoint unequal = new CartesianPoint(4, 5, 6);
            
            Assert.IsTrue(SUT.Equals(equal));
            Assert.IsFalse(SUT.Equals(unequal));
        }
        
        [Test]
        public void Can_calculate_vector_to_another_point()
        {
            CartesianPoint SUT = new CartesianPoint(3, 4, 5);
            CartesianPoint other = new CartesianPoint(5, 4, 3);
            
            GeometricVector result = SUT.VectorTo(other);
            Assert.AreEqual(2, result.X);
            Assert.AreEqual(0, result.Y);
            Assert.AreEqual(-2, result.Z);
        }
        
        [Test]
        public void Can_subtract_another_point()
        {
            CartesianPoint SUT = new CartesianPoint(3, 4, 5);
            CartesianPoint other = new CartesianPoint(5, 4, 3);
            
            GeometricVector result = SUT.Subtract(other);
            Assert.AreEqual(-2, result.X);
            Assert.AreEqual(0, result.Y);
            Assert.AreEqual(2, result.Z);
        }
        
        [Test]
        public void Can_add_a_vector()
        {
            CartesianPoint SUT = new CartesianPoint(3, 4, 5);
            GeometricVector other = new GeometricVector(5, 4, -3);
            
            CartesianPoint result = SUT.Add(other);
            Assert.AreEqual(8, result.X);
            Assert.AreEqual(8, result.Y);
            Assert.AreEqual(2, result.Z);
        }
    }
}
