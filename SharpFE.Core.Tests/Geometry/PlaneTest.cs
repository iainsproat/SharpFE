//-----------------------------------------------------------------------
// <copyright file="PlaneTest.cs" company="Iain Sproat">
//     Copyright Iain Sproat, 2013.
// </copyright>
//-----------------------------------------------------------------------
using System;
using NUnit.Framework;
using SharpFE.Geometry;

namespace SharpFE.Core.Tests.Geometry
{
    [TestFixture]
    public class PlaneTest
    {
        [Test]
        public void Can_be_constructed()
        {
            GeometricVector planeNormal = new GeometricVector(0, 0, 1);
            CartesianPoint pointOnPlane = new CartesianPoint(0, 0, 0);
            Plane SUT = new Plane(planeNormal, pointOnPlane);
            Assert.AreEqual(planeNormal, SUT.Normal);
            Assert.AreEqual(pointOnPlane, SUT.Point);
        }
        
        [Test]
        public void Can_determine_if_point_is_in_plane()
        {
            GeometricVector planeNormal = new GeometricVector(0, 0, 1);
            CartesianPoint pointOnPlane = new CartesianPoint(0, 0, 0);
            Plane SUT = new Plane(planeNormal, pointOnPlane);
            
            CartesianPoint pointInPlane = new CartesianPoint(1, 1, 0);
            CartesianPoint pointOutOfPlane = new CartesianPoint(0, 0, 1);
            Assert.IsTrue(SUT.IsInPlane(pointInPlane));
            Assert.IsFalse(SUT.IsInPlane(pointOutOfPlane));
        }
    }
}
