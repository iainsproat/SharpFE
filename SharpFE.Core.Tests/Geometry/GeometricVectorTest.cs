//-----------------------------------------------------------------------
// <copyright file="?.cs" company="Iain Sproat">
//     Copyright Iain Sproat, 2013.
// </copyright>
//-----------------------------------------------------------------------
using System;
using NUnit.Framework;
using SharpFE.Geometry;

namespace SharpFE.Core.Tests.Geometry
{
    [TestFixture]
    public class GeometricVectorTest
    {
        [Test]
        public void Can_be_constructed()
        {
            GeometricVector SUT = new GeometricVector(3, 4, 5);
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
            GeometricVector SUT = new GeometricVector(3, 4, 5);
            GeometricVector equal = new GeometricVector(3, 4, 5);
            GeometricVector unequal = new GeometricVector(4, 5, 6);
            
            Assert.IsTrue(SUT.Equals(equal));
            Assert.IsFalse(SUT.Equals(unequal));
        }
    }
}
