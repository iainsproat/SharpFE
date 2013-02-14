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
    public class UnboundedLineTest
    {
        [Test]
        public void Can_be_created()
        {
            GeometricVector vectorOfLine = new GeometricVector(1, 2, 3);
            CartesianPoint pointOnLine = new CartesianPoint(3, 4, 5);
            UnboundedLine SUT = new UnboundedLine(vectorOfLine, pointOnLine);
            
            Assert.AreEqual(vectorOfLine, SUT.Vector);
            Assert.AreEqual(pointOnLine, SUT.PointOnLine);
            Assert.AreEqual(vectorOfLine.X, SUT.X);
            Assert.AreEqual(vectorOfLine.Y, SUT.Y);
            Assert.AreEqual(vectorOfLine.Z, SUT.Z);
        }
        
        [Test]
        public void Can_determine_if_point_is_on_line()
        {
            GeometricVector vectorOfLine = new GeometricVector(2, 2, 2);
            CartesianPoint pointOnLine = new CartesianPoint(3, 4, 5);
            UnboundedLine SUT = new UnboundedLine(vectorOfLine, pointOnLine);
            
            CartesianPoint aPointOnTheLine = new CartesianPoint(4, 5, 6);
            CartesianPoint aPointNotOnTheLine = new CartesianPoint(3, 5, 7);
            CartesianPoint aPointOnTheLineInNegativeDirectionFromDefinedPointOnLine = new CartesianPoint(2, 3, 4);
            Assert.IsTrue(SUT.IsOnLine(aPointOnTheLine));
            Assert.IsFalse(SUT.IsOnLine(aPointNotOnTheLine));
            Assert.IsTrue(SUT.IsOnLine(aPointOnTheLineInNegativeDirectionFromDefinedPointOnLine));
        }
        
        [Test]
        public void Can_calculate_perpendicular_line_to_a_point()
        {
            GeometricVector vectorOfLine = new GeometricVector(2, 2, 0);
            CartesianPoint pointOnLine = new CartesianPoint(3, 4, 5);
            UnboundedLine SUT = new UnboundedLine(vectorOfLine, pointOnLine);
            
            CartesianPoint perpendicularPoint = new CartesianPoint(3, 6, 5);
            BoundedLine result = SUT.PerpendicularLineTo(perpendicularPoint);
            Assert.AreEqual(-1, result.X);
            Assert.AreEqual(1, result.Y);
            Assert.AreEqual(0, result.Z);
            Assert.AreEqual(4, result.Start.X);
            Assert.AreEqual(5, result.Start.Y);
            Assert.AreEqual(5, result.Start.Z);
        }
    }
}
