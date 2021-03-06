﻿//-----------------------------------------------------------------------
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
    public class CartesianPoint : GeometricVector, IEquatable<CartesianPoint>
    {
        public CartesianPoint(double x, double y, double z)
            :base(x, y, z)
        {
            // empty
        }
        
        public CartesianPoint(KeyedVector<DegreeOfFreedom> coords)
            : base(coords)
        {
            // empty
        }
        
        
        
        #region Equals and GetHashCode implementation
        public override int GetHashCode()
        {
            int hashCode = 0;
            
            unchecked
            {
                hashCode += 1000000987 * base.GetHashCode();
            }
            
            return hashCode;
        }
        
        public override bool Equals(object obj)
        {
            CartesianPoint other = obj as CartesianPoint;
            if (other == null)
                return false;
            return this.Equals(other);
        }
        
        /// <summary>
        /// IEquatable implementation
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(CartesianPoint other)
        {
            return base.Equals(other);
        }
        
        public static bool operator ==(CartesianPoint lhs, CartesianPoint rhs)
        {
            if (ReferenceEquals(lhs, rhs))
                return true;
            if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null))
                return false;
            return lhs.Equals(rhs);
        }
        
        public static bool operator !=(CartesianPoint lhs, CartesianPoint rhs)
        {
            return !(lhs == rhs);
        }
        #endregion

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        /// <remarks>Inverse of point.Subtract(this)</remarks>
        public GeometricVector VectorTo(XYZ point)
        {
            double deltaX = point.X - this.X;
            double deltaY = point.Y - this.Y;
            double deltaZ = point.Z - this.Z;
            return new GeometricVector(deltaX, deltaY, deltaZ);
        }
        
        public GeometricVector Subtract(XYZ point)
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
        public CartesianPoint Add(GeometricVector vectorToNewPoint)
        {
            return new CartesianPoint(base.Add(vectorToNewPoint));
        }
    }
}
