//-----------------------------------------------------------------------
// <copyright file="SolidRectangle.cs" company="SharpFE">
//     Copyright Iain Sproat, 2012 - 2013.
// </copyright>
//-----------------------------------------------------------------------

namespace SharpFE
{
    using System;

    /// <summary>
    /// A cross section formed of a solid (that is, it has no internal voids) rectangle, or square.
    /// </summary>
    public class SolidRectangle : ICrossSection
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="height"></param>
        /// <param name="width"></param>
        public SolidRectangle(double height, double width)
        {
            if (height <= 0)
            {
                throw new ArgumentException("height has to be a positive, non-zero number");
            }
            
            if (width <= 0)
            {
                throw new ArgumentException("width has to be a positive, non-zero number");
            }
            
            this.MaximumDepth = height;
            this.MaximumWidth = width;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public double MaximumDepth
        {
            get;
            private set;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public double MaximumWidth
        {
            get;
            private set;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public double Area
        {
            get
            {
                return this.MaximumDepth * this.MaximumWidth;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public double SecondMomentOfAreaAroundYY
        {
            get
            {
                return this.MaximumWidth * this.MaximumDepth * this.MaximumDepth * this.MaximumDepth / 12.0;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public double SecondMomentOfAreaAroundZZ
        {
            get
            {
                return this.MaximumDepth * this.MaximumWidth * this.MaximumWidth * this.MaximumWidth / 12.0;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public double ExternalPerimeterLength
        {
            get
            {
                return (this.MaximumDepth * 2) + (this.MaximumWidth * 2);
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public double MomentOfInertiaInTorsion
        {
            get
            {
                return (Math.Pow(this.MaximumDepth, 3) / 3.0) * (this.MaximumWidth - (0.63 * this.MaximumDepth * (1 - (Math.Pow(this.MaximumDepth, 4) / (12.0 * Math.Pow(this.MaximumWidth, 4))))));
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public Coordinate2D GeometricCentroid
        {
            get
            {
                return new Coordinate2D(0, 0);
            }
        }
    }
}
