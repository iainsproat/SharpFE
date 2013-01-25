//-----------------------------------------------------------------------
// <copyright file="ModelType.cs" company="SharpFE">
//     Copyright Iain Sproat, 2012.
// </copyright>
//-----------------------------------------------------------------------

namespace SharpFE
{
    using System;
    
    /// <summary>
    /// </summary>
    public enum ModelType
    {
        /// <summary>
        /// A one dimensional truss.
        /// : Positioning of nodes and elements:
        ///     - 1D along the x-axis only.
        /// : Forces can be applied:
        ///     - translational along the x-axis only.
        /// : Results will occur:
        ///     - translation along the x-axis only.
        /// </summary>
        Truss1D,
        
        /// <summary>
        /// A one dimensional beam.
        /// : Positioning of nodes and elements:
        ///     - 1D along the x-axis only.
        /// : Forces can be applied:
        ///     - translational in the y-axis.
        ///     - rotational around the z-axis.
        /// : Results will occur:
        ///     - translation along the y-axis only
        ///     - rotational reactions around the z-axis only.
        /// </summary>
        Beam1D,
        
        /// <summary>
        /// A two dimensional truss.
        /// : Positioning of nodes and elements:
        ///     - 2D along the x-axis and z-axis.
        /// : Forces can be applied:
        ///     - translational in the x-axis and z-axis.
        /// : Results will occur:
        ///     - translation along the x-axis and z-axis.
        ///     - rotations within members are not allowed.
        /// </summary>
        Truss2D,
        
        /// <summary>
        /// A two dimensional frame.
        /// : Positioning of nodes and elements:
        ///     - 2D along the x-axis and z-axis.
        /// : Forces can be applied:
        ///     - translational in the x-axis and z-axis.
        ///     - rotation around the z-axis.
        /// : Results will occur:
        ///     - translation along the x-axis and z-axis.
        ///     - rotational reactions around the y-y-axis.
        /// </summary>
        Frame2D,
        
        /// <summary>
        /// A two dimensional slab.
        /// : Positioning of nodes and elements:
        ///     - 2D along the x-axis and y-axis.
        /// : Forces can be applied:
        ///     - translational in the z-axis.
        ///     - rotation around the x-axis and the y-axis.
        /// : Results will occur:
        ///     - translation along the z-axis.
        ///     - rotational reactions around the x-x axis and y-y axis.
        /// </summary>
        Slab2D,
        
        /// <summary>
        /// A three dimensional truss.
        /// : Positioning of nodes and elements:
        ///     - 3D along the x-axis, y-axis and z-axis.
        /// : Forces can be applied:
        ///     - translational in the x-axis, y-axis and z-axis.
        /// : Results will occur:
        ///     - translation along the x-axis, y-axis and z-axis.
        ///     - rotations within members are not allowed.
        /// </summary>
        Truss3D,
        
        /// <summary>
        /// Multiple 2D slabs linked by 1D spring elements in the z-axis.
        /// : Positioning of nodes and elements:
        ///     - 3D along the x-axis, y-axis and z-axis.  Plate elements are restricted to being in a plane with normal parallel to the z-axis.
        /// : Forces can be applied:
        ///     - translational in the z-axis.
        ///     - rotational in the x-x axis and y-y axis.
        /// : Results will occur:
        ///     - translation along the z-axis.
        ///     - rotation in plate elements in the x-x axis and y-y axis.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Multi", Justification = "The use of the word 'Multi' is relevant in this context")]
        MultiStorey2DSlab,
        
        /// <summary>
        /// A three dimensional model.
        /// : Positioning of nodes and elements:
        ///     - 3D along the x-axis, y-axis and z-axis.
        /// : Forces can be applied:
        ///     - translational in the x-axis, y-axis and z-axis.
        ///     - rotational around the x-axis, y-axis and z-axis.
        /// : Results will occur:
        ///     - translation along the x-axis, y-axis and z-axis.
        ///     - rotational around the x-axis, y-axis and z-axis.
        /// </summary>
        Full3D
    }
}
