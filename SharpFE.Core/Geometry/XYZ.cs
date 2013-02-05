//-----------------------------------------------------------------------
// <copyright file="XYZ.cs" company="Iain Sproat">
//     Copyright Iain Sproat, 2013.
// </copyright>
//-----------------------------------------------------------------------
using System;

namespace SharpFE
{
    /// <summary>
    /// A point, vector or coordinate in 3D space.
    /// </summary>
    public interface XYZ
    {
        /// <summary>
        /// Distance along the X-axis
        /// </summary>
        double X { get; }
        
        /// <summary>
        /// Distance along the Y-axis
        /// </summary>
        double Y { get; }
        
        /// <summary>
        /// Distance along the Z-axis
        /// </summary>
        double Z { get; }
    }
}
