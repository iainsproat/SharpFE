//-----------------------------------------------------------------------
// <copyright file="IFiniteElement.cs" company="Iain Sproat">
//     Copyright Iain Sproat, 2013.
// </copyright>
//-----------------------------------------------------------------------

namespace SharpFE
{
    using System;
    using System.Collections.Generic;
    
    using SharpFE.Geometry;
    
    public interface IFiniteElement
    {
        IList<IFiniteElementNode> Nodes { get; }
        
        #region Local coordinates
        CartesianPoint LocalOrigin { get; }
        /// <summary>
        /// Vector defining the direction of the local X axis, given in global coordinates
        /// </summary>
        GeometricVector LocalXAxis { get; }
        
        /// <summary>
        /// Vector defining the direction of the local Y axis, given in global coordinates
        /// </summary>
        GeometricVector LocalYAxis { get; }
        
        /// <summary>
        /// Vector defining the direction of the local Z axis, given in global coordinates
        /// </summary>
        GeometricVector LocalZAxis { get; }
        CartesianPoint ConvertGlobalCoordinatesToLocalCoordinates(CartesianPoint globalPoint);
        CartesianPoint ConvertLocalCoordinatesToGlobalCoordinates(CartesianPoint localPoint);
        KeyedSquareMatrix<DegreeOfFreedom> CalculateElementRotationMatrix();
        #endregion
       
        bool IsASupportedBoundaryConditionDegreeOfFreedom(DegreeOfFreedom degreeOfFreedom);
        
        bool IsDirty(int previousHash);
    }
}
