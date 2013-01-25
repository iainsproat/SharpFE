//-----------------------------------------------------------------------
// <copyright file="IMaterial.cs" company="Iain Sproat">
//     Copyright Iain Sproat, 2013.
// </copyright>
//-----------------------------------------------------------------------

namespace SharpFE
{
    using System;
    
    /// <summary>
    /// A material
    /// </summary>
    public interface IMaterial
    {
        /// <summary>
        /// 
        /// </summary>
        double Density { get; }
        
        /// <summary>
        /// 
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Youngs", Justification = "spelling is correct")]
        double YoungsModulus { get; }
        
        /// <summary>
        /// 
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Poissons", Justification = "spelling is correct")]
        double PoissonsRatio { get; }
        
        /// <summary>
        /// 
        /// </summary>
        double ShearModulusElasticity { get; }
    }
}
