//-----------------------------------------------------------------------
// <copyright file="IElementStiffnessMatrixBuilderFactory.cs" company="Iain Sproat">
//     Copyright Iain Sproat, 2013.
// </copyright>
//-----------------------------------------------------------------------

namespace SharpFE.Stiffness
{
    using System;
    
    public interface IElementStiffnessMatrixBuilderFactory
    {
        IElementStiffnessCalculator Create<T>(T element) where T : IFiniteElement;
    }
}
