//-----------------------------------------------------------------------
// <copyright file="DegreeOfFreedom.cs" company="SharpFE">
//     Copyright Iain Sproat, 2012.
// </copyright>
//-----------------------------------------------------------------------

namespace SharpFE
{
    using System;
    
    /// <summary>
    /// Possible degrees of freedom
    /// </summary>
    public enum DegreeOfFreedom
    {
        /// <summary>
        /// Linear translation along the x axis
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "X", Justification = "X is common usage for X axis")]
        X = 0,
        
        /// <summary>
        /// Linear translation along the y axis
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Y", Justification = "Y is common usage for Y axis")]
        Y = 1,
        
        /// <summary>
        /// Linear translation along the z axis
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Z", Justification = "Z is common usage for Z axis")]
        Z = 2,
        
        /// <summary>
        /// Rotation around the x axis
        /// </summary>
        XX = 3,
        
        /// <summary>
        /// Rotation around the y axis
        /// </summary>
        YY = 4,
        
        /// <summary>
        /// Rotation around the z axis
        /// </summary>
        ZZ = 5
    }
}
