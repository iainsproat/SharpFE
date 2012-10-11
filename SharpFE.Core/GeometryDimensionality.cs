//-----------------------------------------------------------------------
// <copyright file="GeometryDimensionality.cs" company="SharpFE">
//     Copyright Iain Sproat, 2012.
// </copyright>
//-----------------------------------------------------------------------

namespace SharpFE
{
    using System;
    
    /// <summary>
    /// Indicates the number of spatial dimensions
    /// </summary>
    public enum GeometryDimensionality
    {
        /// <summary>
        /// One dimension. i.e. constrained to a line
        /// </summary>
        OneDimension,
        
        /// <summary>
        /// Two dimensions. i.e. constrained to a plane
        /// </summary>
        TwoDimension,
        
        /// <summary>
        /// Three dimensions
        /// </summary>
        ThreeDimension
    }
}
