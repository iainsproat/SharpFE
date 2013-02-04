//-----------------------------------------------------------------------
// <copyright file="UnboundedLine.cs" company="Iain Sproat">
//     Copyright Iain Sproat, 2013.
// </copyright>
//-----------------------------------------------------------------------
using System;

namespace SharpFE.Geometry
{
    public class UnboundedLine : GeometricVector
    {
        public UnboundedLine(GeometricVector vectorOfLine, Point pointOnLine)
        {
            Guard.AgainstNullArgument(vectorOfLine, "vectorOfLine");
            Guard.AgainstNullArgument(pointOnLine, "pointOnLine");
            
            this.Vector = vectorOfLine;
            this.PointOnLine = pointOnLine;
        }
        
        /// <summary>
        /// Vector of line
        /// </summary>
        public virtual GeometricVector Vector
        {
            get
            {
                return this;
            }
            protected set
            {
                this.X = value.X;
                this.Y = value.Y;
                this.Z = value.Z;
            }
        }
        
        /// <summary>
        /// Point on the line.
        /// A point through which the vector passes.  It can be any point, and does not indicate a boundary.
        /// </summary>
        public virtual Point PointOnLine
        {
            get;
            protected set;
        }
        
        public virtual bool IsOnLine(Point pointToCheck)
        {
            GeometricVector normalizedVector = Vector.Normalize(2);
            GeometricVector vectorToPointToCheck = this.PointOnLine.VectorTo(pointToCheck);
            vectorToPointToCheck = vectorToPointToCheck.Normalize(2);
            if (normalizedVector.Equals(vectorToPointToCheck))
            {
                return true;
            }
            
            GeometricVector inversedNormalizedVector = normalizedVector.Negate();
            
            return inversedNormalizedVector.Equals(vectorToPointToCheck);
        }
        
        /// <summary>
        /// Calculates the perpendicular line from this line to the given point
        /// </summary>
        /// <param name="pointNotOnLine"></param>
        /// <returns></returns>
        public BoundedLine PerpendicularLineTo(Point pointNotOnLine)
        {
            GeometricVector betweenPoints = pointNotOnLine.Subtract(this.PointOnLine);
            GeometricVector normalizedLineVector = this.Vector.Normalize(2);
            double projectionDistanceOfEndPointAlongLine = betweenPoints.DotProduct(normalizedLineVector);
            
            GeometricVector vectorAlongLine = normalizedLineVector.Multiply(projectionDistanceOfEndPointAlongLine);
            Point endPointOfPerpendicularLine = this.PointOnLine.Add(vectorAlongLine);
            
            GeometricVector result = pointNotOnLine.Subtract(endPointOfPerpendicularLine);
            return new BoundedLine(endPointOfPerpendicularLine, result);
        }
    }
}
