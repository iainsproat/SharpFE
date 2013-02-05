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
        GeometricVector LocalXAxis { get; }
        GeometricVector LocalYAxis { get; }
        GeometricVector LocalZAxis { get; }
        bool IsASupportedBoundaryConditionDegreeOfFreedom(DegreeOfFreedom degreeOfFreedom);
        bool IsDirty(int previousHash);
    }
}
