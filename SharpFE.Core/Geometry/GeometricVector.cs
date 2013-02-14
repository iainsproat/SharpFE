//-----------------------------------------------------------------------
// <copyright file="?.cs" company="Iain Sproat">
//     Copyright Iain Sproat, 2013.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;

namespace SharpFE.Geometry
{
    /// <summary>
    /// Vector in 3D space
    /// </summary>
    public class GeometricVector : KeyedVector<DegreeOfFreedom>, XYZ, IEquatable<GeometricVector>
    {
        #region Constructor
        private static IList<DegreeOfFreedom> geometricKeys = new List<DegreeOfFreedom>(3){ DegreeOfFreedom.X, DegreeOfFreedom.Y, DegreeOfFreedom.Z };
        public GeometricVector()
            : base(geometricKeys)
        {
            // empty
        }
        
        public GeometricVector(double xCoord, double yCoord, double zCoord)
            : base(geometricKeys, xCoord, yCoord, zCoord)
        {
            // empty
        }
        
        public GeometricVector(KeyedVector<DegreeOfFreedom> vectorToClone)
            : base(vectorToClone)
        {
            Guard.AgainstBadArgument(() => {
                                         return !vectorToClone.Keys.Contains(DegreeOfFreedom.X);
                                     }, "DegreeOfFreedom.X was expected as a key", "vectorToClone");
            Guard.AgainstBadArgument(() => {
                                         return !vectorToClone.Keys.Contains(DegreeOfFreedom.Y);
                                     }, "DegreeOfFreedom.Y was expected as a key", "vectorToClone");
            Guard.AgainstBadArgument(() => {
                                         return !vectorToClone.Keys.Contains(DegreeOfFreedom.Z);
                                     }, "DegreeOfFreedom.Z was expected as a key", "vectorToClone");
        }
        #endregion
        
        #region Properties
        public virtual double X
        {
            get
            {
                return this[DegreeOfFreedom.X];
            }
            protected set
            {
                this[DegreeOfFreedom.X] = value;
            }
        }
        
        public virtual double Y
        {
            get
            {
                return this[DegreeOfFreedom.Y];
            }
            protected set
            {
                this[DegreeOfFreedom.Y] = value;
            }
        }
        
        public virtual double Z
        {
            get
            {
                return this[DegreeOfFreedom.Z];
            }
            protected set
            {
                this[DegreeOfFreedom.Z] = value;
            }
        }
        #endregion
        
        #region Overridden KeyedVector Methods
        public GeometricVector CrossProduct(GeometricVector other)
        {
            return new GeometricVector(base.CrossProduct(other));
        }
        
        public new GeometricVector Multiply(double scalar)
        {
            return new GeometricVector(base.Multiply(scalar));
        }
        
        public new GeometricVector Negate()
        {
            return new GeometricVector(base.Negate());
        }
        
        public new GeometricVector Normalize(double p)
        {
            return new GeometricVector(base.Normalize(p));
        }
        #endregion
        
        /// <summary>
        /// Calculates the perpendicular line from this line to the given point
        /// </summary>
        /// <param name="pointNotOnLine"></param>
        /// <returns></returns>
        public GeometricVector PerpendicularLineTo(CartesianPoint pointOnLine, CartesianPoint pointToCalculatePerpendicularVectorTo)
        {
            GeometricVector betweenPoints = pointToCalculatePerpendicularVectorTo.Subtract(pointOnLine);
            GeometricVector normalizedLineVector = this.Normalize(2);
            double projectionDistanceOfEndPointAlongLine = betweenPoints.DotProduct(normalizedLineVector);
            
            GeometricVector scaledVectorAlongLine = normalizedLineVector.Multiply(projectionDistanceOfEndPointAlongLine);
            CartesianPoint startPointOfPerpendicularLine = pointOnLine.Add(scaledVectorAlongLine);
            
            GeometricVector result = pointToCalculatePerpendicularVectorTo.Subtract(startPointOfPerpendicularLine);
            return result;
        }
        
        /// <summary>
        /// IEquatable implementation
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(GeometricVector other)
        {
            return base.Equals(other);
        }
    }
}
