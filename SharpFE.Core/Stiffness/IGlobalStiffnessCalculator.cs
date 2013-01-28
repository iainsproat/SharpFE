//-----------------------------------------------------------------------
// <copyright file="IGlobalStiffnessCalculator.cs" company="Iain Sproat">
//     Copyright Iain Sproat, 2013.
// </copyright>
//-----------------------------------------------------------------------

namespace SharpFE.Stiffness
{
    using System;
    
    /// <summary>
    /// 
    /// </summary>
    public interface IElementStiffnessCalculator
    {
        /// <summary>
        /// Element stiffness matrix as rotated to global coordinates
        /// </summary>
        StiffnessMatrix StiffnessMatrixInGlobalCoordinates { get; } ////FIXME only here for testing, to be removed
 
        /// <summary>
        /// Retrieves particular values of the element stiffness matrix as rotated to global coordinates.
        /// </summary>
        /// <param name="rowNode"></param>
        /// <param name="rowDegreeOfFreedom"></param>
        /// <param name="columnNode"></param>
        /// <param name="columnDegreeOfFreedom"></param>
        /// <returns></returns>
        double GetStiffnessInGlobalCoordinatesAt(IFiniteElementNode rowNode, DegreeOfFreedom rowDegreeOfFreedom, IFiniteElementNode columnNode, DegreeOfFreedom columnDegreeOfFreedom);
    }
}
