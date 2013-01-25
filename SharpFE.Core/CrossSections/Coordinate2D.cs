//-----------------------------------------------------------------------
// <copyright file="Coordinate2D.cs" company="Iain Sproat">
//     Copyright Iain Sproat, 2013.
// </copyright>
//-----------------------------------------------------------------------

namespace SharpFE
{
    using System;
    
    /// <summary>
    /// Coordinate in two dimensions
    /// </summary>
    public struct Coordinate2D : IEquatable<Coordinate2D>
    {
        /// <summary>
        /// Position of the coordinate along the x-axis
        /// </summary>
        private double coordinateInXAxis;
        
        /// <summary>
        /// Position of the coordinate along the y-axis
        /// </summary>
        private double coordinateInYAxis;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="Coordinate2D">Coordinate2D</see> class.
        /// </summary>
        /// <param name="x">The position of the coordinate along the x axis</param>
        /// <param name="y">The position of the coordinate along the y axis</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "y", Justification = "valid spelling with clear meaning")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "x", Justification = "valid spelling with clear meaning")]
        public Coordinate2D(double x, double y)
        {
            this.coordinateInXAxis = x;
            this.coordinateInYAxis = y;
        }
        
        /// <summary>
        /// Gets the position of this coordinate along the X axis
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "X", Justification = "Valid spelling in common use")]
        public double X
        {
            get
            {
                return this.coordinateInXAxis;
            }
        }
        
        /// <summary>
        /// Gets the position of this coordinate along the Y axis.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Y", Justification = "Valid spelling in common use")]
        public double Y
        {
            get
            {
                return this.coordinateInYAxis;
            }
        }
        
        #region Equals and GetHashCode implementation
        /// <summary>
        /// Equality operator
        /// </summary>
        /// <param name="leftHandSide">Object to the left of the equality operator</param>
        /// <param name="rightHandSide">Object to the right of the equality operator</param>
        /// <returns>True if the coordinates are equal, false otherwise.</returns>
        public static bool operator ==(Coordinate2D leftHandSide, Coordinate2D rightHandSide)
        {
            return leftHandSide.Equals(rightHandSide);
        }
        
        /// <summary>
        /// Inequality operator.
        /// </summary>
        /// <param name="leftHandSide">Coordinate to the left of the inequality operator</param>
        /// <param name="rightHandSide">Object to the right of the inequality operator</param>
        /// <returns>True if the coordinates are not equal, false if they are equal</returns>
        public static bool operator !=(Coordinate2D leftHandSide, Coordinate2D rightHandSide)
        {
            return !(leftHandSide == rightHandSide);
        }
        
        /// <summary>
        /// Determines whether another object is equal to this.
        /// </summary>
        /// <param name="obj">The other object to check for equality with this.</param>
        /// <returns>True if the other object is equal to this object</returns>
        public override bool Equals(object obj)
        {
            return (obj is Coordinate2D) && this.Equals((Coordinate2D)obj);
        }
        
        /// <summary>
        /// Determines whether another Coordinate2D is equal to this.
        /// </summary>
        /// <param name="other">The other Coordinate2D to check for equality with this.</param>
        /// <returns>True if the other coordinate is equal to this coordinate</returns>
        public bool Equals(Coordinate2D other)
        {
            return object.Equals(this.coordinateInXAxis, other.coordinateInXAxis) && object.Equals(this.coordinateInYAxis, other.coordinateInYAxis);
        }
        
        /// <summary>
        /// The HashCode of this coordinate
        /// </summary>
        /// <returns>An unsigned 32-bit integer representing the HashCode of this object</returns>
        public override int GetHashCode()
        {
            int hashCode = 0;
            unchecked
            {
                hashCode += 1000000007 * this.coordinateInXAxis.GetHashCode();
                hashCode += 1000000009 * this.coordinateInYAxis.GetHashCode();
            }
            
            return hashCode;
        }
        #endregion
    }
}
