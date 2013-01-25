//-----------------------------------------------------------------------
// <copyright file="IHasMaterial.cs" company="Iain Sproat">
//     Copyright Iain Sproat, 2013.
// </copyright>
//-----------------------------------------------------------------------

namespace SharpFE
{
    using System;
    
    /// <summary>
    /// Indicates that this finite element is defined by material properties
    /// </summary>
    public interface IHasMaterial
    {
        /// <summary>
        /// 
        /// </summary>
        IMaterial Material { get; }
    }
}
