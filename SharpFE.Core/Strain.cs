//-----------------------------------------------------------------------
// <copyright file="Strain.cs" company="Iain Sproat">
//     Copyright Iain Sproat, 2012.
// </copyright>
//-----------------------------------------------------------------------

namespace SharpFE
{
    using System;
    
    /// <summary>
    /// </summary>
    public enum Strain
    {
        /// <summary>
        /// Strain along the x axis
        /// </summary>
        LinearStrainX,
        
        /// <summary>
        /// Strain along the y axis
        /// </summary>
        LinearStrainY,
        
        /// <summary>
        /// Shear strain through the x-y plane
        /// </summary>
        ShearStrainXY,
        
        /// <summary>
        /// Shear strain through the x-z plane
        /// </summary>
        ShearStrainXZ,
        
        /// <summary>
        /// Shear strain through the y-z plane
        /// </summary>
        ShearStrainYZ
    }
}
