//-----------------------------------------------------------------------
// <copyright file="Plane.cs" company="Iain Sproat">
//     Copyright Iain Sproat, 2013.
// </copyright>
//-----------------------------------------------------------------------
using System;

namespace SharpFE.Geometry
{
    /// <summary>
    /// A plane in 3D space
    /// </summary>
    public class Plane
    {
        public Plane(GeometricVector planeNormal, CartesianPoint pointOnPlane)
        {
            this.Normal = planeNormal;
            this.Point = pointOnPlane;
        }
        
        public GeometricVector Normal
        {
            get;
            protected set;
        }
        
        /// <summary>
        /// Point on the plane
        /// </summary>
        public CartesianPoint Point
        {
            get;
            protected set;
        }
        
        /// <summary>
        /// Determines whether a point lies on the plane
        /// </summary>
        /// <param name="pointToCheck"></param>
        /// <returns></returns>
        public bool IsInPlane(CartesianPoint pointToCheck)
        {
            double dotProductOfPlane = this.Point.DotProduct(Normal);
            double dotProductOfPointToCheck = pointToCheck.DotProduct(Normal);
            double delta = dotProductOfPlane - dotProductOfPointToCheck;
            double tolerance = 0.001;
            return (delta * delta) < tolerance;
        }
    }
}
