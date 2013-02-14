//-----------------------------------------------------------------------
// <copyright file="IFiniteElementNode.cs" company="Iain Sproat">
//     Copyright Iain Sproat, 2013.
// </copyright>
//-----------------------------------------------------------------------


namespace SharpFE
{
    using System;

    public interface IFiniteElementNode : XYZ, IEquatable<FiniteElementNode>
    {
        Geometry.CartesianPoint Location { get; }
    }
}
