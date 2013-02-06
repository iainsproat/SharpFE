//-----------------------------------------------------------------------
// <copyright file="?.cs" company="Iain Sproat">
//     Copyright Iain Sproat, 2013.
// </copyright>
//-----------------------------------------------------------------------
using System;
using NUnit.Framework;
using SharpFE.Geometry;

namespace SharpFE.Core.Tests.Elements
{
    [TestFixture]
    public class FiniteElementTest_LocalCoordinates
    {
        protected NodeFactory nodeFactory;
        protected ElementFactory elementFactory;
        protected FiniteElementNode start;
        protected FiniteElementNode end;
        protected FiniteElement SUT;
        
        #region Global to Local
        [Test]
        public void Can_convert_from_global_to_local_when_local_axes_aligned_with_global_and_point_at_origin()
        {
            CreateAndStore2DSpringBetween(0, 0, 1, 0);
            Assert.AreEqual(0, SUT.LocalOrigin.X);
            Assert.AreEqual(0, SUT.LocalOrigin.Z);
            Assert.AreEqual(1, SUT.LocalXAxis.X);
            
            Point pointInGlobalCoords = new Point(0, 0, 0);
            this.AssertConversionFromGlobalToLocalCoordinates(pointInGlobalCoords, 0, 0, 0);
        }
        
        [Test]
        public void Can_convert_from_global_to_local_when_local_axes_aligned_with_global()
        {
            CreateAndStore2DSpringBetween(0, 0, 1, 0);
            
            Point pointInGlobalCoords = new Point(2, 0, 0);
            this.AssertConversionFromGlobalToLocalCoordinates(pointInGlobalCoords, 2, 0, 0);
        }
        
        [Test]
        public void Can_convert_from_global_to_local_when_local_axes_aligned_with_global_but_origin_offset()
        {
            CreateAndStore2DSpringBetween(1, 0, 2, 0);
            
            Point pointInGlobalCoords = new Point(2, 0, 0);
            this.AssertConversionFromGlobalToLocalCoordinates(pointInGlobalCoords, 1, 0, 0);
        }
        
        [Test]
        public void Can_convert_from_global_to_local_when_local_axes_rotated()
        {
            CreateAndStore2DSpringBetween(0, 0, 1, 1);
            
            Point pointInGlobalCoords = new Point(0, 0, 0);
            this.AssertConversionFromGlobalToLocalCoordinates(pointInGlobalCoords, 0, 0, 0);
        }
        
        [Test]
        public void Can_convert_from_global_to_local_when_local_axes_rotated_and_origin_offset()
        {
            CreateAndStore2DSpringBetween(1, 1, 2, 2);
            
            Point pointInGlobalCoords = new Point(1, 0, 1);
            this.AssertConversionFromGlobalToLocalCoordinates(pointInGlobalCoords, 0, 0, 0);
        }
        
        [Test]
        public void Can_convert_from_global_to_local_when_local_axes_rotated_and_origin_offset_and_point_offset()
        {
            CreateAndStore2DSpringBetween(3, 4, 4, 5);
            Assert.AreEqual(3, SUT.LocalOrigin.X);
            Assert.AreEqual(4, SUT.LocalOrigin.Z);
            Assert.AreEqual(1, SUT.LocalXAxis.X);
            Assert.AreEqual(1, SUT.LocalXAxis.Z);
            
            Point pointInGlobalCoords = new Point(4, 0, 6);
            this.AssertConversionFromGlobalToLocalCoordinates(pointInGlobalCoords, 1.5 * Math.Sqrt(2), 0, 0.5 * Math.Sqrt(2));
        }
        #endregion
        
        #region Local to Global
        [Test]
        public void Can_convert_from_local_to_global_when_local_axes_aligned_with_global_and_point_at_origin()
        {
            CreateAndStore2DSpringBetween(0, 0, 1, 0);
            Assert.AreEqual(0, SUT.LocalOrigin.X);
            Assert.AreEqual(0, SUT.LocalOrigin.Z);
            Assert.AreEqual(1, SUT.LocalXAxis.X);
            
            Point pointInLocalCoords = new Point(0, 0, 0);
            this.AssertConversionFromLocalToGlobalCoordinates(pointInLocalCoords, 0, 0, 0);
        }
        
