//-----------------------------------------------------------------------
// <copyright file="ModelType.cs" company="SharpFE">
//     Copyright Iain Sproat, 2012.
// </copyright>
//-----------------------------------------------------------------------

namespace SharpFE
{
    using System;
    
    /// <summary>
    /// Description of ModelType.
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
        ///     - 2D along the x-axis and y-axis.
        /// : Forces can be applied:
        ///     - translational in the x-axis and y-axis.
        /// : Results will occur:
        ///     - translation along the x-axis and y-axis.
        ///     - rotations within members are not allowed.
        /// </summary>
        Truss2D,
        
        /// <summary>
        /// A two dimensional frame.
        /// : Positioning of nodes and elements:
        ///     - 2D along the x-axis and y-axis.
        /// : Forces can be applied:
        ///     - translational in the x-axis and y-axis.
        ///     - rotation around the z-axis.
        /// : Results will occur:
        ///     - translation along the x-axis and y-axis.
        ///     - rotational reactions around the z-axis.
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
        ///     - rotational reactions around the x-axis and y-axis.
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
        ///     - 3D along the x-axis, y-axis and z-axis.  Plate elements are restricted to being in a plane with normals parallel to the z-axis.
        /// : Forces can be applied:
        ///     - translational in the z-axis.
        ///     - rotational in the xx-axis and yy-axis.
        /// : Results will occur:
        ///     - translation along the z-axis.
        ///     - rotation in plate elements in the xx-axis and yy-axis.
        /// </summary>
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
