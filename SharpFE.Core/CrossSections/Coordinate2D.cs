namespace SharpFE
{
    using System;
    
    /// <summary>
    /// Coordinate in two dimensions
    /// </summary>
    public struct Coordinate2D : IEquatable<Coordinate2D>
    {
        private double _x;
        private double _y;
        
        public Coordinate2D(double x, double y)
        {
            this._x = x;
            this._y = y;
        }
        
        public double X
        {
            get
            {
                return this._x;
            }
        }
        
        public double Y
        {
            get
            {
                return this._y;
            }
        }
        
        #region Equals and GetHashCode implementation
        public override bool Equals(object obj)
        {
            return (obj is Coordinate2D) && this.Equals((Coordinate2D)obj);
        }
        
        public bool Equals(Coordinate2D other)
        {
            return object.Equals(this._x, other._x) && object.Equals(this._y, other._y);
        }
        
        public override int GetHashCode()
        {
            int hashCode = 0;
            unchecked
            {
                hashCode += 1000000007 * this._x.GetHashCode();
                hashCode += 1000000009 * this._y.GetHashCode();
            }
            
            return hashCode;
        }
        
        public static bool operator ==(Coordinate2D lhs, Coordinate2D rhs)
        {
            return lhs.Equals(rhs);
        }
        
        public static bool operator !=(Coordinate2D lhs, Coordinate2D rhs)
        {
            return !(lhs == rhs);
        }
        #endregion

    }
}
