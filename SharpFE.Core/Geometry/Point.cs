//-----------------------------------------------------------------------
// <copyright file="Point.cs" company="Iain Sproat">
//     Copyright Iain Sproat, 2013.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;

namespace SharpFE.Geometry
{
    /// <summary>
    /// A point in 3D space.
    /// A special kind of geometric vector
    /// </summary>
    public class Point : GeometricVector, IEquatable<Point>
    {
        public Point(double x, double y, double z)
            :base(x, y, z)
        {
            // empty
        }
        
        public Point(KeyedVector<DegreeOfFreedom> coords)
            : base(coords)
        {
            // empty
        }
        
        /// <summary>
        /// IEquatable implementation
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(Point other)
        {
            return base.Equals(other);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        /// <remarks>Inverse of point.Subtract(this)</remarks>
        public GeometricVector VectorTo(Point point)
        {
            double deltaX = point.X - this.X;
            double deltaY = point.Y - this.Y;
            double deltaZ = point.Z - this.Z;
            return new GeometricVector(deltaX, deltaY, deltaZ);
        }
        
        public GeometricVector Subtract(Point point)
        {
            double deltaX = this.X - point.X;
            double deltaY = this.Y - point.Y;
            double deltaZ = this.Z - point.Z;
            return new GeometricVector(deltaX, deltaY, deltaZ);
        }
        
        /// <summary>
        /// Adds a vector to this point to calculate the location of a new point
        /// </summary>
        /// <param name="vectorToNewPoint"></param>
        /// <returns></returns>
        public Point Add(GeometricVector vectorToNewPoint)
        {
            return new Point(base.Add(vectorToNewPoint));
        }
    }
}
