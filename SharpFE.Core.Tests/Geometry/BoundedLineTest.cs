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
    public class BoundedLineTest
    {
        GeometricVector vectorOfLine;
        Point startPoint;
        BoundedLine SUT;
        
        [SetUp]
        public void SetUp()
        {
            vectorOfLine = new GeometricVector(2, 3, 4);
            startPoint = new Point(3, 4, 5);
            SUT = new BoundedLine(startPoint, vectorOfLine);
        }
        
        [Test]
        public void Can_be_constructed_from_vector_and_point()
        {
            Assert.AreEqual(2, SUT.X);
            Assert.AreEqual(3, SUT.Y);
            Assert.AreEqual(4, SUT.Z);
            
            Assert.AreEqual(2, SUT.Vector.X);
            Assert.AreEqual(3, SUT.Vector.Y);
            Assert.AreEqual(4, SUT.Vector.Z);
            
            Assert.AreEqual(3, SUT.PointOnLine.X);
            Assert.AreEqual(4, SUT.PointOnLine.Y);
            Assert.AreEqual(5, SUT.PointOnLine.Z);
            
            Assert.AreEqual(3, SUT.Start.X);
            Assert.AreEqual(4, SUT.Start.Y);
            Assert.AreEqual(5, SUT.Start.Z);
            
            Assert.AreEqual(5, SUT.End.X);
            Assert.AreEqual(7, SUT.End.Y);
            Assert.AreEqual(9, SUT.End.Z);
        }
        
        [Test]
        public void Can_be_constructed_from_start_and_end_points()
        {
            SUT = new BoundedLine(startPoint, vectorOfLine);
            
            Assert.AreEqual(2, SUT.X);
            Assert.AreEqual(3, SUT.Y);
            Assert.AreEqual(4, SUT.Z);
            
            Assert.AreEqual(2, SUT.Vector.X);
            Assert.AreEqual(3, SUT.Vector.Y);
            Assert.AreEqual(4, SUT.Vector.Z);
            
            Assert.AreEqual(3, SUT.Start.X);
            Assert.AreEqual(4, SUT.Start.Y);
            Assert.AreEqual(5, SUT.Start.Z);
            
            Assert.AreEqual(5, SUT.End.X);
            Assert.AreEqual(7, SUT.End.Y);
            Assert.AreEqual(9, SUT.End.Z);
        }
        
        [Test]
        public void Can_determine_if_point_is_on_the_line()
        {
            vectorOfLine = new GeometricVector(2, 2, 2);
            startPoint = new Point(3, 4, 5);
            SUT = new BoundedLine(startPoint, vectorOfLine);
            
            Point pointOnTheLine = new Point(4, 5, 6);
            Point pointAtEndOfLine = new Point(5, 6, 7);
            Point pointAtStartOfLine = new Point(3, 4, 5);
            
            Point pointBeyondEndOfLine = new Point(6, 7, 8);
            Point pointBeyondStartOfLine = new Point(2, 3, 4);
            
            Assert.IsTrue(SUT.IsOnLine(pointOnTheLine));
            Assert.IsTrue(SUT.IsOnLine(pointAtEndOfLine));
            Assert.IsTrue(SUT.IsOnLine(pointAtStartOfLine));
            Assert.IsFalse(SUT.IsOnLine(pointBeyondEndOfLine));
            Assert.IsFalse(SUT.IsOnLine(pointBeyondStartOfLine));
        }
        
        [Test]
        public void Can_calculate_length()
        {
            double result = SUT.Length;
            Assert.AreEqual(5.38516, result, 0.00001);
        }
    }
}
