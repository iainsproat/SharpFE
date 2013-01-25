//-----------------------------------------------------------------------
// <copyright file="ICrossSection.cs" company="Iain Sproat">
//     Copyright Iain Sproat, 2013.
// </copyright>
//-----------------------------------------------------------------------

namespace SharpFE
{
    using System;
    
    /// <summary>
    /// ICrossSection defines properties of a cross section which are
    /// dependent on geometry only, and independent of material
    /// </summary>
    public interface ICrossSection
    {
        /// <summary>
        /// The maximum depth.
        /// </summary>
        /// <remarks>
        /// This defines the height of the bounding box.  The bounding box centroid is located at the midpoint of the height.
        /// </remarks>
        double MaximumDepth { get; }
        
        /// <summary>
        /// The maximum width.
        /// </summary>
        /// <remarks>This defines the width of the bounding box.  The bounding box centroid is located at the midpoint of the width.</remarks>
        double MaximumWidth { get; }
        
        /// <summary>
        /// The total cross sectional area, accounting for all internal holes etc.
        /// </summary>
        double Area { get; }
        
        /// <summary>
        /// The second moment of area around the YY axis.
        /// The location of the YY axis is defined by the geometric centroid.
        /// </summary>
        /// <remarks>Major axis moment in most vertically dominant beams</remarks>
        double SecondMomentOfAreaAroundYY { get; }
        
        /// <summary>
        /// The second moment of area around the ZZ axis.
        /// The location of the ZZ axis is defined by the geometric centroid.
        /// </summary>
        /// <remarks>Minor axis moment in most vertically dominant beams</remarks>
        double SecondMomentOfAreaAroundZZ { get; }
        
        /// <summary>
        /// The length of the external perimeter of the cross section.
        /// </summary>
        double ExternalPerimeterLength { get; }
        
        /// <summary>
        /// The value of the moment of inertia in torsion
        /// </summary>
        double MomentOfInertiaInTorsion { get; }
        
        /// <summary>
        /// As measured from centre of bounding box
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Centroid", Justification = "valid spelling")]
        Coordinate2D GeometricCentroid { get; }
                
        ////TODO storing of coordinate data for profile (may need to work with b-splines etc...)
    }
}