        [Test]
        public void Can_convert_from_local_to_global_when_local_axes_aligned_with_global()
        {
            CreateAndStore2DSpringBetween(0, 0, 1, 0);
            
            Point pointInLocalCoords = new Point(2, 0, 0);
            this.AssertConversionFromLocalToGlobalCoordinates(pointInLocalCoords, 2, 0, 0);
        }
        
        [Test]
        public void Can_convert_from_local_to_global_when_local_axes_aligned_with_global_but_origin_offset()
        {
            CreateAndStore2DSpringBetween(1, 0, 2, 0);
            
            Point pointInLocalCoords = new Point(1, 0, 0);
            this.AssertConversionFromLocalToGlobalCoordinates(pointInLocalCoords, 2, 0, 0);
        }
        
        [Test]
        public void Can_convert_from_local_to_global_when_local_axes_rotated()
        {
            CreateAndStore2DSpringBetween(0, 0, 1, 1);
            
            Point pointInLocalCoords = new Point(0, 0, 0);
            this.AssertConversionFromLocalToGlobalCoordinates(pointInLocalCoords, 0, 0, 0);
        }
        
        [Test]
        public void Can_convert_from_local_to_global_when_local_axes_rotated_and_origin_offset()
        {
            CreateAndStore2DSpringBetween(1, 1, 2, 2);
            
            Point pointInLocalCoords = new Point(0, 0, 0);
            this.AssertConversionFromLocalToGlobalCoordinates(pointInLocalCoords, 1, 0, 1);
        }
        
        [Test]
        public void Can_convert_from_local_to_global_when_local_axes_rotated_and_origin_offset_and_point_offset()
        {
            CreateAndStore2DSpringBetween(3, 4, 4, 5);
            
            Point pointInLocalCoords = new Point(1.5 * Math.Sqrt(2), 0, 0.5 * Math.Sqrt(2));
            this.AssertConversionFromLocalToGlobalCoordinates(pointInLocalCoords, 4, 0, 6);
        }
        #endregion
        
        protected void CreateAndStore2DSpringBetween(double startX, double startZ, double endX, double endZ)
        {
            this.nodeFactory = new NodeFactory(ModelType.Truss2D);
            this.start = nodeFactory.CreateForTruss(startX, startZ);
            this.end = nodeFactory.CreateForTruss(endX, endZ);
            
            this.elementFactory = new ElementFactory();
            this.SUT = elementFactory.CreateLinearConstantSpring(this.start, this.end, 1);
        }
        
        protected void AssertConversionFromGlobalToLocalCoordinates(Point pointInGlobalCoords, double expectedX, double expectedY, double expectedZ)
        {
            Point pointInLocalCoords = SUT.ConvertGlobalCoordinatesToLocalCoordinates(pointInGlobalCoords);
            
            Assert.AreEqual(expectedX, pointInLocalCoords.X, 0.000001);
            Assert.AreEqual(expectedY, pointInLocalCoords.Y, 0.000001);
            Assert.AreEqual(expectedZ, pointInLocalCoords.Z, 0.000001);
        }
        
        protected void AssertConversionFromLocalToGlobalCoordinates(Point pointInLocalCoords, double expectedX, double expectedY, double expectedZ)
        {
            Point pointInGlobalCoords = SUT.ConvertLocalCoordinatesToGlobalCoordinates(pointInLocalCoords);
            
            Assert.AreEqual(expectedX, pointInGlobalCoords.X, 0.000001);
            Assert.AreEqual(expectedY, pointInGlobalCoords.Y, 0.000001);
            Assert.AreEqual(expectedZ, pointInGlobalCoords.Z, 0.000001);
        }
    }
}
