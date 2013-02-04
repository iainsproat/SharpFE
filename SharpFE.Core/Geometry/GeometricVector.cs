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
            : base(new double[3]{ xCoord, yCoord, zCoord}, geometricKeys)
        {
            // empty
        }
        
        public GeometricVector(KeyedVector<DegreeOfFreedom> coords)
            : this((MathNet.Numerics.LinearAlgebra.Generic.Vector<double>)coords)
        {
            // empty
        }
        
        public GeometricVector(MathNet.Numerics.LinearAlgebra.Generic.Vector<double> coords)
            : base(coords, geometricKeys)
        {
            // empty
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
