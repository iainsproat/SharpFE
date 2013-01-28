//-----------------------------------------------------------------------
// <copyright file="IFiniteElement.cs" company="Iain Sproat">
//     Copyright Iain Sproat, 2013.
// </copyright>
//-----------------------------------------------------------------------

namespace SharpFE
{
    using System;
    using System.Collections.Generic;
    
    public interface IFiniteElement
    {
        IList<IFiniteElementNode> Nodes { get; }
        KeyedVector<DegreeOfFreedom> LocalXAxis { get; }
        KeyedVector<DegreeOfFreedom> LocalYAxis { get; }
        KeyedVector<DegreeOfFreedom> LocalZAxis { get; }
        bool IsASupportedBoundaryConditionDegreeOfFreedom(DegreeOfFreedom degreeOfFreedom);
        bool IsDirty(int previousHash);
    }
}
