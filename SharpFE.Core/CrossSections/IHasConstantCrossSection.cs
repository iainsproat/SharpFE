//-----------------------------------------------------------------------
// <copyright file="IHasConstantCrossSection.cs" company="Iain Sproat">
//     Copyright Iain Sproat, 2013.
// </copyright>
//-----------------------------------------------------------------------

namespace SharpFE
{
    using System;
    
    /// <summary>
    /// 
    /// </summary>
    public interface IHasConstantCrossSection : IHasCrossSection
    {
        /// <summary>
        /// 
        /// </summary>
        ICrossSection CrossSection { get; }
    }
}
