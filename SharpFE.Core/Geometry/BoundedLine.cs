//-----------------------------------------------------------------------
// <copyright file="BoundedLine.cs" company="Iain Sproat">
//     Copyright Iain Sproat, 2013.
// </copyright>
//-----------------------------------------------------------------------
using System;

namespace SharpFE.Geometry
{
    /// <summary>
    /// A line which is bounded by a start and end point
    /// </summary>
    public class BoundedLine : UnboundedLine
    {
        public BoundedLine(CartesianPoint startOfLine, GeometricVector vectorOfLineFromStartToEnd)
            : base(vectorOfLineFromStartToEnd, startOfLine)
        {
            // empty
        }
        
        public BoundedLine(CartesianPoint startOfLine, CartesianPoint endOfLine)
            : base(endOfLine.Subtract(startOfLine), startOfLine)
        {
            // empty
        }
        
        public CartesianPoint Start
        {
            get
            {
                return this.PointOnLine;
            }
//            protected set
//            {
//                this.PointOnLine = value;
//            }
        }
        
        public CartesianPoint End
        {
            get
            {
                return this.Start.Add(this);
            }
//            protected set
//            {
//                this.Vector = value.Subtract(this.Start);
//            }
        }
        
        public double Length
        {
            get
            {
                return this.Vector.Norm(2);
            }
        }
        
        public override bool IsOnLine(CartesianPoint pointToCheck)
        {
            
            GeometricVector normalizedVector = this.Vector.Normalize(2);
            
            GeometricVector vectorToPointToCheck = this.Start.VectorTo(pointToCheck);
            if (vectorToPointToCheck.X == 0 && vectorToPointToCheck.Y == 0 && vectorToPointToCheck.Z == 0)
            {
                // the pointToCheck is at the start of the bounded line
                return true;
            }
            
            GeometricVector normalizedVectorToPointToCheck = vectorToPointToCheck.Normalize(2);
            if (!normalizedVector.Equals(normalizedVectorToPointToCheck))
            {
                return false;
            }
            
            double norm = this.Vector.Norm(2);
            double normToPointToCheck = vectorToPointToCheck.Norm(2);
            return normToPointToCheck <= norm;
        }
    }
}
