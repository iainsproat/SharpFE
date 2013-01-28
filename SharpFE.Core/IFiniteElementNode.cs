//-----------------------------------------------------------------------
// <copyright file="IFiniteElementNode.cs" company="Iain Sproat">
//     Copyright Iain Sproat, 2013.
// </copyright>
//-----------------------------------------------------------------------


namespace SharpFE
{
    using System;

    public interface IFiniteElementNode : IEquatable<FiniteElementNode>
    {
        double OriginalX { get; }
        double OriginalY { get; }
        double OriginalZ { get; }
    }
}
