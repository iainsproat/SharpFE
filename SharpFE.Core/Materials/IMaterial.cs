

namespace SharpFE
{
    using System;
    
    /// <summary>
    /// A material
    /// </summary>
    public interface IMaterial
    {
        double Density { get; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Youngs")]
        double YoungsModulus { get; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Poissons")]
        double PoissonsRatio { get; }
        double ShearModulusElasticity { get; }
    }
}
